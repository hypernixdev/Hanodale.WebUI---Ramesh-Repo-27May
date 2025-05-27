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

namespace Hanodale.DataAccessLayer.Services
{
    public class ShipToAddressService : BaseService, IShipToAddressService
    {
        #region ShipToAddresses

        /// <summary>
        /// This method is to get the Module Item details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>ShipToAddressess list</returns> 

        public ShipToAddressDetails GetShipToAddressBySearch(DatatableFilters entityFilter)
        {
            ShipToAddressDetails _result = new ShipToAddressDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                   
                    var query = model.ShipToAddress.AsNoTracking().Where(p => true);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                           p.shippingCode.Contains(entityFilter.search)
                        || p.address1.Contains(entityFilter.search)
                        || p.address2.Contains(entityFilter.search)
                        || p.address3.Contains(entityFilter.search)
                        || (p.store_Id == null ? false : p.Store.name.Contains(entityFilter.search))
                        || (p.city.Contains(entityFilter.search))
                        || (p.state.Contains(entityFilter.search))
                        || (p.country.Contains(entityFilter.search))
                        || (p.zip.Contains(entityFilter.search))
                        ));

                     }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new ShipToAddresses
                        {
                            id = p.id,
                            name = p.name,
                            storeName = p.plantName,
                            custId = p.custID,
                            shippingCode = p.shippingCode,
                            address1 = p.address1,
                            address2 = p.address2,
                            address3 = p.address3,                       
                            cityName = p.city,                   
                            stateName = p.state,                    
                            countryName = p.country,
                            zip = p.zip,
                        });

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstShipToAddress = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to save the ShipToAddressess details
        /// </summary> 
        public ShipToAddresses CreateShipToAddress(ShipToAddresses entityEn)
        {
            var _shipToAddressEn = new ShipToAddress();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Ship to Address

                    _shipToAddressEn.id = entityEn.id;
                    _shipToAddressEn.shippingCode = entityEn.shippingCode;
                    _shipToAddressEn.address1 = entityEn.address1;
                    _shipToAddressEn.address2 = entityEn.address2;
                    _shipToAddressEn.address3 = entityEn.address3;
                    _shipToAddressEn.zip = entityEn.zip;                                   
                    model.ShipToAddress.Add(_shipToAddressEn);
                    model.SaveChanges();

                    entityEn.id = _shipToAddressEn.id;
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
        /// This method is to update the ShipToAddressess details
        /// </summary> 
        public ShipToAddresses UpdateShipToAddress(ShipToAddresses entityEn)
        {
            var _shipToAddressEn = new ShipToAddress();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _shipToAddressEn = model.ShipToAddress.SingleOrDefault(p => p.id == entityEn.id);
                    if (_shipToAddressEn != null)
                    {
                        
                        _shipToAddressEn.id = entityEn.id;
                        _shipToAddressEn.shippingCode = entityEn.shippingCode;
                        _shipToAddressEn.address1 = entityEn.address1;
                        _shipToAddressEn.address2 = entityEn.address2;
                        _shipToAddressEn.address3 = entityEn.address3;
                        _shipToAddressEn.zip = entityEn.zip;
                        model.ShipToAddress.Add(_shipToAddressEn);

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
        public bool DeleteShipToAddress(int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _companyStatutoryEn = model.ShipToAddress.SingleOrDefault(p => p.id == id);

                    if (_companyStatutoryEn != null)
                    {
                        model.ShipToAddress.Remove(_companyStatutoryEn);
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
        /// This method is to get the ShipToAddressess by ShipToAddressess id
        /// </summary>
        /// <param name="roleId">ShipToAddressess Id</param>
        /// <returns>ShipToAddressess details</returns>
        public ShipToAddresses GetShipToAddressById(int id)
        {
            ShipToAddresses _ShipToAddressEn = new ShipToAddresses();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.ShipToAddress.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _ShipToAddressEn = new ShipToAddresses
                        {
                            id = entity.id,
                            custId = entity.custID,
                            shippingCode = entity.shippingCode,
                            address1 = entity.address1,
                            address2 = entity.address2,
                            address3 = entity.address3,
                            cityName = entity.city,
                            stateName = entity.state,
                            countryName = entity.country,
                            zip = entity.zip


                        };
}
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _ShipToAddressEn;
        }

        public ShipToAddresses GetShipToAddressByCode(string code)
        {
            var _shipToAddressEn = new ShipToAddresses();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.ShipToAddress.SingleOrDefault(p => p.shippingCode == code);
                    if (entity != null)
                    {
                        _shipToAddressEn = new ShipToAddresses
                        {
                            id = entity.id,
                            shippingCode = entity.shippingCode

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _shipToAddressEn;
        }

        ///// <summary>
        ///// This method is to check the ShipToAddressess exists or not.
        ///// </summary>
        ///// <param name="stockName">ShipToAddresses Name</param>  
        public bool IsShipToAddressExists(ShipToAddresses entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.ShipToAddress.Any(p =>  p.shippingCode == entityEn.shippingCode && (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        #endregion

        #region Get ship to by customer id 
        public List<ShipToAddresses> GetShipToAddressByCustomerId(int customerId,string searchby)
        {
            var shipToAddressesList = new List<ShipToAddresses>();
            List<ShipToAddress> entities;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (searchby == "code")
                    {
                        var cId = customerId.ToString();
                        entities = model.ShipToAddress
                                   .Join(model.Customer,
                                         shipTo => shipTo.custID,
                                         customer => customer.code,
                                         (shipTo, customer) => new { ShipTo = shipTo, Customer = customer })
                                   .Where(x => x.Customer.code == cId)
                                   .Select(x => x.ShipTo)
                                   .ToList();

                    }
                    else
                    {
                         entities = model.ShipToAddress
                                   .Join(model.Customer,
                                         shipTo => shipTo.custID,
                                         customer => customer.code,
                                         (shipTo, customer) => new { ShipTo = shipTo, Customer = customer })
                                   .Where(x => x.Customer.id == customerId)
                                   .Select(x => x.ShipTo)
                                   .ToList();
                    }
                    System.Diagnostics.Debug.WriteLine(entities);
                    if (entities.Any())
                    {
                        shipToAddressesList = entities.Select(entity => new ShipToAddresses
                        {
                            id = entity.id,
                            storeName = entity.plantName,
                            shippingCode = entity.shippingCode,
                            address1 = entity.address1,
                            address2 = entity.address2,
                            address3 = entity.address3,
                            cityName = entity.city,
                            stateName = entity.state,
                            countryName = entity.country,
                            zip = entity.zip
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException?.InnerException? .Message ?? ex.Message);
            }

            return shipToAddressesList;
        }

        #endregion
        public List<Customers> GetCustomerList(string searchParam)
        {
            try
            {
                var _result = new List<Customers>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.Customer.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(searchParam))
                    {
                        query = query.Where(p => p.name.Contains(searchParam) ||
                            p.code.Contains(searchParam) || p.address1.Contains(searchParam)
                        );
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new Customers
                        {
                            id = p.id,
                            name = p.name,
                            address1 = p.address1,
                            address2 = p.address2,
                            code = p.code,
                            city = p.city,
                            state = p.state,
                            address3 = p.address3,
                        }).Take(100).ToList();
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
