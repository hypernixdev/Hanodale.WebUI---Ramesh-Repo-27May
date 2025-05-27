using Hanodale.BusinessLogic;
using Hanodale.WebUI.Logging.Elmah;
using Microsoft.Practices.ServiceLocation;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class TermsController : AuthorizedController
    {

        #region Initialize
        public TermsController()
            
        {

        }
        #endregion

        [Authorize]
        public virtual ActionResult Index()
        {
            return View();
        }

        #region ChangePassword
        [HttpPost]
        public virtual ActionResult UpdateTerms(FormCollection collection)
        {
            bool isUserAccepted = false;
            try
            {
                if (!string.IsNullOrEmpty(collection["chkAgree"]))
                 {
                     string checkResp = collection["chkAgree"];
                     bool chkAgree =Convert.ToBoolean(checkResp);

                    //update the terms and condition
                     //get Dashboard count
                         isUserAccepted = true; //svc.UpdateTerms(this.CurrentUserId, chkAgree);
                 }
                if (isUserAccepted)
                {
                    return RedirectToRoute("Dashboard");
                }
                else
                {
                    return RedirectToRoute("Terms");
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        #endregion

    }
}
