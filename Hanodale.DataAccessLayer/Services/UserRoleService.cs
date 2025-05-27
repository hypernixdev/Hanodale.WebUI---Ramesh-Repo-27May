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
    public class UserRoleService : IUserRoleService
    {
        #region UserRole

        /// <summary>
        /// This method is to get the user details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>

        public RoleDetails GetUserRoleBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            RoleDetails _result = new RoleDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    bool a, b, c, d;
                    a = Common.AdminStatus.Admin.ToString().ToLower().Contains(search.ToLower()); b = Common.AdminStatus.Admin.ToString().ToLower().Contains(search.ToLower()); c = Common.RecordStatus.Active.ToString().ToLower().Contains(search.ToLower()); d = Common.RecordStatus.InActive.ToString().ToLower().Contains(search.ToLower());
                    _result.recordDetails.totalRecords = model.UserRoles.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;
                    var result = model.UserRoles.OrderByDescending(p => p.modifiedDate).Where(
                        p => p.roleName.Contains(search)
                        || p.description.Contains(search)
                        || (c ? p.status == true  :d? p.status== false: false ))
                       .Select(p => new UserRoles
                       {
                           id = p.id,
                           roleName = p.roleName,
                           isAdmin = p.isAdmin,
                           createdBy = p.createdBy,
                           createdDate = p.createdDate,
                           modifiedBy = p.modifiedBy,
                           modifiedDate = p.modifiedDate,
                           status = p.status,
                           description = p.description
                       }).ToList();

                    //Get filter data
                    _result.recordDetails.totalDisplayRecords = result.Count;
                    _result.lstRoles = result.Skip(startIndex).Take(pageSize).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public RoleDetails GetUserRole(int currentUserId, int userId, int startIndex, int pageSize)
        {
            RoleDetails _result = new RoleDetails();
            _result.recordDetails = new RecordDetails();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //get total record
                    _result.recordDetails.totalRecords = model.UserRoles.Count();
                    _result.recordDetails.totalDisplayRecords = _result.recordDetails.totalRecords;

                    _result.lstRoles = model.UserRoles.OrderByDescending(p => p.modifiedDate).Skip(startIndex).Take(pageSize).Select(p => new UserRoles
                    {
                        id = p.id,
                        roleName = p.roleName,
                        isAdmin = p.isAdmin,
                        createdBy = p.createdBy,
                        createdDate = p.createdDate,
                        modifiedBy = p.modifiedBy,
                        modifiedDate = p.modifiedDate,
                        status = p.status,
                        description = p.description
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
        /// This method is to delete the userrole details
        /// </summary>
        /// <param name="roleId">Role ID</param>  
        public bool DeleteUserRole(int currentUserId, int roleId, string pageName)
        {
            bool isDeleted = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    UserRole _roleEn = model.UserRoles.SingleOrDefault(p => p.id == roleId);

                    if (_roleEn != null)
                    {
                        model.UserRoles.Remove(_roleEn);
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

        public UserRoles CreateUserRole(int currentUserId, UserRoles roleEn, string pageName)
        {
            UserRole _roleEn = new UserRole();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //Add new Role
                    _roleEn.roleName = roleEn.roleName;
                    _roleEn.description = roleEn.description;
                    _roleEn.isAdmin = roleEn.isAdmin;
                    _roleEn.status = roleEn.status;
                    _roleEn.user_Id = roleEn.user_Id;
                    _roleEn.createdBy = roleEn.createdBy;
                    _roleEn.createdDate = roleEn.createdDate;
                    _roleEn.modifiedBy = roleEn.modifiedBy;
                    _roleEn.modifiedDate = roleEn.modifiedDate;

                    model.UserRoles.Add(_roleEn);
                    model.SaveChanges();

                    roleEn.id = _roleEn.id;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return roleEn;
        }

        public UserRoles UpdateUserRole(int currentUserId, UserRoles roleEn, string pageName)
        {
            UserRole _roleEn = new UserRole();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    //update role
                    _roleEn = model.UserRoles.SingleOrDefault(p => p.id == roleEn.id);
                    if (_roleEn != null)
                    {
                        _roleEn.roleName = roleEn.roleName;
                        _roleEn.description = roleEn.description;
                        _roleEn.isAdmin = roleEn.isAdmin;
                        _roleEn.landingPage = roleEn.landingPage;
                        _roleEn.status = roleEn.status;
                        _roleEn.modifiedDate = roleEn.modifiedDate;
                        _roleEn.modifiedBy = roleEn.modifiedBy;
                    }
                    model.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return roleEn;
        }

        /// <summary>
        /// This method is to get the role by role id
        /// </summary>
        /// <param name="roleId">role Id</param>
        /// <returns>Role details</returns>
        public UserRoles GetRoleById(int id)
        {
            UserRoles _roleEn = new UserRoles();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.UserRoles.SingleOrDefault(p => p.id == id);
                    if (entity != null)
                    {
                        _roleEn = new UserRoles
                        {
                            id = entity.id,
                            roleName = entity.roleName,
                            description = entity.description,
                            landingPage = entity.landingPage,
                            isAdmin = entity.isAdmin,
                            status = entity.status
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _roleEn;
        }

        /// <summary>
        /// This method is to check the userrole exists or not.
        /// </summary>
        /// <param name="roleName">Role Name</param>  
        public bool RoleExists(UserRoles role)
        {
            UserRole _roleEn = new UserRole();
            bool isExists = false;
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    if (role.id == 0)
                    {
                        _roleEn = model.UserRoles.SingleOrDefault(p => p.roleName == role.roleName);
                        if (_roleEn != null)
                            isExists = true;
                    }
                    else
                    {
                        _roleEn = model.UserRoles.SingleOrDefault(p => p.roleName == role.roleName && p.id != role.id);
                        if (_roleEn != null)
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
