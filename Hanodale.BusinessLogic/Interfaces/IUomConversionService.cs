using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IUomConversionService
    {
        UomConversionDetails GetUomConversion(DatatableFilters entityFilter);

        UomConversions SaveUomConversion(UomConversions entityEn);

        bool DeleteUomConversion(int id);

        UomConversions GetUomConversionById(int id);

        bool IsUomConversionExists(UomConversions entityEn);
    }
}
