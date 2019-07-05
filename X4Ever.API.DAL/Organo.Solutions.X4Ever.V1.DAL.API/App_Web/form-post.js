"use strict"

$(function () {
    console.log("on load function");
    data.post();
});

var data = {
    baseUrl:"https://mapp.oghq.ca/",
    post: function () {
        $(".div-msg").html("Posting...");
        console.log("posting... data");
        let fullname = data.getQueryString("fullname");
        let useremail = data.getQueryString("useremail");
        let experience = data.getQueryString("experience");
        let comments = data.getQueryString("comments");
        let attachedfile = data.getQueryString("attchedfile");
        let allowaccess = data.getQueryString("allowaccess");
        let token = data.getQueryString("token");
        let userFeedback = {
            FullName: fullname,
            Email: useremail,
            Experience: experience,
            Comments: comments,
            AttachedFileName: attachedfile,
            AllowAccess: allowaccess,
            Token: token
        };
        let jsonData = JSON.stringify(userFeedback);
        let url = data.baseUrl + "api/logs/feedback";
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: jsonData,
            success: function (response) {
                console.log('response:', response);
                if (response.indexOf("Success") > -1) {
                    console.log("Posted");
                    data.removeQueryString();
                } else {
                    console.log("Not Posted");
                }
                $(".div-msg").html("Thank you for your valuable feedback");
                //hideModalLoader();
                //$('#model-notification').modal('hide');
            },
            error: function (x, y, z) {
                //if (x.status === 401) {
                //    basics.logout();
                //}
                console.log('x:', x);
                //$('.div-model-message').text(x.responseText);
                //hideModalLoader();
            }
        });
    },
    check: function () {

    },
    
    // Getting query string value by key name
    getQueryString: function(name) {
        let vars = [],
            hash;
        let hashes = window.location.href.slice(window.location.href.indexOf("?") + 1).split("&");
        for (let i = 0; i < hashes.length; i++) {
            hash = hashes[i].split("=");
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars[name];
    },

    // Removing query string from url
    removeQueryString: function() {
        history.pushState(null, "", location.href.split("?")[0]);
    },
};