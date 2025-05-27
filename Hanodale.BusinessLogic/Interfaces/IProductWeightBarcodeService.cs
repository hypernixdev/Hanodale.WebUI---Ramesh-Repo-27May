using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IProductWeightBarcodeService
    {
        ProductWeightBarcodeDetails GetProductWeightBarcode(DatatableFilters entityFilter);

        ProductWeightBarcodes SaveProductWeightBarcode(ProductWeightBarcodes entityEn);

        bool DeleteProductWeightBarcode(int id);

        ProductWeightBarcodes GetProductWeightBarcodeById(int id);

        bool IsProductWeightBarcodeExists(ProductWeightBarcodes entityEn);

        bool CheckBarcodeExists(string barcode, string partNo);

        List<ProductWeightBarcodes> GetProductWeightBarcodeValue(string epicorePartNo);



        ProductWeightBarcodeFileUploadResult SaveProductWeightBarcodeBunchList(ProductWeightBarcodeFileUpload entityEn);
    }
}
