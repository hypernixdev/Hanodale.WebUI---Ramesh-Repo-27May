using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Models;
//using Hanodale.WebUI.MailService;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel;
using System.Web.Mvc;
using System.Linq;
using Hanodale.Utility;
using Hanodale.Utility.Globalize;
using Hanodale.WebUI.Logging.Elmah;
using System;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.IO;
using System.Globalization;

namespace Hanodale.WebUI.Controllers
{
    public partial class HelpDeskController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "HelpDesk/Index";
        #endregion

        #region Constructor

        private readonly IHelpDeskService svc; private readonly ICommonService svcCommon;
        private readonly IOrganizationService svcOrganization;
        private readonly IUserService svcUser;
        private readonly IBusinessService svcBusiness;

        public HelpDeskController(IHelpDeskService _bLService, ICommonService _commonService
            , IOrganizationService _organizationService
, IUserService _userService
, IBusinessService _businessService)
        {
            this.svc = _bLService; this.svcCommon = _commonService;
            this.svcOrganization = _organizationService;
            this.svcUser = _userService;
            this.svcBusiness = _businessService;
        }
        #endregion

        #region HelpDesk Details
        [AppAuthorize]
        public virtual ActionResult Index(string search = null)
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _accessRight.elementId = search ?? "";

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.Index, _accessRight)
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
        public virtual ActionResult BindHelpDesk(DataTableModel param, string myKey)
        {
            int totalRecordCount = 0;
            IEnumerable<HelpDesks> filteredHelpDesks = null;
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        var idFilter0 = Convert.ToString(Request["sSearch_0"]).Trim();
                        var idFilter1 = Convert.ToString(Request["sSearch_1"]).Trim();
                        var idFilter2 = Convert.ToString(Request["sSearch_2"]).Trim();
                        var idFilter3 = Convert.ToString(Request["sSearch_3"]).Trim();
                        var idFilter4 = Convert.ToString(Request["sSearch_4"]).Trim();

                        // Get login user Id
                        int userId = this.CurrentUserId;
                        int stockId = 0;
                        var filterEntity = new HelpDesks();
                        if (myKey != null)
                            stockId = Common.DecryptToID(this.CurrentUserId.ToString(), myKey);

                        if (!string.IsNullOrEmpty(idFilter0) || !string.IsNullOrEmpty(idFilter1) || !string.IsNullOrEmpty(idFilter2) || !string.IsNullOrEmpty(idFilter3) || !string.IsNullOrEmpty(idFilter4))
                        {
                            if (!string.IsNullOrEmpty(idFilter0))
                            {
                                filterEntity.createdDateFrom = DateTime.ParseExact(idFilter0, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            if (!string.IsNullOrEmpty(idFilter1))
                            {
                                filterEntity.createdDateTo = DateTime.ParseExact(idFilter1, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1);
                            }
                            if (!string.IsNullOrEmpty(idFilter2))
                            {
                                filterEntity.workFollowStatus_Id = Convert.ToInt32(idFilter2);
                            }
                            if (!string.IsNullOrEmpty(idFilter3))
                            {
                                filterEntity.feedbackModule = idFilter3;
                            }

                        }

                        var helpDeskModel = this.svc.GetHelpDesk(this.CurrentUserId, this.SubCostCenter, param.iDisplayStart, param.iDisplayLength, param.sSearch, filterEntity);

                        if (svc != null)
                        {
                            HelpDeskViewModel _helpDeskViewModel = new HelpDeskViewModel();

                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            if (param.sSortDir_0 == null)
                                sortColumnIndex--;
                            Func<HelpDesks, string> orderingFunction = (c => sortColumnIndex == 1 ? c.code :
                                                            sortColumnIndex == 2 ? c.feedback :
                                                            sortColumnIndex == 3 ? c.createdDate.ToString("dd/MM/yyyy hh:mm ttt") :
                                                            sortColumnIndex == 4 ? c.receivedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") : c.workFollowStatusName
                                                            );

                            filteredHelpDesks = helpDeskModel.lstHelpDesk;
                            //param.sSortDir_0 = sortColumnIndex >= 0 ? param.sSortDir_0 : "desc";
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredHelpDesks = filteredHelpDesks.OrderBy(orderingFunction);
                                else
                                    filteredHelpDesks = filteredHelpDesks.OrderByDescending(orderingFunction);
                            }

                            var result = HelpDeskData(filteredHelpDesks, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = helpDeskModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = helpDeskModel.recordDetails.totalDisplayRecords,
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
        /// This method is to get the HelpDesk data as string array to bind into datatbale
        /// </summary>
        /// <param name="userEntry">HelpDesk list</param>
        /// <returns></returns>
        public List<string[]> HelpDeskData(IEnumerable<HelpDesks> userEntry, int currentUserId)
        {
            return userEntry.Select(entry => new string[]
            {  
                GetFlag(currentUserId,entry) + entry.code,
                entry.feedback,
                entry.createdDate.ToString("dd/MM/yyyy hh:mm ttt") + "</br>" + entry.name,
                entry.receivedDate != null ? entry.receivedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") + "</br>" + entry.receivedBy : "",
                GetColorCodes(currentUserId,entry),
               Common.Encrypt(currentUserId.ToString(), entry.id.ToString()),
               GetUserRights(currentUserId,entry)
            }).ToList();
        }

        public string GetUserRights(int userId, HelpDesks _helpdeskEn)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            if (_helpdeskEn.workFollowStatus_Id == 6547) //new
            {
                return "30";
            }
            else if (_helpdeskEn.workFollowStatus_Id == 98 ||
                _helpdeskEn.workFollowStatus_Id == 6549 ||
                _helpdeskEn.workFollowStatus_Id == 6548)
            {
                if (_accessRight.canView)
                {
                    if (_helpdeskEn.woCodes.Count() > 0)
                    {
                        return "3";
                    }
                    else
                    {
                        return "2";
                    }
                }
                else
                {
                    return "0";
                }
            }
            else if (_helpdeskEn.workFollowStatus_Id == 2904 ||
                _helpdeskEn.workFollowStatus_Id == 96 ||
                _helpdeskEn.workFollowStatus_Id == 6551 ||
                _helpdeskEn.workFollowStatus_Id == 6552 ||
                _helpdeskEn.workFollowStatus_Id == 6550) //new
            {
                return "31";
            }
            else if (_helpdeskEn.workFollowStatus_Id == 6553) //new
            {
                if (_helpdeskEn.woCodes.Count() > 0)
                {
                    return "3";
                }
                else
                {
                    return "31";
                }
            }
            else
            {
                return "1";
            }
        }

        public string GetColorCodes(int userId, HelpDesks _helpdeskEn)
        {
            string _str = "";

            switch (_helpdeskEn.workFollowStatus_Id)
            {
                //drop
                case 6548:
                    _str += " <small style='font-size: 16px;'><span class='badge badge-dark'>[" + _helpdeskEn.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") + "] " + _helpdeskEn.workFollowStatusName + "</span></small>";
                    break;

                //in progress
                case 6551:
                    _str += " <small style='font-size: 16px;'><span class='label bg-color-magenta txt-color-black'>[" + _helpdeskEn.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") + "] " + _helpdeskEn.workFollowStatusName + "</span></small>";
                    break;

                //new
                case 6547:
                    _str += " <small style='font-size: 16px;'><span class='badge badge-warning'>[" + _helpdeskEn.createdDate.ToString("dd/MM/yyyy hh:mm ttt") + "] " + _helpdeskEn.workFollowStatusName + "</span></small>";
                    break;

                //received
                case 6550:
                    _str += " <small style='font-size: 16px;'><span class='badge badge-primary'>[" + _helpdeskEn.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") + "] " + _helpdeskEn.workFollowStatusName + "</span></small>";
                    break;

                //resolved
                case 6549:
                    _str += " <small style='font-size: 16px;'><span class='badge badge-success'>[" + _helpdeskEn.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") + "] " + _helpdeskEn.workFollowStatusName + "</span></small>";
                    break;

                //WAIT FOR RESOLUTION
                case 6553:
                    _str += " <small style='font-size: 16px;'><span class='badge badge-danger'>[" + _helpdeskEn.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") + "] " + _helpdeskEn.workFollowStatusName + "</span></small>";
                    break;

                default:
                    _str += " <small style='font-size: 16px;'><span class='badge badge-dark'>[" + _helpdeskEn.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm ttt") + "] " + _helpdeskEn.workFollowStatusName + "</span></small>";
                    break;
            }

            return _str;
        }

        public string GetFlag(int userId, HelpDesks _helpdeskEn)
        {
            string _str = "";

            if (_helpdeskEn.newFlag)
            {
                _str = "<small style='font-size: 16px;'><span class=saveChange' title='Receival Pending! ticket shall be received 30 minutes from created date' data-rel='tooltip' data-container='body'><i  class='fa fa-warning'  style='color: red'></i></small> ";
            }

            if (_helpdeskEn.receiveFlag)
            {
                _str = "<small style='font-size: 16px;'><span class=saveChange' title='Received, Action Pending! Action to be taken 30 minutes from received date' data-rel='tooltip' data-container='body'><i  class='fa fa-warning'  style='color: red'></i></small> ";
            }

            if (_helpdeskEn.actionFlag)
            {
                _str = "<small style='font-size: 16px;'><span class=saveChange' title='In Progress, Action Pending! Action to be taken 3 hours from in progress date' data-rel='tooltip' data-container='body'><i  class='fa fa-warning'  style='color: red'></i></small> ";
            }
            return _str;
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [AppAuthorize]
        public virtual ActionResult GetCustomSearchPanel()
        {
            var obj = new HelpDeskModel();
            int helpDeskStatus_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["HelpDeskStatus"]);
            int feedbackModule_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["HelpDeskFeedbackModule"]);

            var helpDeskStatus = svcCommon.GetListModuleItem(helpDeskStatus_Id);

            obj.lstStatus = helpDeskStatus.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.id.ToString()
            });

            var helpDeskFeedbackModule = svcCommon.GetListModuleItem(feedbackModule_Id);

            obj.lstModule = helpDeskFeedbackModule.Select(a => new SelectListItem
            {
                Text = a.name,
                Value = a.id.ToString()
            });

            return PartialView(MVC.HelpDesk.Views._SearchPanel, obj);
        }

        #endregion

        #region HelpDesk ADD,EDIT,DELETE
        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                HelpDeskModel _model = new HelpDeskModel();

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {

                        var workFlowStatus = svcCommon.GetListModuleItem(66);
                        _model.lsthelpdeskworkfollow = workFlowStatus.Select(a => new SelectListItem
                        {
                            Text = a.name,
                            Value = a.id.ToString(),
                            Selected = (a.id == 6547)
                        });


                        int sectionId = Convert.ToInt32(WebConfigurationManager.AppSettings["Section"]);
                        bool checkcreateoredit = true;
                        var source = svcUser.GetListUserByBusinessTypeId(sectionId, this.SubCostCenter, this.CurrentUserId, checkcreateoredit);
                        _model.lsthelpdeskuser = source.Select(a => new SelectListItem
                        {
                            Text = a.userName,
                            Value = a.id.ToString(),
                            Selected = (a.id == this.CurrentUserId)
                        });
                        _model.isEdit = false;
                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");

                        _model.code = "-";

                        var username = svcUser.GetUserById(this.CurrentUserId, this.CurrentUserId);
                        if (username != null)
                        {
                            _model.department = username.department;
                            _model.cellPhone = username.mobileNo;
                            _model.officePhone = username.mobileNo;
                            _model.email = username.email;
                            _model.name = username.firstName;
                            _model.designation = username.jobTitle;
                        }

                        var selectedFileNames = string.Empty;
                        _model.FilesToBeUploaded = selectedFileNames;
                        var dic = new Dictionary<string, string>();
                        _model.selectedFileNames = dic;
                        _model.isCreate = Common.Encrypt(this.CurrentUserId.ToString(), "true");

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.Create, _model)
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
        public virtual JsonResult SaveHelpDesk(HelpDeskModel model, List<HttpPostedFileBase> fileUpload)
        {
            //if (ModelState.IsValid)
            //{
            AccessRightsModel _accessRight = new AccessRightsModel();
            string direction = ConfigurationManager.AppSettings["HelpDeskFilePath"];
            List<string> newCollectionName = new List<string>();
            try
            {
                int currentId = this.CurrentUserId;
                _accessRight = Common.GetUserRights(currentId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(currentId.ToString(), model.id);
                    var isCreate = false;
                    if (!string.IsNullOrEmpty(model.isCreate))
                    {
                        isCreate = (Common.Decrypt(currentId.ToString(), model.isCreate) == "true");
                    }

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

                    var subCostCenter = this.SubCostCenter;
                    if (svc != null)
                    {
                        HelpDesks entity = new HelpDesks();
                        entity.organization_Id = subCostCenter;
                        entity.user_Id = model.user_Id;
                        entity.workFollowStatus_Id = model.workFollowStatus_Id;
                        //entity.workFollowStatus_Id = 96;
                        entity.code = model.code;
                        entity.feedback = model.feedback;
                        entity.name = model.name;
                        entity.department = model.department;
                        entity.designation = model.designation;
                        entity.officePhone = model.officePhone;
                        entity.cellPhone = model.cellPhone;
                        entity.email = model.email;
                        entity.createdBy = this.UserName;
                        entity.createdDate = DateTime.Now;
                        entity.modifiedBy = this.UserName;
                        entity.modifiedDate = DateTime.Now;
                        entity.modifiedBy = this.UserName;
                        entity.remarks = model.remarks ?? "";

                        if (newId > 0)
                        {
                            entity.id = newId;
                        }
                        else
                        {
                            entity.workFollowStatus_Id = 6547;
                            entity.createdBy = this.UserName;
                            entity.createdDate = DateTime.Now;

                            var menuType = new MenuTypes();
                            menuType.isHelpDesk = true;
                            menuType.appSettingList = Common.GetAppSettingItem(WebConfigurationManager.AppSettings["Auto_HDPrefix"]);
                            entity.prefix = WebConfigurationManager.AppSettings["Auto_HDPrefix"];

                        }

                        entity.isCreate = isCreate;


                        entity.fileNames = new List<string>();
                        bool hasFile = false;
                        List<string> collectionName = new List<string>();

                        if (!string.IsNullOrEmpty(model.FilesToBeUploaded))
                            collectionName = model.FilesToBeUploaded.Split(',').ToList();

                        if (fileUpload != null)
                        {
                            if (!System.IO.Directory.Exists(Path.Combine(@"" + direction)))
                            {
                                System.IO.Directory.CreateDirectory(Path.Combine(@"" + direction));
                            }

                            foreach (HttpPostedFileBase file in fileUpload)
                            {
                                if (file != null)
                                {
                                    string staticCode = currentId + "-" + subCostCenter + "-" + PublicFunc.GenerateRandonCode() + "_";
                                    var fileName = Path.GetFileName(file.FileName.Replace(",", ""));
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        collectionName.Remove(fileName);

                                        fileName = staticCode + fileName;
                                        newCollectionName.Add(fileName);
                                        file.SaveAs(Path.Combine(@"" + direction, fileName));
                                        entity.fileNames.Add(fileName);
                                        bool hasNewFile = true;
                                    }
                                }
                            }


                            foreach (var item in collectionName)
                            {
                                if (!string.IsNullOrEmpty(item) && !entity.fileNames.Any(p => p == item))
                                {
                                    entity.fileNames.Add(item);
                                }
                            }
                        }

                        var save = svc.SaveHelpDesk(this.CurrentUserId, entity, _accessRight.pageName);

                        if (save != null)
                        {
                            if (save.isPass)
                            {
                                if (save.removeFileName != null)
                                {
                                    foreach (var item in save.removeFileName)
                                    {
                                        if (item != null)
                                        {
                                            string filePath = Path.Combine(@"" + direction, item);
                                            if (System.IO.File.Exists(filePath))
                                            {
                                                System.IO.File.Delete(Path.Combine(@"" + direction, item));
                                            }
                                        }
                                    }
                                }

                            }

                            if (newId > 0)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_UPDATE,
                                    id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),
                                    code = save.code
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_SAVE,
                                    id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),
                                    code = save.code
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
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
                return Json(new
                {
                    status = Common.Status.Error.ToString(),
                    message = Resources.MSG_ERR_INVALIDMODEL
                });
            }
            catch (Exception err)
            {
                foreach (var item in newCollectionName)
                {
                    if (item != null)
                    {
                        string filePath = Path.Combine(@"" + direction, item);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(Path.Combine(@"" + direction, item));
                        }
                    }
                }
                throw new ErrorException(err.Message);
            }
            //}

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveApprovalHelpDesk(HelpDeskModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int newId = 0;
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
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


                        int submenu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["HelpDesk"]);


                        if (svc != null)
                        {
                            HelpDesks entity = new HelpDesks();
                            entity.id = newId;
                            entity.organization_Id = this.SubCostCenter;
                            entity.user_Id = model.user_Id;
                            entity.workFollowStatus_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["HeplDeskSubmittedWorkFlowStatus"]);
                            //entity.workFollowStatus_Id = 97;
                            entity.code = model.code;
                            entity.feedback = model.feedback;
                            entity.name = model.name;
                            entity.department = model.department;
                            entity.designation = model.designation;
                            entity.officePhone = model.officePhone;
                            entity.cellPhone = model.cellPhone;
                            entity.email = model.email;
                            entity.modifiedBy = model.modifiedBy;
                            entity.modifiedDate = model.modifiedDate;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;

                            //if (!isExists)
                            //{
                            var saveHelpDesk = svc.SaveHelpDesk(this.CurrentUserId, entity, _accessRight.pageName);

                            if (saveHelpDesk != null)
                            {
                                return Json(new
                                {
                                    viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.Index, _accessRight),
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_SAVE
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
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_INVALIDMODEL
            });
        }

        //[HttpPost]
        [Authorize]
        public virtual ActionResult Edit(string id, bool readOnly, bool hideBackButton = false)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            HelpDeskModel _model = new HelpDeskModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    _model.readOnly = readOnly;

                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var helpDesk = svc.GetHelpDeskById(newId);

                            if (helpDesk != null)
                            {
                                _model.isEdit = true;
                                _model.id = id;
                                _model.organization_Id = helpDesk.organization_Id;
                                _model.user_Id = helpDesk.user_Id;
                                _model.workFollowStatus_Id = helpDesk.workFollowStatus_Id;
                                _model.code = helpDesk.code;
                                _model.feedback = helpDesk.feedback;
                                _model.name = helpDesk.name;
                                _model.department = helpDesk.department;
                                _model.designation = helpDesk.designation;
                                _model.officePhone = helpDesk.officePhone;
                                _model.cellPhone = helpDesk.cellPhone;
                                _model.email = helpDesk.email;
                                _model.remarks = helpDesk.remarks;


                                int workFlowStatusId = Convert.ToInt32(WebConfigurationManager.AppSettings["HelpDeskStatus"]);

                                var workFlowStatus = svcCommon.GetListModuleItem(workFlowStatusId);
                                if (helpDesk.workFollowStatus_Id == 6550) //Received
                                {
                                    workFlowStatus = workFlowStatus.Where(a => a.id == 6550 || a.id == 6551 || a.id == 6553 || a.id == 6549 || a.id == 6548).ToList();
                                }
                                else if (helpDesk.workFollowStatus_Id == 6551) //In Progress
                                {
                                    workFlowStatus = workFlowStatus.Where(a => a.id == 6551 || a.id == 6553 || a.id == 6549 || a.id == 6548).ToList();
                                }
                                else if (helpDesk.workFollowStatus_Id == 6553) //WAIT FOR RESOLUTION
                                {
                                        workFlowStatus = workFlowStatus.Where(a => a.id == 6553 || a.id == 6549 || a.id == 6548).ToList();
                                }
                                else
                                {
                                    workFlowStatus = workFlowStatus.Where(a => a.id == helpDesk.workFollowStatus_Id).ToList();
                                }

                                _model.lsthelpdeskworkfollow = workFlowStatus.Select(a => new SelectListItem
                                {
                                    Text = a.name,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == helpDesk.workFollowStatus_Id)
                                });

                                int sectionId = Convert.ToInt32(WebConfigurationManager.AppSettings["Section"]);
                                bool checkcreateoredit = false;
                                var source = svcUser.GetListUserByBusinessTypeId(sectionId, this.SubCostCenter, this.CurrentUserId, checkcreateoredit);
                                _model.lsthelpdeskuser = source.Select(a => new SelectListItem
                                {
                                    Text = a.userName,
                                    Value = a.id.ToString(),
                                    Selected = (a.id == helpDesk.user_Id)
                                });

                                _model.hideBackButton = hideBackButton;
                                if (helpDesk.fileNames != null)
                                {
                                    _model.selectedFileNames = helpDesk.fileNames.Where(p => !string.IsNullOrEmpty(p)).ToDictionary(p => Common.Encrypt(this.CurrentUserId.ToString(), p.ToString()), p => p);
                                    _model.FilesToBeUploaded = string.Join(",", helpDesk.fileNames.Where(p => !string.IsNullOrEmpty(p)).Select(p => p));
                                }

                                _model.isCreate = Common.Encrypt(this.CurrentUserId.ToString(), "false");
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

            if (_model.readOnly)
            {
                if (!Request.IsAjaxRequest())
                {
                    return View(MVC.HelpDesk.Views.Create, _model);
                }
                else
                {
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.Create, _model)
                    });
                }
            }
            else
            {
                if (!Request.IsAjaxRequest())
                {
                    return View(MVC.HelpDesk.Views.UpdateStatus, _model);
                }
                else
                {
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.UpdateStatus, _model)
                    });
                }
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Check(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

            if (_accessRight != null)
            {
                if (_accessRight.canEdit)
                {
                    HelpDesks entity = new HelpDesks();
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    entity.id = newId;
                    entity.workFollowStatus_Id = 6550;
                    entity.modifiedBy = this.UserName;
                    entity.modifiedDate = DateTime.Now;
                    entity.isReceived = true;

                    var save = svc.SaveHelpDesk(this.CurrentUserId, entity, _accessRight.pageName);
                    if (save != null)
                    {
                        return Json(new
                          {
                              status = Common.Status.Success.ToString(),
                              message = "Test",
                              viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.Index, _accessRight)
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
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        int newId = 0;
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteHelpDesk(this.CurrentUserId, newId, _accessRight.pageName);
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


        #region CallBack

        [HttpPost]
        [Authorize]
        public virtual JsonResult CallBack(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            HelpDeskModel _model = new HelpDeskModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    _model.readOnly = readOnly;
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            _model.id = id;
                            _model.remarks = "";
                            //_model.subMenu_Id = 41;

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
                viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.CallBack, _model)
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveCallBackHelpDesk(HelpDeskModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int newId = 0;
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
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
                            HelpDesks entity = new HelpDesks();

                            var helpDesk = svc.GetHelpDeskById(newId);
                            entity.organization_Id = this.SubCostCenter;
                            entity.user_Id = helpDesk.user_Id;
                            entity.asset_Id = helpDesk.asset_Id;
                            entity.workFollowStatus_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["HeplDeskAssignedWorkFlowStatus"]);
                            entity.code = helpDesk.code;
                            entity.feedback = helpDesk.feedback;
                            entity.name = helpDesk.name;
                            entity.department = helpDesk.department;
                            entity.designation = helpDesk.designation;
                            entity.officePhone = helpDesk.officePhone;
                            entity.cellPhone = helpDesk.cellPhone;
                            entity.email = helpDesk.email;

                            if (newId > 0)
                            {
                                entity.id = newId;
                                entity.modifiedBy = this.UserName;
                                entity.modifiedDate = DateTime.Now;
                            }
                            else
                            {
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;

                            }

                            var saveHelpDesk = svc.SaveHelpDesk(this.CurrentUserId, entity, _accessRight.pageName);

                            if (saveHelpDesk != null)
                            {
                                return Json(new
                                {
                                    viewMarkup = Common.RenderPartialViewToString(this, MVC.HelpDesk.Views.Index, _accessRight),
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_CALLBACK
                                });

                            }
                            else
                            {
                                if (newId > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE
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
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_INVALIDMODEL
            });
        }


        #endregion

        public virtual ActionResult GetFile(string id)
        {
            var fileName = Common.Decrypt(this.CurrentUserId.ToString(), id);
            string uploadDirectory = WebConfigurationManager.AppSettings["HelpDeskFilePath"];
            var filepath = @"" + uploadDirectory + fileName;// System.IO.Path.Combine(@"" + direction, url);

            if (!System.IO.File.Exists(filepath))
                return HttpNotFound();

            string fileExtension = Path.GetExtension(fileName);

            string mimeType = "application/unknown";
            switch (fileExtension)
            {
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".tif":
                    mimeType = "image/tiff";
                    break;
                case (".xls"):
                case (".xlsx"):
                    mimeType = "application/vnd.ms-excel";
                    break;
                case (".doc"):
                case (".docx"):
                    mimeType = "application/msword";
                    break;
            }


            byte[] FileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(FileBytes, mimeType);
        }
    }
}
