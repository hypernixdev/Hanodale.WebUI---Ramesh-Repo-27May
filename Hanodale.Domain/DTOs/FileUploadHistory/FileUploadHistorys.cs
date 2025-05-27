using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class FileUploadHistorys
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int uploadType { get; set; }

        [DataMember]
        public string fileName { get; set; }

        [DataMember]
        public int totalRecords { get; set; }

        [DataMember]
        public int user_Id { get; set; }
        
        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public System.DateTime createdDate { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
     
        [DataMember]
        public string userName { get; set; }
    }

    public class FileUploadHistoryDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<FileUploadHistorys> lstFileUploadHistory { get; set; }
    }
}
