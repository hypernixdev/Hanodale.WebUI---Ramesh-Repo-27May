using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ICalendarEventService
   {
       #region CalendarEvent

       CalendarEventDetails GetCalendarEvent(int currentUserId, int organization_Id, int startIndex, int pageSize, string search);

       CalendarEvents SaveCalendarEvent(int currentUserId, CalendarEvents CalendarEventEn, string pageName);

       bool DeleteCalendarEvent(int currentUserId, int CalendarEventId, string pageName);

       CalendarEvents GetCalendarEventById(int id);

       List<CalendarEvents> GetListCalendarEvent(int currentUserId, int organization_Id);

       bool IsCalendarEventExists(CalendarEvents CalendarEvent);

        #endregion
    }
}
