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
    public class BusinessFileService : IBusinessFileService
    {
        #region BusinessFile

        /// <summary>
        /// This method is to get the BusinessFile details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessFileDetails GetBusinessFileBySearch(int currentUserId, int userId, int businessId, int startIndex, int pageSize, string search)
        {
            BusinessFileDetails _result = new BusinessFileDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessFiles.Where(p => p.business_Id == businessId).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    var result = model.BusinessFiles
                                .OrderByDescending(a => a.modifiedDate)
                                .Where(a => (a.name.Contains(search)
                                     || ((a.modifiedDate.Value.Day < 10 ? "0" : "") + SqlFunctions.StringConvert((double)a.modifiedDate.Value.Day).Trim() + "/" + (a.modifiedDate.Value.Month < 10 ? "0" : "") + (SqlFunctions.StringConvert((double)a.modifiedDate.Value.Month)).Trim() + "/" + (SqlFunctions.StringConvert((double)a.modifiedDate.Value.Year)).Trim()).Contains(search)
                                     || a.ModuleItem.name.Contains(search)
                                     || a.description.Contains(search))
                                       && a.business_Id == businessId)
                                     .Select(p => new BusinessFiles
                                     {
                                         id = p.id,
                                         business_Id = p.business_Id,
                                         fileType_Id = p.fileType_Id,
                                         fileTypeName = p.ModuleItem.name,
                                         name = p.name,
                                         urlPath = p.urlPath,
                                         description = p.description,
                                         createdBy = p.createdBy,
                                         createdDate = p.createdDate,
                                         modifiedBy = p.modifiedBy,
                                         modifiedDate = p.modifiedDate
                                     }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstBusinessFile = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the BusinessFile details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessFileDetails GetBusinessFile(int currentUserId, int userId, int businessId, int startIndex, int pageSize)
        {
            BusinessFileDetails _result = new BusinessFileDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessFiles.Where(p => p.business_Id == businessId).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    _result.lstBusinessFile = model.BusinessFiles
                                      .OrderByDescending(a => a.modifiedDate)
                                      .Where(p => p.business_Id == businessId)
                                      .Skip(startIndex).Take(pageSize)
                                      .Select(p => new BusinessFiles
                                      {
                                          id = p.id,
                                          business_Id = p.business_Id,
                                          fileType_Id = p.fileType_Id,
                                          fileTypeName = p.ModuleItem.name,
                                          name = p.name,
                                          urlPath = p.urlPath,
                                          description = p.description,
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
        /// This method is to save the BusinessFile details
        /// </summary> 
        public BusinessFiles CreateBusinessFile(int currentUserId, BusinessFiles entity, string pageName)
        {
            BusinessFile _entity = new BusinessFile();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    //Add new BusinessFile
                    _entity.business_Id = entity.business_Id;
                    _entity.fileType_Id = entity.fileType_Id;
                    _entity.name = entity.name;
                    _entity.urlPath = entity.urlPath;
                    _entity.description = entity.description;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;

                    model.BusinessFiles.Add(_entity);
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

        /// <summary>
        /// This method is to update the BusinessFile details
        /// </summary> 
        public BusinessFiles UpdateBusinessFile(int currentUserId, BusinessFiles entity, string pageName)
        {
            BusinessFile _entity = new BusinessFile();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update BusinessFile
                    _entity = model.BusinessFiles.SingleOrDefault(p => p.id == entity.id);
                    if (_entity != null)
                    {
                        _entity.business_Id = entity.business_Id;
                        _entity.fileType_Id = entity.fileType_Id;
                        _entity.name = entity.name;
                        if (!string.IsNullOrEmpty(entity.urlPath))
                            _entity.urlPath = entity.urlPath;
                        _entity.description = entity.description;
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

        /// <summary>
        /// This method is to delete the BusinessFile details
        /// </summary>
        /// <param name="stockId">BusinessFile id</param>  
        public bool DeleteBusinessFile(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    BusinessFile _entity = model.BusinessFiles.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.BusinessFiles.Remove(_entity);
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
        /// This method is to get the BusinessFile by stock id
        /// </summary>
        /// <param name="assetId">BusinessFile Id</param>
        /// <returns>BusinessFile details</returns>
        public BusinessFiles GetBusinessFileById(int id)
        {
            BusinessFiles _entity = new BusinessFiles();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.BusinessFiles.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new BusinessFiles
                        {
                            id = entity.id,
                            business_Id = entity.business_Id,
                            fileType_Id = entity.fileType_Id,
                            name = entity.name,
                            urlPath = entity.urlPath,
                            description = entity.description,
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

        /// <summary>
        /// Get List of BusinessFiles
        /// </summary> 
        /// <returns></returns>
        public List<BusinessFiles> GetListBusinessFile()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Type List
                    //.Where(p => p.visibility)
                    return model.BusinessFiles.Select(p => new BusinessFiles
                    {
                        id = p.id,
                        business_Id = p.business_Id,
                        fileType_Id = p.fileType_Id,
                        name = p.name,
                        urlPath = p.urlPath,
                        description = p.description,
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
        ///// This method is to check the BusinessFiles exists or not.
        ///// </summary>
        ///// <param name="Asset">Asset</param>  
        public bool IsBusinessFileExists(BusinessFiles entity)
        {
            BusinessFile _entity = new BusinessFile();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.BusinessFiles.SingleOrDefault(p => p.name == entity.name);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.BusinessFiles.SingleOrDefault(p => p.name == entity.name && p.id != entity.id);
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
