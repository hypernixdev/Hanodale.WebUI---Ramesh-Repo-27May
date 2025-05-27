using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class UomConversions
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string partNum { get; set; }
        [DataMember]
        public string uomCode { get; set; }
        [DataMember]
        public string convFactor { get; set; }
        [DataMember]
        public string uniqueField { get; set; }
        [DataMember]    
        public string convOperator { get; set; }        
        public bool isSuccess { get; set; }    
     
    }

    public class UomConversionDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<UomConversions> lstUomConversion { get; set; }
    }

}
