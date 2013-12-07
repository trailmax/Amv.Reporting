using System.Web.Optimization;

namespace AmvReporting
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/Datatables-1.9.4/media/css/jquery.*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.min.js",
                "~/Scripts/flot/jquery.*",
                "~/Scripts/flot/excanvas.js"));

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
