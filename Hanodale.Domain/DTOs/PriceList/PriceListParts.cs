using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class PriceListParts
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public bool isSuccess { get; set; }    
        [DataMember]
        public string listCode { get; set; }
        [DataMember]
        public string partNum { get; set; }
        [DataMember]
        public Nullable<decimal> basePrice { get; set; }
        [DataMember]
        public string uomCode { get; set; }
       
    }
        public class PriceListPartsDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<PriceListParts> lstPriceListParts { get; set; }
    }
}
