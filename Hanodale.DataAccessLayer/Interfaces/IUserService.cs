using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IUserService
    {
        #region User

        UserDetails GetUserBySearch(int currentUserId, int businessId, int startIndex, int pageSize, string search, int businessTypeId, int organization_Id, bool all, bool isActive);

        UserDetails GetUser(int currentUserId, int businessId, int startIndex, int pageSize, int businessTypeId, int organization_Id, bool all, bool isActive);

        Users CreateUser(int currentUserId, Users userEn);

        Users UpdateUser(int currentUserId, Users userEn);

        int IsUserExists(Users userEn);

        Users ResetPassword(Users userEn);

        Users GetUserById(int currentUserId, int id);

        Users GetUserByMCId(int currentUserId, int id, int mainCostId);

        Users GetUserBySCId(int currentUserId, int id, int subCostId);

        bool DeleteUser(int currentUserId, int id);

        List<Users> GetListUserByStaff(int businessTypeId, bool check, int[] userIds);

        List<Users> GetUserBySC(int currentUserId, int subCostId);

        List<Users> GetListUserByBusinessId(int businessId, int businessType_Id, bool check, int[] userIds);

        List<Users> GetListUserByBusinessTypeId(int businessTypeId, int organizationId, int user_Id, bool check, int[] userIds);

        AssignedBusinesss GetUserBuinessById(int currentUserId);

        List<Users> GetListUserByStaffMember(int businessTypeId, int businessId, int organizationId, int user_Id, bool check, int[] userIds);

        Users CreateBusinessUser(int currentUserId, Users userEn);

        Users UpdateBusinessUser(int currentUserId, Users userEn);

        Users GetUserDetailsByUserName(string userName);

        List<Users> GetListUserBySupplier(int businessTypeId, int business_Id, bool check, int[] userIds);

        List<Users> GetListUserBySection(int businessTypeId, int business_Id, int organizationId, int user_Id, bool check, int[] userIds);

        Users ChangePassword(Users userEn);

        string GetProfileFileName(int id);

        Users UpdateUserInfo(Users userEn);

        #endregion
    }
}
