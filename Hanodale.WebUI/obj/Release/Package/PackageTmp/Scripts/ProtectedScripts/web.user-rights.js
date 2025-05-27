$(function () {

    var container = $(".page-content");

    var userRights = (function () {
        var init = function () {

            $("#userRole_Id").change(function () {
                if (this.value > 0) {
                    _bindRole(this.value);
                }
            });

            pageSetUp();
        },
       _bindRole = function (id) {

           var actionUrl = "UserRights/GetUserRightsByRoleId/" + id;

           var container = $(".page-content");
           $.ajax({
               type: "POST",
               url: actionUrl,
               data: {},
               cache: false,
               contentType: "application/json; charset=utf-8",
               success: function (data) {
                   container.html("");
                   container.html(data.viewMarkup);

                   //Row selection
                   $('.make-toggle-row').on('click', function () {
                       //alert('1')
                       var value = $(this).children('label:first-child').children('input:first-child').is(":checked");
                       var rowId = $(this).parents('tr').attr("data-row");
                       $('.label-toggle-switch-' + rowId).each(function (index) {
                           $(this).bootstrapSwitch('setState', value);
                       });
                   });

                   //Select All
                   $('.make-all-col').on('click', function () {
                       //alert('2')
                       var colId = $(this).attr("data-main-id");
                       var isChecked = $(this).is(":checked");
                       $('.toggle-all-' + colId).each(function (index) {
                           $(this).bootstrapSwitch('setState', isChecked);
                           var row = $(this).closest("tr").attr('data-row');
                           $('.label-toggle-switch-' + row).each(function (index) {
                               $(this).bootstrapSwitch('setState', isChecked);
                           });
                       });
                       $('.make-view-col[data-main-id="' + colId + '"]').prop("checked", isChecked);
                       $('.make-add-col[data-main-id="' + colId + '"]').prop("checked", isChecked);
                       $('.make-edit-col[data-main-id="' + colId + '"]').prop("checked", isChecked);
                       $('.make-delete-col[data-main-id="' + colId + '"]').prop("checked", isChecked);
                   });

                   //View Column  selection
                   $('.make-view-col').on('click', function () {
                       //alert('3')
                       var colId = $(this).attr("data-main-id");
                       var isChecked = $(this).is(":checked");
                       $('.toggle-view-' + colId).each(function (index) {
                           $(this).bootstrapSwitch('setState', isChecked);
                       });
                   });

                   //Add Column  selection
                   $('.make-add-col').on('click', function () {
                       //alert('4')
                       var colId = $(this).attr("data-main-id");
                       var isChecked = $(this).is(":checked");
                       $('.toggle-add-' + colId).each(function (index) {
                           $(this).bootstrapSwitch('setState', isChecked);
                       });
                   });

                   //Edit Column  selection
                   $('.make-edit-col').on('click', function () {
                       var colId = $(this).attr("data-main-id");
                       var isChecked = $(this).is(":checked");
                       $('.toggle-edit-' + colId).each(function (index) {
                           $(this).bootstrapSwitch('setState', isChecked);
                       });
                   });

                   //Delete Column  selection
                   $('.make-delete-col').on('click', function () {
                       //alert('6')
                       var colId = $(this).attr("data-main-id");
                       var isChecked = $(this).is(":checked");
                       $('.toggle-delete-' + colId).each(function (index) {
                           $(this).bootstrapSwitch('setState', isChecked);
                       });
                   });

                   //Check All of Status
                   $('.element-checkbox .toggle input').on('change', function (e, data) {
                       //alert('7')
                       var sectionId = $(this).parents('tr').attr("data-menu-id");
                       var rowId = $(this).parents('tr').attr("data-row");
                       var colId = $(this).closest('div').attr("data-column");

                       var tempColumn = -1;
                       var statusColumn = true;
                       var tempRow = -1;
                       var statusRow = true;
                       $('[name="element-checkbox-' + rowId + '"]').each(function (index) {
                           var $element = $(this);
                           var value = $element.is(":checked")
                           if (tempRow != -1) {
                               if (value != tempRow) {
                                   statusRow = false;
                                   tempRow = false;
                                   return false;
                               } else {
                                   statusRow = true;
                               }
                           } else {
                               tempRow = value;
                           }
                       });


                       $('#allCheck-' + sectionId + '-' + rowId).closest('div.toggle-all-' + sectionId).bootstrapSwitch('setState', tempRow);

                       tempColumn = -1;
                       statusColumn = true;
                       tempRow = -1;
                       statusRow = true;
                       $("tr[data-menu-id=" + sectionId + "] [data-column=" + colId + "]").each(function (index) {
                           var $element = $(this);
                           var value = $element.children('label:first-child').children('input:first-child').is(":checked")//.children('div')//.children(':input').attr("id");
                           if (tempColumn != -1) {
                               if (value != tempColumn) {
                                   statusColumn = false;
                                   tempColumn = false;
                                   return false;
                               } else {
                                   statusColumn = true;
                               }
                           } else {
                               tempColumn = value;
                           }

                       });
                       var checkBoxes = $('[name="checkbox-toggle"][data-main-id="' + sectionId + '"][data-column="' + colId + '"]')//('checked', tempColumn ? 'checked' : '')
                       checkBoxes.prop("checked", tempColumn);

                       tempColumn = -1;
                       statusColumn = true;
                       tempRow = -1;
                       statusRow = true;

                       $('input[id^="allCheck-' + sectionId + '-"').each(function (index) {
                           var $element = $(this);
                           var value = $element.is(":checked")
                           if (tempRow != -1) {
                               if (value != tempRow) {
                                   statusRow = false;
                                   tempRow = false;
                                   return false;
                               } else {
                                   statusRow = true;
                               }
                           } else {
                               tempRow = value;
                           }
                       });

                       $('input.make-all-col[data-main-id="' + sectionId + '"').prop("checked", tempRow);

                   });
               },
               error: function (xhr, ajaxOptions, thrownError) {
                   //log error
                   Error.redirectToErrorPage();
               }
           });

       },
        saveUserRights = function () {
            if ($("#validation-form").valid()) {
                var userRights = [];
                var actionUrl = "UserRights/SaveUserRights";
                var message = $("#MSG_CONFIRM_SAVE").val();
                var role_Id = $("#userRole_Id").val();
                $(".user-all-rights").each(function (index) {

                    var userRightsModel = { userRole_Id: 0, subMenu_Id: 0, canView: false, canAdd: false, canEdit: false, canDelete: false };
                    var mainMenuId = $(this).attr("data-menu-id");
                    //Get subMenu
                    var _subMenuId = $(this).find(".toggle-view-" + mainMenuId).parents('tr').attr("data-row");//.attr("data-submenu-id");

                    var _viewChk = $(this).find(".toggle-view-" + mainMenuId).find("input[type='checkbox']");
                    var _addChk = $(this).find(".toggle-add-" + mainMenuId).find("input[type='checkbox']");
                    var _editChk = $(this).find(".toggle-edit-" + mainMenuId).find("input[type='checkbox']");
                    var _deleteChk = $(this).find(".toggle-delete-" + mainMenuId).find("input[type='checkbox']");
                    userRightsModel.userRole_Id = role_Id;
                    userRightsModel.subMenu_Id = _subMenuId;
                    userRightsModel.canView = _viewChk.prop('checked') ;
                    userRightsModel.canAdd = _addChk.prop('checked');
                    userRightsModel.canEdit = _editChk.prop('checked');
                    userRightsModel.canDelete = _deleteChk.prop('checked');
                    userRights.push(userRightsModel);
                });

                navigate.saveCallBackJsonStringify(actionUrl, userRights, message, false, null);
            }
        },
         _showErrorMessage = function () {
             // log the error and show model popup
             Error.redirectToErrorPage();
         };

        //PUBLIC API
        return {
            init: init,
            saveUserRights: saveUserRights
        };
    })();

    //call init on page load
    userRights.init();

    $("#create-userRights").bind("click", function () {
        userRights.saveUserRights();
    });
});
