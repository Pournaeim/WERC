$(document).ready(function ($) {
    $(".pricingTable-wrap-toggle").click(function () {
        var self = this;
         
        $(self).siblings(".pricingTable-wrap").slideToggle("fast", "swing", function () {
            $(self).siblings(".pricingTable-wrap").toggleClass("collapsed-xs");
            $(self).siblings(".pricingTable-wrap").toggleClass('expand-in-none-xs');
            $(self).toggleClass('pricingTable-wrap-toggle-expand');

            $(self).siblings(".pricingTable-wrap").removeAttr("style");

        });

        //if ($(".pricingTable-wrap-toggle").html() == "Expand Details") {
        //    $(".pricingTable-wrap-toggle").html("Hide Details")
        //}
        //else {
        //    $(".pricingTable-wrap-toggle").html("Expand Details")
        //}

        if ($(".pricingTable-wrap-toggle").children(".pricingTable-arrow").hasClass('fa-arrow-circle-down') === true) {

            $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").addClass("fa-arrow-circle-up");
            $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").removeClass("fa-arrow-circle-down");
        }
        else {

            $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").removeClass("fa-arrow-circle-up");
            $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").addClass("fa-arrow-circle-down");
        }
    });
    $(".pricingTable-header").click(function () {

        
        var self = this;
         
        if (window.innerWidth < 768) {
            $(this).siblings(".pricingTable-toggle-wrapper").find(".pricingTable-wrap").slideToggle("fast", "swing", function () {
                 
                $(this).toggleClass("collapsed-xs");
                $(this).toggleClass('expand-in-none-xs');

                //$(this).toggleClass('pricingTable-wrap-toggle-expand');

                $(this).removeAttr("style");

            });

            if ($(".pricingTable-wrap-toggle").children(".pricingTable-arrow").hasClass('fa-arrow-circle-down') === true) {

                $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").addClass("fa-arrow-circle-up");
                $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").removeClass("fa-arrow-circle-down");
            }
            else {

                $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").removeClass("fa-arrow-circle-up");
                $(".pricingTable-wrap-toggle").children(".pricingTable-arrow").addClass("fa-arrow-circle-down");
            }
        }
    });
});