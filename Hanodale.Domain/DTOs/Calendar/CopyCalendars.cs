using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class CopyCalendars
    {
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public int fromYear { get; set; }
        [DataMember]
        public int toYear { get; set; }
        [DataMember]
        public bool replace { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
    }
}
