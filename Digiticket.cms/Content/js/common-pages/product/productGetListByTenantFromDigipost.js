const baseUrlProductGetListByTenantFromDigipost = `${domain}/Product/productGetListByTenantFromDigipost`;
var dtTableProduct;
var $containerProductGetListByTenantFromDigipost = '.table-ProductGetListFromDigipost';
var isProductGetListByTenantFromDigipostShown = false;

var productGetListByTenantFromDigipost = (function () {
    return {
        initTable: function (id) {
            let table = $('#html_table_ProductGetListFromDigipost');
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
                    scrollY: 'calc(100vh - 40.2rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlProductGetListByTenantFromDigipost}?id=${id}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'Name',
                            className: 'name'
                        },
                        { data: 'Description' },
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
                                if (data) return `<div style="max-width: 60px;" class="text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                                else return '';
                            }
                        },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -1,
                            width: '30px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="productGetListByTenantFromDigipost.onImportProductFromDigipost('${id}', '${full.Id}', '${full.Name}')"
                                                class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg"
                                                >
                                                    <i class="text-success flaticon2-add-square"></i>
                                        </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerProductGetListByTenantFromDigipost);
                        $('.my-loading').hide();
                    }
                });
        },
        onImportProductFromDigipost: function (id, productI, name) {
            Swal.fire({
                title: `Bạn chắn chắn import "${name}"?`,
                text: 'Sau khi import KHÔNG THỂ khôi phục lại được',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Có, import!',
                cancelButtonText: 'Không!'
            }).then((result) => {
                if (result.value) {
                    Common_another_ajax.get(`Product/ImportProductFromDigipost?SupplierId=${id}&ProductId=${productI}`).then((data) => {
                        if (data.Success) {
                            window.location.href = `${domain}/Product/ProductDetail?productId=${data.Data}`;
                        } else {
                            swal.fire({
                                //position: 'top-right',
                                type: 'error',
                                title: `Đã có lỗi xảy ra! ${data.Message} `,
                                showConfirmButton: false,
                                timer: 1500
                            });
                        }
                    });
                }
            });
        }
    };
})();
