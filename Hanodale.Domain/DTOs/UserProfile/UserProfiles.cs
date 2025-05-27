using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
     [DataContract]
    public class UserProfiles
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public int userType_Id { get; set; }
        [DataMember]
        public int person_Id { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string passwordHash { get; set; }
        [DataMember]
        public Nullable<bool> verified { get; set; }
        [DataMember]
        public Nullable<int> language { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }

    public class UserProfileDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<UserProfiles> lstUserProfile { get; set; }
    }
}
