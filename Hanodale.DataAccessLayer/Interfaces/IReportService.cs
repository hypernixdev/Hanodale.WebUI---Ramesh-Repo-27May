using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;


namespace Hanodale.DataAccessLayer.Interfaces
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
