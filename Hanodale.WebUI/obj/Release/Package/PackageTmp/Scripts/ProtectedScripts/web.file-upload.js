$(function () {
    $("#validation-form").submit(function (e) {
        if ($("#validation-form").valid()) {
            //check whether browser fully supports all File API
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

            var message = $("#MSG_CONFIRM_SAVE").val();
            var formObj = $(this);
            var formURL = formObj.attr("action");
            var formData = new FormData(this);
            var param = formData;
            submitFormWithCallBack(e, formURL, formData, formObj, message, null, null);

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

    function submitFormWithCallBack(e, formURL, formData, formObj, message, checkValidate, callback) {
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
                            var container = $(".page-content");
                            $.ajax({
                                type: "POST",
                                url: $('#reloadurl').val(),
                                data: {},
                                cache: false,
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.status == "Denied") {
                                        Error.accessDenied(data.message);
                                    } else {
                                        container.html("");
                                        container.html(data.viewMarkup);
                                    }
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    //log error
                                    Error.redirectToErrorPage();
                                }
                            });
                            //alert(data.status + "---" + data.message)
                            //userMessage.show("Success", "The Record has been Updated Successfully");
                            var obj = jQuery.parseJSON(data);
                            if (data.id != "" && data.id != null && data.id != 'undefined') {
                                if ($("#id").length > 0)
                                    $("#id").val(data.id)
                                else {
                                    $("#validation-form").prepend('<input type="hidden" id="id" value="' + data.id + '" />')
                                }
                                $('button[data-value="update"]').show();
                                $('button[data-value="maintenance"]').show();
                                $('button[data-value="save"]').html(' <i class="fa fa-save"></i> Save as a New');
                                alert($('#reloadurl').val());
                                
                            }
                            //alert(data)
                            //alert(data["status"]+"----"+ data["message"])
                            userMessage.show(obj.status, obj.message);
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


    }

    var userMessage = {
        show: function (status, msg) {
            var msgContainer = $("#message");
            userMessage.showMsg(status, msg, msgContainer);
        },
        showMsg: function (status, msg, container) {
            var msgContent = "";
            $('#main').append('<div class="divMessageBox animated fadeIn fast" id="MsgBoxBack" style="z-index:9999; left:inherit;background:rgba(0, 0, 0, 0.35)"><div class="MessageBoxContainer animated fadeIn fast" id="Msg1" style="border-left: 10px solid;"><div class="MessageBoxMiddle"><span class="MsgTitle">' + status + '!</span><p class="pText">' + msg + '</p><div class="MessageBoxButtonSection"></div></div></div></div>')
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
            if (status == "Success") {
                $('#Msg1').removeClass("alert-danger").removeClass("alert-warning").addClass("alert-success");
            }
            else if (status == "Error") {
                $('#Msg1').removeClass("alert-warning").removeClass("alert-success").addClass("alert-danger");
            }
            else if (status == "Warning") {
                $('#Msg1').removeClass("alert-danger").removeClass("alert-success").addClass("alert-warning");
            }
                // Added by Anand 
            else if (status == "Access Denied") {
                $('#Msg1').removeClass("alert-danger").removeClass("alert-success").addClass("alert-warning");
            }

            //container.css({ 'display': 'none' });
            //container.css({ 'display': 'block' });
            //container.text('');
            //container.append(msgContent);

            setTimeout(function () {
                //container.css({ 'display': 'none' });
                $('#MsgBoxBack').remove();
            }, 2000);
        }
    };
});