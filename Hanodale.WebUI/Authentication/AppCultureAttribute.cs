using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Hanodale.BusinessLogic;
using System.Web.Security;
using Hanodale.Utility.Helpers;

namespace Hanodale.WebUI.Authentication
{
    public class AppCultureAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Allows only verified users to proceed, else force unverified users to the change password page.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            //IUserService svc = new UserService();
            //int userId = int.Parse(filterContext.HttpContext.User.Identity.Name);
            //var user = svc.GetUserById(userId, userId);

            //if (user != null)
            //{
            //    CultureHelper _helper = new CultureHelper();
            //    string _langName = _helper.GetCultureName(Convert.ToInt32(user.language));
            //    CultureInfo _culture = new CultureInfo(_langName);
            //    System.Threading.Thread.CurrentThread.CurrentUICulture = _culture;
            //    System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_culture.Name);
            //}
            //else
            //{
            //    //force kickout if user is authenticated but account is deleted
            //    if (HttpContext.Current.User.Identity.IsAuthenticated)
            //    {
            //        FormsAuthentication.SignOut();
            //        FormsAuthentication.RedirectToLoginPage();
            //    }
            //}
        }

    }
}