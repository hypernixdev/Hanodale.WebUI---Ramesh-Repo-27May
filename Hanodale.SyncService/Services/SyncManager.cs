using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Hanodale.Entity.Core;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Data.Entity.Validation;
using Hanodale.SyncService.Models;
using Hanodale.Domain.DTOs.Order;
using Hanodale.Domain.DTOs.Sync;
using System.Threading;
using System.Collections;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel;
using System.Collections.Concurrent;
using System.Data.Objects;

namespace Hanodale.SyncService
{
    public class SyncManager : ISyncManager
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientWithShortTimeout;
        private const string API_BASE_URL = "http://192.168.11.8:7008/";
        private const string API_MODEL_NAMESPACE = "Hanodale.SyncService.Models";

        public SyncManager()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_BASE_URL),
                Timeout = TimeSpan.FromSeconds(600)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Second HttpClient with a 3 - second timeout
            _httpClientWithShortTimeout = new HttpClient
            {
                BaseAddress = new Uri(API_BASE_URL),
                Timeout = TimeSpan.FromSeconds(3) // 3 seconds timeout
            };
            _httpClientWithShortTimeout.DefaultRequestHeaders.Accept.Clear();
            _httpClientWithShortTimeout.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ProductPriceApiModel GetProductPriceAsync(string partNum, string shipToNum, string custNum, string orderDate)
        {
            try
            {
                // Prepare the request URL
                string requestUrl = $"api/get/productprice?partNums={partNum}&shipToNum={shipToNum}&custNum={custNum}&orderDate={orderDate}";
                System.Diagnostics.Debug.WriteLine(requestUrl);

                // Send the GET request synchronously with a timeout of 5 seconds
                HttpResponseMessage response = _httpClientWithShortTimeout.GetAsync(requestUrl).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ProductPriceApiModel
                    {
                        is404 = true
                    };
                }
                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a JSON string
                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Deserialize the JSON string into a ProductPriceDTO object
                List<ProductPriceApiModel> productPrice = JsonConvert.DeserializeObject<List<ProductPriceApiModel>>(jsonResponse);
                var product = productPrice[0];
                System.Diagnostics.Debug.WriteLine("Price: " + product.BasePrice);
                return product;
            }
            catch (HttpRequestException httpRequestException)
            {
                throw new Exception("Error fetching product price data from API", httpRequestException);
            }
            catch (TaskCanceledException taskCanceledException) when (!taskCanceledException.CancellationToken.IsCancellationRequested)
            {
                throw new Exception("The request timed out", taskCanceledException);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the product price data", ex);
            }
        }


        public List<UomConvApiResponseModel> GetUomConversions(List<string> partNums)
        {
            try
            {
                string partNumsQuery = string.Join("&partNums=", partNums);
                string requestUrl = $"api/get/uomconv?partNums={partNumsQuery}";
                System.Diagnostics.Debug.WriteLine(requestUrl);
                HttpResponseMessage response = _httpClient.GetAsync(requestUrl).Result;
                System.Diagnostics.Debug.WriteLine(response);
                response.EnsureSuccessStatusCode();

                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                List<UomConvApiResponseModel> uomConversions = JsonConvert.DeserializeObject<List<UomConvApiResponseModel>>(jsonResponse);

                return uomConversions;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error fetching UOM conversion data from API", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the UOM conversion data", ex);
            }
        }

        public bool SyncAllOrdersToEpicore()
        {
            try
            {
                // Get all orders with syncStatus = false
                var ordersToSync = GetOrdersToSync();  // This method should fetch orders where syncStatus == false
                bool allSuccessful = true;
               

                foreach (var order in ordersToSync)
                {
                    // Trigger API call for each order
                    var orderApiDto = order;
                    ApiResponse res = PostOrderToApi(orderApiDto);

                    OrderSyncStatusDto statusDto = new OrderSyncStatusDto
                    {
                        OrderId = order.Id ?? 0,
                        EpicoreOrderId = res.OrderResult.OrderNumber != 0 ? res.OrderResult.OrderNumber.ToString() : "",
                        ShipPackNumber = res.ShipmentResult.PackNumber != 0 ? res.ShipmentResult.PackNumber.ToString() : "",
                        UD16Key1 = res.PaymentResult.Key1AsCommaSeparated,
                        EpicoreInvNumber = res.ShipmentResult.InvoiceNumber,
                        SyncMessage = res.OrderResult.Message,
                        SyncStatus = res.OrderResult.IsSuccess
                    };

                    // Update the sync status for the order in the database
                    bool updateSyncStatus = UpdateOrderSyncStatus(statusDto);

                    // Collect result for each order processed
                    if (res.OrderResult.IsSuccess)
                    {
                        allSuccessful = true;
                    }
                    else
                    {
                        allSuccessful = true;
                    }
                }

                // Return the result as JSON for all orders
                return allSuccessful;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
        }

        public List<OrderApiDto> GetOrdersToSync()
        {
            try
            {
                var _result = new List<OrderApiDto>();
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Fetch all orders with syncStatus = false
                    DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);
                    var query = model.Order.Where(p => (p.syncStatus == false || p.syncStatus == null)
                            && p.orderStatus == "Completed"
                            && p.orderDate != null
                            && p.orderDate >= sevenDaysAgo)
                        .AsQueryable();
                    var orders = query.ToList();  // Get all orders with syncStatus = false

                    // Loop through each order to create the OrderApiDto
                    foreach (var _entity in orders)
                    {
                        var queryOrderScanned = model.OrderItemScanned
                            .Where(p => p.orderId == _entity.id)
                            .ToList();

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

                        var orderDto = new OrderApiDto
                        {
                            Id = _entity.id,
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
                                    if (retQty == effectiveQuantity)
                                        return null;  // Skip this item by returning null

                                    return new OrderDetailApiDto
                                    {
                                        LineNo = index + 1,  // Assuming line number is based on the index in the list
                                        PartNum = item.partNum,
                                        Quantity = effectiveQuantity,  // Adjusted Quantity
                                        Uom = item.salesUm,  // UOM (Unit of Measure)
                                        UnitPrice = item.unitPrice, // (item.unitPrice * effectiveQuantity) + item.operationCost,  // Adjusted price for the effective quantity
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
                                }
                                else
                                {
                                    orderDto.paymentDtl.Add(new PaymentDetail
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

                        _result.Add(orderDto);
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

        public ApiResponse PostOrderToApi(OrderApiDto orderData)
        {
            ApiResponse result = new ApiResponse();
            result.OrderResult = new OrderResult() { OrderNumber = 0 };
            result.ShipmentResult = new ShipmentResult() { PackNumber = 0 };
            result.PaymentResult = new PaymentResult() { Key1 = new List<string>() { "" } };
            IntegrationLog logEntry = new IntegrationLog();
            try
            {
                // Initialize log entry
                logEntry.apiPoint = "api/order";
                logEntry.triggerPoint = "PostOrderToApi";
                logEntry.startDateTime = DateTime.Now;

                // Prepare the request URL
                string requestUrl = "api/order";

                // Serialize the orderData to JSON
                string jsonOrderData = JsonConvert.SerializeObject(orderData);

                // Create the content for the POST request
                var content = new StringContent(jsonOrderData, Encoding.UTF8, "application/json");
                System.Diagnostics.Debug.WriteLine(content);

                // Send the POST request asynchronously
                HttpResponseMessage response = _httpClient.PostAsync(requestUrl, content).Result;

                // Capture response return time
                logEntry.responseReturnTime = DateTime.Now;

                // Ensure the request was successful or throw an exception
                if (!response.IsSuccessStatusCode)
                {
                    // If status code is 500, get the error message from the response
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().Result;
                        result.OrderResult.IsSuccess = false;
                        result.OrderResult.Message = $"Server error: {errorResponse}";

                        // Log the error details
                        logEntry.responseStatus = "Failed";
                        logEntry.responseMessage = $"Server error: {errorResponse}";

                        // Save JSON input in log details for failure cases
                        logEntry.IntegrationLogDetail.Add(new IntegrationLogDetail
                        {
                            jsonInput = jsonOrderData
                        });
                    }
                    else
                    {
                        // For other unsuccessful status codes, return a generic message
                        result.OrderResult.IsSuccess = false;
                        result.OrderResult.Message = $"Request failed with status code: {response.StatusCode}";
                        string errorResponse = response.Content.ReadAsStringAsync().Result;
                        System.Diagnostics.Debug.WriteLine(errorResponse);
                        // Log the failure status and message
                        logEntry.responseStatus = "Failed";
                        logEntry.responseMessage = $"Request failed with status code: {response.StatusCode}";

                        // Save JSON input in log details for failure cases
                        logEntry.IntegrationLogDetail.Add(new IntegrationLogDetail
                        {
                            jsonInput = jsonOrderData
                        });
                    }

                    // Save the log entry here
                    SaveIntegrationLog(logEntry);
                    return result;
                }

                // Read the response content as a JSON string
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                System.Diagnostics.Debug.WriteLine($"RESPONSE: {jsonResponse}");
                System.Diagnostics.Debug.WriteLine($"RESPONSE END");

                // Deserialize the response into a dynamic object
                var responseObj = JsonConvert.DeserializeObject<ApiResponseMessage>(jsonResponse);

                // Log success status and response
                logEntry.responseStatus = "Success";
                logEntry.responseMessage = "Order posted successfully.";

                // Save the log entry here
                SaveIntegrationLog(logEntry);

                return responseObj.Message;
            }
            catch (HttpRequestException httpRequestException)
            {
                result.OrderResult.IsSuccess = false;
                result.OrderResult.Message = "Error posting order data to API: " + httpRequestException.Message;

                // Log the exception
                logEntry.responseStatus = "Failed";
                logEntry.responseMessage = "Error posting order data to API: " + httpRequestException.Message;

                // Save JSON input in log details for failure cases
                logEntry.IntegrationLogDetail.Add(new IntegrationLogDetail
                {
                    jsonInput = JsonConvert.SerializeObject(orderData)
                });

                // Save the log entry here
                SaveIntegrationLog(logEntry);
            }
            catch (Exception ex)
            {
                result.OrderResult.IsSuccess = false;
                result.OrderResult.Message = "Error posting order data to API: " + ex.Message;

                // Log the exception
                logEntry.responseStatus = "Failed";
                logEntry.responseMessage = "Error posting order data to API: " + ex.Message;

                // Save JSON input in log details for failure cases
                logEntry.IntegrationLogDetail.Add(new IntegrationLogDetail
                {
                    jsonInput = JsonConvert.SerializeObject(orderData)
                });

                // Save the log entry here
                SaveIntegrationLog(logEntry);
            }

            return result;
        }

        // Method to save the log entry to the database
        private void SaveIntegrationLog(IntegrationLog logEntry)
        {
            try
            {
                using (var context = new HanodaleEntities())
                {
                    context.IntegrationLog.Add(logEntry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }



        public SyncReponse SyncEntity(string entityName, string apiUrl, string uniqueField, bool disablePagination, string customDbModel)
        {
            return SyncEntity(entityName, apiUrl, uniqueField, disablePagination, customDbModel, 1);
        }

        public SyncReponse SyncEntity(string entityName, string apiUrl, string uniqueField, bool disablePagination, string customDbModel, int pageNumber = 1)
        {
            System.Diagnostics.Debug.WriteLine(entityName);
            SyncReponse res = new SyncReponse();
            IntegrationLog logEntry = new IntegrationLog();
            string syncId = Guid.NewGuid().ToString();
            const int BATCH_SIZE = 10; // Process 10 pages concurrently

            try
            {
                // Initialize log entry
                logEntry.apiPoint = apiUrl == "" ? $"api/get/{entityName.ToLower()}s" : $"api/get/{apiUrl.ToLower()}";
                logEntry.triggerPoint = "SyncEntity";
                logEntry.startDateTime = DateTime.Now;

                // Get the full type name and validate types
                var typeName = $"Hanodale.Entity.Core.{entityName}, Hanodale.Entity.Core";
                Type apiModelType = Type.GetType($"{API_MODEL_NAMESPACE}.{entityName}ApiModel");
                var dbModelType = Type.GetType(typeName);

                if (apiModelType == null || dbModelType == null)
                {
                    logEntry.responseStatus = "Failed";
                    logEntry.responseMessage = $"Could not find model classes for entity '{entityName}'";
                    logEntry.responseReturnTime = DateTime.Now;
                    SaveIntegrationLog(logEntry);
                    throw new ArgumentException($"Could not find model classes for entity '{entityName}'");
                }

                bool hasSyncIdField = dbModelType.GetProperties()
                    .Any(p => p.Name.Equals("syncId", StringComparison.OrdinalIgnoreCase));

                var syncContext = new SynchronizationContext
                {
                    EntityName = entityName,
                    ApiUrl = apiUrl,
                    UniqueField = uniqueField,
                    SyncId = syncId,
                    HasSyncIdField = hasSyncIdField,
                    ApiModelType = apiModelType,
                    DbModelType = dbModelType,
                    CustomDbModel = customDbModel,
                    DisablePagination = disablePagination
                };

                var errors = new ConcurrentBag<string>();
                var processedPages = new ConcurrentDictionary<int, bool>();
                bool hasMoreData = true;
                int currentBatchStart = pageNumber;
                bool firstRun = true;

                // Modified while loop condition to ensure at least one run
                while ((hasMoreData && !disablePagination) || firstRun)
                {
                    var tasks = new List<Task>();
                    var currentBatchPages = new ConcurrentDictionary<int, bool>();

                    // Process batch of pages concurrently
                    using (var semaphore = new Semaphore(BATCH_SIZE, BATCH_SIZE))
                    {
                        // If pagination is disabled, only process one page
                        int pagesToProcess = disablePagination ? 1 : BATCH_SIZE;

                        for (int i = 0; i < pagesToProcess; i++)
                        {
                            int currentPage = currentBatchStart + i;
                            tasks.Add(Task.Run(() => ProcessPage(currentPage, syncContext, currentBatchPages, semaphore)));
                        }

                        // Wait for all tasks in current batch to complete
                        Task.WaitAll(tasks.ToArray());
                    }

                    // Check if we found any data in this batch
                    hasMoreData = currentBatchPages.Any(p => p.Value);

                    // Merge results from this batch
                    foreach (var page in currentBatchPages)
                    {
                        processedPages.TryAdd(page.Key, page.Value);
                    }

                    // Move to next batch
                    currentBatchStart += BATCH_SIZE;
                    firstRun = false;

                    // Break if we're in single page mode
                    if (disablePagination)
                    {
                        break;
                    }
                }

                // Log completion
                logEntry.responseStatus = errors.Any() ? "Partial Success" : "Success";
                logEntry.responseMessage = $"Synced entity {entityName}. Processed pages: {processedPages.Count}, Errors: {errors.Count}";
                logEntry.responseReturnTime = DateTime.Now;
                SaveIntegrationLog(logEntry);

                res.Result = !errors.Any();
                res.Message = errors.Any()
                    ? $"Completed with {errors.Count} errors"
                    : "Successfully synced all data";
                res.Errors = errors.ToList();

                return res;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error syncing entity {entityName}: {ex.Message}");

                logEntry.responseStatus = "Failed";
                logEntry.responseMessage = ex.Message;
                logEntry.responseReturnTime = DateTime.Now;
                SaveIntegrationLog(logEntry);

                res.Result = false;
                res.Message = ex.Message;
                return res;
            }
        }

        private void ProcessPage(
            int pageNumber,
            SynchronizationContext context,
            ConcurrentDictionary<int, bool> processedPages,
            Semaphore semaphore)
        {
            semaphore.WaitOne();
            try
            {
                string apiData = FetchDataFromApi(context.EntityName, pageNumber, context.ApiUrl);

                // Check if we got any data
                var apiEntities = JsonConvert.DeserializeObject(apiData, typeof(List<>).MakeGenericType(context.ApiModelType)) as IEnumerable<object>;
                if (apiEntities == null || !apiEntities.Any())
                {
                    processedPages.TryAdd(pageNumber, false);
                    return;
                }

                apiData = ConvertResponseFormat(apiData, context.CustomDbModel);

                bool success = ParseAndSaveData(
                    apiData,
                    context.ApiModelType,
                    context.DbModelType,
                    context.UniqueField,
                    context.SyncId,
                    context.HasSyncIdField
                );

                processedPages.TryAdd(pageNumber, success);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("NotFound"))
            {
                System.Diagnostics.Debug.WriteLine($"Page {pageNumber} not found for {context.EntityName}");
                processedPages.TryAdd(pageNumber, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing page {pageNumber}: {ex.Message}");
                processedPages.TryAdd(pageNumber, false);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private class SynchronizationContext
        {
            public string EntityName { get; set; }
            public string ApiUrl { get; set; }
            public string UniqueField { get; set; }
            public string SyncId { get; set; }
            public bool HasSyncIdField { get; set; }
            public Type ApiModelType { get; set; }
            public Type DbModelType { get; set; }
            public string CustomDbModel { get; set; }
            public bool DisablePagination { get; set; }
        }

        private string ConvertResponseFormat(string response, string customDbModel)
        {
            if (customDbModel == "UomConv")
            {
                var apiEntities = JsonConvert.DeserializeObject<List<dynamic>>(response);
                var uomConvApiModels = new List<UomConvApiModel>();

                foreach (var entity in apiEntities)
                {
                    string company = entity.company;
                    string partNum = entity.partNum;

                    foreach (var uomDetail in entity.uomConvDetails)
                    {
                        var model = new UomConvApiModel
                        {
                            company = company,
                            partNum = partNum,
                            uomCode = uomDetail.uomCode,
                            convOperator = uomDetail.convOperator,
                            convFactor = uomDetail.convFactor.ToString(), // Convert convFactor to string
                            uniqueField = $"{company}-{partNum}-{uomDetail.uomCode}"
                        };

                        uomConvApiModels.Add(model);
                    }
                }

                // Serialize the list of UomConvApiModel back to JSON
                return JsonConvert.SerializeObject(uomConvApiModels);
            } else
            {
                return response;
            }
        }
        
        private string FetchDataFromApi(string entityName, int page, string apiUrl)
        {
            int PAGE_SIZE = 600;
            string requestUri = "";
            if (apiUrl == "")
            {
                requestUri = $"api/get/{entityName.ToLower()}s?pageNumber={page}&pageSize={PAGE_SIZE}";
            }
            else
            {
                requestUri = $"api/get/{apiUrl.ToLower()}?pageNumber={page}&pageSize={PAGE_SIZE}"; // &partNum=C-KARAAGE-MY-TYSON-1KGX10";
            }
            Debug.WriteLine($"Sending GET request to: {_httpClient.BaseAddress}{requestUri}");

            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result;

                Debug.WriteLine($"Response status code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine($"Error response content: {errorContent}");
                    throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
                }

                string content = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"Received content length: {content}");
                return content;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HTTP Request failed: {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"HTTP Request timed out: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error during HTTP request: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        /*
         * Batch processing added, 1000 records will be processed at one time.
         * 
         */
        private bool ParseAndSaveData(string jsonData, Type apiModelType, Type dbModelType, string uniqueField, string syncId, bool hasSyncIdField)
        {
            var apiEntities = JsonConvert.DeserializeObject(jsonData, typeof(List<>).MakeGenericType(apiModelType)) as IEnumerable<object>;
            System.Diagnostics.Debug.WriteLine("Called this");
            System.Diagnostics.Debug.WriteLine(apiEntities);
            System.Diagnostics.Debug.WriteLine(dbModelType.Name);

            // Batch size of 300
            const int batchSize = 300;
            var apiEntitiesList = apiEntities.ToList();

            using (var context = new HanodaleEntities())
            {
                // Process in batches of 300
                for (int batchStart = 0; batchStart < apiEntitiesList.Count; batchStart += batchSize)
                {
                    // Get the current batch
                    var currentBatch = apiEntitiesList.Skip(batchStart).Take(batchSize).ToList();

                    // Extract unique fields for the current batch
                    var uniqueFields = new List<string>();
                    foreach (var entity in currentBatch)
                    {
                        // Use reflection to get properties dynamically
                        var uniqueValue = entity.GetType().GetProperty(uniqueField)?.GetValue(entity, null)?.ToString();
                        uniqueFields.Add(uniqueValue);
                    }

                    // Build the SQL query with WHERE IN clause dynamically using parameter placeholders
                    string sqlQuery = $"SELECT * FROM {dbModelType.Name} READONLY WHERE {uniqueField} IN ({string.Join(",", uniqueFields.Select((_, i) => $"@p{i}"))})";

                    // Create an array of parameters to safely pass to the query
                    var parameters = uniqueFields.Select((value, i) => new SqlParameter($"@p{i}", value)).ToArray();

                    // Execute the raw SQL query and map the results to a generic model
                    var results = context.Database.SqlQuery(dbModelType, sqlQuery, parameters);
                    var resultsDictionary = new Dictionary<string, dynamic>();
                    // Output results for debugging
                    foreach (var result in results)
                    {
                        //System.Diagnostics.Debug.WriteLine(result);
                        // Extract the unique field value (adjust based on your actual property name)
                        var uniqueValue = result.GetType().GetProperty(uniqueField)?.GetValue(result, null)?.ToString();

                        if (!string.IsNullOrEmpty(uniqueValue))
                        {
                            // Add to dictionary, ensuring no duplicate keys
                            if (!resultsDictionary.ContainsKey(uniqueValue))
                            {
                                resultsDictionary[uniqueValue] = result; // Cast to your specific entity type
                            }
                        }
                    }

                    // Get the DbSet for the dbModelType
                    var dbSetProperty = context.GetType().GetProperties()
                        .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                             p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                             p.PropertyType.GetGenericArguments()[0] == dbModelType);

                    if (dbSetProperty == null)
                    {
                        throw new InvalidOperationException($"DbSet for {dbModelType.Name} not found in HanodaleEntities.");
                    }

                    dynamic dbSet = dbSetProperty.GetValue(context);

                    // Process each entity in the current batch
                    foreach (var apiEntity in currentBatch)
                    {
                        //var dbEntity = FindExistingEntity(dbSet, apiEntity, dbModelType, uniqueField);
                        var uniqueValue = apiEntity.GetType().GetProperty(uniqueField)?.GetValue(apiEntity, null)?.ToString();

                        if (uniqueValue == null)
                        {
                            continue; // If uniqueValue is null, skip this entity
                        }

                        // Check if the uniqueValue exists in the results (existing entities)
                        if (resultsDictionary.TryGetValue(uniqueValue, out var dbEntity))
                        {                            
                            // Update existing entity
                            CopyProperties(apiEntity, dbEntity);
                            if (hasSyncIdField)
                            {
                                dbEntity.GetType().GetProperty("syncId").SetValue(dbEntity, syncId);
                            }
                            //System.Diagnostics.Debug.WriteLine("Updating " + uniqueField);
                            if (uniqueValue == "C-KARAAGE-MY-TYSON-1KGX10")
                            {
                                context.Entry(dbEntity).State = EntityState.Modified;
                            }
                            context.Entry(dbEntity).State = EntityState.Modified;
                        }
                        else
                        {
                            // Insert new entity
                            dbEntity = Activator.CreateInstance(dbModelType);
                            CopyProperties(apiEntity, dbEntity);
                            if (hasSyncIdField)
                            {
                                dbEntity.GetType().GetProperty("syncId").SetValue(dbEntity, syncId);
                            }
                            //System.Diagnostics.Debug.WriteLine("Inserting " + uniqueField);
                            dbSet.Add(dbEntity);  // Use dynamic dbSet to invoke Add method
                        }
                    }

                    // Save changes after each batch
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Log the validation errors
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            }
                        }
                        throw; // Re-throw the exception after logging it
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return true;
            }
        }


        //private bool ParseAndSaveData(string jsonData, Type apiModelType, Type dbModelType, string uniqueField)
        //{
        //    var apiEntities = JsonConvert.DeserializeObject(jsonData, typeof(List<>).MakeGenericType(apiModelType)) as IEnumerable<object>;
        //    System.Diagnostics.Debug.WriteLine("Called this");
        //    System.Diagnostics.Debug.WriteLine(apiEntities);
        //    System.Diagnostics.Debug.WriteLine(dbModelType.Name);
        //    using (var context = new HanodaleEntities())
        //    {
        //        // Loop through apiEntities and extract uniqueField for each entry
        //        var uniqueFields = new List<string>();
        //        //var ii = 0;
        //        foreach (var entity in apiEntities)
        //        {
        //            // Use reflection to get properties dynamically
        //            var uniqueValue = entity.GetType().GetProperty(uniqueField)?.GetValue(entity, null)?.ToString();
        //            uniqueFields.Add(uniqueValue);
        //            //if (ii == 100)
        //            //{
        //            //    break;
        //            //}
        //            //ii++;
        //        }
        //        // Build the SQL query with WHERE IN clause dynamically using parameter placeholders
        //        string sqlQuery = $"SELECT id, {uniqueField} as uniqueField FROM {dbModelType.Name} WHERE {uniqueField} IN ({string.Join(",", uniqueFields.Select((_, i) => $"@p{i}"))})";

        //        // Create an array of parameters to safely pass to the query
        //        var parameters = uniqueFields.Select((value, i) => new SqlParameter($"@p{i}", value)).ToArray();

        //        // Execute the raw SQL query and map the results to dbModelType
        //        var results = context.Database.SqlQuery<GenericDbModel>(sqlQuery, parameters);

        //        // Output results for debugging
        //        foreach (var result in results)
        //        {
        //            System.Diagnostics.Debug.WriteLine(result);
        //        }

        //        var dbSetProperty = context.GetType().GetProperties()
        //            .FirstOrDefault(p => p.PropertyType.IsGenericType &&
        //                                 p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
        //                                 p.PropertyType.GetGenericArguments()[0] == dbModelType);

        //        if (dbSetProperty == null)
        //        {
        //            throw new InvalidOperationException($"DbSet for {dbModelType.Name} not found in HanodaleEntities.");
        //        }

        //        dynamic dbSet = dbSetProperty.GetValue(context);

        //        foreach (var apiEntity in apiEntities)
        //        {
        //            var dbEntity = FindExistingEntity(dbSet, apiEntity, dbModelType, uniqueField);

        //            if (dbEntity == null)
        //            {
        //                // Insert new entity
        //                dbEntity = Activator.CreateInstance(dbModelType);
        //                CopyProperties(apiEntity, dbEntity);
        //                System.Diagnostics.Debug.WriteLine("Inserting " + uniqueField);
        //                dbSet.Add(dbEntity);  // Use dynamic dbSet to invoke Add method
        //            }
        //            else
        //            {
        //                // Update existing entity
        //                CopyProperties(apiEntity, dbEntity);
        //                System.Diagnostics.Debug.WriteLine("Updating " + uniqueField);
        //                context.Entry(dbEntity).State = EntityState.Modified;
        //            }
        //        }

        //        try
        //        {
        //            context.SaveChanges();
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            // Log the validation errors
        //            foreach (var validationErrors in ex.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
        //                }
        //            }
        //            throw; // Re-throw the exception after logging it
        //        }

        //        return true;
        //    }
        //}


        private object FindExistingEntity(object dbSet, object apiEntity, Type dbModelType, string uniqueField)
        {
            var custNumProperty = apiEntity.GetType().GetProperty(uniqueField);
            //var custNumProperty = apiEntity.GetType().GetProperty("code");
            if (custNumProperty == null)
            {
                throw new InvalidOperationException($"No {uniqueField} property found on the API model.");
            }

            var custNumValue = custNumProperty.GetValue(apiEntity);
            var parameter = Expression.Parameter(dbModelType, "e");
            var property = Expression.Property(parameter, uniqueField);
            //var property = Expression.Property(parameter, "code");
            var condition = Expression.Equal(property, Expression.Constant(custNumValue));
            var lambda = Expression.Lambda(condition, parameter);

            // Create a generic method call to Where and FirstOrDefault
            var whereMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(dbModelType);

            var firstOrDefaultMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "FirstOrDefault" && m.GetParameters().Length == 1)
                .MakeGenericMethod(dbModelType);

            // Apply the Where clause
            var filteredQuery = whereMethod.Invoke(null, new[] { dbSet, lambda });

            // Execute FirstOrDefault
            return firstOrDefaultMethod.Invoke(null, new[] { filteredQuery });
        }

        private void CopyProperties(object source, object destination)
        {
            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                var destinationProperty = destination.GetType().GetProperty(sourceProperty.Name);
                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    if (value is null)
                        continue;

                    // If source type is string and destination type is DateTime?, try to convert
                    if (sourceProperty.PropertyType == typeof(string) &&
                        Nullable.GetUnderlyingType(destinationProperty.PropertyType) == typeof(DateTime))
                    {
                        DateTime? convertedValue = null;
                        if (DateTime.TryParse(value.ToString(), out DateTime dateTimeValue))
                        {
                            convertedValue = dateTimeValue; // Parse successful
                        }
                        destinationProperty.SetValue(destination, convertedValue);
                    }
                    else
                    {
                        destinationProperty.SetValue(destination, value);
                    }
                }
            }
        }
    }
}
