using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class CustomerPriceLists
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public bool isSuccess { get; set; }    
        [DataMember]
        public Nullable<int> CustNum { get; set; }
        [DataMember]
        public string ShipToNum { get; set; }
        [DataMember]
        public Nullable<int> SeqNum { get; set; }
        [DataMember]
        public string ListCode { get; set; }
        [DataMember]
        public string CurrencyCode { get; set; }
        [DataMember]
        public string ListDescription { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }

        [DataMember]
        public string CustGroup { get; set; }
    }
  
        public class CustomerPriceListDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<PriceLists> lstPriceList { get; set; }
    }
}
