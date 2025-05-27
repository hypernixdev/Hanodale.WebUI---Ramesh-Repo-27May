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
    public partial class WorkCategoryController : AuthorizedController
    {
        #region Declaration

        const string PAGE_URL = "WorkCategory/Index";
        #endregion

        #region Constructor

        private readonly IWorkCategoryService svc; private readonly ICommonService svcCommon;

        public WorkCategoryController(IWorkCategoryService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }

        #endregion

        #region WorkCategory Details

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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.WorkCategory.Views.Index, _accessRight)
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
        public virtual JsonResult WorkCategory()
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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.WorkCategory.Views.Index, null)
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
        public virtual ActionResult BindWorkCategory(DataTableModel param)
        {
            int totalRecordCount = 0;
            IEnumerable<WorkCategorys> filteredWorkCategory = null;
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

                        var taskCategoryModel = this.svc.GetWorkCategory(this.CurrentUserId, this.CurrentUserId, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {
                            WorkCategoryViewModel _taskCategoryViewModel = new WorkCategoryViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<WorkCategorys, string> orderingFunction = (c => sortColumnIndex == 0 ? c.name :
                                                            sortColumnIndex == 1 ? c.description :
                                                            sortColumnIndex == 2 ? c.remarks : c.isVisible.ToString()
                                                            );

                            filteredWorkCategory = taskCategoryModel.lstWorkCategory;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredWorkCategory = filteredWorkCategory.OrderBy(orderingFunction);
                                else
                                    filteredWorkCategory = filteredWorkCategory.OrderByDescending(orderingFunction);
                            }

                            var result = WorkCategoryData(filteredWorkCategory, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = taskCategoryModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = taskCategoryModel.recordDetails.totalDisplayRecords,
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
        /// This method is to get the WorkCategory data as string array to bind into datatbale
        /// </summary>
        /// <param name="taskCategoryEntry">WorkCategory list</param>
        /// <returns></returns>
        public static List<string[]> WorkCategoryData(IEnumerable<WorkCategorys> taskCategoryEntry, int currentUserId)
        {
            return taskCategoryEntry.Select(entry => new string[]
            {  
                entry.name,
                entry.description,
                entry.remarks, 
                entry.isVisible.ToString(),
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.WorkCategory.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        #region WorkCategory ADD,EDIT,DELETE

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create()
        {
            AccessRightsModel _accessRight = new AccessRightsModel();

            WorkCategoryModel _model = new WorkCategoryModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.isEdit = false;

                _model.isVisible = true;

                _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                if (_accessRight.canView && _accessRight.canAdd)
                {
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.WorkCategory.Views.Create, _model)
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
        public virtual JsonResult SaveWorkCategory(WorkCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int newId = 0;
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);

                        if (newId > 0)
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
                            WorkCategorys entity = new WorkCategorys();
                            entity.name = model.name;
                            entity.description = model.description;
                            entity.remarks = model.remarks;
                            entity.isVisible = model.isVisible;
                            entity.createdBy = this.UserName;
                            entity.createdDate = DateTime.Now;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;
                            if (newId > 0)
                            {
                                entity.id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                            }
                            else
                            {
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            bool isExists = svc.IsWorkCategoryExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveWorkCategory(this.CurrentUserId, entity, _accessRight.pageName);

                                if (save != null)
                                {
                                    if (newId > 0)
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
                                    if (newId > 0)
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
        public virtual JsonResult Edit(string id,bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            WorkCategoryModel _model = new WorkCategoryModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    _model.readOnly = readOnly;
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var workCategory = svc.GetWorkCategoryById(newId);

                            if (workCategory != null)
                            {
                                _model.id = id;
                                _model.isEdit = true;
                                _model.name = workCategory.name;
                                _model.description = workCategory.description;
                                _model.remarks = workCategory.remarks;
                                _model.isVisible = workCategory.isVisible;
                                _model.modifiedBy = workCategory.modifiedBy;
                                _model.modifiedDate = workCategory.modifiedDate;
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
                                status = "Access " + Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_VIEW
                            });
                        }
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = "Access " + Common.Status.Denied.ToString(),
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
                viewMarkup = Common.RenderPartialViewToString(this, MVC.WorkCategory.Views.Create, _model)
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
                            bool isSuccess = svc.DeleteWorkCategory(this.CurrentUserId, newId, _accessRight.pageName);
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
