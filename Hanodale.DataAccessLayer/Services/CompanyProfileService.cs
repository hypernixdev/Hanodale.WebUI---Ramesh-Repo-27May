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
    public class CompanyProfileService : BaseService, ICompanyProfileService
    {
        public CompanyProfileDetails GetCompanyProfileBySearch(DatatableFilters entityFilter)
        {
            CompanyProfileDetails _result = new CompanyProfileDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entityFilter == null)
                        entityFilter = new DatatableFilters();

                    
                    //get total record

                    var query = model.CompanyProfiles.Where(p => (entityFilter.all ? true : p.isActive ?? false));

                    _result.recordDetails.totalRecords = query.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    if (!string.IsNullOrEmpty(entityFilter.search))
                    {
                        bool c = Common.Visibility.True.ToString().ToLower().Contains(entityFilter.search.ToLower());
                        bool d = Common.Visibility.False.ToString().ToLower().Contains(entityFilter.search.ToLower());

                        query = query.Where(p => (
                           p.name.Contains(entityFilter.search)
                        || p.code.Contains(entityFilter.search)
                        ||  (p.companyType_Id==null ? false : p.ModuleItem.name.Contains(entityFilter.search))
                        || p.emailAddress.Contains(entityFilter.search)
                        || SqlFunctions.StringConvert((double)p.noOfUser).Contains(entityFilter.search)
                        || p.phoneNo.Contains(entityFilter.search)
                        || (p.service_Id == null ? false : p.ModuleItem1.name.Contains(entityFilter.search))
                        || SqlFunctions.StringConvert((double)p.totalCapital).Contains(entityFilter.search)
                        || SqlFunctions.StringConvert((double)p.totalRevenue).Contains(entityFilter.search)
                        || p.description.Contains(entityFilter.search)
                        || (c ? p.isActive == true : d ? p.isActive == false : false)));
                    }

                    var result = query.OrderByDescending(p => p.id)
                        .Select(p => new CompanyProfiles
                        {
                            id = p.id,
                            name = p.name,
                            code = p.code,
                            companyType_Id = p.companyType_Id,
                            companyTypeName= (p.companyType_Id==null? "": p.ModuleItem.name),
                            effectiveDate = p.effectiveDate,
                            emailAddress = p.emailAddress,
                            noOfUser = p.noOfUser,
                            phoneNo = p.phoneNo,
                            service_Id = p.service_Id,
                            serviceName = (p.service_Id == null ? "" : p.ModuleItem1.name),
                            totalCapital = p.totalCapital,
                            totalRevenue = p.totalRevenue,
                            description = p.description,
                            isActive = p.isActive,
                        });

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count();
                    _result.lstCompanyProfile = result.Skip(entityFilter.startIndex).Take(entityFilter.pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public CompanyProfiles CreateCompanyProfile(CompanyProfiles entityEn)
        {
            CompanyProfile _companyProfileEn = new CompanyProfile();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new stock

                    _companyProfileEn.name = entityEn.name;
                    _companyProfileEn.code = entityEn.code;
                    _companyProfileEn.companyType_Id = entityEn.companyType_Id;
                    _companyProfileEn.effectiveDate = entityEn.effectiveDate;
                    _companyProfileEn.emailAddress = entityEn.emailAddress;
                    _companyProfileEn.noOfUser = entityEn.noOfUser;
                    _companyProfileEn.phoneNo = entityEn.phoneNo;
                    _companyProfileEn.service_Id = entityEn.service_Id;
                    _companyProfileEn.totalCapital = entityEn.totalCapital;
                    _companyProfileEn.totalRevenue = entityEn.totalRevenue;
                    _companyProfileEn.description = entityEn.description;
                    _companyProfileEn.isActive = entityEn.isActive;

                    model.CompanyProfiles.Add(_companyProfileEn);
                    model.SaveChanges();

                    entityEn.id = _companyProfileEn.id;
                    entityEn.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entityEn;
        }

        public CompanyProfiles UpdateCompanyProfile(CompanyProfiles entityEn)
        {
            CompanyProfile _companyProfileEn = new CompanyProfile();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update stock
                    _companyProfileEn = model.CompanyProfiles.SingleOrDefault(p => p.id == entityEn.id);
                    if (_companyProfileEn != null)
                    {
                        _companyProfileEn.name = entityEn.name;
                        _companyProfileEn.code = entityEn.code;
                        _companyProfileEn.companyType_Id = entityEn.companyType_Id;
                        _companyProfileEn.effectiveDate = entityEn.effectiveDate;
                        _companyProfileEn.emailAddress = entityEn.emailAddress;
                        _companyProfileEn.noOfUser = entityEn.noOfUser;
                        _companyProfileEn.phoneNo = entityEn.phoneNo;
                        _companyProfileEn.service_Id = entityEn.service_Id;
                        _companyProfileEn.totalCapital = entityEn.totalCapital;
                        _companyProfileEn.totalRevenue = entityEn.totalRevenue;
                        _companyProfileEn.description = entityEn.description;
                        _companyProfileEn.isActive = entityEn.isActive;
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
        
        public bool DeleteCompanyProfile(int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    CompanyProfile _companyProfileEn = model.CompanyProfiles.SingleOrDefault(p => p.id == id);

                    if (_companyProfileEn != null)
                    {
                        model.CompanyProfiles.Remove(_companyProfileEn);
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
        
        public CompanyProfiles GetCompanyProfileById(int id)
        {
            CompanyProfiles _companyProfileEn = new CompanyProfiles();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.CompanyProfiles.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _companyProfileEn = new CompanyProfiles
                        {
                            id = entity.id,
                            name = entity.name,
                            code = entity.code,
                            companyType_Id = entity.companyType_Id,
                            companyTypeName = (entity.companyType_Id == null ? "" : entity.ModuleItem.name),
                            effectiveDate = entity.effectiveDate,
                            emailAddress = entity.emailAddress,
                            noOfUser = entity.noOfUser,
                            phoneNo = entity.phoneNo,
                            service_Id = entity.service_Id,
                            serviceName = (entity.service_Id == null ? "" : entity.ModuleItem1.name),
                            totalCapital = entity.totalCapital,
                            totalRevenue = entity.totalRevenue,
                            description = entity.description,
                            isActive = entity.isActive,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _companyProfileEn;
        }

        public bool IsCompanyProfileExists(CompanyProfiles entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.CompanyProfiles.Any(p => p.name == entityEn.name && p.code == entityEn.code && (entityEn.id == 0 ? true : p.id != entityEn.id));
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
