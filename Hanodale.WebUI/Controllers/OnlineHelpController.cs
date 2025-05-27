using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using System.Web.Mvc;
namespace Hanodale.WebUI.Controllers
{
    public partial class OnlineHelpController : Controller
    {
        //
        // GET: /OnlineHelp/

        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult OnlineHelp()
        {
            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, "OnlineHelp", null)
            });
        }
       
    }
}
