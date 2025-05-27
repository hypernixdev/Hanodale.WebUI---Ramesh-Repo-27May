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
    public class StoreModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_COMPANY")]
        public string company { get; set; }
        public TableProfileMetadataModel company_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_NAME")]
        public string name { get; set; }
        public TableProfileMetadataModel name_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_CODE")]
        public string plant { get; set; }
        public TableProfileMetadataModel plant_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_ADDRESS1")]
        public string address1 { get; set; }
        public TableProfileMetadataModel address1_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_ADDRESS2")]
        public string address2 { get; set; }
        public TableProfileMetadataModel address2_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_ADDRESS3")]
        public string address3 { get; set; }
        public TableProfileMetadataModel address3_Metadata { get; set; }


        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_CITY")]
        public Nullable<int> city_Id { get; set; }
        public TableProfileMetadataModel city_Id_Metadata { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_STATE")]
        public Nullable<int> state_Id { get; set; }
        public TableProfileMetadataModel state_Id_Metadata { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_COUNTRY")]
        public Nullable<int> country_Id { get; set; }
        public TableProfileMetadataModel country_Id_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STORE_ZIP")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string zip { get; set; }

        public TableProfileMetadataModel zip_Metadata { get; set; }

        public string stateName { get; set; }
        public string cityName { get; set; }
        public string countryName { get; set; }

        [CustomDisplayName("STORE_CITY")]
        public string searchCity { get; set; }

        [CustomDisplayName("STORE_STATE")]
        public string searchState { get; set; }

        [CustomDisplayName("STORE_COUNTRY")]
        public string searchCountry { get; set; }

        [CustomDisplayName("STORE_ZIP")]
        public string searchZip { get; set; }


        public IEnumerable<SelectListItem> lstStoreCode { get; set; }
        public IEnumerable<SelectListItem> lstStorePartNo { get; set; }
        public IEnumerable<SelectListItem> lstCity { get; set; }
        public IEnumerable<SelectListItem> lstState { get; set; }
        public IEnumerable<SelectListItem> lstCountry { get; set; }



    }
}