using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IWorkCategoryService
    {
        #region WorkCategory

        WorkCategoryDetails GetWorkCategory(int currentUserId, int userId, int startIndex, int pageSize, string search);

        WorkCategorys SaveWorkCategory(int currentUserId, WorkCategorys entity, string pageName);

        bool DeleteWorkCategory(int currentUserId, int id, string pageName);

        WorkCategorys GetWorkCategoryById(int id);

        List<WorkCategorys> GetListWorkCategory();

        bool IsWorkCategoryExists(WorkCategorys entity);

        #endregion
    }
}
