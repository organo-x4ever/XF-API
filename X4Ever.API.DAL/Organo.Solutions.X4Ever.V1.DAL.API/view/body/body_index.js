$(function() {
    var baseUrl = basics.baseUrl;
    var token = cookie.getCookie(cookie.tokenHeader);
    if (token === null || token === 'undefined' || token.trim().length === 0) {
        location.replace(baseUrl + "view/index.html");
    }
    function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    }

    var url = baseUrl + 'api/frontend/getnotificationbodybykeyasync?key=' + getUrlParameter('key');
    jQuery.support.cors = true;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        headers: {
            'Token': token
        },
        success: function(users) {
            //var response = users[0].NotificationBody.split('body>');
            //var html = '<div>' + response[1] + 'div>';
            $('body').html(users[0].NotificationBody);
        },
        error: function(x, y, z) {
            console.log(x);
            console.log(y);
            console.log(z);
        }
    });
});