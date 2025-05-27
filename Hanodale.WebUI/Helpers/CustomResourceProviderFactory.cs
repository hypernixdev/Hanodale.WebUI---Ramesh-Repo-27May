using Hanodale.BusinessLogic;
using Hanodale.Domain;
using Hanodale.Domain.DTOs;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.UI.Design;

namespace Hanodale.WebUI.Helpers
{
    //[DesignTimeResourceProviderFactoryAttribute(typeof(CustomDesignTimeResourceProviderFactory))]
    public sealed class CustomResourceProviderFactory : ResourceProviderFactory
    {

        public CustomResourceProviderFactory()
        {
        }

        public override IResourceProvider
               CreateGlobalResourceProvider(string classKey)
        {
            return new CustomResourceProvider(null, classKey);
        }
        public override IResourceProvider
               CreateLocalResourceProvider(string virtualPath)
        {
            virtualPath = System.IO.Path.GetFileName(virtualPath);
            return new CustomResourceProvider(virtualPath, null);
        }
    }

    public sealed class CustomResourceProvider : IResourceProvider
    {
        private string _virtualPath;
        private string _className;
        private IDictionary _resourceCache;
        private static object CultureNeutralKey = new object();
        ILocalizationHandlerService svc;

        public CustomResourceProvider(string virtualPath, string className)
        {
            _virtualPath = virtualPath;
            _className = className;
            svc = ServiceLocator.Current.GetInstance<ILocalizationHandlerService>();
        }

        private IDictionary GetResourceCache(string cultureName)
        {
            object cultureKey;
            if (cultureName != null)
            {
                cultureKey = cultureName;
            }
            else
            {
                CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
                if (!String.Equals(currentUICulture.Name, CultureInfo.InstalledUICulture.Name))
                {
                    cultureKey = currentUICulture.Name;
                }
                else
                {
                    cultureKey = CultureNeutralKey;
                }
            }

            if (_resourceCache == null)
            {
                _resourceCache = new ListDictionary();
            }
            IDictionary resourceDict = _resourceCache[cultureKey] as IDictionary;
            if (resourceDict == null || (resourceDict == null && resourceDict.Count == 0))
            {
                resourceDict = CustomResourceHelper.GetResources(_virtualPath, _className, cultureName, svc);
                _resourceCache[cultureKey] = resourceDict;
            }
            return resourceDict;
        }


        object IResourceProvider.GetObject(string resourceKey, CultureInfo culture)
        {
            string cultureName = string.Empty ;
            if (culture != null)
            {
                cultureName = culture.Name;
            }
            else
            {
                cultureName = CultureInfo.CurrentUICulture.Name;
            }


            object value = GetResourceCache(cultureName)[resourceKey];
            //if (value == null)
            //{
            //    // resource is missing for current culture, use default
            //    SqlResourceHelper.AddResource(resourceKey,
            //            _virtualPath, _className, cultureName);
            //    value = GetResourceCache(null)[resourceKey];
            //}
            //if (value == null)
            //{
            //    // the resource is really missing, no default exists
            //    SqlResourceHelper.AddResource(resourceKey,
            //         _virtualPath, _className, string.Empty);
            //}
            return value;
        }
        IResourceReader IResourceProvider.ResourceReader
        {
            get
            {
                return new CustomResourceReader(GetResourceCache(null));
            }
        }
    }

    public sealed class CustomResourceReader : IResourceReader
    {
        private IDictionary _resources;
        public CustomResourceReader(IDictionary resources)
        {
            _resources = resources;
        }
        IDictionaryEnumerator IResourceReader.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }
        void IResourceReader.Close()
        {
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }
        void IDisposable.Dispose()
        {
        }
    }

    internal static class CustomResourceHelper
    {
        public static IDictionary GetResources(string virtualPath, string className, string cultureName, ILocalizationHandlerService serviceProvider)
        {

            var resources = new ListDictionary();
            try
            {
                return serviceProvider.GetResources(new LocalizationFilters { cultureName = cultureName });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }

            return resources;
        }

    }

    //public class CustomResourceProviderFactory : ResourceProviderFactory
    //{
    //    public override IResourceProvider
    //      CreateGlobalResourceProvider(string classname)
    //    {
    //        return new CustomResourceProvider(null, classname);
    //    }
    //    public override IResourceProvider
    //      CreateLocalResourceProvider(string virtualPath)
    //    {
    //        return new CustomResourceProvider(virtualPath, null);
    //    }
    //}

    //// Define the resource provider for global and local resources.
    //internal class CustomResourceProvider : IResourceProvider
    //{
    //    string _virtualPath;
    //    string _className;

    //    public CustomResourceProvider(string virtualPath, string classname)
    //    {
    //        _virtualPath = virtualPath;
    //        _className = classname;
    //    }

    //    private IDictionary GetResourceCache(string culturename)
    //    {
    //        return (IDictionary)
    //            System.Web.HttpContext.Current.Cache[culturename];
    //    }

    //    object IResourceProvider.GetObject
    //        (string resourceKey, CultureInfo culture)
    //    {
    //        object value;

    //        string cultureName = null;
    //        if (culture != null)
    //        {
    //            cultureName = culture.Name;
    //        }
    //        else
    //        {
    //            cultureName = CultureInfo.CurrentUICulture.Name;
    //        }

    //        value = GetResourceCache(cultureName)[resourceKey];
    //        value = GetResourceCache(null)[resourceKey];
    //        return value;
    //    }

    //    IResourceReader IResourceProvider.ResourceReader
    //    {
    //        get
    //        {
    //            string cultureName = null;
    //            CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
    //            if (!String.Equals(currentUICulture.Name,
    //                CultureInfo.InstalledUICulture.Name))
    //            {
    //                cultureName = currentUICulture.Name;
    //            }

    //            return new CustomResourceReader
    //                (GetResourceCache(cultureName));
    //        }
    //    }
    //}

    //internal sealed class CustomResourceReader : IResourceReader
    //{
    //    private IDictionary _resources;

    //    public CustomResourceReader(IDictionary resources)
    //    {
    //        _resources = resources;
    //    }

    //    IDictionaryEnumerator IResourceReader.GetEnumerator()
    //    {
    //        return _resources.GetEnumerator();
    //    }

    //    void IResourceReader.Close() { }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return _resources.GetEnumerator();
    //    }

    //    void IDisposable.Dispose() { return; }
    //}
}