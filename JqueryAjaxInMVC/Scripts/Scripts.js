$(function () {
    $("#loaderbody").addClass('hide');


    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}


function jQueryAjaxPost(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {

        var ajaxconfig = {

            type: "POST",
            url: form.action,
            data: new FormData(form),
            success: function (response) {

                if (response.success) {
                    $("#firstTab").html(response.html);
                    refreshAddNewTab($(form).attr('data-restUrl'), true);
                    $.notify(response.message, "success");
                    if (typeof activateJqueryDataTable !== 'undefined' && $.isFunction(activateJqueryDataTable))
                        activateJqueryDataTable();
                }
                else {
                    $.notify(response.message, "error");
                }
              
              
            }
        }

        if ($(form).attr("enctype") == "multipart/form-data") {
            ajaxconfig["contentType"] = false;
            ajaxconfig["processData"] = false;

        }

        $.ajax(ajaxconfig);

    }
    return false;
}

function refreshAddNewTab(resetUrl, showViewTab) {
  
    $.ajax({
        type: "GET",
        url: resetUrl,
        success: function (response) {
            $("#secondTab").html(response);
            $("ul.nav.nav-tabs a:eq(1)").html("Add New");
            if (showViewTab) {
                $("ul.nav.nav-tabs a:eq(0)").tab("show");
            }
        }
    });
}
function Edit(url) {
    $.ajax({

        type: "GET",
        url: url,
        success: function (response) {
            $("#secondTab").html(response);
            $("ul.nav.nav-tabs a:eq(1)").html("Edit");
            $("ul.nav.nav-tabs a:eq(1)").tab("show");

        }

    })
}

function Delete(url) {
    if (confirm("Are you sure you want to delete the record?") == true) {
        $.ajax({
            type: "POST",
            url: url,
            success: function (response) {
                if (response.success) {
                    $("#firstTab").html(response.html);
                    $.notify(response.message, "warn");
                    if (typeof activateJqueryDataTable !== 'undefined' && $.isFunction(activateJqueryDataTable))
                        activateJqueryDataTable();
                }

                else {
                    $.notify(response.message, "warn");
                }
            }
        })
    }
    
}