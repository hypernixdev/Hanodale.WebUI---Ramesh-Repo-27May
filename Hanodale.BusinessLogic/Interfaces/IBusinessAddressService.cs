using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IBusinessAddressService
    {
        #region BusinessAddress

        BusinessAddressDetails GetBusinessAddress(int currentUserId, int userId, int startIndex, int pageSize, string search);

        BusinessAddresses SaveBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName);

        bool DeleteBusinessAddress(int currentUserId, int id, string pageName);

        BusinessAddresses GetBusinessAddressById(int id);

        List<BusinessAddresses> GetListBusinessAddress();

        bool IsBusinessAddressExists(BusinessAddresses entity);

        #endregion
    }
}
