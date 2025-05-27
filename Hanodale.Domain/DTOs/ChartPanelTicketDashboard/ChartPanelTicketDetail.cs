using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartPanelTicket
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string feedback { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string department { get; set; }
        [DataMember]
        public string designation { get; set; }
        [DataMember]
        public string createDateStr { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    public class ChartPanelTicketDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ChartPanelTicket> lstTicket { get; set; }
    }
}
