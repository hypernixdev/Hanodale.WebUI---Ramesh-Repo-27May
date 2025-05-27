using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class ProductWeightBarcodeService : IProductWeightBarcodeService
    {
        public Hanodale.DataAccessLayer.Interfaces.IProductWeightBarcodeService DataProvider;

        public ProductWeightBarcodeService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.ProductWeightBarcodeService();
        }

        public ProductWeightBarcodeDetails GetProductWeightBarcode(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetProductWeightBarcodeBySearch(entityFilter);
        }

        public ProductWeightBarcodes SaveProductWeightBarcode(ProductWeightBarcodes entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateProductWeightBarcode(entityEn);
            else
                return this.DataProvider.CreateProductWeightBarcode(entityEn);
        }

        public bool DeleteProductWeightBarcode(int id)
        {
            return this.DataProvider.DeleteProductWeightBarcode(id);
        }

        public ProductWeightBarcodes GetProductWeightBarcodeById(int id)
        {
            return this.DataProvider.GetProductWeightBarcodeById(id);
        }
        public List<ProductWeightBarcodes> GetProductWeightBarcodeValue(string epicorePartNo)
        {
            return this.DataProvider.GetProductWeightBarcodeValue(epicorePartNo);
        }
        public bool IsProductWeightBarcodeExists(ProductWeightBarcodes entityEn)
        {
            return this.DataProvider.IsProductWeightBarcodeExists(entityEn);
        }

        public bool CheckBarcodeExists(string barcode, string partNo)
        {
            return this.DataProvider.CheckBarcodeExists(barcode, partNo);
        }

        public ProductWeightBarcodeFileUploadResult SaveProductWeightBarcodeBunchList(ProductWeightBarcodeFileUpload entityEn)
        {
            return this.DataProvider.SaveProductWeightBarcodeBunchList(entityEn);
        }
    }
}
