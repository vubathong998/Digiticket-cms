var isShowPageComponentItems = false;
var isMustLoadTable = true;
var isTableReload = false;

var pageComponentDetail = (function () {
    return {
        init: function () {
            this.onInitpageTagify();
            this.onAfterClickPageComponentItems();
            this.onCKEditor();
            this.onFilterType();
            this.onSelect2();
        },
        onInitpageTagify: function () {
            var LinkPageStyles = $('#LinkPageStyles')[0];
            var LinkPaegScripts = $('#LinkPaegScripts')[0];
            new Tagify(LinkPageStyles);
            new Tagify(LinkPaegScripts);
        },
        onAfterClickPageComponentItems: function () {
            $('[href="#pageComponentItems"]').on('click', function (e) {
                isShowPageComponentItems = true;
                if (isMustLoadTable) {
                    if (!isTableReload) {
                        pageComponentItems.init();
                        isTableReload = true;
                        isMustLoadTable = false;
                    } else {
                        // Common.onReloadDataTable(dtTablePageComponentItems, '.table-PageComponentItems', mess);
                        dtTablePageComponentItems.ajax.reload();
                        isMustLoadTable = false;
                    }
                }
                $('.addPageComponentItemPlace').removeClass('display-none');
            });
            $('[href="#info"]').on('click', function (e) {
                $('.addPageComponentItemPlace').addClass('display-none');
                isShowPageComponentItems = false;
            });
        },
        onCKEditor: function () {
            Common.onLoadEditor('.CKEditor');
        },
        onFilterType: function () {
            $('.filterType').on('change', function (e) {
                let _value = $(this).val();
                let initSortTable = function () {
                    $('.tablePageComponentItems')
                        .find('tbody')
                        .sortable({
                            placeholder: 'ui-state-highlight',
                            // change: function (event, ui) {
                            //     var start_pos = ui.item.data('start_pos');
                            //     var index = ui.placeholder.index();
                            // },
                            update: function (event, ui) {
                                $('.updateIdx').removeClass('disabled').removeClass('cursor-not-allowed');
                                let $container = $(event.target);
                                for (let index = 0; index < $container.find('tr').length; index++) {
                                    const element = $container.find('tr')[index];
                                    $($(element).find('td')[0]).html(index + 1);
                                }
                            }
                        });
                };
                softExpandVar = {
                    Type: _value,
                    Field: 'Idx',
                    Sort: 'asc'
                };
                perpage = 10;
                Common.onReloadTableAjax($containerPageComponentItems, baseUrlPageComponentItems, dtTablePageComponentItems);
                $('.updateIdx').addClass('disabled').addClass('cursor-not-allowed');
                if (_value === 'null') {
                    $('.tablePageComponentItems').find('tbody').sortable('disable');
                    $('.tablePageComponentItems').removeClass('cursor');
                } else {
                    // if ($('.tablePageComponentItems').find('.dataTables_empty').length > 0) {
                    //     initSortTable();
                    //     $('.tablePageComponentItems').find('tbody').sortable('disable');
                    //     $('.tablePageComponentItems').removeClass('cursor');
                    // } else {
                    initSortTable();
                    $('.tablePageComponentItems').find('tbody').sortable('enable');
                    $('.tablePageComponentItems').addClass('cursor');
                    // }
                }
            });
        },
        onSelect2: function () {
            $('.kt-select2-TypePageComponentItem').select2({
                placeholder: 'Tìm thể loại cho Page component item'
            });
            $('.kt-select2-filterType').select2({
                placeholder: 'Tìm type'
            });
        }
    };
})();

var pageComponent = (function () {
    return {
        onRendAfterUpdatePageComponent: function (mess) {
            $('.my-loading').hide();
            Common.onSuccess(mess);
        }
    };
})();
