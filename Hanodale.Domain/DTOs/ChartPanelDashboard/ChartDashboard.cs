using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartDashboard
    {
        [DataMember]
        public int value { get; set; }

        [DataMember]
        public int valuePercentage { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string backColor { get; set; }

    }
}
