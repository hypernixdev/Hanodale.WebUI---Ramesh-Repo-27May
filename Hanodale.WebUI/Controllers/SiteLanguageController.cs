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
using System.Globalization;
using System.Threading;
using System.Web;

namespace Hanodale.WebUI.Controllers
{
    public partial class SiteLanguageController : Controller
    {
        private readonly ICommonService svcCommon;

        public SiteLanguageController(ICommonService _commonService)
        {
            this.svcCommon = _commonService;
        }

        public virtual ActionResult GetLanguage()
        {
            try
            {
                var lst = GetLanguageList();

                var _model = new LanguageModel();
                if (lst != null)
                {
                    _model.defaultLanguage = lst.FirstOrDefault(p => p.isDefault);
                    _model.lstLanguage = lst.ToList();
                }

                return View("_LanguageSection", _model);

            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }

        }

        private List<LanguageItemModel> GetAvailableLanguageList(string cultureName)
        {
            try
            {
                var lstLanguageItem = new List<LanguageItemModel>();

                if (svcCommon != null)
                {
                    var lstAvailableLanguage = svcCommon.GetAvailableLanguageList();
                    if (lstAvailableLanguage != null)
                    {
                        if (string.IsNullOrEmpty(cultureName) || !lstAvailableLanguage.Any(p => p.culture == cultureName))
                        {
                            var obj = lstAvailableLanguage.FirstOrDefault(p => p.isDefault);
                            if (obj != null)
                            {
                                cultureName = obj.culture;
                            }
                            else
                            {
                                cultureName = Common.defaultSystemLanguageCulture;
                            }
                        }

                        lstLanguageItem = lstAvailableLanguage.Select(p => new LanguageItemModel { cultureName = p.culture, flagSymbol = p.flagIconName, languageName = p.name, visibility = p.visibility, isDefault = (p.culture==cultureName) }).ToList();

                        var defaultItem=lstLanguageItem.FirstOrDefault(p=>p.isDefault);

                        if (defaultItem == null)
                        {
                            lstLanguageItem[0].isDefault=true;
                        }
                    }
                }

                return lstLanguageItem;
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        private List<LanguageItemModel> GetLanguageList()
        {
            try
            {

                var langCookie = Request.Cookies[Common.cookieCultrueName];
                var cultureName = string.Empty;
                if (langCookie != null && !string.IsNullOrEmpty(langCookie.Value))
                {
                    cultureName = langCookie.Value;
                }
               
                var availableLanguages = GetAvailableLanguageList(cultureName);

                return availableLanguages;
            }
            catch
            {
                return null;
            }
        }


        //public bool IsLanguageAvailable(string cultureName, List<LanguageItemModel> availableLanguages)
        //{
        //    return availableLanguages.Any(p => p.cultureName == cultureName && p.visibility);
        //}

        public string GetDefaultLanguageCulture(List<LanguageItemModel> availableLanguages)
        {
            var obj = availableLanguages.FirstOrDefault(p => p.isDefault && p.visibility);
            if (obj != null)
            {
                return obj.cultureName;
            }

            return Common.defaultSystemLanguageCulture;

        }

        private void SetLanguage(string cultureName)
        {
            var availableLanguages = GetAvailableLanguageList(cultureName);
           
                cultureName = GetDefaultLanguageCulture(availableLanguages);
            

            var cultureInfo = new CultureInfo(cultureName);

            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

            HttpCookie langCookie = new HttpCookie(Common.cookieCultrueName, cultureName);
            langCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(langCookie);
        }

        public virtual ActionResult ChangeLanguage(string id)
        {
            try
            {
                SetLanguage(id);
                if (Request.IsAuthenticated)
                {
                    return RedirectToRoute("Dashboard");
                }
                else
                {
                    //return RedirectToAction(MVC.Auth.SignIn());
                    return this.RedirectToAction("SignIn", "Auth");

                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}
