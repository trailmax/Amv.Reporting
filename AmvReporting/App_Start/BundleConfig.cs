using System;
using System.Collections.Generic;
using System.Web.Optimization;

namespace AmvReporting
{
    public class BundleConfig
    {
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

            var styleBundle = new BetterStyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/Datatables-1.9.4/media/css/jquery.*",
                "~/Content/jquery.treetable.css");
            styleBundle.Include("~/Content/jquery.treetable.theme.default.css");
            bundles.Add(styleBundle);

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.min.js",
                "~/Scripts/ie8.js",
                "~/Scripts/amplify.store.js",
                "~/Scripts/jquery.treetablePersist.js"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                "~/Scripts/flot/jquery.flot.js",
                "~/Scripts/flot/jquery.flot.categories.js",
                "~/Scripts/flot/jquery.flot.stack.patched.js",
                "~/Scripts/flot/excanvas.js",
                "~/Scripts/jquery.treetable.js"));


#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }




    public class BetterStyleBundle : StyleBundle
    {
        public override IBundleOrderer Orderer
        {
            get
            {
                return new NonOrderingBundleOrderer();
            }

            set
            {
                throw new Exception("Unable to override Non-Ordred bundler");
            }
        }

        public BetterStyleBundle(string virtualPath)
            : base(virtualPath)
        {
        }


        public BetterStyleBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath)
        {
        }


        public override Bundle Include(params string[] virtualPaths)
        {
            foreach (var virtualPath in virtualPaths)
            {
                base.Include(virtualPath, new CssRewriteUrlTransform());
            }

            return this;
        }
    }

    // This provides files in the same order as they have been added. 
    public class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
