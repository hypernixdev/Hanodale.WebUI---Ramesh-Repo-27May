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
    public interface IAdhocReportService
    {
        #region AdhocReport

        AdhocReportDetails GetAdhocReportBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        AdhocReportDetails GetAdhocReport(int currentUserId, int userId, int startIndex, int pageSize);

        AdhocReports CreateAdhocReport(int currentUserId, AdhocReports entity, string pageName);

        AdhocReports UpdateAdhocReport(int currentUserId, AdhocReports entity, string pageName);

        bool DeleteAdhocReport(int currentUserId, int id, string pageName);

        AdhocReports GetAdhocReportById(int id);

        List<AdhocReports> GetListAdhocReport();

        int IsAdhocReportExists(AdhocReports entity);

       // AdhocReports SubmitAdhocReport(int currentUserId, AdhocReports entity, string pageName);

        #endregion
    }
}
