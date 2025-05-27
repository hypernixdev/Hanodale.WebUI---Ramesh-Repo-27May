using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class BusinessService : IBusinessService
    {
        #region Business

        public Hanodale.DataAccessLayer.Interfaces.IBusinessService DataProvider;

        public BusinessService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.BusinessService();
        }

        public List<Businesses> GetAllBusiness(int currentUserId)
        {
            return this.DataProvider.GetAllBusiness(currentUserId);
        }

        public BusinessDetails GetBusiness(int currentUserId, bool all, int startIndex, int pageSize, string search, Businesses filterModel, int organization_Id)
        {
                return this.DataProvider.GetBusinessBySearch(currentUserId, all, startIndex, pageSize, search, filterModel, organization_Id);
        }

        public Businesses SaveBusiness(int currentUserId, Businesses entity, string pageName)
        {
            if (entity.id > 0)
                return this.DataProvider.UpdateBusiness(currentUserId, entity, pageName);
            else
                return this.DataProvider.CreateBusiness(currentUserId, entity, pageName);
        }

        public bool DeleteBusiness(int currentUserId, int id, string pageName)
        {
            return this.DataProvider.DeleteBusiness(currentUserId, id, pageName);
        }

        public Businesses GetBusinessById(int id)
        {
            return this.DataProvider.GetBusinessById(id);
        }

        public bool IsBusinessExists(Businesses entity)
        {
            return this.DataProvider.IsBusinessExists(entity);
        }

        public List<Businesses> GetListBusiness(int businessTypeId = 0)
        {
            return this.DataProvider.GetListBusiness(businessTypeId);
        }

        public List<Businesses> GetBusinessbybusinessType()
        {
            return this.DataProvider.GetBusinessbybusinessType();
        }

        public Businesses GetBusinessDetailsByUserId(int user_Id)
        {
            return this.DataProvider.GetBusinessDetailsByUserId(user_Id);
        }

        public List<int> GetListBusinessworkCategory(int business_Id)
        {
            return this.DataProvider.GetListBusinessworkCategory(business_Id);
        }

        public List<Businesses> GetListBusinessBySubCostId(int organization_Id)
        {
            return this.DataProvider.GetListBusinessBySubCostId(organization_Id);
        }

        public List<Businesses> GetListBusinessByOrganizationId(int organizationId, int businessType_Id, bool check)
        {
            return this.DataProvider.GetListBusinessByOrganizationId(organizationId, businessType_Id, check);
        }

        public List<int> GetBusinessOrganizationById(int business_Id)
        {
            return this.DataProvider.GetBusinessOrganizationById(business_Id);
        }


        public BusinessDetails GetBusinessMaster(int currentUserId, int organization_Id, int startIndex, int pageSize, string search, object filterModel)
        {
            return this.DataProvider.GetBusinessMasterBySearch(currentUserId, organization_Id, startIndex, pageSize, search, filterModel);
        }

        public List<Businesses> GetBusinessBySubCostId(int organization_Id)
        {
            return this.DataProvider.GetBusinessBySubCostId(organization_Id);
        }

        public List<Businesses> GetListBusinessBybusinessType()
        {
            return this.DataProvider.GetListBusinessBybusinessType();
        }

        public List<Businesses> GetBusinessSupplierBySubCostId(int organization_Id)
        {
            return this.DataProvider.GetBusinessSupplierBySubCostId(organization_Id);
        }

        public List<Businesses> GetListBusinessByBusinessTypeId(int businessType_Id)
        {
            return this.DataProvider.GetListBusinessByBusinessTypeId(businessType_Id);
        }
        public bool IsBusinessWorkCategoryandOrganisationExists(int[] workCategoryIds, int[] organisationIds)
        {
            return this.DataProvider.IsBusinessWorkCategoryandOrganisationExists(workCategoryIds, organisationIds);
        }

        public int IsBusinessSupplierExists(Businesses entity)
        {
            return this.DataProvider.IsBusinessSupplierExists(entity);
        }

        public Users GetListBusinessUser(Users entity)
        {
            return this.DataProvider.GetListBusinessUser(entity);
        }
        #endregion


    }
}
