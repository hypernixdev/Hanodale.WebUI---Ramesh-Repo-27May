using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IAdhocReportService
    {
        #region AdhocReport

        AdhocReportDetails GetAdhocReport(int currentUserId, int userId, int startIndex, int pageSize, string search);

        AdhocReports SaveAdhocReport(int currentUserId, AdhocReports entity, string pageName);

        bool DeleteAdhocReport(int currentUserId, int id, string pageName);

        AdhocReports GetAdhocReportById(int id);

        List<AdhocReports> GetListAdhocReport();

        int IsAdhocReportExists(AdhocReports entity);

        #endregion
    }
}
