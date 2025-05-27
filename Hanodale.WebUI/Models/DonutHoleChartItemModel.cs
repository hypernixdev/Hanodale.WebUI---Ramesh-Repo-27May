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
    public class DonutHoleChartItemModel
    {
        public int value { get; set; }

        public string valueStr { get; set; }

        public int valuePercentage { get; set; }

        public string type { get; set; }

        public string chartType { get; set; }

        public string title { get; set; }

        public string backColor { get; set; }

        public int sortId { get; set; }

        //public string loadedDateFromString { get; set; }

        //public string loadedDateToString { get; set; }

        //public string lastUpdatedDateString { get; set; }

        //public string lastUpdatedTimeString { get; set; }

    }
}