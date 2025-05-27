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
using System.Web;
using System.IO;
using Hanodale.Entity.Core;
using System.Data;
using System.Globalization;
using Hanodale.Domain.Models;
using System.Data.Objects;


namespace Hanodale.WebUI.Controllers
{
    public partial class OrderApprovalController : BaseController
    {
        #region Declaration
        readonly string PAGE_URL = string.Empty;

        #endregion

        #region Constructor

        private readonly IOrderApprovalService svc;
        private readonly IProductService psvc;
        private readonly IProductService svcProduct;
        private readonly ISyncManager syncManager;

        public OrderApprovalController(IOrderApprovalService _bLService, IProductService _svcProduct, ICommonService _svcCommon, ISyncManager syncManager)
        {
            this.svcCommon = _svcCommon;
            this.svcProduct = _svcProduct;
            this.sectionName = "OrderApproval";
            this.svc = _bLService;
            this.menu_Id = Convert.ToInt32(WebConfigurationManager.AppSettings["OrderApproval_Menu_Id"]);
            PAGE_URL = this.sectionName + "/Index";
            this.syncManager = syncManager;
        }
        #endregion

        #region OrderApproval Profile Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("This is called");
            try
            {
                var _model = this.GetVisibleColumnForGridView(new OrderApprovalModel());

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {

                        using (HanodaleEntities model = new HanodaleEntities())
                        {

                            ViewBag.AwaitingApproval = model.OrderApproval.Count(o => o.approvalStatus == "Awaiting Approval");
                            ViewBag.Approved = model.OrderApproval.Count(o => o.approvalStatus == "Approved");
                            ViewBag.Rejected = model.OrderApproval.Count(o => o.approvalStatus == "Rejected");

                        }
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.OrderApproval.Views.Index, _model)
                        });
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

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult OrderApproval()
        {
            try
            {
                var _model = this.GetVisibleColumnForGridView(new OrderApprovalModel(), 3);

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                _model.accessRight = _accessRight;

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.OrderApproval.Views.Index, _model)
                        });
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

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult Approval()
        {
            try
            {
                var id = Session["Id"] as string;
                var orderid = Session["OrderId"] as string;

                OrderApprovalMaintenanceModel _model = new OrderApprovalMaintenanceModel();
                _model.orderApproval = GetOrderApprovalModel(id.ToString(), false);
                _model.orderApproval.order_Id = orderid;

                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.OrderApproval.Views.Approval, _model)
                });
            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [Authorize]
        public virtual ActionResult BindOrderApproval(DataTableModel param)
        {
            int totalRecordCount = 0;
            List<OrderApprovals> filteredOrderApprovals = null;
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        int userId = this.CurrentUserId;

                        var filter = new DatatableFilters { currentUserId = this.CurrentUserId, all = true, startIndex = param.iDisplayStart, pageSize = param.iDisplayLength, search = param.sSearch };
                        var OrderApprovalModel = this.svc.GetOrderApproval(filter);

                        if (svc != null)
                        {
                            var lstFieldMetadata = this.GetVisibleIndexFieldMetadata();

                            filteredOrderApprovals = OrderApprovalModel.lstOrderApproval;
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

                                filteredOrderApprovals = filteredOrderApprovals.OrderByDynamic(sortField, (param.sSortDir_0 == "asc" ? false : true)).ToList();
                            }

                            var result = OrderApprovalData(filteredOrderApprovals, this.CurrentUserId);

                            var sEcho = param.sEcho;
                            var iTotalRecords = OrderApprovalModel.recordDetails.totalRecords;
                            var iTotalDisplayRecords = OrderApprovalModel.recordDetails.totalDisplayRecords;

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
                return Msg_ErrorInRetriveData(ex);
            }
        }

        public List<string[]> OrderApprovalData(List<OrderApprovals> OrderApprovalEntry, int currentUserId)
        {
            var result = this.GetDatatableData<OrderApprovals>(OrderApprovalEntry, currentUserId);
            return result;
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
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.OrderApproval.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        #endregion

        #region Add, Edit, and Delete

        [HttpPost]
        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (!_accessRight.canAdd)
                {
                    return this.Msg_AccessDeniedInAdd();
                }

                if (svc != null)
                {
                    var _model = new OrderApprovalModel();

                    this.FillupFieldMetadata(_model, false);

                    _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");


                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.OrderApproval.Views.Create, _model)
                    });
                }
                else
                {
                    return this.Msg_ErrorInService();
                }

            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [HttpPost]
        [Authorize]

        public virtual JsonResult SaveApproval(string id, string order_id, string approvalstatus, string remarks,string orderTotal)
        {
            try
            {
                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                OrderApprovals entity = new OrderApprovals();
                entity.id = decrypted_Id;
                entity.approvalStatus = approvalstatus;
                entity.remarks = remarks;
                entity.order_Id = Convert.ToInt32(order_id);
                entity.approvedUser_Id = this.CurrentUserId;
                entity.submittedUser_Id = this.CurrentUserId;
                entity.OrderTotal=Convert.ToDecimal(orderTotal);


                bool isExists = svc.IsOrderApprovalExists(entity);
                if (!isExists)
                {
                    var save = svc.SaveOrderApproval(entity);

                    if (save != null && save.isSuccess)
                    {
                        var new_Id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString());
                        return Json(new
                        {
                            success = true,
                          
                            newId = new_Id
                        }, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        return this.Msg_ErrorInSave(false);
                    }
                }
                else
                {
                    return this.Msg_ErrorInSave(false);
                }

            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveOrderApproval(OrderApprovalModel model)
        {
            try
            {
                CheckModelState(ModelState, model.isEdit);
                if (ModelState.IsValid)
                {
                    AccessRightsModel _accessRight = new AccessRightsModel();

                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int decrypted_Id = 0;
                        if(model.id==null)
                        {
                            model.id= Session["Id"] as string;
                        }
                        decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
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
                                return this.Msg_AccessDeniedInAdd();
                            }
                        }

                        if (svc != null)
                        {
                            OrderApprovals entity = new OrderApprovals();
                            entity.id = decrypted_Id;
                            entity.approvalStatus = model.ApprovalStatus;
                            entity.remarks = model.Remarks;
                            entity.approvedUser_Id = this.CurrentUserId;
                            entity.OrderStatus = model.OrderStatus;
                            entity.OrderDate = model.OrderDate;
                            entity.OrderTotal = model.OrderTotal;
                            entity.order_Id = Convert.ToInt32(model.order_Id);
                            entity.CreatedBy = entity.CreatedBy;
                            entity.approvedDate = model.ApprovalDate;
                            entity.submittedUser_Id = this.CurrentUserId;

                            bool isExists = svc.IsOrderApprovalExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveOrderApproval(entity);

                                if (save != null && save.isSuccess)
                                {

                                    return Json(new
                                    {
                                        status = Common.Status.Success.ToString(),
                                        message = Resources.MSG_UPDATE,
                                        id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),
                                    });

                                }
                                else
                                {
                                    return this.Msg_ErrorInSave(isUpdate);
                                }
                            }
                            else
                            {
                             
                                return Json(new
                                {
                                    success = true,
                                    message="Order Approval already exists!"
                                }, JsonRequestBehavior.AllowGet);
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
                return Msg_ErrorInRetriveData(ex);
            }
        }

        private OrderApprovalModel GetOrderApprovalModel(string id, bool readOnly)
        {
            try
            {
                OrderApprovalModel _model = new OrderApprovalModel();
                this.FillupFieldMetadata(_model, true);
                _model.readOnly = readOnly;

                var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                var productCarton = svc.GetOrderApprovalById(decrypted_Id);

                if (productCarton != null)
                {
                    _model.id = id;
                    _model.isEdit = true;
                    _model.orderNum = productCarton.orderNum;
                    _model.CustomerName = productCarton.CustomerName;
                    _model.OrderDate = productCarton.OrderDate;
                    _model.OrderStatus = productCarton.OrderStatus;
                    _model.OrderTotal = productCarton.OrderTotal;
                    _model.CreatedBy = productCarton.CreatedBy;
                    _model.ApprovalBy = productCarton.ApprovalBy;
                    _model.ApprovalStatus = productCarton.approvalStatus;
                    _model.CustomerId = productCarton.customerid;
                    _model.order_Id = productCarton.order_Id.ToString();

                    _model.ApprovalDate = productCarton.approvedDate;


                }


                return _model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            try
            {
                var _model = new OrderApprovalMaintenanceModel();
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (id == "")
                {
                    id = Session["Id"] as string;
                    _model.tableProfile = this.GetTableProfileWithTab();
                    _model.orderApproval = GetOrderApprovalModel(id, readOnly);

                    if (_model == null)
                    {
                        return this.Msg_ErrorInRetriveData();
                    }
                    _model.orderApproval.lstApprovalStatus = new List<string>
                            {
                                "Awaiting Approval",
                                "Approved",
                                "Rejected"
                            };
                    return Json(new
                    {
                        viewMarkup = Common.RenderPartialViewToString(this, MVC.OrderApproval.Views.Create, _model.orderApproval)
                    });
                }

                Session["Id"] = null;
                Session["OrderId"] = null;
                Session["CustomerId"] = null;



                if (_accessRight != null)
                {
                    int decrypted_Id = 0;
                    if ((_accessRight.canView && readOnly) || _accessRight.canEdit)
                    {
                        _model.tableProfile = this.GetTableProfileWithTab();
                        _model.orderApproval = GetOrderApprovalModel(id, readOnly);
                        ViewBag.IsReadOnly = readOnly;
                        if (_model == null)
                        {
                            return this.Msg_ErrorInRetriveData();
                        }

                        Session["Id"] = _model.orderApproval.id;
                        Session["OrderId"] = _model.orderApproval.order_Id;
                        Session["CustomerId"] = _model.orderApproval.CustomerId;
                        _model.readOnly = readOnly;
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.OrderApproval.Views.Maintenance, _model)
                        });
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

        [HttpPost]
        [Authorize]
        public virtual JsonResult Delete(string id)
        {
            try
            {
                var _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (!_accessRight.canDelete)
                    {
                        return this.Msg_AccessDeniedInDelete();
                    }

                    var decrypted_Id = Common.DecryptToID(this.CurrentUserId.ToString(), id);

                    if (svc != null)
                    {
                        var result = svc.DeleteOrderApproval(decrypted_Id);

                        if (result)
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
                    return this.Msg_AccessDenied();
                }
            }
            catch (Exception ex)
            {
                return Msg_ErrorInRetriveData(ex);
            }
        }




        #endregion




    }
}