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
    public class StackingBarChartItemModel
    {
        public Nullable<int> Year { get; set; }

        public Nullable<int> Month { get; set; }

        public string MonthName { get; set; }

        public string FullDateName { get; set; }

        public Nullable<int> Count { get; set; }

        public long DateTimeSpan { get; set; }

    }
}