using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface ICalendarEventService
    {
        #region CalendarEvent

        CalendarEventDetails GetCalendarEventBySearch(int currentUserId, int organization_Id, int startIndex, int pageSize, string search);

        CalendarEventDetails GetCalendarEvent(int currentUserId, int organization_Id, int startIndex, int pageSize);

        CalendarEvents CreateCalendarEvent(int currentUserId, CalendarEvents entity, string pageName);

        CalendarEvents UpdateCalendarEvent(int currentUserId, CalendarEvents entity, string pageName);

        bool DeleteCalendarEvent(int currentUserId, int id, string pageName);

        CalendarEvents GetCalendarEventById(int id);

        List<CalendarEvents> GetListCalendarEvent(int currentUserId, int organization_Id);

        bool IsCalendarEventExists(CalendarEvents entity);


        #endregion
    }
}
