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
    public class MenuService : IMenuService
    {
        #region Menu
        /// <summary>
        /// This method is to get menu based on the user rights
        /// </summary>
        /// <param name="userId">User id - Login user id</param>
        /// <returns>Menu collection</returns>

        public List<Menu> GetUserMenu(int currentUserId, int userId)
        {

            List<Menu> _result = new List<Menu>();

            List<Menu> lstMainMenu = new List<Menu>(_result);
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var user = model.Users.SingleOrDefault(p => p.id == currentUserId);
                    var mainMenu = model.UserRights.Include("UserRole.Users").Include("MenuItem.MenuItem2").Where(p => p.canView && p.UserRole.Users.Any(u => u.id == currentUserId) && p.MenuItem.MenuItem2.showAsMain && p.MenuItem.MenuItem2.visibility).
                        GroupBy(p => p.MenuItem.MenuItem2).ToList().Select(y => new Menu
                    {
                        id = y.Key.id,
                        menuName = (y.Key.name == "Resource" && user.bussinessType_Id == 52) ? "Profile" : y.Key.name,
                        pageName = y.Key.pageName,
                        pageUrl = y.Key.pageUrl,
                        imageUrl = y.Key.imageUrl,
                        ordering = y.Key.ordering,
                        status = y.Key.visibility.ToString(),
                        subMenus = y.Select(item => new SubMenus
                        {
                            id = item.id,
                            mainMenu_Id = (int)item.MenuItem.reference_Id,
                            subMenuName = item.MenuItem.name,
                            pageName = item.MenuItem.pageName,
                            pageUrl = item.MenuItem.pageUrl,
                            imageUrl = item.MenuItem.imageUrl,
                            isMainMenu = item.MenuItem.showAsMain,
                            ordering = item.MenuItem.ordering,
                        }).Where(a => (( user.bussinessType_Id != 52) || user.bussinessType_Id == 52)).OrderBy(p => p.ordering).ToList()
                    }).OrderBy(p => p.ordering).ToList();

                    lstMainMenu = new List<Menu>(mainMenu);

                }


            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }

            return lstMainMenu.OrderBy(p => p.ordering).ToList();
        }
        #endregion
    }
}
