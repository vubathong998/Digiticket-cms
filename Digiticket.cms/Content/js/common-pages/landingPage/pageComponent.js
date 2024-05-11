var pageComponent = (function () {
    return {
        init: function () {
            this.onShowPageComponentAddModal();
        },
        onShowPageComponentAddModal: function (id, viewImage) {
            loadModalContent(`${domain}/LandingPage/PageComponentAddOrEditModal?id=${id}&ViewImage=${viewImage}`, 'modal-xl', 'pageDetail.onLoadAfterLoadAddPageComponentModal();');
        },
        onShowPageComponentItemsAddModal: function () {},
        onRendAfterUpdatePageComponent: function (mess) {
            $('#defaultModal').modal('hide');
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: mess,
                showConfirmButton: false,
                timer: 1500
            });
            pageDetail.onInitTable();
        },
        onUpdateCoponent: function () {
            var firstValue;

            $('body').on('click', '.updateStatus', function (e) {
                firstValue = $(this).val();
            });
            $('body').on('change', '.updateStatus', function (e) {
                let $this = $(this);
                Common_another.onUserSwalFire__ques('', 'Thay đổi status?').then((result) => {
                    if (result.value) {
                        let _id = $this.data('id');
                        let _status = $this.val();
                        Common_another_ajax.get(`LandingPage/PageComponentStatusEdit?id=${_id}&status=${_status}`).then((data) => {
                            if (data.Success) {
                                eval(data.CallBack);
                            } else {
                                Common.onError(data.Message);
                            }
                        });
                    }
                });
            });
        }
    };
})();
