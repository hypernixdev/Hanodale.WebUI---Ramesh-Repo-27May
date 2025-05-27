using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic; 

namespace Hanodale.BusinessLogic
{
    public class AdhocReportService : IAdhocReportService
    {
        #region AdhocReport

        public Hanodale.DataAccessLayer.Interfaces.IAdhocReportService DataProvider;

        public AdhocReportService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.AdhocReportService();
        }

        public AdhocReportDetails GetAdhocReport(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetAdhocReport(currentUserId, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetAdhocReportBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public AdhocReports SaveAdhocReport(int currentUserId, AdhocReports entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateAdhocReport(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateAdhocReport(currentUserId, entity, pageName);
        }

        public bool DeleteAdhocReport(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteAdhocReport(currentUserId, id, pageName);
        }

        public AdhocReports GetAdhocReportById(int id)
        {
            return this.DataProvider.GetAdhocReportById(id);
        }

        public int IsAdhocReportExists(AdhocReports entity)
        {
            return this.DataProvider.IsAdhocReportExists(entity);
        }

        public List<AdhocReports> GetListAdhocReport()
        {
            return this.DataProvider.GetListAdhocReport();
        }

        #endregion

       
    }
}
