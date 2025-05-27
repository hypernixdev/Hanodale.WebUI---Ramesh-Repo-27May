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
    public partial class BusinessOthersController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "Business/Index";
        #endregion

        #region Constructor

        private readonly IBusinessOthersService svc; private readonly ICommonService svcCommon;

        public BusinessOthersController(IBusinessOthersService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region BusinessOthers Details

        [AppAuthorize]
        public virtual ActionResult Index(string id, bool readOnly)
        {
            BusinessOthersModel _model = new BusinessOthersModel();
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);
                _model.isEdit = false;
                _model.business_Id = id;
                _model.readOnly = readOnly;
                // Get Stock Id
                int newId = 0;
                newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var businessother = svc.GetBusinessOthersById(newId);
                if (businessother.id > 0)
                {
                    _model.businessOtherID = Common.Encrypt(this.CurrentUserId.ToString(), businessother.id.ToString());
                    _model.business_Id = id;
                    _model.isEdit = true;
                    _model.readOnly = readOnly;
                    _model.bumiShare = businessother.bumiShare;
                    _model.nonBumiShare = businessother.nonBumiShare;
                    _model.foreignShare = businessother.foreignShare;
                    _model.bumiCapital = businessother.bumiCapital;
                    _model.classA = businessother.classA;
                    _model.classB = businessother.classB;
                    _model.classBX = businessother.classBX;
                    _model.classC = businessother.classC;
                    _model.classD = businessother.classD;
                    _model.classE = businessother.classE;
                    _model.classEX = businessother.classEX;
                    _model.classF = businessother.classF;
                    _model.pkk = businessother.pkk;
                    _model.tnb = businessother.tnb;
                    _model.jba = businessother.jba;
                    _model.maras = businessother.mara;
                    _model.dbkl = businessother.dbkl;
                    _model.financeMinistry = businessother.financeMinistry;
                    _model.jkh = businessother.jkh;
                    _model.jkrs = businessother.jkr;
                    _model.paidUpCapital = businessother.paidUpCapital;
                    _model.businessCategory = businessother.businessCategory;
                }
                else
                {
                    _model.classA = false;
                    _model.classB = false;
                    _model.classBX = false;
                    _model.classC = false;
                    _model.classD = false;
                    _model.classE = false;
                    _model.classEX = false;
                    _model.classF = false;
                    _model.pkk = false;
                    _model.tnb = false;
                    _model.jba = false;
                    _model.maras = false;
                    _model.dbkl = false;
                    _model.financeMinistry = false;
                    _model.jkh = false;
                    _model.jkrs = false;

                }


                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canAdd)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessOthers.Views._Create, _model)
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


        //[Authorize]
        //public virtual ActionResult BindStockSpecification(DataTableModel param)
        //{
        //    int totalRecordCount = 0;
        //    IEnumerable<StockSpecifications> filteredStockSpecification = null;
        //    try
        //    {
        //        AccessRightsModel _accessRight = new AccessRightsModel();
        //        _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

        //        if (_accessRight != null)
        //        {
        //            if (_accessRight.canView || _accessRight.canEdit)
        //            {
        //                // Get login user Id
        //                int userId = this.CurrentUserId;
        //                // Get stock Id
        //                var stockId = Convert.ToInt32(TempData["stock_Id"]);
        //                TempData["stock_Id"] = stockId;
        //                var stockSpecificationModel = this.svc.GetStockSpecification(this.CurrentUserId, this.CurrentUserId, stockId, param.iDisplayStart, param.iDisplayLength, param.sSearch);

        //                if (svc != null)
        //                {
        //                    StockSpecificationViewModel _stockSpecificationViewModel = new StockSpecificationViewModel();

        //                    //Sorting
        //                    var sortColumnIndex = param.iSortCol_0;
        //                    Func<StockSpecifications, string> orderingFunction = (c => sortColumnIndex == 0 ? c.width :
        //                                                    sortColumnIndex == 1 ? c.length :
        //                                                    sortColumnIndex == 2 ? c.height :
        //                                                    sortColumnIndex == 3 ? c.diameter :
        //                                                    sortColumnIndex == 3 ? c.other : c.modifiedDate.ToString()
        //                                                    );

        //                    filteredStockSpecification = stockSpecificationModel.lstStockSpecification;
        //                    if (param.sSortDir_0 == "asc")
        //                        filteredStockSpecification = filteredStockSpecification.OrderBy(orderingFunction);
        //                    else
        //                        filteredStockSpecification = filteredStockSpecification.OrderByDescending(orderingFunction);


        //                    var result = StockSpecificationData(filteredStockSpecification);
        //                    return Json(new
        //                    {
        //                        sEcho = param.sEcho,
        //                        iTotalRecords = stockSpecificationModel.recordDetails.totalRecords,
        //                        iTotalDisplayRecords = stockSpecificationModel.recordDetails.totalDisplayRecords,
        //                        aaData = result
        //                    }, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    return Json(new
        //                    {
        //                        status = Common.Status.Error.ToString(),
        //                        message = Resources.MSG_ERR_SERVICE
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Denied.ToString(),
        //                    message = Resources.NO_ACCESS_RIGHTS_EDIT
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

        ///// <summary>
        ///// This method is to get the Stock Specification data as string array to bind into datatbale
        ///// </summary>
        ///// <param name="StockSpecification Entry">Stock Specification list</param>
        ///// <returns></returns>
        //public static List<string[]> StockSpecificationData(IEnumerable<StockSpecifications> userEntry)
        //{
        //    return userEntry.Select(entry => new string[]
        //    {  
        //        entry.width,
        //        entry.length, 
        //        entry.height,
        //        entry.diameter,
        //        entry.other,
        //        entry.modifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
        //        entry.id.ToString()
        //    }).ToList();
        //}

        //[Authorize]
        //[HttpPost] public virtual JsonResult RenderAction()
        //{

        //    AccessRightsModel _accessRight = new AccessRightsModel();
        //    try
        //    {
        //        _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

        //        return Json(new
        //        {
        //            viewMarkup = Common.RenderPartialViewToString(this, MVC.StockSpecification.Views._RenderAction, _accessRight)
        //        });
        //    }
        //    catch (Exception err)
        //    {
        //        throw new ErrorException(err.Message);
        //    }
        //}

        #endregion

        #region ADD,EDIT,DELETE

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveBusinessOthers(BusinessOthersModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);


                    if (_accessRight != null)
                    {

                        if (model.businessOtherID != null)
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
                            BusinessOtherss entity = new BusinessOtherss();
                            entity.business_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.business_Id);
                            entity.bumiShare = model.bumiShare;
                            entity.nonBumiShare = model.nonBumiShare;
                            entity.foreignShare = model.foreignShare;
                            entity.bumiCapital = model.bumiCapital;
                            entity.classA = model.classA;
                            entity.classB = model.classB;
                            entity.classBX = model.classBX;
                            entity.classC = model.classC;
                            entity.classD = model.classD;
                            entity.classE = model.classE;
                            entity.classEX = model.classEX;
                            entity.classF = model.classF;
                            entity.pkk = model.pkk;
                            entity.tnb = model.tnb;
                            entity.jba = model.jba;
                            entity.mara = model.maras;
                            entity.dbkl = model.dbkl;
                            entity.financeMinistry = model.financeMinistry;
                            entity.jkh = model.jkh;
                            entity.jkr = model.jkrs;
                            entity.businessCategory = model.businessCategory;
                            entity.paidUpCapital = model.paidUpCapital;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;

                            if (model.businessOtherID != null)
                            {

                                entity.id = Common.DecryptToID(this.CurrentUserId.ToString(), model.businessOtherID);
                            }
                            else
                            {
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            //bool isExists = svc.IsStockSpecificationExists(entity);
                            //if (!isExists)
                            //{
                            var save = svc.SaveBusinessOthers(this.CurrentUserId, entity, _accessRight.pageName);

                            if (save != null)
                            {
                                if (model.businessOtherID != null)
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
                                if (model.businessOtherID != null)
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
                            // }
                            //else
                            //{
                            //    return Json(new
                            //    {

                            //        status = Common.Status.Warning.ToString(),
                            //        message = Resources.STOCKSPECIFICATION_WIDTH_EXITS
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

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            BusinessOthersModel _model = new BusinessOthersModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var businessother = svc.GetBusinessOthersById(newId);
                            if (businessother != null)
                            {
                                _model.business_Id = id;
                                _model.bumiShare = businessother.bumiShare;
                                _model.nonBumiShare = businessother.nonBumiShare;
                                _model.foreignShare = businessother.foreignShare;
                                _model.bumiCapital = businessother.bumiCapital;
                                _model.classA = businessother.classA;
                                _model.classB = businessother.classB;
                                _model.classBX = businessother.classBX;
                                _model.classC = businessother.classC;
                                _model.classD = businessother.classD;
                                _model.classE = businessother.classE;
                                _model.classEX = businessother.classEX;
                                _model.classF = businessother.classF;
                                _model.pkk = businessother.pkk;
                                _model.tnb = businessother.tnb;
                                _model.jba = businessother.jba;
                                _model.maras = businessother.mara;
                                _model.dbkl = businessother.dbkl;
                                _model.financeMinistry = businessother.financeMinistry;
                                _model.jkh = businessother.jkh;
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
                viewMarkup = Common.RenderPartialViewToString(this, MVC.BusinessOthers.Views._Create, _model)
            });
        }

        //[HttpPost]
        //[Authorize]
        //public virtual ActionResult Delete(string id)
        //{
        //    AccessRightsModel _accessRight = new AccessRightsModel();
        //    try
        //    {
        //        _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

        //        if (_accessRight != null)
        //        {
        //            int newId = 0;
        //            newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);

        //            if (_accessRight.canView && _accessRight.canDelete)
        //            {
        //                if (svc != null)
        //                {
        //                    bool isSuccess = svc.DeleteStockSpecification(this.CurrentUserId, newId, _accessRight.pageName);
        //                    if (isSuccess)
        //                    {
        //                        return Json(new
        //                        {
        //                            status = Common.Status.Success.ToString(),
        //                            message = Resources.MSG_DELETE
        //                        });
        //                    }
        //                    else
        //                    {
        //                        return Json(new
        //                        {
        //                            status = Common.Status.Error.ToString(),
        //                            message = Resources.MSG_ERR_DELETE
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    return Json(new
        //                    {
        //                        status = Common.Status.Error.ToString(),
        //                        message = Resources.MSG_ERR_SERVICE
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                return Json(new
        //                {
        //                    status = Common.Status.Denied.ToString(),
        //                    message = Resources.NO_ACCESS_RIGHTS_DELETE
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("REFERENCE"))
        //        {
        //            return Json(new
        //            {
        //                status = Common.Status.Warning.ToString(),
        //                message = Resources.MSG_RECORD_IN_USE
        //            });
        //        }
        //        else
        //        {
        //            throw new ErrorException(ex.Message);
        //        }
        //    }
        //    return View();
        //}

        #endregion

    }
}
