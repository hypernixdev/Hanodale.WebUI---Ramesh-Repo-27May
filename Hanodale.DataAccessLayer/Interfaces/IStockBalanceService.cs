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
    public interface IStockBalanceService
    {
        StockBalanceDetails GetStockBalanceBySearch(DatatableFilters entityFilter);

        StockBalances CreateStockBalance(StockBalances entityEn);

        StockBalances UpdateStockBalance(StockBalances entityEn);

        bool DeleteStockBalance(int id);

        StockBalances GetStockBalanceById(int id);

        bool IsStockBalanceExists(StockBalances entityEn);

    }
}
