using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic; 

namespace Hanodale.BusinessLogic
{
    public class BusinessAddressService : IBusinessAddressService
    {
        #region BusinessAddress

        public Hanodale.DataAccessLayer.Interfaces.IBusinessAddressService DataProvider;

        public BusinessAddressService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.BusinessAddressService();
        }

        public BusinessAddressDetails GetBusinessAddress(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetBusinessAddress(currentUserId, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetBusinessAddressBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public BusinessAddresses SaveBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateBusinessAddress(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateBusinessAddress(currentUserId, entity, pageName);
        }

        public bool DeleteBusinessAddress(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteBusinessAddress(currentUserId, id, pageName);
        }

        public BusinessAddresses GetBusinessAddressById(int id)
        {
            return this.DataProvider.GetBusinessAddressById(id);
        }

        public bool IsBusinessAddressExists(BusinessAddresses entity)
        {
            return this.DataProvider.IsBusinessAddressExists(entity);
        }

        public List<BusinessAddresses> GetListBusinessAddress()
        {
            return this.DataProvider.GetListBusinessAddress();
        }
        #endregion

       
    }
}
