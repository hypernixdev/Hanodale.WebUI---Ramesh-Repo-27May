
//Using the module pattern 
$(function () {

    var container = $(".page-content");
    var navigationContainer = $(".nav-list");

    $(document).on('click', '.showPDFFiles', function (e) {
        var pdfName = $(this).attr("data-pdf-name");
        window.open("PDF_Files/" + pdfName, "User Manual", "width=800,height=600,scrollbars=yes, target=_blank");
    });

    var navigate = (function () {
        var init = function () {

            //This event is fired if submenu is clicked to load the respective page based on the user rights
            $('.nav-list li a').bind("click", function () {
                var actionUrl = $(this).attr("data-url")
                if (actionUrl) {
                    //highlight selected menu
                    $('.nav-list li').removeClass("active");
                    $(this).parent().removeClass("active").addClass("active");
                    $('.nav-list li').find(".open").addClass("active");

                    //change the header name
                    //$("#header-main").text($(this).attr("data-main"));
                    //$("#header-sub").text($(this).attr("data-sub"));
                    $("#ribbon ol.breadcrumb li:not(:first)").remove();
                    $("#ribbon ol.breadcrumb").append("<li class='breadcrumb-item'>" + $(this).attr("data-main") + "</li>");
                    if ($(this).attr("data-sub") != "")
                        $("#ribbon ol.breadcrumb").append("<li class='breadcrumb-item'>" + $(this).attr("data-sub") + "</li>");
                    navigate.navigateUrl(actionUrl);
                    
                    if ($('body').hasClass('mobile-view-activated')) {
                        $('a[data-action="toggleMenu"]').trigger('click');
                    }
                }
            });

            //Change password
            $("#change-password").bind("click", function () {

                var actionUrl = $(this).attr("data-url");
                navigate.navigateUrl(actionUrl);
            });

            //Profile
            $("#profile").bind("click", function () {

                var actionUrl = $(this).attr("data-url");
                navigate.navigateUrl(actionUrl);
            });

            //Online help
            $("#online-help").bind("click", function () {
                var actionUrl = $(this).attr("data-url");
                navigate.navigateUrl(actionUrl);
            });
        },
        navigateUrl = function (actionUrl) {
            //load view without postback
            //console.log(1+"  ---  "+new Date())
            $.ajax({
                type: "POST",
                url: actionUrl,
                data: {},
                cache: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //console.log(2 + "  ---  " + new Date())
                    if (typeof ExtendedFilterMethod == 'function') {
                        ExtendedFilterMethod = null;
                    }
                    
                    if (data.status == "Denied") {
                        Error.accessDenied(data.message);
                    }
                    else {
                        if (data.viewMarkup != null) {
                            container.html("");
                            var viewMarkup = $(data.viewMarkup);
                            viewMarkup = viewMarkup.find("#page-view").html();
                            if (viewMarkup != null) {
                                container.html(viewMarkup);
                            }
                            else {
                                container.html(data.viewMarkup);
                            }
                            //console.log(3 + "  ---  " + new Date())
                        }
                    }

                    //Clear storage
                    tableSetting.clearStorage();

                    //initialize date range picker
                    $("ul.date-range li>a").hover(function () {
                        var id = $(this).attr("data-id");
                        dateRange.show(id);
                    });
                    $("ul.date-range li>a").bind("click", function () {
                        var id = parseInt($(this).attr("data-id"));
                        if (id != 9) {

                            dateRange.hideDateRange();
                        }
                    });
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //log error
                    _showErrorMessage();
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
            navigateUrl: navigateUrl
        };
    })();

    // bind control event if necessary
    navigate.init();

    //$("#sidebar-shortcuts").find(".btn").bind("click", function () {
    //    var actionUrl = $(this).attr("data-url");
    //    if (actionUrl) {
    //        //change the header name
    //        $("#header-main").text($(this).attr("data-main"));
    //        $("#header-sub").text($(this).attr("data-sub"));
    //        navigate.navigateUrl(actionUrl);
    //    }
    //});
});