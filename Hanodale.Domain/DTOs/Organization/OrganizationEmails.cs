using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class OrganizationEmails
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int organization_Id { get; set; }

        [DataMember]
        public int department_Id { get; set; }

        [DataMember]
        public string emailTo { get; set; }

        [DataMember]
        public string emailFrom { get; set; }

        [DataMember]
        public string userName { get; set; }

        [DataMember]
        public string password { get; set; }

        [DataMember]
        public string smtp { get; set; }

        [DataMember]
        public int smptPort { get; set; }

        [DataMember]
        public bool isSSL { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public DateTime createdDate { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public DateTime? modifiedDate { get; set; }

        [DataMember]
        public string departmentName { get; set; }

    }

    public class OrganizationEmailDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<OrganizationEmails> lstOrganizationEmails { get; set; }
    }
}
