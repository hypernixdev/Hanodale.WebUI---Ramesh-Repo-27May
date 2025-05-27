using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class StackingBarChartPanelTicketInfo : ChartInfo
    {
        [DataMember]
        public List<StackingBarChartPanelTicketList> stackingBarChartPanelList { get; set; }

    }

    public class StackingBarChartPanelTicketList
    {
        public string categoryName { get; set; }
        [DataMember]
        public List<StackingBarChartPanelTicketSubItem> subItems { get; set; }
    }
}
