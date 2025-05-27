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
    public class UserRightsService : IUserRightsService
    {
        #region User Rights

        /// <summary>
        /// This method is to get the user access
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public UserRights GetUserAccess(int userId, string pageUrl)
        {
            UserRights _userRight = new UserRights();
            try
            {
                pageUrl = (string.IsNullOrEmpty(pageUrl) ? null : pageUrl);

                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var MemberRights = model.UserRights.FirstOrDefault(p => p.MenuItem.pageUrl == pageUrl && p.UserRole.Users.Any(m => m.id == userId));

                    if (MemberRights != null)
                    {
                        _userRight.id = MemberRights.id;
                        _userRight.menuItem_Id = MemberRights.menuItem_Id;
                        _userRight.canAdd = MemberRights.canAdd;
                        _userRight.canEdit = MemberRights.canEdit;
                        _userRight.canDelete = MemberRights.canDelete;
                        _userRight.canView = MemberRights.canView;

                        var submenu = model.MenuItems.FirstOrDefault(p => p.pageUrl == pageUrl);
                        if (submenu != null)
                        {
                            _userRight.subMenus = new SubMenus();
                            _userRight.subMenus.id = submenu.id;
                            _userRight.subMenus.subMenuName = submenu.name;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _userRight;
        }

        /// <summary>
        /// This method is to get user role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserRoles> GetUserRoles(int userId)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var _result = model.UserRoles.Where(p => p.status).OrderBy(p => p.roleName).Select(p => new UserRoles { id = p.id, roleName = p.roleName, isAdmin = p.isAdmin }).ToList();
                    return _result;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);

                return null;
            }
            
        }

        /// <summary>
        /// This method is to get user rights based on the user role
        /// </summary>
        /// <param name="userId">User id - Login user id</param>
        /// <returns>Menu collection</returns>
        public List<Menu> GetUserRightsByRole(int roleId)
        {
            List<Menu> _result = new List<Menu>();
            //List<Menu> lstMainMenu = new List<Menu>(_result);
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var lst = model.MenuItems.Include("MenuItem1.UserRights").Where(p => p.MenuItem1.Where(s => s.visibility).Count() > 0).OrderBy(p=>p.name);

                    foreach (var item in lst)
                    {
                        Menu _menuEn = new Menu();
                        _menuEn.id = item.id;
                        _menuEn.menuName = item.name;
                        _menuEn.pageName = item.pageName;
                        _menuEn.pageUrl = item.pageUrl;
                        _menuEn.imageUrl = item.imageUrl;
                        _menuEn.status = item.visibility ? "Active" : "Inactive";

                        _menuEn.subMenus = new List<SubMenus>();

                        foreach (var subMenu in item.MenuItem1.OrderBy(p=>p.name))
                        {
                            SubMenus _submenu = new SubMenus();
                            _submenu.id = subMenu.id;
                            _submenu.mainMenu_Id = subMenu.reference_Id == null ? 0 : (int)subMenu.reference_Id;
                            _submenu.subMenuName = subMenu.name;
                            _submenu.pageUrl = subMenu.pageUrl;
                            _submenu.imageUrl = subMenu.imageUrl;

                            //get user rights for the submenu
                            var userRights = subMenu.UserRights.SingleOrDefault(p => p.userRole_Id == roleId);
                            _submenu.userRights = new UserRights();
                            if (userRights != null)
                            {
                                _submenu.userRights.canAdd = userRights.canAdd;
                                _submenu.userRights.canDelete = userRights.canDelete;
                                _submenu.userRights.canEdit = userRights.canEdit;
                                _submenu.userRights.canView = userRights.canView;
                            }
                            _menuEn.subMenus.Add(_submenu);
                        }

                        _result.Add(_menuEn);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public bool CreateUserRights(List<UserRights> lstUserRights, string pageName)
        {
            UserRight _userRights = new UserRight();

            if (lstUserRights == null) return false;
            if (lstUserRights.Count == 0) return false;

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    foreach (UserRights UserRights in lstUserRights)
                    {
                        _userRights = (from ur in model.UserRights
                                       where ur.userRole_Id == UserRights.userRole_Id
                                         && ur.menuItem_Id == UserRights.menuItem_Id
                                       select ur).FirstOrDefault();

                        if (_userRights != null)
                        {
                            //Save User Rights
                            UserRight _ur = new UserRight();
                            _ur.userRole_Id = UserRights.userRole_Id;
                            _ur.menuItem_Id = UserRights.menuItem_Id;
                            _ur.canAdd = UserRights.canAdd;
                            _ur.canDelete = UserRights.canDelete;
                            _ur.canEdit = UserRights.canEdit;
                            _ur.canView = UserRights.canView;
                            _ur.createdBy = UserRights.createdBy;
                            _ur.createdDate = UserRights.createdDate;
                            _ur.modifiedBy = UserRights.modifiedBy;
                            _ur.modifiedDate = UserRights.modifiedDate;

                            model.UserRights.Add(_ur);
                        }
                    }

                    model.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return true;
        }

        public bool UpdateUserRights(List<UserRights> lstUserRights, string pageName)
        {
            UserRight _userRights = new UserRight();

            if (lstUserRights == null) return false;
            if (lstUserRights.Count == 0) return false;

            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    foreach (UserRights UserRights in lstUserRights)
                    {
                        _userRights = (from ur in model.UserRights
                                       where ur.userRole_Id == UserRights.userRole_Id
                                         && ur.menuItem_Id == UserRights.menuItem_Id
                                       select ur).FirstOrDefault();

                        if (_userRights != null)
                        {
                            //Update User rights
                            _userRights.userRole_Id = UserRights.userRole_Id;
                            _userRights.menuItem_Id = UserRights.menuItem_Id;
                            _userRights.canView = UserRights.canView;
                            _userRights.canAdd = UserRights.canAdd;
                            _userRights.canEdit = UserRights.canEdit;
                            _userRights.canDelete = UserRights.canDelete;

                            _userRights.modifiedBy = UserRights.modifiedBy;
                            _userRights.modifiedDate = UserRights.modifiedDate;
                        }
                        else
                        {
                            //Save User Rights
                            UserRight _ur = new UserRight();
                            _ur.userRole_Id = UserRights.userRole_Id;
                            _ur.menuItem_Id = UserRights.menuItem_Id;
                            _ur.canAdd = UserRights.canAdd;
                            _ur.canDelete = UserRights.canDelete;
                            _ur.canEdit = UserRights.canEdit;
                            _ur.canView = UserRights.canView;
                            _ur.createdBy = UserRights.createdBy;
                            _ur.createdDate = UserRights.createdDate;
                            _ur.modifiedBy = UserRights.modifiedBy;
                            _ur.modifiedDate = UserRights.modifiedDate;

                            model.UserRights.Add(_ur);
                        }
                    }

                    model.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return true;
        }

        #endregion
    }
}
