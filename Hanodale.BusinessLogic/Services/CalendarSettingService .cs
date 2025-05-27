using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
namespace Hanodale.BusinessLogic
{
    public class CalendarSettingService : ICalendarSettingService
    {
        #region CalendarSettingService

        public Hanodale.DataAccessLayer.Interfaces.ICalendarSettingService DataProvider;

        public CalendarSettingService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.CalendarSettingService();
        }

        public CalendarSettingDetails GetCalendarSetting(int currentUserId, int organization_Id, int startIndex, int pageSize, string search)
        {
                return this.DataProvider.GetCalendarSetting(currentUserId, organization_Id, startIndex, pageSize, search);
        }

        public List<CalendarSettings> GetCalendarItem(int currentUserId, int organization_Id, int year)
        {
            return this.DataProvider.GetCalendarItem(currentUserId, organization_Id, year);
        }

        public bool SaveCalendarSetting(int currentUserId, List<CalendarSettings> lst, int organizationId, int year )
        {
            return this.DataProvider.SaveCalendarSetting(currentUserId, lst, organizationId, year);
        }

        public bool DeleteCalendarSetting(int organization_Id, int year)
        {
            return this.DataProvider.DeleteCalendarSetting(organization_Id, year);
        }

        public CalendarSettings GetCalendarSettingById(int id)
        {
            return this.DataProvider.GetCalendarSettingById(id);
        }

        public bool IsCalendarSettingExists(CalendarSettings CalendarSetting)
        {
            return this.DataProvider.IsCalendarSettingExists(CalendarSetting);
        }


        public List<int> GetCalendarYears(int organization_Id)
        {
            return this.DataProvider.GetCalendarYears(organization_Id);
        }

        public bool CopyCalendar(CopyCalendars entity)
        {
            return this.DataProvider.CopyCalendar(entity);
        }
        
        #endregion
    }
}
