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
    public interface ICustomerService
    {
        CustomerDetails GetCustomerBySearch(DatatableFilters entityFilter, object filterEntity);

        Customers CreateCustomer(Customers entityEn);

        Customers UpdateCustomer(Customers entityEn);

        bool DeleteCustomer(int id);

        Customers GetCustomerById(int id);

        Customers GetCustomerByCode(string code);
        List<Customers> GetCustomerList(string searchParam);
        List<Customers> GetCustomerList(string searchName, string searchCode, string searchCity, string searchState);
        bool IsCustomerExists(Customers entityEn);

        List<Districts> GetDistrictList(string searchParam);
    }
}
