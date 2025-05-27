using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Assets
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int? parentId { get; set; }

        [DataMember]
        public int organization_Id { get; set; }

        [DataMember]
        public int orgCategory_Id { get; set; }

        [DataMember]
        public int assetorganization_Id { get; set; }

        [DataMember]
        public int? assetType_Id { get; set; }

        [DataMember]
        public bool allowChild { get; set; }

        [DataMember]
        public int category_Id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public bool isRoot { get; set; }

        [DataMember]
        public bool canAdd { get; set; }

        [DataMember]
        public bool canEdit { get; set; }

        [DataMember]
        public bool canDelete { get; set; }

         [DataMember]
        public bool isMainCost { get; set; }

         [DataMember]
        public bool isSubCost { get; set; }

        [DataMember]
        public bool isStatic { get; set; }

        [DataMember]
        public bool isActiveBranch { get; set; }

        [DataMember]
        public bool hasChild { get; set; }

        [DataMember]
        public int childCount { get; set; }

        [DataMember]
        public int subOrganizationCount { get; set; }

        [DataMember]
        public string imageUrl { get; set; }

        [DataMember]
        public string backColor { get; set; }

        [DataMember]
        public string forceColor { get; set; }

        [DataMember]
        public int ordering { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public DateTime createdDate { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }

        [DataMember]
        public List<Assets> additionalParameters = new List<Assets>();// { get; set; }

        [DataMember]
        public string model { get; set; }

        [DataMember]
        public string manufacturer { get; set; }

        [DataMember]
        public string serial { get; set; }

        [DataMember]
        public string barcode { get; set; }

        [DataMember]
        public string otherProductNo { get; set; }

        [DataMember]
        public string remarks { get; set; }

        [DataMember]
        public string aisle { get; set; }

        [DataMember]
        public string row { get; set; }

        [DataMember]
        public string bin { get; set; }

        [DataMember]
        public Nullable<bool> isMsi { get; set; }

        [DataMember]
        public string categoryName { get; set; }

        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string organizationName { get; set; }

        [DataMember]
        public string parentName { get; set; }

        [DataMember]
        public List<int> assetCategoryConfig { get; set; }

        [DataMember]
        public Nullable<int> supplierBusiness_Id { get; set; }

        [DataMember]
        public Nullable<int> supplierUser_Id { get; set; }

        [DataMember]
        public Nullable<int> sectionBusiness_Id { get; set; }

        [DataMember]
        public Nullable<int> sectionUser_Id { get; set; }

        [DataMember]
        public string serviceTag { get; set; }

        [DataMember]
        public Nullable<System.DateTime> commisionedDate { get; set; }

        [DataMember]
        public Nullable<decimal> downtimeCost { get; set; }

        [DataMember]
        public string lifespan { get; set; }

        [DataMember]
        public Nullable<int> priority { get; set; }

        [DataMember]
        public bool status { get; set; }

        [DataMember]
        public Nullable<int> radioSpare { get; set; }

        [DataMember]
        public Nullable<int> radioComponent { get; set; }

        [DataMember]
        public Nullable<int> radioSchedule { get; set; }

        [DataMember]
        public Nullable<int> radionone { get; set; }
        [DataMember]
        public string assetTypeName { get; set; }

        [DataMember]
        public string sectionBusinessName { get; set; }


        [DataMember]
        public int tools_Id { get; set; }

        [DataMember]
        public int[] assetMaster_Ids { get; set; }

        [DataMember]
        public bool isEdit { get; set; }

        [DataMember]
        public int mainCostCenter_Id { get; set; }

        [DataMember]
        public int subCostCenter_Id { get; set; }
    }

    public class AssetDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Assets> lstAsset { get; set; }
    }
}
