using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class AdhocReportModel
    {
        public string id { get; set; }

        public string reportfileID { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }


        public int organization_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "ADHOCREPORT_REPORTTYPEID", ResourceType = typeof(Resources))]
        public int reportType_Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ADHOCREPORT_REPORTNAME", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1} characters")]
        public string reportName { get; set; }

        [Display(Name = "ADHOCREPORT_REPORTFILENAME", ResourceType = typeof(Resources))]
        public string reportFileName { get; set; }

        [UIHint("FileBrowser")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ADHOCREPORT_REPORTFILENAME", ResourceType = typeof(Resources))]
        public string file { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "ADHOCREPORT_REMARKS", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string remarks { get; set; }

        [UIHint("HBool")]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "ADHOCREPORT_ISCOMMON", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public bool isCommon { get; set; }

        [UIHint("HBool")]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "ADHOCREPORT_ISVISIBLE", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public bool isVisible { get; set; }


        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

        public IEnumerable<SelectListItem> lstAdhocReportType { get; set; }

    }
    public partial class AdhocReportViewModel
    {
        public List<AdhocReportModel> lstAdhocReportModel { get; set; }
    }
}