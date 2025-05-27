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
    public class PieChartDashboardModel
    {
        public string sectionType { get; set; }

        public string type { get; set; }

        public string chartType { get; set; }

        public int sortId { get; set; }

        public string title { get; set; }

        public List<PieChartItemModel> listItems { get; set; }

    }
}