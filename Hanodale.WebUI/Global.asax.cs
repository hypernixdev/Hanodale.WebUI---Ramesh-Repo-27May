using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Logging.Elmah;
using Hanodale.WebUI.Models;
using Hanodale.WebUI.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Hanodale.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        //  DispatcherClient dispatchClient = new DispatcherClient();
        //  NotifierClient notifyClient = new NotifierClient();
        private static IUnityContainer container;
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("PDF_Vendor_Files/HRMS User Manual - Vendor.pdf");

            routes.MapRoute(
                "Dashboard", // Route name
                "Dashboard", // URL
                new { controller = "Dashboard", action = "Index" } // Parameter defaults
                );

            routes.MapRoute(
                "Pickup", // Route name
                "Pickup", // URL
                new { controller = "Pickup", action = "Index" } // Parameter defaults
                );

            routes.MapRoute(
              "Orders", // Route name
              "Orders", // URL
              new { controller = "Orders", action = "Index" } // Parameter defaults
              );

            routes.MapRoute(
              "LooseConversion", // Route name
              "LooseConversion", // URL
              new { controller = "LooseConversion", action = "Index" } // Parameter defaults
              );

            routes.MapRoute(
              "Store", // Route name
              "Store", // URL
              new { controller = "Store", action = "Index" } // Parameter defaults
              );

       
            routes.MapRoute(
            "Customer", // Route name
            "Customer", // URL
            new { controller = "Customer", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
              "ShipToAddress", // Route name
              "ShipToAddress", // URL
              new { controller = "ShipToAddress", action = "Index" } // Parameter defaults
              );

            routes.MapRoute(
            "PriceList", // Route name
            "PriceList", // URL
            new { controller = "PriceList", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
            "ProductCarton", // Route name
            "ProductCarton", // URL
            new { controller = "ProductCarton", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
            "ProductWeightBarcode", // Route name
            "ProductWeightBarcode", // URL
            new { controller = "ProductWeightBarcode", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "StockBalance", // Route name
                "StockBalance", // URL
                new { controller = "StockBalance", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "SchedulerSetup", // Route name
                "SchedulerSetup", // URL
                new { controller = "SchedulerSetup", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
             "Product", // Route name
             "Product", // URL
             new { controller = "Product", action = "Index" } // Parameter defaults
         );

            routes.MapRoute(
               "Terms", // Route name
               "Terms", // URL
               new { controller = "Terms", action = "Index" } // Parameter defaults
               );

            routes.MapRoute(
                "ChangePassword", // Route name
                "ChangePassword", // URL
                new { controller = "ChangePassword", action = "Index" } // Parameter defaults
                );

            routes.MapRoute(
                "ChartPanelWODashboard", // Route name
                "ChartPanelWODashboard", // URL
                new { controller = "ChartPanelWODashboard", action = "Index" } // Parameter defaults
                );
            routes.MapRoute(
              "ViewTicketDetail", // Route name
              "ViewTicketDetail", // URL
              new { controller = "HelpDesk", action = "Edit", id = UrlParameter.Optional, readOnly = UrlParameter.Optional, hideBackButton = UrlParameter.Optional } // Parameter defaults
              );


            routes.MapRoute(
               "Error", // Route name
               "Error", // URL
               new { controller = "Error", action = "Index" } // Parameter defaults
               );

            routes.MapRoute(
                "AccountVerify", // Route name
                "Auth/AccountVerify/{toVerifyHash}", // URL
                new { controller = "Auth", action = "AccountVerify" } // Parameter defaults
                );
            routes.MapRoute(
               "AuthIdentify", // Route name
               "Auth/AccountIdentify/{toVerifyHash}", // URL
               new { controller = "Auth", action = "AccountIdentify" } // Parameter defaults
               );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        public override void Init()
        {
            this.PostAuthenticateRequest += this.PostAuthenticateRequestHandler;
            this.EndRequest += this.EndRequestHandler;

            base.Init();
        }

        
        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(DateTime?), new MyDateTimeModelBinder());
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");

            //dispatchClient.InsertData();
            //notifyClient.GetData();
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;


            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //BundleTable.EnableOptimizations = true;

            //net::ERR_CONNECTION_RESET 200 (OK)
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            InitializeDependencyInjectionContainer();

            // Setup our custom controller factory so that the [HandleErrorWithElmah] attribute
            // is automatically injected into all of the controllers
            ControllerBuilder.Current.SetControllerFactory(new ErrorHandlingControllerFactory());

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PhoneNumberAttribute), typeof(RegularExpressionAttributeAdapter));


        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);

            //Do a direct 401 unautorized
            if (Context.Response.StatusCode == 400)
            {
            }
            if (Context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {
                if (!context.Request.IsAuthenticated)
                {
                    Context.Response.Clear();
                    Context.Response.StatusCode = 401;
                }
            }
        }

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first
                Response.Redirect("Dashboard/Dashboard", true);
            }
        }

        private void EndRequestHandler(object sender, EventArgs e)
        {
            
        }

        private void PostAuthenticateRequestHandler(object sender, EventArgs e)
        {
            HttpCookie authCookie = this.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (IsValidAuthCookie(authCookie))
            {
                var formsAuthentication = ServiceLocator.Current.GetInstance<IFormsAuthentication>();

                var ticket = formsAuthentication.Decrypt(authCookie.Value);
                var blackSunIdentity = new HanodaleIdentity(ticket);
                this.Context.User = new GenericPrincipal(blackSunIdentity, null);

                //// Reset cookie for a sliding expiration.
                //formsAuthentication.SetAuthCookie(this.Context, ticket);
            }
        }

        private static bool IsValidAuthCookie(HttpCookie authCookie)
        {
            return authCookie != null && !String.IsNullOrEmpty(authCookie.Value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "This should survive the lifetime of the application.")]
        private static void InitializeDependencyInjectionContainer()
        {
            container = new UnityContainerFactory().CreateConfiguredContainer();
            var serviceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
         {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["culture"];

            if (cookie != null && cookie.Value != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
            }
            else
            {

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("En");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("En");
            }
        }


    }
}