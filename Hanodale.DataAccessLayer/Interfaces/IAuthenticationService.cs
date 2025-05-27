using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;
using System.Threading.Tasks;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IAuthenticationService
    {
        #region Authenticate user

        AuthenticateUser DigestAuthentication(int uid);

        AuthenticateUser AuthenticateUser(string userName, string password);

        AuthenticateUser AuthenticateUserByUserId(string emilAddress, string password, int user_Id);

        /// <summary>
        /// This method is to change the password
        /// </summary>
        /// <param name="userEn">User entity</param>
        /// <param name="newPassword">new password</param>
        /// <param name="pageName">page name</param>
        /// <returns></returns>
       bool ChangePassword(Users userEn, string newPassword, string pageName);

        //User ForgotPassword(string emailId);
        //void UpdateUserPassword(User user, bool markAsVerified);

        //bool UpdateTerms(int userId, bool isAccepted);//

        #endregion
    }
}
