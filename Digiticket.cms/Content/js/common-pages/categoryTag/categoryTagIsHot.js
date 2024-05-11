var categoryTagIsHot = (function () {
    return {
        initImedi: function () {
            this.initTable();
        },
        initTable: function () {
            hardExpandVar = {
                isHot: true
            };
            categoryTag.init('IdxHot');
        }
    };
})();
(() => {
    categoryTagIsHot.initImedi();
})();
