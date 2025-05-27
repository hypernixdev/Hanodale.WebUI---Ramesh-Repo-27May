using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class ChartPanelTicketDashboardModel
    {
        public string section { get; set; }

        public int chartType { get; set; }

        public int id { get; set; }

        public int organizationId { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "LABEL_FILTER_DATE", ResourceType = typeof(Resources))]
        public int filterTypeId { get; set; }
        
        public string type { get; set; }

        public Nullable<System.DateTime> startDate { get; set; }
        
        public Nullable<System.DateTime> endDate { get; set; }

        public string dashboardBoxName { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "LABEL_DATE_FROM", ResourceType = typeof(Resources))]
        public DateTime loadedDateFrom { get; set; }

        [UIHint("Date")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "LABEL_DATE_TO", ResourceType = typeof(Resources))]
        public DateTime loadedDateTo { get; set; }

        public string loadedDateFromString { get; set; }

        public string loadedDateToString { get; set; }

        public DateTime lastUpdatedDate { get; set; }
        public string lastUpdatedDateString { get; set; }
        public string lastUpdatedTimeString { get; set; }

        public IEnumerable<SelectListItem> lstFilterType { get; set; }

        public List<DonutHoleChartItemModel> lstDonutHole { get; set; }
        public List<StackingBarChartDashboardModel> lstStackingBarChart { get; set; }
        public List<ChartPanelTicketDashboardSubItemModel> lstPieChartSubItem { get; set; }

    }

    public class ChartPanelTicketDashboardSubItemModel
    {
        public string title { get; set; }
        public List<PieChartDashboardModel> lstPieChart { get; set; }
    }
}