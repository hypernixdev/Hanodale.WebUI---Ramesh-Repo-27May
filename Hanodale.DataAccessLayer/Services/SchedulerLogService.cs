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
    public class SchedulerLogService : BaseService, ISchedulerLogService
    {
        #region SchedulerLogs

        /// <summary>
        /// This method is to get the Module Item details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>SchedulerLogss list</returns> 

        public SchedulerLogDetails GetSchedulerLogBySearch(DatatableFilters entityFilter)
        {
            SchedulerLogDetails _result = new SchedulerLogDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();
                     var filter = new SchedulerLogs();


                      var query = model.SchedulerLog.AsNoTracking().Where(p => p.schedulerSetting_Id == entityFilter.masterRecord_Id);

                   

                    // Record counts
                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    // Projecting the results
                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new SchedulerLogs
                        {
                            id = p.id,
                            startDateTime = p.startDateTime,
                            endDateTime = p.endDateTime,
                            result = p.result,
                            totalRecordProcessed = p.totalRecordProcessed ?? 0,
                            errorMessage = p.errorMessage,
                        }).ToList();


                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstSchedulerLog = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to save the SchedulerLogss details
        /// </summary> 
        public SchedulerLogs CreateSchedulerLog(SchedulerLogs entityEn)
        {
            var _schedulerLogEn = new SchedulerLog(); // SchedulerLog();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Ship to Address

                    _schedulerLogEn.id = entityEn.id;
                   // _schedulerLogEn.syncModule_Id = entityEn.syncModule_Id;
                   
                    model.SchedulerLog.Add(_schedulerLogEn);
                    model.SaveChanges();

                    entityEn.id = _schedulerLogEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        /// <summary>
        /// This method is to update the SchedulerLogss details
        /// </summary> 
        public SchedulerLogs UpdateSchedulerLog(SchedulerLogs entityEn)
        {
            var _schedulerLogEn = new SchedulerLog(); //SchedulerLog();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _schedulerLogEn = model.SchedulerLog.SingleOrDefault(p => p.id == entityEn.id);
                    if (_schedulerLogEn != null)
                    {

                        _schedulerLogEn.id = entityEn.id;
                        //_schedulerLogEn.syncModule_Id = entityEn.syncModule_Id;
                        //_schedulerLogEn.startDate = entityEn.startDate;
                        //_schedulerLogEn.timeSlot = entityEn.timeSlot;
                        //_schedulerLogEn.isActive = entityEn.isActive;
                        //_schedulerLogEn.createdBy = entityEn.createdBy;
                        //_schedulerLogEn.createdDate = entityEn.createdDate;

                        model.SchedulerLog.Add(_schedulerLogEn);

                    }
                    model.SaveChanges();
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        /// <summary>
        /// This method is to delete the stock details
        /// </summary>
        /// <param name="stockId">stock id</param>  
        public bool DeleteSchedulerLog(int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _companyStatutoryEn = model.SchedulerLog.SingleOrDefault(p => p.id == id);

                    if (_companyStatutoryEn != null)
                    {
                        model.SchedulerLog.Remove(_companyStatutoryEn);
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
        /// This method is to get the SchedulerLogss by SchedulerLogss id
        /// </summary>
        /// <param name="roleId">SchedulerLogss Id</param>
        /// <returns>SchedulerLogss details</returns>
        public SchedulerLogs GetSchedulerLogById(int id)
        {
            var _schedulerLogEn = new SchedulerLogs();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.SchedulerLog.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _schedulerLogEn = new SchedulerLogs
                        {
                            id = entity.id,
                            //syncModule_Id = entity.syncModule_Id,
                            //startDate = entity.startDate,
                            //timeSlot = entity.timeSlot,
                            //isActive = entity.isActive,
                            //createdBy = entity.createdBy,
                            //createdDate = entity.createdDate,



                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _schedulerLogEn;
        }

        ///// <summary>
        ///// This method is to check the SchedulerLogss exists or not.
        ///// </summary>
        ///// <param name="stockName">SchedulerLogs Name</param>  
        public bool IsSchedulerLogExists(SchedulerLogs entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.SchedulerLog.Any(p => p.schedulerSetting_Id == entityEn.schedulerSetting_Id && (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        #endregion
    }
}
