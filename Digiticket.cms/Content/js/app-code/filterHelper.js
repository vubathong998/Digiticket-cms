var FilterHelper = (function () {
    return {
        initEmmediatly: function () {
            this.onResponsiveText();
        },
        onResponsiveText: function () {
            $(window).on('resize', function () {
                changeTextWhenResize();
            });
            changeTextWhenResize();
            function changeTextWhenResize() {
                let $publicElement = $('.FilterAsPublic').find('label');
                let $clearFilterBtn = $('.clearFilter');
                let $clearFilter = $('.clearFilter').closest('.col-12').find('label');
                let innerWidth = window.innerWidth;

                if (innerWidth >= 1472) {
                    $publicElement.html('Lọc Theo public');
                } else if (innerWidth >= 1224) {
                    $publicElement.html('Lọc.T public');
                } else {
                    $publicElement.html('Pulic');
                    $publicElement.attr('title', 'Lọc theo public');
                }

                if (innerWidth >= 1728) {
                    $clearFilterBtn.html('Xóa bộ lọc <i class="fa fa-magic"></i>');
                } else if (innerWidth >= 1268) {
                    $clearFilterBtn.html('Xóa <i class="fa fa-magic"></i>');
                } else if (innerWidth >= 1024) {
                    $clearFilterBtn.html('<i class="fa fa-magic"></i>');
                    $clearFilter.html('Clear');
                } else {
                    $clearFilterBtn.html('Xóa hết các bộ lọc <i class="fa fa-magic"></i>');
                }
            }
        },
        HTMLonCreateToolTip___obj: function (title = '') {
            return {
                dataToggle: 'tooltip',
                dataPlacement: 'top',
                title: title
            };
        }
    };
})();

(() => {
    FilterHelper.initEmmediatly();
})();
