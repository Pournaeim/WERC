$(function ($) {
    var displayed = false;
    var oldX = 0;
    var oldY = 0;
    var current = { x: -1, y: -1 };
    var canDisplayOnStop = true;

    $(".pricingTable").mousemove(function (event) {
        current.x = event.clientX;
        current.y = event.clientY;

        if (displayed === true && (Math.abs(oldX - current.x) > 5 || Math.abs(oldY - current.y) > 5)) {
            $(this).children(".tootip-popup").hide();
            displayed = false;
            canDisplayOnStop = false;
        }
    });

    // ELSEWHERE, your code that needs to know the mouse position without an event


    $(".pricingTable").on("mouseleave", function () {
        canDisplayOnStop = true;
    });
   
    $(".pricingTable").mousestop(function () {
        if (canDisplayOnStop === true) {
            $(this).children(".tootip-popup").css({ top: current.y - 95, left: current.x - 75, display: 'block' })
            displayed = true;
            oldX = current.x;
            oldY = current.y;
        }
    })


    $(window).scroll(function () {
        $(".tootip-popup").hide();
    });
});
