_initActionExtended = function (partName) {

    //Edit Entity
    $('.workorder-' + partName).bind("click", function () {
        var id = $(this).attr("data-url")
        if (id) {
            var actionUrl = "HelpDesk" + "/CreateWorkOrder/" + id;
            navigate.view(actionUrl);
        }
    });

}
$(function () {
    $('.table-congif').each(function (index, value) {
        
        var $elem = $(this);

        var val = $(".mainPart[value='" + $elem.val() + "']").data('container')
        if (container == null || val == 'undefined') {
            container = $(".page-content");
        } else {
            var container = $(val)
        }

        var editElement;

        var mainPart = $(".mainPart[value='" + $elem.val() + "']").val();
        var readOnly = false;
        var entity = (function () {
            var init = function () {
                // DO NOT REMOVE : GLOBAL FUNCTIONS! pageSetUp();
                pageSetUp();
                _loadTable();
            }

            //_deleteCallBack = function (data) {
            //    message.show(data.status, data.message);
            //    if (data.status == "Success") {
            //        _loadTable();
            //    }
            //}

            // Load Table
            _loadTable = function (action) {


                if ($elem.attr('data-active') != null && $elem.attr('data-active') == 'false') {

                } else {
                    var partName = $elem.val();
                    var columnCount = $elem.data('columncount');
                    var hasButton = $elem.data('button');
                    var hasCondition = $elem.data('condition');
                    var extendedEventBtn = $elem.attr('data-Extended-Event-Btn');
                    var conditionId = $elem.attr('data-conditionId');
                    var filterId = $elem.attr('data-filterId');
                    var displayAll = $elem.attr('data-disAll');
                    if ($elem.attr('data-readOnly') != null && $elem.attr('data-readOnly') != 'undefined') {
                        readOnly = $elem.attr('data-readOnly');
                    }
                    var hideButton = $elem.data('hidebtn');
                    var sort_index = $elem.data('sort-index');
                    var autoHideColumn = $elem.data('auto-hide-column');
                    var export_btn = $elem.data('export-btn');
                    navigate.loadTable(partName, hasButton, hasCondition, columnCount, extendedEventBtn, null, conditionId, filterId, displayAll, readOnly, hideButton, sort_index, autoHideColumn, export_btn);

                }
            },

            _initAction = function (partName) {
                //View Entity
                $('.view-' + partName).bind("click", function () {
                    var id = $(this).attr("data-url")
                    readOnly = true;
                    var destInner = $(this).closest('div.tab-pane-content').attr("id")
                    if (id) {
                        updateEntity(partName, id, destInner);
                    }
                });

                //Edit Entity
                $('.edit-' + partName).bind("click", function () {
                    var id = $(this).attr("data-url")
                    //var destInner = $(this).closest('div[data-has-dest="true"]').attr("id")
                    var destInner = $(this).closest('div.tab-pane-content').attr("id")
                    if (id) {
                        updateEntity(partName, id, destInner);
                    }
                });



                //Delete Entity
                $('.delete-' + partName).bind("click", function () {
                    var id = $(this).attr("data-url")
                    if (id) {
                        deleteEntity(partName, id);
                    }
                });



                $('.view-workorderlink').bind("click", function () {
                    var id1 = $(this).attr("data-id")
                    if (id1) {
                        var actionUrl = "WorkOrder" + "/Maintenance?id=" + id1 + "&readOnly=true";
                        navigate.view(actionUrl);
                    }
                });

          

                //Callback Entity For Purchase Request
                $('.followup-' + partName).bind("click", function () {
                    var id = $(this).attr("data-url")
                    if (id) {
                        var actionUrl = partName + "/CreateWorkOrder/";
                        var param = { id: id}
                        navigate.viewByParameter(actionUrl, param);
                    }
                });

                $('.check-' + partName).bind("click", function () {
                    var id = $(this).attr("data-url")
                    var message = $("#MSG_CONFIRM_SAVE").val();
                    if (id) {
                        var actionUrl = partName + "/Check/";
                        var param = { id: id, readOnly: readOnly }
                        navigate.viewByParameter(actionUrl, param);
                    }
                });

                $('#confirm-' + partName).on("click", function () {
                    var message = $("#MSG_CONFIRM_SAVE").val();
                    var actionUrl = "StockMovementSpare/Confirm/";
                    var id = $(this).attr("data-url")
                    if (id) {
                        var param = {
                            stockMovementSpareID: id
                        };
                        navigate.saveCallBackRedirect(actionUrl, param, message, false, editCallBack);
                    }
                });


                //Enable tooltips
                $('[data-rel=tooltip]').tooltip({ boundary: 'window' });
            },
            //Save RFQMaster Entity BY SURYA
              editCallBack = function (data) {
                  //message.show(data.status, data.message);
                  if (data.status == "Success") {
                      //_loadTable();
                      var tblName = $elem.val();
                      var destinationTbl = $elem.attr('data-destination-table');
                      if (destinationTbl != null && destinationTbl != "undefined") {
                          tblName = '#dt_' + destinationTbl;
                      }
                      $('#dt_' + tblName).dataTable()._fnAjaxUpdate();
                  }
              },

            deleteCallBack = function (data) {
                //message.show(data.status, data.message);
                if (data.status == "Success") {
                    //_loadTable();
                    var tblName = $elem.val();
                    var destinationTbl = $elem.attr('data-destination-table');
                    if (destinationTbl != null && destinationTbl != "undefined") {
                        tblName = '#dt_' + destinationTbl;
                    }
                    $('#dt_' + tblName).dataTable()._fnAjaxUpdate();
                }
            },

            deleteEntity = function (partName, id) {
                var actionUrl = partName + "/Delete/" + id;
                navigate.deleteCallBack(actionUrl, deleteCallBack);
            },
            updateEntity = function (partName, id, destInner) {
                var param = { id: id, readOnly: readOnly }
                var actionUrl = partName + "/Edit/";
                var destOuter = $('.table-congif[value="' + partName + '"]').attr('data-edit-dest');
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
            },



            _showErrorMessage = function () {
                // log the error and show model popup
                Error.redirectToErrorPage();
            };

            //PUBLIC API
            return {
                init: init,
            };

        })();

        //call init on page load
        entity.init();

        //Open Create Box

        $("#create-" + mainPart + " , #groupedit-" + mainPart).bind("click", function () {
            // alert('dfsd')
            var actionUrl = $(this).attr("data-url")
            var destInner = $(this).closest('div[data-has-dest="true"]').attr("id")
            var destOuter = $(this).data("dest")

            if (actionUrl) {
                if (destInner) {
                    navigate.resetActivityTables();
                    navigate.navigateElement(actionUrl, $("#" + destInner));
                } else if (destOuter) {
                    navigate.resetActivityTables();
                    navigate.navigateElement(actionUrl, $(destOuter));
                }
                else {
                    navigate.navigateElement(actionUrl, container);
                }
            }
        });

        $('#back-' + mainPart).bind("click", function () {
            var actionUrl = $(this).attr("data-url")
            var destinationElem = $(this).data("dest")
            if (actionUrl) {
                var destInner = $(this).closest('div[data-has-dest="true"]').attr("id")
                var destOuter = $(this).data("dest")
                if (actionUrl) {
                    if (destInner) {
                        navigate.resetActivityTables();
                        navigate.navigateElement(actionUrl, $("#" + destInner));
                    } else if (destOuter) {
                        navigate.resetActivityTables();
                        navigate.navigateElement(actionUrl, $(destOuter));
                    }
                    else {
                        navigate.navigateElement(actionUrl, container);
                    }
                }
            }
        });

        $('#backTo-' + mainPart).bind("click", function () {

            var actionUrl = $(this).attr("data-url")
            var destinationElem = $(this).data("dest")
            var id = $(this).data("back");
            if (actionUrl) {
                var destInner = $(this).closest('div[data-has-dest="true"]').attr("id")
                var destOuter = $(this).data("dest")
                var param = { id: id, readOnly: readOnly }
                if (actionUrl) {
                    if (destInner) {
                        navigate.resetActivityTables();
                        navigate.navigateElementByParameter(actionUrl, param, $("#" + destInner));
                    } else if (destOuter) {
                        navigate.resetActivityTables();
                        navigate.navigateElementByParameter(actionUrl, param, $(destOuter));
                    }
                    else {
                        navigate.navigateElementByParameter(actionUrl, param, container);
                    }
                }
            }
        });

    });
});





