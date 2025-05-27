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
    public class CompanyProfileModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_CODE", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_CODE")]
        [StringLength(50, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string code { get; set; }

        public TableProfileMetadataModel code_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_NAME", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_NAME")]
        [StringLength(50, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string name { get; set; }

        public TableProfileMetadataModel name_Metadata { get; set; }


        [UIHint("TextArea")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_DESCRIPTION", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_DESCRIPTION")]
        [StringLength(1000, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string description { get; set; }

        public TableProfileMetadataModel description_Metadata { get; set; }


        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_TOTAL_CAPITAL", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_TOTAL_CAPITAL")]
        public Nullable<decimal> totalCapital { get; set; }

        public TableProfileMetadataModel totalCapital_Metadata { get; set; }


        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_COMPANYTYPE_ID", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_COMPANYTYPE")]
        public Nullable<int> companyType_Id { get; set; }

        public TableProfileMetadataModel companyType_Id_Metadata { get; set; }


        [UIHint("Number")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_NO_OF_USER", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_NO_OF_USER")]
        [Range(0, 1000, ErrorMessageResourceName = "AllowanceRange", ErrorMessageResourceType = typeof(Resources))]
        public Nullable<int> noOfUser { get; set; }

        public TableProfileMetadataModel noOfUser_Metadata { get; set; }


        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_SERVICE_ID", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_SERVICE_ID")]
        public Nullable<int> service_Id { get; set; }

        public TableProfileMetadataModel service_Id_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [PhoneNumber(ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_PHONE_NO", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_PHONE_NO")]
        [StringLength(50, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        //[Phone(ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string phoneNo { get; set; }

        public TableProfileMetadataModel phoneNo_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_EMAIL_ADDRESS", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_EMAIL_ADDRESS")]
        //[RegularExpression(Common.emailAddressPattern, ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(Resources))]
        [Email(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(255, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string emailAddress { get; set; }

        public TableProfileMetadataModel emailAddress_Metadata { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "COMPANYPROFILE_EFECTIVE_DATE", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_EFECTIVE_DATE")]
        public System.DateTime effectiveDate { get; set; }

        public TableProfileMetadataModel effectiveDate_Metadata { get; set; }

        [UIHint("HBool")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_IS_ACTIVE", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_IS_ACTIVE")]
        public Nullable<bool> isActive { get; set; }

        public TableProfileMetadataModel isActive_Metadata { get; set; }

        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name = "COMPANYPROFILE_TOTAL_REVENUE", ResourceType = typeof(Resources))]
        [CustomDisplayName("COMPANYPROFILE_TOTAL_REVENUE")]
        public Nullable<decimal> totalRevenue { get; set; }

        public TableProfileMetadataModel totalRevenue_Metadata { get; set; }


        public IEnumerable<SelectListItem> lstCompanyType { get; set; }

        public IEnumerable<SelectListItem> lstService { get; set; }

    }

    public partial class CompanyProfileMaintenanceModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }

        public CompanyProfileModel companyProfile { get; set; }
    }
}