using Hanodale.Domain.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface ILocalizationHandlerService
   {
       IDictionary GetResources(LocalizationFilters entityFilter);

       List<Localizations> GetLocalizationList(LocalizationFilters entityFilter);

       Localizations GetLocalizationItem(Localizations entityEn);

       bool IsLocalizationExists(Localizations entityEn);
    }
}
