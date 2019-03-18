
$(function () {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    //getwebusermetarealtimeasync';
    var realtimeTrackerUrl = 'api/frontend/getwebusermetarealtime2018async';
    var refreshWait = 60;
    var minDate = '2018-10-15';
    var fetchRecordOnLoad = false;
    refreshWaitSeconds(checkToLoad);
    var skipUptoId = 0;
    var userTrackers = [{}];
    var dateString = '';
    //Date picker
    $.fn.datepicker.defaults.format = "yyyy-mm-dd";
    $('#date-submit').datepicker({
        //format: 'yyyy-mm-dd',
        startDate: '2018-10-15'
    });
    $('button[type=submit].button-search').click(function() {
        if ($(this).hasClass('disabled') === false) {
            userTrackers = [{}];
            skipUptoId = 0;
            dateString = '';
            try {
                if ($('#date-submit').val() !== null && $('#date-submit').val() !== 'undefined') {
                    dateString = $('#date-submit').val();
                    if (dateString.indexOf('/') > -1) {
                        var splits = dateString.split('/');
                        dateString = splits[2] + '-' + splits[0] + '-' + splits[1];
                    }
                }
            } catch (err) {
                //
            }
            showLoader();
            $('.td-start-weight').html('');
            $('.td-weekly-weight').html('');
            submitRequest();
        }
    });

    function checkToLoad() {
        if (fetchRecordOnLoad) {
            $('#date-submit').val(minDate);
            $('button[type=submit].button-search').click();
        }
    }

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
        var query = '?fromDate=' + dateString;
        var url = baseUrl + realtimeTrackerUrl;
        jQuery.support.cors = true;
        $.ajax({
            url: url + query,
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
                }else if (trackers !== null && trackers.length > 0) {
                    $.each(trackers,
                        function(index, tracker) {
                            var i = tracker.ID - 1;
                            userTrackers[i] = tracker;
                        });
                    skipUptoId = userTrackers[userTrackers.length - 1].ID;
                    try {
                        var date = userTrackers[userTrackers.length - 1].TrackerLastDate;
                        dateString = date.split('T')[0] + ' ' + date.split('T')[1];
                    } catch (err) {
                        //
                    }
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
                        "order": [[0, "desc"]],
                        columns: [
                            { "data": "ID" },
                            { "data": "user_first_name" },
                            { "data": "user_email" },
                            { "data": "application_name" },
                            { "data": "country" },
                            { "data": "tracker_start_date" },
                            { "data": "tracker_last_date" },
                            { "data": "start_weight" },
                            { "data": "weekly_weight_lost" },
                            { "data": "total_weight_lost" }
                        ],
                        "columnDefs": [
                            {
                                "targets": [0],
                                "visible": false
                            },
                            {
                                "targets": [5],
                                "render": function(data, type, full, meta) {
                                    var date = '';
                                    try {
                                        date = full !== null &&
                                            full !== 'undefined' &&
                                            full.tracker_start_date !== null &&
                                            full.tracker_start_date !== 'null' &&
                                            full.tracker_start_date !== 'undefined'
                                            ? (full.tracker_start_date.split('T')[0] +
                                                ' ' +
                                                full.tracker_start_date.split('T')[1]
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
                                            full.tracker_last_date !== null &&
                                            full.tracker_last_date !== 'null' &&
                                            full.tracker_last_date !== 'undefined'
                                            ? (full.tracker_last_date.split('T')[0] +
                                                ' ' +
                                                full.tracker_last_date.split('T')[1]
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
                                "targets": [1],
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
                        "footerCallback": function(row, data, start, end, display) {
                            var api = this.api(), data;

                            // Remove the formatting to get integer data for summation
                            var intVal = function(i) {
                                return typeof i === 'string'
                                    ? i.replace(/[\$,]/g, '') * 1
                                    : typeof i === 'number'
                                    ? i
                                    : 0;
                            };

                            // COLUMN: 7
                            // Total over all pages
                            total_7 = api
                                .column(7)
                                .data()
                                .reduce(function(a, b) {
                                        return parseFloat(intVal(a) + intVal(b)).toFixed(2);
                                    },
                                    0);

                            // Total over this page
                            pageTotal_7 = api
                                .column(7, { page: 'current' })
                                .data()
                                .reduce(function(a, b) {
                                        return parseFloat(intVal(a) + intVal(b)).toFixed(2);
                                    },
                                    0);

                            // Update footer
                            $(api.column(7).footer()).html(
                                parseFloat(pageTotal_7).toFixed(2) + 'kg (' + parseFloat(total_7).toFixed(2) + 'kg)'
                            );


                            // COLUMN: 8
                            // Total over all pages
                            total_8 = api
                                .column(8)
                                .data()
                                .reduce(function(a, b) {
                                        return parseFloat(intVal(a) + intVal(b)).toFixed(2);
                                    },
                                    0);

                            // Total over this page
                            pageTotal_8 = api
                                .column(8, { page: 'current' })
                                .data()
                                .reduce(function(a, b) {
                                        return parseFloat((intVal(a) + intVal(b))).toFixed(2);
                                    },
                                    0);

                            // Update footer
                            $(api.column(8).footer()).html(
                                parseFloat(pageTotal_8).toFixed(2) + 'kg (' + parseFloat(total_8).toFixed(2) + 'kg)'
                            );


                            // COLUMN: 9
                            // Total over all pages
                            total_9 = api
                                .column(9)
                                .data()
                                .reduce(function(a, b) {
                                        return parseFloat((intVal(a) + intVal(b))).toFixed(2);
                                    },
                                    0);

                            // Total over this page
                            pageTotal_9 = api
                                .column(9, { page: 'current' })
                                .data()
                                .reduce(function(a, b) {
                                        return parseFloat((intVal(a) + intVal(b))).toFixed(2);
                                    },
                                    0);

                            // Update footer
                            $(api.column(9).footer()).html(
                                parseFloat(pageTotal_9).toFixed(2) + 'kg (' + parseFloat(total_9).toFixed(2) + 'kg)'
                            );

                        },
                        destroy: true
                    });

                    //if (userTrackers !== null) {
                    //    $('.td-start-weight').html('');
                    //    $('.td-weekly-weight').html('');
                    //    var startWeight = 0, weeklyWeight = 0;
                    //    $.each(userTrackers,
                    //        function() {
                    //            startWeight += parseFloat(this.StartWeight);
                    //            weeklyWeight += parseFloat(this.WeeklyWeightLost);
                    //        });
                    //    var weekly = parseFloat(startWeight) - parseFloat(weeklyWeight);
                    //    console.log(weekly);
                    //    $('.td-start-weight').html('Start Weight: ' + parseFloat(startWeight));
                    //    $('.td-weekly-weight').html('Start - Weekly = ' + weekly);
                    //}
                }
                hideLoader();
                $('.div-message').text('');
                setTimeout(submitRequest, refreshWait * 1000);
            },
            error: function (x, y, z) {
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

    function refreshWaitSeconds(callback) {
        var configUrl = baseUrl + 'realtime/realtime-tracking-config.json';
        jQuery.support.cors = true;
        $.ajax({
            url: configUrl,
            type: 'GET',
            dataType: 'json',
            success: function(config) {
                refreshWait = config.webuserrefreshwaitseconds;
                minDate = config.minDate;
                fetchRecordOnLoad = config.fetchRecordOnLoad;
                callback();
                return;
            },
            error: function(x, y, z) {
                //console.log(x);
                //console.log(y);
                //console.log(z);
            }
        });
    }
});