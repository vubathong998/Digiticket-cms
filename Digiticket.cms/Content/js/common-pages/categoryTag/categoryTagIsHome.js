var categoryTagIsHome = (function () {
    return {
        initImedi: function () {
            this.initTable();
        },
        initTable: function () {
            hardExpandVar = {
                isHome: true
            };
            categoryTag.init('IdxHome');
        }
    };
})();
(() => {
    categoryTagIsHome.initImedi();
})();
