var mediaAddModal = (function () {
    return {
        init: function () {
            this.onAutoFillSizeMedia();
        },
        onAutoFillSizeMedia: function () {
            function readImage(file) {
                var useBlob = false && window.URL;
                // Create a new FileReader instance
                // https://developer.mozilla.org/en/docs/Web/API/FileReader
                var reader = new FileReader();

                // Once a file is successfully readed:
                reader.addEventListener('load', function () {
                    // At this point `reader.result` contains already the Base64 Data-URL
                    // and we've could immediately show an image using
                    // `elPreview.insertAdjacentHTML("beforeend", "<img src='"+ reader.result +"'>");`
                    // But we want to get that image's width and height px values!
                    // Since the File Object does not hold the size of an image
                    // we need to create a new image and assign it's src, so when
                    // the image is loaded we can calculate it's width and height:
                    var image = new Image();
                    image.addEventListener('load', function () {
                        // Concatenate our HTML image info
                        // var imageInfo =
                        //     file.name +
                        //     ' ' + // get the value of `name` from the `file` Obj
                        //     image.width +
                        //     '×' + // But get the width from our `image`
                        //     image.height +
                        //     ' ' +
                        //     file.type +
                        //     ' ' +
                        //     Math.round(file.size / 1024) +
                        //     'KB';

                        // Finally append our created image and the HTML info string to our `#preview`
                        // elPreview.appendChild(this);
                        // elPreview.insertAdjacentHTML('beforeend', imageInfo + '<br>');

                        $('#mediaUpload .resizeWidth').val(image.width);
                        $('#mediaUpload .resizeHeight').val(image.height);

                        // If we set the variable `useBlob` to true:
                        // (Data-URLs can end up being really large
                        // `src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAA...........etc`
                        // Blobs are usually faster and the image src will hold a shorter blob name
                        // src="blob:http%3A//example.com/2a303acf-c34c-4d0a-85d4-2136eef7d723"
                        if (useBlob) {
                            // Free some memory for optimal performance
                            window.URL.revokeObjectURL(image.src);
                        }
                    });

                    image.src = useBlob ? window.URL.createObjectURL(file) : reader.result;
                });

                // https://developer.mozilla.org/en-US/docs/Web/API/FileReader/readAsDataURL
                reader.readAsDataURL(file);
            }
            // this.onDisableAddBtn();
            let fileInput = $('[name="fileImage"]')[0];
            $('[name="fileImage"]').on('change', function (e) {
                // Let's store the FileList Array into a variable:
                // https://developer.mozilla.org/en-US/docs/Web/API/FileList
                var files = this.files;
                // Let's create an empty `errors` String to collect eventual errors into:
                var errors = '';

                if (!files) {
                    errors += 'File upload not supported by your browser.';
                }

                // Check for `files` (FileList) support and if contains at least one file:
                if (files && files[0]) {
                    // Iterate over every File object in the FileList array
                    for (var i = 0; i < files.length; i++) {
                        // Let's refer to the current File as a `file` variable
                        // https://developer.mozilla.org/en-US/docs/Web/API/File
                        var file = files[i];

                        // Test the `file.name` for a valid image extension:
                        // (pipe `|` delimit more image extensions)
                        // The regex can also be expressed like: /\.(png|jpe?g|gif)$/i
                        if (/\.(png|jpeg|jpg|gif)$/i.test(file.name)) {
                            // SUCCESS! It's an image!
                            // Send our image `file` to our `readImage` function!
                            readImage(file);
                        } else {
                            errors += file.name + ' Unsupported Image extension\n';
                        }
                    }
                }

                // Notify the user for any errors (i.e: try uploading a .txt file)
                if (errors) {
                    alert(errors);
                }
            });
        },
        onUploadMedia: function () {
            $('body').on('click', '.btn-upload-media', function (e) {
                e.preventDefault();
                $('.my-loading').show();
                mediaAddModal.onDisableAddBtn();
                let $this = $(this);
                let $parent = $this.closest('#mediaUpload');
                let $input = $parent.find('input[type="file"]');
                let _width = $parent.find('input[name="resizeWidth"]').val();
                let _height = $parent.find('input[name="resizeHeight"]').val();
                setTimeout(function () {
                    Common.onShowLoadingBtn($this);
                    if ($input.val() == '') {
                        swal.fire({
                            //position: 'top-right',
                            type: 'warning',
                            title: 'Chưa chọn file',
                            showConfirmButton: false,
                            timer: 1500
                        });
                        Common.onHideLoadingBtn($this);
                        return;
                    }
                    if (_width == '' || _width <= 0 || _height == '' || _height <= 0) {
                        swal.fire({
                            //position: 'top-right',
                            type: 'warning',
                            title: 'Kích thước ảnh gốc nhập chưa đúng',
                            showConfirmButton: false,
                            timer: 1500
                        });
                        Common.onHideLoadingBtn($this);
                        return;
                    }
                    let data = new FormData();
                    data.append('file0', $input[0].files[0]);
                    data.append('resizeWidth', _width);
                    data.append('resizeHeight', _height);
                    $.ajax({
                        url: `${domain}/Media/MediaAdd`,
                        type: 'POST',
                        data: data,
                        async: false,
                        cache: false,
                        contentType: false,
                        enctype: 'multipart/form-data',
                        processData: false,
                        success: function (result) {
                            if (result.Success) {
                                // swal.fire('Great!', 'Thành công!', 'success');
                                result.CallBack ? eval(result.CallBack) : '';
                            } else {
                                swal.fire({
                                    //position: 'top-right',
                                    type: 'warning',
                                    title: result.Message,
                                    showConfirmButton: false,
                                    timer: 1500
                                });
                            }
                        }
                    }).done(function () {
                        Common.onHideLoadingBtn($this);
                    });
                }, 500);
            });
        },
        onRendAfterMediaAdd: function () {
            Common.onReloadDataTable(dtTableMedia, '.table-media', 'Thêm mới thành công!');
        },
        onDisableAddBtn: function () {
            let $form = $('#mediaUpload');
            $form.find('button[type=submit]').attr('disabled', true);
            if (!$form.find('button[type=submit] .kt-spinner').length) $form.find('button[type=submit]').addClass(' kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        }
    };
})();
