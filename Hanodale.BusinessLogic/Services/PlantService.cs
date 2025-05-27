using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class PlantService : IPlantService
    {
        public Hanodale.DataAccessLayer.Interfaces.IPlantService DataProvider;

        public PlantService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.PlantService();
        }

        public PlantDetails GetPlant(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetPlantBySearch(entityFilter);
        }

        public Plants SavePlant(Plants entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdatePlant(entityEn);
            else
                return this.DataProvider.CreatePlant(entityEn);
        }

        public bool DeletePlant(int id)
        {
            return this.DataProvider.DeletePlant(id);
        }

        public Plants GetPlantById(int id)
        {
            return this.DataProvider.GetPlantById(id);
        }

        public bool IsPlantExists(Plants entityEn)
        {
            return this.DataProvider.IsPlantExists(entityEn);
        }
    }
}
