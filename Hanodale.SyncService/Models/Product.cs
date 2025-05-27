using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class ProductApiModel
    {
        [JsonProperty("company")]
        public string Company { get; set; }
        [JsonProperty("partNum")]
        public string partNumber { get; set; }
        public string itemBrandCode { get; set; }

        [JsonProperty("itemBrand")]
        public string UD04_Character01_Item_Brand { get; set; }

        public string description { get; set; }

        [JsonProperty("group")]
        public string code { get; set; }
        
        [JsonProperty("groupDesc")]
        public string ProdGrup_Description { get; set; }

        [JsonProperty("class")]
        public string Part_ClassID { get; set; }

        [JsonProperty("classDesc")]
        public string PartClass_Description { get; set; }

        [JsonProperty("temperatureTypeCode")]
        public string Part_ShortChar01_Temprature_Type_Code { get; set; }

        [JsonProperty("countryOfOriginDescription")]
        public string Country_Description { get; set; }

        [JsonProperty("fullSellingUOM")]
        public string UOMClass_DefUomCode { get; set; }

        [JsonProperty("looseSellingUOM")]
        public string Part_SalesUM { get; set; }

        [JsonProperty("classType")]
        public string UOMClass_ClassType { get; set; }

        [JsonProperty("looseUOM")]
        public string Part_ShortChar03_LooseUOM { get; set; }

        [JsonProperty("inventoryUOM")]
        public string Part_IUM { get; set; }

        [JsonProperty("baseUOMCode")]
        public string UOMClass_BaseUOMCode { get; set; }

        [JsonProperty("allowSellingVaryWeight")]
        public bool AllowSellingVaryWeight { get; set; }

        [JsonProperty("allowSellingLoose")]
        public bool Part_CheckBox01_Allow_Selling_Loose { get; set; }

        [JsonProperty("slice")]
        public bool Part_CheckBox03_Slice { get; set; }

        [JsonProperty("cube")]
        public bool Part_CheckBox04_Cube { get; set; }

        [JsonProperty("strip")]
        public bool Part_CheckBox13_Strip { get; set; }

        [JsonProperty("standardFullQty")]
        public bool Part_CheckBox05_Standard_Full_Qty { get; set; }

        public bool inActive { get; set; }
        public decimal conversionFactor { get; set; }

    }
}
