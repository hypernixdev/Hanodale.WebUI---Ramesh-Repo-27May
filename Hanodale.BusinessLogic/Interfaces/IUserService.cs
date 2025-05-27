using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IUserService
    {
        #region User

        UserDetails GetUser(int currentUserId, int businessId, int startIndex, int pageSize, string search, int businessTypeId, int organization_Id, bool all, bool isActive);

        Users SaveUser(int currentUserId, Users userEn);

        int IsUserExists(Users userEn);

        Users ResetPassword(Users userEn);

        Users GetUserById(int currentUserId, int id);

        Users GetUserByMCId(int currentUserId, int id, int mainCostId);

        Users GetUserBySCId(int currentUserId, int id, int subCostId);

        bool DeleteUser(int currentUserId, int id);

        List<Users> GetListUserByStaff(int businessTypeId, bool check, int[] userIds = null);

        List<Users> GetUserBySC(int currentUserId, int subCostId);

        List<Users> GetListUserByBusinessId(int businessId, int businessType_Id, bool check, int[] userIds = null);

        List<Users> GetListUserByBusinessTypeId(int businessTypeId, int organizationId, int user_Id, bool check, int[] userIds = null);

        AssignedBusinesss GetUserBuinessById(int currentUserId);

        List<Users> GetListUserByStaffMember(int businessTypeId, int busnessId, int organizationId, int user_Id, bool check, int[] userIds = null);

        Users SaveBusinessUser(int currentUserId, Users userEn);

        Users GetUserDetailsByUserName(string userName);

        List<Users> GetListUserBySupplier(int businessTypeId, int business_Id, bool check, int[] userIds = null);

        List<Users> GetListUserBySection(int businessTypeId, int business_Id, int organizationId, int user_Id, bool check, int[] userIds = null);

        Users ChangePassword(Users userEn);

        string GetProfileFileName(int id);

        Users UpdateUserInfo(Users userEn);

        #endregion
    }
}
