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
    public interface ISchedulerLogService
    {
        SchedulerLogDetails GetSchedulerLogBySearch(DatatableFilters entityFilter);

        SchedulerLogs CreateSchedulerLog(SchedulerLogs entityEn);

        SchedulerLogs UpdateSchedulerLog(SchedulerLogs entityEn);

        bool DeleteSchedulerLog(int id);

        SchedulerLogs GetSchedulerLogById(int id);

        bool IsSchedulerLogExists(SchedulerLogs entityEn);
    }
}
