using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class LooseConversions
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int productCarton_Id { get; set; }

        [DataMember]
        public string postingStatus { get; set; }

        [DataMember]
        public string epicorPartNo { get; set; }

        [DataMember]
        public string barcode { get; set; }

        [DataMember]
        public string createdBy { get; set; }
   
        [DataMember]
        public Nullable<DateTime> createdDate { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
        [DataMember]
        public List<LooseConversionItems> LooseConversionItems { get; set; }


    }
    public class LooseConversionItems
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public Nullable<int> looseConversion_Id { get; set; }

        [DataMember]
        public Nullable<int> weighScaleBarcode_Id { get; set; }
        [DataMember]
        public string barcode { get; set; }
        [DataMember]
        public Nullable<decimal> LooseQty { get; set; }
        [DataMember]
        public Nullable<decimal> RunningBalance { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }

        public string createdBy { get; set; }
    }
    public class LooseBarcodeSettings
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public Nullable<int> barcodeFromPos { get; set; }
        [DataMember]
        public Nullable<int> barcodeToPos { get; set; }
        [DataMember]
        public Nullable<int> weightFromPos { get; set; }
        [DataMember]
        public Nullable<int> weightToPos { get; set; }
        [DataMember]
        public Nullable<decimal> weightMutiply { get; set; }
        [DataMember]
        public Nullable<int> barcodeLength { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }
    public class LooseConversionDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<LooseConversions> lstLooseConversion { get; set; }
    }
}
