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
    public interface IBusinessOthersService
    {
        #region BusinessOtherss

        //BusinessOthersDetails GetBusinessOtherssBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        //BusinessOthersDetails GetBusinessOtherss(int currentUserId, int userId, int startIndex, int pageSize);

        BusinessOtherss CreateBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName);

        BusinessOtherss UpdateBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName);

        //bool DeleteBusinessOtherss(int currentUserId, int id, string pageName);

        BusinessOtherss GetBusinessOthersById(int id);

        List<BusinessOtherss> GetListBusinessOthers();

        bool IsBusinessOthersExists(BusinessOtherss entity);

        #endregion
    }
}
