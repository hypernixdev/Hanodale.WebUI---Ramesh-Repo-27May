using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class UomConversionService : IUomConversionService
    {
        public Hanodale.DataAccessLayer.Interfaces.IUomConversionService DataProvider;

        public UomConversionService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.UomConversionService();
        }

        public UomConversionDetails GetUomConversion(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetUomConversionBySearch(entityFilter);
        }

        public UomConversions SaveUomConversion(UomConversions entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateUomConversion(entityEn);
            else
                return this.DataProvider.CreateUomConversion(entityEn);
        }

        public bool DeleteUomConversion(int id)
        {
            return this.DataProvider.DeleteUomConversion(id);
        }

        public UomConversions GetUomConversionById(int id)
        {
            return this.DataProvider.GetUomConversionById(id);
        }

        public bool IsUomConversionExists(UomConversions entityEn)
        {
            return this.DataProvider.IsUomConversionExists(entityEn);
        }
    }
}
