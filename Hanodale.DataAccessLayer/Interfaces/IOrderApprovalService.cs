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
    public interface IOrderApprovalService
    {
        OrderApprovalDetails GetOrderApprovalBySearch(DatatableFilters entityFilter);

        OrderApprovals CreateOrderApproval(OrderApprovals entityEn);

        OrderApprovals UpdateOrderApproval(OrderApprovals entityEn);

        bool DeleteOrderApproval(int id);

        OrderApprovals GetOrderApprovalById(int id);

        bool IsOrderApprovalExists(OrderApprovals entityEn);

    }
}
