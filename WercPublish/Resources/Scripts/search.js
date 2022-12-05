
$("#fa_search").click(function () {
    $("#aboutUsModal").modal();
});

$("#search").keypress(function (e) {
    var code = e.keyCode;
    if (code == 13) {
        $("#aboutUsModal").modal();
    }
    return code;
});
