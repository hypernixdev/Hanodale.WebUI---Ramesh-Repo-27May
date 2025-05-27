using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ISchedulerLogService
    {
        SchedulerLogDetails GetSchedulerLog(DatatableFilters entityFilter);

        SchedulerLogs SaveSchedulerLog(SchedulerLogs entityEn);

        bool DeleteSchedulerLog(int id);

        SchedulerLogs GetSchedulerLogById(int id);

        bool IsSchedulerLogExists(SchedulerLogs entityEn);
    }
}
