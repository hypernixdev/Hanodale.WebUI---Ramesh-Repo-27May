using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class BusinessFiles
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int business_Id { get; set; }

        [DataMember]
        public int fileType_Id { get; set; }

        [DataMember]
        public string fileTypeName { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string urlPath { get; set; }

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

    }

    public class BusinessFileDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<BusinessFiles> lstBusinessFile { get; set; }
    }
}
