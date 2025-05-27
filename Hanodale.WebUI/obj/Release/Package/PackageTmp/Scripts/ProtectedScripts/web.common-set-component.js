setTimeout(function () {
    $(function () {
            
            var entityFunc = (function () {

                var init22 = function () {
                        
                    $('#changeFile').click(function () {
                        $('#filenameBox').toggle();
                        $('#fileBox').toggle();
                        if ($(this).text() == "Cancel")
                            $(this).text("Change File")
                        else
                            $(this).text("Cancel")
                    });

                    $('input[data-type="mask"]').each(function (index, value) {
                        var old = $(this).val().replace(/,/g, '');
                        var value = addCommas(old, 2);
                        var parts = value.split(".");
                        if (parts.length > 1) {
                            $(this).val(value)
                        } else {
                            if (!isFloat(old)) {
                                $(this).val("0.00");
                            }
                            else if (parts[0] != '')
                                $(this).val(value + ".00");
                        }
                    })

                    $('input[data-type="mask"]').on('keyup', function () {
                        var lastCurser = $(this).getCursorPosition();
                        var matchBefore = $(this).val().match(new RegExp(",", 'g'));
                        var commaLengthBefore = matchBefore ? matchBefore.length : 0;
                        var old = $(this).val().replace(/,/g, '');
                        var value = addCommas(old, 2);
                        var matchAfter = value.match(new RegExp(",", 'g'));
                        var commaLengthAfter = matchAfter ? matchAfter.length : 0;
                        ////console.log(parts.length)
                        $(this).val(value);
                        ////console.log(commaLengthAfter + "  ---   " + commaLengthBefore);
                        setCaretToPos($(this)[0], (lastCurser + ((commaLengthAfter - commaLengthBefore))))
                    })

                    $('input[data-type="mask"]').focus(function () {
                    }).blur(function () {
                        var old = $(this).val();
                        var value = addCommas(old, 2);
                        var parts = value.split(".");
                        if (parts.length > 1) {
                            $(this).val(value)
                        } else {
                            if (!isFloat(old)) {
                                $(this).val("0.00");
                            }
                            else if (parts[0] != '')
                                $(this).val(value + ".00");
                        }
                    });

                    function addCommas(nStr, decLength) {
                        if (!decLength)
                            decLength = 6;
                        nStr += '';
                        var x = nStr.split('.');
                        var x1 = x[0];
                        var x2 = x.length > 1 ? '.' + x[1] : '';
                        var rgx = /(\d+)(\d{3})/;
                        while (rgx.test(x1)) {
                            x1 = x1.replace(rgx, '$1' + ',' + '$2');
                        }
                        if (x2.indexOf(".") != -1) {
                            x2 = x2.substr(0, (decLength + 1));
                        }
                        return x1 + x2;
                    }

                    $('input[data-type="mask"]').keypress(function (e) {
                        var $this = $(this);
                        if (e.which != 46 && e.which != 8 && e.which != 0 && e.which != 250 && (e.which < 48 || e.which > 57)) {
                            //display error message
                            return false;
                        }

                        var text = $(this).val();
                        if ((event.which == 46) && (text.indexOf('.') == -1)) {
                            setTimeout(function () {
                                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                                }
                            }, 1);
                        }

                        if ((text.indexOf('.') != -1) &&
                            (text.substring(text.indexOf('.')).length > 2) &&
                            (event.which != 0 && event.which != 8) &&
                            ($(this)[0].selectionStart >= text.length - 2)) {
                            event.preventDefault();

                        }
                    });

                    function isFloat(ng) {
                        //
                        var n = ng.replace(/,/g, '');
                        //console.log(Number(n) == n)
                        return Number(n) == n;
                    }

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

                    $('select.multipleSelect').each(function () {$(this).select2().select2("readonly", $(this)[0].hasAttribute("readonly"));})

                    var currentCulture = "en-CA",// $("meta[name='accept-language']").attr("content"),
                         language = "en-CA";
                    
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
                    })
                   

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
                        //defaultDate: new Date(1985, 00, 01),
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
                                            $('#ui-datepicker-div').on().prepend('<div class="fc-event fc-event-skin fc-event-hori fc-event-draggable fc-corner-left fc-corner-right event ' + color + ' ui-draggable"><div class="fc-event-inner fc-event-skin"><span class="fc-event-title">' + title + '<br><span class="ultra-light">' + desc + '</span><i class="air air-top-right fa fa-check "></i></span></div></div>')
                                        }, 1);
                                        $(this).data('datepicker').inline = true;
                                    }
                                } else {

                                    if (convertedStartDate <= currentDate && currentDate <= convertedStartDate) {

                                        var title = lst[i].title;
                                        var desc = lst[i].description;
                                        var color = lst[i].color;
                                        // inst.dpDiv.find('.ui-datepicker-current-day').addClass('ui-state-highlight');
                                        setTimeout(function () {
                                            $('#ui-datepicker-div').on().prepend('<div class="fc-event fc-event-skin fc-event-hori fc-event-draggable fc-corner-left fc-corner-right event ' + color + ' ui-draggable"><div class="fc-event-inner fc-event-skin"><span class="fc-event-title">' + title + '<br><span class="ultra-light">' + desc + '</span><i class="air air-top-right fa fa-check "></i></span></div></div>')
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

                            var affect_elem = $(this).attr('data_affect_on');
                            if (affect_elem != "undefined" && affect_elem != "" && affect_elem != null) {
                                $('#' + affect_elem).datepicker("option", "minDate", currentDate)
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
                        beforeShow: function () {
                            if ($(this).hasClass('setDefaultYear')) {
                                //console.log($(this).hasClass('maxToday'))
                                var data_default_year = $(this).attr('data_default_year');
                                var moonLanding = new Date();
                                var grtYear = moonLanding.getFullYear();
                                if (data_default_year != null && data_default_year != undefined) {
                                    
                                    grtYear = parseInt(data_default_year);
                                    //var grtMonth = 1;
                                    //var grtDay = 1;
                                    //var res = data_default_year.split('/');
                                    //if (res != null) {
                                    //    grtYear = res[0];
                                    //    if (res[1] != null && res[1] != '') {
                                    //        grtMonth = res[1];
                                    //        if (res[2] != null && res[2] != '') {
                                    //            grtDay = res[2];
                                    //        }
                                    //    }
                                    //}

                                    $(this).datepicker('option', 'defaultDate', new Date(grtYear, 1, 1));
                                    $(this).removeClass('setDefaultYear')
                                    ////startDate: new Date(),      // set a minimum date
                                    ////endDate: Infinity,
                                    ////endDate: new Date(),          // set a maximum date 
                                    //maxDate: new Date(),//($(this).hasClass('maxToday')? new Date(): new Date(year + 100, month, day))
                                }
                            }

                            var tomorrow = new Date();
                            tomorrow.setDate(tomorrow.getDate() + 1);
                            var start_now_elem = $(this).attr('data_start_now');

                            if (start_now_elem == "True" || start_now_elem == "true" || start_now_elem == true) {
                                $(this).datepicker("option", "minDate", tomorrow)
                            }

                            var start_max_now_elem = $(this).attr('data_start_max_now');
                            if (start_max_now_elem == "True" || start_max_now_elem == "true" || start_max_now_elem == true) {
                                $(this).datepicker("option", "maxDate", new Date())
                            }

                            return [true];
                        },
                        beforeShowDay: function (date) {

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
                    })

                    var imgSrc = $('#imagePreview').attr('src');
                    var imgPath = $('#URLPath').val();
                    var fileStyle = $('#filenameBox').css('display')

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
                        if (e.which != 46 && e.which != 8 && e.which != 0 && e.which != 250 && (e.which < 48 || e.which > 57)) {
                            //display error message
                            return false;
                        }
                    });

                    //if (readOnly == true || readOnly == 'True') {
                    //    $('input, select, checkbox, textarea').attr("disabled", "disabled");//.addClass("form-label fl col-md-10")
                    //    $('label[for]').parent().removeClass('required');
                    //    $('.smart-form .select i').css('box-shadow', 'none');

                    //    setTimeout(function () {
                    //        // $('input[type="checkbox"]').attr("disabled", true);
                    //        $("#content").find('input[type="checkbox"]').css("background-color", "red");
                    //        $(".bootstrap-duallistbox-container").find("*").prop("disabled", true);
                    //    }, 0);
                    //}

                    $('input[data-val-length-max]').keypress(function (e) {
                        var value = $(this).attr('data-val-length-max');
                        if (value < $(this).val().length) {
                            //display error message
                            return false;
                        }
                    });
                }

                //PUBLIC API
                return {
                    init22: init22
                };
            })();

            entityFunc.init22();

        pageSetUp();
    });
}, 200);