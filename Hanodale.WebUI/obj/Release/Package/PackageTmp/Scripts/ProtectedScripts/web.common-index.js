$(document).ready(function () {
        $('.table-congif').each(function (index, value) {
            var $elem = $(this);

            //var val = $(".mainPart[value='" + $elem.val() + "']").data('container')
            var val = $elem.data('container')
            if (container == null || val == 'undefined') {
                container = $(".page-content");
            } else {
                var container = $(val)
            }

            var editElement;

            //var mainPart = $(".mainPart[value='" + $elem.val() + "']").val();
            var mainPart = $elem.val();
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
                        var conditionType = $elem.attr('data-conditionType');
                        var conditionId = $elem.attr('data-conditionId');
                        var filterId = $elem.attr('data-filterId');
                        var displayAll = $elem.attr('data-disAll');
                        var displayLength = $elem.attr('data-displayLength') != null && $elem.attr('data-displayLength') != 'undefined' ? $elem.attr('data-displayLength') : 10;
                        if ($elem.attr('data-readOnly') != null && $elem.attr('data-readOnly') != 'undefined') {
                            readOnly = $elem.attr('data-readOnly');
                        }
                        var hideButton = $elem.data('hidebtn');
                        var sort_index = $elem.data('sort-index');
                        var autoHideColumn = $elem.data('auto-hide-column');
                        var export_btn = $elem.data('export-btn');
                        var recordId = $elem.data('record-id');
                        var scrollX = $elem.attr('data-scroll-X');
                        var destinationTbl = $elem.attr('data-destination-table');
                        var stockInSpare = $elem.attr('data-stockInSpare');
                        var scrollY = $elem.attr('data-scroll-Y');
                        var hasCheckBox = $elem.attr('data-has-checkbox');
                        var minButtonWide = $elem.attr('data-min-button-wide');

                        navigate.loadTable(partName, hasButton, hasCondition, columnCount, extendedEventBtn, conditionType, conditionId, filterId, displayAll, readOnly, hideButton, sort_index, autoHideColumn, export_btn, recordId, scrollX, destinationTbl, stockInSpare, scrollY, hasCheckBox, minButtonWide, displayLength);


                    }
                },

                _initAction = function (partName) {

                    //View Entity
                    $('.view-' + partName).bind("click", function () {
                        if (typeof $('#readonly').val() != 'undefined') {
                            readOnly = $('#readonly').val();
                        }
                        else {
                            readOnly = true;
                        }
                        var id = $(this).attr("data-url")
                        //readOnly = true;
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



                    //Maintenance View Entity
                    $('.maintenance-view-' + partName).bind("click", function () {
                        console.log(65)
                        var id = $(this).attr("data-url")
                        if (id) {
                            var actionUrl = partName + "/Maintenance/";
                            var param = { id: id, readOnly: true }
                            navigate.viewByParameter(actionUrl, param);
                        }
                    });

                    //Maintenance Entity
                    $('.maintenance-' + partName).bind("click", function () {
                        var id = $(this).attr("data-url")
                        if (id) {
                            var actionUrl = partName + "/Maintenance/";
                            var param = { id: id, readOnly: readOnly }
                            navigate.viewByParameter(actionUrl, param);
                        }
                    });

                    //Callback Entity
                    $('.callback-' + partName).bind("click", function () {
                        var id = $(this).attr("data-url")
                        if (id) {
                            var actionUrl = partName + "/CallBack/";
                            var param = { id: id, readOnly: readOnly }
                            navigate.viewByParameter(actionUrl, param);
                        }
                    });


                    //Delete Entity
                    $('.delete-' + partName).bind("click", function () {
                        var id = $(this).attr("data-url")
                        if (id) {
                            deleteEntity(partName, id);
                        }
                    });

                    //Callback Entity
                    $('.followUp-' + partName).bind("click", function () {
                        var id = $(this).attr("data-url")
                        var message = $('#alert_followup').val();
                        if (id) {
                            var actionUrl = partName + "/FollowUp/" + id;
                            navigate.orderCallBack(actionUrl, deleteCallBack, message);
                        }
                    });




                    $('.checkboxSelectable').bind("click", function () {
                        var elem = $(this).closest('tr').toggleClass('active row_active');
                    });

                    $('.checkAsAll').change(function () {
                        var elem = $(this);
                        if ($(this).is(":checked")) {
                            $('.checkboxSelectable').prop('checked', true);
                        } else {
                            //$('.checkAsAll').prop('checked', false);
                            $('.checkboxSelectable').prop('checked', false);
                        }
                    })

                    //Enable tooltips
                    $('[data-rel=tooltip]').tooltip({ boundary: 'window' });
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
                var destInner = $(this).closest('div[data-has-dest="true"]').attr("id");
                var destOuter = $(this).data("dest");
                var actionUrl = $(this).attr("data-url")
                var id = $(this).data("back");
                if ($(this).closest('div.tab-pane').length > 0) {
                    var placeId = $(this).closest('div.tab-pane').attr('id')
                    actionUrl = $('[href="#' + placeId + '"]:first').attr('data-url');
                    destInner = "d-" + placeId;
                    id = $('#tab-info').val();
                }
                var destinationElem = $(this).data("dest")

                if (actionUrl) {
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





