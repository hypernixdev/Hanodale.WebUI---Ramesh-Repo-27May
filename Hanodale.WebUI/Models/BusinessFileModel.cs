using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class BusinessFileModel
    {
        public string id { get; set; }

        public string business_Id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESSFILE_FILE_TYPE", ResourceType = typeof(Resources))]
        public int fileType_Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESSFILE_NAME", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string name { get; set; }

        [Display(Name = "BUSINESSFILE_URLPATH", ResourceType = typeof(Resources))]
        public string urlPath { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "BUSINESSFILE_DESCRIPTION", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string description { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public System.DateTime modifiedDate { get; set; }

        [UIHint("FileBrowser")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "STOCKFILE_URLPATH", ResourceType = typeof(Resources))]
        public string file { get; set; }
        
        

        public IEnumerable<SelectListItem> lstbusinessFile { get; set; }
    }
    public partial class BusinessFileViewModel
    {
        public List<BusinessFileModel> lstBusinessFileModel { get; set; }
    }
}