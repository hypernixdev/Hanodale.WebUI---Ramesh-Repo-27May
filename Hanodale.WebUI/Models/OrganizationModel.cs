using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class OrganizationModel
    {
        public string id { get; set; }

        public string organaizationId { get; set; }
       
        //public string orgCategory_Id { get; set; }

        [UIHint("ComboBox")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATION_CATEGORY_TYPE", ResourceType = typeof(Resources))]
        public int categoryType_Id { get; set; }

        [Display(Name = "ORGANIZATION_CATEGORY_NAME", ResourceType = typeof(Resources))]
        public string orgCategoryName { get; set; }
       
        public string parent_Id { get; set; }

        [Display(Name = "ORGANIZATION_PARENT_NAME", ResourceType = typeof(Resources))]
        public string parentName { get; set; }

        public bool readOnly { get; set; }

        [Display(Name = "ORGANIZATION_NAME", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string name { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "ORGANIZATION_DESCRIPTION", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1} characters")]
        public string description { get; set; }

        [Display(Name = "ORGANIZATION_PREFIX", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string prefix { get; set; }

        [UIHint("HBool")]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "USER_ROLE_ACTIVE", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public bool status { get; set; }

        [Display(Name = "ORGANIZATION_CODE", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1} characters")]
        public string code { get; set; }

        [Display(Name = "ORGANIZATION_SAPCODE", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1} characters")]
        public string sapcode { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

        public bool isEdit { get; set; }

        public bool isMainCostCenter { get; set; }

        public bool isTreeView { get; set; }

        public IEnumerable<SelectListItem> lstCategoryType { get; set; }

    }
   
}