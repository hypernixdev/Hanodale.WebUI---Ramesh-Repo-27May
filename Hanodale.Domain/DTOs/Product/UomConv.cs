using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs.Order
{
    public class UomConvs
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string UomCode { get; set; }
        public string ConvFactor { get; set; }
        public string UniqueField { get; set; }
        public string ConvOperator { get; set; }
    }
}
