field = 'name';
sort = 'acs';

const baseUrlUpdateDesination = `${domain}/Lib/LibDestinationGetByPage`;
var dtTableDestination;
var $containerProductDestination = '.table-destination';

var destination = (function () {
    return {
        init: function () {
            this.onFillter();
            this.initTable(1, 10, 'Name', 'asc');
            Common.onClearFilter($containerProductDestination, baseUrlUpdateDesination, dtTableDestination);
            Common.onSort($containerProductDestination, baseUrlUpdateDesination, dtTableDestination);
            Common.onSelectPageSize($containerProductDestination, baseUrlUpdateDesination, dtTableDestination);
            Common.onGotoPage($containerProductDestination, baseUrlUpdateDesination, dtTableDestination);
            Common.onSearchByKey($containerProductDestination, baseUrlUpdateDesination, dtTableDestination);
        },
        initAfterUpdateModalIsOpened: function () {
            this.onSuggestPlace();
            this.onSelectImg();
            this.onDeleteListImg();
            Common.onInitSelect2LinkInImage();
        },
        initTable: function (page, perpage, field, sort) {
            let table = $('#html_table_destination');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableDestination = table
                .on('preXhr.dt', function (e, settings, data) {
                    $('.my-loading').show();
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    Common.onLoadTableAjax(xhr.responseText);
                })
                .DataTable({
                    responsive: true,
                    paging: false,
                    info: false,
                    searching: false,
                    ordering: false,
                    scrollY: 'calc(100vh - 38.5rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlUpdateDesination}?page=${page}&perpage=${perpage}&field=${field}&sort=${sort}&supplierId=${supplierId}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        { data: 'Code' },
                        { data: 'CountProduct' },
                        { data: 'Place' },
                        {
                            data: 'IsHome',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `
                                        <div class="text-center">
                                            <input type="checkbox" class="cursor-not-allowed" ${data ? 'checked' : ''} onclick="event.preventDefault();" />
                                        </div>
                                    `;
                            }
                        },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -1,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.edit,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `
                                    <div class="text-center">
                                        <button onclick="loadModalContent('${domain}/Lib/LibDestinationUpdateModal?id=${full.Id}','modal-xl','destination.initAfterUpdateModalIsOpened();')"
                                            class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                            >
                                                <i class="text-success flaticon2-edit"></i>
                                        </button>
                                    </div>
                                `;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerProductDestination);
                        $('.my-loading').hide();
                    }
                });
        },
        onRendAfterUpdate: function () {
            Common.onReloadDataTable(dtTableDestination, '.table-destination');
        },
        onSuggestPlace: function () {
            Common.onLoadSelect2Show('select-DestinationPlace', $('#FormLibCategoryUpdate'), '/Place/SuggestPlaceInBaseInfo', 'Description', '', '', true);
            $('body').on('change', '.select-DestinationPlace', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    Common_another_ajax.get(`Place/DeleteTokenSSIC`).then(() => {
                        $('.my-loading').hide();
                        let selected = JSON.parse($this.val());
                        $('.place').val(selected.Description);
                        $('.placeId').val(selected.Place_id);
                    });
                } else {
                    let $div = $this.closest('.place-container');
                    $div.find('input.d-none').remove();
                }
            });
        },
        onSelectImg: function () {
            Common.onLoadSelect2Show('select-img-destination', $('#FormLibCategoryUpdate'), 'Media/SuggestImages', 'Url', 'Nhập để tìm kiếm ảnh sản phẩm', '', false, 'Url');
            Common.onFillSelect2ForImage('.imgDestination-container', '.select-img-destination', '.ListImgContainer');
        },
        onDeleteListImg: function () {
            $(document).on('click', '.clearListImg', function (e) {
                let $this = $(this);
                let _value = $this.data('url');
                let $li = $('.imgDestination-container').find('li');
                for (let i = 0; i < $li.length; i++) {
                    let _valueLi;
                    if ($($li[i]).attr('title')) _valueLi = $($li[i]).attr('title').trim();
                    else _valueLi = $($li[i]).attr('title');

                    if (_valueLi === _value) {
                        $($li[i]).find('span').trigger('click');
                    }
                }
                $('.select2-dropdown').hide();
                $('.tooltip').remove();
                $('.tooltip-inner').remove();
                $('.imgDestination-container').find('.select2-search__field').blur().trigger('input');
            });
        },
        onFillter: function () {
            $('.select-isHome').on('change', function (e) {
                let $this = $(this);
                if (!$this.filter('.noLoad').length) {
                    softExpandVar = {
                        isHome: $this.val()
                    };
                    Common.onReloadTableAjax($containerProductDestination, baseUrlUpdateDesination, dtTableDestination);
                }
            });
            $('.clearFilter').on('click', function (e) {
                new Promise((resolve) => {
                    $('.select-isHome').addClass('noLoad');
                    $('.select-isHome').addClass('noLoad');
                    resolve();
                })
                    .then(() => {
                        $('.select-isHome').val('').trigger('change');
                    })
                    .then(() => {
                        $('.select-isHome').removeClass('noLoad');
                    });
            });
        }
    };
})();
