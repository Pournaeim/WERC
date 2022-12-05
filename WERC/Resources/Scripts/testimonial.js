$('#carousel-example-generic_1').hover(function () {
    $(this).carousel('pause');
    $('#carousel-example-generic_2').carousel('pause');
    $('#carousel-example-generic_3').carousel('pause');
}, function () {
    $(this).carousel('cycle');
    $('#carousel-example-generic_2').carousel('cycle');
    $('#carousel-example-generic_3').carousel('cycle');
});

$('#carousel-example-generic_2').hover(function () {
    $(this).carousel('pause');
    $('#carousel-example-generic_1').carousel('pause');
    $('#carousel-example-generic_3').carousel('pause');
}, function () {
    $(this).carousel('cycle');
    $('#carousel-example-generic_1').carousel('cycle');
    $('#carousel-example-generic_3').carousel('cycle');
});

$('#carousel-example-generic_3').hover(function () {
    $(this).carousel('pause');
    $('#carousel-example-generic_1').carousel('pause');
    $('#carousel-example-generic_2').carousel('pause');
}, function () {
    $(this).carousel('cycle');
    $('#carousel-example-generic_1').carousel('cycle');
    $('#carousel-example-generic_2').carousel('cycle');
});

$(".prevCarousel").click(function () {

    $("#carousel-example-generic_1").carousel("prev");
    $("#carousel-example-generic_2").carousel("prev");
    $("#carousel-example-generic_3").carousel("prev");
});
$(".nextCarousel").click(function () {

    $("#carousel-example-generic_1").carousel("next");
    $("#carousel-example-generic_2").carousel("next");
    $("#carousel-example-generic_3").carousel("next");
});
