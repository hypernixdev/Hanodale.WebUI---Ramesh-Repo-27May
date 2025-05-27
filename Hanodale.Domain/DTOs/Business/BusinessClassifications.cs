using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class BusinessClassifications
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int business_Id { get; set; }
        [DataMember]
        public int classification_Id { get; set; }
        [DataMember]
        public bool isCheck { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
        [DataMember]
        public int[] classification_Ids { get; set; }
    }

    public class BusinessClassificationDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<BusinessClassifications> lstBusinessClassification { get; set; }
    }
}