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
using System.IO;


namespace Hanodale.WebUI.Controllers
{
    [Authorize]
    public partial class UserController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "User/UserDetails";
        #endregion

        #region Constructor
        private readonly IUserService svc; private readonly ICommonService svcCommon;
        private readonly IBusinessService svcBusiness;
        private readonly IUserRightsService svcUserRights;

        public UserController(IUserService _bLService, ICommonService _commonService
            , IBusinessService _businessService
            , IUserRightsService _userRightsService)
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcBusiness = _businessService;
            this.svcUserRights = _userRightsService;
        }

        #endregion

        #region User Details
        [AppAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }

        [AppAuthorize]
        public virtual void SetSubCostCenter()
        {
            string sdf = this.CurrentUserId.ToString();
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(sdf, false);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

            var user = svc.GetUserById(this.CurrentUserId, this.CurrentUserId);
            string sessionId = Guid.NewGuid().ToString();
            var userData = new Domain.Models.User
                                {
                                    UserId = user.id,
                                    DisplayName = user.firstName,// + ' ' + user.lastName,
                                    AuthorizationId = user.id.ToString(),
                                    Language = Convert.ToInt32(user.language),
                                    SessionId = sessionId,
                                    SubCostCenter = 3
                                };

            FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, userData.ToString(), ticket.CookiePath);

            // add the encrypted ticket to the cookie as data.
            cookie.Value = FormsAuthentication.Encrypt(newticket);

            // Update the outgoing cookies collection.
            Response.Cookies.Set(cookie);


        }

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult UserDetails(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                string newId = string.IsNullOrEmpty(id) ? Common.Encrypt(this.CurrentUserId.ToString(), "0") : id;
                _accessRight.elementId = newId;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.User.Views.Index, _accessRight)
                        });
                    }
                    else
                    {
                        //Redirect to access denied page
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    //Redirect to access denied page
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
        public virtual ActionResult BindUser(DataTableModel param, string myKey)
        {
            IEnumerable<Users> filteredUser = null;
            try
            {
                var idFilter0 = Convert.ToString(Request["sSearch_0"]);
                var idFilter1 = Convert.ToString(Request["sSearch_1"]);
                var idFilter2 = Convert.ToString(Request["sSearch_2"]);
                var idFilter3 = Convert.ToString(Request["sSearch_3"]);
                var idFilter4 = Convert.ToString(Request["sSearch_4"]);
                var idFilter5 = Convert.ToString(Request["sSearch_5"]);
                int businessId = 0;
                int businessTypeId = 0;
                if (!string.IsNullOrEmpty(myKey))
                    businessId = Common.DecryptToID(this.CurrentUserId.ToString(), myKey);
                if (businessId == 0)
                {
                    businessTypeId = 65;
                }
                // Get login user Id
                int userId = this.CurrentUserId;
                bool _isActive = (idFilter0 ?? "") == "InActive" ? false : true;

                var userModel = svc.GetUser(this.CurrentUserId, businessId, param.iDisplayStart, param.iDisplayLength, param.sSearch, businessTypeId, this.SubCostCenter, param.iFilterAct, _isActive);

                if (svc != null)
                {
                    if (svc != null)
                    {
                        UserViewModel _userViewModel = new UserViewModel();
                        if (param.sColumns.StartsWith("#") && param.iSortCol_0 > 0)
                        {
                            param.iSortCol_0--;
                        }
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
               // entry.employeeNo,
                //entry.jobTitle,
                entry.roleName,
                //entry.businessName!=null?entry.businessName:"-",
                entry.status,
                Common.Encrypt(currentUserId.ToString(), entry.id.ToString())
            }).ToList();
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
                    viewMarkup = Common.RenderPartialViewToString(this, "RenderAction", _accessRight)
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
            UserModel _userModel = new UserModel();

            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight.canView && _accessRight.canAdd)
                {
                    if (svc != null)
                    {
                        // var newId = "0";
                        int business_Id = 0;
                        if (id != null)
                            business_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                        var userRoles = svcUserRights.GetUserRoles(this.CurrentUserId);
                        _userModel.UserRoles = userRoles.Select(a => new SelectListItem
                        {
                            Text = a.roleName,
                            Value = a.id.ToString()
                        });

                        //   var subCostCenters = svc.GetAllSubCostCenterByFilterId(Convert.ToInt32(ConfigurationManager.AppSettings["subCostCategoryId"]), 0, this.SubCostCenter);
                        var subCostCenters = svcCommon.GetAllSubCostCenter();
                        _userModel.lstOrganizationItem = subCostCenters.Select(a => new SelectListItem
                        {
                            Text = a.parentName + "-" + a.name,
                            Value = a.id.ToString()
                        });

                        var business = svcBusiness.GetListBusinessBybusinessType();
                        _userModel.lstBusinessItem = business.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });

                        int businessTypeId = Convert.ToInt32(WebConfigurationManager.AppSettings["BusinessType"]);

                        var businessType = svcCommon.GetListModuleItem(businessTypeId);
                        _userModel.lstBusinessType = businessType.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == 65)
                        });

                        if (business_Id == 0)
                        {
                            _userModel.UserBusinesses = new List<SelectListItem>();
                        }
                        else
                        {
                            var obj = svcBusiness.GetBusinessById(business_Id);

                            List<SelectListItem> lst = new List<SelectListItem>();

                            lst.Add(new SelectListItem
                            {
                                Text = obj.name + " ( " + obj.code + " )",
                                Value = obj.id.ToString(),
                                Selected = true
                            });

                            _userModel.UserBusinesses = lst;
                        }
                        var subCostCenter = svcCommon.GetAllSubCostCenterByFilterId(Convert.ToInt32(ConfigurationManager.AppSettings["subCostCategoryId"]), 0, this.SubCostCenter);
                        _userModel.lstOrganization = new List<SelectListItem>();
                        _userModel.status = true;
                        //   _userModel.isAll = false;

                        _userModel.passwordHash = ConfigurationManager.AppSettings["DefaultPassword"];

                        int gredId = Convert.ToInt32(WebConfigurationManager.AppSettings["GredType"]);

                        var gred = svcCommon.GetListModuleItem(gredId);
                        _userModel.lstGred = gred.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });

                        int bankId = Convert.ToInt32(WebConfigurationManager.AppSettings["BankType"]);

                        var bank = svcCommon.GetListModuleItem(bankId);
                        _userModel.lstBank = bank.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });
                        int employeeId = Convert.ToInt32(WebConfigurationManager.AppSettings["EmployeeType"]);

                        var employee = svcCommon.GetListModuleItem(employeeId);
                        _userModel.lstEmployeeGroup = employee.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString()
                        });
                        _userModel.isAccessAllOrganization = false;
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
                viewMarkup = Common.RenderPartialViewToString(this, MVC.User.Views.Create, _userModel)
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveUser(UserModel userModel)
        {
            if (userModel.birthDate == null)
                ModelState.Remove("birthDate");
            if (userModel.entryDate == null)
                ModelState.Remove("entryDate");
            if (userModel.expireddate == null)
                ModelState.Remove("expireddate");
            if (userModel.id == null)
                ModelState.Remove("id");
            if (userModel.businessType_Id == null)
                ModelState.Remove("businessType_Id");
            int check = 0;
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), userModel.id);
                    if (_accessRight != null)
                    {
                        if (newId > 0)
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
                            Users userEn = new Users();

                            userEn.firstName = userModel.firstName;
                            userEn.lastName = userModel.lastName;
                            userEn.email = userModel.email;
                            userEn.userName = userModel.userName;
                            userEn.organization_Ids = userModel.organization_Ids;
                            userEn.defaultOrganizationId = userModel.defaultOrganization_Id;
                            //if (userModel.business_Id <= 0)
                            //{
                            //    userEn.business_Id = userModel.defaultbusiness_Id;
                            //    userEn.business_Ids = userModel.business_Ids;
                            //    userEn.defaultbusiness_Id = userModel.defaultbusiness_Id;
                            //}
                            userEn.bussinessType_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Section"]);
                            //  userEn.isAll = userEn.isAll;
                            userEn.salaryId = userModel.salaryId;
                            userEn.userRole_Id = userModel.userRole_Id;
                            userEn.subCost_Id = this.SubCostCenter;
                            userEn.verified = false;
                            userEn.status = Convert.ToString(userModel.status == true ? "Active" : "InActive");
                            userEn.address = userModel.address;
                            userEn.employeeNo = userModel.employeeNo;
                            userEn.department = userModel.department;
                            userEn.jobTitle = userModel.jobTitle;
                            userEn.mobileNo = userModel.mobileNo;
                            userEn.officeNo = userModel.officeNo;
                            userEn.gred = userModel.gred;
                            userEn.idNo = userModel.idNo;
                            userEn.birthDate = userModel.birthDate;
                            userEn.age = userModel.age;
                            userEn.accountNo = userModel.accountNo;
                            userEn.bankId = userModel.bankId;
                            userEn.salary = userModel.salaryAmount;
                            userEn.employeegroupId = userModel.employeegroupId;
                            userEn.entryDate = userModel.entryDate;
                            userEn.expireddate = userModel.expireddate;
                            userEn.yearofservice = userModel.yearofservice == 0 ? null : userModel.yearofservice;
                            userEn.modifiedBy = this.UserName;
                            userEn.modifiedDate = DateTime.Now;
                            string passwordHash = string.Empty;

                            if (newId > 0)
                            {
                                userEn.id = Common.DecryptToID(this.CurrentUserId.ToString(), userModel.id);

                                DateTime? expdt = userModel.expireddate != null ? userModel.expireddate : null;
                                DateTime? entdt = userModel.entryDate != null ? userModel.entryDate : null;
                                var year = expdt.GetValueOrDefault().Year - entdt.GetValueOrDefault().Year;
                                var month = expdt.GetValueOrDefault().Month - entdt.GetValueOrDefault().Month;
                                // userEn.yearofservice = userModel.yearofservice; //Convert.ToDecimal(year);

                                DateTime? dt = userModel.birthDate != null ? userModel.birthDate : null;
                                DateTime? ds = DateTime.Now;
                                if (dt != null)
                                    userEn.age = ds.GetValueOrDefault().Year - dt.GetValueOrDefault().Year;
                            }
                            else
                            {
                                passwordHash = userModel.passwordHash;
                                userEn.passwordHash = Common.MD5(userModel.passwordHash) + ConfigurationManager.AppSettings["Encryption"].ToString();

                                userEn.createdBy = this.UserName;
                                userEn.createdDate = DateTime.Now;

                                DateTime? dt = userModel.birthDate != null ? userModel.birthDate : null;
                                DateTime? ds = DateTime.Now;
                                userEn.age = ds.GetValueOrDefault().Year - dt.GetValueOrDefault().Year;

                                DateTime? expdt = userModel.expireddate != null ? userModel.expireddate : null;
                                DateTime? entdt = userModel.entryDate != null ? userModel.entryDate : null;
                                var year = expdt.GetValueOrDefault().Year - entdt.GetValueOrDefault().Year;
                                var month = expdt.GetValueOrDefault().Month - entdt.GetValueOrDefault().Month;
                                // userEn.yearofservice = Convert.ToDecimal(year);
                            }

                            //check is "user name" exists
                            check = svc.IsUserExists(userEn);
                            if (check != 0)
                            {
                                //if (userModel.id == null)
                                //{
                                //    userModel.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                                //}
                                if (check == 1)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.USER_RECORD_STAFF_EXISTS //.Replace("$USER_NAME$", userModel.userName)
                                        //id = Common.Encrypt(this.CurrentUserId.ToString(), userModel.id)
                                    });
                                }
                                else if (check == 2)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.USER_RECORD_EMAIL_EXISTS //.Replace("$USER_NAME$", userModel.userName)
                                        //id = Common.Encrypt(this.CurrentUserId.ToString(), userModel.id)
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Warning.ToString(),
                                        message = Resources.USER_RECORD_USERNAME_EXISTS //.Replace("$USER_NAME$", userModel.userName)
                                        //id = Common.Encrypt(this.CurrentUserId.ToString(), userModel.id)
                                    });
                                }
                            }

                            var user = svc.SaveUser(this.CurrentUserId, userEn);

                            if (user != null)
                            {
                                if (newId > 0)
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
                                    if (!string.IsNullOrEmpty(userEn.email))
                                    {
                                        //send mail if user has email id    
                                        // var body = WebHelper.Placeholders.ReplaceAll(Emails.UserCreatedBody, userEn);
                                        string body = Helpers.MailSetting.OpenFile(ConfigurationManager.AppSettings["EmailPath"] + @"\" + "UserCreatedBody.txt");
                                        body = WebHelper.Placeholders.ReplaceAll(body, userEn);
                                        body = WebHelper.Placeholders.ReplaceAll(body, "$PASSWORD_HASH$", passwordHash);
                                        body = WebHelper.Placeholders.ReplaceAll(body, "$URL$", ConfigurationManager.AppSettings["DeployedUrl"]);

                                        try
                                        {
                                            //WebHelper.Mail.SendMail(Emails.UserCreatedFrom,
                                            //    new string[] { userEn.email },
                                            //    Emails.UserCreatedSubject, body);
                                        }
                                        catch (Exception err)
                                        {
                                            //throw new ErrorException("Please try again later!");
                                            return Json(new
                                            {
                                                status = Common.Status.Error.ToString(),
                                                message = "Unable to send email to " + userEn.email + "; Exception: " + err.Message,
                                                id = Common.Encrypt(this.CurrentUserId.ToString(), user.id.ToString()),
                                            });
                                        }
                                    }

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
                                if (newId > 0)
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
        public virtual ActionResult Edit(string id, string businessId, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            var _userModel = new UserModel();
            _userModel.isEdit = true;

            try
            {
                int userId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                int business_Id = 0;
                if (!string.IsNullOrEmpty(businessId))
                    business_Id = Common.DecryptToID(this.CurrentUserId.ToString(), businessId);
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
                            _userModel.firstName = user.firstName;
                            _userModel.lastName = user.lastName;
                            _userModel.email = user.email;
                            _userModel.userName = user.userName;

                            _userModel.passwordHash = "000000";

                            _userModel.userRole_Id = user.userRole_Id;

                            _userModel.status = user.status == "Active" ? true : false;

                            _userModel.address = user.address;
                            _userModel.employeeNo = user.employeeNo;
                            _userModel.department = user.department;
                            _userModel.jobTitle = user.jobTitle;
                            _userModel.mobileNo = user.mobileNo;
                            _userModel.officeNo = user.officeNo;

                            _userModel.gred = user.gred;
                            _userModel.idNo = user.idNo;
                            _userModel.birthDate = user.birthDate != null ? user.birthDate : null;
                            _userModel.age = user.age;
                            _userModel.accountNo = user.accountNo;
                            _userModel.bankId = user.bankId;
                            _userModel.salaryAmount = user.salary;
                            _userModel.employeegroupId = user.employeegroupId;
                            _userModel.entryDate = user.entryDate;
                            _userModel.expireddate = user.expireddate;
                            _userModel.yearofservice = user.yearofservice;
                            _userModel.businessType_Id = user.bussinessType_Id;
                            _userModel.salaryId = user.salaryId;
                            _userModel.isAccessAllOrganization = user.isAccessAllOrganization;


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


                        if (business_Id == 0)
                        {
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
                        }
                        else
                        {
                            var obj = svcBusiness.GetBusinessById(business_Id);

                            List<SelectListItem> lst = new List<SelectListItem>();

                            lst.Add(new SelectListItem
                            {
                                Text = obj.name + " ( " + obj.code + " )",
                                Value = obj.id.ToString(),
                                Selected = true
                            });

                            _userModel.UserBusinesses = lst;
                        }


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
            //if (_userModel.businessType_Id == 52)
            //{
            //    return Json(new
            //    {
            //        viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessUser.Views.Create, _userModel)
            //    });
            //}
            //else
            //{
            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.User.Views.Create, _userModel)
            });
            //}
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
        public virtual ActionResult View(string id, string businessId)
        {
            return Edit(id, businessId, true);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveProfileImage(UserModel model)
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {

                string fileName = this.CurrentUserId + "_" + PublicFunc.GenerateRandonCode() + "_";
                string direction = Common.GetPath(System.Configuration.ConfigurationManager.AppSettings["UserProfileFilePath"]);

                if (svc != null)
                {
                    bool hasFile = false;
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i]; //Uploaded file

                        if (!string.IsNullOrEmpty(file.FileName))
                        {

                            fileName += Path.GetFileName(file.FileName);
                            file.SaveAs(Path.Combine(@"" + direction, fileName));
                            hasFile = true;                             

                        }
                    }


                    var entity = new Users();
                    entity.isProfileImageUpload = true;
                    bool canSave = false;
                    // entity.image = model.image;
                    if (hasFile)
                    {
                        entity.HasFile = true;
                        entity.urlPath = fileName;
                        canSave = true;
                    }
                    else if (model.ImgRemoved)
                    {
                        entity.HasFile = true;
                        entity.urlPath = null;
                        canSave = true;
                    }

                    if (canSave)
                    {
                        string oldFileName = svc.GetProfileFileName(this.CurrentUserId);
                        if (oldFileName != fileName)
                        {
                            if (!string.IsNullOrEmpty(oldFileName))
                            {
                                string filePath = Path.Combine(@"" + direction, oldFileName);
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(Path.Combine(@"" + direction, oldFileName));
                                }
                            }
                        }
                        entity.modifiedBy = this.UserName;
                        entity.modifiedDate = DateTime.Now;
                        entity.id = this.CurrentUserId;
                        var save = svc.SaveUser(this.CurrentUserId, entity);
                        if (save != null)
                        {

                            return Json(new
                            {
                                status = Common.Status.Success.ToString(),
                                message = Resources.MSG_SAVE,
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
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Success.ToString(),
                            message = Resources.MSG_SAVE,
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
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

        }

        #endregion

        #region Reset Password

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
                                       // MailSetting.SaveEmail(_email);

                                        WebHelper.Mail.SendMail(
                                          Emails.ChangedPasswordFrom, new string[] { _userEn.email },
                                          Emails.UserPasswordResetSubject, _email.Description);
                                    }
                                    catch (Exception err)
                                    {
                                        //throw new ErrorException("Please try again later!");
                                        return Json(new
                                        {
                                            status = Common.Status.Warning.ToString(),
                                            message = Resources.MSG_RESET_PASSWORD+ Resources.MESSAGE_BUT+ Resources.MSG_ERR_EMAIL
                                        });
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

        #endregion

        //#region User Profile

        //[HttpPost]
        //[AppAuthorize]
        //public JsonResult Profile()
        //{
        //    try
        //    {
        //        // Get login user Id
        //        int userId = this.CurrentUserId;

        //        ChannelFactory<IConfigurationService> configurationProxy = new ChannelFactory<IConfigurationService>("ConfigurationService");
        //        IConfigurationService svc = configurationProxy.CreateChannel();
        //        CultureHelper culturehelper = new CultureHelper();
        //        var userProfile = svc.GetUserProfile(userId);

        //        UserModel _userModel = new UserModel();
        //        if (userProfile != null)
        //        {
        //            _userModel.id = userProfile.id;
        //            _userModel.firstName = userProfile.firstName;
        //            _userModel.lastName = userProfile.lastName;
        //            _userModel.email = userProfile.email;
        //            _userModel.userName = userProfile.userName;
        //            _userModel.UserLocations = userProfile.UserLocations;
        //            _userModel.UserRole = userProfile.UserRole;
        //            _userModel.Languages = culturehelper.GetLanguages();
        //            _userModel.language = userProfile.language;
        //        }
        //        return Json(new
        //        {
        //            viewMarkup = Common.RenderPartialViewToString(this, "Profile", _userModel)
        //        });
        //    }
        //    catch (Exception err)
        //    {
        //        throw new ErrorException(err.Message);
        //    }
        //}

        //[HttpPost]
        //[Authorize]
        //public JsonResult UpdateProfile(UserModel profile)
        //{
        //    try
        //    {
        //        ChannelFactory<IConfigurationService> configurationProxy = new ChannelFactory<IConfigurationService>("ConfigurationService");
        //        IConfigurationService svc = configurationProxy.CreateChannel();
        //        if (svc != null)
        //        {
        //            User userEn = new User();
        //            userEn.id = this.CurrentUserId;
        //            userEn.language = profile.language;
        //            userEn.modifiedDate = DateTime.Now;
        //            userEn.modifiedBy = this.UserName;
        //            var updateProfile = svc.UpdateUserProfile(userEn);

        //            if (updateProfile != null)
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Success.ToString(),
        //                    message = Resources.MSG_UPDATE
        //                });
        //            }
        //            else
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Error.ToString(),
        //                    message = Resources.MSG_ERR_UPDATE
        //                });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                status = Common.Status.Error.ToString(),
        //                message = Resources.MSG_ERR_SERVICE
        //            });
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        throw new ErrorException(err.Message);
        //    }
        //}

        //#endregion

        //#region MD5 Encryption
        ///// <summary>
        ///// Encodes a string into MD5 encryption
        ///// </summary>
        ///// <param name="password">String to encode</param>
        ///// <returns>MD5 version of the string</returns>
        //private static string MD5(string password)
        //{
        //    byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
        //    try
        //    {
        //        System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
        //        cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //        byte[] hash = cryptHandler.ComputeHash(textBytes);
        //        string ret = "";
        //        foreach (byte a in hash)
        //        {
        //            if (a < 16)
        //                ret += "0" + a.ToString("x");
        //            else
        //                ret += a.ToString("x");
        //        }
        //        return ret;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //#endregion

        #region DropDown

        [HttpPost]
        public virtual JsonResult LoadBusiness(int businessType_Id)
        {

            UserModel _model = new UserModel();
            var user = svcBusiness.GetListBusiness(businessType_Id);
            _model.lstBusinessItem = user.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.id.ToString()
            });
            var result = _model.lstBusinessItem;
            return Json(result);
        }

        [HttpPost]
        public virtual JsonResult LoadOrganization()
        {

            UserModel _model = new UserModel();
            var user = svcCommon.GetAllSubCostCenter();
            _model.lstOrganization = user.Select(a => new SelectListItem
            {
                Text = a.parentName + "-" + a.name,
                Value = a.id.ToString()
            });
            var result = _model.lstOrganization;
            return Json(result);
        }

        #endregion

        #region UserInfo
        [HttpPost]
        public virtual JsonResult UserInfo()
        {
            try
            {
                var _userInfoModel = new UserInfoModel();
                var user = svc.GetUserBySCId(this.CurrentUserId, this.CurrentUserId, this.SubCostCenter);

                if (user != null)
                {

                    _userInfoModel.readOnly = false;
                    _userInfoModel.address = user.address;
                    _userInfoModel.employeeNo = user.employeeNo;
                    _userInfoModel.department = user.department;
                    _userInfoModel.jobTitle = user.jobTitle;
                    _userInfoModel.mobileNo = user.mobileNo;
                    _userInfoModel.officeNo = user.officeNo;

                    _userInfoModel.gred = user.gred;
                    _userInfoModel.idNo = user.idNo;
                    _userInfoModel.birthDate = user.birthDate != null ? user.birthDate : null;
                    _userInfoModel.age = user.age;
                    _userInfoModel.accountNo = user.accountNo;
                    _userInfoModel.bankId = user.bankId;
                    _userInfoModel.salaryAmount = user.salary;
                    _userInfoModel.employeegroupId = user.employeegroupId;
                    _userInfoModel.entryDate = user.entryDate;
                    _userInfoModel.expireddate = user.expireddate;
                    _userInfoModel.yearofservice = user.yearofservice;
                    _userInfoModel.salaryId = user.salaryId;
                    _userInfoModel.URLPath = user.urlPath;
                    _userInfoModel.HasFile = user.urlPath != null;
                    if (string.IsNullOrEmpty(user.urlPath))
                        _userInfoModel.URLPathPreview = "/Images/empty.png";
                    else
                        _userInfoModel.URLPathPreview = System.Configuration.ConfigurationManager.AppSettings["UserProfileFilePath"] + "/" + user.urlPath;


                }

                int gredId = Convert.ToInt32(WebConfigurationManager.AppSettings["GredType"]);

                var gred = svcCommon.GetListModuleItem(gredId);
                _userInfoModel.lstGred = gred.Select(a => new SelectListItem
                {
                    Text = a.name,
                    Value = a.id.ToString(),
                    Selected = (a.id == user.gred)
                });

                int bankId = Convert.ToInt32(WebConfigurationManager.AppSettings["BankType"]);

                var bank = svcCommon.GetListModuleItem(bankId);
                _userInfoModel.lstBank = bank.Select(a => new SelectListItem
                {
                    Text = a.name,
                    Value = a.id.ToString(),
                    Selected = (a.id == user.bankId)
                });
                int employeeId = Convert.ToInt32(WebConfigurationManager.AppSettings["EmployeeType"]);

                var employee = svcCommon.GetListModuleItem(employeeId);
                _userInfoModel.lstEmployeeGroup = employee.Select(a => new SelectListItem
                {
                    Text = a.name,
                    Value = a.id.ToString(),
                    Selected = (a.id == user.employeegroupId)
                });

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.User.Views.UserDetails, _userInfoModel)
                });
            }
            catch (Exception err)
            {
                return Json(new
                {
                    status = Common.Status.Error.ToString(),
                    message = Resources.NO_ACCESS_RIGHTS
                });
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveUserInfo(UserInfoModel userModel)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), userModel.id);
                    if (_accessRight != null)
                    {
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = "Access " + Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
                        }

                        if (svc != null)
                        {
                            Users userEn = new Users();

                            userEn.id = this.CurrentUserId;
                            userEn.salaryId = userModel.salaryId;
                            userEn.employeeNo = userModel.employeeNo;
                            userEn.department = userModel.department;
                            userEn.jobTitle = userModel.jobTitle;
                            userEn.mobileNo = userModel.mobileNo;
                            userEn.officeNo = userModel.officeNo;
                            userEn.address = userModel.address;
                            userEn.gred = userModel.gred;
                            userEn.idNo = userModel.idNo;
                            userEn.birthDate = userModel.birthDate;
                            userEn.age = userModel.age;
                            userEn.accountNo = userModel.accountNo;
                            userEn.bankId = userModel.bankId;
                            userEn.salary = userModel.salaryAmount;
                            userEn.employeegroupId = userModel.employeegroupId;
                            userEn.entryDate = userModel.entryDate;
                            userEn.expireddate = userModel.expireddate;
                            userEn.yearofservice = userModel.yearofservice == 0 ? null : userModel.yearofservice;
                            userEn.modifiedBy = this.UserName;
                            userEn.modifiedDate = DateTime.Now;

                            //if (newId > 0)
                            //{
                            //    userEn.id = Common.DecryptToID(this.CurrentUserId.ToString(), userModel.id);

                            DateTime? expdt = userModel.expireddate != null ? userModel.expireddate : null;
                            DateTime? entdt = userModel.entryDate != null ? userModel.entryDate : null;
                            if (expdt != null && entdt != null)
                            {
                                var year = expdt.GetValueOrDefault().Year - entdt.GetValueOrDefault().Year;
                                var month = expdt.GetValueOrDefault().Month - entdt.GetValueOrDefault().Month;
                                userEn.yearofservice = Convert.ToDecimal(year);

                            }
                            DateTime? dt = userModel.birthDate != null ? userModel.birthDate : null;
                            DateTime? ds = DateTime.Now;
                            if (dt != null)
                                userEn.age = ds.GetValueOrDefault().Year - dt.GetValueOrDefault().Year;
                            //}


                            var user = svc.UpdateUserInfo(userEn);

                            if (user != null)
                            {
                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE,
                                        //id = Common.Encrypt(this.CurrentUserId.ToString(), user.id.ToString()),
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE,
                                        //id = Common.Encrypt(this.CurrentUserId.ToString(), user.id.ToString()),
                                    });
                                }
                            }
                            else
                            {
                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Error.ToString(),
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

        #endregion

    }
}
