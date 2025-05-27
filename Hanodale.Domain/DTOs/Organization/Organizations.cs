using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Organizations
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int? parent_Id { get; set; }

        [DataMember]
        public int parentOrgCategory_Id { get; set; }

        [DataMember]
        public string parentName { get; set; }

        [DataMember]
        public int orgCategory_Id { get; set; }

        [DataMember]
        public string orgCategoryName { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string prefix { get; set; }

        [DataMember]
        public bool isActive { get; set; }
        
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string sapcode { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public DateTime createdDate { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public DateTime? modifiedDate { get; set; }

        [DataMember]
        public Organizations subOrganization { get; set; }

        [DataMember]
        public bool isDefault { get; set; }

        [DataMember]
        public bool isMainCost { get; set; }

        [DataMember]
        public bool isSelected { get; set; }

        [DataMember]
        public bool hasCategoryChild { get; set; }
    }

    public class OrganizationDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Organizations> lstOrganizations { get; set; }
    }
}
