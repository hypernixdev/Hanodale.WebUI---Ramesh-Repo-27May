using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;

namespace Hanodale.DataAccessLayer.Services
{
    public class OrderApprovalService : BaseService, IOrderApprovalService
    {
        public OrderApprovalDetails GetOrderApprovalBySearch(DatatableFilters entityFilter)
        {
            OrderApprovalDetails _result = new OrderApprovalDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    var query = from oa in model.OrderApproval
                                join o in model.Order
                                    on oa.order_Id equals o.id
                                select new
                                {
                                    oa.id,
                                    oa.order_Id,
                                    o.orderNum,
                                    CustomerName = o.Customer.name,
                                    o.orderDate,
                                    OrderStatusFromOrder = o.orderStatus,
                                    oa.approvalStatus,
                                    oa.submittedUser_Id,
                                    oa.approvedUser_Id,
                                    Createdby = oa.User.firstName,
                                    ApprovalBy = oa.User.firstName,
                                    o.docOrderAmt

                                };

                    //var totalOrderAmount = Math.Round(
                    //   (double)model.OrderItems.Sum(i => (i.unitPrice * i.orderQty) + i.operationCost),
                    //   2, MidpointRounding.AwayFromZero);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.CustomerName.Contains(entityFilter.search)
                                || p.OrderStatusFromOrder.Contains(entityFilter.search)
                                || p.orderNum.Contains(entityFilter.search)
                            ));
                    }


                    var result = query
                                .OrderBy(p => p.order_Id)

                                .Select(p => new OrderApprovals
                                {
                                    id = p.id,
                                    orderNum = p.orderNum,
                                    CustomerName = p.CustomerName,
                                    OrderDate = p.orderDate,
                                    OrderStatus = p.approvalStatus,
                                    CreatedBy = p.Createdby,
                                    ApprovalBy = p.ApprovalBy,
                                    OrderTotal = (decimal)p.docOrderAmt,
                                    order_Id = p.order_Id

                                });

                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstOrderApproval = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public OrderApprovals CreateOrderApproval(OrderApprovals entityEn)
        {
            var _OrderApprovalEn = new Entity.Core.OrderApproval();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //_OrderApprovalEn.company = entityEn.company;
                    //_OrderApprovalEn.partNum = entityEn.partNum;
                    //_OrderApprovalEn.warehouseCode = entityEn.warehouseCode;                 
                    //_OrderApprovalEn.uom = entityEn.uom;
                    //_OrderApprovalEn.onHandQty = entityEn.onHandQty;

                    model.OrderApproval.Add(_OrderApprovalEn);
                    model.SaveChanges();

                    entityEn.id = _OrderApprovalEn.id;
                    //entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public OrderApprovals UpdateOrderApproval(OrderApprovals entityEn)
        {
           
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var OrderApprovalentity = model.OrderApproval.SingleOrDefault(p => p.order_Id == entityEn.order_Id);
                    var _OrderEn = model.Order.SingleOrDefault(p => p.id == entityEn.order_Id);
                    if (entityEn.remarks != "" && entityEn.remarks != null)
                    {
                        var orderUpdate = new OrderUpdate
                        {
                            order_Id = entityEn.order_Id,
                            actionDate = DateTime.Now,
                            actionName = entityEn.approvalStatus,
                            user_Id = entityEn.approvedUser_Id,
                        };

                        model.OrderUpdate.Add(orderUpdate);
                    }
                    if (OrderApprovalentity != null)
                    {
                        // Update the OrderApproval entity
                        OrderApprovalentity.order_Id = entityEn.order_Id;
                        OrderApprovalentity.approvalStatus = (entityEn.approvalStatus == null)? "Awaiting Approval" : entityEn.approvalStatus;
                        OrderApprovalentity.remarks = entityEn.remarks;
                        OrderApprovalentity.approvedUser_Id =(entityEn.remarks==null)?null: entityEn.approvedUser_Id;
                        OrderApprovalentity.approvedDate = (entityEn.remarks == null) ? (DateTime?) null : DateTime.Now;
                        _OrderEn.orderStatus = (entityEn.approvalStatus!=null)? entityEn.approvalStatus: entityEn.OrderStatus; // No need to explicitly assign `null`

                        // Optionally, assign submittedUser_Id if remarks is null
                        OrderApprovalentity.submittedUser_Id = entityEn.submittedUser_Id;
                        OrderApprovalentity.submittedDate = DateTime.Now;

                        // Make sure to update the order total and any other required fields
                        _OrderEn.docOrderAmt = entityEn.OrderTotal;

                        


                        model.SaveChanges();

                        entityEn.isSuccess = true;
                        // Map updated entity back to OrderApprovals
                        entityEn.approvalStatus = OrderApprovalentity.approvalStatus;
                        entityEn.remarks = OrderApprovalentity.remarks;
                        entityEn.approvedUser_Id = OrderApprovalentity.approvedUser_Id;
                        entityEn.approvedDate = OrderApprovalentity.approvedDate;
                        entityEn.submittedUser_Id = OrderApprovalentity.submittedUser_Id;
                        entityEn.submittedDate = OrderApprovalentity.submittedDate;
                        entityEn.isSuccess = true; // Set success flag
                    }

                }
            }
            catch (DbEntityValidationException ex)
            {
                // Capture validation errors
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        // Log or handle each validation error
                        Console.WriteLine("Property: {0}, Error: {1}",
                            validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                // You can also rethrow the error if you want it to propagate
                throw;
            }
            return entityEn;
        }

        public bool DeleteOrderApproval(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _OrderApprovalEn = model.OrderApproval.SingleOrDefault(p => p.id == id);
                    if (_OrderApprovalEn != null)
                    {
                        model.OrderApproval.Remove(_OrderApprovalEn);
                        model.SaveChanges();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public OrderApprovals GetOrderApprovalById(int id)
        {
            OrderApprovals _OrderApprovalEn = new OrderApprovals();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = from oa in model.OrderApproval
                                join o in model.Order
                                    on oa.order_Id equals o.id 
                                select new
                                {
                                    oa.id,
                                    oa.order_Id,
                                    o.orderNum,
                                    CustomerName = o.Customer.name,
                                    o.orderDate,
                                    OrderStatusFromOrder = o.orderStatus,
                                    oa.approvalStatus,
                                    oa.submittedUser_Id,
                                    oa.approvedUser_Id,
                                    Createdby = oa.User.firstName,
                                    ApprovalBy = oa.User.firstName,
                                    o.docOrderAmt,
                                    o.customer_Id,
                                    oa.approvedDate

                                };

                    var result = query
                         .Where(p => p.id == id)
                         .Select(p => new OrderApprovals
                         {
                             id = p.id,
                             orderNum = p.orderNum,
                             CustomerName = p.CustomerName,
                             OrderDate = p.orderDate,
                             OrderStatus = p.approvalStatus,
                             CreatedBy = p.Createdby,
                             ApprovalBy = p.ApprovalBy,
                             OrderTotal = (decimal)p.docOrderAmt,
                             order_Id = p.order_Id,
                             approvalStatus = p.approvalStatus,
                             customerid=p.customer_Id,
                             approvedDate=p.approvedDate


                         })
                         .FirstOrDefault(); // Get a single instance or null

                    _OrderApprovalEn = result;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _OrderApprovalEn;
        }

        public bool IsOrderApprovalExists(OrderApprovals entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.OrderApproval.Any(p => p.order_Id == entityEn.order_Id && (entityEn.id == 0 || p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }


    }
}
