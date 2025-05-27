using Hanodale.Utility.Globalize;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Hanodale.WebUI.Models;
using System.Linq;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Web.Configuration;
using Hanodale.Utility;
using Hanodale.WebUI.Logging.Elmah;

namespace Hanodale.WebUI.Controllers
{
    public partial class AuthController : Controller
    {
        #region Constructor

        private readonly IAuthenticationService svc;
        private readonly IFormsAuthentication formsAuthentication;
        private readonly IUserService svcUser;
        private readonly INewsService svcNews;
        private readonly IDashboardService svcDashboard;
        

        [InjectionConstructor]
        public AuthController(IAuthenticationService _bLService, IFormsAuthentication _formsAuthentication,
                              IUserService _userServices, INewsService _newsServices, IDashboardService _dashboardService)
        {
            this.svc = _bLService;
            this.formsAuthentication = _formsAuthentication;
            this.svcUser = _userServices;
            this.svcNews = _newsServices;
            this.svcDashboard = _dashboardService;
        }

        #endregion

        public virtual ActionResult Index(string userName, string screen)
        {
            LoginModel _model = new LoginModel();
            try
            {
                int _uid = Convert.ToInt32(userName);
                var _auth = this.svc.DigestAuthentication(_uid);
                _model.UserName = _auth.name;
                _model.Password = _auth.pwd;
                _model.isDigestAuthentication = true;
                return SignIn(_model, null, screen);
            }
            catch (Exception err)
            {
                _model.message = err.InnerException == null ? err.Message : err.InnerException.Message;
                return View(_model.message);
            }
        }

        #region Bulletin
        public virtual ActionResult GetNews()
        {
            int totalRecordCount = 0;
            IEnumerable<UserRoles> filteredUserRoles = null;
            try
            {
                
                

                //Newss obj = new Newss();
                //obj.loggedDate= DateTime.Now.AddMonths(-1);

                var lst = svcNews.GetNews(0, false, 0, 0, 100, null, DateTime.Now.AddMonths(-1));
                return PartialView(MVC.Auth.Views._Bulletin, lst.lstNews);
            }
            catch (Exception err)
            {
                throw;
            }
        }

        #endregion

        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0)]
        public virtual ActionResult SignIn()
        {

            this.ViewData["login"] = "visible";
            if (Request.IsAuthenticated)
            {
                return this.RedirectToRoute("Dashboard");
            }

            GetFooterInfo();

            return View();
        }

        public virtual FileResult GetReport()
        {
            string ReportURL =  Common.GetPath("PDF_Vendor_Files/HRMS User Manual - Vendor.pdf");
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
           
        }

        public virtual ActionResult DownloadFile(string path)
        {
            string ReportURL = Common.GetPath("PDF_Files/" + path);
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, path);
        }  

        private void GetFooterInfo()
        {
            ViewData["CompanyName"] = WebConfigurationManager.AppSettings["CompanyName"];
            ViewData["CompanyAddress"] = WebConfigurationManager.AppSettings["CompanyAddress"];
            ViewData["CompanyPhone"] = WebConfigurationManager.AppSettings["CompanyPhone"];
            ViewData["CompanyFax"] = WebConfigurationManager.AppSettings["CompanyFax"];
            ViewData["PersonName"] = WebConfigurationManager.AppSettings["PersonName"];
            ViewData["PersonDepartment"] = WebConfigurationManager.AppSettings["PersonDepartment"];
            ViewData["PersonPhone"] = WebConfigurationManager.AppSettings["PersonPhone"];
            ViewData["PersonEmail"] = WebConfigurationManager.AppSettings["PersonEmail"];
        }

        public virtual ActionResult ForgotPassword()
        {
            GetFooterInfo();

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public virtual ActionResult SignIn(LoginModel entity, string toVerifyHash, string screen = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.Password))
                    {
                        if (string.IsNullOrEmpty(toVerifyHash))
                        {
                            this.ViewData["authenticationError"] = Resources.MSG_INVALID_USER_OR_PASSWORD;
                            ViewData["login"] = "visible";
                            ModelState.AddModelError("", Resources.MSG_INVALID_USER_OR_PASSWORD);
                            entity.Password = null;
                            GetFooterInfo();
                            return View(MVC.Auth.Views.SignIn, entity);
                        }
                    }
                    if (toVerifyHash == null)
                    {
                        toVerifyHash = "";
                    }

                    if (!string.IsNullOrEmpty(entity.UserName) && !string.IsNullOrEmpty(entity.Password))
                    {


                        if (!entity.isDigestAuthentication)
                        {
                            //Hash the password and send to service
                            entity.Password = Common.MD5(entity.Password) + ConfigurationManager.AppSettings["Encryption"].ToString();
                        }

                        var user = svc.AuthenticateUser(entity.UserName, entity.Password);

                        if (user == null)
                        {
                            this.ViewData["authenticationError"] = Resources.MSG_INVALID_USER_OR_PASSWORD;
                            ViewData["login"] = "visible";
                            //ModelState.Clear();
                            ModelState.AddModelError("", Resources.MSG_INVALID_USER_OR_PASSWORD);
                            entity.Password = null;
                            GetFooterInfo();
                            return View(MVC.Auth.Views.SignIn, entity);
                        }
                        else
                        {
                            if (user.expireDate != null)
                            {
                                var supplierBusinessType = ConfigurationManager.AppSettings["SupplierBusinessType"].ToString();
                                if (supplierBusinessType == user.supplierBusinessType_Id.ToString() && user.expireDate < DateTime.Now)
                                {
                                    this.ViewData["authenticationError"] = "Contract Expired! Please contact System Admin";
                                    ViewData["login"] = "visible";
                                    //ModelState.Clear();
                                    ModelState.AddModelError("", "Contract Expired! Please contact System Admin");
                                    entity.Password = null;
                                    GetFooterInfo();
                                    return View(MVC.Auth.Views.SignIn, entity);
                                }
                            }
                            if (user.status.ToLower() == "inactive")
                            {
                                this.ViewData["authenticationError"] = Resources.ACCOUNT_INACTIVE;
                                ViewData["login"] = "visible";
                                //ModelState.Clear();
                                ModelState.AddModelError("", Resources.MSG_USER_INACTIVE);
                                entity.Password = null;
                                GetFooterInfo();
                                return View(MVC.Auth.Views.SignIn, entity);
                            }
                            else
                            {
                                string sessionId = Guid.NewGuid().ToString();
                                this.formsAuthentication.SetAuthCookie(
                                   this.HttpContext,
                                   UserAuthenticationTicketBuilder.CreateAuthenticationTicket(new Domain.Models.User
                                   {
                                       UserId = user.id,
                                       DisplayName = user.name,// + ' ' + user.lastName,
                                       AuthorizationId = user.id.ToString(),
                                       Language = Convert.ToInt32(user.language),
                                       SessionId = sessionId,
                                       SubCostCenter = user.defaultOrganization
                                   }));
                                

                                if (!string.IsNullOrEmpty(screen))
                                {
                                    screen = screen.ToLower();
                                  
                                }
                             
                                if (screen == "ticket")
                                    return this.RedirectToAction(MVC.HelpDesk.Index());
                                else
                                    return this.RedirectToAction(MVC.Dashboard.Index());
                            }

                        }
                    }
                    else
                    {
                        this.ViewData["authenticationError"] = Resources.MSG_INVALID_USER_OR_PASSWORD;
                        ViewData["login"] = "visible";
                        ModelState.AddModelError("", Resources.MSG_INVALID_USER_OR_PASSWORD);
                        entity.Password = null;
                        GetFooterInfo();
                        return View(MVC.Auth.Views.SignIn, entity);
                    }
                }
            }
            catch (TimeoutException timeProblem)
            {
                this.ViewData["authenticationError"] = ("The service operation timed out. " + timeProblem.Message);
                ViewData["login"] = "visible";
                ModelState.AddModelError("", "The service operation timed out. " + timeProblem.Message);
                entity.Password = null;
                GetFooterInfo();
                return View(MVC.Auth.Views.SignIn, entity);
            }
            catch (FaultException ex)
            {
                // TODO: Log exception
                this.ViewData["authenticationError"] = Resources.MSG_INVALID_USER_OR_PASSWORD;
                ViewData["login"] = "visible";
                ModelState.AddModelError("", Resources.MSG_INVALID_USER_OR_PASSWORD);
                entity.Password = null;
                GetFooterInfo();
                return View(MVC.Auth.Views.SignIn, entity);

            }
            catch (CommunicationException commProblem)
            {
                this.ViewData["authenticationError"] = ("There was a communication problem. " + commProblem.Message);
                ViewData["login"] = "visible";
                ModelState.AddModelError("", "There was a communication problem. " + commProblem.Message);
                entity.Password = null;
                GetFooterInfo();
                return View(MVC.Auth.Views.SignIn, entity);
            }

            GetFooterInfo();
            return View(MVC.Auth.Views.SignIn);
        }


        public virtual JsonResult ReleaseSession(int id)
        {
            try
            {

                
                
                string sessionId = Guid.NewGuid().ToString();
                var userId = Convert.ToInt32(this.User.Identity.Name);
                var user = svcUser.GetUserBySCId(userId, userId, id);
                if (user != null)
                {
                    if (user.businessExpiredDate != null)
                    {
                        var supplierBusinessType = ConfigurationManager.AppSettings["SupplierBusinessType"].ToString();
                        if (supplierBusinessType == user.supplierBusinessType_Id.ToString() && user.businessExpiredDate < DateTime.Now)
                        {
                            SignOut();
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                            });

                        }
                    }
                    this.formsAuthentication.SetAuthCookie(
                        this.HttpContext,
                        UserAuthenticationTicketBuilder.CreateAuthenticationTicket(new Domain.Models.User
                        {
                            UserId = user.id,
                            DisplayName = user.firstName,// + ' ' + user.lastName,
                            AuthorizationId = user.id.ToString(),
                            Language = Convert.ToInt32(user.language),
                            SessionId = sessionId,
                            SubCostCenter = id,
                            //TimeOut = FormsAuthentication.Timeout
                        }));
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Error.ToString(),
                    });
                }
                return Json(new
                {
                    status = Common.Status.Success.ToString(),
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = Common.Status.Error.ToString(),
                });
            }
        }

        public virtual ActionResult SetSubCostCenter(int id)
        {
            try
            {
                
                
                string sessionId = Guid.NewGuid().ToString();
                var userId = Convert.ToInt32(this.User.Identity.Name);
                var user = svcUser.GetUserBySCId(userId, userId, id);
                if (user != null)
                {
                    if (user.businessExpiredDate != null)
                    {
                        var supplierBusinessType = ConfigurationManager.AppSettings["SupplierBusinessType"].ToString();
                        if (supplierBusinessType == user.supplierBusinessType_Id.ToString() && user.businessExpiredDate < DateTime.Now)
                        {
                            SignOut();
                            return this.RedirectToAction(MVC.Auth.SignIn());

                        }
                    }
                    this.formsAuthentication.SetAuthCookie(
                        this.HttpContext,
                        UserAuthenticationTicketBuilder.CreateAuthenticationTicket(new Domain.Models.User
                        {
                            UserId = user.id,
                            DisplayName = user.firstName,// + ' ' + user.lastName,
                            AuthorizationId = user.id.ToString(),
                            Language = Convert.ToInt32(user.language),
                            SessionId = sessionId,
                            SubCostCenter = id
                        }));
                }
                else
                {
                    return this.RedirectToAction(MVC.Auth.SignIn());
                }

                return this.RedirectToRoute("Dashboard");

            }
            catch (Exception ex)
            {
                return this.RedirectToAction(MVC.Auth.SignIn());
            }
        }

        public virtual ActionResult SetMainCostCenter(int id)
        {
            try
            {
                
                
                string sessionId = Guid.NewGuid().ToString();
                var userId = Convert.ToInt32(this.User.Identity.Name);
                var user = svcUser.GetUserByMCId(userId, userId, id);
                if (user != null)
                {
                    if (user.businessExpiredDate != null)
                    {
                        var supplierBusinessType = ConfigurationManager.AppSettings["SupplierBusinessType"].ToString();
                        if (supplierBusinessType == user.supplierBusinessType_Id.ToString() && user.businessExpiredDate < DateTime.Now)
                        {
                            SignOut();
                            return this.RedirectToAction(MVC.Auth.SignIn());

                        }
                    }
                    this.formsAuthentication.SetAuthCookie(
                        this.HttpContext,
                        UserAuthenticationTicketBuilder.CreateAuthenticationTicket(new Domain.Models.User
                        {
                            UserId = user.id,
                            DisplayName = user.firstName,// + ' ' + user.lastName,
                            AuthorizationId = user.id.ToString(),
                            Language = Convert.ToInt32(user.language),
                            SessionId = sessionId,
                            SubCostCenter = user.defaultOrganizationId
                        }));
                }
                else
                {
                    return this.RedirectToAction(MVC.Auth.SignIn());
                }

                return this.RedirectToRoute("Dashboard");

            }
            catch (Exception ex)
            {
                return this.RedirectToAction(MVC.Auth.SignIn());
            }
        }

        public virtual ActionResult AccountIdentify(string toVerifyHash)
        {
            if (string.IsNullOrEmpty(toVerifyHash))
            {
                return this.RedirectToAction(MVC.Auth.SignIn());
            }

            try
            {
                return SignIn(null, toVerifyHash);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                //this.ViewData["authenticationError"] = Resources.AuthC_SingInFailed_Msg;
                return View(MVC.Auth.Views.SignIn);
            }
        }

        public virtual ActionResult SignOut()
        {
            this.formsAuthentication.Signout();
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            HttpCookie authCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            authCookie.Value = null;

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            if (cookie1 != null)
            {
                cookie1.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie1);
            }

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)

            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            if (cookie1 != null)
            {
                cookie2.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie2);
            }

            //this.formsAuthentication.Signout();
            //FormsAuthentication.SignOut();



            //Request.Cookies.Remove("UserId");

            //HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (cookie != null)
            //{
            //    cookie.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(cookie);
            //}

            //// Invalidate the Cache on the Client Side
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
            //Response.Cache.SetExpires(DateTime.Now.AddDays(-30));
            //HttpContext.Session.Abandon();
            //Session.Clear();
            //Session.Abandon();
            //Session.RemoveAll();


            //this.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-30));
            //this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //this.Response.Cache.SetNoStore();

            //HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            //if (authCookie != null)
            //{
            //    authCookie.Expires = DateTime.Now.AddYears(-10);
            //    Response.Cookies.Add(authCookie);
            //}

            //HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId", "");
            //if (sessionCookie != null)
            //{
            //    sessionCookie.Expires = DateTime.Now.AddYears(-10);
            //    Response.Cookies.Add(sessionCookie);
            //}
            return this.RedirectToAction(MVC.Auth.SignIn());
        }

        public virtual JsonResult GetListNews()
        {
            try
            {
                
                
                var result = svcNews.GetListNews();
                return Json(result);
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }


        [HttpPost]
        public virtual ActionResult ForgotPassword(UserModel userMoel)
        {
             ViewData["CompanyName"] = WebConfigurationManager.AppSettings["CompanyName"];
            ViewData["CompanyAddress"] = WebConfigurationManager.AppSettings["CompanyAddress"];
            ViewData["CompanyPhone"] = WebConfigurationManager.AppSettings["CompanyPhone"];
            ViewData["CompanyFax"] = WebConfigurationManager.AppSettings["CompanyFax"];
            ViewData["PersonName"] = WebConfigurationManager.AppSettings["PersonName"];
            ViewData["PersonDepartment"] = WebConfigurationManager.AppSettings["PersonDepartment"];
            ViewData["PersonPhone"] = WebConfigurationManager.AppSettings["PersonPhone"];
            ViewData["PersonEmail"] = WebConfigurationManager.AppSettings["PersonEmail"];

            try
            {
                
                

                if (svc != null)
                {
                    Users _userEn = new Users();
                    _userEn.id = 0;

                    _userEn.passwordHash = Common.MD5(ConfigurationManager.AppSettings["DefaultPassword"]) + ConfigurationManager.AppSettings["Encryption"].ToString();
                    _userEn.email = userMoel.email;
                    _userEn.verified = false;
                    _userEn.modifiedBy = "User";
                    _userEn.modifiedDate = DateTime.Now;

                    _userEn = svcUser.ResetPassword(_userEn);
                    
                    if (_userEn != null && !string.IsNullOrEmpty(userMoel.email))
                    {
                        if (!string.IsNullOrEmpty(_userEn.email))
                        {
                            //send mail if user has email id    
                            //var body = WebHelper.Placeholders.ReplaceAll(Emails.ForgotPasswordBody, _userEn);
                            string body = Helpers.MailSetting.OpenFile(ConfigurationManager.AppSettings["EmailPath"] + @"\" + "ForgotPasswordBody.txt");
                            body = WebHelper.Placeholders.ReplaceAll(body, _userEn);
                            body = WebHelper.Placeholders.ReplaceAll(body, "$USER_EMAIL$", _userEn.email);
                            body = WebHelper.Placeholders.ReplaceAll(body, "$USERNAME$", _userEn.userName);
                            body = WebHelper.Placeholders.ReplaceAll(body, "$PASSWORD_HASH$", ConfigurationManager.AppSettings["DefaultPassword"]);
                            body = WebHelper.Placeholders.ReplaceAll(body, "$URL$", ConfigurationManager.AppSettings["DeployedUrl"]);

                            try
                            {
                                  WebHelper.Mail.SendMail(Emails.UserCreatedFrom,
                                    new string[] { _userEn.email },
                                    Emails.UserPasswordResetSubject, body);
                                   ViewBag.Message = "The email was sent successfully.";

                            }
                            catch (Exception err)
                            {
                                ViewBag.ErrorMessage = "There was an issue sending the email. Please try again later.";


                                //throw new ErrorException("Please try again later!");
                            }
                        }

                        var entity = new UserModel();
                        entity.email = userMoel.email;
                        this.ViewData["authenticationError"] = Resources.MSG_SENT_FORGOT_EMAIL;
                        ViewData["login"] = "visible";
                        ModelState.AddModelError("", Resources.MSG_SENT_FORGOT_EMAIL);
                        return View(MVC.Auth.Views.ForgotPassword, entity);
                    }
                    else
                    {
                        var entity = new UserModel();
                        entity.email = userMoel.email;
                        this.ViewData["authenticationError"] = Resources.MSG_SENT_FORGOT_EMAIL;
                        ViewData["login"] = "visible";
                        ModelState.AddModelError("", Resources.MSG_INVALID_EMAIL);
                        return View(MVC.Auth.Views.ForgotPassword, entity);
                    }
                }
                else
                {
                    var entity = new UserModel();
                    entity.email = userMoel.email;
                    this.ViewData["authenticationError"] = Resources.MSG_SENT_FORGOT_EMAIL;
                    ViewData["login"] = "visible";
                    ModelState.AddModelError("", Resources.MSG_ERR_SERVICE);
                    return View(MVC.Auth.Views.ForgotPassword, entity);
                }


            }
            catch (Exception ex)
            {
                //hrow new ErrorException(ex.Message);
            }
            return View();
        }
    }
}
