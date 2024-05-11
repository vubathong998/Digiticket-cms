var Currency = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' });
var DateTimeFormat = new Intl.DateTimeFormat(CurrentCulture);

var regexGuid = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/gi;
var isNotNull = function (o) {
    return o !== null && o !== undefined && o !== '';
};
var isNumber = function (n) {
    return typeof n == 'number' || (typeof n == 'object' && n.constructor === Number) || (!isNaN(parseFloat(n)) && !isNaN(n - 0));
};
var isGuid = function (t) {
    if (t[0] === '{') {
        t = t.substring(1, stringToTest.length - 1);
    }
    return regexGuid.test(t);
};
var isNotGuidEmpty = function (g) {
    if (isNotNull(g)) {
        if (g.toUpperCase().includes('00000000-0000-0000-0000-000000000000')) {
            return false;
        } else {
            return true;
        }
    } else {
        return false;
    }
};
var isJson = function (item) {
    item = typeof item !== 'string' ? JSON.stringify(item) : item;
    try {
        item = JSON.parse(item);
    } catch (e) {
        return false;
    }
    if (typeof item === 'object' && isNotNull(item)) {
        return true;
    }
    return false;
};

// list filter option (global)
const guidEmpty = '00000000-0000-0000-0000-000000000000';
var key = '';
var status = '';
var theParentKey = '';
var system = '';
var type = '';
var template = '';
var categoryId = '';
var destinationId = '';
var parentId = '';
var isPublic = '';
var productId = '';
var supplierId = '';

//var dtTable;
var page;
var pagesize;
var perpage;
var sort = 'desc';
var field = 'CreatedDate';
var onReloadTableAjaxExcute__fun = function () {};

//biến thêm khi gửi request. và k thể thay đổi.
var hardExpandVar = {};
//biến thêm khi gửi request. và có thể thay đổi.
var softExpandVar = {};

// paging info for table
var meta = new Object();
var CreateMeta = function (json) {
    if (json.meta === null) {
        meta.page = 1;
        meta.pages = 1;
        meta.perpage = 10;
        meta.total = 0;
        meta.sort = '';
        meta.field = '';
        page = 1;
        perpage = 1;
    } else {
        meta.page = json.meta.page;
        meta.pages = json.meta.pages;
        meta.perpage = json.meta.perpage;
        meta.total = json.meta.total;
        meta.sort = json.meta.sort;
        meta.field = json.meta.field;
        page = json.meta.page;
        perpage = json.meta.perpage;
    }
};

let t;
t = KTUtil.isRTL()
    ? {
          leftArrow: '<i class="la la-angle-right"></i>',
          rightArrow: '<i class="la la-angle-left"></i>'
      }
    : {
          leftArrow: '<i class="la la-angle-left"></i>',
          rightArrow: '<i class="la la-angle-right"></i>'
      };
$.fn.datepicker.dates['vi'] = {
    days: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
    daysShort: ['CN', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'],
    daysMin: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
    months: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
    monthsShort: ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'],
    today: 'Hôm nay',
    clear: 'Xóa',
    format: 'dd/mm/yyyy'
};

var Common = (function () {
    return {
        init: function () {
            Common.onBootstrapJS();
            Common.onActiveCurrent();
            Common.onCustomInputCurrency();
            Common.onCustomInputPercent();
            Common.onJustNumber();
        },
        onLoadSelectTags: function (select, parent, type = '') {
            function formatRepo(repo) {
                if (repo.loading) {
                    return repo.text;
                }
                let markup = `<div class='select2-result-repository clearfix'>
                                <div class='select2-result-repository__meta'>
                                    <div class='select2-result-repository__title'>
                                        ${repo.name ? repo.name : repo.text}
                                    </div>
                                </div>
                              </div>`;
                return markup;
            }
            function formatRepoSelection(repo) {
                return repo.name ? repo.name : repo.text;
            }
            $(parent)
                .find(`.${select}`)
                .select2({
                    placeholder: 'Search for tags',
                    allowClear: true,
                    ajax: {
                        url: `${domain}/Tags/SuggesTagsInUpdateTags`,
                        dataType: 'json',
                        delay: 1000,
                        data: function (params) {
                            var query = {
                                key: params.term,
                                type
                            };
                            return query;
                        },
                        processResults: function (data) {
                            return {
                                results: data.Data.map(function (item) {
                                    return {
                                        id: JSON.stringify(item),
                                        text: item.name,
                                        parentName: item.parentName,
                                        category: item.categoryName,
                                        destination: item.DestinationName
                                    };
                                })
                            };
                        },
                        cache: true
                    },
                    minimumInputLength: 3,
                    closeOnSelect: false,
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    templateResult: formatRepo,
                    templateSelection: formatRepoSelection
                });
        },
        onTimeOut: function () {
            Swal.fire({
                title: 'Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại!',
                type: 'warning',
                showCancelButton: false,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            }).then((result) => {
                if (result.value) {
                    window.location.href = `${domain}/Account/Login`;
                }
            });
        },
        onBootstrapJS: function () {
            $('body').tooltip({ selector: '[data-toggle=tooltip]', sanitize: false });
            $('body').popover({ selector: '[data-toggle=popover]', sanitize: false });
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $(e.target.hash).closest('.tab-content').find(`> form > .tab-pane.active, > .tab-pane.active`).not(`${e.target.hash}`).removeClass('active');
            });
        },
        onActiveCurrent: function () {
            $('.kt-aside-menu a.kt-menu__link').each(function () {
                if ($(this).prop('href') === window.location.href) {
                    $(this).closest('.kt-menu__item').addClass('kt-menu__item--active');
                    if ($(this).closest('.kt-menu__item--submenu').length) {
                        $(this).closest('.kt-menu__item--submenu').addClass('kt-menu__item--open');
                    }
                }
            });
        },
        onFormatVN: function (str) {
            if (str != null) {
                //str = str.trim().replace(/-/g, '');
                str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, 'a');
                str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, 'e');
                str = str.replace(/ì|í|ị|ỉ|ĩ/g, 'i');
                str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, 'o');
                str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, 'u');
                str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, 'y');
                str = str.replace(/đ/g, 'd');
                str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, 'A');
                str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, 'E');
                str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, 'I');
                str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, 'O');
                str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, 'U');
                str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, 'Y');
                str = str.replace(/Đ/g, 'D');
                str = str.replace(/[.,~`!@#$%^&*()_=+{}\|\[\]:;\'\"?/><]/g, '');
                str = str.replace(/ /g, '-');
                str = str.trim().replace(/\s+/g, '-');
            }
            return str;
        },
        onCustomInputCurrency: function () {
            var _currentValue;
            $('body').on('focusin', '.custom-input-currency', function () {
                _currentValue = $(this).val();
                if (_currentValue === '0' || _currentValue === '0,00') {
                    $(this).val('');
                }
            });
            $('body').on('focusout', '.custom-input-currency', function () {
                if ($(this).val() === '') {
                    $(this).val(_currentValue);
                }
            });
            $('body').on('keyup', '.custom-input-currency', function () {
                if (event.which >= 37 && event.which <= 40) return;
                // format number
                $(this)
                    .val(Common.onFormatCurrency($(this).val()))
                    .trigger('input');
                //Common.onFormatCurrency();
            });
        },
        onFormatCurrency: function (value) {
            if (CountryCode === 'vi') {
                return value.replace(/\D/g, '').replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            } else if (CountryCode === 'en') {
                return value.replace(/\D/g, '').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
            }
        },
        onCustomInputPercent: function () {
            let oldPercentValue = '';
            $(document).on('focusin', '.custom-input-percent', function () {
                let $this = $(this);
                oldPercentValue = $this.val();
            });
            $(document).on('keyup', '.custom-input-percent', function () {
                let $this = $(this);
                let value = $this.val();
                if (value === '' || (Number(value) >= 0 && Number(value) <= 100)) {
                    oldPercentValue = $this.val();
                    $this.val(value.replace(/\s+/g, ''));
                } else {
                    $this.val(oldPercentValue);
                }
            });
        },
        onJustNumber: function () {
            let oldValue = '';

            $(document).on('focusin', '[justNumber]', function (e) {
                let $this = $(this);
                oldValue = $this.val();
            });

            $(document).on('keyup', '[justNumber]', function (e) {
                let $this = $(this);
                if (isNaN($this.val()) || Number($this.val()) < 0) {
                    $this.val(oldValue);
                } else {
                    oldValue = $this.val();
                }
            });
        },
        onResetCurrency: function (value) {
            if (CountryCode === 'vi') {
                return value.split('.').join('');
            } else if (CountryCode === 'en') {
                return value.split(',').join('');
            }
        },
        onResetFormatCurrency: function (form) {
            $(form)
                .find('.custom-input-currency')
                .each(function () {
                    let _val = $(this).val();
                    if (CountryCode === 'vi') {
                        $(this).val(_val.split('.').join('')).trigger('input');
                    } else if (CountryCode === 'en') {
                        $(this).val(_val.split(',').join('')).trigger('input');
                    }
                });
        },
        onLoadTagify: function () {
            let $this = document.querySelector('.custom-tag'),
                tagify = new Tagify($this, {
                    // email address validation (https://stackoverflow.com/a/46181/104380)
                    pattern: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
                    callbacks: {
                        add: onAddTag,
                        invalid: onInvalidTag
                    }
                });
            function onAddTag(e) {}
            function onInvalidTag(e) {}
        },
        onLoadTagifyPattern: function (pattern = '') {
            let $this = document.querySelector('.custom-tag-pattern');
            $('.custom-tag-pattern').each(function () {
                tagify = new Tagify(this, {
                    pattern: pattern,
                    callbacks: {
                        add: onAddTag,
                        invalid: onInvalidTag
                    }
                });
                function onAddTag(e) {}
                function onInvalidTag(e) {}
            });
        },
        onResetTagify: function (form) {
            $(form)
                .find('input.custom-tag, input.custom-tag-pattern')
                .each(function () {
                    let $this = $(this);
                    let input = $this.val();
                    if (input != '') {
                        try {
                            let _val = JSON.parse(input);
                            let _newVal = '';
                            _val.forEach((x) => (_newVal += x.value + ','));
                            _newVal = _newVal.substr(0, _newVal.length - 1);
                            $this.val(_newVal).trigger('input');
                        } catch (error) {}
                    }
                });
        },
        onDatepicker: function (selector, startDateIsToday = false) {
            $(selector).find('.date-single').datepicker({
                rtl: KTUtil.isRTL(),
                todayHighlight: !0,
                orientation: 'bottom left',
                templates: t,
                autohide: true,
                autoclose: true,
                //startDate: new Date(),
                language: 'vi',
                dateFormat: 'DD-MM-YYYY',
                weekStart: 1
            });
            $(selector)
                .find('.date-range')
                .datepicker({
                    rtl: KTUtil.isRTL(),
                    todayHighlight: !0,
                    orientation: 'bottom left',
                    templates: t,
                    autohide: true,
                    autoclose: true,
                    ...(startDateIsToday && { startDate: new Date() }),
                    language: 'vi',
                    dateFormat: 'DD-MM-YYYY',
                    weekStart: 1
                });
            $(selector).find('.date-multiple').datepicker({
                rtl: KTUtil.isRTL(),
                multidate: true,
                todayHighlight: !0,
                orientation: 'bottom left',
                templates: t,
                //startDate: new Date(),
                language: 'vi',
                dateFormat: 'DD-MM-YYYY',
                weekStart: 1
            });
        },
        onTimepicker: function (selector) {
            $(selector).find('.time-picker').timepicker({
                minuteStep: 30,
                defaultTime: '',
                showSeconds: false,
                showMeridian: false,
                snapToStep: true
            });
        },
        onTimepickerCustomStep: function (selector, step = 1) {
            $(selector).find('.time-picker').timepicker({
                minuteStep: step,
                defaultTime: '',
                showSeconds: false,
                showMeridian: false,
                snapToStep: true
            });
        },
        onChangeDateSelect: function (selector) {
            $(selector)
                .find('.date-range')
                .datepicker()
                .on('changeDate', function (e) {
                    $(selector).find('.black-list .row').remove();
                    let datemin = $(selector).find('.begin-date').val();
                    let datemax = $(selector).find('.end-date').val();
                    Common.onAddDateRangeOff($(selector).find('.list-range'), datemin, datemax);
                });
        },
        onChangeListDateSelect: function (selector) {
            $(selector)
                .find('.date-multiple')
                .datepicker()
                .on('hide', function (e) {
                    let dates = $(selector).find('.list-datetime').val();
                    Common.onAddListDateOff($(selector).find('.list-dates'), dates);
                });
        },
        onAddDateRangeOff: function (selector, start, end) {
            let RenderDateRange = function () {
                return `<div class="row mb-1">
                            <div class="col-6">
                                <div class="input-daterange input-group date-range">
                                    <input type="text" class="form-control" name="" value="">
                                    <div class="input-group-append">
                                        <span class="input-group-text"><i class="la la-calendar-check-o"></i></span>
                                    </div>
                                    <input type="text" class="form-control" name="" value="">
                                </div>
                            </div>
                            <div class="col-6">
                                <a href="javascript:;" class="btn btn-danger btn-sm btn-icon btn-remove-daterange"><i class="la la-remove"></i></a>
                            </div>
                        </div>`;
            };
            $(selector)
                .off('click')
                .on('click', '.btn-add-daterange', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    $(selector).find('.black-list').append(RenderDateRange());
                    $(selector)
                        .find('.date-range')
                        .datepicker({
                            rtl: KTUtil.isRTL(),
                            todayHighlight: !0,
                            orientation: 'bottom left',
                            templates: t,
                            autohide: true,
                            autoclose: true,
                            language: 'vi',
                            dateFormat: DateFormat,
                            startDate: moment(start, DateFormat).toDate(),
                            endDate: moment(end, DateFormat).toDate(),
                            weekStart: 1
                        });
                });
        },
        onRemoveDateRange: function () {
            $('body').on('click', '.btn-remove-daterange', function (e) {
                e.preventDefault;
                $(this).closest('.row').remove();
            });
        },
        onAddListDateOff: function (selector, dates) {
            let $select = $(selector).find('.kt-select2');
            $select.empty();
            if ($select.hasClass('select2-hidden-accessible')) {
                $select.select2('destroy');
            }
            if (dates != '' && dates != undefined) {
                let data = dates.split(',');
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        $select.append(new Option(data[i], data[i]));
                    }
                }
            }
            $select.select2();
        },
        onDateOff: function (form, obj) {
            let $form = $(form);
            let datemin = $form.find('.begin-date').val();
            let datemax = $form.find('.end-date').val();
            let dates = $form.find('.list-datetime').val();
            Common.onChangeDateSelect($form);
            Common.onAddDateRangeOff($form.find('.list-range'), datemin, datemax);
            Common.onChangeListDateSelect($form);
            Common.onAddListDateOff($form.find('.list-dates'), dates);
            $form.on('change', `input[name="${obj}.TimeType"]`, function () {
                let $this = $(this);
                let $listrange = $form.find('.list-range');
                let $listdates = $form.find('.list-dates');
                let $select = $listdates.find('.kt-select2');

                switch ($this.val()) {
                    case '0':
                        $listdates.addClass('d-none');
                        $listrange.removeClass('d-none');
                        $form.find('.black-list .row').remove();
                        break;
                    case '1':
                        $listrange.addClass('d-none');
                        $listdates.removeClass('d-none');
                        $select.empty();
                        if ($select.hasClass('select2-hidden-accessible')) {
                            $select.select2('destroy');
                        }
                        $select.select2();
                        break;
                    default:
                        break;
                }
            });
        },
        onLoadOption: function (meta, parent) {
            let $containerPageSize = $(parent).find('.dataTables_length');
            let $containerTableInfo = $(parent).find('.dataTables_info');
            let $containerPaging = $(parent).find('.dataTables_paginate');
            $.ajax({
                url: `${domain}/Base/SelectPageSize?pagesize=${meta.perpage}`,
                type: 'get',
                cache: false,
                contentType: 'application/json',
                processData: false,
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                success: function (result) {
                    if (result.Success) {
                        if ($containerPageSize.find('.select-page-size').length) {
                            $containerPageSize.find('.select-page-size').remove();
                        }
                        $containerPageSize.append(result.Data);
                    }
                }
            });
            if ($containerTableInfo.find('.tbl-info').length) {
                $containerTableInfo.find('.tbl-info').remove();
            }
            $containerTableInfo.append(`<span class="tbl-info">Tổng số <span class="kt-font-warning"><b>${meta.total}</b></span> bản ghi</span>`);
            $.ajax({
                url: `${domain}/Base/Pagination`,
                type: 'post',
                data: JSON.stringify(meta),
                cache: false,
                contentType: 'application/json',
                processData: false,
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                success: function (result) {
                    if (result.Success) {
                        if ($containerPaging.find('.pagination').length) {
                            $containerPaging.find('.pagination').remove();
                        }
                        $containerPaging.append(result.Data);
                    }
                }
            });
        },
        onSelectPageSize: function (parent, baseUrl, dtTable, callback = '', objectNameInMulple = {}) {
            $('body').on('change', parent + ' .page-size', function () {
                let $this = $(this);
                if (Object.keys(objectNameInMulple).length) {
                    perpage = $this.val();
                    objectNameInMulple.perpage = $this.val();
                    key = objectNameInMulple.key || '';
                    sort = objectNameInMulple.sort || 'desc';
                    field = objectNameInMulple.field || 'CeatedDate';
                } else perpage = $this.val();
                page = 1;
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onGotoPage: function (parent, baseUrl, dtTable, callback = '', objectNameInMulple = {}) {
            $('body').on('click', parent + ' .page-link', function () {
                let $this = $(this);
                let $element = $(`${parent} .page-size`);
                if (Object.keys(objectNameInMulple).length) {
                    page = $this.data('dt-idx');
                    perpage = $element.val();
                    objectNameInMulple.page = $this.data('dt-idx');
                    objectNameInMulple.perpage = $element.val();
                    key = objectNameInMulple.key || '';
                    sort = objectNameInMulple.sort || 'desc';
                    field = objectNameInMulple.field || 'CreatedDate';
                } else {
                    perpage = $element.val();
                    page = $this.data('dt-idx');
                }
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onSort: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('click', '.sortable', function () {
                $('.sortable').not(this).removeClass('asc desc');
                if (!$(this).hasClass('asc') && !$(this).hasClass('desc')) {
                    $(this).addClass('desc');
                    sort = 'desc';
                } else if ($(this).hasClass('asc')) {
                    $(this).removeClass('asc').addClass('desc');
                    sort = 'desc';
                } else if ($(this).hasClass('desc')) {
                    $(this).removeClass('desc').addClass('asc');
                    sort = 'asc';
                }
                field = $(this).data('field');
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onClearFilter: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('click', `button.clearFilter`, function (e) {
                let $this = $(this);

                softExpandVar = {};
                page = '';
                pagesize = '';
                perpage = '';
                key = '';
                status = '';
                theParentKey = '';
                system = '';
                type = '';
                template = '';
                categoryId = '';
                destinationId = '';
                parentId = '';
                isPublic = '';
                productId = '';
                supplierId = '';
                sort = 'desc';
                field = $this.data('field') || 'CreatedDate';
                new Promise((resolve) => {
                    $(`${parent} .kt-select2-categories`).addClass('noLoad');
                    $(`${parent} .kt-select2-destination`).addClass('noLoad');
                    $(`${parent} .select-status`).addClass('noLoad');
                    $(`${parent} .select-public`).addClass('noLoad');
                    $(`${parent} .select-type`).addClass('noLoad');
                    $(`${parent} .filterBannerType`).addClass('noLoad');
                    resolve();
                })
                    .then(() => {
                        $(`${parent} input[name="key"]`).val('');
                        $(`${parent} .kt-select2-categories`).val('null').trigger('change');
                        $(`${parent} .kt-select2-destination`).val('null').trigger('change');
                        $(`${parent} .select-status`).val('').trigger('change');
                        $(`${parent} .select-public`).val('').trigger('change');
                        $(`${parent} .select-type`).val('').trigger('change');
                        $(`${parent} .filterBannerType`).val('').trigger('change');
                    })
                    .then(() => {
                        $(`${parent} .kt-select2-categories`).removeClass('noLoad');
                        $(`${parent} .kt-select2-destination`).removeClass('noLoad');
                        $(`${parent} .select-status`).removeClass('noLoad');
                        $(`${parent} .select-public`).removeClass('noLoad');
                        $(`${parent} .select-type`).removeClass('noLoad');
                        $(`${parent} .filterBannerType`).removeClass('noLoad');
                    });

                $(`${parent} .hightLighted`).removeClass('hightLighted');
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onSearchByKey: function (parent, baseUrl, dtTable, callback = '', objectNameInMulple = {}) {
            $('body').on('change', `${parent} input[name="key"]`, function () {
                let $this = $(this);
                page = 1;
                perpage = 10;
                if (Object.keys(objectNameInMulple).length) {
                    key = $this.val();
                    objectNameInMulple.key = $this.val();
                    page = objectNameInMulple.page || '';
                    perpage = objectNameInMulple.perpage || '';
                    sort = objectNameInMulple.sort || '';
                    field = objectNameInMulple.field || '';
                } else key = $this.val();
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onSelectType: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('change', '.select-type', function () {
                let $this = $(this);
                if (!$this.filter('.noLoad').length) {
                    page = 1;
                    perpage = 10;
                    type = $(this).val();
                    Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
                }
            });
        },
        onSelectStatus: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('change', '.select-status', function () {
                page = 1;
                perpage = 10;
                status = $(this).val();
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onSelectTemplate: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('change', '.select-template', function () {
                page = 1;
                perpage = 10;
                template = $(this).val();
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onSelectSystem: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('change', '.select-system', function () {
                page = 1;
                perpage = 10;
                system = $(this).val();
                Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
            });
        },
        onSelectFilterCateAndDes: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('change', '.kt-select2-categories', function () {
                let $this = $(this);
                if (!$this.filter('.noLoad').length) {
                    page = 1;
                    perpage = 10;
                    categoryId = $this.val();
                    Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
                }
            });
            $('body').on('change', '.kt-select2-destination', function () {
                let $this = $(this);
                if (!$this.filter('.noLoad').length) {
                    page = 1;
                    perpage = 10;
                    destinationId = $this.val();
                    Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
                }
            });
        },
        onSelectFilterStatus: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('change', '.select-status', function () {
                let $this = $(this);
                if (!$this.filter('.noLoad').length) {
                    page = 1;
                    perpage = 10;
                    status = $this.val();
                    Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
                }
            });
        },
        onSelectFilterPublic: function (parent, baseUrl, dtTable, callback = '') {
            $('body').on('change', '.select-public', function () {
                let $this = $(this);
                if (!$this.filter('.noLoad').length) {
                    page = 1;
                    perpage = 10;
                    isPublic = $this.val();
                    Common.onReloadTableAjax(parent, baseUrl, dtTable, callback);
                }
            });
        },
        onLoadTableAjax: function (res) {
            if (res) {
                if (res.includes('login page')) {
                    $('.my-loading').hide();
                    Common.onTimeOut();
                } else {
                    $('.my-loading').hide();
                }
            }
        },
        onReloadTableAjax: function (parent, baseUrl, dtTable, callback = '') {
            function convertExpandVarToUrlRequest(expandVar) {
                var resultString = '';
                if (Object.keys(expandVar).length > 0) {
                    let expandVarArr = Object.getOwnPropertyNames(expandVar);
                    for (let i = 0; i < expandVarArr.length; i++) {
                        resultString += `&${expandVarArr[i]}=${encodeURI(expandVar[expandVarArr[i]])}`;
                    }
                }
                return resultString;
            }
            let hardExpandVarStr = convertExpandVarToUrlRequest(hardExpandVar);
            let softExpandVarStr = convertExpandVarToUrlRequest(softExpandVar);
            dtTable.ajax.url(
                `${baseUrl}?page=${page}&perpage=${perpage}${hardExpandVarStr}${softExpandVarStr}&field=${field}&sort=${sort}&key=${key}&status=${status}&type=${type}&system=${system}&template=${template}&pagesize=${pagesize}&categoryId=${categoryId}&destinationId=${destinationId}&stringIsPublic=${isPublic}&status=${status}&productId=${productId}&supplierId=${supplierId}`
            );
            dtTable.ajax.reload(function () {
                if (callback !== '') {
                    eval(callback);
                }
                CreateMeta(dtTable.ajax.json());
                Common.onLoadOption(meta, parent);
                $('.my-loading').hide();
                if (onReloadTableAjaxExcute__fun) onReloadTableAjaxExcute__fun();
            });
        },
        onShowLoadingBtn: function (btn) {
            $(btn).attr('disabled', true);
            if (!$(btn).hasClass('kt-spinner')) $(btn).addClass(' kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        },
        onHideLoadingBtn: function (btn) {
            $(btn).attr('disabled', false);
            $(btn).removeClass(' kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        },
        onLoadEditor: function (el) {
            let cfg = {
                language: 'vi',
                removeDialogTabs: 'image:advanced;link:advanced',
                removeButtons: 'Save,NewPage,Preview,Print,Templates,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,Language,Flash,IFrame,ShowBlocks'
            };
            let cfg_content = {
                toolbar: [
                    { name: 'document', items: ['Source'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                    { name: 'align', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                    { name: 'paragraph', items: ['NumberedList', 'BulletedList'] },
                    { name: 'links', items: ['Link', 'Unlink'] },
                    { name: 'insert', items: ['Image', 'Table'] },
                    { name: 'styles', items: ['Format', 'FontSize'] },
                    { name: 'tools', items: ['Maximize'] }
                ],
                removeDialogTabs: 'image:advanced;link:advanced',
                language: 'vi'
            };
            $(el)
                .find('.text-html')
                .each(function () {
                    CKEDITOR.replace($(this).attr('id'), cfg_content);
                });
        },
        onLoadSelect2: function () {
            $('.kt-select2').each(function () {
                if (!$(this).hasClass('select2-hidden-accessible')) {
                    if ($(this).prop('multiple') == true) {
                        $(this).select2({ allowClear: true });
                    } else {
                        $(this).select2();
                    }
                }
            });
        },
        onProcess: function (max = 100, callback = '') {
            var elem = document.getElementById('groupServiceBar');
            let percent = 0;
            let flag = false;
            // mỗi 0,5s gọi hàm frame một lần
            let id = setInterval(frame, 500);
            function frame() {
                if (percent >= max) {
                    // nếu đã 100% thì ngừng việc gọi hàm
                    clearInterval(id);
                    if (flag && callback !== '') {
                        $(elem).remove();
                        eval(callback);
                    }
                } else {
                    // tăng chiều dài lên random từ 1-10
                    percent += Math.floor(Math.random() * 50) + 1;
                    if (percent > max) percent = max;
                    // tăng số liệu lên = width
                    elem.style.width = percent + '%';
                    elem.innerHTML = percent + '%';
                    if (percent === max) flag = true;
                }
            }
        },
        onGetUrlParameter: function (name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        },
        onReloadTable: function (nameTable, elementTable) {
            [nameTable].ajax.reload(function () {
                initComplete([nameTable].ajax.json());
                Common.onLoadOption(meta, elementTable);
            });
        },
        /**
         * @summary : tạo select 2 đơn.
         *
         * @param {string} select 1. select element name ( chỉ chứa tên class )
         * @param {string} parent 2. parent name ( một jquery element $ )
         * @param {string} url 3. url
         * @param {string?Object?} vari 4. tên biến muốn lấy để fill vào ô select. nếu muốn lấy thêm chi tiết để fill, thay vì truyền string sẽ truyền lên object.
         *              {
         *                     vari.name: (vari.exNameToShow: vari.exName)
         *              }
         * @param {object?} dataAddation 5. data thêm
         * @param {string?} message 6. placeholder
         * @param {boolean?} isCloseSelected 7. đóng hay không sau khi select
         * @param {string?} avoidDuplicateVar 8. Tên trường lấy ra so sánh để tránh bị trùng
         *
         * @return {string}:
         *
         * @Exceptions :
         */
        onLoadSelect2Show: function (select, parent, url, vari = 'Name', dataAddation = {}, message = 'Nhập để tìm kiếm', isCloseSelected = false, avoidDuplicateVar = 'Id') {
            let $select;
            if (typeof select === 'string') $select = $(parent).find(`.${select}`);
            else $select = $(select);
            function formatRepo(repo) {
                if (repo.loading) {
                    return repo.text;
                }
                function markupValue() {
                    let value = JSON.parse(repo.id);
                    if (typeof vari === 'object') {
                        let exNameStr = value[vari.exName] ? `(${vari.exNameToShow}: ${value[vari.exName]})` : '';
                        return `${value[vari.name]} ${exNameStr}`;
                    } else if (typeof vari === 'string') {
                        if (vari) {
                            return value[vari];
                        } else if (repo.text) return repo.text;
                        else return repo.name;
                    }
                }
                let markup = `<div class='select2-result-repository clearfix'>
                                <div class='select2-result-repository__meta'>
                                    <div class='select2-result-repository__title'>
                                        ${markupValue()}
                                    </div>
                                </div>
                              </div>`;
                return markup;
            }
            function formatRepoSelection(repo) {
                // if (repo[vari]) {
                //     return repo[vari];
                // } else {
                //     return repo.text;
                // }
                return repo.Name || repo.text;
            }
            $select.select2({
                placeholder: message || 'Nhập để tìm kiếm',
                allowClear: true,
                ajax: {
                    url: `${domain}/${url}`,
                    dataType: 'json',
                    delay: 1000,
                    data: function (params) {
                        var query = {
                            key: params.term,
                            ...dataAddation
                        };
                        return query;
                    },
                    processResults: function (data) {
                        if (data.data) data.Data = data.data;
                        let results = new Array();
                        for (let i = 0; i < data.Data.length; i++) {
                            let isDuplicate = false;
                            let elementValArr = $select.val();
                            if ($select.filter('[multiple]').length > 0) {
                                for (let j = 0; j < elementValArr.length; j++) {
                                    if (JSON.parse(elementValArr[j])[avoidDuplicateVar] === data.Data[i][avoidDuplicateVar]) {
                                        isDuplicate = true;
                                        break;
                                    }
                                }
                            }
                            if (isDuplicate) continue;
                            else {
                                if (typeof vari === 'object') {
                                    let exNameStr = data.Data[i][vari.exName] ? `(${vari.exNameToShow}: ${data.Data[i][vari.exName]})` : '';
                                    results.push({
                                        id: JSON.stringify(data.Data[i]),
                                        Name: `${data.Data[i][vari.name]} ${exNameStr}`,
                                        text: data.Data[i].Url
                                    });
                                } else if (typeof vari === 'string') {
                                    results.push({
                                        id: JSON.stringify(data.Data[i]),
                                        Name: data.Data[i][vari],
                                        text: data.Data[i].Url
                                    });
                                }
                            }
                        }
                        return { results: results };
                    },
                    cache: true
                },
                // minimumInputLength: 2,
                closeOnSelect: isCloseSelected,
                escapeMarkup: function (markup) {
                    return markup;
                },
                templateResult: formatRepo,
                templateSelection: formatRepoSelection
            });
            $('input.select2-search__field').attr('style', '');
        },
        /**
         * @summary : Show thành công
         *
         * @param {string?} mess .
         * @param {number?} sweelAlertDuration .
         *
         * @return {string}:
         *
         * @Exceptions :
         */
        onSuccess: function (mess = 'Update thành công', keepModal = false, sweelAlertDuration) {
            $('.my-loading').hide();
            if (!keepModal) $('#defaultModal').modal('hide');
            if (sweelAlertDuration != false && !sweelAlertDuration) sweelAlertDuration = 1500;
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: mess,
                showConfirmButton: sweelAlertDuration === false ? true : false,
                timer: sweelAlertDuration
            });
        },
        /**
         * @summary : trường hợp false mặc định sẽ không đóng modal
         *
         * @param {string}  .
         *
         * @return {string}:
         *
         * @Exceptions :
         */
        onError: function (mess, keepModal = true, sweelAlertDuration) {
            if (!mess) mess = 'Update không thành công';
            if (!sweelAlertDuration) sweelAlertDuration = 1500;
            $('.my-loading').hide();
            if (!keepModal) {
                $('#defaultModal').modal('hide');
            }
            swal.fire('Oops', mess, 'error');
        },
        /**
         * @summary : sử dụng khi update thành công và reload lại table, ẩn modal và đóng loading
         *
         * @param {object} nameTable . datatable
         * @param {string} tableClassName . tên của class
         * @param {string?} mess . Message hiển thị khi update thành công
         * @param {boolean?} keepModal . có đóng modal không. mặc định là đóng
         * @param {number?} sweelAlertDuration . Thời gian tồn tại của sweetAlert
         *
         * @return {}:
         *
         * @Exceptions :
         */
        onReloadDataTable: function (nameTable, nameTableClass, mess, keepModal = false, sweelAlertDuration) {
            if (!mess) mess = 'Update thành công';
            if (!sweelAlertDuration) sweelAlertDuration = 1500;
            $('.my-loading').hide();
            if (!keepModal) $('#defaultModal').modal('hide');
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: mess,
                showConfirmButton: false,
                timer: sweelAlertDuration
            });
            nameTable.ajax.reload(function () {
                CreateMeta(nameTable.ajax.json());
                Common.onLoadOption(meta, nameTableClass);
                if (onReloadTableAjaxExcute__fun) onReloadTableAjaxExcute__fun();
            });
        },
        /**
         * @summary : apend tags
         *
         * @param {string} $parentTags .
         * @param {string} key .
         * @param {string} selected .
         * @param {object} objectTags .
         * @param {int?} type .
         *
         * @return {string}:
         *
         * @Exceptions :
         */
        onAppendTags: function ($parentTags, key, objectTags, type = 1) {
            if (type === 1) {
                for (var i = 0; i < objectTags.length; i++) {
                    let obj = JSON.parse(typeof objectTags[i] === 'object' ? $(objectTags[i]).val() : objectTags[i]);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].Id" value="${obj.Id}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].ParentId" value="${obj.ParentId}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].CategoryId" value="${obj.CategoryId}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].DestinationId" value="${obj.DestinationId}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].TextView" value="${obj.TextView}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].TextLink" value="${obj.TextLink}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].Name" value="${obj.Name}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].Type" value="${obj.Type}" />`);
                    $parentTags.append(`<input type="text" class="d-none" name="${key}[${i}].Idx" value="${obj.Idx}" />`);
                }
            }
        },
        onInitSelect2LinkInImage: function () {
            let $pElement = $('.linkInImage');
            let nameVari = {
                name: 'Name',
                exNameToShow: 'URL',
                exName: 'URL'
            };
            for (let i = 0; i < $pElement.length; i++) {
                const $pE = $($pElement[i]);
                for (let j = 0; j < $pE.length; j++) {
                    const $this = $($pE[j]).find('select');
                    const isNeedToSet = !$pE.find('.select2-container--default').length;
                    if (isNeedToSet) {
                        Common.onLoadSelect2Show($this, $('.linkInImage'), '/LandingPage/PageGetByPage', nameVari, '', 'Nhập để tìm kiếm URL', true);
                        $('body').on('change', $this, function () {
                            if ($this.val()) {
                                let selected;
                                try {
                                    selected = JSON.parse($this.val()).URL;
                                } catch (error) {
                                    selected = $this.val();
                                }
                                if (selected) $this.closest('.linkInImage').find('.linkInput').val(selected);
                            } else {
                                $this.closest('.linkInImage').find('.linkInput').val('');
                            }
                        });
                    }
                }
            }
        },
        onInputSelectLinkInImage: function (imageValue, imageText, name = 'link') {
            let option = '';
            if (imageValue && imageText) {
                /*html*/
                option = `
                    <option class="link" value="${imageValue}" selected>${imageText}</option>
                `;
            }
            /*html*/
            return `
                <div class="linkInImage">
                    <select class="form-control select-LinkInImage">
                        ${option}
                    </select>
                    <input type="hidden" name="${name}" class="linkInput" value="${imageValue ? imageValue : ''}"/>
                </div>     
            `;
        },
        /**
         * @summary : fill HTMl từ select2. chỉ dùng cho ảnh
         *
         * @param {string?} parent . tên class, thẻ cha
         * @param {string} select . tên class, thẻ select
         * @param {string} bodySelect . tên class, body để fill
         * @param {boolean} isProduct .
         *
         * @return {string}:
         *
         * @Exceptions :
         */
        onFillSelect2ForImage: function (parent = 'body', select, bodySelect, isProduct = false, nameClear = 'clearListImg') {
            let resetIndex = function () {
                let $list = $(bodySelect).find('.imgListItem');
                let index = 0;
                $list.each(function () {
                    let $thisList = $(this);
                    $thisList.find('.name').attr('name', `images[${index}].name`);
                    $thisList.find('.linkInput').attr('name', `images[${index}].link`);
                    $thisList.find('.linkOption').attr('name', `images[${index}].linkOption`);
                    $thisList.find('.alt').attr('name', `images[${index}].alt`);
                    $thisList.find('.idx').attr('name', `images[${index}].idx`);
                    $thisList.find('.type').attr('name', `images[${index}].type`);
                    $thisList.find('.Url').attr('name', `images[${index}].url`);
                    index++;
                });
            };

            $(parent).on('select2:select', select, function (e) {
                let $this = $(this);
                let key = $this.data('key');
                let data = e.params.data;
                let url = JSON.parse(data.id).Url;
                let link = `${domainCDN_MediaShowContent}${url}`;
                let $container = $(bodySelect);
                // let index = e.target.length - 1;
                /*html*/
                let HTML = `
                <div class="mt-2 row align-items-center imgListItem">
                    <div class="col-1 text-center" data-toggle="tooltip" data-placement="top" data-original-title="${url}">
                        <a href="${link}" target="_blank"><img class="imgSmall3" src="${link}"/></a>
                    </div>
                    <div class="col-3"><input class="form-control name" name="${key}[].name" /></div>
                    <div class="col-3">
                        ${Common.onInputSelectLinkInImage('', '')}
                    </div>
                    <div class="col-1 text-center">
                        <select class="cursor-pointer form-control linkOption" name="${key}[].linkOption">
                            <option value="0">Không có</option>
                            <option value="1">Nofollow</option>
                        </select>
                        </div>
                        <div class="col-2"><input class="form-control alt" name="${key}[].alt" /></div>
                        <div class="col-1"><input type="number" min="0" class="form-control idx" name="${key}[].idx" /></div>
                        <div class="col-1 text-center">
                        <button type="button" class="${nameClear} bg-transparent border-0 type" data-url="${url}" data-toggle="tooltip" data-placement="top" title="" data-original-title="${url}">
                        <i class="flaticon2-cancel-music"></i>
                        </button>
                        <input type="hidden" class="Url" name="${key}[].url" value="${url}"/>
                        <input type="hidden" class="type" name="${key}[].type" value="1"/>
                    </div>
                </div>
                `;

                $this
                    .siblings()
                    .find('.select2-search__field')
                    .on('keypress', function (e) {
                        $('.select2-dropdown').show();
                    });

                new Promise((resolve) => {
                    $container.append(HTML);
                    resolve();
                })
                    .then(() => {
                        Common.onInitSelect2LinkInImage();
                    })
                    .then(() => {
                        if (isProduct) {
                            ProductDetail.inSetCoutImgsValid();
                        }
                        resetIndex();
                    });
            });
            $(parent).on('select2:unselect', select, function (e) {
                let $this = $(this);
                let data = e.params.data;
                let url = JSON.parse(data.id).Url;
                let theElements = $('.list-img-preview').find('div[data-original-title]');
                $this
                    .siblings()
                    .find('.select2-search__field')
                    .on('keypress', function (e) {
                        $('.select2-dropdown').show();
                    });
                $('.img-container')
                    .find('.select2-search__field')
                    .on('keypress', function (e) {});
                new Promise((resolve) => {
                    for (let i = 0; i < theElements.length; i++) {
                        if ($(theElements[i]).data('original-title') === url) {
                            theElements[i].closest('.imgListItem').remove();
                        }
                    }
                    resolve();
                })
                    .then(() => {
                        if (isProduct) {
                            ProductDetail.inSetCoutImgsValid();
                        } else {
                            return;
                        }
                    })
                    .then(() => {
                        resetIndex();
                    });
            });
        }
    };
})();

$(function () {
    Common.init();
});
