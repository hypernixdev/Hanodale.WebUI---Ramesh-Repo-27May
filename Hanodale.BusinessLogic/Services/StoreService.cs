using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class StoreService : IStoreService
    {
        public Hanodale.DataAccessLayer.Interfaces.IStoreService DataProvider;

        public StoreService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.StoreService();
        }

        public StoreDetails GetStore(DatatableFilters entityFilter,object filterEntity)
        {
            return this.DataProvider.GetStoreBySearch(entityFilter, filterEntity);
        }

        public Stores SaveStore(Stores entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateStore(entityEn);
            else
                return this.DataProvider.CreateStore(entityEn);
        }

        public bool DeleteStore(int id)
        {
            return this.DataProvider.DeleteStore(id);
        }

        public Stores GetStoreById(int id)
        {
            return this.DataProvider.GetStoreById(id);
        }

        public bool IsStoreExists(Stores entityEn)
        {
            return this.DataProvider.IsStoreExists(entityEn);
        }
    }
}
