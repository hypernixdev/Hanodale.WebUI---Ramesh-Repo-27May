/* DO NOT REMOVE : GLOBAL FUNCTIONS!
  *
  * pageSetUp(); WILL CALL THE FOLLOWING FUNCTIONS
  *
  * // activate tooltips
  * $("[rel=tooltip]").tooltip();
  *
  * // activate popovers
  * $("[rel=popover]").popover();
  *
  * // activate popovers with hover states
  * $("[rel=popover-hover]").popover({ trigger: "hover" });
  *
  * // activate inline charts
  * runAllCharts();
  *
  * // setup widgets
  * setup_widgets_desktop();
  *
  * // run form elements
  * runAllForms();
  *
  ********************************
  *
  * pageSetUp() is needed whenever you load a page.
  * It initializes and checks for all basic elements of the page
  * and makes rendering easier.
  *
  */

pageSetUp();

/*
 * ALL PAGE RELATED SCRIPTS CAN GO BELOW HERE
 * eg alert("my home function");
 * 
 * var pagefunction = function() {
 *   ...
 * }
 * loadScript("js/plugin/_PLUGIN_NAME_.js", pagefunction);
 * 
 * TO LOAD A SCRIPT:
 * var pagefunction = function (){ 
 *  loadScript(".../plugin.js", run_after_loaded);	
 * }
 * 
 * OR
 * 
 * loadScript(".../plugin.js", run_after_loaded);
 */

// PAGE RELATED SCRIPTS

// pagefunction

//var pagefunction = function () {

// full calendar
var newList = [];
var lstCalendarItems = [];
var cd = new Date();
var date = new Date($('#year').val(), cd.getMonth(), cd.getDate());
var d = date.getDate();
var m = date.getMonth();
var y = date.getFullYear();
var counter = -1;
var hdr = {
    center: 'title',
    left: 'month,agendaWeek,agendaDay',
    right: 'prev,today,next'
};

var initDrag = function (e) {
    // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
    // it doesn't need to have a start or end
    var eventObject = {
        title: $.trim(e.children().text()), // use the element's text as the event title
        description: $.trim(e.children('span').attr('data-description')),
        icon: $.trim(e.children('span').attr('data-icon')),
        className: $.trim(e.children('span').attr('class')), // use the element's children as the event class
        eventId: $.trim(e.children('span').attr('data-eventId')),
        //id: counter,
    };

    // store the Event Object in the DOM element so we can get to it later
    e.data('eventObject', eventObject);

    // make the event draggable using jQuery UI
    e.draggable({
        zIndex: 999,
        revert: true, // will cause the event to go back to its
        revertDuration: 0 //  original position after the drag
    });
};

var addEvent = function (title, priority, description, icon, eventId) {
    title = title.length === 0 ? "Untitled Event" : title;
    description = description.length === 0 ? "No Description" : description;
    icon = icon.length === 0 ? " " : icon;
    priority = priority.length === 0 ? "badge badge-dark" : priority;
    eventId = eventId.length === 0 ? "0" : eventId;

    var html = $('<li><span class="' + priority + '" data-description="' + description + '" data-eventId="' + eventId + '" data-icon="' +
        icon + '">' + title + '</span></li>').prependTo('ul#external-events').hide().fadeIn();

    $("#event-container").effect("highlight", 800);

    initDrag(html);
};

/* initialize the external events
 -----------------------------------------------------------------*/

$('.edit-calendarsetting').attr('data-url', $('#year').val())
$('.delete-calendarsetting').attr('data-url', $('#year').val())

//$('#external-events > li').each(function () {
//    initDrag($(this));

//});

$('#external-events-list > div').each(function () {
    initDrag($(this));
});

deleteCallBack = function (data) {
    var year = $('#year').val();
    if (year) {
        var param = { id: year }
        var actionUrl = "CalendarSetting/Create/";

        if (actionUrl) {
            navigate.viewByParameter(actionUrl, param);
        }
    }
},

$('.delete-calendarsetting').bind("click", function () {
    var id = $('#year').val();
    if (id) {
        var actionUrl = "CalendarSetting/Delete/" + id;
        navigate.deleteCallBack(actionUrl, deleteCallBack);
    }
});

$('#create-calendarsetting , #back-calendarsetting , #copy-calendarsetting').bind("click", function () {
    var param = {}
    var actionUrl = $(this).attr("data-url");

    if (actionUrl) {
        navigate.viewByParameter(actionUrl, param);
    }
});

$('.edit-calendarsetting').bind("click", function () {
    var year = $('#year').val();
    if (year) {
        var param = { year: year, readOnly: $('#readOnly').val() }
        var actionUrl = "CalendarSetting/Edit/";

        if (actionUrl) {
            navigate.viewByParameter(actionUrl, param);
        }
    }
});


$('#add-event').click(function () {
    var title = $('#title').val(),
        priority = $('input:radio[name=priority]:checked').val(),
        description = $('#description').val(),
        icon = $('input:radio[name=iconselect]:checked').val();

    addEvent(title, priority, description, icon);
});

$('#year').on("change", function () {
    //$('.edit-calendarsetting').attr('data-url', $('#year').val())
    //$('.delete-calendarsetting').attr('data-url', $('#year').val())

    var year = $('#year').val();
    var isEdit = $('#isEdit').val();
    
    if (year) {
        var param = { id: year }
        var actionUrl = "CalendarSetting/Index/";
        
        if (isEdit == "True" || isEdit == "true" || isEdit == "TRUE" || isEdit == true) {
            actionUrl = "CalendarSetting/Create/";
        }

        if (actionUrl) {
            navigate.viewByParameter(actionUrl, param);
        }
    }
});

$('#saveaCalendarSetting').click(function () {
    //var obj = $('#calendar').fullCalendar('clientEvents', [999]);
    var lst = [];
    var obj = newList; //$('#calendar').fullCalendar('clientEvents');

    //if (obj.length <= 0) {
    //    userMessage.show("Warning", "Please drog & drop one or more event");
    //    return false;
    //}
    for (var i = 0; i < obj.length; i++) {
        if (obj[i].id < 0) {
            obj[i].id = 0;
        }

        var item = { id: obj[i].id, calendarEvent_Id: obj[i]._def.extendedProps.eventId, startDate: obj[i].start, endDate: obj[i].end }
        //var item = { id: obj[i].id, calendarEvent_Id: obj[i].eventId, startDate: obj[i].start, endDate: obj[i].end }
        //alert(obj[i].title + " --- " + obj[i].description + " --- " + obj[i].start + " --- " + obj[i].end + " --- " + obj[i].id + " --- " + obj[i].eventId)
        lst.push(item);
    }

    var param = {
        lst: lst,
        year: date.getFullYear()
    }

    var actionUrl = "CalendarSetting/SaveCalendarEvent";
    var message = $("#MSG_CONFIRM_SAVE").val();
    navigate.saveCallBackJsonStringify(actionUrl, param, message, false, null);
    //alert(obj["title"] + " --- " + obj["description"] + " --- " + obj["start"] + " --- " + obj["end"] + " --- " + obj["id"])
    //alert(obj)
});

//alert($('#year').val())
/* initialize the calendar
 -----------------------------------------------------------------*/
$.ajax({
    type: "POST",
    url: "CalendarSetting/GetCalendarItem",
    data: JSON.stringify({ year: $('#year').val() }),
    cache: false,
    contentType: "application/json; charset=utf-8",
    success: function (res) {
        for (var i = 0; i < res.length ; i++) {
            //alert(m+"---"+ res[i].title + " --- " + res[i].description + " --- " + res[i].start + " --- " + res[i].end + " --- " + res[i].id + " --- " + res[i].eventId + " --- " + res[i].icon + " --- " + res[i].color)
            var obj = {
                id: res[i].id,
                title: res[i].title,
                description: res[i].description,
                start: new Date(res[i].sYear, res[i].sMonth - 1, res[i].sDay),
                //allDay: false,
                end: res[i].eYear < 1001 ? null : new Date(res[i].eYear, res[i].eMonth - 1, res[i].eDay),
                className: ["event", res[i].color],
                icon: res[i].icon,
                eventId: res[i].eventId


            }

            lstCalendarItems.push(obj)
        }
        //document.addEventListener('DOMContentLoaded', function () {
        var Calendar = FullCalendar.Calendar;
        var Draggable = FullCalendarInteraction.Draggable

        /* initialize the external events
        -----------------------------------------------------------------*/

        var containerEl = document.getElementById('external-events-list');
        if (containerEl) {
            new Draggable(containerEl, {
                itemSelector: '.fc-event',
                eventData: function (eventEl) {
                    return {
                        title: eventEl.innerText.trim(),
                        className: eventEl.className,
                        extendedProps: {
                            description: $(eventEl).attr('data-description'),
                            icon: $(eventEl).attr('data-icon'),
                            eventId: $(eventEl).attr('data-eventid')
                        },

                    }
                }
            });
        }

        //// the individual way to do it
        // var containerEl = document.getElementById('external-events-list');
        // var eventEls = Array.prototype.slice.call(
        //   containerEl.querySelectorAll('.fc-event')
        // );
        // eventEls.forEach(function(eventEl) {
        //   new Draggable(eventEl, {
        //     eventData: {
        //       title: eventEl.innerText.trim(),
        //     }
        //   });
        // });

        /* initialize the calendar
        -----------------------------------------------------------------*/
        var cd = new Date();
        var selectedYear = $('#year').val()
        if (!selectedYear) {
            selectedYear = cd.getFullYear()
        }
        var selectedDate = new Date($('#year').val(), cd.getMonth(), cd.getDate());
        
        console.log(selectedYear)
        var calendarEl = document.getElementById('calendar');
        var calendar = new Calendar(calendarEl, {
            plugins: ['interaction', 'dayGrid', 'timeGrid'],
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
            },
            defaultDate: selectedDate,
            validRange: {
                start: selectedYear + '-01-01',
                end: selectedYear + '-12-31'
            },
            visibleRange: {
                start: selectedYear + '-01-01',
                end: selectedYear + '-12-31'
            },
            events: lstCalendarItems,
            editable: true,
            droppable: true, // this allows things to be dropped onto the calendar
            drop: function (arg) {
                newList = [];
                // is the "remove after drop" checkbox checked?
                var originalEventObject = arg.draggedEl;

                var copiedEventObject = $.extend({}, originalEventObject);
                copiedEventObject.start = arg.date;
                copiedEventObject.allDay = arg.allDay;
                copiedEventObject.id = counter;
                counter = counter - 1;

                //calendar.renderEvent(copiedEventObject, true);
                //calendar.render()
                if (document.getElementById('drop-remove').checked) {
                    // if so, remove the element from the "Draggable Events" list
                    arg.draggedEl.parentNode.removeChild(arg.draggedEl);
                }
            },
            //eventDrop: function(info) {
            //    alert(info.event.title + " was dropped on " + info.event.start.toISOString());

            //    if (!confirm("Are you sure about this change?")) {
            //        info.revert();
            //    }
            //},
            eventRender: function (info) {
                var event = info.event.extendedProps;
                if (!event.description == "") {
                    //$(info.el).find('.fc-title').append("<br/><span class='ultra-light'>" + event.description +
                    //    "</span>");
                    $(info.el).find('.fc-title').attr("data-description", event.description);
                }
                if (!event.icon == "") {
                    $(info.el).find('.fc-title').prepend("<i class='air air-top-right fa " + event.icon +
                        " '></i>");
                }
                newList.push(info.event)
                var isEdit=$('#isEdit').val();
                if (isEdit == true || isEdit == 'true' || isEdit == 'True' || isEdit == 'TRUE') {
                    $(info.el).bind('dblclick', function () {
                        $.SmartMessageBox({
                            title: "Alert!",
                            content: "Are you sure to delete this item?",
                            buttons: '[No][Yes]'
                        }, function (ButtonPressed) {
                            if (ButtonPressed === "Yes") {

                                var index = newList.indexOf(info.event);
                                if (index > -1) {
                                    newList.splice(index, 1);
                                }
                                info.event.remove();
                                //$('#calendar').fullCalendar('removeEvents', event.id);
                            }
                        });

                    });
                }
            }
            
        });
        calendar.render();
    },
    error: function (xhr, ajaxOptions, thrownError) {
        //log error
        _showErrorMessage();
    }
});




//};

// end pagefunction

// loadscript and run pagefunction
//loadScript("Scripts/plugin/fullcalendar/jquery.fullcalendar.min.js", pagefunction);
//loadScript("Scripts/calendar-3.9.0/fullcalendar.js", pagefunction);