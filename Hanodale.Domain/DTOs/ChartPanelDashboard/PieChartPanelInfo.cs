using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class PieChartPanelInfo : ChartInfo
    {
        [DataMember]
        public List<PieChartPanelSubItem> pieChartSubItems { get; set; }

        [DataMember]
        public PieChartPanelSubItem pieChartFilteredItem { get; set; }

        [DataMember]
        public string module { get; set; }

        [DataMember]
        public string workOrderSectionType { get; set; }

    }
}
