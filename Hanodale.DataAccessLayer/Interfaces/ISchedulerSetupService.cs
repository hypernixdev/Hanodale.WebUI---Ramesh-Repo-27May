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
    public interface ISchedulerSetupService
    {
        SchedulerSetupDetails GetSchedulerSetupBySearch(DatatableFilters entityFilter);

        SchedulerSetups CreateSchedulerSetup(SchedulerSetups entityEn);

        SchedulerSetups UpdateSchedulerSetup(SchedulerSetups entityEn);

        bool DeleteSchedulerSetup(int id);

        SchedulerSetups GetSchedulerSetupById(int id);

        bool IsSchedulerSetupExists(SchedulerSetups entityEn);
    }
}
