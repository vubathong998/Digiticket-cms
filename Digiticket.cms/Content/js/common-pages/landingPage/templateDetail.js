var _idTemplate = $('#idTemplate').val();

var templateDetail = (function () {
    return {
        init: function () {
            this.onInitTable();
            this.onEditIdx();
        },
        onInitTable: function () {
            Common_another_ajax.get(`LandingPage/TemplateGetByTemplate/${_idTemplate}`).then((data) => {
                $('.my-loading').hide();
                new Promise((resolve) => {
                    $('.tableGetByTemplate').html(data.Data);
                    resolve();
                }).then(() => {
                    $('.templaTatable').DataTable({
                        searching: false,
                        paging: false,
                        info: false,
                        ordering: false
                    });
                });
            });
        },
        onDeleteMap: function (id) {
            Common_another_ajax.onDelete___get(`LandingPage/TemplateDeleteMap/${id}`);
        },
        onEditIdx: function () {
            let firstValue = '';
            let oldValue = '';
            $('body').on('focusin', `.idxInput`, function (e) {
                let $this = $(this);
                oldValue = $this.val();
                firstValue = $this.val();
            });
            $('body').on('change', `.idxInput`, function (e) {
                e.preventDefault();
                let $this = $(this);

                if ($this.val() === '') {
                    Swal.fire('Bạn chưa nhập idx!');
                    $this.val(oldValue);
                } else if (Number($this.val()) < 0 || isNaN($this.val())) {
                    Swal.fire('Idx không chính xác!');
                    $this.val(oldValue);
                } else {
                    Common_another.onUserSwalFire__ques('', 'Thay đổi idx?').then((result) => {
                        if (result.value) {
                            let _id = $(this).data('id');
                            let _idx = $this.val();
                            Common_another_ajax.get(`LandingPage/TemplateMapEdit?id=${_id}&idx=${_idx}`).then((data) => {
                                if (data.Success) {
                                    eval(data.CallBack);
                                } else {
                                    Common.onError(data.Message);
                                }
                            });
                        } else {
                            $this.val(firstValue);
                        }
                    });
                }
            });
        }
    };
})();

var template = (function () {
    return {
        onRendAfterTemplateUpdate: function (mess) {
            $('#defaultModal').modal('hide');
            $('.my-loading').hide();
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: mess,
                showConfirmButton: false,
                timer: 1500
            });
            templateDetail.onInitTable();
        },
        onRendAfterTemplateEdit: function () {
            Common.onSuccess();
        },
        onLoadAfterLoadTemplateMapModal: function () {
            var dataAdd = {
                page: 1,
                perpage: 30,
                field: 'createddate',
                sort: 'desc'
            };
            Common.onLoadSelect2Show('select-templateComponent', $('#TemplateMap'), '/LandingPage/TemplateComponentGetByPage', 'Name', dataAdd, '', true);
            $('body').on('change', '.select-templateComponent', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('.parttenImg').show();
                    if (selected.ViewImage) {
                        $('.parttenImg').attr('src', domainCDN_MediaShowContent + selected.ViewImage);
                    } else {
                        $('.parttenImg').attr('src', '');
                        $('.parttenImg').hide();
                    }
                    $('#templateComponentId').val(selected.Id);
                } else {
                    $('#templateComponentId').val('');
                    $('.parttenImg').hide();
                    $('.parttenImg').attr('src', '');
                }
            });
        }
    };
})();
