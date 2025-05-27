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
    public class BarChartDashboardModel
    {
        public string sectionType { get; set; }

        public string type { get; set; }

        public int sortId { get; set; }

        public List<BarChartDashboardSubItemModel> lstItem { get; set; }

    }

    public class BarChartDashboardSubItemModel
    {
        public string categoryName { get; set; }
        public int count { get; set; }

    }
}