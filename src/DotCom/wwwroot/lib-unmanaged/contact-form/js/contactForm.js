var ContactForm = ContactForm || {};

ContactForm.SubmitUrl = "";

ContactForm.resetForm = function () {
    $('input').each(function () {
        $(this).val('');
    });

    $('#message').val('');
    $('#charCount').text('0/1000');

    $("#contactFormToggle").show();
    $("#contactForm").slideUp();
}

ContactForm.showToast = function(message, error) {
    toastr[error ? "error" : "success"](message);
}

ContactForm.validate = function () {
    var emailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var phoneRegex = /[0-9\-\(\)\s]+/;
    var valid = true;
    var message = "<ul>";

    $('.required').each(function () {
        if (!$(this).val()) {
            $(this).addClass('warning');
            valid = false;
        }
    });

    if (!valid) {
        message += "<li>Please fill in missing fields.</li>";
    }

    if (!emailRegex.test($('#email').val())) {
        $('#email').addClass('warning');
        message += "<li>Please enter a valid email address</li>";
        valid = false;
    }

    if (!phoneRegex.test($('#phone').val())) {
        $('#phone').addClass('warning');
        message += "<li>Please enter a valid US phone number</li>";
        valid = false;
    }

    if ($('#message').val().length > 1000) {
        $('#message').addClass('warning');
        message += "<li>Please reduce your message to under 1000 characters.</li>";
        valid = false;
    }

    if (!valid) {
        ContactForm.showToast(message + "</ul>", true);
    }

    return valid;
}

ContactForm.submit = function () {
    var contactFormViewModel = {
        FirstName: $('#firstName').val(),
        LastName: $('#lastName').val(),
        Email: $('#email').val(),
        Phone: $('#phone').val(),
        Message: $('#message').val()
    };

    $.post(ContactForm.SubmitUrl, contactFormViewModel, function (result) {
        if (result) {
            ContactForm.showToast("Message sent successfully! We'll be in touch soon <i class=\"fa fa-smile-o\"></i>");
            ContactForm.resetForm();
        }

        else {
            ContactForm.showToast("Message NOT sent!", true);
        }
    });
}

$(document).ready(function () {
    $('#playVideo').click(function () {
        $('#videoModal').modal('show');
        $('#videoModal iframe').attr('src', '/videos/explainer_HD1080.mp4');
    });

    $('#videoModal').on('hidden.bs.modal', function () {
        $('#videoModal iframe').removeAttr('src');
    })

    $('#message').keyup(function () {
        $(this).removeClass('warning');

        var count = $(this).val().length;
        $('#charCount').text(count + '/1000');
    });

    $("#contactFormToggle").click(function () {
        $("#contactFormToggle").hide();
        $("#contactForm").slideDown();
    });

    $("#contactFormCancel").click(function () {
        ContactForm.resetForm();
    });

    $('.required').each(function () {
        $(this).change(function () {
            $(this).removeClass('warning');
        })
    });

    $("#contactFormSubmit").click(function () {
        if(!ContactForm.validate()){
            return false;
        }

        ContactForm.submit();
    });
});