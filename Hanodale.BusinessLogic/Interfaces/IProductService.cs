using Hanodale.Domain.DTOs;
using Hanodale.Domain.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IProductService
    {
        ProductDetails GetProduct(DatatableFilters entityFilter, object filterEntity);

        List<Products> GetProductList(Products entityEn);

        Products SaveProduct(Products entityEn);

        bool DeleteProduct(int id);

        Products GetProductById(int id);
        Products GetProductByPartNum(String partNum);

        ProductStockBalance GetProductStockBalance(string partNum, string uom);

        List<ProductStockBalance> GetProductStockBalanceList(List<string> partNumbers);

        ProductDetails GetProductList(ProductDatatableFilter param);

        List<CustomerPricing> GetCustomerPricing(string custID, List<string> partNumbers, string priceDate);

        List<UomConvs> GetUomConvList(List<string> partNumbers);

        List<ModuleItems> GetOrderTypeList(int moduleTypeId);
        bool IsProductExists(Products entityEn);
        List<ModuleItems> GetOrderOpertionalStyleList(int orderOpertionalStyleList_modulType_Id);
        List<OrderUOM> GetOrderUOMList(int orderTypeId);
        List<ModuleItems> GetComplimentaryList(int moduleType_Id);
        ModuleItems GetOperationStyleRemarks(int complimentaryId);

        ModuleItems GetRrmarksComplimentary(int complimentaryId);
    }
}
