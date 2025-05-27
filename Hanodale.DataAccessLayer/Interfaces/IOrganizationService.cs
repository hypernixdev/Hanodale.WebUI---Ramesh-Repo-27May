using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Hanodale.Domain.DTOs;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IOrganizationService
    {
        #region Organization

        OrganizationDetails GetOrganizationBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        Organizations CreateOrganization(int currentUserId, Organizations entity, string pageName);

        Organizations UpdateOrganization(int currentUserId, Organizations entity, string pageName);
         
        bool DeleteOrganization(int currentUserId, int id, string pageName);
         
        Organizations GetOrganizationById(int id);

        OrganizationCategories GetOrganizationCategoryById(int id);

        bool OrganizationExists(Organizations entity);

        List<OrganizationCategoryConfigs> GetOrganizationCategoryConfig();

        List<OrganizationCategoryConfigs> GetOrganizationCategoryConfigByOrganizationId(int id);

        List<OrganizationCategoryConfigs> GetOrganizationCategoryConfigByCategoryId(int id);

        List<Assets> GetCostCenter(int currentUserId, List<bool> lstAccess);

        List<Organizations> GerOrganisation(int id);
        #endregion

       
    }
}
