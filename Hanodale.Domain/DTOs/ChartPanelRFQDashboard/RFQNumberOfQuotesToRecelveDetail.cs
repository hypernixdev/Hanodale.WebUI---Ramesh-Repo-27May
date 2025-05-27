using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class RFQNumberOfQuotesToRecelve
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string rfqStatus { get; set; }

        [DataMember]
        public Nullable<decimal> totalPrice { get; set; }

        [DataMember]
        public Nullable<System.DateTime> adminSubmitDate { get; set; }

        [DataMember]
        public string numberOfQuotes { get; set; }
    }

    public class RFQNumberOfQuotesToRecelveDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<RFQNumberOfQuotesToRecelve> lstRFQ { get; set; }
    }
}
