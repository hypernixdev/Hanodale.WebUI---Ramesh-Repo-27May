using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic; 

namespace Hanodale.BusinessLogic
{
    public class BusinessFileService : IBusinessFileService
    {
        #region BusinessFile

        public Hanodale.DataAccessLayer.Interfaces.IBusinessFileService DataProvider;

        public BusinessFileService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.BusinessFileService();
        }

        public BusinessFileDetails GetBusinessFile(int currentUserId, int userId, int businessId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetBusinessFile(currentUserId, userId, businessId, startIndex, pageSize);
            else
                return this.DataProvider.GetBusinessFileBySearch(currentUserId, userId, businessId, startIndex, pageSize, search);
        }

        public BusinessFiles SaveBusinessFile(int currentUserId, BusinessFiles entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateBusinessFile(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateBusinessFile(currentUserId, entity, pageName);
        }

        public bool DeleteBusinessFile(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteBusinessFile(currentUserId, id, pageName);
        }

        public BusinessFiles GetBusinessFileById(int id)
        {
            return this.DataProvider.GetBusinessFileById(id);
        }

        public bool IsBusinessFileExists(BusinessFiles entity)
        {
            return this.DataProvider.IsBusinessFileExists(entity);
        }

        public List<BusinessFiles> GetListBusinessFile()
        {
            return this.DataProvider.GetListBusinessFile();
        }
        #endregion

       
    }
}
