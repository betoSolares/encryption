using System.Web.Optimization;

namespace encryption
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                        "~/Styles/menu.css"));

            // JS
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-3.4.1.min.js"));
        }
    }
}