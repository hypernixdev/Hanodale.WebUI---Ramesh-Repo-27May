using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class SchedulerLogs
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int schedulerSetting_Id { get; set; }

        [DataMember]
        public Nullable<System.DateTime> startDateTime { get; set; }

        [DataMember]
        public Nullable<System.DateTime> endDateTime { get; set; }  

        [DataMember]
        public Nullable<bool> result { get; set; }

        [DataMember]
        public int totalRecordProcessed { get; set; }

        [DataMember]
        public string errorMessage { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }

    public class SchedulerLogDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<SchedulerLogs> lstSchedulerLog { get; set; }
    }
}
