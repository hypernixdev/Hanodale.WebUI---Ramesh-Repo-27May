using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class UserProfileModel
    {
        public int id { get; set; }
        public string userName { get; set; }
        public int userType_Id { get; set; }
        public int person_Id { get; set; }
        public string password { get; set; }
        public string passwordHash { get; set; }
        public Nullable<bool> verified { get; set; }
        public Nullable<int> language { get; set; }
        public bool isActive { get; set; }
        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }
}