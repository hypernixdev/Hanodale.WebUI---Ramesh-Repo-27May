﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static class GlobalizationHelpers
    {
        /// <summary>
        /// Taken from Scott Hanselman's blog post: http://www.hanselman.com/blog/GlobalizationInternationalizationAndLocalizationInASPNETMVC3JavaScriptAndJQueryPart1.aspx
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>

        public static IEnumerable<SelectListItem> InsertEmptyFirst(this IEnumerable<SelectListItem> list, string emptyText = "", string emptyValue = "")
        {
            return new[] { new SelectListItem { Text = emptyText, Value = emptyValue } }.Concat(list);
        }

        public static IHtmlString MetaAcceptLanguage<t>(this HtmlHelper<t> htmlHelper)
        {
            var acceptLanguage = HttpUtility.HtmlAttributeEncode(System.Globalization.CultureInfo.CurrentUICulture.ToString());
            return new HtmlString(string.Format("<meta name=\"accept-language\" content=\"{0}\" />", acceptLanguage));
        }

        /// <summary>
        /// Return the JavaScript bundle for this users culture
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <returns>a culture bundle that looks something like this: "~/js-culture.en-GB"</returns>
        public static string JsCultureBundle<t>(this HtmlHelper<t> htmlHelper)
        {
            return "~/js-culture.en-GB";// +System.Globalization.CultureInfo.CurrentUICulture.ToString();
        }
    }

    public static class CommonHtmlExtensions
    {
        public static object GetGlobalResource(this HtmlHelper htmlHelper, string resourceKey)
        {
            return htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(null, resourceKey);
        }

        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey);
        }

        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey, culture);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey, culture);
        }

    }
}