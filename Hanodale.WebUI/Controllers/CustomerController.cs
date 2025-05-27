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
using System.Threading.Tasks;
using System.Globalization;

namespace Hanodale.WebUI.Controllers
{
    public partial class CustomerController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string ORDERS_PAGE_URL = string.Empty;
        #endregion

        #region Constructor

        private readonly ICustomerService svc;
        private readonly ISyncManager syncManager;
        public CustomerController(ICustomerService _bLService, ICommonService _svcCommon, ISyncManager syncManager)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "Customer";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Customer_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            ORDERS_PAGE_URL = "Orders/Index";
            this.syncManager = syncManager;
        }
        #endregion

        #region Customer Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new CustomerModel());

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
        public virtual JsonResult Customer()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new CustomerModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Customer.Views.Index, _model)
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
        public virtual ActionResult BindCustomer(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            List<Customers> filteredCustomers = null;
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

                        var filterEntity = new Customers();
                        var idFilter0 = Request["sSearch_0"].Trim(); //datefrom
                        var idFilter1 = Request["sSearch_1"].Trim(); // dateto
                        var idFilter2 = Convert.ToString(Request["sSearch_2"]).Trim(); ; //OrderCode
                        var idFilter3 = Convert.ToString(Request["sSearch_3"]).Trim(); //orderStatus
                        var idFilter4 = Convert.ToString(Request["sSearch_4"]).Trim();//cust Name
                        var idFilter5 = Convert.ToString(Request["sSearch_5"]).Trim();//cust code
                        var idFilter6 = Convert.ToString(Request["sSearch_6"]).Trim();//city
                        var idFilter7 = Convert.ToString(Request["sSearch_7"]).Trim();//state
                        if (!string.IsNullOrEmpty(idFilter0))
                        {
                            filterEntity.searchOrderDateFrom = DateTime.ParseExact(idFilter0, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1);
                        }
                        if (!string.IsNullOrEmpty(idFilter1))
                        {
                            filterEntity.searchOrderDateTo = DateTime.ParseExact(idFilter1, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1);
                        }
                        if (!string.IsNullOrEmpty(idFilter2))
                        {
                            filterEntity.searchOrderCode = Convert.ToString(idFilter2);
                        }
                        if (!string.IsNullOrEmpty(idFilter3))
                        {
                            filterEntity.searchOrderStatus = Convert.ToString(idFilter3);
                        }

                        if (!string.IsNullOrEmpty(idFilter4))
                        {
                            filterEntity.searchName = Convert.ToString(idFilter4);
                        }
                        if (!string.IsNullOrEmpty(idFilter5))
                        {
                            filterEntity.searchCode = Convert.ToString(idFilter5);
                        }
                        if (!string.IsNullOrEmpty(idFilter6))
                        {
                            filterEntity.searchCity = Convert.ToString(idFilter6);
                        }
                        if (!string.IsNullOrEmpty(idFilter7))
                        {
                            filterEntity.searchState = Convert.ToString(idFilter7);

                        }
                        var filter = new DatatableFilters { currentUserId = this.CurrentUserId, all = true, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };

                        var CustomerModel = this.svc.GetCustomer(filter, filterEntity);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            //Sorting

                            filteredCustomers = CustomerModel.lstCustomer;
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

                                filteredCustomers = filteredCustomers.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = CustomerData(filteredCustomers, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = CustomerModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = CustomerModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> CustomerData(List<Customers> CustomerEntry, int currentUserId)
        {
            var result = this.GetDatatableData<Customers>(CustomerEntry, currentUserId);
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.Customer.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception ex)
            {
                //throw new ErrorException(err.Message);
                return Msg_ErrorInRetriveData(ex);
            }
        }
        [HttpPost]
        [Authorize]
        public virtual JsonResult Maintenance(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            var _model = new CustomerMaintenanceModel();

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
                        _model.Customer = GetCustomerModel(id, readOnly);

                        if (_model.Customer == null)
                        {
                            return this.Msg_ErrorInService();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Customer.Views.Maintenance, _model)
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
                    var _model = new CustomerModel();

                    this.FillupFieldMetadata(_model, false);



                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.Customer.Views.Create, _model)
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
        public virtual JsonResult SaveCustomer(CustomerModel model)
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
                            Customers entity = new Customers();
                            entity.id = decrypted_Id;
                            entity.code = model.code;
                            entity.name = model.name;
                            entity.address1 = model.address1;
                            entity.address2 = model.address2;


                            bool isExists = svc.IsCustomerExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveCustomer(entity);

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

        private CustomerModel GetCustomerModel(string id, bool readOnly)
        {
            try
            {
                CustomerModel _model = new CustomerModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;
                Customers Customer = new Customers();

                if (Session["CustomerId"] != null && long.TryParse(Session["CustomerId"].ToString(), out long parsedId))
                {
                    Customer = svc.GetCustomerById(Convert.ToInt32(id));
                }
                else
                {
                    var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    Customer = svc.GetCustomerById(decrypted_Id);

                }
                if (Customer != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    _model.code = Customer.code;
                    _model.name = Customer.name;
                    _model.address1 = Customer.address1;
                    _model.address2 = Customer.address2;
                    _model.groupCode = Customer.groupCode;
                    _model.custID = Customer.custID;


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
            var _model = new CustomerMaintenanceModel();

            try
            {
                // Handle empty ID and retrieve it from the session
                if (string.IsNullOrEmpty(id))
                {
                    if (Session["CustomerId"] != null && long.TryParse(Session["CustomerId"].ToString(), out long parsedId))
                    {
                        id = parsedId.ToString();
                        readOnly = true;

                        _model.record_Id = id;
                        _model.Customer = GetCustomerModel(id, readOnly);

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Customer.Views.Create, _model.Customer)
                        });
                    }
                    else
                    {
                        return Json(new { error = "CustomerId is missing or invalid in session." });
                    }
                }

                // Check user rights
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        _model.record_Id = id;
                        _model.readOnly = readOnly;

                        _model.tableProfile = this.GetTableProfileWithTab();
                        _model.Customer = GetCustomerModel(id, readOnly);

                        if (_model.Customer == null)
                        {
                            return this.Msg_ErrorInService();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Customer.Views.Maintenance, _model)
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
                // Graceful error handling
                return Msg_ErrorInRetriveData(ex);
            }
        }


        [HttpPost]
        [Authorize]
        public virtual JsonResult View(string id, bool readOnly)
        {

            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int decrypted_Id = 0;
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        var _model = GetCustomerModel(id, readOnly);

                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Customer.Views.Create, _model)
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
        [AppAuthorize]
        public virtual ActionResult GetCustomSearchPanel()
        {
            var _model = new CustomerModel();

            List<string> status = new List<string>();
            status.Add("");
            status.Add("Pending");
            status.Add("Payment");
            status.Add("Picked");
            status.Add("PickupAccepted");
            status.Add("Completed");
            status.Add("Cancelled");
            status.Add("SubmitForPicking");
            var statusList = status
                .Select(stat => new { text = stat, value = stat });

            ViewBag.status = new SelectList(statusList, "value", "text");



            return PartialView(MVC.Customer.Views._SearchPanel, _model);
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
                            var result = svc.DeleteCustomer(decrypted_Id);

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

        #region search customers 
        [HttpPost]
        [Authorize]
        public virtual JsonResult SearchCustomers(string searchParam)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canAdd || _accessRight.canEdit)
                    {
                        var _customers = this.svc.GetCustomerList(searchParam);
                        if (_customers == null || !_customers.Any())
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            customers = _customers
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
        public virtual JsonResult SearchCustomerList(string searchName, string searchCustomer, string searchCity, string searchState)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canAdd || _accessRight.canEdit)
                    {
                        var _customers = this.svc.GetCustomerList(searchName, searchCustomer, searchCity, searchState);
                        if (_customers == null || !_customers.Any())
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            customers = _customers
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

        #region
        [HttpPost]
        [Authorize]
        public virtual JsonResult SearchDistricts(string searchParam)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, ORDERS_PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canAdd || _accessRight.canEdit)
                    {
                        var _districts = this.svc.GetDistrictList(searchParam);
                        if (_districts == null || !_districts.Any())
                        {
                            return this.Msg_ErrorInRetriveData();
                        }
                        return Json(new
                        {
                            success = true,
                            districts = _districts
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

        #region Sync service 
        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncCustomers()
        {
            try
            {
                // var success = this.syncManager.syncEntity("PriceListPart", "pricelistparts", "listCode", false);
                var success4 = this.syncManager.syncEntity("Customer", "", "code", false, "");
                return Json(new
                {
                    success = success4.Result,
                    message = success4.Message
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

        public virtual ActionResult SyncCustomersPriceList()
        {
            try
            {
                // var success = this.syncManager.syncEntity("PriceListPart", "pricelistparts", "listCode", false);
                var success4 = this.syncManager.syncEntity("CustomerGroupPriceList", "CustomerGroupPriceList", "sysRowID", false, "");
                return Json(new
                {
                    success = success4.Result,
                    message = success4.Message
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

        // function to sync Districts
        public virtual ActionResult SyncDistrictsList()
        {
            try
            {
                // var success = this.syncManager.syncEntity("PriceListPart", "pricelistparts", "listCode", false);
                var success4 = this.syncManager.syncEntity("District", "districts", "sysRowID", false, "");
                return Json(new
                {
                    success = success4.Result,
                    message = success4.Message
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

    }
}
