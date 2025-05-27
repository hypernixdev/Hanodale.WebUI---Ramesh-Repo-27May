using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic;
using System.Collections;

namespace Hanodale.BusinessLogic
{
    public class LocalizationHandlerService : ILocalizationHandlerService
    {
        public Hanodale.DataAccessLayer.Interfaces.ILocalizationHandlerService DataProvider;

        public LocalizationHandlerService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.LocalizationHandlerService();
        }

        public IDictionary GetResources(LocalizationFilters entityFilter)
    {
        return this.DataProvider.GetResources(entityFilter);
    }

        public List<Localizations> GetLocalizationList(LocalizationFilters entityFilter)
        {
            return this.DataProvider.GetLocalizationList(entityFilter);
        }

        public Localizations GetLocalizationItem(Localizations entityEn)
        {
            return this.DataProvider.GetLocalizationItem(entityEn);
        }

        public bool IsLocalizationExists(Localizations entityEn)
        {
            return this.DataProvider.IsLocalizationExists(entityEn);
        }
    }
}
