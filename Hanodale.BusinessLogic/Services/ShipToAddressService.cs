using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class ShipToAddressService : IShipToAddressService
    {
        public Hanodale.DataAccessLayer.Interfaces.IShipToAddressService DataProvider;

        public ShipToAddressService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.ShipToAddressService();
        }

       /* public List<ShipToAddresses> GetShipToAddressList(ShipToAddresses entityEn)
        {
            return this.DataProvider.GetShipToAddressList(entityEn);
        }*/

        public ShipToAddressDetails GetShipToAddress(DatatableFilters entityFilter  )
        {
            return this.DataProvider.GetShipToAddressBySearch(entityFilter);
        }

        public ShipToAddresses SaveShipToAddress(ShipToAddresses entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateShipToAddress(entityEn);
            else
                return this.DataProvider.CreateShipToAddress(entityEn);
        }

        public bool DeleteShipToAddress(int id)
        {
            return this.DataProvider.DeleteShipToAddress(id);
        }

        public ShipToAddresses GetShipToAddressById(int id)
        {
            return this.DataProvider.GetShipToAddressById(id);
        }

        public ShipToAddresses GetShipToAddressByCode(string code)
        {
            return this.DataProvider.GetShipToAddressByCode(code);
        }
        public bool IsShipToAddressExists(ShipToAddresses entityEn)
        {
            return this.DataProvider.IsShipToAddressExists(entityEn);
        }

        public List<ShipToAddresses> GetShipToAddressByCustomerId(int customerId, string searchby)
        {
            return this.DataProvider.GetShipToAddressByCustomerId(customerId, searchby);
        }
    }
}
