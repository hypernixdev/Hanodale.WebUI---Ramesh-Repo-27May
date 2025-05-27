using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ILooseConversionService
    {
        LooseConversionDetails GetLooseConversion(DatatableFilters entityFilter);

        LooseConversions SaveLooseConversion(LooseConversions entityEn);
        // LooseConversionItems SaveLooseConversionItem(LooseConversionItems entityEn);
         bool SaveLooseConversionItem(List<LooseConversionItems> itemsList);

        //List<LooseConversionItems> itemsList
        bool DeleteLooseConversion(int id);

        LooseConversions GetLooseConversionById(int id);

        LooseBarcodeSettings GetLooseBarcodeSettingById(int id);

        bool IsLooseConversionExists(LooseConversions entityEn);

        decimal CalculateLooseQty(string barcode);
    }
}
