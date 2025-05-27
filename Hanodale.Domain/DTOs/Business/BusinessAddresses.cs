using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class BusinessAddresses
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int business_Id { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string province { get; set; }
        [DataMember]
        public string postalCode { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }

        // BUSINESS CLASSIFICATION MODEL
        [DataMember]
        public bool isCheck { get; set; } 
        [DataMember]
        public int[] classification_Ids { get; set; }
    }

    public class BusinessAddressDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<BusinessAddresses> lstBusinessAddress { get; set; }
    }
}