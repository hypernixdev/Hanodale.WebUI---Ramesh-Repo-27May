using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartPanelInfo : ChartInfo
    {
        [DataMember]
        public List<ChartDashboard> chartItems { get; set; }

        [DataMember]
        public ChartDashboard chartFilteredItem { get; set; }
    }
}
