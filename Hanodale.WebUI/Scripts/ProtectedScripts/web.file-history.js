$(function () {

    var container = $(".page-content");
    var tblfilehistroy = null;
    var filehistory = (function () {
        var init = function () {
            _loadFilehistory();
        },
			_loadFilehistory = function () {
			    var actionUrl = "Trainingstaff/RenderAction";
			    $.ajax({
			        type: "POST",
			        url: actionUrl,
			        datatype: 'json',
			        data: {},
			        contentType: "application/json; charset=utf-8",
			        success: function (data) {
			            if (tblfilehistroy) {
			                tblfilehistroy.fnDestroy();
			                //$("#tbl-timesheet").dataTable().fnDestroy();
			                // $(tbltimesheet).find('thead tr th').remove();
			                //tbltimesheet.fnDraw();
			            }
			            //bind dataTable
			            tblfilehistroy = $('#dt_trainingstaff').dataTable({
			                "iDisplayLength": 10,
			                "aLengthMenu": [[10, 20, 50, 100], [10, 20, 50, 100]],
			                "bServerSide": true,
			                "sAjaxSource": "Trainingstaff/BindTrainingstaff/",
			                "fnServerParams": function (aoData) {
			                    aoData.push(
                                    { "name": "sParam", "value": $("#history_Id").val() }
                                );
			                },
			                "bProcessing": false,
			                "autoWidth": false,
			                "aaSorting": [],
			                "aoColumns": [
                                null,
                                null,
                                null,
                                null,
                                null,
                                {
                                    "sName": "ID",
                                    "bSearchable": false,
                                    "bSortable": false,
                                    "fnRender": function (oObj) {
                                        console.log(data.viewMarkup);
                                        return data.viewMarkup.replace(/TRAININGSTAFF_ID/g, oObj.aData[5]);
                                        //var r = /<([a]*)\b[^>]*class="pencil[^>]*>(.*?)<\/\1>/gi
                                        //var rr = /<([a]*)\b[^>]*class="red[^>]*>(.*?)<\/\1>/gi
                                        //var rrr = /<([a]*)\b[^>]*class="yellow[^>]*>(.*?)<\/\1>/gi
                                        ////var chk = /<([a]*)\b[^>]*class="ace[^>]*>(.*?)<\/\1>/gi
                                        //if (oObj.aData[4] == "Submitted")
                                        //    return data.viewMarkup.replace(/\s+/g, " ").replace(r, "").replace(rr, "").replace(/TRAININGSTAFF_ID/g, oObj.aData[5]);//.replace("edit-projectTask", "hide-edit");
                                        //else
                                        //    return data.viewMarkup.replace(/\s+/g, " ").replace(rrr, "").replace(/TRAININGSTAFF_ID/g, oObj.aData[5]);//.replace("edit-projectTask", "hide-edit");
                                        ////return data.viewMarkup.replace(/\s+/g, " ").replace(/TIMELOG_ID/g, oObj.aData[4]);//.replace("edit-projectTask", "hide-edit");
                                        ////return data.viewMarkup.replace(/TIMELOG_ID/g, oObj.aData[4]);
                                    }
                                }
			                ],
			                "fnDrawCallback": function (oSettings) {
			                    _initAction();
			                },
			                "oLanguage": {
			                    "sLengthMenu": $("#DATATABLE_MENU").val(),
			                    "sInfo": $("#DATATABLE_INFO").val(),
			                    "sInfoEmpty": $("#DATATABLE_INFO_EMPTY").val(),
			                    "sZeroRecords": $("#DATATABLE_ZERO_RECORD").val(),
			                    "sSearch": $("#DATATABLE_SEARCH").val(),
			                    "sInfoFiltered": $("#DATATABLE_INFO_FILTERED").val(),
			                    "sEmptyTable": $("#DATATABLE_EMPTY_TABLE").val()
			                }
			            });
			        },
			        error: function (jqXHR, ajaxOptions, thrownError) {
			            //log error
			            alert(thrownError);
			            _showErrorMessage();
			        }
			    });
			},
			 _initAction = function () {

			     $('.view-TimeLog').bind("click", function () {
			         var actionUrl = $(this).attr("data-url")
			         if (actionUrl) {
			             viewTimeSheet(actionUrl);
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
            loadFilehisotry: _loadFilehistory
        };
    })();
    //call init on page load
    filehistory.init();
});