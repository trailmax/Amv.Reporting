using System;
using System.Collections.Generic;
using System.Web.Optimization;

namespace AmvReporting
{
    public static class Bundles
    {
        public const String ClientJs = "~/bundles/clientjs";
        public const String ClientCss = "~/bundles/clientcss";

        public const String AdminJs = "~/bundles/adminjs";
        public const String AdminCss = "~/bundles/admincss";

    }

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(Bundles.ClientJs).Include(
                    "~/Scripts/modernizr-*",
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery-migrate-1.2.1.min.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/DataTables/js/jquery.dataTables.js",
                    "~/Scripts/DataTables/extras/FixedHeader/FixedHeader.js",
                    "~/Scripts/DataTables/extras/Scroller/js/dataTables.scroller.js",
                    "~/Scripts/DataTables/extras/TableTools/js/TableTools.js",
                    "~/Scripts/DataTables/extras/TableTools/js/ZeroClipboard.js",
                    "~/Scripts/ie8.js",
                    "~/Scripts/amplify.store.js",
                    "~/Scripts/jquery.treetable.js",
                    "~/Scripts/jquery.treetablePersist.js",
                    "~/Scripts/flot/excanvas.js",
                    "~/Scripts/flot/jquery.flot.js",
                    "~/Scripts/flot/jquery.flot.categories.js",
                    "~/Scripts/flot/jquery.flot.stack.patched.js"
                ));


            var clientStyleBundle = new BetterStyleBundle(Bundles.ClientCss).Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Scripts/Datatables/css/jquery.dataTables.css",
                "~/Scripts/Datatables/css/jquery.dataTables_themeroller.css",
                "~/Scripts/Datatables/extras/FixedHeader/FixedHeader.js",
                "~/Scripts/Datatables/extras/Scroller/css/dataTables.scroller.css",
                "~/Scripts/Datatables/extras/TableTools/css/TableTools.css",
                "~/Scripts/Datatables/extras/TableTools/css/TableTools_JUI.css",
                "~/Content/jquery.treetable.css");
            clientStyleBundle.Include("~/Content/jquery.treetable.theme.default.css");
            bundles.Add(clientStyleBundle);

            bundles.Add(new ScriptBundle(Bundles.AdminJs).Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery-ui-1.10.3.custom.js",
                "~/Scripts/toastr.min.js",
                "~/Scripts/CodeMirror/codemirror.js",
                "~/Scripts/CodeMirror/mode/sql.js",
                "~/Scripts/CodeMirror/mode/javascript.js",
                "~/Scripts/CodeMirror/mode/css.js",
                "~/Scripts/CodeMirror/mode/xml.js",
                "~/Scripts/CodeMirror/mode/htmlmixed.js",
                "~/Scripts/CodeMirror/addon/display/fullscreen.js",
                "~/Scripts/searchcursor.js",
                "~/Scripts/mergely.js"
                ));

            var adminStyleBundle = new BetterStyleBundle(Bundles.AdminCss).Include(
                "~/Content/toastr.min.css",
                "~/Content/mergely.css",
                "~/Scripts/CodeMirror/codemirror.css",
                "~/Scripts/CodeMirror/addon/display/fullscreen.css");
            bundles.Add(adminStyleBundle);

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
