using System.Web;
using System.Web.Optimization;

namespace WERC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Resources/Scripts/jquery").Include(
                        "~/Resources/Scripts/jquery-{version}.js",
                        "~/Resources/Scripts/jquery.countup.js",
                        "~/Resources/Scripts/jquery.waypoints.min.js"));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/jqueryval").Include(
                        "~/Resources/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/Resources/Scripts/modernizr").Include(
                        "~/Resources/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/All-JS").Include(
                "~/Resources/Scripts/bootstrap.min.js",
                "~/Resources/Scripts/search.js",
                "~/Resources/Scripts/header-footer.js",
                "~/Resources/Scripts/respond.js",
                "~/Resources/Scripts/menu.js",
                //"~/Resources/Scripts/offcanvas.js",
                "~/Resources/Scripts/mousestop.js",
                "~/Resources/Controls/jsTree/dist/jstree.min.js",
                "~/Resources/Controls/jsgrid-1.5.3/Tools/jsgrid.min.js",
                "~/Resources/Scripts/tooltip.js",
                //"~/Resources/Scripts/testimonial.js",
                "~/Resources/Scripts/collapse-expand.js",
                "~/Resources/Scripts/jquery.easing.min.js",
                "~/Resources/Scripts/slick.min.js",
                "~/Resources/Scripts/portfolio.js",
                "~/Resources/Scripts/dragable-items.js",
                "~/Resources/Scripts/moveable-fixed-panel.js",
                "~/Resources/Scripts/editor.js",
                "~/Resources/Scripts/BootstrapSelect/bootstrap-select.min.js",
                "~/Resources/Scripts/jquery.lettering.js",
                "~/Resources/Controls/fullscreen-Loading-Indicator/js/HoldOn.min.js"
            ));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/jquery-UI").Include(
                
                "~/Resources/Scripts/jquery-ui.min.js",
                "~/Resources/Scripts/jquery.form.js"
            ));

            bundles.Add(new ScriptBundle("~/Resources/Scripts/chosen-JS").Include(
                "~/Resources/Scripts/chosen.jquery.min.js",
                "~/Resources/Scripts/chosen.proto.min.js"));

            bundles.Add(new StyleBundle("~/Resources/CSS/All-css").Include(
                "~/Resources/CSS/site_main-style.css",
                "~/Resources/CSS/bootstrap.css",
                "~/Resources/CSS/font.css",
                "~/Resources/CSS/font-awesome.min.css",
                "~/Resources/CSS/font-awesome-animation.min.css",
                "~/Resources/CSS/site.css",
                "~/Resources/CSS/menu.css",
                "~/Resources/CSS/search.css",
                "~/Resources/CSS/bootstrap.css",
                "~/Resources/CSS/footer.css",
                "~/Resources/CSS/logo.css",
                "~/Resources/CSS/about-modal.css",
                "~/Resources/CSS/modal.css",
                "~/Resources/CSS/contact-modal.css",
                "~/Resources/CSS/login.css",
                "~/Resources/CSS/members.css",
                "~/Resources/CSS/logout.css",
                "~/Resources/CSS/google-api-fonts.css",
                "~/Resources/CSS/spin-button.css",
                "~/Resources/CSS/offcanvas.css",
                "~/Resources/CSS/tootip.css",
                "~/Resources/Controls/jsgrid-1.5.3/Tools/jsgrid.min.css",
                "~/Resources/Controls/jsgrid-1.5.3/Tools/jsgrid-theme.min.css",
                "~/Resources/Controls/jsgrid-1.5.3/Tools/detail-style.css",
                "~/Resources/CSS/chosen.min.css",
                "~/Resources/CSS/collapse-expand.css",
                "~/Resources/CSS/slick-team-slider.css",
                "~/Resources/CSS/portfolio.css",
                "~/Resources/CSS/_draggable.css",
                "~/Resources/CSS/item-selection.css",
                "~/Resources/CSS/moveable-fixed-panel.css",
                "~/Resources/CSS/editor.css",
                "~/Resources/CSS/BootstrapSelect/bootstrap-select.min.css",
                "~/Resources/SCSS/text-circle.css",
                "~/Resources/Controls/fullscreen-Loading-Indicator/css/HoldOn.min.css"
            ));

            bundles.Add(new StyleBundle("~/Resources/Controls/jsTree/dist/themes/default/style-css").Include(
                "~/Resources/Controls/jsTree/dist/themes/default/style.css"));

            bundles.Add(new StyleBundle("~/Resources/CSS/jquery-ui").Include(
                "~/Resources/CSS/jquery-ui.min.css"));

        }
    }
}

