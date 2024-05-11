const baseUrlTemplateComponent = `${domain}/LandingPage/TemplateComponentGetByPage`;
var dtTableTemplateComponent;
var $containerTemplateComponent = '.table-TemplateComponent';

var templateComponent = (function () {
    return {
        init: function () {
            this.initTable(1, 10, 'createddate', 'desc');
            Common.onClearFilter($containerTemplateComponent, baseUrlTemplateComponent, dtTableTemplateComponent);
            Common.onSort($containerTemplateComponent, baseUrlTemplateComponent, dtTableTemplateComponent);
            Common.onSelectPageSize($containerTemplateComponent, baseUrlTemplateComponent, dtTableTemplateComponent);
            Common.onGotoPage($containerTemplateComponent, baseUrlTemplateComponent, dtTableTemplateComponent);
            Common.onSearchByKey($containerTemplateComponent, baseUrlTemplateComponent, dtTableTemplateComponent);
        },
        onLoadAfterLoad: function () {
            this.onSelectTemplateComponent();
            this.onInitTagify();
        },
        onRendAfterTemplateComponentUpdate: function (mess) {
            Common.onReloadDataTable(dtTableTemplateComponent, '.table-TemplateComponent', mess);
        },
        initTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_TemplateComponent');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableTemplateComponent = table
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
                        url: `${baseUrlTemplateComponent}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        { data: 'TypeName' },
                        { data: 'ViewName' },
                        {
                            data: 'ViewImage',
                            width: '30px',
                            render: function (data) {
                                if (data) {
                                    /*html*/
                                    return `
                                    <div class="text-center">
                                        <a href="${domainCDN_MediaShowContent}${data}" target="_blank">Xem</a>
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
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="templateComponent.onLoadEditModal('${full.Id}');"
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
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="templateComponent.onDelete('${full.Id}','${full.Name}');"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                     <i class="text-red flaticon-delete"></i>
                                         </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerTemplateComponent);
                        $('.my-loading').hide();
                    }
                });
        },
        onSelectTemplateComponent: function () {
            Common.onLoadSelect2Show('select-type', $('#TemplateComponentAddOrUpdate'), '/LandingPage/TemplateComponentTypeGetListSuggest', 'Name', '', '', true);
            $('body').on('change', '.select-type', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('#type').val(selected.Id);
                } else {
                    $('#type').val('');
                }
            });
            var dataAdd = {
                page: 1,
                perpage: 30,
                field: 'createddate',
                sort: 'desc'
            };
            Common.onLoadSelect2Show('select-img', $('#TemplateComponentAddOrUpdate'), '/Media/MediaGetByPage', 'Url', dataAdd, '', true);
            $('body').on('change', '.select-img', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('#imgInput').val(selected.Url);
                } else {
                    $('#imgInput').val('');
                }
            });
        },
        onInitTagify: function () {
            var selectDefaultLinkStyles = $('#DefaultLinkStyles')[0];
            var DefaultLinkScripts = $('#DefaultLinkScripts')[0];
            new Tagify(selectDefaultLinkStyles);
            new Tagify(DefaultLinkScripts);
        },
        onLoadEditModal: function (id) {
            loadModalContent(`${domain}/LandingPage/TemplateComponentAddOrEditModal/${id}`, 'modal-lg', 'templateComponent.onLoadAfterLoad();');
        },
        onDelete: function (id, name) {
            Common_another_ajax.onDelete___get(`LandingPage/TemplateComponentDelete/${id}`, name);
        }
    };
})();
