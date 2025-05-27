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
using Hanodale.Domain.DTOs.Order;

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IProductService
    {
        ProductDetails GetProductBySearch(DatatableFilters entityFilter, object filterEntity);

        List<Products> GetProductList(Products entityEn);

        Products CreateProduct(Products entityEn);

        Products UpdateProduct(Products entityEn);

        bool DeleteProduct(int id);

        Products GetProductById(int id);
        Products GetProductByPartNum(string partNum);

        ProductStockBalance GetProductStockBalance(string partNum, string uom);

        List<ProductStockBalance> GetProductStockBalanceList(List<string> partNumbers);

        ProductDetails GetProductList(ProductDatatableFilter param);

        List<CustomerPricing> GetCustomerPricing(string custID, List<string> partNumbers, string priceDate);

        List<UomConvs> GetUomConvList(List<string> partNumbers);

        bool IsProductExists(Products entityEn);
        List<ModuleItems> GetOrderTypeList(int moduleTypeId);
        List<ModuleItems> GetOrderOpertionalStyleList(int moduleTypeId);
        List<Domain.DTOs.OrderUOM> GetOrderUOMList(int orderTypeId);
        List<ModuleItems> GetComplimentaryList(int moduleTypeId);
        ModuleItems GetRemarksComplimentary(int complimentaryId);
        ModuleItems GetOperationStyleRemarks(int operationStyleId);
    }
}
