
$(function() {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    
    submitRequestTable();
    
    function showLoader() {
        $('.loading-results').removeClass("hide");
        $('.div-message').text("");
        $('.table').addClass("disabled");
    }

    function hideLoader() {
        $('.loading-results').addClass("hide");
        $('.table').removeClass("disabled");
    }

    function submitRequestTable() {
        showLoader();
        var url = baseUrl + "api/frontend/listOfFeedbacks";
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function (users, textStatus, request) {
                var jsonUsers = JSON.parse(users);
                var table = $('#example2').DataTable({
                    data: jsonUsers,
                    retrieve: true,
                    "paging": false,
                    "lengthChange": false,
                    "searching": false,
                    "ordering": true,
                    "info": true,
                    "autoWidth": true,
                    "search": false,
                    "searchable": false,
                    "pageLength": 10,
                    "lengthMenu": [[10], [10]],
                    "order":[[0,"desc"],[1, "asc"]],
                    columns: [
                        { "data": "Date" },
                        { "data": "FullName" },
                        { "data": "Email" },
                        { "data": "Experience" },
                        { "data": "Comments" },
                        { "data": "AllowAccess" },
                        { "data": "AttachedFileName" },
                        { "data": "ID" },
                        { "data": "Token" }
                    ],
                    "columnDefs": [
                        {
                            "targets":[5],
                            "render": function (data, type, full, meta) {
                                return data !== null && data !== "null" && data.length > 0 && data === "on" ? "ON" : "OFF";
                            }
                        },
                        {
                            "targets": [6],
                            "render": function (data, type, full, meta) {
                                if (data !== null && data !== "null" && data.length > 0) {
                                    return "<a href=\"" + baseUrl + data + "\" target=\"_blank\"><img width src=\"" + baseUrl + data + "\" width=\"60px\" height=\"75px\" /></a>";
                                }
                                return "";
                            }
                        },
                        {
                            "targets": [7],
                            "render": function (data, type, full, meta) {
                                if (data !== null && data !== "null" && data.length > 0) {
                                    return "<button class=\"btn bg-red btn-delete-feedback\" type=\"button\">Delete</button>";
                                }
                                return "";
                            }
                        },
                        {
                            "targets": [8],
                            "visible":false
                        }
                    ],
                    destroy: true
                });
                
                $('button[type=button].btn-delete-feedback').click(function() {
                    if ($(this).hasClass("disabled") === true) {
                        return;
                    }
                    var data = table.row($(this).closest('tr')).data();
                    deleteFeedback(data.ID);
                });

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

    function deleteFeedback(id) {
        showLoader();
        var url = baseUrl + "api/frontend/deleteFeedback?id=" + id;
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function (users, textStatus, request) {
                window.open("user_feedbacks.html","_parent");
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
});