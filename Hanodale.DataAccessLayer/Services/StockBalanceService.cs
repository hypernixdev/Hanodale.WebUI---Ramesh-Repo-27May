using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;

namespace Hanodale.DataAccessLayer.Services
{
    public class StockBalanceService : BaseService, IStockBalanceService
    {
        //public StockBalanceDetails GetStockBalanceBySearch(DatatableFilters entityFilter)
        //{
        //    StockBalanceDetails _result = new StockBalanceDetails();
        //    _result.recordDetails = new RecordDetails();
        //    try
        //    {
        //        using (HanodaleEntities model = new HanodaleEntities())
        //        {
        //            if (entityFilter == null)
        //                entityFilter = new DatatableFilters();

        //            //var query = model.StockBalance.AsNoTracking().Where(p => true);
        //            var query = from sb in model.StockBalance
        //                        join sbv in model.StockBalanceView
        //                            on new { sb.partNum, sb.warehouseCode } equals new { sbv.partNum, sbv.warehouseCode } // join on partNum and warehouseCode
        //                        select new
        //                        {
        //                            sb.id,
        //                            sb.company,
        //                            sb.partNum,
        //                            sb.warehouseCode,
        //                            sb.onHandQty,
        //                            sb.uom,
        //                            sb.Location, // 🆕 Add this line
        //                            sbv.totalQtyBeforePayment,
        //                            sbv.totalQtyAfterPayment,
        //                            sbv.remainingQty
        //                        };


        //            _result.recordDetails.totalRecords = query.Count();
        //            _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

        //            if (!string.IsNullOrEmpty(entityFilter.search))
        //            {
        //                query = query.Where(p => (
        //                        p.company.Contains(entityFilter.search)
        //                        || p.partNum.Contains(entityFilter.search)
        //                        || p.warehouseCode.Contains(entityFilter.search)
        //                        || p.uom.Contains(entityFilter.search)
        //                    ));
        //            }

        //            var result = query
        //                        .OrderBy(p => p.company)
        //                        .ThenBy(p => p.partNum)
        //                        .ThenBy(p => p.warehouseCode)
        //                        .Select(p => new StockBalances
        //                        {
        //                            id = p.id,
        //                            company = p.company,
        //                            partNum = p.partNum,
        //                            warehouseCode = p.warehouseCode,
        //                            uom = p.uom,
        //                            onHandQty = p.onHandQty,
        //                            totalQtyBeforePayment = p.totalQtyBeforePayment,
        //                            totalQtyAfterPayment = p.totalQtyAfterPayment,
        //                            location = p.Location,
        //                        });

        //            _result.recordDetails.totalDisplayRecords = result.Count();
        //            _result.lstStockBalance = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.InnerException.InnerException.Message);
        //    }
        //    return _result;
        //}


        public StockBalanceDetails GetStockBalanceBySearch(DatatableFilters entityFilter)
        {
            StockBalanceDetails _result = new StockBalanceDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    var query = model.StockBalance
                        .Where(p => p.onHandQty > 0);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.company.Contains(entityFilter.search)
                                || p.partNum.Contains(entityFilter.search)
                                || p.warehouseCode.Contains(entityFilter.search)
                                || p.uom.Contains(entityFilter.search)
                            ));
                    }

                    var result = query
                                .OrderBy(p => p.company)
                                .ThenBy(p => p.partNum)
                                .ThenBy(p => p.warehouseCode)
                                .Select(p => new StockBalances
                                {
                                    id = p.id,
                                    company = p.company,
                                    partNum = p.partNum,
                                    warehouseCode = p.warehouseCode,
                                    uom = p.uom,
                                    onHandQty = p.onHandQty,
                                    location = p.Location
                                });

                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstStockBalance = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
            return _result;
        }

        public StockBalances CreateStockBalance(StockBalances entityEn)
        {
            var _StockBalanceEn = new Entity.Core.StockBalance();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _StockBalanceEn.company = entityEn.company;
                    _StockBalanceEn.partNum = entityEn.partNum;
                    _StockBalanceEn.warehouseCode = entityEn.warehouseCode;                 
                    _StockBalanceEn.uom = entityEn.uom;
                    _StockBalanceEn.onHandQty = entityEn.onHandQty;
                    
                    model.StockBalance.Add(_StockBalanceEn);
                    model.SaveChanges();

                    entityEn.id = _StockBalanceEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public StockBalances UpdateStockBalance(StockBalances entityEn)
        {
            var _StockBalanceEn = new Entity.Core.StockBalance();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _StockBalanceEn = model.StockBalance.SingleOrDefault(p => p.id == entityEn.id);
                    if (_StockBalanceEn != null)
                    {
                        _StockBalanceEn.company = entityEn.company;
                        _StockBalanceEn.partNum = entityEn.partNum;
                        _StockBalanceEn.warehouseCode = entityEn.warehouseCode;
                        _StockBalanceEn.uom = entityEn.uom;
                        _StockBalanceEn.onHandQty = entityEn.onHandQty;
                      

                        model.SaveChanges();

                        entityEn.isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public bool DeleteStockBalance(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _StockBalanceEn = model.StockBalance.SingleOrDefault(p => p.id == id);
                    if (_StockBalanceEn != null)
                    {
                        model.StockBalance.Remove(_StockBalanceEn);
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

        public StockBalances GetStockBalanceById(int id)
        {
            StockBalances _StockBalanceEn = new StockBalances();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.StockBalance.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _StockBalanceEn = new StockBalances
                        {
                            id = entity.id,
                            company = entity.company,
                            partNum = entity.partNum,
                            warehouseCode = entity.warehouseCode,                         
                            uom = entity.uom,
                            onHandQty = entity.onHandQty,
                            uniqueField = entity.uniqueField,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _StockBalanceEn;
        }

        public bool IsStockBalanceExists(StockBalances entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.StockBalance.Any(p => p.company == entityEn.company && p.partNum == entityEn.partNum && (entityEn.id == 0 || p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }


        public List<string> GetStockLocationOptions(string partNum)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.StockBalance
                        .Where(p => p.partNum == partNum && p.onHandQty > 0 && !string.IsNullOrEmpty(p.Location))
                        .Select(p => p.Location)
                        .Distinct()
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }
        public List<string> GetCartonLocationsOptions(string partNum, string barcode)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.ProductCarton
                        .Where(p => p.epicorPartNo == partNum && p.barcode==barcode && !string.IsNullOrEmpty(p.Location))
                        .Select(p => p.Location)
                        .Distinct()
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }


    }
}
