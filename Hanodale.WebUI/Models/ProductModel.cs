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
    public class ProductModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCT_PART_NUMBER")]
        public string partNumber { get; set; }
        public TableProfileMetadataModel partNumber_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCT_DESCRIPTION")]
        public string description { get; set; }
        public TableProfileMetadataModel description_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCT_CODE")]
        public string code { get; set; }
        public TableProfileMetadataModel code_Metadata { get; set; }

        [UIHint("ComboBox")]
        [CustomDisplayName("PRODUCT_CODE")]
        public string code_Id { get; set; }

        [UIHint("ComboBox")]
        [CustomDisplayName("PRODUCT_PART_NUMBER")]
        public string part_Id { get; set; }
        [CustomDisplayName("PRODUCT_PRODGRUP_DESCRIPTION")]
        public string prodGrup_Description { get; set; }
        public TableProfileMetadataModel prodGrup_Description_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_PART_CLASSID")]
        public string part_ClassID { get; set; }
        public TableProfileMetadataModel part_ClassID_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_PARTCLASS_DESCRIPTION")]
        public string partClass_Description { get; set; }
        public TableProfileMetadataModel partClass_Description_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_COUNTRY_DESCRIPTION")]
        public string country_Description { get; set; }
        public TableProfileMetadataModel country_Description_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_UOMCLASS_DEFUOMCODE")]
        public string uomClass_DefUomCode { get; set; }
        public TableProfileMetadataModel uomClass_DefUomCode_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_PART_SALESUM")]
        public string part_SalesUM { get; set; }
        public TableProfileMetadataModel part_SalesUM_Metadata { get; set; }
     
        [CustomDisplayName("PRODUCT_PART_IUM")]
        public string part_IUM { get; set; }
        public TableProfileMetadataModel part_IUM_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_UOMCLASS_BASEUOMCODE")]
        public string uomClass_BaseUOMCode { get; set; }
        public TableProfileMetadataModel uomClass_BaseUOMCode_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_COUNTRY_COUNTRYNUM")]
        public int country_CountryNum { get; set; }
        public TableProfileMetadataModel country_CountryNum_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_UOMCLASS_CLASSTYPE")]
        public string uomClass_ClassType { get; set; }
        public TableProfileMetadataModel uomClass_ClassType_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_UOMCLASS_DESCRIPTION")]
        public string uomClass_Description { get; set; }
        public TableProfileMetadataModel uomClass_Description_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_UNITPRICE")]
        public decimal unitPrice { get; set; }
        public TableProfileMetadataModel unitPrice_Metadata { get; set; }

        [CustomDisplayName("PRODUCT_DESCRIPTION")]
        public string searchDescription { get; set; }

        [CustomDisplayName("PRODUCT_PRODGRUP_DESCRIPTION")]
        public string searchGrupDescription { get; set; }

        [CustomDisplayName("PRODUCT_PARTCLASS_DESCRIPTION")]
        public string searchPartClassDescription { get; set; }

        [CustomDisplayName("PRODUCT_COUNTRY_DESCRIPTION")]
        public string searchCountryDescription { get; set; }

        public IEnumerable<SelectListItem> lstProductCode { get; set; }
        public IEnumerable<SelectListItem> lstProductPartNo { get; set; }

    }
    public partial class ProductMaintenanceModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }

        public ProductModel Product { get; set; }
    }
}