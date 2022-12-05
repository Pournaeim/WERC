
function navbarDirect(id) {

    if (screen.width < 768) {
        $('#' + id).click();
    }
}

//$('ul.nav li.dropdown').hover(function () {
//    $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeIn(500);
//}, function () {
//    $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeOut(500);
//});
