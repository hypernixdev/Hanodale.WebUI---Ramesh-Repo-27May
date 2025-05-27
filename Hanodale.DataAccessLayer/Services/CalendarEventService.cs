using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System.ServiceModel;
using Hanodale.Domain;

namespace Hanodale.DataAccessLayer.Services
{
    public class CalendarEventService : ICalendarEventService
    {

        public CalendarEventDetails GetCalendarEventBySearch(int currentUserId, int organization_Id, int startIndex, int pageSize, string search)
        {
            CalendarEventDetails _result = new CalendarEventDetails();
            _result.recordDetails = new RecordDetails();
            bool c, d, e, f;
            c = string.IsNullOrEmpty(search) ? true : Common.Visibility.True.ToString().ToLower().Contains(search.ToLower()); d = string.IsNullOrEmpty(search) ? true : Common.Visibility.False.ToString().ToLower().Contains(search.ToLower());
            e = string.IsNullOrEmpty(search) ? true : Common.Visibility.True.ToString().ToLower().Contains(search.ToLower()); f = string.IsNullOrEmpty(search) ? true : Common.Visibility.False.ToString().ToLower().Contains(search.ToLower());

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record

                    var obj = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (obj != null)
                    {
                        _result.recordDetails.totalRecords = model.CalendarEvents.Where(p => p.organization_Id == obj.parent_Id).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                        var result = model.CalendarEvents.Where(p => p.organization_Id == obj.parent_Id)
                                        .OrderByDescending(p => p.modifiedBy)
                                        .Where(p => p.title.Contains(search)
                                               || p.description.Contains(search)
                                               || p.icon.Contains(search)
                                               || p.eventColor.Contains(search)
                                               || (c ? p.visibility == true : d ? p.visibility == false : false)
                                               || (e ? p.allowToSelect == true : f ? p.allowToSelect == false : false))
                                               .Select(p => new CalendarEvents
                                               {
                                                   id = p.id,
                                                   title = p.title,
                                                   description = p.description,
                                                   icon = p.icon,
                                                   eventColor = p.eventColor,
                                                   organization_Id = p.organization_Id,
                                                   visibility = p.visibility,
                                                   allowToSelect = p.allowToSelect,
                                                   createdBy = p.createdBy,
                                                   createdDate = p.createdDate,
                                                   modifiedBy = p.modifiedBy,
                                                   modifiedDate = p.modifiedDate
                                               }).ToList();

                        //Get filter data
                        _result.recordDetails.totalDisplayRecords = result.Count;
                        _result.lstCalendarEvents = result.Skip(startIndex).Take(pageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public CalendarEventDetails GetCalendarEvent(int currentUserId, int organization_Id, int startIndex, int pageSize)
        {
            CalendarEventDetails _result = new CalendarEventDetails();
            _result.recordDetails = new RecordDetails();

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record

                    var obj = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (obj != null)
                    {

                        _result.recordDetails.totalRecords = model.CalendarEvents.Where(p => p.organization_Id == obj.parent_Id).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;


                        _result.lstCalendarEvents = model.CalendarEvents.Where(p => p.organization_Id == obj.parent_Id)
                                        .OrderByDescending(p => p.modifiedBy).Skip(startIndex).Take(pageSize).Select(p => new CalendarEvents
                                        {
                                            id = p.id,
                                            title = p.title,
                                            description = p.description,
                                            icon = p.icon,
                                            eventColor = p.eventColor,
                                            organization_Id = p.organization_Id,
                                            visibility = p.visibility,
                                            allowToSelect = p.allowToSelect,
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


        public CalendarEvents CreateCalendarEvent(int currentUserId, CalendarEvents item, string pageName)
        {
            CalendarEvent _calendarEventEn = new CalendarEvent();
            CalendarEvents entity = new CalendarEvents();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var obj = model.Organizations.SingleOrDefault(p => p.id == item.organization_Id);
                    if (obj != null)
                    {
                        //Add new stock
                        _calendarEventEn.organization_Id = (int)obj.parent_Id;
                        _calendarEventEn.title = item.title.Trim();
                        _calendarEventEn.description = string.IsNullOrEmpty(item.description) ? null : item.description.Trim();
                        _calendarEventEn.icon = item.icon.Trim();
                        _calendarEventEn.eventColor = item.eventColor.Trim();
                        _calendarEventEn.visibility = item.visibility;
                        _calendarEventEn.allowToSelect = item.allowToSelect;
                        _calendarEventEn.createdBy = item.createdBy;
                        _calendarEventEn.createdDate = item.createdDate;
                        _calendarEventEn.modifiedBy = item.modifiedBy;
                        _calendarEventEn.modifiedDate = item.modifiedDate;
                        model.CalendarEvents.Add(_calendarEventEn);
                        model.SaveChanges();


                        entity.id = _calendarEventEn.id;
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
            return entity;
        }

        public CalendarEvents UpdateCalendarEvent(int currentUserId, CalendarEvents entity, string pageName)
        {
            CalendarEvent _calendarEventEn = new CalendarEvent();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _calendarEventEn = model.CalendarEvents.SingleOrDefault(p => p.id == entity.id);
                    if (_calendarEventEn != null)
                    {
                        _calendarEventEn.title = entity.title.Trim();
                        _calendarEventEn.description = string.IsNullOrEmpty(entity.description) ? null : entity.description.Trim();
                        _calendarEventEn.icon = entity.icon.Trim();
                        _calendarEventEn.eventColor = entity.eventColor.Trim();
                        _calendarEventEn.visibility = entity.visibility;
                        _calendarEventEn.allowToSelect = entity.allowToSelect;
                        _calendarEventEn.allowToSelect = entity.allowToSelect;
                        _calendarEventEn.modifiedBy = entity.modifiedBy;
                        _calendarEventEn.modifiedDate = entity.modifiedDate;

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

        public bool DeleteCalendarEvent(int currentUserId, int id, string pageName)
        {
            CalendarEvent _calendarEventEn = new CalendarEvent();
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _calendarEventEn = model.CalendarEvents.SingleOrDefault(p => p.id == id);
                    if (_calendarEventEn != null)
                    {
                        model.CalendarEvents.Remove(_calendarEventEn);
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

        public CalendarEvents GetCalendarEventById(int id)
        {
            CalendarEvents _calendarEventEn = new CalendarEvents();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var entity = model.CalendarEvents.SingleOrDefault(p => p.id == id);


                    if (entity != null)
                    {
                        _calendarEventEn = new CalendarEvents
                        {
                            id = entity.id,
                            title = entity.title,
                            description = entity.description,
                            icon = entity.icon,
                            eventColor = entity.eventColor,
                            organization_Id = entity.organization_Id,
                            visibility = entity.visibility,
                            allowToSelect = entity.allowToSelect,
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
            return _calendarEventEn;
        }

        /// <summary>
        /// Method Finds whether record exists based on Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsCalendarEventExists(CalendarEvents entity)
        {
            CalendarEvent _calendarEventEn = new CalendarEvent();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var obj = model.Organizations.SingleOrDefault(p => p.id == entity.organization_Id);
                    if (obj != null)
                    {
                        if (entity.id > 0)
                        {
                            _calendarEventEn = model.CalendarEvents.SingleOrDefault(p => p.id != entity.id && p.organization_Id == obj.parent_Id && p.title == entity.title.Trim() && p.description == entity.description.Trim());
                            if (_calendarEventEn != null)
                                isExists = true;
                        }
                        else
                        {
                            _calendarEventEn = model.CalendarEvents.SingleOrDefault(p => p.organization_Id == obj.parent_Id && p.title == entity.title.Trim() && p.description == entity.description.Trim());
                            if (_calendarEventEn != null)
                                isExists = true;
                        }
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


        public List<CalendarEvents> GetListCalendarEvent(int currentUserId, int organization_Id)
        {
            var result = new List<CalendarEvents>();
            try
            {

                using (HanodaleEntities model = new HanodaleEntities())
                {

                    //get total record

                    var obj = model.Organizations.SingleOrDefault(p => p.id == organization_Id);
                    if (obj != null)
                    {


                        result = model.CalendarEvents.Where(p => p.organization_Id == obj.parent_Id && p.visibility)
                                        .OrderByDescending(p => p.title).Select(p => new CalendarEvents
                                        {
                                            id = p.id,
                                            title = p.title,
                                            description = p.description,
                                            icon = p.icon,
                                            eventColor = p.eventColor,
                                            organization_Id = p.organization_Id,
                                            visibility = p.visibility,
                                            allowToSelect = p.allowToSelect,
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
            return result;
        }
    }
}
