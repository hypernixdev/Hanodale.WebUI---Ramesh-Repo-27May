using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
namespace Hanodale.BusinessLogic
{
    public class CalendarEventService : ICalendarEventService
    {
        #region CalendarEventService

        public Hanodale.DataAccessLayer.Interfaces.ICalendarEventService DataProvider;

        public CalendarEventService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.CalendarEventService();
        }

        public CalendarEventDetails GetCalendarEvent(int currentUserId, int organization_Id, int startIndex, int pageSize, string search)
        {
            if(string.IsNullOrEmpty(search))
            return this.DataProvider.GetCalendarEvent(currentUserId, organization_Id, startIndex, pageSize);
            else
                 return this.DataProvider.GetCalendarEventBySearch(currentUserId, organization_Id, startIndex, pageSize, search);
        }

        public CalendarEvents SaveCalendarEvent(int currentUserId, CalendarEvents CalendarEventEn, string pageName)
        {
            if(CalendarEventEn.id>0)
            return this.DataProvider.UpdateCalendarEvent(currentUserId, CalendarEventEn, pageName);
            else
                return this.DataProvider.CreateCalendarEvent(currentUserId, CalendarEventEn, pageName);
        }

        public bool DeleteCalendarEvent(int currentUserId, int CalendarEventId, string pageName)
        {
            return this.DataProvider.DeleteCalendarEvent(currentUserId, CalendarEventId, pageName);
        }

        public CalendarEvents GetCalendarEventById(int id)
        {
            return this.DataProvider.GetCalendarEventById(id);
        }

        public List<CalendarEvents> GetListCalendarEvent(int currentUserId, int organization_Id)
        {
            return this.DataProvider.GetListCalendarEvent(currentUserId,organization_Id);
        }

        public bool IsCalendarEventExists(CalendarEvents CalendarEvent)
        {
            return this.DataProvider.IsCalendarEventExists(CalendarEvent);
        }

        #endregion
    }
}
