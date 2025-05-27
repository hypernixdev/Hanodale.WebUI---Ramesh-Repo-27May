using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class PriceLists
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public bool isSuccess { get; set; }    
        [DataMember]
        public string listCode { get; set; }
        [DataMember]
        public string currencyCode { get; set; }
        [DataMember]
        public string listDescription { get; set; }
        [DataMember]
        public int? custNum { get; set; }// will refer to custID
        [DataMember]
        public string shipToNum { get; set; }
        [DataMember]
        public int? seqNum { get; set; }
        [DataMember]
        public DateTime? startDate { get; set; }
        [DataMember]
        public DateTime? endDate { get; set; }

        [DataMember]
        public string custID { get; set; } 
        [DataMember]
        public string CustGroup { get; set; }

    }
  
        public class PriceListDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<PriceLists> lstPriceList { get; set; }
    }
}
