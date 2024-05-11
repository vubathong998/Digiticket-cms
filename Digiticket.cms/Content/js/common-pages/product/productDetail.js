$.validator.addMethod(
    'not0andText',
    function (value) {
        return /^[1-9]+$/g.test(value);
    },
    'Trường này chỉ chứa số khác 0'
);
var filterServicePrice = {
    page: 1,
    perpage: 1,
    pagesize: 10,
    key: ''
};

const TYPE = {
    0: '',
    1: 'Product',
    2: 'GroupService',
    3: 'GroupServiceProcess',
    4: 'ProductHighlight',
    5: 'GroupServiceView'
};

const baseUrlServicePrice = `${domain}/ServicePrice/GetByPageServicePrice`;
var dtTableServicePrice;
var $containerServicePrice = '.table-ServicePrice';
var $isServicePriceShown = false;
var isShowTableListImgAsndVideos = false;

var ProductDetail = (function () {
    return {
        initDetail: function () {
            $(document).ready(() => {
                this.onCKEditor();
                this.onSelectTags();
                this.onSelectTagsHightLight();
                this.onSelect2TagView();
                this.onSelect2TagGroup();
                this.onSelectTagsView();
                this.onFillDataSelectTag();
                Common.onInitSelect2LinkInImage();
                this.onValidation();
                this.onAutoSuggestPlaceInBaseInfo();
                this.onSelectImg();
                this.onAutoSuggestOnePic();
                this.initTagsGroup();
                this.onSelectVideo();
                this.onVideoProduct();
                this.onUpdateQueryPrice();
                this.onDraggable();
                this.onHandleTagsHightLightValue();
            });
        },
        loadIndex: function () {
            let index = 0;
            let $table = $('#DataTables_Table_1.tableVideoProduct');
            let $list = $table.find('tbody tr:not([class])');
            $list.each(function () {
                let $this = $(this);
                $this.find('.name').attr('name', `videos[${index}].name`);
                $this.find('.link').attr('name', `videos[${index}].link`);
                $this.find('.alt').attr('name', `videos[${index}].alt`);
                $this.find('.idx').attr('name', `videos[${index}].idx`);
                $this.find('.type').attr('name', `videos[${index}].type`);
                $this.find('.url').attr('name', `videos[${index}].url`);
                index++;
            });
        },
        onRendAfterUpdateMainInfo: function (success, mess = '') {
            if (success) {
                $('.my-loading').hide();
                swal.fire({
                    //position: 'top-right',
                    type: 'success',
                    title: 'Update Thành công!',
                    showConfirmButton: false,
                    timer: 1500
                });
                location.reload();
            } else {
                $('.my-loading').hide();
                swal.fire({
                    //position: 'top-right',
                    type: 'error',
                    title: mess,
                    showConfirmButton: false,
                    timer: 1500
                });
            }
        },
        onRendAfterBaseUpdate: function () {
            $('.my-loading').hide();
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: 'Update thành công',
                showConfirmButton: false,
                timer: 1500
            });
        },
        onAutoSuggestOnePic: function () {
            Common.onLoadSelect2Show('select-productAvatar', $('#updateImgInfo'), '/Media/AutoSuggestAvatar', 'Url', '', '', true);
            $('body').on('change', '.select-productAvatar', function () {
                let $this = $(this);
                let $container = $('#updateImgInfo');
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    let url = `${domainCDN_MediaShowContent}${selected.Url}`;
                    $('.productAvatar').val(selected.Url);

                    $container.find('.lookImg').attr('src', `${url}`);
                    $container.find('.nameLockingImg').val(selected.Url);
                    $container.find('.img').find('.showProductAvatar').html(`    
                        <b><a href="${url}" title="" target="_blank">Xem trước</a></b>
                     `);
                } else {
                    $container.find('.lookImg').attr('src', '');
                    $container.find('.nameLockingImg').val('');
                    $container.find('.img').find('a').remove();
                    $container.find('.img').find('.showProductAvatar').html(``);
                }
            });
        },
        onDraggable: function () {
            $('.tagsGroup').sortable({
                placeholder: 'ui-state-highlight'
            });
        },
        onSuggestAvatar: function () {
            $('.kt-select2-category').select2({
                placeholder: 'Tìm danh mục'
            });
            $('.kt-select2-destination').select2({
                placeholder: 'Tìm điểm đến'
            });
        },
        initTagsGroup: function () {
            this.onSuggestTagsParent();
        },
        onCKEditor: function () {
            Common.onLoadEditor('.CKEditor');
        },
        onSelect2TagView: function () {
            $('.kt-select2-tagsView').select2({
                placeholder: 'Chọn tagsView',
                closeOnSelect: false
            });
        },
        onSelect2TagGroup: function () {
            $('.select-tagGroup').select2({
                closeOnSelect: false,
                placeholder: 'Chọn sub Tag Group'
            });
            $('.select-tagGroup').on('change', function (e) {
                let $this = $(this);
                let _val = $this.val();
                let _valLength = _val.length;
                let $groupSubTag = $this.closest('.groupSubTag');
                let $previewGroups = $('.previewGroup');
                let parentId = $this.closest('.groupSubTag').data('id');

                $groupSubTag.find('input[type="hidden"]').remove();
                if ($this.val) {
                    for (let i = 0; i < _valLength; i++) {
                        let element = _val[i];
                        let selected;
                        try {
                            selected = JSON.parse(element);
                        } catch (error) {
                            selected = JSON.parse(decodeURIComponent(element));
                        }
                        $groupSubTag.append(`<input type="hidden" class="tagGroupIsSelected" data-sub-name="${selected.Name}" data-sub-id="${selected.Id}" />`);
                    }
                    $previewGroups.each(function () {
                        let $this = $(this);

                        if ($this.data('id') === parentId) {
                            /*html*/
                            let isFirst = true;
                            let HTML = _val
                                .map((e) => {
                                    let name = '';
                                    let className = '';
                                    if (isFirst) {
                                        className = 'active';
                                        isFirst = false;
                                    }
                                    try {
                                        name = JSON.parse(e).Name;
                                    } catch (error) {
                                        name = JSON.parse(decodeURIComponent(e)).Name;
                                    }
                                    return `
                                            <button type="button" 
                                                onclick="ProductDetail.onPreviewSelectTagGroup(this);"
                                                class="previewComponentSub ${className} mr-2 d-inline-block border mt-1">
                                                    ${name}
                                            </button> 
                                        `;
                                })
                                .join('');
                            $this.html(HTML);
                        }
                    });
                }
            });
        },
        onAutoSuggestPlaceInBaseInfo: function () {
            Common.onLoadSelect2Show('select-productPlace', $('#updateBaseInfo'), '/Place/SuggestPlaceInBaseInfo', 'Description', '', '', true);
            $('body').on('change', '.select-productPlace', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    Common_another_ajax.get(`Place/DeleteTokenSSIC`).then(() => {
                        $('.my-loading').hide();
                        let selected = JSON.parse($this.val());
                        $('[name="place"]').val(selected.Description);
                        $('[name="placeId"]').val(selected.Place_id);
                    });
                } else {
                    let $div = $this.closest('.place-container ');
                    $div.find('input.d-none').remove();
                }
            });
        },
        onPreviewSelectTagGroup: function (e) {
            let $this = $(e);
            $previewComponentSubs = $this.closest('.previewGroup').find('.previewComponentSub');

            $previewComponentSubs.removeClass('active');
            $this.addClass('active');
        },
        onSelectImg: function () {
            Common.onLoadSelect2Show('select-tags-img', $('#image'), '/Media/SuggestImages', 'Url', 'Nhập để tìm kiếm ảnh sản phẩm', '', false, 'Url');
            Common.onFillSelect2ForImage('body', '.select-tags-img', '.imgListItemContainer', true);
        },
        onSelectVideo: function () {
            function selectVideo() {
                let $parent = $('#updateImgInfo');
                let $videoLink = $parent.find('.previewAvatarVideo');
                let $input = $parent.find('.inputVideo');

                function fillUrl() {
                    $videoLink.attr('href', $input.val());
                    if ($input.val()) {
                        $videoLink.removeClass('d-none');
                    } else {
                        $videoLink.addClass('d-none');
                    }
                }
                $input.on('change', function (e) {
                    e.preventDefault();
                    fillUrl();
                    ProductDetail.inSetCoutImgsValid();
                });
                $input.on('keypress', function (e) {
                    if (e.key === 'Enter') {
                        fillUrl();
                        ProductDetail.inSetCoutImgsValid();
                        return false;
                    }
                });
            }

            function deleteListimg() {
                $(document).on('click', '.clearListImg', function (e) {
                    let $this = $(this);
                    let _value = $this.data('url');
                    let $li = $('.img-container').find('li');

                    for (let i = 0; i < $li.length; i++) {
                        let _valueLi;
                        if ($($li[i]).attr('title')) _valueLi = $($li[i]).attr('title').trim();
                        else _valueLi = $($li[i]).attr('title');

                        if (_valueLi === _value) {
                            $($li[i]).find('span').trigger('click');
                        }
                    }
                    $('.select2-dropdown').hide();
                    $('.img-container').find('.select2-search__field').blur().trigger('input');
                    $('.tooltip').remove();
                    $('.tooltip-inner').remove();
                });
            }

            function deleteListVideo() {
                $(document).on('click', '.clearListVideo', function (e) {
                    let $this = $(this);
                    let _value = $this.data('url');
                    let $tags = $('tags.videoProduct').find('tag');

                    $this.closest('.videoListItem').remove();
                    for (let i = 0; i < $tags.length; i++) {
                        if ($($tags[i]).attr('title') === _value) {
                            $($tags[i]).remove();
                        }
                    }
                    ProductDetail.loadIndex();
                    ProductDetail.inSetCoutImgsValid();
                    $('.tooltip-inner').remove();
                    $('.tooltip').remove();
                });
            }

            selectVideo();
            deleteListimg();
            deleteListVideo();
        },
        onVideoProduct: function () {
            let inputVideoProduct = $('.videoProduct')[0];
            let $table = $('.tableVideoProduct');

            new Tagify(inputVideoProduct, {
                transformTag: function (tagData, tagData2) {}
            })
                .on('add', function (e) {
                    let value = e.detail.data.value;
                    let $tableVideo = $('.list-video-preview').find('.videoListItemContainer');
                    /*html*/
                    let HTML = `
                        <div class="videoListItem row mt-2">
                            <div class="col-3 text-center">
                                <input class="name form-control" name="videos[].name"/>
                            </div>
                            <div class="col-3">
                                <input class="link form-control" name="videos[].link"/>
                            </div>
                            <div class="col-3">
                                <input class="alt form-control" name="videos[].alt"/>
                            </div>
                            <div class="col-1">
                                <input type="number" min="0" class="idx form-control" name="videos[].idx" value="0"/>
                                <input type="hidden" class="type" name="videos[].type" value="2"/>
                                <input type="hidden" class="url" name="videos[].url" value="${value}"/>
                            </div>
                            <div class="col-1 text-center">
                                <a href='${value}' target="_blank" title="${value}" data-toggle="tooltip" data-placement="top" data-original-title="${value}">Xem</a>
                            </div>
                            <div class="col-1 text-center">
                                <button type="button" class="clearListVideo bg-transparent border-0" data-url="${value}" data-toggle="tooltip" data-placement="top" data-original-title="${value}">
                                    <i class="flaticon2-cancel-music"></i>
                                </button>
                            </div>
                        </div>
                    `;
                    $tableVideo.append(HTML);
                    ProductDetail.loadIndex();
                    ProductDetail.inSetCoutImgsValid();
                })
                .on('remove', function (e) {
                    let value = e.detail.data.value;
                    let $trElement = $table.find('.videoListItem');
                    $trElement.each(function () {
                        let $this = $(this);
                        if ($this.find('a').attr('href') === value) {
                            $this.remove();
                            fadeOut(300, function () {
                                $(this).remove();
                            });
                            ProductDetail.loadIndex();
                        }
                    });
                    ProductDetail.inSetCoutImgsValid();
                });
            $('.videoProduct').on('click', function (e) {
                e.preventDefault();
            });
            $('.videoProduct').on('keypress', function (e) {
                if (e.key === 'Enter') return false;
            });
        },
        onSelectTagsHightLight: function () {
            let type = $('.select-tags-highlight').data('type');
            let vari = {
                name: 'Name',
                exNameToShow: 'ParentName',
                exName: 'ParentName'
            };
            $('.select-tags-highlight').select2({
                closeOnSelect: false
            });
            // Common.onLoadSelect2Show('select-tags-highlight', $('#select-tags-highlight'), '/Tags/SuggesTagsInUpdateTags', vari, { type: type }, '', false);
            $('body').on('change', '.select-tags-highlight', function () {
                let $this = $(this);
                let key = $this.data('key');
                let $div = $this.closest('.tags-container');
                $div.find('input.d-none').remove();
                let selected = $this.val();
                Common.onAppendTags($div, key, selected, 1);
            });
        },
        onSelectTags: function () {
            let type = $('.select-tags').data('type');
            let vari = {
                name: 'Name',
                exNameToShow: 'ParentName',
                exName: 'ParentName'
            };
            Common.onLoadSelect2Show('select-tags', $('#select-tags'), '/Tags/SuggesTagsInUpdateTags', vari, { type: type }, '', false);
            $('body').on('change', '.select-tags', function () {
                let $this = $(this);
                let key = $this.data('key');
                let $div = $this.closest('.tags-container');
                $div.find('input.d-none').remove();
                let selected = $this.val();
                Common.onAppendTags($div, key, selected, 1);
            });
        },
        onPickTagsHightLight: function (e) {
            let $this = $(e);
            let $eTagsHightLightValue = $('#tags').find('.eTagsHightLightValue');
            let value = $this.data('value');
            let isChecked = $this[0].checked;
            let $clearTagsHightLightBtn = $('#tags .clearTagsHightLightBtn');

            if (isChecked) {
                /*html*/
                let HTML = `
                <div class="hightLightTagPickedItem border d-inline-block mr-1 pl-1">
                    <span class="mr-2 cursor-default">${value.Name}</span>
                    <button type="button"
                            data-id="${value.Id}"
                            class="clearTagsHightLightBtn border-0 bg-transparent"
                            onclick="ProductDetail.onClearTagsHightLight(this);">
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
                    $eTagsHightLightValue.append(HTML);

                    resolve();
                }).then(() => {
                    ProductDetail.onResetIndexTagsHightLight();
                });
            } else {
                $clearTagsHightLightBtn.each(function () {
                    if ($(this).data('id') === value.Id) {
                        $(this).closest('.hightLightTagPickedItem').remove();
                    }
                });
            }
        },
        onHandleTagsHightLightValue: function () {
            let $dropDown = $('#tags .tagsHightLightDropDown');
            let pos_left;
            let pos_top;
            let pos_right;
            let pos_bot;

            let gettagHightLightChoiceInfo = function () {
                let rect = $dropDown[0].getBoundingClientRect();
                pos_left = rect.x;
                pos_top = rect.y;
                pos_right = rect.x + rect.width;
                pos_bot = rect.y + rect.height;
            };

            let clickETagsHightLightValue = function () {
                let $eTagsHightLightValue = $('#tags .eTagsHightLightValue');
                $eTagsHightLightValue.on('click', function (e) {
                    if ($dropDown.css('display') === 'none') {
                        $dropDown.show();
                    } else {
                        $dropDown.hide();
                    }
                });
                $(document).on('click', function (e) {
                    if (e.target !== $eTagsHightLightValue[0]) {
                        if ($dropDown.css('display') === 'block') {
                            let mPos_x = e.clientX;
                            let mPos_y = e.clientY;

                            new Promise((resolve) => {
                                gettagHightLightChoiceInfo();
                                resolve();
                            }).then(() => {
                                if (mPos_x < pos_left || mPos_x > pos_right || mPos_y < pos_top || mPos_y > pos_bot) {
                                    $dropDown.hide();
                                }
                            });
                        }
                    }
                });
            };

            let clickAddTagHightlight = function () {
                let $addBtn = $('#tags .addHightlightBtn');
                let $additionBody = $('#tags .additionBody');

                $addBtn.on('click', function (e) {
                    let $this = $(this);

                    $this.toggleClass('btn-primary');
                    $additionBody.toggle();
                });
            };

            let actionAddTagHightlight = function () {
                let $btn = $('#tags .actionAddTagHightlight');

                $btn.on('click', function (e) {
                    let $container = $('#tags .additionBody');
                    let name = $container.find('.tagName').val();
                    let textView = $container.find('.tagShowText').val();
                    let idx = $container.find('.tagIndex').val();

                    if (name.trim() === '') {
                        $container.find('.errName').css('display', 'inline');
                        $container.find('.tagName').css('border', '1px solid red');
                    }
                    if (textView.trim() === '') {
                        $container.find('.tagShowText').css('border', '1px solid red');
                        $container.find('.errorTagShowText').css('display', 'inline');
                    }

                    if (name.trim() !== '' && textView.trim() !== '') {
                        let request = {
                            name,
                            textView,
                            idx,
                            parentId: 0,
                            categoryId: '00000000-0000-0000-0000-000000000000',
                            destinationId: '00000000-0000-0000-0000-000000000000',
                            type: Object.values(TYPE).reduceRight((acc, e, i) => {
                                if (e === 'ProductHighlight') return (acc += i);
                                else return (acc += 0);
                            }, 0),
                            typeResponse: 3
                        };
                        Common_another_ajax.post('Tags/Add', request).then((data) => {
                            eval(data.CallBack);
                        });
                    }
                });
            };

            clickETagsHightLightValue();
            clickAddTagHightlight();
            actionAddTagHightlight();
        },
        onResetIndexTagsHightLight: function () {
            let $tagsAreSelected = $('#tags .hightLightTagPickedItem');
            let tagsAreSelectedLength = $tagsAreSelected.length;

            for (let i = 0; i < tagsAreSelectedLength; i++) {
                let $item = $($tagsAreSelected[i]);

                $item.find('.Id').attr('name', `TagsHighlight[${i}].Id`);
                $item.find('.ParentId').attr('name', `TagsHighlight[${i}].ParentId`);
                $item.find('.CategoryId').attr('name', `TagsHighlight[${i}].CategoryId`);
                $item.find('.DestinationId').attr('name', `TagsHighlight[${i}].DestinationId`);
                $item.find('.TextView').attr('name', `TagsHighlight[${i}].TextView`);
                $item.find('.TextLink').attr('name', `TagsHighlight[${i}].TextLink`);
                $item.find('.Name').attr('name', `TagsHighlight[${i}].Name`);
                $item.find('.Type').attr('name', `TagsHighlight[${i}].Type`);
                $item.find('.Idx').attr('name', `TagsHighlight[${i}].Idx`);
            }
        },
        onClearTagsHightLight: function (e) {
            let $this = $(e);
            let id = $this.data('id');
            let $tagIsSelected = $this.closest('.hightLightTagPickedItem');
            let $subItemsSelected = $('#tags').find('.updateSelectTagsHightLight ');
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
                ProductDetail.onResetIndexTagsHightLight();
            });
        },
        onSelectTagsView: function () {
            $('body').on('change', '.kt-select2-tagsView', function () {
                let $this = $(this);
                let key = $this.data('key');
                let $div = $this.closest('.col-12');
                $div.find('input.d-none').remove();
                let selected = $this.val();
                for (var i = 0; i < selected.length; i++) {
                    let obj = JSON.parse(selected[i]);
                    $div.append(`<input type="text" class="d-none" name="${key}[${i}].Id" value="${obj.Id}" />`);
                    $div.append(`<input type="text" class="d-none" name="${key}[${i}].Icon" value="${obj.Icon}" />`);
                    $div.append(`<input type="text" class="d-none" name="${key}[${i}].Detail" value="${obj.Detail}" />`);
                    $div.append(`<input type="text" class="d-none" name="${key}[${i}].Type" value="${obj.Type}" />`);
                }
            });
        },
        onFillDataSelectTag: function () {
            {
                $('.tags-container').each(function () {
                    let $this = $(this);
                    let key = $this.find('select').data('key');
                    let $selected = $this.find('option').filter('[selected]');
                    Common.onAppendTags($this, key, $selected);
                });
            }
            {
                let $tagsViewElement = $('.kt-select2-tagsView');
                let key = $tagsViewElement.data('key');
                let $selected = $tagsViewElement.find('option').filter('[selected]');
                for (let i = 0; i < $selected.length; i++) {
                    let obj = JSON.parse($($selected[i]).val());
                    $tagsViewElement.closest('.col-12').append(`<input type="text" class="d-none" name="${key}[${i}].Id" value="${obj.Id}" />`);
                    $tagsViewElement.closest('.col-12').append(`<input type="text" class="d-none" name="${key}[${i}].Icon" value="${obj.Icon}" />`);
                    $tagsViewElement.closest('.col-12').append(`<input type="text" class="d-none" name="${key}[${i}].Detail" value="${obj.Detail}" />`);
                    $tagsViewElement.closest('.col-12').append(`<input type="text" class="d-none" name="${key}[${i}].Type" value="${obj.Type}" />`);
                }
            }
            {
                let $selectImg = $('.select-tags-img');
                let key = $selectImg.data('key');
                let $selected = $selectImg.find('option').filter('[selected]');
                let HTML__tableContent = '';
                for (let i = 0; i < $selected.length; i++) {
                    let obj = JSON.parse($($selected[i]).val());
                    let link = `${domainCDN_MediaShowContent}${obj.Url}`;
                    if (obj.Type !== 2) {
                        // <input class="form-control" name="${key}[${i}].Link" value="${obj.Link}" />
                        /*html*/
                        HTML__tableContent += `
                        <div class="mt-2 row align-items-center imgListItem">
                            <div class="col-1 text-center" data-toggle="tooltip" data-placement="top" data-original-title="${
                                obj.Url
                            }"><a href="${link}" target="_blank"><img class="imgSmall3" src="${link}" alt="${obj.fileName || ''}" title="${link}"/></a></div>
                            <div class="col-3"><input class="form-control name" name="${key}[${i}].name" value="${obj.Name}" /></div>
                            <div class="col-3">
                                ${Common.onInputSelectLinkInImage(obj.Link, obj.Link, `${key}[${i}].link`)}
                            </div>
                            <div class="col-1 text-center">
                                <select class="cursor-pointer form-control linkOption" name="${key}[${i}].linkOption">
                                    <option value="0" ${obj.LinkOption === 0 ? 'selected' : ''}>Không có</option>
                                    <option value="1" ${obj.LinkOption === 1 ? 'selected' : ''}>Nofollow</option>
                                </select>
                            </div>
                            <div class="col-2"><input class="form-control alt" name="${key}[${i}].alt" value="${obj.Alt}" /></div>
                            <div class="col-1"><input type="number" min="0" class="form-control idx" name="${key}[${i}].idx" value="${obj.Idx || ''}" /></div>
                            <div class="col-1 text-center">
                                <button type="button" class="clearListImg bg-transparent border-0" data-url="${obj.Url}" data-toggle="tooltip" data-placement="top" data-original-title="${obj.Url}">
                                    <i class="flaticon2-cancel-music"></i>
                                </button>
                                <input type="hidden" class="Url" name="${key}[${i}].url" value="${obj.Url}"/>
                                <input type="hidden" class="type" name="${key}[${i}].type" value="${obj.Type || 1}"/>
                            </div>
                        </div>
                        `;
                        /*html*/
                    }
                }
                /*html*/
                let HTML = `
                    <div class="border">
                        <div class="row mx-0 align-items-center headerList py-2">
                            <div class="col-1 font-weight-bold text-center pl-0">Ảnh/Xem</div>
                            <div class="col-3 font-weight-bold text-center">Tên mô tả</div>
                            <div class="col-3 font-weight-bold text-center">Link</div>
                            <div class="col-1 font-weight-bold text-center">Link Option</div>
                            <div class="col-2 font-weight-bold text-center">Thẻ alt</div>
                            <div class="col-1 font-weight-bold text-center">Idx</div>
                            <div class="col-1 font-weight-bold text-center">Clear</div>
                        </div>
                        <div class="imgListItemContainer">
                            ${HTML__tableContent}
                        </div>
                    </div>
                    `;
                $('.list-img-preview').html(HTML);
            }
        },
        onValidation() {
            this.inSetCoutImgsValid();

            $.validator.addMethod(
                'numberGreaterThan0',
                function (value) {
                    return /^[\d]+$/g.test(value);
                },
                'Chỉ chứa số'
            );
            $.validator.addMethod(
                'not0',
                function (value) {
                    return !/0/g.test(value);
                },
                'Code chứa ít nhất một ký tự chữ'
            );
            $.validator.addMethod(
                'greaterThan5',
                function (value) {
                    return Number(value) >= 5;
                },
                'Tối thiểu là 5'
            );
            let updateBaseInfo = function () {
                $('#updateBaseInfo').validate({
                    rules: {
                        name: {
                            required: true
                        }
                    },
                    messages: {
                        name: {
                            required: 'Không được bỏ trống tiêu đề !'
                        }
                    }
                });
            };
            let updateImgInfo = function () {
                $('#updateImgInfo').validate({
                    ignore: '',
                    rules: {
                        total__imagesArePicked: {
                            greaterThan5: true
                        },
                        lookImg: {
                            required: true
                        }
                    },
                    messages: {
                        total__imagesArePicked: {
                            greaterThan5: 'Tổng số url avata video, ảnh sản phẩm và video sản phẩm phải lớn hơn 5!'
                        },
                        lookImg: {
                            required: 'Chưa chọn avatar!'
                        }
                    }
                });
            };
            let updateSeoInfo = function () {
                $('#updateSeoInfo').validate({
                    rules: {
                        metaTitles: {
                            required: true
                        }
                    },
                    messages: {
                        metaTitles: {
                            required: 'Không được bỏ trống meta tiêu đề!'
                        }
                    }
                });
            };
            let updateConfigInfo = function () {
                $('#updateConfigInfo').validate({
                    rules: {
                        ticketLimit: {
                            numberGreaterThan0: true
                        }
                    },
                    messages: {
                        ticketLimit: {
                            numberGreaterThan0: 'Giới hạn vé Chỉ số'
                        }
                    }
                });
            };
            $(document).ready(() => {
                updateBaseInfo();
                updateSeoInfo();
                updateConfigInfo();
                updateImgInfo();
            });
        },
        inSetCoutImgsValid: function () {
            let numImgAvatar = 0;
            let numImgProducts = 0;
            let numVideoProducts = 0;

            if ($('.inputVideo').val()) numImgAvatar = 1;
            numImgProducts = $('#updateImgInfo').find('.list-img-preview').find('.imgListItem').length;
            numVideoProducts = $('#updateImgInfo').find('.tableVideoProduct ').find('.videoListItem').length;
            $('.total__imagesArePicked').val(numImgAvatar + numImgProducts + numVideoProducts);
            $('[for="total__imagesArePicked"]').html('');
        },
        onSuggestTagsParent: function () {
            let CurentParentId;
            let CurentParentName;
            let $container = $('#updateTagsGroup');
            let type = $('#updateTagsGroup').find('.select-productTagsGroup').data('type');

            let SuggesParentTags = function () {
                let vari = {
                    name: 'Name',
                    exNameToShow: 'ParentName',
                    exName: 'ParentName'
                };
                Common.onLoadSelect2Show('select-productTagsGroup', $('#updateTagsGroup'), '/Tags/SuggesTagsInUpdateTags', vari, { type: type }, '', true);
                $('body').on('change', '.select-productTagsGroup', function () {
                    let $this = $(this);
                    if ($this.val()) {
                        let selected = JSON.parse($this.val());
                        CurentParentId = selected.Id;
                        CurentParentName = selected.Name;
                    } else {
                        CurentParentId = '';
                        CurentParentName = '';
                    }
                });
            };

            let actionClose = function () {
                $container.on('click', '.closeSubTags-btn', function (e) {
                    let $this = $(this);
                    let id = $this.closest('.groupSubTag').data('id');
                    let $previewGroup = $('.previewGroup');

                    $this.closest('[data-id]').remove();
                    $previewGroup.each(function () {
                        if ($(this).data('id') === id) {
                            $(this).closest('.mt-3').remove();
                        }
                    });
                });
            };

            let actionAddPlaceSubTags = function () {
                $container.find('.addParentTags').on('click', function (e) {
                    function isCheckAdded() {
                        let isAdded = false;
                        $container.find('div[data-id]').each(function () {
                            let $this = $(this);
                            if ($this.data('id') === CurentParentId) isAdded = true;
                        });
                        return isAdded;
                    }
                    if (CurentParentId && CurentParentName) {
                        if (!isCheckAdded()) {
                            Common_another_ajax.get(`Tags/SuggesTagsInUpdateTags?parentId=${encodeURI(CurentParentId)}&type=${type}`).then((data) => {
                                $('.my-loading').hide();
                                if (data.Success) {
                                    let HTML = '';
                                    // if(data.Data.length <= 16) {}
                                    /*html*/
                                    HTML = `
                                            <div class="groupSubTag border p-4 p-3 mt-4 ui-sortable-handle" data-id="${CurentParentId}" data-name="${CurentParentName}">
                                                <div class="position-relative">
                                                    <button type="button" class="closeSubTags-btn btn position-absolute right-0"><i class="flaticon2-cancel-music"></i></button>
                                                    <h2 class="mb-3">${CurentParentName}</h2>
                                                    <div class="mt-3">
                                                    ${
                                                        data.Data.length > 0
                                                            ? `
                                                                <select class="select-tagGroup form-control cursor-pointer d-inline-block w-300px" multiple="multiple">
                                                                    ${data.Data.map(
                                                                        (e) => `
                                                                            <option value="${encodeURIComponent(JSON.stringify(e))}">
                                                                                ${e.Name}
                                                                            </option>
                                                                        `
                                                                    )}
                                                                </select>
                                                            `
                                                            : ``
                                                    }
                                                    </div>
                                                </div>
                                            </div>
                                        `;

                                    let preview_HTML = '';
                                    /*html*/
                                    preview_HTML = `
                                        <div class="mt-3">
                                            <div>
                                                <b class="previewNameSub">${CurentParentName}</b>
                                            </div>
                                            <div class="previewGroup" data-id="${CurentParentId}">
                                            </div>
                                        </div>
                                    `;

                                    $('.previewTag').prepend(preview_HTML);

                                    new Promise((resolve) => {
                                        $('.tagsGroup').prepend(HTML);
                                        resolve();
                                    }).then(() => {
                                        ProductDetail.onSelect2TagGroup();
                                    });
                                }
                            });
                        } else {
                            Swal.fire('Tag này đã được thêm trong danh sách rồi!');
                        }
                    } else {
                        Swal.fire('Chưa chọn tag!');
                    }
                });
            };

            let submit = function () {
                $container.submit(function (e) {
                    e.preventDefault();
                    let data = {
                        id: productId,
                        tagsGroup: new Array()
                    };
                    $container.find('.groupSubTag').each(function () {
                        let subOptions = new Array();
                        $(this)
                            .find('.tagGroupIsSelected')
                            .each(function () {
                                let $this = $(this);
                                subOptions.push({
                                    id: $this.data('sub-id'),
                                    name: $this.data('sub-name')
                                });
                            });
                        data.tagsGroup.push({
                            id: $(this).data('id'),
                            name: $(this).data('name'),
                            subOptions: subOptions
                        });
                    });
                    Common_another_ajax.post('Product/ProductUpdateTagsGroup', data).then((data) => {
                        eval(data.CallBack);
                    });
                });
            };

            SuggesParentTags();
            actionClose();
            actionAddPlaceSubTags();
            submit();
        },
        onUpdateQueryPrice: function () {
            $('.updateQueryPrice').on('click', function (e) {
                Common_another_ajax.get(`Product/ProductUpdateQueryPrice/${productId}`).then((data) => {
                    eval(data.CallBack);
                });
            });
        },
        onRendAfterUpdateQueryPrice: function () {
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: 'Cập nhật giá hiển thị thành công!',
                showConfirmButton: false,
                timer: 1500
            });
        },
        onDeleteTagGroupPlaceElement: function (e) {
            let $this = $(e);
            let $tagGoupPlaceItem = $this.closest('.tagGoupPlaceItem');
            $tagGoupPlaceItem.remove();
        }
    };
})();

var TagsGetByPage = (function () {
    return {
        onRendAfterUpdate: function (mess, dataJSON) {
            let data = JSON.parse(dataJSON);

            if (data.TypeResponse === 2) {
                let $tagSelected = $('#groupServiceUpdateTagsInfo .tagSelected');
                let $lastSubItem = $('#groupServiceUpdateTagsInfo .itemSubItems ');

                Common.onSuccess('Thêm mới sub Tag thành công! Sub tag cũng đã được tự động thêm vào "Tags đã chọn!"', true, false);

                {
                    /*html*/
                    let HTML = `
                        <div class="tagSelected--item border mt-1 cursor-pointer"
                             onclick="GroupServiceDetail.onFindCurrentSubItemTag(this);">
                                ${data.Name}
                                <button type="button" 
                                    class="clearGroupServiceTagBtn border-0 bg-transparent" 
                                    onclick="GroupServiceDetail.onClearGroupServiceTags(this);"
                                    data-id="${data.Id}">
                                    <i class="font-size-0r8 ml-1 flaticon2-cancel-music"></i>
                                </button>
                                <input type="hidden" class="Id" value="${data.Id}" name="Tags[].Id" />
                                <input type="hidden" class="ParentId" value="${data.ParentId}" name="Tags[].ParentId" />
                                <input type="hidden" class="CategoryId" value="${data.CategoryId}" name="Tags[].CategoryId" />
                                <input type="hidden" class="DestinationId" value="${data.DestinationId}" name="Tags[].DestinationId" />
                                <input type="hidden" class="TextView" value="${data.TextView}" name="Tags[].TextView" />
                                <input type="hidden" class="TextLink" value="${data.TextLink}" name="Tags[].TextLink" />
                                <input type="hidden" class="Name" value="${data.Name}" name="Tags[].Name" />
                                <input type="hidden" class="Type" value="${data.Type}" name="Tags[].Type" />
                                <input type="hidden" class="Idx" value="${data.Idx}" name="Tags[].Idx" />
                        </div>
                    `;
                    new Promise((resolve) => {
                        $tagSelected.append(HTML);
                        resolve();
                    }).then(() => {
                        GroupServiceDetail.onResetIndexGroupServiceTags();
                    });
                }

                {
                    $lastSubItem.each(function () {
                        let $this = $(this);

                        if ($this.css('display') === 'block') {
                            /*html*/
                            let HTML = `
                            <div class="lastSubItem pl-1" data-id="${data.Id}">
                                <input id="theGroupServiceTag-@subItem.Id"
                                        class="updateSelectedCheckbox cursor-pointer"
                                        data-value="${Convert.onHtmlEncode(JSON.stringify(data))}"
                                        type="checkbox"
                                        checked 
                                        onchange="GroupServiceDetail.onUpdateTagSelected(this);" />
                                <label for="theGroupServiceTag-@subItem.Id" class="p-0 m-0 pl-1 cursor-pointer font-weight-light">
                                    ${data.Name}
                                </label>
                            </div>
                        `;
                            new Promise((resolve) => {
                                $this.prepend(HTML);
                                resolve();
                            }).then(() => {
                                $this.find('.tagSelected--noneItem').remove();
                            });
                        }
                    });
                }
            } else if (data.TypeResponse === 3) {
                let $tagSelected = $('#tags .eTagsHightLightValue');
                let $lastSubItem = $('#tags .tagHightLightChoice');

                Common.onSuccess('Thêm mới Tag highlight thành công! Tag highlight cũng đã được tự động thêm vào "TagsHighLight đã chọn!"', true, false);

                {
                    /*html*/
                    let HTML = `
                    <div class="hightLightTagPickedItem border d-inline-block mr-1 pl-1">
                        <span class="mr-2 cursor-default">${data.Name}</span>
                        <button type="button"
                                data-id="${data.Id}"
                                data-value="${Convert.onHtmlEncode(dataJSON)}"
                                class="clearTagsHightLightBtn border-0 bg-transparent"
                                onclick="ProductDetail.onClearTagsHightLight(this);">
                            <i class="font-size-0r8 ml-1 flaticon2-cancel-music"></i>
                        </button>
                        <input type="hidden" class="Id" value="${data.Id}" name="Tags[].Id" />
                        <input type="hidden" class="ParentId" value="${data.ParentId}" name="Tags[].ParentId" />
                        <input type="hidden" class="CategoryId" value="${data.CategoryId}" name="Tags[].CategoryId" />
                        <input type="hidden" class="DestinationId" value="${data.DestinationId}" name="Tags[].DestinationId" />
                        <input type="hidden" class="TextView" value="${data.TextView}" name="Tags[].TextView" />
                        <input type="hidden" class="TextLink" value="${data.TextLink}" name="Tags[].TextLink" />
                        <input type="hidden" class="Name" value="${data.Name}" name="Tags[].Name" />
                        <input type="hidden" class="Type" value="${data.Type}" name="Tags[].Type" />
                        <input type="hidden" class="Idx" value="${data.Idx}" name="Tags[].Idx" />
                    </div>
                    `;
                    new Promise((resolve) => {
                        $tagSelected.append(HTML);
                        resolve();
                    }).then(() => {
                        ProductDetail.onResetIndexTagsHightLight();
                    });
                }

                {
                    /*html*/
                    let HTML = `
                        <div class="tagsHightPickItem">
                            <input data-value="${Convert.onHtmlEncode(dataJSON)}"
                                    checked
                                    id="tagsHightPickItem-${data.Id}"
                                    class="updateSelectTagsHightLight cursor-pointer mr-2"
                                    type="checkbox"
                                    onchange="ProductDetail.onPickTagsHightLight(this);" />
                            <label for="tagsHightPickItem-${data.Id}" class="p-0 m-0 text-truncate cursor-pointer font-weight-light">
                                    ${data.Name}
                            </label>
                        </div>
                        `;
                    $lastSubItem.append(HTML);
                    resolve();
                }
            }
        }
    };
})();
