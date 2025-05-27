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
using Hanodale.Domain.Models;
using System.Windows.Input;
using System.Data.Objects;
using Elmah.ContentSyndication;
using Hanodale.DataAccessLayer.Services;


namespace Hanodale.WebUI.Controllers
{
    public partial class PickupController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string PAGE_URLForAccessRight = "Pickup/Index";
        #endregion

        #region Constructor

        private readonly IOrderService svc;
        private readonly IUserService svcUser;

        public PickupController(IOrderService _bLService, ICommonService _svcCommon, IUserService _svcUser)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "Pickup";
            this.svc = _bLService;
            this.svcUser = _svcUser;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Pickup_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
        }
        #endregion

        #region Module Item Details

        [AppAuthorize]
        public virtual ActionResult Index(string id = "0", bool readOnly = false)
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new OrderModel());

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

                _model.accessRight = _accessRight;

                _model.masterRecord_Id = id;
                _model.readOnly = readOnly;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        int userId = this.CurrentUserId;
                        //check picker login

                        var _user = this.svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                        bool _isPicker = false;
                        if (_user != null)
                        {
                            _isPicker = _user.roleName.ToLower().Contains("picker") ? true : false;
                        }


                        using (HanodaleEntities model = new HanodaleEntities())
                        {
                            
                            if (id != "0")
                            {
                                var masterRecord_Id = Common.DecryptToID(userId.ToString(), id);

                                ViewBag.PendingCount = model.Order.Count(o => o.orderStatus == "SubmitForPicking" && o.customer_Id == masterRecord_Id && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                ViewBag.PickedCount = model.Order.Count(o =>
                                    o.orderStatus == "Picked"
                                    && o.customer_Id == masterRecord_Id
                                    && EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7
                                    && o.OrderUpdate
                                         .Where(ou => ou.actionName == "Items Scanned" && (ou.user_Id == userId || !_isPicker))
                                         .OrderByDescending(ou => ou.actionDate)
                                         .FirstOrDefault() != null
                                );
                                // ViewBag.PickedCount = model.Order.Count(o => o.orderStatus == "Picked" && o.customer_Id == masterRecord_Id && (o.OrderUpdate.OrderByDescending(a=>a.actionDate).FirstOrDefault(p => p.actionName == "Picked").user_Id == userId || !_isPicker) && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                ViewBag.PickupAcceptedCount = model.Order.Count(o => o.orderStatus == "PickupAccepted" && o.customer_Id == masterRecord_Id && (o.OrderUpdate.OrderByDescending(a => a.actionDate).FirstOrDefault(p => p.actionName == "Picked").user_Id == userId || !_isPicker) && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));

                                //ViewBag.PendingCount = model.Order.Count(o => o.orderStatus == "SubmitForPicking" && o.customer_Id == masterRecord_Id && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                //ViewBag.PickedCount = model.Order.Count(o => o.orderStatus == "Picked" && o.customer_Id == masterRecord_Id && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                //ViewBag.PickupAcceptedCount = model.Order.Count(o => o.orderStatus == "PickupAccepted" && o.customer_Id == masterRecord_Id &&  (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                            }
                            else
                            {
                                ViewBag.PendingCount = model.Order.Count(o => o.orderStatus == "SubmitForPicking" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                ViewBag.PickedCount = model.Order.Count(o =>
                                    o.orderStatus == "Picked"
                                    && EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7
                                    && o.OrderUpdate
                                         .Where(ou => ou.actionName == "Items Scanned" && (ou.user_Id == userId || !_isPicker))
                                         .OrderByDescending(ou => ou.actionDate)
                                         .FirstOrDefault() != null
                                );
                                //ViewBag.PickedCount = model.Order.Count(o => o.orderStatus == "Picked" && (o.OrderUpdate.OrderByDescending(a => a.actionDate).FirstOrDefault(p => p.actionName == "Item Scanned").user_Id == userId || !_isPicker)  && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                ViewBag.PickupAcceptedCount = model.Order.Count(o => o.orderStatus == "PickupAccepted" && (o.OrderUpdate.OrderByDescending(a => a.actionDate).FirstOrDefault(p => p.actionName == "PickupAccepted").user_Id == userId || !_isPicker) && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));

                                //ViewBag.PendingCount = model.Order.Count(o => o.orderStatus == "SubmitForPicking" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                //ViewBag.PickedCount = model.Order.Count(o => o.orderStatus == "Picked" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                //ViewBag.PickupAcceptedCount = model.Order.Count(o => o.orderStatus == "PickupAccepted" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                            }
                        }

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
        public virtual ActionResult BindPickup(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            List<Orders> filteredOrder = null;
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
                        if (myKey != null && myKey != "0")
                            masterRecord_Id = Common.DecryptToID(userId.ToString(), myKey);

                        var filter = new DatatableFilters
                        {
                            currentUserId = userId,
                            masterRecord_Id = masterRecord_Id,
                            startIndex = param.iDisplayStart,
                            pageSize = param.iDisplayLength,
                            search = param.sSearch,
                            conditionType = param.conditionType
                        };
                        var orderModel = this.svc.GetOrder(filter);

                        

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();
                            //check picker login
                            bool _isPicker = false;
                            var _user = this.svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                            if (_user != null)
                            {
                                _isPicker = _user.roleName.ToLower().Contains("picker") ? true : false;
                            }

                            //Sorting
                            if ((param.conditionType == "PickupAccepted" || param.conditionType == "Picked") && _isPicker)
                            {
                                filteredOrder = orderModel.lstOrder.Where(a => a.pickerUserId == this.CurrentUserId ).ToList();
                            }
                            else
                            {
                                filteredOrder = orderModel.lstOrder;
                            }
                           
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

                                filteredOrder = filteredOrder.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = OrderData(filteredOrder, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = orderModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = orderModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> OrderData(List<Orders> orderEntry, int currentUserId)
        {
            var result = this.GetDatatableData<Orders>(orderEntry, currentUserId);
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
                    viewMarkup = Common.RenderPartialViewToString(this, "RenderAction", _accessRight)
                });
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }


        #region status workflow 
        [HttpPost]
        [Authorize]
        public virtual JsonResult AcceptOrder(string id)
        {
            try
            {
                var userId = this.CurrentUserId;
                int orderId = Common.DecryptToID(userId.ToString(), id);
                bool result = this.svc.UpdateOrderStatus(orderId, "PickupAccepted", this.CurrentUserId, "PickupAccepted", "");

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Order status updated successfully."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Failed to update order status. Order not found or status unchanged."
                    });
                }
            }

            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(
                    ex);
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

                        var _model = GetOrderDetails(decrypted_Id, readOnly);

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, "ViewOrder", _model)
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
        public virtual JsonResult OrderPicking(string id, bool readOnly)
        {
            try
            {
                var currentUserId = this.CurrentUserId;

                var _accessRight = Common.GetUserRights(currentUserId, PAGE_URLForAccessRight);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var decrypted_Id = Common.DecryptToID(currentUserId.ToString(), id);

                        var _model = GetOrderDetails(decrypted_Id, readOnly);
                        _model.minTolerance = Decimal.Parse(WebConfigurationManager.AppSettings["ScannedQtyMinTolerance"] ?? "0.5");
                        _model.maxTolerance = Decimal.Parse(WebConfigurationManager.AppSettings["ScannedQtyMaxTolerance"] ?? "0.5");
                        _model.DisableScannedQtyValidation = bool.Parse(WebConfigurationManager.AppSettings["DisableScannedQtyValidation"] ?? "true");
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, "OrderPicking", _model)
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
        public virtual JsonResult VerifyOrder(string id, bool readOnly)
        {
            try
            {
                var currentUserId = this.CurrentUserId;

                var _accessRight = Common.GetUserRights(currentUserId, PAGE_URLForAccessRight);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var decrypted_Id = Common.DecryptToID(currentUserId.ToString(), id);

                        var _model = GetOrderDetails(decrypted_Id, readOnly);

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, "VerifyOrder", _model)
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

        //private ViewOrderModel GetOrderDetails(int decrypted_Id, bool readOnly)
        //{
        //    var order = this.svc.GetOrderDetails(decrypted_Id);
        //    // var orderScannedItems = this.svc.getOrderScannedItems(decrypted_Id);
        //    var _model = new ViewOrderModel
        //    {
        //        id = order.id,
        //        customer_Id = order.customer_Id,
        //        orderNum = order.orderNum,
        //        shipToAddress_Id = order.shipToAddress_Id,
        //        shipToName = order.shipToName ?? "",
        //        orderDate = order.orderDate?.ToString("dd/MM/yyyy hh:mm tt"),
        //        orderComment = order.orderComment,
        //        entryPerson = order.entryPerson,
        //        docOrderAmt = order.docOrderAmt,
        //        customerName = order.customerName,
        //        orderStatus = order.orderStatus,
        //        customerAddress = order.customerAddress,
        //        tinId = order.tinId ?? "",
        //        shipToAddress = order.shipToAddress ?? "",
        //        OrderItems = order.OrderItems?.Select(item => new ViewOrderItemModel
        //        {
        //            partNum = item.partNum,
        //            product_Id = (int)item.product_Id,
        //            itemId = (int)item.itemId,
        //            itemDbId = item.itemId,
        //            lineDesc = item.lineDesc,
        //            prodGroup = item.prodGroup,
        //            ium = item.ium,
        //            comments = item.comments,
        //            unitPrice = (decimal)item.unitPrice,
        //            orderUOM = item.orderUOM,
        //            salesUm = item.salesUm,
        //            AllowSellingVaryWeight = item.AllowSellingVaryWeight,
        //            orderQty = item.orderQty,
        //            orderType = item.orderType,
        //            operationName = item.operationName,
        //            complementary = item.complementary,
        //            scannedQty = (decimal)item.scannedQty,
        //            orderId = item.orderId,
        //            orderItemId = item.orderItemId,
        //            itemBrandName = item.itemBrandName ?? "",
        //            country = item.country,
        //            description = item.partName,
        //            scanQtyStr = item.scannedQtyStr,
        //            operationCost = item.operationCost,
        //            listPrice = item.listPrice,
        //        }).ToList() ?? new List<ViewOrderItemModel>(),
        //        OrderPayments = new List<OrderPaymentModel>(),
        //        OrderScanned = order.OrderScanned.Select(item => new OrderScannedModel
        //        {
        //            serialNo = item.serialNo,
        //            orderItem_Id = (int)item.orderItem_Id,
        //            orderId = (int)item.orderId,
        //            scannedQty = item.scannedQty,
        //            status = item.status,
        //            verifyStatus = item.verifyStatus,
        //            Group = item.Group,
        //            partNo = item.partNo,
        //            partName = item.partName,
        //            orderUOM = item.orderUOM,
        //            allowVaryWeight = item.allowVaryWeight,

        //        }).ToList() ?? new List<OrderScannedModel>(),
        //    };

        //    return _model;
        //}

        //private ViewOrderModel GetOrderDetails(int decrypted_Id, bool readOnly)
        //{
        //    var order = this.svc.GetOrderDetails(decrypted_Id);
        //    var stockService = new Hanodale.DataAccessLayer.Services.StockBalanceService();
        //    var _model = new ViewOrderModel
        //    {
        //        id = order.id,
        //        customer_Id = order.customer_Id,
        //        orderNum = order.orderNum,
        //        shipToAddress_Id = order.shipToAddress_Id,
        //        shipToName = order.shipToName ?? "",
        //        orderDate = order.orderDate?.ToString("dd/MM/yyyy hh:mm tt"),
        //        orderComment = order.orderComment,
        //        entryPerson = order.entryPerson,
        //        docOrderAmt = order.docOrderAmt,
        //        customerName = order.customerName,
        //        orderStatus = order.orderStatus,
        //        customerAddress = order.customerAddress,
        //        tinId = order.tinId ?? "",
        //        shipToAddress = order.shipToAddress ?? "",

        //        OrderItems = order.OrderItems?.Select(item => new ViewOrderItemModel
        //        {
        //            partNum = item.partNum,
        //            product_Id = (int)item.product_Id,
        //            itemId = (int)item.itemId,
        //            itemDbId = item.itemId,
        //            lineDesc = item.lineDesc,
        //            prodGroup = item.prodGroup,
        //            ium = item.ium,
        //            comments = item.comments,
        //            unitPrice = (decimal)item.unitPrice,
        //            orderUOM = item.orderUOM,
        //            salesUm = item.salesUm,
        //            AllowSellingVaryWeight = item.AllowSellingVaryWeight,
        //            orderQty = item.orderQty,
        //            orderType = item.orderType,
        //            operationName = item.operationName,
        //            complementary = item.complementary,
        //            scannedQty = (decimal)item.scannedQty,
        //            orderId = item.orderId,
        //            orderItemId = item.orderItemId,
        //            itemBrandName = item.itemBrandName ?? "",
        //            country = item.country,
        //            description = item.partName,
        //            scanQtyStr = item.scannedQtyStr,
        //            operationCost = item.operationCost,
        //            listPrice = item.listPrice,
        //        }).ToList() ?? new List<ViewOrderItemModel>(),

        //        OrderPayments = new List<OrderPaymentModel>(),

        //        OrderScanned = order.OrderScanned.Select(item =>
        //        {
        //            var locations = stockService.GetStockLocationOptions(item.partNo);
        //            return new OrderScannedModel
        //            {
        //                serialNo = item.serialNo,
        //                orderItem_Id = (int)item.orderItem_Id,
        //                orderId = (int)item.orderId,
        //                scannedQty = item.scannedQty,
        //                status = item.status,
        //                verifyStatus = item.verifyStatus,
        //                Group = item.Group,
        //                partNo = item.partNo,
        //                partName = item.partName,
        //                orderUOM = item.orderUOM,
        //                allowVaryWeight = item.allowVaryWeight,
        //                productLocation = item.productLocation,
        //                LocationOptions = locations.Select(loc => new SelectListItem
        //                {
        //                    Text = loc,
        //                    Value = loc
        //                }).ToList()

        //            };
        //        }).ToList() ?? new List<OrderScannedModel>()
        //    };

        //    return _model;
        //}


        private ViewOrderModel GetOrderDetails(int decrypted_Id, bool readOnly)
        {
            var order = this.svc.GetOrderDetails(decrypted_Id);
            var stockService = new Hanodale.DataAccessLayer.Services.StockBalanceService();

            var _model = new ViewOrderModel
            {
                id = order.id,
                customer_Id = order.customer_Id,
                orderNum = order.orderNum,
                shipToAddress_Id = order.shipToAddress_Id,
                shipToName = order.shipToName ?? "",
                orderDate = order.orderDate?.ToString("dd/MM/yyyy hh:mm tt"),
                orderComment = order.orderComment,
                entryPerson = order.entryPerson,
                docOrderAmt = order.docOrderAmt,
                customerName = order.customerName,
                orderStatus = order.orderStatus,
                customerAddress = order.customerAddress,
                tinId = order.tinId ?? "",
                shipToAddress = order.shipToAddress ?? "",

                // ✅ Group OrderItems by partNum and sum orderQty
                //OrderItems = order.OrderItems?
                //    .GroupBy(item => item.partNum)
                //    .Select(g => {
                //        var firstItem = g.First();
                //        return new ViewOrderItemModel
                //        {
                //            partNum = firstItem.partNum,
                //            orderQty = g.Sum(x => x.orderQty),   // ✅ SUM of quantities
                //            product_Id = (int)firstItem.product_Id,
                //            itemId = (int)firstItem.itemId,
                //            itemDbId = firstItem.itemId,
                //            lineDesc = firstItem.lineDesc,
                //            prodGroup = firstItem.prodGroup,
                //            ium = firstItem.ium,
                //            comments = firstItem.comments,
                //            unitPrice = (decimal)firstItem.unitPrice,
                //            orderUOM = firstItem.orderUOM,
                //            salesUm = firstItem.salesUm,
                //            AllowSellingVaryWeight = firstItem.AllowSellingVaryWeight,
                //            orderType = firstItem.orderType,
                //            operationName = firstItem.operationName,
                //            complementary = firstItem.complementary,
                //            scannedQty = (decimal)firstItem.scannedQty,
                //            orderId = firstItem.orderId,
                //            orderItemId = firstItem.orderItemId,
                //            itemBrandName = firstItem.itemBrandName ?? "",
                //            country = firstItem.country,
                //            description = firstItem.partName,
                //            scanQtyStr = firstItem.scannedQtyStr,
                //            operationCost = firstItem.operationCost,
                //            listPrice = firstItem.listPrice,
                //            LocationList = stockService.GetStockLocationOptions(firstItem.partNum), // Fetch locations for each partNum
                //        };
                //    }).ToList() ?? new List<ViewOrderItemModel>(),
                OrderItems = order.OrderItems?.Select(item => new ViewOrderItemModel
                {
                    orderLine = item.orderLine,
                    partNum = item.partNum,
                    product_Id = (int)item.product_Id,
                    itemId = (int)item.itemId,
                    itemDbId = item.itemId,
                    lineDesc = item.lineDesc,
                    prodGroup = item.prodGroup,
                    ium = item.ium,
                    comments = item.comments,
                    unitPrice = (decimal)item.unitPrice,
                    orderUOM = item.orderUOM,
                    salesUm = item.salesUm,
                    AllowSellingVaryWeight = item.AllowSellingVaryWeight,
                    orderQty = item.orderQty,
                    orderType = item.orderType,
                    operationName = item.operationName,
                    complementary = item.complementary,
                    scannedQty = (decimal)item.scannedQty,
                    orderId = item.orderId,
                    orderItemId = item.orderItemId,
                    itemBrandName = item.itemBrandName ?? "",
                    country = item.country,
                    description = item.partName,
                    scanQtyStr = item.scannedQtyStr,
                    operationCost = item.operationCost,
                    listPrice = item.listPrice,
                    LocationList = stockService.GetStockLocationOptions(item.partNum), // Fetch locations for each partNum
                }).ToList() ?? new List<ViewOrderItemModel>(),
                OrderPayments = new List<OrderPaymentModel>(),

                // ✅ OrderScanned remains as it is — fetching product locations for scanning
                OrderScanned = order.OrderScanned.Select(item =>
                {
                    var locations = stockService.GetCartonLocationsOptions(item.partNo,item.serialNo);

                    return new OrderScannedModel
                    {
                        serialNo = item.serialNo,
                        orderItem_Id = (int)item.orderItem_Id,
                        orderId = (int)item.orderId,
                        scannedQty = item.scannedQty,
                        status = item.status,
                        verifyStatus = item.verifyStatus,
                        Group = item.Group,
                        partNo = item.partNo,
                        partName = item.partName,
                        orderUOM = item.orderUOM,
                        allowVaryWeight = item.allowVaryWeight,
                        productLocation = item.productLocation,

                        LocationOptions = locations.Select(loc => new SelectListItem
                        {
                            Text = loc,
                            Value = loc
                        }).ToList()
                        // productLocation = item.productLocation,
                        //LocationOptions = locations.Select(loc => new SelectListItem
                        //{
                        //    Text = loc,
                        //    Value = loc
                        //}).ToList()
                    };
                }).ToList() ?? new List<OrderScannedModel>()
            };

            return _model;
        }


        //private ViewOrderModel GetOrderDetails(int decrypted_Id, bool readOnly)
        //{
        //    var order = this.svc.GetOrderDetails(decrypted_Id);
        //    var stockService = new Hanodale.DataAccessLayer.Services.StockBalanceService();
        //    var _model = new ViewOrderModel
        //    {
        //        id = order.id,
        //        customer_Id = order.customer_Id,
        //        orderNum = order.orderNum,
        //        shipToAddress_Id = order.shipToAddress_Id,
        //        shipToName = order.shipToName ?? "",
        //        orderDate = order.orderDate?.ToString("dd/MM/yyyy hh:mm tt"),
        //        orderComment = order.orderComment,
        //        entryPerson = order.entryPerson,
        //        docOrderAmt = order.docOrderAmt,
        //        customerName = order.customerName,
        //        orderStatus = order.orderStatus,
        //        customerAddress = order.customerAddress,
        //        tinId = order.tinId ?? "",
        //        shipToAddress = order.shipToAddress ?? "",

        //        // ✅ Replace OrderItems with Grouping logic
        //        OrderItems = order.OrderItems?
        //            .GroupBy(item => item.partNum)
        //            .Select(g => {
        //                var firstItem = g.First();
        //                return new ViewOrderItemModel
        //                {
        //                    partNum = firstItem.partNum,
        //                    product_Id = (int)firstItem.product_Id,
        //                    itemId = (int)firstItem.itemId,
        //                    itemDbId = firstItem.itemId,
        //                    lineDesc = firstItem.lineDesc,
        //                    prodGroup = firstItem.prodGroup,
        //                    ium = firstItem.ium,
        //                    comments = firstItem.comments,
        //                    unitPrice = (decimal)firstItem.unitPrice,
        //                    orderUOM = firstItem.orderUOM,
        //                    salesUm = firstItem.salesUm,
        //                    AllowSellingVaryWeight = firstItem.AllowSellingVaryWeight,
        //                    orderQty = g.Sum(x => x.orderQty), // Sum all orderQty inside same partNum group
        //                    orderType = firstItem.orderType,
        //                    operationName = firstItem.operationName,
        //                    complementary = firstItem.complementary,
        //                    scannedQty = (decimal)firstItem.scannedQty,
        //                    orderId = firstItem.orderId,
        //                    orderItemId = firstItem.orderItemId,
        //                    itemBrandName = firstItem.itemBrandName ?? "",
        //                    country = firstItem.country,
        //                    description = firstItem.partName,
        //                    scanQtyStr = firstItem.scannedQtyStr,
        //                    operationCost = firstItem.operationCost,
        //                    listPrice = firstItem.listPrice,
        //                };
        //            }).ToList() ?? new List<ViewOrderItemModel>(),

        //        OrderPayments = new List<OrderPaymentModel>(),

        //        OrderScanned = order.OrderScanned.Select(item =>
        //        {
        //            var locations = stockService.GetStockLocationOptions(item.partNo);
        //            System.Diagnostics.Debug.WriteLine("LOCATIONS FOR " + item.partNo + ": " + string.Join(",", locations));

        //            return new OrderScannedModel
        //            {
        //                serialNo = item.serialNo,
        //                orderItem_Id = (int)item.orderItem_Id,
        //                orderId = (int)item.orderId,
        //                scannedQty = item.scannedQty,
        //                status = item.status,
        //                verifyStatus = item.verifyStatus,
        //                Group = item.Group,
        //                partNo = item.partNo,
        //                partName = item.partName,
        //                orderUOM = item.orderUOM,
        //                allowVaryWeight = item.allowVaryWeight,
        //                productLocation = item.productLocation,
        //                LocationOptions = locations.Select(loc => new SelectListItem
        //                {
        //                    Text = loc,
        //                    Value = loc
        //                }).ToList()

        //            };
        //        }).ToList() ?? new List<OrderScannedModel>()

        //    };

        //    return _model;
        //}



        //[HttpPost]
        //[Authorize]
        //public virtual JsonResult Scan(int orderId, string serial)
        //{
        //    var _order = this.svc.GetOrderDetails(orderId);//replace this from model
        //    ProductBarcode _barcodeItem;
        //    bool _isMatch = false;
        //    string _barcode = serial;
        //    string _labelWeight = "0.0000";

        //    try
        //    {
        //        //chk the productcarton labels
        //        _barcodeItem = this.svc.GetProductCartons(serial);
        //        if (_barcodeItem != null)
        //        {
        //            var _item = _order.OrderItems.Find(a => a.partNum == _barcodeItem.epicorePartNo);//&& a.orderType == "Full Quantity");

        //            if (_item != null)
        //            {
        //                if (serial.Length >= _barcodeItem.weightToPos && _barcodeItem.weightFromPos != 0)
        //                {                            
        //                    _labelWeight = serial.Substring(_barcodeItem.weightFromPos - 1, ((_barcodeItem.weightToPos - _barcodeItem.weightFromPos) + 1));
        //                    _barcodeItem.weightValue = (Convert.ToDecimal(_labelWeight) * Convert.ToDecimal(_barcodeItem.weightMutiplier)).ToString();                            
        //                }
        //                int _labelCnt= _order.OrderScanned.Where(p => p.serialNo == serial && !p.IsReturned && p.partNo == _item.partNum).Count();
        //                if (_labelCnt <= 0)
        //                {
        //                    _isMatch = true;
        //                    _barcodeItem.orderItemId = _item.orderId;
        //                    _barcodeItem.productId = _item.product_Id;
        //                }
        //                _barcodeItem.allowVaryWeight = _item.AllowSellingVaryWeight == "Yes";
        //                // If not vary weight then scanned qty becomes 1
        //                if (_item.AllowSellingVaryWeight != "Yes")
        //                {
        //                    _barcodeItem.weightValue = "1";
        //                    _isMatch = true;
        //                    _barcodeItem.orderItemId = _item.orderId;
        //                    _barcodeItem.productId = _item.product_Id;
        //                }
        //            }
        //        }

        //        _labelWeight = "0.0000";
        //        if (serial.Length > 6)
        //        {
        //            _barcode = serial.Substring(0, 6);
        //        }
        //        //chk the product weight labels
        //        if (!_isMatch)
        //        {
        //            _barcodeItem = null;
        //            _barcodeItem = this.svc.GetProductWeightBarcodes(_barcode);
        //            _barcodeItem.barcode = serial;
        //            if (_barcodeItem != null)
        //            {
        //                if (serial.Length >= _barcodeItem.weightToPos) // serial.Length
        //                {
        //                    _labelWeight = serial.Substring(_barcodeItem.weightFromPos-1, ((_barcodeItem.weightToPos-_barcodeItem.weightFromPos)+1));
        //                    _barcodeItem.weightValue = (Convert.ToDecimal(_labelWeight) * Convert.ToDecimal(_barcodeItem.weightMutiplier)).ToString();
        //                }
        //                var _item = _order.OrderItems.Find(a => a.partNum == _barcodeItem.epicorePartNo);// && a.orderType == "Loose Quantity");
        //                if (_item != null)
        //                {
        //                    int _labelCnt = _order.OrderScanned.Where(p => p.serialNo == serial && !p.IsReturned && p.partNo == _item.partNum).Count();
        //                    if (_labelCnt <= 0)
        //                    {
        //                        _isMatch = true;
        //                        _barcodeItem.orderItemId = _item.orderId;
        //                        _barcodeItem.productId = _item.product_Id;
        //                    }

        //                    _barcodeItem.allowVaryWeight = _item.AllowSellingVaryWeight == "Yes";
        //                    // If not vary weight then scanned qty becomes 1
        //                    if (_item.AllowSellingVaryWeight != "Yes")
        //                    {
        //                        _barcodeItem.weightValue = "1";
        //                        _isMatch = true;
        //                        _barcodeItem.orderItemId = _item.orderId;
        //                        _barcodeItem.productId = _item.product_Id;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //construct the product here
        //    return Json(new
        //    {
        //        scannedItem = _barcodeItem
        //    });
        //}

        //[HttpPost]
        //[Authorize]
        //public virtual JsonResult Scan(int orderId, string serial)
        //{
        //    var _order = this.svc.GetOrderDetails(orderId);
        //    ProductBarcode _barcodeItem = null;
        //    bool _isMatch = false;
        //    string _barcode = serial;
        //    string _labelWeight = "0.0000";

        //    try
        //    {
        //        // 1️⃣ Check in ProductCarton table (Serial linked barcodes)
        //        _barcodeItem = this.svc.GetProductCartons(serial);

        //        if (_barcodeItem != null)
        //        {
        //            var _item = _order.OrderItems.Find(a => a.partNum == _barcodeItem.epicorePartNo);

        //            if (_item != null)
        //            {
        //                // Calculate weight if required
        //                if (serial.Length >= _barcodeItem.weightToPos && _barcodeItem.weightFromPos != 0)
        //                {
        //                    _labelWeight = serial.Substring(_barcodeItem.weightFromPos - 1, ((_barcodeItem.weightToPos - _barcodeItem.weightFromPos) + 1));
        //                    _barcodeItem.weightValue = (Convert.ToDecimal(_labelWeight) * Convert.ToDecimal(_barcodeItem.weightMutiplier)).ToString();
        //                }

        //                // Avoid duplicate serial scan
        //                int _labelCnt = _order.OrderScanned.Count(p => p.serialNo == serial && !p.IsReturned && p.partNo == _item.partNum);
        //                if (_labelCnt <= 0)
        //                {
        //                    _isMatch = true;
        //                    _barcodeItem.orderItemId = _item.orderId;
        //                    _barcodeItem.productId = _item.product_Id;

        //                }

        //                _barcodeItem.allowVaryWeight = _item.AllowSellingVaryWeight == "Yes";

        //                if (_item.AllowSellingVaryWeight != "Yes")
        //                {
        //                    _barcodeItem.weightValue = "1";
        //                    _isMatch = true;
        //                    _barcodeItem.orderItemId = _item.orderId;
        //                    _barcodeItem.productId = _item.product_Id;
        //                    _barcodeItem.productLocation = _barcodeItem.Location;
        //                }
        //               // _barcodeItem.productLocation = _barcodeItem.Location;

        //            }
        //        }

        //        // 2️⃣ If not matched, try ProductWeightBarcode table (Weight based barcodes)
        //        if (!_isMatch)
        //        {
        //            _barcodeItem = null;

        //            if (serial.Length > 6)
        //            {
        //                _barcode = serial.Substring(0, 6);
        //            }

        //            _barcodeItem = this.svc.GetProductWeightBarcodes(_barcode);

        //            if (_barcodeItem != null)
        //            {
        //                _barcodeItem.barcode = serial;

        //                if (serial.Length >= _barcodeItem.weightToPos)
        //                {
        //                    _labelWeight = serial.Substring(_barcodeItem.weightFromPos - 1, ((_barcodeItem.weightToPos - _barcodeItem.weightFromPos) + 1));
        //                    _barcodeItem.weightValue = (Convert.ToDecimal(_labelWeight) * Convert.ToDecimal(_barcodeItem.weightMutiplier)).ToString();
        //                }

        //                var _item = _order.OrderItems.Find(a => a.partNum == _barcodeItem.epicorePartNo);

        //                if (_item != null)
        //                {
        //                    int _labelCnt = _order.OrderScanned.Count(p => p.serialNo == serial && !p.IsReturned && p.partNo == _item.partNum);
        //                    if (_labelCnt <= 0)
        //                    {
        //                        _isMatch = true;
        //                        _barcodeItem.orderItemId = _item.orderId;
        //                        _barcodeItem.productId = _item.product_Id;
        //                    }

        //                    _barcodeItem.allowVaryWeight = _item.AllowSellingVaryWeight == "Yes";

        //                    if (_item.AllowSellingVaryWeight != "Yes")
        //                    {
        //                        _barcodeItem.weightValue = "1";
        //                        _isMatch = true;
        //                        _barcodeItem.orderItemId = _item.orderId;
        //                        _barcodeItem.productId = _item.product_Id;
        //                        _barcodeItem.productLocation = _barcodeItem.Location;
        //                    }
        //                    //_barcodeItem.productLocation = _barcodeItem.Location;

        //                }
        //            }
        //        }

        //        // 3️⃣ ⛳️ If found, fetch possible locations from StockBalance table
        //        //if (_barcodeItem != null && _isMatch)
        //        //{
        //        //    using (var model = new HanodaleEntities())
        //        //    {
        //        //        var partNum = _barcodeItem.epicorePartNo;

        //        //        var locations = model.StockBalance
        //        //            .Where(sb => sb.partNum == partNum && sb.onHandQty > 0)
        //        //            .Select(sb => sb.Location)
        //        //            .Distinct()
        //        //            .ToList();

        //        //        _barcodeItem.LocationList = locations;
        //        //        _barcodeItem.defaultLocation = locations.Count == 1 ? locations.FirstOrDefault() : null;
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        // Safer error handling
        //        throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
        //    }

        //    return Json(new
        //    {
        //        scannedItem = _barcodeItem
        //    });
        //}


        [HttpPost]
        [Authorize]
        public virtual JsonResult Scan(int orderId, string serial)
        {
            var _order = this.svc.GetOrderDetails(orderId);
            ProductBarcode _barcodeItem = null;
            bool _isMatch = false;
            string _barcode = serial;
            string _labelWeight = "0.0000";

            try
            {
                // 7704 : Full Quantity
                // 7705 : Loose Quantity
                int orderType = 7704; // Default to Full Quantity for now
                string barcodeType = "Carton";
                // Try ProductCarton
                _barcodeItem = this.svc.GetProductCartons(serial, barcodeType);

                if (_barcodeItem != null)
                {
                    //If user selected orderType = Full Qty (7704) - Match items with Cartons only
                    // But sometimes User not select orderType as Full Qty for Varywg items, empty scannedlabel could be used to match for full carton
                    var _item = _order.OrderItems.Where(w=>w.QtyType_ModuleItem_Id==orderType || string.IsNullOrEmpty(w.scannedLabel))
                                    .FirstOrDefault(a => a.partNum == _barcodeItem.epicorePartNo);

                    if (_item != null)
                    {
                        //if (serial.Length >= _barcodeItem.weightToPos && _barcodeItem.weightFromPos != 0)
                        //{
                        //    _labelWeight = serial.Substring(_barcodeItem.weightFromPos - 1, ((_barcodeItem.weightToPos - _barcodeItem.weightFromPos) + 1));
                        //    _barcodeItem.weightValue = (Convert.ToDecimal(_labelWeight) * Convert.ToDecimal(_barcodeItem.weightMutiplier)).ToString();
                        //}

                        //int _labelCnt = _order.OrderScanned.Count(p => p.serialNo == serial && !p.IsReturned && p.partNo == _item.partNum);
                        int _labelCnt = _order.OrderScanned.Count(p => p.orderItem_Id == _item.itemId && !p.IsReturned);
                        if (_labelCnt <= 0)
                        {
                            //First Scan
                            _isMatch = true;
                            _barcodeItem.orderItemId = _item.orderId;
                            _barcodeItem.productId = _item.product_Id;
                            
                        }

                        _barcodeItem.allowVaryWeight = _item.AllowSellingVaryWeight == "Yes";

                        // Always set productLocation if Location is present
                        if (!string.IsNullOrEmpty(_barcodeItem.Location))
                            _barcodeItem.productLocation = _barcodeItem.Location;
                        System.Diagnostics.Debug.WriteLine($"[SCAN] Location picked: {_barcodeItem.productLocation} for PartNo: {_barcodeItem.epicorePartNo}");

                        if (_item.AllowSellingVaryWeight != "Yes")
                        {
                            _barcodeItem.weightValue = "1";
                        }
                        else
                        {
                            _barcodeItem.weightValue = _barcodeItem.IumQty.ToString("0.0000"); // Set default weight value to IUM quantity
                        }

                    }
                }
                else
                {
                    throw new Exception($"{serial} : Barcode Carton Not Found");
                }

            
            
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return Json(new
            {
                scannedItem = _barcodeItem
            });
        }


        [HttpPost]
        [Authorize]
        public bool Reset(int orderId)
        {
            var _order = this.svc.GetOrderDetails(100);//replace this from model
            ProductBarcode _barcodeItem;
            bool _isMatch = false;
            try
            {
                //chk the productcarton labels
                bool _ret = this.svc.DeleteScanItem(orderId);
                return _ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
    #endregion
}