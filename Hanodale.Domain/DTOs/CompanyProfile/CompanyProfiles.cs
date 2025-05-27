using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class CompanyProfiles
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public Nullable<decimal> totalCapital { get; set; }
        [DataMember]
        public Nullable<int> companyType_Id { get; set; }
        [DataMember]
        public string companyTypeName { get; set; }
        [DataMember]
        public Nullable<int> noOfUser { get; set; }
        [DataMember]
        public Nullable<int> service_Id { get; set; }
        [DataMember]
        public string serviceName { get; set; }
        [DataMember]
        public string phoneNo { get; set; }
        [DataMember]
        public string emailAddress { get; set; }
        [DataMember]
        public System.DateTime effectiveDate { get; set; }
        [DataMember]
        public Nullable<bool> isActive { get; set; }
        [DataMember]
        public Nullable<decimal> totalRevenue { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }

    public class CompanyProfileDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<CompanyProfiles> lstCompanyProfile { get; set; }
    }
}
