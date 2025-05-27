using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IBusinessClassificationService
    {
        #region BusinessClassification

        BusinessClassificationDetails GetBusinessClassification(int currentUserId, int userId, int startIndex, int pageSize, string search);

        BusinessClassifications SaveBusinessClassification(int currentUserId, BusinessClassifications entity, string pageName);

        bool DeleteBusinessClassification(int currentUserId, int id, string pageName);

        BusinessClassifications GetBusinessClassificationById(int id);

        List<BusinessClassifications> GetListBusinessClassification();

        List<BusinessClassifications> GetListBusinessClassificationByBusinessId(int id);

        bool IsBusinessClassificationExists(BusinessClassifications entity);

        #endregion
    }
}
