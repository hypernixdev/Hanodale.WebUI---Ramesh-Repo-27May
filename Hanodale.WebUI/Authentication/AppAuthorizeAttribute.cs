using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Hanodale.BusinessLogic;

namespace Hanodale.WebUI.Authentication
{
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Allows only verified users to proceed, else force unverified users to the change password page.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //ChannelFactory<IService> configurationProxy = new ChannelFactory<IService>("Service");
            //IService confService = configurationProxy.CreateChannel();
            //int userId = Convert.ToInt32(filterContext.HttpContext.User.Identity.Name);

            //var user = confService.GetUserById(userId, userId);

            // Redirect the user to the force change password page if the user has not been verified
            
            //if (user != null && !(user.isTermsAccepted ?? false))
            //{
            //    filterContext.Controller.TempData["RequiresPasswordChange"] = true;
            //    filterContext.Result = new RedirectResult("~/Terms");
            //}


            //if (user != null && !((bool)user.verified))
            //{
            //    filterContext.Controller.TempData["RequiresPasswordChange"] = true;
            //    filterContext.Result = new RedirectResult("~/ChangePassword");
            //}
        }
    }

    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            var cache = GetCache(filterContext);

            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            cache.SetValidUntilExpires(false);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Get the reponse cache
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual HttpCachePolicyBase GetCache(ResultExecutingContext filterContext)
        {
            return filterContext.HttpContext.Response.Cache;
        }
    }
}