
$(function() {
    console.log("INITIAL");
    var baseUrl = "https://mapp.oghq.ca";
    var url=`${baseUrl}/api/news/get`;
    function showLoader() {
        //$('table[key=data-table] tbody').remove();
        $('.loading-results').removeClass("hide");
        //$('.div-message').text("");
    }

    function hideLoader() {
        $(".loading-results").addClass("hide");
    }

    submitRequest();

    function submitRequest() {
        var token = getQueryString("token");
        var theme = getQueryString("theme");
        var application = getQueryString("application");
        var languageCode = getQueryString("languagecode");
        var active = getQueryString("active");
        console.log("Query String: ",token,theme,languageCode,active);
        console.log("URL: ",url);
        url = url+`?languageCode=${languageCode}&active=${active}`;
        jQuery.support.cors = true;
        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            headers: {
                "Token": token,
                "Application": application
            },
            success: function (data) {
                console.log(data);
                var html="";
                $.each(data,
                    function () {
                        html += "<div class=\"box box-default blog\">";
                        html += "<div class=\"box-body\">";
                        html += "<div class=\"row\">";
                        html += "<div class=\"col-sm-12\">";
                        html += "<div class=\"blog-header\">" + this.Header + "</div>";
                        html += "<div class=\"blog-date\">" + this.PostDate + "</div>";
                        if (this.NewsImage !== undefined && this.NewsImage !== null && this.NewsImage.trim().length > 0) {
                            html += "<div class=\"blog-image\">";
                            html += "<img src=\"" + this.NewsImage + "\" class=\"img-responsive center\" alt=\"Responsive image\"></img>";
                            html += "</div>";
                        }
                        html += "<div class=\"blog-body\">" + this.Body + "</div>>";
                        if (this.PostedBy !== undefined && this.PostedBy !== null) {
                            html += "<div class=\"blog-footer\">" + this.PostedBy + "</div>";
                        }
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                        html += "</div><!-- /.box -->";
                    }
                );


                $('.blog-content').html(html);
                hideLoader();
            },
            error: function (x, y, z) {
                console.log("Error: ", x, y, z);
                if (x.status === 401) {
                    $('.div-message').text("You are unauthorized");
                    return;
                }
                $('.div-message').text("Something went wrong. Unable to fetch data.");
                hideLoader();
            }
        });
    }    
    //Getting query string value by key name
    function getQueryString(name) {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars[name];
    }
});