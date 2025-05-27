using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Customers
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string custID { get; set; }
        [DataMember]
        public string address1 { get; set; }
        [DataMember]
        public string address2 { get; set; }

        [DataMember]
        public string address3 { get; set; }

        [DataMember]
        public string city { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string orderNum { get; set; }
        [DataMember]
        public Nullable<System.DateTime> orderDate { get; set; }
        [DataMember]
        public string orderStatus { get; set; }
        [DataMember]
        public string searchCode { get; set; }
        [DataMember]
        public string searchName { get; set; }
        [DataMember]
        public string searchCity { get; set; }
        [DataMember]
        public string searchState { get; set; }
        [DataMember]
        public string searchCountry { get; set; }
        [DataMember]
        public string searchOrderCode { get; set; }
        [DataMember]
        public Nullable<System.DateTime> searchOrderDateFrom { get; set; }

        [DataMember]
        public Nullable<System.DateTime> searchOrderDateTo { get; set; }
        [DataMember]
        public string searchOrderStatus { get; set; }
        [DataMember]
        public string groupCode { get; set; }
        
        [DataMember]
        public bool isSuccess { get; set; }

        [DataMember]
        public bool creditHold { get; set; }
    }

    public class CustomerDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Customers> lstCustomer { get; set; }
    }
}
