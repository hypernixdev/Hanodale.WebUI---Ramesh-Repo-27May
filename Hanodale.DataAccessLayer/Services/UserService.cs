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
    public class UserService : IUserService
    {
        #region User

        public UserDetails GetUserBySearch(int currentUserId, int businessId, int startIndex, int pageSize, string search, int businessTypeId, int organization_Id, bool all, bool isActive)
        {
            search = search.Trim();
            UserDetails _result = new UserDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var userIds = model.AssignedOrganizations.Where(p => p.organization_Id == organization_Id).Select(p => p.user_Id).ToList();
                    //get total record
                    _result.recordDetails.totalRecords = model.Users.Include("UserRole").Include("Business").Where(p => all ? userIds.Contains(p.id) : businessId != 0 ? (p.bussiness_Id == businessId && p.bussinessType_Id == businessTypeId) : p.bussinessType_Id == businessTypeId &&  (search == "InActive" ? p.status == "InActive" : p.status == "Active")).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;


                    //Filtered count
                    var result = model.Users.Include("UserRole").Include("Business").OrderByDescending(p => p.modifiedDate).Where(p => (all ? userIds.Contains(p.id) : businessId != 0 ? (p.bussiness_Id == businessId 
                        && p.bussinessType_Id == businessTypeId) : p.bussinessType_Id == businessTypeId)
                        && (isActive ? p.status =="Active" : p.status == "InActive")
                        && (p.userName.Contains(search)
                                   || p.email.Contains(search)
                                   || p.firstName.Contains(search)
                                   || p.lastName.Contains(search)
                                   || (p.firstName + " " + p.lastName).Contains(search)
                                   || p.UserRole.roleName.Contains(search)
                                   || p.Business.name.Contains(search)
                                   || p.employeeNo.Contains(search)
                                   || p.jobTitle.Contains(search)
                                   || p.status.Contains(search)
                                   )).ToList().Select(p => new Users
                                   {
                                       id = p.id,
                                       firstName = p.firstName,
                                       lastName = p.lastName,
                                       email = p.email,
                                       employeeNo = p.employeeNo,
                                       jobTitle = p.jobTitle,
                                       userName = p.userName,
                                       createdBy = p.createdBy,
                                       status = p.status,
                                       roleName = p.UserRole.roleName,
                                       businessName = p.Business.name,
                                       bussinessType_Id = p.bussinessType_Id,
                                       gred = p.gred,
                                       idNo = p.idNo,
                                       birthDate = p.birthDate,
                                       age = p.age,
                                       accountNo = p.accountNo,
                                       bankId = p.bankId,
                                       salary = p.salary,
                                       employeegroupId = p.employeegroupId,
                                       entryDate = p.entryDate,
                                       expireddate = p.expireddate,
                                       yearofservice = p.yearofservice

                                   }).ToList();
                    //Get filtered cound
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstUsers = result.Skip(startIndex).Take(pageSize).ToList();

                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public UserDetails GetUser(int currentUserId, int businessId, int startIndex, int pageSize, int businessTypeId, int organization_Id, bool all, bool isActive)
        {
            UserDetails _result = new UserDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var userIds = model.AssignedOrganizations.Where(p => p.organization_Id == organization_Id && (isActive ? p.User.status == "Active" : p.User.status == "InActive")).Select(p => p.user_Id).ToList();

                    //get total record
                    _result.recordDetails.totalRecords = model.Users.Include("UserRole").Include("Business").Where(p => (all || businessId==0) ? userIds.Contains(p.id) : businessId != 0 ? (p.bussiness_Id == businessId && p.bussinessType_Id == businessTypeId) : p.bussinessType_Id == businessTypeId ).Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;


                    _result.lstUsers = model.Users.Include("UserRole").Include("Business").Where(p => (all || businessId == 0) ? userIds.Contains(p.id) : businessId != 0 ? (p.bussiness_Id == businessId 
                        && p.bussinessType_Id == businessTypeId) : p.bussinessType_Id == businessTypeId)
                      //    && isActive ? p.status =="Active" : p.status == "InActive")
                        .OrderByDescending(p => p.modifiedDate)
                        .Select(p => new Users
                    {
                        id = p.id,
                        firstName = p.firstName,
                        lastName = p.lastName,
                        email = p.email,
                        employeeNo = p.employeeNo,
                        jobTitle = p.jobTitle,
                        userName = p.userName,
                        createdBy = p.createdBy,
                        status = p.status,
                        roleName = p.UserRole.roleName,
                        businessName = p.Business.name,
                        gred = p.gred,
                        bussinessType_Id = p.bussinessType_Id,
                        idNo = p.idNo,
                        birthDate = p.birthDate,
                        age = p.age,
                        accountNo = p.accountNo,
                        bankId = p.bankId,
                        salary = p.salary,
                        employeegroupId = p.employeegroupId,
                        entryDate = p.entryDate,
                        expireddate = p.expireddate,
                        yearofservice = p.yearofservice
                    }).ToList();

                    //Get filtered cound

                    _result.recordDetails.totalDisplayRecords = _result.lstUsers.Count;
                    _result.lstUsers = _result.lstUsers.Skip(startIndex).Take(pageSize).ToList();
                }


            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public Users CreateUser(int currentUserId, Users userEn)
        {
            User _userEn = new User();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _userEn.userRole_Id = userEn.userRole_Id;
                    _userEn.bussiness_Id = 1591;
                    _userEn.bussinessType_Id = 65;
                    _userEn.firstName = userEn.firstName;
                    _userEn.lastName = userEn.lastName;
                    _userEn.email = userEn.email;
                    _userEn.userName = userEn.userName;
                    _userEn.passwordHash = userEn.passwordHash;
                    _userEn.employeeNo = userEn.employeeNo;
                    _userEn.jobTitle = userEn.jobTitle;
                    _userEn.address = userEn.address;
                    _userEn.department = userEn.department;
                    _userEn.mobileNo = userEn.mobileNo;
                    _userEn.officeNo = userEn.officeNo;
                    _userEn.salaryId = userEn.salaryId;
                    _userEn.status = userEn.status;
                    _userEn.gred = userEn.gred;
                    _userEn.idNo = userEn.idNo;
                    //_userEn.bussinessType_Id = userEn.bussinessType_Id;
                    _userEn.birthDate = userEn.birthDate;
                    _userEn.age = userEn.age;
                    _userEn.accountNo = userEn.accountNo;
                    _userEn.bankId = userEn.bankId;
                    _userEn.salary = userEn.salary;
                    _userEn.employeegroupId = userEn.employeegroupId;
                    _userEn.entryDate = userEn.entryDate;
                    _userEn.expireddate = userEn.expireddate;
                    _userEn.yearofservice = userEn.yearofservice;
                    _userEn.createdBy = userEn.createdBy;
                    _userEn.createdDate = userEn.createdDate;
                    _userEn.verified = userEn.verified;
                    _userEn.modifiedBy = userEn.modifiedBy;
                    _userEn.modifiedDate = userEn.modifiedDate;
                    if (userEn.organization_Ids != null)
                    {
                        foreach (var item in userEn.organization_Ids)
                        {
                            if (item == 0)
                            {
                                _userEn.isAccessAllOrganization = true;
                            }
                            else
                            {
                                _userEn.isAccessAllOrganization = false;
                            }
                        }
                    }

                    model.Users.Add(_userEn);

                    var _mainCostCenterId = model.Organizations.SingleOrDefault(p => p.id == userEn.subCost_Id).parent_Id;

                    if (userEn.organization_Ids != null)
                    {
                        foreach (var item in userEn.organization_Ids)
                        {
                            if (item == 0)
                            {
                                AssignedOrganization _entity = new AssignedOrganization();
                                _entity.organization_Id = userEn.defaultOrganizationId;
                                _entity.user_Id = userEn.id;
                                _entity.createdDate = userEn.createdDate;
                                _entity.createdBy = userEn.createdBy;
                                _entity.isDefault = true;
                                model.AssignedOrganizations.Add(_entity);
                            }
                            else
                            {
                                AssignedOrganization _entity = new AssignedOrganization();
                                _entity.organization_Id = item;
                                _entity.user_Id = userEn.id;
                                _entity.createdDate = userEn.createdDate;
                                _entity.createdBy = userEn.createdBy;
                                _entity.isDefault = item == userEn.defaultOrganizationId ? true : false;
                                model.AssignedOrganizations.Add(_entity);
                            }

                        }

                    }

                    if (userEn.business_Ids != null)
                    {
                        foreach (var item in userEn.business_Ids)
                        {
                            AssignedBusiness _entitys = new AssignedBusiness();
                            _entitys.business_Id = item;
                            _entitys.user_Id = userEn.id;
                            _entitys.createdDate = userEn.createdDate;
                            _entitys.createdBy = userEn.createdBy;
                            _entitys.isDefault = item == userEn.business_Id ? true : false;
                            model.AssignedBusinesses.Add(_entitys);
                        }
                    }



                    model.SaveChanges();
                    userEn.id = _userEn.id;
                    //Log User Action
                    //UserLog("User", Action.Insert.ToString(), userEn);
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return userEn;
        }

        public Users UpdateUser(int currentUserId, Users userEn)
        {
            User _userEn = new User();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _userEn = model.Users.SingleOrDefault(p => p.id == userEn.id);
                    if (_userEn != null)
                    {
                        if (userEn.isProfileImageUpload)
                        {
                            _userEn.profileImageName = userEn.urlPath;
                        }
                        else
                        {
                            _userEn.userRole_Id = userEn.userRole_Id;
                            _userEn.bussiness_Id = 1591;
                            _userEn.bussinessType_Id = userEn.bussinessType_Id;
                            _userEn.firstName = userEn.firstName;
                            _userEn.lastName = userEn.lastName;
                            _userEn.email = userEn.email;
                            _userEn.userName = userEn.userName;
                            _userEn.employeeNo = userEn.employeeNo;
                            _userEn.department = userEn.department;
                            _userEn.address = userEn.address;
                            _userEn.jobTitle = userEn.jobTitle;
                            _userEn.mobileNo = userEn.mobileNo;
                            _userEn.officeNo = userEn.officeNo;
                            _userEn.salaryId = userEn.salaryId;
                            _userEn.bussinessType_Id = userEn.bussinessType_Id;
                            _userEn.status = userEn.status;
                            _userEn.gred = userEn.gred;
                            _userEn.idNo = userEn.idNo;
                            _userEn.birthDate = userEn.birthDate;
                            _userEn.age = userEn.age;
                            _userEn.accountNo = userEn.accountNo;
                            _userEn.bankId = userEn.bankId;
                            _userEn.salary = userEn.salary;
                            _userEn.employeegroupId = userEn.employeegroupId;
                            _userEn.entryDate = userEn.entryDate;
                            _userEn.expireddate = userEn.expireddate;
                            _userEn.yearofservice = userEn.yearofservice;
                            _userEn.modifiedBy = userEn.modifiedBy;
                            _userEn.modifiedDate = userEn.modifiedDate;

                            if (userEn.organization_Ids != null)
                            {
                                foreach (var item in userEn.organization_Ids)
                                {
                                    if (item == 0)
                                    {
                                        _userEn.isAccessAllOrganization = true;
                                    }
                                    else
                                    {
                                        _userEn.isAccessAllOrganization = false;
                                    }
                                }
                            }
                            //  Assigned Business
                            //var list = model.AssignedBusinesses.Where(p => p.user_Id == userEn.id);
                            //foreach (var item in list)
                            //{
                            //    if (item.business_Id != userEn.defaultbusiness_Id)
                            //        item.isDefault = false;
                            //    else
                            //        item.isDefault = true;
                            //}

                            //List<int> Ids = list.Select(p => p.business_Id).ToList();
                            //if (userEn.business_Ids != null)
                            //{
                            //    foreach (var item in userEn.business_Ids)
                            //    {
                            //        if (Ids.Contains(item))
                            //        {
                            //            Ids.Remove(item);
                            //        }
                            //        else
                            //        {
                            //            AssignedBusiness _entitys = new AssignedBusiness();
                            //            _entitys.business_Id = item;
                            //            _entitys.user_Id = userEn.id;
                            //            _entitys.createdDate = Convert.ToDateTime(userEn.modifiedDate);
                            //            _entitys.createdBy = userEn.modifiedBy;
                            //            _entitys.isDefault = item == userEn.defaultOrganizationId ? true : false;
                            //            model.AssignedBusinesses.Add(_entitys);
                            //        }
                            //    }

                            //    foreach (var item in Ids)
                            //    {
                            //        var _entity = model.AssignedBusinesses.SingleOrDefault(p => p.user_Id == userEn.id && p.business_Id == item);
                            //        if (_entity != null)
                            //        {
                            //            model.AssignedBusinesses.Remove(_entity);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    foreach (var item in Ids)
                            //    {
                            //        var _entity = model.AssignedBusinesses.SingleOrDefault(p => p.user_Id == userEn.id && p.business_Id == item);
                            //        if (_entity != null)
                            //        {
                            //            model.AssignedBusinesses.Remove(_entity);
                            //        }
                            //    }
                            //}

                            // Assigned Organization

                            var _mainCostCenterId = model.Organizations.SingleOrDefault(p => p.id == userEn.subCost_Id).parent_Id;

                            var lst = model.AssignedOrganizations.Where(p => p.user_Id == userEn.id);// && p.isDefault == true);
                            foreach (var item in lst)
                            {
                                if (item.organization_Id != userEn.defaultOrganizationId)
                                    item.isDefault = false;
                                else
                                    item.isDefault = true;
                            }



                            List<int> ids = lst.Select(p => p.organization_Id).ToList();
                            if (userEn.organization_Ids != null)
                            {
                                foreach (var item in userEn.organization_Ids)
                                {
                                    if (item == 0)
                                    {
                                        var listAll = model.AssignedOrganizations.Where(p => p.user_Id == userEn.id);
                                        foreach (var p in listAll)
                                        {
                                            model.AssignedOrganizations.Remove(p);
                                        }
                                        AssignedOrganization _entity = new AssignedOrganization();
                                        _entity.organization_Id = userEn.defaultOrganizationId;
                                        _entity.user_Id = userEn.id;
                                        _entity.createdDate = (DateTime)userEn.modifiedDate;
                                        _entity.createdBy = (string)userEn.modifiedBy;
                                        _entity.isDefault = true;
                                        model.AssignedOrganizations.Add(_entity);
                                    }
                                    else
                                    {
                                        if (ids.Contains(item))
                                        {
                                            ids.Remove(item);
                                        }
                                        else
                                        {

                                            var lsts = model.AssignedOrganizations.SingleOrDefault(p => p.user_Id == userEn.id && p.organization_Id == item);
                                            if (lsts == null)
                                            {
                                                AssignedOrganization _entity = new AssignedOrganization();
                                                _entity.organization_Id = item;
                                                _entity.user_Id = userEn.id;
                                                _entity.createdDate = (DateTime)userEn.modifiedDate;
                                                _entity.createdBy = (string)userEn.modifiedBy;
                                                _entity.isDefault = item == userEn.defaultOrganizationId ? true : false;
                                                model.AssignedOrganizations.Add(_entity);
                                            }
                                        }
                                    }

                                }

                                foreach (var item in ids)
                                {
                                    var _entity = model.AssignedOrganizations.SingleOrDefault(p => p.user_Id == userEn.id && p.organization_Id == item);
                                    if (_entity != null)
                                    {
                                        model.AssignedOrganizations.Remove(_entity);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in ids)
                                {
                                    var _entity = model.AssignedOrganizations.SingleOrDefault(p => p.user_Id == userEn.id && p.organization_Id == item);
                                    if (_entity != null)
                                    {
                                        model.AssignedOrganizations.Remove(_entity);
                                    }
                                }
                            }
                        }

                        // Assigned Organization


                         var lst1 = model.AssignedOrganizations.Where(p => p.user_Id == userEn.id);
                         foreach (var item in lst1)
                        {
                            if (item.organization_Id != userEn.defaultOrganizationId)
                                item.isDefault = false;
                            else
                                item.isDefault = true;
                        }

                        model.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return userEn;
        }

        /// <summary>
        /// This method is to check the user is exist or not
        /// </summary>
        /// <param name="userEn"></param>
        /// <returns></returns>
        public int IsUserExists(Users userEn)
        {
            int value = 0;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (userEn.id != 0)
                    {
                        var salary = model.Users.FirstOrDefault(p => p.salaryId == userEn.salaryId && p.id != userEn.id);
                        var email = model.Users.FirstOrDefault(p => p.email == userEn.email && p.id != userEn.id);
                        var userName = model.Users.FirstOrDefault(p => p.userName == userEn.userName && p.id != userEn.id);
                        if (salary != null)
                        {
                            value = 1;
                        }
                        //if (email != null)
                        //{
                        //    value = 2;
                        //}
                        if (userName != null)
                        {
                            value = 3;
                        }
                    }
                    else
                    {
                        var salary = model.Users.FirstOrDefault(p => p.salaryId == userEn.salaryId);
                        var email = model.Users.FirstOrDefault(p => p.email == userEn.email);
                        var userName = model.Users.FirstOrDefault(p => p.userName == userEn.userName);
                        if (salary != null)
                        {
                            value = 1;
                        }
                        //if (email != null)
                        //{
                        //    value = 2;
                        //}
                        if (userName != null)
                        {
                            value = 3;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return value;
        }

        /// <summary>
        /// This method is to reset the password
        /// </summary>
        /// <param name="userEn">User Entity</param>
        /// <returns>true/false</returns>
        public Users ResetPassword(Users userEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    User _userEn = null;

                    if (userEn.id == 0 && !string.IsNullOrEmpty(userEn.email))
                        _userEn = model.Users.SingleOrDefault(p => p.email == userEn.email.Trim());
                    else
                        _userEn = model.Users.SingleOrDefault(p => p.id == userEn.id);

                    if (_userEn != null)
                    {
                        _userEn.passwordHash = userEn.passwordHash;
                        _userEn.modifiedBy = userEn.modifiedBy;
                        _userEn.modifiedDate = userEn.modifiedDate;
                        _userEn.verified = userEn.verified;

                        model.SaveChanges();
                        userEn.email = _userEn.email;
                        userEn.userName = _userEn.userName;
                        userEn.firstName = _userEn.firstName;
                    }
                    else
                    {
                        return null;
                    }

                    //Log User Action
                    //UserLog("User", Action.Update.ToString(), _userEn);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return userEn;
        }

        /// <summary>
        /// This method is to get the user by user id
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Users GetUserById(int currentUserId, int id)
        {
            Users _userEn = null;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Users.Include("UserRole").SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _userEn = new Users
                        {
                            id = entity.id,
                            firstName = entity.firstName,
                            lastName = entity.lastName,
                            email = entity.email,
                            employeeNo = entity.employeeNo,
                            jobTitle = entity.jobTitle,
                            department = entity.department,
                            address = entity.address,
                            bussinessType_Id = entity.bussinessType_Id,
                            officeNo = entity.officeNo,
                            mobileNo = entity.mobileNo,
                            userRole_Id = entity.userRole_Id,
                            //business_Id = entity.bussiness_Id,
                            verified = entity.verified,
                            roleName = entity.UserRole.roleName,
                            status = entity.status,
                            language = entity.language != null ? (int)entity.language : 0,
                            userName = entity.userName,
                            gred = entity.gred,
                            idNo = entity.idNo,
                            birthDate = entity.birthDate,
                            age = entity.age,
                            accountNo = entity.accountNo,
                            bankId = entity.bankId,
                            salary = entity.salary,
                            employeegroupId = entity.employeegroupId,
                            entryDate = entity.entryDate,
                            expireddate = entity.expireddate,
                            yearofservice = entity.yearofservice,
                            isAccessAllOrganization = entity.isAccessAllOrganization,
                            businessExpiredDate = entity.Business.expiryDate,
                            businessName = entity.Business.name,
                            urlPath = entity.profileImageName,
                            landingPage = entity.UserRole.landingPage ?? "",
                            
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _userEn;
        }

        public Users GetUserByMCId(int currentUserId, int id, int mainCostId)
        {
            Users _userEn = null;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Users.SingleOrDefault(p => p.id == id);
                    var assignedOrg = model.AssignedOrganizations.Include("Organization").Where(p => p.user_Id == id && p.Organization.parent_Id == mainCostId).ToList();

                    if (entity != null)
                    {
                        _userEn = new Users
                        {
                            id = entity.id,
                            firstName = entity.firstName,
                            lastName = entity.lastName,
                            email = entity.email,
                            employeeNo = entity.employeeNo,
                            bussinessType_Id = entity.bussinessType_Id,
                            jobTitle = entity.jobTitle,
                            department = entity.department,
                            address = entity.address,
                            officeNo = entity.officeNo,
                            mobileNo = entity.mobileNo,
                            salaryId = entity.salaryId,
                            userRole_Id = entity.userRole_Id,
                            //business_Id = entity.bussiness_Id,
                            verified = entity.verified,
                            status = entity.status,
                            gred = entity.gred,
                            idNo = entity.idNo,
                            birthDate = entity.birthDate,
                            age = entity.age,
                            accountNo = entity.accountNo,
                            bankId = entity.bankId,
                            salary = entity.salary,
                            employeegroupId = entity.employeegroupId,
                            entryDate = entity.entryDate,
                            expireddate = entity.expireddate,
                            yearofservice = entity.yearofservice,
                            language = entity.language != null ? (int)entity.language : 0,
                            userName = entity.userName,
                            isAccessAllOrganization = entity.isAccessAllOrganization,
                            // organization_Ids = hasAssignedOrg ? assignedOrg.Select(p => p.organization_Id).ToArray() : new int[0],
                            // defaultOrganizationId = hasAssignedOrg && assignedOrg.Any(p => p.isDefault) ? assignedOrg.FirstOrDefault(p => p.isDefault).organization_Id : assignedOrg.FirstOrDefault().organization_Id,
                            //organization_Ids = entity.isAccessAllOrganization == true ? model.Organizations.Where(a => a.orgCategory_Id == 2 && a.parent_Id == mainCostId).Select(a => a.id).ToArray() : entity.AssignedOrganizations.Where(p => p.Organization.parent_Id == mainCostId).Select(p => p.organization_Id).ToArray(),
                            //defaultOrganizationId = entity.AssignedOrganizations.Any(p => p.isDefault && p.Organization.parent_Id == mainCostId) ? entity.AssignedOrganizations.FirstOrDefault(p => p.isDefault && p.Organization.parent_Id == mainCostId).organization_Id : model.Organizations.FirstOrDefault(a => a.orgCategory_Id == 2 && a.parent_Id == mainCostId).id,

                            organization_Ids = entity.isAccessAllOrganization == true ?
                            model.Organizations.Where(a => a.orgCategory_Id == 2 && a.parent_Id == mainCostId).Select(a => a.id).ToArray() :
                            entity.AssignedOrganizations.Where(p => p.Organization.parent_Id == mainCostId).Select(p => p.organization_Id).ToArray(),
                            defaultOrganizationId = entity.AssignedOrganizations.Any(p => p.isDefault && p.Organization.parent_Id == mainCostId) ? entity.AssignedOrganizations.FirstOrDefault(p => p.isDefault && p.Organization.parent_Id == mainCostId).organization_Id : model.Organizations.FirstOrDefault(a => a.orgCategory_Id == 2 && a.parent_Id == mainCostId).id,

                            businessExpiredDate = entity.Business.expiryDate,
                            urlPath = entity.profileImageName
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _userEn;
        }

        public Users GetUserBySCId(int currentUserId, int id, int subCostId)
        {
            Users _userEn = null;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    bool hasAssignedBus = true;
                    var mainCostId = model.Organizations.SingleOrDefault(p => p.id == subCostId).parent_Id;
                    var entity = model.Users.SingleOrDefault(p => p.id == id);
                    
                    if (entity != null)
                    {
                        _userEn = new Users
                        {
                            id = entity.id,
                            firstName = entity.firstName,
                            lastName = entity.lastName,
                            email = entity.email,
                            employeeNo = entity.employeeNo,
                            jobTitle = entity.jobTitle,
                            department = entity.department,
                            address = entity.address,
                            officeNo = entity.officeNo,
                            mobileNo = entity.mobileNo,
                            salaryId = entity.salaryId,
                            userRole_Id = entity.userRole_Id,
                            //business_Id = entity.bussiness_Id,
                            bussinessType_Id = entity.bussinessType_Id,
                            verified = entity.verified,
                            status = entity.status,
                            gred = entity.gred != null ? entity.gred : null,
                            idNo = entity.idNo,
                            birthDate = entity.birthDate,
                            age = entity.age,
                            accountNo = entity.accountNo,
                            bankId = entity.bankId,
                            salary = entity.salary,
                            employeegroupId = entity.employeegroupId,
                            entryDate = entity.entryDate,
                            expireddate = entity.expireddate,
                            yearofservice = entity.yearofservice,
                            language = entity.language != null ? (int)entity.language : 0,
                            userName = entity.userName,
                            isAccessAllOrganization = entity.isAccessAllOrganization,
                            organization_Ids = entity.isAccessAllOrganization == true ? new int[0] : entity.AssignedOrganizations.Select(p => p.organization_Id).ToArray(),
                            defaultOrganizationId = entity.AssignedOrganizations.Any(p => p.isDefault) ? entity.AssignedOrganizations.FirstOrDefault(p => p.isDefault).organization_Id : 0,
                            business_Ids = hasAssignedBus ? entity.AssignedBusinesses.Select(p => p.business_Id).ToArray() : new int[0],
                            defaultbusiness_Id = entity.AssignedBusinesses.Any(p => p.isDefault) ? entity.AssignedBusinesses.FirstOrDefault(p => p.isDefault).business_Id : 0,
                            businessExpiredDate = entity.Business.expiryDate,
                            urlPath = entity.profileImageName
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _userEn;
        }

        /// <summary>
        ///  This method is to delete the user by id
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(int currentUserId, int id)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var businessList = model.AssignedBusinesses.Where(p => p.user_Id == id);
                    foreach (var item in businessList)
                    {
                        model.AssignedBusinesses.Remove(item);
                    }

                    var orgList = model.AssignedOrganizations.Where(p => p.user_Id == id);
                    foreach (var item in orgList)
                    {
                        model.AssignedOrganizations.Remove(item);
                    }

                    User _userEn = model.Users.SingleOrDefault(p => p.id == id);

                    if (_userEn != null)
                    {
                        model.Users.Remove(_userEn);
                    }
                    model.SaveChanges();

                    //Log User Action
                    //UserLog("User", Action.Delete.ToString(), _userEn);
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
        /// This method is to get the user by user id
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Users> GetListUserByStaff(int businessTypeId, bool check, int[] userIds)
        {
            try
            {

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (userIds == null)
                        userIds = new int[1];

                    return model.Users.Include("Business").Where(p => userIds.Contains(p.id) || p.Business.businessType_Id == businessTypeId && (check ? p.status == "Active" : true)).Select(p => new Users
                    {
                        id = p.id,
                        firstName = p.firstName,
                        lastName = p.lastName,
                        email = p.email,
                        employeeNo = p.employeeNo,
                        jobTitle = p.jobTitle,
                        department = p.department,
                        address = p.address,
                        officeNo = p.officeNo,
                        mobileNo = p.mobileNo,
                        salaryId = p.salaryId,
                        userRole_Id = p.userRole_Id,
                        bussinessType_Id = p.bussinessType_Id,
                        //business_Id = p.bussiness_Id,
                        verified = p.verified,
                        status = p.status,
                        language = p.language != null ? (int)p.language : 0,
                        userName = p.userName,
                        gred = p.gred,
                        idNo = p.idNo,
                        birthDate = p.birthDate,
                        age = p.age,
                        accountNo = p.accountNo,
                        bankId = p.bankId,
                        salary = p.salary,
                        employeegroupId = p.employeegroupId,
                        entryDate = p.entryDate,
                        expireddate = p.expireddate,
                        yearofservice = p.yearofservice,
                        isAccessAllOrganization = p.isAccessAllOrganization
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Users> GetUserBySC(int currentUserId, int subCostId)
        {
            List<Users> lstuser = new List<Users>();

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    var assignedoragnisation = model.AssignedOrganizations.Include("User").Where(p => p.organization_Id == subCostId && p.User.status == "Active").ToList();

                    foreach (var items in assignedoragnisation)
                    {
                        Users entity = new Users();
                        entity = model.Users.Where(p => p.id == items.user_Id)
                        .Select(p => new Users
                        {
                            id = p.id,
                            firstName = p.firstName,
                            lastName = p.lastName,
                            email = p.email,
                            employeeNo = p.employeeNo,
                            jobTitle = p.jobTitle,
                            userName = p.userName,
                            createdBy = p.createdBy,
                            bussinessType_Id = p.bussinessType_Id,
                            status = p.status,
                            roleName = p.UserRole.roleName,
                            businessName = p.Business.name,
                            gred = p.gred,
                            idNo = p.idNo,
                            birthDate = p.birthDate,
                            age = p.age,
                            accountNo = p.accountNo,
                            bankId = p.bankId,
                            salary = p.salary,
                            employeegroupId = p.employeegroupId,
                            entryDate = p.entryDate,
                            expireddate = p.expireddate,
                            yearofservice = p.yearofservice,
                            isAccessAllOrganization = p.isAccessAllOrganization
                        }).FirstOrDefault();

                        lstuser.Add(entity);

                    }

                    return lstuser;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

        }

        /// <summary>
        /// 1. User belongs to business where businessType = ‘Supplier’
        /// 2. User Visibility = ‘True’
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="businessType_Id"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public List<Users> GetListUserByBusinessId(int businessId, int businessType_Id, bool check, int[] userIds)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (userIds == null)
                        userIds = new int[1];
                    //var userIds = model.AssignedBusinesses.Where(p => p.business_Id == businessId).Select(p => p.user_Id).ToList();

                    return model.Users.Where(p => userIds.Contains(p.id) || p.AssignedBusinesses.Any(a => a.business_Id == businessId) && p.bussinessType_Id == businessType_Id && (check ? p.status == "Active" : true)).Select(p => new Users
                    {
                        id = p.id,
                        firstName = p.firstName,
                        lastName = p.lastName,
                        email = p.email,
                        employeeNo = p.employeeNo,
                        jobTitle = p.jobTitle,
                        department = p.department,
                        address = p.address,
                        officeNo = p.officeNo,
                        bussinessType_Id = p.bussinessType_Id,
                        mobileNo = p.mobileNo,
                        salaryId = p.salaryId,
                        userRole_Id = p.userRole_Id,
                        //business_Id = p.bussiness_Id,
                        verified = p.verified,
                        status = p.status,
                        language = p.language != null ? (int)p.language : 0,
                        userName = p.userName,
                        gred = p.gred,
                        idNo = p.idNo,
                        birthDate = p.birthDate,
                        age = p.age,
                        accountNo = p.accountNo,
                        bankId = p.bankId,
                        salary = p.salary,
                        employeegroupId = p.employeegroupId,
                        entryDate = p.entryDate,
                        expireddate = p.expireddate,
                        yearofservice = p.yearofservice,
                        isAccessAllOrganization = p.isAccessAllOrganization
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        /// <summary>
        /// 1. User is assigned to selected sub costcenter or ‘isAccessAllOrganization = true.
        ///  2. User belongs to business where businessType = ‘Section’
        ///  3. User Visibility = ‘True’
        /// </summary>
        /// <param name="businessTypeId"></param>
        /// <param name="organizationId"></param>
        /// <param name="user_Id"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public List<Users> GetListUserByBusinessTypeId(int businessTypeId, int organizationId, int user_Id, bool check, int[] userIds)
        {
            List<Users> lstUser = new List<Users>();
            Users entity = new Users();

            if (userIds == null)
                userIds = new int[1];
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (userIds == null)
                        userIds = new int[1];
                    lstUser = model.Users.Where(p => userIds.Contains(p.id) || p.bussinessType_Id == businessTypeId && (check ? p.status == "Active" : true) && (p.isAccessAllOrganization == true || p.AssignedOrganizations.Any(s => s.organization_Id == organizationId))).Select(p => new Users
                    {
                        id = p.id,
                        firstName = p.firstName,
                        lastName = p.lastName,
                        email = p.email,
                        employeeNo = p.employeeNo,
                        jobTitle = p.jobTitle,
                        department = p.department,
                        address = p.address,
                        officeNo = p.officeNo,
                        mobileNo = p.mobileNo,
                        bussinessType_Id = p.bussinessType_Id,
                        salaryId = p.salaryId,
                        userRole_Id = p.userRole_Id,
                        //business_Id = p.bussiness_Id,
                        verified = p.verified,
                        status = p.status,
                        language = p.language != null ? (int)p.language : 0,
                        userName = p.userName,
                        gred = p.gred,
                        idNo = p.idNo,
                        birthDate = p.birthDate,
                        age = p.age,
                        accountNo = p.accountNo,
                        bankId = p.bankId,
                        salary = p.salary,
                        employeegroupId = p.employeegroupId,
                        entryDate = p.entryDate,
                        expireddate = p.expireddate,
                        yearofservice = p.yearofservice,
                        isAccessAllOrganization = p.isAccessAllOrganization
                    }).ToList();
                  
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return lstUser;
        }

        public AssignedBusinesss GetUserBuinessById(int currentUserId)
        {
            AssignedBusinesss _userEn = null;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var lst = model.AssignedBusinesses.Where(p => p.user_Id == currentUserId);
                    var entity = lst.OrderBy(p => p.id).FirstOrDefault(p => p.isDefault);
                    if (entity == null)
                        entity = lst.OrderBy(p => p.id).FirstOrDefault();

                    if (entity != null)
                    {
                        _userEn = new AssignedBusinesss
                        {
                            id = entity.id,
                            user_Id = entity.user_Id,
                            business_Id = entity.business_Id,
                            isDefault = entity.isDefault
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _userEn;
        }

        /// <summary>
        /// Section:
        /// 1. User is assigned to selected sub costcenter or ‘isAccessAllOrganization = true.
        ///  2. User belongs to business where businessType = ‘Section’
        ///  3. User Visibility = ‘True’
        ///  Supplier :
        /// 1. User belongs to business where businessType = ‘Supplier’
        /// 2. User Visibility = ‘True’
        /// </summary>
        /// <param name="businessTypeId"></param>
        /// <param name="organizationId"></param>
        /// <param name="user_Id"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public List<Users> GetListUserByStaffMember(int businessTypeId, int businessId, int organizationId, int user_Id, bool check, int[] userIds)
        {
            List<Users> lstUser = new List<Users>();
            Users entity = new Users();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (userIds == null)
                        userIds = new int[1];

                    var _staffIds = model.AssignedBusinesses.Where(p => p.business_Id == businessId).Select(p => p.user_Id).ToList();
                    if (businessTypeId == 65)
                    {
                        lstUser = model.Users.Where(p=>p.bussinessType_Id == businessTypeId && (check ? p.status == "Active" : true) && (_staffIds.Contains(p.id))).Select(p => new Users
                        {
                            id = p.id,
                            firstName = p.firstName,
                            lastName = p.lastName,
                            email = p.email,
                            employeeNo = p.employeeNo,
                            jobTitle = p.jobTitle,
                            department = p.department,
                            address = p.address,
                            officeNo = p.officeNo,
                            mobileNo = p.mobileNo,
                            bussinessType_Id = p.bussinessType_Id,
                            salaryId = p.salaryId,
                            userRole_Id = p.userRole_Id,
                            //business_Id = p.bussiness_Id,
                            verified = p.verified,
                            status = p.status,
                            language = p.language != null ? (int)p.language : 0,
                            userName = p.userName,
                            gred = p.gred,
                            idNo = p.idNo,
                            birthDate = p.birthDate,
                            age = p.age,
                            accountNo = p.accountNo,
                            bankId = p.bankId,
                            salary = p.salary,
                            employeegroupId = p.employeegroupId,
                            entryDate = p.entryDate,
                            expireddate = p.expireddate,
                            yearofservice = p.yearofservice,
                            isAccessAllOrganization = p.isAccessAllOrganization
                        }).OrderBy(a=>a.firstName).ToList();
                    }
                    else
                    {
                        return model.Users.Where(p => userIds.Contains(p.id) || (businessId==0 ? true : p.AssignedBusinesses.Any(a => a.business_Id == businessId)) && (businessTypeId==0 ? true :p.bussinessType_Id == businessTypeId) && (check ? p.status == "Active" : true)).Select(p => new Users
                        {
                            id = p.id,
                            firstName = p.firstName,
                            lastName = p.lastName,
                            email = p.email,
                            employeeNo = p.employeeNo,
                            jobTitle = p.jobTitle,
                            department = p.department,
                            address = p.address,
                            officeNo = p.officeNo,
                            bussinessType_Id = p.bussinessType_Id,
                            mobileNo = p.mobileNo,
                            salaryId = p.salaryId,
                            userRole_Id = p.userRole_Id,
                            //business_Id = p.bussiness_Id,
                            verified = p.verified,
                            status = p.status,
                            language = p.language != null ? (int)p.language : 0,
                            userName = p.userName,
                            gred = p.gred,
                            idNo = p.idNo,
                            birthDate = p.birthDate,
                            age = p.age,
                            accountNo = p.accountNo,
                            bankId = p.bankId,
                            salary = p.salary,
                            employeegroupId = p.employeegroupId,
                            entryDate = p.entryDate,
                            expireddate = p.expireddate,
                            yearofservice = p.yearofservice,
                            isAccessAllOrganization = p.isAccessAllOrganization
                        }).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return lstUser;
        }

        public Users GetUserDetailsByUserName(string userName)
        {
            Users _userEn = null;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.Users.Include("UserRole").SingleOrDefault(p => p.userName == userName);
                    if (entity != null)
                    {
                        _userEn = new Users
                        {
                            id = entity.id,
                            firstName = entity.firstName,
                            lastName = entity.lastName,
                            email = entity.email,
                            employeeNo = entity.employeeNo,
                            jobTitle = entity.jobTitle,
                            department = entity.department,
                            address = entity.address,
                            bussinessType_Id = entity.bussinessType_Id,
                            officeNo = entity.officeNo,
                            mobileNo = entity.mobileNo,
                            userRole_Id = entity.userRole_Id,
                            //business_Id = entity.bussiness_Id,
                            verified = entity.verified,
                            roleName = entity.UserRole.roleName,
                            status = entity.status,
                            language = entity.language != null ? (int)entity.language : 0,
                            userName = entity.userName,
                            gred = entity.gred,
                            idNo = entity.idNo,
                            birthDate = entity.birthDate,
                            age = entity.age,
                            accountNo = entity.accountNo,
                            bankId = entity.bankId,
                            salary = entity.salary,
                            employeegroupId = entity.employeegroupId,
                            entryDate = entity.entryDate,
                            expireddate = entity.expireddate,
                            yearofservice = entity.yearofservice,
                            isAccessAllOrganization = entity.isAccessAllOrganization,
                            businessExpiredDate = entity.Business.expiryDate
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _userEn;
        }

        public List<Users> GetListUserBySupplier(int businessTypeId, int business_Id, bool check, int[] userIds)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (userIds == null)
                        userIds = new int[1];

                    return model.Users.Include("Business").Where(p => userIds.Contains(p.id) || (businessTypeId==0 ? true : p.Business.businessType_Id == businessTypeId) && (business_Id==0? true :p.AssignedBusinesses.Any(a => a.business_Id == business_Id)) && (check ? p.status == "Active" : true)).Select(p => new Users
                    {
                        id = p.id,
                        firstName = p.firstName,
                        lastName = p.lastName,
                        email = p.email,
                        employeeNo = p.employeeNo,
                        jobTitle = p.jobTitle,
                        department = p.department,
                        address = p.address,
                        officeNo = p.officeNo,
                        mobileNo = p.mobileNo,
                        salaryId = p.salaryId,
                        userRole_Id = p.userRole_Id,
                        bussinessType_Id = p.bussinessType_Id,
                        //business_Id = p.bussiness_Id,
                        verified = p.verified,
                        status = p.status,
                        language = p.language != null ? (int)p.language : 0,
                        userName = p.userName,
                        gred = p.gred,
                        idNo = p.idNo,
                        birthDate = p.birthDate,
                        age = p.age,
                        accountNo = p.accountNo,
                        bankId = p.bankId,
                        salary = p.salary,
                        employeegroupId = p.employeegroupId,
                        entryDate = p.entryDate,
                        expireddate = p.expireddate,
                        yearofservice = p.yearofservice,
                        isAccessAllOrganization = p.isAccessAllOrganization
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public List<Users> GetListUserBySection(int businessTypeId, int business_Id, int organizationId, int user_Id, bool check, int[] userIds)
        {
            List<Users> lstUser = new List<Users>();
            Users entity = new Users();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (userIds == null)
                        userIds = new int[1];

                    lstUser = model.Users.Where(p => userIds.Contains(p.id) || (businessTypeId == 0 ? true : p.Business.businessType_Id == businessTypeId) && (business_Id == 0 ? true : p.AssignedBusinesses.Any(a => a.business_Id == business_Id)) && (check ? p.status == "Active" : true) && (p.isAccessAllOrganization == true || p.AssignedOrganizations.Any(s => s.organization_Id == organizationId))).Select(p => new Users
                    {
                        id = p.id,
                        firstName = p.firstName,
                        lastName = p.lastName,
                        email = p.email,
                        employeeNo = p.employeeNo,
                        jobTitle = p.jobTitle,
                        department = p.department,
                        address = p.address,
                        officeNo = p.officeNo,
                        mobileNo = p.mobileNo,
                        bussinessType_Id = p.bussinessType_Id,
                        salaryId = p.salaryId,
                        userRole_Id = p.userRole_Id,
                        //business_Id = p.bussiness_Id,
                        verified = p.verified,
                        status = p.status,
                        language = p.language != null ? (int)p.language : 0,
                        userName = p.userName,
                        gred = p.gred,
                        idNo = p.idNo,
                        birthDate = p.birthDate,
                        age = p.age,
                        accountNo = p.accountNo,
                        bankId = p.bankId,
                        salary = p.salary,
                        employeegroupId = p.employeegroupId,
                        entryDate = p.entryDate,
                        expireddate = p.expireddate,
                        yearofservice = p.yearofservice,
                        isAccessAllOrganization = p.isAccessAllOrganization
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return lstUser;
        }

        public Users ChangePassword(Users userEn)
        {
            try
            {
                using (var model = new HanodaleEntities())
                {
                    var _userEn = model.Users.SingleOrDefault(p => p.id == userEn.id && p.passwordHash == userEn.oldPasswordHash);

                    if (_userEn != null)
                    {
                        _userEn.passwordHash = userEn.passwordHash;
                        _userEn.modifiedDate = userEn.modifiedDate;
                        model.SaveChanges();
                        userEn.email = _userEn.email;
                    }
                    else
                    {
                        userEn.email = null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return userEn;
        }

        public string GetProfileFileName(int id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var obj = model.Users.SingleOrDefault(p => p.id == id);
                    if (obj != null)
                    {

                        return obj.profileImageName;

                    }
                    else
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public Users UpdateUserInfo(Users userEn)
        {
            User _userEn = new User();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _userEn = model.Users.SingleOrDefault(p => p.id == userEn.id);
                    if (_userEn != null)
                    {
                        _userEn.employeeNo = userEn.employeeNo;
                        _userEn.department = userEn.department;
                        _userEn.address = userEn.address;
                        _userEn.jobTitle = userEn.jobTitle;
                        _userEn.mobileNo = userEn.mobileNo;
                        _userEn.officeNo = userEn.officeNo;
                        _userEn.salaryId = userEn.salaryId;
                        _userEn.gred = userEn.gred;
                        _userEn.idNo = userEn.idNo;
                        _userEn.birthDate = userEn.birthDate;
                        _userEn.age = userEn.age;
                        _userEn.accountNo = userEn.accountNo;
                        _userEn.bankId = userEn.bankId;
                        _userEn.salary = userEn.salary;
                        _userEn.employeegroupId = userEn.employeegroupId;
                        _userEn.entryDate = userEn.entryDate;
                        _userEn.expireddate = userEn.expireddate;
                        _userEn.yearofservice = userEn.yearofservice;
                        _userEn.modifiedBy = userEn.modifiedBy;
                        _userEn.modifiedDate = userEn.modifiedDate;

                        model.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return userEn;
        }

        #endregion

        #region BusinessUser
        public Users CreateBusinessUser(int currentUserId, Users userEn)
        {
            User _userEn = new User();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    _userEn.userRole_Id = userEn.userRole_Id;
                    _userEn.bussiness_Id = 1591;
                    _userEn.bussinessType_Id = userEn.bussinessType_Id;
                    _userEn.firstName = userEn.firstName;
                    _userEn.lastName = userEn.lastName;
                    _userEn.email = userEn.email;
                    _userEn.userName = model.Businesses.FirstOrDefault(a => a.id == userEn.business_Id).code;
                    _userEn.passwordHash = userEn.passwordHash;
                    //_userEn.employeeNo = userEn.employeeNo;
                    _userEn.jobTitle = userEn.jobTitle;
                    //_userEn.address = userEn.address;
                    _userEn.department = userEn.department;
                    _userEn.mobileNo = userEn.mobileNo;
                    _userEn.officeNo = userEn.officeNo;
                    //_userEn.salaryId = userEn.salaryId;
                    _userEn.status = userEn.status;
                    //_userEn.gred = userEn.gred;
                    //_userEn.idNo = userEn.idNo;
                    //_userEn.birthDate = userEn.birthDate;
                    //_userEn.age = userEn.age;
                    //_userEn.accountNo = userEn.accountNo;
                    //_userEn.bankId = userEn.bankId;
                    //_userEn.salary = userEn.salary;
                    //_userEn.employeegroupId = userEn.employeegroupId;
                    //_userEn.entryDate = userEn.entryDate;
                    //_userEn.expireddate = userEn.expireddate;
                    //_userEn.yearofservice = userEn.yearofservice;
                    _userEn.createdBy = userEn.createdBy;
                    _userEn.createdDate = userEn.createdDate;
                    _userEn.verified = userEn.verified;
                    _userEn.modifiedBy = userEn.modifiedBy;
                    _userEn.modifiedDate = userEn.modifiedDate;
                    if (userEn.organization_Ids != null)
                    {
                        foreach (var item in userEn.organization_Ids)
                        {
                            if (item == 0)
                            {
                                _userEn.isAccessAllOrganization = true;
                            }
                            else
                            {
                                _userEn.isAccessAllOrganization = false;
                            }
                        }
                    }

                    model.Users.Add(_userEn);

                    var _mainCostCenterId = model.Organizations.SingleOrDefault(p => p.id == userEn.subCost_Id).parent_Id;

                    if (userEn.organization_Ids != null)
                    {
                        foreach (var item in userEn.organization_Ids)
                        {
                            if (item == 0)
                            {
                                AssignedOrganization _entity = new AssignedOrganization();
                                _entity.organization_Id = userEn.defaultOrganizationId;
                                _entity.user_Id = userEn.id;
                                _entity.createdDate = userEn.createdDate;
                                _entity.createdBy = userEn.createdBy;
                                _entity.isDefault = true;
                                model.AssignedOrganizations.Add(_entity);
                            }
                            else
                            {
                                AssignedOrganization _entity = new AssignedOrganization();
                                _entity.organization_Id = item;
                                _entity.user_Id = userEn.id;
                                _entity.createdDate = userEn.createdDate;
                                _entity.createdBy = userEn.createdBy;
                                _entity.isDefault = item == userEn.defaultOrganizationId ? true : false;
                                model.AssignedOrganizations.Add(_entity);
                            }

                        }

                    }

                    if (userEn.business_Id != null)
                    {
                        AssignedBusiness _entitys = new AssignedBusiness();
                        _entitys.business_Id = userEn.business_Id;
                        _entitys.user_Id = _userEn.id;
                        _entitys.createdDate = userEn.createdDate;
                        _entitys.createdBy = userEn.createdBy;
                        _entitys.isDefault = true;
                        model.AssignedBusinesses.Add(_entitys);

                        AssignedOrganization _entity = new AssignedOrganization();
                        _entity.organization_Id = userEn.defaultOrganizationId;
                        _entity.user_Id = _userEn.id;
                        _entity.createdDate = userEn.createdDate;
                        _entity.createdBy = userEn.createdBy;
                        _entity.isDefault = true;
                        model.AssignedOrganizations.Add(_entity);
                  
                    }

                    model.SaveChanges();
                    userEn.id = _userEn.id;
         
                    //Log User Action
                    //UserLog("User", Action.Insert.ToString(), userEn);
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return userEn;
        }

        public Users UpdateBusinessUser(int currentUserId, Users userEn)
        {
            User _userEn = new User();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                    _userEn = model.Users.SingleOrDefault(p => p.id == userEn.id);
                    if (_userEn != null)
                    {
                        
                        _userEn.bussinessType_Id = userEn.bussinessType_Id;
                        _userEn.firstName = userEn.firstName;
                        _userEn.lastName = userEn.lastName;
                        _userEn.email = userEn.email;
                        _userEn.employeeNo = userEn.employeeNo;
                        _userEn.department = userEn.department;
                        _userEn.address = userEn.address;
                        _userEn.jobTitle = userEn.jobTitle;
                        _userEn.mobileNo = userEn.mobileNo;
                        _userEn.officeNo = userEn.officeNo;
                        // _userEn.salaryId = userEn.salaryId;
                        //_userEn.passwordHash = _userEn.passwordHash;
                        //_userEn.organization_Id = userEn.defaultOrganizationId;
                        //_userEn.verified = userEn.verified;
                        _userEn.status = userEn.status;
                        //_userEn.gred = userEn.gred;
                        //_userEn.idNo = userEn.idNo;
                        //_userEn.birthDate = userEn.birthDate;
                        //_userEn.age = userEn.age;
                        //_userEn.accountNo = userEn.accountNo;
                        //_userEn.bankId = userEn.bankId;
                        //_userEn.salary = userEn.salary;
                        //_userEn.employeegroupId = userEn.employeegroupId;
                        //_userEn.entryDate = userEn.entryDate;
                        //_userEn.expireddate = userEn.expireddate;
                        //_userEn.yearofservice = userEn.yearofservice;
                        _userEn.modifiedBy = userEn.modifiedBy;
                        _userEn.modifiedDate = userEn.modifiedDate;

                        }

                        model.SaveChanges();
                    }
                }
                 catch (Exception ex)
                {
                    //we don't want to reveal any details to the client
                    throw new FaultException(ex.InnerException.InnerException.Message);
                }
                return userEn;
            }
           
        
        #endregion
    }
}
