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
    public class StoreService : BaseService, IStoreService
    {
        #region Stores

        /// <summary>
        /// This method is to get the Module Item details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>Storess list</returns> 

        public StoreDetails GetStoreBySearch(DatatableFilters entityFilter, object filterEntity)
        {
            StoreDetails _result = new StoreDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();
                    var filter = new Stores();
                    if (filterEntity != null)
                        filter = (Stores)filterEntity;

                    var query = model.Store.AsNoTracking().Where(p => true);

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    if (!string.IsNullOrEmpty(filter.searchCity))
                    {
                        query = query.Where(p => (p.address1.Contains(filter.searchCity) || p.address2.Contains(filter.searchCity) || p.address3.Contains(filter.searchCity) || p.Address_City.name.Contains(filter.searchCity)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchState))
                    {
                        query = query.Where(p => (p.address1.Contains(filter.searchState) || p.address2.Contains(filter.searchState) || p.address3.Contains(filter.searchState) || p.Address_State.name.Contains(filter.searchState)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchCountry))
                    {
                        query = query.Where(p => (p.address1.Contains(filter.searchCountry) || p.address2.Contains(filter.searchCountry) || p.address3.Contains(filter.searchCountry) || p.Address_Country.name.Contains(filter.searchCountry)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchZip))
                    {
                        query = query.Where(p => (p.address1.Contains(filter.searchZip) || p.address2.Contains(filter.searchZip) || p.address3.Contains(filter.searchZip) || p.zip.Contains(filter.searchZip)));
                    }
                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        query = query.Where(p => (
                                p.company.Contains(entityFilter.search)
                                || p.plant.Contains(entityFilter.search)
                                || p.name.Contains(entityFilter.search)
                                || p.address1.Contains(entityFilter.search)
                                || p.address2.Contains(entityFilter.search)
                                || p.address3.Contains(entityFilter.search)
                                || p.Address_City.name.Contains(entityFilter.search)
                                || p.Address_State.name.Contains(entityFilter.search)
                                || p.Address_Country.name.Contains(entityFilter.search)
                            ));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new Stores
                        {
                            id = p.id,
                            company = p.company,
                            plant = p.plant,
                            name = p.name,
                            address1 = p.address1,
                            address2 = p.address2,
                            address3 = p.address3,
                            city_Id = p.address_City_id ?? 0,
                            cityName = p.Address_City.name,
                            state_Id = p.address_State_id ?? 0,
                            stateName = p.Address_State.name,
                            country_Id = p.address_Country_id ?? 0,
                            countryName = p.Address_Country.name,
                            zip = p.zip,                        
                        });

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstStore = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        /// <summary>
        /// This method is to save the Storess details
        /// </summary> 
        public Stores CreateStore(Stores entityEn)
        {
            var _storeEn = new Store();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Ship to Address

                    _storeEn.id = entityEn.id;
                    _storeEn.plant = entityEn.plant;
                    _storeEn.name = entityEn.name;
                    _storeEn.address1 = entityEn.address1;
                    _storeEn.address2 = entityEn.address2;
                    _storeEn.address3 = entityEn.address3;
                    _storeEn.address_City_id = entityEn.city_Id;
                    _storeEn.address_State_id = entityEn.state_Id;
                    _storeEn.address_Country_id = entityEn.country_Id;
                    _storeEn.zip = entityEn.zip;

                    model.Store.Add(_storeEn);
                    model.SaveChanges();

                    entityEn.id = _storeEn.id;
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
        /// This method is to update the Storess details
        /// </summary> 
        public Stores UpdateStore(Stores entityEn)
        {
            var _storeEn = new Store();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _storeEn = model.Store.SingleOrDefault(p => p.id == entityEn.id);
                    if (_storeEn != null)
                    {

                        _storeEn.id = entityEn.id;
                        _storeEn.company = entityEn.company;
                        _storeEn.plant = entityEn.plant;
                        _storeEn.name = entityEn.name;
                        _storeEn.address1 = entityEn.address1;
                        _storeEn.address2 = entityEn.address2;
                        _storeEn.address3 = entityEn.address3;
                        _storeEn.address_City_id = entityEn.city_Id;
                        _storeEn.address_State_id = entityEn.state_Id;
                        _storeEn.address_State_id = entityEn.country_Id;
                        _storeEn.zip = entityEn.zip;

                        model.Store.Add(_storeEn);

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
        public bool DeleteStore(int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _companyStatutoryEn = model.Store.SingleOrDefault(p => p.id == id);

                    if (_companyStatutoryEn != null)
                    {
                        model.Store.Remove(_companyStatutoryEn);
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
        /// This method is to get the Storess by Storess id
        /// </summary>
        /// <param name="roleId">Storess Id</param>
        /// <returns>Storess details</returns>
        public Stores GetStoreById(int id)
        {
            var _storeEn = new Stores();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Store.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _storeEn = new Stores
                        {
                            id = entity.id,
                            company = entity.company,
                            plant = entity.plant,
                            name = entity.name,
                            address1 = entity.address1,
                            address2 = entity.address2,
                            address3 = entity.address3,
                            city_Id = entity.address_City_id ?? 0,
                            state_Id = entity.address_State_id ?? 0,
                            country_Id = entity.address_Country_id ?? 0,
                            zip = entity.zip


                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _storeEn;
        }

        ///// <summary>
        ///// This method is to check the Storess exists or not.
        ///// </summary>
        ///// <param name="stockName">Stores Name</param>  
        public bool IsStoreExists(Stores entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.Store.Any(p => p.plant == entityEn.plant  && (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        #endregion
    }
}
