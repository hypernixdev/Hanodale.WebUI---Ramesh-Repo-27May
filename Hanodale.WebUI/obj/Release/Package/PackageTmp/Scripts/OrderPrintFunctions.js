
function printLabel(watermark) {
    // Get the HTML content of the label section
    var printContents = document.getElementById('labelPrint').outerHTML;

    // Add a watermark div
    var watermarkHtml = `
    <div style="
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        z-index: 1000;
        display: flex;
        justify-content: center;
        align-items: center;
        color: rgba(0, 0, 0, 0.3); /* Light color for watermark effect */
        font-size: 36px;
        text-align: center;
        transform: rotate(-45deg);
        pointer-events: none;">
        ${watermark}
    </div>
`;


    // Wrap content and add watermark
    var wrappedContents = `
        <div style="position: relative;">
            ${watermarkHtml}
            ${printContents}
        </div>
    `;

    // Create a hidden iframe
    var iframe = document.createElement('iframe');
    iframe.style.position = 'absolute';
    iframe.style.top = '-10000px';
    iframe.style.width = '0px';
    iframe.style.height = '0px';
    document.body.appendChild(iframe);

    // Write the content to the iframe's document
    var iframeDoc = iframe.contentWindow || iframe.contentDocument;
    if (iframeDoc.document) iframeDoc = iframeDoc.document;

    iframeDoc.open();
    iframeDoc.write(`
        <html>
            <head>
                <title>Print Label</title>
                <style>
@font-face {
    font-family: 'RobotoMono';
    src: url('Content/fonts/RobotoMono-Regular.ttf') format('truetype');
}
                    /* Add print styles */
                    body, html {
                        margin: 0;
                        padding: 0;
                        width: 90mm;
                        height: auto;
font-size: 15px;
font-family:'RobotoMono',Arial !important;
                    }
                    .content-window {
                        width: 100%;
                        height: auto;
                        word-wrap: break-word;
                        overflow: hidden;
                        box-sizing: border-box;
font-size: 15px;
font-family:'RobotoMono';
                    }
               @media print {
                        /* Ensure no extra margins */
                        body {
                            margin: 0;
                            padding: 0;
                        }

                        /* Shrink content to fit width (80mm) */
                        .content-window {
                            transform: scale(0.8); /* Scale down content to fit */
                            transform-origin: top left; /* Ensure scaling starts from the top left */
                            width: 100%;
                            height: auto;
font-size: 15px;
                        }

                        .content-window * {
                            font-size: 10px !important; /* Force consistent small font size for all elements */
                            line-height: 1.2;
                            margin: 0 !important;
                            padding: 0 !important;
font-family: 'RobotoMono';
font-size: 15px;
                        }
                    }
                </style>
            </head>
            <body>
                ${wrappedContents}
            </body>
        </html>
    `);
    iframeDoc.close();

    // Wait for the iframe's content to fully load
    iframe.onload = function () {
        // Print the iframe content
        iframe.contentWindow.focus();
        iframe.contentWindow.print();

        // Clean up after printing
        setTimeout(() => {
            document.body.removeChild(iframe);
        }, 1000); // Adjust timeout as needed
    };
}






function loadTableData(letterName, model) {
     
    var _model = model;
    var _itemTotal = 0.00;
    var _total = 0.00;

    const tbody = document.querySelector('#data-table tbody');
    tbody.innerHTML = ''; // Clear existing table data

    // Header Section
    const rowHead1 = document.createElement('tr');
    rowHead1.innerHTML = "<td colspan='5' style='font-weight:bold; text-align:center;'>LUCKY FROZEN SDN BHD</td>";
    tbody.appendChild(rowHead1);

    const rowHead2 = document.createElement('tr');
    rowHead2.innerHTML = "<td colspan='5' style='text-align:center;'>198201003571 (083316-M)</td>";
    tbody.appendChild(rowHead2);

    const rowHead3 = document.createElement('tr');
    rowHead3.innerHTML = "<td colspan='5' style='text-align:center;'>BRANCH : 8-10, Jalan Baba, Off Changkat Tambi Dollah, Pudu, 55100 Kuala Lumpur</td>";
    tbody.appendChild(rowHead3);

    const rowHead4 = document.createElement('tr');
    rowHead4.innerHTML = "<td colspan='5' style='text-align:center;'>Tel: 03-2145 5251, 2141 1574, 2141 5586 Fax: 03-2141 2336</td>";
    tbody.appendChild(rowHead4);

    const rowDate = document.createElement('tr');
    const currentDate = new Date();
    const formattedDate = currentDate.toLocaleDateString("en-GB"); // Format date as DD/MM/YYYY
    const formattedTime = currentDate.toLocaleTimeString("en-GB"); // Format time as HH:mm:ss

    rowDate.innerHTML = "<td colspan='5' style='text-align:right;'> <span style='color: blue;'>DATE:</span> " + formattedDate + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='color: blue;'>TIME:</span>: " + formattedTime + "</td>";
    tbody.appendChild(rowDate);

    tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue;color:blue'></td>";

    // Customer and Delivery Details
    const customerRow = document.createElement('tr');
    customerRow.innerHTML = "<td colspan='3' style='text-align: left;color:blue'>DELIVER TO:</td><td style='text-align: left;color:blue'>BILL TO:</td>";

    tbody.appendChild(customerRow);


    const customerDetails = document.createElement('tr');
    customerDetails.innerHTML = `<td colspan='3' style='text-align: left;'>${_model.customerName}</td>
                                 <td colspan='3' style='text-align: left;'>${_model.shipToName}</td>`;
    tbody.appendChild(customerDetails);


    // Data Row for Delivery and Billing Information
    const dataRow = document.createElement('tr');
    dataRow.innerHTML = "<td colspan='3' style='text-align: left;'></td><td colspan='3' style='text-align: left;'>" + _model.customerAddress + "</td>";
    tbody.appendChild(dataRow);
    // Data Row for Delivery and Billing Information
    const dataRows = document.createElement('tr');
    dataRows.innerHTML = "<td colspan='3' style='text-align: left;'></td><td colspan='3' style='text-align: left;'>" + _model.shipToAddress + "</td>";
    tbody.appendChild(dataRows);

    // Customer and Delivery Details
    const tinRow = document.createElement('tr');
    tinRow.innerHTML = "<td style='text-align: left;color:blue'>TIN ID: " + _model.tinId + "</td>";

    tbody.appendChild(tinRow);

    tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue;color:blue'></td>";

    // Delivery Information Section
    const deliveryInfo = document.createElement('tr');
    deliveryInfo.innerHTML = `  <td colspan='5' style='text-align: left;'>
        <span style="color: blue;">ISSUE BY:</span> ${_model.entryPerson}<br>       
        <span style="color: blue;">CUSTOMER REF:</span> SELFCOLL<br>        
        <span style="color: blue;">POS-ORDER NO: </span>${_model.orderNum}
    </td>`;
    tbody.appendChild(deliveryInfo);

    tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue;color:blue'></td>";




    // Product Table Header
    const productHeader = document.createElement('tr');
    productHeader.innerHTML = "<td style='text-align: left; padding: 2px;'>No Description</td>" +
        "<td style='padding: 2px;text-align: right;' colspan=1>Pack/KG</td>" +
        "<td style='padding: 2px;' colspan=2>Total Units</td>" +
        "<td style='padding: 2px;'>Unit Price</td>";
    tbody.appendChild(productHeader);
    tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue; color: blue'></td>";

    // Initialize total variable
    let total = 0.00;
    let discount = 0;
    let itemTotal = 0;
    let rettotal = 0;

    // Initialize variables to store totals
    let kgTotal = 0;
    let groupedTotals = {};

    // Iterate over each order item
_model.OrderItems.forEach((item, index) => {
    // Update `orderQty` based on `scannedQty`
    item.orderQty = item.scannedQty > 0 ? item.scannedQty : item.orderQty;

    const description = item.description; // Main description 
    itemTotal = parseFloat(item.listPrice) || 0;
    rettotal = parseFloat(item.returnTotal) || 0;
    total += itemTotal + rettotal;
    // Main Item Row
    appendOrderRow(tbody, item, description, index + 1, false);

    // Check if the item is returned
    if (item.IsReturned) {
      
        const returnedRow = document.createElement('tr');
        returnedRow.innerHTML = `
        <td colspan="4" style="text-align: left; padding: 2px;">
            ${index + 1}. ${description} <span style="color: red;">(Returned)</span>
        </td>
    `;
        tbody.appendChild(returnedRow);
        // Create a second row for returned item details (Pack/KG, Total Units, Unit Price)
        const returnedDetailsRow  = document.createElement('tr');
        returnedDetailsRow .innerHTML = `
            <td style="text-align: right; padding: 2px;" colspan="2">
                -${item.retQty}
            </td>
            <td style="padding: 2px;" colspan="2">
                -${item.retQty}
            </td>
            <td style="padding: 2px;">
                ${item.returnTotal}
            </td>
        `;
        tbody.appendChild(returnedDetailsRow);

        // Check the sales UOM and accumulate totals
        if (item.salesUm === 'KG') {
            kgTotal += item.orderQty;
        } else if (item.salesUm === 'CTN' || item.salesUm === 'PCK') {
            // Group and sum other UOMs
            if (!groupedTotals[item.salesUm]) {
                groupedTotals[item.salesUm] = 0;
            }
            groupedTotals[item.salesUm] += item.orderQty;
        }
    }

});





    // Summary Section
    tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue;color:blue'></td>";

    const subtotalRow = document.createElement('tr');
    subtotalRow.innerHTML = `<td colspan='3' style='text-align: left; color:blue'>TOTAL BEFORE DISCOUNT:</td>
                             <td  colspan='2' style='text-align: right; font-weight:bold;'>${formatNumber(total.toFixed(2))} MYR</td>`;
    tbody.appendChild(subtotalRow);
   
    //// Filter and sum DiscountAmt from OrderItems
    //const discountFromOrderItems = _model.OrderItems
    //    .map(item => parseFloat(item.discountAmt || 0))  // Extract DiscountAmt
    //    .reduce((total, amount) => total + amount, 0);  // Sum them

    //// Filter OrderPayments for valid discounts where isRefund = 0 and paymentType = 'Discount'
    //const discountFromPayments = _model.OrderPayments
    //    .filter(payment => payment.IsRefund === false && payment.PaymentType === 'Discount')  // Filter valid payments
    //    .map(payment => parseFloat(payment.Amount || 0))  // Extract Amount
    //    .reduce((total, amount) => total + amount, 0);  // Sum them

    //// Filter OrderPayments for refunded discounts where (isRefund = 1 or isRefund = null) and paymentType = 'Discount'
    //const refundedDiscount = _model.OrderPayments
    //    .filter(payment => (payment.IsRefund === true || payment.IsRefund === null) && payment.PaymentType === 'Discount')  // Filter refunded payments
    //    .map(payment => parseFloat(payment.Amount || 0))  // Extract Amount
    //    .reduce((total, amount) => total + amount, 0);  // Sum them

    // Calculate total discount
    const totalDiscount = _model.OrderPayments
        .filter(payment => payment.PaymentType === "Discount" && (payment.IsRefund === null || payment.IsRefund === false))
        .reduce((sum, payment) => sum + (parseFloat(payment.Amount) || 0), 0);


    //// Calculate total discount
    //const totalDiscount = discountFromOrderItems + discountFromPayments - refundedDiscount;



    // Create the discount row
    const discountRow = document.createElement('tr');
    discountRow.innerHTML = `
        <td colspan='3' style='text-align: left; color: blue;'>DISCOUNT:</td>
        <td colspan='2' style='text-align: right;'>${formatNumber(totalDiscount)} MYR</td>
    `;
    tbody.appendChild(discountRow);

    const totalAfterDiscountRow = document.createElement('tr');
    totalAfterDiscountRow.innerHTML = `<td colspan='3' style='text-align: left;color:blue'>TOTAL AFTER DISCOUNT:</td>
                                      <td colspan='2' style='text-align: right; font-weight:bold;'>${formatNumber((total - totalDiscount).toFixed(2))} MYR</td>`;
    tbody.appendChild(totalAfterDiscountRow);
    tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue;color:blue'></td>";
    const totalPayableRow = document.createElement('tr');
    totalPayableRow.innerHTML = `<td colspan='3' style='text-align: left; color:blue'>TOTAL PAYABLE:</td>
                                <td colspan='2' style='text-align: right; font-weight:bold;'>${formatNumber((total - totalDiscount).toFixed(2))} MYR</td>`;
    tbody.appendChild(totalPayableRow);

    //// Iterate over each order item and exclude return quantities
    //_model.OrderItems.forEach((item) => {
    //    if (item.IsReturned !== true) { // Exclude items marked as returns
    //        // Accumulate KG total
    //        if (item.unit === "KG") {
    //            kgTotal += parseFloat(item.orderQty || 0);
    //        }

    //        // Accumulate totals for other units like CTN, PCK
    //        if (item.unit !== "KG") {
    //            if (!groupedTotals[item.unit]) {
    //                groupedTotals[item.unit] = 0;
    //            }
    //            groupedTotals[item.unit] += parseFloat(item.orderQty || 0);
    //        }
    //    }
    //});


    // Create the UOM Summary row
    const uomSummaryHeaderRow = document.createElement('tr');
    let uomSummaryText = `<td colspan='3' style='text-align: left;'>
                         <span style='color:blue;'>UOM Summary:</span>`;

    // Display KG Total if present
    if (kgTotal > 0) {
        uomSummaryText += ` ${kgTotal.toFixed(2)} KG`;
    }

    // Display grouped totals for CTN and PCK, concatenated inline
    Object.keys(groupedTotals).forEach(unit => {
        if (groupedTotals[unit] > 0) {
            uomSummaryText += ` / ${groupedTotals[unit].toFixed(2)} ${unit}`;
        }
    });

    uomSummaryText += `</td>`; // Close the <td> element

    // Set the innerHTML of the UOM summary row
    uomSummaryHeaderRow.innerHTML = uomSummaryText;

    // Append the UOM summary row to the table body
    tbody.appendChild(uomSummaryHeaderRow);


    tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue;color:blue'></td>";

    // Payment Information Section (only display if OrderPayments is not null or empty)
    if (_model.OrderPayments && _model.OrderPayments.length > 0) {

        // Payment Table Header
        const paymentTHeader = document.createElement('tr');
        paymentTHeader.innerHTML = "<td style='width: 40%; text-align: left;font-weight:bold'>Payment</td>"

        tbody.appendChild(paymentTHeader);
        tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-bottom: 1px dashed blue;color:blue'></td>";

        // Payment Table Header
        const paymentTableHeader = document.createElement('tr');
        paymentTableHeader.innerHTML = "<td style='width: 40%; text-align: left;'>Payment ID</td>" +
            "<td style='width: 40%; text-align: center;'>Reference</td>" +
            "<td style='width: 40%; text-align: left;'>Mode</td>" +
            "<td style='width: 40%; text-align: center;'>Amount</td>" +
            "<td style='width: 40%; text-align: center;'>Date</td>";
        tbody.appendChild(paymentTableHeader);

        // Filter and add Payment Rows where IsRefund is null or false
        _model.OrderPayments
            .filter(payment => (payment.IsRefund === null || payment.IsRefund === false) && payment.PaymentType !== "Discount")
            .forEach(payment => {
                const paymentRow = document.createElement('tr');
                paymentRow.innerHTML = `
                    <td style='width: 20%; text-align: left;'>${payment.Id}</td>
                    <td style='width: 20%; text-align: center;'>${payment.RefNumber}</td>
                    <td style='width: 20%; text-align: left;'>${payment.PaymentType}</td>
                    <td style='width: 20%; text-align: left;'>${formatNumber(payment.Amount)}</td>
                    <td style='width: 20%; text-align: left;'>${formatDateFromDateString(payment.PaymentDate)}</td>
                `;
                tbody.appendChild(paymentRow);
            });

        // Add a separator line after the payment table
        tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue; color: blue;'></td>";

        if (_model.OrderPayments.some(payment => payment.IsRefund === true)) {

            // Refund Section Header
            const refundTHeader = document.createElement('tr');
            refundTHeader.innerHTML = "<td style='width: 40%; text-align: left; font-weight: bold;'>Refund</td>";
            tbody.appendChild(refundTHeader);
            tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-bottom: 1px dashed blue; color: red;'></td>";

            // Refund Table Header
            const refundTableHeader = document.createElement('tr');
            refundTableHeader.innerHTML = "<td style='width: 40%; text-align: left;'>Refund ID</td>" +
                "<td style='width: 40%; text-align: center;'>Reference</td>" +
                "<td style='width: 40%; text-align: left;'>Mode</td>" +
                "<td style='width: 40%; text-align: center;'>Amount</td>" +
                "<td style='width: 40%; text-align: center;'>Date</td>";
            tbody.appendChild(refundTableHeader);

            // Filter and add Refund Rows where IsRefund is true
            _model.OrderPayments
                .filter(payment => payment.IsRefund === true)
                .forEach(refund => {
                    const refundRow = document.createElement('tr');
                    refundRow.innerHTML = `
                <td style='width: 20%; text-align: left;'>${refund.Id}</td>
                <td style='width: 20%; text-align: center;'>${refund.RefNumber}</td>
                <td style='width: 20%; text-align: left;'>${refund.PaymentType}</td>
                <td style='width: 20%; text-align: left;'>${formatNumber(refund.Amount)}</td>
                <td style='width: 20%; text-align: left;'>${formatDateFromDateString(refund.PaymentDate)}</td>
            `;
                    tbody.appendChild(refundRow);
                });

            // Add a separator line after the refund table
            tbody.appendChild(document.createElement('tr')).innerHTML = "<td colspan='5'><hr style='border: none; border-top: 1px dashed blue; color: red;'></td>";
        }
    }
    // Footer
    const thankYouRow = document.createElement('tr');
    thankYouRow.innerHTML = "<td colspan='5' style='text-align: center;'>THANK YOU FOR YOUR PURCHASE.</td>";
    tbody.appendChild(thankYouRow);
    tbody.style.fontSize = '10px';  // Set your desired font size here
    $('#printModal').modal('show');
}
// Function to append an order row
function appendOrderRow(tbody, item, description, displayIndex, isReturned) {
    // First row with No. and Description
    const row1 = document.createElement('tr');
    row1.innerHTML = `
        <td colspan="4" style="text-align:left; vertical-align: top; padding: 2px;">
            ${displayIndex}.&#9;${description}
        </td>`;
    tbody.appendChild(row1);

    // Second row with Pack/KG, Total Units, and Unit Price
    const row2 = document.createElement('tr');
    row2.innerHTML = `
        <td style="text-align:right; padding: 2px;" colspan=2>
            ${item.orderQty} ${item.salesUm}
        </td>
        <td style="padding: 2px;" colspan=2>
            ${item.orderQty} ${item.salesUm}
        </td>
        <td style="padding: 2px;">
            ${formatNumber(item.listPrice)}
        </td>`;
    tbody.appendChild(row2);
}
function formatDateFromDateString(dateStr) {
    // Extract milliseconds from the string
    let milliseconds = parseInt(dateStr.replace("/Date(", "").replace(")/", ""));

    // Convert milliseconds to a Date object
    let date = new Date(milliseconds);

    // Format the date (e.g., in DD/MM/YYYY format)
    return date.toLocaleDateString("en-GB", {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
}


function formatNumber(number) {
    // Round to two decimal places
    number = Math.round(number * 100) / 100;

    // Split the number into whole and decimal parts
    let parts = number.toString().split(".");
    let wholePart = parts[0];
    let decimalPart = parts.length > 1 ? "." + parts[1].padEnd(2, '0') : ".00"; // Ensure two decimal places

    // Add thousand separator
    wholePart = wholePart.replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    return wholePart + decimalPart;
}
async function printReceipt(orderId, orderStatus) {
    try {
         
        let labelPrint = document.getElementById('labelPrint').outerHTML;
        let watermark = '';
        let actionType = 'Print'; // Default action type

        if (orderStatus === "Completed") {
            // Check if any item in the OrderItems table has IsItemReturned = true
            const itemReturnResponse = await fetch(`${checkItemReturnStatusUrl}?orderId=${orderId}`);
            const itemReturnData = await itemReturnResponse.json();

            if (itemReturnData.isItemReturned) {
                // Check if Print-Refund record exists
                const refundResponse = await fetch(`${checkPrintRefundStatusUrl}?orderId=${orderId}`);
                const refundData = await refundResponse.json();

                if (refundData.isRefundPrinted) {
                    watermark = 'Refund Copy';
                } else {
                    watermark = 'Refund';
                    actionType = 'Print-Refund'; // Log as a Print-Refund action
                }
            } else {
                // No items are returned, handle regular print or reprint
                const printResponse = await fetch(`${checkPrintRefundStatusUrl}?orderId=${orderId}`);
                const printData = await printResponse.json();

                if (printData.isRefundPrinted) {
                    watermark = 'COPY';
                }
            }
        }

        // Add watermark to the print content
        labelPrint = watermark;
        printLabel(watermark); // Trigger the printing
    } catch (error) {
        console.error('Error in printReceipt:', error);
    }
}








