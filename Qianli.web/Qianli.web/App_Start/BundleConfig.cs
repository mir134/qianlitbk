using System.Web;
using System.Web.Optimization;

namespace Qianli.web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"                        
                        ));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));           

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/toutiao/index").Include(
                      "~/Content/toutiao/css/global-min.css",
                      "~/Content/toutiao/css/index.css",
                      "~/Content/toutiao/css/index-min.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/toutiao/goods").Include(                   
                      "~/Content/toutiao/css/a.css"                    
                      ));
            bundles.Add(new ScriptBundle("~/bundles/jstoutiao").Include(
                       "~/Content/toutiao/js/index.js",
                     //  "~/Content/toutiao/js/config.js",
                      // "~/Content/toutiao/js/aplus_v2.js",
                       "~/Content/toutiao/js/index2.js"));

            bundles.Add(new StyleBundle("~/Content/tt/vcss").Include(
                      "~/Content/tt/css/base.css",
                      "~/Content/tt/css/reading.css",
                      "~/Content/tt/css/base-read-mode.css"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                      "~/Content/tt/js/base.js",
                   "~/Content/tt/js/reading-base.js"));
        }
    }
}
