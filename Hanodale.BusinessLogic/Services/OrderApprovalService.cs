using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class OrderApprovalService : IOrderApprovalService
    {
        public Hanodale.DataAccessLayer.Interfaces.IOrderApprovalService DataProvider;

        public OrderApprovalService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.OrderApprovalService();
        }

        public OrderApprovalDetails GetOrderApproval(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetOrderApprovalBySearch(entityFilter);
        }

        public OrderApprovals SaveOrderApproval(OrderApprovals entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateOrderApproval(entityEn);
            else
                return this.DataProvider.CreateOrderApproval(entityEn);
        }

        public bool DeleteOrderApproval(int id)
        {
            return this.DataProvider.DeleteOrderApproval(id);
        }

        public OrderApprovals GetOrderApprovalById(int id)
        {
            return this.DataProvider.GetOrderApprovalById(id);
        }

        public bool IsOrderApprovalExists(OrderApprovals entityEn)
        {
            return this.DataProvider.IsOrderApprovalExists(entityEn);
        }

       
    }
}
