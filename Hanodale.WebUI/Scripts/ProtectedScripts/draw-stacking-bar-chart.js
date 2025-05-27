function searchForArray(arrayList, item) {
    var i, j, current;
    for (i = 0; i < arrayList.length; ++i) {
        if (arrayList[i][1] === item) {
            return arrayList[i][0];
        }
    }
    return -1;
}



function drawStackingBarChart(controllerName, actionName, isCategortByName) {

    if (isCategortByName == "undefined" || isCategortByName == null) {
        isCategortByName = false;
    }

    var color = ["#fe3993", "#ffc33f", "#8a6ab5", "#24c6b1", "#1e97f3", "#228DF5", "#F522CB", "#22D5F5", "#B822F5", "#B8F522", "#614E43"];

    var fromDate = "" //  $('#estimatedStartDateFrom').val();
    var toDate = "" // $('#estimatedEndDateTo').val();

    $.ajax({
        type: "POST",
        //url: "/ChartPanelPurchaseDashboard/GetStackingBarChartInfo",
        url:  controllerName + "/" + actionName,
        data: JSON.stringify({ startDate: fromDate, endDate: toDate, type: $("#type").val() }),
        cache: false,
        hideLoading: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            for (var k = 0; k < data.list.length; k++) {
                var obj = data.list[k];
                var dataList = obj.lstGroupedItems;

                var status = [];
                var totalStatus = 0;

                //$.each(dataList, function () {
                //    totalStatus += this.value;
                //});

                //if (totalStatus == 0)
                //    totalStatus = 1;
                var chartLabels = []
                var min = 0;
                var max = 0;

                for (var i = 0; i < dataList.length; i++) {

                    var entity = dataList[i].lstItems;
                    var subList = [];
                    if (isCategortByName) {
                        for (var f = 0; f < entity.length; f++) {
                            var index = 0;
                            var labelName = entity[f].FullDateName
                            var resStatus = searchForArray(chartLabels, labelName)
                            if (resStatus >= 0) {
                                index = resStatus;
                            } else {
                                index = chartLabels.length;
                                chartLabels.push([index, labelName]);
                            }
                            subList.push([index, entity[f].Count]);
                        }
                    } else {
                        for (var f = 0; f < entity.length; f++) {
                            var dateTimeSpan = entity[f].DateTimeSpan;
                            if (dateTimeSpan < min || min == 0)
                                min = dateTimeSpan
                            if (dateTimeSpan > max || max == 0)
                                max = dateTimeSpan
                            subList.push([entity[f].DateTimeSpan, entity[f].Count]);
                        }


                    }
                    status.push({ data: subList, label: dataList[i].categoryName });
                }


                var xaxis = { ticks: chartLabels, tickLength: 0, color: "white", min: -0.5, max: (chartLabels.length > 0 ? chartLabels.length - 0.5 : 0.5) };
                var yaxis = {};
                var bars = { barWidth: .3, align: "center" };

                if (!isCategortByName) {

                    var minTimeSpan = new Date()
                    if (min > 0) {
                        minTimeSpan = new Date(min)
                    }
                    //minTimeSpan.setMonth(minTimeSpan.getMonth() - 1);
                    minTimeSpan.setDate(minTimeSpan.getDate() - 15)

                    var maxTimeSpan = new Date()
                    if (max > 0) {
                        maxTimeSpan = new Date(max)
                    }
                    //maxTimeSpan.setMonth(maxTimeSpan.getMonth() + 1);
                    maxTimeSpan.setDate(maxTimeSpan.getDate() + 15)

                    xaxis = {
                        min: minTimeSpan,
                        max: maxTimeSpan,
                        axisLabelPadding: 20,
                        tickLength: 0,
                        color: "white",
                        axisLabelUseCanvas: true,
                        autoScale: "none",
                        mode: "time",
                        minTickSize: [1, "month"],
                        timeBase: "milliseconds",
                    }

                    yaxis = {
                        //tickLength: 10,
                        //color: "black",
                        axisLabelUseCanvas: true,
                        axisLabelFontSizePixels: 12,
                        axisLabelFontFamily: 'Verdana, Arial',
                        axisLabelPadding: 30,

                    }

                    bars = {
                        align: "center",
                        //barWidth: 94 * 60 * 60 * 600
                        barWidth: 15 * 24 * 60 * 60 * 1000,
                    }
                }


                $.plot($("#stackingBarCharPanelDashboardPlaceholderBody_" + obj.chartType), status,
                          {
                              xaxis: xaxis,
                              yaxis: yaxis,
                              series: {
                                  stack: true,
                                  bars: {
                                      show: true,
                                      align: "center",
                                      lineWidth: 0.8,
                                      fill: true,
                                      opacity: .9,
                                      barWidth: 12 * 44 * 60 * 60 * 300,
                                      fillColor: { colors: [{ opacity: 0.7 }, { opacity: 0.7 }] }
                                  }
                              }, grid: {
                                  hoverable: true,
                                  borderWidth: 0,
                                  //backgroundColor: { colors: ["#EDF5FF", "#ffffff"] }
                              },
                              legend: {
                                  show: true,
                                  container: $("#stackingBarChartLegend_" + obj.chartType),
                                  noColumns: 3,
                                  flexibleLegend: true
                              },
                              zoom: {
                                  interactive: true
                              },
                              bars: bars
                          });
                $("#stackingBarCharPanelDashboardPlaceholderBody_" + obj.chartType).UseTooltip();
                //$("#stackingBarCharPanelDashboardPlaceholderBody_" + obj.chartType).resizable({
                //    maxWidth: 900,
                //    maxHeight: 500,
                //    minWidth: 270,
                //    minHeight: 250
                //});

                //window.onresize = function (event) {
                //    $.plot($("#placeholder"), [d1, d2, d3]);
                //}


            }

        },


        error: function (xhr, ajaxOptions, thrownError) {
            //log error
            _showErrorMessage();
        }
    });
}


var previousPoint = null, previousLabel = null;

$.fn.UseTooltip = function () {
    $(this).bind("plothover", function (event, pos, item) {
        if (item) {
            if ((previousLabel != item.series.label) || (previousPoint != item.dataIndex)) {
                previousPoint = item.dataIndex;
                previousLabel = item.series.label;
                $("#tooltip").remove();

                var x = item.datapoint[0];
                var y = item.datapoint[1];

                var color = item.series.color;

                //console.log(item.series.xaxis.ticks[x].label);                

                showTooltip(item.pageX,
                item.pageY,
                color,
                //"<strong>" + item.series.label + "</strong><br>" + item.series.xaxis.ticks[x].label + " : <strong>" + y + "</strong> °C");
                "<strong>" + item.series.label + "</strong><br> Count" + " : <strong>" + y + "</strong>");
            }
        } else {
            $("#tooltip").remove();
            previousPoint = null;
        }
    });
};


function showTooltip(x, y, color, contents) {
    $('<div id="tooltip">' + contents + '</div>').css({
        position: 'absolute',
        display: 'none',
        top: y - 20,
        left: x - 60,
        border: '2px solid ' + color,
        padding: '3px',
        'font-size': '9px',
        'border-radius': '5px',
        'background-color': '#fff',
        'font-family': 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
        opacity: 0.9
    }).appendTo("body").fadeIn(200);
}