const baseUrlCategoryTag = `${domain}/CategoryTag/CategoryTagGetByPage`;
var dtTableCategoryTag;
const $containerCategoryTags = '.table-categoryTags';

var categoryTag = (function () {
    return {
        init: function (nameIdx) {
            this.initTable(1, 10, 'CreatedDate', 'desc', nameIdx);
            Common.onClearFilter($containerCategoryTags, baseUrlCategoryTag, dtTableCategoryTag);
            Common.onSort($containerCategoryTags, baseUrlCategoryTag, dtTableCategoryTag);
            Common.onSelectPageSize($containerCategoryTags, baseUrlCategoryTag, dtTableCategoryTag);
            Common.onGotoPage($containerCategoryTags, baseUrlCategoryTag, dtTableCategoryTag);
            Common.onSearchByKey($containerCategoryTags, baseUrlCategoryTag, dtTableCategoryTag);
            this.onUpdateIdx();
        },
        initTable: function (page, perpage, field = '', sort = '', nameIdx = 'Idx') {
            let table = $('#html_table_categoryTags');
            $.fn.dataTableExt.sErrMode = 'throw';
            let isHome__request = '';
            let isHot__request = '';
            if (isHome) isHome__request = `&isHome=true`;
            if (isHot) isHot__request = `&isHot=true`;
            dtTableCategoryTag = table
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
                    scrollY: 'calc(100vh - 38rem)',
                    scrollCollapse: true,
                    ajax: {
                        url: `${baseUrlCategoryTag}?page=${page}&perpage=${perpage}&field=${field}&sort=${sort}&parentId=${parentId}${isHome__request || isHot__request}`,

                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        { data: 'Name' },
                        { data: 'ParentName' },
                        { data: 'TextView' },
                        { data: 'TextLink' },
                        {
                            data: 'IsView',
                            visible: !isHome && !isHot,
                            className: 'text-center',
                            render: (data, type, full, meta) => {
                                /*html*/
                                return `<div class="text-center">
                                            <input ${data ? 'checked' : ''} 
                                                onchange="categoryTag.onUpdateIsView(this)" 
                                                data-name="${full.Name}"
                                                data-id="${full.Id}"
                                                data-idx="${full.Idx}"
                                                class="checkBoxIsView cursor-pointer" 
                                                type="checkbox"/>
                                        </div>`;
                            }
                        },
                        {
                            data: 'IsHome',
                            className: 'text-center',
                            visible: !!isHome,
                            render: (data, type, full, meta) => {
                                /*html*/
                                return `<div class="text-center">
                                            <input checked 
                                                onchange="categoryTag.onUnHome(this);" 
                                                class="cursor-pointer" 
                                                type="checkbox" 
                                                data-id="${full.Id}"/>
                                        </div>`;
                            }
                        },
                        {
                            data: 'IsHot',
                            className: 'text-center',
                            visible: !!isHot,
                            render: (data, type, full, meta) => {
                                /*html*/
                                return `<div class="text-center">
                                            <input 
                                                onchange="categoryTag.onUnHot(this);" 
                                                ${data ? 'checked' : ''} 
                                                class="cursor-pointer" 
                                                type="checkbox" 
                                                data-id="${full.Id}"/>
                                        </div>`;
                            }
                        },
                        {
                            title: nameIdx,
                            data: nameIdx,
                            width: '30px',
                            className: 'text-center',
                            render: (data, type, full, meta) => {
                                if (permission.edit) {
                                    /*html*/
                                    return `
                                    <div>
                                        <input class="inputTextIdx inputInTable" value="${data}" data-type="${nameIdx}" data-id="${full.Id}" data-name="${full.Name}" />
                                    </div>
                                    `;
                                } else {
                                    return data;
                                }
                            }
                        },
                        { data: '' },
                        { data: '' }
                        // { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -2,
                            width: '70px',
                            className: 'text-center',
                            visible: permission.add && Number($('#round').val()) < 3 && !isHome && !isHot,
                            render: function (data, type, full, meta) {
                                return `<button onclick="loadModalContent('${domain}/CategoryTag/CategoryTagAddOrUpdateModal?parentId=${full.TagId}&parentName=${encodeURIComponent(full.Name)}&round=${
                                    Number($('#round').val()) + 1
                                }','modal-lg', 'categoryTag.onRendAfterAddOrUpdateShown(${`\`${full.TagId}\``});')"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                                                 >
                                                    <i class="text-primary flaticon2-add-1"></i>
                                                 </button>`;
                            }
                        },
                        {
                            targets: -1,
                            width: '60px',
                            className: 'text-center',
                            visible: Number($('#round').val()) < 3,
                            render: function (data, type, full, meta) {
                                let parentName = full.Name ? `&parentName=${encodeURIComponent(full.Name)}` : '';
                                /*html*/
                                return `<a href="${domain}/CategoryTag/CategoryTag?parentId=${encodeURI(full.TagId)}&round=${$('#round').val()}${parentName}"
                                class="addTags-btn cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" target="_blank"
                                >
                                    <i class="text-primary flaticon2-fast-next"></i>
                                </a>`;
                            }
                        }
                        // {
                        //     targets: -1,
                        //     width: '60px',
                        //     className: 'text-center',
                        //     visible: permission.add,
                        //     render: function (data, type, full, meta) {
                        //         /*html*/
                        //         return `<button onclick="loadModalContent('${domain}/CategoryTag/CategoryTagAddOrUpdateModal?id=${full.Id}','modal-lg')"
                        //         class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg" data-callback=""
                        //         >
                        //             <i class="text-success flaticon2-edit font-size-1r8"></i>
                        //         </button>`;
                        //     }
                        // }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerCategoryTags);
                        $('.my-loading').hide();
                    }
                });
        },
        onRendAfterAddOrUpdateShown: function (getParentId) {
            let parentId = $('#CategoryTagAddOrUpdate').find('.select-categoryTag').data('parentid');
            let vari = {
                name: 'Name',
                exNameToShow: 'ParentName',
                exName: 'ParentName'
            };
            let dataAdd = {
                type: 1,
                parentId: getParentId ? getParentId : parentId
            };
            Common.onLoadSelect2Show('select-categoryTag', $('#CategoryTagAddOrUpdate'), '/Tags/SuggesTagsInUpdateTags', vari, dataAdd, '', true);
            $('body').on('change', '.select-categoryTag', function () {
                let $this = $(this);
                let $parent = $('#CategoryTagAddOrUpdate');
                let data__obj = JSON.parse($this.val());

                $parent.find('.ParentId').val(data__obj.ParentId);
                $parent.find('.TagId').val(data__obj.Id);
            });
        },
        onRendAfterUpdated: function (mess) {
            Common.onReloadDataTable(dtTableCategoryTag, $containerCategoryTags, mess);
        },
        onUnHome: function (e) {
            let $this = $(e);

            $this.prop('checked', true);
            Common_another.onUserSwalFire__ques('', 'Bỏ is home').then((result) => {
                if (result.value) {
                    Common_another_ajax.get(`CategoryTag/CategoryTagUpdateHome?id=${$this.data('id')}&status=false`);
                }
            });
        },
        onUnHot: function (e) {
            let $this = $(e);
            let status = e.checked;

            let mess = '';
            if (status) mess = 'Thiết lập chuyên mục is hot?';
            else mess = 'Bỏ is hot?';
            Common_another.onUserSwalFire__ques('', mess).then((result) => {
                if (result.value) {
                    Common_another_ajax.get(`CategoryTag/CategoryTagUpdateHot?id=${$this.data('id')}&status=${status}`).then((data) => {
                        if (!data.Success) $this.prop('checked', !status);
                    });
                } else {
                    $this.prop('checked', !status);
                }
            });
        },
        onUpdateIsView: function (e) {
            let $this = $(e);
            let _name = $this.data('name');
            let _id = $this.data('id');
            let _idx = $this.data('idx');
            let isView = e.checked;

            Common_another.onUserSwalFire__ques('', `${isView ? 'Bật' : 'Bỏ'} IsView '${_name}'?`).then((result) => {
                if (result.value) {
                    Common_another_ajax.get(`CategoryTag/CategoryTagEdit?id=${_id}&idx=${_idx}&IsView=${isView}`).then((data) => {
                        eval(data.CallBack);
                    });
                } else {
                    $this.prop('checked', !isView);
                }
            });
        },
        onRendAfterModalIsShown: function (type) {
            let select2 = function () {
                let vari = {
                    name: 'Name',
                    exNameToShow: 'ParentName',
                    exName: 'ParentName'
                };
                let dataAdd = {
                    type: 1,
                    [type]: false
                };
                Common.onLoadSelect2Show('select-categoryTag', $(`#CategoryTag${type}Add`), '/CategoryTag/CategoryTagGetByPage', vari, dataAdd, '', true);
                $('body').on('change', '.select-categoryTag', function () {
                    if ($(this).val()) {
                        let value = JSON.parse($(this).val()).Id;
                        $(`#CategoryTag${type}Add`).find('.id').val(value);
                        $('.completeAdd').attr('type', 'submit');
                        $('.completeAdd').removeClass('cursor-not-allowed').removeClass('opacity-50p');
                    } else {
                        $('.completeAdd').attr('type', 'button');
                        $('.completeAdd').addClass('cursor-not-allowed').addClass('opacity-50p');
                    }
                });
            };

            select2();
        },
        onUpdateIdx: function () {
            $('body').on('focusin', `.inputTextIdx`, function (e) {
                let $this = $(this);
                oldValue = $this.val();
                firstValue = $this.val();
            });
            $('body').on('change', `.inputTextIdx`, function (e) {
                e.preventDefault();
                let $this = $(this);

                if ($this.val() === '') {
                    Swal.fire('Bạn chưa nhập idx!');
                    $this.val(oldValue);
                } else if (Number($this.val()) < 0 || isNaN($this.val())) {
                    Swal.fire('Idx không chính xác!');
                    $this.val(oldValue);
                } else {
                    let _name = $this.data('name');
                    let type = $this.data('type');
                    Common_another.onUserSwalFire__ques('', `Update ${type} '${_name}' ?`).then((result) => {
                        if (result.value) {
                            let _id = $(this).data('id');
                            let _idx = $this.val();

                            if (type === 'Idx') {
                                let isView = $this.closest('tr').find('.checkBoxIsView')[0].checked;
                                Common_another_ajax.get(`CategoryTag/CategoryTagEdit?id=${_id}&idx=${_idx}&IsView=${isView}`).then((data) => {
                                    eval(data.CallBack);
                                });
                            } else if (type === 'IdxHome') {
                                Common_another_ajax.get(`CategoryTag/CategoryTagUpdateHome?id=${_id}&idxHome=${_idx}&IsHome=true`).then((data) => {
                                    eval(data.CallBack);
                                });
                            } else if (type === 'IdxHot') {
                                Common_another_ajax.get(`CategoryTag/CategoryTagUpdateHot?id=${_id}&idxHot=${_idx}&IsHot=true`).then((data) => {
                                    eval(data.CallBack);
                                });
                            }
                        } else {
                            $this.val(firstValue);
                        }
                    });
                }
            });
        }
    };
})();
