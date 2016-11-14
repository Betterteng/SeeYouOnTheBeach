using System.Web;
using System.Web.Optimization;

namespace SeeYouOnTheBeach.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/linq").Include(
            "~/Scripts/linq*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jsslider").Include(
                        "~/Scripts/jssor*"));

            bundles.Add(new ScriptBundle("~/bundles/canvasjs").Include(
                        "~/Scripts/canvasjs.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/beachdetail").Include(
                        "~/Scripts/beachdetail.js"));

            bundles.Add(new ScriptBundle("~/bundles/homepage").Include(
                        "~/Scripts/homepage.js"));

            bundles.Add(new ScriptBundle("~/bundles/filterpage").Include(
            "~/Scripts/filterpage.js"));

            bundles.Add(new ScriptBundle("~/bundles/floatpanel").Include(
            "~/Scripts/float-panel.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
