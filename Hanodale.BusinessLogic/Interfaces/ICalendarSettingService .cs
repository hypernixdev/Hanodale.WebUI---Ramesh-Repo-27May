using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
   public interface ICalendarSettingService
   {
       #region CalendarSetting

       CalendarSettingDetails GetCalendarSetting(int currentUserId, int organization_Id, int startIndex, int pageSize, string search);

       List<CalendarSettings> GetCalendarItem(int currentUserId, int organization_Id, int year);

       bool SaveCalendarSetting(int currentUserId, List<CalendarSettings> lst, int organizationId, int year );

       bool DeleteCalendarSetting(int organization_Id, int year);

       CalendarSettings GetCalendarSettingById(int id);

       bool IsCalendarSettingExists(CalendarSettings CalendarSetting);


       List<int> GetCalendarYears(int organization_Id);

       bool CopyCalendar(CopyCalendars entity);

        #endregion
    }
}
