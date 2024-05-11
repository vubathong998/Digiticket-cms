var mediaResize = (function () {
    return {
        init: function () {
            this.onShowRoot();
        },
        onShowRoot: function () {
            $('.my-loading').show();
            let _Id = $('[name=originalId]').val();
            $.ajax({
                url: `${domain}/Media/RenByRoot?Id=${_Id}`,
                dataType: 'json',
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                async: true,
                processData: false,
                cache: false,
                success: function (data) {
                    $('.my-loading').hide();
                    if (data.Success) {
                        $('#showByRoot').html(data.Data);
                    }
                },
                error: function (xhr) {
                    Common_another.showErr(xhr.status + ' ');
                }
            });
        },
        onRendAfterResizeMediaAction: function () {
            $('.my-loading').hide();
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: 'Resize thành công!',
                showConfirmButton: false,
                timer: 1500
            });
            mediaResize.init();
        }
    };
})();
