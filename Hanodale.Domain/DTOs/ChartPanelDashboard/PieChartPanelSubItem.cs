using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class PieChartPanelSubItem
    {
        [DataMember]
        public string section { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public List<PieChartDashboard> pieChartItems { get; set; }

        [DataMember]
        public PieChartDashboard pieChartItem { get; set; }

    }
}
