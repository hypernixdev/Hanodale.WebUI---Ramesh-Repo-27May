using System;
using System.Web.Security;
using Hanodale.Domain.Models;

namespace Hanodale.WebUI.Authentication
{
    public class UserAuthenticationTicketBuilder
    {
        /// <summary>
        /// Creates a new <see cref="FormsAuthenticationTicket"/> from a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <remarks>
        /// Encodes the <see cref="UserInfo"/> into the <see cref="FormsAuthenticationTicket.UserData"/> property
        /// of the authentication ticket.  This can be recovered by using the <see cref="UserInfo.FromString"/> method.
        /// </remarks>
        public static FormsAuthenticationTicket CreateAuthenticationTicket(User user)
        {
            UserInfo userInfo = CreateUserContextFromUser(user);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.AuthorizationId,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                userInfo.ToString());

            return ticket;
        }

        private static UserInfo CreateUserContextFromUser(User user)
        {
            var userContext = new UserInfo
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                ClaimsIdentifier = user.AuthorizationId,
                SessionId =user.SessionId,
                SubCostCenter=user.SubCostCenter
            };

            return userContext;
        }
    }
}