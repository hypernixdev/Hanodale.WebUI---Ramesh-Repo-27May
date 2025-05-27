using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class PieChartPanelWorkOrderInfo : ChartWOInfo
    {
        [DataMember]
        public List<PieChartPanelWOSubItem> pieChartSubItems { get; set; }

        [DataMember]
        public PieChartPanelWOSubItem pieChartFilteredItem { get; set; }

        //[DataMember]
        //public string rootCauseItemType { get; set; }
        //[DataMember]
        //public string assetTypeItemType { get; set; }
        //[DataMember]
        //public string sectionItemType { get; set; }
    }
}
