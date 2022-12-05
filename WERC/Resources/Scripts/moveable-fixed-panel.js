$(document).ready(function () {

    //$("#launchPadContent").append($("#dropZone"));

    //$("#dropZone").css("left", $("#launchPad").width() + 40);


    // Add drag and resize option to panel
    $("#dropZone").draggable({
        handle: ".panel-heading",
        scroll: false,
        containment: "#fixedContainer",
        stop: function (evt, el) {
            
        }
    }).resizable({
        handles: "e, w, s, se",
        stop: function (evt, el) {
             
        }
        });
    $("#dropZone").draggable('disable');

    $("#toggleFixed").on("click", function () {
        var panel = $("#dropZone");

        panel.toggleClass("set-fixed");
        panel.toggleClass("set-absolute");
        $(this).toggleClass("set-pin");
        $(this).toggleClass("set-unpin");

        if (panel.hasClass("set-absolute")) {
            $("#launchPad").append($("#dropZone"));
            $("#dropZone").css("left", $("#launchPad").width() + 20);
            $("#dropZone").css("top", 0);
            $("#dropZone").draggable('disable');
        }
        else if (panel.hasClass("set-fixed")) {
            $("#launchPad").append($("#dropZone"));
            $("#dropZone").css("bottom", 0);
            $("#dropZone").draggable('enable');

        }

    });
    // Expand and collaps the toolbar
    $("#toggle-dropZone").on("click", function () {
        var panel = $("#dropZone");

        if ($(panel).data("org-height") == undefined) {
            $(panel).data("org-height", $(panel).css("height"));
            $(panel).css("height", "41px");
        } else {
            $(panel).css("height", $(panel).data("org-height"));
            $(panel).removeData("org-height");
        }

        $(this).toggleClass('fa-chevron-down').toggleClass('fa-chevron-right');
    });

    // Make toolbar groups sortable
    $("#sortable").sortable({
        stop: function (event, ui) {
            var ids = [];
            $.each($(".draggable-group"), function (idx, grp) {
                ids.push($(grp).attr("id"));
            });

            // Save order of groups in cookie
            //$.cookie("group_order", ids.join());
        }
    });
    $("#sortable").disableSelection();


    // Make Tools panel group minimizable
    $.each($(".draggable-group"), function (idx, grp) {
        var tb = $(grp).find(".toggle-button-group");

        $(tb).on("click", function () {
            $(grp).toggleClass("minimized");
            $(this).toggleClass("fa-caret-down").toggleClass("fa-caret-up");

            // Save draggable groups to cookie (frue = Minimized, false = Not Minimized)
            var ids = [];
            $.each($(".draggable-group"), function (iidx, igrp) {
                var itb = $(igrp).find(".toggle-button-group");
                var min = $(igrp).hasClass("minimized");

                ids.push($(igrp).attr("id") + "=" + min);
            });

            $.cookie("group_order", ids.join());
        });
    });



    // Close thr panel
    $(".close-panel").on("click", function () {
        $(this).parent().parent().hide();
    });


    // Add Tooltips
    $('button').tooltip();
    $('.toggle-button-group').tooltip();

});
