using Hanodale.Domain.DTOs.Order;
using Hanodale.Domain.DTOs.Sync;
using Hanodale.SyncService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService
{
    public interface ISyncManager
    {
        SyncReponse SyncEntity(string entityName, string apiUrl, string uniqueField, bool disablePagination, string customDbModel);

        SyncReponse SyncEntity(string entityName, string apiUrl, string uniqueField, bool disablePagination, string customDbModel, int pageNumber);
        ProductPriceApiModel GetProductPriceAsync(string partNum, string shipToNum, string custNum, string orderDate);
        List<UomConvApiResponseModel> GetUomConversions(List<string> partNums);
        ApiResponse PostOrderToApi(OrderApiDto orderData);

        bool SyncAllOrdersToEpicore();
    }
}
