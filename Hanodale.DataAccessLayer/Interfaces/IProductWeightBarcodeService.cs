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
    public interface IProductWeightBarcodeService
    {
        ProductWeightBarcodeDetails GetProductWeightBarcodeBySearch(DatatableFilters entityFilter);

        ProductWeightBarcodes CreateProductWeightBarcode(ProductWeightBarcodes entityEn);

        ProductWeightBarcodes UpdateProductWeightBarcode(ProductWeightBarcodes entityEn);

        bool DeleteProductWeightBarcode(int id);

        ProductWeightBarcodes GetProductWeightBarcodeById(int id);

        bool IsProductWeightBarcodeExists(ProductWeightBarcodes entityEn);

        bool CheckBarcodeExists(string barcode, string partNo);

        List<ProductWeightBarcodes> GetProductWeightBarcodeValue(string epicorePartNo);


        ProductWeightBarcodeFileUploadResult SaveProductWeightBarcodeBunchList(ProductWeightBarcodeFileUpload entityEn);
    }
}
