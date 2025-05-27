using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanodale.Entity.Core;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using System.Data.Objects.SqlClient;
using System.Collections;
using System.Globalization;
using Hanodale.Domain.DTOs;
using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain;
using Hanodale.Domain.DTOs.Order;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using Hanodale.Domain.Models;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Security.Cryptography;
using System.Data.Entity.Migrations;

namespace Hanodale.DataAccessLayer.Services
{
    public class OrderService : BaseService, IOrderService
    {
        #region Orders

        /// <summary>
        /// This method is to get the Module Item details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>Orderss list</returns> 

        public OrderDetails GetOrderBySearch(DatatableFilters entityFilter)
        {
            OrderDetails _result = new OrderDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    //get total record

                    //var query = model.Order.AsQueryable();
                    var query = (from o in model.Order
                                 join ou in model.OrderUpdate
                                     on o.id equals ou.order_Id into orderUpdateJoin
                                 from ou in orderUpdateJoin
                                     .Where(x => x.actionName == "PickupAccepted")
                                     .OrderByDescending(p => p.actionDate)
                                     .Take(1)
                                     .DefaultIfEmpty()

                                 join ur in model.Users
                                     on ou.user_Id equals ur.id into userJoin
                                 from ur in userJoin.DefaultIfEmpty()
                                 join ouPayment in model.OrderUpdate
                                    on o.id equals ouPayment.order_Id into paymentJoin
                                 // join oitems in model.OrderItems
                                 //on o.id equals oitems.orderId 
                                 from ouPayment in paymentJoin
                                     .Where(x => x.actionName == "Payment")
                                     .OrderByDescending(p => p.actionDate)
                                     .Take(1)
                                     .DefaultIfEmpty()
                                 join cashier in model.Users
                                     on ouPayment.user_Id equals cashier.id into cashierJoin
                                 from cashier in cashierJoin.DefaultIfEmpty()
                                 select new Orders
                                 {
                                     id = o.id,
                                     customerName = o.Customer.name ?? string.Empty,
                                     orderNum = o.orderNum,
                                     shipToAddressCode = o.ShipToAddress.shippingCode ?? string.Empty,
                                     orderDate = o.orderDate,
                                     orderTotal = o.docOrderAmt,
                                     orderStatus = o.orderStatus,
                                     createdBy = o.entryPerson,
                                     createdDate = o.createdDate,
                                     customer_Id = o.customer_Id,
                                     orderComment = o.orderComment,
                                     picker = (ur != null ? (!string.IsNullOrEmpty(ur.firstName) ? ur.firstName : "") + " " + (!string.IsNullOrEmpty(ur.lastName) ? ur.lastName : "") : null),
                                     pickerDate = ou.actionDate,
                                     pickerUserId = ur != null ? ur.id : 0,
                                     payment = cashier != null ? (string.IsNullOrEmpty(cashier.firstName) ? "" : cashier.firstName) + " " + (string.IsNullOrEmpty(cashier.lastName) ? "" : cashier.lastName) : null,
                                     paymentDate = ouPayment != null ? ouPayment.actionDate : (DateTime?)null,
                                     syncStatus = o.syncStatus,
                                     epicoreResponse = o.epicoreResponse,
                                     syncedAt=o.syncedAt


                                 });




                    if (entityFilter.masterRecord_Id != 0)
                    {
                        query = query.Where(p => p.customer_Id == entityFilter.masterRecord_Id);
                    }

                    if (entityFilter.conditionType != null && entityFilter.conditionType != "PostingFailed" && entityFilter.conditionType != "Payment" && entityFilter.conditionType != "Pending" && entityFilter.conditionType != "PickupAll")
                    {
                        query = query.Where(p => p.orderStatus == entityFilter.conditionType);
                    }

                    if (entityFilter.conditionType == "PickupAll")
                    {
                        var statuses = new[] { "SubmitForPicking" };
                        query = query.Where(p => statuses.Contains(p.orderStatus));
                    }

                    // If pending show all except Completed and Cancelled
                    if (entityFilter.conditionType == "Pending")
                    {
                        query = query.Where(p => p.orderStatus != "Completed" && p.orderStatus != "Cancelled" && p.orderStatus != "Payment");
                    }
                    if (entityFilter.conditionType == "Payment")
                    {
                        query = query.Where(p => p.orderStatus =="Payment");
                    }
                    if (entityFilter.conditionType == "PostingFailed")
                    {
                        query = query.Where(p =>
                            p.orderStatus == "Completed" &&
                            p.syncStatus == false &&
                            !string.IsNullOrEmpty(p.epicoreResponse) && p.syncedAt!=null);
                    }
                    if ( !String.IsNullOrEmpty(entityFilter.CustomerName))
                    {
                        query = query.Where(p => p.customerName.Contains(entityFilter.CustomerName));
                    }
                    if (!String.IsNullOrEmpty(entityFilter.OrderNum))
                    {
                        query = query.Where(p => p.orderNum.Contains(entityFilter.OrderNum));
                    }
                    if (!String.IsNullOrEmpty(entityFilter.CreatedBy))
                    {
                        query = query.Where(p => p.createdBy.Contains(entityFilter.CreatedBy));
                    }
                    if (!string.IsNullOrEmpty(entityFilter.OrderDateFrom) || !string.IsNullOrEmpty(entityFilter.OrderDateTo))
                    {
                        DateTime parsedDateFrom = DateTime.MinValue; // Default to MinValue if parsing fails
                        bool isDateFrom = DateTime.TryParse(entityFilter.OrderDateFrom, out parsedDateFrom); // Try parsing OrderDateFrom
                        if (!isDateFrom) parsedDateFrom = DateTime.MinValue; // Use MinValue if invalid
                        parsedDateFrom = isDateFrom
                        ? parsedDateFrom.Date // Set time to 00:00:00
                        : DateTime.MinValue;
                        DateTime parsedDateTo = DateTime.MaxValue; // Default to MaxValue if parsing fails
                        bool isDateTo = DateTime.TryParse(entityFilter.OrderDateTo, out parsedDateTo); // Try parsing OrderDateTo
                        if (!isDateTo) parsedDateTo = DateTime.MaxValue; // Use MaxValue if invalid
                        parsedDateTo = parsedDateTo == default
                            ? DateTime.MaxValue
                            : parsedDateTo.Date.AddDays(1).AddTicks(-1);
                        // Apply the filter only if valid dates are provided
                        query = query.Where(p =>
                            p.orderDate.HasValue &&
                            p.orderDate.Value >= parsedDateFrom && p.orderDate.Value <= parsedDateTo
                        );
                    }
                   // query = query.Where(p => (EntityFunctions.DiffDays(p.orderDate, DateTime.Now) <= 7));
                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        string searchValue = entityFilter.search.Trim();
                        decimal parsedDecimal;
                        bool isDecimal = decimal.TryParse(searchValue, out parsedDecimal);
                        DateTime parsedDate;
                        bool isDate = DateTime.TryParse(searchValue, out parsedDate);  


                        query = query.Where(p =>
                            (p.orderComment ?? "").Contains(searchValue) ||
                             (p.orderNum ?? "").Contains(searchValue) ||
                            (p.orderStatus ?? "").Contains(searchValue) ||
                            (p.shipToAddressCode ?? "").Contains(searchValue) ||
                            (p.customerName ?? "").Contains(searchValue) ||
                            (p.createdBy ?? "").Contains(searchValue) ||
                            (p.picker ?? "").Contains(searchValue) ||
                            (p.payment ?? "").Contains(searchValue) ||
                            (isDecimal && p.orderTotal == parsedDecimal)|| // Use the parsed decimal value here
                           (isDate && p.orderDate.HasValue && p.orderDate.Value == parsedDate) // Compare order date if parsed as DateTime
    );
                    }

                   
 
                    var result = query.OrderByDescending(p => p.id);

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    var resultList = result.ToList();

                    // Now apply formatting to the createdBy field
                    foreach (var order in resultList)
                    {
                        // Format the createdBy field with createdDate and append a line break
                        order.createdBy = order.createdBy + "<br/>" + (order.createdDate?.ToString("dd/MM/yyyy hh:mm tt") ?? string.Empty);

                        // Format the picker field with pickerDate and append a line break
                        order.picker = order.picker + "<br/>" + (order.pickerDate?.ToString("dd/MM/yyyy hh:mm tt") ?? string.Empty);

                        // Format the payment field with paymentDate and append a line break
                        order.payment = order.payment + "<br/>" + (order.paymentDate?.ToString("dd/MM/yyyy hh:mm tt") ?? string.Empty);

                        if (order.orderStatus == "Completed")
                        {
                            if (order.syncStatus == true && order.epicoreResponse == "Sales Order Created Successfully")
                            {
                                // Append a <div> tag or any other HTML you want
                                order.orderStatus = order.orderStatus; // + "<div class='status-label btn-success' style='padding-left:0px;padding-right:0px'>Posting Success</div>";
                                order.postStatus = 1;
                            }
                            else if (order.syncStatus == false && !string.IsNullOrEmpty(order.epicoreResponse))
                            {

                                // Append a <div> tag or any other HTML you want
                                order.orderStatus = order.orderStatus; //  + "<div class='status-label  btn-danger' style='padding-left:0px;padding-right:0px'>Posting Failed</div>";
                                order.postStatus = 2;
                            }
                            else
                            {
                                order.orderStatus = order.orderStatus; //  + "<div class='status-label  btn-danger' style='padding-left:0px;padding-right:0px'>Posting Failed</div>";
                                order.postStatus = 3;
                            }
                        }
                        else
                        {
                            order.orderStatus = order.orderStatus;
                        }
                    }

                    // Set the formatted result back to _result.lstOrder
                    //_result.lstOrder = resultList;
                    _result.lstOrder = resultList.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public List<OrderUpdates> GetOrderLog(int orderId)
        {
            var OrderLogList = new List<OrderUpdates>();
            //List<CustomerPriceList> entities;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {


                    var entities = new List<OrderUpdates>(); ;

                    entities = (from orderUpdate in model.OrderUpdate
                                join user in model.Users on orderUpdate.user_Id equals user.id
                                orderby orderUpdate.actionDate ascending
                                where orderUpdate != null && orderUpdate.order_Id == orderId
                                select new OrderUpdates
                                {
                                    userName = user.userName,
                                    actionName = orderUpdate.actionName,
                                    actionDate = orderUpdate.actionDate
                                }).ToList();


                    if (entities.Any())
                    {
                        OrderLogList = entities.Select(entity => new OrderUpdates
                        {

                            userName = entity.userName,
                            //actionDate = entity.actionDate.HasValue ? entity.actionDate.Value.ToString("dd/MM/yyyy") : string.Empty, // Formatting date
                            actionName = entity.actionName,
                            actionDateFormatted = entity.actionDate.HasValue ? entity.actionDate.Value.ToString("dd/MM/yyyy hh:mm tt") : string.Empty // For null dates

                        }).ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return OrderLogList;
        }

        /// <summary>
        /// This method is to save the Orderss details
        /// </summary> 
        public Orders CreateOrder(Orders entityEn)
        {
            var _orderEn = new Order();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Order

                    /*_orderEn.id = entityEn.id;
                    _orderEn.customer_Id = entityEn.customer_Id;
                    _orderEn.orderNum = entityEn.orderCode;
                    _orderEn.priceTier = entityEn.priceTier;
                    _orderEn.shipToAddress_Id = entityEn.shipToAddress_Id;
                    _orderEn.orderDate = entityEn.orderDate;*/
                    model.Order.Add(_orderEn);
                    model.SaveChanges();

                    entityEn.id = _orderEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        /// <summary>
        /// This method is to update the Orderss details
        /// </summary> 
        public Orders UpdateOrder(Orders entityEn)
        {
            var _orderEn = new Order();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _orderEn = model.Order.SingleOrDefault(p => p.id == entityEn.id);
                    if (_orderEn != null)
                    {


                        _orderEn.id = entityEn.id;
                        //_orderEn.customer_Id = entityEn.customer_Id;
                        /*_orderEn.orderCode = entityEn.orderCode;
                        _orderEn.priceTier = entityEn.priceTier;
                        _orderEn.shipToAddress_Id = entityEn.shipToAddress_Id;
                        _orderEn.orderDate = entityEn.orderDate; */

                        model.Order.Add(_orderEn);


                    }
                    model.SaveChanges();
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        /// <summary>
        /// This method is to delete the stock details
        /// </summary>
        /// <param name="stockId">stock id</param>  
        public bool DeleteOrder(int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _companyStatutoryEn = model.Order.SingleOrDefault(p => p.id == id);

                    if (_companyStatutoryEn != null)
                    {
                        model.Order.Remove(_companyStatutoryEn);
                    }
                    model.SaveChanges();
                }
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to get the Orderss by Orderss id
        /// </summary>
        /// <param name="roleId">Orderss Id</param>
        /// <returns>Orderss details</returns>
        public Orders GetOrderById(int id)
        {
            var _orderEn = new Orders();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Order.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _orderEn = new Orders
                        {
                            id = entity.id,
                            /*customer_Id = entity.customer_Id ?? 0,
                            customerName = entity.Customer.name,
                            orderCode = entity.orderCode,
                            priceTier = entity.priceTier, */
                            // shipToAddress_Id = entity.shipToAddress_Id ?? 0,
                            orderDate = entity.orderDate,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _orderEn;
        }

        ///// <summary>
        ///// This method is to check the Orderss exists or not.
        ///// </summary>
        ///// <param name="stockName">Orders Name</param>  
        public bool IsOrderExists(Orders entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.Order.Any(p => (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public Order SubmitOrder(SubmitOrder entity, int userId)
        {
            Order ordermodel = new Order();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var store = model.Store.FirstOrDefault();
                    //var nextOrderNumber = model.Order.Any()
                    //? model.Order.Max(o => o.orderNum) + 1
                    //: 1;
                    //var activeYearRecord = model.POSOrderNumber
                    //    .FirstOrDefault(o => o.IsActive);
                    //activeYearRecord.LastRunningNumber++;

                    var currentDate = DateTime.UtcNow;
                    int financialYearStart = currentDate.Month >= 7
                        ? currentDate.Year + 1
                        : currentDate.Year;
                    int financialYearSuffix = financialYearStart % 100;
                    // Check if there's an active record for the current financial year
                    var activeYearRecord = model.POSOrderNumber
                        .FirstOrDefault(o => o.Year == financialYearSuffix && o.IsActive);

                    if (activeYearRecord == null)
                    {
                        // No active record for the current financial year, create a new one
                        activeYearRecord = new POSOrderNumber
                        {
                            Year = financialYearSuffix,
                            LastRunningNumber = 10000,
                            IsActive = true
                        };
                        model.POSOrderNumber.Add(activeYearRecord);
                    }
                    else
                    {
                        // Increment the last running number
                        activeYearRecord.LastRunningNumber++;
                        model.Entry(activeYearRecord).State = EntityState.Modified;
                    }

                    var currentUser = model.Users.Where(p => p.id == userId).FirstOrDefault();
                    decimal totalOrderAmount = 0; // Math.Round(entity.OrderItems.Sum(i => Math.Round(((i.unitPrice) * i.orderQty) + i.operationCost - (i.discountAmt ?? 0), 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero);
                    var nextOrderNumber = $"POSPD{activeYearRecord.Year}-{activeYearRecord.LastRunningNumber:D6}";

                    var order = new Order
                    {
                        company = "LUCKY05", // Default first store
                        orderNum = nextOrderNumber,
                        customer_Id = entity.customer_Id,
                        entryPerson = currentUser.userName, // "System Admin",
                        shipToAddress_Id = entity.shipToAddress_Id == 0 ? (int?)null : entity.shipToAddress_Id,
                        districtId = entity.districtId == 0 ? (int?)null : entity.districtId,
                        orderDate = DateTime.ParseExact(entity.orderDate.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", null), // entity.orderDate, // DateTime.Now, 
                        salesRepList = "",
                        orderComment = entity.orderComment,
                        district = "",
                        docOrderAmt = totalOrderAmount, // entity.OrderItems.Sum(i => i.unitPrice * i.orderQty),
                        orderStatus = entity.orderStatus, // "Pending"
                        oneTimeCustomer = entity.oneTimeCustomer,
                        orderContact = entity.orderContact,
                        orderContactPhone = entity.orderContactPhone,
                        orderContactName = entity.orderContactName,
                        createdBy = userId,
                        createdDate = DateTime.Now
                    };

                    foreach (var itemDto in entity.OrderItems)
                    {
                        System.Diagnostics.Debug.WriteLine(itemDto);
                        //var scannedQty = string.IsNullOrEmpty(itemDto.scannedLabel) ? 0 : itemDto.orderQty;
                        //if (!itemDto.allowVaryWeight && !string.IsNullOrEmpty(itemDto.scannedLabel))
                        //{
                        //    scannedQty = 1;
                        //}
                        var itemQty = itemDto.scannedQty ?? 0;//scannedQty == 0 ? itemDto.orderQty : scannedQty;
                        decimal subtotal = Math.Round(((itemDto.unitPrice) * itemQty) + itemDto.operationCost, 2, MidpointRounding.AwayFromZero);
                        decimal discountAmount = 0;
                        if (itemDto.discountPer.HasValue && itemDto.discountPer > 0)
                        {
                            discountAmount = Math.Round((subtotal * itemDto.discountPer.Value) / 100, 2, MidpointRounding.AwayFromZero);
                        }
                        itemDto.discountAmt = discountAmount;
                        totalOrderAmount += Math.Round(((itemDto.unitPrice) * itemQty) + itemDto.operationCost - discountAmount, 2, MidpointRounding.AwayFromZero);
                        var orderItem = new OrderItems
                        {
                            orderLine = entity.OrderItems.IndexOf(itemDto) + 1,
                            partNum = itemDto.partNum,
                            product_Id = itemDto.product_Id,
                            lineDesc = itemDto.lineDesc,
                            ium = itemDto.ium,
                            salesUm = itemDto.ium,
                            unitPrice = itemDto.unitPrice,
                            orderQty = itemDto.orderQty,
                            discount = itemDto.discount,
                            listPrice = itemDto.listPrice,
                            returnTotal = itemDto.returnTotal,
                            QtyType_ModuleItem_Id = itemDto.QtyType_ModuleItem_Id != 0
                            ? itemDto.QtyType_ModuleItem_Id
                            : (int?)null,
                            OrderUOM_Id = itemDto.OrderUOM_Id != 0
                            ? itemDto.OrderUOM_Id
                            : (int?)null,
                            operationStyle_ModuleItem_Id = itemDto.operationStyle_ModuleItem_Id != 0
                            ? itemDto.operationStyle_ModuleItem_Id
                            : (int?)null,
                            operationCost = itemDto.operationCost,
                            actualOperationCost = itemDto.actualOperationCost,
                            complimentary_ModuleItem_Id = itemDto.complimentary_ModuleItem_Id != 0
                            ? itemDto.complimentary_ModuleItem_Id
                            : (int?)null,
                            allowVaryWeight = itemDto.allowVaryWeight,
                            originalUnitPrice = itemDto.originalUnitPrice,
                            realOriginalUnitPrice = itemDto.realOriginalUnitPrice,
                            Comments = itemDto.comments,
                            scannedLabel = string.IsNullOrWhiteSpace(itemDto.scannedLabel)?string.Empty: itemDto.scannedLabel,
                            Location = itemDto.scannedLocation,
                            scannedQty = itemDto.scannedQty,
                            discountPer = itemDto.discountPer,
                            discountAmt = itemDto.discountAmt,
                            createdAt = DateTime.Now,
                            createdBy = userId,
                        };
                        order.OrderItems.Add(orderItem);


                    }
                    order.OrderUpdate.Add(new OrderUpdate
                    {
                        actionDate = DateTime.Now,
                        actionName = "Submitted",
                        user_Id = userId,
                        orderStatus= "Submitted"
                    });

                    order.docOrderAmt = totalOrderAmount;
                    model.Order.Add(order);
                    model.SaveChanges();

                    
                    
                    // Store the scanned label to the database.
                    var savedOrderItems = model.OrderItems
                        .Where(oi => oi.orderId == order.id)
                        .ToList();

                    //// Get the all scanlabels from the order items
                    //var _scanLabels = savedOrderItems.Select(oi => oi.scannedLabel).ToList();
                    
                    //List<ProductCarton> productCartonBarcodes = new List<ProductCarton>();
                    //if (_scanLabels != null && _scanLabels.Count > 0)
                    //{
                    //    productCartonBarcodes = model.ProductCarton
                    //    .Where(p => _scanLabels.Contains(p.barcode) && p.IsPickedComplete == false && p.OnHold == false)
                    //        .ToList();
                    //}
                    // Iterate over the retrieved order items
                    foreach (var savedOrderItem in savedOrderItems)
                    {

                        if (!string.IsNullOrWhiteSpace(savedOrderItem.scannedLabel))
                        {
                            // Get ProductCtn Barcode / Weighting Barcode
                            // Location
                            //var looseBarcode = productCartonBarcodes.Where(a=>a.barcode == savedOrderItem.scannedLabel).FirstOrDefault();
                            if (savedOrderItem.allowVaryWeight == true)
                            {
                                var newScannedItem = new OrderItemScanned
                                {
                                    orderItemId = savedOrderItem.orderItemId ?? savedOrderItem.id,
                                    serialNo = savedOrderItem.scannedLabel,
                                    scannedQty = savedOrderItem.orderQty, // Define specific scanned quantity if available
                                    scannedDate = DateTime.Now,
                                    scannedBy = currentUser.userName,
                                    status = "Matched", // Replace with savedOrderItem.Status if needed
                                    orderId = savedOrderItem.orderId,
                                    Location = savedOrderItem.Location,
                                };
                                model.OrderItemScanned.Add(newScannedItem);

                                if (savedOrderItem.allowVaryWeight==true)
                                {
                                    // Carton Vwg Or Loose Vwg is unique SerialNumber, update as Pickupcomplete = true
                                    var vwgBarcode = model.ProductCarton.Where(p => p.epicorPartNo == savedOrderItem.partNum  
                                                && p.barcode == savedOrderItem.scannedLabel  && p.Location == savedOrderItem.Location
                                                && p.IsPickedComplete == false && p.OnHold == false)
                                        .FirstOrDefault();
                                    if (vwgBarcode != null)
                                    {
                                        vwgBarcode.IsPickedComplete = true;
                                        vwgBarcode.PickedCompletedDate = DateTime.Now;
                                        vwgBarcode.PickedOrderId = savedOrderItem.orderId;
                                        vwgBarcode.PickedOrderItemId = savedOrderItem.id;
                                        vwgBarcode.PickedLastModifiedDate = DateTime.Now;
                                        vwgBarcode.IsPickStatusToSync = true;
                                        model.Entry(vwgBarcode).State = EntityState.Modified;
                                    }
                                }
                                else
                                {
                                    // Std Carton is not unique, update as Pickupcomplete = true for top item
                                    var stdCartonBarcode = model.ProductCarton.Where(p => p.epicorPartNo == savedOrderItem.partNum 
                                                        && p.barcode == savedOrderItem.scannedLabel && p.Location == savedOrderItem.Location
                                                && p.IsCarton==true && p.IsPickedComplete == false && p.OnHold == false)
                                        .FirstOrDefault();
                                    if (stdCartonBarcode != null)
                                    {
                                        stdCartonBarcode.IsPickedComplete = true;
                                        stdCartonBarcode.PickedCompletedDate = DateTime.Now;
                                        stdCartonBarcode.PickedOrderId = savedOrderItem.orderId;
                                        stdCartonBarcode.PickedOrderItemId = savedOrderItem.id;
                                        stdCartonBarcode.PickedLastModifiedDate = DateTime.Now;
                                        stdCartonBarcode.IsPickStatusToSync = true;
                                        model.Entry(stdCartonBarcode).State = EntityState.Modified;
                                    }
                                }
                                
                            }
                            else
                            {
                                //Check later this logic when it works - Ramesh
                                int orderQty = 1; //  (int)Math.Floor(savedOrderItem.orderQty); // Ensure orderQty is not null

                                for (int i = 0; i < orderQty; i++)
                                {
                                    var newScannedItem = new OrderItemScanned
                                    {
                                        orderItemId = savedOrderItem.orderItemId ?? savedOrderItem.id,
                                        serialNo = savedOrderItem.scannedLabel, // You can modify this if serial numbers vary
                                        scannedQty = 1, // Always set to 1
                                        scannedDate = DateTime.Now,
                                        scannedBy = currentUser.userName,
                                        status = "Matched", // Replace with savedOrderItem.Status if needed
                                        orderId = savedOrderItem.orderId,
                                        Location = savedOrderItem.Location,
                                    };

                                    model.OrderItemScanned.Add(newScannedItem);
                                }
                            }

                        }
                        if (savedOrderItem.discountAmt > 0)
                        {
                            var orderdiscountapprovals = model.OrderItemDiscountApproval
         .FirstOrDefault(p => (p.orderId == null || p.orderId == 0) &&
                              (p.orderItem_Id == null || p.orderItem_Id == 0));
                            if (orderdiscountapprovals != null)
                            {
                                orderdiscountapprovals.orderItem_Id = savedOrderItem.orderItemId ?? savedOrderItem.id;
                                orderdiscountapprovals.orderId = savedOrderItem.orderId;
                                model.Entry(orderdiscountapprovals).State = EntityState.Modified;
                            }

                            model.SaveChanges();
                        }
                        savedOrderItem.orderItemId = savedOrderItem.id;
                        model.Entry(savedOrderItem).State = EntityState.Modified;
                        var OrderUpdate = model.OrderUpdate.Where(p => p.order_Id == null).ToList();
                        foreach (var item in OrderUpdate)
                        {
                            item.order_Id = savedOrderItem.orderId;

                            model.Entry(item).State = EntityState.Modified;
                        }

                    }

                    // Save any additional changes to OrderItemScanned
                    model.SaveChanges();



                    ordermodel = order;
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"Property: {e.PropertyName} Error: {e.ErrorMessage}")
                    .ToList();

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = $"Validation failed for one or more entities. Details: {fullErrorMessage}";

                throw new FaultException(exceptionMessage);
            }
            catch (Exception ex)
            {

                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return ordermodel;
        }

        public bool SubmitReturnedItems(SubmitReturnItems entity, int userId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Find the existing order
                    var currentUser = model.Users.FirstOrDefault(p => p.id == userId);

                    if (currentUser == null)
                    {
                        throw new FaultException($"User with ID {userId} not found.");
                    }
                    var order = model.Order.FirstOrDefault(o => o.id == entity.OrderId);
                    model.Entry(order).State = EntityState.Modified;
                    var actionName = "Return Items";

                    var orderUpdate = new OrderUpdate
                    {
                        order_Id = entity.OrderId,
                        actionDate = DateTime.Now,
                        actionName = actionName,
                        user_Id = userId,
                    };
                    model.OrderUpdate.Add(orderUpdate);
                    if (order == null)
                    {
                        throw new FaultException($"Order with ID {entity.OrderId} not found.");
                    }
                    // Create a dictionary for quick lookup of returned items
                    var returnedItemsDict = entity.ReturnOrderItems.ToDictionary(
                        item => item.OrderItemId,
                        item => item
                    );
                    // Reset all status to not returned 
                    foreach (var orderItem in order.OrderItems)
                    {
                        orderItem.IsItemReturned = false;
                        // Check if this item is in the returned items
                        if (returnedItemsDict.TryGetValue(orderItem.orderItemId ?? 0, out var returnedItem))
                        {
                            // Update the discount amount and cutting cost for returned items
                            orderItem.discountAmt = returnedItem.discountAmt;
                        }
                        model.Entry(orderItem).State = EntityState.Modified;
                    }
                    var orderScannedItems = model.OrderItemScanned.Where(p => p.orderId == entity.OrderId).ToList();
                    foreach (var orderScannedItem in orderScannedItems)
                    {
                        orderScannedItem.IsItemReturned = false;
                        model.Entry(orderScannedItem).State = EntityState.Modified;
                    }

                    //decimal totalAmount = 0; 
                    //foreach(var orderItem in order.OrderItems)
                    //{
                    //    // Calculate the amount for each order item
                    //    decimal itemAmount = (orderItem.unitPrice ?? 0) *
                    //     (orderItem.scannedQty == null || orderItem.scannedQty == 0
                    //         ? orderItem.orderQty
                    //         : orderItem.scannedQty ?? 0) + orderItem.operationCost ?? 0;

                    //    // Round the item amount and add it to the total amount
                    //    totalAmount += Math.Round(itemAmount, 2, MidpointRounding.AwayFromZero);
                    //}

                    //decimal returnedAmount = 0;
                    // Add new ScannedItems
                    foreach (var scannedItem in entity.ReturnedItems)
                    {
                        var existingScannedItem = model.OrderItemScanned.FirstOrDefault(si => si.orderItemId == scannedItem.OrderItemId && si.id == scannedItem.scannedId);
                        var existingOrderItem = model.OrderItems.FirstOrDefault(oi => oi.orderItemId == scannedItem.OrderItemId);

                        if (existingScannedItem != null && existingOrderItem != null)
                        {
                            existingOrderItem.IsItemReturned = true;
                            //decimal unitPrice = existingOrderItem.unitPrice.GetValueOrDefault(0);
                            //decimal qty = existingScannedItem.scannedQty;
                            //returnedAmount += Math.Round(unitPrice * qty, 2, MidpointRounding.AwayFromZero);
                            // Also calculate cutting cost and reduce it 
                            // Update scanned items 
                            existingScannedItem.IsItemReturned = true;
                            existingScannedItem.returnQty = scannedItem.returnQty;
                            var returnedItem = entity.ReturnOrderItems.FirstOrDefault(oi => oi.OrderItemId == scannedItem.OrderItemId);
                            if (returnedItem != null)
                            {
                                existingOrderItem.operationCost = returnedItem.cuttingCost;
                                existingOrderItem.listPrice = returnedItem.listPrice;
                                existingOrderItem.returnTotal = returnedItem.returnTotal;
                            }
                            model.Entry(existingOrderItem).State = EntityState.Modified;
                            model.Entry(existingScannedItem).State = EntityState.Modified;
                        }
                    }
                    // Reduce returned item amounts
                    //order.docOrderAmt = Math.Round(totalAmount - returnedAmount, 2, MidpointRounding.AwayFromZero);
                    order.docOrderAmt = entity.orderTotal;
                    model.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                throw;
            }
            catch (Exception ex)
            {

                // Consider logging the full exception details before throwing
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return true;
        }

        public bool SubmitScannedItems(SubmitOrderScan entity, int userId, bool IsVerification)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Find the existing order
                    var currentUser = model.Users.FirstOrDefault(p => p.id == userId);

                    if (currentUser == null)
                    {
                        throw new FaultException($"User with ID {userId} not found.");
                    }
                    var order = model.Order.FirstOrDefault(o => o.id == entity.OrderId);
                    model.Entry(order).State = EntityState.Modified;
                    var actionName = "";
                    if (IsVerification)
                    {
                        order.verifiedStatus = "Pass";
                        actionName = "Verified";
                        order.verifiedBy = userId;
                        order.verifiedDate = DateTime.Now;
                    }
                    else
                    {
                        if (entity.action == "submit")
                        {
                            order.orderStatus = "Picked";
                            actionName = "Items Scanned";
                        }
                    }
                    var orderUpdate = new OrderUpdate
                    {
                        order_Id = entity.OrderId,
                        actionDate = DateTime.Now,
                        actionName = actionName,
                        user_Id = userId,
                    };
                    model.OrderUpdate.Add(orderUpdate);
                    if (order == null)
                    {
                        throw new FaultException($"Order with ID {entity.OrderId} not found.");
                    }

                    decimal totalAmount = 0;
                    // Update existing OrderItems
                    foreach (var scannedItem in entity.OrderItems)
                    {
                        var orderItem = model.OrderItems.FirstOrDefault(oi => oi.id == scannedItem.itemId);
                        if (orderItem != null)
                        {
                            model.Entry(orderItem).State = EntityState.Modified;
                            orderItem.scannedQty = scannedItem.ScannedQty;
                            orderItem.orderItemId = scannedItem.OrderItemId;
                            // Update other fields as necessary
                            // Calculate and add the cutting cost here also
                            var cuttingCost = orderItem.actualOperationCost;
                            decimal finalQtyWd = Math.Floor(scannedItem.ScannedQty);
                            var finalCutCost = finalQtyWd * cuttingCost;
                            orderItem.operationCost = finalCutCost;
                            // totalAmount += Math.Round((scannedItem.ScannedQty * orderItem.unitPrice ?? 0) + finalCutCost ?? 0, 2, MidpointRounding.AwayFromZero);
                            // Calculate subtotal (quantity * unit price + cutting cost)
                            decimal subtotal = Math.Round((scannedItem.ScannedQty * (orderItem.unitPrice ?? 0)) + (finalCutCost ?? 0), 2, MidpointRounding.AwayFromZero);

                            // Calculate discount amount if discount percentage exists
                            decimal discountAmount = 0;
                            if (orderItem.discountPer.HasValue && orderItem.discountPer > 0)
                            {
                                discountAmount = Math.Round((subtotal * orderItem.discountPer.Value) / 100, 2, MidpointRounding.AwayFromZero);
                            }

                            // Calculate final total after discount
                            decimal itemTotal = subtotal - discountAmount;
                            orderItem.listPrice = itemTotal;
                            // Store the discount amount if needed
                            orderItem.discountAmt = discountAmount;

                            // Add to total amount
                            totalAmount += itemTotal;

                            System.Diagnostics.Debug.WriteLine(totalAmount);
                        }
                    }
                    order.docOrderAmt = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
                    // Add new ScannedItems
                    if (!IsVerification)
                    {
                        if (entity.ScannedItems != null && entity.ScannedItems.Count > 0)
                        {
                            //foreach (var scannedItem in entity.ScannedItems)
                            //{
                            //    if (IsVerification)
                            //    {
                            //        var existingScannedItem = model.OrderItemScanned.FirstOrDefault(si => si.orderItemId == scannedItem.OrderItemId);
                            //        if (existingScannedItem != null)
                            //        {
                            //            existingScannedItem.verifyStatus = "Verified";
                            //            model.Entry(existingScannedItem).State = EntityState.Modified;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        // Delete previous records and add again.
                            //        //foreach (var item in entity.ScannedItems)
                            //        //{
                            //        //    var existingScannedItem = model.OrderItemScanned.FirstOrDefault(si => si.orderItemId == scannedItem.OrderItemId);
                            //        //    if (existingScannedItem != null)
                            //        //    {
                            //        //        model.OrderItemScanned.Remove(existingScannedItem);
                            //        //    }
                            //        //}
                            //        // Get all the OrderItemIds from the scanned items
                            //        try
                            //        {
                            //            var orderItemIds = entity.OrderItems.Select(si => si.OrderItemId).ToList();

                            //            // Fetch all matching scanned items in a single query
                            //            var existingScannedItems = model.OrderItemScanned
                            //                .Where(si => orderItemIds.Contains((int)si.orderItemId))
                            //                .ToList();

                            //            // Remove the existing scanned items in bulk
                            //            foreach (var item in existingScannedItems)
                            //            {
                            //                model.OrderItemScanned.Remove(item);
                            //            }
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            System.Diagnostics.Debug.WriteLine(ex);
                            //        }
                            //        var newScannedItem = new OrderItemScanned
                            //        {
                            //            orderItemId = scannedItem.OrderItemId,
                            //            serialNo = scannedItem.SerialNumber,
                            //            scannedQty = scannedItem.ScannedQty,
                            //            scannedDate = DateTime.Now,
                            //            scannedBy = currentUser.userName,
                            //            status = scannedItem.Status,
                            //            orderId = scannedItem.OrderId,
                            //            Location = scannedItem.productLocation,

                            //            returnQty = 0
                            //        };
                            //        System.Diagnostics.Debug.WriteLine("LOCATION RECEIVED IN SERVICE: " + scannedItem.productLocation);

                            //        model.OrderItemScanned.Add(newScannedItem);
                            //    }

                            //}
                            var newScannedItems = entity.ScannedItems;
                            var newScannedOrderItemIds = newScannedItems.Select(si => si.OrderItemId).Distinct().ToList();

                            // Fetch all existing scanned items for the current order
                            var existingScannedItems = model.OrderItemScanned
                                .Where(si => si.orderId == entity.OrderId)
                                .ToList();

                            // DELETE: Remove only scanned items that are no longer in the new scanned list
                            var scannedItemsToDelete = existingScannedItems
                                .Where(existing => !newScannedOrderItemIds.Contains(existing.orderItemId ?? 0))
                                .ToList();

                            foreach (var item in scannedItemsToDelete)
                                model.OrderItemScanned.Remove(item);

                            // UPSERT: Update if exists, else insert

                            foreach (var scannedItem in newScannedItems)
                            {
                                var existingItem = existingScannedItems
                                    .FirstOrDefault(si => si.orderItemId == scannedItem.OrderItemId);

                                if (existingItem != null)
                                {
                                    // UPDATE existing scanned item
                                    existingItem.serialNo = scannedItem.SerialNumber;
                                    existingItem.scannedQty = scannedItem.ScannedQty;
                                    existingItem.scannedDate = DateTime.Now;
                                    existingItem.scannedBy = currentUser.userName;
                                    existingItem.status = scannedItem.Status;
                                    existingItem.Location = scannedItem.productLocation;
                                    existingItem.returnQty = 0;
                                    model.Entry(existingItem).State = EntityState.Modified;
                                }
                                else
                                {
                                    // INSERT new scanned item
                                    var newScannedItem = new OrderItemScanned
                                    {
                                        orderItemId = scannedItem.OrderItemId,
                                        serialNo = scannedItem.SerialNumber,
                                        scannedQty = scannedItem.ScannedQty,
                                        scannedDate = DateTime.Now,
                                        scannedBy = currentUser.userName,
                                        status = scannedItem.Status,
                                        orderId = scannedItem.OrderId,
                                        Location = scannedItem.productLocation,
                                        returnQty = 0
                                    };
                                    model.OrderItemScanned.Add(newScannedItem);

                                    var orderItem = model.OrderItems.FirstOrDefault(oi => oi.id == newScannedItem.orderItemId);
                                    if (orderItem.allowVaryWeight == true)
                                    {
                                        // Carton Vwg Or Loose Vwg is unique SerialNumber, update as Pickupcomplete = true
                                        var vwgBarcode = model.ProductCarton.Where(p => p.epicorPartNo == orderItem.partNum
                                                    && p.barcode == newScannedItem.serialNo
                                                    && p.IsPickedComplete == false && p.OnHold == false)
                                            .FirstOrDefault();
                                        if (vwgBarcode != null)
                                        {
                                            vwgBarcode.IsPickedComplete = true;
                                            vwgBarcode.PickedCompletedDate = DateTime.Now;
                                            vwgBarcode.PickedOrderId = orderItem.orderId;
                                            vwgBarcode.PickedOrderItemId = orderItem.id;
                                            vwgBarcode.PickedLastModifiedDate = DateTime.Now;
                                            vwgBarcode.IsPickStatusToSync = true;
                                            model.Entry(vwgBarcode).State = EntityState.Modified;
                                        }
                                    }
                                    else
                                    {
                                        // Std Carton is not unique, update as Pickupcomplete = true for top item
                                        var stdCartonBarcode = model.ProductCarton.Where(p => p.epicorPartNo == orderItem.partNum
                                                            && p.barcode == newScannedItem.serialNo
                                                    && p.IsCarton == true && p.IsPickedComplete == false && p.OnHold == false)
                                            .FirstOrDefault();
                                        if (stdCartonBarcode != null)
                                        {
                                            stdCartonBarcode.IsPickedComplete = true;
                                            stdCartonBarcode.PickedCompletedDate = DateTime.Now;
                                            stdCartonBarcode.PickedOrderId = orderItem.orderId;
                                            stdCartonBarcode.PickedOrderItemId = orderItem.id;
                                            stdCartonBarcode.PickedLastModifiedDate = DateTime.Now;
                                            stdCartonBarcode.IsPickStatusToSync = true;
                                            model.Entry(stdCartonBarcode).State = EntityState.Modified;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            // If no scanned items sent, remove all existing ones
                            var allScannedItems = model.OrderItemScanned.Where(w=>w.orderId== entity.OrderId).ToList();

                            foreach (var item in allScannedItems)
                            {
                                model.OrderItemScanned.Remove(item);
                            }

                        }
                    }
                    else
                    {
                        // If verification - Pelase test later
                        foreach (var scannedItem in entity.ScannedItems)
                        {
                            var existingScannedItem = model.OrderItemScanned.FirstOrDefault(si => si.orderItemId == scannedItem.OrderItemId);
                            if (existingScannedItem != null)
                            {
                                existingScannedItem.verifyStatus = "Verified";
                                model.Entry(existingScannedItem).State = EntityState.Modified;
                            }
                        }
                    }



                    model.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                throw;
            }
            catch (Exception ex)
            {

                // Consider logging the full exception details before throwing
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return true;
        }

        public bool OrderScanned(OrderScanned entity)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var order = new OrderItemScanned
                    {
                        orderItemId = entity.orderItem_Id,
                        scannedQty = entity.scannedQty,
                        serialNo = entity.serialNo,
                        scannedBy = "System Admin",
                        scannedDate = DateTime.Now, // entity.orderDate,
                        status = "Matched"
                    };

                    model.OrderItemScanned.Add(order);
                    model.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return true;
        }



        public Order UpdateOrders(UpdateOrder entity, int userId)
        {
            Order ordermodel = new Order();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    int orderId = Convert.ToInt32(entity.id); // Convert the string ID to an integer

                    // Find the existing order by its primary key (e.g., orderNum)
                    var order = model.Order.FirstOrDefault(o => o.id == orderId);

                    if (order != null)
                    {
                        // Add this at the start of the method to handle removed items
                        if (entity.RemovedOrderItemIds != null && entity.RemovedOrderItemIds.Any())
                        {
                            // Retrieve the OrderItems to be removed
                            var removedItems = model.OrderItems
                                .Where(oi => entity.RemovedOrderItemIds.Contains(oi.id))
                                .ToList();

                            // Map each removed item to OrderItemDeleted
                            foreach (var removedItem in removedItems)
                            {
                                var deletedItem = new OrderItemDeleted
                                {
                                    orderId = removedItem.orderId,
                                    orderItemId = removedItem.id,
                                    orderLine = removedItem.orderLine,
                                    partNum = removedItem.partNum,
                                    product_Id = removedItem.product_Id,
                                    lineDesc = removedItem.lineDesc,
                                    ium = removedItem.ium,
                                    salesUm = removedItem.salesUm,
                                    unitPrice = removedItem.unitPrice,
                                    orderQty = removedItem.orderQty,
                                    scannedQty = removedItem.scannedQty,
                                    discount = removedItem.discount,
                                    listPrice = removedItem.listPrice,
                                    QtyType_ModuleItem_Id = removedItem.QtyType_ModuleItem_Id,
                                    OrderUOM_Id = removedItem.OrderUOM_Id,
                                    operationStyle_ModuleItem_Id = removedItem.operationStyle_ModuleItem_Id,
                                    operationCost = removedItem.operationCost,
                                    actualOperationCost = removedItem.actualOperationCost,
                                    complimentary_ModuleItem_Id = removedItem.complimentary_ModuleItem_Id,
                                    allowVaryWeight = removedItem.allowVaryWeight,
                                    originalUnitPrice = removedItem.originalUnitPrice,
                                    realOriginalUnitPrice = removedItem.realOriginalUnitPrice,
                                    Comments = removedItem.Comments,
                                    scannedLabel = removedItem.scannedLabel,
                                    finalQty = removedItem.finalQty,
                                    avlQty = removedItem.avlQty,
                                    IsItemReturned = removedItem.IsItemReturned,
                                    createdBy = removedItem.createdBy,
                                    createdAt = removedItem.createdAt,
                                    deletedBy = userId,
                                    deletedAt = DateTime.Now,
                                    discountAmt = removedItem.discountAmt,
                                    discountPer = removedItem.discountPer
                                };

                                // Add the deleted item to OrderItemDeleted table
                                model.OrderItemDeleted.Add(deletedItem);
                            }
                        }


                        // Update the existing order with new values
                        decimal totalOrderAmount = 0;
                        foreach (var item in entity.OrderItems)
                        {
                            if (string.IsNullOrEmpty(item.partNum))
                                continue;
                            // Determine the quantity to use (scanned or ordered)
                            decimal quantity = item.scannedQty == null || item.scannedQty == 0
                                ? item.orderQty
                                : item.scannedQty ?? 0;

                            // Calculate subtotal (unit price * quantity + operation cost)
                            decimal subtotal = (item.unitPrice * quantity) + item.operationCost;
                            subtotal = Math.Round(subtotal, 2, MidpointRounding.AwayFromZero);

                            // Calculate discount amount if discount percentage exists
                            decimal discountAmount = 0;
                            if (item.discountPer.HasValue && item.discountPer > 0)
                            {
                                discountAmount = Math.Round((subtotal * item.discountPer.Value) / 100, 2, MidpointRounding.AwayFromZero);
                            }

                            // Store the calculated discount amount
                            item.discountAmt = discountAmount;

                            // Calculate final item amount with discount
                            decimal itemAmount = subtotal - discountAmount;

                            // Add the rounded item amount to the total
                            totalOrderAmount += Math.Round(itemAmount, 2, MidpointRounding.AwayFromZero);

                            // For debugging
                            System.Diagnostics.Debug.WriteLine($"ItemId: {item.partNum}, Quantity: {quantity}, Subtotal: {subtotal}, Discount: {discountAmount}, Final Amount: {itemAmount}");
                        }

                        // Round the final total order amount
                        totalOrderAmount = Math.Round(totalOrderAmount, 2, MidpointRounding.AwayFromZero);
                        //  + i.operationCost as this is included in the unitprice already
                        order.customer_Id = entity.customer_Id;
                        order.shipToAddress_Id = entity.shipToAddress_Id == 0 ? (int?)null : entity.shipToAddress_Id;
                        order.districtId = entity.districtId == 0 ? (int?)null : entity.districtId;
                        order.orderComment = entity.orderComment;
                        order.docOrderAmt = totalOrderAmount;
                        //order.orderStatus = entity.orderStatus;
                        order.orderDate = DateTime.ParseExact(entity.orderDate.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", null);
                        order.oneTimeCustomer = entity.oneTimeCustomer;
                        order.orderContact = entity.orderContact;
                        order.orderContactName = entity.orderContactName;
                        order.orderContactPhone = entity.orderContactPhone;

                        //// Manually remove existing order items
                        //foreach (var item in order.OrderItems.ToList())
                        //{
                        //    model.OrderItems.Remove(item);
                        //}

                        //foreach (var itemDto in entity.OrderItems)
                        //{
                        //    if (string.IsNullOrEmpty(itemDto.partNum))
                        //        continue;
                        //    var scannedQty = string.IsNullOrEmpty(itemDto.scannedLabel) ? itemDto.scannedQty : itemDto.orderQty;
                        //    if (!itemDto.allowVaryWeight && !string.IsNullOrEmpty(itemDto.scannedLabel))
                        //    {
                        //        scannedQty = 1;
                        //    }
                        //    System.Diagnostics.Debug.WriteLine(itemDto.scannedQty);
                        //    var orderItem = new OrderItems
                        //    {
                        //        orderLine = entity.OrderItems.IndexOf(itemDto) + 1,
                        //        partNum = itemDto.partNum,
                        //        product_Id = itemDto.product_Id,
                        //        lineDesc = itemDto.lineDesc,
                        //        ium = itemDto.ium,
                        //        salesUm = itemDto.ium,
                        //        unitPrice = itemDto.unitPrice,
                        //        orderQty = itemDto.orderQty,
                        //        scannedQty = scannedQty,
                        //        discount = itemDto.discount,
                        //        listPrice = itemDto.listPrice,
                        //        QtyType_ModuleItem_Id = itemDto.QtyType_ModuleItem_Id != 0
                        //    ? itemDto.QtyType_ModuleItem_Id
                        //    : (int?)null,
                        //        OrderUOM_Id = itemDto.OrderUOM_Id != 0
                        //    ? itemDto.OrderUOM_Id
                        //    : (int?)null,
                        //        operationStyle_ModuleItem_Id = itemDto.operationStyle_ModuleItem_Id != 0
                        //    ? itemDto.operationStyle_ModuleItem_Id
                        //    : (int?)null,
                        //        operationCost = itemDto.operationCost,
                        //        actualOperationCost = itemDto.actualOperationCost,
                        //        complimentary_ModuleItem_Id = itemDto.complimentary_ModuleItem_Id != 0
                        //    ? itemDto.complimentary_ModuleItem_Id
                        //    : (int?)null,
                        //        allowVaryWeight = itemDto.allowVaryWeight,
                        //        originalUnitPrice = itemDto.originalUnitPrice,
                        //        realOriginalUnitPrice = itemDto.realOriginalUnitPrice,
                        //        orderItemId = itemDto.orderItemId == 0 ? (int?)null : itemDto.orderItemId,
                        //        Comments = itemDto.comments,
                        //        scannedLabel = itemDto.scannedLabel,
                        //        createdAt = itemDto.createdAt != null && itemDto.createdAt != DateTime.MinValue ? itemDto.createdAt : DateTime.Now,
                        //        createdBy = itemDto.createdBy != 0 ? itemDto.createdBy : userId,
                        //        discountPer = itemDto.discountPer,
                        //        discountAmt = itemDto.discountAmt
                        //    };

                        //    order.OrderItems.Add(orderItem);
                        //}

                        // Step 1: Build a dictionary for quick lookup from existing items
                        var existingItems = order.OrderItems.ToDictionary(x => x.id, x => x);
                        // Step 2: Track orderItemIds from incoming for deletion logic
                        var incomingItemIds = new HashSet<int>();
                        var deletedItemIds = new HashSet<int>();

                        // Step 3: Process incoming items (Add or Update)
                        foreach (var itemDto in entity.OrderItems)
                        {
                            if (string.IsNullOrEmpty(itemDto.partNum))
                                continue;

                            var scannedQty = string.IsNullOrEmpty(itemDto.scannedLabel) ? itemDto.scannedQty : itemDto.orderQty;
                            if (!itemDto.allowVaryWeight && !string.IsNullOrEmpty(itemDto.scannedLabel))
                            {
                                scannedQty = 1;
                            }

                            OrderItems orderItem;

                            // Determine if this is an update or new insert
                            if (itemDto.orderItemId != 0 && existingItems.TryGetValue(itemDto.orderItemId, out orderItem))
                            {
                                // Update existing item
                                incomingItemIds.Add(itemDto.orderItemId);

                                orderItem.partNum = itemDto.partNum;
                                orderItem.product_Id = itemDto.product_Id;
                                orderItem.lineDesc = itemDto.lineDesc;
                                orderItem.ium = itemDto.ium;
                                orderItem.salesUm = itemDto.ium;
                                orderItem.unitPrice = itemDto.unitPrice;
                                orderItem.orderQty = itemDto.orderQty;
                                orderItem.scannedQty = scannedQty;
                                orderItem.discount = itemDto.discount;
                                orderItem.listPrice = itemDto.listPrice;
                                orderItem.QtyType_ModuleItem_Id = itemDto.QtyType_ModuleItem_Id != 0 ? itemDto.QtyType_ModuleItem_Id : (int?)null;
                                orderItem.OrderUOM_Id = itemDto.OrderUOM_Id != 0 ? itemDto.OrderUOM_Id : (int?)null;
                                orderItem.operationStyle_ModuleItem_Id = itemDto.operationStyle_ModuleItem_Id != 0 ? itemDto.operationStyle_ModuleItem_Id : (int?)null;
                                orderItem.operationCost = itemDto.operationCost;
                                orderItem.actualOperationCost = itemDto.actualOperationCost;
                                orderItem.complimentary_ModuleItem_Id = itemDto.complimentary_ModuleItem_Id != 0 ? itemDto.complimentary_ModuleItem_Id : (int?)null;
                                orderItem.allowVaryWeight = itemDto.allowVaryWeight;
                                orderItem.originalUnitPrice = itemDto.originalUnitPrice;
                                orderItem.realOriginalUnitPrice = itemDto.realOriginalUnitPrice;
                                orderItem.Comments = itemDto.comments;
                                //orderItem.scannedLabel = itemDto.scannedLabel;
                                //orderItem.Location = itemDto.scannedLocation;
                                orderItem.discountPer = itemDto.discountPer;
                                orderItem.discountAmt = itemDto.discountAmt;
                                // You can also track updatedAt/user info if needed
                                //model.OrderItems.AddOrUpdate(orderItem);
                            }
                            else
                            {
                                // New item
                                var newItem = new OrderItems
                                {
                                    orderLine = entity.OrderItems.IndexOf(itemDto) + 1,
                                    partNum = itemDto.partNum,
                                    product_Id = itemDto.product_Id,
                                    lineDesc = itemDto.lineDesc,
                                    ium = itemDto.ium,
                                    salesUm = itemDto.ium,
                                    unitPrice = itemDto.unitPrice,
                                    orderQty = itemDto.orderQty,
                                    scannedQty = scannedQty,
                                    discount = itemDto.discount,
                                    listPrice = itemDto.listPrice,
                                    QtyType_ModuleItem_Id = itemDto.QtyType_ModuleItem_Id != 0 ? itemDto.QtyType_ModuleItem_Id : (int?)null,
                                    OrderUOM_Id = itemDto.OrderUOM_Id != 0 ? itemDto.OrderUOM_Id : (int?)null,
                                    operationStyle_ModuleItem_Id = itemDto.operationStyle_ModuleItem_Id != 0 ? itemDto.operationStyle_ModuleItem_Id : (int?)null,
                                    operationCost = itemDto.operationCost,
                                    actualOperationCost = itemDto.actualOperationCost,
                                    complimentary_ModuleItem_Id = itemDto.complimentary_ModuleItem_Id != 0 ? itemDto.complimentary_ModuleItem_Id : (int?)null,
                                    allowVaryWeight = itemDto.allowVaryWeight,
                                    originalUnitPrice = itemDto.originalUnitPrice,
                                    realOriginalUnitPrice = itemDto.realOriginalUnitPrice,
                                    orderItemId = null, // will be generated by DB
                                    Comments = itemDto.comments,
                                    scannedLabel = string.IsNullOrWhiteSpace(itemDto.scannedLabel)? string.Empty :itemDto.scannedLabel.Trim(),
                                    Location = itemDto.scannedLocation,
                                    createdAt = itemDto.createdAt != null && itemDto.createdAt != DateTime.MinValue ? itemDto.createdAt : DateTime.Now,
                                    createdBy = itemDto.createdBy != 0 ? itemDto.createdBy : userId,
                                    discountPer = itemDto.discountPer,
                                    discountAmt = itemDto.discountAmt
                                };

                                order.OrderItems.Add(newItem);
                            }
                        }
                        // Step 4: Remove deleted items
                        var deletedItems = model.OrderItems
                            //.Where(x => x.orderItemId != null && !incomingItemIds.Contains(x.orderItemId.Value))
                            .Where(x => x.orderId == order.id && !incomingItemIds.Contains(x.id))
                            .ToList();

                        foreach (var deleted in deletedItems)
                        {
                            model.OrderItems.Remove(deleted);
                            var deletedItemsScanRecords = model.OrderItemScanned.Where(x => x.orderId==order.id && x.orderItemId ==  deleted.id)
                            .ToList();
                            foreach (var deletedScan in deletedItemsScanRecords)
                            {
                                model.OrderItemScanned.Remove(deletedScan);

                                // Update back IsPickCompleted=false the ProductCarton records if they are related to this order item
                                
                                    
                                    var itemBarcodes = model.ProductCarton.Where(p => p.epicorPartNo == deleted.partNum
                                                && p.barcode == deletedScan.serialNo
                                                && p.PickedOrderId == deletedScan.orderId && p.PickedOrderItemId == deletedScan.orderItemId)
                                        .ToList();
                                    if (itemBarcodes != null)
                                    {
                                        foreach (var barcode in itemBarcodes)
                                        {
                                            
                                            barcode.IsPickedComplete = false;
                                            barcode.PickedCompletedDate = null;
                                            barcode.PickedOrderId = 0;
                                            barcode.PickedOrderItemId = 0;
                                            barcode.PickedLastModifiedDate   = DateTime.Now;
                                            barcode.IsPickStatusToSync = true; // Mark for sync
                                            model.Entry(barcode).State = EntityState.Modified;
                                            
                                        }
                                        
                                    }
                                
                            }
                        }
                        
                        // Log the update action
                        order.OrderUpdate.Add(new OrderUpdate
                        {
                            actionDate = DateTime.Now,
                            actionName = "Updated",
                            user_Id = userId,
                            orderStatus="Updated"
                        });
                        // Save changes to the database
                        model.SaveChanges();
                        // Log the update action
                        //order.OrderApproval.Add(new OrderApproval
                        //{
                        //    order_Id = Convert.ToInt32(entity.id),
                        //    submittedUser_Id = userId,
                        //    submittedDate = DateTime.Now,
                        //    approvalStatus= "Awaiting Approval"

                        //});
                        //// Save changes to the database
                        //model.SaveChanges();
                        var currentUser = model.Users.Where(p => p.id == userId).FirstOrDefault();
                        // Store the scanned label to the database.
                        // new orderitems related any orderitemscannd will inserted to scantable
                        var savedOrderItems = model.OrderItems
                            .Where(oi => oi.orderId == order.id && !incomingItemIds.Contains(oi.id))
                            .ToList();
                        //// Get the all scanlabels from the order items
                        //var _scanLabels = savedOrderItems.Where(w=>!string.IsNullOrEmpty(w.scannedLabel)).Select(oi => oi.scannedLabel).ToList();
                        
                        //List<ProductCarton> productCartonBarcodes = new List<ProductCarton>();
                        //if (_scanLabels != null && _scanLabels.Count > 0)
                        //{
                        //    productCartonBarcodes = model.ProductCarton
                        //        .Where(p => p.IsCarton==false && p.OnHold == false && p.IsPartialCarton==false && _scanLabels.Contains(p.barcode) )
                        //        .ToList();
                        //}
                        // Iterate over the retrieved order items
                        foreach (var savedOrderItem in savedOrderItems)
                        {

                            if (!string.IsNullOrWhiteSpace(savedOrderItem.scannedLabel))
                            {
                                //var looseBarcode = productCartonBarcodes.Where(a => a.barcode == savedOrderItem.scannedLabel).FirstOrDefault();
                                if (savedOrderItem.allowVaryWeight == true)
                                {
                                    var newScannedItem = new OrderItemScanned
                                    {
                                        orderItemId = savedOrderItem.orderItemId ?? savedOrderItem.id,
                                        serialNo = savedOrderItem.scannedLabel,
                                        scannedQty = savedOrderItem.orderQty, // Define specific scanned quantity if available
                                        scannedDate = DateTime.Now,
                                        scannedBy = currentUser.userName,
                                        status = "Matched", // Replace with savedOrderItem.Status if needed
                                        orderId = savedOrderItem.orderId,
                                        Location = savedOrderItem.Location,
                                    };
                                    model.OrderItemScanned.Add(newScannedItem);

                                    if (savedOrderItem.allowVaryWeight == true)
                                    {
                                        // Carton Vwg Or Loose Vwg is unique SerialNumber, update as Pickupcomplete = true
                                        var vwgBarcode = model.ProductCarton.Where(p => p.epicorPartNo == savedOrderItem.partNum
                                                    && p.barcode == savedOrderItem.scannedLabel
                                                    && p.IsPickedComplete == false && p.OnHold == false)
                                            .FirstOrDefault();
                                        if (vwgBarcode != null)
                                        {
                                            vwgBarcode.IsPickedComplete = true;
                                            vwgBarcode.PickedCompletedDate = DateTime.Now;
                                            vwgBarcode.PickedOrderId = savedOrderItem.orderId;
                                            vwgBarcode.PickedOrderItemId = savedOrderItem.id;
                                            vwgBarcode.PickedLastModifiedDate = DateTime.Now;
                                            vwgBarcode.IsPickStatusToSync = true; // Mark for sync
                                            model.Entry(vwgBarcode).State = EntityState.Modified;
                                        }
                                    }
                                    else
                                    {
                                        // Std Carton is not unique, update as Pickupcomplete = true for top item
                                        var stdCartonBarcode = model.ProductCarton.Where(p => p.epicorPartNo == savedOrderItem.partNum
                                                            && p.barcode == savedOrderItem.scannedLabel
                                                    && p.IsCarton == true && p.IsPickedComplete == false && p.OnHold == false)
                                            .FirstOrDefault();
                                        if (stdCartonBarcode != null)
                                        {
                                            stdCartonBarcode.IsPickedComplete = true;
                                            stdCartonBarcode.PickedCompletedDate = DateTime.Now;
                                            stdCartonBarcode.PickedOrderId = savedOrderItem.orderId;
                                            stdCartonBarcode.PickedOrderItemId = savedOrderItem.id;
                                            stdCartonBarcode.PickedLastModifiedDate = DateTime.Now;
                                            stdCartonBarcode.IsPickStatusToSync = true; // Mark for sync
                                            model.Entry(stdCartonBarcode).State = EntityState.Modified;
                                        }
                                    }
                                }
                                else
                                {
                                    int orderQty = 1; //  (int)Math.Floor(savedOrderItem.orderQty); // Ensure orderQty is not null

                                    for (int i = 0; i < orderQty; i++)
                                    {
                                        var newScannedItem = new OrderItemScanned
                                        {
                                            orderItemId = savedOrderItem.orderItemId ?? savedOrderItem.id,
                                            serialNo = savedOrderItem.scannedLabel, // You can modify this if serial numbers vary
                                            scannedQty = 1, // Always set to 1
                                            scannedDate = DateTime.Now,
                                            scannedBy = currentUser.userName,
                                            status = "Matched", // Replace with savedOrderItem.Status if needed
                                            orderId = savedOrderItem.orderId,
                                            Location = savedOrderItem.Location,
                                        };

                                        model.OrderItemScanned.Add(newScannedItem);
                                    }
                                }

                                savedOrderItem.orderItemId = savedOrderItem.orderItemId ?? savedOrderItem.id;
                                model.Entry(savedOrderItem).State = EntityState.Modified;
                            }

                            if (savedOrderItem.discountAmt > 0)
                            {
                                var orderdiscountapproval = model.OrderItemDiscountApproval.Where(p => (p.orderItem_Id == 0|| p.orderItem_Id == null) && p.discount==savedOrderItem.discountAmt).FirstOrDefault();
                                if (orderdiscountapproval != null)
                                {

                                    orderdiscountapproval.orderItem_Id = savedOrderItem.orderItemId ?? savedOrderItem.id;
                                        model.Entry(orderdiscountapproval).State = EntityState.Modified;
                                        model.SaveChanges();
                                    
                                }
                            }
                                var orderupdateapproval = model.OrderUpdate.Where(p => p.order_Id == null).ToList();
                            if (orderupdateapproval != null)
                            {
                                foreach (var item in orderupdateapproval)
                                {
                                    item.order_Id = order.id;
                                    model.Entry(item).State = EntityState.Modified;
                                    model.SaveChanges();
                                }
                            }
                            

                        }

                        // Save any additional changes to OrderItemScanned
                        model.SaveChanges();
                        ordermodel = order;
                    }
                    else
                    {
                        // Handle the case where the order does not exist (optional)
                        throw new Exception("Order not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return ordermodel;
        }

        public List<OrderScanned> getOrderScannedItems(int orderId)
        {
            try
            {

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var scannedItems = model.OrderItemScanned
                        .Join(model.OrderItems,
                              ois => ois.orderItemId,           // The key from OrderItemScanned
                              oi => oi.orderItemId,             // The key from OrderItems
                              (ois, oi) => new { Scanned = ois, OrderItem = oi })  // The result of the join
                        .Where(joined => joined.OrderItem.orderId == orderId)
                        .Select(item => new OrderScanned
                        {

                            serialNo = item.Scanned.serialNo,
                            orderItem_Id = (int)item.Scanned.orderItemId,
                            scannedQty = item.Scanned.scannedQty,
                            status = item.Scanned.status,

                            // Ensure you're accessing the Product entity via OrderItems navigation property
                            Group = (item.OrderItem != null && item.OrderItem.Product != null) ? item.OrderItem.Product.ProdGrup_Description : "",
                            partNo = (item.OrderItem != null) ? item.OrderItem.partNum : "",
                            partName = (item.OrderItem != null) ? item.OrderItem.lineDesc : "",
                            orderUOM = (item.OrderItem != null && item.OrderItem.OrderUOM != null) ? item.OrderItem.OrderUOM.code : "",
                             productLocation = item.Scanned.Location,
                            LocationOptions = model.StockBalance
        .Where(sb => sb.partNum == item.OrderItem.partNum)
        .Select(sb => sb.Location)
        .Distinct()
        .ToList()



                        }).ToList();

                    return scannedItems;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }

        public ViewOrder GetOrderDetails(int id)
        {
            try
            {
                var _result = new ViewOrder();
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.Order.AsQueryable();
                    var _entity = query.SingleOrDefault(p => p.id == id);
                    var credithold = model.Customer.SingleOrDefault(u => u.id == _entity.customer_Id);


                    // Fetch and enrich order items
                    var orderItemsInit = model.OrderItems
                        .Where(x => x.orderId == id && x.partNum != null && x.ium != null)
                        .Include(x => x.Product) // Include Product to access itemBrandCode
                        .ToList();

                    var brands = orderItemsInit
                            .Where(x => x.Product != null && !string.IsNullOrEmpty(x.Product.itemBrandCode))
                        .Select(p => p.Product.itemBrandCode).Distinct().ToList();
                    // Cache Brand list into a dictionary for fast lookups
                    var brandDict = model.Brand
                        .Where(b => brands.Contains(b.brandCode))
                        .ToDictionary(b => b.brandCode, b => b);

                    var orderItemsRaw = orderItemsInit
                        .Select(x => new
                        {
                            Item = x,
                            Product = x.Product,
                            Brand = (x.Product != null && !string.IsNullOrEmpty(x.Product.itemBrandCode) && brandDict.ContainsKey(x.Product.itemBrandCode))
                                ? brandDict[x.Product.itemBrandCode]
                                : null,
                            OrderUOM = x.OrderUOM,
                            QtyType = x.ModuleItem2,
                            Complimentary = x.ModuleItem,
                            Operation = x.ModuleItem1,
                            CreatedByUser = x.User
                        })
                        .ToList();

                   
                    // Extract partNum and ium into separate lists.  This is crucial.
                    var partNums = orderItemsInit.Select(p => p.partNum).ToList();
                    var uoms = orderItemsInit.Select(p => p.ium).ToList();

                    var stockList = model.StockBalanceView
                        .Where(stock => partNums.Contains(stock.partNum) && uoms.Contains(stock.uom))
                        .ToList();


                    var scannedItems = model.OrderItemScanned.Where(w=>w.orderId == id).ToList();

                    var orderItems = orderItemsRaw.Select(x =>
                    {
                        var stock = stockList.FirstOrDefault(s => s.partNum == x.Item.partNum && s.uom == x.Item.ium);
                        return new ViewOrderItem
                        {
                            orderLine = x.Item.orderLine,
                            partNum = x.Item.partNum,
                            partName = x.Product.description,
                            country = x.Product.Country_Description ?? "",
                            product_Id = (int)x.Item.product_Id,
                            itemId = (int)x.Item.id,
                            lineDesc = x.Item.lineDesc,
                            prodGroup = x.Product.ProdGrup_Description,
                            ium = x.Item.ium,
                            comments = x.Item.Comments ?? "",
                            unitPrice = x.Item.unitPrice ?? 0,
                            orderUOM = x.OrderUOM?.code ?? "",
                            AllowSellingVaryWeight = (x.Product?.AllowSellingVaryWeight ?? false) ? "Yes" : "No",
                            orderType = x.QtyType?.description ?? "",
                            complementary = x.Complimentary?.description ?? "",
                            operationName = x.Operation?.description ?? "",
                            scannedQty = x.Item.scannedQty ?? 0,
                            discountPer = x.Item.discountPer ?? 0,
                            discountAmt = x.Item.discountAmt ?? 0,
                            orderQty = x.Item.orderQty,
                            orderId = x.Item.id,
                            allowVaryWeight = x.Item.allowVaryWeight,
                            salesUm = x.Item.salesUm,
                            operationCost = x.Item.operationCost ?? 0,
                            actualOperationCost = x.Item.actualOperationCost ?? 0,
                            conversionFactor = x.Product.conversionFactor ?? 1,
                            QtyType_ModuleItem_Id = x.Item.QtyType_ModuleItem_Id,
                            OrderUOM_Id = x.Item.OrderUOM_Id,
                            operationStyle_ModuleItem_Id = x.Item.operationStyle_ModuleItem_Id,
                            complimentary_ModuleItem_Id = x.Item.complimentary_ModuleItem_Id,
                            originalUnitPrice = x.Item.originalUnitPrice ?? 0,
                            realOriginalUnitPrice = x.Item.realOriginalUnitPrice ?? 0,
                            orderItemId = x.Item.orderItemId ?? x.Item.id,
                            IsReturned = x.Item.IsItemReturned ?? false,
                            scannedQtyStr = string.Join(",", scannedItems.Where(a => a.orderItemId == x.Item.id).Select(a => a.scannedQty)),
                            avlQty = stock?.remainingQty ?? 0,
                            listPrice = x.Item.listPrice ?? 0,
                            returnTotal = x.Item.returnTotal ?? 0,
                            scannedLabel = x.Item.scannedLabel,
                            scannedLocation = x.Item.Location,
                            createdAt = x.Item.createdAt,
                            createdBy = x.CreatedByUser?.userName ?? "",
                            createdById = x.Item.createdBy,
                            itemBrandName = x.Brand?.brandDescription ?? ""
                        };
                    }).ToList();

                    var deletedItems = _entity.OrderItemDeleted.Select(x => new ViewOrderItemDeleted
                    {
                        partNum = x.partNum,
                        partName = x.Product.description,
                        country = x.Product.Country_Description ?? "",
                        product_Id = (int)x.product_Id,
                        itemId = (int)x.id,
                        lineDesc = x.lineDesc,
                        prodGroup = x.Product.ProdGrup_Description,
                        ium = x.ium,
                        comments = x.Comments ?? "",
                        unitPrice = x.unitPrice ?? 0,
                        orderUOM = x.OrderUOM?.code ?? "",
                        AllowSellingVaryWeight = (x.Product?.AllowSellingVaryWeight ?? false) ? "Yes" : "No",
                        orderType = x.ModuleItem2?.description ?? "",
                        complementary = x.ModuleItem?.description ?? "",
                        operationName = x.ModuleItem1?.description ?? "",
                        scannedQty = x.scannedQty ?? 0,
                        discountPer = x.discountPer ?? 0,
                        discountAmt = x.discountAmt ?? 0,
                        orderQty = x.orderQty,
                        orderId = x.id,
                        allowVaryWeight = x.allowVaryWeight,
                        salesUm = x.salesUm,
                        operationCost = x.operationCost ?? 0,
                        actualOperationCost = x.actualOperationCost ?? 0,
                        conversionFactor = x.Product.conversionFactor ?? 1,
                        QtyType_ModuleItem_Id = x.QtyType_ModuleItem_Id,
                        OrderUOM_Id = x.OrderUOM_Id,
                        operationStyle_ModuleItem_Id = x.operationStyle_ModuleItem_Id,
                        complimentary_ModuleItem_Id = x.complimentary_ModuleItem_Id,
                        originalUnitPrice = x.originalUnitPrice ?? 0,
                        realOriginalUnitPrice = x.realOriginalUnitPrice ?? 0,
                        orderItemId = x.orderItemId ?? x.id,
                        IsReturned = x.IsItemReturned ?? false,
                        scannedQtyStr = string.Join(",", scannedItems.Where(a => a.orderItemId == x.id).Select(a => a.scannedQty)),
                        scannedLabel = x.scannedLabel,
                        deletedAt = x.deletedAt,
                        deletedBy = x.User?.userName ?? "",
                        itemBrandName = string.IsNullOrEmpty(x.Product.itemBrandCode) ? "" :
                                        model.Brand.FirstOrDefault(a => a.brandCode == x.Product.itemBrandCode)?.brandDescription ?? ""
                    }).ToList();

                    var orderScanned = scannedItems
                        .Join(model.OrderItems, s => s.orderItemId, o => o.orderItemId, (s, o) => new { s, o })
                        .Where(j => j.o.orderId == id)
                        .Select(x => new OrderScanned
                        {
                            Id = x.s.id,
                            serialNo = x.s.serialNo,
                            orderItem_Id = x.s.orderItemId ?? 0,
                            orderId = x.s.orderId ?? 0,
                            scannedQty = x.s.scannedQty,
                            status = x.s.status,
                            verifyStatus = x.s.verifyStatus,
                            Group = x.o.Product?.ProdGrup_Description ?? "",
                            partNo = x.o.partNum,
                            partName = x.o.lineDesc,
                            orderUOM = x.o.salesUm,
                            IsReturned = x.s.IsItemReturned ?? false,
                            allowVaryWeight = x.o.allowVaryWeight ?? false,
                            returnQty = x.s.returnQty ?? 0,
                            productLocation = x.s.Location,
                        }).ToList();

                    var orderPayments = _entity.OrderPayment.Select(item => new OrderPayments
                    {
                        Id = item.id,
                        Bank = item.Bank,
                        RefNumber = item.refNumber,
                        PaymentDate = item.paymentDate,
                        PaymentStatus = item.paymentStatus,
                        PaymentType = item.paymentType,
                        UserId = item.user_Id,
                        Amount = item.amount,
                        IsRefund = item.IsRefund ?? false
                    }).ToList();

                    _result = new ViewOrder
                    {
                        id = _entity.id.ToString(),
                        customer_Id = _entity.customer_Id,
                        orderNum = _entity.orderNum,
                        orderComment = _entity.orderComment,
                        orderDate = _entity.orderDate,
                        entryPerson = _entity.entryPerson,
                        customerName = _entity.Customer.name,
                        orderStatus = _entity.orderStatus,
                        docOrderAmt = _entity.docOrderAmt.ToString(),
                        verifiedBy = _entity.User != null ? _entity.User.firstName + " " + _entity.User.lastName : "",
                        verifiedDate = _entity.verifiedDate,
                        verifiedStatus = _entity.verifiedStatus,
                        verifyRemarks = _entity.verifyRemarks,
                        shipToAddress_Id = _entity.shipToAddress_Id,
                        shipToName = _entity.ShipToAddress?.shippingCode,
                        districtId = _entity.districtId,
                        oneTimeCustomer = _entity.oneTimeCustomer ?? false,
                        orderContact = _entity.orderContact,
                        orderContactName = _entity.orderContactName,
                        orderContactPhone = _entity.orderContactPhone,
                        customerAddress = "",
                        shipToAddress = "",
                        tinId = "",
                        epicoreInvNumber = _entity.epicoreInvNumber,
                        epicoreResponse = _entity.epicoreResponse,
                        epicoreOrderId = _entity.epicoreOrderId,
                        shipPackNumber = _entity.shipPackNumber,
                        UD16Key1 = _entity.UD16Key1,
                        syncedAt = _entity.syncedAt,
                        syncStatus = _entity.syncStatus ?? false,
                        OrderItems = orderItems,
                        OrderItemDeleted = deletedItems,
                        OrderScanned = orderScanned,
                        OrderPayments = orderPayments,
                        creditHold = credithold?.creditHold ?? false
                    };

                    var _customer = model.Customer.Where(x => x.id == _result.customer_Id).FirstOrDefault();
                    if (_customer != null)
                    {
                        _result.tinId = _customer.tin ?? "";
                        //_result.customerAddress = ""; ""
                    }

                    var _shipTo = model.ShipToAddress.Where(x => x.id == _result.shipToAddress_Id).FirstOrDefault();
                    if (_shipTo != null)
                    {
                        _result.shipToAddress = (_shipTo.address1 ?? "") + Environment.NewLine +
                            _shipTo.city ?? "";
                    }

                }
                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public OrderApiDto GetOrderApiData(int id)
        {
            try
            {
                var _result = new OrderApiDto();
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.Order.AsQueryable();
                    var _entity = query.SingleOrDefault(p => p.id == id);
                    var queryOrderScanned = model.OrderItemScanned
                        .Where(p => p.orderId == id)
                        .ToList();

                    if (_entity != null)
                    {
                        bool parseSuccess = int.TryParse(_entity.Customer.code, out int custId);

                        // Payments and refunds
                        //var payments = _entity.OrderPayment.Where(p => p.IsRefund != true).OrderByDescending(p => p.amount).ToList();
                        var refunds = _entity.OrderPayment.Where(p => p.IsRefund == true).OrderByDescending(p => p.amount).ToList();
                        bool hasDiscountRefund = _entity.OrderPayment
                            .Any(p => p.IsRefund == true && p.paymentType == "Discount");
                        var payments = _entity.OrderPayment
                            .Where(p => p.IsRefund != true && (!hasDiscountRefund || p.paymentType != "Discount"))
                            .OrderByDescending(p => p.amount)
                            .ToList();

                        decimal totalRefundAmount = refunds.Sum(r => r.amount);
                        //// Reduce refund amount from payments
                        foreach (var refund in refunds)
                        {
                            decimal refundBalance = refund.amount;

                            foreach (var payment in payments)
                            {
                                if (payment.paymentType == "Cash" && _entity.Customer.custID == "CASH/WALK" && refund.paymentType == "Cash")
                                {
                                    if (refundBalance <= 0) break;

                                    if (payment.amount > refundBalance)
                                    {
                                        payment.amount -= refundBalance;
                                        refundBalance = 0;
                                    }
                                    else
                                    {
                                        refundBalance -= payment.amount;
                                        payment.amount = 0;
                                    }
                                }
                            }
                        }
                        // Exclude payments with zero amount
                        payments = payments.Where(p => p.amount > 0).ToList();

                        _result = new OrderApiDto
                        {
                            PosOrderNumber = _entity.orderNum,  // Assuming this is the POS order number
                            OrderDate = _entity.orderDate,
                            Plant = _entity.company,  // Hardcoded as per your original code
                            CustNum = custId,
                            ShipToNum = _entity.ShipToAddress != null ? _entity.ShipToAddress.shippingCode : "",  // ShipToAddress related info
                            districtID = _entity.District1 != null ? _entity.District1.districtID : "",
                            Remarks = _entity.orderComment,
                            OneTimeCustomer = _entity.oneTimeCustomer ?? false,
                            contact = _entity.orderContact,
                            name = _entity.orderContactName,
                            phone = _entity.orderContactPhone,
                            OrderDtl = _entity.OrderItems
                                .Select((item, index) =>
                                {
                                    // Calculate RetQty
                                    decimal retQty = Math.Round(queryOrderScanned?
                                        .Where(p => p.IsItemReturned == true && p.orderItemId == item.orderItemId)
                                        .Sum(p => p.returnQty) ?? 0, 2, MidpointRounding.AwayFromZero);

                                    // Calculate effective Quantity for price (Quantity - retQty)
                                    decimal effectiveQuantity = (item.scannedQty.HasValue && item.scannedQty > 0)
                                        ? item.scannedQty.Value - retQty
                                        : item.orderQty - retQty;

                                    // Skip this item if retQty == Quantity
                                    if (effectiveQuantity == 0)
                                        return null;  // Skip this item by returning null

                                    return new OrderDetailApiDto
                                    {
                                        LineNo = index + 1,  // Assuming line number is based on the index in the list
                                        PartNum = item.partNum,
                                        Quantity = effectiveQuantity,  // Adjusted Quantity
                                        Uom = item.salesUm,  // UOM (Unit of Measure)
                                        UnitPrice = item.unitPrice,  // Adjusted price for the effective quantity
                                        IsLoose = item.QtyType_ModuleItem_Id != null && item.ModuleItem?.description == "Loose",  // Check if the item is loose
                                        IsFOC = item.complimentary_ModuleItem_Id != null && item.ModuleItem?.description == "FOC",  // Free of charge check
                                        IsSample = item.complimentary_ModuleItem_Id != null && item.ModuleItem?.description == "Sample",  // Sample check
                                        IsExchange = false, // item.isExchange,
                                        IsVaryWeight = item.allowVaryWeight,  // Allow varying weight
                                        IsStdFullQty = item.QtyType_ModuleItem_Id != null && item.ModuleItem?.description == "Full",  // Standard full quantity check
                                        IsSlice = item.operationStyle_ModuleItem_Id != null && item.ModuleItem1?.description == "Slice",  // Check if it's sliced
                                        IsCube = item.operationStyle_ModuleItem_Id != null && item.ModuleItem1?.description == "Cube",  // Check if it's cubed
                                        IsStrip = item.operationStyle_ModuleItem_Id != null && item.ModuleItem1?.description == "Strip",  // Check if it's cubed
                                        SliceOrCubeCharges = item.operationCost,  // Charges for slicing or cubing
                                        Remarks = item.Comments ?? "",  // Item-specific remarks
                                        discountPercent = item.discountPer ?? 0,
                                        ScannedQtyList = queryOrderScanned
                                            .Where(p => p.orderItemId == item.orderItemId && p.IsItemReturned != true)
                                            .Select(p => new ScannedQtyModel
                                            {
                                                ScannedQty = p.scannedQty,  // Assuming scannedQty can be null, default to 0
                                                Barcode = p.serialNo
                                            })
                                            .ToList()  // Add scannedQtyList here
                                    };
                                })
                                .Where(item => item != null)  // Remove null items (those where retQty == Quantity)
                                .ToList(),
                            paymentDtl = payments.Select(item => new PaymentDetail
                            {
                                paymentType = item.paymentType,
                                paymentDate = item.paymentDate,
                                totalAmt = _entity.docOrderAmt ?? item.amount,
                                amount = item.amount,
                                chequeDate = item.paymentDate,
                                bank = item.Bank,
                                chequeNo = item.refNumber,
                                reference = item.refNumber
                            }).ToList(),
                        };

                        // Add refunds as "Advance Cash"
                        if (totalRefundAmount > 0)
                        {
                            foreach (var refund in refunds)
                            {
                                if (_entity.Customer.custID == "CASH/WALK" && refund.paymentType == "Cash")
                                {
                                    // Exclude - reserve for any other logics
                                } else
                                {
                                    _result.paymentDtl.Add(new PaymentDetail
                                    {
                                        paymentType = GetRefundPaymentType(refund.paymentType),
                                        paymentDate = refund.paymentDate,  // Current date
                                        totalAmt = _entity.docOrderAmt ?? refund.amount,
                                        amount = refund.amount,
                                        chequeDate = refund.paymentDate,
                                        bank = refund.Bank,
                                        chequeNo = refund.refNumber,
                                        reference = refund.refNumber
                                    });
                                }
                                    
                            }
                        }

                    }
                }
                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }

        private string GetRefundPaymentType(string paymentType)
        {
            switch (paymentType)
            {
                case "Cash":
                    return "RefundAsCash";
                case "Cheque":
                    return "RefundAsCheque";
                case "Discount":
                    return "Discount";
                case "Online":
                    return "RefundAsOnline";
                default:
                    return "Advance Cash";
            }
        }

        public bool UpdateOrderSyncStatus(OrderSyncStatusDto data)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var order = model.Order.SingleOrDefault(o => o.id == data.OrderId);
                    if (order != null)
                    {
                        order.syncStatus = data.SyncStatus;
                        order.syncedAt = DateTime.Now;
                        order.epicoreResponse = data.SyncMessage;
                        order.epicoreOrderId = data.EpicoreOrderId;
                        order.shipPackNumber = data.ShipPackNumber;
                        order.UD16Key1 = data.UD16Key1;
                        order.epicoreInvNumber = data.EpicoreInvNumber;
                        //var orderUpdate = new OrderUpdate
                        //{
                        //    order_Id = orderId,
                        //    actionDate = DateTime.Now,
                        //    actionName = syncStatus ? "SyncSuccess" : "SyncFailed"
                        //};
                        //model.OrderUpdate.Add(orderUpdate);
                        model.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
            return false;
        }

        public bool UpdateOrderStatus(int orderId, string newStatus, int userId, string actionName, string remark)
        {
            if (remark == null)
            {
                remark = "";
            }
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var order = model.Order.SingleOrDefault(o => o.id == orderId);
                    if (order != null)
                    {
                        if (newStatus == "VerificationPass")
                        {
                            // Update order status
                            order.verifiedStatus = "Pass";
                            order.verifiedBy = userId;
                            order.verifiedDate = DateTime.Now;
                            order.verifyRemarks = remark;

                            // Create OrderUpdate
                            var orderUpdate = new OrderUpdate
                            {
                                order_Id = orderId,
                                actionDate = DateTime.Now,
                                actionName = actionName,
                                user_Id = userId,
                            };

                            // Add OrderUpdate to the context
                            model.OrderUpdate.Add(orderUpdate);

                            // Save changes
                            model.SaveChanges();
                        } else if (newStatus == "EditPayment")
                        {
                            order.orderStatus = "Payment";
                            // Create OrderUpdate
                            var orderUpdate = new OrderUpdate
                            {
                                order_Id = orderId,
                                actionDate = DateTime.Now,
                                actionName = "Editing order",                                
                                user_Id = userId,
                            };
                            // Add OrderUpdate to the context
                            model.OrderUpdate.Add(orderUpdate);
                            // Update the order items here 
                            ResetReturnQtyAndRefunds(orderId);
                            model.SaveChanges();
                        }
                        else
                        {
                            // Update order status
                            order.orderStatus = (newStatus == "SubmitForApproval") ? "AwaitingApproval" : newStatus;

                            // Create OrderUpdate
                            var orderUpdate = new OrderUpdate
                            {
                                order_Id = orderId,
                                actionDate = DateTime.Now,
                                actionName = actionName,
                                user_Id = userId,
                                orderStatus = newStatus
                            };

                            if (newStatus == "Cancelled")
                            {
                                order.cancelRemarks = remark;
                                order.orderStatus = newStatus;
                                orderUpdate.actionName = "Cancelled";

                                // Update back IsPickCompleted=false the ProductCarton records if they are related to this order item

                                var cancelOrderItems = model.OrderItems.Where(w => w.orderId == order.id).ToList();
                                if (cancelOrderItems != null)
                                {
                                    foreach(var item in cancelOrderItems)
                                    {
                                        var itemBarcodes = model.ProductCarton.Where(p => p.epicorPartNo == item.partNum
                                            && p.PickedOrderId == order.id && p.PickedOrderItemId == item.orderItemId).ToList();
                                        if (itemBarcodes != null)
                                        {
                                            foreach (var barcode in itemBarcodes)
                                            {
                                                
                                                    barcode.IsPickedComplete = false;
                                                    barcode.PickedCompletedDate = null;
                                                    barcode.PickedOrderId = 0;
                                                    barcode.PickedOrderItemId = 0;
                                                    barcode.PickedLastModifiedDate = DateTime.Now;
                                                    barcode.IsPickStatusToSync = true; // Mark for sync
                                                model.Entry(barcode).State = EntityState.Modified;
                                                
                                            }
                                        }
                                    }
                                }
                                
                            }

                            // Add OrderUpdate to the context
                            model.OrderUpdate.Add(orderUpdate);

                            // Save changes
                            model.SaveChanges();
                        }
                        return true;
                    }
                    return false;
                }
            }

            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }


        private bool ResetReturnQtyAndRefunds(int orderId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    Order order = model.Order.SingleOrDefault(o => o.id == orderId);
                    decimal totalOrderAmount = 0;
                    foreach (var item in order.OrderItems)
                    {
                        // Determine the quantity to use (scanned or ordered)
                        decimal quantity = item.scannedQty == null || item.scannedQty == 0
                            ? item.orderQty
                            : item.scannedQty ?? 0;

                        // Calculate subtotal (unit price * quantity + operation cost)
                        decimal subtotal = ((item.unitPrice ?? 0) * quantity) + (item.operationCost ?? 0);
                        subtotal = Math.Round(subtotal, 2, MidpointRounding.AwayFromZero);

                        // Calculate discount amount if discount percentage exists
                        decimal discountAmount = 0;
                        if (item.discountPer.HasValue && item.discountPer > 0)
                        {
                            discountAmount = Math.Round((subtotal * item.discountPer.Value) / 100, 2, MidpointRounding.AwayFromZero);
                        }

                        // Store the calculated discount amount
                        item.discountAmt = discountAmount;

                        // Calculate final item amount with discount
                        decimal itemAmount = subtotal - discountAmount;
                        item.returnTotal = 0;
                        item.listPrice = itemAmount;

                        model.Entry(item).State = EntityState.Modified;

                        // Add the rounded item amount to the total
                        totalOrderAmount += Math.Round(itemAmount, 2, MidpointRounding.AwayFromZero);

                        // For debugging
                        System.Diagnostics.Debug.WriteLine($"ItemId: {item.partNum}, Quantity: {quantity}, Subtotal: {subtotal}, Discount: {discountAmount}, Final Amount: {itemAmount}");
                    }

                    foreach (var item in order.OrderPayment.ToList())
                    {
                        if (item.IsRefund == true)
                        {
                            model.OrderPayment.Remove(item);
                        }
                    }

                    var queryOrderScanned = model.OrderItemScanned.Where(p => p.orderId == orderId)
                        .ToList();

                    foreach (var item in queryOrderScanned)
                    {
                        item.IsItemReturned = false;
                        item.returnQty = 0;
                        model.Entry(item).State = EntityState.Modified;
                    }

                    // Round the final total order amount
                    totalOrderAmount = Math.Round(totalOrderAmount, 2, MidpointRounding.AwayFromZero);
                    order.docOrderAmt = totalOrderAmount;
                    model.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }


        public bool CreateOrderPayment(List<CreateOrderPayment> entities, bool IsRefund)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = entities.FirstOrDefault();
                    var order = model.Order.SingleOrDefault(o => o.id == entity.OrderId);
                    if (!IsRefund)
                    {
                        if (entity.Payment == "Save")
                        {
                            order.orderStatus = "Payment";
                        }
                        else
                        {
                            order.orderStatus = "Completed";
                        }
                        // Manually remove existing order items
                        foreach (var item in order.OrderPayment.ToList())
                        {
                            model.OrderPayment.Remove(item);
                        }
                    }
                    foreach (var item in entities)
                    {
                        var orderPayment = new OrderPayment
                        {
                            order_Id = item.OrderId,
                            paymentType = item.PaymentType,
                            amount = item.Amount,
                            refNumber = item.RefNumber,
                            paymentDate = item.PaymentDate,
                            paymentStatus = item.PaymentStatus,
                            Bank = item.Bank,
                            user_Id = item.UserId,
                            IsRefund = item.IsRefund
                        };
                        model.OrderPayment.Add(orderPayment);
                    }

                    System.Diagnostics.Debug.Write(entities);
                    model.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }


        public ProductBarcode GetProductCartons(string serialNo, string barcodeType)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Query once and store the result
                    List<ProductCarton> cartonList = new List<ProductCarton>();
                    if(!string.IsNullOrEmpty(barcodeType) && barcodeType == "Carton")
                    {
                        cartonList = model.ProductCarton
                                                .Where(ois => ois.barcode == serialNo
                                                    && ois.IsPartialCarton == false
                                                    && ois.IsPickedComplete == false
                                                    && ois.OnHold == false
                                                    &&  ois.IsCarton == true)
                                                .ToList();
                    }
                    else if (!string.IsNullOrEmpty(barcodeType) && barcodeType == "Loose")
                    {
                        cartonList = model.ProductCarton
                                                .Where(ois => ois.barcode == serialNo
                                                    && ois.IsPartialCarton == false
                                                    && ois.IsPickedComplete == false
                                                    && ois.OnHold == false
                                                    && ois.IsCarton == false)
                                                .ToList();
                    }
                    else if (!string.IsNullOrEmpty(barcodeType) && barcodeType == "CartonAndLoose")
                    {
                        // PickedComplete also include
                        cartonList = model.ProductCarton
                                                .Where(ois => ois.barcode == serialNo
                                                    && ois.IsPartialCarton == false
                                                    && ois.OnHold == false)
                                                .ToList();
                    }


                    if (!cartonList.Any())
                        return null;

                    // Map the first item to ProductBarcode
                    var firstItem = cartonList.First();

                    var _product = new ProductBarcode
                    {
                        id = firstItem.id,
                        epicorePartNo = firstItem.epicorPartNo,
                        barcode = firstItem.barcode,
                        weightValue = firstItem.weightValue,
                        weightMutiplier = firstItem.weightMutiplier,
                        barcodeFromPos = Convert.ToInt32(firstItem.productCodeFromPosition),
                        barcodeToPos = Convert.ToInt32(firstItem.productCodeToPosition),
                        weightFromPos = Convert.ToInt32(firstItem.weightFromPosition),
                        weightToPos = Convert.ToInt32(firstItem.weightToPosition),
                        Location = firstItem.Location,
                        CtnQty = firstItem.CtnQty??0,
                        IumQty = firstItem.IumQty ?? 0,
                        Ium = firstItem.Ium,
                        IsCarton = firstItem.IsCarton??false,
                        IsPickedComplete = firstItem.IsPickedComplete ?? false,
                        IsPartialCarton = firstItem.IsPartialCarton ?? false,
                        IsVaryWg = firstItem.IsVaryWg ?? false,
                        LooseQty = firstItem.LooseQty ?? 0,
                        LooseUom = firstItem.LooseUom,
                        ReceivedDate = firstItem.ReceivedDate,
                        ExpiryDate = firstItem.ExpiryDate,
                        OnHold = firstItem.OnHold ?? false,
                        SeqNum = firstItem.SeqNum ?? 0,
                        allowVaryWeight = firstItem.IsVaryWg,
                        productLocation = firstItem.Location,
                        LocationList = cartonList
                            .Where(x => !string.IsNullOrEmpty(x.Location))
                            .Select(x => x.Location)
                            .Distinct()
                            .ToList()
                    };

                    return _product;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }

        public ProductBarcode GetProductWeightBarcodes(string serialNo)
        {
            try
            {

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _product = model.ProductWeightBarcode
                        .Where(ois => ois.barcode == serialNo)
                        .ToList()
                        .Select(item => new ProductBarcode
                        {
                            id = item.id,
                            epicorePartNo = item.epicorePartNo,
                            barcode = item.fullBarcode,
                            weightValue = item.weightValue,
                            weightMutiplier = item.weightMultiply,
                            barcodeFromPos = Convert.ToInt32(item.barcodeFromPos),
                            barcodeToPos = Convert.ToInt32(item.barcodeToPos),
                            weightFromPos = Convert.ToInt32(item.weightFromPos),
                            weightToPos = Convert.ToInt32(item.weightToPos),
                            Location = item.Location,

                        }).FirstOrDefault();
                    if (_product != null)
                    {
                        var locList = model.StockBalance
                        .Where(ois => ois.partNum == _product.epicorePartNo && ois.onHandQty > 0)
                        .ToList();
                        if (locList != null)
                            _product.LocationList = locList.Where(w => !string.IsNullOrEmpty(w.Location))
                                                    .Select(x => x.Location).ToList();
                    }
                    

                    return _product;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }

        public bool DeleteScanItem(int orderId)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _scannedItems = model.OrderItemScanned
                    .Join(model.OrderItems,                       // Join OrderItemScanned with OrderItems
                          scanned => scanned.orderItemId,         // The key from OrderItemScanned
                          orderItem => orderItem.orderItemId,     // The key from OrderItems
                          (scanned, orderItem) => new { Scanned = scanned, OrderItem = orderItem }) // Result of the join
                    .Where(item => item.OrderItem.orderId == orderId)  // Apply the where condition on orderId
                    .Select(item => item.Scanned)                // Select only OrderItemScanned
                    .ToList();



                    if (_scannedItems != null)
                    {
                        foreach (var item in _scannedItems)
                        {
                            _scannedItems.Remove(item);
                        }
                    }
                    model.SaveChanges();
                }
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isDeleted;
        }

        public string GetFormattedAddress(string name, string address1, string address2, string address3, string city, string postcode,
           string state, string country)
        {
            string _address = "";
            char[] MyChar = { ',', ' ', '-' };

            address1 = address1 == null ? "" : address1.TrimStart(MyChar).TrimEnd(MyChar);
            address2 = address2 == null ? "" : address2.TrimStart(MyChar).TrimEnd(MyChar);
            address3 = address3 == null ? "" : address3.TrimStart(MyChar).TrimEnd(MyChar);
            city = city == null ? "" : city.TrimStart(MyChar).TrimEnd(MyChar);
            postcode = postcode == null ? "" : postcode.TrimStart(MyChar).TrimEnd(MyChar);
            state = state == null ? "" : state.TrimStart(MyChar).TrimEnd(MyChar);
            country = country == null ? "" : country.TrimStart(MyChar).TrimEnd(MyChar);

            if (!String.IsNullOrEmpty(name))
                _address += name.ToUpper() + "\n";

            if (!String.IsNullOrEmpty(address1))
                _address += address1.ToUpper() + "\n";

            if (!String.IsNullOrEmpty(address2))
                _address += address2 + "\n";

            if (!String.IsNullOrEmpty(address3))
                _address += address3 + " ";

            if (city != state)
            {
                if (!String.IsNullOrEmpty(city))
                    _address += postcode + " " + city + "\n";

                if (state != "HQ")
                {
                    if (!String.IsNullOrEmpty(state))
                        _address += ", " + state + "\n";
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(city))
                    _address += postcode + " " + state + "\n";
            }

            _address += country + "\n";
            _address = _address.TrimEnd(MyChar).ToUpper();

            return _address;

        }


        private ChartInfo SetDefaultFilterDate(ChartInfo entity)
        {
            var obj = new ChartPanelInfo();
            if (entity.loadDateFrom == DateTime.MinValue || entity.loadDateTo == DateTime.MinValue)
            {
                var currentDate = DateTime.Now.Date;
                if (entity.loadDateFrom == DateTime.MinValue)
                    obj.loadDateFrom = new DateTime(currentDate.Year, currentDate.Month, 1); // firstOfTheMonth.AddMonths(-3).AddDays(1);
                else
                    obj.loadDateFrom = entity.loadDateFrom;

                if (entity.loadDateTo == DateTime.MinValue)
                    obj.loadDateTo = currentDate;
                else
                    obj.loadDateTo = entity.loadDateTo;

                return obj;
            }
            return entity;
        }

        public List<ChartPanelInfo> GetChartPanelInfo(ChartPanelInfo entity)
        {
            try
            {
                var obj = SetDefaultFilterDate(entity);
                entity.loadDateFrom = obj.loadDateFrom;
                entity.loadDateTo = obj.loadDateTo;

                var result = new List<ChartPanelInfo>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    foreach (var item in entity.typeList)
                    {
                        var _chartInfo = new ChartPanelInfo();
                        _chartInfo.sectionType = item;

                        var statusList = new List<string> { "Pending", "Draft", "PickupAccepted", "SubmitForPicking" };

                       
                        // Filter orders by status and date range
                        var _lst = model.Order
                            .Where(order => order.orderDate >= entity.loadDateFrom && order.orderDate <= entity.loadDateTo) // Filter by date range
                            .GroupBy(order => statusList.Contains(order.orderStatus) ? "Pending" : order.orderStatus)
                            .Select(g => new ChartDashboard
                            {
                                type = g.Key,
                                value = g.Count()
                            })
                            .ToList(); 
                        _chartInfo.chartItems = _lst;

                        result.Add(_chartInfo);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }

        public BarChartPanelOrderInfo GetBarChartPanelOrderInfo(BarChartPanelOrderInfo entity)
        {
            try
            {
                var _entity = new BarChartPanelOrderInfo();
                _entity.barChartPanelOrderList = new List<BarChartPanelOrders>();

                var obj = SetDefaultFilterDate(entity);
                entity.loadDateFrom = obj.loadDateFrom;
                entity.loadDateTo = obj.loadDateTo;


                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var statusList = new List<string> { "Pending", "Draft", "PickupAccepted", "SubmitForPicking" };

                    foreach (var item in entity.typeList)
                    {
                        var _order = new BarChartPanelOrders();
                        _order.sectionType = item;

                        var result = model.Order.GroupBy(order => statusList.Contains(order.orderStatus) ? "Pending" : order.orderStatus).Select(g => new BarChartPanelOrderItems { categoryName = g.Key, count = g.Count() }).ToList();

                        _order.lstItem = result;

                        _entity.barChartPanelOrderList.Add(_order);
                    }
                }
                return _entity;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }
        public SalesSummaryResult GetSalesSummary(DateTime dateFrom, DateTime dateTo)
        {

            using (var context = new HanodaleEntities())
            {
                // Open a connection to the database
             var connection = context.Database.Connection;
                connection.Open();

                // Create a command for executing the stored procedure
                var command = connection.CreateCommand();
                command.CommandText = "GetSalesSummary";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Add parameters
                var paramDateFrom = command.CreateParameter();
                paramDateFrom.ParameterName = "@StartDate";
                paramDateFrom.Value = dateFrom;
                command.Parameters.Add(paramDateFrom);

                var paramDateTo = command.CreateParameter();
                paramDateTo.ParameterName = "@EndDate";
                paramDateTo.Value = dateTo;
                command.Parameters.Add(paramDateTo);

                // Execute the command and read multiple result sets
                var result = new SalesSummaryResult();
                using (var reader = command.ExecuteReader())
                {
                    // Translate Pending Sales Table
                    if (reader.HasRows)
                    {
                        result.PendingSalesTable = ((IObjectContextAdapter)context).ObjectContext.Translate<PendingSalesTable>(reader).ToList();
                    }
                    else
                    {
                        result.PendingSalesTable = new List<PendingSalesTable>(); // Initialize as empty if no rows
                    }

                    // Move to the next result set and translate Completed Sales Table
                    if (reader.NextResult() && reader.HasRows)
                    {
                        result.CompletedSalesTable = ((IObjectContextAdapter)context).ObjectContext.Translate<CompletedSalesTable>(reader).ToList();
                    }
                    else
                    {
                        result.CompletedSalesTable = new List<CompletedSalesTable>();
                    }

                    // Move to the next result set and translate Sync Completed Sales Table
                    if (reader.NextResult() && reader.HasRows)
                    {
                        result.SyncCompletedSalesTable = ((IObjectContextAdapter)context).ObjectContext.Translate<SyncCompletedSalesTable>(reader).ToList();
                    }
                    else
                    {
                        result.SyncCompletedSalesTable = new List<SyncCompletedSalesTable>();
                    }

                    // Move to the next result set and translate Payment Collection Table
                    if (reader.NextResult() && reader.HasRows)
                    {
                        result.PaymentCollectionTable = ((IObjectContextAdapter)context).ObjectContext.Translate<PaymentCollectionTable>(reader).ToList();
                    }
                    else
                    {
                        result.PaymentCollectionTable = new List<PaymentCollectionTable>();
                    }
                }


                return result;
            }
        }
        public OrderItemDiscountApproval OrderItemDiscountApproval(OrderItemDiscountApproval entity)
        {
            try
            {


                using (HanodaleEntities model = new HanodaleEntities())
                {

                    // Check if a record with the same orderId, orderItemId, discount, and approverUserId already exists
                    var existingApproval = model.OrderItemDiscountApproval.FirstOrDefault(x =>
        (entity.orderId == x.orderId || (entity.orderId == 0 && x.orderId == 0)) &&
    (entity.orderItem_Id == x.orderItem_Id || (entity.orderItem_Id == 0 && x.orderItem_Id == 0)) &&
    (entity.discount == x.discount));

                    // If a matching approval already exists, return the existing entity
                    if (existingApproval != null)
                    {
                        return existingApproval;
                    }

                    // If no matching record exists, create and insert a new approval record
                    var approval = new OrderItemDiscountApproval
                    {
                        orderItem_Id = entity.orderItem_Id,
                        approverUser_Id = entity.approverUser_Id,
                        discount = entity.discount,
                        approvalDate = entity.approvalDate,
                        remarks = entity.remarks,
                        requester_Id = entity.requester_Id,
                        orderId = entity.orderId ?? 0
                    };

                    // Add the new approval record
                    model.OrderItemDiscountApproval.Add(approval);

                    model.SaveChanges();
                    return approval;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }



        #endregion
    }
}
