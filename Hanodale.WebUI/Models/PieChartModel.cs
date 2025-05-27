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
    public class PieChartModel
    {
        public string title { get; set; }
        public string type { get; set; }

        public int count { get; set; }

        public List<PieChartItemModel> listItems { get; set; }

    }

    public class PieChartItemModel
    {
        public int value { get; set; }

        public int valuePercentage { get; set; }

        public int count { get; set; }

        public string type { get; set; }

        public string dataType { get; set; }

    }
}