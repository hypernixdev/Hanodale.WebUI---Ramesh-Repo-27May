using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class WorkCategoryService : IWorkCategoryService
    {
        #region WorkCategory

        public Hanodale.DataAccessLayer.Interfaces.IWorkCategoryService DataProvider;

        public WorkCategoryService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.WorkCategoryService();
        }

        public WorkCategoryDetails GetWorkCategory(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetWorkCategory(currentUserId, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetWorkCategoryBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public WorkCategorys SaveWorkCategory(int currentUserId, WorkCategorys entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateWorkCategory(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateWorkCategory(currentUserId, entity, pageName);
        }

        public bool DeleteWorkCategory(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteWorkCategory(currentUserId, id, pageName);
        }

        public WorkCategorys GetWorkCategoryById(int id)
        {
            return this.DataProvider.GetWorkCategoryById(id);
        }

        public bool IsWorkCategoryExists(WorkCategorys entity)
        {
            return this.DataProvider.IsWorkCategoryExists(entity);
        }

        public List<WorkCategorys> GetListWorkCategory()
        {
            return this.DataProvider.GetListWorkCategory();
        }

        #endregion


    }
}
