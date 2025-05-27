using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Dashboards
    {
        [DataMember]
        public List<DashboardItems> itemList { get; set; }

        [DataMember]
        public int ticketCount { get; set; }
    }

    [DataContract]
    public class OrderPaymentTotals
    {
        [DataMember]
        public decimal TotalSales { get; set; }
        [DataMember]
        public decimal TotalRefund { get; set; }
    }
    [DataContract]
    public class DashboardItems
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int menu_Id { get; set; }

        [DataMember]
        public int? parent_Id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string icon { get; set; }

        [DataMember]
        public int newCount { get; set; }

        [DataMember]
        public string fontColor { get; set; }

        [DataMember]
        public string backColor { get; set; }

        [DataMember]
        public string pageURL { get; set; }

        [DataMember]
        public int ordering { get; set; }

        [DataMember]
        public bool visibility { get; set; }

        [DataMember]
        public List<DashboardItems> ChildList { get; set; }
    }
}
