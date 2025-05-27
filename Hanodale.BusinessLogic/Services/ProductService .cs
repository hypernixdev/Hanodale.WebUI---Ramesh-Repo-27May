using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs.Order;

namespace Hanodale.BusinessLogic
{
    public class ProductService : IProductService
    {
        public Hanodale.DataAccessLayer.Interfaces.IProductService DataProvider;

        public ProductService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.ProductService();
        }

        public List<Products> GetProductList(Products entityEn)
        {
            return this.DataProvider.GetProductList(entityEn);
        }

        public ProductDetails GetProduct(DatatableFilters entityFilter  , object filterEntity)
        {
            return this.DataProvider.GetProductBySearch(entityFilter, filterEntity);
        }

        public Products SaveProduct(Products entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateProduct(entityEn);
            else
                return this.DataProvider.CreateProduct(entityEn);
        }

        public bool DeleteProduct(int id)
        {
            return this.DataProvider.DeleteProduct(id);
        }

        public Products GetProductById(int id)
        {
            return this.DataProvider.GetProductById(id);
        }

        public Products GetProductByPartNum(string partNum)
        {
            return this.DataProvider.GetProductByPartNum(partNum);
        }

        public ProductStockBalance GetProductStockBalance(string partNum, string uom)
        {
            return this.DataProvider.GetProductStockBalance(partNum, uom);
        }

        public List<ProductStockBalance> GetProductStockBalanceList(List<string> partNumbers)
        {
            return this.DataProvider.GetProductStockBalanceList(partNumbers);
        }

        public ProductDetails GetProductList(ProductDatatableFilter param)
        {
            return this.DataProvider.GetProductList(param);
        }

        public List<CustomerPricing> GetCustomerPricing(string custID, List<string> partNumbers, string priceDate)
        {
            return this.DataProvider.GetCustomerPricing(custID, partNumbers, priceDate);
        }

        public List<UomConvs> GetUomConvList(List<string> partNumbers)
        {
            return this.DataProvider.GetUomConvList(partNumbers);
        }

        public List<ModuleItems> GetOrderTypeList(int moduleTypeId)
        {
            return this.DataProvider.GetOrderTypeList(moduleTypeId);
        }

        public List<ModuleItems> GetOrderOpertionalStyleList(int moduleTypeId)
        {
            return this.DataProvider.GetOrderOpertionalStyleList(moduleTypeId);
        }
        public List<OrderUOM> GetOrderUOMList(int orderTypeId)
        {
            return this.DataProvider.GetOrderUOMList(orderTypeId);
        }

        public List<ModuleItems> GetComplimentaryList(int moduleTypeId)
        {
            return this.DataProvider.GetComplimentaryList(moduleTypeId);
        }
        public ModuleItems GetRrmarksComplimentary(int complimentaryId)
        {
            return this.DataProvider.GetRemarksComplimentary(complimentaryId);
        }
        public ModuleItems GetOperationStyleRemarks(int operationStyleId)
        {
            return this.DataProvider.GetOperationStyleRemarks(operationStyleId);
        }
        public bool IsProductExists(Products entityEn)
        {
            return this.DataProvider.IsProductExists(entityEn);
        }
    }
}
