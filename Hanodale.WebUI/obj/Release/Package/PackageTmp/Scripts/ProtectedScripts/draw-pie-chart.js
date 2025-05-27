function drawPieChart(controllerName, actionName, isDonutHole, flexibleLegend) {
    var pieInnerRadius = 0;
    var noColumns = 3;

    if ((isDonutHole === '') || (isDonutHole == 'undefined') || (isDonutHole == null)) {
        isDonutHole = false;
    }

    if ((flexibleLegend === '') || (flexibleLegend == 'undefined') || (flexibleLegend == null)) {
        flexibleLegend = false;
    }

    if (isDonutHole == true) {
        pieInnerRadius = 0.5;
    }

    // Color Array for Pie Chart
    var color = ["#fe3993", "#24c6b1" , "#8a6ab5", "#ffc33f", "#1e97f3", "#228DF5", "#F522CB", "#22D5F5", "#B822F5", "#B8F522", "#614E43"];

     
    var fromDate = $('#loadedDateFrom').val();
    var toDate = $('#loadedDateTo').val();

    fromDate = formatDateToDDMMYYYY(fromDate);
    toDate = formatDateToDDMMYYYY(toDate);

    $.ajax({
        type: "POST",
        url: controllerName + "/" + actionName,
        data: JSON.stringify({ startDate: fromDate, endDate: toDate, chartType_Id: $("#chartType_Id").val(), section: $("#section").val() }),
        cache: false,
        hideLoading: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            for (var k = 0; k < data.list.length; k++) {
                var obj = data.list[k];
                var dataList = obj.listItems;
                var status = [];
                var totalStatus = 0;

                // Calculate total count
                $.each(dataList, function () {
                    totalStatus += this.value;
                });

                if (totalStatus == 0) totalStatus = 1; // Avoid division by zero

                // Prepare data with percentage
                for (var i = 0; i < dataList.length; i++) {
                    var percentage = Math.round((dataList[i].value / totalStatus) * 100);
                    var label = dataList[i].type+" - " + dataList[i].value + " (" + percentage + "%)"; // Label with count and percentage

                    status.push({
                        insideLabel: percentage + "%", // Inside label with percentage only
                        label: label, // Label with count and percentage (to be displayed in the legend)
                        data: dataList[i].value,
                        color: color[i]
                    });
                }

                // If data is available, plot the pie chart
                if (status != null && status.length > 0) {
                    $.plot($("#pieCharPanelDashboardPlaceholderBody_" + obj.type), status,
                        {
                            series: {
                                pie: {
                                    show: true,
                                    radius: 1,
                                    label: {
                                        show: true, // Show labels inside the pie chart
                                        radius: 2 / 3,
                                        formatter: function (label, series) {
                                            return '<div style="font-size:7pt;text-align:center;padding:7px;color:black;">' +
                                                series.insideLabel + '</div>'; // Show only percentage inside the pie chart
                                        },
                                    },
                                    innerRadius: pieInnerRadius, // Inner radius for donut hole
                                }
                            },
                            legend: {
                                show: true,
                                container: $("#chartLegend_" + obj.type),
                                noColumns: noColumns,
                                flexibleLegend: flexibleLegend
                            },
                            grid: {
                                hoverable: true
                            },
                            tooltip: true,
                            tooltipOpts: {
                                content: "%s (%p.0%)", // Show percentages in the tooltip
                                shifts: {
                                    x: 20,
                                    y: 0
                                },
                                defaultTheme: false
                            }
                        });
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            _showErrorMessage(); // Error handling
        }
    });
}



function formatDateToDDMMYYYY(dateString) {
     
    var dateObj = new Date(dateString);
    var day = String(dateObj.getDate()).padStart(2, '0');
    var month = String(dateObj.getMonth() + 1).padStart(2, '0'); // Month is 0-based
    var year = dateObj.getFullYear();
    return `${day}/${month}/${year}`;
}
