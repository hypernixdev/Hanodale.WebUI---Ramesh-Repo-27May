$(function () {

    var container = $(".page-content");
  
    var changePassword = (function () {
        var init = function () {
            _validatePassword();
            _validateForcePassword();
        },
        saveChangePassword = function () { 
            var $validation = true;
            if ($validation) {
                if (!$('#validation-form').valid())
                {
                    return false;
                }
                else {
                    passwordObj = {
                        oldPassword: $("#oldPassword").val(),
                        newPassword: $("#confirmPassword").val()
                    }

                    //Ajax post back to change the password
                    $.ajax({
                        url: "ChangePassword/UpdatePassword",
                        datatype: 'json',
                        type: 'POST',
                        data: JSON.stringify({ passwordModel: passwordObj }),
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            userMessage.show(data.status, data.message);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            }
        },
        _validatePassword = function () {
            $('#validation-form').validate({
                errorElement: 'div',
                errorClass: 'help-block',
                focusInvalid: false,
                rules: {
                    oldPassword: {
                        required: true
                    },
                    newPassword: {
                        required: true,
                        minlength: 6
                    },
                    confirmPassword: {
                        required: true,
                        minlength: 6,
                        equalTo: "#newPassword_id"
                    }
                },
                messages: {
                    oldPassword: {
                        required: $("#required").val()
                    },
                    newPassword: {
                        required: $("#required").val()
                    },
                    confirmPassword: {
                        required: $("#required").val()
                    }
                },
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
        forcePasswordChange = function () {
            var $validation = true;
            if ($validation) {
                if (!$('#validation-first-form').valid()) {
                    return false;
                }
                else {
                    passwordObj = {
                        newPassword: $("#newPassword_id").val()
                    }

                    //Ajax post back to change the password
                    $.ajax({
                        url: "ChangePassword/ForcePasswordChange",
                        datatype: 'json',
                        type: 'POST',
                        data: JSON.stringify({ passwordModel: passwordObj }),
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            userMessage.show(data.status, data.message);
                            if (data.status = "Success") {
                                bootbox.alert($("#WELCOME_ALERT").val(), function () {
                                    window.location.replace('/Dashboard');
                                });
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }
                    });
                }
            }
        },
        _validateForcePassword = function () {  
            $('#validation-first-form').validate({
                errorElement: 'div',
                errorClass: 'help-block',
                focusInvalid: false,
                rules: {
                    newPassword: {
                        required: true,
                        minlength: 6
                    },
                    confirmPassword: {
                        required: true,
                        minlength: 6,
                        equalTo: "#newPassword_id"
                    }
                },
                messages: {
                    newPassword: {
                        required: $("#required").val()
                    },
                    confirmPassword: {
                        required: $("#required").val()
                    }
                },
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
        _showErrorMessage = function () {
            // log the error and show model popup
            Error.redirectToErrorPage();
        };

        //PUBLIC API
        return {
            init: init,
            saveChangePassword: saveChangePassword,
            forcePasswordChange: forcePasswordChange
        };
    })();

    //call init on page load
    changePassword.init();

    $("#changepassword").bind("click", function () {
        changePassword.saveChangePassword();
    });

    $("#force-password").bind("click", function () {
        changePassword.forcePasswordChange();
    });
    
});


