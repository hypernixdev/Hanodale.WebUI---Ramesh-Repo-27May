using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hanodale.BusinessLogic
{
    public class UserRoleService : IUserRoleService
    {
        #region UserRole

        public Hanodale.DataAccessLayer.Interfaces.IUserRoleService DataProvider;

        public UserRoleService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.UserRoleService();
        }

        public RoleDetails GetUserRole(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetUserRole(currentUserId, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetUserRoleBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public UserRoles SaveUserRole(int currentUserId, UserRoles roleEn, string pageName)
        {
            if (roleEn.id > 0)
                return this.DataProvider.UpdateUserRole(currentUserId, roleEn, pageName);
            else
                return this.DataProvider.CreateUserRole(currentUserId, roleEn, pageName); 
        }

        public bool DeleteUserRole(int currentUserId, int roleId, string pageName)
        {
            return this.DataProvider.DeleteUserRole(currentUserId, roleId, pageName);
        }

        public UserRoles GetRoleById(int id)
        {
            return this.DataProvider.GetRoleById(id);
        }

        public bool RoleExists(UserRoles role)
        {
            return this.DataProvider.RoleExists(role);
        }

        #endregion
    }
}
