const baseUrlTemplate = `${domain}/LandingPage/TemplateGetByPage`;
var dtTableTemplate;
var $containerTemplate = '.table-Template';

var template = (function () {
    return {
        init: function () {
            this.initTable(1, 10, 'createddate', 'desc');
            Common.onClearFilter($containerTemplate, baseUrlTemplate, dtTableTemplate);
            Common.onSort($containerTemplate, baseUrlTemplate, dtTableTemplate);
            Common.onSelectPageSize($containerTemplate, baseUrlTemplate, dtTableTemplate);
            Common.onGotoPage($containerTemplate, baseUrlTemplate, dtTableTemplate);
            Common.onSearchByKey($containerTemplate, baseUrlTemplate, dtTableTemplate);
        },
        onRendAfterTemplateUpdate: function (mess) {
            Common.onReloadDataTable(dtTableTemplate, '.table-Template', mess);
        },
        onRendAfterTemplateEdit: function (mess) {
            Common.onReloadDataTable(dtTableTemplate, '.table-Template', mess);
        },
        onLoadAfterTemplateMapModalIsShown: function () {
            Common.onLoadSelect2Show('select-templateComponent', $('#TemplateMap'), '/LandingPage/TemplateComponentGetByPage', 'Name', '', '', true);
            $('body').on('change', '.select-templateComponent', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('#inputTemplateComponentId').val(selected.Id);
                } else {
                    $('#inputTemplateComponentId').val('');
                }
            });
        },
        onRendAfterTemplateMap: function (mess) {
            Common.onSuccess('Map thành công!');
            // Common.onReloadDataTable(dtTableTemplate, '.table-Template', mess);
        },
        initTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_Template');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableTemplate = table
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
                        url: `${baseUrlTemplate}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'Name',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `
                                <div>
                                    <a href="${domain}/LandingPage/TemplateDetail/${full.Id}" target="_blank">${data}</a>
                                </div>
                            `;
                            }
                        },
                        { data: 'SystemName' },
                        { data: 'ViewName' },
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
                        // { data: '' },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -2,
                            width: '30px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="template.onLoadEditModal('${full.Id}');"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg p-0" data-callback=""
                                                 >
                                                     <i class="font-size-1r4 text-success flaticon2-edit"></i>
                                         </button>`;
                            }
                        },
                        // {
                        //     targets: -2,
                        //     width: '30px',
                        //     className: 'text-center',
                        //     render: function (data, type, full, meta) {
                        //         /*html*/
                        //         return `<button onclick="template.onTemplateMap('${full.Id}');"
                        //                         class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg p-0">
                        //                             <i class="font-size-1r6 text-success flaticon-map"></i>
                        //                 </button>`;
                        //     }
                        // },
                        {
                            targets: -1,
                            width: '30px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="template.onDelete('${full.Id}','${full.Name}');"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg p-0" data-callback=""
                                                 >
                                                     <i class="font-size-1r4 text-red flaticon-delete"></i>
                                         </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerTemplate);
                        $('.my-loading').hide();
                    }
                });
        },
        onLoadEditModal: function (id) {
            loadModalContent(`${domain}/LandingPage/TemplateAddOrEditModal/${id}`, 'modal-lg');
        },
        onTemplateMap: function (id) {
            loadModalContent(`${domain}/LandingPage/TemplateMapModal/${id}`, 'modal-lg', 'template.onLoadAfterLoadTemplateMapModal();');
        },
        onDelete: function (id, name) {
            Common_another_ajax.onDelete___get(`LandingPage/TemplateDelete/${id}`, name);
        }
    };
})();
