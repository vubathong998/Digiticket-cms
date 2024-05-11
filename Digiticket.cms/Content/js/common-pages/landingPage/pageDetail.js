var _idPage = $('#idPage').val();
var isClickPagecomponent = false;
var isLoadTable = false;
var pageDetailTable;

var pageDetail = (function () {
    return {
        init: function () {
            this.onInitTable();
            this.onInitpageTagify();
            this.onDataTable();
            this.onUpdateIdx();
            this.onInitSelect2URLTarget();
            pageCommon.select2();
        },
        onDataTable: function () {
            $('[href="#pageComponent"]').on('click', function (e) {
                if (!isClickPagecomponent) {
                    pageDetailTable = $('.pageTemplateTable').DataTable({ searching: false, paging: false, info: false, ordering: false });
                    isClickPagecomponent = true;
                    isLoadTable = true;
                }
                $('.addPageComponentPlace').removeClass('display-none');
            });
            $('[href="#infor"]').on('click', function (e) {
                $('.addPageComponentPlace').addClass('display-none');
            });
            $('[href="#updateURLTarget"]').on('click', function (e) {
                $('.addPageComponentPlace').addClass('display-none');
            });
        },
        onInitTable: function () {
            Common_another_ajax.get(`LandingPage/PageComponentGetByPageId/${_idPage}`)
                .then((data) => {
                    $('.my-loading').hide();
                    $('.tablePageComponent').html(data.Data);
                })
                .then(() => {
                    $('.pageTemplateTable')
                        .find('tbody')
                        .sortable({
                            placeholder: 'ui-state-highlight',
                            // change: function (event, ui) {
                            //     var start_pos = ui.item.data('start_pos');
                            //     var index = ui.placeholder.index();
                            // },
                            update: function (event, ui) {
                                $('.updateIdx').removeClass('disabled').removeClass('cursor-not-allowed');
                                let $container = $(event.target);
                                for (let index = 0; index < $container.find('tr').length; index++) {
                                    const element = $container.find('tr')[index];
                                    $($(element).find('td')[4]).html(index + 1);
                                }
                            }
                        });
                });
        },
        onLoadAfterLoadAddPageComponentModal: function () {
            this.onInitSelect2();
            this.onInitTagify();
            pageComponentItems.onCKEditor();
            this.onCustomeSelect2();
            pageDetail.onSelect2();
        },
        onSelect2: function () {
            // $('.kt_select2_Category').select2({
            //     placeholder: 'Tìm danh mục'
            // });
            $('.kt-select2-Destination').select2({
                placeholder: 'Tìm điểm đến'
            });
        },
        onInitSelect2: function () {
            $('.kt-select2-TemplateComponentId').select2({
                placeholder: 'Tìm templateComponentId'
            });
            $('.kt-select2-TemplateComponentId').on('change', function (e) {
                let $this = $(this);
                let value = $this.val();
                try {
                    let selected = JSON.parse(value);
                    if (value) {
                        $('.TemplateComponentId').val(selected.id);
                        if (selected.viewImage) {
                            $('.parttenImgAdd').show();
                            $('.parttenImgAdd')
                                .find('img')
                                .attr('src', domainCDN_MediaShowContent + selected.viewImage);
                            $('.parttenImgAddWarning').hide();
                        } else {
                            $('.parttenImgAdd').hide();
                            $('.parttenImgAddWarning').show();
                        }
                    } else {
                        $('.parttenImgAdd').hide();
                        $('.parttenImgAddWarning').show();
                        $('.TemplateComponentId').val('');
                    }
                } catch (error) {
                    $('.parttenImgAdd').hide();
                    $('.parttenImgAddWarning').show();
                    $('.TemplateComponentId').val('');
                }
            });
        },
        onInitTagify: function () {
            var LinkStyles = $('#LinkStyles')[0];
            var LinkScripts = $('#LinkScripts')[0];
            new Tagify(LinkStyles);
            new Tagify(LinkScripts);
        },
        onInitpageTagify: function () {
            var LinkPageStyles = $('#LinkPageStyles')[0];
            var LinkPaegScripts = $('#LinkPaegScripts')[0];
            new Tagify(LinkPageStyles);
            new Tagify(LinkPaegScripts);
        },
        onCustomeSelect2: function () {
            $(document).ready(function () {
                function formatState(state) {
                    if (!state.id) {
                        return state.text;
                    }

                    return $('<div style="' + $(state.element).data('style') + '"> ' + state.text + '</div>');
                }

                $('.kt_select2_Category').select2({
                    templateResult: formatState
                });
            });
        },
        onUpdateIdx: function () {
            $(document).on('click', '.updateIdx', function (e) {
                let $this = $(this);
                if (!$this.filter('.disabled').length) {
                    Common_another.onUserSwalFire__ques('Bạn chắc chắn cập nhật lại Idx?').then((result) => {
                        if (result.value) {
                            let request = {
                                Components: new Array()
                            };
                            let $trs = $('.pageTemplateTable').find('tr');

                            for (let i = 1; i < $trs.length; i++) {
                                request.Components.push({
                                    Id: $($trs[i]).find('.id').val(),
                                    Idx: $($($trs[i]).find('td')[4]).html()
                                });
                            }
                            Common_another_ajax.post(`Landingpage/PageComponentUpdateidx?`, request).then((data) => {
                                if (data.Success) {
                                    Common.onSuccess('Cập nhật lại Idx thành công!');
                                    $this.addClass('disabled').addClass('cursor-not-allowed');
                                } else {
                                    eval(data.CallBack);
                                }
                            });
                        }
                    });
                }
            });
        },
        onInitSelect2URLTarget: function () {
            let dataAdd = {
                page: 1,
                perpage: 30,
                field: 'CreatedDate',
                sort: 'Desc'
            };
            let exNameToShow = {
                name: 'Name',
                exName: 'URL',
                exNameToShow: 'URL'
            };
            Common.onLoadSelect2Show('select-URLTarget', $('#pageUpdateURLTarget'), '/LandingPage/PageGetByPage', exNameToShow, dataAdd, '', true);
            $('body').on('change', '.select-URLTarget', function () {
                let $this = $(this);
                $('.my-loading').hide();
                if ($this.val()) {
                    let selected = JSON.parse($this.val());
                    $('#URLTargetInput').val(selected.URL);
                } else {
                    $('#URLTargetInput').val('');
                }
            });
        }
    };
})();

var pageFunc = (function () {
    return {
        onRendAfterEditPage: function (mess) {
            Common.onSuccess(mess);
        },
        onRendAfterUpdatePage: function (mess) {
            Common.onSuccess(mess, '', 2000);
            $('#selectPageTypeModal').modal('hide');
        }
    };
})();
