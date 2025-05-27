using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Staffs
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string jobTitle { get; set; }
        [DataMember]
        public Nullable<bool> isActive { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }
    public class StaffDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Staffs> lstStaff { get; set; }
    }
}
