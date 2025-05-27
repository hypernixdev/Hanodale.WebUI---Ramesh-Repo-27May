using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Metadata.Edm;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;

namespace Hanodale.DataAccessLayer.Services
{
    public class ProductWeightBarcodeService : BaseService, IProductWeightBarcodeService
    {
        public ProductWeightBarcodeDetails GetProductWeightBarcodeBySearch(DatatableFilters entityFilter)
        {
            ProductWeightBarcodeDetails _result = new ProductWeightBarcodeDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    var query = model.ProductWeightBarcode.AsNoTracking().Where(p => true);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.epicorePartNo.Contains(entityFilter.search)
                                || p.fullBarcode.Contains(entityFilter.search)
                                || p.barcode.Contains(entityFilter.search)

                            ));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new ProductWeightBarcodes
                        {
                            id = p.id,
                            epicorePartNo = p.epicorePartNo,
                            fullBarcode = p.fullBarcode,
                            barcode = p.barcode,
                            weightValue = p.weightValue,
                            weightMultiply = p.weightMultiply,

                        });

                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstProductWeightBarcode = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public ProductWeightBarcodes CreateProductWeightBarcode(ProductWeightBarcodes entityEn)
        {
            var _ProductWeightBarcodeEn = new Entity.Core.ProductWeightBarcode();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _ProductWeightBarcodeEn.epicorePartNo = entityEn.epicorePartNo;
                    _ProductWeightBarcodeEn.description = entityEn.description;
                    _ProductWeightBarcodeEn.fullBarcode = entityEn.fullBarcode;
                    _ProductWeightBarcodeEn.barcode = entityEn.barcode;
                    _ProductWeightBarcodeEn.barcodeFromPos = entityEn.barcodeFromPos.ToString();
                    _ProductWeightBarcodeEn.barcodeToPos = entityEn.barcodeToPos.ToString(); ;
                    _ProductWeightBarcodeEn.weightFromPos = entityEn.weightFromPos.ToString();
                    _ProductWeightBarcodeEn.weightToPos = entityEn.weightToPos.ToString();
                    _ProductWeightBarcodeEn.weightMultiply = entityEn.weightMultiply;
                    _ProductWeightBarcodeEn.weightValue = entityEn.weightValue?.ToString() ?? "0.00";

                    //  _ProductWeightBarcodeEn.weightValue = entityEn.weightValue.ToString();
                    _ProductWeightBarcodeEn.barcodeLength = entityEn.barcodeLength.ToString();
                    _ProductWeightBarcodeEn.offSet1 = entityEn.offSet1;
                    _ProductWeightBarcodeEn.offSet2 = entityEn.offSet2;
                    _ProductWeightBarcodeEn.createdBy = entityEn.createdBy;
                    _ProductWeightBarcodeEn.createdDate = entityEn.createdDate;

                    model.ProductWeightBarcode.Add(_ProductWeightBarcodeEn);
                    model.SaveChanges();

                    entityEn.id = _ProductWeightBarcodeEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public ProductWeightBarcodes UpdateProductWeightBarcode(ProductWeightBarcodes entityEn)
        {
            var _ProductWeightBarcodeEn = new Entity.Core.ProductWeightBarcode();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _ProductWeightBarcodeEn = model.ProductWeightBarcode.SingleOrDefault(p => p.id == entityEn.id);
                    if (_ProductWeightBarcodeEn != null)
                    {
                        _ProductWeightBarcodeEn.epicorePartNo = entityEn.epicorePartNo;
                        _ProductWeightBarcodeEn.description = entityEn.description;
                        _ProductWeightBarcodeEn.fullBarcode = entityEn.fullBarcode;
                        _ProductWeightBarcodeEn.barcode = entityEn.barcode;
                        _ProductWeightBarcodeEn.barcodeFromPos = entityEn.barcodeFromPos.ToString();
                        _ProductWeightBarcodeEn.barcodeToPos = entityEn.barcodeToPos.ToString();
                        _ProductWeightBarcodeEn.weightFromPos = entityEn.weightFromPos.ToString();
                        _ProductWeightBarcodeEn.weightToPos = entityEn.weightToPos.ToString();
                        _ProductWeightBarcodeEn.weightMultiply = entityEn.weightMultiply;
                        _ProductWeightBarcodeEn.barcodeLength = entityEn.barcodeLength.ToString();
                        _ProductWeightBarcodeEn.offSet1 = entityEn.offSet1;
                        _ProductWeightBarcodeEn.offSet2 = entityEn.offSet2;
                        _ProductWeightBarcodeEn.weightValue = entityEn.weightValue;
                        _ProductWeightBarcodeEn.modifiedBy = entityEn.modifiedBy;
                        _ProductWeightBarcodeEn.modifiedDate = entityEn.modifiedDate;

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

        public bool DeleteProductWeightBarcode(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _ProductWeightBarcodeEn = model.ProductWeightBarcode.SingleOrDefault(p => p.id == id);
                    if (_ProductWeightBarcodeEn != null)
                    {
                        model.ProductWeightBarcode.Remove(_ProductWeightBarcodeEn);
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
        public List<ProductWeightBarcodes> GetProductWeightBarcodeValue(string epicorePartNo)
        {
            var productweightbarcode = new List<ProductWeightBarcodes>();
            //List<ProductWeightBarcode> entities;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entities = (from pl in model.ProductWeightBarcode
                                    join cpl in model.Product on pl.epicorePartNo equals cpl.partNumber into values
                                    from cpl in values.DefaultIfEmpty()
                                    orderby pl.id
                                    where pl.epicorePartNo.Contains(epicorePartNo)
                                    select new
                                    {
                                        id = pl.id,
                                        epicorePartNo = pl.epicorePartNo,

                                    }).ToList();




                    if (entities.Any())
                    {
                        productweightbarcode = entities.Select(entity => new ProductWeightBarcodes
                        {
                            id = entity.id,
                            epicorePartNo = entity.epicorePartNo,



                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return productweightbarcode;
        }

        public ProductWeightBarcodes GetProductWeightBarcodeById(int id)
        {
            ProductWeightBarcodes _ProductWeightBarcodeEn = new ProductWeightBarcodes();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.ProductWeightBarcode.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _ProductWeightBarcodeEn = new ProductWeightBarcodes
                        {
                            id = entity.id,
                            epicorePartNo = entity.epicorePartNo,
                            description = entity.description,
                            fullBarcode = entity.fullBarcode,
                            barcode = entity.barcode,
                            barcodeFromPos = int.Parse(entity.barcodeFromPos),
                            barcodeToPos = int.Parse(entity.barcodeToPos),
                            weightFromPos = int.Parse(entity.weightFromPos),
                            weightToPos = int.Parse(entity.weightToPos),
                            weightMultiply = entity.weightMultiply,
                            barcodeLength = int.Parse(entity.barcodeLength),
                            offSet1 = entity.offSet1,
                            offSet2 = entity.offSet2,
                            weightValue = entity.weightValue,

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _ProductWeightBarcodeEn;
        }

        public bool IsProductWeightBarcodeExists(ProductWeightBarcodes entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.ProductWeightBarcode.Any(p => p.epicorePartNo == entityEn.epicorePartNo && p.barcode == entityEn.barcode && (entityEn.id == 0 || p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool CheckBarcodeExists(string barcode, string partNo)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _looseConfig = model.LooseBarcodeSetting.FirstOrDefault();
                    var _weighBarcode = new Entity.Core.ProductWeightBarcode();
                    int _startPos = (_looseConfig.barcodeFromPos ?? 0) - 1;
                    int _length = ((_looseConfig.barcodeToPos ?? 0) - (_looseConfig.barcodeFromPos ?? 0)) + 1;

                    string _shortBarcode = barcode.Substring(_startPos, _length);
                    return model.ProductWeightBarcode.Any(p => p.barcode == _shortBarcode && p.epicorePartNo == partNo);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public ProductWeightBarcodeFileUploadResult SaveProductWeightBarcodeBunchList(ProductWeightBarcodeFileUpload entityEn)
        {
            try
            {
                var result = new ProductWeightBarcodeFileUploadResult();

                const int batchSize = 100; // Adjust batch size as needed
                int counter = 0;
                int totalRecords = entityEn.lstProductWeightBarcode.Count;
                var csvData = entityEn.lstProductWeightBarcode;

                if (entityEn.lstProductWeightBarcode != null)
                {
                    while (counter < totalRecords)
                    {
                        using (var dbContext = new HanodaleEntities())
                        {
                            var batch = csvData.Skip(counter).Take(batchSize).ToList();
                            var lst = batch.Select(p => p.epicorePartNo+ "-" +p.barcode).ToList();


                            counter += batchSize;

                            var existingList = dbContext.ProductWeightBarcode.Where(p => lst.Contains(p.epicorePartNo + "-" + p.barcode)).ToList();

                            if (existingList != null)
                            {
                                foreach (var _productWeightBarcodeEn in existingList)
                                {
                                    lst.Remove(_productWeightBarcodeEn.epicorePartNo+"-"+ _productWeightBarcodeEn.barcode);
                                    var item = batch.FirstOrDefault(p => p.epicorePartNo == _productWeightBarcodeEn.epicorePartNo && p.barcode == _productWeightBarcodeEn.barcode);

                                    if (item != null)
                                    {
                                        // Update existing record
                                        _productWeightBarcodeEn.barcode = item.barcode;
                                        _productWeightBarcodeEn.barcodeFromPos = item.barcodeFromPos.ToString();
                                        _productWeightBarcodeEn.barcodeLength = item.barcodeLength.ToString();
                                        _productWeightBarcodeEn.barcodeToPos = item.barcodeToPos.ToString();
                                        // _productWeightBarcodeEn.createdBy = item.createdBy;
                                        //  _productWeightBarcodeEn.createdDate = item.createdDate;
                                        _productWeightBarcodeEn.description = item.description;
                                        _productWeightBarcodeEn.epicorePartNo = item.epicorePartNo;
                                        _productWeightBarcodeEn.fullBarcode = item.fullBarcode;
                                        _productWeightBarcodeEn.offSet1 = item.offSet1;
                                        _productWeightBarcodeEn.offSet2 = item.offSet2;
                                        _productWeightBarcodeEn.weightFromPos = item.weightFromPos.ToString();
                                        _productWeightBarcodeEn.weightMultiply = item.weightMultiply;
                                        _productWeightBarcodeEn.weightToPos = item.weightToPos.ToString();
                                        _productWeightBarcodeEn.weightValue = item.weightValue;

                                        _productWeightBarcodeEn.modifiedDate = item.createdDate;
                                        _productWeightBarcodeEn.modifiedBy = item.createdBy;
                                        //dbContext.Entry(_productWeightBarcodeEn).State = EntityState.Modified;
                                    }
                                }
                            }


                            var newBatchList = batch.Where(p => lst.Contains(p.epicorePartNo + "-" + p.barcode)).Select(item => new ProductWeightBarcode
                            {

                                barcode = item.barcode,
                                barcodeFromPos = item.barcodeFromPos.ToString(),
                                barcodeLength = item.barcodeLength.ToString(),
                                barcodeToPos = item.barcodeToPos.ToString(),
                                description = item.description,
                                epicorePartNo = item.epicorePartNo,
                                fullBarcode = item.fullBarcode,
                                offSet1 = item.offSet1,
                                offSet2 = item.offSet2,
                                weightFromPos = item.weightFromPos.ToString(),
                                weightMultiply = item.weightMultiply,
                                weightToPos = item.weightToPos.ToString(),
                                weightValue = item.weightValue,
                                createdDate = item.createdDate,
                                createdBy = item.createdBy,

                            });

                            foreach (var newRecord in newBatchList)
                            {
                                dbContext.ProductWeightBarcode.Add(newRecord);
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


        //public ProductWeightBarcodeFileUploadResult SaveProductWeightBarcodeBunchList(ProductWeightBarcodeFileUpload entityEn)
        //{
        //    try
        //    {
        //        using (HanodaleEntities model = new HanodaleEntities())
        //        {
        //            if (entityEn.lstProductWeightBarcode != null)
        //            {
        //                var result = new ProductWeightBarcodeFileUploadResult();
        //                result.lstItem = new List<ProductWeightBarcodeFileUploadResultItem>();

        //                if (entityEn.lstProductWeightBarcode != null)
        //                {

        //                    foreach (var item in entityEn.lstProductWeightBarcode)
        //                    {
        //                        bool isNew = false;

        //                        var _productWeightBarcodeEn = model.ProductWeightBarcode.SingleOrDefault(p => p.epicorePartNo == item.epicorePartNo && p.barcode == item.barcode);
        //                        if (_productWeightBarcodeEn == null)
        //                        {
        //                            _productWeightBarcodeEn = new ProductWeightBarcode();
        //                            isNew = true;
        //                        }

        //                        _productWeightBarcodeEn.barcode = item.barcode;
        //                        _productWeightBarcodeEn.barcodeFromPos = item.barcodeFromPos.ToString();
        //                        _productWeightBarcodeEn.barcodeLength = item.barcodeLength.ToString();
        //                        _productWeightBarcodeEn.barcodeToPos = item.barcodeToPos.ToString();
        //                        // _productWeightBarcodeEn.createdBy = item.createdBy;
        //                        //  _productWeightBarcodeEn.createdDate = item.createdDate;
        //                        _productWeightBarcodeEn.description = item.description;
        //                        _productWeightBarcodeEn.epicorePartNo = item.epicorePartNo;
        //                        _productWeightBarcodeEn.fullBarcode = item.fullBarcode;
        //                        _productWeightBarcodeEn.offSet1 = item.offSet1;
        //                        _productWeightBarcodeEn.offSet2 = item.offSet2;
        //                        _productWeightBarcodeEn.weightFromPos = item.weightFromPos.ToString();
        //                        _productWeightBarcodeEn.weightMultiply = item.weightMultiply;
        //                        _productWeightBarcodeEn.weightToPos = item.weightToPos.ToString();
        //                        _productWeightBarcodeEn.weightValue = item.weightValue;

        //                        if (isNew)
        //                        {
        //                            _productWeightBarcodeEn.createdDate = item.createdDate;
        //                            _productWeightBarcodeEn.createdBy = item.createdBy;
        //                            model.ProductWeightBarcode.Add(_productWeightBarcodeEn);
        //                        }
        //                        else
        //                        {
        //                            _productWeightBarcodeEn.modifiedDate = item.createdDate;
        //                            _productWeightBarcodeEn.modifiedBy = item.createdBy;

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
