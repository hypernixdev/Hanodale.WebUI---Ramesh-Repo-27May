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
using System.Runtime.InteropServices;


namespace Hanodale.WebUI.Controllers
{
    public partial class SchedulerLogController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string PAGE_URLForAccessRight = "Product/Index";

        #endregion

        #region Constructor

        private readonly ISchedulerLogService svc;

        public SchedulerLogController(ISchedulerLogService _bLService, ICommonService _svcCommon)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "SchedulerLog";
            this.svc = _bLService;
            this.parentMenu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["SchedulerSetup_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
        }

        #endregion

        #region Uom Conversion Details

        [AppAuthorize]
        public virtual ActionResult Index(string id, bool readOnly)
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new SchedulerLogModel());

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

                _model.accessRight = _accessRight;

                _model.masterRecord_Id = id;
                _model.readOnly = readOnly;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.SchedulerLog.Views.Index, _model)
                        });
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
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }

        }


        [Authorize]
        public virtual ActionResult BindSchedulerLog(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            List<SchedulerLogs> filteredSchedulerLog = null;
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

                        int masterRecord_Id = 0;
                        if (myKey != null)
                            masterRecord_Id = Common.DecryptToID(userId.ToString(), myKey);

                        var filter = new DatatableFilters { currentUserId = userId, masterRecord_Id = masterRecord_Id, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };
                        var uomConversionModel = this.svc.GetSchedulerLog(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            //Sorting

                            filteredSchedulerLog = uomConversionModel.lstSchedulerLog;
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

                                filteredSchedulerLog = filteredSchedulerLog.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = SchedulerLogData(filteredSchedulerLog, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = uomConversionModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = uomConversionModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> SchedulerLogData(List<SchedulerLogs> uomConversionEntry, int currentUserId)
        {
            var result = this.GetDatatableData<SchedulerLogs>(uomConversionEntry, currentUserId);
            return result;
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.SchedulerLog.Views.RenderAction, _accessRight)
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
        public virtual JsonResult Create(string id)
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
                    var _model = new SchedulerLogModel();

                    this.FillupFieldMetadata(_model, false);

                   // _model.partNumId = id;

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    //TryValidateModel(_uomConversionModel);
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.SchedulerLog.Views.Create, _model)
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
        public virtual JsonResult SaveSchedulerLog(SchedulerLogModel model)
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
                            SchedulerLogs entity = new SchedulerLogs();
                            entity.id = decrypted_Id;
                            //  entity.partNum = Common.DecryptToID(currentUserId.ToString(), model.partNum);
                           // entity.partNum = Common.DecryptToID(currentUserId.ToString(), model.partNumId).ToString();

                            //entity.company = model.company;
                            //entity.uomCode = model.uomCode;
                            //entity.convFactor = model.convFactor;
                            //entity.uniqueField = model.uniqueField;
                            //entity.convOperator = model.convOperator;

                            bool isExists = svc.IsSchedulerLogExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveSchedulerLog(entity);

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
                                return this.Msg_WarningExistRecord(Resources.MSG_COMPANYPROFILE_EXIST);
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

                        var uomConversion = svc.GetSchedulerLogById(decrypted_Id);

                        if (uomConversion != null)
                        {
                            SchedulerLogModel _model = new SchedulerLogModel();
                            this.FillupFieldMetadata(_model, true);

                            _model.readOnly = readOnly;

                            _model.id = id;
                            _model.isEdit = true;
                            //_model.partNumId = Common.Encrypt(currentUserId.ToString(), uomConversion.partNum.ToString());

                            //_model.company = uomConversion.company;
                            //_model.partNumId = uomConversion.partNum;
                            //_model.uomCode = uomConversion.uomCode;
                            //_model.convFactor = uomConversion.convFactor;
                            //_model.uniqueField = uomConversion.uniqueField;
                            //_model.convOperator = uomConversion.convOperator;



                            return Json(new
                            {
                                viewMarkup = Common.RenderPartialViewToString(this, MVC.SchedulerLog.Views.Create, _model)
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
                            bool isSuccess = svc.DeleteSchedulerLog(decrypted_Id);

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
