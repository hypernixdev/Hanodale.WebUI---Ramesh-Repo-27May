using Hanodale.Domain.DTOs.Order;
using Hanodale.Domain.DTOs.Sync;
using Hanodale.SyncService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public class SyncManager : ISyncManager
    {
        public Hanodale.SyncService.ISyncManager DataProvider;

        public SyncManager()
        {
            this.DataProvider = new Hanodale.SyncService.SyncManager();
        }

        public SyncReponse syncEntity(string entityName, string apiUrl, string uniqueField, bool disablePagination, string customDbModel)
        {
            return this.DataProvider.SyncEntity(entityName, apiUrl, uniqueField, disablePagination, customDbModel);
        }

        public SyncReponse syncEntity(string entityName, string apiUrl, string uniqueField, bool disablePagination, string customDbModel, int pageNumber)
        {
            return this.DataProvider.SyncEntity(entityName, apiUrl, uniqueField, disablePagination, customDbModel, pageNumber);
        }

        public ProductPriceApiModel GetProductPriceAsync(string partNum, string shipToNum, string custNum, string orderDate)
        {
            return this.DataProvider.GetProductPriceAsync(partNum, shipToNum, custNum, orderDate);
        }

        public List<UomConvApiResponseModel> GetUomConversions(List<string> partNums)
        {
            return this.DataProvider.GetUomConversions(partNums);
        }

        public ApiResponse PostOrderToApi(OrderApiDto orderData)
        {
            return this.DataProvider.PostOrderToApi(orderData);
        }

        public bool SyncAllOrdersToEpicore()
        {
            return this.DataProvider.SyncAllOrdersToEpicore();
        }
    }
}
