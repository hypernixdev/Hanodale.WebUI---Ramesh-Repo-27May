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
    public class ShipToAddressModel
    {
        public string id { get; set; }
        public bool isEdit { get; set; }
        public AccessRightsModel accessRight { get; set; }
        public bool readOnly { get; set; }

        /* [UIHint("ComboBoxSearchable")]
         [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
         [CustomDisplayName("SHIPTOADDRESS_PLANT")]
         public Nullable<int> store_Id { get; set; }
         public TableProfileMetadataModel store_Id_Metadata { get; set; }

        [UIHint("ComboBoxSearchable")]
         [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
         [CustomDisplayName("SHIPTOADDRESS_CUSTOMER")]
         public Nullable<int> customer_Id { get; set; }
         public TableProfileMetadataModel customer_Id_Metadata { get; set; }*/

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_PLANT")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string storeName { get; set; }
        public TableProfileMetadataModel storeName_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_CUSTOMER_CODE")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string custId { get; set; }
        public TableProfileMetadataModel custId_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_SHIPPINGCODE")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string shippingCode { get; set; }

        public TableProfileMetadataModel shippingCode_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_ADDRESS1")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string address1 { get; set; }

        public TableProfileMetadataModel address1_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_ADDRESS2")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string address2 { get; set; }

        public TableProfileMetadataModel address2_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_ADDRESS3")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string address3 { get; set; }

        public TableProfileMetadataModel address3_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_ADDRESS_CITY")]
        public string cityName { get; set; }
        public TableProfileMetadataModel cityName_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_ADDRESS_STATE")]
        public string stateName { get; set; }
        public TableProfileMetadataModel stateName_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_ADDRESS_COUNTRY")]
        public string countryName { get; set; }
        public TableProfileMetadataModel countryName_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_ZIP")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string zip { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SHIPTOADDRESS_NAME")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string name { get; set; }


        public TableProfileMetadataModel zip_Metadata { get; set; }

       // public IEnumerable<SelectListItem> lstStore { get; set; }
     //   public IEnumerable<SelectListItem> lstCustomer { get; set; }
     //   public IEnumerable<SelectListItem> lstCity { get; set; }
     //   public IEnumerable<SelectListItem> lstState { get; set; }
     //   public IEnumerable<SelectListItem> lstCountry { get; set; }

    }
}