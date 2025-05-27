using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;
using Hanodale.WebUI.Helpers;
using System.ComponentModel;
using Hanodale.BusinessLogic;
using Microsoft.Practices.ServiceLocation;

namespace Hanodale.WebUI.Models
{
    public class ProductCartonModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }
        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_EPICODE_PART_NO")]
        public string epicorPartNo { get; set; }
        public TableProfileMetadataModel epicorPartNo_Metadata { get; set; }

        
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_BARCODE")]
        public string barcode { get; set; }
        public TableProfileMetadataModel barcode_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_VENDOR_PRODUCT_CODE")]
        public string vendorProductCode { get; set; }
        public TableProfileMetadataModel vendorProductCode_Metadata { get; set; }

        [UIHint("Decimal")]
       // [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_PRODUCTKEY")]
        public Nullable<decimal> productKey { get; set; }
        public TableProfileMetadataModel productKey_Metadata { get; set; }

        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_PRODUCTBARCODELENGTH")]
        public Nullable<decimal> productBarcodeLength { get; set; }
        public TableProfileMetadataModel productBarcodeLength_Metadata { get; set; }

        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_PRODUCTCODEFROMPOSITION")]
        public Nullable<decimal> productCodeFromPosition { get; set; }
        public TableProfileMetadataModel productCodeFromPosition_Metadata { get; set; }

        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_PRODUCTCODETOPOSITION")]
        public Nullable<decimal> productCodeToPosition { get; set; }
        public TableProfileMetadataModel productCodeToPosition_Metadata { get; set; }

        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_WEIGHTFROMPOSITION")]
        public Nullable<decimal> weightFromPosition { get; set; }
        public TableProfileMetadataModel weightFromPosition_Metadata { get; set; }

        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_WEIGHTTOPOSITION")]
        public Nullable<decimal> weightToPosition { get; set; }
        public TableProfileMetadataModel weightToPosition_Metadata { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_WEIGHTVALUE")]
        public string weightValue { get; set; }
        public string weightValue1 { get; set; }

        public TableProfileMetadataModel weightValue_Metadata { get; set; }

        [UIHint("Decimal")]
        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_WEIGHTMUTIPLIER")]
        public Nullable<decimal> weightMutiplier { get; set; }
        public Nullable<decimal> weightMutiplier1 { get; set; }

        public TableProfileMetadataModel weightMutiplier_Metadata { get; set; }

        public IEnumerable<SelectListItem> lstProduct { get; set; }

        // Additional properties for ProductCarton model
        // You may add other properties as needed
    }
}