using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class StockBalanceService : IStockBalanceService
    {
        public Hanodale.DataAccessLayer.Interfaces.IStockBalanceService DataProvider;

        public StockBalanceService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.StockBalanceService();
        }

        public StockBalanceDetails GetStockBalance(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetStockBalanceBySearch(entityFilter);
        }

        public StockBalances SaveStockBalance(StockBalances entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateStockBalance(entityEn);
            else
                return this.DataProvider.CreateStockBalance(entityEn);
        }

        public bool DeleteStockBalance(int id)
        {
            return this.DataProvider.DeleteStockBalance(id);
        }

        public StockBalances GetStockBalanceById(int id)
        {
            return this.DataProvider.GetStockBalanceById(id);
        }

        public bool IsStockBalanceExists(StockBalances entityEn)
        {
            return this.DataProvider.IsStockBalanceExists(entityEn);
        }

       
    }
}
