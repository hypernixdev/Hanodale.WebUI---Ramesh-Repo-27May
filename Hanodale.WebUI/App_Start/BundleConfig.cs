using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace Hanodale.WebUI
{
    public static partial class bundles
    {
        public static partial class scripts
        {
            public static readonly string jquery = "~/scripts/jquery";
            public static readonly string layout_user_script = "~/scripts/jqueryui";

            public static readonly string dashboard = "~/scripts/dashboard";

            public static readonly string common_index = "~/scripts/ci";
            public static readonly string common_add_edit = "~/scripts/cae";

            public static readonly string user_index = "~/scripts/ui";
            public static readonly string user = "~/scripts/user";

            public static readonly string role_index = "~/scripts/role-index";

            public static readonly string user_role_index = "~/scripts/user-role-index";

            public static readonly string user_role = "~/scripts/role";

            public static readonly string user_rights = "~/scripts/ur";

            public static readonly string business_index = "~/scripts/business-index";

            public static readonly string helpdesk_add_edit = "~/scripts/helpdesk-add-edit";

            public static readonly string helpdesk_index = "~/scripts/helpdesk-index";

            public static readonly string business_add_edit = "~/scripts/business_add_edit";

            public static readonly string changepassword = "~/scripts/changepassword";

            public static readonly string fileupload = "~/scripts/fileupload";

            public static readonly string view_order = "~/scripts/vieworder";
        }

        public static partial class styles
        {
            public static readonly string layout = "~/styles/boostrap";

            public static readonly string font = "~/styles/fonts";

            public static readonly string common = "~/styles/common";

            public static readonly string IE7 = "~/styles/ie7";
            public static readonly string IE8 = "~/styles/ie8";
        }
    }

    public class BundleConfig
    {
        private const string delta = "~";

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {

            //bundles.IgnoreList.Clear();
            //AddDefaultIgnorePatterns(bundles.IgnoreList);
            BundleTable.EnableOptimizations = false ;
            //Layout style
            bundles.Add(new StyleBundle(Hanodale.WebUI.bundles.styles.layout).Include(
                Url(Links.Content.css.bootstrap_switch_css)
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.5.0/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.12.1/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/globalize/globalize.js",
                "~/Scripts/jquery.validate/jquery.validate.js",
                "~/Scripts/jquery.validate/jquery.validate.unobtrusive.bootstrap.js",
                "~/Scripts/jquery.validate/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate/jquery.validate.globalize.js",
                "~/Scripts/jquery.validate/jquery.validate.datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/login-jqueryval").Include(
                "~/Scripts/globalize/globalize.js",
                "~/Scripts/jquery.validate/jquery.validate.js",
                "~/Scripts/jquery.validate/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate/jquery.validate.globalize.js",
                "~/Scripts/jquery.validate/jquery.validate.datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
               "~/Scripts/datatable/jquery.dataTables.js",
               "~/Scripts/datatable/dataTables.bootstrap4.js",
               "~/Scripts/datatable/dataTables.buttons.js",
               "~/Scripts/datatable/buttons.bootstrap4.js",
               "~/Scripts/datatable/buttons.colVis.js",
               "~/Scripts/datatable/buttons.flash.js",
               "~/Scripts/datatable/buttons.html5.js",
               "~/Scripts/datatable/buttons.print.js",
               "~/Scripts/datatable/jszip.js",
               "~/Scripts/datatable/pdfmake.js",
               "~/Scripts/datatable/vfs_fonts.js",
               "~/Scripts/datatable/datatables.responsive.js",
               "~/Scripts/datatable/responsive.bootstrap4"));


            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                bundles.Add(new ScriptBundle("~/js-culture." + culture.Name).Include( //example bundle name would be "~/js-culture.en-GB"
                    DetermineCultureFile(culture, "~/Scripts/globalize/cultures/globalize.culture.{0}.js"),        //The Globalize locale-specific JavaScript file
                    DetermineCultureFile(culture, "~/Scripts/bootstrap-datepicker-locales/bootstrap-datepicker.{0}.js") //The Bootstrap Datepicker locale-specific JavaScript file
                ));
            }


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/plugin/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/globalize").Include(
                         "~/Scripts/plugin/jquery-validate/jquery.gvalidate*"));

            bundles.Add(new StyleBundle(Hanodale.WebUI.bundles.styles.font).Include(
                "~/Content/css/font-awesome.css",
                "~/Content/css/ace-fonts.css"));



            /// Common Index
            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.common_index).Include(
                //Url(Links.Scripts.ProtectedScripts.web_common_index_js)
            "~/Scripts/ProtectedScripts/web.common-index.js"
        ));

            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.common_add_edit).Include(
                //Url(Links.Scripts.ProtectedScripts.web_common_add_edit_js),
                //Url(Links.Scripts.bootstrap_switch_js)
              "~/Scripts/ProtectedScripts/web.common-add-edit.js",
                "~/Scripts/bootstrap-switch.js"
          ));


            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.view_order).Include(
              "~/Scripts/ProtectedScripts/vieworder.js"
            ));

            //   //User
            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.user_index).Include(
                //Url(Links.Scripts.ProtectedScripts.web_user_js)
               "~/Scripts/ProtectedScripts/web.user.js"
          ));

            //  bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.user).Include(
            //    Url(Links.Scripts.ProtectedScripts.web_user_add_edit_js),
            //    Url(Links.Scripts.bootstrap_switch_js)
            //));

            //User Rights
            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.user_rights).Include(
              "~/Scripts/ProtectedScripts/web.user-rights.js",
                "~/Scripts/bootstrap-switch.js"
                //Url(Links.Scripts.ProtectedScripts.web_user_rights_js),
                //Url(Links.Scripts.bootstrap_switch_js)
          ));

            //HelpDesk ADD & EDIT
            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.helpdesk_add_edit).Include(
              "~/Scripts/ProtectedScripts/web-helpdesk-add-edit.js"
                //Url(Links.Scripts.ProtectedScripts.web_helpdesk_add_edit_js)
          ));

            //HelpDesk Index
            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.helpdesk_index).Include(
              "~/Scripts/ProtectedScripts/web.helpdesk-index.js"
                //Url(Links.Scripts.ProtectedScripts.web_helpdesk_index_js)
          ));


            // Business Add & Edit
            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.business_add_edit).Include(
              "~/Scripts/ProtectedScripts/web.business-add-edit.js"
                //Url(Links.Scripts.ProtectedScripts.web_business_add_edit_js)
          ));



            // Change Password
            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.changepassword).Include(
              "~/Scripts/ProtectedScripts/web.changepassword.js"
                //Url(Links.Scripts.ProtectedScripts.web_changepassword_js)
          ));

            bundles.Add(new ScriptBundle(Hanodale.WebUI.bundles.scripts.fileupload).Include(
              "~/Scripts/ProtectedScripts/web.file-upload.js"
                //Url(Links.Scripts.ProtectedScripts.web_file_upload_js)
          ));


            //BundleTable.EnableOptimizations = true;
        }

        private static string Url(string fileName)
        {
            return string.Concat("~", fileName);
        }

        private static string DetermineCultureFile(CultureInfo culture,
            string filePattern,
            string defaultCulture = "en-CA" // I'm a Brit and this is my default
            )
        {
            //Determine culture - GUI culture for preference, user selected culture as fallback
            var regionalisedFileToUse = string.Format(filePattern, defaultCulture);

            //Try to pick a more appropriate regionalisation if there is one
            if (File.Exists(HttpContext.Current.Server.MapPath(string.Format(filePattern, culture.Name)))) //First try for a globalize.culture.en-GB.js style file
                regionalisedFileToUse = string.Format(filePattern, culture.Name);
            else if (File.Exists(HttpContext.Current.Server.MapPath(string.Format(filePattern, culture.TwoLetterISOLanguageName)))) //That failed; now try for a globalize.culture.en.js style file
                regionalisedFileToUse = string.Format(filePattern, culture.TwoLetterISOLanguageName);

            return regionalisedFileToUse;
        }
    }

    public class AppJsOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            // return files.OrderBy(f => f.Name == "eyNav.css" ? -1 : 1); 
            if (context == null)
                throw new ArgumentNullException("context");

            if (files == null)
                throw new ArgumentNullException("files");

            return files;
        }
    }
}