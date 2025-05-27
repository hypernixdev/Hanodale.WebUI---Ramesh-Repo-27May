using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Authentication

       

        public Hanodale.DataAccessLayer.Interfaces.IAuthenticationService DataProvider;

        public AuthenticationService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.AuthenticationService();
        }

        public AuthenticateUser DigestAuthentication(int uid)
        {
            return this.DataProvider.DigestAuthentication(uid);
        }

        public AuthenticateUser AuthenticateUser(string userName, string password)
        {
            return this.DataProvider.AuthenticateUser(userName, password);
        }

        public AuthenticateUser AuthenticateUserByUserId(string emilAddress, string password, int user_Id)
        {
            return this.DataProvider.AuthenticateUserByUserId(emilAddress, password, user_Id);
        }

        public bool ChangePassword(Users userEn, string newPassword, string pageName)
        {
            return this.DataProvider.ChangePassword(userEn, newPassword, pageName);
        }

        //public User ForgotPassword(string emailId) {
        //    return this.DataProvider.ForgotPassword(emailId);//
        //}

        //public void UpdateUserPassword(User user, bool markAsVerified) {
        //    this.DataProvider.UpdateUserPassword(user, markAsVerified);
        //}

        //public bool UpdateTerms(int userId, bool isAccepted) {
        //    return this.DataProvider.UpdateTerms(userId, isAccepted);
        //}

        #endregion
    }
}
