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
    public partial class BusinessAddressController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "Business/Index";
        #endregion

        #region Constructor

        private readonly IBusinessAddressService svc; private readonly ICommonService svcCommon;

        public BusinessAddressController(IBusinessAddressService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region BusinessAddress Details

        [AppAuthorize]
        public virtual ActionResult Index(string id, bool readOnly)
        {
            BusinessAddressModel _model = new BusinessAddressModel();
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                int businessId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                // Get Business Id
                _model.isEdit = false;
                _model.business_Id = id;
                _model.readOnly = readOnly;

                var businessAddress = svc.GetBusinessAddressById(businessId);
                if (businessAddress.id > 0)
                {
                    _model.businessaddressID = businessAddress.id;
                    _model.business_Id = id;
                    _model.isEdit = true;
                    _model.address = businessAddress.address;
                    _model.city = businessAddress.city;
                    _model.province = businessAddress.province;
                    _model.postalCode = businessAddress.postalCode;
                    if (businessAddress.country == null)
                    {
                        _model.country = "Malaysia";
                    }
                    else
                    {
                        _model.country = businessAddress.country;
                    }
                }

                //int bussClassification_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["Classification"]);

                //var classfication = svcCommon.GetListModuleItem(bussClassification_Id);
                //var lst = svc.GetListBusinessClassificationByBusinessId(id);
                //_model.lstClassificationItem = classfication.Select(a => new SelectListItem
                //{
                //    Text = a.name,
                //    Value = a.id.ToString(),
                //    Selected = lst.Any(p=>p.classification_Id==a.id)
                //});


                //foreach (var item in classfication)
                //{
                //    BusinessAddressViewModel obj = new BusinessAddressViewModel();
                //    obj.moduleItem_Id = item.id;
                //    obj.moduleItemName = item.name;
                //    obj.isCheck = lst.Any(p => p.classification_Id == item.id);
                //    _model.lstClassificationItem.Add(obj);
                //}

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessAddress.Views._Index, _model)
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

        //[HttpPost]
        //[AppAuthorize]
        //public virtual ActionResult BusinessAddress(int id)
        //{
        //    BusinessAddressModel _model = new BusinessAddressModel();
        //    try
        //    {
        //        AccessRightsModel _accessRight = new AccessRightsModel();
        //        _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
        //        // Get Business Id
        //        _model.business_Id = id;
        //        //  TempData["business_Id"] = id;
        //        var businessAddress = svc.GetBusinessAddressById(id);
        //        if (businessAddress.id > 0)
        //        {
        //            _model.businessaddressID = businessAddress.id;
        //            _model.business_Id = businessAddress.business_Id;
        //            _model.address = businessAddress.address;
        //            _model.city = businessAddress.city;
        //            _model.province = businessAddress.province;
        //            _model.postalCode = businessAddress.postalCode;
        //            _model.country = businessAddress.country;
        //        }

        //        if (_accessRight != null)
        //        {
        //            if (_accessRight.canView)
        //            {
        //                //return Json(new
        //                //{
        //                //    viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessAddress.Views._Index, _model)
        //                //});
        //                return PartialView(MVC.BusinessAddress.Views._Index, _model);

        //            }
        //            else
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Denied.ToString(),
        //                    message = Resources.NO_ACCESS_RIGHTS_VIEW
        //                });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                status = Common.Status.Denied.ToString(),
        //                message = Resources.NO_ACCESS_RIGHTS
        //            });
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        throw new ErrorException(err.Message);
        //    }
        //}

        [HttpPost]
        [Authorize]
        public virtual JsonResult SaveBusinessAddress(BusinessAddressModel model)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {

                    if (model.businessaddressID > 0)
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
                        BusinessAddresses entity = new BusinessAddresses();
                        entity.business_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.business_Id);
                        entity.address = model.address;
                        entity.city = model.city;
                        entity.province = model.province;
                        entity.postalCode = model.postalCode;
                        entity.country = model.country;
                        // int[] _classification_Ids = model.classification_Ids;
                        entity.modifiedBy = this.UserName;
                        entity.modifiedDate = DateTime.Now;
                        if (model.businessaddressID > 0)
                        {
                            entity.id = model.businessaddressID;
                        }
                        else
                        {
                            entity.createdBy = this.UserName;
                            entity.createdDate = DateTime.Now;
                        }
                        //bool isExists = svc.IsBusinessAddressExists(entity);
                        //if (!isExists)
                        //{
                        // entity.classification_Ids = _classification_Ids;
                        var save = svc.SaveBusinessAddress(this.CurrentUserId, entity, _accessRight.pageName);

                        if (save != null)
                        {
                            if (model.businessaddressID > 0)
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
                            if (model.businessaddressID > 0)
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
                        //        message = Resources.BUSINESSADDRESS_RECORD_EXISTS.Replace("$BUSINESS_ADDRESS$", entity.address)
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

        #endregion

    }
}
