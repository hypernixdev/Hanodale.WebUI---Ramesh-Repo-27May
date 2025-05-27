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
using System.Configuration;


namespace Hanodale.WebUI.Controllers
{
    public partial class OrganizationEmailController : AuthorizedController
    {
        #region Declaration

        const string PAGE_URL = "Organization/Organization";
        #endregion

        #region Constructor

        private readonly IOrganizationEmailService svc; private readonly ICommonService svcCommon;

        public OrganizationEmailController(IOrganizationEmailService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }

        #endregion

        #region Task Details

        [AppAuthorize]
        public virtual ActionResult Index(string id, bool readOnly)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _accessRight.elementId = id;
                _accessRight.readOnly = readOnly;
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.OrganizationEmail.Views.Index, _accessRight)
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
        public virtual ActionResult BindOrganizationEmail(DataTableModel param)
        {
            int totalRecordCount = 0;
            IEnumerable<OrganizationEmails> filteredOrganizationEmails = null;
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

                        var organizationEmailModel = this.svc.GetOrganizationEmail(this.CurrentUserId, this.CurrentUserId, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {
                            OrganizationEmailViewModel _taskViewModel = new OrganizationEmailViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<OrganizationEmails, string> orderingFunction = (c => sortColumnIndex == 0 ? c.departmentName :
                                                                         sortColumnIndex == 1 ? c.emailFrom : c.emailTo
                                                            );

                            filteredOrganizationEmails = organizationEmailModel.lstOrganizationEmails;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredOrganizationEmails = filteredOrganizationEmails.OrderBy(orderingFunction);
                                else
                                    filteredOrganizationEmails = filteredOrganizationEmails.OrderByDescending(orderingFunction);
                            }

                            var result = OrganizationEmailData(filteredOrganizationEmails, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = organizationEmailModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = organizationEmailModel.recordDetails.totalDisplayRecords,
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
        /// This method is to get the Task data as string array to bind into datatbale
        /// </summary>
        /// <param name="taskEntry">Task list</param>
        /// <returns></returns>
        public static List<string[]> OrganizationEmailData(IEnumerable<OrganizationEmails> organizationEmailEntry, int currentUserId)
        {
            return organizationEmailEntry.Select(entry => new string[]
            {  
                entry.departmentName,
                entry.emailFrom, 
                entry.emailTo,
                 Common.Encrypt(currentUserId.ToString(), entry.id.ToString())
            }).ToList();
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction(bool readOnly)
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.readOnly = readOnly;
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.OrganizationEmail.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        #region OrganizationEmail ADD,EDIT,DELETE

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            OrganizationEmailModel _model = new OrganizationEmailModel();

            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight.canView && _accessRight.canAdd)
                {
                    _model.organization_Id = id;

                    int departmentId = Convert.ToInt32(WebConfigurationManager.AppSettings["DepartmentType"]);
                    var department = svcCommon.GetListModuleItem(departmentId);
                    _model.lstdepartment = department.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.id.ToString()
                    });

                    _model.isEdit = false;

                    _model.password = ConfigurationManager.AppSettings["DefaultPassword"];

                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.OrganizationEmail.Views.Create, _model)
                    });
                }
                else
                {
                    //Redirect to access denied page
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS_ADD
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveOrganizationEmail(OrganizationEmailModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {

                        if (model.id != null)
                        {
                            if (!_accessRight.canEdit)
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
                            if (!_accessRight.canAdd)
                            {
                                return Json(new
                                {
                                    status = "Access " + Common.Status.Denied.ToString(),
                                    message = Resources.NO_ACCESS_RIGHTS_ADD
                                });
                            }
                        }

                        if (svc != null)
                        {
                            OrganizationEmails entity = new OrganizationEmails();
                            entity.organization_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.organization_Id.ToString());
                            entity.department_Id = model.department_Id;
                            entity.emailTo = model.emailTo;
                            entity.emailFrom = model.emailFrom;
                            entity.userName = model.userName;
                            entity.password = model.password;
                            entity.smtp = model.smtp;
                            entity.smptPort = model.smptPort;
                            entity.isSSL = model.isSSL;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;

                            if (model.id != null)
                            {
                                entity.id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                            }
                            else
                            {
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            bool isExists = svc.IsOrganizationEmailExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveOrganizationEmail(this.CurrentUserId, entity, _accessRight.pageName);

                                if (save != null)
                                {
                                    if (model.id != null)
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_UPDATE,
                                            id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),
                                        });
                                    }
                                    else
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_SAVE,
                                            id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),
                                        });
                                    }
                                }
                                else
                                {
                                    if (model.id != null)
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_ERR_UPDATE
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
                            }
                            else
                            {
                                return Json(new
                                {

                                    status = Common.Status.Warning.ToString(),
                                    message = Resources.RECORD_EXISTS
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

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            OrganizationEmailModel _model = new OrganizationEmailModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _model.readOnly = readOnly;
                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var task = svc.GetOrganizationEmailById(newId);

                            if (task != null)
                            {
                                _model.id = id;
                                _model.organization_Id = Common.Encrypt(this.CurrentUserId.ToString(), task.organization_Id.ToString());
                                _model.isEdit = true;
                                _model.department_Id = task.department_Id;
                                _model.emailTo = task.emailTo;
                                _model.emailFrom = task.emailFrom;
                                _model.userName = task.userName;
                                _model.password = task.password;
                                _model.smtp = task.smtp;
                                _model.smptPort = task.smptPort;
                                _model.isSSL = task.isSSL;

                                int departmentId = Convert.ToInt32(WebConfigurationManager.AppSettings["DepartmentType"]);
                                var department = svcCommon.GetListModuleItem(departmentId);
                                _model.lstdepartment = department.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == task.department_Id)
                                });
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
                viewMarkup = Common.RenderPartialViewToString(this, MVC.OrganizationEmail.Views.Create, _model)
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
                            bool isSuccess = svc.DeleteOrganizationEmail(this.CurrentUserId, newId, _accessRight.pageName);
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
