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

namespace Hanodale.WebUI.Controllers
{
    public partial class StoreController : BaseController
    {

        #region Declaration
        readonly string PAGE_URL = string.Empty;
        readonly string PAGE_URLForAccessRight = "Store/Index";
        #endregion

        #region Constructor

        private readonly IStoreService svc;
        private readonly ISyncManager syncManager;
        public StoreController(IStoreService _bLService, ICommonService _svcCommon, ISyncManager syncManager)
        {
            this.svcCommon = _svcCommon;
            this.sectionName = "Store";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Store_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            this.syncManager = syncManager;
        }
        #endregion

        #region Module Item Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new StoreModel());

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URLForAccessRight);

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
        [AppAuthorize]
        public virtual ActionResult GetCustomSearchPanel()
        {
            var _model = new StoreModel();

            return PartialView(MVC.Store.Views._SearchPanel, _model);
        }

        [Authorize]
        public virtual ActionResult BindStore(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            List<Stores> filteredStore = null;
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

                         var filterEntity = new Stores();
                        var idFilter0 = Convert.ToString(Request["sSearch_0"]).Trim();
                        var idFilter1 = Convert.ToString(Request["sSearch_1"]).Trim();
                        var idFilter2 = Convert.ToString(Request["sSearch_2"]).Trim();
                        var idFilter3 = Convert.ToString(Request["sSearch_3"]).Trim();

                        int masterRecord_Id = 0;
                        if (myKey != null)
                            masterRecord_Id = Common.DecryptToID(userId.ToString(), myKey);
                        if (!string.IsNullOrEmpty(idFilter0))
                        {
                            filterEntity.searchCity = Convert.ToString(idFilter0);
                        }
                        if (!string.IsNullOrEmpty(idFilter1 ))
                        {
                            filterEntity.searchState = Convert.ToString(idFilter1);
                        }
                        if (!string.IsNullOrEmpty(idFilter2))
                        {
                            filterEntity.searchCountry = Convert.ToString(idFilter2);
                        }
                        if (!string.IsNullOrEmpty(idFilter3))
                        {
                            filterEntity.searchZip = Convert.ToString(idFilter3);
                        }
                        var filter = new DatatableFilters { currentUserId = userId, masterRecord_Id = masterRecord_Id, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };
                        var storeModel = this.svc.GetStore(filter, filterEntity);


                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            //Sorting

                            filteredStore = storeModel.lstStore;
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

                                filteredStore = filteredStore.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = StoreData(filteredStore, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = storeModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = storeModel.recordDetails.totalDisplayRecords;

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

        public List<string[]> StoreData(List<Stores> storeEntry, int currentUserId)
        {
            var result = this.GetDatatableData<Stores>(storeEntry, currentUserId);
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
                    var _model = new StoreModel();

                    this.FillupFieldMetadata(_model, false);

                    /*
                    if (_model.store_Id_Metadata.isEditableInCreate)
                    {

                        HanodaleEntities model = new HanodaleEntities();
                        //Get Plant List
                        var query = from p in model.Store
                                    select new
                                    {
                                        id = p.id.ToString(),
                                        company = p.company,
                                    };
                        var _storeList = query.ToList();

                        _model.lstStore = _storeList.Select(p => new SelectListItem
                        {
                            Text = p.company,
                            Value = p.id.ToString(),
                        }).ToList();

                    }
                    if (_model.customer_Id_Metadata.isEditableInCreate)
                    {

                        HanodaleEntities model = new HanodaleEntities();
                        //Get Plant List
                        var _customerList = model.Customer.OrderBy(p => p.name).Select(p => new Customer
                        {
                            id = p.id,
                            name = p.name,
                        }).ToList();

                        _model.lstCustomer = _customerList.Select(p => new SelectListItem
                        {
                            Text = p.name,
                            Value = p.id.ToString(),

                        }).ToList();

                    }*/
                    dropdownItemList.Add(_model.country_Id_Metadata.fieldName, _model.country_Id_Metadata.dropdownDefaultValue);
                    dropdownItemList.Add(_model.state_Id_Metadata.fieldName, _model.state_Id_Metadata.dropdownDefaultValue);
                    dropdownItemList.Add(_model.city_Id_Metadata.fieldName, _model.city_Id_Metadata.dropdownDefaultValue);
                    // Load Countries
                    _model.lstCountry = GetCountryList(this.GetDropdownParentValue(_model.country_Id_Metadata.dropdownParentFieldName), _model.country_Id_Metadata.dropdownDefaultValue ?? 0);
                    // Load States
                    _model.lstState = GetStateList(this.GetDropdownParentValue(_model.state_Id_Metadata.dropdownParentFieldName), _model.state_Id_Metadata.dropdownDefaultValue ?? 0);
                    // Load Cities
                    _model.lstCity = GetCityList(this.GetDropdownParentValue(_model.city_Id_Metadata.dropdownParentFieldName), _model.city_Id_Metadata.dropdownDefaultValue ?? 0);

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

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
        public virtual JsonResult SaveStore(StoreModel model)
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
                            Stores entity = new Stores();
                            entity.id = decrypted_Id;
                            entity.plant = model.plant;
                            entity.company = model.company;
                            entity.address1 = model.address1;
                            entity.address2 = model.address2;
                            entity.address3 = model.address3;
                            entity.city_Id = model.city_Id ?? 0;
                            entity.state_Id = model.state_Id ?? 0;
                            entity.country_Id = model.country_Id ?? 0;
                            entity.zip = model.zip;

                            bool isExists = svc.IsStoreExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveStore(entity);

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

                        var store = svc.GetStoreById(decrypted_Id);

                        if (store != null)
                        {
                            StoreModel _model = new StoreModel();
                            PlantModel pModel = new PlantModel();


                            this.FillupFieldMetadata(_model, true);

                            _model.readOnly = readOnly;

                            _model.id = id;
                            _model.isEdit = true;
                            _model.plant = store.plant;
                            _model.company = store.company;
                            _model.address1 = store.address1;
                            _model.address2 = store.address2;
                            _model.address3 = store.address3;
                            _model.city_Id = store.city_Id;
                            _model.state_Id = store.state_Id;
                            _model.country_Id = store.country_Id;
                            _model.zip = store.zip;


                            /*
                            if (_model.store_Id_Metadata.isEditableInCreate)
                            {

                                HanodaleEntities model = new HanodaleEntities();
                                //Get Plant List

                                var query = from p in model.Plant
                                            select new
                                            {
                                                id = p.id,
                                                name = p.name,
                                            };
                                var _storeList = query.ToList();

                                _model.lstStore = _storeList.Select(p => new SelectListItem
                                {
                                    Text = p.name,
                                    Value = p.id.ToString(),
                                    Selected = (p.id == store.store_Id)


                                }).ToList();

                            }
                            if (_model.customer_Id_Metadata.isEditableInCreate)
                            {

                                HanodaleEntities model = new HanodaleEntities();
                                //Get Plant List


                                var query1 = from p in model.Customer
                                             select new
                                             {
                                                 id = p.id,
                                                 name = p.name,
                                             };
                                var _customerList = query1.ToList();

                                _model.lstCustomer = _customerList.Select(p => new SelectListItem
                                {
                                    Text = p.name,
                                    Value = p.id.ToString(),
                                    Selected = (p.id == store.customer_Id)


                                }).ToList();

                            } */
                            dropdownItemList.Add(_model.country_Id_Metadata.fieldName, store.country_Id);
                            dropdownItemList.Add(_model.state_Id_Metadata.fieldName, store.state_Id);
                            dropdownItemList.Add(_model.city_Id_Metadata.fieldName, store.city_Id);

                            // Load Countries
                            _model.lstCountry = GetCountryList(this.GetDropdownParentValue(_model.country_Id_Metadata.dropdownParentFieldName), store.country_Id);

                            // Load States
                            _model.lstState = GetStateList(this.GetDropdownParentValue(_model.state_Id_Metadata.dropdownParentFieldName), store.state_Id);

                            // Load Cities
                            _model.lstCity = GetCityList(this.GetDropdownParentValue(_model.city_Id_Metadata.dropdownParentFieldName), store.city_Id);


                            return Json(new
                            {
                                viewMarkup = Common.RenderPartialViewToString(this, "Create", _model)
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
                            bool isSuccess = svc.DeleteStore(decrypted_Id);

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
        #region Sync service 
        [HttpPost]
        [Authorize]
        public virtual ActionResult SyncStores()
        {
            try
            {
                //var success = this.syncManager.syncEntity("Store", "plants", "plant", true, "");
                var success = this.syncManager.syncEntity("PaymentReport", "paymentreport", "paymentDate", true, "");
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
    }
}
