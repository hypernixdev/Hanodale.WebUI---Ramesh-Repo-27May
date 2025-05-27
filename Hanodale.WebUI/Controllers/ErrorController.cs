using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Microsoft.Practices.ServiceLocation;
using System.Web.Mvc;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class ErrorController : AuthorizedController
    {
        
        [AppAuthorize]
        public virtual ViewResult Index()
        {
            return View("Error");
        }

        [AppAuthorize]
        public virtual ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View(MVC.Shared.Views.Error);
        }

        [AppAuthorize]
        public virtual ViewResult Error()
        {
            Response.StatusCode = 500;  //you may want to set this to 200
            return View(MVC.Shared.Views.Error);
        }
    }
}