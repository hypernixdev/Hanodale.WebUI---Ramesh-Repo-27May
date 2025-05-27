using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class OrganizationController : AuthorizedController
    {

        #region Declaration
        const string PAGE_URL = "Organization/Organization";
        #endregion

        #region Constructor

        private readonly IOrganizationService svc; private readonly ICommonService svcCommon;

        public OrganizationController(IOrganizationService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region User Organization Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.elementId = System.Configuration.ConfigurationManager.AppSettings["subCostCategoryId"];

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Organization.Views.Index, _accessRight)
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
        //[CheckAccessRights(PageName = PAGE_URL, UserId=1)]
        public virtual ActionResult Organization()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.elementId = System.Configuration.ConfigurationManager.AppSettings["subCostCategoryId"];


                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Organization.Views.Index, _accessRight)
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
        public virtual ActionResult BindOrganization(DataTableModel param)
        {
            IEnumerable<Organizations> filteredOrganizations = null;
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

                        var userOrganizationModel = this.svc.GetOrganizationBySearch(this.CurrentUserId, this.CurrentUserId, param.iDisplayStart, param.iDisplayLength, param.sSearch);

                        if (svc != null)
                        {

                            if (svc != null)
                            {
                                OrganizationModel _userOrganizationViewModel = new OrganizationModel();

                                //Sorting
                                var sortColumnIndex = param.iSortCol_0;
                                Func<Organizations, string> orderingFunction = (c => sortColumnIndex == 0 ? c.name :
                                                               sortColumnIndex == 1 ? c.parentName : 
                                                               sortColumnIndex == 2 ? c.orgCategoryName : 
                                                               sortColumnIndex == 3 ? c.description : 
                                                               sortColumnIndex == 4 ? c.prefix : (c.isActive ? Common.RecordStatus.Active.ToString() : Common.RecordStatus.InActive.ToString()));

                                filteredOrganizations = userOrganizationModel.lstOrganizations;
                                if (param.sSortDir_0 != null)
                                {
                                    if (param.sSortDir_0 == "asc")
                                        filteredOrganizations = filteredOrganizations.OrderBy(orderingFunction);
                                    else
                                        filteredOrganizations = filteredOrganizations.OrderByDescending(orderingFunction);
                                }
                            }

                            var result = OrganizationData(filteredOrganizations, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = userOrganizationModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = userOrganizationModel.recordDetails.totalDisplayRecords,
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
        public List<string[]> OrganizationData(IEnumerable<Organizations> userEntry, int currentUserId)
        {
            return userEntry.Select(entry => new string[]
            {  
                entry.name,
                entry.parentName,
                entry.orgCategoryName,
                entry.description,
                entry.prefix,
                ((entry.isActive)?Common.RecordStatus.Active.ToString():Common.RecordStatus.InActive.ToString()),
                //(entry.parent_Id==Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mainCostCategoryId"].ToString())).ToString(),
                Common.Encrypt(currentUserId.ToString(), entry.id.ToString()),
                 GetUserRights(currentUserId,entry)
            }).ToList();
        }

        public string GetUserRights(int userId, Organizations _organizationEn)
        {
            if (_organizationEn.hasCategoryChild)
            {
                return "0";
            }
            else
            {
                return "1";
            }
            //if (_organizationEn.orgCategoryName == "SUB COST CENTER")
            //{
            //    return "1";
            //}
            //else
            //{
            //    return "0";
            //}
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.Organization.Views.RenderAction, _accessRight)
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
        public virtual JsonResult Create(int id = 0, int category_Id = 0, bool isTreeView = false, bool isCostCenter = false)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, (isCostCenter ? PAGE_URL : PAGE_URL));

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {
                        OrganizationModel _model = new OrganizationModel();
                        _model.parent_Id = Common.Encrypt(this.CurrentUserId.ToString(), id.ToString());
                        //_model.orgCategory_Id = Common.Encrypt(this.CurrentUserId.ToString(), category_Id.ToString());

                        //if (category_Id == 0)
                        //    category_Id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mainCostCategoryId"]);

                        //var orgCategory = svc.GetOrganizationCategoryById(category_Id);
                        //if (orgCategory != null)
                        //{
                        //    _model.orgCategoryName = orgCategory.name;
                        //}

                        if (category_Id == 0)
                        {
                            var _orgCategory_Id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mainCostCategoryId"]);
                            var orgCategory = svc.GetOrganizationCategoryById(_orgCategory_Id);
                            if (orgCategory != null)
                            {
                                var lst = new List<SelectListItem>();
                                lst.Add(new SelectListItem { Text = orgCategory.name, Selected = true, Value = orgCategory.id.ToString() });
                                _model.lstCategoryType = lst;
                            }
                        }
                        else
                        {

                            var orgCategory = svc.GetOrganizationCategoryConfigByCategoryId(category_Id);
                            if (orgCategory != null)
                            {
                                _model.lstCategoryType = orgCategory.Select(p => new SelectListItem { Text = p.childCategoryName, Value = p.childCategory_Id.ToString() }).ToList();
                            }
                        }

                        var org = svc.GetOrganizationById(id);
                        if (org != null)
                        {
                            _model.parentName = org.name;
                        }

                        //_model.isMainCostCenter = id;
                        _model.status = true;
                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                        _model.isEdit = false;
                        _model.organaizationId = id.ToString();
                        _model.isTreeView = isTreeView;

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Organization.Views.Create, _model)
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

        [Authorize]
        public virtual JsonResult CreateSubCost(string id)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {
                        //var category_Id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["subCostCategoryId"]);
                        OrganizationModel _model = new OrganizationModel();
                        _model.parent_Id = id;
                        // _model.orgCategory_Id = Common.Encrypt(this.CurrentUserId.ToString(), category_Id.ToString());
                        var newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                        //var orgCategory = svc.GetOrganizationCategoryById(category_Id);
                        //if (orgCategory != null)
                        //{
                        //    _model.orgCategoryName = orgCategory.name;
                        //}

                        var orgCategory = svc.GetOrganizationCategoryConfigByOrganizationId(newId);
                        if (orgCategory != null)
                        {
                            _model.lstCategoryType = orgCategory.Select(p => new SelectListItem { Text = p.childCategoryName, Value = p.childCategory_Id.ToString() }).ToList();
                        }

                        var org = svc.GetOrganizationById(newId);
                        if (org != null)
                        {
                            _model.parentName = org.name;
                        }

                        //_model.isMainCostCenter = id;
                        _model.status = true;
                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                        _model.isEdit = false;
                        _model.organaizationId = id.ToString();
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.Organization.Views.CreateSubCost, _model)
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
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveOrganization(OrganizationModel OrganizationModel)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    int.TryParse(OrganizationModel.id, out newId);

                    if (newId == 0)
                    {
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), OrganizationModel.id);
                    }

                    if (newId > 0)
                    {
                        if (!_accessRight.canEdit)
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
                        if (!_accessRight.canAdd)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_ADD
                            });
                        }
                    }



                    if (svc != null)
                    {
                        Organizations OrganizationEn = new Organizations();



                        OrganizationEn.name = OrganizationModel.name;
                        OrganizationEn.description = OrganizationModel.description;
                        OrganizationEn.prefix = OrganizationModel.prefix;
                        OrganizationEn.isActive = OrganizationModel.status;
                        OrganizationEn.code = OrganizationModel.code;
                        OrganizationEn.sapcode = OrganizationModel.sapcode;
                        OrganizationEn.modifiedBy = this.UserName;
                        OrganizationEn.modifiedDate = DateTime.Now;

                        if (newId > 0)
                        {
                            OrganizationEn.id = newId;

                        }
                        else
                        {
                            OrganizationEn.createdBy = this.UserName;
                            OrganizationEn.createdDate = DateTime.Now;
                            //if (!OrganizationModel.isMainCostCenter)
                            //{
                            //var org = svcOrganization.GetOrganizationById(this.SubCostCenter);
                            //if (org != null)
                            //    OrganizationEn.parent_Id = org.parent_Id;
                            var parent_Id = Common.DecryptToID(this.CurrentUserId.ToString(), OrganizationModel.parent_Id.ToString());
                            if (parent_Id != 0)
                            {
                                OrganizationEn.parent_Id = parent_Id; // Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["subCostCategoryId"]);
                                //OrganizationEn.orgCategory_Id = Common.DecryptToID(this.CurrentUserId.ToString(), OrganizationModel.orgCategory_Id.ToString());
                                OrganizationEn.orgCategory_Id = OrganizationModel.categoryType_Id;
                            }
                            else
                            {
                                OrganizationEn.orgCategory_Id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mainCostCategoryId"]);
                            }

                        }
                        bool isExists = svc.OrganizationExists(OrganizationEn);
                        if (!isExists)
                        {
                            var saveOrganization = svc.SaveOrganization(this.CurrentUserId, OrganizationEn, _accessRight.pageName);

                            if (saveOrganization != null)
                            {
                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), saveOrganization.id.ToString()),
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), saveOrganization.id.ToString()),
                                    });
                                }
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
                        else
                        {
                            return Json(new
                            {

                                status = Common.Status.Warning.ToString(),
                                message = Resources.ORGANIZATION_RECORD_EXISTS
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

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly = false, bool isTreeView = false, bool isCostCenter = false)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            OrganizationModel organizationModel = new OrganizationModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, (isCostCenter ? PAGE_URL : PAGE_URL));

                if (_accessRight != null)
                {
                    int newId = 0;
                    int.TryParse(id, out newId);

                    if (newId == 0)
                    {
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    }
                    organizationModel.readOnly = readOnly;
                    organizationModel.isTreeView = isTreeView;

                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var Organization = svc.GetOrganizationById(newId);

                            if (Organization != null)
                            {
                                organizationModel.id = id; // Common.Encrypt(this.CurrentUserId.ToString(), newId.ToString());
                                organizationModel.name = Organization.name;
                                organizationModel.description = Organization.description;
                                organizationModel.prefix = Organization.prefix;
                                organizationModel.parentName = Organization.parentName;
                                //organizationModel.orgCategoryName = Organization.orgCategoryName;
                                //organizationModel.orgCategory_Id = Common.Encrypt(this.CurrentUserId.ToString(), Organization.orgCategory_Id.ToString());
                                organizationModel.parent_Id = Organization.parent_Id == null ? Common.Encrypt(this.CurrentUserId.ToString(), "0") : Common.Encrypt(this.CurrentUserId.ToString(), Organization.parent_Id.ToString());
                                organizationModel.status = Organization.isActive;
                                organizationModel.isEdit = true;
                               // organizationModel.organaizationId = organizationModel.orgCategory_Id;// new
                                organizationModel.organaizationId = Common.Encrypt(this.CurrentUserId.ToString(), Organization.orgCategory_Id.ToString());
                                organizationModel.code = Organization.code;
                                organizationModel.sapcode = Organization.sapcode;

                                if (Organization.parentOrgCategory_Id == 0)
                                {
                                    var _orgCategory_Id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mainCostCategoryId"]);
                                    var orgCategory = svc.GetOrganizationCategoryById(_orgCategory_Id);
                                    if (orgCategory != null)
                                    {
                                        var lst = new List<SelectListItem>();
                                        lst.Add(new SelectListItem { Text = orgCategory.name, Selected = true, Value = orgCategory.id.ToString() });
                                        organizationModel.lstCategoryType = lst;
                                    }
                                }
                                else
                                {
                                    var orgCategory = svc.GetOrganizationCategoryConfigByCategoryId(Organization.parentOrgCategory_Id);
                                    if (orgCategory != null)
                                    {
                                        organizationModel.lstCategoryType = orgCategory.Select(p =>
                                            new SelectListItem
                                            {
                                                Text = p.childCategoryName,
                                                Value = p.childCategory_Id.ToString(),
                                                Selected = p.childCategory_Id == Organization.orgCategory_Id
                                            }).ToList();
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
                        if (!_accessRight.canView)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_VIEW
                            });
                        }
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.Organization.Views.Create, organizationModel)
            });
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Maintenance(string id, bool readOnly = false, bool isTreeView = false, bool isCostCenter = false)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            OrganizationModel organizationModel = new OrganizationModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, (isCostCenter ? PAGE_URL : PAGE_URL));

                if (_accessRight != null)
                {
                    int newId = 0;
                    int.TryParse(id, out newId);

                    if (newId == 0)
                    {
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    }
                    organizationModel.readOnly = readOnly;

                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var Organization = svc.GetOrganizationById(newId);

                            if (Organization != null)
                            {
                                organizationModel.isTreeView = isTreeView;
                                organizationModel.id = Common.Encrypt(this.CurrentUserId.ToString(), newId.ToString());
                                organizationModel.name = Organization.name;
                                organizationModel.description = Organization.description;
                                organizationModel.prefix = Organization.prefix;
                                organizationModel.parentName = Organization.parentName;
                                //organizationModel.orgCategoryName = Organization.orgCategoryName;
                                //organizationModel.orgCategory_Id = Common.Encrypt(this.CurrentUserId.ToString(), Organization.orgCategory_Id.ToString());
                                organizationModel.parent_Id = Organization.parent_Id == null ? Common.Encrypt(this.CurrentUserId.ToString(), "0") : Common.Encrypt(this.CurrentUserId.ToString(), Organization.parent_Id.ToString());
                                organizationModel.status = Organization.isActive;
                                organizationModel.isEdit = true;
                                // organizationModel.organaizationId = organizationModel.orgCategory_Id;// new
                                organizationModel.organaizationId = Common.Encrypt(this.CurrentUserId.ToString(), Organization.orgCategory_Id.ToString());
                                organizationModel.code = Organization.code;
                                organizationModel.sapcode = Organization.sapcode;

                                if (Organization.parentOrgCategory_Id == 0)
                                {
                                    var _orgCategory_Id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mainCostCategoryId"]);
                                    var orgCategory = svc.GetOrganizationCategoryById(_orgCategory_Id);
                                    if (orgCategory != null)
                                    {
                                        var lst=new List<SelectListItem>();
                                        lst.Add(new SelectListItem { Text = orgCategory.name, Selected = true, Value = orgCategory.id.ToString() });
                                        organizationModel.lstCategoryType = lst;
                                    }
                                }
                                else
                                {

                                    var orgCategory = svc.GetOrganizationCategoryConfigByCategoryId(Organization.parentOrgCategory_Id);
                                    if (orgCategory != null)
                                    {
                                        organizationModel.lstCategoryType = orgCategory.Select(p =>
                                            new SelectListItem
                                            {
                                                Text = p.childCategoryName,
                                                Value = p.childCategory_Id.ToString(),
                                                Selected = p.childCategory_Id == Organization.orgCategory_Id
                                            }).ToList();
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
                        if (!_accessRight.canView)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_VIEW
                            });
                        }
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.Organization.Views.Maintenance, organizationModel)
            });
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult View(string id)
        {
            return Edit(id, true,true);
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
                    int newId = 0;
                    int.TryParse(id, out newId);

                    if (newId == 0)
                    {
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    }
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteOrganization(this.CurrentUserId, newId, _accessRight.pageName);
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
