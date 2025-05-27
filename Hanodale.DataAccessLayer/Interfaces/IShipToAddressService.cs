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
    public interface IShipToAddressService
    {
        ShipToAddressDetails GetShipToAddressBySearch(DatatableFilters entityFilter);

        //List<ShipToAddresses> GetShipToAddressList(ShipToAddresses entityEn);

        ShipToAddresses CreateShipToAddress(ShipToAddresses entityEn);

        ShipToAddresses UpdateShipToAddress(ShipToAddresses entityEn);

        bool DeleteShipToAddress(int id);

        ShipToAddresses GetShipToAddressById(int id);
        ShipToAddresses GetShipToAddressByCode(string code);
        bool IsShipToAddressExists(ShipToAddresses entityEn);

        List<ShipToAddresses> GetShipToAddressByCustomerId(int customerId, string searchby);
    }
}
