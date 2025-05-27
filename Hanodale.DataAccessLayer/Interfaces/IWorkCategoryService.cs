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
    public interface IWorkCategoryService
    {
        #region WorkCategory

        WorkCategoryDetails GetWorkCategoryBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        WorkCategoryDetails GetWorkCategory(int currentUserId, int userId, int startIndex, int pageSize);

        WorkCategorys CreateWorkCategory(int currentUserId, WorkCategorys entity, string pageName);

        WorkCategorys UpdateWorkCategory(int currentUserId, WorkCategorys entity, string pageName);

        bool DeleteWorkCategory(int currentUserId, int id, string pageName);

        WorkCategorys GetWorkCategoryById(int id);

        List<WorkCategorys> GetListWorkCategory();

        bool IsWorkCategoryExists(WorkCategorys entity);

        #endregion
    }
}
