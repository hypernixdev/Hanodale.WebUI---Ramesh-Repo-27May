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
using System.Data.Entity.Infrastructure;

namespace Hanodale.DataAccessLayer.Services
{
    public class PriceListService : BaseService, IPriceListService
    {
        public PriceListDetails GetPriceListBySearch(DatatableFilters entityFilter)
        {
            PriceListDetails _result = new PriceListDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    /* var query = (from pl in model.PriceList
                                  join cpl in model.CustomerPriceList on pl.ListCode  equals cpl.ListCode into prices
                                  from cpl in prices.DefaultIfEmpty()
                                  orderby pl.id
                                  select new PriceLists
                                  {
                                      id = pl.id,
                                      listCode = pl.ListCode,
                                      currencyCode = pl.CurrencyCode,
                                      listDescription = pl.ListDescription,
                                      startDate = pl.StartDate,
                                      endDate = pl.EndDate,
                                      custNum = cpl.CustNum,
                                      shipToNum = cpl.ShipToNum,
                                      seqNum = cpl.SeqNum
                                  });*/
                    var customerList = model.Customer.ToList();  
                    var priceListQuery = (from pl in model.PriceList
                                          join cpl in model.CustomerPriceList on pl.ListCode equals cpl.ListCode into prices
                                          from cpl in prices.DefaultIfEmpty()
                                          orderby pl.id
                                          select new
                                          {
                                              pl.id,
                                              pl.ListCode,
                                              pl.CurrencyCode,
                                              pl.ListDescription,
                                              pl.StartDate,
                                              pl.EndDate,
                                              cpl.CustNum,
                                              cpl.ShipToNum,
                                              cpl.SeqNum
                                          }).ToList(); 

                    // Perform the join in memory
                    var query = (from price in priceListQuery
                                 join cust in customerList on price.CustNum.ToString() equals cust.code into customers
                                 from cust in customers.DefaultIfEmpty()
                                 select new PriceLists
                                 {
                                     id = price.id,
                                     listCode = price.ListCode,
                                     currencyCode = price.CurrencyCode,
                                     listDescription = price.ListDescription,
                                     startDate = price.StartDate,
                                     endDate = price.EndDate,
                                     custNum = price.CustNum,
                                     shipToNum = price.ShipToNum,
                                     seqNum = price.SeqNum,
                                     custID = cust?.custID,
                                     CustGroup = cust?.groupCode
                                 });  



                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                 p.listCode.Contains(entityFilter.search)
                                 || p.currencyCode.Contains(entityFilter.search)
                                 || p.listDescription.Contains(entityFilter.search)
                                 || p.startDate.ToString().Contains(entityFilter.search)
                                 || p.endDate.ToString().Contains(entityFilter.search)
                                 || p.custNum.ToString().Contains(entityFilter.search)
                                 || (p.shipToNum != null && p.shipToNum.Contains(entityFilter.search))
                                 || p.seqNum.ToString().Contains(entityFilter.search)
                                 || (p.custID != null && p.custID.Contains(entityFilter.search))
                                 || (p.CustGroup != null && p.CustGroup.Contains(entityFilter.search))
                             ));

                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new PriceLists
                        {
                            id = p.id,
                            listCode = p.listCode,
                            currencyCode = p.currencyCode,
                            listDescription = p.listDescription,
                            startDate = p.startDate,
                            endDate = p.endDate,
                            custNum = p.custNum,
                            shipToNum = p.shipToNum,
                            seqNum = p.seqNum,
                            custID = p.custID,
                            CustGroup = p.CustGroup
                        });

                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstPriceList = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }
        //List<PriceListParts> GetPriceListParts(string ListCode);


        #region Get parts by list code 
        public List<PriceListParts> GetPriceListParts(string ListCode)
        {
            var partsList = new List<PriceListParts>();
            List<PriceListPart> entities;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {                 
                      entities = model.PriceListPart
                      .Join(model.PriceList,
                            plp => plp.ListCode.Trim().ToLower(),
                            pl => pl.ListCode.Trim().ToLower(),
                            (plp, pl) => new { Plp = plp, Pl = pl })
                      .Where(x => x.Plp.ListCode.Trim().ToLower() == ListCode.Trim().ToLower())
                      .Select(x => x.Plp)
                      .ToList();

                    System.Diagnostics.Debug.WriteLine(entities);
                    if (entities.Any())
                    {
                        partsList = entities.Select(entity => new PriceListParts
                        {
                            id = entity.id,
                            partNum = entity.PartNum,
                            basePrice = entity.BasePrice,
                            uomCode = entity.UomCode,
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return partsList;
        }
        public List<CustomerPriceLists> GetCustomerPriceList(int CustNum, string groupCode)
        {
            var CustomerPriceList = new List<CustomerPriceLists>();
           // List<CustomerPriceList> entities;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    /*  var entities = (from pl in model.PriceList
                                   join cpl in model.CustomerPriceList on pl.ListCode equals cpl.ListCode into prices
                                   from cpl in prices.DefaultIfEmpty()
                                   where cpl.CustNum == CustNum
                                   orderby pl.id
                                   select new PriceLists
                                   {
                                       id = pl.id,
                                       listCode = pl.ListCode,
                                       currencyCode = pl.CurrencyCode,
                                       listDescription = pl.ListDescription,
                                       startDate = pl.StartDate,
                                       endDate = pl.EndDate,
                                       custNum = cpl.CustNum,
                                       shipToNum = cpl.ShipToNum,
                                       seqNum = cpl.SeqNum
                                   }).ToList();*/
                    var entities = new List<PriceLists>(); ;
                    if (!string.IsNullOrEmpty(groupCode))
                    {
                         entities = (from cgp in model.CustomerGroupPriceList
                                     join cpl in model.CustomerPriceList on cgp.listCode equals cpl.ListCode into prices
                                     from cpl in prices.DefaultIfEmpty()
                                     join pl in model.PriceList on cpl.ListCode equals pl.ListCode into priceList
                                     from pl in priceList.DefaultIfEmpty()
                                     where cpl != null && cpl.CustNum == CustNum && cgp != null && cgp.groupCode != null && cgp.groupCode.Contains(groupCode)  // Handle null checks
                                     orderby pl.id
                                     select new PriceLists
                                        {
                                            id = pl.id,
                                            listCode = pl.ListCode,
                                            currencyCode = pl.CurrencyCode,
                                            listDescription = pl.ListDescription,
                                            startDate = pl.StartDate,
                                            endDate = pl.EndDate,
                                            custNum = cpl.CustNum,
                                            shipToNum = cpl.ShipToNum,
                                            seqNum = cpl.SeqNum,
                                            CustGroup = cgp.groupCode
                                        }).ToList();
                    }
                    else
                    {
                         entities = (from cgp in model.CustomerGroupPriceList
                                     join cpl in model.CustomerPriceList on cgp.listCode equals cpl.ListCode into prices
                                     from cpl in prices.DefaultIfEmpty()
                                     join pl in model.PriceList on cpl.ListCode equals pl.ListCode into priceList
                                     from pl in priceList.DefaultIfEmpty()
                                     where cpl != null && cpl.CustNum == CustNum
                                     orderby pl.id
                                     select new PriceLists
                                        {
                                            id = pl.id,
                                            listCode = pl.ListCode,
                                            currencyCode = pl.CurrencyCode,
                                            listDescription = pl.ListDescription,
                                            startDate = pl.StartDate,
                                            endDate = pl.EndDate,
                                            custNum = cpl.CustNum,
                                            shipToNum = cpl.ShipToNum,
                                            seqNum = cpl.SeqNum,
                                            CustGroup = cgp.groupCode
                                        }).ToList();

                    }

                    /*if (!string.IsNullOrEmpty(groupCode))
                    {
                        entities = entities.Where(p => p.CustGroup.Contains(groupCode)).ToList();
                    }*/
                    if (entities.Any())
                    {
                        CustomerPriceList = entities.Select(entity => new CustomerPriceLists
                        {                    
                            ListCode = entity.listCode,
                            ListDescription = entity.listDescription,                           
                            ShipToNum = entity.shipToNum,
                            SeqNum = entity.seqNum,
                            CurrencyCode = entity.currencyCode,
                            StartDate = entity.startDate.HasValue ? entity.startDate.Value.ToString("dd/MM/yyyy") : string.Empty, // Formatting date
                            EndDate = entity.endDate.HasValue ? entity.endDate.Value.ToString("dd/MM/yyyy") : string.Empty, // Formatting date
                            CustGroup = entity.CustGroup
                        }).ToList();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }

            return CustomerPriceList;
        }
        public PriceLists CreatePriceList(PriceLists entityEn)
        {
            var _PriceListEn = new Entity.Core.PriceList();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _PriceListEn.ListCode = entityEn.listCode;
                    _PriceListEn.CurrencyCode = entityEn.currencyCode;
                    _PriceListEn.ListDescription = entityEn.listDescription;
                    _PriceListEn.StartDate = entityEn.startDate;
                    _PriceListEn.EndDate = entityEn.endDate;
                
                    model.PriceList.Add(_PriceListEn);
                    model.SaveChanges();

                    entityEn.id = _PriceListEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public PriceLists UpdatePriceList(PriceLists entityEn)
        {
            var _PriceListEn = new Entity.Core.PriceList();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _PriceListEn = model.PriceList.SingleOrDefault(p => p.id == entityEn.id);
                    if (_PriceListEn != null)
                    {
                        _PriceListEn.ListCode = entityEn.listCode;
                        _PriceListEn.CurrencyCode = entityEn.currencyCode;
                        _PriceListEn.ListDescription = entityEn.listDescription;
                        _PriceListEn.StartDate = entityEn.startDate;
                        _PriceListEn.EndDate = entityEn.endDate;
                            
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

        public bool DeletePriceList(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _PriceListEn = model.PriceList.SingleOrDefault(p => p.id == id);
                    if (_PriceListEn != null)
                    {
                        model.PriceList.Remove(_PriceListEn);
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

        public PriceLists GetPriceListById(int id)
        {
            PriceLists _PriceListEn = new PriceLists();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    /*var entity = (from pl in model.PriceList
                                  join cpl in model.CustomerPriceList on pl.ListCode equals cpl.ListCode into prices
                                  from cpl in prices.DefaultIfEmpty()
                                  join cgp in model.CustomerGroupPriceList on pl.ListCode equals cgp.listCode into groupPrices
                                  from cgp in groupPrices.DefaultIfEmpty()
                                  where pl.id == id
                                  orderby pl.id
                                  select new PriceLists
                                  {
                                      id = pl.id,
                                      listCode = pl.ListCode,
                                      currencyCode = pl.CurrencyCode,
                                      listDescription = pl.ListDescription,
                                      startDate = pl.StartDate,
                                      endDate = pl.EndDate,
                                      custNum = cpl.CustNum,
                                      shipToNum = cpl.ShipToNum,
                                      seqNum = cpl.SeqNum,
                                      CustGroup = cgp.groupCode,
                                    //  CustNum = cgp.code
                                  }).FirstOrDefault();
                    var custId = entity.custNum.ToString();

                    var cusCode = (from cus in model.Customer
                                        where cus.code.Contains(custId)
                                   select cus.custID).FirstOrDefault();
                    if (entity != null)
                    {
                        _PriceListEn = new PriceLists
                        {
                            id = entity.id,
                            listCode = entity.listCode,
                            currencyCode = entity.currencyCode,
                            listDescription = entity.listDescription,
                            startDate = entity.startDate,
                            endDate = entity.endDate,
                            custNum = entity.custNum,
                            shipToNum = entity.shipToNum,
                            seqNum = entity.seqNum,
                            CustGroup = entity.CustGroup,
                            custID = cusCode

                        };
                    }*/

                    var customerList = model.Customer.ToList();
                    var priceListQuery = (from pl in model.PriceList
                                          join cpl in model.CustomerPriceList on pl.ListCode equals cpl.ListCode into prices
                                          from cpl in prices.DefaultIfEmpty()
                                          where pl.id == id
                                          orderby pl.id
                                          select new
                                          {
                                              pl.id,
                                              pl.ListCode,
                                              pl.CurrencyCode,
                                              pl.ListDescription,
                                              pl.StartDate,
                                              pl.EndDate,
                                              cpl.CustNum,
                                              cpl.ShipToNum,
                                              cpl.SeqNum
                                          }).ToList();

                    // Perform the join in memory
                    var entityList = (from price in priceListQuery
                                 join cust in customerList on price.CustNum.ToString() equals cust.code into customers
                                 from cust in customers.DefaultIfEmpty()
                                 select new PriceLists
                                 {
                                     id = price.id,
                                     listCode = price.ListCode,
                                     currencyCode = price.CurrencyCode,
                                     listDescription = price.ListDescription,
                                     startDate = price.StartDate,
                                     endDate = price.EndDate,
                                     custNum = price.CustNum,
                                     shipToNum = price.ShipToNum,
                                     seqNum = price.SeqNum,
                                     custID = cust?.custID,
                                     CustGroup = cust?.groupCode
                                 }).ToList();
                    var entity = entityList.FirstOrDefault();

                    if (entity != null)
                    {
                        _PriceListEn = new PriceLists
                        {
                            id = entity.id,
                            listCode = entity.listCode,
                            currencyCode = entity.currencyCode,
                            listDescription = entity.listDescription,
                            startDate = entity.startDate,
                            endDate = entity.endDate,
                            custNum = entity.custNum,
                            shipToNum = entity.shipToNum,
                            seqNum = entity.seqNum,
                            CustGroup = entity.CustGroup,
                            custID = entity.custID

                        };
                    }

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _PriceListEn;
        }

        public bool IsPriceListExists(PriceLists entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.PriceList.Any(p => p.ListCode == entityEn.listCode && (entityEn.id == 0 || p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
    }
}
#endregion