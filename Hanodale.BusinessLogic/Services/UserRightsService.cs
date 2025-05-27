using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hanodale.BusinessLogic
{
    public class UserRightsService : IUserRightsService
    {
        #region User Rights

        public Hanodale.DataAccessLayer.Interfaces.IUserRightsService DataProvider;

        public UserRightsService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.UserRightsService();
        }

        public UserRights GetUserAccess(int userId, string pageUrl)
        {
            return this.DataProvider.GetUserAccess(userId, pageUrl);
        }

        public List<UserRoles> GetUserRoles(int userId)
        {
            return this.DataProvider.GetUserRoles(userId);
        }

        public List<Menu> GetUserRightsByRole(int roleId)
        {
            return this.DataProvider.GetUserRightsByRole(roleId);
        }

        public bool SaveUserRights(List<UserRights> lstUserRights, string pageName)
        {
            //foreach (UserRights UserRights in lstUserRights)
            //{
            //    if (UserRights.id > 0)
            //    {
            //        return this.DataProvider.UpdateUserRights(lstUserRights, pageName);
            //    }
            //    else
            //    {
            //        return this.DataProvider.CreateUserRights(lstUserRights, pageName);
            //    }
            //}
            //return true;

            return this.DataProvider.UpdateUserRights(lstUserRights, pageName);
        }
        #endregion
    }
}
