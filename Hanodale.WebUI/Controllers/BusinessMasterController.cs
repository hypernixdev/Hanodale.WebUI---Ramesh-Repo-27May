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


namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class BusinessMasterController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "BusinessMaster/Index";
        #endregion

        #region Constructor

        private readonly IBusinessService svc; 
        private readonly ICommonService svcCommon;
        private readonly IUserService svcUser;
        private readonly IWorkCategoryService svcWorkCategory;
        private readonly IBusinessClassificationService svcBusinessClassification;

        public BusinessMasterController(IBusinessService _bLService, ICommonService _commonService
            , IUserService _userService
            , IWorkCategoryService _workCategoryService
            , IBusinessClassificationService _businessClassificationService)
        {
            this.svc = _bLService; 
            this.svcCommon = _commonService;
            this.svcUser = _userService;
            this.svcWorkCategory = _workCategoryService;
            this.svcBusinessClassification = _businessClassificationService;
        }
        #endregion

        #region Business Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.pageId = 0;
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessMaster.Views.Index, _accessRight)
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
        public virtual ActionResult BindBusinessMaster(DataTableModel param)
        {
            int totalRecordCount = 0;
            IEnumerable<Businesses> filteredBusiness = null;
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


                        Businesses filterModel = null;
                        string idFilter0 = Convert.ToString(Request["sSearch_0"]);
                        var idFilter1 = Convert.ToString(Request["sSearch_1"]);
                        var idFilter2 = Convert.ToString(Request["sSearch_2"]);
                        var idFilter3 = Convert.ToString(Request["sSearch_3"]);
                        var bus_Id = -1;

                        int.TryParse(idFilter2, out bus_Id);
                        filterModel = new Businesses();
                        filterModel.supplierBusinessType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]);


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

                        var businessModel = this.svc.GetBusinessMaster(this.CurrentUserId, this.SubCostCenter, param.iDisplayStart, param.iDisplayLength, param.sSearch, filterModel);

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
        [HttpPost]
        public virtual JsonResult RenderAction()
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessMaster.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [AppAuthorize]
        public virtual ActionResult GetCustomSearchPanel(int searchType)
        {
            var obj = new BusinessModel();
            obj.searchType = searchType;
            int businessTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessType"]);

            var businessType = svcCommon.GetListModuleItem(businessTypeId);
            obj.BusinessTypes = businessType.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.id.ToString(),
                Selected = a.name == "Supplier",
            });

            var _statusList = new List<ModuleItems>();
            _statusList.Add(new ModuleItems { id = 0, name = "Active" });
            _statusList.Add(new ModuleItems { name = "InActive" });
            obj.lstStatus = _statusList.Select(p => new SelectListItem
            {
                Text = p.name,
                Value = p.name,
                Selected = p.name == "Active",
            }).ToList();

            return PartialView(MVC.BusinessMaster.Views._SearchPanel, obj);
        }

        #endregion

        #region Add,Edit and Delete

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
                _model.readOnly = true;
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

                                // Display the updated latest profileLastUpdated
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

                                //Hiding the Profile Update Latest
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

                                List<Organizations> subcostcenter = svcCommon.GetSubCostByMainCostId(this.SubCostCenter); //svc.GetSubCostCenterById(this.SubCostCenter, this.CurrentUserId);
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
                viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessMaster.Views.Edit, _model)
            });
        }

        #endregion
    }
}
