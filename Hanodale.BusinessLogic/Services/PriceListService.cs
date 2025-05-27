using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class PriceListService : IPriceListService
    {
        public Hanodale.DataAccessLayer.Interfaces.IPriceListService DataProvider;

        public PriceListService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.PriceListService();
        }

        public PriceListDetails GetPriceList(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetPriceListBySearch(entityFilter);
        }

        public PriceLists SavePriceList(PriceLists entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdatePriceList(entityEn);
            else
                return this.DataProvider.CreatePriceList(entityEn);
        }

        public bool DeletePriceList(int id)
        {
            return this.DataProvider.DeletePriceList(id);
        }

        public PriceLists GetPriceListById(int id)
        {
            return this.DataProvider.GetPriceListById(id);
        }

        public bool IsPriceListExists(PriceLists entityEn)
        {
            return this.DataProvider.IsPriceListExists(entityEn);
        }
       
        public List<PriceListParts> GetPriceListParts(string ListCode)
        {
            return this.DataProvider.GetPriceListParts(ListCode);
        }

        public List<CustomerPriceLists> GetCustomerPriceList(int CustNum, string groupCode)
        {
            return this.DataProvider.GetCustomerPriceList(CustNum , groupCode);
        }


    }
}
