using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IPlantService
    {
        PlantDetails GetPlant(DatatableFilters entityFilter);

        Plants SavePlant(Plants entityEn);

        bool DeletePlant(int id);

        Plants GetPlantById(int id);

        bool IsPlantExists(Plants entityEn);
    }
}
