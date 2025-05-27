//function addOrUpdateProductRow(product, desktop_table, mobile_table) {
//     
//    // Check if a row with the same product ID already exists
//    var existingRow = $('#dt2_product tbody tr td').find('span[data-id="' + product.id + '"]');
//    var orderQty = product.orderQty ? parseInt(product.orderQty) : 1;
//    var orderQtyExists = product.orderQty ? true : false;
//    if (existingRow.length > 0) {
//        var existingTr = existingRow.parents("tr")
//        // If the product already exists, increase the order quantity
//        var qtyInput = existingTr.find('input[data-field="orderQty"]');
//        var currentQty = parseFloat(qtyInput.val());
//        if (orderQtyExists) {
//            qtyInput.val(orderQty);
//        } else {
//            qtyInput.val(currentQty + 1); // Increment the quantity by 1
//        }
//        //updateTotal(existingTr); // Update the total for this row
//    } else {
//        var rows = [
//            `<span data-id="${product.id}" class="product-id-field">${product.partNumber}</span>`,
//            `<span>${product.description}</span>`,
//            `<span data-field="price">${product.unitPrice || ''}</span>`,
//            `<span>${product.prodGrup_Description}</span>`,
//            `<span>${product.part_IUM}</span>`,
//            `<input type="number" class="form-control" data-field="orderQty" value="${orderQty}">`,
//            `<span data-field="total">${product.unitPrice || ''}</span>`,
//            `<button class="btn btn-danger btn-sm"><i class="fa fa-times"></i></button>`
//        ];

//        desktop_table.row.add(rows).draw();
//        mobile_table.row.add(rows).draw();
//        //updateOrderTotal()
//    }
//    updateAllTotals();
//}

function addOrUpdateProductRow(product, desktop_table, mobile_table) {
     
    // Check if a row with the same product ID already exists
    var tbody = $('#dt2_product tbody');
    var existingRow = tbody.find('tr td').find('span[data-id="' + product.id + '"]');
    var orderQty = product.orderQty ? parseInt(product.orderQty) : 1;
    var orderQtyExists = product.orderQty ? true : false;

    if (existingRow.length > 0) {
        var existingTr = existingRow.parents("tr");
        // If the product already exists, update the order quantity
        var qtyInput = existingTr.find('input[data-field="orderQty"]');
        var currentQty = parseFloat(qtyInput.val());
        if (orderQtyExists) {
            qtyInput.val(orderQty);
        } else {
            qtyInput.val(currentQty + 1); // Increment the quantity by 1
        }
        // Update line total
        var unitPrice = parseFloat(existingTr.find('span[data-field="price"]').text());
        var lineTotal = (unitPrice * parseFloat(qtyInput.val())).toFixed(2);
        existingTr.find('.product-lineTotal').text(lineTotal);

    } else {
        // Prepare row details from product object
        var productId = product.id || '';
        var productSearch = product.partNumber || '';
        var description = product.description || '';
        var unitPrice = parseFloat(product.unitPrice || 0).toFixed(2);
        var prodGroup = product.prodGrup_Description || '';
        var code = product.partCode || '';
        var orderUOM = product.part_IUM || '';
        var lineTotal = (unitPrice * orderQty).toFixed(2);
        var qtyType_ModuleItem_Id = product.QtyType_ModuleItem_Id || '';
        var orderUOM_Id = product.OrderUOM_Id || '';
        var operationStyle_ModuleItem_Id = product.operationStyle_ModuleItem_Id || '';
        var operationCost = product.operationCost || '';
        var complimentary_ModuleItem_Id = product.complimentary_ModuleItem_Id || '';

        // Construct the row HTML
        var row = `
            <tr>
                <td>
                    <button class="btn btn-danger btn-sm btn-remove"><i class="fa fa-times"></i></button>
                    <button class="btn btn-sm btn-success btn-edit"><i class="fa fa-edit"></i></button>
                </td>
                <td hidden>${productId}</td>
                <td class="product-id-field" data-id="${productId}">${productSearch}</td>
                <td class="product-desc-field">${description}</td>
                <td><span data-field="price">${unitPrice}</span></td>
                <td class="product-prodGroup">${prodGroup}</td>
                <td class="product-code" hidden>${code}</td>
                <td class="product-orderUOM">${orderUOM}</td>
                <td><input type="number" class="form-control" data-field="orderQty" value="${orderQty}" /></td>
                <td class="product-lineTotal">${lineTotal}</td>
                <td class="product-QtyType_ModuleItem_Id" hidden>${qtyType_ModuleItem_Id}</td>
                <td class="product-OrderUOM_Id" hidden>${orderUOM_Id}</td>
                <td class="product-operationStyle_ModuleItem_Id" hidden>${operationStyle_ModuleItem_Id}</td>
                <td class="product-operationCost" hidden>${operationCost}</td>
                <td class="product-complimentary_ModuleItem_Id" hidden>${complimentary_ModuleItem_Id}</td>
            </tr>
        `;

        // Add new row to the table
        tbody.append(row);
        desktop_table.row.add($(row)).draw();  // If using a datatable
        mobile_table.row.add($(row)).draw();   // If using a datatable
    }

    updateAllTotals(); // Update all totals after adding or updating the row
}


function updateAllTotals() {
    $('#dt2_product tbody tr').each(function () {
        updateTotal($(this));
    });
}
function updateTotal(row) {
     
    var qty = parseFloat(row.find('input[data-field="orderQty"]').val());
    var price = parseFloat(row.find('span[data-field="price"]').text().replace(/,/g, ''));
    console.log(price);
    var total = qty * price;
    // row.find('span[data-field="total"]').text(total.toFixed(2)); // Update the total field
    row.find('.product-lineTotal').text(formatNumberWithThousandSeparator(total));
    updateOrderTotal()
}

function updateOrderTotal() {
     
    var total = 0;
    $('#dt2_product tbody tr').each(function () {
        total += parseFloat($('.product-lineTotal').text()); // Remove thousand separators for parsing
    });
    // $('#orderTotal').text(total.toFixed(2));
    $('#orderTotal').text(formatNumberWithThousandSeparator(total));
}

function formatNumberWithThousandSeparator(number) {
    return number.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}


// Initialize Select2 on page load
function initializeSelect2($select, placeholder) {
    $select.select2({
        placeholder: placeholder,
        allowClear: true
    });
}
// Function to load dropdown options
function loadDropdown(url, $select, placeholder, selectedValue) {
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            // Clear current options
            $select.empty();

            // Add a placeholder option
            $select.append(new Option('', '', true, true)); // Placeholder option

            // Append new options
            $.each(data.ModuleItems, function (index, item) {
                $select.append(new Option(item.name, item.id));
            });

            // Initialize Select2 after options are added
            initializeSelect2($select, placeholder);

            // Set the selected value if available
            if (selectedValue) {
                // Ensure the value is available in the dropdown
                setTimeout(function () {
                    $select.val(selectedValue).trigger('change');
                }, 100); // A short delay ensures that Select2 is fully initialized
            } else {
                $select.val('').trigger('change');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown options:', error);
        }
    });
}




