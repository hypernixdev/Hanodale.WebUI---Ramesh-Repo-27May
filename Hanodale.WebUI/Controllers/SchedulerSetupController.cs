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
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.ComponentModel;
using Hanodale.Utility;
using System.Data.Objects.SqlClient;
using Hanodale.Entity.Core;

namespace Hanodale.WebUI.Controllers
{
    public partial class SchedulerSetupController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string PAGE_URLForAccessRight = "SchedulerSetup/Index";
        #endregion

        #region Constructor

        private readonly ISchedulerSetupService svc;
        private readonly ISyncManager syncManager;
        public SchedulerSetupController(ISchedulerSetupService _bLService, ICommonService _svcCommon, ISyncManager syncManager)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "SchedulerSetup";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["SchedulerSetup_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            this.syncManager = syncManager;
        }
        #endregion

        #region Module Item Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new SchedulerSetupModel());

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        if (Request.IsAjaxRequest())
                        {
                            return Json(new
                            {
                                viewMarkup = Common.RenderPartialViewToString(this, "Index", _model)
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return View("Index", _model);
                        }

                    }
                    else
                    {
                        return this.Msg_AccessDeniedInView();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }

        }


        [Authorize]
        public virtual ActionResult BindSchedulerSetup(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            List<SchedulerSetups> filteredSchedulerSetup = null;
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        // Get login user Id
                        int userId = this.CurrentUserId;

                        /*    var filterEntity = new SchedulerSetups();
                           var idFilter0 = Convert.ToString(Request["sSearch_0"]).Trim();
                           var idFilter1 = Convert.ToString(Request["sSearch_1"]).Trim();
                           var idFilter2 = Convert.ToString(Request["sSearch_2"]).Trim();
                           var idFilter3 = Convert.ToString(Request["sSearch_3"]).Trim();

                           int masterRecord_Id = 0;
                           if (myKey != null)
                               masterRecord_Id = Common.DecryptToID(userId.ToString(), myKey);
                           if (!string.IsNullOrEmpty(idFilter0))
                           {
                               filterEntity.searchCity = Convert.ToString(idFilter0);
                           }
                           if (!string.IsNullOrEmpty(idFilter1 ))
                           {
                               filterEntity.searchState = Convert.ToString(idFilter1);
                           }
                           if (!string.IsNullOrEmpty(idFilter2))
                           {
                               filterEntity.searchCountry = Convert.ToString(idFilter2);
                           }
                           if (!string.IsNullOrEmpty(idFilter3))
                           {
                               filterEntity.searchZip = Convert.ToString(idFilter3);
                           } */
                        var filter = new DatatableFilters { currentUserId = userId, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };

                        var schedulersetupModel = this.svc.GetSchedulerSetup(filter);


                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            //Sorting

                            filteredSchedulerSetup = schedulersetupModel.lstSchedulerSetup;
                            if (param.sSortDir_0 != null)
                            {
                                var sortColumnIndex = param.iSortCol_0;
                                var sortField = string.Empty;
                                if (lstFieldMetadata.Count > sortColumnIndex)
                                {
                                    if (sortColumnIndex > 0)
                                        sortColumnIndex--;

                                    sortField = lstFieldMetadata[sortColumnIndex].fieldName;
                                }

                                filteredSchedulerSetup = filteredSchedulerSetup.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = SchedulerSetupData(filteredSchedulerSetup, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = schedulersetupModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = schedulersetupModel.recordDetails.totalDisplayRecords;

                            return GetDataBindingJsonResult(sEcho, iTotalRecords, iTotalDisplayRecords, result);
                        }
                        else
                        {
                            return this.Msg_ErrorInService();
                        }
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInViewOrEdit();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        public List<string[]> SchedulerSetupData(List<SchedulerSetups> schedulersetupEntry, int currentUserId)
        {
            var result = this.GetDatatableData<SchedulerSetups>(schedulersetupEntry, currentUserId);
            return result;
        }
        [HttpPost]
        [Authorize]
        public virtual JsonResult Maintenance(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            var _model = new SchedulerSetupMaintenanceModel();

            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _model.record_Id = id;
                _model.readOnly = readOnly;
                if (_accessRight != null)
                {
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        _model.tableProfile = this.GetTableProfileWithTab();
                        _model.SchedulerSetup = GetSchedulerSetupModel(id, readOnly);

                        if (_model.SchedulerSetup == null)
                        {
                            return this.Msg_ErrorInService();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.SchedulerSetup.Views.Maintenance, _model)
                        });
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInViewOrEdit();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }

        }


        private SchedulerSetupModel GetSchedulerSetupModel(string id, bool readOnly)
        {
            try
            {
                SchedulerSetupModel _model = new SchedulerSetupModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;
                SchedulerSetups SchedulerSetup = new SchedulerSetups();

                if (Session["SchedulerSetupId"] != null && long.TryParse(Session["SchedulerSetupId"].ToString(), out long parsedId))
                {
                    SchedulerSetup = svc.GetSchedulerSetupById(Convert.ToInt32(id));
                }
                else
                {
                    var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    SchedulerSetup = svc.GetSchedulerSetupById(decrypted_Id);

                }
                if (SchedulerSetup != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    _model.id = id;
                    _model.isEdit = true;
                    // _model.syncModule_Id = schedulersetup.syncModule_Id;
                    _model.startDate = SchedulerSetup.startDate;
                    //_model.timeSlot = schedulersetup.timeSlot;
                    _model.isActive = SchedulerSetup.isActive;
                }
                var _syncModuleList = svcCommon.GetListModuleItem(_model.syncModule_Id_Metadata.dropdownModuleType_Id);
                _model.lstsyncModule = _syncModuleList.Select(a => new SelectListItem
                {
                    Text = a.name,
                    Value = a.id.ToString(),
                    Selected = (a.id == SchedulerSetup.syncModule_Id)

                });

                var _timeSlotList = svcCommon.GetListModuleItem(_model.timeSlot_Metadata.dropdownModuleType_Id);
                _model.lsttimeSlot = _timeSlotList.Select(a => new SelectListItem
                {
                    Text = a.name,
                    Value = a.id.ToString(),
                    Selected = (a.id == SchedulerSetup.timeSlot)

                });

                return _model;
            }
            catch
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public virtual JsonResult RenderAction(bool readOnly)
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, "RenderAction", _accessRight)
                });
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        #endregion

        #region Add,Edit and Delete

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

                if (!_accessRight.canAdd)
                {
                    return this.Msg_AccessDeniedInAdd();
                }

                if (svc != null)
                {
                    var _model = new SchedulerSetupModel();

                    this.FillupFieldMetadata(_model, false);
                    _model.isActive = true;
                    if (_model.syncModule_Id_Metadata.isEditableInCreate)
                    {
                        var _syncModuleList = svcCommon.GetListModuleItem(_model.syncModule_Id_Metadata.dropdownModuleType_Id);
                        _model.lstsyncModule = _syncModuleList.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                        });
                    }                   

                    if (_model.timeSlot_Metadata.isEditableInCreate)
                    {
                        var _timeSlotList = svcCommon.GetListModuleItem(_model.timeSlot_Metadata.dropdownModuleType_Id);
                        _model.lsttimeSlot = _timeSlotList.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                        });
                    }

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                    System.Diagnostics.Debug.WriteLine(_model != null ? "Model is valid" : "Model is null");

                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, "Create", _model)
                    });
                }
                else
                {
                    return this.Msg_ErrorInService();
                }

            }
            catch (Exception ex)
            {
                //throw new ErrorException(ex.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveSchedulerSetup(SchedulerSetupModel model)
        {
            try
            {
                CheckModelState(ModelState, model.isEdit);
                if (ModelState.IsValid)
                {
                    AccessRightsModel _accessRight = new AccessRightsModel();

                    var currentUserId = this.CurrentUserId;

                    _accessRight = Common.GetUserRights(currentUserId, PAGE_URLForAccessRight);

                    if (_accessRight != null)
                    {
                        int decrypted_Id = 0;


                        decrypted_Id = Common.DecryptToID(currentUserId.ToString(), model.id);
                        bool isUpdate = false;
                        if (decrypted_Id > 0)
                            isUpdate = true;

                        if (isUpdate)
                        {
                            if (!_accessRight.canEdit)
                            {
                                return this.Msg_AccessDeniedInEdit();
                            }
                        }
                        else
                        {
                            if (!_accessRight.canAdd)
                            {
                                return this.Msg_AccessDeniedInEdit();
                            }
                        }

                        if (svc != null)
                        {
                            SchedulerSetups entity = new SchedulerSetups();
                            entity.id = decrypted_Id;
                            entity.syncModule_Id = model.syncModule_Id;
                            entity.startDate = model.startDate;
                            entity.timeSlot = model.timeSlot;
                            entity.isActive = model.isActive;
                            entity.createdBy = this.CurrentUserId;
                            entity.createdDate = DateTime.Now;
                          

                            bool isExists = svc.IsSchedulerSetupExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveSchedulerSetup(entity);

                                if (save != null && save.isSuccess)
                                {
                                    var new_Id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString());
                                    return this.Msg_SuccessInSave(new_Id, isUpdate);

                                }
                                else
                                {
                                    return this.Msg_ErrorInSave(isUpdate);
                                }
                            }
                            else
                            {
                                return this.Msg_WarningExistRecord(Resources.MSG_SCHEDULERSETUP_EXIST_RECORD);
                            }
                        }
                        else
                        {
                            return this.Msg_ErrorInService();
                        }
                    }
                    else
                    {
                        return this.Msg_AccessDenied();
                    }

                }
                else
                {
                    return this.Msg_ErrorInvalidModel();
                }

            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }


        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            try
            {
                var currentUserId = this.CurrentUserId;

                var _accessRight = Common.GetUserRights(currentUserId, PAGE_URLForAccessRight);

                if (_accessRight != null)
                {
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        var decrypted_Id = Common.DecryptToID(currentUserId.ToString(), id);

                        var schedulersetup = svc.GetSchedulerSetupById(decrypted_Id);

                        if (schedulersetup != null)
                        {
                            SchedulerSetupModel _model = new SchedulerSetupModel();
                            this.FillupFieldMetadata(_model, true);
                            _model.readOnly = readOnly;


                            var _syncModuleList = svcCommon.GetListModuleItem(_model.syncModule_Id_Metadata.dropdownModuleType_Id);
                                _model.lstsyncModule = _syncModuleList.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == schedulersetup.syncModule_Id)

                                });
                           
                                var _timeSlotList = svcCommon.GetListModuleItem(_model.timeSlot_Metadata.dropdownModuleType_Id);
                                _model.lsttimeSlot = _timeSlotList.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == schedulersetup.timeSlot)

                                });
                            

                            this.FillupFieldMetadata(_model, true);

                            _model.readOnly = readOnly;

                            _model.id = id;
                            _model.isEdit = true;
                           // _model.syncModule_Id = schedulersetup.syncModule_Id;
                            _model.startDate = schedulersetup.startDate;
                            //_model.timeSlot = schedulersetup.timeSlot;
                            _model.isActive = schedulersetup.isActive;
                          //  _model.createdBy = schedulersetup.createdBy;
                          //  _model.createdDate = schedulersetup.createdDate;

                            return Json(new
                            {
                                viewMarkup = Common.RenderPartialViewToString(this, "Create", _model)
                            });
                        }
                        else
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInViewOrEdit();
                    }
                }
                else
                {
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }

   


        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

                if (_accessRight != null)
                {
                    int decrypted_Id = 0;
                    decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteSchedulerSetup(decrypted_Id);

                            if (isSuccess)
                            {
                                return this.Msg_SucessInDelete();
                            }
                            else
                            {
                                return this.Msg_ErrorInDelete();
                            }
                        }
                        else
                        {
                            return this.Msg_ErrorInService();
                        }
                    }
                    else
                    {
                        return this.Msg_AccessDeniedInDelete();
                    }
                }
            }
            catch (Exception ex)
            {
                return this.Msg_ErrorInRetriveData(ex);
            }
            return View();
        }


        #endregion
       
    }
}
