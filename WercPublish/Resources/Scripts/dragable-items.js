var afterDraggedItemEvent = function () { };
function initialDragable(callBackFunction) {

    afterDraggedItemEvent = callBackFunction;
    $("#launchPad .card").dblclick(function (DragableItem) {
        SendItemToList(DragableItem);
    });

    $("#launchPad .card").draggable({
        appendTo: "body",
        scope: "first",
        cursor: "move",
        helper: 'clone',
        revert: "invalid"
    });

    $("#launchPad").droppable({
        tolerance: "intersect",
        accept: "*",
        activeClass: "ui-state-default",
        hoverClass: "ui-state-hover",

    });

    $(".stack-drop").droppable({
        tolerance: "intersect",
        scope: "first",
        accept: "*",
        activeClass: "ui-state-default",
        hoverClass: "stack-drop-hover",
        drop: function (DragableItem, ui) {

            SendItemToList(DragableItem, ui);
        }
    });
}

function SendItemToList(DragableItem, ui) {

    var dragedItem;

    if (DragableItem.type == "dblclick") {
        dragedItem = $(DragableItem.currentTarget).clone();
    }
    else if (DragableItem.type == "drop") {
        dragedItem = $(ui.draggable).clone();
    }

    //var pictureId = dragedItem.context.id;
    //var detailId = "cardDetail_" + dragedItem.context.id;

    var pictureId = dragedItem.attr("id");
    var detailId = "cardDetail_" + dragedItem.attr("id");

    //check if item already added
    var stackDropChildren = $(".stack-drop").children(":first");
    var stackDropChildrenlength = $(".stack-drop").children().length;
    var resume = true;

    for (var i = 0; i < stackDropChildrenlength; i++) {
        if (parseInt(stackDropChildren.attr("id")) == parseInt(pictureId)) {
            resume = false;
            break;
        }
        else {
            stackDropChildren = stackDropChildren.next();
        }
    }

    if (resume == true) {
        $(".stack-drop").append(dragedItem);

        dragedItem.removeClass("orginalCard");

        dragedItem.find(".margin-div").removeClass("itemList-div-content");

        dragedItem.find(".margin-div").addClass("dropped-select-item-hihghlight");

        dragedItem.find(".margin-div").click(function () { });

        dragedItem.find("#removeSection").removeClass("hidden");
        dragedItem.find(".operation-key-bottom").remove();


        if ($(".stack-drop").height() > $("#dropZone").height()) {
            if ($("#dropZone").hasClass("drop-zone-scroll") === false) {
                $("#dropZone").addClass("drop-zone-scroll");
            }
            $("#dropZone").animate({ scrollTop: $(".stack").prop("scrollHeight") }, 1000);
        }
       
        afterDraggedItemEvent(dragedItem);
    }
}

