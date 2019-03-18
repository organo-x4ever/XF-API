
$(function() {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    //Date range picker
    $('#date-submit').daterangepicker();
    var userData = null;
    loadWithCurrentDate(submitRequest);
    $('button[type=submit]').click(function() {
        if ($(this).hasClass('disabled') === false) {
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
        showLoader();
        var query = '';
        if ($('#date-submit').val() !== null && $('#date-submit').val() !== 'undefined') {
            var dateString = $('#date-submit').val();
            if (dateString !== null && dateString !== 'undefined' && dateString.length > 0) {
                var splits = dateString.split('-');
                if (splits.length === 2)
                    query = '?fromDate=' + splits[0].trim() + '&toDate=' + splits[1].trim();
            }
        }
        var url = baseUrl + 'api/frontend/gettrackerdetailasync';
        jQuery.support.cors = true;
        $.ajax({
            url: url + query,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function (users) {
                userData = users;
                var table = $('#example2').DataTable({
                    data: userData,
                    retrieve: false,
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "autoWidth": true,
                    "search": true,
                    "searchable": true,
                    "pageLength": 1000,
                    "lengthMenu": [[25, 50, 100, 500, 1000], [25, 50, 100, 500, 1000]],
                    "order": [[4, "desc"]],
                    columns: [
                        { "data": "FirstName" },
                        { "data": "CreateDate" },
                        { "data": "ApplicationName" },
                        { "data": "Address" },
                        { "data": "TrackerCreateDate" },
                        { "data": "StartWeight" },
                        { "data": "TShirtSize" },
                        { "data": "FrontPhoto" },
                        { "data": "Testimonials" }
                    ],
                    "columnDefs": [
                        {
                            "targets": [0],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                var html;
                                html = data + ' ' + full.LastName;
                                html += '<br/>';
                                html += full.EmailAddress;
                                if (full.Gender !== null && full.Gender !== 'null') {
                                    html += '<br/>';
                                    html += full.Gender;
                                }
                                return html;
                            }
                        },
                        {
                            "targets": [1],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                return full.CreateDate.split('T')[0] + ' ' + full.CreateDate.split('T')[1];
                            }
                        },
                        {
                            "targets": [3],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                return full.Address +
                                    '<br/>' +
                                    full.City +
                                    '<br/>' +
                                    full.State + ' ' + full.PostalCode +
                                    '<br/>' +
                                    full.Country;
                            }
                        },
                        {
                            "targets": [4],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                return data.split('T')[0] + '<br>' + data.split('T')[1];
                            }
                        },
                        {
                            "targets": [5],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                return '<b>Start: </b>' +
                                    data +
                                    full.WeightVolumeType +
                                    '<br/>' +
                                    '<b>To Lose: </b>' +
                                    full.WeightToLose +
                                    full.WeightVolumeType +
                                    '<br/>' +
                                    '<b>Goal Reached: </b>' +
                                    full.WeightGoalReached +
                                    full.WeightVolumeType +
                                    '<br/>' +
                                    '<b>Current:</b> <b style="color:green; width:50px;">' +
                                    full.WeeklyWeightLost +
                                    full.WeightVolumeType +
                                    '</b>';
                            }
                        },
                        {
                            "targets": [6],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                return data !== null && data !== 'null' ? data : '';
                            }
                        },
                        {
                            "targets": [7],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                return (full.FrontPhoto !== null && full.FrontPhoto !== 'null'
                                        ? '<a href=' + baseUrl + full.FrontPhoto + ' target="_blank">Front Photo</a>'
                                        : '') +
                                    (full.SidePhoto !== null && full.SidePhoto !== 'null'
                                        ? '<br>' +
                                        '<a href=' +
                                        baseUrl +
                                        full.SidePhoto +
                                        ' target="_blank">Side Photo</a>'
                                        : '');
                            }
                        },
                        {
                            "targets": [8],
                            //"searchable": true,
                            //"orderable": true,
                            "render": function(data, type, full, meta) {
                                return data !== null && data !== 'null'
                                    ? '<a id=\"button-testimonial\" class="btn button-search center button-testimonial" data-text=' +
                                    full.ID +
                                    '>Show</a>'
                                    : '';
                            }
                        }
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
                    destroy: true
                });

                //delegate
                $('.button-testimonial').click(function() {
                    var index = $(this).data('text');
                    var users = getObjects(userData, 'ID', index);
                    showModel(users[0]);
                });

                hideLoader();
            },
            error: function (x, y, z) {
                if (x.status === 401) {
                    basics.logout();
                } else {
                    $('.div-message').text("Something went wrong. Unable to fetch data.");
                }
                hideLoader();
            }
        });
    }

    function getObjects(obj, key, val) {
        var objects = [];
        for (var i in obj) {
            if (!obj.hasOwnProperty(i)) continue;
            if (typeof obj[i] === 'object') {
                objects = objects.concat(getObjects(obj[i], key, val));
            } else if (i === key && obj[key] === val) {
                objects.push(obj);
            }
        }
        return objects;
    }

    function showModel(user) {
        var date = user.TrackerCreateDate.split('T')[0] + ' ' + user.TrackerCreateDate.split('T')[1];
        $('.modal-title').text(user.FirstName + ' ' + user.LastName + ' ' + date);
        $('.modal-body p').text(user.Testimonials);
        $('#model-testimonial').modal('show');
    }

    function setItem(myObject) {
        localStorage.setItem("myObject", JSON.stringify(myObject));
    }

    function getItem() {
        return JSON.parse(localStorage.getItem("myObject"));
    }

    function loadWithCurrentDate(callback) {
        var d = basics.currentDate();
        $('#date-submit').val(d + ' - ' + d);
        callback();
    }
});