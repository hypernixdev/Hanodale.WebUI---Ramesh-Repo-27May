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
    public class WorkCategoryService : IWorkCategoryService
    {
        #region WorkCategory

        public WorkCategoryDetails GetWorkCategoryBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            WorkCategoryDetails _result = new WorkCategoryDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    bool c, d;
                    c = Common.Visibility.True.ToString().ToLower().Contains(search.ToLower()); d = Common.Visibility.False.ToString().ToLower().Contains(search.ToLower());
                    //get total record
                    _result.recordDetails.totalRecords = model.WorkCategories.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    var result = model.WorkCategories
                                .OrderBy(a => a.modifiedDate)
                                .Where(a => a.name.Contains(search)
                                     || a.description.Contains(search)
                                     || a.remarks.Contains(search)
                                     || (c ? a.isVisible == true : d ? a.isVisible == false : false))
                                     .Select(p => new WorkCategorys
                                     {
                                         id = p.id,
                                         name = p.name,
                                         description = p.description,
                                         remarks = p.remarks,
                                         isVisible = p.isVisible,
                                         createdBy = p.createdBy,
                                         createdDate = p.createdDate,
                                         modifiedBy = p.modifiedBy,
                                         modifiedDate = p.modifiedDate
                                     }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstWorkCategory = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public WorkCategoryDetails GetWorkCategory(int currentUserId, int userId, int startIndex, int pageSize)
        {
            WorkCategoryDetails _result = new WorkCategoryDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.WorkCategories.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    _result.lstWorkCategory = model.WorkCategories
                                      .OrderBy(a => a.modifiedDate) //
                                      .Skip(startIndex).Take(pageSize)
                                      .Select(p => new WorkCategorys
                                      {
                                          id = p.id,
                                          name = p.name,
                                          description = p.description,
                                          remarks = p.remarks,
                                          isVisible = p.isVisible,
                                          createdBy = p.createdBy,
                                          createdDate = p.createdDate,
                                          modifiedBy = p.modifiedBy,
                                          modifiedDate = p.modifiedDate
                                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public WorkCategorys CreateWorkCategory(int currentUserId, WorkCategorys entity, string pageName)
        {
            WorkCategory _entity = new WorkCategory();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new WorkCategory 
                    _entity.name = entity.name;
                    _entity.description = entity.description;
                    _entity.remarks = entity.remarks;
                    _entity.isVisible = true;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;

                    model.WorkCategories.Add(_entity);
                    model.SaveChanges();

                    entity.id = _entity.id;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        public WorkCategorys UpdateWorkCategory(int currentUserId, WorkCategorys entity, string pageName)
        {
            WorkCategory _entity = new WorkCategory();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update WorkCategory
                    _entity = model.WorkCategories.SingleOrDefault(p => p.id == entity.id);
                    if (_entity != null)
                    {
                        _entity.name = entity.name;
                        _entity.description = entity.description;
                        _entity.remarks = entity.remarks;
                        _entity.isVisible = entity.isVisible;
                        _entity.modifiedBy = entity.modifiedBy;
                        _entity.modifiedDate = entity.modifiedDate;
                    }
                    model.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        public bool DeleteWorkCategory(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    WorkCategory _entity = model.WorkCategories.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.WorkCategories.Remove(_entity);
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

        public WorkCategorys GetWorkCategoryById(int id)
        {
            WorkCategorys _entity = new WorkCategorys();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.WorkCategories.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new WorkCategorys
                        {
                            id = entity.id,
                            name = entity.name,
                            description = entity.description,
                            remarks = entity.remarks,
                            isVisible = entity.isVisible,
                            createdBy = entity.createdBy,
                            createdDate = entity.createdDate,
                            modifiedBy = entity.modifiedBy,
                            modifiedDate = entity.modifiedDate

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

        public List<WorkCategorys> GetListWorkCategory()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.WorkCategories.Where(p => p.isVisible == true).Select(p => new WorkCategorys
                    {
                        id = p.id,
                        name = p.name,
                        description = p.description,
                        remarks = p.remarks,
                        isVisible = p.isVisible,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool IsWorkCategoryExists(WorkCategorys entity)
        {
            WorkCategory _entity = new WorkCategory();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.WorkCategories.SingleOrDefault(p => p.name == entity.name);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.WorkCategories.SingleOrDefault(p => p.name == entity.name && p.id != entity.id);
                        if (_entity != null)
                            isExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isExists;
        }

        #endregion
    }
}
