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
using System.ComponentModel;

namespace Hanodale.DataAccessLayer.Services
{
    public class SchedulerSetupService : BaseService, ISchedulerSetupService
    {
        #region SchedulerSetups

        /// <summary>
        /// This method is to get the Module Item details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>SchedulerSetupss list</returns> 

        public SchedulerSetupDetails GetSchedulerSetupBySearch(DatatableFilters entityFilter)
        {
            SchedulerSetupDetails _result = new SchedulerSetupDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();
                     var filter = new SchedulerSetups();


                    //  var query = model.SchedulerSetting.AsNoTracking().Where(p => true);

                    var query = (from ss in model.SchedulerSetting
                                 join mi in model.ModuleItems on ss.syncModule_Id equals mi.id into syncModuleGroup
                                 from mi in syncModuleGroup.DefaultIfEmpty() // Left join
                                 join mi1 in model.ModuleItems on ss.timeSlot equals mi1.id into timeSlotGroup
                                 from mi1 in timeSlotGroup.DefaultIfEmpty() // Left join
                                 join ur in model.Users on ss.createdBy equals ur.id into usrGroup
                                 from ur in usrGroup.DefaultIfEmpty() // Left join
                                 select new
                                 {
                                     SchedulerSetting = ss,
                                     SyncModule = mi,
                                     TimeSlot = mi1,
                                     User = ur
                                 }).ToList();
                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        string searchValue = entityFilter.search.Trim();
                        DateTime parsedDate;
                      //  bool isDate = DateTime.TryParse(searchValue, out parsedDate);
                        bool c = Common.Visibility.True.ToString().ToLower().Contains(entityFilter.search.ToLower());
                        bool d = Common.Visibility.False.ToString().ToLower().Contains(entityFilter.search.ToLower());
                        bool isDate = DateTime.TryParseExact(
                                       searchValue,
                                       "dd/MM/yyyy hh:mm tt", // Adjust format to match DB date format
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None,
                                       out parsedDate
                                   );
                        query = query
                                 .Where(p => (
                                     (p.SyncModule?.name?.ToLower().Contains(entityFilter.search.ToLower()) ?? false)
                                     || (p.TimeSlot?.name?.ToLower().Contains(entityFilter.search.ToLower()) ?? false)
                                     || (p.User?.firstName?.ToLower().Contains(entityFilter.search.ToLower()) ?? false)
                                     || (isDate && p.SchedulerSetting.createdDate.ToString("dd/MM/yyyy hh:mm tt") == parsedDate.ToString("dd/MM/yyyy hh:mm tt"))
                                     || (isDate && p.SchedulerSetting.startDate.HasValue && p.SchedulerSetting.startDate.Value.ToString("dd/MM/yyyy hh:mm tt") == parsedDate.ToString("dd/MM/yyyy hh:mm tt"))
                                     || (c ? (p.SchedulerSetting.isActive ?? false) == true : d ? (p.SchedulerSetting.isActive ?? false) == false : false)
                                 ))
                                 .ToList(); 

                    }

                    // Record counts
                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    // Projecting the results
                    var result = query.OrderByDescending(p => p.SchedulerSetting.id)
                        .Select(p => new SchedulerSetups
                        {
                            id = p.SchedulerSetting.id,
                            SyncModuleName = p.SyncModule?.name ?? "N/A",
                            startDate = p.SchedulerSetting.startDate?.Date,
                            TimeSlotName = p.TimeSlot?.name ?? "N/A",
                            isActive = p.SchedulerSetting.isActive,
                            createByName = p.User?.firstName ?? "Unknown",
                            createdDate = p.SchedulerSetting.createdDate,
                        }).ToList();
                     
                  
                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstSchedulerSetup = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to save the SchedulerSetupss details
        /// </summary> 
        public SchedulerSetups CreateSchedulerSetup(SchedulerSetups entityEn)
        {
            var _schedulersetupEn = new SchedulerSetting(); // SchedulerSetup();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Ship to Address

                    _schedulersetupEn.id = entityEn.id;
                    _schedulersetupEn.syncModule_Id = entityEn.syncModule_Id;
                    _schedulersetupEn.startDate = entityEn.startDate;
                    _schedulersetupEn.timeSlot = entityEn.timeSlot;
                    _schedulersetupEn.isActive = entityEn.isActive;
                    _schedulersetupEn.createdBy = entityEn.createdBy;
                    _schedulersetupEn.createdDate = entityEn.createdDate;


                    model.SchedulerSetting.Add(_schedulersetupEn);
                    model.SaveChanges();

                    entityEn.id = _schedulersetupEn.id;
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
        /// This method is to update the SchedulerSetupss details
        /// </summary> 
        public SchedulerSetups UpdateSchedulerSetup(SchedulerSetups entityEn)
        {
            var _schedulersetupEn = new SchedulerSetting(); //SchedulerSetup();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _schedulersetupEn = model.SchedulerSetting.SingleOrDefault(p => p.id == entityEn.id);
                    if (_schedulersetupEn != null)
                    {

                        _schedulersetupEn.id = entityEn.id;
                        _schedulersetupEn.syncModule_Id = entityEn.syncModule_Id;
                        _schedulersetupEn.startDate = entityEn.startDate;
                        _schedulersetupEn.timeSlot = entityEn.timeSlot;
                        _schedulersetupEn.isActive = entityEn.isActive;
                        _schedulersetupEn.createdBy = entityEn.createdBy;
                        _schedulersetupEn.createdDate = entityEn.createdDate;

                       // model.SchedulerSetting.Add(_schedulersetupEn);

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
        public bool DeleteSchedulerSetup(int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _companyStatutoryEn = model.SchedulerSetting.SingleOrDefault(p => p.id == id);

                    if (_companyStatutoryEn != null)
                    {
                        model.SchedulerSetting.Remove(_companyStatutoryEn);
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
        /// This method is to get the SchedulerSetupss by SchedulerSetupss id
        /// </summary>
        /// <param name="roleId">SchedulerSetupss Id</param>
        /// <returns>SchedulerSetupss details</returns>
        public SchedulerSetups GetSchedulerSetupById(int id)
        {
            var _schedulersetupEn = new SchedulerSetups();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.SchedulerSetting.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _schedulersetupEn = new SchedulerSetups
                        {
                            id = entity.id,
                            syncModule_Id = entity.syncModule_Id,
                            startDate = entity.startDate,
                            timeSlot = entity.timeSlot,
                            isActive = entity.isActive,
                            createdBy = entity.createdBy,
                            createdDate = entity.createdDate,



                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _schedulersetupEn;
        }

        ///// <summary>
        ///// This method is to check the SchedulerSetupss exists or not.
        ///// </summary>
        ///// <param name="stockName">SchedulerSetups Name</param>  
        public bool IsSchedulerSetupExists(SchedulerSetups entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.SchedulerSetting.Any(p => p.syncModule_Id == entityEn.syncModule_Id && p.timeSlot == entityEn.timeSlot && (entityEn.id == 0 ? true : p.id != entityEn.id));
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
