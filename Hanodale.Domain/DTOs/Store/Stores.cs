using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Stores
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string company { get; set; }

        [DataMember]
        public string plant { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string address1 { get; set; }

        [DataMember]
        public string address2 { get; set; }

        [DataMember]
        public string address3 { get; set; }

        [DataMember]
        public int city_Id { get; set; }

        [DataMember]
        public string cityName { get; set; }    

        [DataMember]
        public int state_Id { get; set; }

        [DataMember]
        public string stateName { get; set; }

        [DataMember]
        public int country_Id { get; set; }

        [DataMember]
        public string countryName { get; set; }

        [DataMember]
        public string zip { get; set; }

        [DataMember]
        public string searchCity { get; set; }

        [DataMember]
        public string searchState { get; set; }

        [DataMember]
        public string searchCountry { get; set; }

        [DataMember]
        public string searchZip { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }

    public class StoreDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Stores> lstStore { get; set; }
    }
}
