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
using System.Globalization;
using System.Web;


namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class BusinessController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "Business/Index";
        #endregion

        #region Constructor

        private readonly IBusinessService svc; private readonly ICommonService svcCommon;
        private readonly IOrganizationService svcOrganization;
        private readonly IUserService svcUser;
        private readonly IBusinessService svcBusiness;
        private readonly IWorkCategoryService svcWorkCategory;
        private readonly IBusinessClassificationService svcBusinessClassification;


        public BusinessController(IBusinessService _bLService, ICommonService _commonService
            , IOrganizationService _organizationService
, IUserService _userService
, IBusinessService _businessService
, IWorkCategoryService _workCategoryService
, IBusinessClassificationService _businessClassificationService)
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcOrganization = _organizationService;
            this.svcUser = _userService;
            this.svcBusiness = _businessService;
            this.svcWorkCategory = _workCategoryService;
            this.svcBusinessClassification = _businessClassificationService;
        }
        #endregion

        #region Business Details

        [AppAuthorize]
        public virtual ActionResult Index(string id)
        {
            try
            {
                id = Common.Encrypt(this.CurrentUserId.ToString(), id != null ? id : "0");
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                var objuser = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                if (objuser.bussinessType_Id == 52)
                {
                    _accessRight.canAdd = false;
                    _accessRight.pageId = 1;
                }
                else
                {
                    _accessRight.pageId = 0;
                }

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Business.Views.Index, _accessRight)
                        });
                    }
                    else
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
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult Business(string id)
        {
            try
            {
                id = Common.Encrypt(this.CurrentUserId.ToString(), id != null ? id : "0");
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Business.Views.Index, null)
                        });
                    }
                    else
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
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [Authorize]
        public virtual ActionResult BindBusiness(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            IEnumerable<Businesses> filteredBusiness = null;
            try
            {
                Businesses filterModel = new Businesses();
                string[] temp = new string[] { };
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                string idFilter0 = Convert.ToString(Request["sSearch_0"]);
                var idFilter1 = Convert.ToString(Request["sSearch_1"]);
                var idFilter2 = Convert.ToString(Request["sSearch_2"]);
                var idFilter3 = Convert.ToString(Request["sSearch_3"]);
                var bus_Id = -1;

                filterModel.supplierBusinessType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]);

                int.TryParse(idFilter2, out bus_Id);
                
                if (string.IsNullOrEmpty(myKey))
                {
                    if (!string.IsNullOrEmpty(idFilter0) || !string.IsNullOrEmpty(idFilter1) || !string.IsNullOrEmpty(idFilter2) || bus_Id > 0 || !string.IsNullOrEmpty(idFilter3))
                    {
                        
                        if (!string.IsNullOrEmpty(idFilter0))
                            filterModel.code = idFilter0;
                        if (!string.IsNullOrEmpty(idFilter1))
                            filterModel.name = idFilter1;
                        if (!string.IsNullOrEmpty(idFilter2) && bus_Id > 0)
                            filterModel.businessType_Id = Convert.ToInt32(idFilter2);
                        if (!string.IsNullOrEmpty(idFilter3))
                            filterModel.isActive = (idFilter3 ?? "") == "InActive" ? false : true;
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(idFilter0) && idFilter0 != "null")
                    {
                        filterModel = new Businesses();
                        filterModel.workCategoryIds = idFilter0.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(p => Convert.ToInt32(p)).ToArray();
                        // filterModel.workCategoryIds = idFilter0.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                    }
                }
                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        // Get login user Id
                        int userId = this.CurrentUserId;

                        var _user= this.svcUser.GetUserById(userId, userId);

                        int _orgId = 0;
                        if (_user.bussinessType_Id == 52)
                        {
                            param.sSearch = "";
                            _orgId = _user.business_Id;
                        }
                        else
                        {
                            if (filterModel == null)
                            {
                                filterModel = new Businesses();
                            }
                        }

                        //int organization_Id = 0;
                        //if (myKey != null)
                        //{
                        //    organization_Id = Common.DecryptToID(this.CurrentUserId.ToString(), myKey);
                        //}

                        var businessModel = this.svc.GetBusiness(this.CurrentUserId, param.iFilterAct, param.iDisplayStart, param.iDisplayLength, param.sSearch, filterModel, this.SubCostCenter);

                        if (svc != null)
                        {
                            //BusinessViewModel _businessViewModel = new BusinessViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<Businesses, string> orderingFunction = (c => sortColumnIndex == 0 ? c.businessTypeName :
                                                            sortColumnIndex == 1 ? c.code :
                                                            sortColumnIndex == 2 ? c.name :
                                                            sortColumnIndex == 3 ? c.primaryContact :
                                                            sortColumnIndex == 4 ? c.phone : c.primaryEmail
                                                            );

                            filteredBusiness = businessModel.lstBusiness;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredBusiness = filteredBusiness.OrderBy(orderingFunction);
                                else
                                    filteredBusiness = filteredBusiness.OrderByDescending(orderingFunction);
                            }

                            var result = BusinessData(filteredBusiness, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = businessModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = businessModel.recordDetails.totalDisplayRecords,
                                aaData = result
                            }, JsonRequestBehavior.AllowGet);
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
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_EDIT
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        /// <summary>
        /// This method is to get the user data as string array to bind into datatbale
        /// </summary>
        /// <param name="userEntry">User list</param>
        /// <returns></returns>
        public List<string[]> BusinessData(IEnumerable<Businesses> businessEntry, int currentUserId)
        {
            return businessEntry.Select(entry => new string[]
            {  
                entry.businessTypeName,
                entry.code, 
                entry.name,
                entry.primaryContact,
                entry.phone,
                entry.primaryEmail,
                Common.Encrypt(currentUserId.ToString(), entry.id.ToString()),
                GetUserRights(currentUserId,entry),
                (entry.statusColor==0?("Empty"):(entry.statusColor==1? ("LoginExist"):("LoginInActive")))
            }).ToList();
        }

        public string GetUserRights(int userId, Businesses _businessEn)
        {
            
            
            var objuser = svcUser.GetUserById(userId, userId);
            if (objuser.userRole_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierRole"]))
            {
                return "3";
            }
            else
            {
                return "1";
            }
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction()
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.Business.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        #region Add,Edit and Delete

        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                BusinessModel _model = new BusinessModel();

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {
                        int currencyId = Convert.ToInt32(WebConfigurationManager.AppSettings["PrimaryCurrency"]);

                        var currency = svcCommon.GetListModuleItem(currencyId);
                        _model.lstprimaryCurrency = currency.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });

                        int companytypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["CompanyType"]);

                        var companyType = svcCommon.GetListModuleItem(companytypeId);
                        _model.lstCompanyType = companyType.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });

                        int businessTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessType"]);

                        var businessType = svcCommon.GetListModuleItem(businessTypeId);
                        _model.BusinessTypes = businessType.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });

                        int bussClassification_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Classification"]);

                        var classfication = svcCommon.GetListModuleItem(bussClassification_Id);
                        _model.lstClassificationItem = classfication.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });

                        _model.isEdit = false;
                        _model.status = true;
                        _model.isMAHBRegistered = true;

                        var user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                        if (user != null)
                        {
                            if (user.bussinessType_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]))
                            {
                                _model.isprofileupdate = true;
                            }
                        }

                        var workcategorylist = svcWorkCategory.GetListWorkCategory();
                        _model.WorkCategoryList = workcategorylist;
                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                        int submenu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Business"]);

                       // var orgPrefix = svcOrganization.GetOrganizationById(this.SubCostCenter);

                      //  var existCode = svcCommon.GetGenerateCodeByOrgId(this.SubCostCenter, submenu_Id);

                      //  var codeGenerated = svcCommon.GetModuleCodes(submenu_Id, orgPrefix.prefix, existCode);

                     //   _model.code = codeGenerated.generateCode;

                        var menuType = new MenuTypes();
                        menuType.isBusiness = true;
                        menuType.appSettingList = Common.GetAppSettingItem(WebConfigurationManager.AppSettings["AutoLoadKey"]);
                        _model.code = svcCommon.GenerateAutoCode(this.SubCostCenter, -1, menuType);



                        var subcostcenter = svcCommon.GetSubCostCenterListById(this.SubCostCenter, this.CurrentUserId);//svc.GetSubCostCenterById(this.SubCostCenter, this.CurrentUserId);
                        var selectedItem = subcostcenter.SingleOrDefault(p => p.id == this.SubCostCenter);
                        if (selectedItem != null)
                            selectedItem.isSelected = true;

                        List<Organizations> lst = new List<Organizations>();
                        foreach (var item in subcostcenter)
                        {
                            if (lst.Contains(item))
                            {
                                item.isSelected = false;
                            }
                        }
                        _model.lstorganization = subcostcenter;
                        _model.registrationDate = DateTime.Now;

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Business.Views.Create, _model)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_ADD
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public virtual JsonResult SaveBusiness(BusinessModel model)
        {
            var user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);

            if (!(model.isMAHBRegistered ?? false) || model.businessType_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["StaffBusinessType"]))
            {
                ModelState.Remove("registrationNo");
                ModelState.Remove("registrationDate");
                ModelState.Remove("expiryDate");
            }

            if (model.organisationIds == null && user.bussinessType_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]))
            {
                int newId = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                List<int> lsts = svc.GetBusinessOrganizationById(newId);
                model.organisationIds = lsts.ToArray();
            }
            bool isExist = svc.IsBusinessWorkCategoryandOrganisationExists(model.workCategoryIds, model.organisationIds);
            if (!isExist)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    int id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        if (id > 0)
                        {
                            if (!_accessRight.canEdit)
                            {
                                return Json(new
                                {
                                    status = "Access " + Common.Status.Denied.ToString(),
                                    message = Resources.NO_ACCESS_RIGHTS_EDIT
                                });
                            }
                        }
                        else
                        {
                            if (!_accessRight.canAdd)
                            {
                                return Json(new
                                {
                                    status = "Access " + Common.Status.Denied.ToString(),
                                    message = Resources.NO_ACCESS_RIGHTS_ADD
                                });
                            }
                        }

                        if (svc != null)
                        {
                            Businesses entity = new Businesses();

                            //entity.workCategoryIds = model.workCategoryIds.Split(',').Select(str => int.Parse(str)).ToArray();
                            entity.workCategoryIds = model.workCategoryIds;
                            entity.businessType_Id = model.businessType_Id;
                            entity.name = model.name;
                            entity.code = model.code;
                            entity.phone = model.phone;
                            entity.phone2 = model.phone2;
                            entity.fax = model.fax;
                            entity.webSite = model.webSite;
                            entity.primaryContact = model.primaryContact;
                            entity.primaryEmail = model.primaryEmail;
                            //entity.primaryCurrency = Convert.ToInt32(model.primaryCurrency);
                            entity.rocNo = model.rocNo;
                            entity.registrationNo = model.registrationNo;
                            entity.registrationDate = model.registrationDate;
                            entity.expiryDate = model.expiryDate;
                            entity.companyType = model.companyType != null ? model.companyType : null;
                            entity.referenceId = model.referenceId;
                            entity.limitation = model.limitation;
                            entity.isMAHBRegistered = model.isMAHBRegistered;
                            entity.remarks = model.remarks;
                            entity.status = true;
                            entity.address = model.address;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;
                            int[] _classification_Ids = model.classification_Ids;
                            entity.classification_Ids = _classification_Ids;
                            entity.organisationIds = model.organisationIds;
                            entity.profileLastUpdated = model.profileLastUpdated;
                            entity.gstNo = model.gstNo;
                            if (id > 0)
                            {
                                entity.id = id;
                            }
                            else
                            {
                                //int submenu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Business"]);

                                //var orgPrefix = svcOrganization.GetOrganizationById(this.SubCostCenter);

                                //var existCode = svcCommon.GetGenerateCodeByOrgId(this.SubCostCenter, submenu_Id);

                                //var codeGenerated = svcCommon.GetModuleCodes(submenu_Id, orgPrefix.prefix, existCode);

                                //entity.code = codeGenerated.generateCode;

                                var menuType = new MenuTypes();
                                menuType.isBusiness = true;
                                menuType.appSettingList = Common.GetAppSettingItem(WebConfigurationManager.AppSettings["AutoLoadKey"]);
                                entity.code = svcCommon.GenerateAutoCode(this.SubCostCenter, -1, menuType);
                               

                              
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            bool isExists = svc.IsBusinessExists(entity);
                            if (!isExists)
                            {
                                int check = svc.IsBusinessSupplierExists(entity);
                                if (check == 0)
                                {
                                    var save = svc.SaveBusiness(this.CurrentUserId, entity, _accessRight.pageName);

                                    if (save != null)
                                    {
                                        if (id > 0)
                                        {
                                            return Json(new
                                            {
                                                status = Common.Status.Success.ToString(),
                                                message = Resources.MSG_UPDATE,
                                                id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString())
                                            });
                                        }
                                        else
                                        {
                                            return Json(new
                                            {
                                                status = Common.Status.Success.ToString(),
                                                message = Resources.MSG_SAVE,
                                                id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString())
                                            });
                                        }
                                    }
                                    else
                                    {
                                        if (id > 0)
                                        {
                                            return Json(new
                                            {
                                                status = Common.Status.Success.ToString(),
                                                message = Resources.MSG_ERR_UPDATE
                                            });
                                        }
                                        else
                                        {
                                            return Json(new
                                            {
                                                status = Common.Status.Error.ToString(),
                                                message = Resources.MSG_ERR_SAVE
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    if (check == 1)
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Warning.ToString(),
                                            message = Resources.BUSINESSTHREEFIELDS_RECORD_EXISTS
                                        });
                                    }
                                    else if (check == 2)
                                    {
                                        return Json(new
                                       {
                                           status = Common.Status.Warning.ToString(),
                                           message = Resources.BUSINESS_ROCNO_EXISTS
                                       });
                                    }
                                    else if (check == 3)
                                    {
                                        return Json(new
                                       {
                                           status = Common.Status.Warning.ToString(),
                                           message = Resources.BUSINESS_REGISTERNO_EXISTS
                                       });
                                    }
                                    else if (check == 4)
                                    {
                                        return Json(new
                                       {
                                           status = Common.Status.Warning.ToString(),
                                           message = Resources.BUSINESS_REGISTERDATE_EXISTS
                                       });
                                    }
                                }
                            }
                            else
                            {
                                return Json(new
                                {

                                    status = Common.Status.Warning.ToString(),
                                    message = Resources.BUSINESS_RECORD_EXISTS
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
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS
                        });
                    }
                }
                catch (Exception err)
                {
                    throw new ErrorException(err.Message);
                }
            }
            return Json(new
            {
                status = Common.Status.Warning.ToString(),
                message = Resources.MSG_WORKCATEGORY_ORGANIZATION_MANDATORY
            });
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            BusinessModel _model = new BusinessModel();
            try
            {
                int businessId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _model.readOnly = readOnly;
                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var business = svc.GetBusinessById(businessId);

                            if (business != null)
                            {
                                _model.id = id;
                                _model.isEdit = true;
                                _model.businessType_Id = business.businessType_Id;
                                _model.name = business.name;
                                _model.code = business.code;
                                _model.phone = business.phone;
                                _model.phone2 = business.phone2;
                                _model.fax = business.fax;
                                _model.webSite = business.webSite;
                                _model.primaryContact = business.primaryContact;
                                _model.primaryEmail = business.primaryEmail;
                                _model.primaryCurrency = business.primaryCurrency.ToString();
                                _model.remarks = business.remarks;
                                _model.isMAHBRegistered = business.isMAHBRegistered;
                                _model.registrationNo = business.registrationNo;
                                _model.registrationDate = business.registrationDate;
                                _model.expiryDate = business.expiryDate;
                                _model.referenceId = business.referenceId;
                                _model.rocNo = business.rocNo;
                                _model.businessCategory = business.businessCategory;
                                _model.paidUpCapital = business.paidUpCapital;
                                _model.companyType = business.companyType;
                                _model.address = business.address;
                                _model.limitation = business.limitation;
                                _model.status = business.status;
                                _model.gstNo = business.gstNo;

                                if (business.businessType_Id != Convert.ToInt32(WebConfigurationManager.AppSettings["Section"]))
                                    _model.isbusinessType_Id = true;
                                else
                                    _model.isbusinessType_Id = false;

                                var user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                                if (user != null)
                                {
                                    if (user.bussinessType_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]))
                                    {
                                        _model.isprofileupdate = true;
                                        _model.profileLastUpdated = business.profileLastUpdated == null ? DateTime.Now : business.profileLastUpdated;
                                    }
                                }


                                int companytypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["CompanyType"]);

                                var companyType = svcCommon.GetListModuleItem(companytypeId);
                                _model.lstCompanyType = companyType.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == business.companyType)
                                });

                                int currencyId = Convert.ToInt32(WebConfigurationManager.AppSettings["PrimaryCurrency"]);

                                var currency = svcCommon.GetListModuleItem(currencyId);
                                _model.lstprimaryCurrency = currency.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == business.primaryCurrency)
                                });

                                int businessTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessType"]);

                                var businessType = svcCommon.GetListModuleItem(businessTypeId);
                                _model.BusinessTypes = businessType.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == business.businessType_Id)
                                });

                                int bussClassification_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Classification"]);

                                var classfication = svcCommon.GetListModuleItem(bussClassification_Id);
                                var lst = svcBusinessClassification.GetListBusinessClassificationByBusinessId(businessId);
                                _model.lstClassificationItem = classfication.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = lst.Any(p => p.classification_Id == a.id)
                                });

                                _model.isEdit = true;
                                var users = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                                if (users != null)
                                {
                                    if (users.bussinessType_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]))
                                    {
                                        _model.isprofileupdate = true;
                                    }
                                }

                                var workcategorylist = svcWorkCategory.GetListWorkCategory();
                                List<int> workcategorylst = svc.GetListBusinessworkCategory(Common.DecryptToID(this.CurrentUserId.ToString(), id));
                                foreach (var item in workcategorylist)
                                {
                                    if (workcategorylst.Contains(item.id))
                                    {
                                        item.isSelected = true;
                                    }
                                }
                                _model.WorkCategoryList = workcategorylist;

                                List<Organizations> subcostcenter = svcCommon.GetSubCostByMainCostId(this.SubCostCenter); 
                                int newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                                List<int> lsts = svc.GetBusinessOrganizationById(newId);
                                foreach (var item in subcostcenter)
                                {
                                    if (lsts.Contains(item.id))
                                    {
                                        item.isSelected = true;
                                    }
                                }

                                _model.lstorganization = subcostcenter;

                                var assignedbusiness = svcUser.GetUserBuinessById(this.CurrentUserId);

                                if (assignedbusiness != null)
                                {
                                    var businesss = svc.GetBusinessById(assignedbusiness.business_Id);
                                    if (businesss.businessType_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]))
                                    {
                                        _model.issupplier = true;
                                    }
                                    else
                                    {
                                        _model.issupplier = false;
                                    }
                                }


                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_ERR_RETRIEVE
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
                    else
                    {
                        //Redirect to access denied page
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.Business.Views.Create, _model)
            });
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Maintenance(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            BusinessModel _model = new BusinessModel();
            try
            {
                int businessId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _model.readOnly = readOnly;
                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var business = svc.GetBusinessById(businessId);

                            if (business != null)
                            {
                                _model.id = id;
                                _model.isEdit = true;
                                _model.businessType_Id = business.businessType_Id;
                                _model.name = business.name;
                                _model.code = business.code;
                                _model.phone = business.phone;
                                _model.phone2 = business.phone2;
                                _model.fax = business.fax;
                                _model.webSite = business.webSite;
                                _model.primaryContact = business.primaryContact;
                                _model.primaryEmail = business.primaryEmail;
                                _model.primaryCurrency = business.primaryCurrency.ToString();
                                _model.remarks = business.remarks;
                                _model.isMAHBRegistered = business.isMAHBRegistered;
                                _model.registrationNo = business.registrationNo;
                                _model.registrationDate = business.registrationDate;
                                _model.expiryDate = business.expiryDate;
                                _model.referenceId = business.referenceId;
                                _model.rocNo = business.rocNo;
                                _model.businessCategory = business.businessCategory;
                                _model.paidUpCapital = business.paidUpCapital;
                                _model.companyType = business.companyType;
                                _model.address = business.address;
                                _model.limitation = business.limitation;
                                _model.status = business.status;
                                _model.gstNo = business.gstNo;

                                if (business.businessType_Id != Convert.ToInt32(WebConfigurationManager.AppSettings["Section"]))
                                    _model.isbusinessType_Id = true;
                                else
                                    _model.isbusinessType_Id = false;
                                var user = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                                if (user != null)
                                {
                                    if (user.bussinessType_Id == Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]))
                                    {
                                        _model.isprofileupdate = true;
                                        if (business.profileLastUpdated == null)
                                        {
                                            _model.profileLastUpdated = DateTime.Now;
                                        }
                                        else
                                        {
                                            _model.profileLastUpdated = business.profileLastUpdated;
                                        }

                                    }
                                }


                                int companytypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["CompanyType"]);

                                var companyType = svcCommon.GetListModuleItem(companytypeId);
                                _model.lstCompanyType = companyType.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == business.companyType)
                                });

                                int currencyId = Convert.ToInt32(WebConfigurationManager.AppSettings["PrimaryCurrency"]);

                                var currency = svcCommon.GetListModuleItem(currencyId);
                                _model.lstprimaryCurrency = currency.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == business.primaryCurrency)
                                });

                                int businessTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessType"]);

                                var businessType = svcCommon.GetListModuleItem(businessTypeId);
                                _model.BusinessTypes = businessType.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == business.businessType_Id)
                                });

                                int bussClassification_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Classification"]);

                                var classfication = svcCommon.GetListModuleItem(bussClassification_Id);
                                var lst = svcBusinessClassification.GetListBusinessClassificationByBusinessId(businessId);
                                _model.lstClassificationItem = classfication.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = lst.Any(p => p.classification_Id == a.id)
                                });

                                _model.isEdit = true;

                                var workcategorylist = svcWorkCategory.GetListWorkCategory();
                                List<int> workcategorylst = svc.GetListBusinessworkCategory(Common.DecryptToID(this.CurrentUserId.ToString(), id));
                                foreach (var item in workcategorylist)
                                {
                                    if (workcategorylst.Contains(item.id))
                                    {
                                        item.isSelected = true;
                                    }
                                }
                                _model.WorkCategoryList = workcategorylist;

                                List<Organizations> subcostcenter = svcCommon.GetSubCostCenterById((this.SubCostCenter == 0 ? 31 : this.SubCostCenter), this.CurrentUserId);
                                int newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                                List<int> lsts = svc.GetBusinessOrganizationById(newId);
                                foreach (var item in subcostcenter)
                                {
                                    if (lsts.Contains(item.id))
                                    {
                                        item.isSelected = true;
                                    }
                                }

                                _model.lstorganization = subcostcenter;

                                var assignedbusiness = svcUser.GetUserBuinessById(this.CurrentUserId);

                                if (assignedbusiness != null)
                                {
                                    var businesss = svc.GetBusinessById(assignedbusiness.business_Id);
                                    if (businesss.businessType_Id == 52)
                                    {
                                        _model.issupplier = true;
                                    }
                                    else
                                    {
                                        _model.issupplier = false;
                                    }
                                }



                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_ERR_RETRIEVE
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
                    else
                    {
                        //Redirect to access denied page
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.Business.Views.Maintenance, _model)
            });

        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                int businessId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteBusiness(this.CurrentUserId, businessId, _accessRight.pageName);
                            if (isSuccess)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_DELETE
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Error.ToString(),
                                    message = Resources.MSG_ERR_DELETE
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
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                {
                    return Json(new
                    {
                        status = Common.Status.Warning.ToString(),
                        message = Resources.MSG_RECORD_IN_USE
                    });
                }
                else
                {
                    throw new ErrorException(ex.Message);
                }
            }
            return View();
        }

        #endregion

    }
}
