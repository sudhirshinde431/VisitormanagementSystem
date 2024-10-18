using System.Web;
using System.Web.Optimization;

namespace VisitorsManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                         "~/Scripts/popper.min.js",
                         "~/Scripts/bootstrap.min.js",
                         "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/loginscript").Include(
                         "~/Angular/LoginController.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs")
                      .Include("~/Scripts/angular.js")
                      .Include("~/Angular/Chart.min.js")
                      .Include("~/Angular/highcharts.js")
                      .Include("~/Angular/toaster.min.js")
                      .Include("~/Angular/freeze-table.js")
                      .Include("~/Angular/exporting.js")
                      .Include("~/Angular/angular-idle.js")
                      .Include("~/Angular/angular-sanitize.js")
                      .Include("~/Scripts/ngStorage.min.js")
                      .Include("~/Angular/dirPaginationNew.js")
                      .Include("~/Angular/xlsx.full.min.js")
                      .Include("~/Angular/ng-file-upload.min.js")
                      .Include("~/Angular/jspdf.umd.min.js")
                      .Include("~/Angular/html2canvas.js")
                      .Include("~/Angular/ng-table.js")
                      .Include("~/Angular/print.min.js")
                      .Include("~/Angular/jquery.stickytableheaders.js")
                      .Include("~/Angular/bootstrap-datepicker.js")
                      .Include("~/Angular/Module.js")
                      .Include("~/Angular/Factory.js")
                      .Include("~/Angular/Controller.js")
                      .Include("~/Angular/Services.js")
                      .Include("~/Angular/SipocFactory.js")
                      .Include("~/Angular/angular-animate.js")
                      .Include("~/Angular/ui-bootstrap-tpls-2.5.0.js")
                      .Include("~/Angular/bootstrap-select.min.js")
                      .Include("~/Angular/CalcSPC.js")
                      .Include("~/Angular/dragscroll.js")
                      .Include("~/Angular/alasql.min.js")
                      .Include("~/Angular/alasql.xlsx.core.min.js")
                     
                      .Include("~/Angular/lodash.min.js"));

       

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                       "~/Content/ng-table.css",
                      "~/Content/bootstrap-select.css",
                      "~/Content/all.css",
                      "~/Content/site.css",
                      "~/Content/toaster.min.css",
                      "~/Content/boxicons.min.css",
                      "~/Content/Design/CSS/bootstrap-datepicker.css"
                      ));

        }
    }
}
