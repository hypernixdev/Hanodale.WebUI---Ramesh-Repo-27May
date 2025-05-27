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
    public partial class BusinessClassificationController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "Business/Index";
        #endregion

        #region Constructor

        private readonly IBusinessClassificationService svc; private readonly ICommonService svcCommon;

        public BusinessClassificationController(IBusinessClassificationService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region BusinessClassification Details

        [AppAuthorize]
        public virtual ActionResult Index(int id)
        {
            BusinessClassificationModel _model = new BusinessClassificationModel();
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                // Get Business Id
                _model.business_Id = id;
                // TempData["business_Id"] = id;

                int bussClassification_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Classification"]);

                var classfication = svcCommon.GetListModuleItem(bussClassification_Id);
                var lst = svc.GetListBusinessClassificationByBusinessId(id);
                foreach (var item in classfication)
                {
                    BusinessClassificationItemViewModel obj = new BusinessClassificationItemViewModel();
                    obj.moduleItem_Id = item.id;
                    obj.moduleItemName = item.name;
                    obj.isCheck = lst.Any(p => p.classification_Id == item.id);
                    _model.lstClassificationItem.Add(obj);
                }

                //_model.lstClassification = classfication.Select(a => new SelectListItem
                //{
                //    Text = a.name,
                //    Value = a.id.ToString()
                //});

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessClassification.Views._Index, _model)
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
        public virtual ActionResult BusinessClassification()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {

                        return PartialView(MVC.BusinessClassification.Views._Index);

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
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveBusinessClassification(BusinessClassificationModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {

                        if (model.businessclassificationID > 0)
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
                            BusinessClassifications entity = new BusinessClassifications();
                            // entity.business_Id = 1;
                            entity.business_Id = model.business_Id;
                            int[] _classification_Ids = model.classification_Ids;

                            entity.id = model.businessclassificationID;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;

                            entity.createdBy = this.UserName;
                            entity.createdDate = DateTime.Now;

                            //bool isExists = svc.IsBusinessClassificationExists(entity);
                            //if (!isExists)
                            //{
                            entity.classification_Ids = _classification_Ids;
                            var save = svc.SaveBusinessClassification(this.CurrentUserId, entity, _accessRight.pageName);

                            if (save != null)
                            {
                                if (model.businessclassificationID > 0)
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_SAVE
                                    });
                                }
                            }
                            else
                            {
                                if (model.businessclassificationID > 0)
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
                            //}
                            //else
                            //{
                            //    return Json(new
                            //    {

                            //        status = Common.Status.Warning.ToString(),
                            //        message = Resources.ROLE_RECORD_EXISTS
                            //    });
                            //}
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

    }
}
