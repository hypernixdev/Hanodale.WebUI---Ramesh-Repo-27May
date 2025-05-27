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
    public class ChartPanelOrderDashboardModel
    {
        public string section { get; set; }

        public string title { get; set; }

        public int id { get; set; }

        public int organizationId { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "LABEL_FILTER_DATE", ResourceType = typeof(Resources))]
        public int filterTypeId { get; set; }

        [UIHint("ComboBox")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "DASHBOARD_CHART_TYPE", ResourceType = typeof(Resources))]
        public string chartType_Id { get; set; }

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

        public List<SelectListItem> lstChartType { get; set; }

        public List<BarChartDashboardModel> lstBarChart { get; set; }
        public List<PieChartModel> lstPie { get; set; }

        public List<SalesSummaryResult> lstSalesSummaryResult { get; set; }

    }

    public class SalesSummaryResult
    {
        public string displayLabel { get; set; }  // Total count of pending sales orders
        public decimal displayValue { get; set; }  // Total amount of pending sales orders
        public string displayDataType { get; set; }  // Count of sales orders

    }
    public class ChartPanelOrderDashboardSubItemModel
    {
        public string title { get; set; }
        public List<PieChartDashboardModel> lstPieChart { get; set; }
    }
}