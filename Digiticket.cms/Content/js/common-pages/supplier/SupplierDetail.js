const baseUrlSupplierBank = `${domain}/Supplier/SupplierGetBank`;
var dtTableSupplierBank;
var $containerSupplierBank = '.table-SupplierBank';
var tagifyHotline;
var isBankShown = false;
var isProductShown = false;

$.validator.addMethod(
    'notOnlyNumb',
    function (value) {
        return !/^\d+$/g.test(value) && !/^\d$/g.test(value);
    },
    'Trường này chứa ít nhất một ký tự chữ'
);

onReloadTableAjaxExcute__fun = function () {
    let $trs = $($containerProduct).find('tr');
    for (let i = 0; i < $trs.length; i++) {
        const element = $trs[i];
        if ($(element).find('.productHasBeenDeleted').length) {
            $(element).addClass('hightlightDeleted');
        }
    }
};

var SupplierDetail = (function () {
    return {
        initContentDetail: function () {
            this.onClickBanks(id);
            this.onClickProductFromDigiPost(id);
            this.onSelectTags();
            this.onValidationCreateBank();
            this.onValidateBaseInfo();
            this.onValidateBookingInfo();
            this.onClickProduct(id);
            this.onSelect2();
            this.onReloadProductFromDigiPostTable();
            isBankShown = false;
        },
        initTenant: function () {
            this.onAutoSuggestGetByKey();
            this.onSubmitUpdateTenant();
            this.onEncodeHtmlNameSupplier();
        },
        onRendAfterUpdateName: function (name) {
            $('.my-loading').hide();
            $('#defaultModal').modal('hide');
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: 'Sửa tenant thành công!',
                showConfirmButton: false,
                timer: 1500
            });
            $('.kt-portlet__head-title').find('span').html(name);
            document.title = 'Thông tin sản phẩm ' + name;
            supplier.name = name;
        },
        onRendAfterUpdateSupplierBank: function () {
            Common.onReloadDataTable(dtTableSupplierBank, '.table-SupplierBank', 'Thêm mới bank thành công');
        },
        onAutoSuggestGetByKey: function () {
            let $form = $('#FormSupplierUpdateTenant');
            Common.onLoadSelect2Show('select-suplierTenant', $form, '/Supplier/SuggestTenantByKey', 'Name', '', '', true);
            $('body').on('change', '.select-suplierTenant', function () {
                let $this = $(this);
                $('.my-loading').hide();
                let $div = $this.closest('.img-container');
                $div.find('input.d-none').remove();
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('#tenantId').val(selected.TenantId);
                    $('#tenantCode').val(selected.Code);
                    $('#nameTenant').val(selected.Name);
                }
                $form.find('.select2-selection.select2-selection--single').removeClass('border-error');
                $form.find('.error').html('');
            });
        },
        onSubmitUpdateTenant: function () {
            $('#FormSupplierUpdateTenant').on('submit', function (e) {
                let $this = $(this);
                let supplierValue = $this.find('#nameSupplier').val();
                let tenantValue = $this.find('#nameTenant').val();
                if ($this.find('#tenantId').val() === '') {
                    $this.find('.error').html('Chưa chọn tenant!');
                    $this.find('.select2-selection.select2-selection--single').addClass('border-error');
                    return false;
                }
                if (supplierValue.toLowerCase() !== tenantValue.toLowerCase()) {
                    $this.find('.error').html('Tên NCC phải trùng với tên tenant!');
                    $this.find('.select2-selection.select2-selection--single').addClass('border-error');
                    return false;
                }
                CommonAjax.onDoAjax($this);
                return false;
            });
        },
        onEncodeHtmlNameSupplier: function () {
            let value = $('#nameSupplier').val();
            $('#nameSupplier').val(Convert.decodeHTMLEntities(decodeURIComponent(Convert.onHtmlEncode(value))));
        },
        onSelectTags: function () {
            var $inputHotLine = $('input[name=hotline]')[0];
            var $inputBookingEmail = $('input[name=bookingEmail]')[0];
            new Tagify($inputHotLine, {
                transformTag: function (tagData) {
                    if (!rexPhoneNumber.test(tagData.value)) tagData.value = '';
                }
            });
            new Tagify($inputBookingEmail, {
                transformTag: function (tagData) {
                    if (!rexEmail.test(tagData.value)) tagData.value = '';
                }
            });
        },
        onClickBanks: function (id) {
            $('a[href="#supplierBanks"]').on('click', () => {
                if (!isBankShown) {
                    this.initTable(id);
                    isBankShown = true;
                }
            });
        },
        onRendAfterGetListByTenantFromDigipost: function () {
            $('.my-loading').hide();
            $('#defaultModal').modal('hide');
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: 'Sửa tenant thành công!',
                showConfirmButton: false,
                timer: 1500
            });
            productGetListByTenantFromDigipost.initTable(id);
            hardExpandVar = {
                id: id
            };
            Common.onSearchByKey($containerProductGetListByTenantFromDigipost, baseUrlProductGetListByTenantFromDigipost, dtTableProduct);
            $('#productFromDigiPost').find('.updateTenantBtn2').remove();
        },
        onClickProductFromDigiPost: function (id) {
            let nameOfRequest = encodeURI(encodeURIComponent(supplier.name));
            $('a[href="#productFromDigiPost"]').on('click', () => {
                if (!isProductGetListByTenantFromDigipostShown) {
                    if (isReadOnly) {
                        loadModalContent(
                            `${domain}/Supplier/SupplierUpdatenantModal?Id=${supplier.id}&Name=${nameOfRequest}&TenantId=${supplier.tenantId}&TenantCode=${supplier.tenantCode}`,
                            '',
                            'SupplierDetail.initTenant();'
                        );
                    } else {
                        productGetListByTenantFromDigipost.initTable(id);
                        hardExpandVar = {
                            id: id
                        };
                        Common.onSearchByKey($containerProductGetListByTenantFromDigipost, baseUrlProductGetListByTenantFromDigipost, dtTableProduct);
                    }
                    isProductGetListByTenantFromDigipostShown = true;
                }
            });
            $('.updateTenantBtn2').on('click', function (e) {
                loadModalContent(
                    `${domain}/Supplier/SupplierUpdatenantModal?Id=${supplier.id}&name=${nameOfRequest}&tenantId=${supplier.tenantId}&tenantCode=${supplier.tenantCode}`,
                    '',
                    'SupplierDetail.initTenant();'
                );
            });
        },
        onReloadProductFromDigiPostTable: function () {
            $(document).on('click', '.reloadProductFromDigiPost', function (e) {
                dtTableProduct.ajax.reload(function () {
                    CreateMeta(dtTableProduct.ajax.json());
                    Common.onLoadOption(meta, '.table-ProductGetListFromDigipost');
                });
            });
        },
        onClickProduct: function (id) {
            $('a[href="#product"]').on('click', () => {
                if (!isProductShown) {
                    productGetByPage.initTable(1, 10, 'CreatedDate', 'desc', id, true);
                    Common.onClearFilter($containerProduct, baseUrlProduct, dtTableProduct);
                    Common.onSort($containerProduct, baseUrlProduct, dtTableProduct);
                    Common.onSelectPageSize($containerProduct, baseUrlProduct, dtTableProduct);
                    Common.onGotoPage($containerProduct, baseUrlProduct, dtTableProduct);
                    Common.onSearchByKey($containerProduct, baseUrlProduct, dtTableProduct);
                    Common.onSelectFilterCateAndDes($containerProduct, baseUrlProduct, dtTableProduct);
                    Common.onSelectFilterStatus($containerProduct, baseUrlProduct, dtTableProduct);
                    Common.onSelectFilterPublic($containerProduct, baseUrlProduct, dtTableProduct);
                    isProductShown = true;
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
        },
        initTable: function (id) {
            let table = $('#html_table_SupplierBank');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableSupplierBank = table
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
                    scrollY: `${$(window).innerHeight() - 435}`,
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlSupplierBank}?id=${id}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'accountName', className: 'accountName' },
                        { data: 'accountNo', className: 'accountNo' },
                        { data: 'bankName', className: 'bankName' },
                        { data: 'branchName', className: 'branchName' },
                        { data: 'status' },
                        {
                            data: 'createdDate',
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
                                if (data) return `<div style="max-width: 60px;" class="text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                                else return '';
                            }
                        },
                        {
                            data: 'lastEditedDate',
                            type: 'date',
                            width: '60px',
                            render: function (data) {
                                if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                                else return '';
                            }
                        },
                        {
                            data: 'lastEditedByName',
                            type: 'string',
                            width: '60px',
                            render: function (data) {
                                if (data) return `<div style="max-width: 60px;" class="text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                                else return '';
                            }
                        },
                        { data: '' },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -1,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.edit,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button title="Xóa" 
                                                onclick="SupplierDetail.onDeleteBank('${full.id}', '${full.supplierId}');" 
                                                class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg"
                                                >
                                                    <i class="text-red flaticon-delete"></i>
                                        </button>`;
                            }
                        },
                        {
                            targets: -2,
                            width: '30px',
                            className: 'text-center',
                            visible: permission.edit,
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button title="Sửa" 
                                                onclick="SupplierDetail.onUpdateBank(${full.id});"
                                                data-updateBank="${full.id}"
                                                data-supplierId=${full.supplierId}
                                                class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg">
                                                    <i class="text-success fa fa-edit"></i>
                                        </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerSupplierBank);
                        $('.my-loading').hide();
                    }
                });
        },
        onUpdateBank: function (id) {
            let $btn = $(`[data-updateBank=${id}]`);
            let $container = $btn.closest('tr[role=row]');
            let _supplierId = $btn.attr('data-supplierid');
            let _accountName = $container.find('.accountName').html();
            let _accountNo = $container.find('.accountNo').html();
            let _bankName = $container.find('.bankName').html();
            let _branchName = $container.find('.branchName').html();
            Common_another_ajax.renPartial___Get(
                `Supplier/SupplierAddOrUpdateBankModal?id=${id}&supplierId=${_supplierId}&accountName=${encodeURI(_accountName)}&accountNo=${encodeURI(_accountNo)}&bankName=${encodeURI(
                    _bankName
                )}&branchName=${encodeURI(_branchName)}&`,
                undefined,
                'SupplierDetail.onValidationUpdateBank();',
                'modal-lg'
            );
        },
        onCleanInputCreateBank() {
            $('[name=accountName]').val('');
            $('[name=accountNo]').val('');
            $('[name=bankName]').val('');
            $('[name=branchName]').val('');
        },
        onDeleteBank: function (id, supplierId) {
            Common_another_ajax.onDelete___get(`/Supplier/SupplierDeleteBank?Id=${id}&supplierId=${supplierId}`);
        },
        // #region validate
        onValidationCreateBank: function () {
            $('.validateCreateBank').validate({
                rules: {
                    accountName: {
                        required: true
                    },
                    accountNo: {
                        required: true
                    },
                    bankName: {
                        required: true
                    },
                    branchName: {
                        required: true
                    }
                },
                messages: {
                    accountName: {
                        required: 'Không được bỏ trống accountName!'
                    },
                    accountNo: {
                        required: 'Không được bỏ trống accountNo!'
                    },
                    bankName: {
                        required: 'Không được bỏ trống bankName!'
                    },
                    branchName: {
                        required: 'Không được bỏ trống branchName!'
                    }
                }
            });
        },
        onValidationUpdateBank: function () {
            $('.validatiteUpdateBank').validate({
                rules: {
                    accountName: {
                        required: true
                    },
                    accountNo: {
                        required: true
                    },
                    bankName: {
                        required: true
                    },
                    branchName: {
                        required: true
                    }
                },
                messages: {
                    accountName: {
                        required: 'Không được bỏ trống accountName!'
                    },
                    accountNo: {
                        required: 'Không được bỏ trống accountNo!'
                    },
                    bankName: {
                        required: 'Không được bỏ trống bankName!'
                    },
                    branchName: {
                        required: 'Không được bỏ trống branchName!'
                    }
                }
            });
        },
        onValidateBaseInfo: function () {
            $('#upDateBaseInfo').validate({
                rules: {
                    shortDescription: {
                        required: true
                    },
                    description: {
                        required: true
                    },
                    title: {
                        required: true
                    },
                    contactName: {
                        required: true
                    },
                    phone: {
                        required: true
                    },
                    address: {
                        required: true
                    },
                    email: {
                        required: true
                    },
                    companyTaxCode: {
                        required: true
                    },
                    companyRepresentative: {
                        required: true
                    },
                    companyName: {
                        required: true
                    },
                    companyAddress: {
                        required: true
                    }
                },
                messages: {
                    shortDescription: {
                        required: 'Không được bỏ trống shortDescription!'
                    },
                    description: {
                        required: 'Không được bỏ trống description!'
                    },
                    title: {
                        required: 'Không được bỏ trống title!'
                    },
                    contactName: {
                        required: 'Không được bỏ trống contactName!'
                    },
                    phone: {
                        required: 'Không được bỏ trống phone!'
                    },
                    address: {
                        required: 'Không được bỏ trống address!'
                    },
                    email: {
                        required: 'Không được bỏ trống email!'
                    },
                    companyTaxCode: {
                        required: 'Không được bỏ trống companyTaxCode!'
                    },
                    companyRepresentative: {
                        required: 'Không được bỏ trống companyRepresentative!'
                    },
                    companyName: {
                        required: 'Không được bỏ trống companyName!'
                    },
                    companyAddress: {
                        required: 'Không được bỏ trống companyAddress!'
                    }
                }
            });
        },
        onValidateBookingInfo: function () {
            $('#updateBookingInfo').validate({
                rules: {
                    hotline: {
                        required: true
                    },
                    bookingEmail: {
                        required: true
                    }
                },
                messages: {
                    hotline: {
                        required: 'Không được bỏ trống hotline!'
                    },
                    bookingEmail: {
                        required: 'Không được bỏ trống bookingEmail!'
                    }
                }
            });
        }
    };
})();
