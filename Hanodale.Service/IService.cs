using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace Hanodale.Services
{
    [ServiceContract]
    public interface IService
    {
        #region Authenticate user

        [OperationContract]
        AuthenticateUser DigestAuthentication(int uid);


        [OperationContract]
        AuthenticateUser AuthenticateUser(string userName, string password);

        [OperationContract]
        AuthenticateUser AuthenticateUserByUserId(string emilAddress, string password, int user_Id);

        [OperationContract]
        bool ChangePassword(Users userEn, string newPassword, string pageName);

        //[OperationContract]
        //User ForgotPassword(string emailId);

        //[OperationContract]
        //void UpdateUserPassword(User user, bool markAsVerified);

        //[OperationContract]
        //bool UpdateTerms(int userId, bool isAccepted);

        #endregion

        #region Common
        [OperationContract]
        List<Organizations> GetMainCostCenter(int userId);

        [OperationContract]
        List<Organizations> GetSubCostCenter(int id, int userId);

        [OperationContract]
        List<Organizations> GetSubCostCenterById(int id, int userId);

        [OperationContract]
        List<Organizations> GetAllSubCostCenterByFilterId(int categoryId, int userId, int subCostId);

        [OperationContract]
        List<ModuleItems> GetListModuleItem(int id);

        [OperationContract]
        List<ModuleTypes> GetListModuleTypes();

        [OperationContract]
        List<Menu> GetUserMenu(int currentUserId, int userId);

        [OperationContract]
        List<Users> GetListUser();

        [OperationContract]
        List<MainMenus> GetListMainMenu();

        [OperationContract]
        ModuleCodes GetModuleCodes(int submenu_Id, string orgPrefix, string existCode);

        [OperationContract]
        string GetGenerateCodeByOrgId(int organization_Id, int submenu_Id);

        [OperationContract]
        List<Organizations> GetSubCostByMainCostId(int mainCostCenter_Id);

        [OperationContract]
        List<Organizations> GetAllMainCostCenter();

        [OperationContract]
        List<Organizations> GetAllSubCostCenter();

        [OperationContract]
        List<ModuleTypes> GetListModuleTypeWorkOrderCodes();

        [OperationContract]
        List<AssignedOrganizations> GetListofAssignedOrganisation(int user_Id);

        [OperationContract]
        List<Organizations> GetListofSubCostCenter(int id);

        [OperationContract]
        List<ModuleItems> GetListAllModuleItem(int id);

        [OperationContract]
        string GenerateAutoCode(int organization_Id, int type_Id, MenuTypes menuType);

        [OperationContract]
        List<ModuleItems> GetListMaintenanceTypeModuleItem(int id);

        [OperationContract]
        List<string> GetReportList();

        [OperationContract]
        List<ModuleItems> GetStatusType();
        #endregion

        #region Dashboard

        [OperationContract]
        Dashboards GetDashoardByUser(int userId, int subCostCenter);

        #endregion

        #region User



        //[OperationContract]
        //User GetUserProfile(int userId);

        //[OperationContract]
        //User UpdateUserProfile(User userEn);

        [OperationContract]
        UserDetails GetUser(int currentUserId, int businessId, int startIndex, int pageSize, string search, int businessTypeId, int organization_Id, bool all, bool isActive);

        [OperationContract]
        Users SaveUser(int currentUserId, Users userEn);

        [OperationContract]
        int IsUserExists(Users userEn);

        [OperationContract]
        Users ResetPassword(Users userEn);

        [OperationContract]
        Users GetUserById(int currentUserId, int id);

        [OperationContract]
        Users GetUserByMCId(int currentUserId, int id, int mainCostId);

        [OperationContract]
        Users GetUserBySCId(int currentUserId, int id, int subCostId);

        [OperationContract]
        bool DeleteUser(int currentUserId, int id);


        [OperationContract]
        List<Users> GetListUserByStaff(int businessTypeId, bool check, int[] userIds = null);

        [OperationContract]
        List<Users> GetUserBySC(int currentUserId, int subCostId);

        [OperationContract]
        List<Users> GetListUserByBusinessId(int businessId, int businessType_Id, bool check, int[] userIds = null);

        [OperationContract]
        List<Users> GetListUserByBusinessTypeId(int businessTypeId, int organizationId, int user_Id, bool check, int[] userIds = null);

        [OperationContract]
        AssignedBusinesss GetUserBuinessById(int currentUserId);

        [OperationContract]
        List<Users> GetListUserByStaffMember(int businessTypeId, int businessId, int organizationId, int user_Id, bool check, int[] userIds = null);

        [OperationContract]
        Users SaveBusinessUser(int currentUserId, Users userEn);

        [OperationContract]
        Users GetUserDetailsByUserName(string userName);

        [OperationContract]
        List<Users> GetListUserBySupplier(int businessTypeId, int business_Id, bool check, int[] userIds = null);

        [OperationContract]
        List<Users> GetListUserBySection(int businessTypeId, int business_Id, int organizationId, int user_Id, bool check, int[] userIds = null);
        #endregion

        #region User Rights
        [OperationContract]
        UserRights GetUserAccess(int userId, string pageUrl);

        [OperationContract]
        List<UserRoles> GetUserRoles(int userId);

        [OperationContract]
        List<Menu> GetUserRightsByRole(int roleId);

        [OperationContract]
        bool SaveUserRights(List<UserRights> lstUserRights, string pageName);

        #endregion

        #region UserRole

        [OperationContract]
        RoleDetails GetUserRole(int currentUserId, int userId, int startIndex, int pageSize, string search);

        [OperationContract]
        UserRoles SaveUserRole(int currentUserId, UserRoles roleEn, string pageName);

        [OperationContract]
        bool DeleteUserRole(int currentUserId, int roleId, string pageName);

        [OperationContract]
        UserRoles GetRoleById(int id);

        [OperationContract]
        bool RoleExists(UserRoles role);

        #endregion

        #region Organization
        [OperationContract]
        OrganizationDetails GetOrganizationBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        [OperationContract]
        Organizations SaveOrganization(int currentUserId, Organizations entityEn, string pageName);

        [OperationContract]
        bool DeleteOrganization(int currentUserId, int id, string pageName);

        [OperationContract]
        Organizations GetOrganizationById(int id);

        [OperationContract]
        OrganizationCategories GetOrganizationCategoryById(int id);

        [OperationContract]
        bool OrganizationExists(Organizations entity);

        [OperationContract]
        List<OrganizationCategoryConfigs> GetOrganizationCategoryConfig();

        [OperationContract]
        List<Assets> GetCostCenter(int currentUserId, List<bool> lstAccess);

        [OperationContract]
        List<Organizations> GerOrganisation(int id);
        #endregion

        #region Reports

        /// <summary>
        /// This method is to get report by user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>report category list</returns>
        [OperationContract]
        List<Reports> GetReportByUser(int userId);

        #endregion

        #region Business

        [OperationContract]
        List<Businesses> GetAllBusiness(int currentUserId);

        [OperationContract]
        BusinessDetails GetBusiness(int currentUserId, bool all, int startIndex, int pageSize, string search, Businesses filterModel, int organization_Id);

        [OperationContract]
        Businesses SaveBusiness(int currentUserId, Businesses entity, string pageName);

        [OperationContract]
        bool DeleteBusiness(int currentUserId, int id, string pageName);

        [OperationContract]
        Businesses GetBusinessById(int id);

        [OperationContract]
        List<Businesses> GetListBusiness(int businessTypeId = 0);

        [OperationContract]
        bool IsBusinessExists(Businesses entity);

        [OperationContract]
        List<Businesses> GetBusinessbybusinessType();

        [OperationContract]
        Businesses GetBusinessDetailsByUserId(int user_Id);

        [OperationContract]
        List<int> GetListBusinessworkCategory(int business_Id);

        [OperationContract]
        List<Businesses> GetListBusinessBySubCostId(int organization_Id);

        [OperationContract]
        List<Businesses> GetListBusinessByOrganizationId(int organizationId, int businessType_Id, bool check);

        [OperationContract]
        List<int> GetBusinessOrganizationById(int business_Id);

        [OperationContract]
        BusinessDetails GetBusinessMaster(int currentUserId, int organization_Id, int startIndex, int pageSize, string search, object filterModel);

        [OperationContract]
        List<Businesses> GetBusinessBySubCostId(int organization_Id);

        [OperationContract]
        List<Businesses> GetListBusinessBybusinessType();

        [OperationContract]
        List<Businesses> GetBusinessSupplierBySubCostId(int organization_Id);

        [OperationContract]
        List<Businesses> GetListBusinessByBusinessTypeId(int businessType_Id);

        [OperationContract]
        bool IsBusinessWorkCategoryandOrganisationExists(int[] workCategoryIds, int[] organisationIds);

        [OperationContract]
        int IsBusinessSupplierExists(Businesses entity);

        [OperationContract]
        Users GetListBusinessUser(Users entity);

        #endregion

        #region BusinessAddress

        BusinessAddressDetails GetBusinessAddress(int currentUserId, int userId, int startIndex, int pageSize, string search);

        BusinessAddresses SaveBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName);

        bool DeleteBusinessAddress(int currentUserId, int id, string pageName);

        BusinessAddresses GetBusinessAddressById(int id);

        List<BusinessAddresses> GetListBusinessAddress();

        bool IsBusinessAddressExists(BusinessAddresses entity);

        #endregion

        #region BusinessClassification

        BusinessClassificationDetails GetBusinessClassification(int currentUserId, int userId, int startIndex, int pageSize, string search);

        BusinessClassifications SaveBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName);

        bool DeleteBusinessClassification(int currentUserId, int id, string pageName);

        BusinessClassifications GetBusinessClassificationById(int id);

        List<BusinessClassifications> GetListBusinessClassification();
        List<BusinessClassifications> GetListBusinessClassificationByBusinessId(int id);

        bool IsBusinessClassificationExists(BusinessClassifications entity);

        #endregion

        #region  BusinessFile

        BusinessFileDetails GetBusinessFile(int currentUserId, int userId, int businessId, int startIndex, int pageSize, string search);

        BusinessFiles SaveBusinessFile(int currentUserId, BusinessFiles entity, string pageName);

        bool DeleteBusinessFile(int currentUserId, int id, string pageName);

        BusinessFiles GetBusinessFileById(int id);

        List<BusinessFiles> GetListBusinessFile();

        bool IsBusinessFileExists(BusinessFiles entity);

        #endregion

        #region BusinessOthers
        // [OperationContract]
        //public BusinessOthersDetails GetBusinessOthers(int currentUserId, int userId, int startIndex, int pageSize, string search);

        [OperationContract]
        BusinessOtherss SaveBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName);

        // [OperationContract]
        // bool DeleteTask(int currentUserId, int id, string pageName);

        [OperationContract]
        BusinessOtherss GetBusinessOthersById(int id);

        [OperationContract]
        List<BusinessOtherss> GetListBusinessOthers();

        [OperationContract]
        bool IsTaskExists(BusinessOtherss entity);

        #endregion

        #region ModuleItem

        [OperationContract]
        ModuleItemDetails GetModuleItem(int currentUserId, bool all, int startIndex, int pageSize, string search);

        [OperationContract]
        ModuleItems SaveModuleItem(int currentUserId, ModuleItems moduleItemEn, string pageName);

        [OperationContract]
        bool DeleteModuleItem(int currentUserId, int moduleItemId, string pageName);

        [OperationContract]
        ModuleItems GetModuleItemById(int id);

        [OperationContract]
        bool IsModuleItemExists(ModuleItems moduleItem);

        #endregion

        #region UserProfile

        [OperationContract]
        UserProfileDetails GetUserProfile(int currentUserId, int userId, int startIndex, int pageSize, string search);

        [OperationContract]
        UserProfiles SaveUserProfile(int currentUserId, UserProfiles entity, string pageName);

        [OperationContract]
        bool DeleteUserProfile(int currentUserId, int id, string pageName);

        [OperationContract]
        UserProfiles GetUserProfileById(int id);

        [OperationContract]
        bool IsUserProfileExists(UserProfiles entity);

        //[OperationContract]
        //List<UserProfiles> GetUserProfileList();
        #endregion

        #region HelpDesk

        [OperationContract]
        HelpDeskDetails GetHelpDesk(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity);

        [OperationContract]
        HelpDesks SaveHelpDesk(int currentUserId, HelpDesks entity, string pageName);

        [OperationContract]
        bool DeleteHelpDesk(int currentUserId, int id, string pageName);

        [OperationContract]
        HelpDesks GetHelpDeskById(int id);

        [OperationContract]
        bool IsHelpDeskExists(HelpDesks entity);

        [OperationContract]
        List<HelpDesks> GetListHelpDesk();

        [OperationContract]
        List<Users> GetListUserByStatus();

        [OperationContract]
        List<HelpDesks> GetListHelpDeskByOrganizationAndApproved(int subCostCenter);

        [OperationContract]
        List<HelpDesks> UpdatedHelpDeskApproval(int currentUserId, List<HelpDesks> helpDeskEn, string pageName);
        #endregion

        #region CalendarEvent

        [OperationContract]
        CalendarEventDetails GetCalendarEvent(int currentUserId, int organization_Id, int startIndex, int pageSize, string search);

        [OperationContract]
        CalendarEvents SaveCalendarEvent(int currentUserId, CalendarEvents entity, string pageName);

        [OperationContract]
        bool DeleteCalendarEvent(int currentUserId, int id, string pageName);

        [OperationContract]
        CalendarEvents GetCalendarEventById(int id);

        [OperationContract]
        List<CalendarEvents> GetListCalendarEvent(int currentUserId, int organization_Id);

        [OperationContract]
        bool IsCalendarEventExists(CalendarEvents entity);

        #endregion

        #region CalendarSetting

        [OperationContract]
        CalendarSettingDetails GetCalendarSetting(int currentUserId, int organization_Id, int startIndex, int pageSize, string search);

        [OperationContract]
        List<CalendarSettings> GetCalendarItem(int currentUserId, int organization_Id, int year);

        [OperationContract]
        bool SaveCalendarSetting(int currentUserId, List<CalendarSettings> lst, int organizationId, int year);

        [OperationContract]
        bool DeleteCalendarSetting(int organization_Id, int year);

        [OperationContract]
        CalendarSettings GetCalendarSettingById(int id);

        [OperationContract]
        bool IsCalendarSettingExists(CalendarSettings entity);

        [OperationContract]
        List<int> GetCalendarYears(int organization_Id);

        [OperationContract]
        bool CopyCalendar(CopyCalendars entity);

        #endregion

        #region  TrainingStaff

        [OperationContract]
        TrainingStaffDetails GetTrainingStaff(int currentUserId, int userId, int startIndex, int pageSize, string search);

        [OperationContract]
        TrainingStaffDetails GetFileTrainingStaff(int currentUserId, int userId, int startIndex, int pageSize, string search, int FileId);

        [OperationContract]
        TrainingStaffs SaveTrainingFile(int currentUserId, List<TrainingStaffs> entity, FileUploadHistorys objHistory, string pageName);

        [OperationContract]
        TrainingStaffs SaveTrainingStaff(int currentUserId, TrainingStaffs entity, string pageName);

        [OperationContract]
        bool DeleteTrainingStaff(int currentUserId, int id, string pageName);

        [OperationContract]
        TrainingStaffs GetTrainingStaffById(int id);

        [OperationContract]
        List<TrainingStaffs> GetListTrainingStaff();

        [OperationContract]
        bool IsTrainingStaffExists(TrainingStaffs entity);

        #endregion

        #region File History

        [OperationContract]
        FileUploadHistoryDetails GetFileHistory(int currentUserId, int userId, int startIndex, int pageSize, string search, bool Istraining);

        [OperationContract]
        FileUploadHistorys SaveFileHistory(int currentUserId, FileUploadHistorys entity, string pageName);

        [OperationContract]
        bool DeleteFileHistory(int currentUserId, int id, string pageName);

        [OperationContract]
        FileUploadHistorys GetFileHistoryById(int id);

        [OperationContract]
        List<TrainingStaffs> GetListFileHistory();

        [OperationContract]
        bool IsFileHistoryExists(FileUploadHistorys entity);

        #endregion

        #region AdhocReport

        [OperationContract]
        AdhocReportDetails GetAdhocReport(int currentUserId, int AdhocReport_Id, int startIndex, int pageSize, string search);

        [OperationContract]
        AdhocReports SaveAdhocReport(int currentUserId, AdhocReports entity, string pageName);

        [OperationContract]
        bool DeleteAdhocReport(int currentUserId, int id, string pageName);

        [OperationContract]
        AdhocReports GetAdhocReportById(int id);

        [OperationContract]
        List<AdhocReports> GetListAdhocReport();

        [OperationContract]
        int IsAdhocReportExists(AdhocReports entity);

        #endregion

        #region OrganizationEmail

        OrganizationEmailDetails GetOrganizationEmail(int currentUserId, int userId, int startIndex, int pageSize, string search);

        OrganizationEmails SaveOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName);

        bool DeleteOrganizationEmail(int currentUserId, int id, string pageName);

        OrganizationEmails GetOrganizationEmailById(int id);

        List<OrganizationEmails> GetListOrganizationEmail();

        bool IsOrganizationEmailExists(OrganizationEmails entity);

        #endregion

        #region News

        [OperationContract]
        NewsDetails GetNews(int currentUserId, bool all, int id, int startIndex, int pageSize, string search, object filterModel);

        [OperationContract]
        Newss SaveNews(int currentUserId, Newss entity, string pageName);

        [OperationContract]
        bool DeleteNews(int currentUserId, int id, string pageName);

        [OperationContract]
        Newss GetNewsById(int id);

        [OperationContract]
        List<Newss> GetListNews();

        [OperationContract]
        bool IsNewsExists(Newss entity);

        #endregion

        #region HelpDesk

        [OperationContract]
        List<HelpDesks> GetNewTickets(int userID, int organizationId);

        #endregion
    }
}
