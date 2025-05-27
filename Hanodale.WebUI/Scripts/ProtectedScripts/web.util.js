
//common mehtod to all module
//var tablenameList = [];
//function CheckTablenameList(x) {
//    for (var i = 0; i < tablenameList.length; i++) {
//        if (tablenameList[i] === x) {
//            return true;
//        }
//    }
//    return false;
//}
//function InseartTablenameList(x) {
//    if (!CheckTablenameList(x)) {
//        tablenameList.push(x);
//    }
//    CleanDatatableList();
//}
//function CleanDatatableList() {
//    for (var i = 0; i < tablenameList.length; i++) {
//        if ($('#dt_' + tablenameList[i]).length <= 0) {
//            $('#dt_' + tablenameList[i]).datatable().destroy();
//        }
//    }
//    return false;
//}

function scrollToElement(targetElem) {
    if ($(targetElem).length > 0) {
        var target;
        target = targetElem;
        if ($(target).length > 0) {
            var navOffset;
            navOffset = 0;
            var datatable_search_div_top = $(target).offset().top - (navOffset + 10);

            $('html, body').animate({
                scrollTop: datatable_search_div_top
            }, 700, function () {
                //// Adds hash to end of URL
                //return window.history.pushState(null, null, target);
            });
        }
    }
}


localStorage.clear();
var setLocalStorage = function (tableName, fieldList) {
    if (typeof (Storage) !== "undefined") {
        // Code for localStorage/sessionStorage.
        //console.log(fieldList)
        localStorage.setItem("tableName_" + tableName, tableName);
        if (fieldList) {
            for (var i = 0; i < fieldList.length; i++) {
                //console.log("get ---   "+tableName + "_" + fieldList[i].id)
                if (fieldList[i].isDeferredLoad) {
                    localStorage.setItem(tableName + "_" + fieldList[i].id + "_DeferredLoad", fieldList[i].value);
                }
                else
                    localStorage.setItem(tableName + "_" + fieldList[i].id, fieldList[i].value);
            }
        }
        else {

        }

        //localStorage.setItem(tableName + "_" + "search", $('input[aria-controls="' + tableName + '"]:first').val());
        //localStorage.setItem(tableName + "_" + "paging", $('#' + tableName + '_paginate ul li.active:first a:first').text());

        //console.log(localStorage.getItem("tableName_"+tableName));
        //console.log(localStorage.getItem(tableName + "_" + "search"));
        //console.log(localStorage.getItem(tableName + "_" + "paging"));
    } else {
        // Sorry! No Web Storage support..
    }
}

var getLocalStorage = function (tableName, isDeferredLoad) {
    //tableName = tableName.replace('#', '')
    if (typeof (Storage) !== "undefined") {
        if (localStorage.getItem("tableName_" + tableName)) {
            if ($('.searchPanelWithSearchBtn').length > 0) {
                var deferredLoad = ""
                var deferredLoadClass = ""
                if (isDeferredLoad == true) {
                    deferredLoad = "_DeferredLoad"
                    deferredLoadClass = ".deferredLoad"
                }
                $('.searchPanelWithSearchBtn :input' + deferredLoadClass).each(function (index, value) {
                    var elem_Id = $(this).attr('id');

                    if (elem_Id) {
                        var val = localStorage.getItem(tableName + "_" + elem_Id + deferredLoad);
                        if ($(this).prop("tagName") == "SELECT") {

                            //console.log("set   --  " + tableName + "_" + elem_Id)
                            //console.log(elem_Id + "   --  " + val)
                            $(this).children('option[value="' + val + '"]').attr('selected', 'selected').trigger('change');
                        } if ($(this).hasClass("multiDates")) {
                            if (val != null && val != '') {
                                var array = val.split(",");
                                for (var i = 0; i < array.length; i++) {
                                    if (array[i] != null && array[i] != '') {
                                        array[i] = array[i].trim();
                                    }
                                }
                                $(this).val(val);
                                $('#mdp-demo').multiDatesPicker({
                                    altField: '#' + elem_Id,
                                    addDates: array,
                                    //numberOfMonths: [1, 3]
                                });
                            }
                        }
                        else {
                            $(this).val(val)
                        }
                    }
                });
            }

            //$('input[aria-controls="' + tableName + '"]:first').val(localStorage.getItem(tableName + "_" + "search"))
            //$('#' + tableName + '_paginate ul li.active:first a:first').text();
        } else {
            //localStorage.clickcount = 1;
        }
    } else {
        // Sorry! No Web Storage support..
    }
}

var dateRange = (function () {
    init = function () {

        var from = _formatedDate(_getTodayDate());
        var to = _formatedDate(_getTodayDate());

        $(".date-range>li a").bind("click", function () {
            var id = $(this).attr("data-id");
            from = _formatedDate($("#date-from").val());
            to = _formatedDate($("#date-to").val());

            $(".show-daterange").text(from + '  - ' + to);
        });

        $(".show-daterange").text(from + '  - ' + to);
    },
        showDateRange = function () {
            $(".show-custom-date").css({ 'display': '' });

            $('input[name=date-range-picker]').daterangepicker().prev().on(ace.click_event, function () {
                $(this).next().focus();
            });
        },
        hideDateRange = function () {
            //console.log($(".show-custom-date").length);
            if ($(".show-custom-date") != null) {
                $(".show-custom-date").css({ 'display': 'none' });
            }
        },
        show = function (flag) {

            var dateFrom, dateTo;
            var currentDate = null;
            var fullDate = new Date();

            var month = fullDate.getMonth();
            var date = fullDate.getDate();
            var year = fullDate.getFullYear();

            var dtFrom = $("#date-from");
            var dtTo = $("#date-to");

            if (flag == 0) {
                //all - this year data
                dateFrom = _formatDate(new Date(year, 0, 1));
                date = new Date(year, 11, 0);
                date = date.getDate() + 1;
                dateTo = _formatDate(new Date(year, 11, date));
            }
            else if (flag == 1 || flag == 9) {
                //Today
                currentDate = _formatDate(new Date(year, month, date));
                dateFrom = currentDate;
                dateTo = currentDate;

            }
            else if (flag == 2) {
                //Yesterday
                currentDate = _formatDate(new Date(year, month, date - 1));
                dateFrom = currentDate;
                dateTo = currentDate;
            }
            else if (flag == 3) {
                //Last 7 Days
                dateFrom = _formatDate(new Date(year, month, date - 6));
                dateTo = _formatDate(new Date(year, month, date));

            }
            else if (flag == 4) {
                //Last 30 Days
                dateFrom = _formatDate(new Date(year, month, date - 29));
                dateTo = _formatDate(new Date(year, month, date));
            }
            else if (flag == 5) {
                //This month
                dateFrom = _formatDate(new Date(year, month, 1));

                date = new Date(year, month, 0);
                date = date.getDate() - 1;

                dateTo = _formatDate(new Date(year, month, date));
            }
            else if (flag == 6) {
                //Last Month
                dateFrom = _formatDate(new Date(year, month - 1, 1));

                date = new Date(year, month - 1, 0);
                date = date.getDate();
                dateTo = _formatDate(new Date(year, month - 1, date));
            }
            else if (flag == 7) {
                //This Year
                dateFrom = _formatDate(new Date(year, 0, 1));
                date = new Date(year, 11, 0);
                date = date.getDate() + 1;
                dateTo = _formatDate(new Date(year, 11, date));
            }
            else if (flag == 8) {
                //Last Year
                dateFrom = _formatDate(new Date(year - 1, 0, 1));
                date = new Date(year - 1, 11, 0);
                date = date.getDate() + 1;
                dateTo = _formatDate(new Date(year - 1, 11, date));
            }


            dtFrom.val(dateFrom);
            dtTo.val(dateTo);
            //_hideDateRange();
        },
        _getTodayDate = function () {
            var dateFrom, dateTo;
            var currentDate = null;
            var fullDate = new Date();

            var month = fullDate.getMonth();
            var date = fullDate.getDate();
            var year = fullDate.getFullYear();

            return _formatDate(new Date(year, month, date));
        },
        _formatedDate = function (date) {

            var value = date.split('/');

            var monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"];

            return monthNames[parseInt(value[1]) - 1] + ' ' + parseInt(value[0]) + ', ' + value[2];
        },
        _formatDate = function (date) {
            var mm = _twoDigit(date.getMonth() + 1);

            var dd = _twoDigit(date.getDate());

            var yyyy = date.getFullYear();

            return dd + "/" + mm + "/" + yyyy;
        },
        _twoDigit = function (digit) {
            var value = (digit < 10) ? ("0" + digit) : digit;

            return value;
        }
    return {
        show: show,
        init: init,
        showDateRange: showDateRange
    };
})();

var userMessage = {
    show: function (status, msg) {
        var msgContainer = $("#message");
        userMessage.showMsg(status, msg, msgContainer);
    },
    showMsg: function (status, msg, container) {
        var msgContent = "";
        $('#main').append(
            '<div class="divMessageBox animated fadeIn fast" id="MsgBoxBack" style="z-index:2000000; background:rgba(0, 0, 0, 0.35)">\
                <div class="MessageBoxContainer animated fadeIn fast" id="Msg1" style="border-left: 10px solid; position:relative;">\
                    <i class="fas fa fa-times close-icon" id="closeMsgBox" style="position: absolute; top: 10px; right: 40px; cursor: pointer; font-size: 28px;"></i>\
                    <div class="MessageBoxMiddle">\
                        <span class="MsgTitle">' + status + '!</span>\
                        <p class="pText">' + msg + '</p>\
                    </div>\
                </div>\
            </div>'
        );
        //$('#main').append('<div class="divMessageBox animated fadeIn fast" id="MsgBoxBack" style="z-index:2000000; background:rgba(0, 0, 0, 0.35)"><div class="MessageBoxContainer animated fadeIn fast" id="Msg1" style="border-left: 10px solid;"><div class="MessageBoxMiddle"><span class="MsgTitle">' + status + '!</span><p class="pText">' + msg + '</p><div class="MessageBoxButtonSection"></div></div></div></div>')
        //if (status == "Success") {
        //    msgContent = "<button type='button' class='close' data-dismiss='alert'></button>" + msg + "<br />";
        //    container.removeClass("alert-danger").removeClass("alert-warning").addClass("alert-success");
        //}
        //else if (status == "Error") {
        //    msgContent = "<button type='button' class='close' data-dismiss='alert'></button>" + msg + "<br />";
        //    container.removeClass("alert-warning").removeClass("alert-success").addClass("alert-danger");
        //}
        //else if (status == "Warning") {
        //    msgContent = "<button type='button' class='close' data-dismiss='alert'></button>" + msg + "<br />";
        //    container.removeClass("alert-danger").removeClass("alert-success").addClass("alert-warning");
        //}
        $('#Msg1').removeClass("alert-danger").removeClass("alert-warning").removeClass("alert-success").removeClass("alert-info");

        if (status == "Success" || status == "success") {
            $('#Msg1').addClass("alert-success");
        }
        else if (status == "Error" || status == "error") {
            $('#Msg1').addClass("alert-danger");
        }
        else if (status == "Warning" || status == "warning") {
            $('#Msg1').addClass("alert-warning");
        }
        else if (status == "Info" || status == "info") {
            $('#Msg1').addClass("alert-info");
        }
        // Added by Anand 
        else if (status == "Access Denied") {
            $('#Msg1').addClass("alert-warning");
        }

        //container.css({ 'display': 'none' });
        //container.css({ 'display': 'block' });
        //container.text('');
        //container.append(msgContent);

        // Close the message box when the close icon is clicked
        $('#closeMsgBox').on('click', function () {
            $('#MsgBoxBack').remove();
        });

        setTimeout(function () {
            //container.css({ 'display': 'none' });
            $('#MsgBoxBack').remove();
        }, 1000); // 4000
    }
};

//This method is to store the datatase settings while user filter or sort or navigate to different page
var tableSetting = {
    setItem: function (oData) {
        var key = "key";
        if (typeof (Storage) !== "undefined") {
            // Yes! localStorage and sessionStorage support!

            localStorage.setItem(key, JSON.stringify(oData));
        }
        else {
            // No web storage support..
            alert('No web storage support!');
        }
    },
    getItem: function () {
        var key = "key";
        if (typeof (Storage) !== "undefined") {
            return JSON.parse(localStorage.getItem(key));
        }
        else {
            // No web storage support..
            alert('No web storage support!');
        }
    },
    clearStorage: function () {

        if (typeof (Storage) !== "undefined") {
            // Yes! localStorage and sessionStorage support!
            localStorage.clear();
        }
        else {
            // No web storage support..
            alert('No web storage support!');
        }
    }
};

//hide unwanted div if the control is hidden
var tags = {
    remove: function () {
        //to hide hidden control
        $(".form-group").each(function (index) {
            var hiddenControl = $(this).find(":hidden").attr("id");
            if (hiddenControl) {
                $(this).css({ 'display': 'none' });
                $(this).next('div').css({ 'display': 'none' });
            }
        });
    }
};


var reloadCurrentPage = false;
var reloadReadOnly = false;
var dataReloadUrl = "";
var dataReloadDestContainer = "";
var reloadSectionParam = {};
var reloadSectionStatus = "";

function ClearReloadParameters() {
    reloadCurrentPage = false;
    reloadReadOnly = false;
    dataReloadUrl = "";
    dataReloadDestContainer = "";
    reloadSectionParam = {};
    var reloadSectionStatus = "";
}

//$('button[data-reload="true"]').live('click', function () {

//    ClearReloadParameters();
//    reloadCurrentPage = true;
//})

$(document).on('click', 'button[data-reload="true"]', function () {

    ClearReloadParameters();
    reloadCurrentPage = true;
})

function ReloadPageContainer() {
    //console.log("ReloadPageContainer"+"   ---   "+new Date());
    var formConfigElem = $('input[id="form-congif"]');

    if (formConfigElem != null && formConfigElem != undefined) {

        var controllerName = formConfigElem.attr('value');
        //console.log(controllerName)
        dataReloadUrl = controllerName + "/Edit/";

        dataReloadDestContainer = "#content.page-content";

        var hasDataReloadUrl = formConfigElem.attr('data-reload-url');
        if (typeof hasDataReloadUrl !== typeof undefined && hasDataReloadUrl !== false) {
            if (formConfigElem.attr('data-reload-url') != "") {
                dataReloadUrl = formConfigElem.attr('data-reload-url');
            }
        }

        var hasDataReloadDestContainer = formConfigElem.attr('data-reload-dest-container');
        //console.log(hasDataReloadDestContainer)
        if (typeof hasDataReloadDestContainer !== typeof undefined && hasDataReloadDestContainer !== false) {
            if (formConfigElem.attr('data-reload-dest-container') != "") {
                dataReloadDestContainer = formConfigElem.attr('data-reload-dest-container');
            }
        }
        //console.log(dataReloadDestContainer)
        var hasReadOnly = formConfigElem.attr('data-readOnly');
        if (typeof hasReadOnly !== typeof undefined && hasReadOnly !== false) {
            if (formConfigElem.attr('data-readOnly') != "") {
                reloadReadOnly = formConfigElem.attr('data-readOnly');
            }
        }

        if ($('#reloadParamSection').length > 0) {

            $('#reloadParamSection .reloadParamData').each(function () {
                reloadSectionParam[$(this).attr('id')] = $(this).val();
            });

            //var json_pages = JSON.stringify(publish);
        } else {

            //console.log($('form #id').val());

            if ($('form input#id').length > 0) {
                reloadSectionParam['id'] = $('form #id').val();
                //reloadSectionParam.push({ id: $('form #id').val() });
            }
            if ($('form input#readOnly').length > 0) {
                reloadSectionParam['readOnly'] = false;//$('form #readOnly').val();
                //reloadSectionParam.push({ readOnly: $('form #readOnly').val() });
            } else {
                reloadSectionParam['readOnly'] = false;// reloadReadOnly;
                //reloadSectionParam.push({ readOnly: reloadReadOnly });
            }
        }
    }

    if (reloadSectionParam != {}) {
        navigate.viewContainerByParameter(dataReloadUrl, reloadSectionParam, $(dataReloadDestContainer))
    } else {
        navigate.viewContainer(dataReloadUrl, $(dataReloadDestContainer))
    }

    ClearReloadParameters();
}

var tblCollection = [];
var tableCounter = 0;
var calendarSett = null;
var navigate = {
    calendarSetting: function () {
        return calendarSett;
    },
    formatDatetimeForSubmit: function (formData) {
        var date = []
        $('.datepicker').each(function (index, value) {
            var name = $(this).attr('name')//.replace(".Date","");
            var dateElem = $("input[name='" + name + "'][class~='datepicker']");
            if (dateElem.length > 0) {
                isTime = false;
                var timeElem = $("input[name='" + name + ".Time'][class~='timepicker']");
                if (timeElem.length > 0) {

                    isTime = true;
                }
                var value = dateElem.val();
                if (isTime) {
                    var is24Hours = timeElem.hasClass('timepicker24Format');
                    if (is24Hours) {
                        var a = timeElem.val().trim();
                        if (a != null && a != "") {
                            var output = a;
                            value = dateElem.val() + " " + output;
                        }

                    } else {
                        var a = timeElem.val().trim();
                        var output = a.replace(" ", ":00 ");
                        value = dateElem.val() + " " + output;
                    }
                }
            }
            date.push({ "name": name, "value": value });
        })

        date.forEach(function (entry) {
            formData.delete(entry.name);
            formData.set(entry.name, entry.value);
        });

        return formData;
    },
    view: function (actionUrl) {
        //load view without postback
        var container = $(".page-content");
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: {},
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.status == "Denied") {
                    Error.accessDenied(data.message);
                } else if (data.isRedirect == "true" || data.isRedirect == true) {
                    navigate.viewByParameter(data.actionUrl, data.param);

                } else {
                    container.html("");
                    container.html(data.viewMarkup);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                Error.redirectToErrorPage(xhr, ajaxOptions, thrownError);
            }
        });
    },
    viewByParameter: function (actionUrl, param) {
        //load view without postback
        var data = {};
        if (param != null)
            data = JSON.stringify(param);

        var container = $(".page-content");
        if ($(".page-content").length <= 0)
            container = $('#content');

        $.ajax({
            type: "POST",
            url: actionUrl,
            data: data,
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.status == "Denied") {
                    Error.accessDenied(data.message);
                } else if (data.isRedirect == "true" || data.isRedirect == true) {

                    navigate.viewByParameter(data.actionUrl, data.param);
                } else {
                    container.html("");
                    container.html(data.viewMarkup);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                Error.redirectToErrorPage(xhr, ajaxOptions, thrownError);
            }
        });
    },
    viewContainer: function (actionUrl, container) {
        //load view without postback
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: {},
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.status == "Denied") {
                    Error.accessDenied(data.message);
                } else {
                    //container.html("");
                    container.html(data.viewMarkup);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                Error.redirectToErrorPage(xhr, ajaxOptions, thrownError);
            }
        });
    },
    viewContainerByParameter: function (actionUrl, param, container) {
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: JSON.stringify(param),
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.status != undefined && data.status != '' && data.status != null) {
                    Error.accessDenied(data.status, data.message);
                } else {
                    //container.html("");
                    container.html(data.viewMarkup);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                Error.redirectToErrorPage(xhr, ajaxOptions, thrownError);
            }
        });
    },
    viewCallBack: function (actionUrl, callback) {
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: {},
            cache: false,
            contentType: "text/html; charset=utf-8",
            success: function (data) {
                if (callback != null)
                    callback(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                Error.redirectToErrorPage(xhr, ajaxOptions, thrownError);
            }
        });
    },


    saveCallBack: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {
            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        url: actionUrl,
                        datatype: 'json',
                        type: 'POST',
                        //data: JSON.stringify({ userModel: userObj }),
                        data: AddAntiForgeryToken(param),
                        //contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            if (data.id != "" && data.id != null && data.id != 'undefined') {
                                if ($("#id").length > 0)
                                    $("#id").val(data.id)
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + data.id + '" />')

                                }
                                if ($('[data-value="save"]').length > 0) {
                                    //$("button[type='reset']").attr('data-type', 'new-reset');
                                    //$("button[data-type='new-reset']").attr('type', 'button');
                                    //$("button[data-type='new-reset']").prop("type", "button");
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');
                            }

                            userMessage.show(data.status, data.message);
                            if (data.status == "Success") {
                                if (data.jobStatus_Id == 93) {
                                    $('#complete-workorder').show();
                                    $('#assign-workorder').hide();

                                }
                            } else {
                                $("#id").val($("#id").attr('data-oldValue'))
                            }

                            if (callback != null)
                                callback(data);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error

                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();

        }
    },

    // CREATED BY SURYA FOR AFTER SAVE REDIRECT TO ANOTHER PAGE with alert message
    saveCallBackRedirect: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;
        var container = $(".page-content");
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {
            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        url: actionUrl,
                        datatype: 'json',
                        type: 'POST',
                        //data: JSON.stringify({ userModel: userObj }),
                        data: AddAntiForgeryToken(param),
                        //contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            //userMessage.show(data.status, data.message);
                            if (data.status == "Warning") {
                                //callback(data);
                                userMessage.show(data.status, data.message);
                            } else if (data.status == "Error") {
                                userMessage.show(data.status, data.message);
                            }
                            else if (data.status == "Success") {
                                container.html("");
                                container.html(data.viewMarkup);
                                userMessage.show(data.status, data.message);
                            }
                            else if (data.status == 'undefined') {
                                container.html("");
                                container.html(data.viewMarkup);
                                userMessage.show(data.status, data.message);
                            }

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();

        }
    },

    // CREATED BY SURYA FOR AFTER SAVE REDIRECT TO ANOTHER PAGE Without alert message
    saveCallBackRedirectToEdit: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;
        var container = $(".page-content");
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {
            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        url: actionUrl,
                        datatype: 'json',
                        type: 'POST',
                        //data: JSON.stringify({ userModel: userObj }),
                        data: AddAntiForgeryToken(param),
                        //contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            // userMessage.show(data.status, data.message);
                            container.html("");
                            container.html(data.viewMarkup);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();

        }
    },

    // CREATED BY SURYA FOR ASSIGNED WORKORDER SAVE
    saveCallBack1: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;

        //var container = $(".page-content");
        //if ($(".page-content").length <= 0)
        //    container = $('#content');
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {
            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        url: actionUrl,
                        datatype: 'json',
                        type: 'POST',
                        //data: JSON.stringify({ userModel: userObj }),
                        data: AddAntiForgeryToken(param),
                        //contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            if (data.id != "" && data.id != null && data.id != 'undefined') {
                                if ($("#id").length > 0)
                                    $("#id").val(data.id)
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + data.id + '" />')
                                }

                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');
                            }
                            userMessage.show(data.status, data.message);
                            if (data.status == "Success") {

                                $('#assign-workorder').hide();
                                $('#complete-workorder').show();
                                //$("#back-ticket")[0].click()
                            } else {
                                $("#id").val($("#id").attr('data-oldValue'))
                            }

                            if (callback != null)
                                callback(data);

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();
        }

        //bootbox.confirm(message, function (result) {
        //    if (result) {
        //        $.ajax({
        //            url: actionUrl,
        //            datatype: 'json',
        //            type: 'POST',
        //            //data: JSON.stringify(param),
        //            data: AddAntiForgeryToken(param),
        //            //contentType: "application/json; charset=utf-8",
        //            success: function (data) {
        //                callback(data);
        //            },
        //            error: function (xhr, ajaxOptions, thrownError) {
        //                //log error
        //                _showErrorMessage();
        //            }

        //        });
        //    }
        //});
    },

    // CREATED BY SURYA FOR RFQSUPPLIER SUBMISSION
    saveCallBackJsonStringifyrefreshgrid: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {

            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        data: JSON.stringify(param),
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            userMessage.show(data.status, data.message);
                            reloadSectionStatus = data.status;
                            if (data.status == "Success") {
                                if (callback != null)
                                    callback(data);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();
        }
    },

    saveCallBackJsonStringify: function (actionUrl, param, message, checkValidate, callback) {

        //validate = validate || false;
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {
            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        data: JSON.stringify(param),
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            if (data.id != "" && data.id != null && data.id != 'undefined') {
                                if ($("#id").length > 0) {
                                    $("#id").val(data.id)
                                }
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + data.id + '" />')
                                }
                                if ($('[data-value="save"]').length > 0) {
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');

                            }

                            userMessage.show(data.status, data.message);

                            if (data.status != "Success") {
                                $("#id").val($("#id").attr('data-oldValue'))
                            }
                            $("#back-stockrequestauth").trigger("click");
                            if (callback != null)
                                callback(data);

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();
        }
    },

    //Created for warehouse
    saveCallBackJsonString: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {

            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        data: JSON.stringify(param),
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            if (data.id != "" && data.id != null && data.id != 'undefined') {
                                if ($("#id").length > 0) {
                                    $("#id").val(data.id)
                                }
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + data.id + '" />')
                                }
                                if ($('[data-value="save"]').length > 0) {
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');

                            }
                            if (data.status != "Success") {
                                //alert($("#id").val($("#id").attr('data-oldValue')))

                            }
                            userMessage.show(data.status, data.message);
                            if (callback != null)
                                callback(data);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();
        }
    },

    saveCallBackJsonStringRedirect: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;

        var container = $(".page-content");
        if ($(".page-content").length <= 0)
            container = $('#content');

        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {

            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        data: JSON.stringify(param),
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            if (data.id != "" && data.id != null && data.id != 'undefined') {
                                if ($("#id").length > 0) {
                                    $("#id").val(data.id)
                                }
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + data.id + '" />')
                                }
                                if ($('[data-value="save"]').length > 0) {
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');

                            }
                            if (data.status == "Success") {
                                container.html("");
                                container.html(data.viewMarkup);
                                userMessage.show(data.status, data.message);
                            }
                            else if (data.status == "Warning") {
                                $("#id").val($("#id").attr('data-oldValue'))
                                userMessage.show(data.status, data.message);
                            }
                            else {
                                //container.html("");
                                //container.html(data.viewMarkup);
                                $("#id").val($("#id").attr('data-oldValue'))
                                userMessage.show(data.status, data.message);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });
                }
            });
            //e.preventDefault();
        }
    },

    saveCallBackForLoginPageJsonStringRedirect: function (actionUrl, param, message, checkValidate, callback) {
        //validate = validate || false;

        var container = $(".page-content");
        if ($(".page-content").length <= 0)
            container = $('#content');

        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {

            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        data: JSON.stringify(param),
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            reloadSectionStatus = data.status;
                            if (data.id != "" && data.id != null && data.id != 'undefined') {
                                if ($("#id").length > 0) {
                                    $("#id").val(data.id)
                                }
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + data.id + '" />')
                                }
                                if ($('[data-value="save"]').length > 0) {
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');

                            }
                            if (data.status == "Success") {
                                if (callback != null) {
                                    callback(data);
                                } else {
                                    container.html("");
                                    container.html(data.viewMarkup);
                                    userMessage.show(data.status, data.message);
                                }
                            }
                            else if (data.status == "Warning") {
                                $("#id").val($("#id").attr('data-oldValue'))
                                userMessage.show(data.status, data.message);
                            }
                            else {
                                //container.html("");
                                //container.html(data.viewMarkup);
                                $("#id").val($("#id").attr('data-oldValue'))
                                userMessage.show(data.status, data.message);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });
                }
            });

            //e.preventDefault();
        }
    },


    submitFormWithCallBack: function (e, formURL, formData, formObj, message, checkValidate, callback) {
        //validate = validate || false;
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {
            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        url: formURL,
                        type: 'POST',
                        data: formData,
                        mimeType: "multipart/form-data",
                        contentType: false,
                        cache: false,
                        processData: false,
                        success: function (data, textStatus, jqXHR) {
                            //alert(data.status + "---" + data.message)
                            //userMessage.show("Success", "The Record has been Updated Successfully");
                            var obj = jQuery.parseJSON(data);
                            reloadSectionStatus = obj.status;
                            if (obj.id != "" && obj.id != null && obj.id != 'undefined') {
                                if ($("#id").length > 0)
                                    $("#id").val(obj.id)
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + obj.id + '" />')
                                }
                                if ($('[data-value="save"]').length > 0) {
                                    //$("button[type='reset']").attr('data-type', 'new-reset');
                                    //$("button[data-type='new-reset']").attr('type', 'button');
                                    //$("button[data-type='new-reset']").prop("type", "button");
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');
                            }

                            if (obj.status != "Success") {
                                $("#id").val($("#id").attr('data-oldValue'))
                                userMessage.show(data.status, data.message);
                            }
                            //alert(data["status"]+"----"+ data["message"])

                            userMessage.show(obj.status, obj.message);
                            if (callback != null) {
                                callback(obj);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            //log error
                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });


                }
            });
        }


    },

    submitFormWithCallBack1: function (e, formURL, formData, formObj, message, checkValidate, callback) {
        //validate = validate || false;
        checkValidate = checkValidate !== null ? checkValidate : true;
        if (checkValidate && !$('#validation-form').valid()) {
            return false;
        }
        else {
            $.SmartMessageBox({
                title: "Alert!",
                content: message,
                buttons: '[No][Yes]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        url: formURL,
                        type: 'POST',
                        data: formData,
                        mimeType: "multipart/form-data",
                        contentType: false,
                        cache: false,
                        processData: false,
                        success: function (data, textStatus, jqXHR) {
                            //alert(data.status + "---" + data.message)
                            //userMessage.show("Success", "The Record has been Updated Successfully");
                            var obj = jQuery.parseJSON(data);
                            reloadSectionStatus = obj.status;
                            if (obj.id != "" && obj.id != null && obj.id != 'undefined') {
                                if ($("#id").length > 0)
                                    $("#id").val(obj.id)
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" name="id" value="' + obj.id + '" />')
                                }
                                if ($('[data-value="save"]').length > 0) {
                                    //$("button[type='reset']").attr('data-type', 'new-reset');
                                    //$("button[data-type='new-reset']").attr('type', 'button');
                                    //$("button[data-type='new-reset']").prop("type", "button");
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');
                            }

                            if (obj.status != "Success") {
                                $("#id").val($("#id").attr('data-oldValue'))
                                userMessage.show(data.status, data.message);
                            }
                            //alert(data["status"]+"----"+ data["message"])

                            userMessage.show(obj.status, obj.message);
                            //$('#code').val(data.code);
                            $('#code').val(obj.code);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            //log error
                            if ($("#tempId").length > 0)
                                if ($("#id").length > 0)
                                    $("#id").val($("#tempId").val())
                            _showErrorMessage();
                        }
                    });


                }
            });
        }


    },

    deleteCallBack: function (actionUrl, callback) {
        var message = $("#alert-delete").val();
        $.SmartMessageBox({
            title: "Alert!",
            content: message,
            buttons: '[No][Yes]'
        },
            function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        global: false,
                        data: {},
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        beforeSend: function () { },
                        success: function (data) {
                            userMessage.show(data.status, data.message);
                            if (data.status == "Success") {
                                if (callback != null)
                                    callback(data);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            }

        );
    },

    // CREATED BY SURYA FOR RFQMaster SAVE from PurchaseRequest
    editCallBack: function (actionUrl, callback) {

        var message = $("#alert-edit").val();
        $.SmartMessageBox({
            title: "Alert!",
            content: message,
            buttons: '[No][Yes]'
        },
            function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        global: false,
                        data: {},
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        beforeSend: function () { },
                        success: function (data) {
                            userMessage.show(data.status, data.message);
                            if (data.status == "Success") {
                                if (callback != null)
                                    callback(data);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            }

        );



        //bootbox.confirm(message, function (result) {
        //    if (result) {
        //        $.ajax({
        //            type: "POST",
        //            url: actionUrl,
        //            data: {},
        //            cache: false,
        //            contentType: "application/json; charset=utf-8",
        //            success: function (data) {
        //                //call back to orgin
        //                userMessage.show(data.status, data.message);
        //                if (data.status == "Success") {
        //                    callback(data);
        //                }
        //            },
        //            error: function (xhr, ajaxOptions, thrownError) {
        //                //log error
        //                Error.redirectToErrorPage(xhr, ajaxOptions, thrownError);
        //            }
        //        });
        //    }
        //});
    },

    // CREATED BY KARTHIK FOR Purchase Order SAVE from PurchaseRequest
    orderCallBack: function (actionUrl, callback, message) {

        if (message == '' || message == null)
            message = $("#alert-order").val();
        $.SmartMessageBox({
            title: "Alert!",
            content: message,
            buttons: '[No][Yes]'
        },
            function (ButtonPressed) {
                if (ButtonPressed === "Yes") {
                    $.ajax({
                        type: "POST",
                        url: actionUrl,
                        global: false,
                        data: {},
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        beforeSend: function () { },
                        success: function (data) {
                            userMessage.show(data.status, data.message);
                            if (data.status == "Success") {
                                if (callback != null)
                                    callback(data);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            }

        );
    },

    navigateElement: function (actionUrl, containerElem) {
        //load view without postback
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: {},
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.status == "Denied") {
                    Error.accessDenied(data.message);
                } else if (data.status == "Erorr") {
                    userMessage.show(data.status, data.message);
                }
                else {
                    containerElem.html('');
                    containerElem.html(data.viewMarkup);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                _showErrorMessage();
            }
        });
    },

    navigateElementByParameter: function (actionUrl, param, containerElem) {
        //load view without postback
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: JSON.stringify(param),
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.status == "Denied") {
                    Error.accessDenied(data.message, containerElem);
                } else {
                    containerElem.html('');
                    containerElem.html(data.viewMarkup);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                _showErrorMessage();
            }
        });
    },


    validateEntity: function (rules, message) {
        $('#validation-form').validate({
            errorElement: 'div',
            errorClass: 'help-block',
            focusInvalid: false,
            rules: rules,
            messages: message,
            invalidHandler: function (event, validator) { //display error alert on form submit   
                $('.alert-danger', $('.login-form')).show();
            },

            highlight: function (e) {
                $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
            },
            success: function (e) {
                $(e).closest('.form-group').removeClass('has-error').addClass('has-info');
                $(e).remove();
            },
            errorPlacement: function (error, element) {
                if (element.is(':checkbox') || element.is(':radio')) {
                    var controls = element.closest('div[class*="col-"]');
                    if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
                    else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
                }
                else if (element.is('.select2')) {
                    error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
                }
                else if (element.is('.chosen-select')) {
                    error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
                }
                else error.insertAfter(element.parent());
            },
            submitHandler: function (form) { },
            invalidHandler: function (form) { }
        });
    },
    loadTable: function (partName, hasButton, hasCondition, columnCount, extendedEventBtn, conditionType, conditionId, filterId, displayAll, readOnly, hideButton, sort_index, AutoHideColumnm, ExportBtn, recordId, scrollX, destinationTbl, stockInSpare, scrollY, hasCheckBox, minButtonWide, displayLength) {
        var tbl = null;

        $('.table-congif').each(function (index, value) {
            tableCounter++;
        })

        var elemName = "[value='" + partName + "']"
        var renderBtnPosition = 0;
        var tblName = '#dt_' + partName;
        if (hideButton == null || hideButton == "undefined")
            hideButton = false;
        if (extendedEventBtn == null || extendedEventBtn == "undefined")
            extendedEventBtn = false;
        if (readOnly == null || readOnly == "undefined")
            readOnly = false;
        if (sort_index == null || sort_index == "undefined" || sort_index == "")
            sort_index = 0;
        if (displayAll == null || displayAll == "undefined")
            displayAll = false;
        if (AutoHideColumnm == null || AutoHideColumnm == "undefined")
            AutoHideColumnm = false;
        if (ExportBtn == null || ExportBtn == "undefined")
            ExportBtn = false;
        if (scrollX == null || scrollX == "undefined" || scrollX == false || scrollX == 'false')
            scrollX = null;
        if (scrollY == null || scrollY == "undefined" || scrollY == false || scrollY == 'false')
            scrollY = null;

        if (destinationTbl != null && destinationTbl != "undefined") {
            tblName = '#dt_' + destinationTbl;
            elemName += "[data-destination-table='" + destinationTbl + "']"
        } else {
            elemName += ':not([data-destination-table])';
        }
        if (hasCheckBox == null || hasCheckBox == "undefined" || hasCheckBox == "false" || hasCheckBox == "False" || hasCheckBox == false) {
            hasCheckBox = false;
        } else {
            renderBtnPosition = 1;
        }

        if (minButtonWide == null || minButtonWide == "undefined" || minButtonWide == '')
            minButtonWide = '100';

        var gridButtonStartIndex = 0;
        setTimeout(function () {
            $.ajax({
                url: partName + '/RenderAction',
                datatype: "json",
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ readOnly: readOnly, myKey: filterId }),
                async: true,
                processData: false,
                cache: false,
                hideLoading: true,
                success: function (data) {
                    if (tbl) {
                        tbl.destroy();
                    }

                    //bind dataTable

                    var responsiveHelper_dt_basic = undefined;
                    var responsiveHelper_datatable_fixed_column = undefined;
                    var responsiveHelper_datatable_col_reorder = undefined;
                    var responsiveHelper_datatable_tabletools = undefined;

                    var breakpointDefinition = {
                        tablet: 1024,
                        phone: 480
                    };

                    if (hasCheckBox || conditionId == 0 || conditionId == 1 || conditionId == 2 || conditionId == 4 || conditionId == 6 || conditionId == 10 || conditionId == 12 || conditionId == 22) {
                        gridButtonStartIndex = 1;
                    }

                    if (hasButton && $(tblName + " thead tr:first").find('th:last-child').hasClass('table-column-header')) {
                        var tempColHTML = $(tblName + " thead tr:first").find('th:last-child');
                        if (tempColHTML != null && tempColHTML != undefined && tempColHTML != '' && tempColHTML != 'undefined') {
                            $(tempColHTML).css('width', minButtonWide + 'px')
                            $(tempColHTML).css('min-width', minButtonWide + 'px')
                            $(tblName + " thead tr:first").find('th:last-child').remove();
                            $(tempColHTML).insertBefore(tblName + " thead tr:first th:nth-child(" + (gridButtonStartIndex + 1) + ")");
                        }
                    }

                    var column = [];
                    var i = 0;
                    $.map(Array(columnCount), function (value, index) {
                        if ((hasCheckBox || conditionId == 2 || conditionId == 1 || conditionId == 0 || conditionId == 22) && index == 0) {
                            var dateValue = columnCount - conditionId;
                            if (hasCheckBox) {
                                dateValue = columnCount - 2;
                            }
                            column[0] = {
                                "mData": columnCount - conditionId,
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    if (full[(columnCount - 1)] == "0") {
                                        return ""
                                    } else if (full[(columnCount)] == "false") {
                                        return ""
                                    }
                                    else {
                                        return '<div class="smart-form" ><label class="checkbox"><input type="checkbox" name="checkbox" class="checkboxSelectable" data-val="' + full[dateValue] + '"><i style="margin-top:-4px"></i></label></div>'
                                    }

                                }
                            }
                        }
                        else if ((conditionId == 10) && index == 0) {
                            column[0] = {
                                "mData": columnCount - conditionId,
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    if (full[(columnCount + 2)] == "0") {
                                        return '<div class="smart-form" style="margin-bottom:4px"><label class="checkbox"><input type="checkbox" name="checkbox" class="checkboxSelectable" data-val="' + full[columnCount] + '" data-val2="' + full[columnCount + 1] + '"><i></i></label></div>'
                                    } else {
                                        return '<div class="smart-form" style="margin-bottom:4px"><label class="' + (full[columnCount + 2] == "1" ? "ScheduleExist" : full[columnCount + 2] == "2" ? "PPMPeriod" : "") + '"></label></div>'
                                    }
                                }
                            }
                        }
                        else if ((conditionId == 12) && index == 0) {
                            column[0] = {
                                "mData": columnCount - conditionId,
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    return '<label class="' + full[columnCount + 1] + '">' + full[0] + '</label>'
                                }
                            }
                            i++
                        }
                        else if (conditionId == 6 && index == 0) {
                            column[0] = {
                                "mData": columnCount - conditionId,
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    if (full[(columnCount)] == "1") {
                                    }
                                    else {
                                        return '<div class="smart-form" style="margin-bottom:4px"><label class="radio"><input type="radio" name="radio" class="radioSelectable" data-val="' + full[columnCount - 1] + '"><i></i></label></div>'
                                    }
                                }
                            }
                        }

                        else if ((conditionId == 2 || conditionId == 7 || conditionId == 22) && hasButton && (hideButton == false || hasButton) && index == gridButtonStartIndex) {
                            column[gridButtonStartIndex] = {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    var edit = /<([span]*)\b[^>]*class="pencil[^>]*>(.*?)<\/\1>/gi
                                    var trash = /<([span]*)\b[^>]*class="trash[^>]*>(.*?)<\/\1>/gi
                                    var eye = /<([span]*)\b[^>]*class="eye[^>]*>(.*?)<\/\1>/gi
                                    var check = /<([span]*)\b[^>]*class="check[^>]*>(.*?)<\/\1>/gi
                                    var star = /<([span]*)\b[^>]*class="star[^>]*>(.*?)<\/\1>/gi
                                    var phone = /<([span]*)\b[^>]*class="phone[^>]*>(.*?)<\/\1>/gi
                                    var create = /<([span]*)\b[^>]*class="create[^>]*>(.*?)<\/\1>/gi
                                    var arrow = /<([span]*)\b[^>]*class="arrow[^>]*>(.*?)<\/\1>/gi
                                    var followup = /<([span]*)\b[^>]*class="followUp[^>]*>(.*?)<\/\1>/gi
                                    var indicator = /<([span]*)\b[^>]*class="indicator[^>]*>(.*?)<\/\1>/gi
                                    var a = partName.toUpperCase() + "_ID";
                                    var re = new RegExp(a, 'g');
                                    var b = partName.toUpperCase() + "_COLOR";
                                    var color = new RegExp(b, 'g');
                                    var tempbuttons = data.viewMarkup;
                                    if (conditionId == 22) {
                                        if (full[(full.length - 1)] == "" || full[(full.length - 1)] == null || full[(full.length - 1)] == undefined) {
                                            tempbuttons = tempbuttons.replace(/\s+/g, " ").replace(indicator, "");
                                        }
                                        else
                                            tempbuttons = tempbuttons.replace(/\s+/g, " ").replace(color, full[(full.length - 1)])
                                    }
                                    if (full[(columnCount)] == "1") {
                                        return tempbuttons.replace(/\s+/g, " ").replace(edit, "").replace(re, full[(columnCount - 1)]);
                                    }
                                    if (full[(columnCount - 1)] == "3") {
                                        return tempbuttons.replace(/\s+/g, " ").replace(followup, "").replace(trash, "").replace(edit, "").replace(eye, "").replace(phone, "").replace(re, full[(columnCount - 2)]);
                                    }
                                    if (full[(columnCount - 1)] == "8") {
                                        return tempbuttons.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(trash, "").replace(edit, "").replace(eye, "").replace(re, full[(columnCount - 2)]);
                                    }
                                    if (full[(columnCount - 1)] == "9") {
                                        return tempbuttons.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(phone, "").replace(re, full[(columnCount - 2)]);
                                    }
                                    else {
                                        re = new RegExp(partName.toUpperCase() + '_ID', "g");
                                        return tempbuttons.replace(re, full[columnCount - 2]);
                                    }
                                }
                            }
                        }



                        //Created by surya for WorkOrder Index
                        else if ((conditionId == 14) && hasButton && (hideButton == false || hideButton) && index == gridButtonStartIndex) {
                            column[gridButtonStartIndex] = {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    var edit = /<([span]*)\b[^>]*class="pencil[^>]*>(.*?)<\/\1>/gi
                                    var trash = /<([span]*)\b[^>]*class="trash[^>]*>(.*?)<\/\1>/gi
                                    var eye = /<([span]*)\b[^>]*class="eye[^>]*>(.*?)<\/\1>/gi
                                    var check = /<([span]*)\b[^>]*class="check[^>]*>(.*?)<\/\1>/gi
                                    var star = /<([span]*)\b[^>]*class="star[^>]*>(.*?)<\/\1>/gi
                                    var phone = /<([span]*)\b[^>]*class="phone[^>]*>(.*?)<\/\1>/gi
                                    var create = /<([span]*)\b[^>]*class="create[^>]*>(.*?)<\/\1>/gi
                                    var arrow = /<([span]*)\b[^>]*class="arrow[^>]*>(.*?)<\/\1>/gi
                                    var followup = /<([span]*)\b[^>]*class="followUp[^>]*>(.*?)<\/\1>/gi
                                    var a = partName.toUpperCase() + "_ID";
                                    var re = new RegExp(a, 'g');
                                    if (index - 1 == "11")
                                        return '<span class="maintenance-view-workorder btn btn-ribbon " data-url="WORKORDER_ID" title="View" data-rel="tooltip" data-placement="bottom" data-container="body"><i class="fa fa-eye "></i></span>'
                                    if (full[(columnCount)] == "1") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(edit, "").replace(re, full[(columnCount - 1)]);
                                    }
                                    if (full[(columnCount - 1)] == "3") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(trash, "").replace(edit, "").replace(eye, "").replace(phone, "").replace(re, full[(columnCount - 2)]);
                                    }
                                    if (full[(columnCount - 1)] == "8") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(trash, "").replace(edit, "").replace(eye, "").replace(re, full[(columnCount - 2)]);
                                    }
                                    if (full[(columnCount - 1)] == "9") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(phone, "").replace(re, full[(columnCount - 2)]);
                                    }
                                    else {
                                        re = new RegExp(partName.toUpperCase() + '_ID', "g");
                                        return data.viewMarkup.replace(re, full[columnCount - 2]);
                                    }
                                }
                            }
                        }

                        else if (conditionId == 3 && hasButton && (hideButton == false || hasButton) && index == gridButtonStartIndex) {
                            column[gridButtonStartIndex] = {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    var maintenance = null; // /<([span]*)\b[^>]*class="maintenance[^>]*>(.*?)<\/\1>/gi
                                    var edit = null; // /<([span]*)\b[^>]*class="delete[^>]*>(.*?)<\/\1>/gi
                                    var tool = null; // /<([span]*)\b[^>]*class="tool[^>]*>(.*?)<\/\1>/gi
                                    var remove = null; // /<([span]*)\b[^>]*class="delete[^>]*>(.*?)<\/\1>/gi
                                    var add = null; // /<([span]*)\b[^>]*class="add[^>]*>(.*?)<\/\1>/gi
                                    var a = partName.toUpperCase() + "_ID";
                                    var re = new RegExp(a, 'g');
                                    if (full[(columnCount - 1)].toLowerCase() == 'false') {

                                        if (full[(columnCount + 2)].toLowerCase() == 'false') {
                                            remove = /<([span]*)\b[^>]*class="delete[^>]*>(.*?)<\/\1>/gi
                                        }
                                        if (full[(columnCount + 1)].toLowerCase() == 'false') {
                                            edit = /<([span]*)\b[^>]*class="maintenance[^>]*>(.*?)<\/\1>/gi
                                        }
                                        if (full[(columnCount)].toLowerCase() == 'false' || full[(columnCount + 3)].toLowerCase() == 'flase') {
                                            add = /<([span]*)\b[^>]*class="add[^>]*>(.*?)<\/\1>/gi
                                        }
                                        if (full[(columnCount - 8)].toUpperCase() != 'EQUIPMENT') {
                                            tool = /<([span]*)\b[^>]*class="tool[^>]*>(.*?)<\/\1>/gi
                                        }
                                    } else {
                                        remove = /<([span]*)\b[^>]*class="delete[^>]*>(.*?)<\/\1>/gi
                                        edit = /<([span]*)\b[^>]*class="maintenance[^>]*>(.*?)<\/\1>/gi
                                        add = /<([span]*)\b[^>]*class="add[^>]*>(.*?)<\/\1>/gi
                                        tool = /<([span]*)\b[^>]*class="tool[^>]*>(.*?)<\/\1>/gi
                                    }

                                    if (full[(columnCount - 1)].toLowerCase() == 'true' && full[(columnCount + 3)].toLowerCase() == 'true') {
                                        add = null;
                                        remove = /<([span]*)\b[^>]*class="delete[^>]*>(.*?)<\/\1>/gi
                                        edit = /<([span]*)\b[^>]*class="maintenance[^>]*>(.*?)<\/\1>/gi
                                        tool = /<([span]*)\b[^>]*class="tool[^>]*>(.*?)<\/\1>/gi
                                    }
                                    re = new RegExp('data-url="' + partName.toUpperCase() + '_ID"', "g");
                                    st = new RegExp('data-status="' + partName.toUpperCase() + '_ID"', "g");
                                    // Added by vijay for enable condition based display of buttons
                                    // var st2 = new RegExp('data-status2="' + partName.toUpperCase() + '_STATUS"', "g");

                                    return data.viewMarkup.replace(/\s+/g, " ").replace(add, "").replace(edit, "").replace(maintenance, "").replace(tool, "")
                                        .replace(remove, "").replace(re, 'data-url="' + full[columnCount + 4] + '"')
                                        .replace(st, 'data-status="' + full[columnCount - 1] + '"');
                                }
                            }
                        }


                        else if (conditionId == 4 && index == 0) {
                            column[0] = {
                                "mData": columnCount - conditionId,
                                "sName": "ID",
                                "bSearchable": true,
                                "bSortable": true,
                                "render": function (oObj, type, full) {
                                    return '<div class="smart-form" style="margin-bottom:4px"><a  class="linkToGrid" data-value="' + full[columnCount] + '">' + full[0] + '</a></div>'

                                }
                            }
                            i++
                        }
                        else if (conditionId == 8 && (hideButton || !hideButton) && index == (gridButtonStartIndex)) {
                            column[gridButtonStartIndex] = {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    var edit = /<([span]*)\b[^>]*class="pencil[^>]*>(.*?)<\/\1>/gi
                                    var trash = /<([span]*)\b[^>]*class="trash[^>]*>(.*?)<\/\1>/gi
                                    var eye = /<([span]*)\b[^>]*class="eye[^>]*>(.*?)<\/\1>/gi
                                    var check = /<([span]*)\b[^>]*class="check[^>]*>(.*?)<\/\1>/gi
                                    var star = /<([span]*)\b[^>]*class="star[^>]*>(.*?)<\/\1>/gi
                                    var a = partName.toUpperCase() + "_ID";
                                    var re = new RegExp(a, 'g');
                                    if (full[(columnCount)] == "0") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(eye, "").replace(check, "").replace(re, full[(columnCount - 1)]);
                                    }
                                    if (full[(columnCount)] == "1") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(edit, "").replace(trash, "").replace(star, "").replace(re, full[(columnCount - 1)]);
                                    }
                                    else {
                                        re = new RegExp(partName.toUpperCase() + '_ID', "g");
                                        return data.viewMarkup.replace(re, full[(columnCount - 1)]);
                                    }

                                }
                            }
                        }

                        else if (conditionId != 3 && hasButton && index == gridButtonStartIndex && hideButton == false) {
                            column[gridButtonStartIndex] = {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    re = new RegExp(partName.toUpperCase() + '_ID', "g");
                                    return data.viewMarkup.replace(re, full[(columnCount - 1)]);
                                }
                            }
                        }

                        else if (hasButton && hideButton && !extendedEventBtn && index == gridButtonStartIndex) {
                            column[gridButtonStartIndex] = {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    var edit = /<([span]*)\b[^>]*class="pencil[^>]*>(.*?)<\/\1>/gi
                                    var a = partName.toUpperCase() + "_ID";
                                    var re = new RegExp(a, 'g');
                                    if (full[(columnCount)] == "0") {

                                        return data.viewMarkup.replace(/\s+/g, " ").replace(edit, "").replace(re, full[(columnCount - 1)]);
                                    }
                                    else {
                                        re = new RegExp(partName.toUpperCase() + '_ID', "g");
                                        return data.viewMarkup.replace(re, full[(columnCount - 1)]);
                                    }

                                }
                            }
                        }
                        else if (hasButton && hideButton && extendedEventBtn && index == gridButtonStartIndex) {
                            column[gridButtonStartIndex] = {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "render": function (oObj, type, full) {
                                    var a = partName.toUpperCase() + "_ID";
                                    var re = new RegExp(a, 'g');
                                    var edit = /<([span]*)\b[^>]*class="pencil[^>]*>(.*?)<\/\1>/gi
                                    var trash = /<([span]*)\b[^>]*class="trash[^>]*>(.*?)<\/\1>/gi
                                    var eye = /<([span]*)\b[^>]*class="eye[^>]*>(.*?)<\/\1>/gi
                                    var check = /<([span]*)\b[^>]*class="check[^>]*>(.*?)<\/\1>/gi
                                    var star = /<([span]*)\b[^>]*class="star[^>]*>(.*?)<\/\1>/gi
                                    var phone = /<([span]*)\b[^>]*class="phone[^>]*>(.*?)<\/\1>/gi
                                    var create = /<([span]*)\b[^>]*class="create[^>]*>(.*?)<\/\1>/gi
                                    var arrow = /<([span]*)\b[^>]*class="arrow[^>]*>(.*?)<\/\1>/gi
                                    if (full[(columnCount)] == undefined) {
                                        columnCount = columnCount - 1;
                                    }
                                    var idIndexColumn = columnCount - 1
                                    var columnCountIndex = columnCount;
                                    if (conditionId == 33) {
                                        idIndexColumn = idIndexColumn - 1;
                                        columnCountIndex = columnCountIndex - 1;
                                    }
                                    //var followup = (full[(columnCount + 1)] == "1" ? "" : /<([span]*)\b[^>]*class="followUp[^>]*>(.*?)<\/\1>/gi)
                                    var followup = /<([span]*)\b[^>]*class="followUp[^>]*>(.*?)<\/\1>/gi
                                    var recall = /<([span]*)\b[^>]*class="recall[^>]*>(.*?)<\/\1>/gi
                                    if (full[(columnCountIndex)] == "0") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(edit, "").replace(phone, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "2") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(eye, "").replace(check, "").replace(star, "").replace(phone, "").replace(arrow, "").replace(recall, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "3") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(trash, "").replace(edit, "").replace(eye, "").replace(phone, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "4") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(eye, "").replace(edit, "").replace(recall, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "5") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(trash, "").replace(edit, "").replace(star, "").replace(check, "").replace(arrow, "").replace(recall, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "6") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(check, "").replace(phone, "").replace(arrow, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "7") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(star, "").replace(trash, "").replace(edit, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "8") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(trash, "").replace(edit, "").replace(eye, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "9") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(phone, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "10") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(check, "").replace(edit, "").replace(trash, "").replace(phone, "").replace(recall, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "11") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(trash, "").replace(eye, "").replace(phone, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "12") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(edit, "").replace(followup, "").replace(create, "").replace(star, "").replace(trash, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "13") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(create, "").replace(edit, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "14") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(followup, "").replace(trash, "").replace(edit, "").replace(eye, "").replace(phone, "").replace(check, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "15") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(edit, "").replace(trash, "").replace(phone, "").replace(recall, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "16") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(edit, "").replace(followup, "").replace(create, "").replace(trash, "").replace(re, full[idIndexColumn]);
                                    }
                                    if (full[(columnCountIndex)] == "17") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(eye, "").replace(recall, "").replace(followup, "").replace(create, "").replace(re, full[idIndexColumn]);
                                    }
                                    //helpdesk
                                    if (full[(columnCountIndex)] == "30") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(eye, "").replace(edit, "").replace(followup, "").replace(re, full[idIndexColumn]);
                                    }
                                    //helpdesk
                                    if (full[(columnCountIndex)] == "31") {
                                        return data.viewMarkup.replace(/\s+/g, " ").replace(eye, "").replace(check, "").replace(phone, "").replace(re, full[idIndexColumn]);
                                    }

                                    else {
                                        re = new RegExp(partName.toUpperCase() + '_ID', "g");
                                        return data.viewMarkup.replace(re, full[idIndexColumn]);
                                    }

                                }
                            }
                        }
                        else {
                            column[index] = { "mData": i }
                            i++;
                        }
                    })

                    if (conditionId == 0 && sort_index == 0)
                        sort_index = 1;
                    var exportButton;
                    if (ExportBtn) {
                        exportButton = [
                            (AutoHideColumnm ? 'colvis' : undefined),
                            {
                                extend: 'copyHtml5',
                                exportOptions: {
                                    columns: ':visible'
                                },
                                text: '<i class="fa fa-files-o"></i>',
                                titleAttr: 'Copy',
                                title: partName
                            },
                            //{
                            //    extend: 'csvHtml5',
                            //    exportOptions: {
                            //        columns: ':visible'
                            //    },
                            //    text:      '<i class="fa fa-file-text-o"></i>',
                            //    titleAttr: 'CSV',
                            //    title: partName
                            //},
                            {
                                extend: 'excelHtml5',
                                exportOptions: {
                                    columns: ':visible'
                                },
                                text: '<i class="fa fa-file-excel-o"></i>',
                                titleAttr: 'Excel',
                                title: partName
                            },
                            {
                                extend: 'pdfHtml5',
                                exportOptions: {
                                    columns: ':visible'
                                },
                                title: partName,
                                text: '<i class="fa fa-file-pdf-o"></i>',
                                titleAttr: 'PDF',
                                download: 'open'
                            },
                            {
                                extend: 'print',
                                exportOptions: {
                                    columns: ':visible'
                                },
                                message: partName,
                                text: '<i class="fa fa-print"></i>',
                                titleAttr: 'Print',
                                autoPrint: false
                            },
                            'pageLength'
                        ];
                    }
                    var selected = [];
                    var delay = 100;//1 seconds
                    var isPaging = false;
                    var loadCallbackCounter = 0;
                    var statusIndexValue = parseInt($(tblName).attr('data-stindex'))
                    //your code to be executed after 1 seconds
                    // Update the column format 
                    var amountIndex = parseInt($(tblName).attr('data-amountcolumn'));
                    // Dynamically apply the render function to the 4th column (index 3)
                    if (!isNaN(amountIndex)) {
                        column[amountIndex].render = function (data, type, row) {
                            if (type === 'display' || type === 'filter') {
                                // Custom money formatter with thousand separators
                                return formatMoney(data);
                            }
                            return data;
                        };
                    }

                    // Custom money formatting function
                    function formatMoney(number) {
                        var numberParts = number.toString().split(".");
                        numberParts[0] = numberParts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                        return numberParts.join(".");
                    }
                    var hideIndexValue = parseInt($(tblName).attr('data-hideindex'))

                    tbl = $(tblName).DataTable({
                        //lengthMenu: [
                        //   [10, 25, 50, -1],
                        //   ['10 rows', '25 rows', '50 rows', 'Show all']
                        //],
                        "language": {
                            search: '', searchPlaceholder: "Search...", lengthMenu: "_MENU_", buttons: {
                                pageLength: {
                                    _: "%d",
                                },
                                // pageLength: {
                                //     _: "Show %d rows",
                                //    '-1': "Show All"
                                //},
                                colvis: '<i class="fa fa-lg fa-fw fa-list-alt auto-width"></i>'
                            }
                        },
                        columnDefs: [
                            { visible: true, targets: [hideIndexValue], className: "hidden" }
                        ],
                        "processing": true,
                        "responsive": true,
                        "order": [[sort_index, "", "asc", "desc"]],
                        "bSortable": true,
                        "bServerSide": true,
                        "bDestroy": true,
                        "sAjaxSource": partName + '/Bind' + partName,
                        "iDisplayLength": displayLength,
                        "iFilterAct": displayAll,
                        "scrollX": true,// scrollX,
                        "stateSave": true,
                        "autoWidth": true,
                        "deferLoading": 0,
                        "deferRender": true,
                        "aoColumns": column,
                        "sExtends": "collection",
                        "sButtonText": "Export",
                        //"dom": 'Bfrtip',
                        //'l' - Length changing
                        //'f' - Filtering input
                        //'t' - The table!
                        //'i' -Information
                        //'p' - Pagination
                        //'r' - pRocessing
                        "dom": "<'row-flex datatable-toolbar-header'<'col-xs-12 col-sm-3 col-md-3 col-lg-3 no-padding'f><'col-xs-12 col-sm-9 col-md-9 col-lg-9 no-padding hidden-xs-down text-right justify-content-end'" + (AutoHideColumnm ? "R" : "") + (ExportBtn ? "B" : "") + ">r>" +
                            "t" + "<'row-flex dt-toolbar-footer'<'col-xs-12 col-sm-6 col-md-6 col-lg-6 hidden-xs-down'i><'col-xs-12 col-sm-6 col-md-6 col-lg-6'p>>",
                        "buttons": exportButton,
                        "fnServerData": function (sSource, aoData, fnCallback) {
                            aoData.push({ "name": "roleId", "value": "admin" });
                            $.ajax({
                                "dataType": 'json',
                                "contentType": "application/json; charset=utf-8",
                                "type": "GET",
                                "url": sSource,
                                "data": aoData,
                                "success": function (msg) {
                                    //var json = jQuery.parseJSON(msg.d);
                                    //if (msg.iTotalDisplayRecords == 0) {
                                    //    scrollX = false;
                                    //}
                                    if (msg != null)
                                        fnCallback(msg);
                                    $(tblName).show();
                                },
                                error: function (xhr, textStatus, error) {
                                    if (typeof console == "object") {
                                        if ($('div.active div.tab-pane-content').length > 0) {
                                            $('div.active div.tab-pane-content').html('<div class="row1"><div class="col-xs-12"><div class="error-container"><div class="well"><h1 class="grey lighter smaller text-center"><span class="blue bigger-125"><i class="icon-random"></i></span>' + error + ' </h1> </div></div> </div></div>')
                                        } else {
                                            $(tblName + "_wrapper .dataTables_info .txt-color-darken , " + tblName + "_wrapper .dataTables_info .text-primary").html('0')
                                            $(tblName + "_wrapper .dataTables_paginate ul.pagination").html('')
                                            $(tblName + "_processing").hide()
                                            $(tblName + " tbody").html('<tr><td colspan="' + columnCount + '"><div class="error-container"><div class="well"><h1 class="grey lighter smaller text-center" style="text-align: center;font-weight: bold;"><span class="blue bigger-125"><i class="icon-random"></i></span>' + error + ' </h1> </div></div></td></tr>')
                                        }
                                    }
                                }
                            });
                        },
                        "preDrawCallback": function () {
                            //// Initialize the responsive datatables helper once.
                            //if (!responsiveHelper_datatable_fixed_column) {
                            //    responsiveHelper_datatable_fixed_column = new ResponsiveDatatablesHelper($(tblName), breakpointDefinition);
                            //}
                            ////if (ExportBtn) {
                            //if (!responsiveHelper_datatable_tabletools) {
                            //    responsiveHelper_datatable_tabletools = new ResponsiveDatatablesHelper($(tblName), breakpointDefinition);
                            //}
                            ////}

                            //if (!responsiveHelper_datatable_col_reorder) {
                            //    responsiveHelper_datatable_col_reorder = new ResponsiveDatatablesHelper($(tblName), breakpointDefinition);
                            //}
                        },
                        "rowCallback": function (nRow, aData, iDisplayIndex) {
                            //responsiveHelper_datatable_fixed_column.createExpandIcon(nRow);
                            ////if (ExportBtn) {
                            //responsiveHelper_datatable_tabletools.createExpandIcon(nRow);
                            ////}
                            if (!isNaN(statusIndexValue)) {
                                $('td', nRow).attr('data-st', aData[statusIndexValue]).addClass(aData[statusIndexValue]);
                                var status = aData[statusIndexValue];
                                var statusCell = $('td', nRow).eq(statusIndexValue + 1);
                                // Check if the status is "Completed" and hideIndexValue is "True"
                                if (status === 'Completed' && aData[hideIndexValue - 1] == 1) {
                                    const statusLabel = $("<div>", {
                                        "class": "status-label btn-success",
                                        "css": { "padding-left": "0px", "padding-right": "0px" },
                                        "text": "Posting Success"
                                    });

                                    statusCell.append(statusLabel);
                                }

                                if (status === 'Completed' && aData[hideIndexValue - 1] == 2) {
                                    const statusLabel = $("<div>", {
                                        "class": "status-label btn-danger",
                                        "css": { "padding-left": "0px", "padding-right": "0px" },
                                        "text": "Posting Failed"
                                    });

                                    statusCell.append(statusLabel);
                                }

                                if (status === 'Completed' && aData[hideIndexValue - 1] == 3) {
                                    const statusLabel = $("<div>", {
                                        "class": "status-label btn-warning",
                                        "css": { "padding-left": "0px", "padding-right": "0px" },
                                        "text": "Posting Pending"
                                    });

                                    statusCell.append(statusLabel);
                                }
                            }
                            if (!isNaN(hideIndexValue)) { $('td', nRow).addClass("hide-status-" + aData[hideIndexValue - 1]); }
                            //responsiveHelper_datatable_col_reorder.createExpandIcon(nRow);
                        },
                        "drawCallback": function (oSettings) {

                            //responsiveHelper_datatable_fixed_column.respond();
                            ////if (ExportBtn) {
                            //responsiveHelper_datatable_tabletools.respond();
                            ////}
                            //responsiveHelper_datatable_col_reorder.respond();
                            if (loadCallbackCounter == 1) {
                                _initAction(partName);
                                if (extendedEventBtn == 'true' || extendedEventBtn == true || extendedEventBtn == 'True') {
                                    _initActionExtended(partName, readOnly, isPaging, destinationTbl);
                                }
                            }
                            $(tblName + ' tbody tr td .smart-form a').on("click", function () {
                                var tableName = $(this).closest('table').attr('id');
                                setLocalStorage(tableName)
                            })
                            isPaging = false;
                            loadCallbackCounter = 1;
                            //$('[data-rel="tooltip"]').tooltip({ boundary: 'window' })
                        },
                        "fnServerParams": function (aoData) {
                            //getLocalStorage(tblName)
                            aoData.push({ "name": "myKey", "value": filterId });
                            aoData.push({ "name": "recordId", "value": recordId });

                            aoData.push({ "name": "stockInSpare", "value": stockInSpare }); // newly added by titus 07/01/16
                            aoData.push({ "name": "conditionType", "value": $('#conditionType').val() }); // newly added by 12/12/2018
                            // newly added by Vijay 25/08/2024

                        },
                    }).columns.adjust();

                    var fieldList = [];
                    $('.searchPanelWithSearchBtn :input').each(function (index, value) {

                        var listItem = $(this).closest('.form-group');
                        if ($(this).val() != 'undefined' && $("div.form-group").index(listItem) > -1) {
                            var fieldValue = $(this).val() == null ? "" : ($(this).val().toString().toLowerCase() == "-- multi select --" ? "" : $(this).val());
                            fieldList.push({ id: $(this).attr('id'), value: fieldValue, tagName: $(this).prop("tagName") })
                            if (!$(this).hasClass('datepicker') && !$(this).hasClass('timepicker')) {
                                tbl.column($("div.form-group").index(listItem))
                                    .search(fieldValue)
                            }
                        }

                    });
                    if (loadCallbackCounter == 1) {
                        getLocalStorage(tblName);
                    }

                    //var reload = false;
                    $('.searchPanel :input:not(".timepicker") , .searchPanelWithSearchBtn :input:not(".timepicker")').each(function (index, value) {

                        var listItem = $(this).closest('.form-group');
                        //alert($(this).val())
                        var index = $("div.form-group").index(listItem);
                        if (index >= columnCount) {
                            index = index - columnCount;
                        }
                        if ($(this).val() != '' && $(this).val() != null && $(this).val() != 'undefined') {

                            var currentValue = $(this).val();
                            var name = $(this).attr('name')//.replace(".Date","");
                            var dateElem = $("input[name='" + name + "'][class~='datepicker']");
                            if (dateElem.length > 0) {
                                isTime = false;
                                var timeElem = $("input[name='" + name + ".Time'][class~='timepicker']");
                                if (timeElem.length > 0) {

                                    isTime = true;
                                }
                                var currentValue = dateElem.val();
                                if (isTime && currentValue != null && currentValue != '') {
                                    var a = timeElem.val().trim();
                                    var b = ":00"
                                    var output = a.replace(" ", ":00 ");
                                    currentValue = dateElem.val() + " " + output;

                                }
                            }
                            reload = true;
                            tbl.column(index)
                                .search(currentValue)
                        }
                    });
                    //if (reload) {
                    //    tbl.draw();
                    //}

                    $(tblName + '-search-panel :input[type="checkbox"]').each(function (index, value) {

                        var listItem = $(this).closest('.form-group');
                        //alert($(this).val())
                        var index = $("div.form-group").index(listItem);
                        if (index >= columnCount) {
                            index = index - columnCount;
                        }
                        if ($(this).val() != '' && $(this).val() != null && $(this).val() != 'undefined') {
                            reload = true;
                            tbl.column(index)
                                .search($(this).val())
                        }
                    });
                    //if (reload) {
                    //    tbl.draw();
                    //}

                    //reload = false;
                    $(tblName + '-search-panel :input.datepickerMY').each(function (index, value) {

                        var listItem = $(this).closest('.form-group');
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        //$(this).datepicker('setDate', new Date(year, month, 1));
                        var stringMonth = (parseInt(month) + 1);
                        if ((parseInt(month) + 1) <= 9)
                            stringMonth = "0" + (parseInt(month) + 1);

                        //var val = "01" + "/" + (parseInt(month) + 1) + "/" + year;
                        var val = "01" + "/" + stringMonth + "/" + year;
                        var index = $("div.form-group").index(listItem);
                        if (index >= columnCount) {
                            index = index - columnCount;
                        }
                        if (val != '' && val != null && val != 'undefined') {
                            reload = true;
                            tbl.column(index)
                                .search(val)
                        }
                    });



                    //if (reload) {
                    //    tbl.draw();
                    //}
                    tbl.draw(false);

                    //$(tblName + "_wrapper thead tr:first").find('th:last-child').remove();

                    //}, delay);

                    // custom toolbar
                    $("div.toolbar").html('<div class="text-right"><img src="img/logo.png" alt="SmartAdmin" style="width: 111px; margin-top: 3px; margin-right: 10px;"></div>');

                    $(tblName).on('page.dt', function () {
                        isPaging = true;
                    });

                    //$(tblName + ' tfoot tr th input').on('change keyup', function () {
                    //    //alert($(this).parent().index() + ':visible' + "   ---   " + this.value)
                    //    // tbl.column($(this).parent().index() + ':visible').search(this.value).draw();
                    //    tbl
                    //            .column($(this).parent().index())
                    //            .search(this.value)
                    //            .draw();
                    //});
                    $('.customFilterDatatableWithSearchBTN').on('click', function () {
                        if (tbl != null) {
                            CustomFilterDatatable();
                        }
                    });

                    $('.customFilterDatatableWithSearchBTNWithValidate').on('click', function () {
                        if (tbl != null) {
                            var form_Id = $(this).closest('form').attr('id');
                            if (form_Id != "" && form_Id != 'undefined') {
                                if (!$('#' + form_Id).valid()) {
                                    return false;
                                }
                                CustomFilterDatatable();
                            }
                        }
                    });

                    function CustomFilterDatatable() {
                        if ($('.avidGridFresh').length <= 0) {
                            var fieldList = [];
                            $('.searchPanelWithSearchBtn :input[type="number"]').each(function (index, value) {

                                //alert($(this).parent().index() + ':visible' + "   ---   " + this.value)
                                // tbl.column($(this).parent().index() + ':visible').search(this.value).draw();
                                var listItem = $(this).closest('.form-group');
                                var index = $("div.form-group").index(listItem);
                                if (index >= columnCount) {
                                    index = index - columnCount;
                                }
                                var fieldValue = $(this).val() == null ? "" : ($(this).val().toString().toLowerCase() == "-- multi select --" ? "" : $(this).val());
                                fieldList.push({ id: $(this).attr('id'), value: fieldValue, tagName: $(this).prop("tagName"), isDeferredLoad: $(this).hasClass('deferredLoad') })
                                //alert($(this).val())
                                tbl
                                    .column(index)
                                    .search($(this).val())

                            });

                            $('.searchPanelWithSearchBtn :input[type!="number"]').each(function (index, value) {

                                var listItem = $(this).closest('.form-group');
                                var index = $("div.form-group").index(listItem);
                                if (index >= columnCount) {
                                    index = index - columnCount;
                                }
                                var fieldValue = $(this).val() == null ? "" : ($(this).val().toString().toLowerCase() == "-- multi select --" ? "" : $(this).val());
                                fieldList.push({ id: $(this).attr('id'), value: fieldValue, tagName: $(this).prop("tagName"), isDeferredLoad: $(this).hasClass('deferredLoad') })
                                //alert($(this).parent().index() + ':visible' + "   ---   " + this.value)
                                //console.log(index + "   ---   " + $(this).val())
                                if (!$(this).hasClass('datepicker') && !$(this).hasClass('timepicker')) {
                                    tbl
                                        .column(index)
                                        .search($(this).val())
                                }
                            });

                            $('.searchPanelWithSearchBtn :input.datepicker').each(function (index, value) {
                                var listItem = $(this).closest('.form-group');
                                var index = $("div.form-group").index(listItem);
                                if (index >= columnCount) {
                                    index = index - columnCount;
                                }
                                var currentValue = this.value;
                                var name = $(this).attr('name')//.replace(".Date","");
                                var dateElem = $("input[name='" + name + "'][class~='datepicker']");
                                fieldList.push({ id: $(this).attr('id'), value: currentValue, tagName: $(this).prop("tagName"), isDeferredLoad: $(this).hasClass('deferredLoad') })
                                if (dateElem.length > 0) {
                                    isTime = false;
                                    var timeElem = $("input[name='" + name + ".Time'][class~='timepicker']");
                                    if (timeElem.length > 0) {
                                        isTime = true;
                                    }
                                    var currentValue = dateElem.val();
                                    if (isTime && currentValue != null && currentValue != '') {
                                        var a = timeElem.val().trim();
                                        var b = ":00"
                                        var output = a.replace(" ", ":00 ");
                                        currentValue = dateElem.val() + " " + output;

                                    }
                                }

                                tbl
                                    .column(index)
                                    .search(currentValue)
                                //   $(this).data('datepicker').inline = false;


                            });

                            setLocalStorage(tblName, fieldList);
                            tbl.draw(true);
                            if (typeof ExtendedFilterMethod == 'function') {
                                ExtendedFilterMethod();
                            }
                        }
                    }



                    $('.searchPanel :input[type="number"]').on('keyup', function () {
                        if ($('.avidGridFresh').length <= 0) {
                            //alert($(this).parent().index() + ':visible' + "   ---   " + this.value)
                            // tbl.column($(this).parent().index() + ':visible').search(this.value).draw();
                            var listItem = $(this).closest('.form-group');
                            var index = $("div.form-group").index(listItem);
                            if (index >= columnCount) {
                                index = index - columnCount;
                            }
                            //alert($(this).val())
                            tbl
                                .column(index)
                                .search($(this).val())
                                .draw();
                        }
                    });

                    $('.searchPanel :input[type!="number"]:not(".select2-input")').on('change', function () {
                        if (!$(this).hasClass('datepicker') && !$(this).hasClass('timepicker')) {
                            if ($('.avidGridFresh').length <= 0) {
                                var listItem = $(this).closest('.form-group');
                                var index = $("div.form-group").index(listItem);
                                if (index >= columnCount) {
                                    index = index - columnCount;
                                }
                                //alert($(this).parent().index() + ':visible' + "   ---   " + this.value)
                                tbl
                                    .column(index)
                                    .search($(this).val())
                                    .draw();
                            }
                        }
                    });

                    $('.searchPanel :input.datepicker').datepicker('option', 'onClose', function () {
                        if ($('.avidGridFresh').length <= 0) {
                            var currentValue = this.value;
                            var name = $(this).attr('name')//.replace(".Date","");
                            var dateElem = $("input[name='" + name + "'][class~='datepicker']");
                            if (dateElem.length > 0) {
                                isTime = false;
                                var timeElem = $("input[name='" + name + ".Time'][class~='timepicker']");
                                if (timeElem.length > 0) {

                                    isTime = true;
                                }
                                var currentValue = dateElem.val();
                                if (isTime && currentValue != null && currentValue != '') {
                                    var a = timeElem.val().trim();
                                    var b = ":00"
                                    var output = a.replace(" ", ":00 ");
                                    currentValue = dateElem.val() + " " + output;

                                }
                            }
                            var listItem = $(this).closest('.form-group');
                            var index = $("div.form-group").index(listItem);
                            if (index >= columnCount) {
                                index = index - columnCount;
                            }
                            tbl
                                .column(index)
                                .search(currentValue)
                                .draw();
                            $(this).data('datepicker').inline = false;

                            if (typeof ExtendedFilterMethod == 'function') {
                                ExtendedFilterMethod();
                            }
                        }
                    });

                    $(tblName + '-search-filter :input[type="number"] , ' + tblName + '-search-filter :input[type="text"]:not(".select2-input")').on('keyup', function () {

                        //alert($(this).parent().index() + ':visible' + "   ---   " + this.value)
                        //alert("A-" + listItem + "-" + $(this).val() + "-" + $(this).closest('th').index() + "-" + $(this).closest('th'))
                        if (!$(this).hasClass('datepicker') && !$(this).hasClass('timepicker')) {
                            tbl
                                .column($(this).closest('th').index())
                                .search($(this).val())
                                .draw();
                        }
                    });

                    $(tblName + '-search-filter :input[type!="number"]:not(".select2-input") , ' + tblName + '-search-filter :input[type!="text"]').on('change', function () {
                        //alert("B-" + listItem + "-" + $(this).val() + "-" + $(this).closest('th').index() + "-" + $(this).closest('th'))
                        tbl
                            .column($(this).closest('th').index())
                            .search($(this).val())
                            .draw();
                    });

                    $(tblName + '-search-filter :input.datepicker').datepicker('option', 'onClose', function () {
                        var currentValue = this.value;
                        var name = $(this).attr('name')//.replace(".Date","");
                        var dateElem = $("input[name='" + name + "'][class~='datepicker']");
                        if (dateElem.length > 0) {
                            isTime = false;
                            var timeElem = $("input[name='" + name + ".Time'][class~='timepicker']");
                            if (timeElem.length > 0) {

                                isTime = true;
                            }
                            var currentValue = dateElem.val();
                            if (isTime && currentValue != null && currentValue != '') {
                                var a = timeElem.val().trim();
                                var b = ":00"
                                var output = a.replace(" ", ":00 ");
                                currentValue = dateElem.val() + " " + output;

                            }
                        }
                        tbl
                            .column($(this).closest('th').index())
                            .search(currentValue)
                            .draw();
                        $(this).data('datepicker').inline = false;
                        if (typeof ExtendedFilterMethod == 'function') {
                            ExtendedFilterMethod();
                        }
                    });


                    $(tblName + '-search-panel :input.datepicker').datepicker('option', 'onClose', function () {
                        var listItem = $(this).closest('.form-group');
                        var index = $("div.form-group").index(listItem);
                        if (index >= columnCount) {
                            index = index - columnCount;
                        }

                        var currentValue = this.value;
                        var name = $(this).attr('name')//.replace(".Date","");
                        var dateElem = $("input[name='" + name + "'][class~='datepicker']");
                        if (dateElem.length > 0) {
                            isTime = false;
                            var timeElem = $("input[name='" + name + ".Time'][class~='timepicker']");
                            if (timeElem.length > 0) {

                                isTime = true;
                            }
                            var currentValue = dateElem.val();
                            if (isTime && currentValue != null && currentValue != '') {
                                var a = timeElem.val().trim();
                                var b = ":00"
                                var output = a.replace(" ", ":00 ");
                                currentValue = dateElem.val() + " " + output;

                            }
                        }

                        tbl
                            .column(index)
                            .search(currentValue)
                            .draw();
                        $(this).data('datepicker').inline = false;

                        if (typeof ExtendedFilterMethod == 'function') {
                            ExtendedFilterMethod();
                        }
                    });

                    $(tblName + '-search-panel select').on('change', function () {
                        //alert($(this).val())
                        //alert("B-" + listItem + "-" + $(this).val() + "-" + $(this).closest('th').index() + "-" + $(this).closest('th'))
                        var listItem = $(this).closest('.form-group');
                        var index = $("div.form-group").index(listItem);
                        if (index >= columnCount) {
                            index = index - columnCount;
                        }
                        tbl
                            .column(index)
                            .search($(this).val())
                            .draw();
                    });

                    $(tblName + '-search-panel :input[type="checkbox"]').on('click', function () {
                        //alert($(this).val())
                        //alert("B-" + listItem + "-" + $(this).val() + "-" + $(this).closest('th').index() + "-" + $(this).closest('th'))
                        var listItem = $(this).closest('.form-group');
                        var index = $("div.form-group").index(listItem);
                        if (index >= columnCount) {
                            index = index - columnCount;
                        }
                        tbl
                            .column(index)
                            .search($(this).val())
                            .draw();
                    });

                    //        
                    var realDate = new Date();
                    $(tblName + '-search-panel :input.datepickerMY').datepicker()
                        .datepicker('option', 'dateFormat', 'MM yy')
                        .datepicker('setDate', realDate)
                        .datepicker("option", "changeMonth", true)
                        .datepicker("option", "changeYear", true)
                        .datepicker("option", "showButtonPanel", true)
                        .datepicker('option', 'onClose', function (e) {
                            var listItem = $(this).closest('.form-group');

                            //function isDonePressed() {
                            //    return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                            //}
                            //if (isDonePressed()) {

                            //    $('.datepickerMY').focusout() //Added to remove focus from datepicker input box on selecting date
                            //}

                            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                            $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                            var stringMonth = (parseInt(month) + 1);
                            if ((parseInt(month) + 1) <= 9)
                                stringMonth = "0" + (parseInt(month) + 1);

                            //var val = "01" + "/" + (parseInt(month) + 1) + "/" + year;
                            var val = "01" + "/" + stringMonth + "/" + year;
                            //alert($(tblName + "-search-panel div.form-group").index(listItem) + "    ---    " + tblName )
                            var index = $("div.form-group").index(listItem);
                            if (index >= columnCount) {
                                index = index - columnCount;
                            }
                            tbl
                                .column(index)
                                .search(val)
                                .draw();

                            $('#ui-datepicker-div').removeClass('datepicMonthYear');
                            $(this).data('datepicker').inline = false;

                            if (typeof ExtendedFilterMethod == 'function') {
                                ExtendedFilterMethod();
                            }


                        })//.datepicker('setDate', new Date($(this).val()))

                    $('a.toggle-vis').on('click', function (e) {
                        e.preventDefault();

                        // Get the column API object
                        var column = table.column($(this).attr('data-column'));

                        // Toggle the visibility
                        column.visible(!column.visible());
                    });

                    //tblCollection[index] = tbl;

                    //if (conditionId == 1) {
                    //    $(tblName + ' tbody').on('click', 'tr', function () {
                    //        //$(this).toggleClass('active ');
                    //        //$(this).toggleClass('active selected ');
                    //        $(this).toggleClass('active row_active');
                    //        //$(this).toggleClass('selected row_active');
                    //        //$(this).children('td:first-child').toggleClass('selected');
                    //    });

                    //    //$(tblName + ' tbody').on('click', 'tr', function () {
                    //    //    var id = this.id;
                    //    //    var index = $.inArray(id, selected);

                    //    //    if (index === -1) {
                    //    //        selected.push(id);
                    //    //    } else {
                    //    //        selected.splice(index, 1);
                    //    //    }

                    //    //    $(this).toggleClass('selected');
                    //    //});
                    //}
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //log error
                    _showErrorMessage();
                }
            });
        }, 100);
    },

    resetActivityTables: function () {
        $('.table-congif').each(function (index, value) {
            var $elem = $(this);
            $elem.attr('data-active', false)
        })
        $('.tab-pane-content').html('');
    },
};

var Error = {
    accessDenied: function (message, containerElement) {

        var container = $(".page-content");
        if (containerElement != 'undefined' && containerElement != '' && containerElement != null) {
            container = containerElement;
        }
        $.ajax({
            type: "POST",
            url: "Home/ErrorPage",
            data: {},
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                container.html("");
                var content = data.viewMarkup.replace(/ERROR_MESSAGE/g, message);
                var viewMarkup = $(content);
                viewMarkup = viewMarkup.find("#page-view").html();
                if (viewMarkup != null) {
                    container.html(content);
                }
                else {
                    container.html(content);
                }
                $(".bootbox").remove();
                $(".modal-backdrop").remove();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                _showErrorMessage();
            }
        });
    },
    logError: function (args) {
        $.ajax({
            type: 'POST',
            url: 'Home/LogJavaScriptError',
            data: { message: args }
        });
    },
    redirectToErrorPage: function (xhr, ajaxOptions, thrownError) {
        //$.ajax({
        //    type: 'POST',
        //    url: 'Home/ErrorPage',
        //    data: {}
        //});
        //var url = this.getRelativeEndpointUrl('GenericError.htm');
        //userMessage.show(ajaxOptions, thrownError)
    },
    getRelativeEndpointUrl: function (endpoint) {

        var rootUrl = location.host;
        var i,
            splitString = function (string) {
                if ((string === null) || (string === undefined)) {
                    return '';
                }
                return string.split('/');
            },
            createUrl = function (newUrl, stringArray) {
                for (i = 0; i < stringArray.length; i += 1) {
                    if (stringArray[i].length > 0) {
                        newUrl += '/' + stringArray[i];
                    }
                }
                return newUrl;
            },
            splitRoot = splitString(rootUrl),
            splitUrl = splitString(endpoint),
            result = '';

        if (!endpoint) {
            return '';
        }

        if (endpoint.indexOf(rootUrl || '') === 0) {
            return endpoint;
        }

        result = createUrl(result, splitRoot);
        result = createUrl(result, splitUrl);

        return result;
    }
};

function _showErrorMessage() {
}

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('#validation-form input[name=__RequestVerificationToken]').val();
    return data;

};

(function ($) {

    if (!$('#isGuest').val()) {
        $.ajax({
            type: "POST",
            url: "Navigation/GetCalendarItem",
            data: {},
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                calendarSett = res;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //log error
                _showErrorMessage();
            }
        });
    }

    var container = $(".main-content");
    var pageContent = $('.page-content');
    var ajaxIsCalled = 0;
    var _ajaxCalls = 0;
    var isAjaxPostType = false;
    var _progressBar = null;

    var progress = (function () {
        var init = function () {
            _initializeProgress();
            _bindEvent();
        },
            _initializeProgress = function () {
                var body = $(document).height();
                var navBar = $("#navbar").height();
                var height = body - navBar;

                _progressBar = $('<div id="waitPop" style="height: 100%;width:100%;position: absolute;left: 0;background: rgba(0,0,0,.3);z-index: 100000;display:none;padding: 0;padding-bottom: 52px;min-height: 500px;">'
                    + '<h2 style="margin-left:40%;color:#fff;margin-top:25%"><b>Loading </b><img src="Content/img/loading.gif"></h2></div>');
                $('#main').prepend(_progressBar);

                _checkingProgressBar = $('<div style="height: 100%;width:100%;position: absolute;left: 0;background: rgba(0,0,0,.3);z-index: 100000;display:none;padding: 0;padding-bottom: 52px;min-height: 500px;">'
                    + '<h2 style="margin-left:40%;color:orange;margin-top:60px"><b>Checking & Validating </b><img src="Content/img/loading.gif"></h2></div>');
                $('#main').prepend(_checkingProgressBar);
            },
            _bindEvent = function () {
                //Ajax starts
                $(document).ajaxStart(function () {
                    isAjaxPostType = false;
                });

                $(document).ajaxSend(function (event, xhr, settings) {

                    if (settings.type == 'POST') {

                        isAjaxPostType = true;
                        ajaxIsCalled = 1;
                        _ajaxCalls++;
                        if (_ajaxCalls > 0 && settings.hideLoading != true) {
                            _progressBar.show();
                        } else if (settings.isChecking == true) {
                            _checkingProgressBar.show();
                        }
                    }
                });

                //ajax completed
                $(document).ajaxComplete(function (e, er, ty) {
                    var _requestedURL = ty.url;
                    if (_requestedURL != null && _requestedURL != 'undefined'
                        && ty.hideLoading != true) {
                        if (ty.type == 'POST' && _requestedURL.toLowerCase().indexOf("renderaction") === -1) {
                            console.log(_requestedURL)
                            console.log(ty.type)
                            _checkingProgressBar.hide();
                            //ajaxIsCalled = 2;
                            //_ajaxCalls--;
                            //if (_ajaxCalls < 0) _ajaxCalls = 0;
                            //if (_ajaxCalls == 0) {
                            _progressBar.hide();
                            //}
                        }
                    }
                });
                //ajax completed
                $(document).ajaxSuccess(function () {
                    // console.log('ajaxSuccess')
                });

                $(document).ajaxError(function (ev, jqXHR, settings, errorThrown) {
                    ev.stopPropagation();
                    _checkingProgressBar.hide();
                    if (settings.hideError == true) {
                        return;
                    }
                    if (jqXHR.status === 401)//|| jqXHR.status === 302) 
                    {
                        if (!$('#isGuest').val()) {
                            userMessage.show("Warning", "Your session is expired. Please login again.");
                            setTimeout(function () {
                                window.location.replace("");
                            }, 3000);
                        }
                    }
                    else {
                        var responseText = null;
                        try {

                            if (jqXHR != null) {
                                var jsonContext = JSON.parse(jqXHR.responseText);
                                if (jsonContext != null && jsonContext != undefined) {
                                    responseText = jsonContext.Message;
                                }
                            }
                            if (responseText == null) {
                                var dom_nodes = $($.parseHTML(jqXHR.responseText));
                                responseText = dom_nodes.filter('title').text();
                                if (responseText == '' || responseText == 'undefined') {
                                    responseText = errorThrown;
                                }
                            }
                        } catch (e) {
                            responseText = errorThrown;
                        }

                        _ajaxCalls--;
                        if (_ajaxCalls < 0) _ajaxCalls = 0;
                        if (_ajaxCalls == 0) {
                            _progressBar.hide();
                        }

                        userMessage.show("Error", responseText);
                        _progressBar.hide();
                    }
                });

                $(document).ajaxStop(function () {
                    //console.log('3')
                    if (isAjaxPostType) {
                        isAjaxPostType = false;
                        //console.log("ajaxStop   ---" + new Date())
                        if (ajaxIsCalled == 2 && reloadCurrentPage && reloadSectionStatus == "Success")
                            ReloadPageContainer();
                        else
                            ClearReloadParameters();

                        ajaxIsCalled = 0;
                    }

                });

                window.onerror = function (msg, url, line, col, error) {
                    _progressBar.hide();
                    userMessage.show("Error", msg);
                };

            },
            _showProgress = function () {


            },
            _hideProgress = function () {
                $(".bootbox").remove();
                $(".modal-backdrop").remove();
            },
            _showErrorMessage = function () {
                // log the error and show model popup
            };

        //PUBLIC API
        return {
            init: init
        };
    })();

    progress.init();
})(jQuery);

