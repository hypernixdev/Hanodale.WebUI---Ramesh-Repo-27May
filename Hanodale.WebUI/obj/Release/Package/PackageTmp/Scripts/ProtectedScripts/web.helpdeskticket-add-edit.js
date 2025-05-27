$(function () {
    var container = $(".page-content");
    var $elem = $('#form-congif')
    var partName = $elem.val();
    var readOnly = false;
    var firstElem = null;
    var funcName;
    var counter = 0;

    var entityFunc = (function () {
        var init = function () {
            var funcName = $('#save-' + partName).data('sp-func');

            if ($elem.attr('data-readOnly') != null || $elem.attr('data-readOnly') != 'undefined')
                readOnly = $elem.attr('data-readOnly');

            $("#save-" + partName + " , #update-" + partName).bind("click", function () {
                if (funcName == null || funcName == 'undefined')
                    funcName = partName;
                var e = $(this).attr('data-value');
                entityFunc.saveEntity(e, funcName);
            });


            $("#approve-" + partName).bind("click", function () {
                if (funcName == null || funcName == 'undefined')
                    funcName = partName;
                var e = $(this).attr('data-value');
                entityFunc.saveApprovalEntity(e, funcName);
            });

            $("#autoapprove-" + partName).bind("click", function () {
                if (funcName == null || funcName == 'undefined')
                    funcName = partName;
                var e = $(this).attr('data-value');
                entityFunc.saveAutoApprovalEntity(e, funcName);
            });


            $("#view-" + partName + " , #maintenance-" + partName).bind("click", function () {
                var id = $(this).attr("data-url")

                if (id) {
                    var actionUrl = partName + "Maintenance" + "/Index/";
                    var param = { id: id, readOnly: readOnly }
                    navigate.viewByParameter(actionUrl, param);
                } else {
                    var idm = $("#id").val();
                    if (idm) {
                        var actionUrl = partName + "/Maintenance/";
                        var param = { id: idm, readOnly: readOnly }
                        navigate.viewByParameter(actionUrl, param);
                    }
                }

            });


            $('#back-' + partName).bind("click", function () {
                var actionUrl = $(this).attr("data-url")
                var destInner = $(this).closest('div[data-has-dest="true"]').attr("id")
                var destOuter = $(this).data("dest")
                if (actionUrl) {
                    if (destInner) {
                        navigate.resetActivityTables();
                        navigate.navigateElement(actionUrl + "/" + $("#tab-info").val(), $("#" + destInner));
                    } else if (destOuter) {
                        navigate.resetActivityTables();
                        navigate.navigateElement(actionUrl, $(destOuter));
                    }
                    else {
                        navigate.navigateElement(actionUrl, container);
                    }
                }
            });

            $('#backTo-' + partName).bind("click", function () {
                var actionUrl = $(this).attr("data-url")
                var destInner = $(this).closest('div[data-has-dest="true"]').attr("id")
                var destOuter = $(this).data("dest")
                var id = $(this).data("back");
                var readOnlyStatus = readOnly;
                if ($('#tab-info').attr('data-readOnly') == 'False' || $('#tab-info').attr('data-readOnly'))
                    readOnlyStatus = $('#tab-info').attr('data-readOnly');
                var param = { id: id, readOnly: readOnlyStatus }
                if (actionUrl) {
                    if (destInner) {
                        navigate.resetActivityTables();
                        navigate.navigateElementByParameter(actionUrl + "/" + $("#tab-info").val(), param, $("#" + destInner));
                    } else if (destOuter) {
                        navigate.resetActivityTables();
                        navigate.navigateElementByParameter(actionUrl, param, $(destOuter));
                    }
                    else {
                        navigate.navigateElementByParameter(actionUrl, param, container);
                    }
                }
            });

            //$('.syncElem').keyup(function (i, oldText) {
            //    var id = $(this).attr('id');
            //    var txt = $(this).val();
            //    $("[name='" + id + "']").val(txt);
            //});



            $('#changeFile').click(function () {
                $('#filenameBox').toggle();
                $('#fileBox').toggle();
                if ($(this).text() == "Cancel")
                    $(this).text("Change File")
                else
                    $(this).text("Cancel")
            });

            //$('*[data-val-required]').each(function () {
            //    $(this).closest('div.form-group').children(':first-child').children('label:first-child').addClass('required')
            //})

            $('*[data-val-required]:not(".form-label")').each(function () {
                $(this).closest('div.form-group').children(':first-child').addClass('required')
            })

            $('.no-required').each(function () {
                $(this).closest('div.form-group').children(':first-child').removeClass('required')
            })

            $('input[data-type="currency"]').each(function () {
                var old = $(this).val().replace(/,/g, '');
                var value = addCommas(old);
                $(this).val(value)
            })

            $('.multipleSelect').each(function () {$(this).select2().select2("readonly", $(this)[0].hasAttribute("readonly"));})

            //$('.datepicker').datepicker({
            //    //showOn: "button",
            //    dateFormat: 'dd/mm/yy'
            //    //buttonImage: '/content/images/calendarIcon.png',
            //    //buttonImageOnly: true,
            //    //constrainInput: false
            //});

            var currentCulture = "en-CA",// $("meta[name='accept-language']").attr("content"),
                 language = "en-CA";
            // Set Globalize to the current culture driven by the meta tag (if any)
            //if (currentCulture) {
            //    language = (currentCulture in $.fn.datepicker.dates)
            //        ? currentCulture //a language exists which looks like "zh-CN" so we'll use it
            //        : currentCulture.split("-")[0]; //we'll try for a language that looks like "de" and use it if it exists (otherwise it will fall back to the default)
            //}





            //Initialise any date pickers
            $(function () {
                realDate = new Date();
                // months are 0-based!
                var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                $('.datepickerMY').datepicker() // format to show
                .datepicker('option', 'dateFormat', 'MM yy')
                //.datepicker('setDate', $(this).val())
                .datepicker("option", "changeMonth", true)
                .datepicker("option", "changeYear", true)
                .datepicker("option", "showButtonPanel", true)
                .datepicker("option", "beforeShow", function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')
                    if ((datestr = $(this).val()).length > 0) {
                        datestr = datestr.split(" ");
                        year = datestr[1];
                        month = monthNames.indexOf(datestr[0]);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
                        $(this).datepicker('setDate', new Date(year, month, 1));
                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');
                        $(".ui-datepicker-calendar").hide();
                    }
                    $('#ui-datepicker-div').addClass('datepicMonthYear');

                }).datepicker("option", "onChangeMonthYear", function (e) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');
                    $(this).focusout()
                })
                .datepicker('setDate', new Date($(this).val())).trigger('change');
                //$('.datepickerMY').datepicker({
                //    dateFormat: "MM yy"
                //}) // format to show
                //.datepicker('setDate', new Date())
                //.datepicker("option", "changeMonth", true)
                //.datepicker("option", "changeYear", true)
                //.datepicker("option", "showButtonPanel", true)
                //.datepicker("option", "beforeShow", function (e) {
                //    $('#ui-datepicker-div').addClass('datepicMonthYear');
                //}).datepicker("option", "onChangeMonthYear", function (dateText, inst) {
                //    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                //    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                //    $(this).datepicker("setDate", new Date(year, month, 1));
                //})//.datepicker("option", "onClose", function (e) {
                ////    $('#ui-datepicker-div').removeClass('datepicMonthYear');
                ////})
            })
            //$('.datepickerMY').datepicker({
            //    language: language,
            //    changeMonth: true,
            //    changeYear: true,
            //    //inline: true,
            //    showButtonPanel: true,
            //    dateFormat: 'MM yy',
            //    //setDate:new Date(),
            //    //defaultDate: new Date(),
            //    onChangeMonthYear: function (dateText, inst) {
            //        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            //        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            //        $(this).datepicker('setDate', new Date(year, month, 1));
            //    },
            //    beforeShow: function () {
            //        $('#ui-datepicker-div').addClass('datepicMonthYear');
            //    },

            //}).datepicker('setDate', new Date())

            $('.datepicker').datepicker({
                language: language,
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                //timeFormat: 'hh:mm p',
                maskInput: false,           // disables the text input mask
                pickDate: true,            // disables the date picker
                pickTime: true,            // disables de time picker
                pick12HourFormat: true,   // enables the 12-hour format time picker
                pickSeconds: false,          // disables seconds in the time picker
                //startDate: new Date(),      // set a minimum date
                //endDate: Infinity,
                //endDate: new Date(),          // set a maximum date 
                autoclose: true,

                onSelect: function (dateText, inst) {

                    var l = dateText.split('/');
                    var currentDate = new Date(l[2], l[1] - 1, l[0], 0, 0, 0, 0);

                    var calendarSettList = navigate.calendarSetting();
                    var lst = $.grep(calendarSettList, function (n, i) {
                        return (n.sMonth == (l[1]) || (n.eYear != 1 ? n.eMonth == (l[1]) : false));
                    });
                    for (var i = 0; i < lst.length; i++) {
                        //alert($.datepicker.formatDate("dd/mm/yy", new Date(2007, 1 - 1, 26)) + "  ---  " + lst[i].sYear + "/" + lst[i].sMonth + "/" + lst[i].sDay)
                        var convertedStartDate = new Date(parseInt(lst[i].startYear.substr(6)));
                        if (lst[i].endYear != '/Date(-62135596800000)/') {

                            var convertedendDate = new Date(parseInt(lst[i].endYear.substr(6)));
                            if (convertedStartDate <= currentDate && currentDate <= convertedendDate) {
                                var title = lst[i].title;
                                var desc = lst[i].description;
                                var color = lst[i].color;

                                setTimeout(function () {
                                    $('#ui-datepicker-div').live().prepend('<div class="fc-event fc-event-skin fc-event-hori fc-event-draggable fc-corner-left fc-corner-right event ' + color + ' ui-draggable"><div class="fc-event-inner fc-event-skin"><span class="fc-event-title">' + title + '<br><span class="ultra-light">' + desc + '</span><i class="air air-top-right fa fa-check "></i></span></div></div>')
                                }, 1);
                                $(this).data('datepicker').inline = true;
                                //inst.dpDiv.find('.ui-datepicker-current-day').addClass('ui-state-highlight');
                                //setTimeout(function () {
                                //    $(this).data('datepicker').inline = false;
                                //}, 4000);


                            }
                        } else {

                            if (convertedStartDate <= currentDate && currentDate <= convertedStartDate) {

                                var title = lst[i].title;
                                var desc = lst[i].description;
                                var color = lst[i].color;
                                // inst.dpDiv.find('.ui-datepicker-current-day').addClass('ui-state-highlight');
                                setTimeout(function () {
                                    $('#ui-datepicker-div').live().prepend('<div class="fc-event fc-event-skin fc-event-hori fc-event-draggable fc-corner-left fc-corner-right event ' + color + ' ui-draggable"><div class="fc-event-inner fc-event-skin"><span class="fc-event-title">' + title + '<br><span class="ultra-light">' + desc + '</span><i class="air air-top-right fa fa-check "></i></span></div></div>')
                                }, 1);

                                $(this).data('datepicker').inline = true;
                            }
                        }
                    }

                    if (typeof ExtendedDatePickerMethod == 'function') {
                        ExtendedDatePickerMethod();
                    } else {
                        //alert('Check yourFunctionName!');
                    }

                },
                onClose: function () {
                    $(this).data('datepicker').inline = false;
                    var id = $(this).attr('id');
                    $(this).removeClass('input-validation-error').addClass('valid')
                    $('[data-valmsg-for="' + id + '"]').removeClass('field-validation-error').addClass('field-validation-valid').html('');
                    if (($('#' + id + "_Time").val() == null || $('#' + id + "_Time").val() == '') && $('#' + id + "_Time").length > 0) {
                        if ($(this).hasClass('datepicker24Format')) {$('#' + id + "_Time").val('16:00')} else {$('#' + id + "_Time").val('04:00 PM')}
                    }

                    if (($('#' + id).val() == null || $('#' + id).val() == '') && $('#' + id).length > 0) {
                        $('#' + id + "_Time").val('')
                    }

                    if (typeof ExtendedDatePickerMethod == 'function') {
                        ExtendedDatePickerMethod();
                    }
                },

                beforeShowDay: function (date) {

                    //var y = date.getFullYear().toString(); // get full year
                    //var m = (date.getMonth() + 1).toString(); // get month.
                    //var d = date.getDate().toString(); // get Day
                    //if (m.length == 1) { m = '0' + m; } // append zero(0) if single digit
                    //if (d.length == 1) { d = '0' + d; } // append zero(0) if single digit
                    //var currDate = y + '-' + m + '-' + d;

                    var calendarSettList = navigate.calendarSetting();
                    var lst = $.grep(calendarSettList, function (n, i) {
                        return (n.sMonth == (date.getMonth() + 1) || (n.eYear != 1 ? n.eMonth == (date.getMonth() + 1) : false));
                    });
                    for (var i = 0; i < lst.length; i++) {
                        //alert($.datepicker.formatDate("dd/mm/yy", new Date(2007, 1 - 1, 26)) + "  ---  " + lst[i].sYear + "/" + lst[i].sMonth + "/" + lst[i].sDay)
                        var convertedStartDate = new Date(parseInt(lst[i].startYear.substr(6)));

                        if (lst[i].endYear != '/Date(-62135596800000)/') {

                            var convertedendDate = new Date(parseInt(lst[i].endYear.substr(6)));
                            if (convertedStartDate <= date && date <= convertedendDate) {
                                return [lst[i].allowToSelect, 'ui-' + lst[i].color];
                            }
                        } else {

                            if (convertedStartDate <= date && date <= convertedStartDate) {
                                return [lst[i].allowToSelect, 'ui-' + lst[i].color];
                            }
                        }

                    }

                    return [true];
                }
            });

            $('.timepicker:not(.timepicker24Format)').timepicker({
                minuteStep: 1,
                //template: 'modal',
                //appendWidgetTo: 'body',
                //showSeconds: true,
                //showMeridian: false,
                showMeridian: true,
                defaultTime: false,
                timeFormat: 'hh:mm p',
                //showInputs: false
            }).on('show.timepicker', function (e) {
                if (e.time.value == '') {
                    $(this).timepicker('setTime', '04:00 PM')
                }
            });

            $('.timepicker24Format').timepicker({
                minuteStep: 1,
                showMeridian: false,
                defaultTime: false,
                timeFormat: 'HH:mm',
            }).on('show.timepicker', function (e) {
                if (e.time.value == '') {
                    $(this).timepicker('setTime', '16:00')
                }
            });



            $("select").bind("change", function () {
                var id = $(this).attr('id');
                $('[data-valmsg-for="' + id + '"]').removeClass('field-validation-error').addClass('field-validation-valid').html('');
            });

            var dropdownValue = [];

            function GetArrayById(id) {
                var item = [];
                for (var i = 0; i < dropdownValue.length; i++) {
                    if (dropdownValue[i].id === id) {
                        item.push({ val: dropdownValue[i].value, data: dropdownValue[i].data })
                    }
                }
                return item;
            }

            $('select.select2-offscreen:not([data_action="resetWidgets"])').each(function (index, value) {
                dropdownValue.push({ id: $(this).attr('id'), value: $(this).select2('val'), data: $(this).select2('data') })
                //console.log($(this).select2('data'))
            })

            var imgSrc = $('#imagePreview').attr('src');
            var imgPath = $('#URLPath').val();
            var fileStyle = $('#filenameBox').css('display')
            //console.log(fileStyle)

            $("button[type='reset']").click(function (event) {

                var dis = $('button[data-value="update"]').css('display')
                if ($('button[data-value="save"]').length > 0 && $('button[data-value="update"]').length > 0 && dis == 'inline-block') {
                    var actionUrl =  partName + "/Create/";
                    var param = $(this).attr('data-value');
                    if (param)
                        actionUrl = actionUrl + param
                    var destInner = $(this).closest('div[data-has-dest="true"]').attr("id")
                    //var destOuter = $(this).data("dest")
                    var destOuter = $(this).closest('div.tab-pane-content').attr('id')
                    //var id = $(this).data("back");
                    var id = null;
                    if ($('#tab-info').val() != 'undefined')
                        id = $('#table-config').val();
                    if ($('#tab-info').val() != 'undefined')
                        id = $('#tab-info').val();

                    //if (actionUrl) {
                    //    if (destInner) {
                    //        navigate.resetActivityTables();
                    //        navigate.navigateElement(actionUrl + "/" + $("#tab-info").val(), $("#" + destInner));
                    //    } else if (destOuter) {
                    //        navigate.resetActivityTables();
                    //        navigate.navigateElement(actionUrl, $(destOuter));
                    //    }
                    //    else {
                    //        navigate.navigateElement(actionUrl, container);
                    //    }
                    //}

                    var param = { id: id, readOnly: readOnly }
                    if (actionUrl) {
                        if (destInner) {
                            navigate.resetActivityTables();
                            navigate.navigateElementByParameter(actionUrl + "/" + $("#tab-info").val(), param, $("#" + destInner));
                        } else if (destOuter) {
                            navigate.resetActivityTables();
                            navigate.navigateElementByParameter(actionUrl, param, $('#' + destOuter));
                        }
                        else {
                            navigate.navigateElementByParameter(actionUrl, param, container);
                        }
                    }


                }

                $('span.field-validation-valid').html("");
                $('span.field-validation-error').html("");
                setTimeout(function () {
                    if ($('select[name="duallistbox_demo1[]"]').length > 0)
                        $('select[name="duallistbox_demo1[]"]').bootstrapDualListbox('refresh');
                }, 10);

                $('#imagePreview').attr('src', imgSrc)
                $('#URLPath').val(imgPath);
                //$('#file').val(imgPath);
                $('#filenameBox').css('display', fileStyle)
                if (fileStyle = 'block')
                    $('#fileBox').css('display', 'none')
                else
                    $('#fileBox').css('display', 'none')

                setTimeout(function () {
                    $('#content').prepend('<input type="hidden" id="isRestBtnCalled" />')
                    $('select.select2-offscreen:not([data_action="resetWidgets"])').each(function (index, value) {
                        var id = $(this).attr('id')
                        $(this).trigger('change');
                        //$('.select2-offscreen').trigger('change');
                        //$(this).closest('form').get(0).reset();
                        //$(this).select2('val', $(this).val());
                        var item = GetArrayById(id);
                        $(this).select2('val', item[0].val);
                        //$(this).select2('data', item[0].data);
                        //$(this).select2('data', null);

                        //$('#' + id + ' option[value!=""]').remove();
                        $(this).select2();
                    })
                }, 0);

                $('#isRestBtnCalled').remove();


            })

            //if ($('.datepicker').length) {
            //    var script = document.createElement('script');
            //    script.type = 'text/javascript';
            //    script.src = '/Scripts/plugin/jquery-validate/test.js';
            //    $('#validation-form', document).append(script);
            //}

            //$('input[data-type="number"]').keypress(function (e) {
            //    //// Allow: backspace, delete, tab, escape, enter and .
            //    //if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            //    //    // Allow: Ctrl+A
            //    //    (e.keyCode == 65 && e.ctrlKey === true) ||
            //    //    // Allow: home, end, left, right, down, up
            //    //    (e.keyCode >= 35 && e.keyCode <= 40)) {
            //    //    // let it happen, don't do anything
            //    //    return;
            //    //}
            //    //// Ensure that it is a number and stop the keypress
            //    //if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            //    //    e.preventDefault();
            //    //}
            //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //        //display error message
            //        return false;
            //    }
            //});

            $('input[data-type="currency"]').on('keyup', function () {
                var old = $(this).val().replace(/,/g, '');
                var value = addCommas(old);
                $(this).val(value)
            })

            function addCommas(nStr) {
                nStr += '';
                var x = nStr.split('.');
                var x1 = x[0];
                var x2 = x.length > 1 ? '.' + x[1] : '';
                var rgx = /(\d+)(\d{3})/;
                while (rgx.test(x1)) {
                    x1 = x1.replace(rgx, '$1' + ',' + '$2');
                }
                return x1 + x2;
            }

            $('input[data-type="number"]').keypress(function (e) {
                //// Allow: backspace, delete, tab, escape, enter and .
                //if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                //    // Allow: Ctrl+A
                //    (e.keyCode == 65 && e.ctrlKey === true) ||
                //    // Allow: home, end, left, right, down, up
                //    (e.keyCode >= 35 && e.keyCode <= 40)) {
                //    // let it happen, don't do anything
                //    return;
                //}
                //// Ensure that it is a number and stop the keypress
                //if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                //    e.preventDefault();
                //}
                if (e.which != 46 && e.which != 8 && e.which != 0 && e.which != 250 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    return false;
                }
            });

            if (readOnly == true || readOnly == 'True') {
                $('input, select, checkbox, textarea').attr("disabled", "disabled");//.addClass("form-label fl col-md-10")
                //$('label[for]').addClass("control-label fl col-md-2").append('<span class="fr" style="padding: 0 5px">:</span>')
                $('label[for]').parent().removeClass('required');
                $('.smart-form .select i').css('box-shadow', 'none');

                setTimeout(function () {
                    $(".bootstrap-duallistbox-container").find("*").prop("disabled", true);
                    //$('select[name="duallistbox_demo1[]_helper1"]').attr('disabled', true);
                    //$('select[name="duallistbox_demo1[]_helper2"]').prop('disabled', true);
                    //$('select[name="duallistbox_demo1[]"]').parent().find('.moveall').prop('disabled', true);
                    //$('select[name="duallistbox_demo1[]"]').parent().find('.move').prop('disabled', true);
                    //$('select[name="duallistbox_demo1[]"]').parent().find('.removeall').prop('disabled', true);
                    //$('select[name="duallistbox_demo1[]"]').parent().find('.remove').prop('disabled', true);
                }, 0);
            }

            $('input[data-val-length-max]').keypress(function (e) {
                var value = $(this).attr('data-val-length-max');
                if (value < $(this).val().length) {
                    //display error message
                    return false;
                }
            });


            //$("#test").click(function () {
            //   // alert("Selected value is: " + $(".multipleSelect").select2("val"));

            //    var classificationIds = [];
            //    $(".multipleSelect").find(".select2-choices li").each(function () {
            //        var id = $(this).find(".select2-search-choice").val();
            //        alert(id);
            //        if (id != null) {
            //            var classificationId = $(".multipleSelect option").eq(id).val();
            //            classificationIds.push(classificationId);
            //        }
            //    });
            //  //  console.log(classificationIds);
            //})

            pageSetUp();

            //$("form button[type=submit]").click(function () {
            //    $("button[type=submit]", $(this).parents("form")).removeAttr("clicked");
            //    $(this).attr("clicked", "true");
            //});
            //_validateEntity();

            //$("#validation-form").submit(function (e) {
            //    if ($("#validation-form").valid()) {
            //        var message = $("#MSG_CONFIRM_SAVE").val();
            //        var formObj = $(this);
            //        var formURL = formObj.attr("action");
            //        var formData = new FormData(this);
            //        var param = formData;
            //        navigate.submitFormWithCallBack(e, formURL, formData, formObj, message, null, null);

            //        e.preventDefault(); //Prevent Default action. 
            //        //e.unbind();
            //    } else {
            //        setTimeout(function () { var input = $('.input-validation-error:first'); if (input) { input.focus(); scrollToElement(input); } }, 100); $('.field-validation-error').each(function (e) {
            //            var id = $(this).closest('.jarviswidget').attr('id');
            //            $('#' + id).removeClass('jarviswidget-collapsed');
            //            $('#' + id + " .jarviswidget-ctrls a.jarviswidget-toggle-btn i:first-child").removeClass("fa-plus").addClass("fa-minus");
            //            $('#' + id + " div[role='content']").css({ "display": "block" });
            //        })
            //    }
            //});

            $("form button[type=submit]").click(function () {
                $("button[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#validation-form").submit(function (e) {
                if ($("#validation-form").valid()) {
                    //check whether browser fully supports all File API
                    if ($('input[type="file"]').val() != '') {
                        if (window.File && window.FileReader && window.FileList && window.Blob) {

                            //get the file size and file type from file input field
                            var fsize = $('input[type="file"]')[0].files[0].size;
                            if (fsize > 10485760) //do something if file size more than 1 mb (1048576)
                            {
                                userMessage.show("Warning", "The current uploaded file is " + Math.round((fsize / 1048576)) + "MB. The Maximum file size is 10MB");
                                e.preventDefault();
                                return false;
                            }
                        } else {

                            // alert("Please upgrade your browser, because your current browser lacks some new features we need!");
                            userMessage.show("Warning", "Please upgrade your browser, because your current browser lacks some new features we need!");
                            e.preventDefault();
                            return false;
                        }
                    }

                    btnStatus = $("button[type=submit][clicked=true]").attr('data-value');
                    if ($("#id").length > 0)
                        if (btnStatus == 'save') {
                            if (firstElem == null)
                                firstElem = $("#id").val();
                            else
                                $("#id").val(firstElem)
                        }
                    if ($("#tempId").length > 0)
                        $("#tempId").val($("#id").val())
                    else
                        $("#validation-form").prepend('<input type="hidden" id="tempId" value="' + $("#id").val() + '" />')

                    var message = $("#MSG_CONFIRM_SAVE").val();
                    var formObj = $(this);
                    var formURL = formObj.attr("action");
                    var formData = new FormData(this);
                    var param = formData;
                    navigate.submitFormWithCallBack(e, formURL, formData, formObj, message, null, saveCallBack);
                    e.preventDefault(); //Prevent Default action. 
                    //e.unbind();
                } else {
                    setTimeout(function () { var input = $('.input-validation-error:first'); if (input) { input.focus(); scrollToElement(input); } }, 100); $('.field-validation-error').each(function (e) {
                        var id = $(this).closest('.jarviswidget').attr('id');
                        $('#' + id).removeClass('jarviswidget-collapsed');
                        $('#' + id + " .jarviswidget-ctrls a.jarviswidget-toggle-btn i:first-child").removeClass("fa-plus").addClass("fa-minus");
                        $('#' + id + " div[role='content']").css({ "display": "block" });
                    })
                }
            });

        },
         saveCallBack = function (data) {
             //message.show(data.status, data.message);
             if (data.status == "Success") {

                 //autogenerateCode
                 $('#code').val(data.code);

                 if ($elem.attr('data-red-url')) {
                     ////////////////////////////////////////////////////////////////////////////////////////////////////// changed \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                     var destInner = $('#btnSection').closest('div[data-has-dest="true"]').attr("id")
                     //var destOuter = $(this).data("dest")
                     var destOuter = $('#btnSection').closest('div.tab-pane-content').attr('id')
                     //alert(destInner + "    ---    " + destOuter)
                     var param = {
                         id: $("#id").val(),
                         readOnly: $elem.attr('data-readOnly')
                     }

                     if (destInner) {
                         //navigate.resetActivityTables();
                         navigate.navigateElementByParameter($elem.attr('data-red-url') + "/" + $("#tab-info").val(), param, $("#" + destInner));
                     } else if (destOuter) {
                         //navigate.resetActivityTables();
                         navigate.navigateElementByParameter($elem.attr('data-red-url'), param, $('#' + destOuter));
                     }
                     else {
                         navigate.navigateElementByParameter($elem.attr('data-red-url'), param, container);
                     }
                 }
                 var oldValue = $('.page-title').text();
                 if ($($elem).attr('data-remove-save')) {
                     $('button[data-value="save"]').remove();
                     var newValue = oldValue.replace("Create", "Edit");
                     $('.page-title').text(newValue);
                 } else {

                     if (oldValue.toLowerCase().indexOf("create/edit") <= 0 && oldValue.toLowerCase().indexOf("edit")) {

                         var newValue = oldValue.replace("Create", "Create/Edit");
                         $('.page-title').text(newValue);
                         //$('#headerTitle').text($('#headerCreateEdit').val())

                     }

                 }
             }
         },

       saveEntity = function (e, funcName) {
           var validator = $("#validation-form").data('validator');
           validator.settings.ignore = "";
           //validator.settings.errorPlacement = function (error, element) {
           //    alert('errorPlacement: ' + element.attr("id"));
           //};
           if ($("#validation-form").valid()) {
               setTimeout(function () {
               var message = $("#MSG_CONFIRM_SAVE").val();
               var actionUrl =  partName + "/Save" + funcName + "/";
               if ($("#id").length > 0)
                   //$("#id").data('oldValue', $("#id").val())
                   $("#id").attr('data-oldValue', $("#id").val())
                   if (e == 'save') {
                       
                       if (firstElem == null)
                           firstElem = $("#id").val();
                       else
                           $("#id").val(firstElem)
                   }
               if ($("#tempId").length > 0)
                   $("#tempId").val($("#id").val())
               else
                   $("#validation-form").prepend('<input type="hidden" id="tempId" value="' + $("#id").val() + '" />')

               //$('button[data-value="update"]').hide();
               //$('button[data-value="maintenance"]').hide();

               var param = $('#validation-form').serialize();
               param = addDatetime(param);
               navigate.saveCallBack(actionUrl, param, message, null, saveCallBack);
               }, 100);
           } else {
               setTimeout(function () { var input = $('.input-validation-error:first'); if (input) { input.focus(); scrollToElement(input); } }, 100); $('.field-validation-error').each(function (e) {
                   var id = $(this).closest('.jarviswidget').attr('id');
                   $('#' + id).removeClass('jarviswidget-collapsed');
                   $('#' + id + " .jarviswidget-ctrls a.jarviswidget-toggle-btn i:first-child").removeClass("fa-plus").addClass("fa-minus");
                   $('#' + id + " div[role='content']").css({ "display": "block" });
               })
           }

       },

        saveApprovalEntity = function (e, funcName) {
            var validator = $("#validation-form").data('validator');
            validator.settings.ignore = "";
            if ($("#validation-form").valid()) {
                setTimeout(function () {
                var message = $("#MSG_CONFIRM_SAVE").val();
                var actionUrl =  partName + "/SaveApproval" + funcName + "/";;
                if ($("#id").length > 0)
                    $("#id").attr('data-oldValue', $("#id").val())
                    if (e == 'save') { 
                        if (firstElem == null)
                            firstElem = $("#id").val();
                        else
                            $("#id").val(firstElem)
                    }
                if ($("#tempId").length > 0)
                    $("#tempId").val($("#id").val())
                else
                    $("#validation-form").prepend('<input type="hidden" id="tempId" value="' + $("#id").val() + '" />')

                //$('button[data-value="update"]').hide();
                //$('button[data-value="maintenance"]').hide();


                var param = $('#validation-form').serialize();
                param = addDatetime(param);
                navigate.saveCallBackRedirect(actionUrl, param, message, null, saveCallBack);
            }, 100);
            } else {
                setTimeout(function () { var input = $('.input-validation-error:first'); if (input) { input.focus(); scrollToElement(input); } }, 100); $('.field-validation-error').each(function (e) {
                    var id = $(this).closest('.jarviswidget').attr('id');
                    $('#' + id).removeClass('jarviswidget-collapsed');
                    $('#' + id + " .jarviswidget-ctrls a.jarviswidget-toggle-btn i:first-child").removeClass("fa-plus").addClass("fa-minus");
                    $('#' + id + " div[role='content']").css({ "display": "block" });
                })
            }

        },

         saveAutoApprovalEntity = function (e, funcName) {
             var validator = $("#validation-form").data('validator');
             validator.settings.ignore = "";
             if ($("#validation-form").valid()) {
                 setTimeout(function () {
                 var message = $("#MSG_CONFIRM_SAVE").val();
                 var actionUrl =  partName + "/SaveAutoApproval" + funcName + "/";;
                 if ($("#id").length > 0)
                     $("#id").attr('data-oldValue', $("#id").val())
                     if (e == 'save') { 
                         if (firstElem == null)
                             firstElem = $("#id").val();
                         else
                             $("#id").val(firstElem)
                     }
                 if ($("#tempId").length > 0)
                     $("#tempId").val($("#id").val())
                 else
                     $("#validation-form").prepend('<input type="hidden" id="tempId" value="' + $("#id").val() + '" />')

                 //$('button[data-value="update"]').hide();
                 //$('button[data-value="maintenance"]').hide();


                 var param = $('#validation-form').serialize();
                 param = addDatetime(param);
                 navigate.saveCallBackRedirect(actionUrl, param, message, null, saveCallBack);
             }, 100);
             } else {
                 setTimeout(function () { var input = $('.input-validation-error:first'); if (input) { input.focus(); scrollToElement(input); } }, 100); $('.field-validation-error').each(function (e) {
                     var id = $(this).closest('.jarviswidget').attr('id');
                     $('#' + id).removeClass('jarviswidget-collapsed');
                     $('#' + id + " .jarviswidget-ctrls a.jarviswidget-toggle-btn i:first-child").removeClass("fa-plus").addClass("fa-minus");
                     $('#' + id + " div[role='content']").css({ "display": "block" });
                 })
             }

         },

      addDatetime = function (param) {
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
                      //var a = timeElem.val().trim();
                      //var b = ":00"
                      ////var output = [a.slice(0, 4), b, a.slice(4)].join('');
                      //var output = a.replace(" ", ":00 ");
                      //// $("#" + name).val(dateElem.val() + " " + output)
                      //value = dateElem.val() + " " + output;


                      var is24Hours = timeElem.hasClass('timepicker24Format');
                      if (is24Hours) {
                          var a = timeElem.val().trim();
                          //console.log(a)
                          if (a != null && a != "") {
                              var output = a;
                              //var hourStr = a.substr(0, 2);
                              //var hour = parseInt(hourStr);
                              //if (hour >= 12) {
                              //    output = a.replace(" ", ":00 PM");
                              //} else {
                              //    output = a.replace(" ", ":00 AM");
                              //}
                              value = dateElem.val() + " " + output;
                          }

                      } else {
                          var a = timeElem.val().trim();
                          //var output = [a.slice(0, 4), b, a.slice(4)].join('');
                          var output = a.replace(" ", ":00 ");
                          // $("#" + name).val(dateElem.val() + " " + output)
                          value = dateElem.val() + " " + output;
                      }
                  }
              }
              date.push({ "name": name, "value": value });
          })

          date.forEach(function (entry) {
              param = param.replace(entry.name, entry.name + ".Date")

              //entry.value = entry.value.replace('/', '%2F').replace('/', '%2F').replace(' ', '+');
              entry.value = entry.value.replace('/', '%2F').replace('/', '%2F').replace(' ', '+').replace(':', '%3A').replace(' ', '+').replace(':', '%3A');
              param += "&" + entry.name + "=" + entry.value;
          });

          return param;
      },
        //_validateEntity = function () {

        //    var rules = [] ;
        //    var message = [];
        //    var lst = [];

        //    var form_data = $("#validation-form").serializeArray();
        //    var error_free = true;
        //    var counter = 0;
        //    for (var input in form_data) {
        //        var element = $("#" + form_data[input]['name']);


        //        var valid = element.attr("data-val");
        //        if (valid) {
        //            var idName = element.attr('id').trim();
        //            if (lst.indexOf(idName) == -1) {
        //                rules[counter] = {
        //                    idName: {
        //                        required: true
        //                    }
        //                }

        //                //alert(element.attr('data-val-required'))
        //                message[counter] = {
        //                    idName: element.attr('data-val-required')
        //                }

        //                //rules[counter] = item;
        //                lst[counter] = idName;
        //                //alert(element.attr("name"))
        //                counter++;

        //            }
        //            //element.parent().append('<span>wwewewee</span>')
        //            ////error_element.removeClass("error").addClass("error_show");
        //            //error_free = false;
        //        }
        //        //else {
        //        //error_element.removeClass("error_show").addClass("error");
        //    }

        //    for (var i = 0; i < counter; i++) {
        //        alert(rules[i].idName)
        //        //rules = {
        //        //    lst[i]: {
        //        //        required: true
        //        //    }

        //        //};
        //    }



        //    //rules = {
        //    //    firstName: {
        //    //        required: true
        //    //    },
        //    //    email: {
        //    //        email: true,
        //    //        required: true
        //    //    },
        //    //    passwordHash: {
        //    //        required: true
        //    //    },
        //    //    UserRoles: {
        //    //        required: true
        //    //    }
        //    //};

        //    // message = {
        //    //    firstName: $('#required').val(),
        //    //    passwordHash: $('#required').val(),
        //    //    UserRoles: $('#required').val(),
        //    //    email: {
        //    //        email: $('#invalid-email').val(),
        //    //        required: $('#required').val()
        //    //    }
        //    //};

        //    navigate.validateEntity(rules, message);

        //},

        navigateEntity = function (actionUrl) {
            navigate.view(actionUrl);
        },
        _showErrorMessage = function () {
            // log the error and show model popup
            Error.redirectToErrorPage();
        };

        //PUBLIC API
        return {
            init: init,
            saveEntity: saveEntity,
            saveApprovalEntity: saveApprovalEntity,
            saveAutoApprovalEntity: saveAutoApprovalEntity,
            navigateEntity: navigateEntity
        };
    })();

    //call init on page load
    entityFunc.init();




    //$("#validation-form").submit(function (event) {
    //    if ($(this).valid()) {
    //        alert($(this).valid())
    //    }
    //    return false;
    //    //$.ajax({
    //    //    url:  partName + "/Save" + partName,
    //    //    type: 'post',
    //    //    dataType: 'json',
    //    //    data: $('#validation-form').serialize(),
    //    //    success: function (data) {

    //    //    }
    //    //});
    //})
});