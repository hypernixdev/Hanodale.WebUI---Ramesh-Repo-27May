using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Users
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int userRole_Id { get; set; }
        [DataMember]
        public int business_Id { get; set; }
        [DataMember]
        public Nullable<int> bussinessType_Id { get; set; }
        [DataMember]
        public int[] business_Ids { get; set; }
        [DataMember]
        public int defaultbusiness_Id { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string passwordHash { get; set; }
        [DataMember]
        public string oldPasswordHash { get; set; }
        [DataMember]
        public bool? verified { get; set; }
        [DataMember]
        public int? language { get; set; }
        [DataMember]
        public int defaultOrganizationId { get; set; }
        [DataMember]
        public int[] organization_Ids { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public DateTime? modifiedDate { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public bool? isTermsAccepted { get; set; }
        [DataMember]
        public int subCost_Id { get; set; }
        [DataMember]
        public string salaryId { get; set; }
        [DataMember]
        public string roleName { get; set; }
        [DataMember]
        public string businessName { get; set; }

        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string employeeNo { get; set; }
        [DataMember]
        public string department { get; set; }
        [DataMember]
        public string jobTitle { get; set; }
        [DataMember]
        public string mobileNo { get; set; }
        [DataMember]
        public string officeNo { get; set; }
        [DataMember]
        public string organisationName { get; set; }
        [DataMember]
        public Nullable<int> gred { get; set; }
        [DataMember]
        public string idNo { get; set; }
        [DataMember]
        public Nullable<System.DateTime> birthDate { get; set; }
        [DataMember]
        public Nullable<int> age { get; set; }
        [DataMember]
        public string accountNo { get; set; }
        [DataMember]
        public Nullable<int> bankId { get; set; }
        [DataMember]
        public Nullable<decimal> salary { get; set; }
        [DataMember]
        public Nullable<int> employeegroupId { get; set; }
        [DataMember]
        public Nullable<System.DateTime> entryDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> expireddate { get; set; }
        [DataMember]
        public Nullable<decimal> yearofservice { get; set; }
        [DataMember]
        public Nullable<bool> isAccessAllOrganization { get; set; }
        [DataMember]
        public Nullable<System.DateTime> businessExpiredDate { get; set; }
        [DataMember]
        public int supplierBusinessType_Id { get; set; }

        [DataMember]
        public string submenuName { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string statusName { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string remarks { get; set; }

        [DataMember]
        public string businessEmail { get; set; }
        [DataMember]
        public Nullable<int>record_Id { get; set; }
        [DataMember]
        public Nullable<int> subMenu_Id { get; set; }

        [DataMember]
        public bool HasFile { get; set; }
        [DataMember]
        public string urlPath { get; set; }
        [DataMember]
        public bool isProfileImageUpload { get; set; }

        public string landingPage { get; set; }
    }

    public class UserDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Users> lstUsers { get; set; }
    }
}