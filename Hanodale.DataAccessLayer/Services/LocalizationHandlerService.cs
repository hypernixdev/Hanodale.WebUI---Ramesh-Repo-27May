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
using Hanodale.DataAccessLayer.Interfaces;
using Hanodale.Domain;
using System.Collections.Specialized;

namespace Hanodale.DataAccessLayer.Services
{
    public class LocalizationHandlerService : BaseService, ILocalizationHandlerService
    {
        public IDictionary GetResources(LocalizationFilters entityFilter)
        {

            var resources = new ListDictionary();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var lst = model.LocalizationResources.Select(p => new { keyName = p.keyName, defaultValue = p.defaultValue, culture = p.LocalizationLanguageResources.FirstOrDefault(s => s.LocalizationLanguage.culture == entityFilter.cultureName) });

                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            resources.Add(item.keyName, (item.culture == null ? item.defaultValue : item.culture.value));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }

            return resources;
        }

        public List<Localizations> GetLocalizationList(LocalizationFilters entityFilter)
        {
            var _result = new List<Localizations>();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {

                }

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return _result;
        }

        public Localizations GetLocalizationItem(Localizations entityEn)
        {
            var obj = new Localizations();
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    var entity = model.LocalizationResources.SingleOrDefault(p => p.keyName == entityEn.keyName);
                    if (entity != null)
                    {
                        obj = new Localizations();
                        obj.id = entity.id;
                        obj.keyName = entity.keyName;
                        obj.value = entity.defaultValue;
                        if (entity.LocalizationLanguageResources != null)
                        {
                            var _culture = entity.LocalizationLanguageResources.FirstOrDefault(p => p.LocalizationLanguage.culture == entityEn.cultureName);
                            if (_culture != null)
                            {
                                obj.value = _culture.value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
            return obj;
        }

        public bool IsLocalizationExists(Localizations entityEn)
        {
            try
            {
                using (HanodaleEntities model = new HanodaleEntities())
                {
                    return true;
                    //return model.Localizations.Any(p => p.name == entityEn.name && p.code == entityEn.code && (entityEn.id == 0 ? true : p.id != entityEn.id));
                }
            }
            catch (Exception ex)
            {
                //we don't want to reveal any details to the client
                throw new FaultException(ex.InnerException.InnerException.Message);
            }
        }
    }
}
