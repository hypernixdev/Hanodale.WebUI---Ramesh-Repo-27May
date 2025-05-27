using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class WorkCategoryModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public bool isSelected { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "WORKCATEGORY_NAME", ResourceType = typeof(Resources))]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string name { get; set; }

        [UIHint("TextArea")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "WORKCATEGORY_DESCRIPTION", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string description { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "WORKCATEGORY_REMARKS", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string remarks { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

        [UIHint("HBool")]
        [Display(Name = "WORKCATEGORY_VISIBILITY", ResourceType = typeof(Resources))]
        public bool isVisible { get; set; }       

    }
    public partial class WorkCategoryViewModel
    {
        public List<WorkCategoryModel> lstWorkCategoryModel { get; set; }
    }
}