using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public class CommonService : ICommonService
    {
        #region Common

        public Hanodale.DataAccessLayer.Interfaces.ICommonService DataProvider;

        public CommonService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.CommonService();
        }

        public List<Organizations> GetMainCostCenter(int userId)
        {
            return this.DataProvider.GetMainCostCenter(userId);
        }

        public List<Organizations> GetSubCostCenter(int id, int userId)
        {
            return this.DataProvider.GetSubCostCenter(id, userId);
        }

        public List<Organizations> GetSubCostCenterById(int id, int userId)
        {
            return this.DataProvider.GetSubCostCenterById(id, userId);
        }

        public List<Organizations> GetAllSubCostCenterByFilterId(int categoryId, int userId, int subCostId)
        {
            return this.DataProvider.GetAllSubCostCenterByFilterId(categoryId, userId, subCostId);
        }

        public List<ModuleItems> GetListModuleItem(int id)
        {
            return this.DataProvider.GetListModuleItem(id);
        }

        public List<ModuleTypes> GetListModuleTypes()
        {
            return this.DataProvider.GetListModuleTypes();
        }

        public List<Users> GetListUser()
        {
            return this.DataProvider.GetListUser();
        }

        public List<MainMenus> GetListMainMenu()
        {
            return this.DataProvider.GetListMainMenu();
        }
        public Organizations GetOrganizationById(int id)
        {
            return this.DataProvider.GetOrganizationById(id);
        }
        public ModuleCodes GetModuleCodes(int submenu_Id, string orgPrefix, string existCode)
        {
            return this.DataProvider.GetModuleCodes(submenu_Id, orgPrefix, existCode);
        }
        public string GetGenerateCodeByOrgId(int organization_Id, int submenu_Id)
        {
            return this.DataProvider.GetGenerateCodeByOrgId(organization_Id, submenu_Id);
        }
        public List<Organizations> GetSubCostByMainCostId(int mainCostCenter_Id)
        {
            return this.DataProvider.GetSubCostByMainCostId(mainCostCenter_Id);
        }

        public List<Organizations> GetSubCostCenterListById(int id, int userId)
        {
            return this.DataProvider.GetSubCostCenterListById(id, userId);
        }
       

        public List<Organizations> GetAllMainCostCenter()
        {
            return this.DataProvider.GetAllMainCostCenter();
        }

        public List<Organizations> GetAllSubCostCenter()
        {
            return this.DataProvider.GetAllSubCostCenter();
        }

        public List<ModuleTypes> GetListModuleTypeWorkOrderCodes()
        {
            return this.DataProvider.GetListModuleTypeWorkOrderCodes();
        }

        public List<AssignedOrganizations> GetListofAssignedOrganisation(int user_Id)
        {
            return this.DataProvider.GetListofAssignedOrganisation(user_Id);
        }

        public List<Organizations> GetListofSubCostCenter(int id)
        {
            return this.DataProvider.GetListofSubCostCenter(id);
        }

        public List<ModuleItems> GetListAllModuleItem(int id)
        {
            return this.DataProvider.GetListAllModuleItem(id);
        }

        public List<ModuleItems> GetListRFQModuleItem(int id)
        {
            return this.DataProvider.GetListRFQModuleItem(id);
        }

        public string GenerateAutoCode(int organization_Id, int type_Id, MenuTypes menuType)
        {
            return this.DataProvider.GenerateAutoCode(organization_Id, type_Id, menuType);
        }

        public List<ModuleItems> GetListMaintenanceTypeModuleItem(int id)
        {
            return this.DataProvider.GetListMaintenanceTypeModuleItem(id);
        }

        public List<ModuleItems> GetListRFQStatusModuleItem(int id)
        {
            return this.DataProvider.GetListRFQStatusModuleItem(id);
        }

        public List<string> GetReportList()
        {
            return this.DataProvider.GetReportList();
        }

        public List<ModuleItems> GetStatusType()
        {
            return this.DataProvider.GetStatusType();
        }

        public List<TableProfileMetadatas> GetFieldMetadata(TableProfiles entityEn)
        {
            return this.DataProvider.GetFieldMetadata(entityEn);
        }

        public List<LocalizationLanguages> GetAvailableLanguageList()
        {
            return this.DataProvider.GetAvailableLanguageList();
        }

        public TableProfiles GetTableProfileWithTabs(TableProfiles entityEn)
        {
            return this.DataProvider.GetTableProfileWithTabs(entityEn);
        }
        public List<AddressCities> GetAddressCityList(AddressCities entityEn)
        {
            return this.DataProvider.GetAddressCityList(entityEn);
        }

        public List<AddressStates> GetAddressStateList(AddressStates entityEn)
        {
            return this.DataProvider.GetAddressStateList(entityEn);
        }

        public List<AddressCountries> GetAddressCountryList(AddressCountries entityEn)
        {
            return this.DataProvider.GetAddressCountryList(entityEn);
        }
        public List<User> GetUsersListByApprovalRole(int approvalId)
        {
            return this.DataProvider.GetUsersListByApprovalRole(approvalId);
        }


        #endregion
    }
}
