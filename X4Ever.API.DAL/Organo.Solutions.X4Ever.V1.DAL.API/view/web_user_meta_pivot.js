
$(function() {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    var realtimeTrackerUrl = 'api/frontend/getwebusermetaasync';
    var userTrackers = [{}];
    submitRequest();
    $('button[type=submit].button-search').click(function() {
        if ($(this).hasClass('disabled') === false) {
            userTrackers = [{}];
            showLoader();
            submitRequest();
        }
    });

    function showLoader() {
        $('table[key=data-table] tbody').remove();
        $('.loading-results').removeClass("hide");
        $('.div-message').text("");
        $('button.button-search').addClass("disabled");
    }

    function hideLoader() {
        $('.loading-results').addClass("hide");
        $('button.button-search').removeClass("disabled");
    }

    function submitRequest() {
        userTrackers = [{}];
        showLoader();
        var url = baseUrl + realtimeTrackerUrl;
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function (trackers) {
                if (trackers === null || trackers.length === 0) {
                    $('.div-message').text('Please wait... it is taking longer than required');
                    setTimeout(submitRequest, 10 * 1000);
                    return;
                } else {
                    userTrackers = trackers;
                    var table = $('#example2').DataTable({
                        data: userTrackers,
                        retrieve: false,
                        "paging": false,
                        "lengthChange": true,
                        "searching": true,
                        "ordering": true,
                        "info": true,
                        "autoWidth": true,
                        "search": true,
                        "searchable": true,
                        "pageLength": 1000,
                        "lengthMenu": [[25, 50, 100, 500, 1000], [25, 50, 100, 500, 1000]],
                        "order": [[6, "desc"]],
                        columns: [
                            { "data": "ID" },
                            { "data": "user_id" },
                            { "data": "user_first_name" },
                            { "data": "user_email" },
                            { "data": "country" },
                            { "data": "user_registered" },
                            { "data": "update_date" },
                            { "data": "weekly_weight_lose" },
                            { "data": "weight_lose_goal" },
                            { "data": "weeks" }
                            //id	user_id	user_login	user_pass	user_nicename	user_email	user_url	user_registered	user_activation_key	user_first_name	userid	user_last_name	user_address	have_you_purchased	age	gender	weekly_weight_lose	weight_lose_goal	why_you_join	address	country	city	province	postal_code	weeks	weight_goal	status	lbs_lost	user_status	display_name	item_user_name	update_date	date_created
                        ],
                        "columnDefs": [
                            {
                                "targets": [0],
                                "visible": false
                            },
                            {
                                "targets": [1],
                                "visible": false
                            }, {
                                "targets": [5],
                                "render": function(data, type, full, meta) {
                                    var date = '';
                                    try {
                                        date = full !== null &&
                                            full !== 'undefined' &&
                                            full.user_registered !== null &&
                                            full.user_registered !== 'null' &&
                                            full.user_registered !== 'undefined'
                                            ? (full.user_registered.split('T')[0] +
                                                ' ' +
                                                full.user_registered.split('T')[1]
                                            )
                                            : '';
                                    } catch (err) {
                                        //
                                        date = '';
                                    }
                                    return date;
                                }
                            },
                            {
                                "targets": [6],
                                "render": function(data, type, full, meta) {
                                    var date = '';
                                    try {
                                        date = full !== null &&
                                            full !== 'undefined' &&
                                            full.update_date !== null &&
                                            full.update_date !== 'null' &&
                                            full.update_date !== 'undefined'
                                            ? (full.update_date.split('T')[0] +
                                                ' ' +
                                                full.update_date.split('T')[1]
                                            )
                                            : '';
                                    } catch (err) {
                                        //
                                        date = '';
                                    }
                                    return date;
                                }
                            },
                            {
                                "targets": [2],
                                "render": function(data, type, full, meta) {
                                    return data + ' ' + full.user_last_name;
                                }
                            },
                        ],
                        "tableTools": {
                            "aButtons": [
                                "copy",
                                "print",
                                {
                                    "sExtends": "collection",
                                    "sButtonText": "Save",
                                    "aButtons": ["csv", "xls", "pdf"]
                                }
                            ]
                        },
                        //"footerCallback": function(row, data, start, end, display) {
                        //    var api = this.api(), data;

                        //    // Remove the formatting to get integer data for summation
                        //    var intVal = function(i) {
                        //        return typeof i === 'string'
                        //            ? i.replace(/[\$,]/g, '') * 1
                        //            : typeof i === 'number'
                        //            ? i
                        //            : 0;
                        //    };

                        //    // COLUMN: 7
                        //    // Total over all pages
                        //    total_7 = api
                        //        .column(7)
                        //        .data()
                        //        .reduce(function(a, b) {
                        //                return parseFloat(intVal(a) + intVal(b)).toFixed(2);
                        //            },
                        //            0);

                        //    // Total over this page
                        //    pageTotal_7 = api
                        //        .column(7, { page: 'current' })
                        //        .data()
                        //        .reduce(function(a, b) {
                        //                return parseFloat(intVal(a) + intVal(b)).toFixed(2);
                        //            },
                        //            0);

                        //    // Update footer
                        //    $(api.column(7).footer()).html(
                        //        parseFloat(pageTotal_7).toFixed(2) + 'kg (' + parseFloat(total_7).toFixed(2) + 'kg)'
                        //    );


                        //    // COLUMN: 8
                        //    // Total over all pages
                        //    total_8 = api
                        //        .column(8)
                        //        .data()
                        //        .reduce(function(a, b) {
                        //                return parseFloat(intVal(a) + intVal(b)).toFixed(2);
                        //            },
                        //            0);

                        //    // Total over this page
                        //    pageTotal_8 = api
                        //        .column(8, { page: 'current' })
                        //        .data()
                        //        .reduce(function(a, b) {
                        //                return parseFloat((intVal(a) + intVal(b))).toFixed(2);
                        //            },
                        //            0);

                        //    // Update footer
                        //    $(api.column(8).footer()).html(
                        //        parseFloat(pageTotal_8).toFixed(2) + 'kg (' + parseFloat(total_8).toFixed(2) + 'kg)'
                        //    );


                        //    //// COLUMN: 9
                        //    //// Total over all pages
                        //    //total_9 = api
                        //    //    .column(9)
                        //    //    .data()
                        //    //    .reduce(function(a, b) {
                        //    //            return parseFloat((intVal(a) + intVal(b))).toFixed(2);
                        //    //        },
                        //    //        0);

                        //    //// Total over this page
                        //    //pageTotal_9 = api
                        //    //    .column(9, { page: 'current' })
                        //    //    .data()
                        //    //    .reduce(function(a, b) {
                        //    //            return parseFloat((intVal(a) + intVal(b))).toFixed(2);
                        //    //        },
                        //    //        0);

                        //    //// Update footer
                        //    //$(api.column(9).footer()).html(
                        //    //    parseFloat(pageTotal_9).toFixed(2) + 'kg (' + parseFloat(total_9).toFixed(2) + 'kg)'
                        //    //);

                        //},
                        destroy: true
                    });
                }
                hideLoader();
            },
            error: function(x, y, z) {
                if (x.status === 401) {
                    basics.logout();
                }
                $('.div-message').text(x.responseText);
                hideLoader();
            }
        });
    }

    function setItem(myObject) {
        localStorage.setItem("myObject", JSON.stringify(myObject));
    }

    function getItem() {
        return JSON.parse(localStorage.getItem("myObject"));
    }
});