using Hanodale.Utility.Globalize;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;
using System;
using System.Web.Mvc;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class DashboardController : AuthorizedController
    {

        #region Declaration
        const string PAGE_URL = "Dashboard/Index";
        #endregion

        #region Constructor

        private readonly IDashboardService svc; private readonly ICommonService svcCommon;
        private readonly IUserService svcUser;
        private readonly IBusinessService svcBusiness;

        public DashboardController(IDashboardService _bLService, ICommonService _commonService
            , IUserService _userService
, IBusinessService _businessService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcUser = _userService;
            this.svcBusiness = _businessService;
        }

        #endregion


        #region Views

        [HttpPost]
        [Authorize]
        public virtual JsonResult MyNotifications()
        {
            try
            {
                var ticket =  this.svc.GetNewTickets(this.CurrentUserId, this.SubCostCenter);
                List<UserNotificationModel> _list= new List<UserNotificationModel>();
                UserNotificationModel _model;
                foreach(var item in ticket)
                {
                    _model = new UserNotificationModel();
                    _model.date = item.createdDate.ToString("dd/MM/yyyy");
                    _model.message = "[" + item.name + "] " + item.feedback;
                    _list.Add(_model);
                }

                return Json(new
                {
                    loadDate = DateTime.Now.ToString(),
                    totalCount = ticket.Count,
                    notificationList = Common.RenderPartialViewToString(this, MVC.Dashboard.Views._NotificationBody, _list),
                    notificationCount = ticket.Count,
                    status = Common.Status.Success.ToString(),
                });
            }
            catch
            {
                return Json(new
                {
                    status = Common.Status.Error.ToString(),
                });
            }
        }

        public virtual ActionResult UserInfoNav()
        {
            UserModel obj = new UserModel();
            obj.firstName = this.UserName;
           
            string fileName = svcUser.GetProfileFileName(this.CurrentUserId);
            var _user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
            obj.businessType_Id = _user.bussinessType_Id;

            if (!string.IsNullOrEmpty(fileName))
            {
                string direction = System.Configuration.ConfigurationManager.AppSettings["UserProfileFilePath"];
                string filePath = System.IO.Path.Combine(@"" + direction, fileName);
                obj.avatarUrl = filePath;
            }
            else
            {
                obj.avatarUrl = "/Content/avatars/avatar2.png";
            }
            return PartialView(MVC.Dashboard.Views._UserInfoNav, obj);
        }

        #endregion

        #region Dashboard

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                var user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.landingPage))
                    {
                        return this.RedirectToRoute(user.landingPage);
                    }
                
                    //if (user.userRole_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["User_Role_Picker_Id"]))
                    //{
                    //    return this.RedirectToRoute("Pickup");
                    //}

                    //if (user.userRole_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["User_Role_Cashier_Id"]))
                    //{
                    //    return this.RedirectToRoute("Orders");
                    //}

                    //if (user.userRole_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["User_Role_Replinisher_Id"]))
                    //{
                    //    return this.RedirectToRoute("LooseConversion");
                    //}
                }

                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var lst = svc.GetDashoardByUser(this.CurrentUserId, this.SubCostCenter);
                        var obj = new DashboardModel();
                        var business = svcUser.GetUserBuinessById(this.CurrentUserId);
                        if (business != null)
                        {
                            var bus = svcBusiness.GetBusinessById(business.business_Id);
                            if (bus.businessType_Id == 52)
                            {
                                obj.check = true;
                            }
                        }
                        obj.lst = lst.itemList;
                        obj.ticketsCount = lst.ticketCount;
                        DateTime startDate = DateTime.Now;  // Current date as start date
                        DateTime endDate = DateTime.Now;
                        var totals = svc.GetOrderPaymentTotals(startDate, endDate);

                        obj.TotalSales = totals.TotalSales;
                        obj.TotalRefund = totals.TotalRefund;
                        //obj.dashboardBoxName= 1;

                        if (Request.IsAjaxRequest())
                        {
                            return Json(new
                            {
                                viewMarkup = Common.RenderPartialViewToString(this, MVC.Dashboard.Views.Index, obj),
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return View(obj);
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Error.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS
                        });

                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Error.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult Dashboard(string dashboardBoxName)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var lst = svc.GetDashoardByUser(this.CurrentUserId, this.SubCostCenter);
                        var obj = new DashboardModel();
                        var business = svcUser.GetUserBuinessById(this.CurrentUserId);
                        if (business != null)
                        {
                            var bus = svcBusiness.GetBusinessById(business.business_Id);
                            if (bus.businessType_Id == 52)
                            {
                                obj.check = true;
                            }
                        }
                        obj.lst = lst.itemList;
                        obj.ticketsCount = lst.ticketCount;
                        // Get the current date
                       
                        DateTime startDate = DateTime.Now;  // Current date as start date
                        DateTime endDate = DateTime.Now;
                        var totals = svc.GetOrderPaymentTotals(startDate, endDate);

                        obj.TotalSales = totals.TotalSales;
                        obj.TotalRefund = totals.TotalRefund;
                        //obj.dashboardBoxName= 1;


                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Dashboard.Views._Index, obj)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Error.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Error.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion
    }
}