using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class BarChartPanelOrderInfo : ChartInfo
    {
        [DataMember]
        public List<BarChartPanelOrders> barChartPanelOrderList { get; set; }
    }

    public class BarChartPanelOrders
    {
        public string sectionType { get; set; }
        public List<BarChartPanelOrderItems> lstItem { get; set; }

    }

    public class BarChartPanelOrderItems
    {
        public string categoryName { get; set; }
        [DataMember]
        public int count { get; set; }
    }
}
