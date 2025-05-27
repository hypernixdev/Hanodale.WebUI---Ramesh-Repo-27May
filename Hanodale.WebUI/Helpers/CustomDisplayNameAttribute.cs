using Hanodale.BusinessLogic;
using Hanodale.Domain;
using Hanodale.Domain.DTOs;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Helpers
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CustomDisplayNameAttribute : DisplayNameAttribute
    {
        // Register the service with the Service Locator
        // Resolve and use the service

        //ILocalizationHandlerService svc;

        private readonly string _propertyName;

        //private readonly Dictionary<int, string> _resourceType;

        //private Dictionary<string, string> _dic;

        public CustomDisplayNameAttribute(string name)
        {
            _propertyName = name;
           // svc = ServiceLocator.Current.GetInstance<ILocalizationHandlerService>();
            //_resourceType = resourceType;
            //_dic = resourceType;

        //    _dic = new Dictionary<string, string>
        //{
        //    { "CODE", "HELLO" },
        //    { "CODE1", "BYE!" },
        //    // Add other property names and their localized display names
        //};

        }

        public override string DisplayName
        {
            get
            {

                

                // Fetch the localized display name from the database based on the current language
                // You can implement your logic here (e.g., querying a resource table or localization service)
                // Replace this placeholder with your actual database retrieval code
                string localizedDisplayName = GetLocalizedDisplayNameFromDatabase(_propertyName);

                return localizedDisplayName ?? base.DisplayName;
            }
        }

        // Implement your logic to retrieve the localized display name from the database
        private string GetLocalizedDisplayNameFromDatabase(string keyName)
        {
            try
            {
                //HttpCookie cookie = HttpContext.Current.Request.Cookies["culture"];
                //var _culture = string.Empty;
                //if (cookie != null && cookie.Value != null)
                //{
                //    _culture=cookie.Value;
                //}

                var obj = HttpContext.GetGlobalResourceObject("", keyName);
                if (obj != null)
                {
                    return obj.ToString();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}