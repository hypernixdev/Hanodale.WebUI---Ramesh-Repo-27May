$(function () {
    var id = $("#tab-info").val();
    var readOnly = $("#tab-info").attr("data-readOnly");
    if (readOnly == null || readOnly == "" || readOnly == "undefined")
        readOnly = false;
    //Open Create Box
    $('[data-toggle="tab"]').bind("click", function () {
        var actionUrl = $(this).attr("data-url") + "/";
        var destinationElem = "#d-" + $(this).attr("href").replace("#", "");
        if (actionUrl) {
            navigate.resetActivityTables();
            var param = { id: id, readOnly: readOnly }
            navigate.navigateElementByParameter(actionUrl, param, $(destinationElem));
        }
    });
});





