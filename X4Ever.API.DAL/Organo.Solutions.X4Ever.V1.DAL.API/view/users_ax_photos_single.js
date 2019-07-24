
$(function() {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    var page, size, maxSize = 50;
    loadPage();
    function loadPage() {
        var data = getQueryString("page");
        if (data !== undefined && data !== null) {
            data = data.replace("#", "");
        }
        if (data === undefined || data === null || isNaN(data)) {
            page = 1;
            addQueryString("page", page);
        } else page = Number(data);

        if (page === undefined || page === null || page < 1) {
            page = 1;
            addQueryString("page", page);
        }
        if (size === undefined || size === null || page < maxSize) {
            size = maxSize;
        }
        submitRequestTable(page);
    }
    
    function showLoader() {
        $('.loading-results').removeClass("hide");
        $('.div-message').text("");
        $('.table').addClass("disabled");
    }

    function hideLoader() {
        $('.loading-results').addClass("hide");
        $('.table').removeClass("disabled");
    }

    $('.paginate_button.next').click(function () {
        if($('.paginate_button.next').hasClass("disabled"))
            return;
       setPageSize(1);
    });
    
    $('.paginate_button.previous').click(function () {
        if($('.paginate_button.previous').hasClass("disabled"))
            return;
        setPageSize(-1);
    });

    function setPageSize(add) {
        if (add === -1)
            page = Number(page) - 1;
        else
            page = Number(page) + 1;
        addQueryString("page", page);
        loadPage();
    }

    function submitRequestTable(p) {
        showLoader();
        var url = baseUrl + "api/frontend/all_photos_model_single?page=" + p + "&size=" + size;
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function (users, textStatus, request) {

                var table = $('#example2').DataTable({
                    data: users,
                    retrieve: false,
                    "paging": false,
                    "lengthChange": false,
                    "searching": false,
                    "ordering": true,
                    "info": false,
                    "autoWidth": true,
                    "search": false,
                    "searchable": false,
                    "pageLength": 10,
                    "lengthMenu": [[10], [10]],
                    columns: [
                        { "data": "COL_1" },
                        { "data": "COL_2" },
                        { "data": "COL_3" },
                        { "data": "COL_4" },
                        { "data": "COL_5" },
                        { "data": "COL_6" },
                        { "data": "COL_7" },
                        { "data": "COL_8" },
                        { "data": "COL_9" },
                        { "data": "COL_10" }
                    ],
                    "columnDefs": [
                        {
                            "targets": [0],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                        {
                            "targets": [1],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [2],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [3],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [4],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [5],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [6],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [7],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [8],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        },
                     {
                            "targets": [9],
                            "render": function(data, type, full, meta) {
                                return "<a href=\"" + data + "\" target=\"_blank\" class=\""+(data!==null?"":"hidden")+"\"><img src=\"" + data + "\" width=\"80px\" height=\"100px\" /></a>";
                            }
                        }
                    ],
                    destroy: true
                });
                var count = request.getResponseHeader('count');
                var previous = request.getResponseHeader('previous');
                var next = request.getResponseHeader('next');
                if (previous !== null && previous !== undefined) {
                    if (Number(previous) === 0) {
                        $('.paginate_button.previous').addClass("disabled");
                    }
                    else {
                        $('.paginate_button.previous').removeClass("disabled");
                    }
                } else {
                    $('.paginate_button.previous').removeClass("disabled");
                }
                if (next !== null && next !== undefined) {
                    if (Number(next) === 0) {
                        $('.paginate_button.next').addClass("disabled");
                    }
                    else {
                        $('.paginate_button.next').removeClass("disabled");
                    }
                } else {
                    $('.paginate_button.next').removeClass("disabled");
                }

                $(".dataTables_info").html("Showing " + (page) + " of " + (Number(count) / Number(maxSize)).toFixed(0));

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

    // Getting query string value by key name
    function getQueryString(name) {
        let vars = [],
            hash;
        let hashes = window.location.href.slice(window.location.href.indexOf("?") + 1).split("&");
        for (let i = 0; i < hashes.length; i++) {
            hash = hashes[i].split("=");
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars[name];
    }
    function addQueryString(name, value) {
        removeQueryString();
        if (history.pushState) {
            var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + "?" + name + "=" + value;
            window.history.pushState({ path: newurl }, '', newurl);
        }
    }
    function removeQueryString() {
        history.pushState(null, "", location.href.split("?")[0]);
    }
});