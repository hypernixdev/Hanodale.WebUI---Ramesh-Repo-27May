using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class AttendanceStaffs
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int fileUploadHistory_Id { get; set; }
        [DataMember]
        public int salaryId { get; set; }
        [DataMember]
        public string employeeName { get; set; }
        [DataMember]
        public Nullable<System.DateTime> date { get; set; }
        [DataMember]
        public Nullable<System.TimeSpan> In_AM { get; set; }
        [DataMember]
        public Nullable<System.TimeSpan> Out_AM { get; set; }
        [DataMember]
        public Nullable<System.TimeSpan> In_PM { get; set; }
        [DataMember]
        public Nullable<System.TimeSpan> Out_PM { get; set; }
        [DataMember]
        public Nullable<System.TimeSpan> OT_In { get; set; }
        [DataMember]
        public Nullable<System.TimeSpan> OT_Out { get; set; }
        [DataMember]
        public Nullable<decimal> lengthWork { get; set; }
        [DataMember]
        public Nullable<decimal> overTime1 { get; set; }
        [DataMember]
        public Nullable<decimal> overTime2 { get; set; }
        [DataMember]
        public Nullable<decimal> overTime3 { get; set; }
        [DataMember]
        public Nullable<decimal> underTime { get; set; }
        [DataMember]
        public Nullable<decimal> lates { get; set; }
        [DataMember]
        public Nullable<decimal> nightDifferent { get; set; }
        [DataMember]
        public string absent { get; set; }
        [DataMember]
        public string comment { get; set; }
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

    public class AttendanceStaffDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<AttendanceStaffs> lstAttendanceStaff { get; set; }
    }
}
