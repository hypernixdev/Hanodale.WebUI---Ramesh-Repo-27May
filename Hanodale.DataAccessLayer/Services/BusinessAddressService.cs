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

namespace Hanodale.DataAccessLayer.Services
{
    public class BusinessAddressService : IBusinessAddressService
    {
        #region BusinessAddress

        /// <summary>
        /// This method is to get the BusinessAddress details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessAddressDetails GetBusinessAddressBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            BusinessAddressDetails _result = new BusinessAddressDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessAddresses.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    var result = model.BusinessAddresses.OrderByDescending(p => p.modifiedDate)
                                   .Where(p => p.address.Contains(search)
                                   || p.city.Contains(search)
                                   || p.province.Contains(search)
                                   || p.postalCode.Contains(search)
                                   || p.country.Contains(search))
                                   .Select(p => new BusinessAddresses
                                   {
                                       id = p.id,
                                       business_Id = p.business_Id,
                                       address = p.address,
                                       city = p.city,
                                       province = p.province,
                                       postalCode = p.postalCode,
                                       country = p.country,
                                       createdBy = p.createdBy,
                                       createdDate = p.createdDate,
                                       modifiedBy = p.modifiedBy,
                                       modifiedDate = p.modifiedDate
                                   }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstBusinessAddress = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to get the Asset Warehouse details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public BusinessAddressDetails GetBusinessAddress(int currentUserId, int userId, int startIndex, int pageSize)
        {
            BusinessAddressDetails _result = new BusinessAddressDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.BusinessAddresses.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    _result.lstBusinessAddress = model.BusinessAddresses.OrderByDescending(p => p.modifiedDate)
                                                 .Skip(startIndex).Take(pageSize)
                                                 .Select(p => new BusinessAddresses
                                                 {
                                                     id = p.id,
                                                     business_Id = p.business_Id,
                                                     address = p.address,
                                                     city = p.city,
                                                     province = p.province,
                                                     postalCode = p.postalCode,
                                                     country = p.country,
                                                     createdBy = p.createdBy,
                                                     createdDate = p.createdDate,
                                                     modifiedBy = p.modifiedBy,
                                                     modifiedDate = p.modifiedDate
                                                 }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to save the Asset Warehouse details
        /// </summary> 
        public BusinessAddresses CreateBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName)
        {
            BusinessAddress _entity = new BusinessAddress();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new BusinessAddress

                    _entity.business_Id = entity.business_Id;
                    _entity.address = entity.address;
                    _entity.city = entity.city;
                    _entity.province = entity.province;
                    _entity.postalCode = entity.postalCode;
                    _entity.country = entity.country;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;

                    model.BusinessAddresses.Add(_entity);
                    model.SaveChanges();
                    entity.id = _entity.id;

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        /// <summary>
        /// This method is to update the BusinessAddress details
        /// </summary> 
        public BusinessAddresses UpdateBusinessAddress(int currentUserId, BusinessAddresses entity, string pageName)
        {
            BusinessAddress _entity = new BusinessAddress();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update BusinessAddress

                    _entity = model.BusinessAddresses.SingleOrDefault(p => p.id == entity.id);
                    if (_entity != null)
                    {
                        _entity.business_Id = entity.business_Id;
                        _entity.address = entity.address;
                        _entity.city = entity.city;
                        _entity.province = entity.province;
                        _entity.postalCode = entity.postalCode;
                        _entity.country = entity.country;
                        _entity.modifiedBy = entity.modifiedBy;
                        _entity.modifiedDate = entity.modifiedDate;
                    }
                       model.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        /// <summary>
        /// This method is to delete the Asset Warehouse details
        /// </summary>
        /// <param name="stockId">AssetSpecification id</param>  
        public bool DeleteBusinessAddress(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    BusinessAddress _entity = model.BusinessAddresses.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.BusinessAddresses.Remove(_entity);
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
        /// This method is to get the BusinessAddress by stock id
        /// </summary>
        /// <param name="roleId">BusinessAddress Id</param>
        /// <returns>BusinessAddress details</returns>
        public BusinessAddresses GetBusinessAddressById(int id)
        {
            BusinessAddresses _entity = new BusinessAddresses();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.BusinessAddresses.SingleOrDefault(p => p.business_Id == id);
                    if (entity != null)
                    {
                        _entity = new BusinessAddresses
                        {
                            id = entity.id,
                            business_Id = entity.business_Id,
                            address = entity.address,
                            city = entity.city,
                            province = entity.province,
                            postalCode = entity.postalCode,
                            country = entity.country,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _entity;
        }

        /// <summary>
        /// Get List of BusinessAddresses
        /// </summary> 
        /// <returns></returns>
        public List<BusinessAddresses> GetListBusinessAddress()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get Module Type List
                    //.Where(p => p.visibility)
                    return model.BusinessAddresses.Select(p => new BusinessAddresses
                    {
                        id = p.id,
                        business_Id = p.business_Id,
                        address = p.address,
                        city = p.city,
                        province = p.province,
                        postalCode = p.postalCode,
                        country = p.country,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        ///// <summary>
        ///// This method is to check the Asset Warehouses exists or not.
        ///// </summary>
        ///// <param name="stockName">Asset Name</param>  
        public bool IsBusinessAddressExists(BusinessAddresses entity)
        {
            BusinessAddress _entity = new BusinessAddress();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.BusinessAddresses.SingleOrDefault(p => p.business_Id == entity.business_Id);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.BusinessAddresses.SingleOrDefault(p => p.business_Id == entity.business_Id && p.id != entity.id);
                        if (_entity != null)
                            isExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return isExists;
        }

        #endregion
    }
}
