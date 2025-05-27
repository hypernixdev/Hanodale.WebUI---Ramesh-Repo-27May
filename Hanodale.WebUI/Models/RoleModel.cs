using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class RoleModel
    {
        public string id { get; set; }

        public bool readOnly { get; set; }

        //[HiddenInput(DisplayValue = false)]
        [Display(Name = "USER_ROLE_NAME", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string roleName { get; set; }

        [HiddenInput(DisplayValue = false)]
        [UIHint("TextArea")]
        [Display(Name = "USER_ROLE_DESCRIPTION", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1} characters")]
        public string description { get; set; }

        //if don't want to show the field commant the UIHint & Display (Name="")
        [UIHint("HBool")]
        //[HiddenInput(DisplayValue = false)]
        [Display(Name = "USER_ROLE_ADMIN", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public bool isAdmin { get; set; }

        [UIHint("HBool")]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "USER_ROLE_ACTIVE", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public bool status { get; set; }


        [UIHint("ComboBox")]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "USER_ROLE_LANDINGPAGE", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public string landingPage { get; set; }

        public string createdBy { get; set; } 
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

        public bool isEdit { get; set; }

        ////Testing Dropdown
        //[UIHint("ComboBox")]
        //[Display(Name = "cmbTest")]
        public IEnumerable<SelectListItem> LandingPages { get; set; }
    }
    public partial class UserRoleViewModel
    { }
}