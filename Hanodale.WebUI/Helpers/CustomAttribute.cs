using Hanodale.BusinessLogic;
using Hanodale.Domain;
using Hanodale.Domain.DTOs;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Helpers
{
    public class CustomAttribute 
    {
       
    }


    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(GetRegex())
        { }

        private static string GetRegex()
        {
            // TODO: Go off and get your RegEx here
            //return @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$";
            //return @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$";
            return @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        }
    }

    public class PhoneNumberAttribute : RegularExpressionAttribute
    {
        public PhoneNumberAttribute()
            : base(GetRegex())
        { }

        private static string GetRegex()
        {
            // TODO: Go off and get your RegEx here
            return @"^(\+\d{1,2}\s?)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
        }
    }

}