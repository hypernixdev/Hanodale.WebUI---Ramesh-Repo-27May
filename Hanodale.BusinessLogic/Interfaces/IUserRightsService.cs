using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IUserRightsService 
    {
        #region User Rights
        UserRights GetUserAccess(int userId, string pageUrl);

        List<UserRoles> GetUserRoles(int userId);

        List<Menu> GetUserRightsByRole(int roleId);

        bool SaveUserRights(List<UserRights> lstUserRights, string pageName);
        #endregion
    }
}
