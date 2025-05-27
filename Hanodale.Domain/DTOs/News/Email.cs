using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Email
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string mailTemplate { get; set; }

        [DataMember]
        public string ToId { get; set; }

        [DataMember]
        public string CcId { get; set; }

        [DataMember]
        public string filePath { get; set; }

        [DataMember]
        public string fileName { get; set; }

        [DataMember]
        public string StatusId { get; set; }

        [DataMember]
        public string remark { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public DateTime? createdDate { get; set; }

    }

}
