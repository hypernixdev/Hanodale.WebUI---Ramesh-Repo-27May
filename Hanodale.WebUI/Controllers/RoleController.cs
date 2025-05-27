using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Hanodale.Entity.Core;

namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class RoleController : AuthorizedController
    {

        #region Declaration
        const string PAGE_URL = "Role/UserRole";
        const string UserId = "";
        #endregion

        #region Constructor

        private readonly IUserRoleService svc; private readonly ICommonService svcCommon;

        public RoleController(IUserRoleService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region User Role Details

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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Role.Views.Index, _accessRight)
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

        [HttpPost]
        [AppAuthorize]
        //[CheckAccessRights(PageName = PAGE_URL, UserId=1)]
        public virtual ActionResult UserRole()
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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Role.Views.Index, _accessRight)
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
        public virtual ActionResult BindRole(DataTableModel param)
        {
            int totalRecordCount = 0;
            IEnumerable<UserRoles> filteredUserRoles = null;
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

                        var userRoleModel = this.svc.GetUserRole(this.CurrentUserId, this.CurrentUserId, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {

                            UserRoleViewModel _userRoleViewModel = new UserRoleViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<UserRoles, string> orderingFunction = (c => sortColumnIndex == 0 ? c.roleName :
                                                           sortColumnIndex == 1 ? c.description : (c.status ? Common.RecordStatus.Active.ToString() : Common.RecordStatus.InActive.ToString()));

                            filteredUserRoles = userRoleModel.lstRoles;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredUserRoles = filteredUserRoles.OrderBy(orderingFunction);
                                else
                                    filteredUserRoles = filteredUserRoles.OrderByDescending(orderingFunction);
                            }

                            var result = UserData(filteredUserRoles, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = userRoleModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = userRoleModel.recordDetails.totalDisplayRecords,
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
        /// This method is to get the user data as string array to bind into datatbale
        /// </summary>
        /// <param name="userEntry">User list</param>
        /// <returns></returns>
        public static List<string[]> UserData(IEnumerable<UserRoles> userEntry, int currentUserId)
        {
            return userEntry.Select(entry => new string[]
            {  
                entry.roleName,
                entry.description, 
                ((entry.status)?Common.RecordStatus.Active.ToString():Common.RecordStatus.InActive.ToString()),
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.Role.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        #region Add,Edit and Delete


        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {

                        RoleModel _model = new RoleModel();
                        _model.status = true;
                        _model.isAdmin = true;
                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                        _model.isEdit = false;

                        //var userRoles = svc.GetUserRoles(this.CurrentUserId);
                        //_model.UserRoles = userRoles.Select(a => new SelectListItem
                        //{
                        //    Text = a.roleName,
                        //    Value = a.id.ToString(),
                        //    Selected = (a.id == 1) ? true : false
                        //});

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Role.Views.Create, _model)
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
        public virtual JsonResult SaveRole(RoleModel roleModel)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), roleModel.id);

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
                        UserRoles roleEn = new UserRoles();

                        roleEn.roleName = roleModel.roleName;
                        roleEn.description = roleModel.description;
                        roleEn.isAdmin = roleModel.isAdmin;
                        roleEn.landingPage = roleModel.landingPage;
                        roleEn.status = roleModel.status;
                        roleEn.modifiedBy = this.UserName;
                        roleEn.modifiedDate = DateTime.Now;
                        roleEn.user_Id = this.CurrentUserId;

                        if (newId > 0)
                        {
                            roleEn.id = Common.DecryptToID(this.CurrentUserId.ToString(), roleModel.id);
                        }
                        else
                        {
                            roleEn.createdBy = this.UserName;
                            roleEn.createdDate = DateTime.Now;
                        }
                        bool isExists = svc.RoleExists(roleEn);
                        if (!isExists)
                        {
                            var saveRole = svc.SaveUserRole(this.CurrentUserId, roleEn, _accessRight.pageName);

                            if (saveRole != null)
                            {
                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), saveRole.id.ToString()),
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), saveRole.id.ToString()),
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
                                message = Resources.ROLE_RECORD_EXISTS.Replace("$ROLE_NAME$", roleModel.roleName)
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
            RoleModel roleModel = new RoleModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)  
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    roleModel.readOnly = readOnly;

                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            HanodaleEntities model = new HanodaleEntities();

                            var role = svc.GetRoleById(newId);
                            var userroleid = Convert.ToInt32(newId);
                            var landingPages = model.UserRights
                             .Where(ur => ur.userRole_Id == userroleid && ur.canView == true && 
                                (ur.MenuItem.reference_Id ==123 || ur.MenuItem.reference_Id ==130 || ur.MenuItem.reference_Id == 1))
                             .Join(model.MenuItems,
                                   ur => ur.menuItem_Id,
                                   mi => mi.id,
                                   (ur, mi) => new { mi.pageUrl })
                             .Distinct()
                             .ToList();
                            var landingPageList = landingPages.Select(a => new SelectListItem
                            {
                                Text = a.pageUrl.Replace("/Index", ""), 
                                Value = a.pageUrl.Replace("/Index", ""),
                                Selected = a.pageUrl.Replace("/Index", "") == role.landingPage
                            }).ToList();

                            // Pass the list to the view using ViewBag or Model
                            roleModel.LandingPages = landingPageList;

                            if (role != null)
                            {
                                roleModel.id = id;
                                roleModel.roleName = role.roleName;
                                roleModel.description = role.description;
                                roleModel.isAdmin = role.isAdmin;
                                roleModel.status = role.status;
                                roleModel.isEdit = true;
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
                        if (!_accessRight.canView)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_VIEW
                            });
                        }
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.Role.Views.Create, roleModel)
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
                            bool isSuccess = svc.DeleteUserRole(this.CurrentUserId, newId, _accessRight.pageName);
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
