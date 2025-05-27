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
    public class BusinessOthersService : IBusinessOthersService
    {
        #region BusinessOthers

        /// <summary>
        /// This method is to get the BusinessOthers details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessOthersDetails GetBusinessOthersBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            BusinessOthersDetails _result = new BusinessOthersDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessOthers.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    var result = model.BusinessOthers
                                .OrderBy(a => a.modifiedDate)
                                .Where(a => a.Business.name.ToString().Contains(search))
                                     .Select(p => new BusinessOtherss
                                     {
                                         id=p.id,
                                         business_Id = p.business_Id,
                                         bumiShare = p.bumiShare,
                                         nonBumiShare = p.nonBumiShare,
                                         foreignShare = p.foreignShare,
                                         bumiCapital = p.bumiCapital,
                                         classA = p.classA,
                                         classB = p.classB,
                                         classBX = p.classBX,
                                         classC = p.classC,
                                         classD = p.classD,
                                         classE = p.classE,
                                         classEX = p.classEX,
                                         classF = p.classF,
                                         pkk = p.pkk,
                                         tnb = p.tnb,
                                         jba = p.jba,
                                         jkr = p.jkr,
                                         dbkl = p.dbkl,
                                         financeMinistry = p.financeMinistry,
                                         jkh = p.jkh,
                                         mara = p.mara,
                                         createdBy = p.createdBy,
                                         createdDate = p.createdDate,
                                         modifiedBy = p.modifiedBy,
                                         modifiedDate = p.modifiedDate
                                     }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstBusinessOther = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the BusinessOthers details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessOthersDetails GetBusinessOthers(int currentUserId, int userId, int startIndex, int pageSize)
        {
            BusinessOthersDetails _result = new BusinessOthersDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessOthers.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    _result.lstBusinessOther = model.BusinessOthers
                                      .OrderBy(a => a.modifiedDate)
                                      .Skip(startIndex).Take(pageSize)
                                      .Select(p => new BusinessOtherss
                                      {
                                          id = p.id,
                                          business_Id = p.business_Id,
                                          bumiShare = p.bumiShare,
                                          nonBumiShare = p.nonBumiShare,
                                          foreignShare = p.foreignShare,
                                          bumiCapital = p.bumiCapital,
                                          classA = p.classA,
                                          classB = p.classB,
                                          classBX = p.classBX,
                                          classC = p.classC,
                                          classD = p.classD,
                                          classE = p.classE,
                                          classEX = p.classEX,
                                          classF = p.classF,
                                          pkk = p.pkk,
                                          tnb = p.tnb,
                                          jba = p.jba,
                                          jkr = p.jkr,
                                          dbkl = p.dbkl,
                                          financeMinistry = p.financeMinistry,
                                          jkh = p.jkh,
                                          mara = p.mara,
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
        /// This method is to save the BusinessOthers details
        /// </summary> 
        public BusinessOtherss CreateBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName)
        {
            BusinessOther _entity = new BusinessOther();
            Business _entitys = new Business();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    //Add new BusinessOthers
                    _entity.business_Id = entity.business_Id;
                    _entity.bumiShare = entity.bumiShare;
                    _entity.nonBumiShare = entity.nonBumiShare;
                    _entity.foreignShare = entity.foreignShare;
                    _entity.bumiCapital = entity.bumiCapital;
                    _entity.classA = entity.classA;
                    _entity.classB = entity.classB;
                    _entity.classBX = entity.classBX;
                    _entity.classC = entity.classC;
                    _entity.classD = entity.classD;
                    _entity.classE = entity.classE;
                    _entity.classEX = entity.classEX;
                    _entity.classF = entity.classF;
                    _entity.pkk = entity.pkk;
                    _entity.tnb = entity.tnb;
                    _entity.jba = entity.jba;
                    _entity.jkr = entity.jkr;
                    _entity.dbkl = entity.dbkl;
                    _entity.financeMinistry = entity.financeMinistry;
                    _entity.jkh = entity.jkh;
                    _entity.mara = entity.mara;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;

                    model.BusinessOthers.Add(_entity);
                    model.SaveChanges();

                    _entitys = model.Businesses.Where(p => p.id == entity.business_Id).FirstOrDefault();
                    if (_entitys != null)
                    {
                        _entitys.paidUpCapital = entity.paidUpCapital;
                        _entitys.businessCategory = entity.businessCategory;
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
        /// This method is to update the BusinessOthers details
        /// </summary> 
        public BusinessOtherss UpdateBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName)
        {
            BusinessOther _entity = new BusinessOther();
            Business _entitys = new Business();

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update BusinessOthers
                    _entity = model.BusinessOthers.SingleOrDefault(p => p.id == entity.id);
                    if (_entity != null)
                    {
                        _entity.business_Id = entity.business_Id;
                        _entity.bumiShare = entity.bumiShare;
                        _entity.nonBumiShare = entity.nonBumiShare;
                        _entity.foreignShare = entity.foreignShare;
                        _entity.bumiCapital = entity.bumiCapital;
                        _entity.classA = entity.classA;
                        _entity.classB = entity.classB;
                        _entity.classBX = entity.classBX;
                        _entity.classC = entity.classC;
                        _entity.classD = entity.classD;
                        _entity.classE = entity.classE;
                        _entity.classEX = entity.classEX;
                        _entity.classF = entity.classF;
                        _entity.pkk = entity.pkk;
                        _entity.tnb = entity.tnb;
                        _entity.jba = entity.jba;
                        _entity.dbkl = entity.dbkl;
                        _entity.jkr = entity.jkr;
                        _entity.mara = entity.mara;
                        _entity.financeMinistry = entity.financeMinistry;
                        _entity.jkh = entity.jkh;
                        _entity.modifiedBy = entity.modifiedBy;
                        _entity.modifiedDate = entity.modifiedDate;

                    }
                    _entitys = model.Businesses.Where(p => p.id == entity.business_Id).FirstOrDefault();
                    if (_entitys != null)
                    {
                        _entitys.paidUpCapital = entity.paidUpCapital;
                        _entitys.businessCategory = entity.businessCategory;

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
        /// This method is to delete the BusinessOthers details
        /// </summary>
        /// <param name="stockId">BusinessOthers id</param>  
        public bool DeleteBusinessOthers(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _entity = model.BusinessOthers.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.BusinessOthers.Remove(_entity);
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
        /// This method is to get the BusinessOthers by stock id
        /// </summary>
        /// <param name="assetId">BusinessOthers Id</param>
        /// <returns>BusinessOthers details</returns>
        public BusinessOtherss GetBusinessOthersById(int id)
        {
            BusinessOtherss _entity = new BusinessOtherss();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.BusinessOthers.SingleOrDefault(p => p.business_Id == id);

                    var business = model.Businesses.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new BusinessOtherss
                        {
                            id = entity.id,
                            business_Id = entity.business_Id,
                            bumiCapital = entity.bumiCapital,
                            bumiShare = entity.bumiShare,
                            nonBumiShare = entity.nonBumiShare,
                            foreignShare = entity.foreignShare,
                            classA = entity.classA,
                            classB = entity.classB,
                            classBX = entity.classBX,
                            classC = entity.classC,
                            classD = entity.classD,
                            classE = entity.classE,
                            classEX = entity.classEX,
                            classF = entity.classF,
                            pkk = entity.pkk,
                            tnb = entity.tnb,
                            jba = entity.jba,
                            dbkl = entity.dbkl,
                            jkr = entity.jkr,
                            mara = entity.mara,
                            financeMinistry = entity.financeMinistry,
                            jkh = entity.jkh,
                            businessCategory = business.businessCategory,
                            paidUpCapital = business.paidUpCapital,
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
        /// Get List of BusinessOtherss
        /// </summary> 
        /// <returns></returns>
        public List<BusinessOtherss> GetListBusinessOthers()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Type List
                    //.Where(p => p.visibility)
                    return model.BusinessOthers.Select(p => new BusinessOtherss
                    {
                        business_Id = p.business_Id,
                        bumiCapital = p.bumiCapital,
                        bumiShare = p.bumiShare,
                        nonBumiShare = p.nonBumiShare,
                        foreignShare = p.foreignShare,
                        classA = p.classA,
                        classB = p.classB,
                        classBX = p.classBX,
                        classC = p.classC,
                        classD = p.classD,
                        classE = p.classE,
                        classEX = p.classEX,
                        classF = p.classF,
                        pkk = p.pkk,
                        tnb = p.tnb,
                        jba = p.jba,
                        dbkl = p.dbkl,
                        jkr = p.jkr,
                        financeMinistry = p.financeMinistry,
                        jkh = p.jkh,
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
        ///// This method is to check the BusinessOtherss exists or not.
        ///// </summary>
        ///// <param name="Asset">Asset</param>  
        public bool IsBusinessOthersExists(BusinessOtherss entity)
        {
            BusinessOther _entity = new BusinessOther();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.BusinessOthers.SingleOrDefault(p => p.bumiShare == entity.bumiShare);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.BusinessOthers.SingleOrDefault(p => p.bumiShare == entity.bumiShare && p.id != entity.id);
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
