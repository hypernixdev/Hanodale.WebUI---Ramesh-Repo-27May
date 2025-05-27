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
    public interface IBusinessClassificationService
    {
        #region BusinessClassification

        BusinessClassificationDetails GetBusinessClassificationBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        BusinessClassificationDetails GetBusinessClassification(int currentUserId, int userId, int startIndex, int pageSize);

        BusinessClassifications CreateBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName );

        BusinessClassifications UpdateBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName);

        bool DeleteBusinessClassification(int currentUserId, int id, string pageName);

        BusinessClassifications GetBusinessClassificationById(int id);

        List<BusinessClassifications> GetListBusinessClassification();

        List<BusinessClassifications> GetListBusinessClassificationByBusinessId(int id);

        bool IsBusinessClassificationExists(BusinessClassifications entity);

        #endregion
    }
}
