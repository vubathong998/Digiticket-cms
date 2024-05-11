var $buttonIsBeingUpdating;

var category = (function () {
    return {
        init: function () {
            this.onLoadTree();
            this.onAction();
        },
        onRendAfterUpdate: function(name) {
            Common.onSuccess();
            $buttonIsBeingUpdating.siblings('span').html(name);
        },
        onLoadTree: function () {
            $(document).ready(function () {
                $('#categoryTree').jstree({
                    core: {
                        themes: {
                            responsive: false
                        }
                    },
                    types: {
                        default: {
                            icon: 'fa fa-folder'
                        },
                        file: {
                            icon: 'fa fa-file'
                        }
                    }
                });
            });
        },
        onAction: function () {
            $(document).on('click', '.categoryBtn', function (e) {
                let $this = $(this);
                let _id = $this.data('id');
                $buttonIsBeingUpdating = $this;
                loadModalContent(`${domain}/Lib/LibCategoryUpdateModal?id=${encodeURI(_id)}`);
            });
        }
    };
})();
