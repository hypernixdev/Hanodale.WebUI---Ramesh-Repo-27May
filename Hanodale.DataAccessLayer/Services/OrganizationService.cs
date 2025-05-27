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
using Hanodale.Domain;


namespace Hanodale.DataAccessLayer.Services
{
    public class OrganizationService : IOrganizationService
    {
        #region Organization

        public OrganizationDetails GetOrganizationBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            var _result = new OrganizationDetails();
            _result.recordDetails = new RecordDetails();
            string a = Common.RecordStatus.Active.ToString();
            string b = Common.RecordStatus.InActive.ToString();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.Organizations.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                    var result = model.Organizations.OrderByDescending(p => p.modifiedDate).Where(p => (string.IsNullOrEmpty(search) ? true :
                         (p.name.Contains(search)
                        || p.Organization2.name.Contains(search)
                        || p.OrganizationCategory.categoryName.Contains(search)
                        || p.description.Contains(search)
                        || p.prefix.Contains(search)
                        || p.description.Contains(search)
                        || (a.Contains(search) ? p.isActive == true : b.Contains(search) ? p.isActive == false : false))))
                       .Select(p => new Organizations
                       {
                           id = p.id,
                           name = p.name,
                           description = p.description,
                           prefix = p.prefix,
                           isActive = p.isActive == null ? true : (bool)p.isActive,
                           parentName = p.parent_Id != null ? p.Organization2.name : "",
                           orgCategoryName = p.OrganizationCategory.categoryName,
                           code = p.code,
                           sapcode = p.sapcode,
                           createdBy = p.createdBy,
                           createdDate = p.createdDate,
                           modifiedBy = p.modifiedBy,
                           modifiedDate = p.modifiedDate,
                           hasCategoryChild = (p.OrganizationCategory!=null && p.OrganizationCategory.OrganizationCategoryConfigs.Count > 0)
                       }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstOrganizations = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public Organizations CreateOrganization(int currentUserId, Organizations entity, string pageName)
        {
            Organization _entity = new Organization();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Organization
                    _entity.name = entity.name;
                    _entity.description = entity.description;
                    _entity.prefix = entity.prefix;
                    _entity.isActive = entity.isActive;
                    _entity.orgCategory_Id = entity.orgCategory_Id;
                    _entity.parent_Id = entity.parent_Id;
                    _entity.code = entity.code;
                    _entity.sapcode = entity.sapcode;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;

                    model.Organizations.Add(_entity);
                    model.SaveChanges();

                    entity.id = _entity.id;
                }
            }
            catch (Exception ex)
            {
                return null;
                //throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        public Organizations UpdateOrganization(int currentUserId, Organizations entity, string pageName)
        {

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _entity = model.Organizations.SingleOrDefault(p => p.id == entity.id);
                    if (entity != null)
                    {
                        _entity.name = entity.name;
                        _entity.description = entity.description;
                        _entity.prefix = entity.prefix;
                        _entity.code = entity.code;
                        _entity.sapcode = entity.sapcode;
                        _entity.isActive = entity.isActive;
                        _entity.modifiedBy = entity.modifiedBy;
                        _entity.modifiedDate = entity.modifiedDate;
                    }
                    model.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return null;
                //throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        public bool DeleteOrganization(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    Organization _roleEn = model.Organizations.SingleOrDefault(p => p.id == id);

                    if (_roleEn != null)
                    {
                        model.Organizations.Remove(_roleEn);
                    }
                    model.SaveChanges();
                }
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isDeleted;
        }

        public Organizations GetOrganizationById(int id)
        {
            Organizations _entity = new Organizations();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Organizations.Include("OrganizationCategory").Include("Organization2").SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new Organizations
                        {
                            name = entity.name,
                            description = entity.description,
                            prefix = entity.prefix,
                            isActive = entity.isActive,
                            orgCategory_Id = entity.orgCategory_Id,
                            parent_Id = entity.parent_Id,
                            code = entity.code,
                            sapcode = entity.sapcode,
                            orgCategoryName = entity.OrganizationCategory.categoryName,
                            parentName = entity.Organization2 == null ? "" : entity.Organization2.name,
                            parentOrgCategory_Id=(entity.Organization2==null ? 0 : entity.Organization2.orgCategory_Id)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _entity;
        }

        public OrganizationCategories GetOrganizationCategoryById(int id)
        {
            var _entity = new OrganizationCategories();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.OrganizationCategories.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new OrganizationCategories
                        {
                            id = entity.id,
                            name = entity.categoryName,
                            icon = entity.imageUrl,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _entity;
        }

        public bool OrganizationExists(Organizations entity)
        {
            Organization _entity = new Organization();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.Organizations.SingleOrDefault(p => p.name == entity.name && p.parent_Id==entity.parent_Id);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.Organizations.SingleOrDefault(p => p.name == entity.name && p.parent_Id == entity.parent_Id && p.id != entity.id);
                        if (_entity != null)
                            isExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                //throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isExists;
        }

        public List<OrganizationCategoryConfigs> GetOrganizationCategoryConfig()
        {
            List<OrganizationCategoryConfigs> result = new List<OrganizationCategoryConfigs>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center
                    result = model.OrganizationCategoryConfigs.Include("OrganizationCategory1").Select(p => new OrganizationCategoryConfigs
                    {
                        category_Id = p.orgCategory_Id,
                        childCategory_Id = p.childOrgCategory_Id,
                        categoryName = p.OrganizationCategory1.categoryName,
                        categoryIcon = p.OrganizationCategory1.imageUrl
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return result;
        }

        public List<OrganizationCategoryConfigs> GetOrganizationCategoryConfigByOrganizationId(int id)
        {
            List<OrganizationCategoryConfigs> result = new List<OrganizationCategoryConfigs>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center
                    var org = model.Organizations.SingleOrDefault(p => p.id == id);
                    if (org != null)
                    {
                        result = model.OrganizationCategoryConfigs.Where(p => p.orgCategory_Id == org.orgCategory_Id).Select(p => new OrganizationCategoryConfigs
                        {
                            childCategory_Id = p.childOrgCategory_Id,
                            childCategoryName = p.OrganizationCategory1.categoryName,
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return result;
        }

        public List<OrganizationCategoryConfigs> GetOrganizationCategoryConfigByCategoryId(int id)
        {
            List<OrganizationCategoryConfigs> result = new List<OrganizationCategoryConfigs>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get main Cost Center
                    result = model.OrganizationCategoryConfigs.Where(p => p.orgCategory_Id == id).Select(p => new OrganizationCategoryConfigs
                    {
                        childCategory_Id = p.childOrgCategory_Id,
                        childCategoryName = p.OrganizationCategory1.categoryName,
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return result;
        }

        public List<Assets> GetCostCenter(int currentUserId, List<bool> lstAccess)
        {
            List<Assets> _lstAsset;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    bool canAdd = true;// lstAccess[0];
                    bool canEdit = true;// lstAccess[1];
                    bool canDelete = true;// lstAccess[2];
                    var result = model.Organizations.Include("Organization1").Include("OrganizationCategory").Where(p => p.isActive && p.parent_Id == null).ToList();

                    return _lstAsset = result.Where(p => p.isActive).Select(p => new Assets
                    {
                        id = p.id,
                        name = p.name,
                        description = p.description,
                        organization_Id = (int)p.orgCategory_Id,
                        orgCategory_Id = (int)p.orgCategory_Id,
                        isRoot = true,
                        imageUrl = p.OrganizationCategory.imageUrl,
                        backColor = p.OrganizationCategory.backColor,
                        forceColor = p.OrganizationCategory.forceColor,
                        ordering = p.OrganizationCategory.ordring,
                        isStatic = true,
                        canAdd = false,
                        isActiveBranch = true,
                        childCount = p.Organization1.Count,
                        categoryName = p.OrganizationCategory.categoryName,
                        hasChild = p.Organization1.Count > 0,
                        subOrganizationCount = p.Organization1.Count,
                        parentName = p.parent_Id != null ? p.Organization2.name : "---",
                        isMainCost = true,
                        additionalParameters = p.Organization1.Where(h => h.isActive).Select(c => new Assets
                        {
                            id = c.id,
                            name = c.name,
                            description = c.description,
                            organization_Id = (int)c.id,
                            orgCategory_Id = (int)c.orgCategory_Id,
                            isSubCost = true,
                            isRoot = true,
                            imageUrl = c.OrganizationCategory.imageUrl,
                            backColor = c.OrganizationCategory.backColor,
                            forceColor = c.OrganizationCategory.forceColor,
                            ordering = c.OrganizationCategory.ordring,
                            isStatic = true,
                            canAdd = canAdd,
                            canEdit = canEdit,
                            canDelete = canDelete,
                            allowChild = false,
                            isActiveBranch = true,
                            childCount = 0,
                            categoryName = c.OrganizationCategory.categoryName,
                            hasChild = false,
                            subOrganizationCount = 0,
                            parentName = c.parent_Id != null ? c.Organization2.name : "---",
                        }).ToList()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Organizations> GerOrganisation(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Type List
                    //.Where(p => p.visibility)
                    return model.Organizations.Where(p => p.isActive && p.orgCategory_Id == id).Select(p => new Organizations
                    {
                        id = p.id,
                        parent_Id = p.parent_Id,
                        orgCategory_Id = p.orgCategory_Id,
                        name = p.name,
                        description = p.description,
                        prefix = p.prefix,
                        isActive = p.isActive,
                        code = p.code,
                        sapcode = p.sapcode,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,


                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
        #endregion
    }
}
