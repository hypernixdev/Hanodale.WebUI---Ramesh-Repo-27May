using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class SchedulerLogService : ISchedulerLogService
    {
        public Hanodale.DataAccessLayer.Interfaces.ISchedulerLogService DataProvider;

        public SchedulerLogService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.SchedulerLogService();
        }

        public SchedulerLogDetails GetSchedulerLog(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetSchedulerLogBySearch(entityFilter);
        }

        public SchedulerLogs SaveSchedulerLog(SchedulerLogs entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateSchedulerLog(entityEn);
            else
                return this.DataProvider.CreateSchedulerLog(entityEn);
        }

        public bool DeleteSchedulerLog(int id)
        {
            return this.DataProvider.DeleteSchedulerLog(id);
        }

        public SchedulerLogs GetSchedulerLogById(int id)
        {
            return this.DataProvider.GetSchedulerLogById(id);
        }

        public bool IsSchedulerLogExists(SchedulerLogs entityEn)
        {
            return this.DataProvider.IsSchedulerLogExists(entityEn);
        }
    }
}
