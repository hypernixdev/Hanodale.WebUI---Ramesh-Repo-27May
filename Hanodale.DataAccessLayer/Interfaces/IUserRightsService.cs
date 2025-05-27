using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;
 
namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IUserRightsService
    {
        #region UserRights

        UserRights GetUserAccess(int userId, string pageUrl);

        List<UserRoles> GetUserRoles(int userId);

        List<Menu> GetUserRightsByRole(int roleId);

        bool CreateUserRights(List<UserRights> lstUserRights, string pageName);

        bool UpdateUserRights(List<UserRights> lstUserRights, string pageName);

        #endregion
    }
}
