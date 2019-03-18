
$(function () {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    //Date range picker
    $('#date-submit').daterangepicker();
    var userData = null;
    loadWithCurrentDate(submitRequest);
    $('button[type=submit].button-search').click(function() {
        if ($(this).hasClass('disabled') === false) {
            submitRequest();
        }
    });
    $('button[type=button].button-send-notification').click(function() {
        if ($(this).hasClass('disabled') === false) {
            showModelNotification();
        }
    });
    $('button[type=button].button-save').click(function() {
        if ($(this).hasClass('disabled') === false) {
            if (Validate()) {
                showModalLoader();
                var url = baseUrl + 'api/frontend/postpushnotificationasync';
                var checkEmail = $('#checkEmail');
                var checkIOS = $('#checkIOS');
                var checkAndroid = $('#checkAndroid');
                var checkAttachFooter = $('#checkEmailFooter');
                var platforms = '';
                if (checkEmail[0].checked) {
                    platforms += 'email';
                }
                if (checkIOS[0].checked) {
                    if (platforms.trim().length > 0)
                        platforms += ',';
                    platforms += 'ios';
                }
                if (checkAndroid[0].checked) {
                    if (platforms.trim().length > 0)
                        platforms += ',';
                    platforms += 'android';
                }

                var query =
                    '?authCode=' +
                        $('#input-auth-code').val().trim() +
                        '&emailIDs=' +
                        $('#input-emails').val().trim() +
                        '&sendToPlatforms=' +
                        platforms +
                        '&notificationTitle=' +
                        $('#notificationTitle').val().trim() +
                        '&notificationBody=' +
                        $('#notificationBody').val().trim() +
                        '&emailSubject=' +
                        $('#emailSubject').val().trim() +
                        '&emailBody=' +
                        $('#emailBody').val().trim() +
                        '&attachFooter=' +
                        checkAttachFooter[0].checked;
                jQuery.support.cors = true;
                $.ajax({
                    url: url + query,
                    type: 'POST',
                    dataType: 'json',
                    headers: {
                        'Token': token
                    },
                    success: function (response) {
                        hideModalLoader();
                        $('#model-notification').modal('hide');
                    },
                    error: function(x, y, z) {
                        if (x.status === 401) {
                            basics.logout();
                        }
                        $('.div-model-message').text(x.responseText);
                        hideModalLoader();
                    }
                });
            }
        }
    });

    function Validate() {
        var message = true;
        //var error = '';
        if ($('#input-emails').val().trim().length === 0) {
            $('#input-emails').parent('.form-group').addClass('has-error');
            message = false;
        } else {
            $('#input-emails').parent('.form-group').removeClass('has-error');
        }
        if ($('#input-auth-code').val().trim().length === 0) {
            $('#input-auth-code').parent().parent('.form-group').addClass('has-error');
            message = false;
        } else {
            $('#input-auth-code').parent().parent('.form-group').removeClass('has-error');
        }
        var checkEmail = $('#checkEmail');
        var checkIOS = $('#checkIOS');
        var checkAndroid = $('#checkAndroid');
        if (!checkEmail[0].checked && !checkIOS[0].checked && !checkAndroid[0].checked) {
            $('#checkEmail').parent().parent().parent('.form-group').addClass('has-error');
            $('#checkIOS').parent().parent().parent().parent('.form-group').addClass('has-error');
            $('#checkAndroid').parent().parent().parent().parent('.form-group').addClass('has-error');
            message = false;
            //error += 'Email/iOS/Android must be selected';
        } else {
            $('#checkEmail').parent().parent().parent('.form-group').removeClass('has-error');
            $('#checkIOS').parent().parent().parent().parent('.form-group').removeClass('has-error');
            $('#checkAndroid').parent().parent().parent().parent('.form-group').removeClass('has-error');
            if (checkEmail[0].checked) {
                if ($('#emailSubject').val().trim().length === 0) {
                    $('#emailSubject').parent('.form-group').addClass('has-error');
                    message = false;
                } else {
                    $('#emailSubject').parent('.form-group').removeClass('has-error');
                }
                if ($('#emailBody').val().trim().length === 0) {
                    $('#emailBody').parent('.form-group').addClass('has-error');
                    message = false;
                } else {
                    $('#emailBody').parent('.form-group').removeClass('has-error');
                }
            }
            if (checkIOS[0].checked || checkAndroid[0].checked) {
                if ($('#notificationTitle').val().trim().length === 0) {
                    $('#notificationTitle').parent('.form-group').addClass('has-error');
                    message = false;
                } else {
                    $('#notificationTitle').parent('.form-group').removeClass('has-error');
                }
                if ($('#notificationBody').val().trim().length === 0) {
                    $('#notificationBody').parent('.form-group').addClass('has-error');
                    message = false;
                } else {
                    $('#notificationBody').parent('.form-group').removeClass('has-error');
                }
            }
        }
        //if (error.trim().length > 0) {
        //    $('.div-model-message').text(error);
        //}
        return message;
    }

    function showLoader() {
        $('table[key=data-table] tbody').remove();
        $('.loading-results').removeClass("hide");
        $('.div-message').text("");
        $('button.button-search').addClass("disabled");
        $('button.button-send-notification').addClass("disabled");
    }

    function hideLoader() {
        $('.loading-results').addClass("hide");
        $('button.button-search').removeClass("disabled");
        $('button.button-send-notification').removeClass("disabled");
    }

    function showModalLoader() {
        $('.model-loading-results').removeClass("hide");
        $('.div-model-message').text("");
        $('button[type=button].button-save').addClass("disabled");
        $('button.btn-default').addClass("disabled");
    }

    function hideModalLoader() {
        $('.model-loading-results').addClass("hide");
        $('button[type=button].button-save').removeClass("disabled");
        $('button.btn-default').removeClass("disabled");
    }

    function submitRequest() {
        showLoader();
        var query = '';
        if ($('#date-submit').val() !== null && $('#date-submit').val() !== 'undefined') {
            var dateString = $('#date-submit').val();
            if (dateString !== null && dateString.length > 0) {
                var splits = dateString.split('-');
                if (splits.length === 2)
                    query = '?fromDate=' + splits[0].trim() + '&toDate=' + splits[1].trim();
            }
        }
        var url = baseUrl + 'api/frontend/getbydateasync';
        jQuery.support.cors = true;
        $.ajax({
            url: url + query,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function(users) {
                userData = users;
                var table = $('#example2').DataTable({
                    data: userData,
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
                    "order": [[0, "asc"]],
                    columns: [
                        {"data": "SentDatetime"},
                        { "data": "FirstName" },
                        { "data": "NotificationType" },
                        { "data": "UserPlatform" },
                        { "data": "StatusDescription" },
                        { "data": "NotificationBody" }
                    ],
                    "columnDefs": [
                        {
                            "targets": [0],
                            "render": function (data, type, full, meta) {
                                return full.SentDatetime.split('T')[0] + ' ' + full.SentDatetime.split('T')[1];
                            }
                        },
                        {
                            "targets": [1],
                            "render": function(data, type, full, meta) {
                                var html;
                                html = data + ' ' + full.LastName;
                                html += '<br>';
                                html += full.UserEmail;
                                return html;
                            }
                        },
                        {
                            "targets": [2],
                            "render": function(data, type, full, meta) {
                                return full.NotificationType +
                                    '<br>' +
                                    (full.IsPush
                                        ? 'Push'
                                        : 'E-Mail') +
                                    '<br>' +
                                    (full.IsScheduled
                                        ? 'Scheduled'
                                        : 'Custom');
                            }
                        },
                        {
                            "targets": [5],
                            "render": function (data, type, full, meta) {
                                return full.IsPush
                                    ? data
                                    : '<a href=' +
                                    baseUrl +
                                    'view/body/?key=' +
                                    full.ID +
                                    ' target="_blank">Show Message Body</a>';
                                //return data !== null && data !== 'null'
                                //    ? '<a id=\"button-notification\" class="btn btn-primary center button-notification" data-text=' +
                                //    full.ID +
                                //    '>Show</a>'
                                //    : '';
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
                $('.button-notification').click(function() {
                    var index = $(this).data('text');
                    var users = getObjects(userData, 'ID', index);
                    showModel(users[0]);
                });

                hideLoader();
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
        var date = user.SentDatetime.split('T')[0] + ' ' + user.SentDatetime.split('T')[1];
        $('.modal-title').text(user.FirstName + ' ' + user.LastName + ' ' + date);
        $('.modal-body p').text(user.NotificationBody);
        $('#model-testimonial').modal('show');
    }

    function showModelNotification() {
        $('.modal-title').text('Push Notification');
        $('#model-notification').modal('show');
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

function checkPlatform(checkboxElem) {
    changeView($(checkboxElem).data('value'), checkboxElem.checked);
}

function changeView(platform, show) {
    var checkIOS = $('#checkIOS');
    var checkAndroid = $('#checkAndroid');
    if (show) {
        // Do something special
        if (platform === 'email') {
            $('.form-email-subject').removeClass('hide');
            $('.form-email-body').removeClass('hide');
            $('.form-email-footer').removeClass('hide');
        } else if (platform === 'ios' || platform === 'android') {
            if (checkIOS[0].checked || checkAndroid[0].checked) {
                $('.form-notification-title').removeClass('hide');
                $('.form-notification-body').removeClass('hide');
            }
        }
    } else {
        // Do something else
        if (platform === 'email') {
            $('.form-email-subject').addClass('hide');
            $('.form-email-body').addClass('hide');
            $('.form-email-footer').addClass('hide');
        } else if (platform === 'ios' || platform === 'android') {
            if (!checkIOS[0].checked && !checkAndroid[0].checked) {
                $('.form-notification-title').addClass('hide');
                $('.form-notification-body').addClass('hide');
            }
        }
    }
}