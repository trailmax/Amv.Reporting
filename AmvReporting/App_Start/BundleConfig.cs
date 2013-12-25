using System.Web.Optimization;

namespace AmvReporting
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-1.10.3.custom.js"));

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
                "~/Content/Datatables-1.9.4/media/css/jquery.*",
                "~/Content/jquery.treetable.css",
                "~/Content/jquery.treetable.theme.default.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.min.js",
                "~/Scripts/ie8.js",
                "~/Scripts/amplify.store.js",
                "~/Scripts/jquery.treetablePersist.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                "~/Scripts/flot/jquery.flot.js",
                "~/Scripts/flot/jquery.flot.categories.js",
                "~/Scripts/flot/jquery.flot.stack.patched.js",
                "~/Scripts/flot/excanvas.js",
                "~/Scripts/jquery.treetable.js"
                ));


#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
