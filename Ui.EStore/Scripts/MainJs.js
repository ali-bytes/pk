


var UploadScript = function() {

    function uploadFileMethod(evt, uploadId,image) {
        evt.preventDefault();
        var data = new FormData();
        var ofile = document.getElementById(uploadId).files[0];
        if (ofile == '' || ofile==null) {
            return;
        }
        data.append('file', ofile);
      
            $.ajax({
                url: "/Helper/UploadFile",
                data: data,
                type: 'POST',
                success: onuploadFileSuccess,
                processData: false,
                contentType: false,
                error: onuploadFileError
            });

        function onuploadFileSuccess(result) {
            var img = $(image);
            img.attr('src', "/app_files/" + result.ClientMessageContent[0]);
        }

        function onuploadFileError() {
            commonViewModel.DisplayErrorTextMessage('Uploading error');
        }

    }

    return {
        uploadFile: uploadFileMethod
    };
}();