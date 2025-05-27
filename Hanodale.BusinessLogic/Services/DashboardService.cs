using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class DashboardService : IDashboardService
    {
        #region Dashboard

        public Hanodale.DataAccessLayer.Interfaces.IDashboardService DataProvider;

        public DashboardService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.DashboardService();
        }

        public Dashboards GetDashoardByUser(int userId,int subCostCenter)
        {
            return this.DataProvider.GetDashoardByUser(userId, subCostCenter);
        }

        public OrderPaymentTotals GetOrderPaymentTotals(DateTime startDate, DateTime endDate)
        {
            return this.DataProvider.GetOrderPaymentTotals(startDate,endDate);
        }

        public List<HelpDesks> GetNewTickets(int userID, int organizationId)
        {
            return this.DataProvider.GetNewTickets(userID, organizationId);
        }

        #endregion
    }
}
