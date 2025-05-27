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

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IModuleItemService
    {
        #region ModuleItem

        ModuleItemDetails GetModuleItemBySearch(int currentUserId, bool all, int startIndex, int pageSize, string search);

        ModuleItemDetails GetModuleItem(int currentUserId, bool all, int startIndex, int pageSize);

        ModuleItems CreateModuleItem(int currentUserId, ModuleItems moduleItemEn, string pageName);

        ModuleItems UpdateModuleItem(int currentUserId, ModuleItems moduleItemEn, string pageName);

        bool DeleteModuleItem(int currentUserId, int moduleItemId, string pageName);

        ModuleItems GetModuleItemById(int id);

        bool IsModuleItemExists(ModuleItems moduleItem);

        #endregion
    }
}
