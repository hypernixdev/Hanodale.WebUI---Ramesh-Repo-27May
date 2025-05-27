using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Utility
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, bool isDescending = false)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (isDescending)
                {
                    return source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
                }
                else
                {
                    return source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
                }

            }
            else
            {
                return source.OrderBy(x => 1);
            }
        }
    }

}
