var CommonAjax = function () {
    var $form;
    return {
        init: function () {
            $(document).on('submit', 'form.ajax-form', function () {
                $form = $(this);
                if($form.filter('[showLoading]').length) $('.my-loading').show();
                Common.onResetFormatCurrency($form);
                Common.onResetTagify($form);
                let checkXSS = function (e) {
                    return $(e).val().includes('&lt;script&gt;') || $(e).val().includes('&lt;script');
                };
                if ($form.find('.text-html').toArray().some(checkXSS)) {
                    alert('Xóa script đi!');
                    return false;
                } else {
                    if ($form.valid())
                        CommonAjax.onDoAjax();
                }
                return false;
            });
        },
        onDoAjax: function (getForm = '') {
            if(getForm) $form = $(getForm);
            var formdata = new FormData($form[0]);
            CommonAjax.onShowLoading();
            CommonAjax.onHideMessage();
            $.ajax({
                url: $form.attr('action'),
                type: $form.attr('method'),
                data: formdata,
                cache: false,
                contentType: false,
                processData: false,
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                success: function (result) {
                    if($form.filter('[showLoading]').length) $('.my-loading').hide();
                    $($form).find('.custom-input-currency').each(function () {
                        $(this).val(Common.onFormatCurrency($(this).val())).trigger('input');
                    });
                    if (result.Message) {
                        CommonAjax.onShowMessage(result.Message, result.Success);
                    }
                    if (result.hasOwnProperty('CallBack'))
                        eval(result.CallBack);
                    if (result.page == "login page") {
                        Common.onTimeOut();
                    }
                }
            }).done(function (response) {
                CommonAjax.onHideLoading();
            });
        },
        onShowLoading: function () {
            $form.find('button[type=submit]').attr("disabled", true);
            if (!$form.find('button[type=submit] .kt-spinner').length)
                $form.find('button[type=submit]').addClass(' kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        },
        onHideLoading: function () {
            $form.find('button[type=submit]').attr("disabled", false);
            $form.find('button[type=submit]').removeClass(' kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        },
        onShowMessage: function (msg, isSuccess = false) {
            if (isSuccess)
                $form.find('.alert').removeClass("alert-warning").addClass("alert-success");

            else
                $form.find('.alert').removeClass("alert-success").addClass("alert-warning");

            $form.find('.alert .alert-text').html(msg);
            $form.find('.alert').show();
        },
        onHideMessage: function () {
            $form.find('.alert').hide();
        }
    };
}();
$(function () { CommonAjax.init(); });