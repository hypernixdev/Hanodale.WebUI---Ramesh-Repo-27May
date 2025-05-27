using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class LooseConversionService : ILooseConversionService
    {
        public Hanodale.DataAccessLayer.Interfaces.ILooseConversionService DataProvider;

        public LooseConversionService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.LooseConversionService();
        }

        public LooseConversionDetails GetLooseConversion(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetLooseConversionBySearch(entityFilter);
        }

        public LooseConversions SaveLooseConversion(LooseConversions entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateLooseConversion(entityEn);
            else
                return this.DataProvider.CreateLooseConversion(entityEn);
        }

        public bool SaveLooseConversionItem(List<LooseConversionItems> itemsList)
        {
           
                return this.DataProvider.CreateLooseConversionItem(itemsList);
        }

        public bool DeleteLooseConversion(int id)
        {
            return this.DataProvider.DeleteLooseConversion(id);
        }

        public LooseConversions GetLooseConversionById(int id)
        {
            return this.DataProvider.GetLooseConversionById(id);
        }
        public LooseBarcodeSettings GetLooseBarcodeSettingById(int id)
        {
            return this.DataProvider.GetLooseBarcodeSettingById(id);
        }

        public bool IsLooseConversionExists(LooseConversions entityEn)
        {
            return this.DataProvider.IsLooseConversionExists(entityEn);
        }
        public decimal CalculateLooseQty(string barcode)
        {
            return this.DataProvider.CalculateLooseQty(barcode);
        }


    }
}
