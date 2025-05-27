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
using Hanodale.Domain.DTOs.Order;
using Hanodale.Domain.DTOs.Sync;
using Newtonsoft.Json;
using System.IO;
using System.Data.Objects;
using Hanodale.SyncService.Models;
using Microsoft.Ajax.Utilities;
using System.Configuration;

namespace Hanodale.WebUI.Controllers
{
    public partial class OrdersController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string PAGE_URLForAccessRight = "Orders/Index";
        #endregion

        #region Constructor

        private readonly IOrderService svc;
        private readonly ICustomerService svcCustomer;
        private readonly IShipToAddressService svcShipTo;
        private readonly IUserService svcUser;
        private readonly ISyncManager syncManager;
        private readonly IAuthenticationService svcAuth;

        public OrdersController(IOrderService _bLService, ICommonService _svcCommon, ISyncManager syncManager,
            ICustomerService _svcCustomer, IShipToAddressService _svcShipTo, IUserService _svcUser)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "Orders";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Orders_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            this.syncManager = syncManager;
            this.svcCustomer = _svcCustomer;
            this.svcShipTo = _svcShipTo;
            svcUser = _svcUser;
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

                        using (HanodaleEntities model = new HanodaleEntities())
                        {
                            if (id != "0")
                            {
                                var masterRecord_Id = Common.DecryptToID(userId.ToString(), id);
                                Session["PendingCount"] = model.Order.Count(o => o.orderStatus != "Completed" && o.orderStatus != "Cancelled" && o.orderStatus != "Payment" && o.customer_Id == masterRecord_Id && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["CancelledCount"] = model.Order.Count(o => o.orderStatus == "Cancelled" && o.customer_Id == masterRecord_Id && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["CompletedCount"] = model.Order.Count(o => o.orderStatus == "Completed" && o.customer_Id == masterRecord_Id && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["PaymentCount"] = model.Order.Count(o => o.orderStatus == "Payment" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["FailedCount"] = model.Order.Count(o => o.orderStatus == "Completed" && o.syncStatus == false && o.syncedAt != null && !String.IsNullOrEmpty(o.epicoreResponse) && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                            }
                            else
                            {
                                Session["PendingCount"] = model.Order.Count(o => o.orderStatus != "Completed" && o.orderStatus != "Cancelled" && o.orderStatus != "Payment" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["CancelledCount"] = model.Order.Count(o => o.orderStatus == "Cancelled" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["CompletedCount"] = model.Order.Count(o => o.orderStatus == "Completed" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["PaymentCount"] = model.Order.Count(o => o.orderStatus == "Payment" && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                                Session["FailedCount"] = model.Order.Count(o => o.orderStatus == "Completed" && o.syncStatus == false && o.syncedAt != null && !String.IsNullOrEmpty(o.epicoreResponse) && (EntityFunctions.DiffDays(o.orderDate, DateTime.Now) <= 7));
                            }
                        }
                        //check cashier login
                        var _user = this.svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                        if (_user != null)
                        {
                            ViewBag.IsSuperAdminLogin = _user.userName.ToLower().Contains("super admin") ? true : false;
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
        public virtual ActionResult BindOrders(DataTableModel param, string myKey)
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
                            conditionType = param.conditionType,
                            OrderDateFrom = string.IsNullOrEmpty(Request["sSearch_0"]) ? "" : Request["sSearch_0"],
                            OrderDateTo = string.IsNullOrEmpty(Request["sSearch_1"]) ? "" : Request["sSearch_1"],
                            OrderNum = string.IsNullOrEmpty(Request["sSearch_2"]) ? "" : Request["sSearch_2"],
                            CustomerName = string.IsNullOrEmpty(Request["sSearch_3"]) ? "" : Request["sSearch_3"],
                            CreatedBy = string.IsNullOrEmpty(Request["sSearch_4"]) ? "" : Request["sSearch_4"],


                        };
                        var orderModel = this.svc.GetOrder(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            //Sorting

                            // Filtered orders
                            filteredOrder = orderModel.lstOrder;
                            DateTime currentDate = DateTime.Now;
                            DateTime defaultFromDate = currentDate.AddDays(-7);
                            DateTime defaultToDate = currentDate;

                            // Get current values from the request
                            string currentOrderDateFrom = string.IsNullOrEmpty(Request["sSearch_0"]) ? "" : Request["sSearch_0"];
                            string currentOrderDateTo = string.IsNullOrEmpty(Request["sSearch_1"]) ? "" : Request["sSearch_1"];

                            // Parse dates for comparison
                            DateTime parsedOrderDateFrom, parsedOrderDateTo;
                            bool isFromDateValid = DateTime.TryParse(currentOrderDateFrom, out parsedOrderDateFrom);
                            bool isToDateValid = DateTime.TryParse(currentOrderDateTo, out parsedOrderDateTo);


                            CalculateAndSetOrderCounts(filter);


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
        private void CalculateAndSetOrderCounts(DatatableFilters filter)
        {
            using (var model = new HanodaleEntities())
            {
                var query = model.Order.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.search))
                {
                    query = query.Where(o => o.orderNum.Contains(filter.search) ||
                                             o.Customer.name.Contains(filter.search) ||
                                             o.entryPerson.Contains(filter.search));
                }

                if (!string.IsNullOrEmpty(filter.CustomerName))
                {
                    query = query.Where(o => o.Customer.name.Contains(filter.CustomerName));
                }


                if (!string.IsNullOrEmpty(filter.OrderDateFrom) && DateTime.TryParse(filter.OrderDateFrom, out var dateFrom))
                {
                    query = query.Where(o => o.orderDate.HasValue &&
                                             EntityFunctions.TruncateTime(o.orderDate) >= EntityFunctions.TruncateTime(dateFrom));
                }

                if (!string.IsNullOrEmpty(filter.OrderDateTo) && DateTime.TryParse(filter.OrderDateTo, out var dateTo))
                {
                    query = query.Where(o => o.orderDate.HasValue &&
                                             EntityFunctions.TruncateTime(o.orderDate) <= EntityFunctions.TruncateTime(dateTo));
                }

                if (!string.IsNullOrEmpty(filter.OrderNum))
                {
                    query = query.Where(o => o.orderNum.Contains(filter.OrderNum));
                }

                if (!string.IsNullOrEmpty(filter.CreatedBy))
                {
                    query = query.Where(o => o.entryPerson.Contains(filter.CreatedBy));
                }


                Session["PendingCount"] = query.Count(o =>
                    o.orderStatus != "Completed" &&
                    o.orderStatus != "Cancelled" &&
                    o.orderStatus != "Payment");

                Session["CancelledCount"] = query.Count(o => o.orderStatus == "Cancelled");
                Session["CompletedCount"] = query.Count(o => o.orderStatus == "Completed");
                Session["PaymentCount"] = query.Count(o => o.orderStatus == "Payment");
                Session["FailedCount"] = query.Count(o =>
                    o.orderStatus == "Completed" &&
                    o.syncStatus == false &&
                    o.syncedAt != null &&
                    !string.IsNullOrEmpty(o.epicoreResponse));
            }
        }


        [HttpPost]
        public virtual ActionResult UpdateCount()
        {
            // Retrieve the value from TempData
            var pendingCount = Session["PendingCount"];
            var completedCount = Session["CompletedCount"];
            var cancelledCount = Session["CancelledCount"];
            var paymentCount = Session["PaymentCount"];
            var failedCount = Session["FailedCount"];

            return Json(new
            {
                PendingCount = pendingCount,
                CancelledCount = cancelledCount,
                CompletedCount = completedCount,
                PaymentCount = paymentCount,
                FailedCount = failedCount
            }, JsonRequestBehavior.AllowGet);
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

        // Action to display the customer picker screen
        [AppAuthorize]
        public virtual ActionResult CustomerBrowse()
        {
            return View("CustomerBrowse"); // This will return CustomerPicker.cshtml
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
                    //var _model = new OrderModel();
                    var _model = new EditOrderModel
                    {
                        OrderItems = new List<ViewOrderItemModel>()
                    };
                    this.FillupFieldMetadata(_model, false);

                    // _model.customer_Id = id;

                    //if (_model.shipToAddress_Id_Metadata.isEditableInCreate)
                    //{

                    //    HanodaleEntities model = new HanodaleEntities();
                    //    //Get Plant List
                    //    var query = from p in model.ShipToAddress
                    //                select new 
                    //                {
                    //                    id = p.id,
                    //                    shippingCode = p.shippingCode,
                    //                };
                    //    var _shipToAddressList = query.ToList();



                    //    _model.lstshipToAddress = _shipToAddressList.Select(p => new SelectListItem
                    //    {
                    //        Text = p.shippingCode,
                    //        Value = p.id.ToString(),


                    //    }).ToList();

                    //}

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    //TryValidateModel(_companyAddressModel);
                    var defaultCustomerId = WebConfigurationManager.AppSettings["OrderDefaultCustomerCode"];
                    var defaultShipToId = WebConfigurationManager.AppSettings["OrderDefaultShipToCode"];

                    _model.oneTimeCustomer = true;

                    var defaultCustomer = this.svcCustomer.GetCustomerByCode(defaultCustomerId);
                    if (defaultCustomer != null)
                    {
                        _model.customer_Id = defaultCustomer.id;
                        _model.customerName = defaultCustomer.name;
                        _model.defaultCustomerId = defaultCustomer.id;
                        _model.creditHold = defaultCustomer.creditHold;
                    }
                    var defaultShipTo = this.svcShipTo.GetShipToAddressByCode(defaultShipToId);
                    if (defaultShipTo != null)
                    {
                        _model.shipToAddress_Id = defaultShipTo.id;
                        _model.shipToName = defaultShipTo.shippingCode;
                        _model.defaultShipTo = defaultShipTo.id;
                    }
                    _model.ValidateStockBalance = Convert.ToBoolean(WebConfigurationManager.AppSettings["ValidateStockBalance"]);
                    _model.disableComplimentary = Boolean.Parse(WebConfigurationManager.AppSettings["DisableComplimentary"] ?? "false");

                    int paymentTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["PaymentType"]);
                    var paymentType = svcCommon.GetListModuleItem(paymentTypeId);
                    _model.lstPaymentTypes = paymentType.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.name
                    });

                    int paymentReturnTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["PaymentReturnType"]);

                    var paymentReturnType = svcCommon.GetListModuleItem(paymentReturnTypeId);
                    _model.lstPaymentReturnTypes = paymentReturnType.Select(a => new SelectListItem
                    {
                        Text = a.name,
                        Value = a.name
                    });



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
        public virtual JsonResult SaveOrder(OrderModel model)
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
                            Orders entity = new Orders();
                            entity.id = decrypted_Id;
                            //entity.customer_Id = Common.DecryptToID(currentUserId.ToString(), model.customer_Id.ToString());

                            // entity.orderCode = model.orderCode;
                            //entity.shipToAddress_Id = model.shipToAddress_Id ?? 0;
                            //entity.priceTier = model.priceTier;
                            entity.orderDate = model.orderDate;

                            bool isExists = svc.IsOrderExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveOrder(entity);

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
                                return this.Msg_WarningExistRecord("This ship to address record already exists!");

                                //return this.Msg_WarningExistRecord(Resources.MSG_SHIPTOADDRESS_EXIST_RECORD);
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
                var orderId = "";
                if (id == "")
                {
                    orderId = Session["OrderId"] as string;
                    readOnly = true;
                }
                var currentUserId = this.CurrentUserId;
                EditOrderModel _model = new EditOrderModel();
                var _accessRight = Common.GetUserRights(currentUserId, PAGE_URLForAccessRight);

                if (_accessRight != null)
                {
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        if (id == "")
                        {
                            if (orderId != null || orderId != "")
                            {
                                _model = GetOrderDetailModel(Convert.ToInt32(orderId), readOnly);
                            }
                        }
                        else
                        {
                            var decrypted_Id = Common.DecryptToID(currentUserId.ToString(), id);
                            _model = GetOrderDetailModel(decrypted_Id, readOnly);

                        }

                        _model.encryptedId = id;
                        _model.ValidateStockBalance = Convert.ToBoolean(WebConfigurationManager.AppSettings["ValidateStockBalance"]);
                        try
                        {
                            var shipToAddresses = this.svcShipTo.GetShipToAddressByCustomerId(_model.customer_Id, null);
                            _model.shipToAddresses = shipToAddresses;
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.Write(e.Message);
                        }
                        if (_model.orderStatus == "AwaitingApproval")
                        {
                            readOnly = true;
                        }
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, readOnly ? "ViewOrder" : "Create", _model)
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

        [Authorize]
        public virtual JsonResult CloneOrder(string id, bool readOnly)
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

                        var _model = GetOrderDetailModel(decrypted_Id, true);
                        _model.isClone = true;
                        _model.orderStatus = "";
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, "Create", _model)
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
        public virtual JsonResult ReturnOrder(string id, bool readOnly)
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

                        var _model = GetOrderDetailModel(decrypted_Id, readOnly);
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, "ReturnOrder", _model)
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

        private List<SelectListItem> GetCountryList(int id, int selectedItem)
        {

            var lst = svcCommon.GetAddressCountryList(new AddressCountries { filterDropdown_Id = id });

            var result = lst.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.id.ToString(),
                Selected = (a.id == selectedItem)
            });
            return result.ToList();
        }

        [HttpGet]
        public virtual JsonResult GetUsersList()
        {
            var approvalManager_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["User_Role_ApprovalManager_Id"]);
            var lst = svcCommon.GetUsersListByApprovalRole(approvalManager_Id);

            var result = lst.Select(a => new SelectListItem
            {
                Text = a.firstName + " " + a.lastName,
                Value = a.id.ToString()

            });
            ViewBag.GetUsersList = new SelectList(result, "value", "text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public virtual JsonResult CheckBulkDiscountApproval(List<DiscountApprovalRequest> items)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var results = new List<object>();

                    foreach (var item in items)
                    {
                        // Check if the item requires approval
                        var existingApproval = model.OrderItemDiscountApproval.FirstOrDefault(x =>
                            (item.orderId == x.orderId || (item.orderId == 0 && x.orderId == 0)) &&
                            (item.orderItemId == x.orderItem_Id || (item.orderItemId == 0 && x.orderItem_Id == 0)) &&
                            (item.orderItemDiscount == x.discount));

                        bool needsApproval = false;
                        if (existingApproval != null)
                        {
                            needsApproval = existingApproval.discount != item.orderItemDiscount; // Check if the discount is different
                        }
                        else
                        {
                            needsApproval = true; // If no record exists, it needs approval
                        }

                        results.Add(new
                        {
                            orderItemId = item.orderItemId,
                            needsApproval = needsApproval
                        });
                    }

                    return Json(results);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Model for the bulk request
        public class DiscountApprovalRequest
        {
            public int orderId { get; set; }
            public int orderItemId { get; set; }
            public decimal orderItemDiscount { get; set; }
        }


        [HttpPost]
        public virtual JsonResult GetDiscountApprovalByOrder(int? orderId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Fetch the discount approvals for the given orderId
                    var orderItemDiscounts = model.OrderItemDiscountApproval
                        .Where(x => x.orderId == orderId)
                        .OrderByDescending(x => x.approvalDate)
                        .Select(items => new
                        {
                            items.approvalDate,
                            items.discount,
                            items.remarks,
                            ApproverName = model.Users
                                .Where(user => user.id == items.approverUser_Id)
                                .Select(user => user.userName)
                                .FirstOrDefault(),
                            RequestedBy = model.Users
                                .Where(user => user.id == items.requester_Id)
                                .Select(user => user.userName)
                                .FirstOrDefault(),
                            PartNumber = model.OrderItems
                                .Where(orderItem => orderItem.orderId == orderId && orderItem.orderItemId == items.orderItem_Id)
                               .Select(orderItem => new
                               {
                                   orderItem.partNum,
                                   orderItem.lineDesc
                               })
                                .FirstOrDefault() // Assuming a 1-to-1 relationship between discount and order item
                        })
                        .ToList();

                    // Return the data
                    return Json(new { success = true, orderItemDiscounts });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public bool ValidateCredentials(string username, string password)
        {
            try
            {
                // Hash the incoming password
                string hashedPassword = Common.MD5(password) + ConfigurationManager.AppSettings["Encryption"].ToString();
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var userId = Convert.ToInt32(username); // Ensure username is actually an integer
                    var userName = model.Users
                        .Where(p => p.id == userId)
                        .Select(a => a.userName)
                        .FirstOrDefault();
                    var user = model.Users.Include("AssignedOrganizations").SingleOrDefault(p => p.userName == userName && p.passwordHash == hashedPassword);

                    if (user == null)
                    {
                        return false;
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public virtual JsonResult CheckItemReturnStatus(int orderId)
        {
            using (HanodaleEntities model = new HanodaleEntities())
            {
                // Check if any OrderItem has IsItemReturned flag set to true
                var isItemReturned = model.OrderItems
                                          .Where(item => item.orderId == orderId && item.IsItemReturned == true)
                                          .Any(); // This will return a boolean indicating if any item has IsItemReturned == true

                // If there is an item returned, check if there's a matching record in OrderUpdate for Print-Refund action
                if (isItemReturned)
                {
                    // Check if any record exists in OrderUpdate table for Print-Refund action for the given orderId
                    var existsPrintRefundRecord = model.OrderUpdate
                                                        .Any(update => update.order_Id == orderId && update.actionName == "Print-Refund");

                    return Json(new { isItemReturned = isItemReturned, existsPrintRefundRecord = existsPrintRefundRecord }, JsonRequestBehavior.AllowGet);
                }
                var existsPrintRecord = model.OrderUpdate
                                      .Any(update => update.order_Id == orderId && update.actionName == "Print");

                // If no record for Print action exists, log the Print action in the OrderUpdate table
                if (!existsPrintRecord)
                {
                    // Capture the log in the OrderUpdate table for the first print action
                    var orderUpdate = new OrderUpdate
                    {
                        order_Id = orderId,
                        actionName = "Print", // Set the action as Print
                        actionDate = DateTime.Now,
                        user_Id = this.CurrentUserId// Set the current date and time
                                                    // Any other necessary fields can be set here
                    };
                    model.OrderUpdate.Add(orderUpdate);
                    model.SaveChanges(); // Save changes to the database
                }
                else
                {
                    // Capture the log in the OrderUpdate table for the first print action
                    var orderUpdate = new OrderUpdate
                    {
                        order_Id = orderId,
                        actionName = "Copy", // Set the action as Print
                        actionDate = DateTime.Now,
                        user_Id = this.CurrentUserId // Set the current date and time
                                                     // Any other necessary fields can be set here
                    };
                    model.OrderUpdate.Add(orderUpdate);
                    model.SaveChanges(); // Save changes to the database
                }
                // If no item has IsItemReturned set to true, return that status
                return Json(new { isItemReturned = false, existsPrintRefundRecord = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public virtual JsonResult CheckPrintRefundStatus(int orderId)
        {
            using (HanodaleEntities model = new HanodaleEntities())
            {
                var printRefundRecord = model.OrderUpdate
                                              .FirstOrDefault(o => o.order_Id == orderId && o.actionName == "Print-Refund");
                if (printRefundRecord != null)
                {
                    var orderUpdate = new OrderUpdate
                    {
                        order_Id = orderId,
                        actionName = "Refund Copy", // Set the action as Print
                        actionDate = DateTime.Now,
                        user_Id = this.CurrentUserId// Set the current date and time
                                                    // Any other necessary fields can be set here
                    };
                    model.OrderUpdate.Add(orderUpdate);
                    model.SaveChanges(); // Save changes to the database
                }

                else
                {
                    var orderUpdate = new OrderUpdate
                    {
                        order_Id = orderId,
                        actionName = "Print-Refund", // Set the action as Print
                        actionDate = DateTime.Now,
                        user_Id = this.CurrentUserId// Set the current date and time
                                                    // Any other necessary fields can be set here
                    };
                    model.OrderUpdate.Add(orderUpdate);
                    model.SaveChanges(); // Save changes to the database
                }

                return Json(new { isRefundPrinted = printRefundRecord != null }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public virtual JsonResult LogPrintAction(int orderId, string actionType)
        {
            using (HanodaleEntities model = new HanodaleEntities())
            {
                try
                {
                    model.OrderUpdate.Add(new OrderUpdate
                    {
                        order_Id = orderId,
                        actionName = actionType,
                        actionDate = DateTime.Now,
                        user_Id = this.CurrentUserId
                    });

                    model.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }


        [HttpPost]
        public virtual JsonResult ApproveDiscounts(List<OrderItems> orderData, string username, string password, string remarks, string orderId)
        {
            try
            {
                // Fetch user based on username
                var success = ValidateCredentials(username, password);
                OrderItemDiscountApproval saveResult = new OrderItemDiscountApproval();
                if (success == false)
                {
                    return Json(new { success = false, message = "Invalid Credentials!" });
                }
                if (orderData != null)
                {
                    using (HanodaleEntities model = new HanodaleEntities())
                    {

                        var orderUpdate = new OrderUpdate
                        {
                            actionName = "Discount Approval", // Set the action as Print
                            actionDate = DateTime.Now,
                            user_Id = Convert.ToInt32(username),
                            orderStatus = "Discount Approval"
                            // Any other necessary fields can be set here
                        };
                        model.OrderUpdate.Add(orderUpdate);
                        model.SaveChanges(); // Save changes to the database
                    }
                    foreach (var data in orderData)
                    {
                        if (data.discountAmt > 0)
                        {
                            // Insert a record into the OrderItemDiscountApproval table
                            var approval = new OrderItemDiscountApproval
                            {

                                orderItem_Id = data.orderItemId ?? 0,
                                orderId = Convert.ToInt32(data.orderId),
                                approverUser_Id = Convert.ToInt32(username),
                                approvalDate = DateTime.Now,
                                discount = (decimal)data.discountAmt,
                                requester_Id = this.CurrentUserId,
                                remarks = remarks
                            };
                            // Save approval record to database
                            saveResult = svc.OrderItemDiscountApproval(approval);
                        }
                    }

                }

                if (saveResult == null)
                {
                    return Json(new { success = false, message = "Failed to save discount approval!" });
                }

                else
                {
                    return Json(new { success = true, message = "Discounts approved successfully!" });
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                // Return error if exception is thrown
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost]
        public virtual JsonResult LoadCountryListItem(int id)
        {
            var result = GetCountryList(id, 0);
            return Json(result);
        }

        private List<SelectListItem> GetStateList(int id, int selectedItem)
        {

            var lst = svcCommon.GetAddressStateList(new AddressStates { filterDropdown_Id = id });

            var result = lst.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.id.ToString(),
                Selected = (a.id == selectedItem)
            });
            return result.ToList();
        }

        [HttpPost]
        public virtual JsonResult LoadStateListItem(int id)
        {
            var result = GetStateList(id, 0);
            return Json(result);
        }


        private List<SelectListItem> GetCityList(int id, int selectedItem)
        {

            var lst = svcCommon.GetAddressCityList(new AddressCities { filterDropdown_Id = id });

            var result = lst.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.id.ToString(),
                Selected = (a.id == selectedItem)
            });
            return result.ToList();
        }

        [HttpPost]
        public virtual JsonResult LoadCityListItem(int id)
        {
            var result = GetCityList(id, 0);
            return Json(result);
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
                            bool isSuccess = svc.DeleteOrder(decrypted_Id);

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

        #region cart functions 
        [HttpPost]
        [Authorize]
        //public virtual JsonResult SubmitOrder(SubmitOrderModel data)
        //{
        //    try
        //    {
        //        // System.Diagnostics.Debug.WriteLine(data);
        //        var entity = new SubmitOrder();
        //        var order = new Order();
        //        entity.customer_Id = data.customer_Id == 0 ? 11454 : data.customer_Id;
        //        entity.orderComment = data.orderComment;
        //        entity.orderStatus = data.orderStatus;

        //        entity.shipToAddress_Id = data.shipToAddress_Id;
        //        entity.districtId = data.districtId;
        //        entity.orderDate = data.orderDate ?? DateTime.Now;
        //        entity.orderComment = data.orderComment;

        //        // Initialize the OrderItems collection
        //        entity.OrderItems = new List<SubmitOrderItem>();

        //        entity.oneTimeCustomer = data.oneTimeCustomer;
        //        entity.orderContact = data.orderContact;
        //        entity.orderContactName = data.orderContactName;
        //        entity.orderContactPhone = data.orderContactPhone;

        //        // Mapping the order items
        //        //if (data.OrderItems != null)
        //        //{
        //        //    foreach (var itemDto in data.OrderItems)
        //        //    {
        //        //        var orderItem = new SubmitOrderItem
        //        //        {
        //        //            product_Id = itemDto.product_Id,
        //        //            partNum = itemDto.partNum,
        //        //            lineDesc = itemDto.lineDesc,
        //        //            ium = itemDto.ium,
        //        //            salesUm = itemDto.salesUm,
        //        //            unitPrice = itemDto.unitPrice,
        //        //            orderQty = itemDto.orderQty,
        //        //            discount = itemDto.discount,
        //        //            listPrice = itemDto.listPrice,
        //        //            returnTotal = itemDto.returnTotal,
        //        //            QtyType_ModuleItem_Id = itemDto.QtyType_ModuleItem_Id,
        //        //            OrderUOM_Id = itemDto.OrderUOM_Id,
        //        //            operationStyle_ModuleItem_Id = itemDto.operationStyle_ModuleItem_Id,
        //        //            operationCost = itemDto.operationCost,
        //        //            actualOperationCost = itemDto.actualOperationCost,
        //        //            complimentary_ModuleItem_Id = itemDto.complimentary_ModuleItem_Id,
        //        //            orderId = itemDto.orderId,
        //        //            allowVaryWeight = itemDto.allowVaryWeight,
        //        //            originalUnitPrice = itemDto.originalUnitPrice,
        //        //            realOriginalUnitPrice = itemDto.realOriginalUnitPrice,
        //        //            comments = itemDto.comments,
        //        //            scannedLabel = itemDto.scannedLabel,
        //        //            discountAmt = itemDto.discountAmt,
        //        //            discountPer = itemDto.discountPer
        //        //        };

        //        //        entity.OrderItems.Add(orderItem);
        //        //    }
        //        //}

        //        //  Grouping and Mapping the Order Items properly
        //        if (data.OrderItems != null)
        //        {
        //            var groupedItems = data.OrderItems
        //                .GroupBy(item => item.partNum)
        //                .Select(group => new SubmitOrderItem
        //                {
        //                    product_Id = group.First().product_Id,
        //                    partNum = group.Key,
        //                    lineDesc = group.First().lineDesc,
        //                    ium = group.First().ium,
        //                    salesUm = group.First().salesUm,
        //                    unitPrice = group.First().unitPrice,
        //                    orderQty = group.Sum(x => x.orderQty), 
        //                    discount = group.First().discount,
        //                    listPrice = group.First().listPrice,
        //                    returnTotal = group.First().returnTotal,
        //                    QtyType_ModuleItem_Id = group.First().QtyType_ModuleItem_Id,
        //                    OrderUOM_Id = group.First().OrderUOM_Id,
        //                    operationStyle_ModuleItem_Id = group.First().operationStyle_ModuleItem_Id,
        //                    operationCost = group.First().operationCost,
        //                    actualOperationCost = group.First().actualOperationCost,
        //                    complimentary_ModuleItem_Id = group.First().complimentary_ModuleItem_Id,
        //                    orderId = group.First().orderId,
        //                    allowVaryWeight = group.First().allowVaryWeight,
        //                    originalUnitPrice = group.First().originalUnitPrice,
        //                    realOriginalUnitPrice = group.First().realOriginalUnitPrice,
        //                    comments = group.First().comments,
        //                    scannedLabel = group.First().scannedLabel,
        //                    discountAmt = group.First().discountAmt,
        //                    discountPer = group.First().discountPer
        //                }).ToList();

        //            entity.OrderItems.AddRange(groupedItems);
        //        }

        //        order = this.svc.SubmitOrder(entity, this.CurrentUserId);

        //        return Json(new
        //        {
        //            success = true,
        //            orderId = order.id,  // Assuming the order has an Id field
        //            orderDetails = new
        //            {

        //                orderItems = order.OrderItems.Select(i => new
        //                {
        //                    i.id,
        //                    i.discountAmt
        //                }).ToList()
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);

        //        return Json(new
        //        {
        //            success = false,
        //            message = "An error occurred while processing the order. Please try again."
        //        });
        //    }
        //}
        public virtual JsonResult SubmitOrder(SubmitOrderModel data)
        {
            try
            {
                var entity = new SubmitOrder();
                entity.customer_Id = data.customer_Id == 0 ? 11454 : data.customer_Id;
                entity.orderComment = data.orderComment;
                entity.orderStatus = data.orderStatus;
                entity.shipToAddress_Id = data.shipToAddress_Id;
                entity.districtId = data.districtId;
                entity.orderDate = data.orderDate ?? DateTime.Now;
                entity.oneTimeCustomer = data.oneTimeCustomer;
                entity.orderContact = data.orderContact;
                entity.orderContactName = data.orderContactName;
                entity.orderContactPhone = data.orderContactPhone;

                entity.OrderItems = new List<SubmitOrderItem>();

                if (data.OrderItems != null && data.OrderItems.Any())
                {
                    foreach (var itemDto in data.OrderItems)
                    {
                        //if (itemDto.QtyType_ModuleItem_Id == 7705 && !itemDto.allowVaryWeight)
                        //{
                        //    // If the QtyType_ModuleItem_Id is "7704" is Carton , Nothing to do here 
                        //    // If the QtyType_ModuleItem_Id is "7705" is Loose and VaryWg is false (std loose) then set the scannedQty to orderQty
                        //    itemDto.scannedQty = itemDto.orderQty;
                        //}
                        //if (!string.IsNullOrEmpty(itemDto.scannedLabel) && itemDto.allowVaryWeight)
                        //{
                        //    // If the it is VaryWg true and labelled scanned then set the scannedQty to orderQty
                        //    itemDto.scannedQty = itemDto.orderQty;
                        //    itemDto.QtyType_ModuleItem_Id = 7705; // Set to Loose if scanned label is present and allowVaryWeight is true
                        //}
                        //else if (string.IsNullOrEmpty(itemDto.scannedLabel) && itemDto.allowVaryWeight)
                        //{
                        //    itemDto.QtyType_ModuleItem_Id = 7704; // This full qty - setup auto
                        //    //Normally ScanQty, this will update at Pickup Scanning, no need here
                        //}
                        
                        // to remove the leading and trailing spaces from scannedLabel
                        if (!string.IsNullOrEmpty(itemDto.scannedLabel))
                            itemDto.scannedLabel = itemDto.scannedLabel.Trim();

                        // Scanned label means LooseBarcode scanned, so update scannedqty here
                        // If user selected Loose Quantity as OrderType then update scannedQty here
                        if (!string.IsNullOrEmpty(itemDto.scannedLabel) || itemDto.QtyType_ModuleItem_Id == 7705)
                            itemDto.scannedQty = itemDto.orderQty;
                        else if (string.IsNullOrWhiteSpace(itemDto.scannedLabel) && itemDto.QtyType_ModuleItem_Id == 7704)
                            itemDto.scannedQty = 0; // This is the Std Scenario - leave for // ScannedQty to be updated at Pickup Scanning

                        var orderItem = new SubmitOrderItem
                        {
                            product_Id = itemDto.product_Id,
                            partNum = itemDto.partNum,
                            lineDesc = itemDto.lineDesc,
                            ium = itemDto.ium,
                            salesUm = itemDto.salesUm,
                            unitPrice = itemDto.unitPrice,
                            orderQty = itemDto.orderQty,
                            scannedQty = itemDto.scannedQty,
                            discount = itemDto.discount,
                            listPrice = itemDto.listPrice,
                            returnTotal = itemDto.returnTotal,
                            QtyType_ModuleItem_Id = itemDto.QtyType_ModuleItem_Id,
                            OrderUOM_Id = itemDto.OrderUOM_Id,
                            operationStyle_ModuleItem_Id = itemDto.operationStyle_ModuleItem_Id,
                            operationCost = itemDto.operationCost,
                            actualOperationCost = itemDto.actualOperationCost,
                            complimentary_ModuleItem_Id = itemDto.complimentary_ModuleItem_Id,
                            orderId = itemDto.orderId,
                            allowVaryWeight = itemDto.allowVaryWeight,
                            originalUnitPrice = itemDto.originalUnitPrice,
                            realOriginalUnitPrice = itemDto.realOriginalUnitPrice,
                            comments = itemDto.comments,
                            scannedLabel = itemDto.scannedLabel,
                            scannedLocation = itemDto.scannedLocation,
                            discountAmt = itemDto.discountAmt,
                            discountPer = itemDto.discountPer
                        };

                        entity.OrderItems.Add(orderItem);
                    }
                }
                

                var order = this.svc.SubmitOrder(entity, this.CurrentUserId);

                return Json(new
                {
                    success = true,
                    orderId = order.id,
                    orderDetails = new
                    {
                        orderItems = order.OrderItems.Select(i => new
                        {
                            i.id,
                            i.discountAmt
                        }).ToList()
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing the order. Please try again."
                });
            }
        }


        [HttpPost]
        [Authorize]
        public virtual JsonResult SubmitScannedItems(SubmitOrderScanModel data)
        {
            try
            {
                // Convert the incoming data to a list of ScanOrderItem
                var entity = new SubmitOrderScan();
                entity.action = data.action;
                entity.OrderItems = new List<ScanOrderItem>();
                entity.ScannedItems = new List<ScannedItem>();
                if (data.OrderItems != null)
                {
                    entity.OrderId = data.OrderId;
                    foreach (var item in data.OrderItems)
                    {
                        var scanOrderItem = new ScanOrderItem
                        {
                            itemId = item.itemId,
                            OrderItemId = item.OrderItemId,
                            ScannedQty = item.ScannedQty
                        };
                        entity.OrderItems.Add(scanOrderItem);
                    }
                    foreach (var item in data.ScannedItems)
                    {
                        var scannedItem = new ScannedItem
                        {
                            OrderId = item.OrderId,
                            OrderItemId = item.OrderItemId,
                            ScannedQty = item.ScannedQty,
                            PartNum = item.PartNum,
                            Status = item.Status,
                            SerialNumber = item.SerialNumber,
                            productLocation = item.Location
                        };
                        System.Diagnostics.Debug.WriteLine("LOCATION FROM FRONTEND: " + item.Location);
                        entity.ScannedItems.Add(scannedItem);
                    }
                }

                // Call the service method to update scanned items
                bool result = this.svc.SubmitScannedItems(entity, this.CurrentUserId, data.IsVerification);

                return Json(new
                {
                    success = true,
                    message = data.action == "submit" ? "Scanned items submitted successfully." : "Scanned items saved successfully."
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing the scanned items. Please try again."
                });
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult SubmitReturnedItems(SubmitReturnedModel data)
        {
            try
            {
                // Convert the incoming data to a list of ScanOrderItem
                var entity = new SubmitReturnItems();
                entity.orderTotal = data.orderTotal;
                entity.ReturnedItems = new List<ScannedItem>();
                entity.ReturnOrderItems = new List<ReturnOrderItems>();
                if (data.ReturnedItems != null)
                {
                    entity.OrderId = data.OrderId;

                    foreach (var item in data.ReturnedItems)
                    {
                        var scannedItem = new ScannedItem
                        {
                            OrderId = item.OrderId,
                            scannedId = item.scannedId,
                            OrderItemId = item.OrderItemId,
                            ScannedQty = item.ScannedQty,
                            returnQty = item.returnQty,
                            PartNum = item.PartNum,
                            Status = item.Status,
                            SerialNumber = item.SerialNumber,
                        };
                        entity.ReturnedItems.Add(scannedItem);
                    }

                    foreach (var item in data.ReturnOrderItems)
                    {
                        entity.ReturnOrderItems.Add(new ReturnOrderItems
                        {
                            OrderItemId = item.OrderItemId,
                            cuttingCost = item.cuttingCost,
                            discountAmt = item.discountAmt,
                            listPrice = item.listPrice,
                            returnTotal = item.returnTotal,
                        });
                    }
                }

                // Call the service method to update scanned items
                bool result = this.svc.SubmitReturnedItems(entity, this.CurrentUserId);

                return Json(new
                {
                    success = true,
                    message = "Returned items submitted successfully."
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing the return order items. Please try again."
                });
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult SubmitOrderScanned(string serialNo, int orderItem_Id, decimal scannedQty)
        {
            try
            {
                // System.Diagnostics.Debug.WriteLine(data);
                var entity = new OrderScanned();
                entity.orderItem_Id = orderItem_Id;
                entity.serialNo = serialNo;
                entity.scannedQty = scannedQty;



                bool result = this.svc.OrderScanned(entity);

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing the order. Please try again."
                });
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult UpdateOrder(UpdateOrderModel data)
        {
            try
            {
                // System.Diagnostics.Debug.WriteLine(data);
                var entity = new UpdateOrder();
                entity.RemovedOrderItemIds = data.RemovedOrderItemIds;
                entity.id = data.id;
                entity.customer_Id = data.customer_Id == 0 ? 11454 : data.customer_Id;
                entity.orderComment = data.orderComment;
                entity.orderStatus = (data.orderStatus == "SubmitForApproval") ? "AwaitingApproval" : data.orderStatus;

                entity.shipToAddress_Id = data.shipToAddress_Id;
                entity.districtId = data.districtId;
                entity.orderDate = data.orderDate ?? DateTime.Now;
                entity.orderComment = data.orderComment;

                // Initialize the OrderItems collection
                entity.OrderItems = new List<SubmitOrderItem>();

                entity.oneTimeCustomer = data.oneTimeCustomer;
                entity.orderContact = data.orderContact;
                entity.orderContactName = data.orderContactName;
                entity.orderContactPhone = data.orderContactPhone;

                // Mapping the order items
                if (data.OrderItems != null)
                {
                    foreach (var itemDto in data.OrderItems)
                    {
                        if (string.IsNullOrEmpty(itemDto.partNum))
                            continue;
                        //if (itemDto.QtyType_ModuleItem_Id == 7705 && !itemDto.allowVaryWeight)
                        //{
                        //    // If the QtyType_ModuleItem_Id is "7704" is Carton , Nothing to do here 
                        //    // If the QtyType_ModuleItem_Id is "7705" is Loose and VaryWg is false (std loose) then set the scannedQty to orderQty
                        //    itemDto.scannedQty = itemDto.orderQty;
                        //}
                        //if (!string.IsNullOrEmpty(itemDto.scannedLabel) && itemDto.allowVaryWeight)
                        //{

                        //    // If the it is VaryWg true and labelled scanned then set the scannedQty to orderQty
                        //    itemDto.scannedQty = itemDto.orderQty;
                        //    itemDto.QtyType_ModuleItem_Id = 7705; // Set to Loose if scanned label is present and allowVaryWeight is true
                        //}
                        //else if (string.IsNullOrEmpty(itemDto.scannedLabel) && itemDto.allowVaryWeight)
                        //{
                        //    itemDto.QtyType_ModuleItem_Id = 7704; // This full qty - setup auto
                        //    itemDto.scannedQty = itemDto.orderQty;
                        //}
                        // to remove the leading and trailing spaces from scannedLabel
                        if (!string.IsNullOrEmpty(itemDto.scannedLabel))
                            itemDto.scannedLabel = itemDto.scannedLabel.Trim();

                        // Scanned label means LooseBarcode scanned, so update scannedqty here
                        // If user selected Loose Quantity as OrderType then update scannedQty here
                        if (!string.IsNullOrEmpty(itemDto.scannedLabel) || itemDto.QtyType_ModuleItem_Id == 7705)
                            itemDto.scannedQty = itemDto.orderQty;
                        else if (string.IsNullOrWhiteSpace(itemDto.scannedLabel) && itemDto.QtyType_ModuleItem_Id == 7704)
                            itemDto.scannedQty = 0; // This is the Std Scenario - leave for // ScannedQty to be updated at Pickup Scanning

                        var orderItem = new SubmitOrderItem
                            {
                                product_Id = itemDto.product_Id,
                                partNum = itemDto.partNum,
                                lineDesc = itemDto.lineDesc,
                                ium = itemDto.ium,
                                salesUm = itemDto.salesUm,
                                unitPrice = itemDto.unitPrice,
                                orderQty = itemDto.orderQty,
                                scannedQty = itemDto.scannedQty,
                                discount = itemDto.discount,
                                listPrice = itemDto.listPrice,
                                QtyType_ModuleItem_Id = itemDto.QtyType_ModuleItem_Id,
                                OrderUOM_Id = itemDto.OrderUOM_Id,
                                operationStyle_ModuleItem_Id = itemDto.operationStyle_ModuleItem_Id,
                                operationCost = itemDto.operationCost,
                                actualOperationCost = itemDto.actualOperationCost,
                                complimentary_ModuleItem_Id = itemDto.complimentary_ModuleItem_Id,
                                orderId = itemDto.orderId,
                                orderItemId = itemDto.orderItemId,
                                originalUnitPrice = itemDto.originalUnitPrice,
                                realOriginalUnitPrice = itemDto.realOriginalUnitPrice,
                                comments = itemDto.comments,
                                scannedLabel = itemDto.scannedLabel,
                                scannedLocation = itemDto.scannedLocation,
                                allowVaryWeight = itemDto.allowVaryWeight,
                                createdBy = itemDto.createdBy,
                                createdAt = itemDto.createdAt,
                                discountAmt = itemDto.discountAmt,
                                discountPer = itemDto.discountPer
                            };

                        entity.OrderItems.Add(orderItem);
                    }
                }
                var order = this.svc.UpdateOrders(entity, this.CurrentUserId);

                return Json(new
                {
                    success = true,
                    orderId = order.id,  // Assuming the order has an Id field
                    orderDetails = new
                    {

                        orderItems = order.OrderItems.Select(i => new
                        {
                            id = i.orderItemId == null ? i.id : i.orderItemId, // Conditional value for orderItemId
                            discountAmt = i.discountAmt
                        }).ToList()
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing the order. Please try again."
                });
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult UpdateOrderStatus(int orderId, string newStatus, string actionName, string remarks)
        {
            try
            {
                bool result = this.svc.UpdateOrderStatus(orderId, newStatus, this.CurrentUserId, actionName, remarks);

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
                System.Diagnostics.Debug.WriteLine(ex);
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating the order status. Please try again."
                });
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult UpdateOrderPayments(UpdateOrderPayments data)
        {
            try
            {
                List<CreateOrderPayment> entities = new List<CreateOrderPayment>();
                int orderId = 0;
                foreach (var payment in data.OrderPayments)
                {
                    orderId = payment.OrderId;
                    CreateOrderPayment entity = new CreateOrderPayment
                    {
                        OrderId = payment.OrderId,
                        PaymentType = payment.PaymentType,
                        Amount = payment.Amount,
                        RefNumber = payment.RefNumber,
                        PaymentDate = payment.PaymentDate,
                        PaymentStatus = payment.PaymentStatus,
                        Bank = payment.Bank,
                        UserId = this.CurrentUserId,
                        Payment = payment.Payment,
                        IsRefund = payment.IsRefund
                    };

                    entities.Add(entity);
                }

                bool result = this.svc.CreateOrderPayment(entities, data.IsRefund);
                if (!data.IsOrderComplete)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Order status updated successfully."
                    });
                }
                //Trigger API call here 
                //var orderApiDto = this.svc.GetOrderApiData(orderId);
                //ApiResponse res = this.syncManager.PostOrderToApi(orderApiDto);
                //bool updateSyncStatus = this.svc.UpdateOrderSyncStatus(orderId, res.OrderResult.OrderNumber.ToString(), res.ShipmentResult.PackNumber.ToString(), res.OrderResult.IsSuccess, res.OrderResult.Message);
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
                System.Diagnostics.Debug.WriteLine(ex);
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating the order status. Please try again."
                });
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult PostOrderToApi(int orderId)
        {
            try
            {
                // Trigger API call here 
                var orderApiDto = this.svc.GetOrderApiData(orderId);
                ApiResponse res = this.syncManager.PostOrderToApi(orderApiDto);
                OrderSyncStatusDto statusDto = new OrderSyncStatusDto();
                statusDto.OrderId = orderId;
                statusDto.EpicoreOrderId = res.OrderResult.OrderNumber != 0 ? res.OrderResult.OrderNumber.ToString() : "";
                statusDto.ShipPackNumber = res.ShipmentResult.PackNumber != 0 ? res.ShipmentResult.PackNumber.ToString() : "";
                statusDto.UD16Key1 = res.PaymentResult.Key1AsCommaSeparated;
                statusDto.EpicoreInvNumber = res.ShipmentResult.InvoiceNumber;
                statusDto.SyncMessage = res.OrderResult.Message;
                statusDto.SyncStatus = res.OrderResult.IsSuccess;
                bool updateSyncStatus = this.svc.UpdateOrderSyncStatus(statusDto);
                if (res.OrderResult.IsSuccess)
                {
                    return Json(new
                    {
                        success = true,
                        syncStatus = res.OrderResult.IsSuccess,
                        syncMessage = res.OrderResult.Message,
                        message = "Order data successfully posted to the API"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Failed to post order data to the API"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return Json(new
                {
                    success = false,
                    message = "An error occurred while posting the data. Please try again."
                });
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Picking(string id)
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
                    var _model = new OrderModel();

                    this.FillupFieldMetadata(_model, false);

                    // _model.customer_Id = id;

                    if (_model.shipToAddress_Id_Metadata.isEditableInCreate)
                    {

                        HanodaleEntities model = new HanodaleEntities();
                        //Get Plant List
                        var query = from p in model.ShipToAddress
                                    select new
                                    {
                                        id = p.id,
                                        shippingCode = p.shippingCode,
                                    };
                        var _shipToAddressList = query.ToList();



                        _model.lstshipToAddress = _shipToAddressList.Select(p => new SelectListItem
                        {
                            Text = p.shippingCode,
                            Value = p.id.ToString(),


                        }).ToList();

                    }



                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    //TryValidateModel(_companyAddressModel);


                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, "Picking", _model)
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

        private EditOrderModel GetOrderDetailModel(int decrypted_Id, bool readOnly)
        {
            var order = this.svc.GetOrderDetails(decrypted_Id);
            //check cashier login
            decimal docOrderAmtDecimal = 0;
            string formattedDocOrderAmt = "";
            // Try to parse the string to decimal
            if (Decimal.TryParse(order.docOrderAmt, out docOrderAmtDecimal))
            {
                // Successfully parsed, now format it
                formattedDocOrderAmt = docOrderAmtDecimal.ToString("N2");
            }
            else
            {
                // Handle the case where parsing fails (optional)
                formattedDocOrderAmt = "Invalid Amount";
            }

            var _model = new EditOrderModel
            {
                id = order.id,
                readOnly = readOnly,
                isEdit = !readOnly,
                customer_Id = order.customer_Id,
                orderNum = order.orderNum,
                shipToAddress_Id = order.shipToAddress_Id,
                districtId = order.districtId,
                shipToName = order.shipToName ?? "",
                orderDate = order.orderDate?.ToString("dd/MM/yyyy hh:mm tt"),
                orderComment = order.orderComment,
                entryPerson = order.entryPerson,
                docOrderAmt = formattedDocOrderAmt,
                customerName = order.customerName,
                orderStatus = order.orderStatus,
                verifyRemarks = order.verifyRemarks,
                verifiedStatus = order.verifiedStatus,
                verifiedDate = order.verifiedDate?.ToString("dd/MM/yyyy hh:mm tt"),
                verifiedBy = order.verifiedBy,
                oneTimeCustomer = order.oneTimeCustomer,
                orderContact = order.orderContact,
                orderContactName = order.orderContactName,
                orderContactPhone = order.orderContactPhone,
                customerAddress = order.customerAddress,
                tinId = order.tinId ?? "",
                shipToAddress = order.shipToAddress ?? "",
                epicoreInvNumber = order.epicoreInvNumber,
                epicoreResponse = order.epicoreResponse,
                epicoreOrderId = order.epicoreOrderId,
                shipPackNumber = order.shipPackNumber,
                UD16Key1 = order.UD16Key1,
                syncedAt = order.syncedAt,
                syncStatus = order.syncStatus,
                disableComplimentary = Boolean.Parse(WebConfigurationManager.AppSettings["DisableComplimentary"] ?? "false"),
                OrderItems = order.OrderItems?.Select(item => new ViewOrderItemModel
                {
                    orderLine = item.orderLine,
                    product_Id = item.product_Id,
                    itemId = item.orderItemId,
                    itemDbId = item.itemId,
                    partNum = item.partNum,
                    description = item.partName,
                    prodGroup = item.prodGroup,
                    lineDesc = item.lineDesc,
                    ium = item.ium,
                    salesUm = item.salesUm,
                    unitPrice = item.unitPrice,
                    orderQty = item.orderQty,
                    discount = item.discount,
                    listPrice = item.listPrice,
                    returnTotal = item.returnTotal,
                    allowVaryWeight = (item.allowVaryWeight ?? false) ? "Yes" : "No",
                    operationCost = item.operationCost,
                    actualOperationCost = item.actualOperationCost,
                    conversionFactor = item.conversionFactor,
                    QtyType_ModuleItem_Id = item.QtyType_ModuleItem_Id,
                    OrderUOM_Id = item.OrderUOM_Id,
                    orderUOM = item.orderUOM,
                    operationStyle_ModuleItem_Id = item.operationStyle_ModuleItem_Id,
                    complimentary_ModuleItem_Id = item.complimentary_ModuleItem_Id,
                    complementary = item.complementary,
                    operationName = item.operationName,
                    originalUnitPrice = item.originalUnitPrice,
                    realOriginalUnitPrice = item.realOriginalUnitPrice,
                    orderItemId = item.orderItemId,
                    scannedQty = item.scannedQty,
                    discountAmt = item.discountAmt,
                    discountPer = item.discountPer,
                    IsReturned = item.IsReturned,
                    scanQtyStr = item.scannedQtyStr,
                    comments = item.comments,
                    avlQty = item.avlQty,
                    scannedLabel = item.scannedLabel,
                    scannedLocation = item.scannedLocation,
                    itemBrandName = item.itemBrandName ?? "",
                    country = item.country,
                    createdBy = item.createdBy,
                    createdAt = item.createdAt,
                    createdById = item.createdById,
                    retQty = Math.Round(order.OrderScanned?
                            .Where(p => p.IsReturned && p.orderItem_Id == item.orderItemId)
                            .Sum(p => p.returnQty) ?? 0, 2, MidpointRounding.AwayFromZero),
                }).ToList() ?? new List<ViewOrderItemModel>(),
                OrderItemDeleted = order.OrderItemDeleted?.Select(item => new ViewOrderItemDeletedModel
                {
                    product_Id = item.product_Id,
                    itemId = item.orderItemId,
                    itemDbId = item.itemId,
                    partNum = item.partNum,
                    description = item.partName,
                    prodGroup = item.prodGroup,
                    lineDesc = item.lineDesc,
                    ium = item.ium,
                    salesUm = item.salesUm,
                    unitPrice = item.unitPrice,
                    orderQty = item.orderQty,
                    discount = item.discount,
                    listPrice = item.listPrice,
                    allowVaryWeight = (item.allowVaryWeight ?? false) ? "Yes" : "No",
                    operationCost = item.operationCost,
                    actualOperationCost = item.actualOperationCost,
                    conversionFactor = item.conversionFactor,
                    QtyType_ModuleItem_Id = item.QtyType_ModuleItem_Id,
                    OrderUOM_Id = item.OrderUOM_Id,
                    orderUOM = item.orderUOM,
                    operationStyle_ModuleItem_Id = item.operationStyle_ModuleItem_Id,
                    complimentary_ModuleItem_Id = item.complimentary_ModuleItem_Id,
                    complementary = item.complementary,
                    operationName = item.operationName,
                    originalUnitPrice = item.originalUnitPrice,
                    realOriginalUnitPrice = item.realOriginalUnitPrice,
                    orderItemId = item.orderItemId,
                    scannedQty = item.scannedQty,
                    discountAmt = item.discountAmt,
                    discountPer = item.discountPer,
                    IsReturned = item.IsReturned,
                    scanQtyStr = item.scannedQtyStr,
                    comments = item.comments,
                    avlQty = item.avlQty,
                    scannedLabel = item.scannedLabel,
                    itemBrandName = item.itemBrandName ?? "",
                    country = item.country,
                    retQty = Math.Round(order.OrderScanned?
                            .Where(p => p.IsReturned && p.orderItem_Id == item.orderItemId)
                            .Sum(p => p.returnQty) ?? 0, 2, MidpointRounding.AwayFromZero),
                    deletedBy = item.deletedBy,
                    deletedAt = item.deletedAt
                }).ToList() ?? new List<ViewOrderItemDeletedModel>(),
                OrderPayments = order.OrderPayments?.Select(item => new OrderPaymentModel
                {
                    Id = item.Id,
                    Bank = item.Bank ?? "",
                    RefNumber = item.RefNumber ?? "",
                    PaymentDate = item.PaymentDate,
                    PaymentStatus = item.PaymentStatus,
                    PaymentType = item.PaymentType,
                    UserId = item.UserId,
                    Amount = item.Amount,
                    IsRefund = item.IsRefund,
                }).ToList() ?? new List<OrderPaymentModel>(),
                OrderScanned = order.OrderScanned?.Select(item => new OrderScannedModel
                {
                    Id = item.Id,
                    serialNo = item.serialNo,
                    orderItem_Id = (int)item.orderItem_Id,
                    scannedQty = item.scannedQty,
                    status = item.status,
                    Group = item.Group,
                    partNo = item.partNo,
                    partName = item.partName,
                    orderUOM = item.orderUOM,
                    IsReturned = item.IsReturned,
                    returnQty = item.returnQty,
                    allowVaryWeight = item.allowVaryWeight,
                }).ToList() ?? new List<OrderScannedModel>(),
                //ApproverName = (order.orderApprovals!=null)?order.orderApprovals.ApprovalBy:"",
                //ApproverDate= (order.orderApprovals != null) ? order.orderApprovals.approvedDate :null,
                //ApprovalRemarks= (order.orderApprovals != null) ? order.orderApprovals.Remarks : "",
                creditHold = order.creditHold

            };

            //check cashier login
            var _user = this.svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
            if (_user != null)
            {
                _model.IsCashierLogin = _user.roleName.ToLower().Contains("cashier") ? true : false;
                _model.IsSuperAdminLogin = _user.userName.ToLower().Contains("super admin") ? true : false;
            }

            int paymentTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["PaymentType"]);
            var paymentType = svcCommon.GetListModuleItem(paymentTypeId);
            _model.lstPaymentTypes = paymentType.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.name
            });

            int paymentReturnTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["PaymentReturnType"]);

            var paymentReturnType = svcCommon.GetListModuleItem(paymentReturnTypeId);
            _model.lstPaymentReturnTypes = paymentReturnType.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.name
            });

            return _model;
        }

        #endregion
        [HttpPost]
        [Authorize]
        public virtual JsonResult GetOrderLog(int orderId)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var OrderLogList = this.svc.GetOrderLog(orderId);
                        if (OrderLogList == null || !OrderLogList.Any())
                        {
                            return this.Msg_ErrorInRetriveData();  // Handle case when no data is found
                        }

                        return Json(new
                        {
                            success = true,
                            OrderLogList = OrderLogList
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


        #region Sync service 
        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncAllOrders()
        {
            try
            {
                var success = this.syncManager.SyncAllOrdersToEpicore();
                return Json(new
                {
                    success = success,
                    message = success ? "All orders synced with Epicore server successfully." : "Failed to post order data to Epicore"
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

        #region Printing functions

        public virtual ActionResult PrintDocument(string id, string letterName, string watermark)
        {
            Dictionary<string, object> _dictObj = new Dictionary<string, object>();
            Dictionary<string, string> _dictVal = new Dictionary<string, string>();
            //var model = new DocumentModel();

            // Path to the template file
            var _filePath = Server.MapPath("~/Content/Templates/");
            string templatePath = _filePath + letterName + ".docx";
            string _status = "";

            // Ensure the file exists
            if (!System.IO.File.Exists(templatePath))
            {
                return HttpNotFound(letterName + " Template file not found!");
            }

            if (svc != null)
            {
                int decrypted_Id = 0;
                if (id.ToString().Length > 10)
                {
                    decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                }
                else
                {

                    decrypted_Id = Convert.ToInt32(id);
                }

                if (decrypted_Id == 0)
                {
                    return HttpNotFound("Please Complete the Order!");
                }
                var order = svc.GetOrderDetails(decrypted_Id);
                if (order != null)
                {
                    _status = order.orderStatus;
                    var _customer = svcCustomer.GetCustomerById(order.customer_Id);
                    var _shipTo = svcShipTo.GetShipToAddressById(order.shipToAddress_Id ?? 0);
                    _dictObj.Add("Order", order);
                    _dictObj.Add("Customer", _customer);
                    _dictObj.Add("ShipTo", _shipTo);
                    _dictVal.Add("$OrderDate$", order.orderDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                }
                else
                {
                    return HttpNotFound("Order Not Exist!");
                }
            }

            _dictVal.Add("$Plant_Name$", "Lucky Frozen Sdn Bhd");
            _dictVal.Add("$Plant_Address$", "No. 1, Jalan 1/57B, Off Jalan," + "\n"
                   + "Kawasan Perusahaan" + "\n"
                   + "Kuala Lumpur Wilayah" + "\n" + "MALAYSIA");
            _dictVal.Add("$Phone$", "60362568688");
            _dictVal.Add("$Fax$", "60362519336");
            _dictVal.Add("$InvNo$", "");

            var _outputStream = new MemoryStream();
            FileStreamResult file = new FileStreamResult(_outputStream, "application/pdf");
            switch (letterName)
            {
                case "pfi":
                    file = Hanodale.Utility.WebHelper.GeneratePFI(_filePath, letterName + ".docx", _dictObj, _dictVal);
                    break;

                case "order":
                    file = Hanodale.Utility.WebHelper.GenerateOrder(_filePath, letterName + ".docx", _dictObj, _dictVal);
                    break;

                case "receipt":
                    file = Hanodale.Utility.WebHelper.GenerateReceipt(_filePath, letterName + ".docx", _dictObj, _dictVal);
                    break;

                case "invoice":
                    if (_status == "Completed")
                    {
                        _dictVal.Add("$INVOICE_HEADER$", "INVOICE");
                    }
                    else
                    {
                        _dictVal.Add("$INVOICE_HEADER$", "PROFORMA INVOICE");
                    }
                    file = Hanodale.Utility.WebHelper.GenerateInvoice(_filePath, letterName + ".docx", _dictObj, _dictVal, watermark);
                    break;

                default:
                    break;
            }


            Response.Clear();
            MemoryStream ms = new MemoryStream();
            file.FileStream.CopyTo(ms);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "inline;filename=" + letterName + ".pdf");
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
            return View("");

        }


        #endregion

        [AppAuthorize]
        public virtual ActionResult GetCustomSearchPanel()
        {
            var _model = new EditOrderModel
            {
                orderDateTo = DateTime.Today, // Set "Order Date (To)" default to current date
                orderDateFrom = DateTime.Today.AddDays(-7), // "Order Date (From)" default to current date - 7
                orderNum = string.Empty,
                customerName = string.Empty,
                createdBy = string.Empty
            };
            return PartialView(MVC.Orders.Views._OrderSearchPanel, _model);
        }
    }
}
