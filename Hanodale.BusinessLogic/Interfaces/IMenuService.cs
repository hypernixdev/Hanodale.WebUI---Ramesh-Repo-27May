using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IMenuService
    {
        #region Menu
        List<Menu> GetUserMenu(int currentUserId, int userId);
        #endregion
    }
}
