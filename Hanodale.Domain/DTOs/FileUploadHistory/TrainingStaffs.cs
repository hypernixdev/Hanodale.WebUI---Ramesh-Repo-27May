using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class TrainingStaffs
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int fileUploadHistory_Id { get; set; }
        [DataMember]
        public int salaryId { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string trainingevent { get; set; }
        [DataMember]
        public string businessevent { get; set; }
        [DataMember]
        public Nullable<System.DateTime> startDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> endDate { get; set; }
        [DataMember]
        public string subarea { get; set; }
        [DataMember]
        public Nullable<int> costCenterId { get; set; }
        [DataMember]
        public string costCenterText { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string organizer { get; set; }
        [DataMember]
        public string trainer { get; set; }
        [DataMember]
        public Nullable<decimal> fees { get; set; }
        [DataMember]
        public Nullable<decimal> days { get; set; }
        [DataMember]
        public Nullable<decimal> hours { get; set; }
        [DataMember]
        public string remarks { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public System.DateTime createdDate { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }


    }

    public class TrainingStaffDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<TrainingStaffs> lstTrainingStaff { get; set; }
    }
}
