using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IUserRoleService
    {
        #region UserRole
         
        RoleDetails GetUserRoleBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        RoleDetails GetUserRole(int currentUserId, int userId, int startIndex, int pageSize);
         
        UserRoles CreateUserRole(int currentUserId, UserRoles roleEn, string pageName);

        UserRoles UpdateUserRole(int currentUserId, UserRoles roleEn, string pageName);
         
        bool DeleteUserRole(int currentUserId, int roleId, string pageName);
         
        UserRoles GetRoleById(int id);
         
        bool RoleExists(UserRoles role);

        #endregion
    }
}
