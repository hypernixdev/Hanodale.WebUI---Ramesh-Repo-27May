using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Defects
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string justification { get; set; }
        [DataMember]
        public string priority { get; set; }
        [DataMember]
        public int minDuration { get; set; }
        [DataMember]
        public string minDurationType { get; set; }
        [DataMember]
        public int maxDuration { get; set; }
        [DataMember]
        public string maxDurationType { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }

        [DataMember]
        public string fullMaxDuration { get; set; }

        [DataMember]
        public DateTime dueDate { get; set; }

        [DataMember]
        public int workOrder_Id { get; set; }

        [DataMember]
        public bool selected { get; set; }

        [DataMember]
        public string durationLeft { get; set; }

        [DataMember]
        public DateTime actualEndDate { get; set; }

        [DataMember]
        public string durationTaken { get; set; }

        [DataMember]
        public double processPercentage { get; set; }
    }

    public class DefectDetails
    {
        [DataMember]
        public int workOrder_Id { get; set; }

        [DataMember]
        public int jobStatus_Id { get; set; }

        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Defects> lstDefect { get; set; }
    }
}
