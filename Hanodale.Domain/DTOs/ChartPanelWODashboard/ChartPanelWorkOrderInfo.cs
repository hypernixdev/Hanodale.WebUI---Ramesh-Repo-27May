using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartPanelWorkOrderInfo : ChartWOInfo
    {
        [DataMember]
        public List<ChartWODashboard> chartItems { get; set; }

        [DataMember]
        public ChartWODashboard chartFilteredItem { get; set; }

        //[DataMember]
        //public ChartWODashboard request { get; set; }
        //[DataMember]
        //public ChartWODashboard highPriority { get; set; }
        //[DataMember]
        //public ChartWODashboard onTimeCompletion { get; set; }
        //[DataMember]
        //public ChartWODashboard overdue { get; set; }
    }
}
