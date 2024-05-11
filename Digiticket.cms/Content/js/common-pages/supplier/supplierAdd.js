var CreateOrUpdate = (function () {
    return {
        init: function () {
            this.onSelectTags();
            this.validation();
        },
        onRendAfterAdd: function (id) {
            $('.my-loading').hide();
            swal.fire({
                //position: 'top-right',
                type: 'success',
                title: 'Thêm mới nhà cung cấp thành công! Bạn sẽ được chuyển đến trang xem chi tiết!',
                showConfirmButton: false,
                timer: 1500
            });
            window.location = `${domain}/Supplier/SupplierDetail?id=${id}`;
        },
        onSelectTags: function () {
            var $inputHotLine = $('input[name=hotline]')[0];
            var $inputBookingEmail = $('input[name=bookingEmail]')[0];
            new Tagify($inputHotLine, {
                transformTag: function (tagData) {
                    if (!rexPhoneNumber.test(tagData.value)) tagData.value = '';
                }
            });
            new Tagify($inputBookingEmail, {
                transformTag: function (tagData) {
                    if (!rexEmail.test(tagData.value)) tagData.value = '';
                }
            });
        },
        validation: function () {
            $.validator.addMethod(
                'phoneNumber',
                function (value) {
                    return /([\+84|84|0]+(3|5|7|8|9|1[2|6|8|9]))+([0-9]{8})\b/g.test(value);
                },
                'Không đúng định dạng số điện thoại!'
            );
            // $.validator.addMethod(
            //     'phoneNumber',
            //     function (value) {
            //         return /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g.test(value);
            //     },
            //     'Không đúng định dạng số điện thoại!'
            // );
            // $("#SupplierCreate").validate({
            //     ignore: [],
            //     rules: {
            //         hotline: "phoneNumber"
            //     },
            //     messages: {
            //         hotline: {
            //             phoneNumber: "HotLine Phải đúng định dạng số điện thoại"
            //         }
            //     }
            //   });
            $('#SupplierCreate').validate({
                rules: {
                    name: {
                        required: true
                    },
                    //code: {
                    //    required: true
                    //},
                    //shortDescription: {
                    //    required: true
                    //},
                    //description: {
                    //    required: true
                    //},
                    //address: {
                    //    required: true
                    //},
                    title: {
                        required: true
                    },
                    contactName: {
                        required: true
                    },
                    phone: {
                        required: true,
                        phoneNumber: true
                    },
                    email: {
                        required: true
                    },
                    hotline: {
                        required: true
                    },
                    bookingEmail: {
                        required: true
                    }
                    //companyName: {
                    //    required: true
                    //},
                    //companyAddress: {
                    //    required: true
                    //},
                    //companyTaxCode: {
                    //    required: true
                    //},
                    //companyRepresentative: {
                    //    required: true
                    //},
                },
                messages: {
                    name: {
                        required: 'Không được bỏ trống name!'
                    },
                    //code: {
                    //    required: 'Không được bỏ trống code!'
                    //},
                    //shortDescription: {
                    //    required: 'Không được bỏ trống shortDescription!'
                    //},
                    //description: {
                    //    required: 'Không được bỏ trống description!'
                    //},
                    //address: {
                    //    required: 'Không được bỏ trống address!'
                    //},
                    title: {
                        required: 'Không được bỏ trống title!'
                    },
                    contactName: {
                        required: 'Không được bỏ trống contactName!'
                    },
                    phone: {
                        required: 'Không được bỏ trống phone!',
                        phoneNumber: 'Không đúng định dạng số điện thoại!'
                    },
                    email: {
                        required: 'Không được bỏ trống email!'
                    },
                    hotline: {
                        required: 'Không được bỏ trống hotline!'
                    },
                    bookingEmail: {
                        required: 'Không được bỏ trống bookingEmail!'
                    }
                    //companyName: {
                    //    required: 'Không được bỏ trống companyName!'
                    //},
                    //companyAddress: {
                    //    required: 'Không được bỏ trống companyAddress!'
                    //},
                    //companyTaxCode: {
                    //    required: 'Không được bỏ trống companyTaxCode!'
                    //},
                    //companyRepresentative: {
                    //    required: 'Không được bỏ trống companyRepresentative!'
                    //},
                }
            });
        }
    };
})();

(() => {
    CreateOrUpdate.init();
})();
