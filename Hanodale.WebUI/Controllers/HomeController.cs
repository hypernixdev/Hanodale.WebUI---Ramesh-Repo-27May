using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;
using Hanodale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Hanodale.Utility.Globalize;

namespace Hanodale.WebUI.Controllers
{
    public partial class HomeController : AuthorizedController
    {
        private readonly ICommonService svcCommon;

        public HomeController(ICommonService _commonService)
        {
            this.svcCommon = _commonService;
        }

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [Authorize]
        public void LogJavaScriptError(string message)
        {
            throw new ErrorException(message);
        }

        [HttpPost]
        [AppAuthorize]
        public virtual ActionResult ErrorPage()
        {
            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, "Error", null)
            });

            //return this.RedirectToRoute("Error"); 
        }
    }
}
