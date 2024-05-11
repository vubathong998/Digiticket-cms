var TagsUpDateIdxModal = (function () {
    return {
        init: function () {
            this.onValidate();
        },
        onValidate: function () {
            $.validator.addMethod(
                'numberGreaterThan0',
                function (value) {
                    return /^[\d]+$/g.test(value);
                },
                'chứa ít nhất một ký tự chữ'
            );
            $('#TagsUpdateIdx').validate({
                rules: {
                    idx: {
                        required: true,
                        numberGreaterThan0: true
                    }
                },
                messages: {
                    idx: {
                        required: "không được bỏ trống",
                        numberGreaterThan0: "Chỉ số và khác 0"
                    }
                }
            });
        }
    }
})()