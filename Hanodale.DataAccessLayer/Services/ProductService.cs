using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain.DTOs;
using Hanodale.Domain.DTOs.Order;
using Hanodale.Entity.Core;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Hanodale.DataAccessLayer.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductDetails GetProductBySearch(DatatableFilters entityFilter, object filterEntity)
        {
            ProductDetails _result = new ProductDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();
                    var filter = new Products();
                    if (filterEntity != null)
                        filter = (Products)filterEntity;


                    //get total record-

                    var query = model.Product.AsNoTracking().Where(p => true);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(filter.part_Id))
                    {
                        query = query.Where(p => (p.partNumber.Contains(filter.part_Id)));
                    }
                    if (!string.IsNullOrEmpty(filter.code_Id))
                    {
                        query = query.Where(p => (p.code.Contains(filter.code_Id)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchDescription))
                    {
                        query = query.Where(p => (p.description.Contains(filter.searchDescription)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchGrupDescription))
                    {
                        query = query.Where(p => (p.ProdGrup_Description.Contains(filter.searchGrupDescription)));

                    }
                    if (!string.IsNullOrEmpty(filter.searchPartClassDescription))
                    {
                        query = query.Where(p => (p.PartClass_Description.Contains(filter.searchPartClassDescription)));

                    }
                    if (!string.IsNullOrEmpty(filter.searchCountryDescription))
                    {
                        query = query.Where(p => (p.Country_Description.Contains(filter.searchCountryDescription)));

                    }

                  //  query = query.Where(p => p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00");
                    query = query.Where(p => p.Company.Trim() == "LUCKY00");

                    query = query.Where(p => p.inActive == false);
                    //Filtered count
                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.partNumber.Contains(entityFilter.search)
                                || p.description.Contains(entityFilter.search)
                                || p.code.Contains(entityFilter.search)
                                || p.Country_Description.Contains(entityFilter.search)
                                || p.Part_IUM.Contains(entityFilter.search)


                            ));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new Products
                        {
                            id = p.id,
                            partNumber = p.partNumber,
                            description = p.description,
                            code = p.code,
                            prodGrup_Description = p.ProdGrup_Description,
                            part_ClassID = p.Part_ClassID,
                            partClass_Description = p.PartClass_Description,
                            country_Description = p.Country_Description,
                            uomClass_DefUomCode = p.UOMClass_DefUomCode,
                            part_SalesUM = p.Part_SalesUM,
                            part_IUM = p.Part_IUM,
                            uomClass_BaseUOMCode = p.UOMClass_BaseUOMCode,
                            country_CountryNum = p.Country_CountryNum ?? 0,
                            uomClass_ClassType = p.UOMClass_ClassType,
                            uomClass_Description = p.UOMClass_Description,
                            unitPrice = p.UnitPrice ?? 0,

                        });

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstProduct = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }
        public List<Products> GetProductList(Products entityEn)
        {
            try
            {
                var _result = new List<Products>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityEn == null)
                        entityEn = new Products();

                 //   _result = model.Product.Where(p => (p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00") && p.inActive == false).OrderByDescending(p => p.id)
                         _result = model.Product.Where(p => (p.Company.Trim() == "LUCKY00" ) && p.inActive == false).OrderByDescending(p => p.id)
                        .Select(p => new Products
                        {
                            id = p.id,
                            partNumber = p.partNumber,
                            code = p.code,
                            description = p.description,
                        }).ToList();

                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
        public Products CreateProduct(Products entityEn)
        {
            var _ProductEn = new Product();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Product

                    _ProductEn.partNumber = entityEn.partNumber;
                    _ProductEn.description = entityEn.description;
                    _ProductEn.code = entityEn.code;
                    model.Product.Add(_ProductEn);
                    model.SaveChanges();

                    entityEn.id = _ProductEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public Products UpdateProduct(Products entityEn)
        {
            var _ProductEn = new Product();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update Product
                    _ProductEn = model.Product.SingleOrDefault(p => p.id == entityEn.id);
                    if (_ProductEn != null)
                    {
                        _ProductEn.partNumber = entityEn.partNumber;
                        _ProductEn.description = entityEn.description;
                        _ProductEn.code = entityEn.code;

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

        public bool DeleteProduct(int id)
        {
            var result = new Products();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _ProductEn = model.Product.SingleOrDefault(p => p.id == id);

                    if (_ProductEn != null)
                    {
                        model.Product.Remove(_ProductEn);
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

        public Products GetProductById(int id)
        {
            Products _ProductEn = new Products();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //var entity = model.Product.Where(p => (p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00") && p.inActive == false).SingleOrDefault(p => p.id == id);
                    var entity = model.Product.Where(p => (p.Company.Trim() == "LUCKY00") && p.inActive == false).SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _ProductEn = new Products
                        {
                            id = entity.id,
                            partNumber = entity.partNumber,
                            description = entity.description,
                            code = entity.code,
                            prodGrup_Description = entity.ProdGrup_Description,
                            part_ClassID = entity.Part_ClassID,
                            partClass_Description = entity.PartClass_Description,
                            country_Description = entity.Country_Description,
                            uomClass_DefUomCode = entity.UOMClass_DefUomCode,  // Full UOM
                            part_SalesUM = entity.Part_SalesUM,
                            part_IUM = entity.Part_IUM,
                            uomClass_BaseUOMCode = entity.UOMClass_BaseUOMCode,
                            country_CountryNum = entity.Country_CountryNum ?? 0,
                            uomClass_ClassType = entity.UOMClass_ClassType,
                            uomClass_Description = entity.UOMClass_Description,
                            unitPrice = entity.UnitPrice ?? 0,

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _ProductEn;
        }

        //public Products GetProductByPartNum(string partNum)
        //{
        //    Products _ProductEn = new Products();
        //    try
        //    {
        //        using (HanodaleEntities model = new HanodaleEntities())
        //        {
        //            var defaultUom = model.OrderUOM.Where(p => p.defaultLooseQty == true).FirstOrDefault();
        //            //var entity = model.Product
        //            //    .Where(p => p.Company == "LUCKY00 " || p.Company == "LUCKY00")
        //            //    .SingleOrDefault(p => p.partNumber == partNum);
        //            var entity = (from p in model.Product
        //                                      from s in model.StockBalanceView
        //                                          .Where(s => s.company == p.Company && s.partNum == p.partNumber && s.uom == (p.Part_ShortChar03_LooseUOM == "0" || p.Part_ShortChar03_LooseUOM == "" ? p.UOMClass_BaseUOMCode : p.Part_ShortChar03_LooseUOM))
        //                                          .DefaultIfEmpty()
        //                                      where p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00"
        //                                      where p.partNumber == partNum
        //                                      where p.inActive == false
        //                                      select new
        //                                      {
        //                                          Product = p,
        //                                          StockBalance = ss
        //                                      }).SingleOrDefault();
        //            // To-Do: Currently Part_SalesUM is used for base uom
        //            if (entity != null)
        //            {
        //                _ProductEn = new Products
        //                {
        //                    id = entity.Product.id,
        //                    partNumber = entity.Product.partNumber,
        //                    description = entity.Product.description,
        //                    code = entity.Product.code,
        //                    prodGrup_Description = entity.Product.ProdGrup_Description,
        //                    part_ClassID = entity.Product.Part_ClassID,
        //                    partClass_Description = entity.Product.PartClass_Description,
        //                    country_Description = entity.Product.Country_Description,
        //                    uomClass_DefUomCode = entity.Product.UOMClass_DefUomCode,
        //                    part_SalesUM = entity.Product.Part_SalesUM, // entity.Product.Part_ShortChar03_LooseUOM == "0" ? entity.Product.UOMClass_BaseUOMCode : entity.Product.Part_ShortChar03_LooseUOM, // Loose or Base UOM
        //                    FullUom = entity.Product.UOMClass_DefUomCode, // Full UOM
        //                    uomClass_BaseUOMCode = entity.Product.UOMClass_BaseUOMCode,
        //                    country_CountryNum = entity.Product.Country_CountryNum ?? 0,
        //                    uomClass_ClassType = entity.Product.UOMClass_ClassType,
        //                    uomClass_Description = entity.Product.UOMClass_Description,
        //                    unitPrice = entity.Product.UnitPrice ?? 0,
        //                    isCube = entity.Product.Part_CheckBox04_Cube,
        //                    isSlice = entity.Product.Part_CheckBox03_Slice,
        //                    isStrip = entity.Product.Part_CheckBox13_Strip,
        //                    allowVaryWeight = entity.Product.AllowSellingVaryWeight ?? false ? "Yes" : "No",
        //                    looseUom = entity.Product.Part_ShortChar03_LooseUOM == "0" || entity.Product.Part_ShortChar03_LooseUOM == "" ? "" : entity.Product.Part_ShortChar03_LooseUOM,
        //                    standardFullQty = entity.Product.Part_CheckBox05_Standard_Full_Qty ?? false ? "Yes" : "No",
        //                    defaultUomId = defaultUom.id,
        //                    AllowLooseSelling = entity.Product.Part_CheckBox01_Allow_Selling_Loose ?? false,
        //                    remainingQty = entity.StockBalance?.remainingQty,
        //                    conversionFactor = entity.Product.conversionFactor ?? 1
        //                };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.InnerException.InnerException.Message);
        //    }
        //    return _ProductEn;
        //}


        //public Products GetProductByPartNum(string partNum)
        //{
        //    Products _ProductEn = new Products();
        //    try
        //    {
        //        using (HanodaleEntities model = new HanodaleEntities())
        //        {
        //            var defaultUom = model.OrderUOM.Where(p => p.defaultLooseQty == true).FirstOrDefault();

        //            var entity = (from p in model.Product
        //                          from s in model.StockBalanceView
        //                              .Where(s => s.company.Trim() == p.Company.Trim()
        //                                  && s.partNum == p.partNumber
        //                                  && s.uom == (
        //                                      (p.Part_ShortChar03_LooseUOM == null || p.Part_ShortChar03_LooseUOM == "0" || p.Part_ShortChar03_LooseUOM == "")
        //                                      ? p.UOMClass_BaseUOMCode
        //                                      : p.Part_ShortChar03_LooseUOM
        //                                  )
        //                              )
        //                              .DefaultIfEmpty()
        //                          where p.Company.Trim() == "LUCKY00"
        //                          where p.partNumber == partNum
        //                          where p.inActive == false
        //                          select new
        //                          {
        //                              Product = p,
        //                              StockBalance = s
        //                          }).SingleOrDefault();



        //            if (entity != null)
        //            {
        //                _ProductEn = new Products
        //                {
        //                    id = entity.Product.id,
        //                    partNumber = entity.Product.partNumber,
        //                    description = entity.Product.description,
        //                    code = entity.Product.code,
        //                    prodGrup_Description = entity.Product.ProdGrup_Description,
        //                    part_ClassID = entity.Product.Part_ClassID,
        //                    partClass_Description = entity.Product.PartClass_Description,
        //                    country_Description = entity.Product.Country_Description,
        //                    uomClass_DefUomCode = entity.Product.UOMClass_DefUomCode,
        //                    part_SalesUM = entity.Product.Part_SalesUM,
        //                    FullUom = entity.Product.UOMClass_DefUomCode,
        //                    uomClass_BaseUOMCode = entity.Product.UOMClass_BaseUOMCode,
        //                    country_CountryNum = entity.Product.Country_CountryNum ?? 0,
        //                    uomClass_ClassType = entity.Product.UOMClass_ClassType,
        //                    uomClass_Description = entity.Product.UOMClass_Description,
        //                    unitPrice = entity.Product.UnitPrice ?? 0,
        //                    isCube = entity.Product.Part_CheckBox04_Cube,
        //                    isSlice = entity.Product.Part_CheckBox03_Slice,
        //                    isStrip = entity.Product.Part_CheckBox13_Strip,
        //                    allowVaryWeight = entity.Product.AllowSellingVaryWeight ?? false ? "Yes" : "No",
        //                    looseUom = string.IsNullOrEmpty(entity.Product.Part_ShortChar03_LooseUOM) || entity.Product.Part_ShortChar03_LooseUOM == "0" ? "" : entity.Product.Part_ShortChar03_LooseUOM,
        //                    standardFullQty = entity.Product.Part_CheckBox05_Standard_Full_Qty ?? false ? "Yes" : "No",
        //                    defaultUomId = defaultUom?.id ?? 0,
        //                    AllowLooseSelling = entity.Product.Part_CheckBox01_Allow_Selling_Loose ?? false,
        //                    remainingQty = entity.StockBalance?.remainingQty,
        //                    conversionFactor = entity.Product.conversionFactor ?? 1
        //                };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
        //    }
        //    return _ProductEn;
        //}

        public Products GetProductByPartNum(string partNum)
        {
            Products _ProductEn = new Products();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var defaultUom = model.OrderUOM.FirstOrDefault(p => p.defaultLooseQty == true);

                    var entity = (from p in model.Product
                                  from s in model.StockBalanceView
                                      .Where(s => //s.company.Trim() == p.Company.Trim() &&
                                           s.partNum == p.partNumber
                                          && (
                                              s.uom == (string.IsNullOrEmpty(p.Part_ShortChar03_LooseUOM) || p.Part_ShortChar03_LooseUOM == "0"
                                                  ? p.UOMClass_BaseUOMCode
                                                  : p.Part_ShortChar03_LooseUOM)
                                              || s.uom == p.UOMClass_BaseUOMCode
                                          )
                                      )
                                      .DefaultIfEmpty()
                                  where p.Company.Trim() == "LUCKY00"
                                  where p.partNumber == partNum
                                  where p.inActive == false
                                  select new
                                  {
                                      Product = p,
                                      StockBalance = s
                                  }).FirstOrDefault();

                    if (entity != null)
                    {
                        _ProductEn = new Products
                        {
                            id = entity.Product.id,
                            partNumber = entity.Product.partNumber,
                            description = entity.Product.description,
                            code = entity.Product.code,
                            prodGrup_Description = entity.Product.ProdGrup_Description,
                            part_ClassID = entity.Product.Part_ClassID,
                            partClass_Description = entity.Product.PartClass_Description,
                            country_Description = entity.Product.Country_Description,
                            uomClass_DefUomCode = entity.Product.UOMClass_DefUomCode,
                            part_SalesUM = entity.Product.Part_SalesUM,
                            FullUom = entity.Product.UOMClass_DefUomCode,
                            uomClass_BaseUOMCode = entity.Product.UOMClass_BaseUOMCode,
                            country_CountryNum = entity.Product.Country_CountryNum ?? 0,
                            uomClass_ClassType = entity.Product.UOMClass_ClassType,
                            uomClass_Description = entity.Product.UOMClass_Description,
                            unitPrice = entity.Product.UnitPrice ?? 0,
                            isCube = entity.Product.Part_CheckBox04_Cube,
                            isSlice = entity.Product.Part_CheckBox03_Slice,
                            isStrip = entity.Product.Part_CheckBox13_Strip,
                            allowVaryWeight = entity.Product.AllowSellingVaryWeight ?? false ? "Yes" : "No",
                            looseUom = string.IsNullOrEmpty(entity.Product.Part_ShortChar03_LooseUOM) || entity.Product.Part_ShortChar03_LooseUOM == "0" ? "" : entity.Product.Part_ShortChar03_LooseUOM,
                            standardFullQty = entity.Product.Part_CheckBox05_Standard_Full_Qty ?? false ? "Yes" : "No",
                            defaultUomId = defaultUom?.id ?? 0,
                            AllowLooseSelling = entity.Product.Part_CheckBox01_Allow_Selling_Loose ?? false,
                            remainingQty = entity.StockBalance?.remainingQty,
                            conversionFactor = entity.Product.conversionFactor ?? 1
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
            return _ProductEn;
        }

        public ProductStockBalance GetProductStockBalance(string partNum, string uom)
        {
            ProductStockBalance _ProductEn = new ProductStockBalance();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = (from p in model.Product
                                  from s in model.StockBalanceView
                                      .Where(s => //s.company == p.Company && 
                                      s.partNum == p.partNumber && s.uom == uom)
                                      .DefaultIfEmpty()
                                  //where p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00"
                                  where p.Company.Trim() == "LUCKY00" 
                                  where p.partNumber == partNum
                                  select new
                                  {
                                      Product = p,
                                      StockBalance = s
                                  }).SingleOrDefault();
                    // To-Do: Currently Part_SalesUM is used for base uom
                    if (entity != null)
                    {
                        _ProductEn = new ProductStockBalance
                        {
                            id = entity.Product.id,
                            partNumber = entity.Product.partNumber,
                            uom = uom,
                            remainingQty = entity.StockBalance?.remainingQty,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _ProductEn;
        }

        public List<ProductStockBalance> GetProductStockBalanceList(List<string> partNumbers)
        {
            List<ProductStockBalance> _ProductEn = new List<ProductStockBalance>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.StockBalanceView
                        .Where(p => partNumbers.Contains(p.partNum))
                        .Select(p => new ProductStockBalance
                        {
                            partNumber = p.partNum,
                            uom = p.uom,
                            remainingQty = p.remainingQty,
                        }).ToList();

                    _ProductEn = query;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
            return _ProductEn;
        }


        public ProductDetails GetProductList(ProductDatatableFilter param)
        {
            try
            {
                var _result = new ProductDetails();
                _result.recordDetails = new RecordDetails();
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.Product.AsQueryable();
                    //query = query.Where(p => p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00");
                    query = query.Where(p => p.Company.Trim() == "LUCKY00");
                    query = query.Where(p => p.inActive == false);
                    _result.recordDetails.totalRecords = query.Count();
                    var defaultUom = model.OrderUOM.Where(p => p.defaultLooseQty == true).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(param.PartNum))
                    {
                        query = query.Where(p => p.partNumber.Contains(param.PartNum));
                    }

                    if (!string.IsNullOrWhiteSpace(param.Code))
                    {
                        query = query.Where(p => p.code.Contains(param.Code));
                    }

                    if (!string.IsNullOrWhiteSpace(param.origin))
                    {
                        query = query.Where(p => p.Country_Description.Contains(param.origin));
                    }

                    if (!string.IsNullOrWhiteSpace(param.brand))
                    {
                        query = query.Where(p => p.UD04_Character01_Item_Brand.Contains(param.brand));
                    }

                    if (!string.IsNullOrWhiteSpace(param.temperature))
                    {
                        query = query.Where(p => p.Part_ShortChar01_Temprature_Type_Code.Contains(param.temperature));
                    }

                    if (!string.IsNullOrWhiteSpace(param.Description))
                    {
                        query = query.Where(p => p.description
                        .ToLower()
                        .Replace("/", "")
                        .Replace("-", "")
                        .Contains(param.Description
                            .ToLower()
                            .Replace("/", "")
                            .Replace("-", "")
                        ));

                    }

                    if (!string.IsNullOrWhiteSpace(param.PartGroup))
                    {
                        query = query.Where(p => p.ProdGrup_Description.Contains(param.PartGroup));
                    }

                    if (!string.IsNullOrWhiteSpace(param.PartClass))
                    {
                        query = query.Where(p => p.PartClass_Description.Contains(param.PartClass));
                    }
                    _result.recordDetails.totalDisplayRecords = query.Count();
                    // Final query with join, ordering, and paging
                    // To-Do: Currently Part_SalesUM is used for base uom.
                    var finalQuery = from p in query
                                     let stock = model.StockBalanceView
       .Where(s => //s.company == p.Company &&
            s.partNum == p.partNumber
           && s.uom == (
               (string.IsNullOrEmpty(p.Part_ShortChar03_LooseUOM) || p.Part_ShortChar03_LooseUOM == "0")
               ? p.UOMClass_BaseUOMCode
               : p.Part_ShortChar03_LooseUOM
           )
       )
       .DefaultIfEmpty()

                                     from s in stock
                                     orderby p.id descending
                                     select new Products
                                     {
                                         id = p.id,
                                         partNumber = p.partNumber,
                                         code = p.code,
                                         description = p.description,
                                         prodGrup_Description = p.ProdGrup_Description,
                                         part_ClassID = p.Part_ClassID,
                                         partClass_Description = p.PartClass_Description,
                                         country_Description = p.Country_Description,
                                         uomClass_DefUomCode = p.UOMClass_DefUomCode,
                                         part_SalesUM =  p.Part_SalesUM, // p.Part_ShortChar03_LooseUOM == "0" ? p.UOMClass_BaseUOMCode : p.Part_ShortChar03_LooseUOM,
                                         part_IUM = p.Part_IUM,
                                         FullUom = p.UOMClass_DefUomCode,
                                         uomClass_BaseUOMCode = p.UOMClass_BaseUOMCode,
                                         country_CountryNum = p.Country_CountryNum ?? 0,
                                         uomClass_ClassType = p.UOMClass_ClassType,
                                         uomClass_Description = p.UOMClass_Description,
                                         unitPrice = p.UnitPrice ?? 0,
                                         isCube = p.Part_CheckBox04_Cube,
                                         isSlice = p.Part_CheckBox03_Slice,
                                         isStrip = p.Part_CheckBox13_Strip,
                                         allowVaryWeight = (p.AllowSellingVaryWeight ?? false) ? "Yes" : "No",
                                         looseUom = p.Part_ShortChar03_LooseUOM == "0" || p.Part_ShortChar03_LooseUOM == "" ? "" : p.Part_ShortChar03_LooseUOM,
                                         standardFullQty = p.Part_CheckBox05_Standard_Full_Qty ?? false ? "Yes" : "No",
                                         defaultUomId = defaultUom.id,
                                         AllowLooseSelling = p.Part_CheckBox01_Allow_Selling_Loose ?? false,
                                         remainingQty = s.remainingQty,
                                         conversionFactor = p.conversionFactor ?? 1
                                     };

                    _result.lstProduct = finalQuery
                        .Skip(param.Start)
                        .Take(param.Length)
                        .ToList();

                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        #region Get price from database 

        public List<CustomerPricing> GetCustomerPricing(string custID, List<string> partNumbers, string priceDate)
        {
            List<CustomerPricing> customerPricingList = new List<CustomerPricing>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Create the comma-separated string for the IN clause
                    string partNumString = string.Join(", ", partNumbers.Select(p => $"'{p}'"));

                    // Define the SQL query with parameters
                    string sql = $@"
                        SELECT 
                            p.code,
                            p.id AS CustNum,
                            p.custID,
                            q.BasePrice AS CustGrpBasePrice,
                            q.UomCode,
                            q.StartDate,
                            q.EndDate,
                            q.ListCode,
                            q.PartNum,
                            r.BasePrice AS CustShipBasePrice
                        FROM 
                            Customer p
                        LEFT JOIN 
                            (
                            SELECT
                                c.code,
                                a.PartNum,
                                a.BasePrice,
                                a.UomCode,
                                d.StartDate,
                                d.EndDate,
                                b.ListCode,
                                c.groupCode
                            FROM 
                                PriceListPart a
                            INNER JOIN 
                               PriceList d ON a.ListCode = d.ListCode
                            LEFT OUTER JOIN 
                                CustomerGroupPriceList b ON d.ListCode = b.listCode
                            LEFT OUTER JOIN 
                                Customer c ON b.groupCode = c.groupCode
                            WHERE 
                                a.PartNum IN ({partNumString}) 
                                AND c.custID = @CustID
                                AND @OrderDate BETWEEN d.StartDate AND d.EndDate
                            ) q ON p.code = q.code
                        LEFT JOIN 
                            (
                            SELECT TOP 1
                                c.code,
                                a.PartNum,
                                a.BasePrice,
                                a.UomCode,
                                d.StartDate,
                                d.EndDate,
                                b.ListCode,
                                c.groupCode,
                                c.code AS CustNum
                            FROM 
                               PriceListPart a
                            INNER JOIN 
                                PriceList d ON a.ListCode = d.ListCode
                            LEFT OUTER JOIN 
                                CustomerPriceList b ON d.ListCode = b.ListCode
                            LEFT OUTER JOIN 
                                Customer c ON b.CustNum = c.code
                            WHERE 
                                a.PartNum IN ({partNumString}) 
                                AND c.custID = @CustID
                                AND b.ShipToNum = ''
                                AND @OrderDate BETWEEN d.StartDate AND d.EndDate
                            ) r ON p.code = r.CustNum
                        WHERE 
                            p.custID = @CustID";

                    // Create the SQL parameters
                    var parameters = new[]
                    {
                        new SqlParameter("@CustID", custID),
                        new SqlParameter("@OrderDate", priceDate)
                    };

                    // Execute the query and map the results to the CustomerPricing model
                    customerPricingList = model.Database.SqlQuery<CustomerPricing>(sql, parameters).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
            return customerPricingList; // Return the list of CustomerPricing
        }


        public List<UomConvs> GetUomConvList(List<string> partNumbers)
        {
            var result = new List<UomConvs>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Start with the UomConv table
                    var query = model.UomConv.AsQueryable();
                    // Apply the filter for partNumbers
                    if (partNumbers != null && partNumbers.Any())
                    {
                        query = query.Where(p => partNumbers.Contains(p.partNum));
                    }

                    result = query.Select(u => new UomConvs
                    {
                        Id = u.id,
                        Company = u.company,
                        PartNum = u.partNum,
                        UomCode = u.uomCode,
                        ConvFactor = u.convFactor,
                        UniqueField = u.uniqueField,
                        ConvOperator = u.convOperator,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
            return result;
        }

        #endregion

        public bool IsProductExists(Products entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.Product.Any(p => p.partNumber == entityEn.partNumber && (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleItems> GetOrderTypeList(int moduleTypeId)
        {
            try
            {
                var _result = new List<ModuleItems>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.ModuleItems.AsQueryable();

                    if (moduleTypeId > 0)
                    {
                        query = query.Where(p => p.modulType_Id == moduleTypeId
                        );
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new ModuleItems
                        {
                            id = p.id,
                            modulType_Id = p.modulType_Id,
                            name = p.name,
                            description = p.description
                        }).Take(100).ToList();
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<ModuleItems> GetOrderOpertionalStyleList(int moduleTypeId)
        {
            try
            {
                var _result = new List<ModuleItems>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.ModuleItems.AsQueryable();

                    if (moduleTypeId > 0)
                    {
                        query = query.Where(p => p.modulType_Id == moduleTypeId
                        );
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new ModuleItems
                        {
                            id = p.id,
                            modulType_Id = p.modulType_Id,
                            name = p.name,
                            description = p.description
                        }).Take(100).ToList();
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Domain.DTOs.OrderUOM> GetOrderUOMList(int orderTypeId)
        {
            try
            {
                var _result = new List<Domain.DTOs.OrderUOM>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // Get the order type name
                    var orderType = model.ModuleItems
                        .Where(p => p.id == orderTypeId)
                        .Select(p => new { p.id, p.name })
                        .FirstOrDefault();

                    if (orderType != null)
                    {
                        var query = model.OrderUOM.AsQueryable();
                        //.Where(uom => uom.id == orderTypeId);

                        // Check if the orderType name is "Loose Quantity" and remove items with disableLooseQty == 1
                        if (orderType.name.Contains("Loose Quantity"))
                        {
                            query = query.Where(uom => uom.disableLooseQty == false || uom.disableLooseQty == null);
                        }
                        if (orderType.name.Contains("Full Quantity"))
                        {
                            query = query.Where(uom => uom.disableFullQty == false || uom.disableFullQty == null);
                        }
                        _result = query.OrderBy(p => p.id)
                            .Select(p => new Domain.DTOs.OrderUOM
                            {
                                id = p.id,
                                code = p.code
                            })
                            .Take(100)
                            .ToList();
                    }
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }

        public List<ModuleItems> GetComplimentaryList(int moduleTypeId)
        {
            try
            {
                var _result = new List<ModuleItems>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.ModuleItems.AsQueryable();

                    if (moduleTypeId > 0)
                    {
                        query = query.Where(p => p.modulType_Id == moduleTypeId
                        );
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new ModuleItems
                        {
                            id = p.id,
                            modulType_Id = p.modulType_Id,
                            name = p.name,
                            description = p.description
                        }).Take(100).ToList();
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public ModuleItems GetRemarksComplimentary(int moduleTypeId)
        {
            try
            {
                var _result = new ModuleItems();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.ModuleItems.AsQueryable();

                    string moduleTypeIdString = moduleTypeId.ToString();



                    if (moduleTypeId > 0)
                    {
                        query = query.Where(p => p.id == moduleTypeId);
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new ModuleItems
                        {
                            id = p.id,
                            modulType_Id = p.modulType_Id,
                            remarks = p.remarks
                        }).FirstOrDefault();
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public ModuleItems GetOperationStyleRemarks(int moduleTypeId)
        {
            try
            {
                var _result = new ModuleItems();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.ModuleItems.AsQueryable();
                    string moduleTypeIdString = moduleTypeId.ToString();



                    if (moduleTypeId > 0)
                    {
                        query = query.Where(p => p.id == moduleTypeId);
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new ModuleItems
                        {
                            id = p.id,
                            modulType_Id = p.modulType_Id,
                            remarks = p.remarks
                        }).FirstOrDefault();
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
    }
}
