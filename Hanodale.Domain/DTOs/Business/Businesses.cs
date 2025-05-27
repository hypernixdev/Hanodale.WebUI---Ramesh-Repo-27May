using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Businesses
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public int businessType_Id { get; set; }
        [DataMember]
        public int rfqMaster_Id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string prefix { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public string phone2 { get; set; }
        [DataMember]
        public string fax { get; set; }
        [DataMember]
        public string webSite { get; set; }
        [DataMember]
        public string primaryContact { get; set; }
        [DataMember]
        public string primaryEmail { get; set; }
        [DataMember]
        public Nullable<int> primaryCurrency { get; set; }
        [DataMember]
        public string remarks { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
        [DataMember]
        public bool status { get; set; }
        [DataMember]
        public int workCategory_Id { get; set; }
        // BUSINESS CLASSIFICATION MODEL
        [DataMember]
        public bool isCheck { get; set; }
        [DataMember]
        public int[] classification_Ids { get; set; }
        [DataMember]
        public int[] workCategoryIds { get; set; }
        [DataMember]
        public string businessTypeName { get; set; }
        [DataMember]
        public Nullable<bool> isMAHBRegistered { get; set; }
        [DataMember]
        public string registrationNo { get; set; }
        [DataMember]
        public Nullable<System.DateTime> registrationDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> expiryDate { get; set; }
        [DataMember]
        public string referenceId { get; set; }
        [DataMember]
        public string rocNo { get; set; }
        [DataMember]
        public string businessCategory { get; set; }
        [DataMember]
        public Nullable<int> paidUpCapital { get; set; }
        [DataMember]
        public Nullable<int> companyType { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string limitation { get; set; }
        [DataMember]
        public int[] organisationIds { get; set; }
        [DataMember]
        public Nullable<System.DateTime> profileLastUpdated { get; set; }
        [DataMember]
        public bool isDefault { get; set; }
        [DataMember]
        public string gstNo { get; set; }
        [DataMember]
        public int statusColor { get; set; }
        [DataMember]
        public bool isActive { get; set; }

        [DataMember]
        public int supplierBusinessType_Id { get; set; }

        
    }

    public class BusinessDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Businesses> lstBusiness { get; set; }
    }
}