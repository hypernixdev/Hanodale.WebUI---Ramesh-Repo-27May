using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IProductCartonService
    {
        ProductCartonDetails GetProductCarton(DatatableFilters entityFilter);

        ProductCartons SaveProductCarton(ProductCartons entityEn);

        bool DeleteProductCarton(int id);

        ProductCartons GetProductCartonById(int id);

        bool IsProductCartonExists(ProductCartons entityEn);

        List<ProductCartons> GetProductCartonValue(string barcode);


        ProductCartonFileUploadResult SaveProductCartonBunchList(ProductCartonFileUpload entityEn);
    }
}
