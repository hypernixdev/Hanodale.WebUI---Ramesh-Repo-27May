using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class ProductCartons
    {
       

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int rowIndex { get; set; }

        [DataMember]
        public string epicorPartNo { get; set; }

        [DataMember]
        public string barcode { get; set; }

        [DataMember]
        public string vendorProductCode { get; set; }

        [DataMember]
        public Nullable<decimal> productKey { get; set; }

        [DataMember]
        public Nullable<decimal> productBarcodeLength { get; set; }

        [DataMember]
        public Nullable<decimal> productCodeFromPosition { get; set; }

        [DataMember]
        public Nullable<decimal> productCodeToPosition  { get; set; }

        [DataMember]
        public Nullable<decimal> weightFromPosition { get; set; }

        [DataMember]
        public Nullable<decimal> weightToPosition { get; set; }

        [DataMember]
        public string weightValue { get; set; }

        [DataMember]
        public string weightValue1 { get; set; }

        [DataMember]
        public Nullable<decimal> weightMutiplier { get; set; }


        [DataMember]
        public Nullable<decimal> weightMutiplier1 { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
        [DataMember]
        public bool isSuccess { get; set; }

        [DataMember]
        public string partName { get; set; }
        [DataMember]
        public string BulkQty { get; set; }
    }

    public class ProductCartonDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ProductCartons> lstProductCarton { get; set; }
    }

    public class ProductCartonFileUpload
    {
        public int organization_Id { get; set; }

        public List<ProductCartons> lstProductCarton { get; set; }
    }

    public class ProductCartonFileUploadResult
    {
        public bool isSuccessful { get; set; }

        public List<ProductCartonFileUploadResultItem> lstItem { get; set; }
    }

    public class ProductCartonFileUploadResultItem
    {
        public int rowIndex { get; set; }

        public string value { get; set; }
    }
}
