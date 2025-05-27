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
using System.Data;
using System.Globalization;
using Newtonsoft.Json;

namespace Hanodale.WebUI.Controllers
{
    public partial class LooseConversionController : BaseController
    {
        #region Declaration
        readonly string PAGE_URL = string.Empty;

        #endregion

        #region Constructor

        private readonly ILooseConversionService svc;
        private readonly IProductService psvc;
        private readonly IProductService svcProduct;

        private readonly IProductWeightBarcodeService svcw;


        public LooseConversionController(ILooseConversionService _bLService, IProductService _svcProduct, ICommonService _svcCommon,  IProductWeightBarcodeService _svcpwb)
        {
            this.svcCommon = _svcCommon;
            this.svcProduct = _svcProduct;
            this.sectionName = "LooseConversion";
            this.svc = _bLService;
            this.svcw = _svcpwb;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["LooseConversion_Menu_Id"]);

            PAGE_URL = this.sectionName + "/Index";
        }
        #endregion

        #region LooseConversion Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("This is called");
            try
            {
                var _model = this.GetVisibleColumnForGridView(new LooseConversionModel());

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
        public virtual JsonResult LooseConversion()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new LooseConversionModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.LooseConversion.Views.Index, _model)
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
        public virtual ActionResult BindLooseConversion(DataTableModel param)
        {
            int totalRecordCount = 0;
            List<LooseConversions> filteredLooseConversions = null;
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
                        var LooseConversionModel = this.svc.GetLooseConversion(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            filteredLooseConversions = LooseConversionModel.lstLooseConversion;
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

                                filteredLooseConversions = filteredLooseConversions.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = LooseConversionData(filteredLooseConversions, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = LooseConversionModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = LooseConversionModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> LooseConversionData(List<LooseConversions> LooseConversionEntry, int currentUserId)
        {
            var result = this.GetDatatableData<LooseConversions>(LooseConversionEntry, currentUserId);
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.LooseConversion.Views.RenderAction, _accessRight)
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
                var LooseConv = WebConfigurationManager.AppSettings["LooseConv"];
               // var readOnly = false;

                if (!_accessRight.canAdd)
                {
                    return this.Msg_AccessDeniedInAdd();
                }

                if (svc != null)
                {
                    var _model = new LooseConversionModel();

                    this.FillupFieldMetadata(_model, false);

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    _model.LooseConv = LooseConv;
                    ViewBag.LooseConversionItemsJson = JsonConvert.SerializeObject(_model.looseItems);
                    ViewBag.isEdit = JsonConvert.SerializeObject(_model.isEdit);
                   // ViewBag.IsReadOnly = readOnly;
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.LooseConversion.Views.Create, _model)
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
        public virtual JsonResult SaveLooseConversion(LooseConversionModel model)
        {
            try
            {
                var LooseCov = WebConfigurationManager.AppSettings["LooseCov"];

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
                            LooseConversions entity = new LooseConversions
                            {
                                id = decrypted_Id,
                                productCarton_Id = model.productCarton_Id,
                                postingStatus ="Draft",
                                createdBy = this.UserName,
                                createdDate = DateTime.Now
                            };

                            //bool isExists = svc.IsLooseConversionExists(entity);
                            if (true)
                            {
                                // Save main entity
                                var save = svc.SaveLooseConversion(entity);

                                if (save != null && save.isSuccess)
                                {
                                    // Get the saved entity ID for linking items
                                    var new_Id = save.id;
                                    List<LooseConversionItems> looseConversionItemsList = new List<LooseConversionItems>();

                                    // Save each LooseItem associated with the LooseConversion
                                    foreach (var looseItem in model.looseItems)
                                    {
                                        LooseConversionItems itemEntity = new LooseConversionItems
                                        {
                                            barcode = looseItem.LooseBarcode,
                                            LooseQty = looseItem.LooseQty,
                                            RunningBalance = looseItem.RunningBalance,
                                            looseConversion_Id = new_Id,
                                            createdBy = this.UserName,
                                            
                                            //weighScaleBarcode_Id = looseItem.weighScaleBarcode_Id,
                                        };

                                        // Save each LooseConversionItem
                                        looseConversionItemsList.Add(itemEntity);
                                        //var itemSaveResult = svc.SaveLooseConversionItem(itemEntity);
                                      
                                    }
                                    var saveResult = svc.SaveLooseConversionItem(looseConversionItemsList);

                                    // Return success with new ID
                                    var encrypted_Id = Common.Encrypt(this.CurrentUserId.ToString(), new_Id.ToString());

                                    //return this.Msg_SuccessInSave(encrypted_Id, isUpdate);
                                    //var _model = this.GetVisibleColumnForGridView(new LooseConversionModel());
                                    //_model.accessRight = _accessRight;
                                    bool success = true;
                                    return Json(new { success });
                                  
                                }
                                else
                                {
                                    return this.Msg_ErrorInSave(isUpdate);
                                }
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Warning.ToString(),
                                    message = "Conversion already exist for the same conversion barcode!",
                                });
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
        [HttpPost]
        [Authorize]
        public virtual JsonResult SaveWeightBarcode(ProductWeightBarcodes model)
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
                        decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id.ToString());
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
                        var loosebarcodesettingid = 1;
                        var looosebarcodesetting = svc.GetLooseBarcodeSettingById(loosebarcodesettingid);

                        if (svc != null)
                        {
                            ProductWeightBarcodes entity = new ProductWeightBarcodes();
                            entity.id = decrypted_Id;
                            entity.epicorePartNo = model.epicorePartNo;
                            entity.description = model.description;
                            entity.fullBarcode = model.fullBarcode;
                            entity.barcode = model.barcode;
                            entity.barcodeFromPos = looosebarcodesetting.barcodeFromPos??0;
                            entity.barcodeToPos = looosebarcodesetting.barcodeToPos??0;
                            entity.weightFromPos = looosebarcodesetting.weightFromPos ?? 0;
                            entity.weightToPos = looosebarcodesetting.weightToPos??0;
                            entity.weightValue = model.weightValue;
                            entity.weightMultiply = looosebarcodesetting.weightMutiply  ?? 0;
                            entity.barcodeLength = looosebarcodesetting.barcodeLength ?? 0;
                            entity.offSet1 = model.offSet1;
                            entity.offSet2 = model.offSet2;

                            bool isExists = svcw.IsProductWeightBarcodeExists(entity);
                            if (!isExists)
                            {
                                // Save main entity
                                var save = svcw.SaveProductWeightBarcode(entity);

                                if (save != null && save.isSuccess)
                                {
                                    // Get the saved entity ID for linking items
                                    var new_Id = save.id;

                                   
                                    // Return success with new ID
                                    var encrypted_Id = Common.Encrypt(this.CurrentUserId.ToString(), new_Id.ToString());
                                    return this.Msg_SuccessInSave(encrypted_Id, isUpdate);
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

        private LooseConversionModel GetLooseConversionModel(string id, bool readOnly)
        {
            try
            {
                LooseConversionModel _model = new LooseConversionModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var productCarton = svc.GetLooseConversionById(decrypted_Id);

                if (productCarton != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    // _model.epicorPartNo = productCarton.epicorPartNo;
                    _model.barcode = productCarton.barcode;

                    _model.looseItems = productCarton.LooseConversionItems
         .Select(item => new LooseConversionItemsModel
         {
             Id = item.id,
             LooseConversionId = (int)item.looseConversion_Id,
             LooseQty = (decimal)item.LooseQty,
             RunningBalance = (decimal)item.RunningBalance,
             LooseBarcode=item.barcode
         })
         .ToList();


                }
                /*if (_model.epicorPartNo_Metadata.isEditableInCreate)
                {
                    var _productList = svcProduct.GetProductList(new Products());

                    _model.lstProduct = _productList.Select(a => new SelectListItem
                    {
                        Text = a.partNumber,
                        Value = a.partNumber,
                        Selected = (a.partNumber == productCarton.epicorPartNo)
                    });
                }*/

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
                        var _model = GetLooseConversionModel(id, readOnly);

                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }

                        ViewBag.LooseConversionItemsJson = JsonConvert.SerializeObject(_model.looseItems);
                        ViewBag.isEdit = JsonConvert.SerializeObject(_model.isEdit);
                        ViewBag.IsReadOnly = readOnly; 
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.LooseConversion.Views.Create, _model)
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
                        var result = svc.DeleteLooseConversion(decrypted_Id);

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

        [HttpPost]
        public virtual JsonResult CheckBarcodeExists(string barcode, string partNo)
        {
            bool exists = svcw.CheckBarcodeExists(barcode, partNo);
            return Json(new { exists });
        }

        [HttpPost]
        public virtual JsonResult fetchLooseQty(string barcode)
        {
            decimal calculatedLooseQty = 0;
            string _labelWeight = "0.0000";
            var _settings = svc.GetLooseBarcodeSettingById(0);

            //check barcode length
            if (barcode.Length < _settings.barcodeLength)
            {
                return Json(new
                {
                    looseQty = calculatedLooseQty,
                    error = "Barcode length should be equal or more than " + _settings.barcodeLength.ToString()
                });
            }

            if (barcode.Length >= _settings.weightToPos && _settings.weightFromPos != 0)
            {
                int _startPos = (_settings.weightFromPos ?? 0) - 1;
                int _length = ((_settings.weightToPos ?? 0) - (_settings.weightFromPos ?? 0)) + 1;
                _labelWeight = barcode.Substring(_startPos, _length);
                calculatedLooseQty = (Convert.ToDecimal(_labelWeight) * _settings.weightMutiply ?? 0);
                return Json(new { looseQty = calculatedLooseQty, error = "" });
            }
            else
            {
                return Json(new { looseQty = calculatedLooseQty, error = "Weight Postions are not well defined!" });
            }
        }

        #endregion
    }

}