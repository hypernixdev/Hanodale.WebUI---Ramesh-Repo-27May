using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ICustomerService
    {
        CustomerDetails GetCustomer(DatatableFilters entityFilter, object filterEntity);

        Customers SaveCustomer(Customers entityEn);

        bool DeleteCustomer(int id);

        Customers GetCustomerById(int id);
        Customers GetCustomerByCode(string code);
        List<Customers> GetCustomerList(string searchParam);

        List<Customers> GetCustomerList(string searchName, string searchCode, string searchCity, string searchState);

        bool IsCustomerExists(Customers entityEn);

        List<Districts> GetDistrictList(string searchParam);
    }
}
