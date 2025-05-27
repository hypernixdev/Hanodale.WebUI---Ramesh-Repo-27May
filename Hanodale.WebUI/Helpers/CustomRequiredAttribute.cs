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
    public class CustomRequiredAttribute : DisplayNameAttribute
    {
        private readonly string _propertyName;

        public CustomRequiredAttribute(string name)
        {
            _propertyName = name;
        }

        public override string DisplayName
        {
            get
            {
                string localizedDisplayName = GetLocalizedDisplayNameFromDatabase(_propertyName);

                return localizedDisplayName ?? base.DisplayName;
            }
        }

        // Implement your logic to retrieve the localized display name from the database
        private string GetLocalizedDisplayNameFromDatabase(string keyName)
        {
            try
            {
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