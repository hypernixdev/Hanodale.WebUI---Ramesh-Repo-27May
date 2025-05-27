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


namespace Hanodale.WebUI.Controllers
{
    public partial class PriceListController : BaseController
    {
        #region Declaration
        readonly string PAGE_URL = string.Empty;

        #endregion

        #region Constructor

        private readonly IPriceListService svc;
        private readonly ISyncManager syncManager;
        public PriceListController(IPriceListService _bLService, ICommonService _svcCommon, ISyncManager syncManager)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "PriceList";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["PriceList_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            this.syncManager = syncManager;
        }
        #endregion

        #region PriceList Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("This is called");
            try
            {
                var _model = this.GetVisibleColumnForGridView(new PriceListModel());

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
        public virtual JsonResult PriceList()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new PriceListModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.PriceList.Views.Index, _model)
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
        public virtual ActionResult BindPriceList(DataTableModel param)
        {
            int totalRecordCount = 0;
            List<PriceLists> filteredPriceLists = null;
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
                        var PriceListModel = this.svc.GetPriceList(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            filteredPriceLists = PriceListModel.lstPriceList;
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

                                filteredPriceLists = filteredPriceLists.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = PriceListData(filteredPriceLists, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = PriceListModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = PriceListModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> PriceListData(List<PriceLists> PriceListEntry, int currentUserId)
        {
            var result = this.GetDatatableData<PriceLists>(PriceListEntry, currentUserId);
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.PriceList.Views.RenderAction, _accessRight)
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
                    var _model = new PriceListModel();

                    this.FillupFieldMetadata(_model, false);

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.PriceList.Views.Create, _model)
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
        public virtual JsonResult SavePriceList(PriceListModel model)
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
                            PriceLists entity = new PriceLists();
                            entity.id = decrypted_Id;
                            entity.listCode = model.listCode;
                            entity.currencyCode = model.currencyCode;
                            entity.listDescription = model.listDescription;
                            entity.startDate = model.startDate;
                            entity.endDate = model.endDate;
                          

                            bool isExists = svc.IsPriceListExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SavePriceList(entity);

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

        private PriceListModel GetPriceListModel(string id, bool readOnly)
        {
            try
            {
                PriceListModel _model = new PriceListModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var pricelist = svc.GetPriceListById(decrypted_Id);

                if (pricelist != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    _model.listCode = pricelist.listCode;
                    _model.currencyCode = pricelist.currencyCode;
                    _model.listDescription = pricelist.listDescription;
                    _model.startDate = pricelist.startDate;
                    _model.endDate = pricelist.endDate;
                    _model.custNum = pricelist.custNum;
                    _model.custID = pricelist.custID;
                    _model.shipToNum = pricelist.shipToNum;
                    _model.SeqNum = pricelist.seqNum;
                    _model.CustGroup = pricelist.CustGroup;

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
                        var _model = GetPriceListModel(id, readOnly);

                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.PriceList.Views.Create, _model)
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
                        var result = svc.DeletePriceList(decrypted_Id);

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

       /* private void FillupFieldMetadata(PriceListModel model, bool isEdit)
        {
            // Implementation for field metadata population
        }*/

        #endregion
        #region Sync service 
        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncPriceLists()
        {
            try
            {
                var success = this.syncManager.syncEntity("PriceList", "PriceList", "sysRowID", false, "");
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

        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncCustomerPriceLists()
        {
            try
            {
                var success = this.syncManager.syncEntity("CustomerPriceList", "CustomerPriceList", "sysRowID", false, "");
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

        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncPartPriceLists()
        {
            try
            {
                var success = this.syncManager.syncEntity("PriceListPart", "PriceListParts", "sysRowID", false, "");
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
        #region   list Screen Search
        [HttpPost]
        [Authorize]
        public virtual JsonResult GetPriceListParts(string ListCode)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var priceListPart = this.svc.GetPriceListParts(ListCode);
                        if (priceListPart == null || !priceListPart.Any())
                        {
                            return this.Msg_ErrorInRetriveData();  // Handle case when no data is found
                        }

                        return Json(new
                        {
                            success = true,
                            priceListPart = priceListPart
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
        public virtual JsonResult GetCustomerPriceList(int CustNum, string groupCode)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        var customerpriceList = this.svc.GetCustomerPriceList(CustNum, groupCode);
                        if (customerpriceList == null || !customerpriceList.Any())
                        {
                            return this.Msg_ErrorInRetriveData();  // Handle case when no data is found
                        }

                        return Json(new
                        {
                            success = true,
                            customerpriceList = customerpriceList
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