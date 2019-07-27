
$(function() {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    //Date range picker
    $('#date-submit').daterangepicker();
    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
    $('button[type=submit]').click(function() {
        if ($(this).hasClass('disabled') === false) {
            showLoader();
            var checkSHowEmpty = $('input[type="checkbox"].minimal')[0];
            var showEmpty = checkSHowEmpty.checked;
            submitRequest(showEmpty);
        }
    });

    function showLoader() {
        $('table[key=data-table] tbody').remove();
        $('.loading-results').removeClass("hide");
        $('.div-message').text("");
        $('button[type=submit]').addClass("disabled");
    }

    function hideLoader() {
        $('.loading-results').addClass("hide");
        $('button[type=submit]').removeClass("disabled");
    }

    function submitRequest(showEmpty) {
        var query = '';
        if (showEmpty !== null && showEmpty !== 'undefined') {
            query = '?showEmptyRecords=' + showEmpty;
        }
        var url = baseUrl + 'api/frontend/getusers';
        jQuery.support.cors = true;
        $.ajax({
            url: url + query,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function(users) {
                var table = $('#example2').DataTable({
                    data: users,
                    retrieve: false,
                    "paging": false,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "autoWidth": true,
                    "search": true,
                    "searchable": true,
                    "pageLength": 25,
                    "lengthMenu": [[25, 50, 100, 500, 1000], [25, 50, 100, 500, 1000]],
                    "order": [[0, "desc"],[1, "asc"],[4, "asc"]],
                    columns: [
                        { "data": "UserRegistered" },
                        { "data": "UserFirstName" },
                        { "data": "UserEmail" },
                        { "data": "UserLogin" },
                        { "data": "UserApplication" },
                        //{ "data": "UserStatus" },
                        { "data": "ID" }
                    ],
                    "columnDefs": [
                        {
                            "targets": [1],
                            "render": function(data, type, full, meta) {
                                return data + ' ' + full.UserLastName;
                            }
                        },
                        {
                            "targets": [0],
                            "render": function(data, type, full, meta) {
                                return full.UserRegistered.split('T')[0] + ' ' + full.UserRegistered.split('T')[1];
                            }
                        },
                        {
                            "targets": [5],
                            "visible": showEmpty === false,
                            "orderable": false,
                            "render": function (data, type, full, meta) {
                                return '<button class=\'btn btn-sm bg-purple btn-show-meta\' type=\'button\'>Show Meta</button> <button class=\'btn btn-sm bg-olive btn-show-tracker\' type=\'button\'>Show Tracker</button>';
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
                hideLoader();
                
                if(showEmpty)
                    $('.hideForEmptyRecords').addClass('hidden');
                else
                    $('.hideForEmptyRecords').removeClass('hidden');

                $('button[type=button].btn-show-meta').click(function() {
                    if ($(this).hasClass("disabled") === true) {
                        return;
                    }
                    var row = $(this).closest('tr');
                    row.after($('#table-meta .tr-meta-editor').clone());
                    $("button.meta-close").click(function() {
                        $(this).closest('.tr-meta-editor').remove();
                        toggleEditRows(true);
                    });
                    toggleEditRows(false);
                    var data = table.row($(this).closest('tr')).data();
                    getUserMeta(data.ID);
                });

                $('button[type=button].btn-show-tracker').click(function() {
                    if ($(this).hasClass("disabled") === true) {
                        return;
                    }
                    var row = $(this).closest('tr');
                    row.after($('#table-meta .tr-tracker-editor').clone());
                    $("button.tracker-close").click(function() {
                        $(this).closest('.tr-tracker-editor').remove();
                        toggleEditRows(true);
                    });
                    toggleEditRows(false);
                    var data = table.row($(this).closest('tr')).data();
                    getUserTracker(data.ID);
                });

                //$('button[type=button].btn-show-login').click(function () {
                //    console.log('tracker');
                //    if ($(this).hasClass("disabled") === true) {
                //        return;
                //    }
                //    var row = $(this).closest('tr');
                //    row.after($('#table-meta .tr-tracker-editor').clone());
                //    $("button.tracker-close").click(function () {
                //        $(this).closest('.tr-tracker-editor').remove();
                //        toggleEditRows(true);
                //    });
                //    toggleEditRows(false);
                //    var data = table.row($(this).closest('tr')).data();
                //    getUserTracker(data.ID);
                //});

                function toggleEditRows(enable) {
                    var tr = $("#example2 tbody tr");
                    $.each(tr,
                        function(index, item) {
                            if (enable === true) {
                                $(item).find("button.btn-show-meta").removeClass("disabled");
                                $(item).find("button.btn-show-tracker").removeClass("disabled");
                            } else {
                                $(item).find("button.btn-show-meta").addClass("disabled");
                                $(item).find("button.btn-show-tracker").addClass("disabled");
                            }
                        });
                }
            },
            error: function (x, y, z) {
                if (x.status === 401) {
                    basics.logout();
                }
                $('.div-message').text("Something went wrong. Unable to fetch data.");
                hideLoader();
            }
        });
    }

    function getUserMeta(id) {
        var url = baseUrl + 'api/frontend/getmetarowbyuserid?id=' + id;
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function (userMeta) {
                if (userMeta.ProfilePhoto !== null && userMeta.ProfilePhoto !== 'undefined' && userMeta.ProfilePhoto.trim().length > 0) {
                    $('div.meta-profile-photo').html('<a href=\'' +
                        baseUrl +
                        userMeta.ProfilePhoto +
                        '\' target=\'blank\'><img src=\'' +
                        baseUrl +
                        userMeta.ProfilePhoto +
                        '\' width=\'25px\' height=\'25px\'></img></a>');
                }
                else $('div.meta-profile-photo').html('');
                if (userMeta.Gender !== null && userMeta.Gender !== 'undefined')
                    $('div.meta-gender').html(userMeta.Gender);
                $('div.meta-age').html(userMeta.Age);
                $('div.meta-weight-lose-goal').html(userMeta.WeightLossGoal);
                $('div.meta-address').html(userMeta.Address);
                $('div.meta-city').html(userMeta.City);
                $('div.meta-province').html(userMeta.State);
                $('div.meta-country').html(userMeta.Country);
                if (userMeta.ModifyDate !== null && userMeta.ModifyDate !== 'undefined') {
                    var modifyDates = userMeta.ModifyDate.split('T');
                    if (modifyDates !== null)
                        $('div.meta-modify-date').html(modifyDates[0] + ' ' + modifyDates[1]);
                }
                hideLoader();
            },
            error: function (x, y, z) {
                if (x.status === 401) {
                    basics.logout();
                }
                $('.div-message').text("Something went wrong. Unable to fetch data.");
                hideLoader();
            }
        });
    }

    function getUserTracker(id) {
        $('.tr-tracker-editor .box-body').html('');
        var url = baseUrl + 'api/frontend/gettrackerrowbyuserid?id=' + id;
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function(userTrackers) {
                var data = '';
                var count = 1;
                $.each(userTrackers,
                    function () {

                        var date = this.ModifyDate;
                        var dateOnly = '', timeOnly = '';
                        var modifyDates = [];
                        if (date !== null && date !== 'undefined') {
                            modifyDates = date.split('T');
                            dateOnly = modifyDates[0];
                            timeOnly = modifyDates[1];
                        }

                        data += '<p>';
                        data += '<div class="row">';
                        data += '<div></div>';
                        data += '<div><h4>' + dateOnly + '</h4></div>';
                        data += '</div>';
                        data += '</p>';
                        data += '<div class="row">';
                        data += '<div>Current weight</div>';
                        data += '<div>' + this.CurrentWeight + '</div>';
                        data += '</div>';

                        data += '<div class="row">';
                        data += '<div>T-Shirt size</div>';
                        data += '<div>';

                        var shirt = this.ShirtSize;
                        if (shirt !== null && shirt !== 'null' && shirt.length !== 4)
                            data += shirt;
                        data += '</div>';
                        data += '</div>';

                        data += '<div class="row">';
                        data += '<div>Modify date</div>';
                        if (modifyDates !== null)
                            data += '<div>' + dateOnly + ' ' + timeOnly + '</div>';
                        else
                            data += '<div>' + date + '</div>';
                        data += '</div>';

                        data += '<div class="row">';
                        data += '<div>Photo</div>';
                        data += '<div class=\'div-photo\'>';

                        var front = this.FrontImage;
                        if (front !== null && front !== 'null' && front.trim().length > 0 && front.length !== 4) {
                            data += '<a href=\'' +
                                baseUrl +
                                front +
                                '\' target=\'blank\'><img src=\'' +
                                baseUrl +
                                front +
                                '\'></img></a>';
                        } else data += ' ';
                        var side = this.SideImage;
                        if (side !== null && side !== 'null' && side.trim().length > 0 && side.length !== 4) {
                            data += '<a href=\'' +
                                baseUrl +
                                side +
                                '\' target=\'blank\'><img src=\'' +
                                baseUrl +
                                side +
                                '\'></img></a>';
                        } else data += ' ';
                        data += '</div>';
                        data += '</div>';
                        data += '<div class="row">';
                        data += '<div>Testimonial</div>';
                        data += '<br>';
                        data += '<div>';
                        if (this.AboutJourney !== null &&
                            this.AboutJourney !== 'null' &&
                            this.AboutJourney.length !== 4)
                            data += this.AboutJourney;
                        data += '</div>';
                        data += '</div>';

                        if (userTrackers.length > count) {
                            data += '<div class="row" style=\'border-bottom:1px solid #EEE; margin-bottom:15px;\'>';
                            data += '</div>';
                        }
                        count++;
                    });

                $('.tr-tracker-editor .box-body').html(data);
                hideLoader();
            },
            error: function (x, y, z) {
                if (x.status === 401) {
                    basics.logout();
                }
                $('.div-message').text("Something went wrong. Unable to fetch data.");
                hideLoader();
            }
        });
    }
    $('button[type=submit]').click();
});