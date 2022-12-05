
$(document).ready(function () {
    $('[data-toggle="offcanvas"]').click(function () {
        $('.row-offcanvas').toggleClass('active');

        if ($('.row-offcanvas').hasClass('active')) {
            $('.offCanvasButton').hide();
        }
        else {
            $('.offCanvasButton').show();
        }
    });
});