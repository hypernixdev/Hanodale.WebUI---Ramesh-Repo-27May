using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic; 

namespace Hanodale.BusinessLogic
{
    public class BusinessClassificationService : IBusinessClassificationService
    {
        #region BusinessClassification

        public Hanodale.DataAccessLayer.Interfaces.IBusinessClassificationService DataProvider;

        public BusinessClassificationService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.BusinessClassificationService();
        }

        public BusinessClassificationDetails GetBusinessClassification(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetBusinessClassification(currentUserId, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetBusinessClassificationBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public BusinessClassifications SaveBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName)
        {
            return this.DataProvider.UpdateBusinessClassification(currentUserId, entity, pageName);
            //if (entity.id > 0)
            //    return this.DataProvider.UpdateBusinessClassification(currentUserId, entity, pageName);
            //else
            //    return this.DataProvider.CreateBusinessClassification(currentUserId, entity, pageName);
        }

        public bool DeleteBusinessClassification(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteBusinessClassification(currentUserId, id, pageName);
        }

        public BusinessClassifications GetBusinessClassificationById(int id)
        {
            return this.DataProvider.GetBusinessClassificationById(id);
        }

        public bool IsBusinessClassificationExists(BusinessClassifications entity)
        {
            return this.DataProvider.IsBusinessClassificationExists(entity);
        }

        public List<BusinessClassifications> GetListBusinessClassification()
        {
            return this.DataProvider.GetListBusinessClassification();
        }

        public List<BusinessClassifications> GetListBusinessClassificationByBusinessId(int id)
        {
            return this.DataProvider.GetListBusinessClassificationByBusinessId(id);
        }

        #endregion
       
    }
}
