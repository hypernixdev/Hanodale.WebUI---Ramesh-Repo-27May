using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class AddressCities
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public Nullable<int> filterDropdown_Id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public Nullable<int> addressState_Id { get; set; }
        [DataMember]
        public string addressStateName { get; set; }
        [DataMember]
        public Nullable<int> addressCountry_Id { get; set; }
        [DataMember]
        public string addressCountryName { get; set; }
    }

    public class AddressCityDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<AddressCities> lstAddressCity { get; set; }
    }
}
