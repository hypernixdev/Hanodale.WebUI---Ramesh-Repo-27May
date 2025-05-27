using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class ShipToAddresses
    {

        [DataMember]
        public int id { get; set; }
      
        [DataMember]
        public string storeName { get; set; }    
       
        [DataMember]
        public string shippingCode { get; set; }
        [DataMember]
        public string address1 { get; set; }
        [DataMember]
        public string address2 { get; set; }
        [DataMember]
        public string address3 { get; set; }
       
        [DataMember]
        public string cityName { get; set; }
       
        [DataMember]
        public string stateName { get; set; }
      
        [DataMember]
        public string countryName { get; set;  }
        [DataMember]
        public string zip { get; set; }
      
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string custId { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }

    public class ShipToAddressDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ShipToAddresses> lstShipToAddress { get; set; }
    }
}
