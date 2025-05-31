using Hanodale.Domain.DTOs;
using Hanodale.Domain.DTOs.Order;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IOrderService
    {
        OrderDetails GetOrder(DatatableFilters entityFilter);

        //List<Order> GetOrderList(Order entityEn);

        Orders SaveOrder(Orders entityEn);

        bool DeleteOrder(int id);

        Orders GetOrderById(int id);

        Order SubmitOrder(SubmitOrder entity, int userId);

        bool SubmitScannedItems(SubmitOrderScan entity, int userId, bool IsVerification);

        bool SubmitReturnedItems(SubmitReturnItems entity, int userId);

        Order UpdateOrders(UpdateOrder entity, int userId);
        List<OrderScanned> getOrderScannedItems(int orderId);
        ViewOrder GetOrderDetails(int id);
        OrderApiDto GetOrderApiData(int id);
        bool UpdateOrderSyncStatus(OrderSyncStatusDto data);
        bool UpdateOrderStatus(int orderId, string newStatus, int userId, string actionName, string remark);
        bool CreateOrderPayment(List<CreateOrderPayment> entities, bool IsRefund);
        bool IsOrderExists(Orders entityEn);
        bool OrderScanned(OrderScanned entity);
        ProductBarcode GetProductCartons(string serialNo, string barcodeType);
        List<ProductBarcode> GetStdLooseFromProductCartonTable(string serialNo, string barcodeType);
        ProductBarcode GetProductWeightBarcodes(string serialNo);

        bool DeleteScanItem(int orderId);
        List<OrderUpdates> GetOrderLog(int orderId);

        List<ChartPanelInfo> GetChartPanelInfo(ChartPanelInfo entity);

        BarChartPanelOrderInfo GetBarChartPanelOrderInfo(BarChartPanelOrderInfo entity);

        OrderItemDiscountApproval OrderItemDiscountApproval(OrderItemDiscountApproval entity);

        SalesSummaryResult GetSalesSummary(DateTime dateFrom, DateTime dateTo);

    }
}
