using Hanodale.BusinessLogic;
using Hanodale.Utility.Helpers;
using Microsoft.Practices.ServiceLocation;
using System.Globalization;
using System.Web.Mvc;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class CultureController : AuthorizedController
    {
        #region Initialize

        
        #endregion

        protected void InitializeCulture(int language)
        {
            if (language == 0)
            {
                return;
            }

            CultureHelper _helper = new CultureHelper();
            string _langName = _helper.GetCultureName(language);
            CultureInfo _culture = new CultureInfo(_langName);
            System.Threading.Thread.CurrentThread.CurrentUICulture = _culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_culture.Name);
        }
    }
}
