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
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerDetails GetCustomerBySearch(DatatableFilters entityFilter, object filterEntity)
        {
            CustomerDetails _result = new CustomerDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();
                    var filter = new Customers();
                    if (filterEntity != null)
                        filter = (Customers)filterEntity;

                    //get total record-

                    var query = (from cus in model.Customer
                                 join ord in model.Order on cus.id equals ord.customer_Id into orders
                                 from ord in orders
                                     .OrderByDescending(o => o.orderDate)  
                                     .Take(1)  
                                     .DefaultIfEmpty()  
                                 orderby cus.id
                                 select new Customers
                                 {
                                     id = cus.id,
                                     code = cus.code,
                                     custID = cus.custID,
                                     name = cus.name,
                                     Company = cus.Company,
                                     address1 = cus.address1,
                                     address2 = cus.address2,
                                     orderNum = ord != null ? ord.orderNum : "", // Null check for order number
                                     orderDate = ord != null ? ord.orderDate : (DateTime?)null, // Nullable DateTime check for order date
                                     orderStatus = ord != null ? ord.orderStatus : string.Empty, // Null check for order status
                                     groupCode = cus.groupCode,
                                 });


                    //Filtered count 
                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                                   query = query.Where(p => (
                                   p.custID.Contains(entityFilter.search)
                                || p.name.Contains(entityFilter.search)
                                || p.address1.Contains(entityFilter.search)
                                || p.address2.Contains(entityFilter.search)
                                || p.groupCode.Contains(entityFilter.search)
                                || p.code.Contains(entityFilter.search)
                            ));
                    }
                    //query = query.Where(p => p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00");
                    query = query.Where(p => p.Company.Trim() == "LUCKY00 " );
                    if (!string.IsNullOrEmpty(filter.searchOrderStatus))
                    {
                        query = query.Where(p => (p.orderStatus.Contains(filter.searchOrderStatus)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchCode))
                    {
                        query = query.Where(p => (p.code.Contains(filter.searchCode)));
                    }

                    if (!string.IsNullOrEmpty(filter.searchName))
                    {
                        query = query.Where(p => (p.name.StartsWith(filter.searchName)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchCity))
                    {
                        query = query.Where(p => (p.address1.Contains(filter.searchCity) || p.address2.Contains(filter.searchCity)));
                    }
                    if (!string.IsNullOrEmpty(filter.searchState))
                    {
                        query = query.Where(p => (p.address1.Contains(filter.searchState) || p.address2.Contains(filter.searchState)));
                    }
                   
                    if (!string.IsNullOrEmpty(filter.searchOrderCode))
                    {
                        string searchOrderNum = filter.searchOrderCode;
                        query = query.Where(p => p.orderNum == searchOrderNum);
                    }
                    if (filter.searchOrderDateFrom.HasValue && filter.searchOrderDateTo.HasValue)
                    {
                        query = query.Where(p => (p.orderDate >= filter.searchOrderDateFrom && p.orderDate <= filter.searchOrderDateTo));
                    }else if (filter.searchOrderDateFrom.HasValue)
                    {
                        var searchDate = filter.searchOrderDateFrom.Value.Date; 
                        query = query.Where(p => (p.orderDate >= searchDate));
                    } else if (filter.searchOrderDateTo.HasValue)
                    {
                        query = query.Where(p => (p.orderDate <= filter.searchOrderDateTo));
                    }
                  

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                    _result.lstCustomer = query.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public Customers CreateCustomer(Customers entityEn)
        {
            var _CustomerEn = new Customer();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Customer

                    _CustomerEn.code = entityEn.code;
                    _CustomerEn.name = entityEn.name;             
                    _CustomerEn.address1 = entityEn.address1;
                    _CustomerEn.address2 = entityEn.address2;
                   
                    model.Customer.Add(_CustomerEn);
                    model.SaveChanges();

                    entityEn.id = _CustomerEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public Customers UpdateCustomer(Customers entityEn)
        {
            var _CustomerEn = new Customer();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update Employee Statutory
                    _CustomerEn = model.Customer.SingleOrDefault(p => p.id == entityEn.id);
                    if (_CustomerEn != null)
                    {
                        _CustomerEn.code = entityEn.code;
                        _CustomerEn.name = entityEn.name;
                        _CustomerEn.address1 = entityEn.address1;
                        _CustomerEn.address2 = entityEn.address2;
                       
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

        public bool DeleteCustomer(int id)
        {
            var result = new Customers();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _CustomerEn = model.Customer.SingleOrDefault(p => p.id == id);

                    if (_CustomerEn != null)
                    {
                        model.Customer.Remove(_CustomerEn);
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

        public Customers GetCustomerById(int id)
        {
            Customers _CustomerEn = new Customers();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Customer.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _CustomerEn = new Customers
                        {
                            id = entity.id,
                            code = entity.code,
                            name = entity.name,
                            address1 = entity.address1,
                            address2 = entity.address2,
                            groupCode = entity.groupCode,
                            custID = entity.custID,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _CustomerEn;
        }

        public Customers GetCustomerByCode(string code)
        {
            Customers _CustomerEn = new Customers();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Customer.SingleOrDefault(p => p.code == code);
                    if (entity != null)
                    {
                        _CustomerEn = new Customers
                        {
                            id = entity.id,
                            code = entity.code,
                            name = entity.name,
                            address1 = entity.address1,
                            address2 = entity.address2,
                            groupCode = entity.groupCode,
                            creditHold= (bool)entity.creditHold

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _CustomerEn;
        }

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
                        query = query.Where(p => p.name.StartsWith(searchParam) || 
                            p.code.Contains(searchParam) || p.address1.Contains(searchParam)
                        );
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new Customers
                        {
                            id = p.id,
                            name=p.name,
                            address1 = p.address1,
                            address2 = p.address2,
                            code = p.code,
                            city = p.city,
                            state = p.state,
                            address3 = p.address3,
                            groupCode = p.groupCode,

                        }).Take(100).ToList();
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }


        public List<Customers> GetCustomerList(string searchName, string searchCode, string searchCity, string searchState)
        {
            try
            {
                var _result = new List<Customers>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.Customer.AsQueryable();


                    if (!string.IsNullOrWhiteSpace(searchName))
                    {
                        // Prioritize StartsWith
                        var startsWithQuery = query.Where(p => p.name.StartsWith(searchName));
                        var containsQuery = query.Where(p => p.name.Contains(searchName) && !p.name.StartsWith(searchName));
                        query = startsWithQuery.Concat(containsQuery);
                    }

                    if (!string.IsNullOrWhiteSpace(searchCode))
                    {
                        query = query.Where(p => p.custID.Contains(searchCode));
                    }

                    if (!string.IsNullOrWhiteSpace(searchCity))
                    {
                        query = query.Where(p => p.city.Contains(searchCity));
                    }

                    if (!string.IsNullOrWhiteSpace(searchState))
                    {
                        query = query.Where(p => p.state.Contains(searchState));
                    }
                   // query = query.Where(p => p.Company.Trim() == "LUCKY00 " || p.Company.Trim() == "LUCKY00");
                    query = query.Where(p => p.Company.Trim() == "LUCKY00 ");
                    _result = query // .OrderByDescending(p => p.id)
                        .Select(p => new Customers
                        {
                            id = p.id,
                            name = p.name,
                            custID = p.custID,
                            address1 = p.address1,
                            address2 = p.address2,
                            code = p.code,   
                            city = p.city,
                            state = p.state,
                            address3 = p.address3,
                            groupCode = p.groupCode,
                            creditHold=(bool)p.creditHold

                        }).Take(100).ToList();
                }

                return _result;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool IsCustomerExists(Customers entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.Customer.Any(p => p.code == entityEn.code && (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Districts> GetDistrictList(string searchParam)
        {
            try
            {
                var _result = new List<Districts>();

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var query = model.District.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(searchParam))
                    {
                        query = query.Where(p => p.districtID.StartsWith(searchParam) ||
                            p.districtDesc.Contains(searchParam)
                        );
                    }

                    _result = query.OrderByDescending(p => p.id)
                        .Select(p => new Districts
                        {
                            id = p.id,
                            districtDesc = p.districtDesc,
                            districtID = p.districtID,
                            sysRowID = p.sysRowID,
                        }).ToList();
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
