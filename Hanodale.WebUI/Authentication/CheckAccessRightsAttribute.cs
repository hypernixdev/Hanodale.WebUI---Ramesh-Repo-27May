using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Models;

namespace Hanodale.WebUI.Authentication
{
    public class CheckAccessRightsAttribute : AuthorizeAttribute
    {

        // Custom property
        public string PageName { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// Allows only verified users to proceed, else force unverified users to the change password page.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (true)
            {
                filterContext.Result = new RedirectResult("~/ChangePassword");
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

        /// <summary>
        /// This method is to get the access rights for the given page
        /// </summary>
        /// <returns>AccessRights</returns>
        public static AccessRightsModel GetUserRights(int userId, string pageUrl)
        {
            AccessRightsModel _userRights = new AccessRightsModel();
            try
            {
                IUserRightsService svc = new UserRightsService();
                var userRights = svc.GetUserAccess(userId, pageUrl);

                if (userRights != null)
                {
                    _userRights.canView = userRights.canView;
                    _userRights.canAdd = userRights.canAdd;
                    _userRights.canEdit = userRights.canEdit;
                    _userRights.canDelete = userRights.canDelete;

                    _userRights.pageName = userRights.subMenus.subMenuName;
                    _userRights.pageId = userRights.subMenus.id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user rights");
            }
            return _userRights;
        }
    }
}