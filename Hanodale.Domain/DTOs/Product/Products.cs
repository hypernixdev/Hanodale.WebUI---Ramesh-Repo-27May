using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Products
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string partNumber { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string code_Id { get; set; }
        [DataMember]
        public string part_Id { get; set; }
        [DataMember]
        public string prodGrup_Description { get; set; }
        [DataMember]
        public string part_ClassID { get; set; }
        [DataMember]
        public string partClass_Description { get; set; }
        [DataMember]
        public string country_Description { get; set; }
        [DataMember]
        public string uomClass_DefUomCode { get; set; }
        [DataMember]
        public string part_SalesUM { get; set; }
        [DataMember]
        public string part_IUM { get; set; }
        [DataMember]
        public string uomClass_BaseUOMCode { get; set; }
        [DataMember]
        public int country_CountryNum { get; set; }
        [DataMember]
        public string uomClass_ClassType { get; set; }
        [DataMember]
        public string uomClass_Description { get; set; }
        [DataMember]
        public decimal unitPrice { get; set; }
        [DataMember]
        public bool isSuccess { get; set; }    
        [DataMember]
        public string searchDescription { get; set; }
        [DataMember]
        public string searchGrupDescription { get; set; }
        [DataMember]
        public string searchPartClassDescription { get; set; }
        [DataMember]
        public string searchCountryDescription  { get; set; }

        [DataMember]
        public bool? isSlice { get; set; }

        [DataMember]
        public bool? isCube { get; set; }

        [DataMember]
        public bool? isStrip { get; set; }

        [DataMember]
        public string allowVaryWeight { get; set; }

        [DataMember]
        public string standardFullQty { get; set; }

        [DataMember]
        public string looseUom { get; set; }

        [DataMember]
        public int defaultUomId { get; set; }

        [DataMember]
        public string FullUom { get; set; }

        [DataMember]
        public string UomBasedOnCondition { get; set;  }

        [DataMember]
        public object UomPrices { get; set; }

        [DataMember]
        public bool AllowLooseSelling { get; set; }

        [DataMember]
        public string weightValue { get; set; }

        [DataMember]
        public Nullable<decimal> remainingQty { get; set; }

        [DataMember]
        public string serialNumber { get; set; }

        [DataMember]
        public decimal conversionFactor { get; set; }

        [DataMember]
        public List<ProductStockBalance> productStockBalances { get; set; }
    }

    public class ProductDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Products> lstProduct { get; set; }
    }

    public class ProductStockBalance
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string partNumber { get; set; }
        [DataMember]
        public string uom { get; set; }

        [DataMember]
        public Nullable<decimal> remainingQty { get; set; }
    }

    public class ProductsView : Products
    {
        public bool isLooseBarcode { get; set; }
    }

}
