using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        #region Const

        /// <summary>
        /// Section name for unity configuration
        /// </summary>
        const string UNITY_SECTION_NAME = "unity";

        /// <summary>
        /// Container name for Unity configuration
        /// </summary>
        const string UNITY_CONTAINER_NAME = "ConfigurationServiceContainer";

        #endregion

        #region Container Property

        private IUnityContainer container;
        /// <summary>
        /// Unity container
        /// </summary>
        public IUnityContainer Container
        {
            get
            {
                if (this.container == null)
                {
                    this.container = new UnityContainer();
                }
                return this.container;
            }
        }

        #endregion

        #region Constructor

        public Service()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(Service.UNITY_SECTION_NAME);
            section.Containers[Service.UNITY_CONTAINER_NAME].Configure(this.Container);

        }

        private void SetConfiguration(string entityName)
        {

        }
        #endregion

        #region Authenticate user
        /// <summary>
        /// This method is to authenticate user
        /// </summary>
        /// <param name="userName">User name -  string</param>
        /// <param name="password">Password -  string</param>
        /// <returns>user details</returns>
        public AuthenticateUser AuthenticateUser(string userName, string password)
        {
            var provider = this.Container.Resolve<IAuthenticationService>();
            return provider.AuthenticateUser(userName, password);
        }

        /// <summary>
        /// This method is to authenticate user By UserID
        /// </summary>
        /// <param name="userName">Email -  string</param>
        /// <param name="password">Password -  string</param>
        /// <returns>user details</returns>
        public AuthenticateUser AuthenticateUserByUserId(string emilAddress, string password, int user_Id)
        {
            var provider = this.Container.Resolve<IAuthenticationService>();
            return provider.AuthenticateUserByUserId(emilAddress, password, user_Id);
        }

        public bool ChangePassword(Users userEn, string newPassword, string pageName)
        {
            var provider = this.Container.Resolve<IAuthenticationService>();
            return provider.ChangePassword(userEn, newPassword, pageName);
        }

        //public User ForgotPassword(string emailId) {
        //    var provider = this.Container.Resolve<var>();
        //    return provider.ForgotPassword(emailId);
        //}


        //public void UpdateUserPassword(User user, bool markAsVerified) {
        //    var provider = this.Container.Resolve<var>();
        //    provider.UpdateUserPassword(user, markAsVerified);
        //}

        //public bool UpdateTerms(int userId, bool isAccepted)
        //{
        //    var provider = this.Container.Resolve<var>();
        //    return provider.UpdateTerms(userId, isAccepted);
        //}

        #endregion

        #region Common

        public List<Organizations> GetMainCostCenter(int userId)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetMainCostCenter(userId);
        }

        public List<Organizations> GetSubCostCenter(int id, int userId)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetSubCostCenter(id, userId);
        }

        public List<Organizations> GetSubCostCenterById(int id, int userId)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetSubCostCenterById(id, userId);
        }

        public List<Organizations> GetAllSubCostCenterByFilterId(int categoryId, int userId, int subCostId)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetAllSubCostCenterByFilterId(categoryId, userId, subCostId);
        }

        public List<ModuleItems> GetListModuleItem(int id)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListModuleItem(id);
        }

        public List<ModuleTypes> GetListModuleTypes()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListModuleTypes();
        }

        public List<Menu> GetUserMenu(int currentUserId, int userId)
        {
            //  var provider = this.Container.Resolve<var>();
            IMenuService provider = this.Container.Resolve<IMenuService>();
            return provider.GetUserMenu(currentUserId, userId);
        }

        public List<Users> GetListUser()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListUser();
        }

        public List<MainMenus> GetListMainMenu()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListMainMenu();
        }

        public ModuleCodes GetModuleCodes(int submenu_Id, string orgPrefix, string existCode)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetModuleCodes(submenu_Id, orgPrefix, existCode);
        }

        public string GetGenerateCodeByOrgId(int organization_Id, int submenu_Id)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetGenerateCodeByOrgId(organization_Id, submenu_Id);
        }

        public List<Organizations> GetSubCostByMainCostId(int mainCostCenter_Id)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetSubCostByMainCostId(mainCostCenter_Id);
        }

        public List<Organizations> GetAllMainCostCenter()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetAllMainCostCenter();
        }

        public List<Organizations> GetAllSubCostCenter()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetAllSubCostCenter();
        }

        public List<ModuleTypes> GetListModuleTypeWorkOrderCodes()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListModuleTypeWorkOrderCodes();
        }

        public List<AssignedOrganizations> GetListofAssignedOrganisation(int user_Id)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListofAssignedOrganisation(user_Id);
        }

        public List<Organizations> GetListofSubCostCenter(int id)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListofSubCostCenter(id);
        }

        public List<ModuleItems> GetListAllModuleItem(int id)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListAllModuleItem(id);
        }

        public string GenerateAutoCode(int organization_Id, int type_Id, MenuTypes menuType)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GenerateAutoCode(organization_Id, type_Id, menuType);
        }

        public List<ModuleItems> GetListMaintenanceTypeModuleItem(int id)
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetListMaintenanceTypeModuleItem(id);
        }

        public List<string> GetReportList()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetReportList();
        }

        public List<ModuleItems> GetStatusType()
        {
            var provider = this.Container.Resolve<ICommonService>();
            return provider.GetStatusType();
        }
        #endregion

        #region Dashoard

        public Dashboards GetDashoardByUser(int userId,int subCostCenter)
        {
            var provider = this.Container.Resolve<IDashboardService>();
            return provider.GetDashoardByUser(userId,subCostCenter);
        }

        #endregion

        #region User

        public UserDetails GetUser(int currentUserId, int businessId, int startIndex, int pageSize, string search, int businessTypeId, int organization_Id, bool all, bool isActive)
        {
            //var provider = this.Container.Resolve<var>();
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetUser(currentUserId, businessId, startIndex, pageSize, search, businessTypeId, organization_Id, all, isActive);
        }

        //public User GetUserProfile(int userId)
        //{
        //    var provider = this.Container.Resolve<var>();
        //    return provider.GetUserProfile(userId);
        //}

        //public User UpdateUserProfile(User userEn)
        //{
        //    var provider = this.Container.Resolve<var>();
        //    return provider.UpdateUserProfile(userEn);
        //}

        public Users SaveUser(int currentUserId, Users userEn)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.SaveUser(currentUserId, userEn);
        }

        public int IsUserExists(Users userEn)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.IsUserExists(userEn);
        }

        public Users ResetPassword(Users userEn)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.ResetPassword(userEn);
        }

        public Users GetUserById(int currentUserId, int id)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetUserById(currentUserId, id);
        }

        public Users GetUserByMCId(int currentUserId, int id, int mainCostId)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetUserByMCId(currentUserId, id, mainCostId);
        }

        public Users GetUserBySCId(int currentUserId, int id, int subCostId)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetUserBySCId(currentUserId, id, subCostId);
        }

        public bool DeleteUser(int currentUserId, int id)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.DeleteUser(currentUserId, id);
        }


        public List<Users> GetListUserByStaff(int businessTypeId, bool check, int[] userIds)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetListUserByStaff(businessTypeId, check, userIds);
        }

        public List<Users> GetUserBySC(int currentUserId, int subCostId)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetUserBySC(currentUserId, subCostId);
        }

        public List<Users> GetListUserByBusinessId(int businessId, int businessType_Id, bool check, int[] userIds)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetListUserByBusinessId(businessId, businessType_Id, check, userIds);
        }

        public List<Users> GetListUserByBusinessTypeId(int businessTypeId, int organizationId, int user_Id, bool check, int[] userIds)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetListUserByBusinessTypeId(businessTypeId, organizationId, user_Id, check, userIds);
        }

        public AssignedBusinesss GetUserBuinessById(int currentUserId)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetUserBuinessById(currentUserId);
        }

        public List<Users> GetListUserByStaffMember(int businessTypeId, int businessId, int organizationId, int user_Id, bool check, int[] userIds)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetListUserByStaffMember(businessTypeId, businessId, organizationId, user_Id, check, userIds);
        }

        public Users SaveBusinessUser(int currentUserId, Users userEn)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.SaveBusinessUser(currentUserId, userEn);
        }

        public Users GetUserDetailsByUserName(string userName)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetUserDetailsByUserName(userName);
        }

        public List<Users> GetListUserBySupplier(int businessTypeId, int business_Id, bool check, int[] userIds)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetListUserBySupplier(businessTypeId, business_Id, check, userIds);
        }

        public List<Users> GetListUserBySection(int businessTypeId, int business_Id, int organizationId, int user_Id, bool check, int[] userIds)
        {
            IUserService provider = this.Container.Resolve<IUserService>();
            return provider.GetListUserBySection(businessTypeId, business_Id, organizationId, user_Id, check, userIds);
        }
        #endregion

        #region User Rights

        public UserRights GetUserAccess(int userId, string pageUrl)
        {
            IUserRightsService provider = this.Container.Resolve<IUserRightsService>();
            return provider.GetUserAccess(userId, pageUrl);
        }

        public List<UserRoles> GetUserRoles(int userId)
        {
            IUserRightsService provider = this.Container.Resolve<IUserRightsService>();
            return provider.GetUserRoles(userId);
        }

        public List<Menu> GetUserRightsByRole(int roleId)
        {
            IUserRightsService provider = this.Container.Resolve<IUserRightsService>();
            return provider.GetUserRightsByRole(roleId);
        }


        public bool SaveUserRights(List<UserRights> lstUserRights, string pageName)
        {
            IUserRightsService provider = this.Container.Resolve<IUserRightsService>();
            return provider.SaveUserRights(lstUserRights, pageName);
        }
        #endregion

        #region UserRole

        public RoleDetails GetUserRole(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            IUserRoleService provider = this.Container.Resolve<IUserRoleService>();
            return provider.GetUserRole(currentUserId, userId, startIndex, pageSize, search);
        }

        public UserRoles SaveUserRole(int currentUserId, UserRoles roleEn, string pageName)
        {
            IUserRoleService provider = this.Container.Resolve<IUserRoleService>();
            return provider.SaveUserRole(currentUserId, roleEn, pageName);
        }

        public bool DeleteUserRole(int currentUserId, int roleId, string pageName)
        {
            IUserRoleService provider = this.Container.Resolve<IUserRoleService>();
            return provider.DeleteUserRole(currentUserId, roleId, pageName);
        }

        public UserRoles GetRoleById(int id)
        {
            IUserRoleService provider = this.Container.Resolve<IUserRoleService>();
            return provider.GetRoleById(id);
        }

        public bool RoleExists(UserRoles role)
        {
            IUserRoleService provider = this.Container.Resolve<IUserRoleService>();
            return provider.RoleExists(role);
        }

        #endregion

        #region Organization

        public OrganizationDetails GetOrganizationBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.GetOrganizationBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public Organizations SaveOrganization(int currentUserId, Organizations entity, string pageName)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.SaveOrganization(currentUserId, entity, pageName);
        }

        public bool DeleteOrganization(int currentUserId, int id, string pageName)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.DeleteOrganization(currentUserId, id, pageName);
        }

        public Organizations GetOrganizationById(int id)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.GetOrganizationById(id);
        }

        public OrganizationCategories GetOrganizationCategoryById(int id)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.GetOrganizationCategoryById(id);
        }

        public bool OrganizationExists(Organizations entity)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.OrganizationExists(entity);
        }

        public List<OrganizationCategoryConfigs> GetOrganizationCategoryConfig()
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.GetOrganizationCategoryConfig();
        }

        public List<Assets> GetCostCenter(int currentUserId, List<bool> lstAccess)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.GetCostCenter(currentUserId, lstAccess);
        }

        public List<Organizations> GerOrganisation(int id)
        {
            IOrganizationService provider = this.Container.Resolve<IOrganizationService>();
            return provider.GerOrganisation(id);
        }
        #endregion

        #region Reports

        /// <summary>
        /// This method is to get report by user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>report category list</returns>
        public List<Reports> GetReportByUser(int userId)
        {
            var provider = this.Container.Resolve<IReportService>();
            return provider.GetReportByUser(userId);
        }

        #endregion

        #region Business

        public List<Businesses> GetAllBusiness(int currentUserId)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetAllBusiness(currentUserId);
        }

        public BusinessDetails GetBusiness(int currentUserId, bool all, int startIndex, int pageSize, string search, Businesses filterModel, int organization_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusiness(currentUserId, all, startIndex, pageSize, search, filterModel, organization_Id);
        }

        public Businesses SaveBusiness(int currentUserId, Businesses entity, string pageName)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.SaveBusiness(currentUserId, entity, pageName);
        }

        public bool DeleteBusiness(int currentUserId, int id, string pageName)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.DeleteBusiness(currentUserId, id, pageName);
        }

        public Businesses GetBusinessById(int id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusinessById(id);
        }

        public List<Businesses> GetListBusiness(int businessTypeId)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetListBusiness(businessTypeId);
        }

        public bool IsBusinessExists(Businesses entity)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.IsBusinessExists(entity);
        }

        public List<Businesses> GetBusinessbybusinessType()
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusinessbybusinessType();
        }

        public Businesses GetBusinessDetailsByUserId(int user_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusinessDetailsByUserId(user_Id);
        }

        public List<int> GetListBusinessworkCategory(int business_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetListBusinessworkCategory(business_Id);
        }

        public List<Businesses> GetListBusinessBySubCostId(int organization_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetListBusinessBySubCostId(organization_Id);

        }

        public List<Businesses> GetListBusinessByOrganizationId(int organizationId, int businessType_Id, bool check)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetListBusinessByOrganizationId(organizationId, businessType_Id, check);
        }

        public List<int> GetBusinessOrganizationById(int business_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusinessOrganizationById(business_Id);
        }

        public BusinessDetails GetBusinessMaster(int currentUserId, int organization_Id, int startIndex, int pageSize, string search, object filterModel)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusinessMaster(currentUserId, organization_Id, startIndex, pageSize, search, filterModel);
        }

        public List<Businesses> GetBusinessBySubCostId(int organization_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusinessBySubCostId(organization_Id);
        }

        public List<Businesses> GetListBusinessBybusinessType()
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetListBusinessBybusinessType();
        }

        public List<Businesses> GetBusinessSupplierBySubCostId(int organization_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetBusinessSupplierBySubCostId(organization_Id);
        }

        public List<Businesses> GetListBusinessByBusinessTypeId(int businessType_Id)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetListBusinessByBusinessTypeId(businessType_Id);
        }

        public bool IsBusinessWorkCategoryandOrganisationExists(int[] workCategoryIds, int[] organisationIds)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.IsBusinessWorkCategoryandOrganisationExists(workCategoryIds, organisationIds);
        }

        public int IsBusinessSupplierExists(Businesses entity)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.IsBusinessSupplierExists(entity);
        }

        public Users GetListBusinessUser(Users entity)
        {
            IBusinessService provider = this.Container.Resolve<IBusinessService>();
            return provider.GetListBusinessUser(entity);
        }

        #endregion

        #region BusinessAddress

        public BusinessAddressDetails GetBusinessAddress(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            IBusinessAddressService provider = this.Container.Resolve<IBusinessAddressService>();
            return provider.GetBusinessAddress(currentUserId, userId, startIndex, pageSize, search);
        }

        public BusinessAddresses SaveBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName)
        {
            IBusinessAddressService provider = this.Container.Resolve<IBusinessAddressService>();
            return provider.SaveBusinessAddress(currentUserId, entity, pageName);
        }

        public bool DeleteBusinessAddress(int currentUserId, int id, string pageName)
        {
            IBusinessAddressService provider = this.Container.Resolve<IBusinessAddressService>();
            return provider.DeleteBusinessAddress(currentUserId, id, pageName);
        }

        public BusinessAddresses GetBusinessAddressById(int id)
        {
            IBusinessAddressService provider = this.Container.Resolve<IBusinessAddressService>();
            return provider.GetBusinessAddressById(id);
        }

        public List<BusinessAddresses> GetListBusinessAddress()
        {
            IBusinessAddressService provider = this.Container.Resolve<IBusinessAddressService>();
            return provider.GetListBusinessAddress();
        }

        public bool IsBusinessAddressExists(BusinessAddresses entity)
        {
            IBusinessAddressService provider = this.Container.Resolve<IBusinessAddressService>();
            return provider.IsBusinessAddressExists(entity);
        }

        #endregion

        #region BusinessClassification

        public BusinessClassificationDetails GetBusinessClassification(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            IBusinessClassificationService provider = this.Container.Resolve<IBusinessClassificationService>();
            return provider.GetBusinessClassification(currentUserId, userId, startIndex, pageSize, search);
        }

        public BusinessClassifications SaveBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName)
        {
            IBusinessClassificationService provider = this.Container.Resolve<IBusinessClassificationService>();
            return provider.SaveBusinessClassification(currentUserId, entity, pageName);
        }

        public bool DeleteBusinessClassification(int currentUserId, int id, string pageName)
        {
            IBusinessClassificationService provider = this.Container.Resolve<IBusinessClassificationService>();
            return provider.DeleteBusinessClassification(currentUserId, id, pageName);
        }

        public BusinessClassifications GetBusinessClassificationById(int id)
        {
            IBusinessClassificationService provider = this.Container.Resolve<IBusinessClassificationService>();
            return provider.GetBusinessClassificationById(id);
        }

        public List<BusinessClassifications> GetListBusinessClassification()
        {
            IBusinessClassificationService provider = this.Container.Resolve<IBusinessClassificationService>();
            return provider.GetListBusinessClassification();
        }

        public List<BusinessClassifications> GetListBusinessClassificationByBusinessId(int id)
        {
            IBusinessClassificationService provider = this.Container.Resolve<IBusinessClassificationService>();
            return provider.GetListBusinessClassificationByBusinessId(id);
        }

        public bool IsBusinessClassificationExists(BusinessClassifications entity)
        {
            IBusinessClassificationService provider = this.Container.Resolve<IBusinessClassificationService>();
            return provider.IsBusinessClassificationExists(entity);
        }

        #endregion

        #region  BusinessFile

        public BusinessFileDetails GetBusinessFile(int currentUserId, int userId, int businessId, int startIndex, int pageSize, string search)
        {
            IBusinessFileService provider = this.Container.Resolve<IBusinessFileService>();
            return provider.GetBusinessFile(currentUserId, userId, businessId, startIndex, pageSize, search);
        }

        public BusinessFiles SaveBusinessFile(int currentUserId, BusinessFiles entity, string pageName)
        {
            IBusinessFileService provider = this.Container.Resolve<IBusinessFileService>();
            return provider.SaveBusinessFile(currentUserId, entity, pageName);
        }

        public bool DeleteBusinessFile(int currentUserId, int id, string pageName)
        {
            IBusinessFileService provider = this.Container.Resolve<IBusinessFileService>();
            return provider.DeleteBusinessFile(currentUserId, id, pageName);
        }

        public BusinessFiles GetBusinessFileById(int id)
        {
            IBusinessFileService provider = this.Container.Resolve<IBusinessFileService>();
            return provider.GetBusinessFileById(id);
        }

        public List<BusinessFiles> GetListBusinessFile()
        {
            IBusinessFileService provider = this.Container.Resolve<IBusinessFileService>();
            return provider.GetListBusinessFile();
        }

        public bool IsBusinessFileExists(BusinessFiles entity)
        {
            IBusinessFileService provider = this.Container.Resolve<IBusinessFileService>();
            return provider.IsBusinessFileExists(entity);
        }

        #endregion

        #region BusinessOthers

        //public BusinessOthersDetails GetBusinessOthers(int currentUserId, int userId, int startIndex, int pageSize, string search)
        //{
        //    IBusinessOthersService provider = this.Container.Resolve<IBusinessOthersService>();
        //    return provider.GetBusinessOthers(currentUserId, userId, startIndex, pageSize, search);
        //}

        public BusinessOtherss SaveBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName)
        {
            IBusinessOthersService provider = this.Container.Resolve<IBusinessOthersService>();
            return provider.SaveBusinessOthers(currentUserId, entity, pageName);
        }

        //public bool DeleteTask(int currentUserId, int id, string pageName)
        //{
        //    IBusinessOthersService provider = this.Container.Resolve<IBusinessOthersService>();
        //    return provider.DeleteTask(currentUserId, id, pageName);
        //}

        public BusinessOtherss GetBusinessOthersById(int id)
        {
            IBusinessOthersService provider = this.Container.Resolve<IBusinessOthersService>();
            return provider.GetBusinessOthersById(id);
        }

        public List<BusinessOtherss> GetListBusinessOthers()
        {
            IBusinessOthersService provider = this.Container.Resolve<IBusinessOthersService>();
            return provider.GetListBusinessOthers();
        }

        public bool IsTaskExists(BusinessOtherss entity)
        {
            IBusinessOthersService provider = this.Container.Resolve<IBusinessOthersService>();
            return provider.IsBusinessOthersExists(entity);
        }

        #endregion

        #region ModuleItem

        public ModuleItemDetails GetModuleItem(int currentUserId, bool all, int startIndex, int pageSize, string search)
        {
            IModuleItemService provider = this.Container.Resolve<IModuleItemService>();
            return provider.GetModuleItem(currentUserId, all, startIndex, pageSize, search);
        }

        public ModuleItems SaveModuleItem(int currentUserId, ModuleItems entity, string pageName)
        {
            IModuleItemService provider = this.Container.Resolve<IModuleItemService>();
            return provider.SaveModuleItem(currentUserId, entity, pageName);
        }

        public bool DeleteModuleItem(int currentUserId, int moduleItemId, string pageName)
        {
            IModuleItemService provider = this.Container.Resolve<IModuleItemService>();
            return provider.DeleteModuleItem(currentUserId, moduleItemId, pageName);
        }

        public ModuleItems GetModuleItemById(int id)
        {
            IModuleItemService provider = this.Container.Resolve<IModuleItemService>();
            return provider.GetModuleItemById(id);
        }

        public bool IsModuleItemExists(ModuleItems entity)
        {
            IModuleItemService provider = this.Container.Resolve<IModuleItemService>();
            return provider.IsModuleItemExists(entity);
        }
        #endregion

        #region UserProfile

        public UserProfileDetails GetUserProfile(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            IUserProfileService provider = this.Container.Resolve<IUserProfileService>();
            return provider.GetUserProfile(currentUserId, userId, startIndex, pageSize, search);
        }

        public UserProfiles SaveUserProfile(int currentUserId, UserProfiles entity, string pageName)
        {
            IUserProfileService provider = this.Container.Resolve<IUserProfileService>();
            return provider.SaveUserProfile(currentUserId, entity, pageName);
        }

        public bool DeleteUserProfile(int currentUserId, int id, string pageName)
        {
            IUserProfileService provider = this.Container.Resolve<IUserProfileService>();
            return provider.DeleteUserProfile(currentUserId, id, pageName);
        }

        public UserProfiles GetUserProfileById(int id)
        {
            IUserProfileService provider = this.Container.Resolve<IUserProfileService>();
            return provider.GetUserProfileById(id);
        }

        public bool IsUserProfileExists(UserProfiles entity)
        {
            IUserProfileService provider = this.Container.Resolve<IUserProfileService>();
            return provider.IsUserProfileExists(entity);
        }

        //public List<UserProfiles> GetUserProfileList()
        //{
        //    IUserProfileService provider = this.Container.Resolve<IUserProfileService>();
        //    return provider.GetUserProfileList();
        //}

        #endregion

        #region HelpDesk

        public HelpDeskDetails GetHelpDesk(int currentUserId, int organizationId, int startIndex, int pageSize, string search, object filterEntity)
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.GetHelpDesk(currentUserId, organizationId, startIndex, pageSize, search, filterEntity);
        }

        public HelpDesks SaveHelpDesk(int currentUserId, HelpDesks entity, string pageName)
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.SaveHelpDesk(currentUserId, entity, pageName);
        }

        public bool DeleteHelpDesk(int currentUserId, int id, string pageName)
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.DeleteHelpDesk(currentUserId, id, pageName);
        }

        public HelpDesks GetHelpDeskById(int id)
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.GetHelpDeskById(id);
        }

        public bool IsHelpDeskExists(HelpDesks entity)
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.IsHelpDeskExists(entity);
        }

        public List<HelpDesks> GetListHelpDesk()
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.GetListHelpDesk();
        }

        public List<Users> GetListUserByStatus()
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.GetListUserByStatus();
        }

        public List<HelpDesks> GetListHelpDeskByOrganizationAndApproved(int subCostCenter)
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.GetListHelpDeskByOrganizationAndApproved(subCostCenter);
        }

        public List<HelpDesks> UpdatedHelpDeskApproval(int currentUserId, List<HelpDesks> helpDeskEn, string pageName)
        {
            IHelpDeskService provider = this.Container.Resolve<IHelpDeskService>();
            return provider.UpdatedHelpDeskApproval(currentUserId, helpDeskEn, pageName);
        }
        #endregion
       
        #region CalendarEvent

        public CalendarEventDetails GetCalendarEvent(int currentUserId, int organization_Id, int startIndex, int pageSize, string search)
        {
            ICalendarEventService provider = this.Container.Resolve<ICalendarEventService>();
            return provider.GetCalendarEvent(currentUserId, organization_Id, startIndex, pageSize, search);
        }

        public CalendarEvents SaveCalendarEvent(int currentUserId, CalendarEvents entity, string pageName)
        {
            ICalendarEventService provider = this.Container.Resolve<ICalendarEventService>();
            return provider.SaveCalendarEvent(currentUserId, entity, pageName);
        }

        public bool DeleteCalendarEvent(int currentUserId, int id, string pageName)
        {
            ICalendarEventService provider = this.Container.Resolve<ICalendarEventService>();
            return provider.DeleteCalendarEvent(currentUserId, id, pageName);
        }

        public CalendarEvents GetCalendarEventById(int id)
        {
            ICalendarEventService provider = this.Container.Resolve<ICalendarEventService>();
            return provider.GetCalendarEventById(id);
        }

        public List<CalendarEvents> GetListCalendarEvent(int currentUserId, int organization_Id)
        {
            ICalendarEventService provider = this.Container.Resolve<ICalendarEventService>();
            return provider.GetListCalendarEvent(currentUserId, organization_Id);
        }

        public bool IsCalendarEventExists(CalendarEvents entity)
        {
            ICalendarEventService provider = this.Container.Resolve<ICalendarEventService>();
            return provider.IsCalendarEventExists(entity);
        }

        #endregion

        #region CalendarSetting

        public CalendarSettingDetails GetCalendarSetting(int currentUserId, int organization_Id, int startIndex, int pageSize, string search)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.GetCalendarSetting(currentUserId, organization_Id, startIndex, pageSize, search);
        }

        public List<CalendarSettings> GetCalendarItem(int currentUserId, int organization_Id, int year)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.GetCalendarItem(currentUserId, organization_Id, year);
        }

        public bool SaveCalendarSetting(int currentUserId, List<CalendarSettings> lst, int organizationId, int year)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.SaveCalendarSetting(currentUserId, lst, organizationId, year);
        }


        public bool DeleteCalendarSetting(int organization_Id, int year)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.DeleteCalendarSetting(organization_Id, year);
        }

        public CalendarSettings GetCalendarSettingById(int id)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.GetCalendarSettingById(id);
        }

        public bool IsCalendarSettingExists(CalendarSettings entity)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.IsCalendarSettingExists(entity);
        }


        public List<int> GetCalendarYears(int organization_Id)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.GetCalendarYears(organization_Id);
        }

        public bool CopyCalendar(CopyCalendars entity)
        {
            ICalendarSettingService provider = this.Container.Resolve<ICalendarSettingService>();
            return provider.CopyCalendar(entity);
        }

        #endregion

        #region  TrainingStaff

        public TrainingStaffDetails GetTrainingStaff(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.GetTrainingStaff(currentUserId, userId, startIndex, pageSize, search);
        }

        public TrainingStaffDetails GetFileTrainingStaff(int currentUserId, int userId, int startIndex, int pageSize, string search, int FileId)
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.GetFileTrainingStaff(currentUserId, userId, startIndex, pageSize, search, FileId);
        }
        public TrainingStaffs SaveTrainingFile(int currentUserId, List<TrainingStaffs> entity, FileUploadHistorys objHistory, string pageName)
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.SaveTrainingStaff(currentUserId, entity, objHistory, pageName);
        }
        public TrainingStaffs SaveTrainingStaff(int currentUserId, TrainingStaffs entity, string pageName)
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.SaveTrainingStaff(currentUserId, entity, pageName);
        }

        public bool DeleteTrainingStaff(int currentUserId, int id, string pageName)
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.DeleteTrainingStaff(currentUserId, id, pageName);
        }

        public TrainingStaffs GetTrainingStaffById(int id)
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.GetTrainingStaffById(id);
        }

        public List<TrainingStaffs> GetListTrainingStaff()
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.GetListTrainingStaff();
        }

        public bool IsTrainingStaffExists(TrainingStaffs entity)
        {
            ITrainingStaffService provider = this.Container.Resolve<ITrainingStaffService>();
            return provider.IsTrainingStaffExists(entity);
        }

        #endregion

        #region File History

        public FileUploadHistoryDetails GetFileHistory(int currentUserId, int userId, int startIndex, int pageSize, string search, bool Istraining)
        {
            IFileHistoryService provider = this.Container.Resolve<IFileHistoryService>();
            return provider.GetFileHistory(currentUserId, userId, startIndex, pageSize, search, Istraining);
        }

        public FileUploadHistorys SaveFileHistory(int currentUserId, FileUploadHistorys entity, string pageName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFileHistory(int currentUserId, int id, string pageName)
        {
            IFileHistoryService provider = this.Container.Resolve<IFileHistoryService>();
            return provider.DeleteFileHistory(currentUserId, id, pageName);
        }

        public FileUploadHistorys GetFileHistoryById(int id)
        {
            IFileHistoryService provider = this.Container.Resolve<IFileHistoryService>();
            return provider.GetFileHistoryById(id);
        }

        public List<TrainingStaffs> GetListFileHistory()
        {
            throw new NotImplementedException();
        }

        public bool IsFileHistoryExists(FileUploadHistorys entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region AdhocReport

        public AdhocReportDetails GetAdhocReport(int currentUserId, int AdhocReport_Id, int startIndex, int pageSize, string search)
        {
            IAdhocReportService provider = this.Container.Resolve<IAdhocReportService>();
            return provider.GetAdhocReport(currentUserId, AdhocReport_Id, startIndex, pageSize, search);
        }

        public AdhocReports SaveAdhocReport(int currentUserId, AdhocReports entity, string pageName)
        {
            IAdhocReportService provider = this.Container.Resolve<IAdhocReportService>();
            return provider.SaveAdhocReport(currentUserId, entity, pageName);
        }

        public bool DeleteAdhocReport(int currentUserId, int id, string pageName)
        {
            IAdhocReportService provider = this.Container.Resolve<IAdhocReportService>();
            return provider.DeleteAdhocReport(currentUserId, id, pageName);
        }

        public AdhocReports GetAdhocReportById(int id)
        {
            IAdhocReportService provider = this.Container.Resolve<IAdhocReportService>();
            return provider.GetAdhocReportById(id);
        }

        public List<AdhocReports> GetListAdhocReport()
        {
            IAdhocReportService provider = this.Container.Resolve<IAdhocReportService>();
            return provider.GetListAdhocReport();
        }

        public int IsAdhocReportExists(AdhocReports entity)
        {
            IAdhocReportService provider = this.Container.Resolve<IAdhocReportService>();
            return provider.IsAdhocReportExists(entity);
        }

        #endregion

        #region OrganizationEmail

        public OrganizationEmailDetails GetOrganizationEmail(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            IOrganizationEmailService provider = this.Container.Resolve<IOrganizationEmailService>();
            return provider.GetOrganizationEmail(currentUserId, userId, startIndex, pageSize, search);
        }

        public OrganizationEmails SaveOrganizationEmail(int currentUserId, OrganizationEmails entity, string pageName)
        {
            IOrganizationEmailService provider = this.Container.Resolve<IOrganizationEmailService>();
            return provider.SaveOrganizationEmail(currentUserId, entity, pageName);
        }

        public bool DeleteOrganizationEmail(int currentUserId, int id, string pageName)
        {
            IOrganizationEmailService provider = this.Container.Resolve<IOrganizationEmailService>();
            return provider.DeleteOrganizationEmail(currentUserId, id, pageName);
        }

        public OrganizationEmails GetOrganizationEmailById(int id)
        {
            IOrganizationEmailService provider = this.Container.Resolve<IOrganizationEmailService>();
            return provider.GetOrganizationEmailById(id);
        }

        public List<OrganizationEmails> GetListOrganizationEmail()
        {
            IOrganizationEmailService provider = this.Container.Resolve<IOrganizationEmailService>();
            return provider.GetListOrganizationEmail();
        }

        public bool IsOrganizationEmailExists(OrganizationEmails entity)
        {
            IOrganizationEmailService provider = this.Container.Resolve<IOrganizationEmailService>();
            return provider.IsOrganizationEmailExists(entity);
        }

        #endregion

        #region News

        public NewsDetails GetNews(int currentUserId, bool all, int id, int startIndex, int pageSize, string search, object filterModel)
        {
            INewsService provider = this.Container.Resolve<INewsService>();
            return provider.GetNews(currentUserId, all, id, startIndex, pageSize, search, filterModel);
        }

        public Newss SaveNews(int currentUserId, Newss entity, string pageName)
        {
            INewsService provider = this.Container.Resolve<INewsService>();
            return provider.SaveNews(currentUserId, entity, pageName);
        }

        public bool DeleteNews(int currentUserId, int id, string pageName)
        {
            INewsService provider = this.Container.Resolve<INewsService>();
            return provider.DeleteNews(currentUserId, id, pageName);
        }

        public Newss GetNewsById(int id)
        {
            INewsService provider = this.Container.Resolve<INewsService>();
            return provider.GetNewsById(id);
        }

        public List<Newss> GetListNews()
        {
            INewsService provider = this.Container.Resolve<INewsService>();
            return provider.GetListNews();
        }

        public bool IsNewsExists(Newss entity)
        {
            INewsService provider = this.Container.Resolve<INewsService>();
            return provider.IsNewsExists(entity);
        }

        #endregion

        #region HelpDesk

        public List<HelpDesks> GetNewTickets(int userID, int organizationId)
        {
            IDashboardService provider = this.Container.Resolve<IDashboardService>();
            return provider.GetNewTickets(userID, organizationId);
        }

        public AuthenticateUser DigestAuthentication(int uid)
        {
            IAuthenticationService provider = this.Container.Resolve<IAuthenticationService>();
            return provider.DigestAuthentication(uid);
        }

        #endregion
    }
}
