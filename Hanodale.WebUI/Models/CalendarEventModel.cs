using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class CalendarEventModel
    {
        public string id { get; set; }

        public int organization_Id { get; set; }

        public bool isEdit { get; set; }
        public bool readOnly { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CALENDAR_EVENT_TITLE", ResourceType = typeof(Resources))]
        public string title { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "CALENDAR_EVENT_DESCRIPTION", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string description { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CALENDAR_EVENT_COLOR", ResourceType = typeof(Resources))]
        public string color { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CALENDAR_EVENT_ICON", ResourceType = typeof(Resources))]
        public string icon { get; set; }

        [UIHint("HBool")]
        [Display(Name = "CALENDAR_EVENT_VISIBILITY", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public bool visibility { get; set; }

        [UIHint("HBool")]
        [Display(Name = "CALENDAR_EVENT_ALLOW_TO_SELECT", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public bool allowToSelect { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

    }
    public partial class CalendarEventViewModel
    {
        public List<CalendarEventModel> lstCalendarEventModel { get; set; }
    }
}