using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System.ServiceModel;
namespace Hanodale.DataAccessLayer.Services
{
    public class CalendarSettingService : ICalendarSettingService
    {

        public CalendarSettingDetails GetCalendarSetting(int currentUserId, int organization_Id, int startIndex, int pageSize, string search)
        {
            CalendarSettingDetails _result = new CalendarSettingDetails();
            _result.recordDetails = new RecordDetails();

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    var obj = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (obj != null)
                    {
                        _result.recordDetails.totalRecords = model.CalendarSettings.Where(p => p.CalendarEvent.organization_Id == obj.parent_Id).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                        var lst = model.CalendarSettings.Where(p => p.CalendarEvent.organization_Id == obj.parent_Id && p.StartDate.Year.ToString() == search)
                                        .OrderBy(p => p.StartDate).Skip(startIndex).Take(pageSize).Select(p => new CalendarSettings
                                        {
                                            id = p.id,
                                            calendarEvent_Id = p.calendarEvent_Id,
                                            calendarEventTitle = p.CalendarEvent.title,
                                            calendarEventDescription = p.CalendarEvent.description,
                                            StartDate = p.StartDate,
                                            EndDate = p.EndDate,
                                            icon = p.CalendarEvent.icon,
                                            color = p.CalendarEvent.eventColor,
                                            createdBy = p.createdBy,
                                            createdDate = p.createdDate
                                        }).ToList();


                        _result.lstCalendarSettings = lst.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public List<CalendarSettings> GetCalendarItem(int currentUserId, int organization_Id, int year)
        {

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    var obj = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (obj != null)
                    {
                        var lstCalendarSetting = model.CalendarSettings.Where(p => p.CalendarEvent.organization_Id == obj.parent_Id && p.StartDate.Year == year && p.CalendarEvent.visibility)
                                        .OrderBy(p => p.StartDate).Select(p => new CalendarSettings
                                        {
                                            id = p.id,
                                            title = p.CalendarEvent.title,
                                            description = p.CalendarEvent.description,
                                            calendarEvent_Id = p.calendarEvent_Id,
                                            calendarEventTitle = p.CalendarEvent.title,
                                            calendarEventDescription = p.CalendarEvent.description,
                                            StartDate = p.StartDate,
                                            EndDate = p.EndDate,
                                            icon = p.CalendarEvent.icon,
                                            color = p.CalendarEvent.eventColor,
                                            allowToSelect=p.CalendarEvent.allowToSelect,
                                            createdBy = p.createdBy,
                                            createdDate = p.createdDate
                                        }).ToList();

                        return lstCalendarSetting;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool SaveCalendarSetting(int currentUserId, List<CalendarSettings> lst, int organization_Id, int year)
        {
            CalendarSetting _calendarSettingEn;

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var objOrg = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (objOrg != null)
                    {
                        var newIds = lst.Select(p => p.id).ToList();
                        var list = model.CalendarSettings.Where(p => p.CalendarEvent.organization_Id == objOrg.parent_Id && p.StartDate.Year == year);
                        var removeItem = list.Where(p => !newIds.Contains(p.id));
                        foreach (var item in removeItem)
                        {
                            model.CalendarSettings.Remove(item);
                            //list.Remove(item);
                        }

                        foreach (var item in lst.Where(p => p.id != 0))
                        {
                            var obj = list.SingleOrDefault(p => p.id == item.id);
                            if (obj != null)
                            {
                                obj.StartDate = item.StartDate;
                                obj.EndDate = item.EndDate;
                                obj.modifiedBy = item.modifiedBy;
                                obj.modifiedDate = item.modifiedDate;
                            }
                        }

                        foreach (var items in lst.Where(p => p.id == 0))
                        {
                            _calendarSettingEn = new CalendarSetting();
                            //Add new stock
                            //_calendarSettingEn.id = items.id;
                            _calendarSettingEn.calendarEvent_Id = items.calendarEvent_Id;
                            _calendarSettingEn.StartDate = items.StartDate;
                            _calendarSettingEn.EndDate = items.EndDate;
                            _calendarSettingEn.createdBy = items.createdBy;
                            _calendarSettingEn.createdDate = items.createdDate;
                            // _calendarSettingEn.modifiedBy = items.modifiedBy;
                            //_calendarSettingEn.modifiedDate = items.modifiedDate;
                            model.CalendarSettings.Add(_calendarSettingEn);

                        }

                        model.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return true;
        }

        public CalendarSettings UpdateCalendarSetting(int currentUserId, CalendarSettings entity, string pageName)
        {
            CalendarSetting _calendarSettingEn = new CalendarSetting();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _calendarSettingEn = model.CalendarSettings.SingleOrDefault(p => p.id == entity.id);
                    if (_calendarSettingEn != null)
                    {
                        _calendarSettingEn.calendarEvent_Id = entity.calendarEvent_Id;
                        _calendarSettingEn.StartDate = entity.StartDate;
                        _calendarSettingEn.EndDate = entity.EndDate;
                        _calendarSettingEn.modifiedBy = entity.modifiedBy;
                        _calendarSettingEn.modifiedDate = entity.modifiedDate;

                        model.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        public bool DeleteCalendarSetting(int organization_Id, int year)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    var obj = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (obj != null)
                    {
                        var _calendarSettingEn = model.CalendarSettings.Where(p => p.CalendarEvent.organization_Id == obj.parent_Id && p.StartDate.Year == year);

                        foreach (var item in _calendarSettingEn)
                        {
                            model.CalendarSettings.Remove(item);
                        }

                        model.SaveChanges();
                        isDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isDeleted;
        }

        /// <summary>
        /// Fetches the CalendarSetting based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CalendarSettings GetCalendarSettingById(int id)
        {
            CalendarSettings _stockEn = new CalendarSettings();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var entity = model.CalendarSettings.SingleOrDefault(p => p.id == id);

                    if (entity != null)
                    {
                        _stockEn = new CalendarSettings
                        {
                            id = entity.id,
                            calendarEvent_Id = entity.calendarEvent_Id,
                            StartDate = entity.StartDate,
                            EndDate = entity.EndDate,
                            icon = entity.CalendarEvent.icon,
                            color = entity.CalendarEvent.eventColor,
                            createdBy = entity.createdBy,
                            createdDate = entity.createdDate,
                            modifiedBy = entity.modifiedBy,
                            modifiedDate = entity.modifiedDate,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _stockEn;
        }

        /// <summary>
        /// Method Finds whether record exists based on Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsCalendarSettingExists(CalendarSettings entity)
        {
            CalendarSetting _calendarSettingEn = new CalendarSetting();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var obj = model.Organizations.SingleOrDefault(p => p.id == entity.organization_Id);
                    if (obj != null)
                    {
                        _calendarSettingEn = model.CalendarSettings.SingleOrDefault(p => p.CalendarEvent.organization_Id == obj.parent_Id && p.calendarEvent_Id == entity.calendarEvent_Id && p.StartDate == entity.StartDate);
                        if (_calendarSettingEn != null)
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


        public List<CalendarSettings> GetListCalendarSetting()
        {
            throw new NotImplementedException();
        }

        public List<int> GetCalendarYears(int organization_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    var obj = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (obj != null)
                    {
                        var keys = model.CalendarSettings.Where(p => p.CalendarEvent.organization_Id == obj.parent_Id).GroupBy(p => p.StartDate.Year).Select(p => p.Key).ToList();

                        return keys;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool CopyCalendar(CopyCalendars entity)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    int val = entity.toYear - entity.fromYear;
                    var obj = model.Organizations.SingleOrDefault(p => p.id == entity.organization_Id);
                    if (obj != null)
                    {
                        if (entity.replace)
                        {
                            bool res = DeleteCalendarSetting(entity.organization_Id, entity.toYear);
                            if (!res)
                                return false;

                        }
                        var _calendarSettingEn = model.CalendarSettings.Where(p => p.CalendarEvent.organization_Id == obj.parent_Id && p.StartDate.Year == entity.fromYear);

                        var lst = new List<CalendarSetting>();

                        foreach (var item in _calendarSettingEn)
                        {
                            var newObj = new CalendarSetting();
                            newObj.calendarEvent_Id = item.calendarEvent_Id;
                            newObj.StartDate = item.StartDate.AddYears(val);
                            if (item.EndDate != null)
                                newObj.EndDate= item.EndDate.GetValueOrDefault().AddYears(val);

                            newObj.calendarEvent_Id = item.calendarEvent_Id;
                            newObj.createdBy = entity.createdBy;
                            newObj.createdDate = entity.createdDate;

                            //lst.Add(newObj);
                            model.CalendarSettings.Add(newObj);
                            
                        }
                        //model.CalendarSettings.AddRange(lst);

                        model.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

    }
}
