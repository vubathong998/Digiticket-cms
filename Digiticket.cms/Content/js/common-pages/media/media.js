const baseUrlMedia = `${domain}/Media/MediaGetByPage`;
var dtTableMedia;
var $containerMedia = '.table-media';

var media = (function () {
    return {
        init: function () {
            this.initTable(1, 10, 'CreatedDate', 'desc');
            Common.onClearFilter($containerMedia, baseUrlMedia, dtTableMedia);
            Common.onSort($containerMedia, baseUrlMedia, dtTableMedia);
            Common.onSearchByKey($containerMedia, baseUrlMedia, dtTableMedia);
            Common.onSelectPageSize($containerMedia, baseUrlMedia, dtTableMedia);
            Common.onGotoPage($containerMedia, baseUrlMedia, dtTableMedia);
            mediaAddModal.onUploadMedia();
        },
        initTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_Media');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableMedia = table
                .on('preXhr.dt', function (e, settings, data) {
                    $('.my-loading').show();
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    Common.onLoadTableAjax(`"{"meta":null,"data":[]}"`);
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
                        url: `${baseUrlMedia}?page=${page}&perpage=${perpage}&field=${field}&sort=${sort}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'FileName',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<a 
                                            href="${domainCDN_MediaShowContent}/${full.Url}" target="_blank"
                                            class="cursor-pointer"
                                        >
                                             ${data}
                                        </a>
                                        `;
                            }
                        },
                        {
                            data: 'AspectRatioX',
                            render: function (data, type, full, meta) {
                                return `${full.AspectRatioX} x ${full.AspectRatioY}`;
                            }
                        },
                        {
                            data: 'Width',
                            render: function (data, type, full, meta) {
                                return `${full.Width} x ${full.Height}`;
                            }
                        },
                        {
                            data: 'CreatedDate',
                            render: function (data, type, full, meta) {
                                return Convert.dateToStrDate(Convert.strDateToDate(data), 'dmy');
                            }
                        },
                        { data: '' },
                        { data: '' },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -3,
                            width: '80px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<a 
                                            href="${domainCDN_MediaShowContent}${full.Url}" target="_blank"
                                            class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg"
                                        >
                                                     <i class="text-info flaticon-eye"></i>
                                        </a>`;
                            }
                        },
                        {
                            targets: -2,
                            width: '80px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<a 
                                            href="${domain}/Media/MediaResize?id=${full.Id}&FileName=${full.FileName}&AspectRatioX=${full.AspectRatioX}&AspectRatioY=${full.AspectRatioY}" target="_blank"
                                            class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg"
                                        >
                                                     <i class="text-info flaticon2-expand"></i>
                                         </a>`;
                            }
                        },
                        {
                            targets: -1,
                            width: '80px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button class="btn text-primary" type="button" onclick="media.onCopyContent(this, '${full.Url}')">
                                            <i class="ml-1 fa fa-copy"></i>
                                        </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerMedia);
                        $('.my-loading').hide();
                    }
                });
        },
        onCopyContent: function (btn, url) {
            let post_x = btn.getBoundingClientRect().x;
            let post_y = btn.getBoundingClientRect().y;
            navigator.clipboard.writeText(`${domainCDN_MediaShowContent}${url}`);
            $('.toastContainer').css({ top: post_y, left: post_x - 200 });
            $('.toast').toast('show');
        }
    };
})();
