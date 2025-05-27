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
    public interface IStoreService
    {
        StoreDetails GetStoreBySearch(DatatableFilters entityFilter, object filterEntity);

        Stores CreateStore(Stores entityEn);

        Stores UpdateStore(Stores entityEn);

        bool DeleteStore(int id);

        Stores GetStoreById(int id);

        bool IsStoreExists(Stores entityEn);
    }
}
