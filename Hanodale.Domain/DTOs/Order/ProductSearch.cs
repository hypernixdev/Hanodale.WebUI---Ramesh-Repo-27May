using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs.Order
{
    #region Product search in order screen 
    public class ProductDatatableFilter
    {
        public int Draw { get; set; } // Corresponds to 'start'
        public int Start { get; set; } // Corresponds to 'start'
        public int Length { get; set; } // Corresponds to 'length'
        public string SearchValue { get; set; } // Corresponds to 'search[value]'
        public bool SearchRegex { get; set; } // Corresponds to 'search[regex]'
        public string PartNum { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PartGroup { get; set; }
        public string PartClass { get; set; }
        public string brand { get; set; }
        public string origin { get; set; }
        public string temperature { get; set; }

        public string customerId { get; set; }
        public string shipToId { get; set; }
        public string orderDate { get; set; }
    }
    #endregion
}
