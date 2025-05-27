using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanodale.Entity.Core;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using System.Data.Objects.SqlClient;
using System.Collections;
using System.Globalization;
using Hanodale.Domain.DTOs;
using Hanodale.DataAccessLayer.Interfaces;
using System.Text.RegularExpressions;
using System.Configuration;


namespace Hanodale.DataAccessLayer.Services
{
    public class CommonService : BaseService, ICommonService
    {
        public List<Organizations> GetMainCostCenter(int userId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.AssignedOrganizations.Include("Organizations").Where(p => p.user_Id == userId).Where(p => p.Organization.isActive && p.Organization.Organization2.isActive).Select(p => p.Organization.Organization2).Distinct().Select(p => new Organizations
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GetSubCostCenter(int id, int userId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center

                    return model.AssignedOrganizations.Include("Organizations").Where(p => p.Organization.isActive && p.Organization.parent_Id == id).Select(p => new Organizations
                    {
                        id = p.Organization.id,
                        name = p.Organization.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GetSubCostCenterById(int id, int userId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center

                    var entity = model.Organizations.SingleOrDefault(p => p.id == id);

                    var user = model.Users.SingleOrDefault(p => p.id == userId);

                    var lst = model.AssignedOrganizations.Include("Organizations").Where(p => p.Organization.isActive && p.Organization.parent_Id == entity.parent_Id && p.user_Id == userId).Select(p => new Organizations
                    {
                        id = p.Organization.id,
                        name = p.Organization.name,
                        parent_Id = p.Organization.parent_Id
                    }).ToList();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GetAllSubCostCenterByFilterId(int categoryId, int userId, int subCostId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _mainCostCenterId = model.Organizations.SingleOrDefault(p => p.id == subCostId).parent_Id;

                    //Get main Cost Center
                    var entity = model.AssignedOrganizations.Include("Organization").FirstOrDefault(p => p.Organization.isActive && p.user_Id == userId && p.Organization.parent_Id == _mainCostCenterId);
                    var defaultItem = model.AssignedOrganizations.Include("Organization").FirstOrDefault(p => p.Organization.isActive && p.user_Id == userId && p.Organization.parent_Id == _mainCostCenterId && p.isDefault);
                    var defaultId = 0;
                    if (defaultItem != null)
                        defaultId = defaultItem.organization_Id;

                    if (entity == null)
                    {
                        _mainCostCenterId = model.Organizations.SingleOrDefault(p => p.id == subCostId).parent_Id;
                    }

                    return model.Organizations.Where(p => p.isActive && p.orgCategory_Id == categoryId && p.parent_Id == _mainCostCenterId).Select(p => new Organizations
                    {
                        id = p.id,
                        name = p.name,
                        parent_Id = p.parent_Id,
                        isDefault = p.id == defaultId,
                        parentName = model.Organizations.Where(a => a.isActive && a.id == _mainCostCenterId).Select(P => P.name).FirstOrDefault()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // throw new FaultException(ex.InnerException.InnerException.Message);
                return new List<Organizations>();
            }
        }

        public List<ModuleItems> GetListModuleItem(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.ModuleItems.OrderBy(p => p.sortOrder).Where(p => p.modulType_Id == id && p.visibility == true).Select(p => new ModuleItems
                    {
                        id = p.id,
                        name = p.name,
                        description = p.description,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleTypes> GetListModuleTypes()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.ModuleTypes.Where(p => p.id != 4 && p.id != 5 && p.id != 6 && p.id != 30).OrderBy(p => p.name).Select(p => new ModuleTypes
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleTypes> GetListModuleTypeWorkOrderCodes()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.ModuleTypes.Where(p => p.id == 4 || p.id == 5 || p.id == 6).OrderBy(p => p.name).Select(p => new ModuleTypes
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Users> GetListUser()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.Users.Select(p => new Users
                    {
                        id = p.id,
                        userName = p.userName,
                        status = p.status
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<MainMenus> GetListMainMenu()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.MenuItems.Where(p => p.reference_Id != null && (p.id == 41 || p.id == 42 || p.id == 44 || p.id == 84 || p.id == 94)).OrderBy(p => p.pageName).Select(p => new MainMenus
                    {
                        id = p.id,
                        pageName = p.pageName,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public Organizations GetOrganizationById(int id)
        {
            Organizations _orgEn = new Organizations();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Organizations.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _orgEn = new Organizations
                        {
                            id = entity.id,
                            prefix = entity.prefix,
                            parent_Id = entity.parent_Id
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _orgEn;
        }

        public ModuleCodes GetModuleCodes(int submenu_Id, string orgPrefix, string existCode)
        {
            ModuleCodes _modEn = new ModuleCodes();
            string _reqCode = string.Empty;
            int _iResult = 0;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var entity = model.ModuleCodes.SingleOrDefault(p => p.menu_Id == submenu_Id);
                    if (entity != null)
                    {
                        _modEn = new ModuleCodes
                        {
                            id = entity.id,
                            menu_Id = entity.menu_Id,
                            prefix = entity.prefix,
                            mask = entity.mask
                        };
                        if (string.IsNullOrEmpty(existCode))
                        {
                            _modEn.generateCode = orgPrefix + "_" + entity.prefix + "_" + entity.mask + 1;
                        }
                        else
                        {

                            _modEn.generateCode = Regex.Replace(existCode, @"[^\d]", "");
                            int.TryParse(_modEn.generateCode, out _iResult);
                            _iResult++;
                            string num = string.Empty;
                            string val = "0";
                            int count = _modEn.mask.ToString().Length;
                            int result = _iResult.ToString().Length;
                            int n = count - result;
                            for (int i = 0; i <= n; i++)
                            {
                                num += val;
                            }

                            _modEn.generateCode = orgPrefix + "_" + entity.prefix + "_" + num + _iResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _modEn;
        }

        public string GetGenerateCodeByOrgId(int organization_Id, int submenu_Id)
        {
            string _result = string.Empty;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    model.Configuration.ProxyCreationEnabled = false;
                    if (submenu_Id == 41)
                    {
                        var _res = model.HelpDesks.Where(p => p.organization_Id == organization_Id)
                              .OrderByDescending(p => p.createdDate)
                           .ThenByDescending(n => n.id)
                           .Select(p => new HelpDesks
                           {
                               code = p.code
                           }).FirstOrDefault();

                        _result = (_res == null ? string.Empty : _res.code);
                    }

                    else if (submenu_Id == 18)
                    {
                        var _res = model.Businesses.OrderByDescending(p => p.id)
                               .OrderByDescending(p => p.createdDate)
                           .ThenByDescending(n => n.id)
                           .Select(p => new Businesses
                           {
                               code = p.code
                           }).FirstOrDefault();

                        _result = (_res == null ? string.Empty : _res.code);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public List<Organizations> GetSubCostByMainCostId(int mainCostCenter_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get User List

                    return model.Organizations.Where(p => p.parent_Id == mainCostCenter_Id && p.isActive == true).Select(p => new Organizations
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GetSubCostCenterListById(int id, int userId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center

                    var entity = model.Organizations.SingleOrDefault(p => p.id == id);

                    var user = model.Users.SingleOrDefault(p => p.id == userId);
                    if (user != null)
                    {
                        if (user.isAccessAllOrganization == true)
                        {
                            return model.Organizations.Where(p => p.parent_Id != null).Select(p => new Organizations
                            {
                                id = p.id,
                                name = "[" + model.Organizations.Where(o => o.id == p.parent_Id).FirstOrDefault().name + "] " + p.name,
                                parent_Id = p.parent_Id
                            }).OrderBy(r => r.name).ToList();
                        }
                    }

                    var lst = model.AssignedOrganizations.Include("Organizations").Where(p => p.Organization.parent_Id == entity.parent_Id && p.user_Id == userId).Select(p => new Organizations
                    {
                        id = p.Organization.id,
                        name = p.Organization.name,
                        parent_Id = p.Organization.parent_Id
                    }).ToList();

                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GetAllMainCostCenter()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center

                    return model.Organizations.Where(p => p.parent_Id == null && p.isActive == true).Select(p => new Organizations
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GetAllSubCostCenter()
        {
            List<Organizations> lstorg = new List<Organizations>();
            Organizations org = new Organizations();
            int maincostId = 0;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center

                    var maincostIds = model.Organizations.Where(p => p.parent_Id == null && p.isActive).Select(p => p.id).ToList();

                    foreach (var items in maincostIds)
                    {
                        maincostId = Convert.ToInt32(items);
                        var subcostcenter = model.Organizations.Where(p => p.parent_Id == maincostId && p.isActive).ToList();
                        foreach (var item in subcostcenter)
                        {
                            org = new Organizations();
                            org.id = item.id;
                            org.name = item.name;
                            org.parentName = model.Organizations.Where(a => a.id == maincostId && a.isActive).Select(P => P.name).FirstOrDefault();
                            lstorg.Add(org);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return lstorg;
        }

        public List<User> GetUsersListByApprovalRole(int approvalId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var users = model.Users
     .Where(p => p.userRole_Id == approvalId)
     .ToList() // Materialize the query to execute it and bring results into memory
     .Select(p => new User
     {
         id = p.id,
         firstName = p.firstName,
         lastName = p.lastName
     })
     .ToList();
                    return users;

                }
          
         
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }


        public List<AssignedOrganizations> GetListofAssignedOrganisation(int user_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center

                    return model.AssignedOrganizations.Where(p => p.Organization.isActive && p.user_Id == user_Id).Select(p => new AssignedOrganizations
                    {
                        organization_Id = p.organization_Id,
                        isDefault = p.isDefault
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GetListofSubCostCenter(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center

                    return model.Organizations.Where(p => p.isActive && p.id == id).Select(p => new Organizations
                    {
                        id = p.id,
                        name = p.name,
                        parent_Id = p.parent_Id
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleItems> GetListAllModuleItem(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Item List

                    return model.ModuleItems.OrderBy(p => p.sortOrder).Where(p => p.modulType_Id == id && p.id != 105 && p.visibility == true).Select(p => new ModuleItems
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleItems> GetListRFQModuleItem(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Item List

                    return model.ModuleItems.OrderBy(p => p.sortOrder).Where(p => p.modulType_Id == id && p.visibility == true && (p.id == 114 || p.id == 538 || p.id == 113 || p.id == 115 || p.id == 112)).Select(p => new ModuleItems
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Plant> GetPlantList()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Plant List

                    return model.Plant.OrderBy(p => p.name).Select(p => new Plant
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleItems> GetListMaintenanceTypeModuleItem(int id)
        {
            List<ModuleItems> lst = new List<ModuleItems>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Item List

                    return model.ModuleItems.OrderBy(p => p.sortOrder).Where(p => p.modulType_Id == id && p.visibility == true && p.id == 54).Select(p => new ModuleItems
                    {
                        id = p.id,
                        name = p.name,
                    }).OrderBy(a => a.name).ToList();


                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleItems> GetListRFQStatusModuleItem(int id)
        {
            List<ModuleItems> lst = new List<ModuleItems>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Item List

                    lst = model.ModuleItems.OrderBy(p => p.sortOrder).Where(p => p.modulType_Id == id && p.visibility == true && (p.id == 109 || p.id == 110)).Select(p => new ModuleItems
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();

                    var lst1 = model.ModuleItems.OrderBy(p => p.sortOrder).Where(p => p.visibility == true && p.id == 120).Select(p => new ModuleItems
                    {
                        id = p.id,
                        name = p.name,
                    }).ToList();

                    lst.AddRange(lst1);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return lst;
        }

        public List<Organizations> GetSubCostCenter(int id)
        {
            throw new NotImplementedException();
        }

        public string GenerateAutoCode(int organization_Id, int type_Id, MenuTypes menuType)
        {
            string result = string.Empty;
            string lastItem = string.Empty;
            string sectionPrefix = string.Empty;
            string itemPrefix = string.Empty;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (itemPrefix == null)
                        itemPrefix = string.Empty;

                    if (menuType == null)
                        menuType = new MenuTypes();

                    string orgPrefix = string.Empty;
                    var org = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (org != null)
                    {
                        orgPrefix = org.prefix;
                    }
                    if (menuType.hasModuleItem)
                    {
                        var moduleItem = model.ModuleItems.SingleOrDefault(p => p.id == type_Id);
                        itemPrefix = moduleItem.remarks ?? "";
                    }

                    ////////////////////////////////////////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

                    if (menuType.isHelpDesk)
                    {
                        sectionPrefix = menuType.appSettingList.FirstOrDefault(p => p.Key == "Auto_WR00Prefix").Value ?? "";
                        var item = model.HelpDesks.OrderByDescending(p => p.createdDate).ThenByDescending(p => p.id).FirstOrDefault(p => p.organization_Id == organization_Id);
                        if (item != null)
                            lastItem = item.code;
                    }
                    else if (menuType.isBusiness)
                    {
                        sectionPrefix = menuType.appSettingList.FirstOrDefault(p => p.Key == "Auto_BusinessPrefix").Value ?? "";
                        var _str = sectionPrefix + orgPrefix.Trim();
                        var item = model.Businesses.Where(p => p.code.Contains(_str)).OrderByDescending(p => p.createdDate).ThenByDescending(p => p.id).FirstOrDefault();
                        if (item != null)
                            lastItem = item.code;
                    }

                    result = sectionPrefix.Trim() + itemPrefix.Trim() + orgPrefix.Trim();

                    string strNumber = lastItem.Replace(result, "").Trim();


                    int number = 0;
                    Int32.TryParse(strNumber, out number);


                    return (result + (number + 1)).Trim();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<string> GetReportList()
        {
            List<string> lst = new List<string>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //lst = model.GetReportFiles().Select(p => p.).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return lst;
        }

        public List<ModuleItems> GetStatusType()
        {
            List<ModuleItems> lstStatus = new List<ModuleItems>();
            lstStatus.Add(new ModuleItems { id = -1, name = "ALL" });
            lstStatus.Add(new ModuleItems { id = 1, name = "Daily" });
            lstStatus.Add(new ModuleItems { id = 2, name = "Weekly" });
            lstStatus.Add(new ModuleItems { id = 3, name = "2Weekly" });
            lstStatus.Add(new ModuleItems { id = 4, name = "Monthly" });
            lstStatus.Add(new ModuleItems { id = 5, name = "2Monthly" });
            lstStatus.Add(new ModuleItems { id = 6, name = "Quarterly" });
            lstStatus.Add(new ModuleItems { id = 7, name = "Halfyearly" });
            lstStatus.Add(new ModuleItems { id = 8, name = "Yearly" });
            return lstStatus;
        }

        public List<LocalizationLanguages> GetAvailableLanguageList()
        {
            var lst = new List<LocalizationLanguages>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    lst = model.LocalizationLanguages.Where(p => p.visibility).Select(p => new LocalizationLanguages { id = p.id, culture = p.culture, flagIconName = p.flagIconName, name = p.name, shortName = p.shortName, visibility = p.visibility, isDefault = p.isDefault }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return lst;
        }
        public List<AddressCities> GetAddressCityList(AddressCities entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //return model.Address_City.Where(p => ((entityEn.addressCountry_Id==null || entityEn.addressCountry_Id == 0) ? true : p.address_Country_Id == entityEn.addressCountry_Id) 
                    //    && ((entityEn.addressState_Id ==null || entityEn.addressState_Id == 0) ? true : p.address_State_Id == entityEn.addressState_Id)).Select(p =>
                    return model.Address_City.Where(p => (entityEn.filterDropdown_Id == 0 ? true : p.address_State_Id == entityEn.filterDropdown_Id)).Select(p =>

                    new AddressCities
                    {
                        id = p.id,
                        code = p.code,
                        name = p.name,
                        addressState_Id = p.address_State_Id,
                        addressStateName = (p.address_State_Id == null ? "" : p.Address_State.name),
                        addressCountry_Id = p.address_Country_Id,
                        addressCountryName = (p.address_Country_Id == null ? "" : p.Address_Country.name),
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<AddressStates> GetAddressStateList(AddressStates entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //return model.Address_State.Where(p => ((entityEn.addressCountry_Id == null || entityEn.addressCountry_Id == 0) ? true : p.address_Country_Id == entityEn.addressCountry_Id)).Select(p =>
                    return model.Address_State.Where(p => (entityEn.filterDropdown_Id == 0 ? true : p.address_Country_Id == entityEn.filterDropdown_Id)).Select(p =>
                        new AddressStates
                        {
                            id = p.id,
                            code = p.code,
                            name = p.name,
                            addressCountry_Id = p.address_Country_Id,
                            addressCountryName = (p.address_Country_Id == null ? "" : p.Address_Country.name),
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<AddressCountries> GetAddressCountryList(AddressCountries entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //return model.Address_Country.Where(p => ((entityEn.regionalProfile_Id ==null || entityEn.regionalProfile_Id == 0) ? true : p.regional_Profile_Id == entityEn.regionalProfile_Id)).Select(p =>
                    return model.Address_Country.Select(p =>
                        new AddressCountries
                        {
                            id = p.id,
                            code = p.code,
                            name = p.name,
                            //regionalProfile_Id = p.regional_Profile_Id,
                            // regionalProfileName = (p.regional_Profile_Id == null ? "" : p.Address_Region.name),
                            // currencyProfile_Id = p.currency_Profile_Id,
                            // currencyProfileName = (p.currency_Profile_Id == null ? "" : p.Currency_Profile.name),
                            // languageProfile_Id = p.Language_Profile_Id,
                            // languageProfileName = (p.Language_Profile_Id == null ? "" : p.Language_Profile.name),
                            // countryCode = p.countryCode
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }


        /*
        public List<AddressCities> GetAddressCityList(AddressCities entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //return model.Address_City.Where(p => ((entityEn.addressCountry_Id==null || entityEn.addressCountry_Id == 0) ? true : p.address_Country_Id == entityEn.addressCountry_Id) 
                    //    && ((entityEn.addressState_Id ==null || entityEn.addressState_Id == 0) ? true : p.address_State_Id == entityEn.addressState_Id)).Select(p =>
                    return model.Address_City.Where(p => ( entityEn.filterDropdown_Id == 0 ? true : p.address_State_Id == entityEn.filterDropdown_Id)).Select(p =>
                           
                    new AddressCities
                        {
                            id = p.id,
                            code = p.code,
                            name = p.name,
                            addressState_Id = p.address_State_Id,
                            addressStateName = (p.address_State_Id == null ? "" : p.Address_State.name),
                            addressCountry_Id = p.address_Country_Id,
                            addressCountryName = (p.address_Country_Id == null ? "" : p.Address_Country.name),
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
        */

    }
}
