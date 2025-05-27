using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class BusinessCategories
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string backColor { get; set; }
        [DataMember]
        public string forceColor { get; set; }
        [DataMember]
        public int ordering { get; set; }
        [DataMember]
        public string imageUrl { get; set; }
    }
}
