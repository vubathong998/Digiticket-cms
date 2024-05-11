sort = 'desc';
field = 'displayName';

const baseUrlUsers = `${domain}/Users/UsersGetByPage`;
var dtTableUsers;
var $containerUsers = '.table-Users';

var users = (function () {
    return {
        init: function () {
            this.initTable(1, 10, 'displayName', 'desc');
            Common.onClearFilter($containerUsers, baseUrlUsers, dtTableUsers);
            Common.onSort($containerUsers, baseUrlUsers, dtTableUsers);
            Common.onSelectPageSize($containerUsers, baseUrlUsers, dtTableUsers);
            Common.onGotoPage($containerUsers, baseUrlUsers, dtTableUsers);
            Common.onSearchByKey($containerUsers, baseUrlUsers, dtTableUsers);
        },
        initAfterAddModalRend: function () {
            this.onSelectTags();
        },
        initAfterUpdateModalIsOpened: function () {
            this.onJSTree();
        },
        initAfterGetUserPermissionModalIsOpened: function() {
            this.onJSTreeUserPermission();
        },
        initTable: function (page, perpage, field, sort) {
            let table = $('#html_table_Users');
            let dataTable__columns = new Object();
            // if()
            $.fn.dataTableExt.sErrMode = 'throw';
            dtTableUsers = table
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
                        url: `${baseUrlUsers}?Page=${page}&Perpage=${perpage}&Field=${field}&Sort=${sort}`,
                        type: 'get'
                    },
                    rowId: 'Id',
                    columns: [
                        {
                            data: 'DisplayName'
                        },
                        { data: 'Email' },
                        { data: 'Address' },
                        { data: 'PhoneNumber' },
                        { data: '' },
                        { data: '' }
                    ],
                    columnDefs: [
                        {
                            targets: -2,
                            width: '60px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="loadModalContent('${domain}/Users/UsersEditModal?id=${full.Id}&Name=${encodeURI(
                                    full.DisplayName
                                )}','modal-xg','users.initAfterUpdateModalIsOpened();')"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg"
                                                 >
                                                     <i class="text-success flaticon2-edit"></i>
                                         </button>`;
                            }
                        },
                        {
                            targets: -1,
                            width: '60px',
                            className: 'text-center',
                            render: function (data, type, full, meta) {
                                /*html*/
                                return `<button onclick="loadModalContent('${domain}/Users/GetUserPermissionModal?Id=${full.Id}&Name=${encodeURI(full.DisplayName)}','modal-lg','users.initAfterGetUserPermissionModalIsOpened();')"
                                                 class="cursor-pointer btn btn-sm btn-clean btn-icon btn-icon-lg"
                                                 >
                                                     <i class="text-primary flaticon-eye"></i>
                                         </button>`;
                            }
                        }
                    ],
                    initComplete: function (settings, json) {
                        CreateMeta(json);
                        Common.onLoadOption(meta, $containerUsers);
                        $('.my-loading').hide();
                    }
                });
        },
        onJSTree: function () {
            $('#UsersUpdateModalTree')
                .on('changed.jstree', function (e, data) {
                    var i,
                        j,
                        r = [];
                    for (i = 0, j = data.selected.length; i < j; i++) {
                        r.push(data.instance.get_node(data.selected[i]).id);
                    }
                    $('#tree-values').val(r.join('~'));
                })
                .jstree({
                    plugins: ['types', 'checkbox'],
                    checkbox: { three_state: true },
                    core: {
                        themes: {
                            responsive: false
                        }
                    },
                    types: {
                        default: {
                            icon: 'fa fa-folder kt-font-warning'
                        },
                        file: {
                            icon: 'fa fa-file  kt-font-warning'
                        }
                    }
                });
        },
        onJSTreeUserPermission: function() {
            $('#UsersPermissionModalTree')
            .on('changed.jstree', function (e, data) {
                var i,
                    j,
                    r = [];
                for (i = 0, j = data.selected.length; i < j; i++) {
                    r.push(data.instance.get_node(data.selected[i]).id);
                }
                $('#tree-values').val(r.join('~'));
            })
            .jstree({
                plugins: ['types', 'checkbox'],
                checkbox: { three_state: true },
                core: {
                    themes: {
                        responsive: false
                    }
                },
                types: {
                    default: {
                        icon: 'fa fa-folder kt-font-warning'
                    },
                    file: {
                        icon: 'fa fa-file  kt-font-warning'
                    }
                }
            });
        },
        onRendAfterUpdate() {
            Common.onSuccess();
        },
        onSelectTags: function () {
            let type = $('#UsersAdd .select-usersAdd').data('type');
            Common.onLoadSelect2Show('select-usersAdd', $('#UsersAdd'), '/Users/UsersSuggestAdd', 'DisplayName', { type: type }, '', false);
            $('body').on('change', '.select-usersAdd', function () {
                let $this = $(this);
                let $div = $this.closest('.usersAddContainer');
                $div.find('input.d-none').remove();
                let selected = $this.val();
                for (var i = 0; i < selected.length; i++) {
                    let obj = JSON.parse(selected[i]);
                    /*html*/
                    let HTML = `
                        <input class="d-none" value="${obj.Id}" name="Ids[${i}]" />
                    `;
                    $('.usersAddContainer').append(HTML);
                }
            });
        },
        onRendAfterAdd() {
            Common.onSuccess();
        }
    };
})();
