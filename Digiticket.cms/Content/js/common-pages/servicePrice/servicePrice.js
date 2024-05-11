var servicePrice = (function () {
    return {
        init: function () {
            this.onClickServicePrice();
        },
        initServicePriceTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_ServicePrice');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableServicePrice = table
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
                        url: `${baseUrlServicePrice}?ProductId=${productId}&Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'Name',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `
                                    <a href="${domain}/ServicePrice/ServicePriceDetailModal?id=${full.Id}"
                                        class="btn-elevate btn-icon-sm" data-callback="servicePrice.onComissionValue();"
                                        data-modal="" data-modal-size="modal-xl">
                                            ${data}
                                    </a>
                                    <input type="hidden" value="${encodeURIComponent(JSON.stringify(full))}" class="oldServicePrice" data-id="${full.Id}" />
                                `;
                            }
                        },
                        {
                            data: 'DigipostName',
                            render: function (data) {
                                if (Common_another.ciEquals(data, 'Đã xóa'))
                                    return `
                                <span class="productHasBeenDeleted">
                                    ${data}
                                <span>
                            `;
                                else return data;
                            }
                        },
                        {
                            data: 'IsChanged',
                            render: (data) => {
                                if (data) return `Có`;
                                else return `Không`;
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
                                if (data) return `<div>${data}</div>`;
                                else return '';
                            }
                        }
                    ],
                    columnDefs: [],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerServicePrice);
                        $('.my-loading').hide();
                        let $trs = $($containerServicePrice).find('tr');
                        for (let i = 0; i < $trs.length; i++) {
                            const element = $trs[i];
                            if ($(element).find('.productHasBeenDeleted').length) {
                                $(element).addClass('hightlightDeleted');
                            }
                        }
                    }
                });
        },
        onRendAfterUpdateServicePriceFromDigipost: function (mess, KeepModal) {
            Common.onReloadDataTable(dtTableServicePrice, '.table-ProductGetListFromDigipost', mess, KeepModal, 2000);
        },
        onClickServicePrice: function () {
            $('#href__services').on('click', () => {
                if (!$isServicePriceShown) {
                    this.initServicePriceTable(1, 10, 'createdDate', 'desc');
                    Common.onSelectPageSize($containerServicePrice, baseUrlServicePrice, dtTableServicePrice, '', filterServicePrice);
                    Common.onSort($containerServicePrice, baseUrlServicePrice, dtTableServicePrice, '', filterServicePrice);
                    Common.onSearchByKey($containerServicePrice, baseUrlServicePrice, dtTableServicePrice, '', filterServicePrice);
                    Common.onGotoPage($containerServicePrice, baseUrlServicePrice, dtTableServicePrice, '', filterServicePrice);
                    $isServicePriceShown = true;
                }
            });
        },
        onComissionValue: function () {
            $('.commissionUnit').on('change', function (e) {
                let $this = $(this);
                let commissionValue = $(this).closest('tr').find('.commissionValue');
                if ($this.val() == commissionUnit.cash) {
                    commissionValue.val('');
                    commissionValue.removeClass('custom-input-percent');
                    commissionValue.addClass('custom-input-currency');
                } else if ($this.val() == commissionUnit.percent) {
                    commissionValue.val('');
                    commissionValue.removeClass('custom-input-currency');
                    commissionValue.addClass('custom-input-percent');
                }
            });
        },
        initImportServicePriceFromDigipostModal: function () {
            this.onMupleUpdateFromDigipost();
        },
        onMupleUpdateFromDigipost: function () {
            $('.mutipleImportFromDigipost').on('click', function (e) {
                let $selectComponents = $('.selectServicePriceToImport');
                let servicePriceIds = new Array();

                for (let i = 0; i < $selectComponents.length; i++) {
                    const $element = $($selectComponents[i]);
                    if ($element[0].checked) servicePriceIds.push($element.data('id'));
                }
                if (servicePriceIds.length === 0) {
                    swal.fire({
                        type: 'error',
                        title: 'Bạn chưa chọn Service price nào!',
                        showConfirmButton: false,
                        timer: 1500
                    });
                } else {
                    Common_another.onUserSwalFire__ques('Bạn đã chắc chắn với hành động này?', 'Sau khi import nhiều giai đoạn giá. Bạn sẽ không thể khôi phục lại được!').then((result) => {
                        if (result.value) {
                            let request = {
                                productId,
                                servicePriceIds
                            };
                            Common_another_ajax.post('ServicePrice/ServicePriceImportListServicePriceFromDigipost', request).then((data) => {
                                eval(data.CallBack);
                                if (data.Success) {
                                    $selectComponents.prop('checked', false);
                                }
                            });
                        }
                    });
                }
            });
            $('.selectServicePriceToImport').on('change', function () {
                let num_selected = 0;
                for (let i = 0; i < $('.selectServicePriceToImport').length; i++) {
                    const element = $('.selectServicePriceToImport')[i];
                    if (element.checked) {
                        num_selected++;
                    }
                }
                $('#selectAllToImport').prop('checked', num_selected === $('.selectServicePriceToImport').length);
            });
            $('#selectAllToImport').on('change', function (e) {
                let $this = $(this);
                let $selectComponents = $('.selectServicePriceToImport');

                $selectComponents.prop('checked', $this[0].checked);
            });
            $('.mutipleUpdateFromDigipost').on('click', function (e) {
                let $selectComponents = $('.selectServicePriceToUpdate');
                let servicePriceIds = new Array();

                for (let i = 0; i < $selectComponents.length; i++) {
                    const $element = $($selectComponents[i]);
                    if ($element[0].checked) servicePriceIds.push($element.data('id'));
                }
                if (servicePriceIds.length === 0) {
                    swal.fire({
                        type: 'error',
                        title: 'Bạn chưa chọn Service price nào!',
                        showConfirmButton: false,
                        timer: 1500
                    });
                } else {
                    let request = {
                        productId,
                        servicePriceIds
                    };
                    Common_another_ajax.post('ServicePrice/UpdateListServicepriceFromDigipost', request).then((data) => {
                        eval(data.CallBack);
                        if (data.Success) {
                            $selectComponents.prop('checked', false);
                        }
                    });
                }
            });
            $('.selectServicePriceToUpdate').on('change', function () {
                let num_selected = 0;
                for (let i = 0; i < $('.selectServicePriceToUpdate').length; i++) {
                    const element = $('.selectServicePriceToUpdate')[i];
                    if (element.checked) {
                        num_selected++;
                    }
                }
                $('#selectAllToUpdate').prop('checked', num_selected === $('.selectServicePriceToUpdate').length);
            });
            $('#selectAllToUpdate').on('change', function (e) {
                let $this = $(this);
                let $selectComponents = $('.selectServicePriceToUpdate');

                $selectComponents.prop('checked', $this[0].checked);
            });
        },
        onLoadCompareServicePriceModal: function (newServicePrice) {
            let newServicePrices__obj = JSON.parse(newServicePrice);
            newServicePrices__obj.PricesTime.BeginDate = Convert.strDateToDate(newServicePrices__obj.PricesTime.BeginDate);
            newServicePrices__obj.PricesTime.EndDate = Convert.strDateToDate(newServicePrices__obj.PricesTime.EndDate);

            let $oldServicePrices = $('.oldServicePrice');
            let request = new Object();
            for (let i = 0; i < $oldServicePrices.length; i++) {
                if ($($oldServicePrices[i]).data('id') === newServicePrices__obj.Id) {
                    request.newServicePrice = newServicePrices__obj;
                    request.oldServiceprice = JSON.parse(decodeURIComponent($($oldServicePrices[i]).val()));
                    request.oldServiceprice.PricesTime.BeginDate = Convert.strDateToDate(request.oldServiceprice.PricesTime.BeginDate);
                    request.oldServiceprice.PricesTime.EndDate = Convert.strDateToDate(request.oldServiceprice.PricesTime.EndDate);

                    break;
                }
            }
            Common_another_ajax.post('ServicePrice/ServicePriceCompare', request).then((data) => {
                $('.my-loading').hide();
                $('#CompareServicePriceModal').modal('show');
                $('#CompareServicePriceModal').find('.modal-content').html(data.Data);
            });
        },
        onImportServiceprice: function (servicePriceIds) {
            let request = {
                productId,
                servicePriceIds: [servicePriceIds],
                KeepModal: true
            };
            Common_another_ajax.post('ServicePrice/UpdateListServicepriceFromDigipost', request).then((data) => {
                eval(data.CallBack);
                $('#CompareServicePriceModal').modal('hide');
                $('#defaultModal').modal('hide');
            });
        }
    };
})();
