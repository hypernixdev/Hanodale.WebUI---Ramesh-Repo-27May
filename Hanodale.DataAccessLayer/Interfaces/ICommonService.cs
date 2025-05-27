using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface ICommonService
    {
        #region Common

        /// <summary>
        /// This method is to get the main cost center
        /// </summary>
        /// <param name="userId">userid</param>
        /// <returns></returns>
        List<Organizations> GetMainCostCenter(int userId);

        /// <summary>
        /// This method is to get subcost center
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        List<Organizations> GetSubCostCenter(int id, int userId);

        List<Organizations> GetSubCostCenterById(int id, int userId);

        List<Organizations> GetAllSubCostCenterByFilterId(int categoryId, int userId, int subCostId);

        List<ModuleItems> GetListModuleItem(int id);

        List<ModuleTypes> GetListModuleTypes();



        List<Users> GetListUser();

        List<MainMenus> GetListMainMenu();

        Organizations GetOrganizationById(int id);

        ModuleCodes GetModuleCodes(int submenu_Id, string orgPrefix, string generatedCode);

        string GetGenerateCodeByOrgId(int organization_Id, int submenu_Id);

        List<Organizations> GetSubCostByMainCostId(int mainCostCenter_Id);

        List<Organizations> GetSubCostCenterListById(int id, int userId);

        List<Organizations> GetAllMainCostCenter();

        List<Organizations> GetAllSubCostCenter();

        List<ModuleTypes> GetListModuleTypeWorkOrderCodes();

        List<AssignedOrganizations> GetListofAssignedOrganisation(int user_Id);

        List<Organizations> GetListofSubCostCenter(int id);

        List<ModuleItems> GetListAllModuleItem(int id);

        List<ModuleItems> GetListRFQModuleItem(int id);

        string GenerateAutoCode(int organization_Id, int type_Id, MenuTypes menuType);

        List<ModuleItems> GetListMaintenanceTypeModuleItem(int id);

        List<ModuleItems> GetListRFQStatusModuleItem(int id);

        List<string> GetReportList();

        List<ModuleItems> GetStatusType();

        List<TableProfileMetadatas> GetFieldMetadata(TableProfiles entityEn);

        List<LocalizationLanguages> GetAvailableLanguageList();

        TableProfiles GetTableProfileWithTabs(TableProfiles entityEn);

        List<AddressCities> GetAddressCityList(AddressCities entityEn);

        List<AddressStates> GetAddressStateList(AddressStates entityEn);

        List<AddressCountries> GetAddressCountryList(AddressCountries entityEn);

        List<User> GetUsersListByApprovalRole(int approvalId);


        #endregion
    }
}
