using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IUserRoleService
    {
        #region UserRole

        /// <summary>
        /// This method is to get the user details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="endIndex">pagesize</param>
        /// <returns></returns> 

        RoleDetails GetUserRole(int currentUserId, int userId, int startIndex, int pageSize, string search);

        /// <summary>
        /// This method is to update the user role details
        /// </summary>
        /// <param name="startIndex">roleEn</param>  
        /// <returns></returns>
        UserRoles SaveUserRole(int currentUserId, UserRoles roleEn, string pageName);

        /// <summary>
        /// This method is to delete the user role details
        /// </summary>
        /// <param name="startIndex">roleId</param>  
        /// <returns></returns>
        bool DeleteUserRole(int currentUserId, int roleId, string pageName);

        /// <summary>
        /// This method is to get the role by role id
        /// </summary>
        /// <param name="roleId">role Id</param>
        /// <returns>Role details</returns> 
        UserRoles GetRoleById(int id);

        /// <summary>
        /// This method is to check the userrole exists or not.
        /// </summary>
        /// <param name="roleName">Role Name</param>
        bool RoleExists(UserRoles role);

        #endregion
    }
}
