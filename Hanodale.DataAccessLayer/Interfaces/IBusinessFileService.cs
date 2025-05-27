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
    public interface IBusinessFileService
    {
        #region BusinessFile

        BusinessFileDetails GetBusinessFileBySearch(int currentUserId, int userId,int businessId, int startIndex, int pageSize, string search);

        BusinessFileDetails GetBusinessFile(int currentUserId, int userId, int businessId, int startIndex, int pageSize);

        BusinessFiles CreateBusinessFile(int currentUserId, BusinessFiles entity, string pageName);

        BusinessFiles UpdateBusinessFile(int currentUserId, BusinessFiles entity, string pageName);

        bool DeleteBusinessFile(int currentUserId, int id, string pageName);

        BusinessFiles GetBusinessFileById(int id);

        List<BusinessFiles> GetListBusinessFile();

        bool IsBusinessFileExists(BusinessFiles entity);

        #endregion
    }
}
