using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace HaselOne
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new ScriptBundle("~/bundles/js/global").Include(
                  "~/Scripts/JQuery/jquery-1.11.1.js",
                  "~/Scripts/jquery-ui_x/jquery-ui.min.js",
                  "~/Scripts/bootstrap/bootstrap.min.js",
                  "~/Scripts/Misc/respond.min.js",
                  "~/Scripts/Moment/moment.js",
                  "~/Scripts/_Resources/moment-locale_tr.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/devexpress").Include(
                  "~/Scripts/Devexpress/cldr.min.js",
                 "~/Scripts/Devexpress/cldr/event.min.js",
                 "~/Scripts/Devexpress/globalize.min.js",
                 "~/Scripts/Devexpress/jszip.js",
                  "~/Scripts/Devexpress/dx.all.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js/coreplugins").Include(
                  "~/Scripts/Misc/js.cookie.min.js",
                  "~/Scripts/Misc/jquery.slimscroll.min.js",
                  "~/Scripts/Misc/jquery.blockui.min.js",
                  //"~/Scripts/jquery-ui_x/jquery.ui.core.js",
                  "~/Scripts/jquery-ui_x/jquery.ui.widget.js",
                  "~/Scripts/jquery-ui_x/jquery.ui.mouse.js",
                  "~/Scripts/jquery-ui_x/jquery.ui.draggable.js",
                  "~/Scripts/jquery-ui_x/jquery.ui.resizable.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/angular").Include(
               "~/Scripts/Misc/es6-promise.min.js",
               "~/Scripts/Angular/angular.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js/angularLibs").Include(
                "~/Scripts/Angular/angular-sanitize.js",
                "~/Scripts/_Resources/angular-locale_tr.js",
                "~/Scripts/Angular/angular-messages.js",
                "~/Scripts/Angular/angular-moment.js",
                "~/Scripts/Angular/angular-cookies.js",
                "~/Scripts/Angular/angular-uimask.js",
                "~/Scripts/Angular/angular-notification.js",
                "~/Scripts/Angular/ng-table.min.js",
                "~/Scripts/Angular/ui-bootstrap-tpls-1.3.2.js",
                "~/Scripts/Angular/select.js",
                "~/Scripts/Angular/ngStorage.min.js",
                "~/Scripts/Angular/angular-ui-tree.min.js",
                "~/Scripts/Misc/jstree.min.js",
                "~/Scripts/Angular/ngJsTree.min.js",
                "~/Scripts/Memento/memento.core.js",
                "~/Scripts/Memento/memento.windowStorage.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js/layout").Include(
                "~/Scripts/Misc/jquery.signalR-2.2.1.js",
                //"~/Scripts/ChatScript.js",
                "~/Scripts/Misc/jquery.jqGrid.min.js",
                "~/Scripts/jqwidgets/jqx-all.js",
                "~/Scripts/Misc/bootbox.min.js",
                "~/Scripts/Misc/jquery.validate.js",
                "~/Scripts/Misc/sweetalert2.js",
                "~/Scripts/_Apps/app.js",
                "~/Scripts/_Apps/layout.js",
                "~/Scripts/Misc/quick-sidebar.js",
                "~/Scripts/Misc/quick-nav.js",
                "~/Scripts/Misc/select2.full.min.js",
                "~/Scripts/bootstrap/bootstrap-toolkit.js",
                "~/Scripts/Bootstrap/bootstrap-datepicker.js",
                "~/Scripts/_Resources/bootstrap-datepicker.tr.min.js",
                "~/Scripts/Bootstrap/bootstrap-datetimepicker.js",
                "~/Scripts/Misc/fileSaver.js",
                "~/Scripts/Misc/intro.js",
                "~/Scripts/_app.js",
                "~/Scripts/_modules.js",
                "~/Scripts/_Services/BaseService.js",
                "~/Scripts/_directives.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js/loginLayout").Include(
               "~/Scripts/jquery-validation/jquery.validate.min.js",
               "~/Scripts/jquery-validation/additional-methods.min.js",
               "~/Scripts/Misc/select2.full.min.js",
               "~/Scripts/_Apps/app.js"

               ));

            bundles.Add(new StyleBundle("~/bundles/css/devexpress").Include(
               "~/Content/Css/Devexpress/dx.common.css",
               "~/Content/Css/Devexpress/dx.spa.css",
               "~/Content/Css/Devexpress/dx.light.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/css/global").Include(
               "~/Content/Css/Icons/simple-line-icons.min.css",
               "~/Content/Css/Icons/font-awesome.css",
               "~/Content/Css/Bootstrap/bootstrap.min.css",
               "~/Content/Css/Misc/bootstrap-switch.min.css"

              ));

            bundles.Add(new StyleBundle("~/bundles/css/pageLevel").Include(
             "~/Content/Css/Misc/jquery-ui.css",
             "~/Content/Css/Misc/daterangepicker.min.css",
             "~/Content/Css/Misc/morris.css",
             "~/Content/Css/Misc/fullcalendar.min.css",
             "~/Content/Css/Misc/jqvmap.css",
             "~/Content/Css/jsTree/style.css",
             "~/Content/Css/Misc/select2.min.css",
             "~/Content/Css/Misc/select2-bootstrap.min.css"
             ));

            bundles.Add(new StyleBundle("~/bundles/css/loginPageLevel").Include(
             "~/Content/Css/Misc/select2.min.css",
              "~/Content/Css/Misc/select2-bootstrap.min.css"

             ));

            bundles.Add(new StyleBundle("~/bundles/css/theme").Include(
            "~/Content/Css/Theme/components.css",
            "~/Content/Css/Theme/plugins.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/css/layout").Include(
            "~/Content/Css/Layout/layout.min.css",
            "~/Content/Css/Layout/darkblue.min.css",
            "~/Content/Css/Layout/custom.min.css",
            "~/Content/Css/Misc/bootstrap-fileinput.css",
            "~/Content/Css/Pages/profile.min.css",
            "~/Content/Css/Misc/jquery-editable-select.css",
            "~/Content/Css/Misc/ng-table.css",
            "~/Content/Css/Misc/select.css",
            "~/Content/Css/Misc/sweetalert2.css",
            "~/Content/Css/Misc/introjs.css",
            "~/Content/Css/Bootstrap/bootstrap-datepicker3.css",
            "~/Content/Css/Bootstrap/bootstrap-datetimepicker.css",
            "~/Content/Css/Misc/angular-ui-tree.min.css",
            "~/Content/Css/Fix.css",
            "~/Content/Css/ChatPanel.css"
           ));
            bundles.Add(new StyleBundle("~/bundles/css/loginLayout").Include(
                "~/Content/Css/Pages/login.min.css"
                ));
        }
    }
}