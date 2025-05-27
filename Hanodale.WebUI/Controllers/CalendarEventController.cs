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
    public partial class CalendarEventController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "CalendarEvent/Index";
        #endregion

        #region Constructor

        private readonly ICalendarEventService svc; private readonly ICommonService svcCommon;

        public CalendarEventController(ICalendarEventService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region CalendarEvent Details
        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarEvent.Views.Index, _accessRight)
                        });
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
        public virtual ActionResult BindCalendarEvent(DataTableModel param, string myKey)
        {
            IEnumerable<CalendarEvents> filteredCalendarEvents = null;
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
                        int submenu_Id = 0;
                        //if (myKey != null)
                        //    submenu_Id = Common.DecryptToID(this.CurrentUserId.ToString(), myKey);

                        var CalendarEventModel = this.svc.GetCalendarEvent(this.CurrentUserId,this.SubCostCenter, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {
                            CalendarEventViewModel _helpDeskViewModel = new CalendarEventViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<CalendarEvents, string> orderingFunction = (c => sortColumnIndex == 0 ? c.title :
                                                            sortColumnIndex == 1 ? c.description :
                                                            sortColumnIndex == 2 ? c.icon :
                                                            sortColumnIndex == 3 ? c.eventColor :
                                                            sortColumnIndex == 4 ? c.allowToSelect.ToString() :
                                                            c.visibility.ToString()
                                                            );

                            filteredCalendarEvents = CalendarEventModel.lstCalendarEvents;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredCalendarEvents = filteredCalendarEvents.OrderBy(orderingFunction);
                                else
                                    filteredCalendarEvents = filteredCalendarEvents.OrderByDescending(orderingFunction);
                            }

                            var result = CalendarEventData(filteredCalendarEvents, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = CalendarEventModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = CalendarEventModel.recordDetails.totalDisplayRecords,
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

        /// <summary>
        /// This method is to get the CalendarEvent data as string array to bind into datatbale
        /// </summary>
        /// <param name="userEntry">CalendarEvent list</param>
        /// <returns></returns>
        public static List<string[]> CalendarEventData(IEnumerable<CalendarEvents> userEntry, int currentUserId)
        {
            return userEntry.Select(entry => new string[]
            {  
                entry.title,
                entry.description, 
                entry.icon.ToString(),
                entry.eventColor,
                entry.allowToSelect.ToString(),
                entry.visibility.ToString(),
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarEvent.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }
        #endregion

        #region CalendarEvent ADD,EDIT,DELETE

        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                CalendarEventModel _model = new CalendarEventModel();

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {
                      
                        _model.isEdit = false;
                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                        _model.allowToSelect = true;
                        _model.visibility = true;
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarEvent.Views.Create, _model)
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
        public virtual JsonResult SaveCalendarEvent(CalendarEventModel CalendarEventModel)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), CalendarEventModel.id);

                    if (newId > 0)
                    {
                        if (!_accessRight.canEdit)
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
                        if (!_accessRight.canAdd)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_ADD
                            });
                        }
                    }

                    if (svc != null)
                    {
                        CalendarEvents CalendarEventEn = new CalendarEvents();

                        CalendarEventEn.title = CalendarEventModel.title;
                        CalendarEventEn.description = CalendarEventModel.description;
                        CalendarEventEn.eventColor = CalendarEventModel.color;
                        CalendarEventEn.organization_Id = this.SubCostCenter;
                        CalendarEventEn.icon = CalendarEventModel.icon;
                        CalendarEventEn.visibility = CalendarEventModel.visibility;
                        CalendarEventEn.allowToSelect = CalendarEventModel.allowToSelect;
                        CalendarEventEn.modifiedBy = this.UserName;
                        CalendarEventEn.modifiedDate = DateTime.Now;
                        

                        if (newId > 0)
                        {
                            CalendarEventEn.id = Common.DecryptToID(this.CurrentUserId.ToString(), CalendarEventModel.id);
                        }
                        else
                        {
                            CalendarEventEn.createdBy = this.UserName;
                            CalendarEventEn.createdDate = DateTime.Now;
                        }
                        bool isExists = svc.IsCalendarEventExists(CalendarEventEn);
                        if (!isExists)
                        {
                            var saveCalendarEvent = svc.SaveCalendarEvent(this.CurrentUserId, CalendarEventEn, _accessRight.pageName);

                            if (saveCalendarEvent != null)
                            {
                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), saveCalendarEvent.id.ToString()),
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), saveCalendarEvent.id.ToString()),
                                    });
                                }
                            }
                            else
                            {
                                //if (newId > 0)
                                //{
                                //    return Json(new
                                //    {
                                //        status = Common.Status.Success.ToString(),
                                //        message = Resources.MSG_ERR_UPDATE
                                //    });
                                //}
                                //else
                                //{
                                return Json(new
                                {
                                    status = Common.Status.Error.ToString(),
                                    message = Resources.MSG_ERR_SAVE
                                });
                                // }
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Warning.ToString(),
                                message = Resources.MSG_NAME_ALREADY_EXIST
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
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            CalendarEventModel CalendarEventModel = new CalendarEventModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    CalendarEventModel.readOnly = readOnly;
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var CalendarEvent = svc.GetCalendarEventById(newId);

                            if (CalendarEvent != null)
                            {
                                CalendarEventModel.id = id;
                                CalendarEventModel.title = CalendarEvent.title;
                                CalendarEventModel.description = CalendarEvent.description;
                                CalendarEventModel.color = CalendarEvent.eventColor;
                                CalendarEventModel.icon = CalendarEvent.icon;
                                CalendarEventModel.visibility = CalendarEvent.visibility;
                                CalendarEventModel.allowToSelect = CalendarEvent.allowToSelect;
                                CalendarEventModel.isEdit = true;
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_ERR_RETRIEVE
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
                        //Redirect to access denied page
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.CalendarEvent.Views.Create, CalendarEventModel)
            });
        }


        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteCalendarEvent(this.CurrentUserId, newId, _accessRight.pageName);
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
