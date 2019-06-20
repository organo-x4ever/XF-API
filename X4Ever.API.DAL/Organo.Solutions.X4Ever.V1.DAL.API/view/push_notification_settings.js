
$(function () {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    submitRequest();

    function showLoader() {
        $('table[key=data-table] tbody').remove();
        $('.loading-results').removeClass("hide");
        $('.div-message').text("");
    }

    function hideLoader() {
        $('.loading-results').addClass("hide");
    }

    function submitRequest() {
        showLoader();
        var query = '';
        var url = baseUrl + 'api/frontend/getnotifysettings';
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
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "autoWidth": true,
                    "search": true,
                    "searchable": true,
                    "pageLength": 20,
                    "lengthMenu": [[20, 50, 100], [20, 50, 100]],
                    "order": [[0, "asc"]],
                    columns: [
                        { "data": "FirstName"},
                        { "data": "UserEmail"},
                        { "data": "IsWeightSubmitReminder"},
                        { "data": "IsVersionUpdate"},
                        { "data": "IsPromotional"},
                        { "data": "IsSpecialOffer"},
                        {"data": "IsGeneralMessage"},
                        { "data": "ModifyDate"}
                    ],
                    "columnDefs": [
                        {
                            "targets": [0],
                            "render": function(data, type, full, meta) {
                                return data + ' ' + full.LastName;
                            }
                        },
                        {
                            "targets": [2],
                            "render": function (data, type, full, meta) {
                                return data === true ? "Enabled" : "Disabled";
                            }
                        },
                        {
                            "targets": [3],
                            "render": function (data, type, full, meta) {
                                return data === true ? "Enabled" : "Disabled";
                            }
                        },
                        {
                            "targets": [4],
                            "render": function (data, type, full, meta) {
                                return data === true ? "Enabled" : "Disabled";
                            }
                        },
                        {
                            "targets": [5],
                            "render": function (data, type, full, meta) {
                                return data === true ? "Enabled" : "Disabled";
                            }
                        },
                        {
                            "targets": [6],
                            "render": function (data, type, full, meta) {
                                return data === true ? "Enabled" : "Disabled";
                            }
                        },
                        {
                            "targets": [7],
                            "render": function (data, type, full, meta) {
                                return full.ModifyDate.split('T')[0] + ' ' + full.ModifyDate.split('T')[1];
                            }
                        }
                    ],
                    destroy: true

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
});