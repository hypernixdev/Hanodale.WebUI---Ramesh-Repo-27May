using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class OrganizationEmailModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public string organization_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATIONEMAIL_DEPARTMENT_NAME", ResourceType = typeof(Resources))]
        public int department_Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATION_EMAILTO", ResourceType = typeof(Resources))]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Enter a valid email")]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string emailTo { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATION_EMAILFROM", ResourceType = typeof(Resources))]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Enter a valid email")]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string emailFrom { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATION_USERNAME", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string userName { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATION_PASSWORD", ResourceType = typeof(Resources))]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string password { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATION_SMTP", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string smtp { get; set; }

        [UIHint("Number")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ORGANIZATION_SMTPPORT", ResourceType = typeof(Resources))]
        [Range(0, 5000)]
        public int smptPort { get; set; }

        [UIHint("HBool")]
        [Display(Name = "ORGANIZATION_ISSL", ResourceType = typeof(Resources))]
        public bool isSSL { get; set; }

        public string createdBy { get; set; }

        public System.DateTime createdDate { get; set; }

        public string modifiedBy { get; set; }

        public Nullable<System.DateTime> modifiedDate { get; set; }

        public IEnumerable<SelectListItem> lstdepartment { get; set; }
    }
    public partial class OrganizationEmailViewModel
    {
        public List<OrganizationEmailModel> lstOrganizationEmailModel { get; set; }
    }
}