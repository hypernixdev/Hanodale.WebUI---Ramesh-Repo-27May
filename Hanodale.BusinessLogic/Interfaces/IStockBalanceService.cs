using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IStockBalanceService
    {
        StockBalanceDetails GetStockBalance(DatatableFilters entityFilter);

        StockBalances SaveStockBalance(StockBalances entityEn);

        bool DeleteStockBalance(int id);

        StockBalances GetStockBalanceById(int id);

        bool IsStockBalanceExists(StockBalances entityEn);

    }
}
