using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain
{
    [DataContract]
    public class Cultures
    {
        [DataMember]
        public string cultureName { get; set; }
    }
}
