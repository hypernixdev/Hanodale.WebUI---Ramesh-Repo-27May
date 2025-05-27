using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class BusinessOrganizations
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int business_Id { get; set; }
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
    }

    public class BusinessOrganizationDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<BusinessOrganizations> lstBusinessOrganization { get; set; }
    }
}
