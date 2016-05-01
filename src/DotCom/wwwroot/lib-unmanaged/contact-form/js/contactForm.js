var ContactForm = ContactForm || {
    urls: {
            submitUrl: ""
        },

    initialize: function (submitUrl) {
        this.urls.submitUrl = submitUrl;
    },

    hide: function () {
        $("#contactFormToggle").show();
        $("#contactForm").slideUp();
    },

    show: function () {
        $("#contactFormToggle").hide();
        $("#contactForm").slideDown();
    },

    resetForm: function () {
        $('input').each(function () {
            $(this).val('');
        });

        $('#message').val('');
        $('#charCount').text('0/1000');
        this.hide();
        $('#contactFormSubmit').removeClass('active');
    },

    showToast: function (message, error) {
        toastr[error ? "error" : "success"](message);
    },

    validate: function () {
        var emailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        var phoneRegex = /[0-9\-\(\)\s]+/;
        var valid = true;
        var message = "<ol>";

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
            this.showToast(message + "</ol>", true);
        }

        return valid;
    },

    submit: function () {
        $('#contactFormSubmit').addClass('active');

        var self = this;
        var contactFormViewModel = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Email: $('#email').val(),
            Phone: $('#phone').val(),
            Message: $('#message').val()
        };

        $.post(self.urls.submitUrl, contactFormViewModel, function (result) {
            if (result) {
                self.showToast("Message sent successfully! We'll be in touch soon <i class=\"fa fa-smile-o\"></i>");
                self.resetForm();
            }

            else {
                self.showToast("Message NOT sent!", true);
                $('#contactFormSubmit').removeClass('active');
            }
        });
    }
};

$(document).ready(function () {
    $('#message').keyup(function () {
        $(this).removeClass('warning');

        var count = $(this).val().length;
        $('#charCount').text(count + '/1000');
    });

    $("#contactFormToggle").click(function () {
        ContactForm.show();
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
        if (!ContactForm.validate()) {
            return false;
        }

        ContactForm.submit();
    });
});