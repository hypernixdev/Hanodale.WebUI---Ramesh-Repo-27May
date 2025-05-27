using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IAuthenticationService
    {
        #region Authenticate user

        AuthenticateUser AuthenticateUser(string userName, string password);

        AuthenticateUser DigestAuthentication(int uid);

        AuthenticateUser AuthenticateUserByUserId(string emilAddress, string password, int user_Id);

        bool ChangePassword(Users userEn, string newPassword, string pageName);

        //User ForgotPassword(string emailId);

        //void UpdateUserPassword(User user, bool markAsVerified);

        //bool UpdateTerms(int userId, bool isAccepted);//

        #endregion
    }
}
