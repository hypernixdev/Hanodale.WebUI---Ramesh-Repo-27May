
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Reports
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int organization_Id { get; set; }

        [DataMember]
        public int? parent_Id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string icon { get; set; }

        [DataMember]
        public string fontColor { get; set; }

        [DataMember]
        public string backColor { get; set; }

        [DataMember]
        public int ordering { get; set; }

        [DataMember]
        public bool visibility { get; set; }

        [DataMember]
        public List<Reports> ChildList { get; set; }

        
    }
}