using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IOrderApprovalService
    {
        OrderApprovalDetails GetOrderApproval(DatatableFilters entityFilter);

        OrderApprovals SaveOrderApproval(OrderApprovals entityEn);

        bool DeleteOrderApproval(int id);

        OrderApprovals GetOrderApprovalById(int id);

        bool IsOrderApprovalExists(OrderApprovals entityEn);

    }
}
