using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Newss
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string loggedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> loggedDate { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }
    public class NewsDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }
        [DataMember]
        public List<Newss> lstNews { get; set; }
    }
}
