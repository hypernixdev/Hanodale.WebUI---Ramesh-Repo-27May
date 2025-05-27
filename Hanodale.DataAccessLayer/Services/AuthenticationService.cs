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
    public class AuthenticationService : IAuthenticationService
    {
        #region Authentication

        /// <summary>
        /// This method is to authenticate user
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>User</returns>
        /// 
        public AuthenticateUser DigestAuthentication(int uid)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _result = model.Users.Include("AssignedOrganizations").SingleOrDefault(p => p.id == uid);
                    if (_result != null)
                    {
                        return new AuthenticateUser()
                        {
                            id = _result.id,
                            firstName = _result.firstName,
                            name = _result.userName,
                            userRole_Id = _result.userRole_Id,
                            language = _result.language != null ? Convert.ToInt32(_result.language) : 0,
                            status = _result.status,
                            defaultOrganization = _result.AssignedOrganizations.Count(p => p.isDefault) > 0 ? _result.AssignedOrganizations.FirstOrDefault(p => p.isDefault).organization_Id : (_result.AssignedOrganizations.Count > 0) ? _result.AssignedOrganizations.FirstOrDefault().organization_Id : 0,
                            expireDate = _result.Business.expiryDate,
                            supplierBusinessType_Id = _result.Business.businessType_Id,
                            urlPath = _result.profileImageName,
                            pwd = _result.passwordHash,
                        };

                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw ex;
            }
        }

        public AuthenticateUser AuthenticateUser(string emilAddress, string password)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _result = model.Users.Include("AssignedOrganizations").SingleOrDefault(p => p.userName == emilAddress && p.passwordHash == password);
                    
                    if (_result != null)
                    {
                        _result.AssignedOrganizations = _result.AssignedOrganizations.Where(a => a.Organization.isActive == true).ToList();
                        return new AuthenticateUser()
                        {
                            id = _result.id,
                            firstName = _result.firstName,
                            name = _result.userName,
                            userRole_Id = _result.userRole_Id,
                            language = _result.language != null ? Convert.ToInt32(_result.language) : 0,
                            status = _result.status,
                            defaultOrganization = _result.AssignedOrganizations.Count(p => p.isDefault)>0? _result.AssignedOrganizations.FirstOrDefault(p => p.isDefault).organization_Id: (_result.AssignedOrganizations.Count>0)? _result.AssignedOrganizations.FirstOrDefault().organization_Id: 0,
                            expireDate = _result.Business.expiryDate,
                            supplierBusinessType_Id = _result.Business.businessType_Id,
                            urlPath = _result.profileImageName
                        };

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        /// <summary>
        /// This method is to authenticate user BY UserId
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>User</returns>
        public AuthenticateUser AuthenticateUserByUserId(string emilAddress, string password, int user_Id)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _result = model.Users.Include("AssignedOrganizations").SingleOrDefault(p => p.userName == emilAddress && p.passwordHash == password);
                    _result.AssignedOrganizations = _result.AssignedOrganizations.Where(a => a.Organization.isActive == true).ToList();
                    if (_result != null)
                    {
                        return new AuthenticateUser()
                        {
                            id = _result.id,
                            firstName = _result.firstName,
                            name = _result.userName,
                            userRole_Id = _result.userRole_Id,
                            language = _result.language != null ? Convert.ToInt32(_result.language) : 0,
                            status = _result.status,
                            defaultOrganization = _result.AssignedOrganizations.FirstOrDefault(p => p.isDefault).organization_Id,
                            expireDate = _result.Business.expiryDate,
                            supplierBusinessType_Id = _result.Business.businessType_Id,
                            urlPath = _result.profileImageName
                        };

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }

        public bool ChangePassword(Users userEn, string newPassword, string pageName)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _userEn = model.Users.SingleOrDefault(u => u.passwordHash == userEn.passwordHash && u.id == userEn.id);

                    if (_userEn != null)
                    {
                        _userEn.passwordHash = newPassword;
                        _userEn.modifiedBy = userEn.modifiedBy;
                        _userEn.modifiedDate = userEn.modifiedDate;
                        _userEn.verified = userEn.verified;
                        model.SaveChanges();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
