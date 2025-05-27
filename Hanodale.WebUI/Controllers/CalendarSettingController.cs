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
    public partial class CalendarSettingController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "CalendarSetting/Index";
        #endregion

        #region Constructor

        private readonly ICalendarSettingService svc; private readonly ICommonService svcCommon;
        private readonly ICalendarEventService svcCalendarEvent;

        public CalendarSettingController(ICalendarSettingService _bLService, ICommonService _commonService
            , ICalendarEventService _calendarEventService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcCalendarEvent = _calendarEventService;
        }
        #endregion

        #region CalendarSetting Details
        [AppAuthorize]
        public virtual ActionResult Index(int id=0)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                CalendarSettingModelDetails _model = new CalendarSettingModelDetails();



                if (_accessRight != null)
                {
                    _model.userAccess = _accessRight;
                    var currentYear = DateTime.Now.Year;
                    var yearList = svc.GetCalendarYears(this.SubCostCenter);
                    _model.yearList = new List<SelectListItem>();

                    _model.yearList = yearList.Select(a => new SelectListItem
                    {
                        Value = a.ToString(),
                        Text = a.ToString(),
                        Selected = a == (id==0?currentYear:id)
                    }).ToList();


                    
                    if (!yearList.Any(p => p.ToString() == DateTime.Now.Year.ToString()))
                    {

                        var yearListTemp = new List<SelectListItem>();
                        foreach (var item in yearList)
                        {
                            yearListTemp.Add(new SelectListItem
                            {
                                Value = (item).ToString(),
                                Text = (item).ToString(),
                                //Selected = true
                            });
                        }

                        
                        
                        yearListTemp.Add(new SelectListItem
                        {
                            Value = (currentYear).ToString(),
                            Text = (currentYear).ToString(),
                            Selected = !yearListTemp.Any(p => p.Selected)
                        });

                        _model.yearList = yearListTemp;

                    }


                    //var calendarEventlist = svc.GetCalendarEvent(this.CurrentUserId, this.SubCostCenter, 0, 1000, DateTime.Now.Year.ToString());
                    //_model.calendarEventlist = calendarEventlist.lstCalendarEvents.Select(a => new CalendarEventModel
                    //{
                    //    id = a.id.ToString(),
                    //    title = a.title,
                    //    description = a.description,
                    //    color = a.eventColor,
                    //    icon = a.icon,
                    //}).ToList();



                    //_model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");


                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarSetting.Views.Create, _model)
                        });


                        //return Json(new
                        //{
                        //    viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarSetting.Views.Index, null)
                        //});
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
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

        [Authorize]
        public virtual ActionResult BindCalendarSetting(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            IEnumerable<CalendarSettings> filteredCalendarSettings = null;
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {


                        // Get login user Id
                        int userId = this.CurrentUserId;
                        //int submenu_Id = 0;
                        //if (myKey != null)
                        //    submenu_Id = Common.DecryptToID(this.CurrentUserId.ToString(), myKey);

                        var CalendarSettingModel = this.svc.GetCalendarSetting(this.CurrentUserId, this.SubCostCenter, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {
                            CalendarSettingModel _helpDeskViewModel = new CalendarSettingModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<CalendarSettings, string> orderingFunction = (c => sortColumnIndex == 0 ? c.title :
                                                            sortColumnIndex == 1 ? c.description :
                                                            sortColumnIndex == 2 ? c.StartDate.ToShortDateString() : c.EndDate.GetValueOrDefault().ToShortDateString()
                                                            );

                            filteredCalendarSettings = CalendarSettingModel.lstCalendarSettings;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredCalendarSettings = filteredCalendarSettings.OrderBy(orderingFunction);
                                else
                                    filteredCalendarSettings = filteredCalendarSettings.OrderByDescending(orderingFunction);
                            }

                            var result = CalendarSettingData(filteredCalendarSettings, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = CalendarSettingModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = CalendarSettingModel.recordDetails.totalDisplayRecords,
                                aaData = result
                            }, JsonRequestBehavior.AllowGet);
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
                            message = Resources.NO_ACCESS_RIGHTS_EDIT
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

        public static List<string[]> CalendarSettingData(IEnumerable<CalendarSettings> userEntry, int currentUserId)
        {
            return userEntry.Select(entry => new string[]
            {  
                entry.title,
                entry.description, 
                entry.StartDate.ToString("dd/MM/yyyy"),
                entry.EndDate==null ?entry.EndDate.GetValueOrDefault().ToString("dd/MM/yyyy"): string.Empty,
               Common.Encrypt(currentUserId.ToString(), entry.id.ToString())
            }).ToList();
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction()
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarSetting.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
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

                CalendarSettingModelDetails _model = new CalendarSettingModelDetails();

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {
                        _model.userAccess = _accessRight;
                        _model.isEdit = true;

                        var yearList = new List<SelectListItem>();


                        var current = DateTime.Now.Year;
                        yearList.Add(new SelectListItem
                        {
                            Value = (current).ToString(),
                            Text = (current).ToString(),
                            Selected = current == id
                        });

                        for (int i = 1; i <= 5; i++)
                        {
                            yearList.Add(new SelectListItem
                            {
                                Value = (current - i).ToString(),
                                Text = (current - i).ToString(),
                                Selected = (current - i) == id
                            });

                            yearList.Add(new SelectListItem
                            {
                                Value = (current + i).ToString(),
                                Text = (current + i).ToString(),
                                Selected = (current + i) == id
                            });
                        }




                        _model.yearList = yearList.OrderBy(p => p.Value);

                        var calendarEventlist = svcCalendarEvent.GetListCalendarEvent(this.CurrentUserId, this.SubCostCenter);
                        _model.calendarEventlist = calendarEventlist.Select(a => new CalendarEventModel
                        {
                            id = a.id.ToString(),
                            title = a.title,
                            description = a.description,
                            color = a.eventColor,
                            icon = a.icon,
                        }).ToList();



                        //_model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarSetting.Views.Create, _model)
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

        public virtual JsonResult GetCalendarItem(int year=0)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            var calendarSettinglist = svc.GetCalendarItem(this.CurrentUserId, this.SubCostCenter, year);
            var lst = calendarSettinglist.Select(a => new
            {
                id = a.id,
                title = a.title,
                description = string.IsNullOrEmpty(a.description) ? "No Description" : a.description,
                sYear = a.StartDate.Year,
                sMonth = a.StartDate.Month,
                sDay = a.StartDate.Day,
                eYear = a.EndDate.GetValueOrDefault().Year,
                eMonth = a.EndDate.GetValueOrDefault().Month,
                eDay = a.EndDate.GetValueOrDefault().Day,
                icon = a.icon,
                color = a.color,
                eventId = a.calendarEvent_Id,

            }).ToList();

            return Json(lst);
        }

        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public virtual JsonResult SaveCalendarEvent(List<CalendarSettingModel> lst, int year)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {


                    //int newId = 0;
                    //newId = Common.DecryptToID(this.CurrentUserId.ToString(), CalendarSettingModel.id);

                    //if (newId > 0)
                    //{
                    //    if (!_accessRight.canEdit)
                    //    {
                    //        return Json(new
                    //        {
                    //            status = Common.Status.Denied.ToString(),
                    //            message = Resources.NO_ACCESS_RIGHTS_EDIT
                    //        });
                    //    }
                    //}
                    //else
                    //{
                    //    if (!_accessRight.canAdd)
                    //    {
                    //        return Json(new
                    //        {
                    //            status = Common.Status.Denied.ToString(),
                    //            message = Resources.NO_ACCESS_RIGHTS_ADD
                    //        });
                    //    }
                    //}
                    var res = false;
                    if (svc != null)
                    {
                        if (lst == null)
                        {
                            res = svc.DeleteCalendarSetting(this.SubCostCenter, year);
                        }
                        else
                        {
                            var calendarSettList = new List<CalendarSettings>();
                            foreach (var item in lst)
                            {

                                CalendarSettings CalendarSettingEn = new CalendarSettings();

                                CalendarSettingEn.calendarEvent_Id = item.calendarEvent_Id;
                                //CalendarSettingEn.title = item.title;
                                //CalendarSettingEn.description = item.description;
                                CalendarSettingEn.StartDate = item.startDate;
                                //CalendarSettingEn.organization_Id = this.SubCostCenter;
                                CalendarSettingEn.EndDate = item.endDate;



                                if (item.id > 0)
                                {
                                    CalendarSettingEn.modifiedBy = this.UserName;
                                    CalendarSettingEn.modifiedDate = DateTime.Now;
                                    CalendarSettingEn.id = item.id;
                                }
                                else
                                {
                                    CalendarSettingEn.createdBy = this.UserName;
                                    CalendarSettingEn.createdDate = DateTime.Now;
                                }

                                calendarSettList.Add(CalendarSettingEn);
                            }

                            res = svc.SaveCalendarSetting(this.CurrentUserId, calendarSettList, this.SubCostCenter, year);

                        }
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

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string year, bool readOnly)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                CalendarSettingModelDetails _model = new CalendarSettingModelDetails();
                _model.isEdit = true;
                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canEdit)
                    {
                        _model.userAccess = _accessRight;

                        var yearList = new List<SelectListItem>();
                        yearList.Add(new SelectListItem
                        {
                            Value = year,
                            Text = year,
                            Selected = true
                        });

                        _model.yearList = yearList;

                        var calendarEventlist = svcCalendarEvent.GetListCalendarEvent(this.CurrentUserId, this.SubCostCenter);
                        _model.calendarEventlist = calendarEventlist.Select(a => new CalendarEventModel
                        {
                            id = a.id.ToString(),
                            title = a.title,
                            description = a.description,
                            color = a.eventColor,
                            icon = a.icon,
                        }).ToList();



                        //_model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarSetting.Views.Create, _model)
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
        public virtual ActionResult Delete(int id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    //int newId = 0;
                    //newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteCalendarSetting(this.SubCostCenter, id);
                            if (isSuccess)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_DELETE
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Error.ToString(),
                                    message = Resources.MSG_ERR_DELETE
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
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                {
                    return Json(new
                    {
                        status = Common.Status.Warning.ToString(),
                        message = Resources.MSG_RECORD_IN_USE
                    });
                }
                else
                {
                    throw new ErrorException(ex.Message);
                }
            }
            return View();
        }
        #endregion
    }
}
