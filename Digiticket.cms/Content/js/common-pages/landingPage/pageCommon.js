var pageCommon = (function () {
    return {
        select2: function () {
            // #region excute
            let select2CategoryTag = function () {
                let variToFill = {
                    name: 'Name',
                    exNameToShow: 'Parent',
                    exName: 'ParentName'
                };
                let exData = {
                    page: 'desc',
                    field: 'CreatedDate',
                    page: 1,
                    perpage: 25
                };
                Common.onLoadSelect2Show('select-categoryTag', $('.selectCategoryTag'), 'CategoryTag/CategoryTagGetByPage', variToFill, exData, '', true);
                $('body').on('change', '.select-categoryTag', function () {
                    let $this = $(this);
                    let $container = $this.closest('.selectCategoryTag');
                    let selected = $this.val();
                    $('.categoryTagInputHidden').remove();

                    let data = JSON.parse(selected);
                    if (data) {
                        /*html*/
                        let HTML = `
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].Id" value="${data.TagId}">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].ParentId" value="${data.TagParentId || 0}">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].CategoryId" value="${guidEmpty}">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].DestinationId" value="${guidEmpty}">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].TextView" value="${data.TextView}">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].TextLink" value="${data.TextLink}">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].Name" value="${data.Name}">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].Type" value="1">
                        <input class="categoryTagInputHidden" type="hidden" name="CategoryTags[0].Idx" value="${data.Idx}">
                        `;
                        $container.append(HTML);
                    }
                });
            };

            let select2Avatar = function () {
                // let exData = {
                //     page: 'desc',
                //     field: 'CreatedDate',
                //     page: 1,
                //     perpage: 25
                // };
                Common.onLoadSelect2Show('select-pageAvatar', $('.selectAvatar'), '/Media/AutoSuggestAvatar', 'Url', '', '', true);
                $('body').on('change', '.select-pageAvatar', function () {
                    let $this = $(this);
                    let $container = $('.selectAvatar');
                    if ($this.val()) {
                        let selected = JSON.parse($this.val());
                        let url = `${domainCDN_MediaShowContent}${selected.Url}`;
                        $('.pageAvatarElement').val(selected.Url);
                        $container.find('.preViewAvatar').attr('href', url).show();
                        $container.find('#avatarElement').val(selected.Url);
                    } else {
                        $container.find('.preViewAvatar').attr('href', '').hide();
                        $('.pageAvatarElement').val('');
                        $container.find('#avatarElement').val('');
                    }
                });
            };

            let select2Tag = function () {
                let variToFill = {
                    name: 'Name',
                    exNameToShow: 'Parent',
                    exName: 'ParentName'
                };
                let exData = {
                    type: 1
                };
                Common.onLoadSelect2Show('select-Tag', $('.selectTag'), 'Tags/SuggesTagsInUpdateTags', variToFill, exData, '', false);
                $('body').on('change', '.select-Tag', function () {
                    let $this = $(this);
                    let $container = $this.closest('.selectTag');
                    let selected = $this.val();
                    $('.tagInputHidden').remove();

                    selected.forEach((element, i) => {
                        let data = JSON.parse(element);

                        /*html*/
                        let HTML = `
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].Id" value="${data.Id}">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].ParentId" value="${data.ParentId || 0}">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].CategoryId" value="${data.CategoryId}">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].DestinationId" value="${data.DestinationId}">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].TextView" value="${data.TextView}">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].TextLink" value="${data.TextLink}">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].Name" value="${data.Name}">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].Type" value="1">
                            <input class="tagInputHidden" type="hidden" name="Tags[${i}].Idx" value="${data.Idx}">
                        `;
                        $container.append(HTML);
                    });
                });
            };
            // #endregion

            select2CategoryTag();
            select2Avatar();
            select2Tag();
        }
    };
})();
