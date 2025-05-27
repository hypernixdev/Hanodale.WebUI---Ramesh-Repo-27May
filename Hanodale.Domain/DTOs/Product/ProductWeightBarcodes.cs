using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class ProductWeightBarcodes
    {
        public bool isEdit;

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int rowIndex { get; set; }

        [DataMember]
        public string epicorePartNo { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string fullBarcode { get; set; }

        [DataMember]
        public string barcode { get; set; }

        [DataMember]
        public string barcode1 { get; set; }

        [DataMember]
        public int barcodeFromPos { get; set; }

        [DataMember]
        public int barcodeToPos { get; set; }

        [DataMember]
        public int weightFromPos { get; set; }

        [DataMember]
        public int weightToPos { get; set; }

        [DataMember]
        public Nullable<decimal> weightMultiply { get; set; }

        [DataMember]
        public int barcodeLength { get; set; }

        [DataMember]
        public string offSet1 { get; set; }

        [DataMember]
        public string offSet2 { get; set; }

        [DataMember]
        public string weightValue { get; set; }
        [DataMember]
        public string weightValue1 { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }

        public string createdBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }

    public class ProductWeightBarcodeDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ProductWeightBarcodes> lstProductWeightBarcode { get; set; }
    }

    public class ProductWeightBarcodeFileUpload
    {
        [DataMember]
        public int organization_Id { get; set; }

        public List<ProductWeightBarcodes> lstProductWeightBarcode { get; set; }
    }

    public class ProductWeightBarcodeFileUploadResult
    {
        public bool isSuccessful { get; set; }

        public List<ProductWeightBarcodeFileUploadResultItem> lstItem { get; set; }
    }

    public class ProductWeightBarcodeFileUploadResultItem
    {
        public int rowIndex { get; set; }

        public string value { get; set; }
    }
}
