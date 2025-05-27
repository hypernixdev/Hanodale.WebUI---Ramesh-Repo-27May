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
    public class NewsService : INewsService
    {
        #region News

        /// <summary>
        /// This method is to get the News details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public NewsDetails GetNewsBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search, object filterModel)
        {
            NewsDetails _result = new NewsDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //var filter = new Newss();
                    //bool hasFilter = false;
                    //if (filterModel != null)
                    //{
                    //    filter = (Newss)filterModel;
                    //    hasFilter = true;
                    //}

                    var filter = new DateTime();
                    bool hasFilter = false;
                    if (filterModel != null)
                    {
                        filter = (DateTime)filterModel;
                        hasFilter = true;
                    }
                    
                    //get total record
                    _result.recordDetails.totalRecords = model.News.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                    var result = model.News
                                .OrderByDescending(a => a.modifiedDate)
                                .Where(a => (hasFilter?(a.loggedDate>=filter):true) && string.IsNullOrEmpty(search) ? true : (a.description.Contains(search)
                                     || a.loggedBy.Contains(search)
                                     || ((a.loggedDate.Value.Day < 10 ? "0" : "") + SqlFunctions.StringConvert((double)a.loggedDate.Value.Day).Trim() + "/" + (a.loggedDate.Value.Month < 10 ? "0" : "") + (SqlFunctions.StringConvert((double)a.loggedDate.Value.Month)).Trim() + "/" + (SqlFunctions.StringConvert((double)a.loggedDate.Value.Year)).Trim()).Contains(search)
                                     ))
                                     .Select(p => new Newss
                                     {
                                         id = p.id,
                                         description = p.description,
                                         loggedBy = p.loggedBy,
                                         loggedDate = p.loggedDate,
                                         createdBy = p.createdBy,
                                         createdDate = p.createdDate,
                                         modifiedBy = p.modifiedBy,
                                         modifiedDate = p.modifiedDate
                                     }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstNews = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the News details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public NewsDetails GetNews(int currentUserId, bool all, int userId, int startIndex, int pageSize)
        {
            NewsDetails _result = new NewsDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    if (all == true)
                    {
                        //get total record
                        _result.recordDetails.totalRecords = model.News.Where(p => p.id == 1).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                        _result.lstNews = model.News
                                          .Where(p => p.id == 1)
                                          .OrderByDescending(a => a.modifiedDate)
                                          .Skip(startIndex).Take(pageSize)
                                          .Select(p => new Newss
                                          {
                                              id = p.id,
                                              description = p.description,
                                              loggedBy = p.loggedBy,
                                              loggedDate = p.loggedDate,
                                              createdBy = p.createdBy,
                                              createdDate = p.createdDate,
                                              modifiedBy = p.modifiedBy,
                                              modifiedDate = p.modifiedDate
                                          }).ToList();
                    }

                    else
                    {
                        _result.recordDetails.totalRecords = model.News.Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                        _result.lstNews = model.News
                                          .OrderByDescending(a => a.modifiedDate)
                                          .Skip(startIndex).Take(pageSize)
                                          .Select(p => new Newss
                                          {
                                              id = p.id,
                                              description = p.description,
                                              loggedBy = p.loggedBy,
                                              loggedDate = p.loggedDate,
                                              createdBy = p.createdBy,
                                              createdDate = p.createdDate,
                                              modifiedBy = p.modifiedBy,
                                              modifiedDate = p.modifiedDate
                                          }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to save the News details
        /// </summary> 
        public Newss CreateNews(int currentUserId, Newss entity, string pageName)
        {
            News _entity = new News();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    //Add new News
                    _entity.description = entity.description;
                    _entity.loggedBy = entity.loggedBy;
                    _entity.loggedDate = entity.loggedDate;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;

                    model.News.Add(_entity);
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
        /// This method is to update the News details
        /// </summary> 
        public Newss UpdateNews(int currentUserId, Newss entity, string pageName)
        {
            News _entity = new News();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update News
                    _entity = model.News.SingleOrDefault(p => p.id == entity.id);
                    if (_entity != null)
                    {
                        _entity.description = entity.description;
                        _entity.loggedBy = entity.loggedBy;
                        _entity.loggedDate = entity.loggedDate;
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
        /// This method is to delete the News details
        /// </summary>
        /// <param name="stockId">News id</param>  
        public bool DeleteNews(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    News _entity = model.News.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.News.Remove(_entity);
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
        /// This method is to get the News by stock id
        /// </summary>
        /// <param name="assetId">News Id</param>
        /// <returns>News details</returns>
        public Newss GetNewsById(int id)
        {
            Newss _entity = new Newss();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.News.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new Newss
                        {
                            id = entity.id,
                            description = entity.description,
                            loggedBy = entity.loggedBy,
                            loggedDate = entity.loggedDate,
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
        /// Get List of Newss
        /// </summary> 
        /// <returns></returns>
        public List<Newss> GetListNews()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    return model.News.Select(p => new Newss
                    {

                        id = p.id,
                        description = p.description,
                        loggedBy = p.loggedBy,
                        loggedDate = p.loggedDate,
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
        ///// This method is to check the Newss exists or not.
        ///// </summary>
        ///// <param name="Asset">Asset</param>  
        public bool IsNewsExists(Newss entity)
        {
            News _entity = new News();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.News.SingleOrDefault(p => p.id == entity.id);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.News.SingleOrDefault(p => p.id == entity.id && p.id != entity.id);
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
