var rexPhoneNumber = /(84|0[3|5|7|8|9])+([0-9]{8})/;
var rexEmail = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;

/**
 * @summary chuyển đổi từ một dữ liệu sang dữ liệu khác.
 *
 * @parameter
 *
 * @return Functions
 *
 * @exceptions
 */
var Convert = (function () {
    return {
        /**
         * @summary tách chuỗi thời gian thành mảng
         *
         * @parameter
         *      date chuỗi thời gian
         *      type kiểu thời gian của chuỗi nhận vào. Gồm: dmy, mdy, ymd
         *
         * @return {array}
         *      array ( chuỗi thời gian được tách).gồm: [year, month, day]
         *
         * @exceptions
         */
        splitStrDateToArr: function (strDate, type = 'ymd') {
            let arrDate = strDate.trim().split('/');
            if (Common_another.ciEquals(type, 'ymd')) {
                return strDate.split(/[\-\/]/g);
            }
            if (Common_another.ciEquals(type, 'dmy')) {
                return [arrDate[2], arrDate[1], arrDate[0]];
            }
            if (Common_another.ciEquals(type, 'mdy')) {
                return [arrDate[2], arrDate[0], arrDate[1]];
            }
            alert('Thời gian hoặc Type k đúng');
            return new Array();
        },
        /**
         * @summary Chuyển chuỗi ngày thành ngày ( 12/12/2022 -> date )
         *
         * @parameter
         *      - str date
         *      - Dạng thời gian truyền vào. gồm: ymd (default), dmy, mdy
         *
         * @return {date}
         *
         * @exceptions
         */
        strDateToDate: function (strDate, type = 'ymd') {
            let rexMini = /^\/*\d{9,10}\/*$/g;
            let rexSecond = /^\/*\d{12,13}\/*$/g;
            let rexTextMiniSecond = /^\/Date\(\d{12,13}\)\/$/g;

            if (rexMini.test(strDate)) {
                let miniStrDate = Number(strDate.replace(/\//g, ''));
                return new Date(Math.floor(miniStrDate) * 1000);
            } else if (rexSecond.test(strDate)) {
                let miniStrDate = Number(strDate.replace(/\//g, ''));
                return new Date(miniStrDate);
            } else if (rexTextMiniSecond.test(strDate)) {
                let strDateNumber = strDate.replace(/[^\d]+/g, '');
                return new Date(Number(strDateNumber));
            } else {
                let arrDate = this.splitStrDateToArr(strDate, type);
                if (!arrDate) return false;
                return new Date(`${arrDate[0]}/${arrDate[1]}/${arrDate[2]}`);
            }
        },
        /**
         * @summary Chuyển thời gian thành chuỗi
         *
         * @parameter
         *      - date
         *      - Dạng thời gian muốn trả về. gồm ymd (default), dmy, mdy
         *
         * @return {string}
         *
         * @exceptions
         */
        dateToStrDate: function (date, type = 'ymd') {
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (Common_another.ciEquals(type, 'ymd')) {
                return `${year}/${month}/${day}`;
            }
            if (Common_another.ciEquals(type, 'dmy')) {
                return `${day}/${month}/${year}`;
            }
            if (Common_another.ciEquals(type, 'mdy')) {
                return `${month}/${day}/${year}`;
            }
            alert('Thời gian hoặc Type k đúng');
            return undefined;
        },
        /**
         * @summary  Tính ngày
         *
         * @param {date} date1 Ngày tính .
         * @param {date} num2 số trừ .
         * @param {date} str3 đơn vị trừ (d,w,m,y,mago) .
         *
         * @return {date}
         *
         * @Exceptions
         */
        calcDate: function (date = new Date(), step, type) {
            if (typeof step !== 'number') step = Number(step);
            if (type === 'now') return new Date();
            else if (Common_another.ciEquals(type, 'd') || Common_another.ciEquals(type, 'day')) return new Date(date.setDate(date.getDate() + step));
            else if (Common_another.ciEquals(type, 'w') || Common_another.ciEquals(type, 'week')) return new Date(date.setDate(date.getDate() + step * 7));
            else if (Common_another.ciEquals(type, 'm') || Common_another.ciEquals(type, 'month')) return new Date(date.setMonth(date.getMonth() + step));
            else if (Common_another.ciEquals(type, 'y') || Common_another.ciEquals(type, 'year')) return new Date(date.setFullYear(date.getFullYear() + step));
            else if (Common_another.ciEquals(type, 'mago') || Common_another.ciEquals(type, 'monthago')) {
                step < 0 ? (step = Math.abs(step)) : step;
                let rexTypeText = /^[a-zA-Z]/g;
                let typeText = type.match(rexTypeText)[0];
                if (typeText === 'm' || typeText === 'M') {
                    let justMonthAgo = new Date(date.setMonth(date.getMonth() - step));
                    let day = 1;
                    let month = justMonthAgo.getMonth() + 1;
                    let year = justMonthAgo.getFullYear();
                    return new Date(`${year}-${month}-${day}`);
                }
            }
            //ngày cuối của tháng
            else if (Common_another.ciEquals(type, 'magolast')) {
                step < 0 ? (step = Math.abs(step)) : step;
                let rexTypeText = /^[a-zA-Z]/g;
                let typeText = type.match(rexTypeText)[0];
                if (typeText === 'm' || typeText === 'M') {
                    let justMonthAgo = new Date(date.setMonth(date.getMonth() - step + 1));
                    let day = 1;
                    let month = justMonthAgo.getMonth() + 1;
                    let year = justMonthAgo.getFullYear();
                    let justMonthAgolastDay = new Date(new Date(`${year}-${month}-${day}`).setDate(new Date(`${year}-${month}-${day}`).getDate() - 1));
                    return justMonthAgolastDay;
                }
            }
        },
        dotAfter3Digits: function (x) {
            x = Number(x);
            var parts = x.toString().split('.');
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            return parts.join(',');
        },
        onFillterStrict: function (value) {
            if (!value || value == '' || value == 'null' || value == 'undefined' || value == 'NaN' || value == 'false' || value == 'False') return '';
            return value;
        },
        onHtmlEncode: function (s) {
            // var el = document.createElement('div');
            // el.innerText = el.textContent = s;
            // s = el.innerHTML;
            // return s;
            return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/'/g, '&#39;').replace(/"/g, '&quot;').replace(/\//, '&#x2F;');
        },
        onHtmlDecode: function (s) {
            var el = document.createElement('div');
            el.innerHTML = s;
            s = el.innerHTML;
            return s;
        },
        decodeHTMLEntities: function (str) {
            var element = document.createElement('div');
            if (str && typeof str === 'string') {
                // strip script/html tags
                str = str.replace(/<script[^>]*>([\S\s]*?)<\/script>/gim, '');
                str = str.replace(/<\/?\w(?:[^"'>]|"[^"]*"|'[^']*')*>/gim, '');
                element.innerHTML = str;
                str = element.textContent;
                element.textContent = '';
            }

            return str;
        },
        removeVNTonesMark: function (str) {
            if (str) {
                str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, 'a');
                str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, 'e');
                str = str.replace(/ì|í|ị|ỉ|ĩ/g, 'i');
                str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, 'o');
                str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, 'u');
                str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, 'y');
                str = str.replace(/đ/g, 'd');
                str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, 'A');
                str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, 'E');
                str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, 'I');
                str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, 'O');
                str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, 'U');
                str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, 'Y');
                str = str.replace(/Đ/g, 'D');
                return str.trim();
            } else {
                return str;
            }
        }
    };
})();

var Common_another = (function () {
    return {
        /**
         * @summary
         *
         * @param {date} data.
         *
         * @return {date} trả về ngày bé hơn. nếu bằng nhau thì trả về false
         *
         * @Exceptions
         */
        findMinDate: function (date1, date2) {
            if (date1.getTime() === date2.getTime()) return false;
            if (date1.getTime() < date2.getTime()) return date1;
            if (date1.getTime() > date2.getTime()) return date2;
        },
        /**
         * @summary so sánh được cả chữ hoa và thường
         *
         * @param {string} str1
         * @param {string} str2
         *
         * @return {boolean} bool
         *
         * @exceptions
         */
        ciEquals: function (a, b) {
            return typeof a === 'string' && typeof b === 'string' ? a.localeCompare(b, undefined, { sensitivity: 'accent' }) === 0 : a === b;
        },
        checkCorrectDate: function (date) {
            if (/^\/Date\(\-*\d+\)\/$/.test(date)) {
                let numDate = Number(date.replace(/[^\d\-]/g, ''));
                if (numDate === 'NaN' || numDate <= 0) return false;
            } else {
            }
            return true;
        },
        IsTrueOrFalse: function (value) {
            if (
                !value ||
                value == '' ||
                value <= 0 ||
                value == 'null' ||
                value == 'undefined' ||
                value == 'NaN' ||
                value == 'false' ||
                value == 'False' ||
                value == '00000000-0000-0000-0000-000000000000'
            )
                return false;
            return true;
        },
        showErr: function (err) {
            swal.fire({
                // //position: 'top-right',
                type: 'error',
                title: 'có lỗi xảy ra! Vui lòng thử lại! ' + err,
                showConfirmButton: false,
                timer: 1500
            });
        },
        onUserSwalFire__ques: function (messageTitle = 'Bạn chắn chắn?', messageText) {
            return Swal.fire({
                title: messageTitle,
                text: messageText,
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Có!',
                cancelButtonText: 'Không!'
            });
        }
    };
})();

var Common_another_ajax = (function () {
    return {
        /**
         * @summary : post ajax
         *
         * @param {string} str . url
         * @param {object} obj . data
         * @param {boolean} bool . cache
         *
         * @return {string}:
         *
         * @Exceptions :
         */
        post: function (url, data, cache = false) {
            return $.ajax({
                url: url,
                dataType: 'json',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ data: data }),
                async: true,
                processData: false,
                cache: cache,
                success: function (data) {
                    return data;
                },
                error: function (xhr) {
                    Common_another.showErr(xhr.status + ' ');
                }
            });
        },
        renPartial: function (url, container, callback) {
            $('.my-loading').show();
            let $container = $(container);
            $($container).load(url, function (response, status, xhr) {
                $('.my-loading').hide();
                $container.html(response);
                if (status === 'error') {
                    swal.fire({
                        //position: 'top-right',
                        type: 'error',
                        title: `Có lỗi xảy ra: ${xhr.status} ${xhr.statusText}`,
                        showConfirmButton: false,
                        timer: 1500
                    });
                } else {
                    if (callback !== '') {
                        eval(callback);
                    }
                }
            });
        },
        /**
         * @summary :
         *
         * @param {string} .
         *
         * @return {string}:
         *
         * @Exceptions :
         */
        renPartial___Get: function (url, container = '#defaultModal', callback, sizeModal = '', cache = false) {
            $('.my-loading').show();
            $('.dropdown-menu').removeClass('show');

            var $modal = $(container);
            var $modalContent = $('#defaultModalContent');
            this.get(url, cache).then((data) => {
                setTimeout(function () {
                    $('.my-loading').hide();
                    if (!data.Success) {
                        swal.fire({
                            //position: 'top-right',
                            type: 'error',
                            title: `Có lỗi xảy ra: ${xhr.status} ${xhr.statusText}`,
                            showConfirmButton: false,
                            timer: 1500
                        });
                    } else {
                        $($modal).find('.modal-dialog').removeClass().addClass('modal-dialog').addClass(sizeModal);
                        new Promise((resolve) => {
                            $($modal).modal('show');
                            $modalContent.html(data.Data);
                            resolve();
                        }).then(() => {
                            if (callback !== '') {
                                eval(callback);
                            }
                        });
                    }
                }, 500);
            });
        },
        get: function (url, cache = false) {
            $('.my-loading').show();
            return $.ajax({
                url: `${domain}/${url}`,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: true,
                processData: false,
                cache: cache,
                success: function (data) {
                    $('.my-loading').hide();
                    eval(data.CallBack);
                    return data;
                },
                error: function (xhr) {
                    $('.my-loading').hide();
                    Common_another.showErr(xhr.status + ' ');
                }
            });
        },
        post: function (url, data, cache = false) {
            $('.my-loading').show();
            return $.ajax({
                url: `${domain}/${url}`,
                dataType: 'json',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ model: data }),
                async: true,
                processData: false,
                cache: cache,
                success: function (data) {
                    return data;
                },
                error: function (xhr) {
                    $('.my-loading').hide();
                    Common_another.showErr(xhr.status + ' ');
                }
            });
        },
        onDelete___get: function (url, mess = '') {
            Swal.fire({
                title: `Bạn chắn chắn ${mess ? `xóa ${mess}` : ''}?`,
                text: 'Sẽ KHÔNG THỂ khôi phục lại được',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Có, Xóa nó!',
                cancelButtonText: 'Không!'
            }).then((result) => {
                if (result.value) {
                    $('.my-loading').show();
                    Common_another_ajax.get(`${url}`).done((data) => {
                        new Function(data.CallBack)();
                    });
                }
            });
        }
    };
})();

var init = (function () {
    return {
        init: function () {
            this.onPrototypeConfig();
            this.onHightLightWhenChosen();
            this.onPreventDefaultInput();
        },
        onHightLightWhenChosen: function () {
            $(document).on('change', '.hightLight', function (e) {
                let $this = $(this);
                if ($this.filter('[class*="kt-select2"]').length > 0) {
                    if (Common_another.IsTrueOrFalse($this.val())) {
                        $this.filter('[class*="kt-select2"]').siblings('.select2 ').find('.select2-selection__rendered').addClass('hightLighted');
                    } else {
                        $this.filter('[class*="kt-select2"]').siblings('.select2 ').find('.select2-selection__rendered').removeClass('hightLighted');
                    }
                } else if ($this.val() != '') {
                    $this.addClass('hightLighted');
                } else {
                    $this.removeClass('hightLighted');
                }
            });
        },
        onPrototypeConfig: function () {
            String.prototype.IsTrueOrFalse = function () {
                if (!this || this == '' || this <= 0 || this == 'null' || this == 'undefined' || this == 'NaN' || this == 'false' || this == 'False' || this == '00000000-0000-0000-0000-000000000000')
                    return false;
                return true;
            };
            String.prototype.FillterStrict = function () {
                if (!this || this == '' || this == 'null' || this == 'undefined' || this == 'NaN' || this == 'false' || this == 'False' || this == '00000000-0000-0000-0000-000000000000') return '';
                return this.toString();
            };
        },
        onPreventDefaultInput: function () {
            $(document).on('click', '.havePreventedDefaul', function (e) {
                e.preventDefault();
            });
        }
    };
})();

(() => {
    init.init();
})();
