using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartPanelWorkOrder
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string requester_Id { get; set; }
        [DataMember]
        public Nullable<DateTime> requiredDate { get; set; }
        [DataMember]
        public string asset { get; set; }
        [DataMember]
        public string section { get; set; }
        [DataMember]
        public string jobStatus { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
        [DataMember]
        public Nullable<DateTime> estimatedStartDate { get; set; }
        [DataMember]
        public Nullable<DateTime> estimatedEndDate { get; set; }
        [DataMember]
        public Nullable<DateTime> actualStartDate { get; set; }
        [DataMember]
        public Nullable<DateTime> actualEndDate { get; set; }
        [DataMember]
        public string code1 { get; set; }
        [DataMember]
        public string rootcause { get; set; }
    }

    public class ChartPanelWorkOrderDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ChartPanelWorkOrder> lstWorkOrder { get; set; }
    }
}
