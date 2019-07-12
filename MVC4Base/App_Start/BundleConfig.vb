Imports System.Web
Imports System.Web.Optimization

Public Class BundleConfig
    ' Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
    Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)
        'JS
        bundles.Add(New ScriptBundle("~/bundles/template").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/bootstrap.js",
                    "~/Content/vendor/fastclick/lib/fastclick.js",
                    "~/Content/vendor/perfect-scrollbar/js/perfect-scrollbar.jquery.js",
                    "~/Scripts/helpers/smartresize.js",
                    "~/Scripts/constants.js",
                    "~/Scripts/main.js"))

        bundles.Add(New ScriptBundle("~/bundles/backbone").Include(
                    "~/Scripts/underscore.js",
                    "~/Scripts/backbone.js"))

        bundles.Add(New ScriptBundle("~/bundles/underscore").Include(
                    "~/Scripts/underscore.js"))

        bundles.Add(New ScriptBundle("~/bundles/jquery").Include(
                   "~/Scripts/jquery-{version}.js"))

        bundles.Add(New ScriptBundle("~/bundles/jqueryui").Include(
                    "~/Scripts/jquery-ui-{version}.js"))

        bundles.Add(New ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.unobtrusive*",
                    "~/Scripts/jquery.validate*"))

        bundles.Add(New ScriptBundle("~/bundles/datatable").Include(
                    "~/Scripts/datatable/jquery.dataTables.js",
                    "~/Scripts/datatable/dataTables.bootstrap.js",
                    "~/Scripts/datatable/dataTables.responsive.js",
                    "~/Scripts/datatable/jquery.dataTables.languajes.js"))

        ' Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
        ' preparado para la producción y podrá utilizar la herramienta de creación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"))

        bundles.Add(New ScriptBundle("~/bundles/blockUI").Include(
                    "~/Scripts/jquery.blockUI.js"))

        bundles.Add(New ScriptBundle("~/bundles/datepicker").Include("~/Content/vendor/bootstrap-datepicker/dist/js/bootstrap-datepicker.js"))
        bundles.Add(New ScriptBundle("~/bundles/timepicker").Include("~/Content/vendor/bootstrap-timepicker/js/bootstrap-timepicker.js"))
        bundles.Add(New ScriptBundle("~/bundles/clockpicker").Include("~/Content/vendor/clockpicker/dist/bootstrap-clockpicker.js"))
        bundles.Add(New ScriptBundle("~/bundles/noty").Include("~/Scripts/jquery.noty.packaged.js", "~/Scripts/helpers/noty-defaults.js"))
        bundles.Add(New ScriptBundle("~/bundles/chosen").Include("~/Content/vendor/chosen_v1.4.0/chosen.jquery.js",
                                                                 "~/Content/vendor/chosen_v1.4.0/chosen.jquery.min.js"))
        bundles.Add(New ScriptBundle("~/bundles/card").Include("~/Content/vendor/card/lib/js/jquery.card.js"))
        bundles.Add(New ScriptBundle("~/bundles/jqurey-validation").Include("~/Content/vendor/jquery-validation/dist/jquery.validate.js"))
        bundles.Add(New ScriptBundle("~/bundles/checkbo").Include("~/Content/vendor/checkbo/src/0.1.4/js/checkBo.js"))
        bundles.Add(New ScriptBundle("~/bundles/twitter-bootstrap-wizard").Include("~/Content/vendor/twitter-bootstrap-wizard/jquery.bootstrap.wizard.js"))

        bundles.Add(New ScriptBundle("~/Content/vendor/flot").Include(
                    "~/Content/vendor/flot/jquery.flot.js",
                    "~/Content/vendor/flot/jquery.flot.resize.js",
                    "~/Content/vendor/flot/jquery.flot.categories.js",
                    "~/Content/vendor/flot/jquery.flot.stack.js",
                    "~/Content/vendor/flot/jquery.flot.time.js",
                    "~/Content/vendor/flot/jquery.flot.pie.js",
                    "~/Content/vendor/flot/flot-spline/js/jquery.flot.spline.js"))

        'Treeview
        bundles.Add(New ScriptBundle("~/bundles/treeView").Include(
                    "~/Scripts/treeView.js"))

        'CSS
        'Template
        bundles.Add(New StyleBundle("~/Content/template").Include(
                  "~/Content/css/webfont.css",
                  "~/Content/css/climacons-font.css",
                  "~/Content/bootstrap.css",
                  "~/Content/css/card.css",
                  "~/Content/css/sli.css",
                  "~/Content/css/font-awesome.css",
                  "~/Content/css/animate.css",
                  "~/Content/css/app.css",
                  "~/Content/css/app.skins.css",
                  "~/Content/css/font-awesome.css"))

        bundles.Add(New StyleBundle("~/Content/css").Include("~/Content/site.css"))

        bundles.Add(New StyleBundle("~/Content/datatable").Include(
                    "~/Content/css/datatables/dataTables.bootstrap.css",
                    "~/Content/css/datatables/responsive.dataTables.css",
                    "~/Content/css/datatables/responsive.bootstrap.css"))

        bundles.Add(New StyleBundle("~/Content/themes/base/css").Include(
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
                    "~/Content/themes/base/jquery.ui.theme.css"))

        bundles.Add(New StyleBundle("~/Content/datepicker").Include("~/Content/vendor/bootstrap-datepicker/dist/css/bootstrap-datepicker3.css"))
        bundles.Add(New StyleBundle("~/Content/clockpicker").Include("~/Content/vendor/clockpicker/dist/bootstrap-clockpicker.css"))

        'JS Customize
        bundles.Add(New ScriptBundle("~/bundles/Util").Include(
                    "~/Scripts/Util.js"))

        'CSS Customize
        bundles.Add(New StyleBundle("~/Content/base").Include(
                  "~/Content/css/base.css"))

        bundles.Add(New StyleBundle("~/Content/login").Include(
                  "~/Content/css/login.css"))

        'Chosen
        bundles.Add(New StyleBundle("~/Content/vendor/chosen").Include("~/Content/vendor/chosen_v1.4.0/chosen.css",
                                                                       "~/Content/vendor/chosen_v1.4.0/chosen.min.css"))

        'Treeview

        bundles.Add(New StyleBundle("~/Content/treeView").Include(
                    "~/Content/themes/base/treeView.css"))
    End Sub
End Class