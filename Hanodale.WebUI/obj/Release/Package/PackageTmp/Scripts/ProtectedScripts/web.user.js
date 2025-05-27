_initActionExtended = function (partName, readOnly) {

    //Reset Password
    $('.reset-password').bind("click", function () {
        var id = $(this).attr("data-url")
        if (id) {
            var message = $("#alert-reset").val();
            var actionUrl = partName + "/ResetPassword/" + id;
            var param = {};
            navigate.saveCallBack(actionUrl, param, message, false, null);
        }
    });

    $('.edituser').bind("click", function () {
        var id = $(this).attr("data-url")
        var destInner = $(this).closest('div.tab-pane-content').attr("id")
        var actionUrl = partName + "/Edit/";
        var elem = $('.table-congif[value="' + partName + '"]');
        var destOuter = $(elem).attr('data-edit-dest');
        var businessId = $(elem).attr("data-filterId")
        var param = { id: id, businessId: businessId, readOnly: readOnly }
        if (actionUrl) {
            if (destInner) {
                navigate.navigateElementByParameter(actionUrl, param, $("#" + destInner));
            } else if (destOuter) {
                navigate.navigateElementByParameter(actionUrl, param, $(destOuter));
            }
            else {
                navigate.viewByParameter(actionUrl, param);
            }
        }
    });

    $('.viewuser').bind("click", function () {
        var id = $(this).attr("data-url")
        var destInner = $(this).closest('div.tab-pane-content').attr("id")
        var actionUrl = partName + "/View/";
        var elem = $('.table-congif[value="' + partName + '"]');
        var destOuter = $(elem).attr('data-edit-dest');
        var businessId = $(elem).attr("data-filterId")
        var param = { id: id, businessId: businessId, readOnly: readOnly }
        if (actionUrl) {
            if (destInner) {
                navigate.navigateElementByParameter(actionUrl, param, $("#" + destInner));
            } else if (destOuter) {
                navigate.navigateElementByParameter(actionUrl, param, $(destOuter));
            }
            else {
                navigate.viewByParameter(actionUrl, param);
            }
        }
    });

    $('.editbusinessuser').bind("click", function () {
        var id = $(this).attr("data-url")
        var destInner = $(this).closest('div.tab-pane-content').attr("id")
        var actionUrl = partName + "/Edit/";
        var elem = $('.table-congif[value="' + partName + '"]');
        var destOuter = $(elem).attr('data-edit-dest');
        var businessId = $(elem).attr("data-filterId")
        var param = { id: id, readOnly: readOnly }
        if (actionUrl) {
            if (destInner) {
                navigate.navigateElementByParameter(actionUrl, param, $("#" + destInner));
            } else if (destOuter) {
                navigate.navigateElementByParameter(actionUrl, param, $(destOuter));
            }
            else {
                navigate.viewByParameter(actionUrl, param);
            }
        }
    });

    $('.viewbusinessuser').bind("click", function () {
        var id = $(this).attr("data-url")
        var destInner = $(this).closest('div.tab-pane-content').attr("id")
        var actionUrl = partName + "/View/";
        var elem = $('.table-congif[value="' + partName + '"]');
        var destOuter = $(elem).attr('data-edit-dest');
        var businessId = $(elem).attr("data-filterId")
        var param = { id: id, readOnly: readOnly }
        if (actionUrl) {
            if (destInner) {
                navigate.navigateElementByParameter(actionUrl, param, $("#" + destInner));
            } else if (destOuter) {
                navigate.navigateElementByParameter(actionUrl, param, $(destOuter));
            }
            else {
                navigate.viewByParameter(actionUrl, param);
            }
        }
    });

}






