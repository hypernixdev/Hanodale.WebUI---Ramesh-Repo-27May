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
using System.Data;
using Microsoft.Ajax.Utilities;
using System.Web.Mvc;
using System.Text;
using System.ServiceModel.Security;


namespace Hanodale.WebUI.Controllers
{
    public partial class ProductWeightBarcodeController : BaseController
    {
        #region Declaration
        readonly string PAGE_URL = string.Empty;

        #endregion

        #region Constructor

        private readonly IProductWeightBarcodeService svc;
        private readonly IProductService psvc;
        private readonly IProductService svcProduct;


        public ProductWeightBarcodeController(IProductWeightBarcodeService _bLService, IProductService _svcProduct, ICommonService _svcCommon)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "ProductWeightBarcode";
            this.svc = _bLService;
            this.svcProduct = _svcProduct;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["ProductWeightBarcode_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
        }
        #endregion

        #region ProductWeightBarcode Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("This is called");
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ProductWeightBarcodeModel());

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
        public virtual JsonResult ProductWeightBarcode()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ProductWeightBarcodeModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductWeightBarcode.Views.Index, _model)
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
        public virtual ActionResult BindProductWeightBarcode(DataTableModel param)
        {
            int totalRecordCount = 0;
            List<ProductWeightBarcodes> filteredProductWeightBarcodes = null;
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
                        var ProductWeightBarcodeModel = this.svc.GetProductWeightBarcode(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            filteredProductWeightBarcodes = ProductWeightBarcodeModel.lstProductWeightBarcode;
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

                                filteredProductWeightBarcodes = filteredProductWeightBarcodes.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = ProductWeightBarcodeData(filteredProductWeightBarcodes, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = ProductWeightBarcodeModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = ProductWeightBarcodeModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> ProductWeightBarcodeData(List<ProductWeightBarcodes> ProductWeightBarcodeEntry, int currentUserId)
        {
            var result = this.GetDatatableData<ProductWeightBarcodes>(ProductWeightBarcodeEntry, currentUserId);
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductWeightBarcode.Views.RenderAction, _accessRight)
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
                    var _model = new ProductWeightBarcodeModel();

                    this.FillupFieldMetadata(_model, false);

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                    if (_model.epicorePartNo_Metadata.isEditableInCreate)
                    {
                        var _productList = svcProduct.GetProductList(new Products());

                        _model.lstProduct = _productList.Select(a => new SelectListItem
                        {
                            Text = a.partNumber,
                            Value = a.partNumber,
                        });
                    }
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductWeightBarcode.Views.Create, _model)
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
        public virtual JsonResult SaveProductWeightBarcode(ProductWeightBarcodeModel model)
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
                            ProductWeightBarcodes entity = new ProductWeightBarcodes();
                            entity.id = decrypted_Id;
                            entity.epicorePartNo = model.epicorePartNo;
                            entity.description = model.description;
                            entity.fullBarcode = model.fullBarcode;
                            entity.barcode = model.barcode;
                            entity.barcodeFromPos = model.barcodeFromPos;
                            entity.barcodeToPos = model.barcodeToPos;
                            entity.weightFromPos = model.weightFromPos;
                            entity.weightToPos = model.weightToPos;
                            entity.weightValue = model.weightValue;
                            entity.weightMultiply = model.weightMultiply;
                            entity.barcodeLength = model.barcodeLength;
                            entity.offSet1 = model.offSet1;
                            entity.offSet2 = model.offSet2;
                            entity.weightValue = model.weightValue;

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
                            bool isExists = svc.IsProductWeightBarcodeExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveProductWeightBarcode(entity);

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
        public virtual ActionResult DownloadCSVTemplate()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\Templates\WeighScaleMapping_Template.csv");

            if (!System.IO.File.Exists(filePath))
            {

            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/csv", "WeighScaleMapping_Template.csv");
        }
        private ProductWeightBarcodeModel GetProductWeightBarcodeModel(string id, bool readOnly)
        {
            try
            {
                ProductWeightBarcodeModel _model = new ProductWeightBarcodeModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var productweightbarcode = svc.GetProductWeightBarcodeById(decrypted_Id);

                if (productweightbarcode != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    //_model.epicorePartNo = productweightbarcode.epicorePartNo;
                    _model.description = productweightbarcode.description;
                    _model.fullBarcode = productweightbarcode.fullBarcode;
                    _model.barcode = productweightbarcode.barcode;
                    _model.barcode1 = productweightbarcode.barcode;
                    _model.barcodeFromPos = productweightbarcode.barcodeFromPos;
                    _model.barcodeToPos = productweightbarcode.barcodeToPos;
                    _model.weightFromPos = productweightbarcode.weightFromPos;
                    _model.weightToPos = productweightbarcode.weightToPos;
                    _model.weightMultiply = productweightbarcode.weightMultiply;
                    _model.barcodeLength = productweightbarcode.barcodeLength;
                    _model.offSet1 = productweightbarcode.offSet1;
                    _model.offSet2 = productweightbarcode.offSet2;
                    _model.weightValue = productweightbarcode.weightValue;
                    _model.weightValue1 = productweightbarcode.weightValue;


                }
                if (_model.epicorePartNo_Metadata.isEditableInCreate)
                {
                    var _productList = svcProduct.GetProductList(new Products());

                    _model.lstProduct = _productList.Select(a => new SelectListItem
                    {
                        Text = a.partNumber,
                        Value = a.partNumber,
                        Selected = (a.partNumber == productweightbarcode.epicorePartNo)
                    });
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
                        var _model = GetProductWeightBarcodeModel(id, readOnly);

                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.ProductWeightBarcode.Views.Create, _model)
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
                        var result = svc.DeleteProductWeightBarcode(decrypted_Id);

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

        //[Authorize]
        //[HttpPost]
        //public virtual JsonResult UploadProductWeightBarcodeFile()
        //{
        //    string direction = @"" + System.Configuration.ConfigurationManager.AppSettings["TempFilePath"];
        //    string filePath = string.Empty;

        //    try
        //    {
        //        var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

        //        if (_accessRight != null)
        //        {
        //            if (!_accessRight.canView)
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Denied.ToString(),
        //                    message = Resources.NO_ACCESS_RIGHTS_VIEW
        //                });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                status = Common.Status.Error.ToString(),
        //                message = Resources.MSG_ERR_SERVICE
        //            });
        //        }

        //        if (svc != null)
        //        {
        //            var currentDateTime = DateTime.Now;

        //            var entityEn = new ProductWeightBarcodeFileUpload();
        //            entityEn.organization_Id = this.CurrentUserId;


        //            var _model = new ProductWeightBarcodeModel();
        //            this.FillupFieldMetadata(_model, false);

        //            if (Request.Files[0].ContentLength > 0)
        //            {
        //                string fileExtension = System.IO.Path.GetExtension(Request.Files[0].FileName);

        //                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

        //                string query = null;
        //                string connString = "";


        //                if (validFileTypes.Contains(fileExtension))
        //                {
        //                    bool exists = System.IO.Directory.Exists(direction);
        //                    if (!exists)
        //                    {
        //                        System.IO.Directory.CreateDirectory(direction);
        //                    }

        //                    string fileName = Path.GetFileName(Request.Files[0].FileName);

        //                    filePath = direction + Request.Files[0].FileName;
        //                    if (System.IO.File.Exists(filePath))
        //                    {
        //                        System.IO.File.Delete(filePath);
        //                    }

        //                    Request.Files[0].SaveAs(filePath);

        //                    DataTable dt = new DataTable();

        //                    if (fileExtension == ".csv")
        //                    {
        //                        dt = Common.ConvertCSVtoDataTable(filePath);
        //                    }
        //                    else
        //                    {
        //                        dt = Common.ConvertXSLXtoDataTable(filePath, (fileExtension.Trim() == ".xlsx"));
        //                    }

        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        var columnName = string.Empty;
        //                        var _header = dt.Columns;

        //                        var productKeyHeaderName = "PRODUCT_KEY";
        //                        var epicorePartNoHeaderName = "item_number";
        //                        var descriptionHeaderName = "description";
        //                        var fullBarcodeHeaderName = "full_barcode";
        //                        var barcodeHeaderName = "barcode";
        //                        var barcodeFromPosHeaderName = "barcode_from_pos";
        //                        var barcodeToPosHeaderName = "barcode_to_pos";
        //                        var weightFromPosHeaderName = "weight_from_pos";
        //                        var weightToPosHeaderName = "weight_to_pos";
        //                        var weightMultiplyHeaderName = "weight_multiply";
        //                        var barcodeLengthHeaderName = "bc_length";

        //                        int rowIndex = 1;

        //                        entityEn.lstProductWeightBarcode = new List<ProductWeightBarcodes>();


        //                        foreach (DataRow row in dt.Rows)
        //                        {
        //                            if (row != null)
        //                            {
        //                                //var selectedRow = row.Split(',');

        //                                var selectedRow = row;

        //                                var details = new ProductWeightBarcodes();
        //                                details.id = rowIndex;

        //                                var epicorePartNo = selectedRow[epicorePartNoHeaderName].ToString();
        //                                var description = selectedRow[descriptionHeaderName].ToString();
        //                                var fullBarcode = selectedRow[fullBarcodeHeaderName].ToString();
        //                                var barcode = selectedRow[barcodeHeaderName].ToString();
        //                                var barcodeFromPos = selectedRow[barcodeFromPosHeaderName].ToString();
        //                                var barcodeToPos = selectedRow[barcodeToPosHeaderName].ToString();
        //                                var weightFromPos = selectedRow[weightFromPosHeaderName].ToString();
        //                                var weightToPos = selectedRow[weightToPosHeaderName].ToString();
        //                                //var weightValue = selectedRow["ACTUAL_WEIGHT_BARCODE_ID"].ToString();
        //                                var weightMultiply = selectedRow[weightMultiplyHeaderName].ToString();
        //                                var barcodeLength = selectedRow[barcodeLengthHeaderName].ToString();

        //                                details.rowIndex = rowIndex;
        //                                // details.createdDate = currentDateTime;
        //                                details.epicorePartNo = epicorePartNo;
        //                                details.description = description;
        //                                details.fullBarcode = fullBarcode;
        //                                details.barcode = barcode;

        //                                //details.offSet1 = offSet1;
        //                                //details.offSet2 = offSet2;
        //                                //details.weightValue = weightValue;


        //                                if (!string.IsNullOrEmpty(barcodeFromPos))
        //                                {
        //                                    int _barcodeFromPos = 0;

        //                                    if (int.TryParse(barcodeFromPos, out _barcodeFromPos))
        //                                    {
        //                                        details.barcodeFromPos = _barcodeFromPos;
        //                                    }
        //                                    else
        //                                    {
        //                                        columnName = barcodeFromPosHeaderName;
        //                                        break;
        //                                    }
        //                                }

        //                                if (!string.IsNullOrEmpty(barcodeToPos))
        //                                {
        //                                    int _barcodeToPos = 0;

        //                                    if (int.TryParse(barcodeToPos, out _barcodeToPos))
        //                                    {
        //                                        details.barcodeToPos = _barcodeToPos;
        //                                    }
        //                                    else
        //                                    {
        //                                        columnName = barcodeToPosHeaderName;
        //                                        break;
        //                                    }
        //                                }
        //                                if (!string.IsNullOrEmpty(weightFromPos))
        //                                {
        //                                    int _weightFromPos = 0;

        //                                    if (int.TryParse(weightFromPos, out _weightFromPos))
        //                                    {
        //                                        details.weightFromPos = _weightFromPos;
        //                                    }
        //                                    else
        //                                    {
        //                                        columnName = weightFromPosHeaderName;
        //                                        break;
        //                                    }
        //                                }

        //                                if (!string.IsNullOrEmpty(weightToPos))
        //                                {
        //                                    int _weightToPos = 0;

        //                                    if (int.TryParse(weightToPos, out _weightToPos))
        //                                    {
        //                                        details.weightToPos = _weightToPos;
        //                                    }
        //                                    else
        //                                    {
        //                                        columnName = weightToPosHeaderName;
        //                                        break;
        //                                    }
        //                                }

        //                                if (!string.IsNullOrEmpty(weightMultiply))
        //                                {
        //                                    decimal _weightMultiply = 0;

        //                                    if (decimal.TryParse(weightMultiply, out _weightMultiply))
        //                                    {
        //                                        details.weightMultiply = _weightMultiply;
        //                                    }
        //                                    else
        //                                    {
        //                                        columnName = weightMultiplyHeaderName;
        //                                        break;
        //                                    }
        //                                }

        //                                if (!string.IsNullOrEmpty(barcodeLength))
        //                                {
        //                                    int _barcodeLength = 0;

        //                                    if (int.TryParse(barcodeLength, out _barcodeLength))
        //                                    {
        //                                        details.barcodeLength = _barcodeLength;
        //                                    }
        //                                    else
        //                                    {
        //                                        columnName = barcodeLengthHeaderName;
        //                                        break;
        //                                    }
        //                                }
        //                                details.createdDate = DateTime.Now;
        //                                details.createdBy = this.UserName;

        //                                entityEn.lstProductWeightBarcode.Add(details);
        //                            }

        //                            rowIndex++;
        //                        }

        //                        if (!string.IsNullOrEmpty(columnName))
        //                        {
        //                            return Json(new
        //                            {
        //                                status = Common.Status.Error.ToString(),
        //                                message = string.Format(Resources.MSG_ERROR_UPLOAD_EMPLOYEE_PROFILE, rowIndex, columnName)
        //                            });
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return Json(new
        //                        {
        //                            status = Common.Status.Warning.ToString(),
        //                            message = Resources.DATATABLE_EMPTY_TABLE
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    return Json(new
        //                    {
        //                        status = Common.Status.Warning.ToString(),
        //                        message = Resources.MSG_INVALIDFILE
        //                    });
        //                }

        //            }
        //            else
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Warning.ToString(),
        //                    message = Resources.MSG_SELECT_FILE
        //                });
        //            }

        //            var result = svc.SaveProductWeightBarcodeBunchList(entityEn);

        //            System.IO.File.Delete(filePath);

        //            if (result != null && result.isSuccessful)
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Success.ToString(),
        //                    message = Resources.MSG_SAVE,
        //                });
        //            }
        //            else
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Error.ToString(),
        //                    message = Resources.MSG_ERR_SERVICE
        //                });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                status = Common.Status.Error.ToString(),
        //                message = Resources.MSG_ERR_SERVICE
        //            });
        //        }

        //    }
        //    catch (Exception err)
        //    {
        //        if (!string.IsNullOrEmpty(filePath))
        //        {
        //            if (System.IO.File.Exists(filePath))
        //            {
        //                System.IO.File.Delete(filePath);
        //            }
        //        }

        //        return Json(new
        //        {
        //            status = Common.Status.Error.ToString(),
        //            message = Resources.MSG_ERR_SERVICE
        //        });
        //    }

        //}

        [Authorize]
        [HttpPost]
        public virtual JsonResult UploadProductWeightBarcodeFile()
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

                    var entityEn = new ProductWeightBarcodeFileUpload();
                    entityEn.organization_Id = this.CurrentUserId;


                    var _model = new ProductWeightBarcodeModel();
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
                                var epicorePartNoHeaderName = "item_number";
                                var descriptionHeaderName = "description";
                                var fullBarcodeHeaderName = "full_barcode";
                                var barcodeHeaderName = "barcode";
                                var barcodeFromPosHeaderName = "barcode_from_pos";
                                var barcodeToPosHeaderName = "barcode_to_pos";
                                var weightFromPosHeaderName = "weight_from_pos";
                                var weightToPosHeaderName = "weight_to_pos";
                                var weightMultiplyHeaderName = "weight_multiply";
                                var barcodeLengthHeaderName = "bc_length";

                                int rowIndex = 1;

                                entityEn.lstProductWeightBarcode = new List<ProductWeightBarcodes>();


                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row != null)
                                    {
                                        //var selectedRow = row.Split(',');

                                        var selectedRow = row;

                                        var details = new ProductWeightBarcodes();
                                        details.id = rowIndex;

                                        var epicorePartNo = selectedRow[epicorePartNoHeaderName].ToString().Trim();
                                        var description = selectedRow[descriptionHeaderName].ToString().Trim();
                                        var fullBarcode = selectedRow[fullBarcodeHeaderName].ToString().Trim();
                                        var barcode = selectedRow[barcodeHeaderName].ToString().Trim();
                                        var barcodeFromPos = selectedRow[barcodeFromPosHeaderName].ToString();
                                        var barcodeToPos = selectedRow[barcodeToPosHeaderName].ToString();
                                        var weightFromPos = selectedRow[weightFromPosHeaderName].ToString();
                                        var weightToPos = selectedRow[weightToPosHeaderName].ToString();
                                        //var weightValue = selectedRow["ACTUAL_WEIGHT_BARCODE_ID"].ToString();
                                        var weightMultiply = selectedRow[weightMultiplyHeaderName].ToString();
                                        var barcodeLength = selectedRow[barcodeLengthHeaderName].ToString();

                                        details.rowIndex = rowIndex;
                                        // details.createdDate = currentDateTime;
                                        details.epicorePartNo = epicorePartNo;
                                        details.description = description;
                                        details.fullBarcode = fullBarcode;
                                        details.barcode = barcode;

                                        //details.offSet1 = offSet1;
                                        //details.offSet2 = offSet2;
                                        //details.weightValue = weightValue;


                                        if (!string.IsNullOrEmpty(barcodeFromPos))
                                        {
                                            int _barcodeFromPos = 0;

                                            if (int.TryParse(barcodeFromPos, out _barcodeFromPos))
                                            {
                                                details.barcodeFromPos = _barcodeFromPos;
                                            }
                                            else
                                            {
                                                columnName = barcodeFromPosHeaderName;
                                                break;
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(barcodeToPos))
                                        {
                                            int _barcodeToPos = 0;

                                            if (int.TryParse(barcodeToPos, out _barcodeToPos))
                                            {
                                                details.barcodeToPos = _barcodeToPos;
                                            }
                                            else
                                            {
                                                columnName = barcodeToPosHeaderName;
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(weightFromPos))
                                        {
                                            int _weightFromPos = 0;

                                            if (int.TryParse(weightFromPos, out _weightFromPos))
                                            {
                                                details.weightFromPos = _weightFromPos;
                                            }
                                            else
                                            {
                                                columnName = weightFromPosHeaderName;
                                                break;
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(weightToPos))
                                        {
                                            int _weightToPos = 0;

                                            if (int.TryParse(weightToPos, out _weightToPos))
                                            {
                                                details.weightToPos = _weightToPos;
                                            }
                                            else
                                            {
                                                columnName = weightToPosHeaderName;
                                                break;
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(weightMultiply))
                                        {
                                            decimal _weightMultiply = 0;

                                            if (decimal.TryParse(weightMultiply, out _weightMultiply))
                                            {
                                                details.weightMultiply = _weightMultiply;
                                            }
                                            else
                                            {
                                                columnName = weightMultiplyHeaderName;
                                                break;
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(barcodeLength))
                                        {
                                            int _barcodeLength = 0;

                                            if (int.TryParse(barcodeLength, out _barcodeLength))
                                            {
                                                details.barcodeLength = _barcodeLength;
                                            }
                                            else
                                            {
                                                columnName = barcodeLengthHeaderName;
                                                break;
                                            }
                                        }
                                        details.createdDate = DateTime.Now;
                                        details.createdBy = this.UserName;

                                        entityEn.lstProductWeightBarcode.Add(details);
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

                    var result = svc.SaveProductWeightBarcodeBunchList(entityEn);

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
        #region JSON Data
        public virtual JsonResult GetProductWeightBarcodeValue(string epicorePartNo)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {

                        var productweightbarcodesList = this.svc.GetProductWeightBarcodeValue(epicorePartNo);
                        var productweightbarcode = productweightbarcodesList.FirstOrDefault();

                        /*string _barcode = barcode;
                        string _labelWeight = "0.0000";
                        if (productweightbarcode == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        if (productweightbarcode.weightFromPosition.HasValue && productweightbarcode.weightToPosition.HasValue &&
                                          barcode.Length >= productweightbarcode.weightToPosition &&
                            productweightbarcode.weightFromPosition > 0 && productweightbarcode.weightToPosition >= productweightbarcode.weightFromPosition)
                        {
                            // Calculate the substring length based on weight positions
                            int substringLength = (int)(productweightbarcode.weightToPosition.Value - productweightbarcode.weightFromPosition.Value + 1);
                            _labelWeight = barcode.Substring((int)(productweightbarcode.weightFromPosition.Value - 1), substringLength);

                            // Calculate the weight value
                            productweightbarcode.weightValue = (Convert.ToDecimal(_labelWeight) * Convert.ToDecimal(productweightbarcode.weightMutiplier)).ToString();
                        }*/
                        return Json(new
                        {
                            success = true,
                            product = productweightbarcode
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


        #endregion
    }
}