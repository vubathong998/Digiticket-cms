const baseUrlBanner = `${domain}/Banner/GetByPage`;
var dtTableBanner;
var $containerBanner = '.table-Banner';

const baseUrlBannerType = `${domain}/Banner/GetByPageType`;
var dtTableBannerType;
var $containerBannerType = '.table-BannerType';
var categoryIdType;
var destinationIdType;

var Banner = (function () {
    return {
        init: function () {
            this.onBannerSelect2();
            this.onFilterBannerType();
            this.initTable(1, 10, 'createddate', 'desc');
            Common.onClearFilter($containerBanner, baseUrlBanner, dtTableBanner);
            Common.onSort($containerBanner, baseUrlBanner, dtTableBanner);
            Common.onSelectPageSize($containerBanner, baseUrlBanner, dtTableBanner);
            Common.onGotoPage($containerBanner, baseUrlBanner, dtTableBanner);
            Common.onSearchByKey($containerBanner, baseUrlBanner, dtTableBanner);
            Common.onSelectFilterCateAndDes($containerBanner, baseUrlBanner, dtTableBanner);
            Common.onSelectFilterStatus($containerBanner, baseUrlBanner, dtTableBanner);
            this.onLickToggleView();
        },
        initTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_Banner');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableBanner = table
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
                    scrollY: 'calc(100vh - 40.2rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlBanner}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}&CategoryId=${categoryId}&Key=${key}&DestinationId=${destinationId}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        { data: 'CategoryName' },
                        { data: 'DestinationName' },
                        { data: 'TypeName' },
                        // {
                        //     data: 'status',
                        //     width: '30px',
                        //     render: function (data, type, full, meta) {
                        //         /*html*/
                        //         return `
                        //             <div class="text-center"><input type="checkbox" data-status="${full.id}" class="cursor-pointer" ${data == 1 ? 'checked' : ''} /></div>
                        //         `;
                        //     }
                        // },
                        {
                            data: 'Url',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<a href="${domainCDN_MediaShowContent}/${full.Url}" target="_blank" class="cursor-pointer">
                                            ${data}
                                        </a>
                                `;
                            }
                        },
                        {
                            data: 'Idx',
                            width: '30px',
                            class: 'text-center',
                            render: function (data) {
                                /*html*/
                                return `
                                    <div class="text-center">${data}</div>
                                `;
                            }
                        },
                        {
                            data: 'CreatedDate',
                            type: 'date',
                            width: '60px',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'CreatedByName',
                            type: 'string',
                            width: '60px',
                            render: function (data) {
                                if (data) return `<div style="max-width: 60px;" class="cursor-context-menu text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedDate',
                            type: 'date',
                            width: '60px',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedByName',
                            type: 'string',
                            width: '60px',
                            render: function (data) {
                                if (data) return `<div style="max-width: 60px;" class="cursor-context-menu text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                                else return '';
                            }
                        },
                        { data: '' },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -2,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.edit,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="loadModalContent('${domain}/Banner/BannerAddOrEditModal?id=${full.Id}','modal-xl','Banner.initAddOrEditModal();')"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                     <i class="text-success flaticon2-edit"></i>
                                         </button>`;
                            }
                        },
                        {
                            targets: -1,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.edit,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="Banner.onDeleteBanner(${full.Id})"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                     <i class="text-red flaticon-delete"></i>
                                         </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerBanner);
                        $('.my-loading').hide();
                    }
                });
        },
        initAddOrEditModal: function () {
            this.onBannerSelect2();
            this.onSuggestImage();
            this.onSuggestBannerType();
            this.onRemoveTypeWhenSelectDesOrCate();
            this.onValidate();
            Common.onInitSelect2LinkInImage();
        },
        initTypeAddOrEditModal: function () {
            this.onBannerSelect2();
            this.onValidateType();
        },
        onFilterBannerType: function () {
            let addationData = {
                page: 1,
                perpage: 30
            };
            Common.onLoadSelect2Show('filterBannerType', $('.filterBannerTypeContainer'), 'Banner/GetByPageType', undefined, addationData, '', true);
            $('body').on('change', '.filterBannerType', function () {
                let $this = $(this);
                $('.my-loading').show();
                if (!$this.filter('.noLoad').length) {
                    if ($this.val()) {
                        let selected = JSON.parse($this.val());
                        type = selected.Id;
                        Common.onReloadTableAjax($containerBanner, baseUrlBanner, dtTableBanner);
                        $this.siblings('span ').find('.select2-selection').addClass('hightLighted');
                    } else {
                        type = '';
                        Common.onReloadTableAjax($containerBanner, baseUrlBanner, dtTableBanner);
                        $('.filterBannerTypeContainer').find('.hightLighted').removeClass('hightLighted');
                    }
                }
            });
        },
        onDeleteBanner: function (id) {
            Common_another_ajax.onDelete___get(`/Banner/BannerDelete?Id=${id}`);
        },
        onRendAfterUpdateBanner: function (mess) {
            Common.onReloadDataTable(dtTableBanner, '.table-Banner', mess);
        },
        onSuggestImage: function () {
            Common.onLoadSelect2Show('select-img', $('#BannerAddOrEdit'), 'Media/SuggestImages', 'Url', '', '', true);
            $('body').on('change', '.select-img', function () {
                let $this = $(this);
                let $div = $this.closest('.img-container');
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $div.find('.eShowImg').show();
                    $div.find('img.img-fit').attr('src', `${domainCDN_MediaShowContent}/${selected.Url}`);
                    $div.find('[name="url"]').val(selected.Url);
                } else {
                    $div.find('.eShowImg').hide();
                    $div.find('img.img-fit').attr('src', '');
                    $div.find('[name="url"]').val('');
                }
                $div.find('.select-LinkInImage').val('').trigger('change');
                $div.find('.elemLinkOption').val(0).trigger('change');
                $div.find('.elemAlt').val('');
                $div.find('.elemIdx').val('');
            });
        },
        onSuggestBannerType: function () {
            Common.onLoadSelect2Show('select-bannerType', $('#BannerAddOrEdit'), '/Banner/SuggestBannerType', 'Name', 'Nhập để tìm kiếm', '', true);
            $('body').on('change', '.select-bannerType', function () {
                let $this = $(this);
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('[name="type"]').val(selected.Id);
                } else {
                    let $div = $this.closest('#BannerAddOrEdit');
                    $div.find('.typeInput').val('');
                }
            });
            $('.select-bannerType-disable').select2({
                disabled: true
            });
        },
        onRemoveTypeWhenSelectDesOrCate: function () {
            $('[name="categoryId"]').on('change', function (e) {
                let $this = $(this);
                categoryIdType = $this.val();
                whenSelectDesOrCate();
            });
            $('[name="destinationId"]').on('change', function (e) {
                let $this = $(this);
                destinationIdType = $this.val();
                whenSelectDesOrCate();
            });
            function whenSelectDesOrCate() {
                $('.select-bannerType').val('').trigger('change');
                $('[name="type"]').val('');
            }
        },
        onLickToggleView: function () {
            $(document).on('click', 'input[data-status]', function (e) {
                e.preventDefault();
                let $this = $(this);
                Swal.fire({
                    title: 'Bạn chắn chắn?',
                    text: 'Toggle view',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Có, Toggle!',
                    cancelButtonText: 'Không!'
                }).then((result) => {
                    let id = $this.data('status');
                    if (result.value) {
                        Common_another_ajax.get(`Banner/BannerToggleView/${id}`);
                    }
                });
            });
        },
        onBannerSelect2: function () {
            $('.kt-select2-categories').select2({
                placeholder: 'Tìm danh mục'
            });
            $('.kt-select2-destination').select2({
                placeholder: 'Tìm điểm đến'
            });
            $('.kt-select2-category--modal').select2({
                placeholder: 'Tìm danh mục',
                closeOnSelect: true
            });
            $('.kt-select2-destination--modal').select2({
                placeholder: 'Tìm điểm đến',
                closeOnSelect: true
            });
            $('.kt-select2-category--modal--disable').select2({
                disabled: true
            });
            $('.kt-select2-destination--modal--disable').select2({
                disabled: true
            });
            $('.kt-select2-type').select2({
                placeholder: 'Không có type'
            });
        },
        initType: function () {
            this.initTableType(1, 10, 'createddate', 'desc');
            Common.onClearFilter($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onSort($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onSelectPageSize($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onGotoPage($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onSearchByKey($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onSelectFilterCateAndDes($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onSelectFilterStatus($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onSelectType($containerBannerType, baseUrlBannerType, dtTableBannerType);
            Common.onSelectFilterPublic($containerBannerType, baseUrlBannerType, dtTableBannerType);
            this.onBannerSelect2();
        },
        initTableType: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_BannerType');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableBannerType = table
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
                    scrollY: 'calc(100vh - 40.2rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlBannerType}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}&CategoryId=${categoryId}&Key=${key}&DestinationId=${destinationId}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        { data: 'CategoryName' },
                        { data: 'DestinationName' },
                        // {
                        //     data: 'status',
                        //     render: function (data, type, full, meta) {
                        //         return data;
                        //     }
                        // },
                        {
                            data: 'CreatedDate',
                            type: 'Date',
                            width: '60px',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'CreatedByName',
                            type: 'string',
                            width: '60px',
                            render: function (data) {
                                if (data) return `<div>${data}</div>`;
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedDate',
                            type: 'date',
                            width: '60px',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedByName',
                            type: 'string',
                            width: '60px',
                            render: function (data) {
                                if (data) return `<div>${data}</div>`;
                                else return '';
                            }
                        },
                        { data: '' },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -2,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.edit,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="loadModalContent('${domain}/Banner/BannerTypeAddOrEditModal?id=${full.Id}','modal-lg','Banner.initTypeAddOrEditModal();')"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                     <i class="text-success flaticon2-edit"></i>
                                         </button>`;
                            }
                        },
                        {
                            targets: -1,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.edit,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="Banner.onDeleteBannerType(${full.Id})"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                     <i class="text-red flaticon-delete"></i>
                                         </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerBannerType);
                        $('.my-loading').hide();
                    }
                });
        },
        onDeleteBannerType: function (id) {
            Common_another_ajax.onDelete___get(`/Banner/BannerTypeDelete?Id=${id}`).then((data) => {
                eval(data.CallBack);
            });
        },
        onRendAfterBannerType: function (mess) {
            Common.onReloadDataTable(dtTableBannerType, '.table-BannerType', mess);
        },
        onValidate: function () {
            $('#BannerAddOrEdit').validate({
                rules: {
                    name: {
                        required: true
                    }
                },
                messages: {
                    name: {
                        required: 'Không được bỏ trống tên'
                    }
                }
            });
        },
        onValidateType: function () {
            $('#BannerTypeAddOrEdit').validate({
                rules: {
                    name: {
                        required: true
                    }
                },
                messages: {
                    name: {
                        required: 'Không được bỏ trống tên'
                    }
                }
            });
        }
    };
})();
