using System.Web;
using System.Web.Optimization;

namespace Ui.EStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/MainJs").Include(
               "~/Scripts/MainJs.js",
               "~/Scripts/jquery/jquery*",
               "~/Scripts/json/json*",
               "~/Scripts/common/dataService.js",
               "~/Scripts/common/common.js",
               "~/Scripts/common/formdata.js",
                //"~/Scripts/AngularJs/angular-underscore.js",

               "~/Scripts/pnotify-1.2.0/jquery.pnotify.js",
               "~/Scripts/pnotify-1.2.0/jquery.cookie.js",
               "~/Scripts/Validation/validate2.js",
               "~/Scripts/Validation/ValidateScript.js",
               "~/Scripts/AngularJs/angular.js",
               "~/Scripts/AngularJs/angular-resource.js",
               "~/Scripts/AngularJs/angular-route.js",
               "~/Scripts/AngularJs/angular-sanitize.js",
               "~/Scripts/AngularJs/mustache.js",
                "~/Scripts/AngularJs/underscore.min.js",

                "~/Scripts/MainJs.js",
                "~/Scripts/js.js"

               ));
            //"~/Scripts/Bootstrap/bootstrap.min.js",
            bundles.Add(new StyleBundle("~/bundles/MainCSS").Include(

                 "~/Contents/responsive.css",
                 "~/Contents/style.css"
                ));
            //"~/Contents/Bootstrap/bootstrap-theme.css",
            //                "~/Contents/Bootstrap/bootstrap.css",
            bundles.Add(new ScriptBundle("~/bundles/AdminMainJs").Include(
               "~/Scripts/MainJs.js",
               "~/Scripts/jquery/jquery*",
               "~/Scripts/json/json*",
               "~/Scripts/common/dataService.js",
               "~/Scripts/common/common.js",
               "~/Scripts/common/formdata.js",
               "~/Scripts/pnotify-1.2.0/jquery.pnotify.js",
               "~/Scripts/pnotify-1.2.0/jquery.cookie.js",
               "~/Scripts/Validation/validate2.js",
               "~/Scripts/Validation/ValidateScript.js",
               "~/Scripts/AngularJs/angular.js",
               "~/Scripts/AngularJs/angular-resource.js",
               "~/Scripts/AngularJs/angular-route.js",
               "~/Scripts/AngularJs/angular-sanitize.js",
               "~/Scripts/AngularJs/mustache.js",
               "~/Scripts/AngularJs/underscore.min.js",
                "~/Scripts/AngularJs/angular-title.js",
                "~/Scripts/AngularJs/angular-xml2json.js",
                 "~/Scripts/Bootstrap/bootstrap.min.js",
                "~/Scripts/MainJs.js",
                "~/Scripts/js.js"

               ));

            bundles.Add(new StyleBundle("~/bundles/AdminMainCSS").Include(
                "~/Content/jquery/jquery*",
                 "~/Contents/Bootstrap/bootstrap-theme.css",
                 "~/Contents/Bootstrap/bootstrap.css",
                 "~/Contents/responsive.css",
                 "~/Contents/style.css"
                ));

            // Code removed for clarity.
            // BundleTable.EnableOptimizations = true;



            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
