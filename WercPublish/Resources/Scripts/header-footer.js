function loadGoogleMap() {
    $("#googleMapFrame").attr("src", "https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d71137.97697726863!2d9.615765630371099!3d56.13874447913182!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x464b8b14dff7f229%3A0xb60d5043c737e15f!2sNygade+1E%2C+8600+Silkeborg%2C+Denmark!5e0!3m2!1sen!2sdk!4v1503264177661");
}
function headerLogoSizeAdapter() {
    //header margin resizer
    //var h = $('#marginerDiv').height();
    
    var h = $('#site-header-div').height();
    h += $('#footerId').height();
    h += 79;
    $('#mainContentId').css("min-height", $("body").height() - h);

    //$('#site-header-div').height(h);

    return;
    //footer brand resizer

    //$('#site-header-div').removeClass('header-static');
    //$('#marginerDiv').removeClass('hide-marginerDiv');

    //if (w < 768) {
    //    $('#footerFirstColId').width(w / 3);
    //    $('#site-header-div').addClass('header-static');
    //    $('#marginerDiv').addClass('hide-marginerDiv');

    //}
    //else if (w < 1200) {
    //    $('#footerFirstColId').width(w / 5);
    //}
    //else if (w < 2300) {
    //    $('#footerFirstColId').width(w / 7);
    //}
    //else if (w < 2600) {
    //    $('#footerFirstColId').width(w / 8);
    //}
    //else if (w < 2800) {
    //    $('#footerFirstColId').width(w / 9);
    //}
    //else if (w < 3800) {
    //    $('#footerFirstColId').width(w / 10);
    //}
    //else if (w < 4096) {
    //    $('#footerFirstColId').width(w / 12);
    //}
    //else {
    //    $('#footerFirstColId').width(w / 13);
    //}
}


function headerMarginResizer() {
    //header margin resizer
    var w = $('#site-header-div').width();
    var h = $('#site-header-div').height();

    $('#marginerDiv').width(w);
    $('#marginerDiv').height(h);

    //footer brand resizer

    $('#site-header-div').removeClass('header-static');
    $('#marginerDiv').removeClass('hide-marginerDiv');

    if (w < 768) {
        $('#footerFirstColId').width(w / 3);
        $('#site-header-div').addClass('header-static');
        $('#marginerDiv').addClass('hide-marginerDiv');

    }
    else if (w < 1200) {
        $('#footerFirstColId').width(w / 5);
    }
    else if (w < 2300) {
        $('#footerFirstColId').width(w / 7);
    }
    else if (w < 2600) {
        $('#footerFirstColId').width(w / 8);
    }
    else if (w < 2800) {
        $('#footerFirstColId').width(w / 9);
    }
    else if (w < 3800) {
        $('#footerFirstColId').width(w / 10);
    }
    else if (w < 4096) {
        $('#footerFirstColId').width(w / 12);
    }
    else {
        $('#footerFirstColId').width(w / 13);
    }
}
