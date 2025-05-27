using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Hanodale.WebUI.Models
{
    public class CopyCalendarModel
    {
        public bool readOnly { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CALENDAR_SETTING_FROM_YEAR", ResourceType = typeof(Resources))]
        public int fromYear { get; set; }

        public IEnumerable<SelectListItem> fromYearList { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CALENDAR_SETTING_TO_YEAR", ResourceType = typeof(Resources))]
        public int toYear { get; set; }

        public IEnumerable<SelectListItem> toYearList { get; set; }

        [UIHint("HBool")]
        [Display(Name = "CALENDAR_SETTING_REPLACE", ResourceType = typeof(Resources))]
        public bool replace { get; set; }

    }
}