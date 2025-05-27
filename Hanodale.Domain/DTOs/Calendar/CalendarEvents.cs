using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class CalendarEvents
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public string eventColor { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
        [DataMember]
        public bool visibility { get; set; }
        [DataMember]
        public bool allowToSelect { get; set; }
       
 
    }
    public class CalendarEventDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<CalendarEvents> lstCalendarEvents { get; set; }
        
        [DataMember]
        public ICollection<CalendarEvents> CalendarEventCollection { get; set; }


    }
}
