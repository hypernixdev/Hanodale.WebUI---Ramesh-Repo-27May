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
    public partial class ModuleItemController : AuthorizedController
    {

        #region Declaration
        const string PAGE_URL = "ModuleItem/ModuleItem";
        #endregion

        #region Constructor

        private readonly IModuleItemService svc; private readonly ICommonService svcCommon;

        public ModuleItemController(IModuleItemService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region Module Item Details

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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ModuleItem.Views.Index, _accessRight)
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
        public virtual JsonResult ModuleItem()
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
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ModuleItem.Views.Index, _accessRight)
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
        public virtual ActionResult BindModuleItem(DataTableModel param)
        {
            int totalRecordCount = 0;
            IEnumerable<ModuleItems> filteredModuleItems = null;
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

                        var moduleItemModel = this.svc.GetModuleItem(this.CurrentUserId, true, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {
                            ModuleItemViewModel _moduleItemViewModel = new ModuleItemViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<ModuleItems, string> orderingFunction = (c => sortColumnIndex == 0 ? c.moduleName :
                                                            sortColumnIndex == 1 ? c.name :
                                                            sortColumnIndex == 2 ? c.description : c.visibility.ToString()
                                                            );

                            filteredModuleItems = moduleItemModel.lstModuleItem;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredModuleItems = filteredModuleItems.OrderBy(orderingFunction);
                                else
                                    filteredModuleItems = filteredModuleItems.OrderByDescending(orderingFunction);
                            }

                            var result = ModuleItemData(filteredModuleItems, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = moduleItemModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = moduleItemModel.recordDetails.totalDisplayRecords,
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
        /// This method is to get the ModuleItem data as string array to bind into datatbale
        /// </summary>
        /// <param name="userEntry">ModuleItem list</param>
        /// <returns></returns>
        public static List<string[]> ModuleItemData(IEnumerable<ModuleItems> moduleItemEntry, int currentUserId)
        {
            return moduleItemEntry.Select(entry => new string[]
            {  
                entry.moduleName, 
                entry.name,
                entry.description,
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ModuleItem.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        #region Add,Edit and Delete

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create()
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            ModuleItemModel _moduleItemModel = new ModuleItemModel();

            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight.canView && _accessRight.canAdd)
                {
                    if (svc != null)
                    {
                        var moduleType = svcCommon.GetListModuleTypes();
                        _moduleItemModel.visibility = true;
                        _moduleItemModel.sortOrder = 1;
                        _moduleItemModel.ModuleTypeItems = moduleType.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });
                        _moduleItemModel.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
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
                        message = Resources.NO_ACCESS_RIGHTS_ADD
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.ModuleItem.Views.Create, _moduleItemModel)
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveModuleItem(ModuleItemModel model)
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
                            ModuleItems entity = new ModuleItems();
                            entity.modulType_Id = model.modulType_Id;
                            entity.name = model.name;
                            entity.description = model.description;
                            entity.visibility = model.visibility;
                            entity.sortOrder = model.sortOrder;
                            entity.remarks = model.remarks;
                            if (newId > 0)
                            {
                                entity.id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                            }
                            else
                            {

                            }
                            bool isExists = svc.IsModuleItemExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveModuleItem(this.CurrentUserId, entity, _accessRight.pageName);

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
                                    message = Resources.MSG_RECORD_MODULE_NAME.Replace("$NAME$", model.name)
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
            ModuleItemModel _model = new ModuleItemModel();
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
                            var moduleItem = svc.GetModuleItemById(newId);

                            if (moduleItem != null)
                            {
                                _model.id = id;
                                _model.isEdit = true;
                                _model.modulType_Id = moduleItem.modulType_Id;
                                _model.name = moduleItem.name;
                                _model.description = moduleItem.description;
                                _model.visibility = moduleItem.visibility;
                                _model.sortOrder = moduleItem.sortOrder;
                                _model.remarks = moduleItem.remarks;

                                var moduleType = svcCommon.GetListModuleTypes();
                                _model.ModuleTypeItems = moduleType.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == moduleItem.modulType_Id)
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
                viewMarkup = Common.RenderPartialViewToString(this, MVC.ModuleItem.Views.Create, _model)
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
                            bool isSuccess = svc.DeleteModuleItem(this.CurrentUserId, newId, _accessRight.pageName);
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
