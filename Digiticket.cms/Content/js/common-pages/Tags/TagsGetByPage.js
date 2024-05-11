const baseUrlProduct = `${domain}/Tags/TagsGetByPage`;
const TYPE = {
    0: '',
    1: 'Product',
    2: 'GroupService',
    3: 'GroupServiceProcess',
    4: 'ProductHighlight',
    5: 'GroupServiceView'
};

var dtTableProduct;
var $containerTags = '.table-Tags';

var TagsGetByPage = (function () {
    return {
        init: function () {
            this.initTable(1, 10, 'CreatedDate', 'desc');
            Common.onClearFilter($containerTags, baseUrlProduct, dtTableProduct);
            Common.onSort($containerTags, baseUrlProduct, dtTableProduct);
            Common.onSelectType($containerTags, baseUrlProduct, dtTableProduct);
            Common.onSelectPageSize($containerTags, baseUrlProduct, dtTableProduct);
            Common.onGotoPage($containerTags, baseUrlProduct, dtTableProduct);
            Common.onSearchByKey($containerTags, baseUrlProduct, dtTableProduct);
            Common.onSelectFilterCateAndDes($containerTags, baseUrlProduct, dtTableProduct);
            Common.onSelectFilterStatus($containerTags, baseUrlProduct, dtTableProduct);
            this.onUpdateIdx();
            this.onSelect2();
            this.onTransShowTextToLinkText();
        },
        initTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_Tags');
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
                        url: `${baseUrlProduct}?page=${page}&perpage=${perpage}&field=${field}&sort=${sort}&categoryId=${categoryId}&key=${key}&destinationId=${destinationId}&parentId=${parentId}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        { data: 'ParentName' },
                        { data: 'CategoryName' },
                        { data: 'DestinationName' },
                        {
                            data: 'Type',
                            render: function (data) {
                                return TYPE[data];
                            }
                        },
                        { data: 'TextView' },
                        {
                            data: 'Idx',
                            width: '30px',
                            render: function (data, type, full, meta) {
                                if (permission.edit) {
                                    /*html*/
                                    return `
                                    <div>
                                        <input class="inputTextIdx inputInTable" value="${data}" data-id="${full.Id}" />
                                    </div>
                                    `;
                                } else {
                                    return data;
                                }
                            }
                        },
                        // { data: '' },
                        { data: '' },
                        { data: '' },
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
                        }
                        // { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: 7,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.add,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="loadModalContent('${domain}/Tags/TagsAddModal?parentId=${encodeURI(full.Id)}&categoryName=${encodeURI(
                                    full.CategoryName
                                )}&categoryId=${encodeURI(full.CategoryId)}&destinationName=${encodeURI(full.DestinationName)}&destinationId=${encodeURI(full.DestinationId)}&parentName=${encodeURI(
                                    full.Name
                                )}&type=${full.Type}','modal-lg','TagsGetByPage.onSelect2Modal();TagsGetByPage.onValidateAdd();')"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                     <i class="text-primary flaticon2-add-1"></i>
                                         </button>`;
                            }
                        },
                        {
                            targets: 8,
                            width: '30px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<a href="${domain}/tags/tags?parentId=${encodeURI(full.Id)}&categoryName=${encodeURI(full.CategoryName)}&categoryId=${encodeURI(
                                    full.CategoryId
                                )}&parentName=${encodeURI(full.Name)}&categoryName=${encodeURI(full.categoryName)}&destinationName=${encodeURI(full.DestinationName)}&destinationId=${encodeURI(
                                    full.DestinationId
                                )}&type=${full.Type}"
                                            class="addTags-btn cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg"
                                            >
                                                     <i class="text-primary flaticon2-fast-next"></i>
                                         </a>`;
                            }
                        }

                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerTags);
                        $('.my-loading').hide();
                    }
                });
        },
        onRendAfterUpdate: function (mess) {
            Common.onReloadDataTable(dtTableProduct, '.table-Tags', mess);
        },
        onValidateAdd: function () {
            $.validator.addMethod(
                'numberGreaterThan0',
                function (value) {
                    return /^[\d]+$/g.test(value);
                },
                'chứa ít nhất một ký tự chữ'
            );
            $.validator.addMethod(
                'GuidNotEmpty',
                function (value) {
                    return !/^00000000-0000-0000-0000-000000000000$/g.test(value);
                },
                'chứa ít nhất một ký tự chữ'
            );

            $('#TagsAdd').validate({
                rules: {
                    idx: {
                        numberGreaterThan0: true
                    },
                    textView: {
                        required: true
                    },
                    name: {
                        required: true
                    }
                },
                messages: {
                    idx: {
                        numberGreaterThan0: 'Chỉ số và khác 0'
                    },
                    textView: {
                        required: 'không được bỏ trống'
                    },
                    name: {
                        required: 'không được bỏ trống'
                    }
                }
            });
        },
        onTransShowTextToLinkText: function () {
            $(document).on('keyup', '[name="textView"]', function () {
                let _value = $(this).val();
                $('[name="textLink"]').val(Common.onFormatVN(_value.trim()).replace(' ', '-'));
            });
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
                            Common_another_ajax.get(`Tags/TagsUpdateIdx?id=${_id}&idx=${_idx}`).then((data) => {
                                eval(data.CallBack);
                            });
                        } else {
                            $this.val(firstValue);
                        }
                    });
                }
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
        onSelect2Modal: function () {
            $('.kt-select2-category--modal').select2({
                placeholder: 'Tìm danh mục'
            });
            $('.kt-select2-destination--modal').select2({
                placeholder: 'Tìm điểm đến'
            });
            $('.kt-select2-category--modal--disable').select2({
                disabled: true
            });
            $('.kt-select2-destination--modal--disable').select2({
                disabled: true
            });
            $('.kt-select2-type--modal--disable').select2({
                disabled: true
            });
        }
    };
})();
