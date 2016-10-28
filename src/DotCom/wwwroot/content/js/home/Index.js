(function () {
    'use strict';

    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    $('a[href*="#"]:not([href="#"])').click(function () {
        if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('.mdl-layout__content').animate({
                    scrollTop: target.get(0).offsetTop
                }, 1000);
                return false;
            }
        }
    });

    var snackbar = document.querySelector('#oa-home-toast');

    $('#message').on('keyup', function () {
        var countRemaining = 1000 - $('#message').val().length;
        $('#message-count-remaining').text(countRemaining + ' / 1000').removeClass('error');

        if (countRemaining < 0) {
            $('#message-count-remaining').text(countRemaining + ' / 1000 (message is too long)').addClass('error');
        }
    });

    $('#oa-submitform').click(function () {
        var isValid = true;
        $('.required').each(function () {
            if ($(this).val().length === 0) {
                isValid = false;
            }
        });

        if (!isValid) {
            snackbar.MaterialSnackbar.showSnackbar({ message: "Please complete all required fields." });
            return false;
        }

        var data = {
            __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val(),
            Email: htmlEncode($('#email').val()), // HTML Encode
            FirstName: htmlEncode($('#firstName').val()),
            LastName: htmlEncode($('#lastName').val()),
            Message: htmlEncode($('#message').val()),
            Phone: htmlEncode($('#phoneNumber').val())
        };

        $.ajax({
            url: '@Url.Action("SubmitForm", "Home")',
            type: 'POST',
            data: data,
            success: function (data) {
                if (data) {
                    snackbar.MaterialSnackbar.showSnackbar({ message: "Message sent! We'll be in touch shortly." });
                    $('#email').val('');
                    $('#firstName').val('');
                    $('#lastName').val('');
                    $('#message').val('');
                    $('#phoneNumber').val('');
                    $('#message-count-remaining').text(1000 - $('#message').val().length + ' / 1000').removeClass('error');
                }

                else {
                    snackbar.MaterialSnackbar.showSnackbar({ message: "Something went wrong! Please contact by phone, text, or email." });
                }
            },
            error: function () {
                snackbar.MaterialSnackbar.showSnackbar({ message: "Something went wrong! Please contact by phone, text, or email." });
            }
        });
    });
}());

function isElementInViewport(el) {

    if (typeof jQuery === "function" && el instanceof jQuery) {
        el = el[0];
    }

    var rect = el.getBoundingClientRect();

    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
}

$('.mdl-layout__content').scroll(function () {
    if (isElementInViewport($('#oa-services'))) {
        if ($('#oa-services i').hasClass('animated')) {
            return false;
        }

        $('#oa-services i').addClass('animated').addClass('tada');
    }

    if (isElementInViewport($('#oa-pricing'))) {
        if ($('#oa-pricing i').hasClass('animated')) {
            return false;
        }

        $('#oa-pricing i').addClass('animated').addClass('zoomIn');
    }
});
