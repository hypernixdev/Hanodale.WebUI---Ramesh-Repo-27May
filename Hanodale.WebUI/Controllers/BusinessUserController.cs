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
using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using Hanodale.Utility;
using System.Web;
using System.Web.Security;
using System.ServiceModel;
using System.Web.Configuration;


namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class BusinessUserController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "Business/Index";
        #endregion

        #region Constructor
        private readonly IUserService svc; private readonly ICommonService svcCommon;
        private readonly IBusinessService svcBusiness;
        private readonly IUserRightsService svcUserRights;

        public BusinessUserController(IUserService _bLService, ICommonService _commonService
            , IBusinessService _businessService
            , IUserRightsService _userRightsService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcBusiness = _businessService;
            this.svcUserRights = _userRightsService;
        }

        #endregion

        #region BusinessUser Details

        [AppAuthorize]
        public virtual ActionResult Index(string id, bool readOnly)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _accessRight.elementId = id;
                var business = svcBusiness.GetBusinessById(Common.DecryptToID(this.CurrentUserId.ToString(), id));
                if (business != null)
                {
                    if (business.businessType_Id == 52)
                    {
                        _accessRight.readOnly = readOnly;
                    }
                    else
                    {
                        _accessRight.readOnly = true;
                    }
                }
                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessUser.Views.Index, _accessRight)
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
        public virtual ActionResult BindBusinessUser(DataTableModel param, string myKey)
        {
            IEnumerable<Users> filteredUser = null;
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

                var idFilter0 = Convert.ToString(Request["sSearch_0"]);
                int businessId = 0;
                int businessTypeId = 52;
                if (!string.IsNullOrEmpty(myKey))
                    businessId = Common.DecryptToID(this.CurrentUserId.ToString(), myKey);
                var userModel = svc.GetUser(this.CurrentUserId, businessId, param.iDisplayStart, param.iDisplayLength, param.sSearch, businessTypeId, this.SubCostCenter, param.iFilterAct, true);

                if (svc != null)
                {
                    if (svc != null)
                    {
                        UserViewModel _userViewModel = new UserViewModel();
                        //Sorting
                        var sortColumnIndex = param.iSortCol_0;
                        Func<Users, string> orderingFunction = (c => sortColumnIndex == 0 ? c.firstName :
                                                        sortColumnIndex == 1 ? c.email :
                                                        sortColumnIndex == 2 ? c.employeeNo :
                                                        sortColumnIndex == 3 ? c.jobTitle :
                                                        sortColumnIndex == 4 ? c.roleName :
                                                        sortColumnIndex == 5 ? c.businessName : c.status
                                                        );

                        filteredUser = userModel.lstUsers;
                        if (param.sSortDir_0 != null)
                        {
                            if (param.sSortDir_0 == "asc")
                                filteredUser = filteredUser.OrderBy(orderingFunction);
                            else
                                filteredUser = filteredUser.OrderByDescending(orderingFunction);
                        }
                    }

                var result = UserData(filteredUser, this.CurrentUserId);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = userModel.recordDetails.totalRecords,
                    iTotalDisplayRecords = userModel.recordDetails.totalDisplayRecords,
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

        //<summary>
        //This method is to get the user data as string array to bind into datatbale
        //</summary>
        //<param name="userEntry">User list</param>
        //<returns></returns>
        public static List<string[]> UserData(IEnumerable<Users> userEntry, int currentUserId)
        {
            return userEntry.Select(entry => new string[]
            {
                entry.firstName+" "+entry.lastName,
                entry.email,
                entry.employeeNo,
                entry.jobTitle,
                entry.roleName,
                entry.businessName!=null?entry.businessName:"-",
                entry.status,
                Common.Encrypt(currentUserId.ToString(), entry.id.ToString())
            }).ToList();
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction(bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.readOnly = readOnly;
                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessUser.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        #endregion

        #region Add,Edit and Delete

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            BusinessUserModel _model = new BusinessUserModel();

            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight.canView && _accessRight.canAdd)
                {
                    if (svc != null)
                    {
                        int business_Id = 0;
                        if (id != null)
                            business_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);


                        _model.business_Id = id;
                        var userRoles = svcUserRights.GetUserRoles(this.CurrentUserId);
                        _model.UserRoles = userRoles.Select(a => new SelectListItem
                        {
                            Text = a.roleName,
                            Value = a.id.ToString()
                        });

                        //   var subCostCenters = svc.GetAllSubCostCenterByFilterId(Convert.ToInt32(ConfigurationManager.AppSettings["subCostCategoryId"]), 0, this.SubCostCenter);
                        var subCostCenters = svcCommon.GetAllSubCostCenter();
                        _model.lstOrganizationItem = subCostCenters.Select(a => new SelectListItem
                        {
                            Text = a.parentName + "-" + a.name,
                            Value = a.id.ToString()
                        });

                        var obj = svcBusiness.GetBusinessById(business_Id);
                        int businessType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["SupplierBusinessType"]);
                        var business = svcBusiness.GetListBusinessByBusinessTypeId(businessType_Id);
                        _model.UserBusinesses = business.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == obj.id)
                        });

                        int businessTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessType"]);

                        var businessType = svcCommon.GetListModuleItem(businessTypeId);
                        _model.lstBusinessType = businessType.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == 52)
                        });

                        //List<SelectListItem> lst = new List<SelectListItem>();

                        //lst.Add(new SelectListItem
                        //{
                        //    Text = obj.name + " ( " + obj.code + " )",
                        //    Value = obj.id.ToString(),
                        //    Selected = true
                        //});

                        //_userModel.UserBusinesses = lst;

                        var subCostCenter = svcCommon.GetAllSubCostCenterByFilterId(Convert.ToInt32(ConfigurationManager.AppSettings["subCostCategoryId"]), 0, this.SubCostCenter);
                        _model.lstOrganization = new List<SelectListItem>();
                        _model.status = true;
                        //   _userModel.isAll = false;

                        _model.passwordHash = ConfigurationManager.AppSettings["DefaultPassword"];

                        //int gredId = Convert.ToInt32(WebConfigurationManager.AppSettings["GredType"]);

                        //var gred = svcCommon.GetListModuleItem(gredId);
                        //_userModel.lstGred = gred.Select(a => new SelectListItem
                        //{
                        //    Text = a.name,
                        //    Value = a.id.ToString()
                        //});

                        //int bankId = Convert.ToInt32(WebConfigurationManager.AppSettings["BankType"]);

                        //var bank = svcCommon.GetListModuleItem(bankId);
                        //_userModel.lstBank = bank.Select(a => new SelectListItem
                        //{
                        //    Text = a.name,
                        //    Value = a.id.ToString()
                        //});
                        //int employeeId = Convert.ToInt32(WebConfigurationManager.AppSettings["EmployeeType"]);

                        //var employee = svcCommon.GetListModuleItem(employeeId);
                        //_userModel.lstEmployeeGroup = employee.Select(a => new SelectListItem
                        //{
                        //    Text = a.name,
                        //    Value = a.id.ToString()
                        //});
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
                        message = Resources.NO_ACCESS_RIGHTS_ADD
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessUser.Views.Create, _model)
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveBusinessUser(BusinessUserModel model)
        {
            int check = 0;
            if (model.businessType_Id == null)
                ModelState.Remove("businessType_Id");
            ModelState.Remove("defaultbusiness_Id");
            ModelState.Remove("organization_Ids");
            ModelState.Remove("defaultOrganization_Id");
            ModelState.Remove("status");
            ModelState.Remove("username");

            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        if (model.id != null)
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
                 

                        if (svc != null)
                        {
                            Users userEn = new Users();

                            //userEn.id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id.ToString());

                            userEn.firstName = model.firstName;
                            userEn.lastName = model.lastName;
                            userEn.email = model.email;
                            userEn.userName = model.userName;
                            userEn.organization_Ids = model.organization_Ids;
                            userEn.defaultOrganizationId = 31;
                            //userEn.business_Ids = Array.ConvertAll(model.defaultbusiness_Id.ToString().ToArray(), x => (int)x);
                            //model.defaultbusiness_Id.ToString().Select(o => Convert.ToInt32(o)).ToArray();
                            //model.defaultbusiness_Id.ToString().Cast<int>().ToArray(); 
                            userEn.defaultbusiness_Id = model.defaultbusiness_Id;
                            userEn.bussinessType_Id = 52;
                            //userEn.salaryId = model.salaryId;

                            userEn.userRole_Id = 2217;// model.userRole_Id;
                            userEn.business_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.business_Id.ToString());
                            userEn.subCost_Id = this.SubCostCenter;
                            userEn.verified = false;
                            userEn.status = "Active";
                            //  userEn.address = userModel.address;
                            //  userEn.employeeNo = model.employeeNo;
                            userEn.department = model.department;
                            userEn.jobTitle = model.jobTitle;
                            userEn.mobileNo = model.mobileNo;
                            userEn.officeNo = model.officeNo;

                            //  userEn.gred = userModel.gred;
                            //  userEn.idNo = userModel.idNo;
                            //  userEn.birthDate = userModel.birthDate;
                            //  userEn.age = userModel.age;
                            //  userEn.accountNo = userModel.accountNo;
                            //  userEn.bankId = userModel.bankId;
                            //  userEn.salary = userModel.salaryAmount;
                            //  userEn.employeegroupId = userModel.employeegroupId;
                            //  userEn.entryDate = userModel.entryDate;
                            //  userEn.expireddate = userModel.expireddate;
                            //  if (userModel.yearofservice == 0)
                            //  {
                            //      userModel.yearofservice = null;
                            //  }
                            //  else
                            //  {
                            //      userEn.yearofservice = userModel.yearofservice;
                            //  }

                            userEn.modifiedBy = this.UserName;
                            userEn.modifiedDate = DateTime.Now;
                            string passwordHash = string.Empty;

                            if (model.id != null)
                            {
                                userEn.id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                                //DateTime expdt = Convert.ToDateTime(userModel.expireddate);
                                //DateTime entdt = Convert.ToDateTime(userModel.entryDate);
                                //var year = expdt.Year - entdt.Year;
                                //var month = expdt.Month - entdt.Month;
                                //userEn.yearofservice = Convert.ToDecimal(year + "." + month);
                                //DateTime dt = Convert.ToDateTime(userModel.birthDate);
                                //DateTime ds = DateTime.Now;
                                //userEn.age = ds.Year - dt.Year;
                            }
                            else
                            {
                                passwordHash = model.passwordHash;
                                userEn.passwordHash = Common.MD5(model.passwordHash) + ConfigurationManager.AppSettings["Encryption"].ToString();

                                userEn.createdBy = this.UserName;
                                userEn.createdDate = DateTime.Now;
                                //DateTime dt = Convert.ToDateTime(userModel.birthDate);
                                //DateTime ds = DateTime.Now;
                                //userEn.age = ds.Year - dt.Year;
                                //DateTime expdt = Convert.ToDateTime(userModel.expireddate);
                                //DateTime entdt = Convert.ToDateTime(userModel.entryDate);
                                //var year = expdt.Year - entdt.Year;
                                //// var month = expdt.Month - entdt.Month;
                                //userEn.yearofservice = Convert.ToDecimal(year);
                            }

                            //check is "user name" exists
                            check = svc.IsUserExists(userEn);
                            if (check != 0)
                            {
                                if (model.id == null)
                                {
                                    model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                                }
                                if (check == 1)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.USER_RECORD_STAFF_EXISTS, //.Replace("$USER_NAME$", userModel.userName)
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), model.id)
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.USER_RECORD_EMAIL_EXISTS, //.Replace("$USER_NAME$", userModel.userName)
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), model.id)
                                    });
                                }
                            }

                            var user = svc.SaveBusinessUser(this.CurrentUserId, userEn);

                            if (user != null)
                            {
                               // MailSetting.SendMailUserData(user);
                                if (model.id != null)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), user.id.ToString()),
                                    });
                                }
                                else
                                {
                                    //if (!string.IsNullOrEmpty(userEn.email))
                                    //{
                                    //    //send mail if user has email id    
                                    //    //var body = WebHelper.Placeholders.ReplaceAll(Emails.UserCreatedBody, userEn);
                                    //    string body = Helpers.MailSetting.OpenFile(ConfigurationManager.AppSettings["EmailPath"] + @"\" + "UserCreatedBody.txt");
                                    //    body = WebHelper.Placeholders.ReplaceAll(body, userEn);
                                    //    body = WebHelper.Placeholders.ReplaceAll(body, "$PASSWORD_HASH$", passwordHash);
                                    //    body = WebHelper.Placeholders.ReplaceAll(body, "$URL$", ConfigurationManager.AppSettings["DeployedUrl"]);

                                    //    try
                                    //    {
                                    //        WebHelper.Mail.SendMail(Emails.UserCreatedFrom,
                                    //            new string[] { userEn.email },
                                    //            Emails.UserCreatedSubject, body);
                                    //    }
                                    //    catch (Exception err)
                                    //    {
                                    //        throw new ErrorException("Please try again later!");
                                    //    }
                                    //}

                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), user.id.ToString()),
                                    });
                                }
                            }
                            else
                            {
                                if (model.id != null)
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
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_INVALIDMODEL
            });
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult ResetPassword(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                int userId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            Users _userEn = new Users();
                            _userEn.id = userId;
                            _userEn.userName = UserName;
                            _userEn.passwordHash = Common.MD5(ConfigurationManager.AppSettings["DefaultPassword"]) + ConfigurationManager.AppSettings["Encryption"].ToString();
                            _userEn.verified = false;
                            _userEn.modifiedBy = this.UserName;
                            _userEn.modifiedDate = DateTime.Now;

                            _userEn = svc.ResetPassword(_userEn);
                            if (_userEn != null)
                            {
                                if (!string.IsNullOrEmpty(_userEn.email))
                                {
                                    //send mail if user has email id    
                                    // var body = WebHelper.Placeholders.ReplaceAll(Emails.UserPasswordResetBody, _userEn);
                                    string body = Helpers.MailSetting.OpenFile(ConfigurationManager.AppSettings["EmailPath"] + @"\" + "UserPasswordResetBody.txt");
                                    body = WebHelper.Placeholders.ReplaceAll(body, _userEn);
                                    body = WebHelper.Placeholders.ReplaceAll(body, "$USER_EMAIL$", _userEn.email);
                                    body = WebHelper.Placeholders.ReplaceAll(body, "$USERNAME$", _userEn.userName);
                                    body = WebHelper.Placeholders.ReplaceAll(body, "$PASSWORD_HASH$", ConfigurationManager.AppSettings["DefaultPassword"]);
                                    body = WebHelper.Placeholders.ReplaceAll(body, "$URL$", ConfigurationManager.AppSettings["DeployedUrl"]);

                                    try
                                    {
                                        //WebHelper.Mail.SendMail(Emails.UserCreatedFrom,
                                        //    new string[] { _userEn.email },
                                        //    Emails.UserPasswordResetSubject, body);

                                        Email _email = new Email();
                                        _email.Subject = Emails.UserPasswordResetSubject;
                                        _email.Description = WebHelper.Placeholders.ReplaceAll(body, _userEn); // WebHelper.Placeholders.ReplaceAll(Emails.ChangedPasswordBody, user);
                                        _email.ToId = _userEn.email;
                                        _email.CcId = "";
                                        _email.createdDate = DateTime.Now;
                                        _email.createdBy = _userEn.firstName;
                                        MailSetting.SaveEmail(_email);
                                    }
                                    catch (Exception err)
                                    {
                                        throw new ErrorException("Please try again later!");
                                    }
                                }

                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_RESET_PASSWORD
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Error.ToString(),
                                    message = Resources.MSG_RESET_PASSWORD_ERR
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
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Edit(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            BusinessUserModel _userModel = new BusinessUserModel();
            _userModel.isEdit = true;

            try
            {
                int userId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _userModel.readOnly = readOnly;
                if (_accessRight.canView || _accessRight.canEdit)
                {
                    if (svc != null)
                    {
                        //get user by id
                        var user = svc.GetUserBySCId(this.CurrentUserId, userId, this.SubCostCenter);

                        if (user != null)
                        {
                            _userModel.id = id;
                            _userModel.business_Id = Common.Encrypt(this.CurrentUserId.ToString(), user.business_Id.ToString());
                            _userModel.firstName = user.firstName;
                            _userModel.lastName = user.lastName;
                            _userModel.email = user.email;
                            _userModel.userName = user.userName;

                            _userModel.passwordHash = "000000";

                            _userModel.userRole_Id = user.userRole_Id;

                            _userModel.status = user.status == "Active" ? true : false;

                           // _userModel.address = user.address;
                           // _userModel.employeeNo = user.employeeNo;
                            _userModel.department = user.department;
                            _userModel.jobTitle = user.jobTitle;
                            _userModel.mobileNo = user.mobileNo;
                            _userModel.officeNo = user.officeNo;
                          //  _userModel.gred = user.gred;
                           // _userModel.idNo = user.idNo;
                          //  _userModel.birthDate = user.birthDate != null ? user.birthDate : null;
                         //   _userModel.age = user.age;
                        //    _userModel.accountNo = user.accountNo;
                         //   _userModel.bankId = user.bankId;
                        //    _userModel.salaryAmount = user.salary;
                         //   _userModel.employeegroupId = user.employeegroupId;
                        //    _userModel.entryDate = user.entryDate;
                        //    _userModel.expireddate = user.expireddate;
                        //    _userModel.yearofservice = user.yearofservice;
                            _userModel.businessType_Id = user.bussinessType_Id;
                        //    _userModel.salaryId = user.salaryId;
                            _userModel.isAccessAllOrganization = user.isAccessAllOrganization;
                       //     _userModel.isBack = true;
                        }

                        var userRoles = svcUserRights.GetUserRoles(this.CurrentUserId);

                        _userModel.UserRoles = userRoles.Select(a => new SelectListItem
                        {
                            Text = a.roleName,
                            Value = a.id.ToString(),
                            Selected = (a.id == user.userRole_Id)
                        });

                        int businessTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessType"]);
                        var businessType = svcCommon.GetListModuleItem(businessTypeId);
                        _userModel.lstBusinessType = businessType.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == user.bussinessType_Id)
                        });

                        var business = svcBusiness.GetListBusinessBybusinessType();

                        _userModel.lstBusinessItem = business.Select(a => new SelectListItem
                        {
                            Text = a.name + " ( " + a.code + " )",
                            Value = a.id.ToString(),
                            Selected = (user.business_Ids.Contains(a.id))
                        });
                        _userModel.UserBusinesses = business.Where(p => user.business_Ids.Contains(p.id)).Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = user.business_Ids.Contains(a.id)
                        });

                        // var subCostCenter = svc.GetAllSubCostCenterByFilterId(Convert.ToInt32(ConfigurationManager.AppSettings["subCostCategoryId"]), userId, this.SubCostCenter).ToList();
                        // var subCostCenters = svc.GetAllSubCostCenterByFilterId(Convert.ToInt32(ConfigurationManager.AppSettings["subCostCategoryId"]), userId, this.SubCostCenter).ToList();
                        var subCostCenters = svcCommon.GetAllSubCostCenter();
                        _userModel.lstOrganizationItem = subCostCenters.Select(a => new SelectListItem
                        {
                            Text = a.parentName + "-" + a.name,
                            Value = a.id.ToString(),
                            Selected = (user.organization_Ids.Contains(a.id))
                        });

                        _userModel.lstOrganization = subCostCenters.Where(p => user.isAccessAllOrganization == true ? true : user.organization_Ids.Contains(p.id)).Select(a => new SelectListItem
                        {
                            Text = a.parentName + "-" + a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == user.defaultOrganizationId)
                        });

                        //var defaultOrganization = subCostCenters.Where(p => user.organization_Ids.Contains(p.id));

                        //if (defaultOrganization != null)
                        //{
                        //    _userModel.lstDefaultOrganizationItem = defaultOrganization.Select(a => new SelectListItem
                        //    {
                        //        Text = a.name,
                        //        Value = a.id.ToString(),
                        //        Selected = (a.id == user.defaultOrganizationId)
                        //    }).ToList();
                        //}

                        int gredId = Convert.ToInt32(WebConfigurationManager.AppSettings["GredType"]);

                        var gred = svcCommon.GetListModuleItem(gredId);
                        _userModel.lstGred = gred.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == user.gred)
                        });

                        int bankId = Convert.ToInt32(WebConfigurationManager.AppSettings["BankType"]);

                        var bank = svcCommon.GetListModuleItem(bankId);
                        _userModel.lstBank = bank.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == user.bankId)
                        });
                        int employeeId = Convert.ToInt32(WebConfigurationManager.AppSettings["EmployeeType"]);

                        var employee = svcCommon.GetListModuleItem(employeeId);
                        _userModel.lstEmployeeGroup = employee.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == user.employeegroupId)
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
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessUser.Views.Create, _userModel)
            });
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                int userId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        if (userId == this.CurrentUserId)
                        {
                            return Json(new
                            {
                                status = Common.Status.Warning.ToString(),
                                message = Resources.USER_DELETE_WARNING
                            });
                        }
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteUser(this.CurrentUserId, userId);
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
                        //Redirect to access denied page
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

        [HttpPost]
        [Authorize]
        public virtual ActionResult View(string id)
        {
            return Edit(id, true);
        }

        #endregion

    }
}
