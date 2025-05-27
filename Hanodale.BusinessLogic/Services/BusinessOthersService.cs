using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic; 

namespace Hanodale.BusinessLogic
{
    public class BusinessOthersService : IBusinessOthersService
    {
        #region BusinessOtherss

        public Hanodale.DataAccessLayer.Interfaces.IBusinessOthersService DataProvider;

        public BusinessOthersService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.BusinessOthersService();
        }

        //public BusinessOthersDetails GetBusinessOthers(int currentUserId, int userId, int startIndex, int pageSize, string search)
        //{
        //    if (string.IsNullOrEmpty(search))
        //        return this.DataProvider.GetBusinesOtherss(currentUserId, userId, startIndex, pageSize);
        //    else
        //        return this.DataProvider.GetBusinessOthersBySearch(currentUserId, userId, startIndex, pageSize, search);
        //}

        public BusinessOtherss SaveBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateBusinessOthers(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateBusinessOthers(currentUserId, entity, pageName);
        }

        //public bool DeleteBusinessOtherss(int currentUserId, int id, string pageName)
        //{
        //    return this.DataProvider.DeleteBusinessOtherss(currentUserId, id, pageName);
        //}

        public BusinessOtherss GetBusinessOthersById(int id)
        {
            return this.DataProvider.GetBusinessOthersById(id);
        }

        public bool IsBusinessOthersExists(BusinessOtherss entity)
        {
            return this.DataProvider.IsBusinessOthersExists(entity);
        }

        public List<BusinessOtherss> GetListBusinessOthers()
        {
            return this.DataProvider.GetListBusinessOthers();
        }
        #endregion

       
    }
}
