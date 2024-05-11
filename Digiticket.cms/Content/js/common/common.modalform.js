$(function () {
    $.ajaxSetup({ cache: false });
    $('body').on('click', 'a[data-modal]', function (e) {
        loadModalContent(this.href, $(this).data('modal-size'), $(this).data('callback'));
        return false;
    });
});

function loadModalContent(url, sizeModal = '', callback = '') {
    $('.my-loading').show();
    $('.dropdown-menu').removeClass('show');
    var modal = $('#defaultModal');
    var modalContent = $('#defaultModalContent');
    $(modalContent).load(url, function (response, status, xhr) {
        setTimeout(function () {
            $('.my-loading').hide();
            if (status === 'error') {
                swal.fire({
                    position: 'top-right',
                    type: 'error',
                    title: `Có lỗi xảy ra: ${xhr.status} ${xhr.statusText}`,
                    showConfirmButton: false,
                    timer: 1500
                });
            } else {
                $(modal).find('.modal-dialog').removeClass().addClass('modal-dialog').addClass(sizeModal);
                new Promise((resolve) => {
                    if (response.includes('login page')) {
                        Common.onTimeOut();
                    } else {
                        $(modal).modal('show');
                    }
                    resolve();
                }).then(() => {
                    if (callback !== '') {
                        eval(callback);
                    }
                });
            }
        }, 500);
    });
}
/**
 * @summary : sử dụng khi dùng tiếng việt render ra modal.
 *
 * @Exceptions
 */
$(function () {
    $('body').on('click', 'a[data-modal-http-get]', function (e) {
        let href;
        if (this.href.includes(domain)) href = this.href.replace(domain, '');
        else href = this.href;
        Common_another_ajax.renPartial___Get(href, undefined, $(this).data('callback'), $(this).data('modal-size'));
        return false;
    });
});

$(function () {
    $('body').on('click',[data-modal-custom] ,function() {
        $('.my-loading').show();
        
    })
});
