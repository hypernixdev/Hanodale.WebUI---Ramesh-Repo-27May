using Hanodale.Domain.DTOs;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;

using System.Linq;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class NavigationController : AuthorizedController
    {
        
        
        private readonly IMenuService svcMenu;private readonly ICommonService svcCommon;
        private readonly IUserService svc;
        private readonly ICalendarSettingService svcCalendarSetting;

        public NavigationController(IUserService _bLService, ICommonService _commonService, IMenuService _menuService, ICalendarSettingService _calendarSettingService)  
        {
            this.svc = _bLService;
            this.svcCommon = _commonService;
            this.svcMenu = _menuService;
            this.svcCalendarSetting = _calendarSettingService;
        }

        #region Index
        [Authorize]
        public virtual ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Method

        /// <summary>
        /// This method is to load the menu based on the user rights
        /// </summary>
        /// <returns>Partial View - Menu</returns>
        [Authorize]
        public virtual ActionResult LoadMenu()
        {

            try
            {
                var menuModel = svcMenu.GetUserMenu(this.CurrentUserId, this.CurrentUserId);
                //if(true)
                //{
                //    (y.Key.name == "Resource" && user.bussinessType_Id == 52) ? "Profile" :
                //}

                return PartialView("_MenuPartial", ConvertorModel.MainMenu(menuModel));
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        /// <summary>
        /// This method is to get the navigation header from the partial view
        /// </summary>
        /// <returns>NavigationHeader - Partial view</returns>
        [Authorize]
        public virtual ActionResult LoadNavigationHeader()
        {
            var _user = svc.GetUserById(this.CurrentUserId, this.CurrentUserId);
            UserModel _userModel = new UserModel();
            _userModel.roleName = _user.roleName;
            _userModel.userName = this.UserName;
            return PartialView("_NavigationHeader", _userModel);
        }

        /// <summary>
        /// This method is to load shortcut menu from the partial view
        /// </summary>
        /// <returns>NavigationHeader - Partial view</returns>
        [Authorize]
        public virtual ActionResult LoadMenuShortcuts()
        {
            return PartialView("_MenuShortcuts");
        }

        /// <summary>
        /// This method is to load Container Settings from the partial view
        /// </summary>
        /// <returns>NavigationHeader - Partial view</returns>
        [Authorize]
        public virtual ActionResult LoadSettings()
        {
            return PartialView("_ContainerSettings");
        }

        /// <summary>
        /// This method is to load the menu based on the user rights
        /// </summary>
        /// <returns>Partial View - Menu</returns>
        [Authorize]
        public virtual ActionResult LoadMainCostCenter()
        {
            try
            {
                MainCostCenterModel model = new MainCostCenterModel();


                var user = svc.GetUserById(this.CurrentUserId, this.CurrentUserId);
                var subcost = svcCommon.GetListofSubCostCenter(this.SubCostCenter);
                var SubCostCenter = svcCommon.GetSubCostCenterById(this.SubCostCenter, this.CurrentUserId);
                if (user.isAccessAllOrganization == true)
                {
                    var assignedOrganisation = svcCommon.GetListofAssignedOrganisation(this.CurrentUserId);
                    if ((SubCostCenter != null && SubCostCenter.Count > 0))
                    {
                        model.parent_Id = SubCostCenter[0].parent_Id;
                        var MainCostCenter = svcCommon.GetAllMainCostCenter().OrderBy(a=>a.name).ToList();
                        if (MainCostCenter != null && MainCostCenter.Count > 0)
                        {
                            model.mainCostCenter_Id = MainCostCenter[0].id;
                            model.MainCostCenter = MainCostCenter.Select(a => new SelectListItem
                            {
                                Text = a.name,
                                Value = a.id.ToString(),
                                Selected = a.id == model.parent_Id
                            });
                        }

                        var SubCostCenters = svcCommon.GetSubCostByMainCostId(Convert.ToInt32(model.parent_Id)).OrderBy(a => a.name).ToList();

                        model.SubCostCenter = SubCostCenters.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = a.id == this.SubCostCenter
                        });



                    }
                    else
                    {
                       
                        if (subcost != null && subcost.Count > 0)
                        {
                            model.parent_Id = subcost[0].parent_Id;
                            var MainCostCenter = svcCommon.GetAllMainCostCenter().OrderBy(a => a.name).ToList();
                            if (MainCostCenter != null && MainCostCenter.Count > 0)
                            {
                                model.mainCostCenter_Id = MainCostCenter[0].id;
                                model.MainCostCenter = MainCostCenter.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = a.id == model.parent_Id
                                });
                            }

                            var SubCostCenters = svcCommon.GetSubCostByMainCostId(Convert.ToInt32(model.parent_Id)).OrderBy(a => a.name).ToList();

                            model.SubCostCenter = SubCostCenters.Select(a => new SelectListItem
                            {
                                Text = a.name,
                                Value = a.id.ToString(),
                                Selected = a.id == this.SubCostCenter
                            });
                        }
                    }
                }
                else
                {
                    if (SubCostCenter != null && SubCostCenter.Count > 0)
                    {
                        model.parent_Id = SubCostCenter[0].parent_Id;
                        model.SubCostCenter = SubCostCenter.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = a.id == this.SubCostCenter
                        });

                        var MainCostCenter = svcCommon.GetMainCostCenter(this.CurrentUserId).OrderBy(a => a.name).ToList();
                        if (MainCostCenter != null && MainCostCenter.Count > 0)
                        {
                            model.mainCostCenter_Id = MainCostCenter[0].id;
                            model.MainCostCenter = MainCostCenter.Select(a => new SelectListItem
                            {
                                Text = a.name,
                                Value = a.id.ToString(),
                                Selected = a.id == model.parent_Id
                            });
                        }
                    }
                }

                //var MainCostCenter = svc.GetMainCostCenter(this.CurrentUserId);
                //if (MainCostCenter != null)
                //{
                //    model.mainCostCenter_Id = MainCostCenter[0].id;
                //    model.MainCostCenter = MainCostCenter.Select(a => new SelectListItem
                //    {
                //        Text = a.name,
                //        Value = a.id.ToString(),
                //        Selected = a.id == model.parent_Id
                //    });
                //}

                //var SubCostCenter = svc.GetSubCostCenterById(this.SubCostCenter);
                //if (SubCostCenter != null && SubCostCenter.Count > 0)
                //{


                //    model.parent_Id = SubCostCenter[0].parent_Id;
                //    model.SubCostCenter = SubCostCenter.Select(a => new SelectListItem
                //    {
                //        Text = a.name,
                //        Value = a.id.ToString(),
                //        Selected = a.id == this.SubCostCenter
                //    });


                //}

                return PartialView("_Search", model);
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }


        //[Authorize]
        //public virtual ActionResult ColumnFilters(List<filters> filters)
        //{

        //    try
        //    {
        //        return PartialView("_ColumnFilter", null);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public virtual JsonResult GetCalendarItem()
        {
            var calendarSettinglist = svcCalendarSetting.GetCalendarItem(this.CurrentUserId, this.SubCostCenter, DateTime.Now.Year);
            if (calendarSettinglist != null)
            {
                var lst = calendarSettinglist.Select(a => new
               {
                   id = a.id,
                   title = a.title,
                   description = string.IsNullOrEmpty(a.description) ? "No Description" : a.description,
                   startYear = a.StartDate,
                   //sYear = a.StartDate.Year,
                   sMonth = a.StartDate.Month,
                   //sDay = a.StartDate.Day,
                   endYear = a.EndDate.GetValueOrDefault(),
                   eYear = a.EndDate.GetValueOrDefault().Year,
                   eMonth = a.EndDate.GetValueOrDefault().Month,
                   //eDay = a.EndDate.GetValueOrDefault().Day,
                   icon = a.icon,
                   color = a.color,
                   allowToSelect = a.allowToSelect,
                   eventId = a.calendarEvent_Id,
               }).ToList();
                return Json(lst);
            }
            else
            {
                return Json(null);
            }
        }

        #endregion
    }
}
