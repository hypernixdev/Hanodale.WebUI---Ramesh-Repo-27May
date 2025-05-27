using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Xml.Linq;

namespace Hanodale.DataAccessLayer.Services
{
    public class ProductCartonService : BaseService, IProductCartonService
    {
        public ProductCartonDetails GetProductCartonBySearch(DatatableFilters entityFilter)
        {
            ProductCartonDetails _result = new ProductCartonDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    var query = model.ProductCarton.AsNoTracking().Where(p => true);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.epicorPartNo.Contains(entityFilter.search)
                                || p.barcode.Contains(entityFilter.search)
                                || p.vendorProductCode.Contains(entityFilter.search)

                            ));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new ProductCartons
                        {
                            id = p.id,
                            epicorPartNo = p.epicorPartNo,
                            barcode = p.barcode,
                            vendorProductCode = p.vendorProductCode,
                            weightValue = p.weightValue,
                            weightMutiplier = p.weightMutiplier ?? 0

                        });

                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstProductCarton = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public ProductCartons CreateProductCarton(ProductCartons entityEn)
        {
            var _ProductCartonEn = new Entity.Core.ProductCarton();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _ProductCartonEn.epicorPartNo = entityEn.epicorPartNo;
                    _ProductCartonEn.barcode = entityEn.barcode;
                    _ProductCartonEn.vendorProductCode = entityEn.vendorProductCode;
                    _ProductCartonEn.productKey = entityEn.productKey;
                    _ProductCartonEn.productBarcodeLength = entityEn.productBarcodeLength;
                    _ProductCartonEn.productCodeFromPosition = entityEn.productCodeFromPosition;
                    _ProductCartonEn.productCodeToPosition = entityEn.productCodeToPosition;
                    _ProductCartonEn.weightFromPosition = entityEn.weightFromPosition;
                    _ProductCartonEn.weightToPosition = entityEn.weightToPosition;
                    _ProductCartonEn.weightValue = entityEn.weightValue;
                    _ProductCartonEn.weightMutiplier = entityEn.weightMutiplier;
                    _ProductCartonEn.createdBy = entityEn.createdBy;
                    _ProductCartonEn.createdDate = entityEn.createdDate;
                    model.ProductCarton.Add(_ProductCartonEn);
                    model.SaveChanges();

                    entityEn.id = _ProductCartonEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public ProductCartons UpdateProductCarton(ProductCartons entityEn)
        {
            var _ProductCartonEn = new Entity.Core.ProductCarton();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _ProductCartonEn = model.ProductCarton.SingleOrDefault(p => p.id == entityEn.id);
                    if (_ProductCartonEn != null)
                    {
                        _ProductCartonEn.epicorPartNo = entityEn.epicorPartNo;
                        _ProductCartonEn.barcode = entityEn.barcode;
                        _ProductCartonEn.vendorProductCode = entityEn.vendorProductCode;
                        _ProductCartonEn.productKey = entityEn.productKey;
                        _ProductCartonEn.productBarcodeLength = entityEn.productBarcodeLength;
                        _ProductCartonEn.productCodeFromPosition = entityEn.productCodeFromPosition;
                        _ProductCartonEn.productCodeToPosition = entityEn.productCodeToPosition;
                        _ProductCartonEn.weightFromPosition = entityEn.weightFromPosition;
                        _ProductCartonEn.weightToPosition = entityEn.weightToPosition;
                        _ProductCartonEn.weightValue = entityEn.weightValue;
                        _ProductCartonEn.weightMutiplier = entityEn.weightMutiplier;
                        _ProductCartonEn.modifiedBy = entityEn.modifiedBy;
                        _ProductCartonEn.modifiedDate = entityEn.modifiedDate;

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

        public bool DeleteProductCarton(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _ProductCartonEn = model.ProductCarton.SingleOrDefault(p => p.id == id);
                    if (_ProductCartonEn != null)
                    {
                        model.ProductCarton.Remove(_ProductCartonEn);
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

        public ProductCartons GetProductCartonById(int id)
        {
            ProductCartons _ProductCartonEn = new ProductCartons();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.ProductCarton.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _ProductCartonEn = new ProductCartons
                        {
                            id = entity.id,
                            epicorPartNo = entity.epicorPartNo,
                            barcode = entity.barcode,
                            vendorProductCode = entity.vendorProductCode,
                            productKey = entity.productKey,
                            productBarcodeLength = entity.productBarcodeLength,
                            productCodeFromPosition = entity.productCodeFromPosition,
                            productCodeToPosition = entity.productCodeToPosition,
                            weightFromPosition = entity.weightFromPosition,
                            weightToPosition = entity.weightToPosition,
                            weightValue = entity.weightValue,
                            weightMutiplier = entity.weightMutiplier,




                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _ProductCartonEn;
        }

        public List<ProductCartons> GetProductCartonValue(string barcode)
        {
            var productcarton = new List<ProductCartons>();
            //List<ProductCarton> entities;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entities = (from pl in model.ProductCarton
                                    join cpl in model.Product on pl.epicorPartNo equals cpl.partNumber into values
                                    from cpl in values.DefaultIfEmpty()
                                    orderby pl.id
                                    where pl.barcode == barcode
                                    select new
                                    {
                                        id = pl.id,
                                        epicorPartNo = pl.epicorPartNo,
                                        partName = cpl.description,
                                        productCodeFromPosition = pl.productCodeFromPosition,
                                        productCodeToPosition = pl.productCodeToPosition,
                                        weightFromPosition = pl.weightFromPosition,
                                        weightToPosition = pl.weightToPosition,
                                        weightMutiplier = pl.weightMutiplier,
                                        barcode = pl.barcode,
                                        weightValue = pl.weightValue
                                    }).ToList();




                    if (entities.Any())
                    {
                        productcarton = entities.Select(entity => new ProductCartons
                        {
                            id = entity.id,
                            epicorPartNo = entity.epicorPartNo,
                            partName = entity.partName,
                            weightToPosition = entity.weightToPosition,
                            weightFromPosition = entity.weightFromPosition,

                            productCodeFromPosition = entity.productCodeFromPosition,
                            productCodeToPosition = entity.productCodeToPosition,
                            weightMutiplier = entity.weightMutiplier,
                            barcode = entity.barcode,
                            weightValue = entity.weightValue


                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return productcarton;
        }
        public bool IsProductCartonExists(ProductCartons entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.ProductCarton.Any(p => p.epicorPartNo == entityEn.epicorPartNo && p.barcode == entityEn.barcode && (entityEn.id == 0 || p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public ProductCartonFileUploadResult SaveProductCartonBunchList(ProductCartonFileUpload entityEn)
        {
            try
            {
                var result = new ProductCartonFileUploadResult();

                const int batchSize = 100; // Adjust batch size as needed
                int counter = 0;
                int totalRecords = entityEn.lstProductCarton.Count;
                var csvData = entityEn.lstProductCarton;

                if (entityEn.lstProductCarton != null)
                {
                    while (counter < totalRecords)
                    {
                        using (var dbContext = new HanodaleEntities())
                        {
                            var batch = csvData.Skip(counter).Take(batchSize).ToList();
                            var lst = batch.Select(p => p.epicorPartNo + "-" + p.barcode).ToList();


                            counter += batchSize;

                            var existingList = dbContext.ProductCarton.Where(p => lst.Contains(p.epicorPartNo + "-" + p.barcode)).ToList();

                            if (existingList != null)
                            {
                                foreach (var _productCartonEn in existingList)
                                {
                                    lst.Remove(_productCartonEn.epicorPartNo + "-" + _productCartonEn.barcode);

                                    var item = batch.FirstOrDefault(p => p.epicorPartNo == _productCartonEn.epicorPartNo && p.barcode == _productCartonEn.barcode);

                                    if (item != null)
                                    {
                                        // Update existing record
                                        _productCartonEn.productKey = item.productKey;
                                        _productCartonEn.epicorPartNo = item.epicorPartNo;
                                        _productCartonEn.barcode = item.barcode;
                                        _productCartonEn.vendorProductCode = item.vendorProductCode;
                                        _productCartonEn.productBarcodeLength = item.productBarcodeLength;
                                        _productCartonEn.productCodeFromPosition = item.productCodeFromPosition;
                                        _productCartonEn.productCodeToPosition = item.productCodeToPosition;
                                        _productCartonEn.weightFromPosition = item.weightFromPosition;
                                        _productCartonEn.weightToPosition = item.weightToPosition;
                                        _productCartonEn.weightValue = item.weightValue;
                                        _productCartonEn.weightMutiplier = item.weightMutiplier;

                                        _productCartonEn.modifiedDate = item.createdDate;
                                        _productCartonEn.modifiedBy = item.createdBy;
                                        //dbContext.Entry(_productWeightBarcodeEn).State = EntityState.Modified;
                                    }
                                }
                            }


                            var newBatchList = batch.Where(p => lst.Contains(p.epicorPartNo + "-" + p.barcode)).Select(item => new ProductCarton
                            {

                                barcode = item.barcode,
                                productKey = item.productKey,
                                epicorPartNo = item.epicorPartNo,
                                vendorProductCode = item.vendorProductCode,
                                productBarcodeLength = item.productBarcodeLength,
                                productCodeFromPosition = item.productCodeFromPosition,
                                productCodeToPosition = item.productCodeToPosition,
                                weightFromPosition = item.weightFromPosition,
                                weightToPosition = item.weightToPosition,
                                weightValue = item.weightValue,
                                weightMutiplier = item.weightMutiplier,
                                createdDate = item.createdDate,
                                createdBy = item.createdBy,
                            });

                            foreach (var newRecord in newBatchList)
                            {
                                dbContext.ProductCarton.Add(newRecord);
                            }


                            dbContext.SaveChanges();
                        }

                        // Optionally, you can add a small delay between batches if needed
                        // System.Threading.Thread.Sleep(100);
                    }

                    result.isSuccessful = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }


        //public ProductCartonFileUploadResult SaveProductCartonBunchList(ProductCartonFileUpload entityEn)
        //{
        //    try
        //    {
        //        using (HanodaleEntities model = new HanodaleEntities())
        //        {
        //            if (entityEn.lstProductCarton != null)
        //            {
        //                var result = new ProductCartonFileUploadResult();
        //                result.lstItem = new List<ProductCartonFileUploadResultItem>();

        //                if (entityEn.lstProductCarton != null)
        //                {

        //                    foreach (var item in entityEn.lstProductCarton)
        //                    {
        //                        bool isNew = false;

        //                        var _productCartonEn = model.ProductCarton.SingleOrDefault(p => p.epicorPartNo == item.epicorPartNo && p.barcode == item.barcode);
        //                        if (_productCartonEn == null)
        //                        {
        //                            _productCartonEn = new ProductCarton();
        //                            isNew = true;
        //                        }

        //                        _productCartonEn.productKey = item.productKey;
        //                        _productCartonEn.epicorPartNo = item.epicorPartNo;
        //                        _productCartonEn.barcode = item.barcode;
        //                        _productCartonEn.vendorProductCode = item.vendorProductCode;
        //                        _productCartonEn.productBarcodeLength = item.productBarcodeLength;
        //                        _productCartonEn.productCodeFromPosition = item.productCodeFromPosition;
        //                        _productCartonEn.productCodeToPosition = item.productCodeToPosition;
        //                        _productCartonEn.weightFromPosition = item.weightFromPosition;
        //                        _productCartonEn.weightToPosition = item.weightToPosition;
        //                        _productCartonEn.weightValue = item.weightValue;
        //                        _productCartonEn.weightMutiplier = item.weightMutiplier;



        //                        if (isNew)
        //                        {
        //                            _productCartonEn.createdDate = item.createdDate;
        //                            _productCartonEn.createdBy = item.createdBy;
        //                            model.ProductCarton.Add(_productCartonEn);
        //                        }
        //                        else
        //                        {
        //                            _productCartonEn.modifiedDate = item.createdDate;
        //                            _productCartonEn.modifiedBy = item.createdBy;

        //                        }

        //                        model.SaveChanges();

        //                    }

        //                    result.isSuccessful = true;
        //                }
        //                return result;
        //            }
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.InnerException.InnerException.Message);
        //    }
        //}


    }
}
