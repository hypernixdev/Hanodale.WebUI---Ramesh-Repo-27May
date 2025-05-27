using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class ReportService : IReportService
    {
        #region Report

        public Hanodale.DataAccessLayer.Interfaces.IReportService DataProvider;

        public ReportService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.ReportService();
        }

        public List<Reports> GetReportByUser(int userId)
        {
            return this.DataProvider.GetReportByUser(userId);
        }

        #endregion
    }
}
