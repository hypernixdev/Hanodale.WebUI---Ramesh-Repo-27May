using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class HelpDeskModel
    {
        public string id { get; set; }

        public int organization_Id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public bool isBack { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "HELPDESK_USERTYPE", ResourceType = typeof(Resources))]
        public Nullable<int> user_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "HELPDESK_WORKFOLLOW_ID", ResourceType = typeof(Resources))]
        public int workFollowStatus_Id { get; set; }

        [Display(Name = "HELPDESK_CODE", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string code { get; set; }


        [UIHint("TextArea")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "HELPDESK_REMARKS", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string remarks { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "HELPDESK_FEEDBACK", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(1000, ErrorMessage = "The Maximum length is {1}")]
        public string feedback { get; set; }

        [Display(Name = "HELPDESK_NAME", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string name { get; set; }

        [Display(Name = "HELPDESK_DEPARTMENT", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string department { get; set; }

        [Display(Name = "HELPDESK_DESIGNATION", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string designation { get; set; }

        [Display(Name = "HELPDESK_OFFICEPHONE", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string officePhone { get; set; }

        [Display(Name = "HELPDESK_CELLPHONE", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string cellPhone { get; set; }

        [Display(Name = "HELPDESK_EMAIL", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Enter a valid email")]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string email { get; set; }


        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<DateTime> modifiedDate { get; set; }

        public IEnumerable<SelectListItem> lsthelpdeskRecord { get; set; }

        public IEnumerable<SelectListItem> lsthelpdeskworkfollow { get; set; }

        public IEnumerable<SelectListItem> lsthelpdeskuser { get; set; }

        public bool hideBackButton { get; set; }

        [Display(Name = "HELP_DESK_FILE_ATTACHMENT", ResourceType = typeof(Resources))]
        public string fileUpload { get; set; }

        public string FilesToBeUploaded { get; set; }

        public Dictionary<string, string> selectedFileNames { get; set; }

        public string isCreate { get; set; }

        [UIHint("ComboBox")]
        //[Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "HELP_DESK_STATUS", ResourceType = typeof(Resources))]
        public int status_Id { get; set; }

        [UIHint("ComboBox")]
        //[Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "HELP_DESK_MODULE", ResourceType = typeof(Resources))]
        public string module_Id { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "LABEL_CREATED_DATE_FROM", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdDateFrom { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "LABEL_TO", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdDateTo { get; set; }

        public IEnumerable<SelectListItem> lstStatus { get; set; }
        public IEnumerable<SelectListItem> lstModule { get; set; }
    }

    public partial class HelpDeskViewModel
    {
        public List<HelpDeskModel> lstHelpDeskModel { get; set; }
    }
}