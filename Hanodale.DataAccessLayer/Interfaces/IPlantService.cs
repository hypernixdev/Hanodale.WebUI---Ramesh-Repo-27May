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
    public interface IPlantService
    {
        PlantDetails GetPlantBySearch(DatatableFilters entityFilter);

        Plants CreatePlant(Plants entityEn);

        Plants UpdatePlant(Plants entityEn);

        bool DeletePlant(int id);

        Plants GetPlantById(int id);

        bool IsPlantExists(Plants entityEn);
    }
}
