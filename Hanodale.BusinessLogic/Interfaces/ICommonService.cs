using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ICommonService
    {
        List<Organizations> GetMainCostCenter(int userId);

        List<Organizations> GetSubCostCenter(int id, int userId);

        List<Organizations> GetSubCostCenterById(int id, int userId);

        List<Organizations> GetAllSubCostCenterByFilterId(int categoryId, int userId, int subCostId);

        List<ModuleItems> GetListModuleItem(int id);

        List<ModuleTypes> GetListModuleTypes();

        List<Users> GetListUser();

        List<MainMenus> GetListMainMenu();

        Organizations GetOrganizationById(int id);

        ModuleCodes GetModuleCodes(int submenu_Id, string orgPrefix, string existCode);

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

        //List<Plant> GetPlantList();
        List<AddressCities> GetAddressCityList(AddressCities entityEn);

        List<AddressStates> GetAddressStateList(AddressStates entityEn);

        List<AddressCountries> GetAddressCountryList(AddressCountries entityEn);



        TableProfiles GetTableProfileWithTabs(TableProfiles entityEn);
        List<User> GetUsersListByApprovalRole(int approvalId);

    }
}
