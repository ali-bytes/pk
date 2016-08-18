var adminScript = function () {

    var user = { Id: 0, RuleId: 0, UserName: '', Email: '', Password: '' ,  IsActive: null };
       var role = { SYS_RuleID: 0, SYS_RuleName: '', SYS_RuleNameEN: '', SYS_RuleIsActive: null };
       var faq = { FAQId: 0, FAQTitle: '', FAQDetails: '', FAQTitleEn: '', FAQDetailsEn: '' };


       function validateUserMethod() {


           user.Email = $("#txt_SYS_UserEmailAddress").val();
           user.Password = $("#txt_SYS_UserPassword").val();
           sysAjax({
               url: "/Admin/ValidateUser",
               blockElement: '#formLogin',
               success: onValidateUserSuccess,
               data: JSON.stringify(user)
           });

       }

       function onValidateUserSuccess(result) {
           if (result.ClientStatusCode == 0)
               window.location = "/Admin";
           else {
               $("#divWarning").text(result.ClientMessageContent[0]);
               if ($("#divWarning").hasClass("invisible"))
                   $("#divWarning").removeClass("invisible");
           }
       }
    
    function getRoleDetailsMethod() {
        var roleId = $("#ddl_Roles").val();
        sysAjax({
            url: "/Admin/GetRoleDetailsByRoleId?roleId=" + roleId,
            success: ongetRoleDetailsSuccess
        });
    }



    function ongetRoleDetailsSuccess(result) {
        if (result.ClientStatusCode == 0) {
            $("#tb_Roles > tbody").each(function () {
                $(this).remove();
            });
            $("#tb_Roles").append("<tbody></tbody>");
            $.each(result.ReturnedData, function (index, Element) {
                var checked = Element.SysPrivilageState == true ? 'checked' : '';
                var strHtml = "";
                strHtml += "<tr>";
                strHtml += "<td><input type='checkbox' " + checked + " id='" + Element.SysPrivilegeId + "'></input></td>";
                strHtml += "<td>" + Element.SysPrivilegeDisplayName + "</td>";
                strHtml += "<td>" + Element.SysPrivilegeCategoryName + "</td>";
                strHtml += "<td>" + Element.SysControllerId + "</td>";
                strHtml += "<td>" + Element.SysActionId + "</td>";
              
                strHtml += "</tr>";
                $("#tb_Roles > tbody").append(strHtml);
            }
            );

        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    function saveRoleDetailsMethod() {
        var roles = [];
        $('input:checkbox').each(function () {
            var role = {};
            role.SysPrivilageState = ($(this).prop('checked') == true ? true : false);
            role.SysPrivilegeId = $(this).prop("id");
            role.SysRoleId = $("#ddl_Roles").val();
            roles.push(role);
        });


        sysAjax({
            url: "/Admin/SaveRoleDetails",
            success: onSaveRoleDetailsSuccess,
            data: JSON.stringify(roles),
        });
    }

    function onSaveRoleDetailsSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.displayTextSuccessMessage(result.ClientMessageContent[0]);
        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    
    
    function addRoleToDbMethod(evt) {
        evt.preventDefault();
        role.SYS_RuleName = $("#txt_EMK_CategoryName").val();
        role.SYS_RuleNameEN = $("#txt_EMK_CategoryNameEN").val();

        sysAjax({
            url: "/Admin/AddRoleToDb",
            blockElement: '#formAddCategory',
            success: onaddRoleToDbSuccess,
            data: JSON.stringify(role)
        });
    }
    
    function onaddRoleToDbSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.displayTextSuccessMessage();
        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }
    


    function getRoleMethod() {

        var roleId = $("#ddl_category").val();
        sysAjax({
            url: "/Admin/GetRole?role=" + roleId,
            success: onGetRoleSuccess,
            blockElement: "#formEditCategory"
        }
        );
    }
    function onGetRoleSuccess(result) {
        if (result.ClientStatusCode == 0) {
            $("#txt_EMK_CategoryName").val(result.ReturnedData[0].SYS_RuleName);
            $("#txt_EMK_CategoryNameEN").val(result.ReturnedData[0].SYS_RuleNameEN);
            if (result.ReturnedData[0].canDelete == true) {
                $("#btnDeleteCategory").hide();
            } else {
                $("#btnDeleteCategory").show();
            }

            if (result.ReturnedData[0].SYS_RuleIsActive)
                $("#chb_EMK_CategoryIsActive").prop("checked", "checked");
            else
                $("#chb_EMK_CategoryIsActive").prop("checked", "");
        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    function editRoleToDbMethod(evt) {
        evt.preventDefault();
        role.SYS_RuleID = $("#ddl_category").val();
        role.SYS_RuleName = $("#txt_EMK_CategoryName").val();
        role.SYS_RuleNameEN = $("#txt_EMK_CategoryNameEN").val();
        role.SYS_RuleIsActive = ($("#chb_EMK_CategoryIsActive").is(":checked"));

        sysAjax({
            url: "/Admin/EditRoleToDb",
            blockElement: '#formEditCategory',
            success: onEditRoleToDbSuccess,
            data: JSON.stringify(role)
        });
    }
    function onEditRoleToDbSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.displayTextSuccessMessage(result.ClientMessageContent[0]);
            $("#formEditCategory")[0].reset();
            $("#div_categoryDetails").hide();
        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }
    
    function deleteRoleToDbMethod(evt) {
        evt.preventDefault();
        var isdeleted = confirm("هل تريد الحذف ؟");
        if (isdeleted) {
            role.SYS_RuleID = $("#ddl_category").val();
            role.SYS_RuleName = $("#txt_EMK_CategoryName").val();
            role.SYS_RuleNameEN = $("#txt_EMK_CategoryNameEN").val();


            sysAjax({
                url: "/Admin/DeleteRoleToDb",
                blockElement: '#formEditCategory',
                success: ondeleteRoleToDbSuccess,
                data: JSON.stringify(role)
            });
        }

    }
    function ondeleteRoleToDbSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.displayTextSuccessMessage();
            $("#formEditCategory")[0].reset();
            $("#div_categoryDetails").hide();
            location.reload();

        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }
    

    /****************************** FAQS ***********************************/
    function AddFAQToDbMethod(evt) {

        evt.preventDefault();

        faq.FAQTitle = $("#txt_FileName").val();
        faq.FAQTitleEn = $("#txt_FileNameEn").val();
        
        faq.FAQDetails = $("#txt_FileDetails").val();
        faq.FAQDetailsEn = $("#txt_FileDetailsEn").val();
   



        sysAjax({
            url: "/Admin/AddFAQToDb",
            blockElement: '#formAddUser',
            success: onAddFAQToDbSuccess,
            data: JSON.stringify(faq)
        });
    }

    function onAddFAQToDbSuccess(result) {
        if (result.ClientStatusCode == 0) {
            $("#formAddUser")[0].reset();
            commonViewModel.DisplaySuccessMessage();
        }
        else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    function getAllFAQSMethod() {

        sysAjax({
            url: "/Admin/GetAllFAQs/",
            success: onGetAllFAQSSuccess,
        });

    }

    function onGetAllFAQSSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.fillSelect("#ddl_Diploma", result);
        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    function getGetFAQMethod() {

        var SYS_UserID = $("#ddl_Diploma").val();
        sysAjax({
            url: "/Admin/GetFAQ?faqId=" + SYS_UserID,
            success: onGetFAQSuccess

        }
        );
    }

    function onGetFAQSuccess(result) {
        if (result.ClientStatusCode == 0) {

            $("#txt_FileName").val(result.ReturnedData[0].FAQTitle);
            
            $("#txt_FileDetails").val(result.ReturnedData[0].FAQDetails);
            $("#txt_FileNameEn").val(result.ReturnedData[0].FAQTitleEn);

            $("#txt_FileDetailsEn").val(result.ReturnedData[0].FAQDetailsEn);


        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    function editEditFAQToDbMethod(evt) {

        evt.preventDefault();
        faq.FAQId = $("#ddl_Diploma").val();
        faq.FAQTitle = $("#txt_FileName").val();
       
        faq.FAQDetails = $("#txt_FileDetails").val();
        faq.FAQTitleEn = $("#txt_FileNameEn").val();

        faq.FAQDetailsEn = $("#txt_FileDetailsEn").val();





        sysAjax({
            url: "/Admin/EditFAQToDb",
            data: JSON.stringify(faq),
            success: onEditFAQToDbSuccess,
            error: onEditFAQToDbError,
            blockElement: "#formEditUser"
        }
        );
    }

    function onEditFAQToDbSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.DisplaySuccessMessage(result.ClientMessageContent[0]);
            location.reload();
            $("#formEditUser")[0].reset();
            $("#div_userDetails").hide();

        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    function onEditFAQToDbError(result) {
        if ($.cookie("_culture") == "ar-EG")
            commonViewModel.DisplayErrorTextMessage('حدث خطأ اثناء الحفظ');
        else
            commonViewModel.DisplayErrorTextMessage('Error');

    }

    function DeleteFAQToDbMethod(evt) {
        evt.preventDefault();
        var isdeleted = confirm("هل تريد الحذف ؟");
        if (isdeleted) {
            faq.FAQId = $("#ddl_Diploma").val();
            faq.FAQTitle = $("#txt_FileName").val();
      
            faq.FAQDetails = $("#txt_FileDetails").val();
            faq.FAQTitleEn = $("#txt_FileNameEn").val();

            faq.FAQDetailsEn = $("#txt_FileDetailsEn").val();


            sysAjax({
                url: "/Admin/DeleteFAQToDb",
                blockElement: '#formEditUser',
                success: onDeleteFAQToDbSuccess,
                data: JSON.stringify(faq)
            });
        }

    }
    function onDeleteFAQToDbSuccess(result) {
        if (result.ClientStatusCode == 0) {
            commonViewModel.DisplaySuccessMessage();
            $("#formEditUser")[0].reset();
            $("#div_userDetails").hide();
            location.reload();

        } else {
            commonViewModel.DisplayErrorTextMessage(result.ClientMessageContent[0]);
        }
    }

    return {
        validateUser: validateUserMethod,
        getRoleDetails: getRoleDetailsMethod,
        saveRoleDetails: saveRoleDetailsMethod,
        addRoleToDb: addRoleToDbMethod,
        getRole: getRoleMethod,
        editRoleToDb: editRoleToDbMethod,
        deleteRoleToDb: deleteRoleToDbMethod,
        AddFAQToDb: AddFAQToDbMethod,
        getAllFAQS: getAllFAQSMethod,
        getGetFAQ: getGetFAQMethod,
        editEditFAQToDb: editEditFAQToDbMethod,
        DeleteFAQToDb: DeleteFAQToDbMethod
    };
}();