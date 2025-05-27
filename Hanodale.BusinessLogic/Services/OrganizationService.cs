using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hanodale.BusinessLogic
{
    public class OrganizationService : IOrganizationService
    {
        #region Organization

        public Hanodale.DataAccessLayer.Interfaces.IOrganizationService DataProvider;

        public OrganizationService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.OrganizationService();
        }

        public OrganizationDetails GetOrganizationBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            return this.DataProvider.GetOrganizationBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public Organizations SaveOrganization(int currentUserId, Organizations entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateOrganization(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateOrganization(currentUserId, entity, pageName);
        }

        public bool DeleteOrganization(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteOrganization(currentUserId, id, pageName);
        }

        public Organizations GetOrganizationById(int id)
        {
            return this.DataProvider.GetOrganizationById(id);
        }

        public OrganizationCategories GetOrganizationCategoryById(int id)
        {
            return this.DataProvider.GetOrganizationCategoryById(id);
        }

        public bool OrganizationExists(Organizations entity)
        {
            return this.DataProvider.OrganizationExists(entity);
        }

        public List<OrganizationCategoryConfigs> GetOrganizationCategoryConfig()
        {
            return this.DataProvider.GetOrganizationCategoryConfig();
        }

        public List<OrganizationCategoryConfigs> GetOrganizationCategoryConfigByOrganizationId(int id)
        {
            return this.DataProvider.GetOrganizationCategoryConfigByOrganizationId(id);
        }

        public List<OrganizationCategoryConfigs> GetOrganizationCategoryConfigByCategoryId(int id)
        {
            return this.DataProvider.GetOrganizationCategoryConfigByCategoryId(id);
        }

        public List<Assets> GetCostCenter(int currentUserId, List<bool> lstAccess)
        {
            return this.DataProvider.GetCostCenter(currentUserId, lstAccess);
        }

        public List<Organizations> GerOrganisation(int id)
        {
            return this.DataProvider.GerOrganisation(id);
        }
        #endregion

    }
}
