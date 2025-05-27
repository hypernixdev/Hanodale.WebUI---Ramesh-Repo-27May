using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartPanelPurchase
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public Nullable<decimal> estimatedCost { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
        [DataMember]
        public string incidentReportNo { get; set; }
        [DataMember]
        public string reference { get; set; }
        [DataMember]
        public DateTime requiredDate { get; set; }
        [DataMember]
        public DateTime validityDate { get; set; }
    }

    public class ChartPanelPurchaseDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ChartPanelPurchase> lstPurchase { get; set; }
    }
}
