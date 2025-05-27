using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IBusinessOthersService
    {
        #region BusinessOtherss

        //BusinessOthersDetails GetBusinessOtherss(int currentUserId, int userId, int startIndex, int pageSize, string search);

        BusinessOtherss SaveBusinessOthers(int currentUserId, BusinessOtherss entity, string pageName);

        //bool DeleteBusinessOtherss(int currentUserId, int id, string pageName);

        BusinessOtherss GetBusinessOthersById(int id);

        List<BusinessOtherss> GetListBusinessOthers();

        bool IsBusinessOthersExists(BusinessOtherss entity);

        #endregion
    }
}
