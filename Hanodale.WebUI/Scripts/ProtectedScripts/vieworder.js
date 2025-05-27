$(document).ready(function () {

    function addOrUpdateProductRow(product, desktop_table, mobile_table) {
        // Check if a row with the same product ID already exists
        var existingRow = $('#dt2_product tbody tr td').find('span[data-id="' + product.id + '"]');
        var orderQty = product.orderQty ? parseInt(product.orderQty) : 1;
        var orderQtyExists = product.orderQty ? true : false;
        if (existingRow.length > 0) {
            var existingTr = existingRow.parents("tr")
            // If the product already exists, increase the order quantity
            var qtyInput = existingTr.find('input[data-field="orderQty"]');
            var currentQty = parseFloat(qtyInput.val());
            if (orderQtyExists) {
                qtyInput.val(orderQty);
            } else {
                qtyInput.val(currentQty + 1); // Increment the quantity by 1
            }
            updateTotal(existingTr); // Update the total for this row
        } else {

            var rows = [
                `<span data-id="${product.id}" class="product-id-field">${product.partNumber}</span>`,
                `<span>${product.description}</span>`,
                `<span data-field="price">${product.unitPrice || ''}</span>`,
                `<span>${product.prodGrup_Description}</span>`,
                `<span>${product.part_IUM}</span>`,
                `<input type="number" class="form-control" data-field="orderQty" value="${orderQty}">`,
                `<span data-field="total">${product.unitPrice || ''}</span>`,
                `<button class="btn btn-danger btn-sm"><i class="fa fa-times"></i></button>`
            ];

            desktop_table.row.add(rows).draw();
            mobile_table.row.add(rows).draw();
            updateOrderTotal()
        }
    }

    function updateOrderTotal() {
        var total = 0;
        $('#dt2_product tbody tr').each(function () {
            total += parseFloat($(this).find('span[data-field="total"]').text() || 0);
        });
        $('#orderTotal').text(total.toFixed(2));
    }

    function updateTotal(row) {
        var qty = parseFloat(row.find('input[data-field="orderQty"]').val());
        var price = parseFloat(row.find('span[data-field="price"]').text());
        var total = qty * price;
        row.find('span[data-field="total"]').text(total.toFixed(2)); // Update the total field
        updateOrderTotal()
    }

    var desktop_table = $('#dt2_product').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "responsive": true
    });

    var mobile_table = $('#dt2_product_mobile').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "responsive": true
    });

    function formatDate(date) {
        var day = String(date.getDate()).padStart(2, '0');
        var month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
        var year = date.getFullYear();

        return day + '/' + month + '/' + year;
    }

    // Get today's date
    var today = new Date();

    // Set the formatted date as the value of the input field
    $('#orderDate').val(formatDate(today));

    // Cart functions begins
    $(document).on('click', '#dt2_product .btn-danger, #dt2_product_mobile .btn-danger', function () {
        var row = $(this).closest('tr');
        console.log("Calling this....")
        desktop_table.row(row).remove().draw();
        mobile_table.row(row.index()).remove().draw();
        updateOrderTotal();
    });
    $(document).on('change', 'input[data-field="orderQty"]', function () {
        updateTotal($(this).closest('tr'));
    });

    $('button.submitOrder').on('click', function () {
        var orderData = {
            customer_Id: $('#customer').val(),
            shipToAddress_Id: $('#shipTo').val(),
            orderDate: $('#orderDate').val(),
            orderComment: '', // Add a textarea for comments if needed
            OrderItems: []
        };

        $('#dt2_product tbody tr').each(function () {
            var row = $(this);
            var item = {
                partNum: row.find('.product-id-field').text(),
                product_Id: row.find(".product-id-field").attr("data-id"),
                lineDesc: row.find('td:eq(1)').text(),
                ium: row.find('td:eq(4)').text(),
                salesUm: row.find('td:eq(4)').text(), // Assuming salesUm is the same as ium
                unitPrice: parseFloat(row.find('span[data-field="price"]').text()),
                orderQty: parseFloat(row.find('input[data-field="orderQty"]').val()),
                discount: 0, // Add a field for discount if needed
                listPrice: parseFloat(row.find('span[data-field="price"]').text()) // Assuming listPrice is the same as unitPrice
            };
            orderData.OrderItems.push(item);
        });

        // Send the data to the server
        $.ajax({
            url: $("#SubmitOrderUrl").val(), // Update with your controller and action names
            type: 'POST',
            data: JSON.stringify(orderData),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    // alert('Order submitted successfully!');
                    userMessage.show("Success", "The order submitted successfully!");
                    $("#back-orders").trigger("click");
                    // Optionally, redirect or clear the form
                } else {
                    alert('Error submitting order: ' + response.message);
                }
            },
            error: function () {
                alert('An error occurred while submitting the order.');
            }
        });
    });

    $(".submit-action").on("click", function () {
        if (!$('.order-action').val()) {
            alert("Please select action to submit");
            return false;
        }
        var orderId = $(this).attr("data-id");
        $.ajax({
            url: $("#UpdateOrderStatusUrl").val(), // Update with your controller and action names
            type: 'POST',
            data: JSON.stringify({
                orderId: orderId,
                newStatus: $('.order-action').val(),
                actionName: $('.order-action').val()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    // alert('Order submitted successfully!');
                    userMessage.show("Success", "The order submitted successfully!");
                    $("#back-orders").trigger("click");
                    // Optionally, redirect or clear the form
                } else {
                    alert('Error submitting order: ' + response.message);
                }
            },
            error: function () {
                alert('An error occurred while submitting the order.');
            }
        });
    })
    // Cart functions end

    // Payment functions begins

    $('#addPaymentBtn').on('click', function () {
        var newRow = `
            <tr>
                <td>
                <select class="form-control paymentType">
                    <option value="Cash" selected>Cash</option>
                    <option value="Credit Card">Credit Card</option>
                    <option value="Debit Card">Debit Card</option>
                    <option value="Online Banking">Online Banking</option>
                    <option value="Online Banking">Others</option>
                  </select>
                <input type="hidden" name="OrderId" value="@Model.id" class="orderId" /></td>
                <td><input type="text" class="form-control bank"></td>
                <td><input type="text" class="form-control referenceNo"></td>
                <td><input type="number" class="form-control amount" step="0.01"></td>
                <td><button class="btn btn-danger btn-sm removePayment"><i class="fa fa-times"></i></button></td>
            </tr>
        `;
        $('#paymentTable tbody').append(newRow);
        updatePaymentTotal();
    });

    $(document).on('click', '.removePayment', function () {
        $(this).closest('tr').remove();
        updatePaymentTotal();
    });

    $(document).on('input', '.amount', function () {
        updatePaymentTotal();
    });

    function updatePaymentTotal() {
        var total = 0;
        $('.amount').each(function () {
            total += parseFloat($(this).val()) || 0;
        });
        $('#paymentTotal').text(total.toFixed(2));
    }

    $('#completePaymentBtn').on('click', function () {
        var paymentData = gatherPaymentData();
        if (paymentData.length > 0) {
            sendPaymentDataToApi(paymentData);
        } else {
            alert('Please add at least one payment before completing.');
        }
    });

    function gatherPaymentData() {
        var payments = [];
        $('#paymentTable tbody tr').each(function () {
            var row = $(this);
            var payment = {
                OrderId: row.find('.orderId').val(),
                PaymentType: row.find('.paymentType').val(),
                Bank: row.find('.bank').val(),
                RefNumber: row.find('.referenceNo').val(),
                Amount: parseFloat(row.find('.amount').val()) || 0,
                PaymentDate: new Date().toISOString(),
                PaymentStatus: 'Pending' // You can change this as needed
            };
            if (payment.Amount > 0) {
                payments.push(payment);
            }
        });
        return payments;
    }

    function sendPaymentDataToApi(paymentData) {
        $.ajax({
            url: $("#UpdateOrderPaymentsUrl").val(), // Update with your actual API endpoint
            type: 'POST',
            data: JSON.stringify({ OrderPayments: paymentData }),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    alert('Payment completed successfully!');
                    // You can add more actions here, like refreshing the page or updating the UI
                } else {
                    alert('Error completing payment: ' + response.message);
                }
            },
            error: function () {
                alert('An error occurred while processing the payment.');
            }
        });
    }

    // Payment functions end

    $('#orderDate').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true
    });

    // Initialize DataTable for the product picker modal
    var table = $('#dt_product_picker').DataTable({
        "paging": true,
        "ordering": false,
        "info": false,
        "searching": false,
        "responsive": true,
        "columns": [
            { "data": null, "defaultContent": "<input type='checkbox' class='product-checkbox'>" },
            { "data": "partNumber" },
            { "data": "description" },
            { "data": "prodGrup_Description" },
            { "data": "part_IUM" },
            { "data": "unitPrice" },
            { "data": null, "defaultContent": "<input type='number' class='default-qty' value='1'>" },
        ]
    });

    // Add selected products to table

    $('#btnAddToOrders').on('click', function () {
        // Find all checked checkboxes in the products table
        var selectedProducts = [];

        table.$('input[type="checkbox"]:checked').each(function () {
            var row = $(this).closest('tr');
            var data = table.row(row).data();
            var orderQty = row.find("input.default-qty").val();
            data['orderQty'] = orderQty;
            selectedProducts.push({
                data: data,
                row: row
            });
        });

        // Do something with the selected products data
        console.log('Selected products:', selectedProducts);
        selectedProducts.forEach(function (product) {
            addOrUpdateProductRow(product.data, desktop_table, mobile_table);
            // Remove the product from the original products table
            table.row(product.row).remove().draw(false);
        });
    });

    // Apply the search filter on input change
    $('.filter-input').on('keyup', function () {
        table.column($(this).parent().index()).search(this.value).draw();
    });

    // Show the modal on Add button click
    $('#btnAdd').on('click', function () {
        // $('#productPickerModal').modal('show');
        $(".productPicker").show();
        setTimeout(function () {
            $('#dt_product_picker').DataTable().columns.adjust().responsive.recalc();
        }, 600);
    });

    $(".closeProductPicker").on("click", function () {
        $(".productPicker").hide();
    })

    // Initialize DataTable for the customer picker modal
    var customerTable = $('#dt_customer_picker').DataTable({
        "paging": true,
        "ordering": true,
        "info": true,
        "searching": true,
        "responsive": true
    });

    // Apply the search filter on input change
    $('.filter-input').on('keyup', function () {
        var column = $(this).parent().index();
        if (table === productTable) {
            productTable.column(column).search(this.value).draw();
        } else if (table === customerTable) {
            customerTable.column(column).search(this.value).draw();
        }
    });

    // Handle the select/deselect all checkboxes for customers
    $('#selectAllCustomers').on('click', function () {
        var isChecked = $(this).prop('checked');
        $('.customer-checkbox').prop('checked', isChecked);
    });

    // Show the modal for customer picker
    $('#btnCustomerPicker').on('click', function () {
        $('#customerPickerModal').modal('show');

        setTimeout(function () {
            $('#dt_customer_picker').DataTable().columns.adjust().responsive.recalc();
        }, 500);
    });

    // Product search begins
    $('#productSearch').on('focus', function () {
        $(this).select();
    });
    // Trigger AJAX call when search input is filled
    $('#productSearch').on('change', function () {
        var searchTerm = $(this).val();

        if (searchTerm.length >= 2) { // Start searching after at least 2 characters
            $.ajax({
                url: $("#GetProductByPartNumUrl").val(), // Replace with your API endpoint
                type: 'POST',
                data: { partNum: searchTerm },
                success: function (response) {
                    if (response.success && response.product && response.product.id) {
                        // Clear the existing table rows
                        // $('#dt2_product tbody').empty();

                        // Assuming response.product is an array of product objects
                        let product = response.product;
                        // Create a new row for each product
                        addOrUpdateProductRow(product, desktop_table, mobile_table);
                    } else {
                        alert('No products found or an error occurred.');
                    }
                },
                error: function () {
                    alert('Failed to fetch products.');
                }
            });
        }
    });

    // Customer search
    // $('.selectCustomer').select2();

    $.ajax({
        url: $("#SearchCustomersUrl").val(),
        type: "POST",
        data: { 'searchParam': "" },
        dataType: "json",
        success: function (data) {
            var $select = $('.selectCustomer');
            $.each(data.customers, function (index, item) {
                $select.append(new Option(item.name, item.id));
                // $select.select2('data', { id: item.id, text: item.name });
            });

            // Initialize Select2 after options are added
            $select.select2();
            $select.val(1).trigger('change.select2');
            // Set default value
            $select.val('cash').trigger('change');
        }
    });

    $('#btnSearch').on('click', function () {
        var partNo = $('#searchPartNo').val();
        var code = $('#searchCode').val();
        var desc = $('#searchDesc').val();

        $.ajax({
            url: $("#SearchProductsUrl").val(), // Replace with your actual API endpoint
            type: 'POST',
            data: { partNum: partNo, code: code, description: desc },
            dataType: 'json',
            success: function (response) {
                if (response.success && response.products) {
                    // Clear existing table data
                    table.clear();

                    // Add new data to the table
                    table.rows.add(response.products).draw();

                    // Reset the "select all" checkbox
                    $('#selectAll').prop('checked', false);
                } else {
                    alert('No products found or an error occurred.');
                }
            },
            error: function () {
                alert('Failed to fetch products.');
            }
        });
    });

    $('#btnReset').on('click', function () {
        $('#searchPartNo, #searchCode, #searchDesc').val('');
        table.clear().draw();
    });

    // Handle "select all" checkbox
    $('#selectAll').on('click', function () {
        $('.product-checkbox').prop('checked', this.checked);
    });


    // Product search ends

});