using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel;
using System.Web.Mvc;
using System.Linq;
using Hanodale.Utility.Globalize;
using Hanodale.WebUI.Logging.Elmah;
using System;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Web.Configuration;

namespace Hanodale.WebUI.Controllers
{
    public partial class CopyCalendarController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "CalendarSetting/Index";
        #endregion

        #region Constructor

        private readonly ICalendarSettingService svc; private readonly ICommonService svcCommon;

        public CopyCalendarController(ICalendarSettingService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region CalendarSetting ADD,EDIT,DELETE

        [Authorize]
        public virtual JsonResult Create(int id = 0)
        {
            try
            {
                if (id == 0)
                    id = DateTime.Now.Year;
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                CopyCalendarModel _model = new CopyCalendarModel();

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {

                        var fromYearList = svc.GetCalendarYears(this.SubCostCenter);
                        _model.fromYearList = fromYearList.Select(a => new SelectListItem
                        {
                            Value = a.ToString(),
                            Text = a.ToString(),
                            Selected = a == DateTime.Now.Year
                        });


                        var toYearList = new List<SelectListItem>();


                        var current = DateTime.Now.Year;
                        toYearList.Add(new SelectListItem
                        {
                            Value = (current).ToString(),
                            Text = (current).ToString(),
                            Selected = current == id
                        });

                        for (int i = 1; i <= 5; i++)
                        {
                            toYearList.Add(new SelectListItem
                            {
                                Value = (current - i).ToString(),
                                Text = (current - i).ToString(),
                                Selected = (current - i) == id
                            });

                            toYearList.Add(new SelectListItem
                            {
                                Value = (current + i).ToString(),
                                Text = (current + i).ToString(),
                                Selected = (current + i) == id
                            });
                        }


                        _model.toYearList = toYearList.OrderBy(p => p.Value);



                        //var calendarEventlist = svc.GetCalendarEvent(this.CurrentUserId, this.SubCostCenter, 0, 1000, null);
                        //_model.calendarEventlist = calendarEventlist.lstCalendarEvents.Select(a => new CalendarEventModel
                        //{
                        //    id = a.id.ToString(),
                        //    title = a.title,
                        //    description = a.description,
                        //    color = a.eventColor,
                        //    icon = a.icon,
                        //}).ToList();



                        //_model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CopyCalendar.Views.CopyCalendar, _model)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_ADD
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveCopyCalendar(CopyCalendarModel entity)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {

                        if (svc != null)
                        {
                            var copyCalendarEn = new CopyCalendars();

                            copyCalendarEn.organization_Id = this.SubCostCenter;
                            copyCalendarEn.fromYear = entity.fromYear;
                            copyCalendarEn.toYear = entity.toYear;
                            copyCalendarEn.replace = entity.replace;
                            copyCalendarEn.createdBy = this.UserName;
                            copyCalendarEn.createdDate = DateTime.Now;


                            var res = svc.CopyCalendar(copyCalendarEn);

                            if (res)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_SAVE,
                                });
                            }
                            else
                            {

                                return Json(new
                                {
                                    status = Common.Status.Error.ToString(),
                                    message = Resources.MSG_ERR_SAVE
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
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS
                        });
                    }
                }
                catch (Exception err)
                {
                    throw new ErrorException(err.Message);
                }
            }
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_INVALIDMODEL
            });
        }

        #endregion
    }
}
