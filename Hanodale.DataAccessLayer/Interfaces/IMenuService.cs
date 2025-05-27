using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;
using System.Threading.Tasks;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IMenuService
    {
        #region Menu
        List<Menu> GetUserMenu(int currentUserId, int userId);
        #endregion
    }
}
