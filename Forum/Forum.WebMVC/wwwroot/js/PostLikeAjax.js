$(document).ready(function () {
    LikePostAjax = (form, id) => {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $(`#likesP-${id}`).html(res.count);
                return false;
            },
            error: function (err) {
                alert("An error occured.");
            }
        })
        return false;
    };
});

$(document).ready(function () {
    LikeCommentAjax = (form, id) => {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $(`#likesC-${id}`).html(res.count);
                return false;
            },
            error: function (err) {
                alert("An error occured.");
            }
        })
        return false;
    };
});