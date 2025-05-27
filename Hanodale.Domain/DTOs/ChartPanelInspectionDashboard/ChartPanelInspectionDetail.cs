using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartPanelInspection
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string checklistName { get; set; }
        [DataMember]
        public string submittedBy { get; set; }
        [DataMember]
        public DateTime? submittedDate { get; set; }
        [DataMember]
        public string assetCode { get; set; }
        [DataMember]
        public string shift { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    public class ChartPanelInspectionDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ChartPanelInspection> lstInspection { get; set; }
    }
}
