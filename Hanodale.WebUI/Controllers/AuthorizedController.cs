using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Microsoft.Practices.ServiceLocation;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace Hanodale.WebUI.Controllers
{
     [NoCache]
     [AppCultureAttribute]
    public partial class AuthorizedController : Controller
    {
        protected readonly IAuthenticationService UserServices;

        public AuthorizedController()
        {

        }
        public AuthorizedController(IAuthenticationService userServices) { }

        /// <summary>
        /// Retrieves the CurrentUserId as stored in the <see cref="PMSIdentity"/>
        /// </summary>
        /// <remarks>
        /// Using this method requires the user to be authorized.
        /// </remarks>
        protected int CurrentUserId
        {
            get { return this.User.PMSIdentity().UserId; }
        }

        protected int CurrentCompanyId
        {
            get { return this.User.PMSIdentity().CompanyId; }
        }

        protected int SubCostCenter
        {
            get { return this.User.PMSIdentity().SubCostCenter; }
        }

        protected string UserName
        {
            get { return this.User.PMSIdentity().DisplayName; }
        }



        /// <summary>
        /// Retrieves the CurrentUser SessionId as stored in the <see cref="PMSIdentity"/>
        /// </summary>
        /// <remarks>
        /// Using this method requires the user to be authorized.
        /// </remarks>
        protected string SessionId
        {
            get { return this.User.PMSIdentity().SessionId; }
        }

        //private User currentUser;

        //private List<UserRoles> role;

        /// <summary>
        /// Returns the current user or recovers the user from the <see cref="PMSIdentity"/>.
        /// </summary>
        /// <remarks>
        /// Using this method requires the user to be authorized.
        /// </remarks>
        //public User CurrentUser
        //{
        //    get
        //    {
        //        return this.UserServices.GetUserFromIdentity(this.User.BlackSunIdentity());

        //    }
        //}

        //public List<UserRoles> UserRolesList
        //{
        //    get
        //    {
        //        return this.role ??
        //               (this.role = this.UserServices.GetUserRoleFromIdentity(this.User.BlackSunIdentity()));
        //    }
        //}

        //protected override void ExecuteCore()
        //{
        //    int culture = 0;
        //    if (this.Session == null || this.Session["CurrentCulture"] == null)
        //    {

        //        int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
        //        this.Session["CurrentCulture"] = culture;
        //    }
        //    else
        //    {
        //        culture = (int)this.Session["CurrentCulture"];
        //    }
        //    // calling CultureHelper class properties for setting  
        //    CultureHelper.CurrentCulture = culture;

        //    base.ExecuteCore();
        //}

        //protected override bool DisableAsyncSupport
        //{
        //    get { return true; }
        //} 

    }
}