using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartPanelRFQ
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string contactName { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public DateTime createdDate { get; set; }

        [DataMember]
        public string contactNo { get; set; }

        [DataMember]
        public string reference { get; set; }

        [DataMember]
        public Nullable<DateTime> dueDate { get; set; }

        [DataMember]
        public Nullable<DateTime> rfqDate { get; set; }

        [DataMember]
        public string location { get; set; }

        [DataMember]
        public string projectManagers { get; set; }

        [DataMember]
        public string referralPO { get; set; }

        [DataMember]
        public Nullable<int> submissionType { get; set; }

        [DataMember]
        public Nullable<System.DateTime> visitDate { get; set; }
    }

    public class ChartPanelRFQDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ChartPanelRFQ> lstRFQ { get; set; }
    }
}
