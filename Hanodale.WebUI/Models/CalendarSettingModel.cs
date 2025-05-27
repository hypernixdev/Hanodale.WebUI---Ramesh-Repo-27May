using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class CalendarSettingModel
    {
        public int id { get; set; }

        public int organization_Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        public int calendarEvent_Id { get; set; }

        public bool isEdit { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string color { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        public DateTime startDate { get; set; }

        public Nullable<DateTime> endDate { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

        

    }

    public class CalendarSettingModelDetails
    {
        public bool readOnly { get; set; }
        public bool isEdit { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        public int year { get; set; }

        public AccessRightsModel userAccess { get; set; }

        public IEnumerable<SelectListItem> yearList { get; set; }

        public List<CalendarEventModel> calendarEventlist { get; set; }
        public List<CalendarSettingModel> calendarSettinglist { get; set; }
    }
}