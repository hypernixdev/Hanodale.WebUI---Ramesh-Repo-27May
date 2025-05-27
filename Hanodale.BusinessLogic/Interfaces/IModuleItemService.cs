using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IModuleItemService
   {
       #region ModuleItem

        ModuleItemDetails GetModuleItem(int currentUserId, bool all, int startIndex, int pageSize, string search);

       ModuleItems SaveModuleItem(int currentUserId, ModuleItems moduleItemEn, string pageName);

       bool DeleteModuleItem(int currentUserId, int moduleItemId, string pageName);

       ModuleItems GetModuleItemById(int id);

       bool IsModuleItemExists(ModuleItems moduleItem);

        #endregion
    }
}
