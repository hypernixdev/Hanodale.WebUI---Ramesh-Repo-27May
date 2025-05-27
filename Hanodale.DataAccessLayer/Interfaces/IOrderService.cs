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
using OrderItemScanned = Hanodale.Entity.Core.OrderItemScanned;
using Hanodale.Domain.DTOs.Order;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IOrderService
    {
        OrderDetails GetOrderBySearch(DatatableFilters entityFilter);

        //List<Order> GetOrderList(Order entityEn);

        Orders CreateOrder(Orders entityEn);

        Orders UpdateOrder(Orders entityEn);

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
        ProductBarcode GetProductWeightBarcodes(string serialNo);
        ProductBarcode GetProductCartons(string serialNo, string barcodeType);
         bool DeleteScanItem(int orderId);

        List<OrderUpdates> GetOrderLog(int orderId);

        List<ChartPanelInfo> GetChartPanelInfo(ChartPanelInfo entity);

        BarChartPanelOrderInfo GetBarChartPanelOrderInfo(BarChartPanelOrderInfo entity);

        OrderItemDiscountApproval OrderItemDiscountApproval(OrderItemDiscountApproval entity);

        SalesSummaryResult GetSalesSummary(DateTime dateFrom, DateTime dateTo);
    }
}
