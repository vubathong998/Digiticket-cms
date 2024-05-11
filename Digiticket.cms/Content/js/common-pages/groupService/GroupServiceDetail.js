const baseUrlGroupService = `${domain}/GroupService/GetByPageGroupService`;
var dtTableGroupService;
var $containerGroupService = '.table-groupService';
var isGroupServiceShown = false;
var current;
let isClickImageGroupServiceDetail = false;

var filterGroupservice = {
    page: 1,
    perpage: 1,
    pagesize: 10,
    key: ''
};

onReloadTableAjaxExcute__fun = function () {
    let $trs = $($containerGroupService).find('tr');
    for (let i = 0; i < $trs.length; i++) {
        const element = $trs[i];
        if ($(element).find('.productHasBeenDeleted').length) {
            $(element).addClass('hightlightDeleted');
        }
    }
};

var GroupServiceDetail = (function () {
    return {
        init: function () {
            this.onClickServicePrice();
            this.onUpdateMinTickets();
            this.onUpdateMaxTickets();
            // this.onSelectTagsGroup();
        },
        initUpdateMainInfo: function () {
            $('.kt-select2-category').select2({
                placeholder: 'Tìm danh mục'
            });
            $('.kt-select2-destination').select2({
                placeholder: 'Tìm điểm đến'
            });
        },
        initGroupSerivceTable: function (page, perpage, field = '', sort = '') {
            let table = $('#html_table_groupService');
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableGroupService = table
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
                        url: `${baseUrlGroupService}?ProductId=${productId}&Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'Name',
                            render: function (data, type, full, meta) {
                                return `
                                    <a href="${domain}/GroupService/GroupServiceDetailModal?id=${full.Id}"
                                        data-modal=""
                                        data-modal-size="modal-xl"
                                        data-callback="GroupServiceDetail.initGroupService();"
                                        >
                                        ${full.Name}
                                    </a>
                                   `;
                            }
                        },
                        {
                            data: 'DigipostName',
                            render: (data) => {
                                if (data === 'Đã xóa')
                                    /*html*/
                                    return `<div class="productHasBeenDeleted cursor-help" data-toggle="tooltip"
                                                title="Tại sao hàng này lại bị bôi xám? Vì gói dịch vụ bên Digipost đã bị xóa.">
                                                ${data} (?)</div>`;
                                else return `<div>${data}</div>`;
                            }
                        },
                        // {
                        //     data: 'status',
                        //     render: function (data, type, full, meta) {
                        //         return data;
                        //     }
                        // },
                        {
                            data: 'MinTickets',
                            width: '50px',
                            render: function (data, type, full, meta) {
                                if (permission.ProductEditGroupService) {
                                    /*html*/
                                    return `
                                <div>
                                    <input class="inputMinTickets inputInTable" value="${data}" data-id="${full.Id}" />
                                </div>
                                `;
                                } else {
                                    return data;
                                }
                            }
                        },
                        {
                            data: 'MaxTickets',
                            width: '50px',
                            render: function (data, type, full, meta) {
                                if (permission.ProductEditGroupService) {
                                    /*html*/
                                    return `
                                <div>
                                    <input class="inputMaxTickets inputInTable" value="${data}" data-id="${full.Id}" />
                                </div>
                            `;
                                } else {
                                    return data;
                                }
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
                        Common.onLoadOption(meta, $containerGroupService);
                        $('.my-loading').hide();
                        if (onReloadTableAjaxExcute__fun) onReloadTableAjaxExcute__fun();
                    }
                });
        },
        onRendAfterUpdate: function (mess) {
            Common.onReloadDataTable(dtTableGroupService, '.table-media', mess, true);
        },
        onClickServicePrice: function () {
            $(document).on('click', '#href__groupService', () => {
                if (!isGroupServiceShown) {
                    this.initGroupSerivceTable(1, 10, 'createdDate', 'desc');
                    Common.onSelectPageSize($containerGroupService, baseUrlGroupService, dtTableGroupService, '', filterGroupservice);
                    Common.onSort($containerGroupService, baseUrlGroupService, dtTableGroupService, '', filterGroupservice);
                    Common.onSearchByKey($containerGroupService, baseUrlGroupService, dtTableGroupService, '', filterGroupservice);
                    Common.onGotoPage($containerGroupService, baseUrlGroupService, dtTableGroupService, '', filterGroupservice);
                    isGroupServiceShown = true;
                }
            });
        },
        initGroupService: function () {
            this.onFillDataSelectPicture();
            this.deleteListimgGroupServiceDetail();
            this.onCKEditor();
            this.onSelectImg();
            this.onSelectTags();
            this.onSelectTagsView();
            this.onFillSelectTags();
            this.onValidate();
            this.onSelectTagsProcess();
            this.onValidateGroupServiceTags();
        },
        onCKEditor: function () {
            Common.onLoadEditor('.CKEditor__groupService');
        },
        onSelectImg: function () {
            Common.onLoadSelect2Show('select-tags-img--groupService', $('#groupService__image'), 'Media/SuggestImages', 'Url', 'Nhập để tìm kiếm ảnh sản phẩm', '', false, 'Url');
            Common.onFillSelect2ForImage('.imgGroupService-container', '.select-tags-img--groupService', '.imgListItemGroupServiceContainer', true, 'clearListImgGroupServiceDetail', '');
        },
        onFillDataSelectPicture: function () {
            let $selectImg = $('.select-tags-img--groupService');
            let key = $selectImg.data('key');
            let $selected = $selectImg.find('option').filter('[selected]');
            let HTML__tableContent = '';
            for (let i = 0; i < $selected.length; i++) {
                let obj = JSON.parse($($selected[i]).val());
                let link = `${domainCDN_MediaShowContent}${obj.Url}`;
                /*html*/
                HTML__tableContent += `
                <div class="mt-2 row align-items-center imgListItem">
                    <div class="col-1 text-center" data-toggle="tooltip" data-placement="top" data-original-title="${obj.Url}">
                        <a href="${link}" target="_blank"><img class="imgSmall3" src="${link}" alt="${obj.fileName || ''}"/></a>
                    </div>
                    <div class="col-3"><input class="form-control name" name="${key}[${i}].Name" value="${obj.Name || ''}" /></div>
                    <div class="col-3">
                        ${Common.onInputSelectLinkInImage(obj.Link, obj.Link, `${key}[${i}].link`)}
                    </div>
                    <div class="col-1 text-center">
                        <select class="cursor-pointer form-control linkOption" name="${key}[${i}].linkOption">
                            <option value="0" ${obj.LinkOption === 0 ? 'selected' : ''}>Không có</option>
                            <option value="1" ${obj.LinkOption === 1 ? 'selected' : ''}>Nofollow</option>
                        </select>
                    </div>
                    <div class="col-2"><input class="form-control alt" name="${key}[${i}].Alt" value="${obj.Alt || ''}" /></div>
                    <div class="col-1"><input type="number" min="0" class="form-control idx" name="${key}[${i}].idx" value="${obj.Idx || ''}" /></div>
                    <div class="col-1 text-center">
                        <button type="button" class="clearListImgGroupServiceDetail bg-transparent border-0" data-url="${
                            obj.Url
                        }" data-toggle="tooltip" data-placement="top" title="" data-original-title="${obj.Url}">
                            <i class="flaticon2-cancel-music"></i>
                            </button>
                            <input class="Url" type="hidden" name="${key}[${i}].Url" value="${obj.Url}"/>
                            <input class="type" type="hidden" name="${key}[${i}].type" value="1"/>
                    </div>
                </div>
                    `;
            }
            /*html*/
            let HTML = `
                    <label>Thông tin ảnh</label>
                    <div class="tableImgServiceDetailList">
                            <div class="row mx-0 align-items-center headerList py-3">
                                <div class="col-1 font-weight-bold text-center p-0">Ảnh/Xem</div>
                                <div class="col-3 font-weight-bold text-center">Tên mô tả</div>
                                <div class="col-3 font-weight-bold text-center">Link</div>
                                <div class="col-1 font-weight-bold text-center">Link Option</div>
                                <div class="col-2 font-weight-bold text-center">Thẻ alt</div>
                                <div class="col-1 font-weight-bold text-center">Idx</div>
                                <div class="col-1 font-weight-bold text-center">Clear</div>
                            </div>
                        <div class="imgListItemGroupServiceContainer border">
                            ${HTML__tableContent}
                        </div>
                    </div>
                    `;
            new Promise((resolve) => {
                $('#groupService__image .list-img-preview').html(HTML);
                resolve();
            }).then(() => {
                Common.onInitSelect2LinkInImage();
            });
        },
        deleteListimgGroupServiceDetail: function () {
            $(document).on('click', '.clearListImgGroupServiceDetail', function (e) {
                let $this = $(this);
                let _value = $this.data('url');
                let $li = $('.imgGroupService-container').find('li');

                for (let i = 0; i < $li.length; i++) {
                    let _valueLi;
                    if ($($li[i]).attr('title')) _valueLi = $($li[i]).attr('title').trim();
                    else _valueLi = $($li[i]).attr('title');

                    if (_valueLi === _value) {
                        $($li[i]).find('span').trigger('click');
                    }
                }
                $('.select2-dropdown').hide();
                $('.tooltip').remove();
                $('.imgGroupService-container').find('.select2-search__field').blur().trigger('input');
            });
        },
        onSubmitServicePriceTags: function () {
            $('#groupServiceUpdateTagsInfoForm').trigger('submit');
        },
        onSelectTags: function () {
            // Tags type 1 - don't delete
            // let type = $('#groupService__tags .select-tags').data('type');
            // let vari = {
            //     name: 'Name',
            //     exNameToShow: 'ParentName',
            //     exName: 'ParentName'
            // };
            // Common.onLoadSelect2Show('select-tags', $('#groupService__tags'), '/Tags/SuggesTagsInUpdateTags', vari, { Type: type }, '', false);
            // $('body').on('change', '.select-tags', function () {
            //     let $this = $(this);
            //     let key = $this.data('key');
            //     let $div = $this.closest('.tags-container--groupService');
            //     $div.find('input.d-none').remove();
            //     let selected = $this.val();
            //     Common.onAppendTags($div, key, selected);
            // });
            // Tags type 2
            // $('.lastSubItem').on('click', function (e) {
            //     let $this = $(this);
            //     let $tagSelected = $('#groupServiceUpdateTagsInfo .tagSelected');
            //     let value = $this.data('value');
            //     // let JSON_value = $this.data('value');
            //     // let value = JSON.parse(JSON_value);
            //     /*html*/
            //     let HTML = `
            //         <div class="tagSelected--item border">
            //             ${value.Name}
            //             <button type="button" class="border-0 bg-transparent" onclick="GroupServiceDetail.onClearGroupServiceTags(this);">
            //                 <i class="font-size-0r8 ml-1 flaticon2-cancel-music"></i>
            //             </button>
            //             <input type="hidden" class="Id" value="${value.Id}" name="Tags[].Id" />
            //             <input type="hidden" class="ParentId" value="${value.ParentId}" name="Tags[].ParentId" />
            //             <input type="hidden" class="CategoryId" value="${value.CategoryId}" name="Tags[].CategoryId" />
            //             <input type="hidden" class="DestinationId" value="${value.DestinationId}" name="Tags[].DestinationId" />
            //             <input type="hidden" class="TextView" value="${value.TextView}" name="Tags[].TextView" />
            //             <input type="hidden" class="TextLink" value="${value.TextLink}" name="Tags[].TextLink" />
            //             <input type="hidden" class="Name" value="${value.Name}" name="Tags[].Name" />
            //             <input type="hidden" class="Type" value="${value.Type}" name="Tags[].Type" />
            //             <input type="hidden" class="Idx" value="${value.Idx}" name="Tags[].Idx" />
            //         </div>
            //     `;
            //     new Promise((resolve) => {
            //         $tagSelected.append(HTML);
            //         resolve();
            //     }).then(() => {
            //         GroupServiceDetail.onResetIndexGroupServiceTags();
            //     });
            //     $this.addClass('active');
            // });
        },
        onUpdateTagSelected: function (e) {
            let $this = $(e);
            let $tagSelected = $('#groupServiceUpdateTagsInfo .tagSelected');
            let value = $this.data('value');
            let isChecked = $this[0].checked;
            let $clearGroupServiceTagBtn = $('#groupServiceUpdateTagsInfo .clearGroupServiceTagBtn');
            if (isChecked) {
                /*html*/
                let HTML = `
                    <div class="tagSelected--item border mt-1"
                        onclick="GroupServiceDetail.onFindCurrentSubItemTag(this);">
                        ${value.Name}
                        <button type="button" 
                            class="clearGroupServiceTagBtn border-0 bg-transparent" 
                            onclick="GroupServiceDetail.onClearGroupServiceTags(this);"
                            data-id="${value.Id}">
                            <i class="font-size-0r8 ml-1 flaticon2-cancel-music"></i>
                        </button>
                        <input type="hidden" class="Id" value="${value.Id}" name="Tags[].Id" />
                        <input type="hidden" class="ParentId" value="${value.ParentId}" name="Tags[].ParentId" />
                        <input type="hidden" class="CategoryId" value="${value.CategoryId}" name="Tags[].CategoryId" />
                        <input type="hidden" class="DestinationId" value="${value.DestinationId}" name="Tags[].DestinationId" />
                        <input type="hidden" class="TextView" value="${value.TextView}" name="Tags[].TextView" />
                        <input type="hidden" class="TextLink" value="${value.TextLink}" name="Tags[].TextLink" />
                        <input type="hidden" class="Name" value="${value.Name}" name="Tags[].Name" />
                        <input type="hidden" class="Type" value="${value.Type}" name="Tags[].Type" />
                        <input type="hidden" class="Idx" value="${value.Idx}" name="Tags[].Idx" />
                    </div>
                    `;

                new Promise((resolve) => {
                    $tagSelected.append(HTML);

                    resolve();
                }).then(() => {
                    GroupServiceDetail.onResetIndexGroupServiceTags();
                });
            } else {
                $clearGroupServiceTagBtn.each(function () {
                    if ($(this).data('id') === value.Id) {
                        $(this).closest('.tagSelected--item').remove();
                    }
                });
            }
        },
        onClearGroupServiceTags: function (e) {
            let $this = $(e);
            let id = $this.data('id');
            let $tagIsSelected = $this.closest('.tagSelected--item');
            let $subItemsSelected = $('#groupServiceUpdateTagsInfo').find('.updateSelectedCheckbox ');
            let subItemsSelectedLength = $subItemsSelected.length;

            new Promise((resolve) => {
                $tagIsSelected.remove();
                for (let i = 0; i < subItemsSelectedLength; i++) {
                    let element = $subItemsSelected[i];
                    if ($(element).data('value').Id === id) {
                        $(element).prop('checked', false);
                    }
                }

                resolve();
            }).then(() => {
                GroupServiceDetail.onResetIndexGroupServiceTags();
            });
        },
        onResetIndexGroupServiceTags: function () {
            let $tagsAreSelected = $('.tagSelected--item');
            let tagsAreSelectedLength = $tagsAreSelected.length;

            for (let i = 0; i < tagsAreSelectedLength; i++) {
                let $item = $($tagsAreSelected[i]);

                $item.find('.Id').attr('name', `Tags[${i}].Id`);
                $item.find('.ParentId').attr('name', `Tags[${i}].ParentId`);
                $item.find('.CategoryId').attr('name', `Tags[${i}].CategoryId`);
                $item.find('.DestinationId').attr('name', `Tags[${i}].DestinationId`);
                $item.find('.TextView').attr('name', `Tags[${i}].TextView`);
                $item.find('.TextLink').attr('name', `Tags[${i}].TextLink`);
                $item.find('.Name').attr('name', `Tags[${i}].Name`);
                $item.find('.Type').attr('name', `Tags[${i}].Type`);
                $item.find('.Idx').attr('name', `Tags[${i}].Idx`);
            }
        },
        onValidateGroupServiceTags: function () {
            $('#servicePriceTagsAdd').validate({
                rules: {
                    name: {
                        required: true
                    },
                    textView: {
                        required: true
                    }
                },
                messages: {
                    name: {
                        required: 'Chưa nhập tên!'
                    },
                    textView: {
                        required: 'Chưa nhập ký tự hiển thị!'
                    }
                }
            });
        },
        onShowSubTags: function (e, parentId) {
            let $this = $(e);
            let $itemParentTags = $this.closest('.itemParentTags');
            let $blindfoldBorderRight = $itemParentTags.find('.blindfoldBorderRight');
            let $container = $this.closest('.row');

            new Promise((resolve) => {
                $('#groupServiceUpdateTagsInfo .itemParentTags').each(function () {
                    let $this = $(this);
                    if ($this[0] !== $itemParentTags) {
                        $this.removeClass('active');
                    }
                });
                $('#groupServiceUpdateTagsInfo .blindfoldBorderRight').css('display', 'none');
                resolve();
            }).then(() => {
                $container.find('.itemSubItems').each(function (e) {
                    let $this = $(this);
                    let $addationPlace = $('#groupServiceUpdateTagsInfo .addSubTag');

                    $addationPlace.hide();
                    if ($this.data('id') == parentId) {
                        $this.toggle();
                        $itemParentTags.each(function (i, el) {
                            if ($this.data('id') === $(el).data('id')) {
                                if ($this.css('display') === 'block') {
                                    $itemParentTags.addClass('active');
                                    $blindfoldBorderRight.show();
                                } else {
                                    $itemParentTags.removeClass('active');
                                    $blindfoldBorderRight.hide();
                                }
                            }
                            if ($this.css('display') === 'block') {
                                $('#groupServiceUpdateTagsInfo .controlSubTags').show();
                            } else {
                                $('#groupServiceUpdateTagsInfo .controlSubTags').hide();
                            }
                        });
                    } else {
                        $this.css('display', 'none');
                    }
                });
            });
        },
        onfilterSubTags: function (e) {
            let $this = $(e);
            let val = Convert.removeVNTonesMark($this.val());
            let $itemSubItems = $('#groupServiceUpdateTagsInfo .lastSubItem');

            $itemSubItems.each(function () {
                let lastSubTagVal = Convert.removeVNTonesMark($(this).text());
                if (lastSubTagVal.toLowerCase().includes(val.toLowerCase())) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
        },
        onFillSelectTags: function () {
            $('.tags-container--groupService').each(function () {
                let $this = $(this);
                let key = $this.find('select').data('key');
                let $selected = $this.find('option').filter('[selected]');
                Common.onAppendTags($this, key, $selected);
            });
        },
        onSelectTagsView: function () {
            let type = $('#groupService__tagsView .select-tagsView').data('type');
            let vari = {
                name: 'Name',
                exNameToShow: 'ParentName',
                exName: 'ParentName'
            };
            Common.onLoadSelect2Show('select-tagsView', $('#groupService__tagsView'), '/Tags/SuggesTagsInUpdateTags', vari, { Type: type }, '', false);
            $('body').on('change', '.select-tagsView', function () {
                let $this = $(this);
                let key = $this.data('key');
                let $div = $this.closest('.tagsView-container--groupService');
                $div.find('input.d-none').remove();
                let selected = $this.val();
                Common.onAppendTags($div, key, selected);
            });
        },
        onAddSubTags: function (e) {
            let $this = $(e);
            let $blindfoldBorderRights = $('.blindfoldBorderRight');
            let blindfoldBorderRightsLength = $blindfoldBorderRights.length;
            let $addationPlace = $('#groupServiceUpdateTagsInfo .addSubTag');

            $addationPlace.toggle();

            for (let i = 0; i < blindfoldBorderRightsLength; i++) {
                let $element = $($blindfoldBorderRights[i]);
                if ($element.css('display') === 'block') {
                    let value = $element.data('value');

                    $addationPlace.find('.nameParentTags').html(value.Name);
                    $addationPlace.find('.parentId').val(value.Id);
                    $addationPlace.find('.categoryName').val(value.CategoryName || 'Không có');
                    $addationPlace.find('.categoryId').val(value.CategoryId || '00000000-0000-0000-0000-000000000000');
                    $addationPlace.find('.destinationName').val(value.DestinationName || 'Không có');
                    $addationPlace.find('.destinationId').val(value.DestinationId || '00000000-0000-0000-0000-000000000000');
                    $addationPlace.find('.typeName').val(TYPE[2]);
                    $addationPlace.find('.type').val(2);
                }
            }
        },
        onValidate: function () {
            $.validator.addMethod(
                'numberGreaterThan0',
                function (value) {
                    return /^[\d]+$/g.test(value);
                },
                'Chỉ chứa số'
            );
            $('#updateBaseInfo').validate({
                rules: {
                    name: {
                        required: true
                    }
                },
                messages: {
                    name: {
                        required: 'Không được bỏ trống trường này!'
                    }
                }
            });
            $('#updateImage').validate({
                rules: {
                    Picture: {
                        required: true
                    }
                },
                messages: {
                    Picture: {
                        required: 'Phải chọn ảnh trước!'
                    }
                }
            });
            $('#updateNumberTicket').validate({
                rules: {
                    minTickets: {
                        numberGreaterThan0: true
                    },
                    maxTickets: {
                        numberGreaterThan0: true
                    }
                },
                messages: {
                    minTickets: {
                        numberGreaterThan0: 'Vui lòng nhập đúng số lượng vé!'
                    },
                    maxTickets: {
                        numberGreaterThan0: 'Vui lòng nhập đúng số lượng vé!'
                    }
                }
            });
            $('#updateTagsInfo').validate({
                rules: {
                    updateTagsInfo: {
                        required: true
                    }
                },
                messages: {
                    updateTagsInfo: {
                        required: 'Chưa chọn tags!'
                    }
                }
            });
        },
        onSelectTagsProcess: function () {
            let type = $('#groupServiceUpdateTagsProcess .select-productTagsProcess').data('type');
            let vari = {
                name: 'Name',
                exNameToShow: 'ParentName',
                exName: 'ParentName'
            };
            Common.onLoadSelect2Show('select-productTagsProcess', $('#groupServiceUpdateTagsProcess'), '/Tags/SuggesTagsInUpdateTags', vari, { type: type }, '', true);
            $('body').on('change', '.select-productTagsProcess', function () {
                let $this = $(this);
                let $div = $this.closest('.tagsProcess-container');
                let key = $this.data('key');
                $div.find('input.d-none').remove();
                $div.find('input[type="hidden"]').remove();
                if ($this.val()) {
                    Common.onAppendTags($div, key, [$this.val()]);
                }
            });
        },
        onUpdateMinTickets: function () {
            let firstValue = '';
            let oldValue = '';
            $('body').on('focusin', `.inputMinTickets`, function (e) {
                let $this = $(this);
                oldValue = $this.val();
                firstValue = $this.val();
            });
            $('body').on('change', `.inputMinTickets`, function (e) {
                e.preventDefault();
                let $this = $(this);

                if ($this.val() === '') {
                    Swal.fire('Bạn chưa nhập vé tối thiểu!');
                    $this.val(oldValue);
                } else if (Number($this.val()) < 0 || isNaN($this.val())) {
                    Swal.fire('Vé tối thiểu không chính xác!');
                    $this.val(oldValue);
                } else {
                    Common_another.onUserSwalFire__ques('', 'Thay đổi vé tối thiểu?').then((result) => {
                        if (result.value) {
                            let _id = $(this).data('id');
                            let _minTicket = $this.val();
                            let _maxTicket = $this.closest('tr').find('.inputMaxTickets').val();
                            let request = {
                                Id: _id,
                                MinTickets: _minTicket,
                                MaxTickets: _maxTicket
                            };
                            Common_another_ajax.post('GroupService/GroupServiceUpdateNumberTicket', request).then((data) => {
                                eval(data.CallBack);
                            });
                        } else {
                            $this.val(firstValue);
                        }
                    });
                }
            });
        },
        onUpdateMaxTickets: function () {
            let firstValue = '';
            let oldValue = '';
            $('body').on('focusin', `.inputMaxTickets`, function (e) {
                let $this = $(this);
                oldValue = $this.val();
                firstValue = $this.val();
            });
            $('body').on('change', `.inputMaxTickets`, function (e) {
                e.preventDefault();
                let $this = $(this);

                if ($this.val() === '') {
                    Swal.fire('Bạn chưa nhập vé tối Đa!');
                    $this.val(oldValue);
                } else if (Number($this.val()) < 0 || isNaN($this.val())) {
                    Swal.fire('Vé tối đa không chính xác!');
                    $this.val(oldValue);
                } else {
                    Common_another.onUserSwalFire__ques('', 'Thay đổi vé tối đa?').then((result) => {
                        if (result.value) {
                            let _id = $(this).data('id');
                            let _maxTicket = $this.val();
                            let _minTicket = $this.closest('tr').find('.inputMinTickets').val();
                            let request = {
                                Id: _id,
                                MaxTickets: _maxTicket,
                                MinTickets: _minTicket
                            };
                            Common_another_ajax.post('GroupService/GroupServiceUpdateNumberTicket', request).then((data) => {
                                eval(data.CallBack);
                            });
                        } else {
                            $this.val(firstValue);
                        }
                    });
                }
            });
        },
        onFindCurrentSubItemTag(e) {
            let $this = $(e);
            let id = $this.find('.clearGroupServiceTagBtn').data('id');
            let $lastSubItems = $('#groupServiceUpdateTagsInfo .lastSubItem');
            let lastSubItemsLenth = $lastSubItems.length;
            for (let i = 0; i < lastSubItemsLenth; i++) {
                const $element = $($lastSubItems[i]);
                if ($element.data('id') === id) {
                    new Promise((resolve) => {
                        $('#groupServiceUpdateTagsInfo .itemSubItems').hide();
                        $('#groupServiceUpdateTagsInfo .lastSubItem').removeClass('justFound');
                        resolve();
                    }).then(() => {
                        let $itemParentTags = $('#groupServiceUpdateTagsInfo .itemParentTags');
                        let itemParentTagsLength = $itemParentTags.length;
                        let parentId = $element.find('.updateSelectedCheckbox').data('value').ParentId;

                        $element.closest('.itemSubItems').show();
                        $element[0].scrollIntoView();
                        $element.addClass('justFound');
                        $itemParentTags.removeClass('active');
                        $itemParentTags.find('.blindfoldBorderRight').hide();

                        for (let j = 0; j < itemParentTagsLength; j++) {
                            const $parentElement = $($itemParentTags[j]);
                            if ($parentElement.data('id') === parentId) {
                                $parentElement.addClass('active');
                                $parentElement.find('.blindfoldBorderRight').show();
                            }
                        }
                    });
                }
            }
        }
        // onSelectTagsGroup: function () {
        //     Common.onLoadSelect2Show('select-productTagsProcess', $('#groupServiceUpdateTagsProcess'), '/Tags/SuggesTagsInUpdateTags', 'Name', { page: 1, perpage: 30 }, '');
        //     $('body').on('change', '.select-productTagsProcess', function () {
        //         let $this = $(this);
        //         let $div = $this.closest('#select-tagsProcess');
        //         $div.find('input.d-none').remove();
        //         $div.find('input[type="hidden"]').remove();
        //         let selected = JSON.parse($this.val());
        //         if ($this.val()) {
        //             let key = $this.data('key');
        //             Common.onAppendTags($div, key, [JSON.stringify(selected)]);
        //         }
        //     });
        // }
    };
})();

// var TagsGetByPage = (function () {
//     return {
//         onRendAfterUpdate: function (mess, dataJSON) {
//             let data = JSON.parse(dataJSON);

//         }
//     };
// })();
