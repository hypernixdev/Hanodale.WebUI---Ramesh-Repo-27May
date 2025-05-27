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
    public interface IProductCartonService
    {
        ProductCartonDetails GetProductCartonBySearch(DatatableFilters entityFilter);

        ProductCartons CreateProductCarton(ProductCartons entityEn);

        ProductCartons UpdateProductCarton(ProductCartons entityEn);

        bool DeleteProductCarton(int id);

        ProductCartons GetProductCartonById(int id);

        bool IsProductCartonExists(ProductCartons entityEn);

        List<ProductCartons> GetProductCartonValue(string barcode);


        ProductCartonFileUploadResult SaveProductCartonBunchList(ProductCartonFileUpload entityEn);
    }
}
