using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class SchedulerSetups
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int syncModule_Id { get; set; }

        [DataMember]
        public string SyncModuleName { get; set; }
        [DataMember]
        public Nullable<System.DateTime> startDate { get; set; }

        [DataMember]
        public int timeSlot { get; set; }

        [DataMember]
        public string TimeSlotName { get; set; }

        

        [DataMember]
        public Nullable<bool> isActive { get; set; }

        [DataMember]
        public int createdBy { get; set; }

        [DataMember]
        public string createByName { get; set; }



        [DataMember]
        public System.DateTime createdDate { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }

    public class SchedulerSetupDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<SchedulerSetups> lstSchedulerSetup { get; set; }
    }
}
