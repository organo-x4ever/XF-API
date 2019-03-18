// Original JavaScript code by Chirp Internet: www.chirp.com.au
// Please acknowledge use of this code by including this header.
var cookie = {
    tokenHeader: 'Token',
    dateFormat: 'yyyy-MM-dd hh:mm:ss',
    today: function() {
        return new Date();
    },
    toDate: function (date) {
        date = new Date(date);
        //var normalized = date.replace(/[^a-zA-Z0-9]/g, '-');
        //var normalizedFormat = format.toLowerCase().replace(/[^a-zA-Z0-9]/g, '-');
        //var formatItems = normalizedFormat.split('-');
        //var dateItems = normalized.split('-');

        //var monthIndex = formatItems.indexOf("mm");
        //var dayIndex = formatItems.indexOf("dd");
        //var yearIndex = formatItems.indexOf("yyyy");
        //var hourIndex = formatItems.indexOf("hh");
        //var minutesIndex = formatItems.indexOf("ii");
        //var secondsIndex = formatItems.indexOf("ss");

        //var year = yearIndex > -1 ? dateItems[yearIndex] : date.getFullYear();
        //var month = monthIndex > -1 ? dateItems[monthIndex] - 1 : date.getMonth() - 1;
        //var day = dayIndex > -1 ? dateItems[dayIndex] : date.getDate();

        //var hour = hourIndex > -1 ? dateItems[hourIndex] : date.getHours();
        //var minute = minutesIndex > -1 ? dateItems[minutesIndex] : date.getMinutes();
        //var second = secondsIndex > -1 ? dateItems[secondsIndex] : date.getSeconds();

        var year = date.getFullYear();
        var month = date.getMonth();
        var day = date.getDate();

        var hour = date.getHours();
        var minute = date.getMinutes();
        var second = date.getSeconds();

        return new Date(year, month, day, hour, minute, second);
    },
    //var expiry = new Date(today.getTime() + 30 * 24 * 3600 * 1000); // plus 30 days
    setCookie: function(name, value, expiry) {
        document.cookie = name + "=" + escape(value) + "; path=/; expires=" + expiry.toGMTString();
    },

    getCookie: function(name) {
        var re = new RegExp(name + "=([^;]+)");
        var value = re.exec(document.cookie);
        var returnObject = (value !== null) ? unescape(value[1]) : null;
        //if (returnObject != null) {
        //    setCookie(name, returnObject);
        //}
        var val = returnObject;
        return val;
    },

    expired: new Date(new Date().getTime() - 24 * 3600 * 1000), // less 24 hours
    deleteCookie: function(name) {
        document.cookie = name + "=null; path=/; expires=" + this.expired.toGMTString();
    },
    GetStringFromByteArray: function(array) {
        var result = "";
        if (typeof array !== 'undefined' && array !== null)
            for (var i = 0; i < array.length; i++) {
                for (var j = 0; j < array[i].length; j++)
                    result += String.fromCharCode(array[i][j]);
            }
        return result;
    }
};