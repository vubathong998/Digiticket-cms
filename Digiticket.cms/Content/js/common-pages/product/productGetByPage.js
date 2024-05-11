const baseUrlProduct = `${domain}/Product/ProductGetByPage`;
const Status__product = {
    0: { name: 'Chưa thiết lập', description: 'border' },
    1: { name: 'Mở bán', description: 'kt-badge--primary' },
    2: { name: 'Sắp mở bán', description: 'kt-badge--success' },
    3: { name: 'Tạm hết hàng', description: 'kt-badge--warning' },
    4: { name: 'Hết hàng', description: 'kt-badge--danger' }
};

var dtTableProduct;
var $containerProduct = '.table-Product';
var isProductShown = false;
var filterProductGetByPage = {
    page: 1,
    perpage: 1,
    pagesize: 10,
    key: ''
};

const baseUrlUpdateHot = `${domain}/Product/ProductGetByPage`;
var dtTableHot;
var $containerProductHot = '.table-Hot';

const baseUrlUpdateHome = `${domain}/Product/ProductGetByPage`;
var dtTableHome;
var $containerProductHome = '.table-Home';

onReloadTableAjaxExcute__fun = function () {
    let $trs = $($containerProduct).find('tr');
    for (let i = 0; i < $trs.length; i++) {
        const element = $trs[i];
        if ($(element).find('.productHasBeenDeleted').length) {
            $(element).addClass('hightlightDeleted');
        }
    }
};

var productGetByPage = (function () {
    return {
        init: function (supplierId = '') {
            this.onTogglePublic();
            this.onSelect2();
            this.initTable(1, 10, 'CreatedDate', 'desc', supplierId);
            Common.onClearFilter($containerProduct, baseUrlProduct, dtTableProduct);
            Common.onSort($containerProduct, baseUrlProduct, dtTableProduct);
            Common.onSelectPageSize($containerProduct, baseUrlProduct, dtTableProduct, filterProductGetByPage);
            Common.onGotoPage($containerProduct, baseUrlProduct, dtTableProduct, filterProductGetByPage);
            Common.onSearchByKey($containerProduct, baseUrlProduct, dtTableProduct, filterProductGetByPage);
            Common.onSelectFilterCateAndDes($containerProduct, baseUrlProduct, dtTableProduct);
            Common.onSelectFilterStatus($containerProduct, baseUrlProduct, dtTableProduct);
            Common.onSelectFilterPublic($containerProduct, baseUrlProduct, dtTableProduct);
            this.onUpdateIdx();
        },
        initTable: function (page, perpage, field = '', sort = '', supplierId = '', isPublicNone = false) {
            let table = $('#html_table_Product');
            let dataTable__columns = new Object();
            // if()
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableProduct = table
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
                    scrollY: 'calc(100vh - 38rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlProduct}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}&SupplierId=${supplierId}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'Name',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<a href="${domain}/Product/ProductDetail?productId=${full.Id}" target="_blank">${data}</a>`;
                            }
                        },
                        {
                            data: 'DigipostName',
                            render: function (data) {
                                if (Common_another.ciEquals(data, 'Đã xóa'))
                                /*html*/    
                                return `
                                    <span class="productHasBeenDeleted" data-toggle="tooltip" title="Tại sao hàng này lại tô màu xám? Vì Sản phẩm này bên Digipost đã bị xóa!">
                                        ${data} (?)
                                    <span>
                                `;
                                else return data;
                            }
                        },
                        { data: 'CategoryName' },
                        { data: 'DestinationName' },
                        { data: 'SupplierName' },
                        {
                            data: 'Status',
                            width: '100px',
                            className: 'text-center',
                            render: function (data) {
                                /*html*/
                                return `<div class="text-center">
                                <span class="kt-badge ${Status__product[data].description} kt-badge--inline">
                                ${Status__product[data].name}
                                </span>
                                </div>`;
                            }
                        },
                        {
                            data: 'Idx',
                            width: '30px',
                            render: function (data, type, full, meta) {
                                return `
                                        <div>
                                            <input class="inputTextIdx inputInTable" value="${data}" data-id="${full.Id}" />
                                        </div>
                                        `;
                            }
                        },
                        { data: '' },
                        {
                            data: 'Url',
                            width: '30px',
                            className: 'text-center',
                            render: function (data) {
                                if (data) {
                                    /*html*/
                                    return `
                                        <div class="text-center">
                                            <a href="${domainFE}${data}" target="_blank">Xem</a>
                                        </div>
                                    `;
                                } else {
                                    return '';
                                }
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
                                /* html */
                                if (data) return `<div style="max-width: 60px;" class="cursor-context-menu text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                                else return '';
                            }
                        }
                    ],
                    columnDefs: [
                        {
                            targets: 7,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.PublicProduct,
                            render: function (data, type, full, meta) {
                                // return `<input class="cursor-pointer ${isPublicNone ? 'noClick' : ''}" ${isPublicNone ? `onclick="event.preventDefault();"` : 'data-checkbox-toggle'} data-id="${
                                //     full.id
                                // }" type="checkbox" ${full.isPublic ? 'checked' : ''} />`;
                                if (!permission.PublicProduct || isPublicNone) {
                                    /*html*/
                                    return `<input class="cursor-pointer noClick" onclick="event.preventDefault();" type="checkbox" ${full.IsPublic ? 'checked' : ''} />`;
                                } else {
                                    /*html*/
                                    return `<input class="cursor-pointer" data-checkbox-toggle data-id="${full.Id}" type="checkbox" ${full.IsPublic ? 'checked' : ''} />`;
                                }
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerProduct);
                        $('.my-loading').hide();
                        onReloadTableAjaxExcute__fun();
                    }
                });
        },
        onRendAfterUpdateIdx: function () {
            Common.onReloadDataTable(dtTableProduct, '.table-Product', 'Update idx thành công');
        },
        onUpdateIdx: function () {
            let firstValue = '';
            let oldValue = '';
            $('body').on('focusin', `.inputTextIdx`, function (e) {
                let $this = $(this);
                oldValue = $this.val();
                firstValue = $this.val();
            });
            $('body').on('change', `.inputTextIdx`, function (e) {
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
                            Common_another_ajax.get(`Product/ProductUpdateIdx?id=${_id}&idx=${_idx}`).then((data) => {
                                eval(data.CallBack);
                            });
                        } else {
                            $this.val(firstValue);
                        }
                    });
                }
            });
        },
        initHot: function () {
            hardExpandVar = {
                IsHot: true
            };
            this.initTableHot(1, 10, 'CreatedDate', 'desc');
            Common.onClearFilter($containerProductHot, baseUrlUpdateHot, dtTableHot);
            Common.onSort($containerProductHot, baseUrlUpdateHot, dtTableHot);
            Common.onSelectPageSize($containerProductHot, baseUrlUpdateHot, dtTableHot);
            Common.onGotoPage($containerProductHot, baseUrlUpdateHot, dtTableHot);
            Common.onSearchByKey($containerProductHot, baseUrlUpdateHot, dtTableHot);
            Common.onSelectFilterCateAndDes($containerProductHot, baseUrlUpdateHot, dtTableHot);
            Common.onSelectFilterStatus($containerProductHot, baseUrlUpdateHot, dtTableHot);
            Common.onSelectFilterPublic($containerProductHot, baseUrlUpdateHot, dtTableHot);
            this.onClickHot();
            this.onChangeInputTextHot();
            this.onSelect2();
        },
        initHome: function () {
            hardExpandVar = {
                IsHome: true
            };
            this.initTableHome(1, 10, 'CreatedDate', 'desc');
            Common.onClearFilter($containerProductHome, baseUrlUpdateHome, dtTableHome);
            Common.onSort($containerProductHome, baseUrlUpdateHome, dtTableHome);
            Common.onSelectPageSize($containerProductHome, baseUrlUpdateHome, dtTableHome);
            Common.onGotoPage($containerProductHome, baseUrlUpdateHome, dtTableHome);
            Common.onSearchByKey($containerProductHome, baseUrlUpdateHome, dtTableHome);
            Common.onSelectFilterCateAndDes($containerProductHome, baseUrlUpdateHome, dtTableHome);
            Common.onSelectFilterStatus($containerProductHome, baseUrlUpdateHome, dtTableHome);
            Common.onSelectFilterPublic($containerProductHome, baseUrlUpdateHome, dtTableHome);
            this.onClickHome();
            this.onChangeInputTextHome();
            this.onSelect2();
        },
        onRendAfterTogglePublic: function (mess) {
            Common.onReloadDataTable(dtTableProduct, '.table-Tags', mess);
        },
        onSelect2HotOrHomeModal: function () {
            let filterCategoryModal;
            let filterDestinationModal;

            $('.kt-select2-categories-modal').select2({
                placeholder: 'Tìm danh mục'
            });
            $('.kt-select2-destination-modal').select2({
                placeholder: 'Tìm điểm đến'
            });

            $('.kt-select2-categories-modal').on('change', function (e) {
                filterCategoryModal = $(this).val();
                let addationData = {
                    IsHot: true,
                    Category: filterCategoryModal,
                    Destination: filterDestinationModal
                };
                Common.onLoadSelect2Show(`select-product${type}`, $(`#FormProductUpdate${type}`), `Product/ProductGetByPage`, undefined, addationData, '', true);
            });
            $('.kt-select2-destination-modal').on('change', function (e) {
                filterDestinationModal = $(this).val();
                let addationData = {
                    IsHome: true,
                    Category: filterCategoryModal,
                    Destination: filterDestinationModal
                };
                Common.onLoadSelect2Show(`select-product${type}`, $(`#FormProductUpdate${type}`), `Product/ProductGetByPage`, undefined, addationData, '', true);
            });
        },
        onSelect2: function () {
            $('.kt-select2-categories').select2({
                placeholder: 'Tìm danh mục'
            });
            $('.kt-select2-destination').select2({
                placeholder: 'Tìm điểm đến'
            });
            $('.kt-select2-categories--disable').select2({
                disabled: true
            });
            $('.kt-select2-destination--disable').select2({
                disabled: true
            });
        },
        initAddOrUpdateHotModal: function () {
            this.onSuggestProductHot();
            this.onValidateHot();
            this.onSelect2HotOrHomeModal();
        },
        initAddOrUpdateHomeModal: function () {
            this.onSuggestProductHome();
            this.onValidateHome();
            this.onSelect2HotOrHomeModal();
        },
        onTogglePublic: function () {
            $(document).on('click', 'input[data-checkbox-toggle]', function (e) {
                e.preventDefault();
                let $this = $(this);
                let isChecked = $this[0].checked;
                let _id = $this.data('id');
                Common_another.onUserSwalFire__ques('', 'Toggle Publish?').then((result) => {
                    if (result.value) {
                        Common_another_ajax.get(`Product/TogglePublic?Id=${_id}&IsChecked=${!isChecked}`).then((data) => {
                            eval(data.CallBack);
                        });
                    }
                });
            });
        },
        onSelect2: function () {
            $('.kt-select2-destination').select2({
                placeholder: 'Tìm điểm đến'
            });
            $('.kt-select2-categories').select2({
                placeholder: 'Tìm danh mục'
            });
        },
        initTableHot: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_Hot');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableHot = table
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
                    scrollY: `${$(window).innerHeight() - 435}px`,
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlUpdateHot}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}&IsHot=true`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        {
                            data: 'IsHot',
                            className: 'text-center',
                            visible: permission.edit,
                            render: (data, type, full, meta) =>
                                /*html*/
                                `<div class="text-center"><input class="inputIsHot cursor-pointer" data-id="${full.Id}" data-idxHot="${full.IdxHot}" type="checkbox" ${data ? 'checked' : ''} /></div>`
                        },
                        { data: 'CategoryName' },
                        { data: 'DestinationName' },
                        {
                            data: 'Status',
                            className: 'text-center',
                            render: function (data) {
                                /*html*/
                                return `<div class="text-center">
                                            <span class="kt-badge ${Status__product[data].description} kt-badge--inline">
                                                ${Status__product[data].name}
                                            </span>
                                        </div>`;
                            }
                        },
                        {
                            data: 'IdxHot',
                            render: function (data, type, full, meta) {
                                if (permission.edit) {
                                    /*html*/
                                    return `<div>
                                                <input class="inputTextHot inputInTable" value="${data}" data-id="${full.Id}" />
                                            </div>`;
                                } else {
                                    return data;
                                }
                            }
                        },
                        {
                            data: 'IsPublic',
                            className: 'text-center',
                            /*html*/
                            render: (data) => `<div class="text-center">
                                                   <input onclick="event.preventDefault();" class="noClick inputIsPublic" type="checkbox" ${data ? 'checked' : ''} />
                                               </div>`
                        },
                        {
                            data: 'CreatedDate',
                            type: 'date',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'CreatedByName',
                            type: 'string',
                            render: function (data) {
                                if (data) return `<div>${data}</div>`;
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedDate',
                            type: 'date',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedByName',
                            type: 'string',
                            render: function (data) {
                                /* html */
                                if (data) return `<div>${data}</div>`;
                                else return '';
                            }
                        }
                    ],
                    columnDefs: [],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerProductHot);
                        $('.my-loading').hide();
                    }
                });
        },
        onSuggestProductHot: function () {
            Common.onLoadSelect2Show('select-productHot', $('#FormProductUpdateHot'), 'Product/ProductGetByPage', undefined, { IsHot: false }, '', true);
            $('body').on('change', '.select-productHot', function () {
                let $this = $(this);
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('[name="id"]').val(selected.Id);
                } else {
                    let $div = $this.closest('#FormProductUpdateHot');
                    $div.find('#productUpdateHotId').val('');
                }
            });
        },
        onClickHot: function () {
            $('body').on('click', '.inputIsHot', function (e) {
                e.preventDefault();
                Common_another.onUserSwalFire__ques('', 'Bỏ hot?').then((result) => {
                    if (result.value) {
                        let $this = $(this);
                        let _idxHot = $this.data('idxhot');
                        let _id = $this.data('id');
                        Common_another_ajax.get(`Product/ProductAddOrUnHot?Id=${_id}&IdxHot=${_idxHot}&IsHot=false`).then((data) => {
                            eval(data.CallBack);
                        });
                    }
                });
            });
        },
        onRendAfterAddOrUnHot: function () {
            Common.onReloadDataTable(dtTableHot, '.table-Tags');
        },
        onChangeInputTextHot: function () {
            productGetByPage.onHandleChangeIndHomeHot('Hot');
        },
        onValidateHot: function () {
            $.validator.addMethod(
                'not0',
                function (value) {
                    return !/0/g.test(value);
                },
                'Code chứa ít nhất một ký tự chữ'
            );
            $('#FormProductUpdateHot').validate({
                rules: {
                    selectHot: {
                        required: true
                    },
                    idxHot: {
                        required: true
                    }
                },
                messages: {
                    selectHot: {
                        required: 'Chưa chọn product'
                    },
                    idxHot: {
                        required: 'không được bỏ trống index hot'
                    }
                },
                errorPlacement: function () {
                    return false;
                }
            });
        },
        initTableHome: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_Home');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableHome = table
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
                    scrollY: `${$(window).innerHeight() - 435}px`,
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlUpdateHome}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}&IsHome=true`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        {
                            data: 'IsHome',
                            visible: permission.edit,
                            render: (data, type, full, meta) =>
                                /*html*/
                                `<div class="text-center"><input class="inputIsHome cursor-pointer" data-id="${full.Id}" data-idxhome="${full.IdxHome}" type="checkbox" ${
                                    data ? 'checked' : ''
                                } /></div>`
                        },
                        { data: 'CategoryName' },
                        { data: 'DestinationName' },
                        {
                            data: 'Status',
                            className: 'text-center',
                            render: function (data) {
                                return `<div class="text-center">
                                            <span class="kt-badge ${Status__product[data].description} kt-badge--inline">
                                                ${Status__product[data].name}
                                            </span>
                                        </div>`;
                            }
                        },
                        {
                            data: 'IdxHome',
                            render: function (data, type, full, meta) {
                                if (permission.edit) {
                                    /*html*/
                                    return `<div>
                                                <input class="inputTextHome inputInTable" value="${data}" data-id="${full.Id}" />
                                            </div>`;
                                } else {
                                    return data;
                                }
                            }
                        },
                        {
                            className: 'text-center',
                            data: 'IsPublic',
                            render: function (data) {
                                /*html*/
                                return `<div class="text-center">
                                        <input onclick="event.preventDefault();" class="noClick inputIsPublic" type="checkbox" ${data ? 'checked' : ''} />
                                </div>`;
                            }
                        },
                        {
                            data: 'CreatedDate',
                            type: 'date',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'CreatedByName',
                            type: 'string',
                            render: function (data) {
                                if (data) return `<div>${data}</div>`;
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedDate',
                            type: 'date',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'LastEditedByName',
                            type: 'string',
                            render: function (data) {
                                /* html */
                                if (data) return `<div>${data}</div>`;
                                else return '';
                            }
                        }
                    ],
                    columnDefs: [],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerProductHome);
                        $('.my-loading').hide();
                    }
                });
        },
        onSuggestProductHome: function () {
            Common.onLoadSelect2Show('select-productHome', $('#FormProductUpdateHome'), '/Product/ProductGetByPage', undefined, { isHome: false }, '', true);
            $('body').on('change', '.select-productHome', function () {
                let $this = $(this);
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('[name="id"]').val(selected.Id);
                } else {
                    let $div = $this.closest('#FormProductUpdateHome');
                    $div.find('#productUpdateHomeId').val('');
                }
            });
        },
        onClickHome: function () {
            $('body').on('click', '.inputIsHome', function (e) {
                e.preventDefault();
                Common_another.onUserSwalFire__ques('', 'Bỏ Home?').then((result) => {
                    if (result.value) {
                        let $this = $(this);
                        let _idxHome = $this.data('idxhome');
                        let _id = $this.data('id');
                        Common_another_ajax.get(`Product/ProductAddOrUnHome?id=${_id}&idxHome=${_idxHome}&isHome=false`).then((data) => {
                            eval(data.CallBack);
                        });
                    }
                });
            });
        },
        onRendAfterAddOrUnHome: function () {
            Common.onReloadDataTable(dtTableHome, '.table-Tags');
        },
        onValidateHome: function () {
            $.validator.addMethod(
                'not0',
                function (value) {
                    return !/0/g.test(value);
                },
                'Code chứa ít nhất một ký tự chữ'
            );
            $('#FormProductUpdateHome').validate({
                rules: {
                    selectHome: {
                        required: true
                    },
                    idxHome: {
                        required: true
                    }
                },
                messages: {
                    selectHome: {
                        required: 'Chưa chọn product'
                    },
                    idxHome: {
                        required: 'không được bỏ trống index home'
                    }
                }
            });
        },
        onChangeInputTextHome: function () {
            productGetByPage.onHandleChangeIndHomeHot('Home');
        },
        onHandleChangeIndHomeHot: function (homeOrHot) {
            let firstValue = '';
            let oldValue = '';
            $('body').on('focusin', `.inputText${homeOrHot}`, function (e) {
                let $this = $(this);
                oldValue = $this.val();
                firstValue = $this.val();
            });
            $('body').on('change', `.inputText${homeOrHot}`, function (e) {
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
                            let _idxHomeOrHot = $this.val();
                            Common_another_ajax.get(`Product/ProductAddOrUn${homeOrHot}?id=${_id}&idx${homeOrHot}=${_idxHomeOrHot}&is${homeOrHot}=true`).then((data) => {
                                eval(data.CallBack);
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
