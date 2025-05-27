using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class UserModel
    {
        [Required]
        public string id { get; set; }

        public bool readOnly { get; set; }

        public int business_Id { get; set; }

        public string businessId { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_ROLE", ResourceType = typeof(Resources))]
        public int userRole_Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_FIRST_NAME", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1} characters")]
        public string firstName { get; set; }

        [Display(Name = "USER_LAST_NAME", ResourceType = typeof(Resources))]
        //[Display(Name = "")]
        //[HiddenInput(DisplayValue = false)]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string lastName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_EMAIL", ResourceType = typeof(Resources))]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Enter a valid email")]
        [StringLength(255, ErrorMessage = "The Maximum length is {1}")]
        public string email { get; set; }

        [Display(Name = "USER_LOGIN_ID", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessage = "The Maximum length is {1} characters")]
        public string userName { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_PASSWORD", ResourceType = typeof(Resources))]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string passwordHash { get; set; }

        public bool? verified { get; set; }

        public int defaultOrganizationId { get; set; }

        public int? language { get; set; }

        public string createdBy { get; set; }

        public DateTime createdDate { get; set; }

        public string modifiedBy { get; set; }

        public DateTime? modifiedDate { get; set; }

        public string avatarUrl { get; set; }

        [UIHint("HBool")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_STATUS", ResourceType = typeof(Resources))]
        public bool status { get; set; }

        public bool isEdit { get; set; }

        public bool isActive { get; set; }

        [UIHint("MultiSelectComboBox")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_ASSIGNED_ORGANIZATION", ResourceType = typeof(Resources))]
        public int[] organization_Ids { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_DEFAULT_ORGANIZATION", ResourceType = typeof(Resources))]
        public int defaultOrganization_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        //[Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_BUSINESSTYPE", ResourceType = typeof(Resources))]
        public Nullable<int> businessType_Id { get; set; }

        [UIHint("MultiSelectComboBox")]
        //[Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_ASSIGNED_BUSINESS", ResourceType = typeof(Resources))]
        public int[] business_Ids { get; set; }

        [UIHint("ComboBoxSearchable")]
        //[Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_BUSINESS", ResourceType = typeof(Resources))]
        public int defaultbusiness_Id { get; set; }

        public bool? isTermsAccepted { get; set; }

        public string roleName { get; set; }
        [UIHint("TextArea")]
        [Display(Name = "USER_ADDRESS", ResourceType = typeof(Resources))]
        //[StringLength(, ErrorMessage = "The Maximum length is {1}")]
        public string address { get; set; }

        [Display(Name = "USER_GRID_EMPLOYEE_NO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string employeeNo { get; set; }

        [Display(Name = "USER_DEPARTMENT", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string department { get; set; }

        [Display(Name = "USER_JOBTITLE", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1} characters")]
        public string jobTitle { get; set; }

        [Display(Name = "USER_MOBILENO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string mobileNo { get; set; }

        [Display(Name = "USER_OFFICENO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string officeNo { get; set; }

        [Display(Name = "USER_SALARYID", ResourceType = typeof(Resources))]
       // [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string salaryId { get; set; }

        //[Display(Name = "USER_SALARYID", ResourceType = typeof(Resources))]
        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        //public string staffNo { get; set; }


        [UIHint("ComboBoxSearchable")]
        [Display(Name = "USER_GRED", ResourceType = typeof(Resources))]
        public int? gred { get; set; }

        [Display(Name = "USER_IDNO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string idNo { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "USER_BIRTHDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> birthDate { get; set; }

        [Display(Name = "USER_AGE", ResourceType = typeof(Resources))]
        public Nullable<int> age { get; set; }

        [Display(Name = "USER_ACCOUNTNO", ResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessage = "The Maximum length is {1}")]
        public string accountNo { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "USER_BANK", ResourceType = typeof(Resources))]
        public Nullable<int> bankId { get; set; }

        [UIHint("Decimal")]
        [Display(Name = "USER_SALARY", ResourceType = typeof(Resources))]
        [Range(1, 1000000)]
        public Nullable<decimal> salaryAmount { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "USER_EMPLOYEEGROUP", ResourceType = typeof(Resources))]
        public Nullable<int> employeegroupId { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "USER_ENTRYDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> entryDate { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "USER_EXPIREDDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> expireddate { get; set; }

        [Display(Name = "USER_YEAROFSERVICE", ResourceType = typeof(Resources))]
        [Range(0, 50)]
        public Nullable<decimal> yearofservice { get; set; }

        [UIHint("HBool")]
        [Display(Name = "USER_VISIBILITY", ResourceType = typeof(Resources))]
        public Nullable<bool> isAccessAllOrganization { get; set; }

        public bool isBack { get; set; }

        public IEnumerable<SelectListItem> UserRoles { get; set; }
        public IEnumerable<SelectListItem> UserBusinesses { get; set; }
        public IEnumerable<SelectListItem> lstOrganizationItem { get; set; }
        public List<SelectListItem> lstDefaultOrganizationItem = new List<SelectListItem>();
        public IEnumerable<SelectListItem> lstOrganization { get; set; }
        public IEnumerable<SelectListItem> lstBank { get; set; }
        public IEnumerable<SelectListItem> lstGred { get; set; }
        public IEnumerable<SelectListItem> lstEmployeeGroup { get; set; }
        public IEnumerable<SelectListItem> lstBusinessItem { get; set; }
        public IEnumerable<SelectListItem> lstBusinessType { get; set; }

        [Display(Name = "STOCKMASTER_IMAGE", ResourceType = typeof(Resources))]
        public string image { get; set; }

        //[Display(Name = "STOCKMASTER_URLPATH", ResourceType = typeof(Resources))]
        public bool HasFile { get; set; }
        public bool ImgRemoved { get; set; }

        public string URLPathPreview { get; set; }

        [UIHint("FileBrowserRemovable")]
        [Display(Name = "STOCKMASTER_URLPATH", ResourceType = typeof(Resources))]
        public string URLPath { get; set; }

        [UIHint("FileBrowser")]
        [Display(Name = "STOCKMASTER_URLPATH", ResourceType = typeof(Resources))]
        public string file { get; set; }

    }

    public class UserInfoModel
    {
        public string id { get; set; }

        public bool readOnly { get; set; }

        public string createdBy { get; set; }

        public DateTime createdDate { get; set; }

        public string modifiedBy { get; set; }

        public DateTime? modifiedDate { get; set; }

        public bool isEdit { get; set; }

        public string roleName { get; set; }
        [UIHint("TextArea")]
        [Display(Name = "USER_ADDRESS", ResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessage = "The Maximum length is {1}")]
        public string address { get; set; }

        [Display(Name = "USER_GRID_EMPLOYEE_NO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string employeeNo { get; set; }

        [Display(Name = "USER_DEPARTMENT", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string department { get; set; }

        [Display(Name = "USER_JOBTITLE", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1} characters")]
        public string jobTitle { get; set; }

        [Display(Name = "USER_MOBILENO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string mobileNo { get; set; }

        [Display(Name = "USER_OFFICENO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string officeNo { get; set; }

        [Display(Name = "USER_SALARYID", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string salaryId { get; set; }

        //[Display(Name = "USER_SALARYID", ResourceType = typeof(Resources))]
        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        //public string staffNo { get; set; }


        [UIHint("ComboBoxSearchable")]
        [Display(Name = "USER_GRED", ResourceType = typeof(Resources))]
        public int? gred { get; set; }

        [Display(Name = "USER_IDNO", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string idNo { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "USER_BIRTHDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> birthDate { get; set; }

        [Display(Name = "USER_AGE", ResourceType = typeof(Resources))]
        public Nullable<int> age { get; set; }

        [Display(Name = "USER_ACCOUNTNO", ResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessage = "The Maximum length is {1}")]
        public string accountNo { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "USER_BANK", ResourceType = typeof(Resources))]
        public Nullable<int> bankId { get; set; }

        [UIHint("Decimal")]
        [Display(Name = "USER_SALARY", ResourceType = typeof(Resources))]
        [Range(1, 1000000)]
        public Nullable<decimal> salaryAmount { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "USER_EMPLOYEEGROUP", ResourceType = typeof(Resources))]
        public Nullable<int> employeegroupId { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "USER_ENTRYDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> entryDate { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "USER_EXPIREDDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> expireddate { get; set; }

        [Display(Name = "USER_YEAROFSERVICE", ResourceType = typeof(Resources))]
        [Range(0, 50)]
        public Nullable<decimal> yearofservice { get; set; }

        public bool isBack { get; set; }

        public IEnumerable<SelectListItem> lstBank { get; set; }
        public IEnumerable<SelectListItem> lstGred { get; set; }
        public IEnumerable<SelectListItem> lstEmployeeGroup { get; set; }
        public IEnumerable<SelectListItem> lstBusinessItem { get; set; }
        public IEnumerable<SelectListItem> lstBusinessType { get; set; }

        [Display(Name = "USER_IMAGE", ResourceType = typeof(Resources))]
        public string image { get; set; }

        //[Display(Name = "STOCKMASTER_URLPATH", ResourceType = typeof(Resources))]
        public bool HasFile { get; set; }
        public bool ImgRemoved { get; set; }

        public string URLPathPreview { get; set; }

        [UIHint("FileBrowserRemovable")]
        [Display(Name = "USER_URLPATH", ResourceType = typeof(Resources))]
        public string URLPath { get; set; }

        [UIHint("FileBrowser")]
        [Display(Name = "USER_URLPATH", ResourceType = typeof(Resources))]
        public string file { get; set; }

    }

    public partial class UserViewModel
    {
        public List<UserModel> lstUserModel { get; set; }
    }

    public partial class ChangePasswordModel
    {
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "OLD_PASSWORD", ResourceType = typeof(Resources))]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string oldPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "NEW_PASSWORD", ResourceType = typeof(Resources))]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string newPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CONFIRM_PASSWORD", ResourceType = typeof(Resources))]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string confirmPassword { get; set; }
    }
}