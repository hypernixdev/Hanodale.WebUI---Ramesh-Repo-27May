using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class ModuleItemModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MODULEITEM_MODULETYPE_ID", ResourceType = typeof(Resources))]
        public int modulType_Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MODULEITEM_NAME", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string name { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "MODULEITEM_DESCRIPTION", ResourceType = typeof(Resources))]
        [StringLength(1000, ErrorMessage = "The Maximum length is {1}")]
        public string description { get; set; }

        [UIHint("HBool")]
        [Required]
        [Display(Name = "MODULEITEM_VISIBILITY", ResourceType = typeof(Resources))]
        public bool visibility { get; set; }
        
        [UIHint("Number")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MODULEITEM_SORTORDER", ResourceType = typeof(Resources))]
        [Range(0, 1000)]
        public int sortOrder { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "ADHOCREPORT_REMARKS", ResourceType = typeof(Resources))]
        //[StringLength(200, MinimumLength = 3, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string remarks { get; set; }

        public IEnumerable<SelectListItem> ModuleTypeItems { get; set; }
    }
    public partial class ModuleItemViewModel
    {
        public List<ModuleItemModel> lstModuleItemModel { get; set; }
    }
}