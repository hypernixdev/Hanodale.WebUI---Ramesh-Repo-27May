using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IBusinessFileService
    {
        #region  BusinessFile

        BusinessFileDetails GetBusinessFile(int currentUserId, int userId, int businessId, int startIndex, int pageSize, string search);

        BusinessFiles SaveBusinessFile(int currentUserId, BusinessFiles entity, string pageName);

        bool DeleteBusinessFile(int currentUserId, int id, string pageName);

        BusinessFiles GetBusinessFileById(int id);

        List<BusinessFiles> GetListBusinessFile();

        bool IsBusinessFileExists(BusinessFiles entity); 

        #endregion
    }
}
