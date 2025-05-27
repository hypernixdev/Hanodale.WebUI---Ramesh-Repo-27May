using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;

namespace Hanodale.BusinessLogic
{
    public class ProductCartonService :  IProductCartonService
    {
        public Hanodale.DataAccessLayer.Interfaces.IProductCartonService DataProvider;

        public ProductCartonService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.ProductCartonService();
        }

        public ProductCartonDetails GetProductCarton(DatatableFilters entityFilter)
        {
            return this.DataProvider.GetProductCartonBySearch(entityFilter);
        }

        public ProductCartons SaveProductCarton(ProductCartons entityEn)
        {
            if (entityEn.id > 0)
                return this.DataProvider.UpdateProductCarton(entityEn);
            else
                return this.DataProvider.CreateProductCarton(entityEn);
        }

        public bool DeleteProductCarton(int id)
        {
            return this.DataProvider.DeleteProductCarton(id);
        }

        public ProductCartons GetProductCartonById(int id)
        {
            return this.DataProvider.GetProductCartonById(id);
        }

        public bool IsProductCartonExists(ProductCartons entityEn)
        {
            return this.DataProvider.IsProductCartonExists(entityEn);
        }

        public ProductCartonFileUploadResult SaveProductCartonBunchList(ProductCartonFileUpload entityEn)
        {
            return this.DataProvider.SaveProductCartonBunchList(entityEn);
        }

        public List<ProductCartons> GetProductCartonValue(string barcode)
        {
            return this.DataProvider.GetProductCartonValue(barcode);
        }

    }
}
