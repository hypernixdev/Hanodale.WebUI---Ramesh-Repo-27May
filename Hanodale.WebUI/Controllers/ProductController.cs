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
using System.Threading.Tasks;
using Hanodale.Entity.Core;
using Hanodale.Domain.DTOs.Order;
using Hanodale.SyncService.Models;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Media3D;


namespace Hanodale.WebUI.Controllers
{
    public partial class ProductController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string ORDERS_PAGE_URL = string.Empty;

        #endregion

        #region Constructor
        private readonly IProductService svc;
        private readonly ISyncManager syncManager;
        private readonly ICustomerService customerService;
        private readonly IOrderService orderService;
        public ProductController(IProductService _bLService, ICommonService _svcCommon, ISyncManager syncManager, ICustomerService customerService, IOrderService orderService)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "Product";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Product_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            ORDERS_PAGE_URL = "Orders/Index";
            this.syncManager = syncManager;
            this.customerService = customerService;
            this.orderService = orderService;
        }
        #endregion

        #region Product Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ProductModel());

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
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }

        }

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult Product()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new ProductModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Product.Views.Index, _model)
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
        public virtual ActionResult BindProduct(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            List<Products> filteredProducts = null;
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
                        var filterEntity = new Products();
                        var idFilter0 = Convert.ToString(Request["sSearch_0"]).Trim();
                        var idFilter1 = Convert.ToString(Request["sSearch_1"]).Trim();
                        var idFilter2 = Convert.ToString(Request["sSearch_2"]).Trim();
                        var idFilter3 = Convert.ToString(Request["sSearch_3"]).Trim();

                        if (!string.IsNullOrEmpty(idFilter0))
                        {
                            filterEntity.searchDescription = Convert.ToString(idFilter0);
                        }
                        if (!string.IsNullOrEmpty(idFilter1))
                        {
                            filterEntity.searchGrupDescription = Convert.ToString(idFilter1);
                        }
                        if (!string.IsNullOrEmpty(idFilter2))
                        {
                            filterEntity.searchPartClassDescription = Convert.ToString(idFilter2);
                        }
                        if (!string.IsNullOrEmpty(idFilter3))
                        {
                            filterEntity.searchCountryDescription = Convert.ToString(idFilter3);
                        }


                        var filter = new DatatableFilters { currentUserId = this.CurrentUserId, all = true, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };
                        var Product = this.svc.GetProduct(filter, filterEntity);
                        System.Diagnostics.Debug.WriteLine(Product);
                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            //Sorting

                            filteredProducts = Product.lstProduct;
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

                                filteredProducts = filteredProducts.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = ProductData(filteredProducts, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = Product.recordDetails.totalRecords;
                            var iTotalDisplayRecords = Product.recordDetails.totalDisplayRecords;

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

        public List<string[]> ProductData(List<Products> productEntry, int currentUserId)
        {
            var result = this.GetDatatableData<Products>(productEntry, currentUserId);
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.Product.Views.RenderAction, _accessRight)
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
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (!_accessRight.canAdd)
                {
                    return this.Msg_AccessDeniedInAdd();
                }

                if (svc != null)
                {
                    var _model = new ProductModel();

                    this.FillupFieldMetadata(_model, false);



                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.Product.Views.Create, _model)
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
        public virtual ActionResult AddProduct(string parCode, string description, string prodGroup, int id, string partNum)
        {
            try
            {


                if (svc != null)
                {
                    var _model = new ViewOrderItemModel();
                    var _product = this.svc.GetProductByPartNum(partNum);
                    _model.code = parCode; // Assuming PartCode is a property in ProductModel
                    _model.description = description; // Assuming Description is a property in ProductModel
                    _model.prodGroup = prodGroup;
                    _model.product_Id = id;
                    _model.partNum = partNum;
                    _model.unitPrice = _product.unitPrice;

                    this.FillupFieldMetadata(_model, false);


                    var viewMarkup = Common.RenderPartialViewToString(this, "AddProduct", _model);

                    return Json(new
                    {
                        viewMarkup = viewMarkup
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
        public virtual ActionResult EditProduct(string parCode, string description, string prodGroup, int id, string partNum, int QtyType_ModuleItem_Id, int orderQty,
            int OrderUOM_Id, int operationStyle_ModuleItem_Id, float operationCost, int complimentary_ModuleItem_Id, float unitPrice)
        {
            try
            {


                if (svc != null)
                {
                    var _model = new ViewOrderItemModel();
                    _model.code = parCode; // Assuming PartCode is a property in ProductModel
                    _model.description = description; // Assuming Description is a property in ProductModel
                    _model.prodGroup = prodGroup;
                    _model.product_Id = id;
                    _model.partNum = partNum;
                    _model.orderQty = orderQty;
                    _model.QtyType_ModuleItem_Id = QtyType_ModuleItem_Id;
                    _model.OrderUOM_Id = OrderUOM_Id;
                    _model.operationStyle_ModuleItem_Id = operationStyle_ModuleItem_Id;
                    _model.operationCost = (decimal)operationCost;
                    _model.complimentary_ModuleItem_Id = complimentary_ModuleItem_Id;
                    _model.unitPrice = (decimal)unitPrice;

                    this.FillupFieldMetadata(_model, false);


                    var viewMarkup = Common.RenderPartialViewToString(this, "AddProduct", _model);

                    return Json(new
                    {
                        viewMarkup = viewMarkup
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
        public virtual JsonResult SaveProduct(ProductModel model)
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
                                return this.Msg_AccessDeniedInEdit();
                            }
                        }

                        if (svc != null)
                        {
                            Products entity = new Products();
                            entity.id = decrypted_Id;
                            entity.partNumber = model.partNumber;
                            entity.description = model.description;
                            entity.code = model.code;

                            bool isExists = svc.IsProductExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveProduct(entity);

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

        private ProductModel GetProductModel(string id, bool readOnly)
        {
            try
            {
                ProductModel _model = new ProductModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var product = svc.GetProductById(decrypted_Id);

                if (product != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    _model.partNumber = product.partNumber;
                    _model.description = product.description;
                    _model.code = product.code;
                    _model.prodGrup_Description = product.prodGrup_Description;
                    _model.part_ClassID = product.part_ClassID;
                    _model.partClass_Description = product.partClass_Description;
                    _model.country_Description = product.country_Description;
                    _model.uomClass_DefUomCode = product.uomClass_DefUomCode;
                    _model.part_SalesUM = product.part_SalesUM;
                    _model.part_IUM = product.part_IUM;
                    _model.uomClass_BaseUOMCode = product.uomClass_BaseUOMCode;
                    _model.country_CountryNum = product.country_CountryNum;
                    _model.uomClass_ClassType = product.uomClass_ClassType;
                    _model.uomClass_Description = product.uomClass_Description;
                    _model.unitPrice = product.unitPrice;
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
            AccessRightsModel _accessRight = new AccessRightsModel();
            var _model = new ProductMaintenanceModel();

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
                        _model.Product = GetProductModel(id, readOnly);
                        if (_model.Product == null)
                        {
                            return this.Msg_ErrorInService();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Product.Views.Maintenance, _model)
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

        [AppAuthorize]
        public virtual ActionResult GetCustomSearchPanel()
        {
            var _model = new ProductModel();

            var _ProductList = svc.GetProductList(new Products());

            _model.lstProductCode = _ProductList.Select(a => new SelectListItem
            {
                Text = a.code,
                Value = a.code.ToString(),
                // Selected = (a.id == _model.Product_Id_Metadata.dropdownDefaultValue)
            });
            _model.lstProductPartNo = _ProductList.Select(a => new SelectListItem
            {
                Text = a.partNumber,
                Value = a.partNumber.ToString(),
                // Selected = (a.id == _model.Product_Id_Metadata.dropdownDefaultValue)
            });

            return PartialView(MVC.Product.Views._SearchPanel, _model);
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
                    int decrypted_Id = 0;
                    decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            var result = svc.DeleteProduct(decrypted_Id);

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

        #region product search
        [HttpPost]
        [Authorize]
        public virtual JsonResult GetProductByPartNum(string partNum, string shipToId, string customerId, string orderDate)
        {
            try
            {
                var serialNumber = partNum;
                string serialNumberLocation = string.Empty;
                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canAdd || _accessRight.canEdit)
                    {
                        var partNumber = partNum;
                        var weightValue = "1";
                        //string _labelWeight1 = "0.0000";
                        var isSerialNumberFound = false;
                        // 7704 : Full Quantity
                        // 7705 : Loose Quantity
                        int orderType = 7705; // Default to Loose Quantity for now
                        string barcodeType = "Loose";
                        try
                        {
                            // Check Loose Barcodes available or not first
                            var productCartonLoose = this.orderService.GetProductCartons(partNum, barcodeType);
                            if (productCartonLoose != null)
                            {

                                partNumber = productCartonLoose.epicorePartNo;
                                isSerialNumberFound = true;
                                serialNumberLocation = productCartonLoose.productLocation;
                                if (productCartonLoose.IsCarton)
                                    weightValue = productCartonLoose.IumQty.ToString("0.00");
                                else
                                {
                                    if(productCartonLoose.IsVaryWg)
                                        weightValue = productCartonLoose.LooseQty.ToString("0.00");
                                    else
                                        weightValue = "1"; // Default to 1 for Std Loose Qty
                                }
                                 
                            }
                            else
                            {
                                // Check Full Barcodes available or not
                                // Just Validate the Barcode right or not , if right provide the partnum
                                barcodeType = "Carton";
                                var productCarton = this.orderService.GetProductCartons(partNum, barcodeType);
                                if (productCarton != null)
                                {
                                    partNumber = productCarton.epicorePartNo;
                                    
                                    if (productCarton.IsVaryWg)
                                    {
                                        //CVary Weight Carton Barcode value or wieght have to provide
                                        isSerialNumberFound = true;
                                        serialNumberLocation = productCarton.productLocation;                                        
                                        weightValue = productCarton.IumQty.ToString("0.00");
                                        
                                    }
                                    else
                                    {
                                        // Std Carton Barcode value set to 1
                                            weightValue = "1"; // Std Carton
                                    }
                                        
                                    
                                    
                                }
                                else
                                {
                                    // Check Serial Barcodes available or not
                                    // For Valiadtion
                                    barcodeType = "CartonAndLoose";
                                    var productCartonAndLooseBarcode = this.orderService.GetProductCartons(partNum, barcodeType);
                                    if(productCartonAndLooseBarcode != null && productCartonAndLooseBarcode.IsPickedComplete)
                                    {
                                        //throw new Exception("This product is already picked complete, you cannot add it again.");
                                        return Json(new
                                        {
                                            success = false,
                                            message = "This product is already picked complete, you cannot add it again."
                                        });
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // 
                        }

                        
                        var _product = this.svc.GetProductByPartNum(partNumber);                        
                        _product.productStockBalances = this.svc.GetProductStockBalanceList(new List<string> { partNumber });

                        if (_product == null)
                        {
                            //return this.Msg_ErrorInRetriveData();
                            return Json(new
                            {
                                success = false,
                                message = $"Can't find {partNum} Product Or Barcode!"
                            });
                        }
                        var customer = new Customers();
                        try
                        {
                            customer = this.customerService.GetCustomerById(int.Parse(customerId));
                        }
                        catch (Exception ex)
                        {
                            // 
                        }
                        try
                        {
                            var PriceLookupMode = WebConfigurationManager.AppSettings["ProductPriceLookupMode"];
                            if (PriceLookupMode == "offline")
                            {
                                var productPriceList = this.svc.GetCustomerPricing(customer != null ? customer.custID : "", new List<string> { _product.partNumber }, orderDate);
                                var productPrice = productPriceList.FirstOrDefault();
                                var uomConversions = this.svc.GetUomConvList(new List<string> { _product.partNumber });

                                if (productPrice != null)
                                {
                                    // Attach the price to the product
                                    _product.unitPrice = productPrice.CustGrpBasePrice ?? 0;

                                    var priceByUom = new Dictionary<string, decimal>();
                                    // Apply UOM conversions if available
                                    if (uomConversions != null)
                                    {
                                        var priceListConv = uomConversions.Where(UomConv => UomConv.UomCode == productPrice.UomCode).FirstOrDefault();
                                        foreach (var detail in uomConversions)
                                        {
                                            // Start with the base price
                                            decimal calculatedPrice = productPrice.CustGrpBasePrice ?? 0;
                                            
                                            if (detail.UomCode != productPrice.UomCode)
                                            {
                                                // Apply conversion based on the operator
                                                
                                                calculatedPrice = (calculatedPrice / Decimal.Parse(priceListConv.ConvFactor)) * Decimal.Parse(detail.ConvFactor)  ;
                                                
                                            }
                                            // Add the calculated price to the dictionary with the key as uomCode + "price"
                                            priceByUom[detail.UomCode + "_Price"] = calculatedPrice;
                                        }
                                    }
                                    _product.UomPrices = priceByUom;
                                }

                            }
                            else
                            {
                                var productPrice = this.syncManager.GetProductPriceAsync(_product.partNumber, shipToId, customer != null ? customer.code : "", orderDate); // Assuming product has ShipToNum
                                // System.Diagnostics.Debug.WriteLine(productPrice);
                                List<UomConvApiResponseModel> uomConversions = new List<UomConvApiResponseModel>();
                                UomConvApiResponseModel uomConversion = new UomConvApiResponseModel();
                                try
                                {
                                    List<string> partNumbers = new List<string>();
                                    partNumbers.Add(_product.partNumber);
                                    uomConversions = this.syncManager.GetUomConversions(partNumbers);
                                    uomConversion = uomConversions[0];
                                }
                                catch (Exception ex)
                                {
                                    // Handle exception
                                }
                                if (productPrice != null && !productPrice.is404)
                                {
                                    // Attach the price to the product
                                    _product.unitPrice = productPrice.BasePrice;

                                    var priceByUom = new Dictionary<string, decimal>();
                                    // Apply UOM conversions if available
                                     if (uomConversion != null && uomConversion.uomConvDetails != null)
                                    {
                                        foreach (var detail in uomConversion.uomConvDetails)
                                        {
                                            // Start with the base price
                                            decimal calculatedPrice = productPrice.BasePrice;

                                            if (detail.uomCode != productPrice.uomCode)
                                            {
                                                // Apply conversion based on the operator
                                                if (detail.convOperator == "*")
                                                {
                                                    calculatedPrice *= detail.convFactor;
                                                }
                                                else if (detail.convOperator == "/")
                                                {
                                                    calculatedPrice /= detail.convFactor;
                                                }
                                            }
                                            // Add the calculated price to the dictionary with the key as uomCode + "price"
                                            priceByUom[detail.uomCode + "_Price"] = calculatedPrice;
                                        }
                                    }
                                    _product.UomPrices = priceByUom;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);
                        }
                        _product.weightValue = weightValue;
                        if (isSerialNumberFound)
                        {
                            _product.serialNumber = serialNumber +"|"+ serialNumberLocation;
                        }
                        return Json(new
                        {
                            success = true,
                            product = _product
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
        public virtual JsonResult GetProductStockBalance(string partNum, string uom)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        
                        var _product = this.svc.GetProductStockBalance(partNum, uom);

                        if (_product == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        
                        return Json(new
                        {
                            success = true,
                            product = _product
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
        public virtual JsonResult SearchProducts(ProductDatatableFilter param)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canAdd || _accessRight.canEdit)
                    {
                        var _result = this.svc.GetProductList(param);
                        var _products = _result.lstProduct;
                        if (_products == null || !_products.Any())
                        {
                            // return this.Msg_ErrorInRetriveData();
                            var response2 = new
                            {
                                draw = param.Draw,
                                recordsTotal = 0,
                                recordsFiltered = 0,
                                data = new List<Product>()
                            };
                            return Json(response2);
                        }
                        // Iterate through the products and attach prices
                        foreach (var product in _products)
                        {
                            bool chkVaryWChecked = false;

                            if (product.standardFullQty == "Yes" && product.allowVaryWeight == "Yes")
                            {
                                chkVaryWChecked = false;
                            }
                            else if (product.standardFullQty == "Yes" && product.allowVaryWeight == "No")
                            {
                                // Set chkStandardFullQty to true (not shown in your context) and disable, chkVaryW to false
                                chkVaryWChecked = false;
                            }
                            else
                            {
                                // Set chkStandardFullQty to false and chkVaryW based on allowSellingVaryWeight
                                if (product.allowVaryWeight == "Yes")
                                {
                                    chkVaryWChecked = true;
                                }
                                else
                                {
                                    chkVaryWChecked = false;
                                }
                            }
                            if (!chkVaryWChecked)
                            {
                                product.UomBasedOnCondition = product.part_SalesUM;
                                if (product.AllowLooseSelling == true)
                                {
                                    product.UomBasedOnCondition = product.looseUom != "" ? product.looseUom : product.uomClass_BaseUOMCode;
                                }
                            }
                            else
                            {
                                product.UomBasedOnCondition = product.part_SalesUM;
                            }
                        }

                        _products = ApplyPricingDetails(_products, param.customerId, param.orderDate, param.shipToId);

                        var response = new
                        {
                            draw = param.Draw,
                            recordsTotal = _result.recordDetails.totalRecords,
                            recordsFiltered = _result.recordDetails.totalDisplayRecords,
                            data = _products
                        };
                        return Json(response);
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
                // return Msg_ErrorInRetriveData(ex);
                var response = new
                {
                    draw = param.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<Product>()
                };
                return Json(response);
            }
        }

        private List<Products> ApplyPricingDetails(List<Products> products, string customerId, string orderDate, string shipToId)
        {
            var PriceLookupMode = WebConfigurationManager.AppSettings["ProductPriceLookupMode"];
            List<string> partNumbers = products.Select(p => p.partNumber).ToList();
            List<ProductStockBalance> productStockBalances = this.svc.GetProductStockBalanceList(partNumbers);

            var customer = new Customers();
            try
            {
                customer = this.customerService.GetCustomerById(int.Parse(customerId));
            }
            catch (Exception ex)
            {
                // 
            }
            if (PriceLookupMode == "offline")
            {
                List<UomConvs> uomConversions = new List<UomConvs>();
                try
                {
                    uomConversions = this.svc.GetUomConvList(partNumbers);
                }
                catch (Exception ex)
                {
                    // Handle exception
                }
                var priceList = this.svc.GetCustomerPricing(customer != null ? customer.custID : "", products.Select(p => p.partNumber).ToList(), orderDate);
                System.Diagnostics.Debug.WriteLine(priceList);

                foreach (var product in products)
                {
                    // Call GetProductPriceAsync for each product using partNum
                    try
                    {
                        var productPrice = priceList.FirstOrDefault(p => p.PartNum == product.partNumber);
                        var baseUom = product.UomBasedOnCondition;
                        baseUom = baseUom.ToUpper();
                        if (productPrice != null)
                        {
                            
                            var priceByUom = new Dictionary<string, decimal>();
                            // Apply UOM conversions if available
                            if (uomConversions != null)
                            {
                                var priceListConv = uomConversions.Where(UomConv => UomConv.UomCode == productPrice.UomCode).FirstOrDefault();
                                foreach (var detail in uomConversions)
                                {
                                    // Start with the base price
                                    decimal calculatedPrice = productPrice.CustGrpBasePrice ?? 0;

                                    if (detail.UomCode != productPrice.UomCode)
                                    {
                                        // Apply conversion based on the operator

                                        calculatedPrice = (calculatedPrice / Decimal.Parse(priceListConv.ConvFactor)) * Decimal.Parse(detail.ConvFactor);

                                    }

                                    // Add the calculated price to the dictionary with the key as uomCode + "price"
                                    priceByUom[detail.UomCode + "_Price"] = calculatedPrice;
                                }
                            }

                            product.unitPrice = priceByUom[product.UomBasedOnCondition + "_Price"];

                            product.UomPrices = priceByUom;                            
                        }
                        product.productStockBalances = productStockBalances.Where(p => p.partNumber == product.partNumber).ToList();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        break;
                    }
                }

            } 
            else
            {
                List<UomConvApiResponseModel> uomConversions = new List<UomConvApiResponseModel>();
                try
                {
                    uomConversions = this.syncManager.GetUomConversions(partNumbers);
                }
                catch (Exception ex)
                {
                    // Handle exception
                }

                foreach (var product in products)
                {
                    // Call GetProductPriceAsync for each product using partNum
                    try
                    {
                        var productPrice = this.syncManager.GetProductPriceAsync(product.partNumber, shipToId, customer != null ? customer.code : "", orderDate); // Assuming product has ShipToNum
                        var baseUom = product.UomBasedOnCondition;
                        baseUom = baseUom.ToUpper();
                        // System.Diagnostics.Debug.WriteLine(productPrice);
                        if (productPrice != null && productPrice.is404 != true)
                        {
                            var uomConversion = uomConversions.FirstOrDefault(u => u.partNum == product.partNumber);
                            var relevantUomConversion = uomConversion?.uomConvDetails.FirstOrDefault(d => d.uomCode == product.UomBasedOnCondition);

                            // Attach the price to the product
                            product.unitPrice = productPrice.BasePrice;

                            // Apply UOM conversion if available
                            if (relevantUomConversion != null)
                            {
                                if (relevantUomConversion.uomCode != productPrice.uomCode)
                                {
                                    if (relevantUomConversion.convOperator == "*")
                                    {
                                        product.unitPrice *= relevantUomConversion.convFactor;
                                    }
                                    else if (relevantUomConversion.convOperator == "/")
                                    {
                                        product.unitPrice /= relevantUomConversion.convFactor;
                                    }
                                }
                            }

                            var priceByUom = new Dictionary<string, decimal>();
                            // Apply UOM conversions if available
                            if (uomConversion != null && uomConversion.uomConvDetails != null)
                            {
                                foreach (var detail in uomConversion.uomConvDetails)
                                {
                                    // Start with the base price
                                    decimal calculatedPrice = productPrice.BasePrice;

                                    // Apply conversion based on the operator
                                    if (detail.convOperator == "*")
                                    {
                                        calculatedPrice *= detail.convFactor;
                                    }
                                    else if (detail.convOperator == "/")
                                    {
                                        calculatedPrice /= detail.convFactor;
                                    }
                                    if (baseUom == detail.uomCode)
                                    {
                                        priceByUom[detail.uomCode + "_Price"] = productPrice.BasePrice;
                                    }
                                    else
                                    {
                                        // Add the calculated price to the dictionary with the key as uomCode + "price"
                                        priceByUom[detail.uomCode + "_Price"] = calculatedPrice;
                                    }
                                }
                            }
                            product.UomPrices = priceByUom;
                            product.productStockBalances = productStockBalances.Where(p => p.partNumber == product.partNumber).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        break;
                    }
                }
            }

            return products;
        }

        #endregion

        #region unit conversion live api
        [HttpPost]
        [Authorize]
        public virtual JsonResult GetProductPrices(ProductPriceRequest request)
        {
            try
            {

                var results = GetProductPriceList(request);                

                return Json(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        /*
         * Price and UOM logics based on offline or online.
         * 
         */
        private List<ProductPriceResult> GetProductPriceList(ProductPriceRequest request)
        {
            var results = new List<ProductPriceResult>();
            var customer = new Customers();
            try
            {
                customer = this.customerService.GetCustomerById(int.Parse(request.custNum));
            }
            catch (Exception ex)
            {
                // 
            }
            var PriceLookupMode = WebConfigurationManager.AppSettings["ProductPriceLookupMode"];
            if (PriceLookupMode == "offline")
            {
                List<UomConvs> uomConversions = new List<UomConvs>();
                try
                {
                    List<string> partNumbers = request.products.Select(p => p.partNumber).ToList();
                    uomConversions = this.svc.GetUomConvList(partNumbers);
                }
                catch (Exception ex)
                {
                    // Handle exception
                }
                var priceList = this.svc.GetCustomerPricing(customer != null ? customer.custID : "", request.products.Select(p => p.partNumber).ToList(), request.orderDate);

                foreach (var product in request.products)
                {
                    try
                    {
                        var productPrice = priceList.FirstOrDefault(p => p.PartNum == product.partNumber);

                        if (productPrice != null)
                        {
                            var relevantUomConversion = uomConversions.FirstOrDefault(u => u.PartNum == product.partNumber && u.UomCode == product.uom);

                            decimal finalPrice = productPrice.CustGrpBasePrice ?? 0;

                            // Apply UOM conversion if available
                            if (relevantUomConversion != null)
                            {
                                if (relevantUomConversion.ConvOperator == "*")
                                {
                                    finalPrice *= Decimal.Parse(relevantUomConversion.ConvFactor);
                                }
                                else if (relevantUomConversion.ConvOperator == "/")
                                {
                                    finalPrice /= Decimal.Parse(relevantUomConversion.ConvFactor);
                                }
                            }

                            results.Add(new ProductPriceResult
                            {
                                partNumber = product.partNumber,
                                uom = product.uom,
                                price = finalPrice,
                                index = product.index
                            });
                        }
                        else
                        {
                            results.Add(new ProductPriceResult
                            {
                                partNumber = product.partNumber,
                                uom = product.uom,
                                price = null,
                                errorMessage = "Price not found",
                                index = product.index
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error processing product {product.partNumber}: {ex.Message}");
                        results.Add(new ProductPriceResult
                        {
                            partNumber = product.partNumber,
                            uom = product.uom,
                            price = null,
                            errorMessage = "Error processing product"
                        });
                    }
                }
            }
            else
            {
                List<UomConvApiResponseModel> uomConversions = new List<UomConvApiResponseModel>();
                try
                {
                    List<string> partNumbers = request.products.Select(p => p.partNumber).ToList();
                    uomConversions = this.syncManager.GetUomConversions(partNumbers);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    System.Diagnostics.Debug.WriteLine($"Error fetching UOM conversions: {ex.Message}");
                }

                foreach (var product in request.products)
                {
                    try
                    {
                        var productPrice = this.syncManager.GetProductPriceAsync(product.partNumber, request.shipToId, customer?.code ?? "", request.orderDate);

                        if (productPrice != null && productPrice.is404 != true)
                        {
                            var uomConversion = uomConversions.FirstOrDefault(u => u.partNum == product.partNumber);
                            var relevantUomConversion = uomConversion?.uomConvDetails.FirstOrDefault(d => d.uomCode == product.uom);

                            decimal finalPrice = productPrice.BasePrice;

                            // Apply UOM conversion if available
                            if (relevantUomConversion != null)
                            {
                                if (relevantUomConversion.convOperator == "*")
                                {
                                    finalPrice *= relevantUomConversion.convFactor;
                                }
                                else if (relevantUomConversion.convOperator == "/")
                                {
                                    finalPrice /= relevantUomConversion.convFactor;
                                }
                            }

                            results.Add(new ProductPriceResult
                            {
                                partNumber = product.partNumber,
                                uom = product.uom,
                                price = finalPrice,
                                index = product.index
                            });
                        }
                        else
                        {
                            results.Add(new ProductPriceResult
                            {
                                partNumber = product.partNumber,
                                uom = product.uom,
                                price = null,
                                errorMessage = "Price not found",
                                index = product.index
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error processing product {product.partNumber}: {ex.Message}");
                        results.Add(new ProductPriceResult
                        {
                            partNumber = product.partNumber,
                            uom = product.uom,
                            price = null,
                            errorMessage = "Error processing product"
                        });
                    }
                }
            }

            return results;
        }

        #endregion
        #region Sync service 
        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncProducts()
        {
            try
            {
                var success = this.syncManager.syncEntity("Product", "", "partNumber", false, "");
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

        public virtual ActionResult syncUOMConv()
        {
            try
            {
                var success = this.syncManager.syncEntity("UomConv", "UomConv", "uniqueField", true, "UomConv");
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

        public virtual ActionResult SyncStockBalances()
        {
            try
            {
                var success = this.syncManager.syncEntity("StockBalance", "stockbalance", "uniqueField", false, "");
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

        public virtual ActionResult SyncBrands()
        {
            try
            {
                var success = this.syncManager.syncEntity("Brand", "brands", "sysRowID", true, "");
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
        #region New order Screen changes
        [HttpPost]
        [Authorize]
        public virtual JsonResult SearchOrderType()
        {
            try
            {
                var moduleType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["OrderType_moduleType_Id"]);
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var _orderType = this.svc.GetOrderTypeList(moduleType_Id);
                        if (_orderType == null || !_orderType.Any())
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            ModuleItems = _orderType
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
        public virtual JsonResult SearchOpertionalStyle()
        {
            try
            {
                var Order_OpertionalStyle_moduleType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Order_OpertionalStyle_moduleType_Id"]);
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var _orderType = this.svc.GetOrderOpertionalStyleList(Order_OpertionalStyle_moduleType_Id);
                        if (_orderType == null || !_orderType.Any())
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            ModuleItems = _orderType
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
        public virtual JsonResult SearchOrderUOM(int orderTypeId)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var _orderType = this.svc.GetOrderUOMList(orderTypeId);
                        if (_orderType == null || !_orderType.Any())
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            OrderUOM = _orderType
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
        public virtual JsonResult SearchComplimentary()
        {
            try
            {
                var moduleType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Complimentary_moduleType_Id"]);
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var _complimentary = this.svc.GetComplimentaryList(moduleType_Id);
                        if (_complimentary == null || !_complimentary.Any())
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            ModuleItems = _complimentary
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
        public virtual JsonResult GetremarksComplimentary(int complimentaryId)
        {
            try
            {

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var _remarks = this.svc.GetRrmarksComplimentary(complimentaryId);
                        if (_remarks == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            Remarks = _remarks
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
        public virtual JsonResult GetOperationStyleRemarks(int operationStyleId)
        {
            try
            {

                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canAdd || _accessRight.canEdit)
                    {
                        var _remarks = this.svc.GetOperationStyleRemarks(operationStyleId);
                        if (_remarks == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            Remarks = _remarks
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
        public virtual JsonResult ProductPanelDetail(string partNum)
        {
            try
            {
                var moduleType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Complimentary_moduleType_Id"]);
                var orderTypeModuleId = Convert.ToInt32(WebConfigurationManager.AppSettings["OrderType_moduleType_Id"]);
                var orderOperationalStyleModuleTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["Order_OpertionalStyle_moduleType_Id"]);
                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);

                if (_accessRight == null)
                {
                    return this.Msg_AccessDenied();
                }

                if (!_accessRight.canAdd && !_accessRight.canEdit)
                {
                    return this.Msg_AccessDeniedInViewOrEdit();
                }

                // Get Complimentary List
                var complimentaries = this.svc.GetComplimentaryList(moduleType_Id);
                if (complimentaries == null || !complimentaries.Any())
                {
                    return this.Msg_ErrorInRetriveData();
                }

                // Get Operational Style List
                var operationalStyles = this.svc.GetOrderOpertionalStyleList(orderOperationalStyleModuleTypeId);
                if (operationalStyles == null || !operationalStyles.Any())
                {
                    return this.Msg_ErrorInRetriveData();
                }

                var orderType = this.svc.GetOrderTypeList(orderTypeModuleId);

                // Prepare the product data model
                var product = svc.GetProductByPartNum(partNum);
                if (product == null)
                {
                    return this.Msg_ErrorInRetriveData();
                }
                product.productStockBalances = this.svc.GetProductStockBalanceList(new List<string> { product.partNumber });
                // Remove the operational style with the name "Cube" if isCube is false
                if (product.isCube == false)
                {
                    operationalStyles = operationalStyles
                        .Where(style => !string.Equals(style.name, "Cube", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                if (product.isSlice == false)
                {
                    operationalStyles = operationalStyles
                        .Where(style => !string.Equals(style.name, "Slice", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                if (product.isStrip == false)
                {
                    operationalStyles = operationalStyles
                        .Where(style => !string.Equals(style.name, "Strip", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                var priceByUom = new Dictionary<string, string>();
                var PriceLookupMode = WebConfigurationManager.AppSettings["ProductPriceLookupMode"];                

                if (PriceLookupMode == "offline")
                {
                    List<UomConvs> uomConversions = new List<UomConvs>();
                    try
                    {
                        List<string> partNumbers = new List<string>();
                        partNumbers.Add(product.partNumber);
                        uomConversions = this.svc.GetUomConvList(partNumbers);
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }

                    if (uomConversions != null)
                    {
                        foreach (var detail in uomConversions)
                        {
                            priceByUom[detail.UomCode] = detail.UomCode;
                        }
                    }
                } else
                {
                    List<UomConvApiResponseModel> uomConversions = new List<UomConvApiResponseModel>();
                    UomConvApiResponseModel uomConversion = new UomConvApiResponseModel();
                    try
                    {
                        List<string> partNumbers = new List<string>();
                        partNumbers.Add(product.partNumber);
                        uomConversions = this.syncManager.GetUomConversions(partNumbers);
                        uomConversion = uomConversions[0];
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }

                    if (uomConversion != null && uomConversion.uomConvDetails != null)
                    {
                        foreach (var detail in uomConversion.uomConvDetails)
                        {
                            priceByUom[detail.uomCode] = detail.uomCode;
                        }
                    }
                }


                product.UomPrices = priceByUom;

                // Return combined JSON response
                return Json(new
                {
                    success = true,
                    product = product,
                    complimentaries = complimentaries,
                    operationalStyles = operationalStyles,
                    orderType = orderType,
                });
            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        #endregion
    }
}