
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class ProfileController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "User/UserDetails";
        #endregion

        #region Constructor
        private readonly IUserService svc; private readonly ICommonService svcCommon;
        public ProfileController(IUserService profileServices)//., IServiceLocator serviceLocator)
        {
            this.svc = profileServices;
        }
        #endregion

        #region Profile Details
        public virtual ActionResult Index()
        {
            var user = svc.GetUserById(this.CurrentUserId, this.CurrentUserId);
            var obj = new UserModel();
            obj.firstName = user.firstName;
            obj.lastName = user.lastName;
            obj.userName = user.userName;
            obj.roleName = user.roleName;
            obj.email = user.email;           

            return View(obj);
        }
        #endregion

        public virtual ActionResult Edit(int id)
        {
            var user = svc.GetUserById(this.CurrentUserId, this.CurrentUserId);
            var obj = new UserModel();
            
            obj.firstName = user.firstName;
            obj.lastName = user.lastName;
            obj.userName = user.userName;
            obj.roleName = user.roleName;
            obj.email = user.email;            

            return View(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_userModule"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(int id, UserModel _userModule)
        {
            return RedirectToAction("Index");
        }

    }
}
