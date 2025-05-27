using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IShipToAddressService
    {
        ShipToAddressDetails GetShipToAddress(DatatableFilters entityFilter);

        //List<ShipToAddresses> GetShipToAddressList(ShipToAddresses entityEn);

        ShipToAddresses SaveShipToAddress(ShipToAddresses entityEn);

        bool DeleteShipToAddress(int id);

        ShipToAddresses GetShipToAddressById(int id);

        ShipToAddresses GetShipToAddressByCode(string code);
        bool IsShipToAddressExists(ShipToAddresses entityEn);

        List<ShipToAddresses> GetShipToAddressByCustomerId(int customerId, string searchby);
    }
}
