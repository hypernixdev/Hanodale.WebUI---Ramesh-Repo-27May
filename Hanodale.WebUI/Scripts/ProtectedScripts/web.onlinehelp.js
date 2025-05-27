$(function () {

    var container = $(".page-content");

    var onlineHelp = (function () {

        var init = function () {

            $('.accordion').on('hide', function (e) {
                $(e.target).prev().children(0).addClass('collapsed');
            })
            $('.accordion').on('show', function (e) {
                $(e.target).prev().children(0).removeClass('collapsed');
            })
        },
        _showErrorMessage = function () {
            // log the error and show model popup
            Error.redirectToErrorPage();
        };

        //PUBLIC API
        return {
            init: init
           // showHelp: showHelp
        };
    })();

    //call init on page load
    onlineHelp.init();
});