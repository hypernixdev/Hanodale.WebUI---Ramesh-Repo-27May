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
using System.Web;
using System.IO;
using Hanodale.Entity.Core;


namespace Hanodale.WebUI.Controllers
{
    public partial class ShipToAddressController : BaseController
    {
        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string ORDERS_PAGE_URL = string.Empty;
        #endregion

        #region Constructor

        private readonly IShipToAddressService svc;
        private readonly ISyncManager syncManager;
        public ShipToAddressController(IShipToAddressService _bLService, ICommonService _svcCommon, ISyncManager syncManager)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "ShipToAddress";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["ShipToAddress_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            ORDERS_PAGE_URL = "Orders/Index";
            this.syncManager = syncManager;
        }
        #endregion

        #region ShipToAddress Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("This is called");
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ShipToAddressModel());

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

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

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult ShipToAddress()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ShipToAddressModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ShipToAddress.Views.Index, _model)
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
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [Authorize]
        public virtual ActionResult BindShipToAddress(DataTableModel param)
        {
            int totalRecordCount = 0;
            List<ShipToAddresses> filteredShipToAddresses = null;
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        int userId = this.CurrentUserId;

                        var filter = new DatatableFilters { currentUserId = this.CurrentUserId, all = true, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };
                        var ShipToAddressModel = this.svc.GetShipToAddress(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            filteredShipToAddresses = ShipToAddressModel.lstShipToAddress;
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

                                filteredShipToAddresses = filteredShipToAddresses.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = ShipToAddressData(filteredShipToAddresses, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = ShipToAddressModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = ShipToAddressModel.recordDetails.totalDisplayRecords;

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
                return Msg_ErrorInRetriveData(ex);
            }
        }

        public List<string[]> ShipToAddressData(List<ShipToAddresses> ShipToAddressEntry, int currentUserId)
        {
            var result = this.GetDatatableData<ShipToAddresses>(ShipToAddressEntry, currentUserId);
            return result;
        }

        [Authorize]
        [HttpPost]
        public virtual JsonResult RenderAction()
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ShipToAddress.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        #endregion

        #region Add, Edit, and Delete

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (!_accessRight.canAdd)
                {
                    return this.Msg_AccessDeniedInAdd();
                }

                if (svc != null)
                {
                    var _model = new ShipToAddressModel();

                    this.FillupFieldMetadata(_model, false);

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.ShipToAddress.Views.Create, _model)
                    });
                }
                else
                {
                    return this.Msg_ErrorInService();
                }

            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveShipToAddress(ShipToAddressModel model)
        {
            try
            {
                CheckModelState(ModelState, model.isEdit);
                if (ModelState.IsValid)
                {
                    AccessRightsModel _accessRight = new AccessRightsModel();

                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int decrypted_Id = 0;
                        decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
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
                                return this.Msg_AccessDeniedInAdd();
                            }
                        }

                        if (svc != null)
                        {
                            ShipToAddresses entity = new ShipToAddresses();
                            entity.id = decrypted_Id;
                          
                            bool isExists = svc.IsShipToAddressExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveShipToAddress(entity);

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
                                return this.Msg_ErrorInSave(isUpdate);
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
                return Msg_ErrorInRetriveData(ex);
            }
        }

        private ShipToAddressModel GetShipToAddressModel(string id, bool readOnly)
        {
            try
            {
                ShipToAddressModel _model = new ShipToAddressModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var shiptoAddress = svc.GetShipToAddressById(decrypted_Id);

                if (shiptoAddress != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    _model.custId = shiptoAddress.custId;
                    _model.name = shiptoAddress.name;
                    _model.shippingCode = shiptoAddress.shippingCode;
                    _model.address1 = shiptoAddress.address1;
                    _model.address2 = shiptoAddress.address2;
                    _model.address3 = shiptoAddress.address3;
                    _model.cityName = shiptoAddress.cityName;
                    _model.stateName = shiptoAddress.stateName;
                    _model.countryName = shiptoAddress.countryName;
                    _model.zip = shiptoAddress.zip;

                }

                return _model;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int decrypted_Id = 0;
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        var _model = GetShipToAddressModel(id, readOnly);

                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ShipToAddress.Views.Create, _model)
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

        [HttpPost]
        [Authorize]
        public virtual JsonResult Delete(string id)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (!_accessRight.canDelete)
                    {
                        return this.Msg_AccessDeniedInDelete();
                    }

                    var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                    if (svc != null)
                    {
                        var result = svc.DeleteShipToAddress(decrypted_Id);

                        if (result)
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
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        #endregion

        #region Helpers

        private void CheckModelState(ModelStateDictionary modelState, bool isEdit)
        {
            if (modelState.IsValid) return;

            foreach (var key in modelState.Keys)
            {
                if (modelState[key].Errors.Count > 0)
                {
                    throw new ValidationException(modelState[key].Errors[0].ErrorMessage);
                }
            }
        }

        /* private void FillupFieldMetadata(ShipToAddressModel model, bool isEdit)
         {
             // Implementation for field metadata population
         }*/

        #endregion
        #region Sync service 
        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncShipToAddresses()
        {
            try
            {
                var success = this.syncManager.syncEntity("ShipToAddress", "shipTo", "shippingCode", false, "");
                return Json(new
                {
                    success = success.Result,
                    message = success.Message
                });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new
                {
                    success = false,
                    message = "An error occurred during sync",
                    error = ex.Message
                });
            }
        }
        #endregion
        #region Order Screen Search
        [HttpPost]
        [Authorize]
        public virtual JsonResult GetShipToAddressesByCustomerId(int customerId, string searchby = null)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canAdd || _accessRight.canEdit)
                    {
                        var shipToAddresses = this.svc.GetShipToAddressByCustomerId(customerId, searchby);
                        if (shipToAddresses == null || !shipToAddresses.Any())
                        {
                            return this.Msg_ErrorInRetriveData();  // Handle case when no data is found
                        }

                        return Json(new
                        {
                            success = true,
                            ShipToAddresses = shipToAddresses
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
                return Msg_ErrorInRetriveData(ex);
            }
        }

        #endregion

    }
}