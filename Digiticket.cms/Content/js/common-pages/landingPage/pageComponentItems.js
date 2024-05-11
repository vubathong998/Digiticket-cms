const baseUrlPageComponentItems = `${domain}/LandingPage/PageComponentItemsGetByPage`;
var dtTablePageComponentItems;
var $containerPageComponentItems = '.table-PageComponentItems';
const EnumPageComponentItemsType = {
    1: { name: 'Banner_1_Medium' },
    2: { name: 'Banner_1_In_Component_Intro' },
    3: { name: 'Experience_1' },
    4: { name: 'Product_1_6' },
    5: { name: 'Product_1_Left' },
    6: { name: 'Btn_Link' },
    7: { name: 'Product_1_8' },
    8: { name: 'Video_1_Left' },
    9: { name: 'Video_1_3' },
    10: { name: 'Image_1_Left' },
    11: { name: 'Image_1_3' },
    12: { name: 'Banner_2_Big' },
    13: { name: 'Banner_2_Small' },
    14: { name: 'Banner_1_Big' },
    15: { name: 'Video_1_Right' },
    16: { name: 'Image_1_Right' },
    17: { name: 'Banner_3_Big' },
    18: { name: 'Banner_3_Small' },
    19: { name: 'Product_1_Right' },
    20: { name: 'Banner_5_Big' },
    21: { name: 'Banner_5_Small' },
    22: { name: 'Slide_1' },
    23: { name: 'Product_1_4' },
    24: { name: 'Banner_2_Medium' },
    25: { name: 'Banner_1_4' },
    26: { name: 'Product_1_3' },
    27: { name: 'Page' }
};

EnumPageComponentStatus = {
    1: { name: 'Public' },
    2: { name: 'Chưa public' }
};

var pageComponentItems = (function () {
    return {
        initImme: function () {
            this.onAddDetail();
        },
        init: function () {
            this.onUpdateIdx();
            new Promise((resolve) => {
                this.initTable(1, 10, 'Idx', 'asc');
                Common.onClearFilter($containerPageComponentItems, baseUrlPageComponentItems, dtTablePageComponentItems);
                Common.onSort($containerPageComponentItems, baseUrlPageComponentItems, dtTablePageComponentItems);
                Common.onSelectPageSize($containerPageComponentItems, baseUrlPageComponentItems, dtTablePageComponentItems);
                Common.onGotoPage($containerPageComponentItems, baseUrlPageComponentItems, dtTablePageComponentItems);
                Common.onSearchByKey($containerPageComponentItems, baseUrlPageComponentItems, dtTablePageComponentItems);
                resolve();
            }).then(() => {
                // $('.tablePageComponentItems')
                //     .find('tbody')
                //     .sortable({
                //         placeholder: 'ui-state-highlight',
                //         // change: function (event, ui) {
                //         //     var start_pos = ui.item.data('start_pos');
                //         //     var index = ui.placeholder.index();
                //         // },
                //         update: function (event, ui) {
                //             $('.updateIdx').removeClass('disabled').removeClass('cursor-not-allowed');
                //             let $container = $(event.target);
                //             for (let index = 0; index < $container.find('tr').length; index++) {
                //                 const element = $container.find('tr')[index];
                //                 $($(element).find('td')[0]).html(index + 1);
                //             }
                //         }
                //     });
            });
        },
        initTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_PageComponentItems');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTablePageComponentItems = table
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
                    // scrollY: 'calc(100vh - 40.2rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlPageComponentItems}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}&PageComponentId=${hardExpandVar.PageComponentId}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Idx' },
                        {
                            data: 'Type',
                            render: function (data) {
                                if (data >= 1 && data <= 29) return `<div>${EnumPageComponentItemsType[data].name}</div>`;
                                else return `<div></div>`;
                            }
                        },
                        {
                            data: 'ProductName',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return data || full.ObjectName || '';
                            }
                        },
                        {
                            data: 'Title',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `
                                    <div>
                                        <input type="hidden" class="id" value="${full.Id}">
                                        ${Convert.onFillterStrict(data)}
                                    </div>
                                `;
                            }
                        },
                        {
                            data: 'Image',
                            width: '30px',
                            render: function (data, type, full, meta) {
                                if (data || full.ObjectAvatar) {
                                    /*html*/
                                    return `
                                    <div class="text-center">
                                        <a href="${domainCDN_MediaShowContent}${data || full.ObjectAvatar}" target="_blank">Xem</a>
                                    </div>
                                    `;
                                } else {
                                    return '';
                                }
                            }
                        },
                        {
                            data: 'Link',
                            width: '30px',
                            render: function (data) {
                                if (data) {
                                    /*html*/
                                    return `
                                        <div class="text-center">
                                            <a href="${domainFE}${data}" target="_blank">Xem</a>
                                        </div>
                                    `;
                                } else {
                                    return '';
                                }
                            }
                        },
                        // {
                        //     data: 'CreatedDate',
                        //     type: 'date',
                        //     width: '60px',
                        //     render: function (data) {
                        //         if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                        //         else return '';
                        //     }
                        // },
                        // {
                        //     data: 'CreatedByName',
                        //     type: 'string',
                        //     width: '60px',
                        //     render: function (data) {
                        //         if (data) return `<div style="max-width: 60px;" class="cursor-context-menu text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                        //         else return '';
                        //     }
                        // },
                        // {
                        //     data: 'LastEditedDate',
                        //     type: 'date',
                        //     width: '60px',
                        //     render: function (data) {
                        //         if (Common_another.checkCorrectDate(data)) return new Intl.DateTimeFormat(CurrentCulture).format(data.slice(6, data.length - 2));
                        //         else return '';
                        //     }
                        // },
                        // {
                        //     data: 'LastEditedByName',
                        //     type: 'string',
                        //     width: '60px',
                        //     render: function (data) {
                        //         if (data) return `<div style="max-width: 60px;" class="cursor-context-menu text-truncate" data-toggle="tooltip" data-placement="top" title="${data}">${data}</div>`;
                        //         else return '';
                        //     }
                        // },
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
                                return `<button onclick="pageComponentItems.onLoadEditModal('${full.Id}','${full.Type}');"
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
                                return `<button onclick="pageComponentItems.onDelete('${full.Id}');"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                     <i class="text-red flaticon-delete"></i>
                                         </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerPageComponentItems);
                        $('.my-loading').hide();
                    }
                });
        },
        onRendAfterPageComponentItemsUpdate: function (mess) {
            Common.onReloadDataTable(dtTablePageComponentItems, '.table-PageComponentItems', mess);
        },
        onRendAfterPageComponentItemsUpdateNoReload: function (mess) {
            // $('#defaultModal').modal('hide');
            // $('#selectTypeModal').modal('hide');
            // isShowPageComponentItems = true;

            if (isShowPageComponentItems) {
                Common.onReloadDataTable(dtTablePageComponentItems, '.table-PageComponentItems', mess);
                // dtTablePageComponentItems.ajax.reload();
            } else {
                isMustLoadTable = true;
            }
            $('#defaultModal').modal('hide');
            $('#selectTypeModal').modal('hide');
        },
        onLoadEditModal: function (id, type) {
            loadModalContent(`${domain}/LandingPage/PageComponentItemsAddOrEditModal?id=${id}&type=${type}`, 'modal-lg', 'pageComponentItems.onSelectPageComponentItems()');
        },
        // onLoadAddModal: function (id) {
        //     loadModalContent(`${domain}/LandingPage/PageComponentItemsAddOrEditModal?PageComponentId=${id}`, 'modal-lg', 'pageComponentItems.onSelectPageComponentItems()');
        // },
        onDelete: function (id) {
            Common_another_ajax.onDelete___get(`LandingPage/PageComponentItemsDelete/${id}`);
        },
        onSelectPageComponentItems: function () {
            $(document).ready(function () {
                let select2Product = function () {
                    let categoryRange = $('#categoryId').val();
                    let destination = $('#destinationId').val();
                    let dataAdd = {
                        page: 1,
                        perpage: 30,
                        field: 'createddate',
                        sort: 'desc'
                    };

                    Common.onInitSelect2LinkInImage();

                    if (categoryRange.IsTrueOrFalse()) dataAdd.CategoryIdRange = categoryRange;
                    if (destination.IsTrueOrFalse()) dataAdd.DestinationId = destination;
                    Common.onLoadSelect2Show('select-Product', $('#PageComponentItemsAddOrUpdate'), '/Product/ProductGetByPage', 'Name', dataAdd, '', true);
                    $('body').on('change', '.select-Product', function () {
                        let $this = $(this);
                        $('.my-loading').hide();
                        if ($this.val()) {
                            let selected = JSON.parse($this.val());
                            $('#selectProductInput').val(selected.Id);
                        } else {
                            $('#selectProductInput').val('');
                        }
                    });
                };
                let select2Image = function () {
                    let dataAdd = {
                        page: 1,
                        perpage: 30,
                        field: 'createddate',
                        sort: 'desc'
                    };
                    Common.onLoadSelect2Show('select-Img', $('#PageComponentItemsAddOrUpdate'), '/Media/MediaGetByPage', 'Url', dataAdd, '', true);
                    $('body').on('change', '.select-Img', function () {
                        let $this = $(this);
                        $('.my-loading').hide();
                        if ($this.val()) {
                            let selected = JSON.parse($this.val());
                            $('#ImageInput').val(selected.Url);
                        } else {
                            $('#ImageInput').val('');
                        }
                    });
                };
                let select2Page = function () {
                    let dataAdd = {
                        page: 1,
                        perpage: 30,
                        field: 'createddate',
                        sort: 'desc'
                    };
                    Common.onLoadSelect2Show('select-Page', $('#PageComponentItemsAddOrUpdate'), '/LandingPage/PageGetByPage', 'Name', dataAdd, '', true);
                    $('body').on('change', '.select-Page', function () {
                        let $this = $(this);
                        $('.my-loading').hide();
                        if ($this.val()) {
                            let selected = JSON.parse($this.val());
                            $('#selectPageInput').val(selected.Id);
                        } else {
                            $('#selectPageInput').val('');
                        }
                    });
                };

                select2Product();
                select2Image();
                select2Page();
            });

            let parttenImgSrc = $('.parttenImg').attr('src');
            if (parttenImgSrc) {
                $('.getParttenImg').show();
                $('.getParttenImg').find('img').attr('src', $('.parttenImg').attr('src'));
            }
        },
        onLoadAfterLoadUpdateModal: function () {
            this.onInitSelect2();
            this.onInitTagify();
        },
        onInitSelect2: function () {
            $('.kt-select2-TemplateComponentId').select2({
                placeholder: 'Tìm templateComponentId'
            });
        },
        onInitTagify: function () {
            var LinkStyles = $('#LinkStyles')[0];
            var LinkScripts = $('#LinkScripts')[0];
            new Tagify(LinkStyles);
            new Tagify(LinkScripts);
        },
        onAddDetail: function () {
            $('.closeSlectTypeModal').on('click', function (e) {
                $('#selectTypeModal').modal('hide');
            });
            $('.continueAddPageComponentItems').on('click', function (e) {
                let value = $('#selectTypeModal').find('select').val();
                if (value != 'null') {
                    loadModalContent(
                        `${domain}/LandingPage/PageComponentItemsAddOrEditModal?PageComponentId=${hardExpandVar.PageComponentId}&Type=${value}`,
                        'modal-lg',
                        'pageComponentItems.onSelectPageComponentItems()'
                    );
                } else {
                    swal.fire({
                        //position: 'top-right',
                        type: 'warning',
                        title: 'Bạn chưa chọn thể loại',
                        showConfirmButton: false,
                        timer: 1500
                    });
                }
            });
        },
        onCKEditor: function () {
            Common.onLoadEditor('.CKEditor');
        },
        onUpdateIdx: function () {
            $(document).on('click', '.updateIdx', function (e) {
                let $this = $(this);
                if (!$this.filter('.disabled').length) {
                    Common_another.onUserSwalFire__ques('Bạn chắc chắn cập nhật lại Idx?').then((result) => {
                        if (result.value) {
                            let request = {
                                ComponentItems: new Array()
                            };
                            let $trs = $('.tablePageComponentItems').find('tr');

                            for (let i = 1; i < $trs.length; i++) {
                                request.ComponentItems.push({
                                    Id: $($trs[i]).find('.id').val(),
                                    Idx: $($($trs[i]).find('td')[0]).html()
                                });
                            }
                            Common_another_ajax.post(`Landingpage/PageComponentItemsUpdateidx?`, request).then((data) => {
                                if (data.Success) {
                                    Common.onSuccess('Cập nhật lại Idx thành công!');
                                } else {
                                    eval(data.CallBack);
                                }
                            });
                        }
                    });
                }
            });
        }
    };
})();

(() => {
    pageComponentItems.initImme();
})();
