var Account = function () {
    return {
        init: function () {
            this.onValidate();
        },
        onValidate: function () {
            jQuery.validator.addMethod("validPhoneEmail", function (value, element) {
                return this.optional(element) || /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})|^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(value);
            }, 'Email hoặc Số điện thoại không đúng');

            jQuery.validator.addMethod("validPhone", function (value, element) {
                return this.optional(element) || /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test(value);
            }, 'Số điện thoại không đúng');

            $('#kt_login_form').validate({
                rules: {
                    "Username": {
                        required: true,
                        email: true
                    },
                    "Password": {
                        required: true
                    }
                },
                messages: {
                    "Username": {
                        required: "Nhập Email hoặc Số điện thoại",
                        email: "Email không đúng"
                    },
                    "Password": {
                        required: "Nhập mật khẩu"
                    }
                }
            });

            $('#kt_register_form').validate({
                rules: {
                    "Username": {
                        required: true,
                        email: true
                    },
                    "Password": {
                        required: true,
                        minlength: 6
                    },
                    "ConfirmPassword": {
                        required: true,
                        minlength: 6,
                        equalTo: "#Password"
                    }
                },
                messages: {
                    "Username": {
                        required: "Nhập số điện thoại",
                        email: "Email không đúng"
                    },
                    "Password": {
                        required: "Nhập mật khẩu",
                        minlength: "Mật khẩu ít nhất 6 ký tự"
                    },
                    "ConfirmPassword": {
                        required: "Nhập lại mật khẩu",
                        minlength: "Mật khẩu ít nhất 6 ký tự",
                        equalTo: "Nhập lại mật khẩu không đúng"
                    }
                }
            });

            $('#kt_forgot_password_form').validate({
                rules: {
                    "Username": {
                        required: true,
                        email: true
                    }
                },
                messages: {
                    "Username": {
                        required: "Nhập Email hoặc Số điện thoại",
                        email: "Email không đúng"
                    }
                }
            });
        }
    };
}();

$(function () {
    Account.init();
});