var LanguageScript = function () {

    var language = { LanguageToolId: 0, Keywords: '', Description: '', Author: '', ControllerName: '', ActionName: '', UrlParameter: '', SeoUrl: '' };

    function getPageToolsMethod(language, page) {

        var PageLanguage = { LanguageId: 0, PageId: 0 };
        PageLanguage.LanguageId = language;
        PageLanguage.PageId = page;


        sysAjax({

            url: "/Admin/GetpageTools",
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(PageLanguage),

            success: onGetpageToolsSuccess
        });
    }

    function getSeoPageToolsMethod(language, page) {

        var PageLanguage = { LanguageId: 0, PageId: 0 };
        PageLanguage.LanguageId = language;
        PageLanguage.PageId = page;


        sysAjax({

            url: "/Admin/GetSeopageTools",
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(PageLanguage),

            success: onGetpageToolsSuccess
        });
    }

    function onGetpageToolsSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.fillSelect("#ddl_Tools", result);
        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    function getToolLanguageDataByIdMethod() {
        var langtoolId = $("#ddl_Tools").val();

        sysAjax({

            url: "/Admin/GetToolLanguageDataById?id=" + langtoolId,


            success: onGetToolLanguageDataById
        });
    }

    function getSeoUrlMagerDataByIdMethod() {
        var langtoolId = $("#ddl_Tools").val();
       

        sysAjax({

            url: "/Admin/GetSeoToolLanguageDataById?id=" + langtoolId  ,


            success: onGetSeoToolLanguageDataById
        });
    }

    
    function onGetToolLanguageDataById(result) {
        if (result.ClientStatusCode == 0) {

             
            $("#txt_SeoUrl").val(result.ReturnedData.SeoUrl);


        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }
    
    function onGetSeoToolLanguageDataById(result) {
        if (result.ClientStatusCode == 0) {

            $("#txt_Keywords").val(result.ReturnedData.Keywords);
            $("#txt_Description").val(result.ReturnedData.Description);
            $("#txt_Author").val(result.ReturnedData.Author);
            $("#txt_Title").val(result.ReturnedData.Title);
           


        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }
    
    function saveSeoUrlMethod(evt) {
        evt.preventDefault();
        var seo = {};
        seo.Id = $("#ddl_Tools").val();
        seo.ControllerName = $("#txt_Controller").val();
        seo.ActionName = $("#txt_Action").val();
        seo.UrlParameter = $("#txt_Parameters").val();
        seo.SeoUrl = $("#txt_SeoUrl").val();



        sysAjax({
            url: "/Admin/SaveSeoUrl",
            data: JSON.stringify(seo),
            success: onSaveSeoSuccess,

            blockElement: "#formAddUser"
        });


    }

    function saveLanguageMethod(evt) {
        evt.preventDefault();
        var seo = {};
        seo.LanguageToolId = $("#ddl_Tools").val();
        seo.SeoUrl = $("#txt_SeoUrl").val();


        sysAjax({
            url: "/Admin/SaveLanguageValue",
            data: JSON.stringify(seo),
            success: onSaveSeoSuccess,

            blockElement: "#formAddUser"
        });
    }

    function saveSeoMethod(evt) {
        evt.preventDefault();

        language.LanguageToolId = $("#ddl_Tools").val();
        language.Keywords = $("#txt_Keywords").val();
        language.Description = $("#txt_Description").val();
        language.Author = $("#txt_Author").val();
        language.Title = $("#txt_Title").val();

      

        sysAjax({
            url: "/Admin/SaveSeo",
            data: JSON.stringify(language),
            success: onSaveSeoSuccess,

            blockElement: "#formAddUser"
        });


    }
    function onSaveSeoSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.DisplaySuccessMessage();
            $("#formAddUser")[0].reset();

        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }


    return {

        GetPageTools: getPageToolsMethod,
        GetSeoPageTools: getSeoPageToolsMethod,
        getToolLanguageDataById: getToolLanguageDataByIdMethod,
        getSeoUrlMagerDataById: getSeoUrlMagerDataByIdMethod,
        saveSeoUrl: saveSeoUrlMethod,
        SaveSeo: saveSeoMethod,
        saveLanguage: saveLanguageMethod
    };
}();