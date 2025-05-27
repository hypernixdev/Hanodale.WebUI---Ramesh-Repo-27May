using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hanodale.BusinessLogic
{
    public class UserService : IUserService
    {
        #region User

        public Hanodale.DataAccessLayer.Interfaces.IUserService DataProvider;

        public UserService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.UserService();
        }

        public UserDetails GetUser(int currentUserId, int businessId, int startIndex, int pageSize, string search, int businessTypeId, int organization_Id, bool all, bool _isActive)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetUser(currentUserId, businessId, startIndex, pageSize, businessTypeId, organization_Id, all, _isActive);
            else
                return this.DataProvider.GetUserBySearch(currentUserId, businessId, startIndex, pageSize, search, businessTypeId, organization_Id, all, _isActive);
        }

        public Users SaveUser(int currentUserId, Users userEn)
        {
            if (userEn.id > 0)
                return this.DataProvider.UpdateUser(currentUserId, userEn);
            else
                return this.DataProvider.CreateUser(currentUserId, userEn);
        }

        public int IsUserExists(Users userEn)
        {
            return this.DataProvider.IsUserExists(userEn);
        }

        public Users ResetPassword(Users userEn)
        {
            return this.DataProvider.ResetPassword(userEn);
        }

        public Users GetUserById(int currentUserId, int id)
        {
            return this.DataProvider.GetUserById(currentUserId, id);
        }

        public Users GetUserByMCId(int currentUserId, int id, int mainCostId)
        {
            return this.DataProvider.GetUserByMCId(currentUserId, id, mainCostId);
        }

        public Users GetUserBySCId(int currentUserId, int id, int subCostId)
        {
            return this.DataProvider.GetUserBySCId(currentUserId, id, subCostId);
        }

        public bool DeleteUser(int currentUserId, int id)
        {
            return this.DataProvider.DeleteUser(currentUserId, id);
        }
        public List<Users> GetListUserByStaff(int businessTypeId, bool check, int[] userIds)
        {
            return this.DataProvider.GetListUserByStaff(businessTypeId, check, userIds);
        }

        public List<Users> GetUserBySC(int currentUserId, int subCostId)
        {
            return this.DataProvider.GetUserBySC(currentUserId, subCostId);
        }

        public List<Users> GetListUserByBusinessId(int businessId, int businessType_Id, bool check, int[] userIds)
        {
            return this.DataProvider.GetListUserByBusinessId(businessId, businessType_Id, check, userIds);
        }

        public List<Users> GetListUserByBusinessTypeId(int businessTypeId, int organizationId, int user_Id, bool check, int[] userIds)
        {
            return this.DataProvider.GetListUserByBusinessTypeId(businessTypeId, organizationId, user_Id, check, userIds);
        }

        public AssignedBusinesss GetUserBuinessById(int currentUserId)
        {
            return this.DataProvider.GetUserBuinessById(currentUserId);
        }

        public List<Users> GetListUserByStaffMember(int businessTypeId, int businessId, int organizationId, int user_Id, bool check, int[] userIds)
        {
            return this.DataProvider.GetListUserByStaffMember(businessTypeId, businessId, organizationId, user_Id, check, userIds);
        }

        public Users SaveBusinessUser(int currentUserId, Users userEn)
        {
            if (userEn.id > 0)
                return this.DataProvider.UpdateBusinessUser(currentUserId, userEn);
            else
                return this.DataProvider.CreateBusinessUser(currentUserId, userEn);
        }

        public Users GetUserDetailsByUserName(string userName)
        {
            return this.DataProvider.GetUserDetailsByUserName(userName);
        }

        public List<Users> GetListUserBySupplier(int businessTypeId, int business_Id, bool check, int[] userIds)
        {
            return this.DataProvider.GetListUserBySupplier(businessTypeId, business_Id, check, userIds);
        }

        public List<Users> GetListUserBySection(int businessTypeId, int business_Id, int organizationId, int user_Id, bool check, int[] userIds)
        {
            return this.DataProvider.GetListUserBySection(businessTypeId, business_Id, organizationId, user_Id, check, userIds);
        }

        public Users ChangePassword(Users userEn)
        {
            return this.DataProvider.ChangePassword(userEn);
        }

        public string GetProfileFileName(int id)
        {
            return this.DataProvider.GetProfileFileName(id);
        }

        public Users UpdateUserInfo(Users userEn)
        {
            return this.DataProvider.UpdateUserInfo(userEn);
        }
        #endregion
    }
}
