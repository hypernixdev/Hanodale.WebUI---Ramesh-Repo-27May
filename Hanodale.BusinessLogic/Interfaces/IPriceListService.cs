using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IPriceListService
    {
        PriceListDetails GetPriceList(DatatableFilters entityFilter);

        PriceLists SavePriceList(PriceLists entityEn);

        bool DeletePriceList(int id);

        PriceLists GetPriceListById(int id);

        bool IsPriceListExists(PriceLists entityEn);

        List<PriceListParts> GetPriceListParts(string ListCode);
        List<CustomerPriceLists> GetCustomerPriceList(int CustNum, string groupCode);


    }
}
