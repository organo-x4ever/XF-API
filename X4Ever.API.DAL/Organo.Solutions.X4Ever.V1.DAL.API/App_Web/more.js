$(document).ready(function () {
    let token = data.getQueryString("token");
    $("input[name=token]").val(token);
    data.hideLoader();
    data.alertClear();
    $(".btn-feedback").click(function (e) {
        data.displayInfo("btn-feedback", "click");
        e.preventDefault();
        data.alertClear();
        if (data.validate()) {
            data.displayInfo("validate()", "CHECK:TRUE");
            data.upload(data.post);
        }
        else {
            data.displayInfo("validate()", "CHECK:FALSE");
            data.alertError("Please provide some information to submit");
            data.hideLoader();
        }
    });
});
    
function ValidateSingleInput(oInput) {
    if (data.fileValidate(oInput)) {
        let sFileName = oInput.value;
        data.fileExists = false;
        for (let j = 0; j < data._validFileExtensions.length; j++) {
            let sCurExtension = data._validFileExtensions[j];
            if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() === sCurExtension.toLowerCase()) {
                data.fileExists = true;
                break;
            }
        }

        if (!data.fileExists) {
            data.alertError("Sorry, " + sFileName + " is invalid, allowed extensions are: " + data._validFileExtensions.join(", "));
            oInput.value = "";
            return false;
        }
    }
    return true;
}

var data = {
    baseUrl: "https://mapp.oghq.ca/",
    message: $("div[name=message]"),
    _validFileExtensions: [".jpg", ".jpeg", ".bmp", ".gif", ".png"],
    fullName: $("input[name=fullname]"),
    userEmail: $("input[name=useremail]"),
    experience: $("textarea[name=experience]"),
    comments: $("textarea[name=comments]"),
    attachedFile: $("input[name=attchedfile]"),
    inputAttachedFile: $("#inputFile"),
    allowAccess: $("input[name=allowaccess]"),
    inputToken: $("input[name=token]"),
    fileExists: false,
    post: function () {
        data.displayInfo("posting...", "data");
        let userFeedback = {
            FullName: $(data.fullName).val(),
            Email: $(data.userEmail).val(),
            Experience: $(data.experience).val(),
            Comments: $(data.comments).val(),
            AttachedFileName: $(data.attachedFile).val(),
            AllowAccess: $(data.allowAccess).val(),
            Token: $(data.inputToken).val()
        };
        data.displayInfo("userFeedback:",userFeedback);
        let jsonData = JSON.stringify(userFeedback);
        data.displayInfo("baseUrl:",data.baseUrl);
        let url = data.baseUrl + "api/logs/feedback";
        data.displayInfo("url:",url);
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: jsonData,
            success: function (response) {
                data.displayInfo('response:', response);
                if (response.indexOf("Success") > -1) {
                    data.displayInfo("RESULT:", "Posted");
                    data.alertSuccess("Thank you for your valuable feedback");
                    data.clearInput();
                } else {
                    data.displayInfo("RESULT:", "Not Posted");
                    data.alertWarning("Your feedbacks are valuable for us. Try again later");
                }
                data.hideLoader();
            },
            error: function (x, y, z) {
                data.displayInfo('x:', x);
                data.alertError(x.responseText);
                data.hideLoader();
            }
        });
    },
    upload: function (callback) {
        data.showLoader();
        if (data.fileExists) {
            //var myID = 3; //uncomment this to make sure the ajax URL works
            if (data.inputAttachedFile.length > 0) {
                if (window.FormData !== undefined) {
                    var formData = new FormData();
                    formData.append("file", $(data.inputAttachedFile)[0].files[0] /*$('#inputFile')[0].files[0]*/);
                    let url = data.baseUrl + "api/files/upload/web?postedFileName=" + data.getDateAsString();
                    $.ajax({
                        url: url,
                        type: "POST",
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        headers: {
                            "Token": $(data.inputToken).val()
                        },
                        success: function (result) {
                            data.displayInfo("RESULT:", result);
                            if (result === "MessageFileUploadFailed") {
                                data.alertError("File upload failed");
                            } else if (result.indexOf("MessageErrorOccurred") > -1) {
                                data.alertError(result.replace("MessageErrorOccurred#", ""));
                            } else if (result.indexOf("MAIN") > -1) {
                                data.alertError(result.replace("MAIN#", ""));
                            } else if (result.indexOf("Success#") > -1) {
                                $(data.attachedFile).val(result.replace("Success#", ""));
                                callback();
                                return;
                            }
                            data.hideLoader();
                        },
                        error: function (xhr, status, p3, p4) {
                            data.displayInfo("Error:", xhr, status, p3, p4);
                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] === "{")
                                err = JSON.parse(xhr.responseText).Message;
                            data.displayInfo("Error:", err);
                        },
                        complete: function () {
                            //on complete event
                        },
                        progress: function (evt) {
                            //progress event
                        },
                        ///Ajax events
                        beforeSend: function (e) {
                            //before event
                        }
                    });
                } else {
                    data.alertWarning("This browser doesn't support HTML5 file uploads!");
                    callback();
                }
            } else {
                callback();
            }
        } else {
            callback();
        }
    },

    clearInput: function () {
        $(data.fullName).val("");
        $(data.userEmail).val("");
        $(data.experience).val("");
        $(data.comments).val("");
        $(data.attachedFile).val("");
        $(data.allowAccess).val("");
    },

    // Getting query string value by key name
    getQueryString: function (name) {
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
    removeQueryString: function () {
        history.pushState(null, "", location.href.split("?")[0]);
    },

    showLoader: function () {
        $('body').find("#feedback").addClass('hidden');
        $('.loading-results').removeClass("hidden");
    },

    hideLoader: function () {
        $('body').find("#feedback").removeClass('hidden');
        $('.loading-results').addClass("hidden");
    },
    alertClear: function () {
        $(data.message).find("span").html("");
        $(data.message).removeClass("alert-danger");
        $(data.message).removeClass("alert-warning");
        $(data.message).removeClass("alert-info");
        $(data.message).removeClass("alert-success");
        $(data.message).find("h4").html("");
        //$(data.message).addClass("hidden");
    },
    alertError: function (text) {
        data.alertClear();
        $(data.message).find("span").html(text);
        $(data.message).addClass("alert-danger");
        //$(data.message).removeClass("hidden");
        $(data.message).find("h4").html("<i class=\"icon fa fa-ban\"></i>Alert!");
    },
    alertWarning: function (text) {
        data.alertClear();
        $(data.message).find("span").html(text);
        $(data.message).addClass("alert-warning");
        //$(data.message).removeClass("hidden");
        $(data.message).find("h4").html("<i class=\"icon fa fa-warning\"></i>Warning!");
    },
    alertInfo: function (text) {
        data.alertClear();
        $(data.message).find("span").html(text);
        $(data.message).addClass("alert-info");
        //$(data.message).removeClass("hidden");
        $(data.message).find("h4").html("<i class=\"icon fa fa-info\"></i>Info!");
    },
    alertSuccess: function (text) {
        data.alertClear();
        $(data.message).find("span").html(text);
        $(data.message).addClass("alert-success");
        //$(data.message).removeClass("hidden");
        $(data.message).find("h4").html("<i class=\"icon fa fa-check\"></i>Success!");
    },
    displayInfo: function (header, text) {
        //console.log(header + ":", text);
    },

    fileValidate: function (oInput) {
        data.displayInfo("fileValidate()", "INSIDE");
        if (oInput.type === "file") {
            let sFileName = oInput.value;
            if (sFileName.length > 0) {
                data.displayInfo("fileValidate()", "VALID");
                return true;
            }
        }
        data.displayInfo("fileValidate()", "INVALID");
        return false;
    },
    validate: function () {
        data.displayInfo("validate()", "INSIDE");
        if (data.isNotNull(data.experience) || data.isNotNull(data.comments) || data.fileExists /*data.isNotNull(data.fullName) || data.isNotNull(data.userEmail)*/) {
            data.displayInfo("validate()", "VALID");
            $(data.experience).parent("div").removeClass("has-error");
            $(data.comments).parent("div").removeClass("has-error");
            $(data.inputAttachedFile).parent("div").removeClass("has-error");
            return true;
        }
        data.displayInfo("validate()", "INVALID");
        $(data.experience).parent("div").addClass("has-error");
        $(data.comments).parent("div").addClass("has-error");
        $(data.inputAttachedFile).parent("div").addClass("has-error");
        return false;
    },

    getDateAsString: function () {
        var date = new Date();
        return data.addZero(date.getFullYear()) + "" +
            data.addZero(date.getMonth()) + "" +
            data.addZero(date.getDate()) + "" +
            data.addZero(date.getHours()) + "" +
            data.addZero(date.getMinutes()) + "" +
            data.addZero(date.getSeconds()) + "" +
            data.addZero(date.getMilliseconds());
    },

    addZero: function (text) {
        if (Number(text) < 10)
            return "0" + text;
        return text;
    },

    isNotNull: function (text) {
        data.displayInfo("DATA:", $(text));
        data.displayInfo("DATA.VALUE:", $(text).val());
        data.displayInfo("isNotNull()", "INSIDE");
        if ($(text).val() !== undefined && $(text).val() !== null && data.isExist(text)) {
            data.displayInfo("isNotNull()", "VALID");
            return true;
        }
        data.displayInfo("isNotNull()", "INVALID");
        return false;
    },

    isExist: function (data) {
        this.displayInfo("isExist()", "INSIDE");
        if (String($(data).val()).trim().length > 0) {
            this.displayInfo("isExist()", "VALID");
            return true;
        }
        this.displayInfo("isExist()", "INVALID");
        return false;
    }
};