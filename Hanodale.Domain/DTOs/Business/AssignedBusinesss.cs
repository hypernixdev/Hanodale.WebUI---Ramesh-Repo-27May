using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class AssignedBusinesss
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int user_Id { get; set; }
        [DataMember]
        public int business_Id { get; set; }
        [DataMember]
        public bool isDefault { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
    }

    public class AssignedBusinesssDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<AssignedBusinesss> lstAssignedBusinesss { get; set; }
    }
}
