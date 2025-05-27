using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class StackingBarChartPanelInspectionInfo : ChartInfo
    {
        [DataMember]
        public List<StackingBarChartPanelInspectionList> stackingBarChartPanelList { get; set; }

    }

    public class StackingBarChartPanelInspectionList
    {
        public string categoryName { get; set; }
        [DataMember]
        public List<StackingBarChartPanelInspectionSubItem> subItems { get; set; }
    }
}
