using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Plants
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
        public string city { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string zip { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }

    public class PlantDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Plants> lstPlant { get; set; }
    }
}
