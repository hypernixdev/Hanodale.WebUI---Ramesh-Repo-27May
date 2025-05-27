using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IReportService
    {
        #region Reports

        /// <summary>
        /// This method is to get report by user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>report category list</returns>
        List<Reports> GetReportByUser(int userId);

        #endregion
    }
}
