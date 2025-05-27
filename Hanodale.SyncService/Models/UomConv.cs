using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    
    public class UomConvApiModel
    {
        public string uomCode { get; set; }
        public string convOperator { get; set; }
        public string convFactor { get; set; }
        public string company { get; set; }
        public string partNum { get; set; }
        public string uniqueField { get; set; }
    }

    public class UomConvDetail
    {
        public string uomCode { get; set; }
        public decimal convFactor { get; set; }
        public string convOperator { get; set; }
    }

    public class UomConvApiResponseModel
    {
        public string company { get; set; }
        public string partNum { get; set; }        
        public List<UomConvDetail> uomConvDetails { get; set; }
    }

    public class ProductPriceRequest
    {
        public string custNum { get; set; }
        public string shipToId { get; set; }
        public string orderDate { get; set; }
        public List<ProductInfo> products { get; set; }
    }

    public class ProductInfo
    {
        public string partNumber { get; set; }
        public string uom { get; set; }
        public int index { get; set; }
    }

    public class ProductPriceResult
    {
        public string partNumber { get; set; }
        public string uom { get; set; }
        public decimal? price { get; set; }
        public string errorMessage { get; set; }
        public int index { get; set; }
    }
}
