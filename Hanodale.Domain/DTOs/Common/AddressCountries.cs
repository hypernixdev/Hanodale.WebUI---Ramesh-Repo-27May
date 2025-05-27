using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class AddressCountries
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public Nullable<int> filterDropdown_Id { get; set; }
       
        [DataMember]
        public Nullable<int> currencyProfile_Id { get; set; }
        [DataMember]
        public string currencyProfileName { get; set; }
        [DataMember]
        public Nullable<int> regionalProfile_Id { get; set; }
        [DataMember]
        public string regionalProfileName { get; set; }
        [DataMember]
        public Nullable<int> languageProfile_Id { get; set; }
        [DataMember]
        public string languageProfileName { get; set; }
        [DataMember]
        public string countryCode { get; set; }
    }

    public class AddressCountryDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<AddressCountries> lstAddressCountry { get; set; }
    }
}
