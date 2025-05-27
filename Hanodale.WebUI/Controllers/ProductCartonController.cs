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


namespace Hanodale.WebUI.Controllers
{
    public partial class ProductCartonController : BaseController
    {
        #region Declaration
        readonly string PAGE_URL = string.Empty;

        #endregion

        #region Constructor

        private readonly IProductCartonService svc;
        private readonly IProductService psvc;
        private readonly IProductService svcProduct;


        public ProductCartonController(IProductCartonService _bLService, IProductService _svcProduct, ICommonService _svcCommon)
        {
            this.svcCommon = _svcCommon;
            this.svcProduct = _svcProduct;
            this.sectionName = "ProductCarton";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["ProductCarton_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
        }
        #endregion

        #region ProductCarton Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("This is called");
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ProductCartonModel());

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
        public virtual JsonResult ProductCarton()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ProductCartonModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductCarton.Views.Index, _model)
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
        public virtual ActionResult BindProductCarton(DataTableModel param)
        {
            int totalRecordCount = 0;
            List<ProductCartons> filteredProductCartons = null;
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
                        var ProductCartonModel = this.svc.GetProductCarton(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            filteredProductCartons = ProductCartonModel.lstProductCarton;
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

                                filteredProductCartons = filteredProductCartons.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = ProductCartonData(filteredProductCartons, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = ProductCartonModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = ProductCartonModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> ProductCartonData(List<ProductCartons> ProductCartonEntry, int currentUserId)
        {
            var result = this.GetDatatableData<ProductCartons>(ProductCartonEntry, currentUserId);
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductCarton.Views.RenderAction, _accessRight)
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
                    var _model = new ProductCartonModel();

                    this.FillupFieldMetadata(_model, false);

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    if (_model.epicorPartNo_Metadata.isEditableInCreate)
                    {
                        var _productList = svcProduct.GetProductList(new Products());

                        _model.lstProduct = _productList.Select(a => new SelectListItem
                        {
                            Text = a.partNumber,
                            Value = a.partNumber,
                            Selected = (a.id == _model.epicorPartNo_Metadata.dropdownDefaultValue)
                        });
                    }
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductCarton.Views.Create, _model)
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
        public virtual JsonResult SaveProductCarton(ProductCartonModel model)
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
                            ProductCartons entity = new ProductCartons();
                            entity.id = decrypted_Id;
                            entity.epicorPartNo = model.epicorPartNo;
                            entity.barcode = model.barcode;
                            entity.productKey = model.productKey;
                            entity.vendorProductCode = model.vendorProductCode;
                            entity.productBarcodeLength = model.productBarcodeLength;
                            entity.productCodeFromPosition = model.productCodeFromPosition;
                            entity.productCodeToPosition = model.productCodeToPosition;
                            entity.weightFromPosition = model.weightFromPosition;
                            entity.weightToPosition = model.weightToPosition;
                            entity.weightValue = model.weightValue;
                            entity.weightMutiplier = model.weightMutiplier;
                           
                            if (isUpdate)
                            {
                                entity.modifiedBy = this.UserName;
                                entity.modifiedDate = DateTime.Now;
                            }
                            else
                            {                 
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            bool isExists = svc.IsProductCartonExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveProductCarton(entity);

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

        private ProductCartonModel GetProductCartonModel(string id, bool readOnly)
        {
            try
            {
                ProductCartonModel _model = new ProductCartonModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var productCarton = svc.GetProductCartonById(decrypted_Id);

                if (productCarton != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    // _model.epicorPartNo = productCarton.epicorPartNo;
                    _model.barcode = productCarton.barcode;
                    _model.vendorProductCode = productCarton.vendorProductCode;
                    _model.productKey = productCarton.productKey;
                    _model.productBarcodeLength = productCarton.productBarcodeLength;
                    _model.productCodeFromPosition = productCarton.productCodeFromPosition;
                    _model.productCodeToPosition = productCarton.productCodeToPosition;
                    _model.weightFromPosition = productCarton.weightFromPosition;
                    _model.weightToPosition = productCarton.weightToPosition;
                    _model.weightValue = productCarton.weightValue;
                    _model.weightMutiplier = productCarton.weightMutiplier;
                    _model.weightValue1 = productCarton.weightValue;
                    _model.weightMutiplier1 = productCarton.weightMutiplier;

                }
                if (_model.epicorPartNo_Metadata.isEditableInCreate)
                {
                    var _productList = svcProduct.GetProductList(new Products());

                    _model.lstProduct = _productList.Select(a => new SelectListItem
                    {
                        Text = a.partNumber,
                        Value = a.partNumber,
                        Selected = (a.partNumber == productCarton.epicorPartNo)
                    });
                }

                return _model;
            }
            catch
            {
                return null;
            }
        }
        public virtual ActionResult DownloadCSVTemplate()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Templates\ProductCarton_Template.csv");

            if (!System.IO.File.Exists(filePath))
            {

            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/csv", "ProductCarton_Template.csv");
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
                        var _model = GetProductCartonModel(id, readOnly);

                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductCarton.Views.Create, _model)
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
                        var result = svc.DeleteProductCarton(decrypted_Id);

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

        [Authorize]
        [HttpPost]
        public virtual JsonResult UploadProductCartonFile()
        {
            string direction = @"" + System.Configuration.ConfigurationManager.AppSettings["TempFilePath"];
            string filePath = string.Empty;
            bool hasSavedFile = false;

            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (!_accessRight.canView)
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
                        status = Common.Status.Error.ToString(),
                        message = Resources.MSG_ERR_SERVICE
                    });
                }

                if (svc != null)
                {
                    var currentDateTime = DateTime.Now;

                    var entityEn = new ProductCartonFileUpload();
                    entityEn.organization_Id = this.CurrentUserId;


                    var _model = new ProductCartonModel();
                    this.FillupFieldMetadata(_model, false);

                    if (Request.Files[0].ContentLength > 0)
                    {
                        string fileExtension = System.IO.Path.GetExtension(Request.Files[0].FileName);

                        string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                        string query = null;
                        string connString = "";


                        if (validFileTypes.Contains(fileExtension))
                        {
                            

                            DataTable dt = new DataTable();

                            if (fileExtension == ".csv")
                            {
                                dt = Common.ConvertCSVtoDataTable(Request.Files[0]);
                            }
                            else
                            {
                                hasSavedFile = true;

                                bool exists = System.IO.Directory.Exists(direction);
                                if (!exists)
                                {
                                    System.IO.Directory.CreateDirectory(direction);
                                }

                                string fileName = Path.GetFileName(Request.Files[0].FileName);

                                filePath = direction + Request.Files[0].FileName;
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }

                                Request.Files[0].SaveAs(filePath);

                                dt = Common.ConvertXSLXtoDataTable(filePath, (fileExtension.Trim() == ".xlsx"));
                            }

                            if (dt.Rows.Count > 0)
                            {
                                var columnName = string.Empty;
                                var _header = dt.Columns;

                                var productKeyHeaderName = "PRODUCT_KEY";
                                var epicorPartNoHeaderName = "EPICOR_ITEM_CODE";
                                var barcodeHeaderName = "PRODUCT_BARCODE_ID";
                                var vendorProductCodeHeaderName = "PRODUCT_ACTUAL_BARCODE_ID";
                                var productBarcodeLengthHeaderName = "PRODUCT_BARCODE_LENGTH";
                                var productCodeFromPositionHeaderName = "PRODUCT_CODE_FROM_POSITION";
                                var productCodeToPositionHeaderName = "PRODUCT_CODE_TO_POSITION";
                                var weightFromPositionHeaderName = "WEIGHT_FROM_POSITION";
                                var weightToPositionHeaderName = "WEIGHT_TO_POSITION";
                                var weightMutiplierHeaderName = "WEIGHT_MULTIPLIER";

                                int rowIndex = 1;

                                entityEn.lstProductCarton = new List<ProductCartons>();


                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row != null)
                                    {
                                        //var selectedRow = row.Split(',');

                                        var selectedRow = row;

                                        var details = new ProductCartons();
                                        details.id = rowIndex;

                                        var productKey = selectedRow[productKeyHeaderName].ToString().Trim();
                                        var epicorPartNo = selectedRow[epicorPartNoHeaderName].ToString().Trim();
                                        var barcode = selectedRow[barcodeHeaderName].ToString().Trim();
                                        var vendorProductCode = selectedRow[vendorProductCodeHeaderName].ToString().Trim();
                                        var productBarcodeLength = selectedRow[productBarcodeLengthHeaderName].ToString();
                                        var productCodeFromPosition = selectedRow[productCodeFromPositionHeaderName].ToString();
                                        var productCodeToPosition = selectedRow[productCodeToPositionHeaderName].ToString();
                                        var weightFromPosition = selectedRow[weightFromPositionHeaderName].ToString();
                                        var weightToPosition = selectedRow[weightToPositionHeaderName].ToString();
                                        //var weightValue = selectedRow["ACTUAL_WEIGHT_BARCODE_ID"].ToString();
                                        var weightMutiplier = selectedRow[weightMutiplierHeaderName].ToString();

                                        details.rowIndex = rowIndex;
                                        details.barcode = barcode;
                                        details.epicorPartNo = epicorPartNo;
                                        details.vendorProductCode = vendorProductCode;
                                        //details.weightValue = weightValue;

                                        if (!string.IsNullOrEmpty(productKey))
                                        {
                                            decimal _productKey = 0;

                                            if (decimal.TryParse(productKey, out _productKey))
                                            {
                                                details.productKey = _productKey;
                                            }
                                            else
                                            {
                                                columnName = productKeyHeaderName;
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(productBarcodeLength))
                                        {
                                            decimal _productBarcodeLength = 0;

                                            if (decimal.TryParse(productBarcodeLength, out _productBarcodeLength))
                                            {
                                                details.productBarcodeLength = _productBarcodeLength;
                                            }
                                            else
                                            {
                                                columnName = productBarcodeLengthHeaderName;
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(productCodeFromPosition))
                                        {
                                            decimal _productCodeFromPosition = 0;

                                            if (decimal.TryParse(productCodeFromPosition, out _productCodeFromPosition))
                                            {
                                                details.productCodeFromPosition = _productCodeFromPosition;
                                            }
                                            else
                                            {
                                                columnName = productCodeFromPositionHeaderName;
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(productCodeToPosition))
                                        {
                                            decimal _productCodeToPosition = 0;

                                            if (decimal.TryParse(productCodeToPosition, out _productCodeToPosition))
                                            {
                                                details.productCodeToPosition = _productCodeToPosition;
                                            }
                                            else
                                            {
                                                columnName = productCodeToPositionHeaderName;
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(weightFromPosition))
                                        {
                                            decimal _weightFromPosition = 0;

                                            if (decimal.TryParse(weightFromPosition, out _weightFromPosition))
                                            {
                                                details.weightFromPosition = _weightFromPosition;
                                            }
                                            else
                                            {
                                                columnName = weightFromPositionHeaderName;
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(weightToPosition))
                                        {
                                            decimal _weightToPosition = 0;

                                            if (decimal.TryParse(weightToPosition, out _weightToPosition))
                                            {
                                                details.weightToPosition = _weightToPosition;
                                            }
                                            else
                                            {
                                                columnName = weightToPositionHeaderName;
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(weightMutiplier))
                                        {
                                            decimal _weightMutiplier = 0;

                                            if (decimal.TryParse(weightMutiplier, out _weightMutiplier))
                                            {
                                                details.weightMutiplier = _weightMutiplier;
                                            }
                                            else
                                            {
                                                columnName = weightMutiplierHeaderName;
                                                break;
                                            }
                                        }

                                        details.createdDate = DateTime.Now;
                                        details.createdBy = this.UserName;

                                        //if (details.productKey != null || !string.IsNullOrEmpty(details.barcode) || !string.IsNullOrEmpty(details.epicorPartNo))
                                        //{
                                            entityEn.lstProductCarton.Add(details);
                                        //}
                                    }

                                    rowIndex++;
                                }

                                if (!string.IsNullOrEmpty(columnName))
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Error.ToString(),
                                        message = string.Format(Resources.MSG_ERROR_UPLOAD_EMPLOYEE_PROFILE, rowIndex, columnName)
                                    });
                                }
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Warning.ToString(),
                                    message = Resources.DATATABLE_EMPTY_TABLE
                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Warning.ToString(),
                                message = Resources.MSG_INVALIDFILE
                            });
                        }

                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Warning.ToString(),
                            message = Resources.MSG_SELECT_FILE
                        });
                    }
                    
                    var result = svc.SaveProductCartonBunchList(entityEn);

                    if (hasSavedFile)
                    {
                        System.IO.File.Delete(filePath);
                    }

                    if (result != null && result.isSuccessful)
                    {
                        return Json(new
                        {
                            status = Common.Status.Success.ToString(),
                            message = Resources.MSG_SAVE,
                            refreshDatatable = true
                        });
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
                        message = Resources.MSG_ERR_SERVICE
                    });
                }

            }
            catch (Exception err)
            {
                if (hasSavedFile && !string.IsNullOrEmpty(filePath))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                return Json(new
                {
                    status = Common.Status.Error.ToString(),
                    message = Resources.MSG_ERR_SERVICE
                });
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


        /*  private void FillupFieldMetadata(ProductCartonModel model, bool isEdit)
          {
              // Implementation for field metadata population
          }
          */
        #endregion
        #region JSON Data
        public virtual JsonResult GetProductCartonValue(string barcode)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {

                        var productCartonsList = this.svc.GetProductCartonValue(barcode);
                        var _productcarton = productCartonsList.FirstOrDefault();

                        string _barcode = barcode;
                        string _labelWeight = "0.0000";
                        if (_productcarton == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        if (_productcarton.weightFromPosition.HasValue && _productcarton.weightToPosition.HasValue &&
                                          barcode.Length >= _productcarton.weightToPosition &&
                            _productcarton.weightFromPosition > 0 && _productcarton.weightToPosition >= _productcarton.weightFromPosition)
                        {
                            // Calculate the substring length based on weight positions
                            int substringLength = (int)(_productcarton.weightToPosition.Value - _productcarton.weightFromPosition.Value + 1);
                            _labelWeight = barcode.Substring((int)(_productcarton.weightFromPosition.Value - 1), substringLength);

                            // Calculate the weight value
                            _productcarton.weightValue = (Convert.ToDecimal(_labelWeight) * Convert.ToDecimal(_productcarton.weightMutiplier)).ToString();
                        }
                        return Json(new
                        {
                            success = true,
                            product = _productcarton
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