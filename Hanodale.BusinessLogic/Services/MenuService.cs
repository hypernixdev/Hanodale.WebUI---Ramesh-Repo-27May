using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class MenuService : IMenuService
    {
        #region Menu

        public Hanodale.DataAccessLayer.Interfaces.IMenuService DataProvider;

        public MenuService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.MenuService();
        }

        public List<Menu> GetUserMenu(int currentUserId, int userId)
        {
            return this.DataProvider.GetUserMenu(currentUserId, userId);
        }

        #endregion
    }
}
