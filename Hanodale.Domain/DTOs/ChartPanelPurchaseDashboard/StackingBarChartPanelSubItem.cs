using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class StackingBarChartPanelSubItem
    {
        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public string MonthName { get; set; }

        [DataMember]
        public string FullDateName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
