using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Utility.Helpers
{
    public class CultureHelper
    {
        public Collection<CultureStruct> GetLanguages()
        {
            var myClass = new Collection<CultureStruct>();

            var q = from c in
                        System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
                    orderby c.EnglishName, c.TwoLetterISOLanguageName
                    select c;

            foreach (System.Globalization.CultureInfo c in q)
            {
                myClass.Add(new CultureStruct { Name = c.EnglishName, ID = c.LCID });
            }
            return myClass;
        }

        public string GetCultureName(int language)
        {
            var myClass = new Collection<CultureStruct>();

            var q = from c in
                        System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
                    where c.LCID == language
                    select c;

            string _langName = "";
            foreach (System.Globalization.CultureInfo c in q)
            {
                _langName = c.Name;
                break;
            }

            return _langName;
        }

        public Collection<TimeZoneStruct> GetTimeZones()
        {
            var myClass = new Collection<TimeZoneStruct>();
            foreach (var timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
            {
                myClass.Add(new TimeZoneStruct { Name = timeZoneInfo.DisplayName, ID = timeZoneInfo.Id });

            }
            return myClass;
        }

        public struct CultureStruct
        {
            public string Name { get; set; }
            public int ID { get; set; }
        }

        public struct TimeZoneStruct
        {
            public string Name { get; set; }
            public string ID { get; set; }
        }
    }
}
