const baseUrlSupplier = `${domain}/Supplier/SupplierGetByPage`;
let dtTableSupplier;
let $containerSupplier = '.table-Supplier';
var SupplierGetByPage = (function () {
    return {
        init: function () {
            this.initTable(1, 10, 'CreatedDate', 'desc');
            Common.onClearFilter($containerSupplier, baseUrlSupplier, dtTableSupplier);
            Common.onSort($containerSupplier, baseUrlSupplier, dtTableSupplier);
            Common.onSelectPageSize($containerSupplier, baseUrlSupplier, dtTableSupplier);
            Common.onGotoPage($containerSupplier, baseUrlSupplier, dtTableSupplier);
            Common.onSearchByKey($containerSupplier, baseUrlSupplier, dtTableSupplier);
        },
        initTable: function (page, perpage) {
            let table = $('#html_table_Supplier');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableSupplier = table
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
                    scrollY: 'calc(100vh - 38.3rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlSupplier}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}`,
                        type: 'get',
                        data: {
                            page: page,
                            perpage: perpage
                        }
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'Name',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<a title="Sửa" href="${domain}/Supplier/SupplierDetail?Id=${full.Id}" 
                                            class="" target="_blank">
                                                ${data}
                                        </a>`;
                            }
                        },
                        { data: 'Title' },
                        { data: 'ContactName' },
                        { data: 'Phone' },
                        { data: 'Address' },
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
                            data: 'createdByName',
                            type: 'string',
                            width: '60px',
                            render: function (data) {
                                if (data) return `<div style="max-width: 60px;" class="text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}<div>`;
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
                                if (data) return `<div style="max-width: 60px;" class="text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}<div>`;
                                else return '';
                            }
                        },
                    ],
                    columnDefs: [],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerSupplier);
                        $('.my-loading').hide();
                    }
                });
        }
    };
})();

(() => {
    SupplierGetByPage.init();
})();
