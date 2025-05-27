using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class ModuleItemService : IModuleItemService
    {
        #region ModuleItem

        public Hanodale.DataAccessLayer.Interfaces.IModuleItemService DataProvider;

        public ModuleItemService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.ModuleItemService();
        }

        public ModuleItemDetails GetModuleItem(int currentUserId, bool all, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetModuleItem(currentUserId, all, startIndex, pageSize);
            else
                return this.DataProvider.GetModuleItemBySearch(currentUserId, all, startIndex, pageSize, search);
        }

        public ModuleItems SaveModuleItem(int currentUserId, ModuleItems moduleItemEn, string pageName)
        {
            if (moduleItemEn.id > 0)
                return this.DataProvider.UpdateModuleItem(currentUserId, moduleItemEn, pageName);
            else
                return this.DataProvider.CreateModuleItem(currentUserId, moduleItemEn, pageName);
        }

        public bool DeleteModuleItem(int currentUserId, int moduleItemId, string pageName)
        {
            return this.DataProvider.DeleteModuleItem(currentUserId, moduleItemId, pageName);
        }

        public ModuleItems GetModuleItemById(int id)
        {
            return this.DataProvider.GetModuleItemById(id);
        }

        public bool IsModuleItemExists(ModuleItems moduleItem)
        {
            return this.DataProvider.IsModuleItemExists(moduleItem);
        }

        #endregion
    }
}
