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
    public class BusinessService : IBusinessService
    {
        #region Business

        public List<Businesses> GetAllBusiness(int currentUserId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var result = model.Businesses.OrderByDescending(p => p.modifiedDate)
                                   .Select(p => new Businesses
                                   {
                                       id = p.id,
                                       name = p.name,
                                       code = p.code,
                                       phone = p.phone,
                                       phone2 = p.phone2,
                                       fax = p.fax,
                                       webSite = p.webSite,
                                       primaryContact = p.primaryContact,
                                       primaryEmail = p.primaryEmail,
                                       primaryCurrency = p.primaryCurrency,
                                       remarks = p.remarks,
                                       status = p.status,
                                       createdBy = p.createdBy,
                                       createdDate = p.createdDate,
                                       modifiedBy = p.modifiedBy,
                                       modifiedDate = p.modifiedDate
                                   }).ToList();

                    return result;
                }



            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public BusinessDetails GetBusinessBySearch(int currentUserId, bool all, int startIndex, int pageSize, string search, Businesses filterModel, int organization_Id)
        {
            BusinessDetails _result = new BusinessDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                bool hasFilter = false;
                bool hascustomSearch = false;
                int[] workCategoryId = null;
                Businesses filter = new Businesses();
                if (filterModel != null)
                {
                    hascustomSearch = true;
                    filter = (Businesses)filterModel;
                    if (filterModel.workCategoryIds != null)
                    {
                        filter.isActive = true;
                        workCategoryId = filterModel.workCategoryIds;
                        hasFilter = true;
                    }
                    else
                    {
                        all = true;
                    }
                }

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var user = model.Users.Include("UserRole").FirstOrDefault(p => p.id == currentUserId);
                    bool hasUser = false;
                    int buss_Id = 0;
                    if (user != null)
                    {
                        hasUser = true;
                        //buss_Id = user.bussiness_Id;
                    }

                    if (!all && hasFilter)
                    {
                        
                        //get total record
                        var businessIds = model.BusinessWorkCategories.Include("WorkCategories").Where(p => workCategoryId.Contains(p.workCategory_Id)).Select(p => p.business_Id).ToList();
                        var bussIds = model.BusinessOrganizations.Include("Organization").Where(a => businessIds.Contains(a.business_Id) && a.organization_Id == organization_Id).Select(p => p.business_Id).ToList();
                        _result.recordDetails.totalRecords = model.Businesses.Include("User").Include("ModuleItem").Where(p => bussIds.Contains(p.id) && p.businessType_Id == filter.supplierBusinessType_Id).Count();
                        //_result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                        //Filtered count
                        var result = model.Businesses.Include("User").Include("ModuleItem").OrderByDescending(p => p.modifiedDate).Where(p => (p.status == filter.isActive) && bussIds.Contains(p.id) && p.businessType_Id == filter.supplierBusinessType_Id && string.IsNullOrEmpty(search) ? true : (p.name.Contains(search)
                                       || p.code.Contains(search)
                                       || p.phone.Contains(search)
                                       || p.primaryContact.Contains(search)
                                       || p.primaryEmail.Contains(search)
                                       || p.ModuleItem.name.Contains(search)))
                            // .Skip(startIndex).Take(pageSize)
                                       .Select(p => new Businesses
                                       {
                                           id = p.id,
                                           name = p.name,
                                           code = p.code,
                                           phone = p.phone,
                                           businessTypeName = p.ModuleItem.name,
                                           phone2 = p.phone2,
                                           fax = p.fax,
                                           webSite = p.webSite,
                                           primaryContact = p.primaryContact,
                                           primaryEmail = p.primaryEmail,
                                           primaryCurrency = p.primaryCurrency,
                                           remarks = p.remarks,
                                           status = p.status,
                                           businessType_Id = p.businessType_Id,
                                           profileLastUpdated = p.profileLastUpdated,
                                           createdBy = p.createdBy,
                                           createdDate = p.createdDate,
                                           modifiedBy = p.modifiedBy,
                                           modifiedDate = p.modifiedDate,
                                           statusColor = (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Active" && a.bussinessType_Id == 52) > 0) ? 1 : (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Inactive" && a.bussinessType_Id == 52) > 0 ? 2 : 0)
                                       }).ToList();

                        //Get filter data
                        _result.recordDetails.totalDisplayRecords = result.Count;
                        _result.lstBusiness = result.Skip(startIndex).Take(pageSize).ToList();
                    }
                    else
                    {
                        //get total record
                        _result.recordDetails.totalRecords = model.Businesses.Include("User").Include("ModuleItem").Where(p => ((all && hasUser) ? (p.id == buss_Id && p.businessType_Id == filter.supplierBusinessType_Id) : (all && !hasUser) ? (p.businessType_Id == filter.supplierBusinessType_Id) : true)).Count();
                        _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                        //Filtered count
                        var result = model.Businesses.Include("User").Include("ModuleItem").OrderByDescending(p => p.modifiedDate).Where(p => (filter.isActive ? p.status == true : p.status == false) && (hascustomSearch ? ((string.IsNullOrEmpty(filter.code) ? true : p.code.Contains(filter.code)) && (string.IsNullOrEmpty(filter.name) ? true : p.name.Contains(filter.name)) && (filter.businessType_Id == 0 ? true : p.businessType_Id == filter.businessType_Id)) : true) && ((all && hasUser) ? (p.id == buss_Id && p.businessType_Id == filter.supplierBusinessType_Id) : (all && !hasUser) ? (p.businessType_Id == filter.supplierBusinessType_Id) : true)
                            && (string.IsNullOrEmpty(search) ? true : (p.name.Contains(search)
                                       || p.code.Contains(search)
                                       || p.phone.Contains(search)
                                       || p.primaryContact.Contains(search)
                                       || p.primaryEmail.Contains(search)
                                       || p.ModuleItem.name.Contains(search))));


                        //Get filter data
                        _result.recordDetails.totalDisplayRecords = result.Count();
                        _result.lstBusiness = result.Skip(startIndex).Take(pageSize)
                                       .Select(p => new Businesses
                                       {
                                           id = p.id,
                                           name = p.name,
                                           code = p.code,
                                           phone = p.phone,
                                           businessTypeName = p.ModuleItem.name,
                                           phone2 = p.phone2,
                                           fax = p.fax,
                                           webSite = p.webSite,
                                           primaryContact = p.primaryContact,
                                           primaryEmail = p.primaryEmail,
                                           primaryCurrency = p.primaryCurrency,
                                           remarks = p.remarks,
                                           status = p.status,
                                           businessType_Id = p.businessType_Id,
                                           profileLastUpdated = p.profileLastUpdated,
                                           createdBy = p.createdBy,
                                           createdDate = p.createdDate,
                                           modifiedBy = p.modifiedBy,
                                           modifiedDate = p.modifiedDate,
                                           //statusColor = (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Active") > 0) ? 1 : (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Inactive") > 0 ? 2 : 0)
                                           statusColor = (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Active" && a.bussinessType_Id == 52) > 0) ? 1 : (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Inactive" && a.bussinessType_Id == 52) > 0 ? 2 : 0)
                                       }).ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public Businesses CreateBusiness(int currentUserId, Businesses entity, string pageName)
        {
            Business _entity = new Business();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Business

                    _entity.name = entity.name;
                    _entity.businessType_Id = entity.businessType_Id;
                    _entity.code = entity.code;
                    _entity.phone = entity.phone;
                    _entity.phone2 = entity.phone2;
                    _entity.fax = entity.fax;
                    _entity.webSite = entity.webSite;
                    _entity.primaryContact = entity.primaryContact;
                    _entity.primaryEmail = entity.primaryEmail;
                    _entity.primaryCurrency = entity.primaryCurrency == 0 ? null : entity.primaryCurrency;
                    _entity.remarks = entity.remarks;
                    _entity.status = entity.status;
                    _entity.profileLastUpdated = entity.profileLastUpdated;
                    _entity.createdBy = entity.createdBy;
                    _entity.createdDate = entity.createdDate;
                    _entity.modifiedBy = entity.modifiedBy;
                    _entity.modifiedDate = entity.modifiedDate;
                    _entity.isMAHBRegistered = entity.isMAHBRegistered;
                    _entity.registrationNo = entity.registrationNo;
                    _entity.registrationDate = entity.registrationDate;
                    _entity.expiryDate = entity.expiryDate;
                    _entity.referenceId = entity.referenceId;
                    _entity.rocNo = entity.rocNo;
                    _entity.businessCategory = entity.businessCategory;
                    _entity.paidUpCapital = entity.paidUpCapital;
                    _entity.companyType = entity.companyType;
                    _entity.address = entity.address;
                    _entity.limitation = entity.limitation;



                    model.Businesses.Add(_entity);
                    model.SaveChanges();

                    if (entity.businessType_Id == 52)
                    {
                        BusinessOther others = new BusinessOther();
                        others.business_Id = _entity.id;
                        others.createdBy = _entity.createdBy;
                        others.createdDate = _entity.createdDate;
                        model.BusinessOthers.Add(others);

                        BusinessAddress address = new BusinessAddress();
                        address.business_Id = _entity.id;
                        address.createdBy = _entity.createdBy;
                        address.createdDate = _entity.createdDate;
                        model.BusinessAddresses.Add(address);
                        model.SaveChanges();

                    }

                    List<int> classificationIds = model.BusinessClassifications.Where(p => p.business_Id == entity.id).Select(p => p.classification_Id).ToList();
                    //
                    if (entity.classification_Ids != null)
                    {
                        foreach (var item in entity.classification_Ids)
                        {
                            if (classificationIds.Contains(item))
                            {
                                classificationIds.Remove(item);
                            }
                            else
                            {
                                if (item != 0)
                                {
                                    BusinessClassification _entityClassification = new BusinessClassification();
                                    _entityClassification.business_Id = _entity.id;
                                    _entityClassification.classification_Id = item;
                                    _entityClassification.createdBy = entity.modifiedBy;
                                    _entityClassification.createdDate = Convert.ToDateTime(entity.modifiedDate);
                                    _entityClassification.modifiedBy = entity.modifiedBy;
                                    _entityClassification.modifiedDate = entity.modifiedDate;
                                    model.BusinessClassifications.Add(_entityClassification);
                                    model.SaveChanges();
                                }
                            }
                        }
                    }


                    if (entity.workCategoryIds != null)
                    {
                        List<int> ids = model.BusinessWorkCategories.Where(p => p.business_Id == _entity.id).Select(p => p.workCategory_Id).ToList();
                        foreach (var items in entity.workCategoryIds)
                        {
                            if (ids.Contains(items))
                            {
                                ids.Remove(items);
                            }
                            else
                            {
                                if (items != 0)
                                {
                                    BusinessWorkCategory _businessWorkCategory = new BusinessWorkCategory();
                                    _businessWorkCategory.business_Id = _entity.id;
                                    _businessWorkCategory.workCategory_Id = items;
                                    _businessWorkCategory.createdBy = entity.createdBy;
                                    _businessWorkCategory.createdDate = DateTime.Now;
                                    model.BusinessWorkCategories.Add(_businessWorkCategory);
                                    model.SaveChanges();
                                }
                            }
                        }


                    }

                    if (entity.organisationIds != null)
                    {
                        List<int> ids = model.BusinessOrganizations.Where(p => p.business_Id == entity.id).Select(p => p.organization_Id).ToList();
                        BusinessOrganization _entitys = new BusinessOrganization();
                        foreach (var items in entity.organisationIds)
                        {
                            if (ids.Contains(items))
                            {
                                ids.Remove(items);
                            }
                            else
                            {
                                if (items != 0)
                                {
                                    _entitys.business_Id = _entity.id;
                                    _entitys.organization_Id = items;
                                    _entitys.createdBy = _entity.createdBy;
                                    _entitys.createdDate = DateTime.Now;
                                    model.BusinessOrganizations.Add(_entitys);
                                    model.SaveChanges();
                                }
                            }
                        }
                    }

                    entity.id = _entity.id;

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return entity;
        }

        public Businesses UpdateBusiness(int currentUserId, Businesses entity, string pageName)
        {
            Business _entity = new Business();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    // update Business

                    _entity = model.Businesses.SingleOrDefault(p => p.id == entity.id);

                    if (_entity != null)
                    {
                        _entity.businessType_Id = entity.businessType_Id;
                        _entity.name = entity.name;
                        _entity.phone = entity.phone;
                        _entity.phone2 = entity.phone2;
                        _entity.fax = entity.fax;
                        _entity.webSite = entity.webSite;
                        _entity.primaryContact = entity.primaryContact;
                        _entity.primaryEmail = entity.primaryEmail;
                        _entity.primaryCurrency = entity.primaryCurrency == 0 ? null : entity.primaryCurrency;
                        _entity.remarks = entity.remarks;
                        _entity.status = true;
                        _entity.modifiedBy = entity.modifiedBy;
                        _entity.modifiedDate = entity.modifiedDate;
                        _entity.isMAHBRegistered = entity.isMAHBRegistered;
                        _entity.registrationNo = entity.registrationNo;
                        _entity.registrationDate = entity.registrationDate;
                        _entity.expiryDate = entity.expiryDate;
                        _entity.referenceId = entity.referenceId;
                        _entity.rocNo = entity.rocNo;
                        _entity.businessCategory = entity.businessCategory;
                        _entity.paidUpCapital = entity.paidUpCapital;
                        _entity.companyType = entity.companyType;
                        _entity.address = entity.address;
                        _entity.limitation = entity.limitation;
                        _entity.profileLastUpdated = entity.profileLastUpdated;
                        _entity.gstNo = entity.gstNo;

                    }
                    model.SaveChanges();
                    // insert and  update BusinessClassification
                    List<int> ids = model.BusinessClassifications.Where(p => p.business_Id == entity.id).Select(p => p.classification_Id).ToList();

                    //
                    if (entity.classification_Ids != null)
                    {
                        foreach (var item in entity.classification_Ids)
                        {
                            if (ids.Contains(item))
                            {
                                ids.Remove(item);
                            }
                            else
                            {
                                if (item != 0)
                                {
                                    BusinessClassification _entityClassification = new BusinessClassification();
                                    _entityClassification.business_Id = entity.id;
                                    _entityClassification.classification_Id = item;
                                    _entityClassification.createdBy = entity.modifiedBy;
                                    _entityClassification.createdDate = Convert.ToDateTime(entity.modifiedDate);
                                    _entityClassification.modifiedBy = entity.modifiedBy;
                                    _entityClassification.modifiedDate = entity.modifiedDate;
                                    model.BusinessClassifications.Add(_entityClassification);
                                }
                            }
                        }

                        foreach (var item in ids)
                        {
                            var _entityClassification = model.BusinessClassifications.SingleOrDefault(p => p.classification_Id == item && p.business_Id == entity.id);
                            if (_entity != null)
                            {
                                model.BusinessClassifications.Remove(_entityClassification);
                            }
                        }
                    }
                    
                    List<int> workCategoryIds = model.BusinessWorkCategories.Where(p => p.business_Id == _entity.id).Select(p => p.workCategory_Id).ToList();
                    if (entity.workCategoryIds != null)
                    {
                        foreach (var items in entity.workCategoryIds)
                        {
                            if (workCategoryIds.Contains(items))
                            {
                                workCategoryIds.Remove(items);
                            }
                            else
                            {
                                if (items != 0)
                                {
                                    BusinessWorkCategory _businessWorkCategory = new BusinessWorkCategory();
                                    _businessWorkCategory.business_Id = _entity.id;
                                    _businessWorkCategory.workCategory_Id = items;
                                    _businessWorkCategory.createdBy = entity.modifiedBy;
                                    _businessWorkCategory.createdDate = DateTime.Now;
                                    model.BusinessWorkCategories.Add(_businessWorkCategory);
                                    model.SaveChanges();
                                }
                            }
                        }
                        foreach (var item in workCategoryIds)
                        {
                            var _entityClassification = model.BusinessWorkCategories.SingleOrDefault(p => p.workCategory_Id == item && p.business_Id == entity.id);
                            if (_entity != null)
                            {
                                model.BusinessWorkCategories.Remove(_entityClassification);
                            }
                        }

                    }
                    
                    List<int> organization_Ids = model.BusinessOrganizations.Where(p => p.business_Id == entity.id).Select(p => p.organization_Id).ToList();
                    if (entity.organisationIds != null)
                    {
                        BusinessOrganization _entitys = new BusinessOrganization();
                        foreach (var items in entity.organisationIds)
                        {
                            if (organization_Ids.Contains(items))
                            {
                                organization_Ids.Remove(items);
                            }
                            else
                            {
                                if (items != 0)
                                {
                                    _entitys.business_Id = _entity.id;
                                    _entitys.organization_Id = items;
                                    _entitys.createdBy = _entity.modifiedBy;
                                    _entitys.createdDate = DateTime.Now;
                                    model.BusinessOrganizations.Add(_entitys);
                                    model.SaveChanges();
                                }
                            }
                        }
                        foreach (var item in organization_Ids)
                        {
                            var _entityClassification = model.BusinessOrganizations.Where(p => p.organization_Id == item && p.business_Id == entity.id).ToList();
                            if (entity != null)
                            {
                                foreach (var _classification in _entityClassification)
                                {
                                    model.BusinessOrganizations.Remove(_classification);
                                }
                            }
                        }
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

        public bool DeleteBusiness(int currentUserId, int id, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    Business _entity = model.Businesses.SingleOrDefault(p => p.id == id);

                    if (_entity != null)
                    {
                        model.Businesses.Remove(_entity);
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

        public Businesses GetBusinessById(int id)
        {
            Businesses _entity = new Businesses();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Businesses.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _entity = new Businesses
                        {
                            id = entity.id,
                            businessType_Id = entity.businessType_Id,
                            name = entity.name,
                            code = entity.code,
                            phone = entity.phone,
                            phone2 = entity.phone2,
                            fax = entity.fax,
                            webSite = entity.webSite,
                            primaryContact = entity.primaryContact,
                            primaryEmail = entity.primaryEmail,
                            primaryCurrency = entity.primaryCurrency,
                            remarks = entity.remarks,
                            status = entity.status,
                            isMAHBRegistered = entity.isMAHBRegistered,
                            registrationNo = entity.registrationNo,
                            registrationDate = entity.registrationDate,
                            expiryDate = entity.expiryDate,
                            referenceId = entity.referenceId,
                            rocNo = entity.rocNo,
                            businessCategory = entity.businessCategory,
                            paidUpCapital = entity.paidUpCapital,
                            companyType = entity.companyType,
                            address = entity.address,
                            limitation = entity.limitation,
                            profileLastUpdated = entity.profileLastUpdated,
                            gstNo = entity.gstNo,
                            statusColor = (entity.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Active" && a.bussinessType_Id == 52) > 0) ? 1 : (entity.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Inactive" && a.bussinessType_Id == 52) > 0 ? 2 : 0)
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

        public List<Businesses> GetListBusiness(int businessTypeId = 0)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    return model.Businesses.Where(p => businessTypeId == 0 && p.status == true ? true : p.businessType_Id == businessTypeId && p.status == true).Select(p => new Businesses
                    {
                        id = p.id,
                        businessType_Id = p.businessType_Id,
                        name = p.name,
                        code = p.code,
                        phone = p.phone,
                        phone2 = p.phone2,
                        fax = p.fax,
                        webSite = p.webSite,
                        primaryContact = p.primaryContact,
                        primaryEmail = p.primaryEmail,
                        primaryCurrency = p.primaryCurrency,
                        remarks = p.remarks,
                        status = p.status,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,
                        isMAHBRegistered = p.isMAHBRegistered,
                        registrationNo = p.registrationNo,
                        registrationDate = p.registrationDate,
                        expiryDate = p.expiryDate,
                        referenceId = p.referenceId,
                        rocNo = p.rocNo,
                        businessCategory = p.businessCategory,
                        paidUpCapital = p.paidUpCapital,
                        companyType = p.companyType,
                        address = p.address,
                        limitation = p.limitation,
                        profileLastUpdated = p.profileLastUpdated,
                        statusColor = (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Active") > 0) ? 1 : (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Inactive") > 0 ? 2 : 0)

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool IsBusinessExists(Businesses entity)
        {
            Business _entity = new Business();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.id == 0)
                    {
                        _entity = model.Businesses.SingleOrDefault(p => p.name == entity.name && p.businessType_Id == entity.businessType_Id);
                        if (_entity != null)
                            isExists = true;
                    }
                    else
                    {
                        _entity = model.Businesses.SingleOrDefault(p => p.name == entity.name && p.businessType_Id == entity.businessType_Id && p.id != entity.id);
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

        public List<Businesses> GetBusinessbybusinessType()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    return model.Businesses.Where(p => p.businessType_Id == 65 && p.status == true).Select(p => new Businesses
                    {
                        id = p.id,
                        businessType_Id = p.businessType_Id,
                        name = p.name,
                        code = p.code,
                        phone = p.phone,
                        phone2 = p.phone2,
                        fax = p.fax,
                        webSite = p.webSite,
                        primaryContact = p.primaryContact,
                        primaryEmail = p.primaryEmail,
                        primaryCurrency = p.primaryCurrency,
                        remarks = p.remarks,
                        status = p.status,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,
                        isMAHBRegistered = p.isMAHBRegistered,
                        registrationNo = p.registrationNo,
                        registrationDate = p.registrationDate,
                        expiryDate = p.expiryDate,
                        referenceId = p.referenceId,
                        rocNo = p.rocNo,
                        businessCategory = p.businessCategory,
                        paidUpCapital = p.paidUpCapital,
                        companyType = p.companyType,
                        address = p.address,
                        limitation = p.limitation,
                        profileLastUpdated = p.profileLastUpdated,

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public Businesses GetBusinessDetailsByUserId(int user_Id)
        {
            Businesses _entity = new Businesses();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var user = model.Users.SingleOrDefault(p => p.id == user_Id);
                    var entity = model.Businesses.SingleOrDefault(p => p.id == user.bussiness_Id && p.businessType_Id == 68);
                    if (entity != null)
                    {
                        _entity = new Businesses
                        {
                            id = entity.id,
                            businessType_Id = entity.businessType_Id,
                            name = entity.name,
                            code = entity.code,
                            phone = entity.phone,
                            phone2 = entity.phone2,
                            fax = entity.fax,
                            webSite = entity.webSite,
                            primaryContact = entity.primaryContact,
                            primaryEmail = entity.primaryEmail,
                            primaryCurrency = entity.primaryCurrency,
                            remarks = entity.remarks,
                            status = entity.status,
                            isMAHBRegistered = entity.isMAHBRegistered,
                            registrationNo = entity.registrationNo,
                            registrationDate = entity.registrationDate,
                            expiryDate = entity.expiryDate,
                            referenceId = entity.referenceId,
                            rocNo = entity.rocNo,
                            businessCategory = entity.businessCategory,
                            paidUpCapital = entity.paidUpCapital,
                            companyType = entity.companyType,
                            address = entity.address,
                            limitation = entity.limitation,
                            profileLastUpdated = entity.profileLastUpdated
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

        public List<int> GetListBusinessworkCategory(int business_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.BusinessWorkCategories.Where(p => p.business_Id == business_Id).Select(p => new BusinessWorkCategorys
                    {
                        Id = p.Id,
                        workCategory_Id = p.workCategory_Id,
                        business_Id = p.business_Id,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate

                    }).Select(p => p.workCategory_Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Businesses> GetListBusinessBySubCostId(int organization_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var objbusiness = model.BusinessOrganizations.Where(u => u.organization_Id == organization_Id).Select(p => p.business_Id).ToList();

                    return model.Businesses.Where(p => objbusiness.Contains(p.id) && p.businessType_Id == 52 && p.status == true).Select(p => new Businesses
                    {
                        id = p.id,
                        name = p.name,
                        code = p.code,
                        businessType_Id = p.businessType_Id,
                        createdDate = p.createdDate

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Businesses> GetListBusinessByOrganizationId(int organizationId, int businessType_Id, bool check)
        {
            List<Businesses> lstbusiness = new List<Businesses>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var businessIds = model.BusinessOrganizations.Where(p => p.organization_Id == organizationId).Select(p => p.business_Id).ToList();
                    lstbusiness = model.Businesses.Where(p => businessIds.Contains(p.id) && p.businessType_Id == businessType_Id && (check ? p.status == true : true)).Select(p => new Businesses
                    {
                        id = p.id,
                        businessType_Id = p.businessType_Id,
                        name = p.name,
                        code = p.code,
                        phone = p.phone,
                        phone2 = p.phone2,
                        fax = p.fax,
                        webSite = p.webSite,
                        primaryContact = p.primaryContact,
                        primaryEmail = p.primaryEmail,
                        primaryCurrency = p.primaryCurrency,
                        remarks = p.remarks,
                        status = p.status,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,
                        isMAHBRegistered = p.isMAHBRegistered,
                        registrationNo = p.registrationNo,
                        registrationDate = p.registrationDate,
                        expiryDate = p.expiryDate,
                        referenceId = p.referenceId,
                        rocNo = p.rocNo,
                        businessCategory = p.businessCategory,
                        paidUpCapital = p.paidUpCapital,
                        companyType = p.companyType,
                        address = p.address,
                        limitation = p.limitation,
                        profileLastUpdated = p.profileLastUpdated

                    }).OrderBy(a => a.name).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return lstbusiness;
        }

        public List<int> GetBusinessOrganizationById(int business_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Get User List

                    return model.BusinessOrganizations.Where(p => p.business_Id == business_Id).Select(p => new BusinessOrganizations
                    {
                        id = p.id,
                        business_Id = p.business_Id,
                        organization_Id = p.organization_Id,
                    }).Select(p => p.organization_Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
        
        public BusinessDetails GetBusinessMasterBySearch(int currentUserId, int organization_Id, int startIndex, int pageSize, string search, object filterModel)
        {
            BusinessDetails _result = new BusinessDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var filter = new Businesses();

                    bool isDashboard = false;

                    if (filterModel != null)
                    {
                        filter = (Businesses)filterModel;
                        isDashboard = true;
                    }

                    _result.recordDetails.totalRecords = model.Businesses.Include("User").Include("ModuleItem").
                        Where(p => (filter.isActive ? p.status == true : p.status == false) &&
                            (isDashboard ? ((string.IsNullOrEmpty(filter.code) ? true : p.code.Contains(filter.code)) && (string.IsNullOrEmpty(filter.name) ? true : p.name.Contains(filter.name)) && (filter.businessType_Id == 0 ? true : p.businessType_Id == filter.businessType_Id)) : true) && p.BusinessOrganizations.Any(s => s.organization_Id == organization_Id)).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    var result = model.Businesses.Include("User").Include("ModuleItem").OrderByDescending(p => p.modifiedDate)
                        .Where(p => (filter.isActive ? p.status == true : p.status == false) && (isDashboard ? ((string.IsNullOrEmpty(filter.code) ? true : p.code.Contains(filter.code)) && (string.IsNullOrEmpty(filter.name) ? true : p.name.Contains(filter.name)) && (filter.businessType_Id == 0 ? true : p.businessType_Id == filter.businessType_Id)) : true) && p.BusinessOrganizations.Any(s => s.organization_Id == organization_Id) && (string.IsNullOrEmpty(search) ? true : (p.name.Contains(search)
                                   || p.code.Contains(search)
                                   || p.phone.Contains(search)
                                   || p.primaryContact.Contains(search)
                                   || p.primaryEmail.Contains(search)
                                   || p.ModuleItem.name.Contains(search)))).Skip(startIndex).Take(pageSize)
                                   .Select(p => new Businesses
                                   {
                                       id = p.id,
                                       name = p.name,
                                       code = p.code,
                                       phone = p.phone,
                                       businessTypeName = p.ModuleItem.name,
                                       phone2 = p.phone2,
                                       fax = p.fax,
                                       webSite = p.webSite,
                                       primaryContact = p.primaryContact,
                                       primaryEmail = p.primaryEmail,
                                       primaryCurrency = p.primaryCurrency,
                                       remarks = p.remarks,
                                       status = p.status,
                                       businessType_Id = p.businessType_Id,
                                       profileLastUpdated = p.profileLastUpdated,
                                       createdBy = p.createdBy,
                                       createdDate = p.createdDate,
                                       modifiedBy = p.modifiedBy,
                                       modifiedDate = p.modifiedDate,
                                       //statusColor = (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Active" && a.bussinessType_Id == 52) > 0) ? 1 : (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Inactive" && a.bussinessType_Id == 52) > 0 ? 2 : 0)
                                   }).ToList();

                    //Get filter data
                    _result.lstBusiness = result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public BusinessDetails GetBusinessMaster(int currentUserId, int organization_Id, int startIndex, int pageSize)
        {
            BusinessDetails _result = new BusinessDetails();
            _result.recordDetails = new RecordDetails();
            try
            {

                using (HanodaleEntities model = new HanodaleEntities())
                {

                  
                    var businessIds = model.BusinessOrganizations.Where(p => p.organization_Id == organization_Id).Select(p => p.business_Id).ToList();
                    _result.recordDetails.totalRecords = model.Businesses.Include("User").Include("ModuleItem").Where(p => businessIds.Contains(p.id)).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    //Filtered count
                    _result.lstBusiness = model.Businesses.Include("User").Include("ModuleItem").Where(p => businessIds.Contains(p.id)).OrderByDescending(p => p.modifiedDate)
                            .Skip(startIndex).Take(pageSize)
                            .Select(p => new Businesses
                            {
                                id = p.id,
                                name = p.name,
                                businessTypeName = p.ModuleItem.name,
                                code = p.code,
                                phone = p.phone,
                                phone2 = p.phone2,
                                fax = p.fax,
                                webSite = p.webSite,
                                primaryContact = p.primaryContact,
                                primaryEmail = p.primaryEmail,
                                primaryCurrency = p.primaryCurrency,
                                remarks = p.remarks,
                                status = p.status,
                                businessType_Id = p.businessType_Id,
                                profileLastUpdated = p.profileLastUpdated,
                                createdBy = p.createdBy,
                                createdDate = p.createdDate,
                                modifiedBy = p.modifiedBy,
                                modifiedDate = p.modifiedDate,
                                //statusColor = (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Active" && a.bussinessType_Id == 52) > 0) ? 1 : (p.Users.Count(a => a.UserRole.roleName == "Supplier" && a.status == "Inactive" && a.bussinessType_Id == 52) > 0 ? 2 : 0)
                            }).ToList();


                }


            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public List<Businesses> GetBusinessBySubCostId(int organization_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var objbusiness = model.BusinessOrganizations.Where(u => u.organization_Id == organization_Id).Select(p => p.business_Id).ToList();

                    return model.Businesses.Where(p => objbusiness.Contains(p.id) && (p.businessType_Id == 52 || p.businessType_Id == 65)).Select(p => new Businesses
                    {
                        id = p.id,
                        name = p.name,
                        code = p.code,
                        businessType_Id = p.businessType_Id,
                        createdDate = p.createdDate

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Businesses> GetListBusinessBybusinessType()
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return model.Businesses.Where(p => (p.businessType_Id == 65 || p.businessType_Id == 52) && p.status == true).Select(p => new Businesses
                    {
                        id = p.id,
                        businessType_Id = p.businessType_Id,
                        name = p.name,
                        code = p.code,
                        phone = p.phone,
                        phone2 = p.phone2,
                        fax = p.fax,
                        webSite = p.webSite,
                        primaryContact = p.primaryContact,
                        primaryEmail = p.primaryEmail,
                        primaryCurrency = p.primaryCurrency,
                        remarks = p.remarks,
                        status = p.status,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,
                        isMAHBRegistered = p.isMAHBRegistered,
                        registrationNo = p.registrationNo,
                        registrationDate = p.registrationDate,
                        expiryDate = p.expiryDate,
                        referenceId = p.referenceId,
                        rocNo = p.rocNo,
                        businessCategory = p.businessCategory,
                        paidUpCapital = p.paidUpCapital,
                        companyType = p.companyType,
                        address = p.address,
                        limitation = p.limitation,
                        profileLastUpdated = p.profileLastUpdated

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Businesses> GetBusinessSupplierBySubCostId(int organization_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var objbusiness = model.BusinessOrganizations.Where(u => u.organization_Id == organization_Id).Select(p => p.business_Id).ToList();

                    return model.Businesses.Where(p => objbusiness.Contains(p.id) && (p.businessType_Id == 52)).Select(p => new Businesses
                    {
                        id = p.id,
                        name = p.name,
                        code = p.code,
                        businessType_Id = p.businessType_Id,
                        createdDate = p.createdDate

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Businesses> GetListBusinessByBusinessTypeId(int businessType_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    return model.Businesses.Where(p => p.businessType_Id == businessType_Id).Select(p => new Businesses
                    {
                        id = p.id,
                        businessType_Id = p.businessType_Id,
                        name = p.name,
                        code = p.code,
                        phone = p.phone,
                        phone2 = p.phone2,
                        fax = p.fax,
                        webSite = p.webSite,
                        primaryContact = p.primaryContact,
                        primaryEmail = p.primaryEmail,
                        primaryCurrency = p.primaryCurrency,
                        remarks = p.remarks,
                        status = p.status,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,
                        isMAHBRegistered = p.isMAHBRegistered,
                        registrationNo = p.registrationNo,
                        registrationDate = p.registrationDate,
                        expiryDate = p.expiryDate,
                        referenceId = p.referenceId,
                        rocNo = p.rocNo,
                        businessCategory = p.businessCategory,
                        paidUpCapital = p.paidUpCapital,
                        companyType = p.companyType,
                        address = p.address,
                        limitation = p.limitation,
                        profileLastUpdated = p.profileLastUpdated

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool IsBusinessWorkCategoryandOrganisationExists(int[] workCategoryIds, int[] organisationIds)
        {
            WorkCategory _entity = new WorkCategory();
            Organization _entitys = new Organization();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if ((workCategoryIds == null && organisationIds == null) || (workCategoryIds != null && organisationIds == null) || (workCategoryIds == null && organisationIds != null))
                    {
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

        public int IsBusinessSupplierExists(Businesses entity)
        {
            Business _entity = new Business();
            int isExists = 0;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (entity.businessType_Id == 52)
                    {
                        if (entity.rocNo == null && entity.registrationNo == null && entity.registrationDate == null)
                        {
                            isExists = 1;
                        }
                        else if (entity.rocNo == null)
                        {
                            isExists = 2;
                        }
                        else if (entity.registrationNo == null)
                        {
                            isExists = 3;
                        }
                        else if (entity.registrationDate == null)
                        {
                            isExists = 4;
                        }
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

        public Users GetListBusinessUser(Users entity)
        {

            List<Users> lstUser = new List<Users>();
            Users _userEn = new Users();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var business = model.Businesses.SingleOrDefault(p => p.id == entity.defaultbusiness_Id);

                    var user = model.Users.SingleOrDefault(p => p.id == entity.id);

                    bool businessuser = model.AssignedBusinesses.Any(p => p.user_Id == user.id && p.business_Id == business.id);
                    if (businessuser)
                    {
                        _userEn.id = user.id;
                        _userEn.status = user.status;
                        _userEn.bussinessType_Id = user.bussinessType_Id;
                        _userEn.address = user.address;
                        _userEn.email = user.email;
                        _userEn.businessName = business.name;
                        _userEn.userName = user.userName;
                        _userEn.passwordHash = "123456";
                    }
                    return _userEn;
                }
            }

            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        #endregion
    }
}
