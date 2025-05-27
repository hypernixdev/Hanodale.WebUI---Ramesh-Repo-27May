using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Runtime;
using System.ServiceModel;
using System.Xml.Linq;

namespace Hanodale.DataAccessLayer.Services
{
    public class LooseConversionService : BaseService, ILooseConversionService
    {
        public LooseConversionDetails GetLooseConversionBySearch(DatatableFilters entityFilter)
        {
            LooseConversionDetails _result = new LooseConversionDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    var query = model.LooseConversion.AsNoTracking().Where(p => true);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.ProductCarton.epicorPartNo.ToString().Contains(entityFilter.search)
                                 || p.ProductCarton.barcode.Contains(entityFilter.search)
                                || p.createdBy.Contains(entityFilter.search)
                                || p.createdDate.ToString().Contains(entityFilter.search)

                            ));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new LooseConversions
                        {
                            id = p.id,
                            epicorPartNo = p.ProductCarton.epicorPartNo,
                            barcode = p.ProductCarton.barcode,
                            createdBy = p.createdBy,
                            createdDate = p.createdDate,

                        });

                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstLooseConversion = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public LooseConversions CreateLooseConversion(LooseConversions entityEn)
        {
            var _LooseConversionEn = new Entity.Core.LooseConversion();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _LooseConversionEn.productCarton_Id = entityEn.productCarton_Id;
                    _LooseConversionEn.postingStatus = entityEn.postingStatus;
                    _LooseConversionEn.createdBy = entityEn.createdBy;
                    _LooseConversionEn.createdDate = entityEn.createdDate;
                    model.LooseConversion.Add(_LooseConversionEn);
                    model.SaveChanges();

                    entityEn.id = _LooseConversionEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public bool CreateLooseConversionItem(List<LooseConversionItems> itemsList)
        {
            try
            {
                using (var model = new HanodaleEntities())
                {
                    foreach (var entityEn in itemsList)
                    {
                        // Convert LooseConversionItems to Entity.Core.LooseConversionItem
                        var _LooseConversionEn = new Entity.Core.LooseConversionItem
                        {
                            looseConversion_Id = entityEn.looseConversion_Id,
                            barcode = entityEn.barcode,
                            LooseQty = entityEn.LooseQty,
                            RunningBalance = entityEn.RunningBalance,
                        };
                      
                        // Additional logic to set weighScaleBarcode_Id
                        string _partNo = "";
                        string _partName = "";
                        var _carton = model.LooseConversion.FirstOrDefault(a => a.id == _LooseConversionEn.looseConversion_Id).ProductCarton;
                        if (_carton != null)
                        {
                            _partNo = _carton.epicorPartNo ?? "";
                        }

                        var _product = model.Product.FirstOrDefault(a => a.partNumber == _partNo);
                        if (_product != null)
                        {
                            _partName = _product.description ?? "";
                        }

                        var _weighBarcode = new Entity.Core.ProductWeightBarcode();
                        if (!string.IsNullOrEmpty(_partNo))
                        {
                            var _looseConfig = model.LooseBarcodeSetting.FirstOrDefault();
                            int _startPos = (_looseConfig.barcodeFromPos ?? 0) - 1;
                            int _length = ((_looseConfig.barcodeToPos ?? 0) - (_looseConfig.barcodeFromPos ?? 0)) + 1;

                            _weighBarcode.barcode = entityEn.barcode.Substring(_startPos, _length);
                            _weighBarcode.fullBarcode = entityEn.barcode;
                            _weighBarcode.epicorePartNo = _partNo;
                            _weighBarcode.description = _partName;
                            _weighBarcode.weightValue = entityEn.LooseQty.ToString();
                            _weighBarcode.barcodeLength = _looseConfig.barcodeLength.ToString();
                            _weighBarcode.weightFromPos = _looseConfig.weightFromPos.ToString();
                            _weighBarcode.weightToPos = _looseConfig.weightToPos.ToString();
                            _weighBarcode.barcodeFromPos = _looseConfig.barcodeFromPos.ToString();
                            _weighBarcode.barcodeToPos = _looseConfig.barcodeToPos.ToString();
                            _weighBarcode.weightMultiply = _looseConfig.weightMutiply;
                            _weighBarcode.createdBy = entityEn.createdBy;
                            _weighBarcode.createdDate = DateTime.Now;
                            model.ProductWeightBarcode.Add(_weighBarcode);
                            model.SaveChanges();
                            _LooseConversionEn.weighScaleBarcode_Id = _weighBarcode.id;
                        }
                        
                        // Add each item to the context without saving yet

                        model.LooseConversionItem.Add(_LooseConversionEn);
                    }

                    // Save all changes in one transaction
                    model.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it accordingly
                throw new FaultException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /*
        public LooseConversionItems CreateLooseConversionItem(LooseConversionItems entityEn)
        {
            var _LooseConversionEn = new Entity.Core.LooseConversionItem();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _LooseConversionEn.looseConversion_Id = entityEn.looseConversion_Id;
                    _LooseConversionEn.barcode = entityEn.barcode;
                    _LooseConversionEn.LooseQty = entityEn.LooseQty;
                    _LooseConversionEn.RunningBalance = entityEn.RunningBalance;

                    //_LooseConversionEn.weighScaleBarcode_Id = entityEn.weighScaleBarcode_Id;

                    model.LooseConversionItem.Add(_LooseConversionEn);

                    string _partNo = "";
                    string _partName = "";
                    var _carton = model.LooseConversion.FirstOrDefault(a => a.id == entityEn.looseConversion_Id).ProductCarton;
                    if (_carton != null)
                    {
                        _partNo = _carton.epicorPartNo ?? "";
                    }

                    var _product = model.Product.FirstOrDefault(a => a.partNumber == _partNo);
                    if (_product != null)
                    {
                        _partName = _product.description ?? "";
                    }

                    if (!string.IsNullOrEmpty(_partNo))
                    {
                        var _looseConfig = model.LooseBarcodeSetting.FirstOrDefault();
                        var _weighBarcode = new Entity.Core.ProductWeightBarcode();
                        int _startPos = (_looseConfig.barcodeFromPos ?? 0) - 1;
                        int _length = ((_looseConfig.barcodeToPos ?? 0) - (_looseConfig.barcodeFromPos ?? 0)) + 1;

                        _weighBarcode.barcode = entityEn.barcode.Substring(_startPos, _length);
                        _weighBarcode.fullBarcode = entityEn.barcode;
                        _weighBarcode.epicorePartNo = _partNo;
                        _weighBarcode.description = _partName;
                        _weighBarcode.weightValue = entityEn.LooseQty.ToString();
                        _weighBarcode.barcodeLength = _looseConfig.barcodeLength.ToString();
                        _weighBarcode.weightFromPos = _looseConfig.weightFromPos.ToString();
                        _weighBarcode.weightToPos = _looseConfig.weightToPos.ToString();
                        _weighBarcode.barcodeFromPos = _looseConfig.barcodeFromPos.ToString();
                        _weighBarcode.barcodeToPos = _looseConfig.barcodeToPos.ToString();
                        _weighBarcode.weightMultiply = _looseConfig.weightMutiply;
                        _weighBarcode.createdBy = entityEn.createdBy;
                        _weighBarcode.createdDate = DateTime.Now;
                        model.ProductWeightBarcode.Add(_weighBarcode);
                        _LooseConversionEn.weighScaleBarcode_Id = _weighBarcode.id;

                    }

                    model.SaveChanges();
                    entityEn.id = _LooseConversionEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }
      */
        public LooseConversions UpdateLooseConversion(LooseConversions entityEn)
        {
            var _LooseConversionEn = new Entity.Core.LooseConversion();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _LooseConversionEn = model.LooseConversion.SingleOrDefault(p => p.id == entityEn.id);
                    if (_LooseConversionEn != null)
                    {
                        _LooseConversionEn.productCarton_Id = entityEn.productCarton_Id;
                        _LooseConversionEn.createdBy = entityEn.createdBy;
                        _LooseConversionEn.createdDate = entityEn.createdDate;

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

        public decimal CalculateLooseQty(string barcode)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var setting = model.LooseBarcodeSetting.FirstOrDefault(s => s.id == 1);
                    if (setting != null)
                    {

                        int fromPos = setting.barcodeFromPos ??0;
                        int toPos = setting.barcodeToPos ?? 0;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return 0;
        }
        
    public bool DeleteLooseConversion(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _LooseConversionEn = model.LooseConversion.SingleOrDefault(p => p.id == id);
                    if (_LooseConversionEn != null)
                    {
                        model.LooseConversion.Remove(_LooseConversionEn);
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

        public LooseConversions GetLooseConversionById(int id)
        {
             LooseConversions _LooseConversionEn = new LooseConversions();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Retrieve the main LooseConversion and related ProductCarton data
                    var entity = (from lc in model.LooseConversion
                                  join pc in model.ProductCarton on lc.productCarton_Id equals pc.id
                                  where lc.id == id
                                  select new
                                  {
                                      lc.id,
                                      lc.productCarton_Id,
                                      lc.createdBy,
                                      lc.createdDate,
                                      barcode = pc.barcode
                                  }).SingleOrDefault();

                    // Check if the entity is found
                    if (entity != null)
                    {
                        // Retrieve the associated LooseConversionItems in a separate query
                        var looseConversionItems = model.LooseConversionItem
                            .Where(lci => lci.looseConversion_Id == entity.id)
                            .Select(item => new
                            {
                                item.id,
                                item.looseConversion_Id,
                                item.LooseQty,
                                item.barcode,
                                item.RunningBalance,
                            }).ToList();

                        // Map the retrieved data into the LooseConversions object
                        _LooseConversionEn = new LooseConversions
                        {
                            id = entity.id,
                            productCarton_Id = entity.productCarton_Id ?? 0,
                            createdBy = entity.createdBy,
                            createdDate = entity.createdDate,
                            barcode = entity.barcode,
                            // Populate the LooseConversionItems list with the fetched items
                            LooseConversionItems = looseConversionItems.Select(item => new LooseConversionItems
                            {
                                id = item.id,
                                looseConversion_Id = item.looseConversion_Id,
                                barcode=item.barcode,
                                LooseQty = item.LooseQty,
                                RunningBalance = item.RunningBalance
                            }).ToList()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
            return _LooseConversionEn;
        }
        public LooseBarcodeSettings GetLooseBarcodeSettingById(int id)
        {
            LooseBarcodeSettings _looseBarcodeSetting = new LooseBarcodeSettings();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.LooseBarcodeSetting.SingleOrDefault();
                    if (entity != null)
                    {
                        _looseBarcodeSetting = new LooseBarcodeSettings
                        {
                            id = entity.id,
                            barcodeFromPos = entity.barcodeFromPos,
                            barcodeToPos = entity.barcodeToPos,
                            weightFromPos = entity.weightFromPos,
                            weightToPos = entity.weightToPos,
                            weightMutiply = entity.weightMutiply,
                            barcodeLength = entity.barcodeLength,

                      
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _looseBarcodeSetting;
        }
        public bool IsLooseConversionExists(LooseConversions entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.LooseConversion.Any(p => p.productCarton_Id == entityEn.productCarton_Id && (entityEn.id == 0 || p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public LooseConversionItems UpdateLooseConversionItem(LooseConversionItems entityEn)
        {
            throw new NotImplementedException();
        }
    }
}
