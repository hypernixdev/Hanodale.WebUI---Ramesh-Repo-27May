using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class SchedulerSetupService : ISchedulerSetupService
    {
        public Hanodale.DataAccessLayer.Interfaces.ISchedulerSetupService DataProvider;

        public SchedulerSetupService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.SchedulerSetupService();
        }

        public SchedulerSetupDetails GetSchedulerSetup(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetSchedulerSetupBySearch(entityFilter);
        }

        public SchedulerSetups SaveSchedulerSetup(SchedulerSetups entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateSchedulerSetup(entityEn);
            else
                return this.DataProvider.CreateSchedulerSetup(entityEn);
        }

        public bool DeleteSchedulerSetup(int id)
        {
            return this.DataProvider.DeleteSchedulerSetup(id);
        }

        public SchedulerSetups GetSchedulerSetupById(int id)
        {
            return this.DataProvider.GetSchedulerSetupById(id);
        }

        public bool IsSchedulerSetupExists(SchedulerSetups entityEn)
        {
            return this.DataProvider.IsSchedulerSetupExists(entityEn);
        }
    }
}
