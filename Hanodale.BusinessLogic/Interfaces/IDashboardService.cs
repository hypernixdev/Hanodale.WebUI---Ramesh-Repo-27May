using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IDashboardService
    {
        #region Dashboard

        Dashboards GetDashoardByUser(int userId,int subCostCenter);
        OrderPaymentTotals GetOrderPaymentTotals(DateTime startDate, DateTime endDate);


        List<HelpDesks> GetNewTickets(int userID, int organizationId);

        #endregion
    }
}
