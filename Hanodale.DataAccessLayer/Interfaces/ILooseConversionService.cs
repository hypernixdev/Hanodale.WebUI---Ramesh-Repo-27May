using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanodale.Entity.Core;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using System.Data.Objects.SqlClient;
using System.Collections;
using System.Globalization;
using Hanodale.Domain.DTOs;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface ILooseConversionService
    {
        LooseConversionDetails GetLooseConversionBySearch(DatatableFilters entityFilter);

        LooseConversions CreateLooseConversion(LooseConversions entityEn);

        LooseConversions UpdateLooseConversion(LooseConversions entityEn);

        bool CreateLooseConversionItem(List<LooseConversionItems> itemsList);

     //   LooseConversionItems UpdateLooseConversionItem(LooseConversionItems entityEn);

        bool DeleteLooseConversion(int id);

        LooseConversions GetLooseConversionById(int id);
        LooseBarcodeSettings GetLooseBarcodeSettingById(int id);
        bool IsLooseConversionExists(LooseConversions entityEn);
        decimal CalculateLooseQty(string barcode);

    }
}
