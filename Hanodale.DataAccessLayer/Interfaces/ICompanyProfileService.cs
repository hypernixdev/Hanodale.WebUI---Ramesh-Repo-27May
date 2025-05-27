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
    public interface ICompanyProfileService
    {
        CompanyProfileDetails GetCompanyProfileBySearch(DatatableFilters entityFilter);

        CompanyProfiles CreateCompanyProfile(CompanyProfiles entityEn);

        CompanyProfiles UpdateCompanyProfile(CompanyProfiles entityEn);

        bool DeleteCompanyProfile(int id);

        CompanyProfiles GetCompanyProfileById(int id);

        bool IsCompanyProfileExists(CompanyProfiles entityEn);
    }
}
