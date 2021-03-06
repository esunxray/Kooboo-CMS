﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Collections;

namespace Kooboo.CMS.Account.Models
{
    [DataContract]
    public partial class User : IPersistable, IIdentifiable
    {
        public string UserName { get; set; }

        [DataMember(Order = 1)]
        public string Email { get; set; }

        [DataMember(Order = 3)]
        public string Password { get; set; }

        [DataMember(Order = 5)]
        public bool IsAdministrator { get; set; }

        [DataMember(Order = 6)]
        public string UICulture { get; set; }

        private DynamicDictionary customFields = null;
        [DataMember(Order = 7)]
        public DynamicDictionary CustomFields
        {
            get
            {
                if (customFields == null)
                {
                    customFields = new DynamicDictionary();
                }
                return customFields;
            }
            set
            {
                customFields = value;
            }
        }

        [XmlIgnore]
        public string CustomFieldsXml
        {
            get
            {
                string xml = "";
                if (CustomFields != null)
                {
                    xml = Kooboo.Runtime.Serialization.DataContractSerializationHelper.SerializeAsXml(this.CustomFields);
                }
                return xml;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    CustomFields = Kooboo.Runtime.Serialization.DataContractSerializationHelper.DeserializeFromXml<DynamicDictionary>(value);
                }
            }
        }
        #region IPersistable
        public string UUID
        {
            get
            {
                return this.UserName;
            }
            set
            {
                this.UserName = value;
            }
        }
        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        public void Init(IPersistable source)
        {
            this.UserName = ((User)source).UserName;
            isDummy = false;
        }

        public void OnSaved()
        {
            isDummy = false;
        }

        public void OnSaving()
        {

        }

        #endregion
    }

    public partial class User
    {
        public int FailedPasswordAttemptCount
        {
            get
            {
                if (CustomFields.ContainsKey("FailedPasswordAttemptCount"))
                {
                    return (int)CustomFields["FailedPasswordAttemptCount"];
                }
                return 0;
            }
            set
            {
                CustomFields["FailedPasswordAttemptCount"] = value;
            }
        }

        public bool IsLockedOut
        {
            get
            {
                if (CustomFields.ContainsKey("IsLockedOut"))
                {
                    return (bool)CustomFields["IsLockedOut"];
                }
                return false;
            }
            set
            {
                CustomFields["IsLockedOut"] = value;
            }
        }

        public DateTime LastLockoutDate
        {
            get
            {
                if (CustomFields.ContainsKey("LastLockoutDate"))
                {
                    return (DateTime)CustomFields["LastLockoutDate"];
                }
                return DateTime.MinValue;
            }
            set
            {
                CustomFields["LastLockoutDate"] = value;
            }
        }

        //public DateTime LastLockoutDate
        //{
        //    get
        //    {
        //        if (CustomFields.ContainsKey("LastLockoutDate"))
        //        {
        //            return (DateTime)CustomFields["LastLockoutDate"];
        //        }
        //        return DateTime.MinValue;
        //    }
        //    set
        //    {
        //        CustomFields["LastLockoutDate"] = value;
        //    }
        //}

    }
}
