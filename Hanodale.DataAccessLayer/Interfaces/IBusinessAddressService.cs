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
    public interface IBusinessAddressService
    {
        #region BusinessAddress

        BusinessAddressDetails GetBusinessAddressBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        BusinessAddressDetails GetBusinessAddress(int currentUserId, int userId, int startIndex, int pageSize);

        BusinessAddresses CreateBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName);

        BusinessAddresses UpdateBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName);

        bool DeleteBusinessAddress(int currentUserId, int id, string pageName);

        BusinessAddresses GetBusinessAddressById(int id);

        List<BusinessAddresses> GetListBusinessAddress();

        bool IsBusinessAddressExists(BusinessAddresses entity);

        #endregion
    }
}
