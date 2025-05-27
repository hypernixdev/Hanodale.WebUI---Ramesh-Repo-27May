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
    public interface IPriceListService
    {
        PriceListDetails GetPriceListBySearch(DatatableFilters entityFilter);

        PriceLists CreatePriceList(PriceLists entityEn);

        PriceLists UpdatePriceList(PriceLists entityEn);

        bool DeletePriceList(int id);

        PriceLists GetPriceListById(int id);

        bool IsPriceListExists(PriceLists entityEn);

        List<PriceListParts> GetPriceListParts(string ListCode);
        List<CustomerPriceLists> GetCustomerPriceList(int CustNum , string groupCode);



    }
}
