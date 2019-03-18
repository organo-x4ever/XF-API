$(function() {
    var baseUrl = basics.baseUrl;
    console.log(baseUrl);
    var token = cookie.getCookie(cookie.tokenHeader);
    console.log(token);
    if (token !== null && token !== 'undefined' && token.trim().length > 0) {
        var url = baseUrl + 'api/user/PostAuthTokenKill';
        console.log(url);
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            headers: {
                'Token': token
            },
            success: function (response) {
                console.log(response);
                cookie.deleteCookie(cookie.tokenHeader);
            },
            error: function(x, y, z) {
                $('.div-model-message').text(x.responseText);
            }
        });
        location.replace(baseUrl + "view/index.html");
    }
});