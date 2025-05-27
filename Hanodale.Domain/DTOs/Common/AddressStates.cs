using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class AddressStates
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
        public Nullable<int> addressCountry_Id { get; set; }
        [DataMember]
        public string addressCountryName { get; set; }
    }

    public class AddressStateDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<AddressStates> lstAddressState { get; set; }
    }
}
