var basics = {
    baseUrl: 'https://mapp.oghq.ca/',
    currentDate: function() {
        var date = new Date();
        var d = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
        return d;
    },
    logout: function() {
        var baseUrl = this.baseUrl;
        var token = cookie.getCookie(cookie.tokenHeader);
        if (token !== null && token !== 'undefined' && token.trim().length > 0) {
            this.logoutAction(token);
        }
        cookie.deleteCookie(cookie.tokenHeader);
        location.replace(baseUrl + "view/index.html");
    },
    logoutAction: function(token) {
        try {
            var url = this.baseUrl + 'api/user/PostAuthTokenKill';
            jQuery.support.cors = true;
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                headers: {
                    'Token': token
                },
                success: function(response) {

                },
                error: function(x, y, z) {
                    //$('.div-model-message').text(x.responseText);
                }
            });
        } catch (ex) {
            // 
        }
    }
};
$(function() {
    $('body').find('a.button-logout').click(function() {
        basics.logout();
    });
});
