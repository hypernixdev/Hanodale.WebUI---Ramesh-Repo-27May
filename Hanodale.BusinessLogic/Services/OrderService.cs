using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs.Order;
using Hanodale.Entity.Core;
using System.ServiceModel;

namespace Hanodale.BusinessLogic
{
    public class OrderService : IOrderService
    {
        public Hanodale.DataAccessLayer.Interfaces.IOrderService DataProvider;

        public OrderService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.OrderService();
        }

       /* public List<Order> GetOrderList(Order entityEn)
        {
            return this.DataProvider.GetOrderList(entityEn);
        }*/

        public OrderDetails GetOrder(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetOrderBySearch(entityFilter);
        }

        public Orders SaveOrder(Orders entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateOrder(entityEn);
            else
                return this.DataProvider.CreateOrder(entityEn);
        }

        public bool DeleteOrder(int id)
        {
            return this.DataProvider.DeleteOrder(id);
        }

        public Orders GetOrderById(int id)
        {
            return this.DataProvider.GetOrderById(id);
        }

        public Order SubmitOrder(SubmitOrder entity, int userId)
        {
            return this.DataProvider.SubmitOrder(entity, userId);
        }

        public bool SubmitScannedItems(SubmitOrderScan entity, int userId, bool IsVerification)
        {
            return this.DataProvider.SubmitScannedItems(entity, userId, IsVerification);
        }

        public bool SubmitReturnedItems(SubmitReturnItems entity, int userId)
        {
            return this.DataProvider.SubmitReturnedItems(entity, userId);
        }

        public bool OrderScanned(OrderScanned entity)
        {
            return this.DataProvider.OrderScanned(entity);
        }

        public Order UpdateOrders(UpdateOrder entity, int userId)
        {
            return this.DataProvider.UpdateOrders(entity, userId);
        }

        public List<OrderScanned> getOrderScannedItems(int orderId)
        {
            return this.DataProvider.getOrderScannedItems(orderId);
        }

        public ViewOrder GetOrderDetails(int id)
        {
            return this.DataProvider.GetOrderDetails(id);
        }

        public OrderApiDto GetOrderApiData(int id)
        {
            return this.DataProvider.GetOrderApiData(id);
        }

        public bool UpdateOrderSyncStatus(OrderSyncStatusDto data)
        {
            return this.DataProvider.UpdateOrderSyncStatus(data);
        }

        public bool UpdateOrderStatus(int orderId, string newStatus, int userId, string actionName, string remark = "")
        {
            return this.DataProvider.UpdateOrderStatus(orderId, newStatus, userId, actionName, remark);
        }
        public bool CreateOrderPayment(List<CreateOrderPayment> entities, bool IsRefund)
        {
            return this.DataProvider.CreateOrderPayment(entities, IsRefund);
        }

        public bool IsOrderExists(Orders entityEn)
        {
            return this.DataProvider.IsOrderExists(entityEn);
        }

        public ProductBarcode GetProductCartons(string serialNo, string barcodeType)
        {
            return this.DataProvider.GetProductCartons(serialNo, barcodeType);
        }

        public ProductBarcode GetProductWeightBarcodes(string serialNo)
        {
            return this.DataProvider.GetProductWeightBarcodes(serialNo);
        }
        public bool DeleteScanItem(int orderId)
        {
            return this.DataProvider.DeleteScanItem(orderId);
        }
        public List<OrderUpdates> GetOrderLog(int orderId)
        {
            return this.DataProvider.GetOrderLog(orderId);

        }

        public List<ChartPanelInfo> GetChartPanelInfo(ChartPanelInfo entity)
        {
            return this.DataProvider.GetChartPanelInfo(entity);

        }

        public BarChartPanelOrderInfo GetBarChartPanelOrderInfo(BarChartPanelOrderInfo entity)
        {
            return this.DataProvider.GetBarChartPanelOrderInfo(entity);

        }
        public OrderItemDiscountApproval OrderItemDiscountApproval(OrderItemDiscountApproval entity)
        {
            return this.DataProvider.OrderItemDiscountApproval(entity);

        }
        public SalesSummaryResult GetSalesSummary(DateTime dateFrom,DateTime dateTo)
        {
            return this.DataProvider.GetSalesSummary(dateFrom, dateTo);

        }



    }
}
