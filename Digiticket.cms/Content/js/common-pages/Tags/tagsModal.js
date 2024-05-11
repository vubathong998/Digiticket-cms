var tagModal = function () {
    return {
        init: function () {
            this.onSelect2();
            this.onValidate();
        },
        onSelect2: function () {
            $('.kt-select2-category').select2({
                placeholder: "Tìm danh mục"
            });
            $('.kt-select2-destination').select2({
                placeholder: "Tìm điểm đến"
            });
        },
        onValidate: function () {
            $.validator.addMethod(
                'not0',
                function (value) {
                    return !/0/g.test(value);
                },
                'Code chứa ít nhất một ký tự chữ'
            );
            $('#TagsUpdateIdx').validate({
                rules: {
                    name: {
                        required: true
                    },
                    textView: {
                        required: true
                    },
                },
                messages: {
                    name: {
                        required: "không được bỏ trống",
                    },
                    textView: {
                        required: "không được bỏ trống",
                    },
                }
            });
        }
    }
}()