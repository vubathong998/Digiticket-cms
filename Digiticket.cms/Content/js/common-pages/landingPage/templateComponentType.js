var templateComponentType = (function () {
    return {
        init: function () {
            this.onLoadTable();
            this.onFilterKey();
        },
        onLoadTable: function (key) {
            var keyRequest = '';
            if (key) keyRequest = `?key=${key}`;
            Common_another_ajax.get(`LandingPage/TemplateComponentTypeRendTable${keyRequest}`).then((data) => {
                $('.my-loading').hide();
                if (data.Success) $('#mainTable').html(data.Data);
                else
                    $('#mainTable').html(`
                    <h1>Có lỗi xảy ra!</h1>
                    <div>${data.Message}</div>
                `);
            });
        },
        onFilterKey: function () {
            $('input[name=key]').on('change', function (e) {
                let value = $(this).val();
                templateComponentType.onLoadTable(value);
            });
        },
        onRendAfterTemplateComponentTypeAddOrEdit: function (mess) {
            $('#defaultModal').modal('hide');
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: mess,
                showConfirmButton: false,
                timer: 1500
            });
            this.onLoadTable();
        },
        onTemplateComponentTypeEditModal: function (id) {
            loadModalContent(`${domain}/LandingPage/TemplateComponentTypeAddOrEditModal/${id}`, 'modal-lg');
        },
        onTemplateComponentTypeDelete: function (id, name) {
            Common_another_ajax.onDelete___get(`LandingPage/TemplateComponentTypeDelete/${id}`, name);
        }
    };
})();
