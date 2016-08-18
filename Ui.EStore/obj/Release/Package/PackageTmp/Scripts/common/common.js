var commonViewModel = function () {

    function isNumber(o) {
        return !isNaN(o - 0) && o !== null && o.replace(/^\s\s*/, '') !== "" && o !== false;
    }


    function savedMsgDataEn() {
        return "تم الحفظ بنجاح.";
    }


    function confirmationDeleteMsgDataEn() {
        return "هل حقا تريد حذف هذا السجل?!";
    }

    function canNotSaveMsgDataEn() {
        return "خطأ لا يمكن حفظ البيانات.";
    }

    function existMsgDataEn(dataName) {
        return dataName + " موجود بالفعل.";
    }


    function canNotDeleteMsgDataEn() {
        return "لا يمكن حذف هذا السجل.";
    }

    function errorOccurred() {
        return "خطأ فى التنفيذ.";
    }

    function isRequiredMsgDataEn(dataName) {
        return dataName + " مطلوب.";
    }

    function noDataToDisplayMsgDataEn(dataName) {
        return "لا توجد بيانات للعرض بخصوص  " + dataName + "!";
    }



    function completeAllRequiredMsgDataEn() {
        return "من فضلك اكمل البيانات المطلوبه.";
    }


    function displayErrorHtmlMessage(result) {
        var errors = result.ClientMessageContent;
        var divError = jQuery("<div/>");
        var ul = jQuery("<ul/>");
        jQuery.each(errors, function (i) {
            jQuery('<li/>').text(errors[i]).appendTo(ul);
        });
        ul.appendTo(divError);


        jQuery.pnotify({
            title: 'خطأ!',
            text: divError.html(),
            type: 'error'
        });
    }


    function displaySuccessMessage() {
        jQuery.pnotify({
            title: '',
            text: savedMsgDataEn(),
            type: 'success'
        });
    }
    
    function displayTextSuccessMessage(msg) {
        jQuery.pnotify({
            title: '',
            text: msg,
            type: 'success'
        });
    }

    function displayNoticeMessage(message) {
        jQuery.pnotify({
            title: 'ملاحظة',
            text: message,
            type: 'notice'
        });
    }

    function displayErrorTextMessage(message) {
        jQuery.pnotify({
            title: 'خطأ!',
            text: message,
            type: 'error'
        });
    }

    function getParameterFromQueryStringByName(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results === null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    function openIframeInModalBox(title, frameSrc, modelBoxId, modelWidth) {
        modelWidth = modelWidth || '90%';

        jQuery(modelBoxId).on('show', function () {
            jQuery(modelBoxId + ' .modal-header h3').html(title);
            jQuery('iframe').attr("src", frameSrc);
        });
        jQuery(modelBoxId).css({ width: modelWidth });
        jQuery(modelBoxId).modal({ show: true });
    }

    function fillSelectMethod(selectId, result) {
        $(selectId+" option[value!='-1']").each(function () {
            $(this).remove();
        });

        $.each(result.ReturnedData, function(index, Element) {
            var o = new Option(Element.Name, Element.Id);
            /// jquerify the DOM object 'o' so we can use the html method
            $(o).html(Element.Name);
            $(selectId).append(o);
        });
    }


    jQuery(".numberOnly").keydown(function (event) {
        //if (this.value != this.value.replace(/[^0-9\.]/g, '')) {
        //    this.value = this.value.replace(/[^0-9\.]/g, '');
        //}
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    jQuery(".noText").keydown(function (event) {
        event.preventDefault();
    });



    function dateFormated(dateObj) {
        var d = new Date(dateObj);
        var day = d.getDate();
        var month = d.getMonth() + 1;
        var year = d.getFullYear();

        if (month < 10) {
            month = "0" + month;
        }
        return year + "/" + month + "/" + day;
    };

    var HasValue = function hasValueMethod(itm) {
        var pageIsValid = true;
        if (itm.val().length == 0) {
            itm.parent().parent().addClass("has-error");
            pageIsValid = false;
        } else {
            if (itm.parent().parent().hasClass("has-error")) {
                itm.parent().parent().removeClass("has-error");
            }
        }
        return pageIsValid;
    };

    var IsValidNumber = function isValidNumberMethod(itm) {
        var pageIsValid = true;
        if ($.isNumeric(itm.val())) {
            if (itm.val() > 0) {
                if (itm.parent().parent().hasClass("has-error")) {
                    itm.parent().parent().removeClass("has-error");
                }
            }
            else {
                itm.parent().parent().addClass("has-error");
                pageIsValid = false;
            }
        }
        else {
            itm.parent().parent().addClass("has-error");
            pageIsValid = false;
        }
        return pageIsValid;
    };

    var IsValidDll = function isValidDllMethod(itm) {
        var pageIsValid = true;
        if (itm.val() == -1) {
            itm.parent().parent().addClass("has-error");
            pageIsValid = false;
        } else {
            if (itm.parent().parent().hasClass("has-error")) {
                itm.parent().parent().removeClass("has-error");
            }
        }
        return pageIsValid;
    };

    return {
        DisplayErrorHtmlMessage: displayErrorHtmlMessage,
        DisplaySuccessMessage: displaySuccessMessage,
        displayTextSuccessMessage:displayTextSuccessMessage,
        DisplayNoticeMessage: displayNoticeMessage,
        DisplayErrorTextMessage: displayErrorTextMessage,
        ConfirmationDeleteMsgDataEn: confirmationDeleteMsgDataEn,
        CanNotDeleteMsgDataEn: canNotDeleteMsgDataEn,
        GetParameterFromQueryStringByName: getParameterFromQueryStringByName,
        NoDataToDisplayMsgDataEn: noDataToDisplayMsgDataEn,
        IsNumber: isNumber,
        OpenIframeInModalBox: openIframeInModalBox,
        DateFormated: dateFormated,
        hasValue: HasValue,
        isValidNumber: IsValidNumber,
        isValidDll: IsValidDll,
        fillSelect: fillSelectMethod
    };
}();