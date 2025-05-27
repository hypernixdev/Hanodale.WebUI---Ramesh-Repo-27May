using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class AuthenticateUser
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string firstName { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string pwd { get; set; }

        [DataMember]
        public int language { get; set; }

        [DataMember]
        public bool verified { get; set; }

        [DataMember]
        public int userRole_Id { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public int defaultOrganization { get; set; }

        [DataMember]
        public DateTime? expireDate { get; set; }

        [DataMember]
        public int supplierBusinessType_Id { get; set; }

        [DataMember]
        public string urlPath { get; set; }
    }
}