$(function () {

    var container = $(".page-content");

    var userProfile = (function () {
        var init = function () {
            _createProfile();
        },
       _createProfile = function () {
           $(".chosen-select").chosen();
       },
        updateProfile = function () {

            var message = $("#alert-update").val();

            bootbox.confirm(message, function (result) {
                if (result) {
                    var lang = $("#language").val();
                    var UserModel = {
                        language: lang
                    };
                    $.ajax({
                        url: "User/UpdateProfile",
                        datatype: 'json',
                        type: 'POST',
                        data: JSON.stringify(UserModel),
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            userMessage.show(data.status, data.message);
                           // window.location.reload();
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //log error
                            _showErrorMessage();
                        }

                    });
                }
            });
        },
        
        _showErrorMessage = function () {
            // log the error and show model popup
            Error.redirectToErrorPage();
        };

        //PUBLIC API
        return {
            init: init,
            updateProfile: updateProfile
        };
    })();
 
    //call init on page load
    userProfile.init();

    $("#update-profile").bind("click", function () {
        userProfile.updateProfile();
    });
});