using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class StackingBarChartPanelInspectionSubItem
    {
        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
