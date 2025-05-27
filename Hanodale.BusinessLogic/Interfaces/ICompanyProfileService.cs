using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ICompanyProfileService
   {
        CompanyProfileDetails GetCompanyProfile(DatatableFilters entityFilter);

        CompanyProfiles SaveCompanyProfile(CompanyProfiles entityEn);

        bool DeleteCompanyProfile(int id);

        CompanyProfiles GetCompanyProfileById(int id);

        bool IsCompanyProfileExists(CompanyProfiles entityEn);
    }
}
