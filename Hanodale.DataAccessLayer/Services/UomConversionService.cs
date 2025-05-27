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
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Hanodale.DataAccessLayer.Services
{
    public class UomConversionService : BaseService, IUomConversionService
    {
        public UomConversionDetails GetUomConversionBySearch(DatatableFilters entityFilter)
        {
            UomConversionDetails _result = new UomConversionDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    var partNum = model.Product.Where(p => p.id == entityFilter.masterRecord_Id).Select(p => p.partNumber).FirstOrDefault();
                    //get total record

                    var query = model.UomConv.Where(p => p.partNum.Contains(partNum));

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                           p.company.Contains(entityFilter.search)
                        || p.uomCode.Contains(entityFilter.search)
                        || p.convFactor.Contains(entityFilter.search)
                        || p.uniqueField.Contains(entityFilter.search)
                        || p.convOperator.Contains(entityFilter.search)
                       ));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new UomConversions
                        {

                            id = p.id,
                            company = p.company,
                            partNum = p.partNum,
                            uomCode = p.uomCode,
                            convFactor = p.convFactor,
                            uniqueField = p.uniqueField,
                            convOperator = p.convOperator,
                        });
                   
                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstUomConversion = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public UomConversions CreateUomConversion(UomConversions entityEn)
        {
            var _uomConversionEn = new UomConv();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Uom Conv

                    _uomConversionEn.company = entityEn.company;
                    _uomConversionEn.partNum = entityEn.partNum;
                    _uomConversionEn.uomCode = entityEn.uomCode;
                    _uomConversionEn.convFactor = entityEn.convFactor;
                    _uomConversionEn.uniqueField = entityEn.uniqueField;
                    _uomConversionEn.convOperator = entityEn.convOperator;
                  
                    model.UomConv.Add(_uomConversionEn);
                    model.SaveChanges();

                    entityEn.id = _uomConversionEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public UomConversions UpdateUomConversion(UomConversions entityEn)
        {
            var _uomConversionEn = new UomConv();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _uomConversionEn = model.UomConv.SingleOrDefault(p => p.id == entityEn.id);
                    if (_uomConversionEn != null)
                    {

                        _uomConversionEn.company = entityEn.company;
                        _uomConversionEn.partNum = entityEn.partNum;
                        _uomConversionEn.uomCode = entityEn.uomCode;
                        _uomConversionEn.convFactor = entityEn.convFactor;
                        _uomConversionEn.uniqueField = entityEn.uniqueField;
                        _uomConversionEn.convOperator = entityEn.convOperator;
                      
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

        public bool DeleteUomConversion(int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _uomConversionEn = model.UomConv.SingleOrDefault(p => p.id == id);

                    if (_uomConversionEn != null)
                    {
                        model.UomConv.Remove(_uomConversionEn);
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

        public UomConversions GetUomConversionById(int id)
        {
            var _uomConversionEn = new UomConversions();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.UomConv.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _uomConversionEn = new UomConversions
                        {
                            id = entity.id,

                            //CompanyProfile_Id = entity.CompanyProfile_Id,
                            company = entity.company,
                            partNum = entity.partNum,
                            uomCode = entity.uomCode,
                            convFactor = entity.convFactor,
                            uniqueField = entity.uniqueField,
                            convOperator = entity.convOperator,

                         
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _uomConversionEn;
        }

        public bool IsUomConversionExists(UomConversions entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.UomConv.Any(p => p.uomCode == entityEn.uomCode && (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
    }
}
