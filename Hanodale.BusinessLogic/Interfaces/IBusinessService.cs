using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IBusinessService
    {
        #region Business

        List<Businesses> GetAllBusiness(int currentUserId);

        BusinessDetails GetBusiness(int currentUserId, bool all, int startIndex, int pageSize, string search, Businesses filterModel, int organization_Id);

        Businesses SaveBusiness(int currentUserId, Businesses entity, string pageName);

        bool DeleteBusiness(int currentUserId, int id, string pageName);

        Businesses GetBusinessById(int id);

        List<Businesses> GetListBusiness(int businessTypeId = 0);

        bool IsBusinessExists(Businesses entity);

        List<Businesses> GetBusinessbybusinessType();

        Businesses GetBusinessDetailsByUserId(int user_Id);

        List<int> GetListBusinessworkCategory(int business_Id);

        List<Businesses> GetListBusinessBySubCostId(int organization_Id);

        List<Businesses> GetListBusinessByOrganizationId(int organizationId, int businessType_Id, bool check);

        List<int> GetBusinessOrganizationById(int business_Id);

        BusinessDetails GetBusinessMaster(int currentUserId, int organization_Id, int startIndex, int pageSize, string search, object filterModel);

        List<Businesses> GetBusinessBySubCostId(int organization_Id);

        List<Businesses> GetListBusinessBybusinessType();

        List<Businesses> GetBusinessSupplierBySubCostId(int organization_Id);

        List<Businesses> GetListBusinessByBusinessTypeId(int businessType_Id);

        bool IsBusinessWorkCategoryandOrganisationExists(int[] workCategoryIds, int[] organisationIds);

        int IsBusinessSupplierExists(Businesses entity);

        Users GetListBusinessUser(Users entity);

        #endregion
    }
}
