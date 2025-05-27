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
    public class StackingBarChartDashboardModel
    {
        public string sectionType { get; set; }

        public string type { get; set; }

        public string chartType { get; set; }

        public int sortId { get; set; }

        public string title { get; set; }

        public List<StackingBarChartDashboardSubItemModel> lstGroupedItems { get; set; }

    }

    public class StackingBarChartDashboardSubItemModel
    {
        public string categoryName { get; set; }
        public List<StackingBarChartItemModel> lstItems { get; set; }

    }
}