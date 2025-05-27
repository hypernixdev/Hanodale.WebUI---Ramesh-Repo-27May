using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IStoreService
    {
        StoreDetails GetStore(DatatableFilters entityFilter,object filterEntity);

        Stores SaveStore(Stores entityEn);

        bool DeleteStore(int id);

        Stores GetStoreById(int id);

        bool IsStoreExists(Stores entityEn);
    }
}
