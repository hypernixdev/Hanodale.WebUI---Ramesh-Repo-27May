using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class CustomerService : ICustomerService
    {
        public Hanodale.DataAccessLayer.Interfaces.ICustomerService DataProvider;

        public CustomerService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.CustomerService();
        }

        public CustomerDetails GetCustomer(DatatableFilters entityFilter, object filterEntity)
        {
            return this.DataProvider.GetCustomerBySearch(entityFilter, filterEntity);
        }

        public Customers SaveCustomer(Customers entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateCustomer(entityEn);
            else
                return this.DataProvider.CreateCustomer(entityEn);
        }

        public bool DeleteCustomer(int id)
        {
            return this.DataProvider.DeleteCustomer(id);
        }

        public Customers GetCustomerById(int id)
        {
            return this.DataProvider.GetCustomerById(id);
        }

        public Customers GetCustomerByCode(string code)
        {
            return this.DataProvider.GetCustomerByCode(code);
        }

        public List<Customers> GetCustomerList(string searchParam)
        {
            return this.DataProvider.GetCustomerList(searchParam);
        }

        public List<Customers> GetCustomerList(string searchName, string searchCode, string searchCity, string searchState)
        {
            return this.DataProvider.GetCustomerList(searchName, searchCode, searchCity, searchState);
        }

        public bool IsCustomerExists(Customers entityEn)
        {
            return this.DataProvider.IsCustomerExists(entityEn);
        }

        public List<Districts> GetDistrictList(string searchParam)
        {
            return this.DataProvider.GetDistrictList(searchParam);
        }
    }
}
