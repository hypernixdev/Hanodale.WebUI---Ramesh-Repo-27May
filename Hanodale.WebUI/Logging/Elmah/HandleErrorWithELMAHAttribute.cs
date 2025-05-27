using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elmah;
using System.ComponentModel;
using System.Reflection;
using Hanodale.WebUI.Helpers;
using System.Threading;

namespace Hanodale.WebUI.Logging.Elmah.Helpers
{
    //From http://stackoverflow.com/questions/766610/
    public class HandleErrorWithELMAHAttribute : HandleErrorAttribute
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public override void OnException(ExceptionContext context)
        {
            try
            {
                base.OnException(context);

                var e = context.Exception;
                System.Diagnostics.Debug.WriteLine("Exception");
                System.Diagnostics.Debug.WriteLine(e.InnerException);
                if (!context.ExceptionHandled   // if unhandled, will be logged anyhow
                        || RaiseErrorSignal(e)      // prefer signaling, if possible
                        || IsFiltered(context))
                {
                    context.ExceptionHandled = true;
                    context.HttpContext.Response.Clear();
                    context.HttpContext.Response.TrySkipIisCustomErrors = true;
                    context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    if (context.Exception.Message.Contains("Invalid length for a Base-64 char array or string"))
                    {
                        context.HttpContext.Response.StatusDescription = GetEnumDescription(Common.ExceptionMessage.InvalidLengthBase64);
                    }
                    else if (context.Exception.Message.Contains("GET request. To allow GET requests"))
                    {
                        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotAcceptable;
                        context.HttpContext.Response.StatusDescription = GetEnumDescription(Common.ExceptionMessage.AllowGETRequest);
                    }
                    else
                    {
                        context.HttpContext.Response.StatusDescription = context.Exception.Message;
                    }
                }// filtered?

                if (context != null && context.HttpContext != null && context.HttpContext.Request != null && context.HttpContext.Request.IsAjaxRequest() && context.Exception != null)
                {
                    context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    context.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            context.Exception.Message,
                            context.Exception.StackTrace
                        }
                    };
                    context.ExceptionHandled = true;
                }

                return;

                LogException(e);
            }
            catch (Exception err)
            {
                //throw new ErrorException(err.Message);
            }
        }


        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return false;
            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
                return false;
            Thread thread = new Thread(() => signal.Raise(e, context));
            thread.Start();
            return true;
        }

        private static bool IsFiltered(ExceptionContext context)
        {
            var config = context.HttpContext.GetSection("elmah/errorFilter")
                                     as ErrorFilterConfiguration;

            if (config == null)
                return false;

            var testContext = new ErrorFilterModule.AssertionHelperContext(
                                                                context.Exception, HttpContext.Current);

            return config.Assertion.Test(testContext);
        }

        private static void LogException(Exception e)
        {
            var context = HttpContext.Current;
            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }
    }
}