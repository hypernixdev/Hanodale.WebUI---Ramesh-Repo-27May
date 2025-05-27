using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Districts
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string company { get; set; }

        [DataMember]
        public string districtID { get; set; }

        [DataMember]
        public string districtDesc { get; set; }

        [DataMember]
        public string sysRowID { get; set; }

    }
}
