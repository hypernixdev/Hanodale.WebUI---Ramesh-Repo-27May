using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class CompanyProfileService : ICompanyProfileService
    {
        public Hanodale.DataAccessLayer.Interfaces.ICompanyProfileService DataProvider;

        public CompanyProfileService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.CompanyProfileService();
        }

        public CompanyProfileDetails GetCompanyProfile(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetCompanyProfileBySearch(entityFilter);
        }

        public CompanyProfiles SaveCompanyProfile(CompanyProfiles entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateCompanyProfile(entityEn);
            else
                return this.DataProvider.CreateCompanyProfile(entityEn);
        }

        public bool DeleteCompanyProfile(int id)
        {
            return this.DataProvider.DeleteCompanyProfile(id);
        }

        public CompanyProfiles GetCompanyProfileById(int id)
        {
            return this.DataProvider.GetCompanyProfileById(id);
        }

        public bool IsCompanyProfileExists(CompanyProfiles entityEn)
        {
            return this.DataProvider.IsCompanyProfileExists(entityEn);
        }
    }
}
