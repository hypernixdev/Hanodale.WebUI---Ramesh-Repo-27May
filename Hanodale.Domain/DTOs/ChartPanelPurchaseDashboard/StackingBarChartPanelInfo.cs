using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class StackingBarChartPanelInfo : ChartInfo
    {
        [DataMember]
        public List<StackingBarChartPanelList> stackingBarChartPanelList { get; set; }

    }

    public class StackingBarChartPanelList
    {
        public string categoryName { get; set; }
        [DataMember]
        public List<StackingBarChartPanelSubItem> subItems { get; set; }
    }
}
