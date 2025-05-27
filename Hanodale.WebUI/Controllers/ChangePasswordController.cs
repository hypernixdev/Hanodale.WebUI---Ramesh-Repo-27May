
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Microsoft.Practices.ServiceLocation;
using System.Web.Mvc;
using System.Globalization;
using Hanodale.Domain.Models;
using Hanodale.WebUI.Models;
using System.Configuration;
using Hanodale.Utility;
using Hanodale.Utility.Globalize;
using System;
using Hanodale.WebUI.Logging.Elmah;
using System.ServiceModel;
using System.Web.Configuration;
using Hanodale.Domain.DTOs;
namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class ChangePasswordController : AuthorizedController
    {

        #region Initialize

        private readonly IAuthenticationService svc; private readonly ICommonService svcCommon;
        private readonly IUserService svcUser;

        public ChangePasswordController(IAuthenticationService _bLService, ICommonService _commonService
            , IUserService _userService)
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcUser = _userService;
        }
        #endregion

        [Authorize]
        public virtual ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        [AppAuthorize]
        public virtual ActionResult ChangePassword(bool isJason = true)
        {
            ChangePasswordModel _model = new ChangePasswordModel();
            if (isJason)
            {
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ChangePassword.Views.Index, _model)
                });
            }
            else
            {
                return PartialView(MVC.ChangePassword.Views.Index, _model);
            }
        }

        #region ChangePassword
        [Authorize]
        public virtual JsonResult SaveChangePassword(ChangePasswordModel passwordModel)
        {
            try
            {
                string currentPasswordHash = Common.MD5(passwordModel.oldPassword) + ConfigurationManager.AppSettings["Encryption"].ToString();
                string newPasswordHash = Common.MD5(passwordModel.newPassword) + ConfigurationManager.AppSettings["Encryption"].ToString();

                //ChannelFactory<IService> configurationProxy = new ChannelFactory<IService>("ConfigurationService");
                //IService svc = configurationProxy.CreateChannel();

                if (svc != null)
                {
                    Users userEn = new Users();
                    userEn.id = this.CurrentUserId;
                    userEn.passwordHash = currentPasswordHash;
                    userEn.verified = true;

                    userEn.modifiedDate = DateTime.Now;
                    userEn.modifiedBy = this.UserName;

                    bool isSuccess = svc.ChangePassword(userEn, newPasswordHash, "First Password Change");
                    if (isSuccess)
                    {

                        //Get user details
                        var user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.email))
                            {
                                try
                                {
                                    // string _body = Helpers.MailSetting.OpenFile("~/Email/ChangedPasswordBody.txt");
                                    string _body = Helpers.MailSetting.OpenFile(ConfigurationManager.AppSettings["EmailPath"] + @"\" + "ChangedPasswordBody.txt");
                                    Email _email = new Email();
                                    _email.Subject = Emails.ChangedPasswordSubject;
                                    _email.Description = WebHelper.Placeholders.ReplaceAll(_body, user); // WebHelper.Placeholders.ReplaceAll(Emails.ChangedPasswordBody, user);
                                    _email.ToId = user.email;
                                    _email.CcId = "";
                                    _email.createdDate = DateTime.Now;
                                    _email.createdBy = user.firstName;
                                    //MailSetting.SaveEmail(_email);
                                    WebHelper.Mail.SendMail(
                                    Emails.ChangedPasswordFrom, new string[] { user.email },
                                    Emails.ChangedPasswordSubject, _email.Description);
                                }
                                catch (Exception ex)
                                {
                                    //throw new Exception(ex.Message);
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.MSG_CHANGE_PASSWORD + " " + Resources.MESSAGE_BUT + " " + Resources.MSG_ERR_EMAIL
                                    });
                                }
                            }
                        }
                        return Json(new
                        {
                            status = Common.Status.Success.ToString(),
                            message = Resources.MSG_CHANGE_PASSWORD
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Error.ToString(),
                            message = Resources.MSG_ERR_CHANGE_PASSWORD
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Error.ToString(),
                        message = Resources.MSG_ERR_SERVICE
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        #endregion


        //#region Force Password Change 
        //[Authorize]
        //public JsonResult ForcePasswordChange(ChangePasswordModel passwordModel)
        //{
        //    try
        //    {

        //        ChannelFactory<IConfigurationService> configurationProxy = new ChannelFactory<IConfigurationService>("ConfigurationService");
        //        IConfigurationService svc = configurationProxy.CreateChannel();

        //        if (svc != null)
        //        {
        //            //Get user details
        //            var user = svc.GetUserById(this.CurrentUserId);

        //            string currentPasswordHash = user.passwordHash;

        //            string newPasswordHash = Common.MD5(passwordModel.newPassword) + ConfigurationManager.AppSettings["Encryption"].ToString();


        //            User userEn = new User();
        //            userEn.id = this.CurrentUserId;
        //            userEn.passwordHash = currentPasswordHash;

        //            userEn.modifiedDate = DateTime.Now;
        //            userEn.modifiedBy = this.UserName;
        //            userEn.verified = true;

        //            bool isSuccess = svc.ChangePassword(userEn, newPasswordHash, "Change Password");
        //            if (isSuccess)
        //            {
        //                //if (user != null)
        //                //{
        //                //    if (!string.IsNullOrEmpty(user.email))
        //                //    {
        //                //        try
        //                //        {
        //                //            //Send well come email
        //                //            WebHelper.Mail.SendMail(
        //                //            Emails.ChangedPasswordFrom, new string[] { user.email },
        //                //            Emails.ChangedPasswordSubject, WebHelper.Placeholders.ReplaceAll(Emails.ChangedPasswordBody, user));
        //                //        }
        //                //        catch (Exception ex)
        //                //        {
        //                //            throw new Exception(ex.Message);
        //                //        }
        //                //    }
        //                //}
        //                return Json(new
        //                {
        //                    status = Common.Status.Success.ToString(),
        //                    message = Resources.MSG_CHANGE_PASSWORD
        //                });
        //            }
        //            else
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Error.ToString(),
        //                    message = Resources.MSG_ERR_CHANGE_PASSWORD
        //                });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                status = Common.Status.Error.ToString(),
        //                message = Resources.MSG_ERR_SERVICE
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ErrorException(ex.Message);
        //    }
        //}
        //#endregion

        [HttpPost]
        public virtual ActionResult Dashboard()
        {
            return this.RedirectToRoute("Dashboard");
        }
    }
}
