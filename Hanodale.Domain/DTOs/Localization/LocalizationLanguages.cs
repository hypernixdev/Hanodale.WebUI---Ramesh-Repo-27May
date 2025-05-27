using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class LocalizationLanguages
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string culture { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string shortName { get; set; }
        [DataMember]
        public string flagIconName { get; set; }
        [DataMember]
        public bool isDefault { get; set; }
        [DataMember]
        public bool visibility { get; set; }
    }
    public class LocalizationLanguageDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }
        [DataMember]
        public List<LocalizationLanguages> lstLocalizationLanguage { get; set; }
    }
}
