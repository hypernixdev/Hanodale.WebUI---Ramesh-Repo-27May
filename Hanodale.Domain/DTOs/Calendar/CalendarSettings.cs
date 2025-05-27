using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class CalendarSettings
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public int calendarEvent_Id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public string color { get; set; }
        [DataMember]
        public bool allowToSelect { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public Nullable<DateTime> EndDate { get; set; }
        [DataMember]
        public string calendarEventTitle { get; set; }
        [DataMember]
        public string calendarEventDescription { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }
    public class CalendarSettingDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<CalendarSettings> lstCalendarSettings { get; set; }
        
        [DataMember]
        public ICollection<CalendarSettings> CalendarSettingCollection { get; set; }


    }
}
