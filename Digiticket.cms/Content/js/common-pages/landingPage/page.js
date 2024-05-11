const baseUrlPage = `${domain}/LandingPage/PageGetByPage`;
var dtTablePage;
var $containerPage = '.table-Page';
const EnumPageStatus = {
    0: { name: 'Chưa chọn' },
    1: { name: 'Public' },
    2: { name: 'Chưa public' }
};
const TYPE = {
    0: '',
    1: 'Product',
    2: 'GroupService',
    3: 'GroupServiceProcess',
    4: 'ProductHighlight',
    5: 'GroupServiceView'
};
const EnumPageType = {
    1: { name: 'Common' },
    2: { name: 'Blog' }
};

page = 1;
perpage = 10;


var pageFunc = (function () {
    return {
        init: function () {
            this.initTable(1, 10, 'createddate', 'desc');
            Common.onClearFilter($containerPage, baseUrlPage, dtTablePage);
            Common.onSort($containerPage, baseUrlPage, dtTablePage);
            Common.onSelectPageSize($containerPage, baseUrlPage, dtTablePage);
            Common.onGotoPage($containerPage, baseUrlPage, dtTablePage);
            Common.onSearchByKey($containerPage, baseUrlPage, dtTablePage);
            this.onEditStatus();
            this.onFilter();
        },
        onLoadAfterLoad: function () {
            this.onSelectTemplate();
            this.onInitTagify();
            this.onSelect2();
            pageCommon.select2();
        },
        onHandleSelectPagetype: function (e) {
            let $this = $(e);
            let value = $this.val();
            if (value === '1') {
                $('.selectCategoryTag').find('.select-categoryTag').val(0).trigger('change');

                $('.selectCategoryTag').hide();
            } else if (value === '2') {
                $('.selectCategoryTag').show();
            }
        },
        onFilter: function () {
            $('.filterStatus').on('change', function (e) {
                let $this = $(this);
                if (!$this.filter('.noLoad').length) {
                    softExpandVar.Status = $(this).val();
                    Common.onReloadTableAjax($containerPage, baseUrlPage, dtTablePage);
                }
            });
            $('.clearFilter').on('click', function (e) {
                new Promise((resolve) => {
                    $('.filterStatus').addClass('noLoad');
                    $('.filterStatus').addClass('noLoad');
                    resolve();
                })
                    .then(() => {
                        $('.filterStatus').val('').trigger('change');
                    })
                    .then(() => {
                        $('.filterStatus').removeClass('noLoad');
                    });
            });
            Common.onLoadSelect2Show('kt-select2-filterTemplate', $('body'), 'LandingPage/TemplateGetByPageForSuggest', 'Name', '', '', true);
            $('body').on('change', '.kt-select2-filterTemplate', function () {
                let $this = $(this);
                let id = '';
                if ($this.val()) {
                    id = JSON.parse($this.val()).Id;
                }
                if (!$this.filter('.noLoad').length) {
                    softExpandVar.TemplateId = id;
                    Common.onReloadTableAjax($containerPage, baseUrlPage, dtTablePage);
                }
                $('.clearFilter').on('click', function (e) {
                    new Promise((resolve) => {
                        $('.kt-select2-filterTemplate').addClass('noLoad');
                        $('.kt-select2-filterTemplate').addClass('noLoad');
                        resolve();
                    })
                        .then(() => {
                            $('.kt-select2-filterTemplate').val('').trigger('change');
                        })
                        .then(() => {
                            $('.kt-select2-filterTemplate').removeClass('noLoad');
                        });
                });
            });
        },
        onSelectTemplate: function () {
            Common.onLoadSelect2Show('select-page', $('#PageAddOrUpdateModal'), '/LandingPage/TemplateGetByPageForSuggest', 'Name', '', '', true);
            $('body').on('change', '.select-page', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('#TemplateIdInput').val(selected.Id);
                } else {
                    $('#TemplateIdInput').val('');
                }
            });
        },
        onInitTagify: function () {
            var LinkStyles = $('#LinkStyles')[0];
            var LinkScripts = $('#LinkScripts')[0];
            new Tagify(LinkStyles);
            new Tagify(LinkScripts);
        },
        initTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_Page');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTablePage = table
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
                        url: `${baseUrlPage}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name', render: (data, type, full, meta) => `<a href="${domain}/LandingPage/PageDetail/${full.Id}" target="_blank">${data}</a>` },
                        {
                            data: 'Type',
                            render: function (data) {
                                if (data == 1 || data == 2) {
                                    return EnumPageType[data].name;
                                } else return '';
                            }
                        },
                        { data: 'TemplateName' },
                        {
                            data: 'Status',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `
                                <div>
                                            <select class="updateStatus selectInTable" data-id="${full.Id}">
                                                <option value="0" ${data == 0 ? 'selected' : ''}>${EnumPageStatus[0].name}</option>
                                                <option value="1" ${data == 1 ? 'selected' : ''}>${EnumPageStatus[1].name}</option>
                                                <option value="2" ${data == 2 ? 'selected' : ''}>${EnumPageStatus[2].name}</option>
                                                </select>
                                                </div>
                                                `;
                            }
                        },
                        {
                            data: 'URL',
                            width: '30px',
                            className: 'text-center',
                            render: function (data) {
                                /*html*/
                                return `
                                        <div class="text-center">
                                            <a href="${domainFE}${data}" target="_blank">Xem</a>
                                        </div>
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
                        { data: '' }
                        // { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -1,
                            width: '30px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="pageFunc.onLoadEditModal('${full.Id}');"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg p-0" data-callback=""
                                                 >
                                                     <i class="font-size-1r4 text-success flaticon2-edit"></i>
                                         </button>`;
                            }
                        }
                        // {
                        //     targets: -1,
                        //     width: '30px',
                        //     className: 'text-center',
                        //     render: function (data, type, full, meta) {
                        //         /*html*/
                        //         return `<button onclick="pageFunc.onDelete('${full.Id}','${full.Name}');"
                        //                          class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg p-0" data-callback=""
                        //                          >
                        //                              <i class="font-size-1r4 text-red flaticon-delete"></i>
                        //                  </button>`;
                        //     }
                        // }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerPage);
                        $('.my-loading').hide();
                    }
                });
        },
        onEditStatus: function () {
            $('body').on('change', `.updateStatus`, function (e) {
                e.preventDefault();
                let $this = $(this);

                Common_another.onUserSwalFire__ques('', 'Thay đổi status?').then((result) => {
                    if (result.value) {
                        let _id = $(this).data('id');
                        let _status = $this.val();
                        Common_another_ajax.get(`LandingPage/PageEditStatus?id=${_id}&Status=${_status}`).then((data) => {
                            if (data.Success) {
                                eval(data.CallBack);
                            } else {
                                Common.onError(data.Message);
                            }
                        });
                    }
                });
            });
        },
        onSelect2: function () {
            // $('.selectPageType').select2({
            //     placeholder: 'Tìm thể loại page'
            // });
            $('.selectPageTypeDisable').select2({
                disabled: true
            });
        },
        onRendAfterUpdatePage: function (mess) {
            Common.onReloadDataTable(dtTablePage, '.table-Page', mess);
            $('#selectPageTypeModal').modal('hide');
        },
        onLoadEditModal: function (id) {
            loadModalContent(`${domain}/LandingPage/PageAddOrEditModal/${id}`, 'modal-xl', 'pageFunc.onLoadAfterLoad();');
        },
        onDelete: function (id, name) {
            Common_another_ajax.onDelete___get(`LandingPage/PageDelete/${id}`, name);
        },
        onShowAddPageModal: function (element) {
            let $this = $(element);
            let value = $this.closest('.modal-content').find('.kt-select2-TypePage').val();
            loadModalContent(`${domain}/LandingPage/PageAddOrEditModal?type=${value}`, 'modal-xl', 'pageFunc.onLoadAfterLoad();');
        }
    };
})();
