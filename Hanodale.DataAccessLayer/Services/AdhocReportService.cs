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
    public class AdhocReportService : IAdhocReportService
    {
        #region AdhocReport

        /// <summary>
        /// This method is to get the AdhocReport details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public AdhocReportDetails GetAdhocReportBySearch(int currentUserId, int organization_Id, int startIndex, int pageSize, string search)
        {
            AdhocReportDetails _result = new AdhocReportDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    bool c, d;
                    c = Common.Visibility.True.ToString().ToLower().Contains(search.ToLower()); d = Common.Visibility.False.ToString().ToLower().Contains(search.ToLower());
                    //get total record
                    _result.recordDetails.totalRecords = model.AdhocReports.Where(p => p.organization_Id == organization_Id).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    var result = (from p in model.AdhocReports
                                  join md in model.ModuleItems on p.reportType_Id equals md.id
                                  where (md.name.Contains(search)
                                    || p.reportName.Contains(search)
                                    || (c ? p.isVisible == true : d ? p.isVisible == false : false)
                                    && p.organization_Id == organization_Id)
                                  orderby p.modifiedBy
                                  select new AdhocReports
                                  {
                                      id = p.id,
                                      organization_Id = p.organization_Id,
                                      reportType_Id = p.reportType_Id,
                                      reportName = p.reportName,
                                      reportFileName = p.reportFileName,
                                      remarks = p.remarks,
                                      isCommon = p.isCommon,
                                      isVisible = p.isVisible,
                                      createdBy = md.name,
                                      createdDate = p.createdDate,
                                      modifiedBy = p.modifiedBy,
                                      modifiedDate = p.modifiedDate
                                  }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstAdhocReport = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the AdhocReport details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public AdhocReportDetails GetAdhocReport(int currentUserId, int organization_Id, int startIndex, int pageSize)
        {
            AdhocReportDetails _result = new AdhocReportDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.AdhocReports.Where(p => p.organization_Id == organization_Id).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;



                    _result.lstAdhocReport = model.AdhocReports.Where(p => p.organization_Id == organization_Id)
                                                  .OrderByDescending(p => p.modifiedDate)
                                                  .Skip(startIndex).Take(pageSize).
                                              Select(p => new AdhocReports
                                  {
                                      id = p.id,
                                      organization_Id = p.organization_Id,
                                      reportType_Id = p.reportType_Id,
                                      reportName = p.reportName,
                                      reportFileName = p.reportFileName,
                                      remarks = p.remarks,
                                      isCommon = p.isCommon,
                                      isVisible = p.isVisible,
                                                  createdBy = p.ModuleItem.name,
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
        /// This method is to save the Purchase Order details
        /// </summary> 
        public AdhocReports CreateAdhocReport(int currentUserId, AdhocReports entity, string pageName)
        {
            AdhocReport _entity = new AdhocReport();


            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    //Add new Purchase Order 
                    _entity.organization_Id = entity.organization_Id;
                    _entity.reportType_Id = entity.reportType_Id;
                    _entity.reportName = entity.reportName;
                    _entity.reportFileName = entity.reportFileName;
                    _entity.remarks = entity.remarks;
                    _entity.isCommon = entity.isCommon;
                    _entity.isVisible = entity.isVisible;
                    _entity.remarks = entity.remarks;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;
                    model.AdhocReports.Add(_entity);
                    model.SaveChanges();

                }
                entity.id = _entity.id;
            }

            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        /// <summary>
        /// This method is to update the Purchase Order details
        /// </summary> 
        public AdhocReports UpdateAdhocReport(int currentUserId, AdhocReports entity, string pageName)
        {
            AdhocReport _entity = new AdhocReport();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update AdhocReport
                    _entity = model.AdhocReports.SingleOrDefault(p => p.id == entity.id);
                    if (_entity != null)
                    {
                        _entity.reportType_Id = entity.reportType_Id;
                        _entity.reportName = entity.reportName;
                        if (!string.IsNullOrEmpty(entity.reportFileName))
                            _entity.reportFileName = entity.reportFileName;
                        _entity.remarks = entity.remarks;
                        _entity.isCommon = entity.isCommon;
                        _entity.isVisible = entity.isVisible;
                        _entity.remarks = entity.remarks;
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
        /// This method is to delete the AdhocReport details
        /// </summary>
        /// <param name="stockId">AdhocReport id</param>  
        public bool DeleteAdhocReport(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    AdhocReport _entity = model.AdhocReports.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.AdhocReports.Remove(_entity);
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
        /// This method is to get the AdhocReport by stock id
        /// </summary>
        /// <param name="assetId">AdhocReport Id</param>
        /// <returns>AdhocReport details</returns>
        public AdhocReports GetAdhocReportById(int id)
        {
            AdhocReports _entity = new AdhocReports();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.AdhocReports.SingleOrDefault(p => p.id == id);


                    if (entity != null)
                    {
                        _entity = new AdhocReports
                        {
                            id = entity.id,
                            organization_Id = entity.organization_Id,
                            reportType_Id = entity.reportType_Id,
                            reportName = entity.reportName,
                            reportFileName = entity.reportFileName,
                            remarks = entity.remarks,
                            isCommon = entity.isCommon,
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

        /// <summary>
        /// Get List of AdhocReports
        /// </summary> 
        /// <returns></returns>
        public List<AdhocReports> GetListAdhocReport()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.AdhocReports.Select(p => new AdhocReports
                    {
                        id = p.id,
                        organization_Id = p.organization_Id,
                        reportType_Id = p.reportType_Id,
                        reportName = p.reportName,
                        reportFileName = p.reportFileName,
                        remarks = p.remarks,
                        isCommon = p.isCommon,
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

        ///// <summary>
        ///// This method is to check the AdhocReports exists or not.
        ///// </summary>
        ///// <param name="Asset">Asset</param>  
        public int IsAdhocReportExists(AdhocReports entity)
        {
            AdhocReport _entity = new AdhocReport();
            bool isExists = false;
            int value = 0;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.AdhocReports.Where(p => p.reportName == entity.reportName && entity.reportType_Id == entity.reportType_Id).FirstOrDefault();
                        var fileExist = model.AdhocReports.Where(p => p.reportFileName == entity.reportFileName && p.reportType_Id == entity.reportType_Id).FirstOrDefault();
                        if (_entity != null)
                        {
                            value = 1;
                        }
                        else if (fileExist != null)
                        {
                            value = 2;
                        }
                    }
                    else
                    {
                        _entity = model.AdhocReports.Where(p => p.reportName == entity.reportName && entity.reportType_Id == entity.reportType_Id && p.id!=entity.id).FirstOrDefault();
                        var fileExist = model.AdhocReports.Where(p => p.reportFileName == entity.reportFileName && p.reportType_Id == entity.reportType_Id && p.id != entity.id).FirstOrDefault();
                        if (_entity != null)
                        {
                            value = 1;
                        }
                        else if (fileExist != null)
                        {
                            value = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return value;
        }

        #endregion
    }
}
