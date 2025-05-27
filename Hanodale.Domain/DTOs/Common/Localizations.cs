using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Localizations : Cultures
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string keyName { get; set; }

        [DataMember]
        public int localizationSection_Id { get; set; }

        [DataMember]
        public int localizationSectionName { get; set; }

        [DataMember]
        public string defaultValue { get; set; }

        [DataMember]
        public string value { get; set; }
    }

    [DataContract]
    public class LocalizationFilters : Cultures
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int localizationSection_Id { get; set; }

        [DataMember]
        public int localizationSectionName { get; set; }

    }
}
