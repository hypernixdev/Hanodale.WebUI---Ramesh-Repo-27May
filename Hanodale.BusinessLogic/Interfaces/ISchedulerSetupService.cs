using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ISchedulerSetupService
    {
        SchedulerSetupDetails GetSchedulerSetup(DatatableFilters entityFilter);

        SchedulerSetups SaveSchedulerSetup(SchedulerSetups entityEn);

        bool DeleteSchedulerSetup(int id);

        SchedulerSetups GetSchedulerSetupById(int id);

        bool IsSchedulerSetupExists(SchedulerSetups entityEn);
    }
}
