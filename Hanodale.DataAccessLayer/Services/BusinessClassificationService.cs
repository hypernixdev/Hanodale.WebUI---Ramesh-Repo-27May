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

namespace Hanodale.DataAccessLayer.Services
{
    public class BusinessClassificationService : IBusinessClassificationService
    {
        #region BusinessClassification

        /// <summary>
        /// This method is to get the BusinessClassification details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessClassificationDetails GetBusinessClassificationBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            BusinessClassificationDetails _result = new BusinessClassificationDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessClassifications.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    var result = model.BusinessClassifications.OrderByDescending(p => p.modifiedDate)
                                   .Where(p => SqlFunctions.StringConvert((double)p.business_Id).Contains(search)
                                   || SqlFunctions.StringConvert((double)p.classification_Id).Contains(search))
                                   .Select(p => new BusinessClassifications
                                   {
                                       id = p.id,
                                       business_Id = p.business_Id,
                                       classification_Id = p.classification_Id,
                                       createdBy = p.createdBy,
                                       createdDate = p.createdDate,
                                       modifiedBy = p.modifiedBy,
                                       modifiedDate = p.modifiedDate
                                   }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstBusinessClassification = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the Asset Warehouse details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessClassificationDetails GetBusinessClassification(int currentUserId, int userId, int startIndex, int pageSize)
        {
            BusinessClassificationDetails _result = new BusinessClassificationDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessClassifications.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    _result.lstBusinessClassification = model.BusinessClassifications.OrderByDescending(p => p.modifiedDate)
                                                 .Skip(startIndex).Take(pageSize)
                                                 .Select(p => new BusinessClassifications
                                                 {
                                                     id = p.id,
                                                     business_Id = p.business_Id,
                                                     classification_Id = p.classification_Id,
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

        /// <summary>
        /// This method is to save the Asset Warehouse details
        /// </summary> 
        public BusinessClassifications CreateBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName)
        {
            BusinessClassification _entity = new BusinessClassification();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new BusinessClassification
                    foreach (var Id in entity.classification_Ids)
                    {
                        _entity.business_Id = entity.business_Id;
                        _entity.classification_Id = Id;
                        _entity.createdBy = entity.createdBy;
                        _entity.createdDate = entity.createdDate;
                        _entity.modifiedBy = entity.modifiedBy;
                        _entity.modifiedDate = entity.modifiedDate;
                        model.BusinessClassifications.Add(_entity);
                        model.SaveChanges();
                    }
                    entity.id = _entity.id;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        /// <summary>
        /// This method is to update the BusinessClassification details
        /// </summary> 
        public BusinessClassifications UpdateBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // insert and  update BusinessClassification
                    List<int> ids = model.BusinessClassifications.Where(p => p.business_Id == entity.business_Id).Select(p => p.classification_Id).ToList();
                    //
                    foreach (var item in entity.classification_Ids)
                    {
                        if (ids.Contains(item))
                        {
                            ids.Remove(item);
                        }
                        else
                        {
                            BusinessClassification _entity = new BusinessClassification();
                            _entity.business_Id = entity.business_Id;
                            _entity.classification_Id = item;
                            _entity.createdBy = entity.createdBy;
                            _entity.createdDate = entity.createdDate;
                            _entity.modifiedBy = entity.modifiedBy;
                            _entity.modifiedDate = entity.modifiedDate;
                            model.BusinessClassifications.Add(_entity);
                        }
                    }

                    foreach (var item in ids)
                    {
                        var _entity = model.BusinessClassifications.SingleOrDefault(p => p.classification_Id == item);
                        if (_entity != null)
                        {
                            model.BusinessClassifications.Remove(_entity);
                        }
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

        /// <summary>
        /// This method is to delete the Asset Warehouse details
        /// </summary>
        /// <param name="stockId">AssetSpecification id</param>  
        public bool DeleteBusinessClassification(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    BusinessClassification _entity = model.BusinessClassifications.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.BusinessClassifications.Remove(_entity);
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

        /// <summary>
        /// This method is to get the BusinessClassification by stock id
        /// </summary>
        /// <param name="roleId">BusinessClassification Id</param>
        /// <returns>BusinessClassification details</returns>
        public BusinessClassifications GetBusinessClassificationById(int id)
        {
            BusinessClassifications _entity = new BusinessClassifications();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.BusinessClassifications.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new BusinessClassifications
                        {
                            id = entity.id,
                            business_Id = entity.business_Id,
                            classification_Id = entity.classification_Id
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

        /// <summary>
        /// Get List of BusinessClassifications
        /// </summary> 
        /// <returns></returns>
        public List<BusinessClassifications> GetListBusinessClassification()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Type List
                    //.Where(p => p.visibility)
                    return model.BusinessClassifications.Select(p => new BusinessClassifications
                    {
                        id = p.id,
                        business_Id = p.business_Id,
                        classification_Id = p.classification_Id,
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

        /// <summary>
        /// Get List of BusinessClassifications
        /// </summary> 
        /// <returns></returns>
        public List<BusinessClassifications> GetListBusinessClassificationByBusinessId(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Type List
                    //.Where(p => p.visibility)
                    return model.BusinessClassifications.Where(p => p.business_Id == id).Select(p => new BusinessClassifications
                    {
                        id = p.id,
                        business_Id = p.business_Id,
                        classification_Id = p.classification_Id,
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

        ///// <summary>
        ///// This method is to check the Asset Warehouses exists or not.
        ///// </summary>
        ///// <param name="stockName">Asset Name</param>  
        public bool IsBusinessClassificationExists(BusinessClassifications entity)
        {
            BusinessClassification _entity = new BusinessClassification();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.BusinessClassifications.SingleOrDefault(p => p.classification_Id == entity.classification_Id);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.BusinessClassifications.SingleOrDefault(p => p.classification_Id == entity.classification_Id && p.id != entity.id);
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
