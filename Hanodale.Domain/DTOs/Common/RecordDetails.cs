using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class RecordDetails
    {
        [DataMember]
        public int totalRecords { get; set; }

        [DataMember]
        public int totalDisplayRecords { get; set; }
    }
}
