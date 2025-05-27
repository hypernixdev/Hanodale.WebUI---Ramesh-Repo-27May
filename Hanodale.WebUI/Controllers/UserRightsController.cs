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

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class UserRightsController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "UserRights/UserRights";
        #endregion

        #region Constructor

        private readonly IUserRightsService svc; private readonly ICommonService svcCommon;

        //public UserRightsController(T4MVC.Dummy d) : base(d) { }

        public UserRightsController(IUserRightsService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }

        #endregion

        #region User Rights Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult UserRights()
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        UserRightsModel _userRightsModel = new UserRightsModel();

                        var userRoles = svc.GetUserRoles(this.CurrentUserId);

                        _userRightsModel.UserRoles = userRoles.Select(a => new SelectListItem
                        {
                            Text = a.roleName,
                            Value = a.id.ToString()
                        });

                        return Json(new
                        {
                            status = "",
                            viewMarkup = Common.RenderPartialViewToString(this, "Index", _userRightsModel)
                        });
                    }
                    else
                    {
                        //Redirect to access denied page
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    //Redirect to access denied page
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.MSG_ERR_SERVICE
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        //<summary>
        //This method is to get the user rights data
        //</summary>
        //<param name="id">role id</param>
        //<returns></returns>
        [AppAuthorize]
        public virtual JsonResult GetUserRightsByRoleId(int id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        UserRightsModel _userRightsModel = new UserRightsModel();
                        var menuModel = svc.GetUserRightsByRole(id);
                        if (menuModel != null)
                        {
                            _userRightsModel.mainMenu = ConvertorModel.MainMenu(menuModel);
                        }

                        var userRoles = svc.GetUserRoles(this.CurrentUserId);
                        _userRightsModel.userRole_Id = id;
                        _userRightsModel.UserRoles = userRoles.Select(a => new SelectListItem
                        {
                            Text = a.roleName,
                            Value = a.id.ToString(),
                            Selected = (a.id == id)
                        });

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, "Index", _userRightsModel)
                        });
                    }
                    else
                    {
                        //Redirect to access denied page
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    //Redirect to access denied page
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.MSG_ERR_SERVICE
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
        public virtual JsonResult SaveUserRights(List<UserRightsModel> userRights)
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        if (!_accessRight.canEdit || !_accessRight.canAdd)
                        {
                            return Json(new
                            {
                                status = "Access " + Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }

                    List<UserRights> lstRights = new List<UserRights>();
                    foreach (UserRightsModel _userRightsModel in userRights)
                    {
                        UserRights _userRight = new UserRights();
                        _userRight.userRole_Id = _userRightsModel.userRole_Id;
                        _userRight.menuItem_Id = _userRightsModel.subMenu_Id;
                        _userRight.canView = _userRightsModel.canView;
                        _userRight.canAdd = _userRightsModel.canAdd;
                        _userRight.canDelete = _userRightsModel.canDelete;
                        _userRight.canEdit = _userRightsModel.canEdit;

                        _userRight.modifiedBy = this.UserName;
                        _userRight.modifiedDate = DateTime.Now;

                        _userRight.createdBy = this.UserName;
                        _userRight.createdDate = DateTime.Now;

                        lstRights.Add(_userRight);
                    }

                    if (svc != null)
                    {
                        bool isSuccess = svc.SaveUserRights(lstRights, PAGE_URL);

                        if (isSuccess)
                        {
                            return Json(new
                            {
                                status = Common.Status.Success.ToString(),
                                message = Resources.MSG_UPDATE
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Success.ToString(),
                                message = Resources.MSG_ERR_UPDATE
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