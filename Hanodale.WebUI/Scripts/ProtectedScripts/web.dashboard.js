
$(function () {

    var dashboard = (function () {
        var init = function () {
        },
        _showErrorMessage = function () {
            // log the error and show model popup
            Error.redirectToErrorPage();
        };

        //PUBLIC API
        return {
            init: init
        };
    })();

    //call init on page load
    dashboard.init();
});