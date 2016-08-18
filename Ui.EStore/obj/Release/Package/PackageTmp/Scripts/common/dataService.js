
// we 'define' undefined to alleviate concerns
// that someone may have done something stupid
// like this in code before this code executes:
// undefined = "Im now defined.";
// credit: Ben Alman (see comments)
var sysAjax = (function ($, undefined) {
    return (
      function (params) {

          // use extend to merge our defaults with parameters
          // passed by function caller
          var settings = $.extend({
              cache: false,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              Accept: 'application/json',
              type: "POST",
              data: "{}",
              url: "",
              spinner: {},
              blockElement: {},
              // use empty object if version 1.3.2-
              // credit: Ben Alman (see comments)
              success: function () { },
              errorMsg: "Oops. Sorry about that."
              // credit: rmurphey (see comment below)
          }, params),
              retries = 0; // setting up retries variable
          // setting up a function that we can call recursively
          // to retry ajax calls 
          function ajaxRequest() {
              $.ajax({
                  beforeSend: function () {
                      if (!jQuery.isEmptyObject(settings.spinner)) {
                          jQuery(settings.spinner).show();
                      }
                      if (!jQuery.isEmptyObject(settings.blockElement)) {
                          jQuery(settings.blockElement).block({ message: '<div style="padding:5px"><img src="/Images/loading.gif"/>Processing...</div>' });
                      }

                  },
                  cache: settings.cache,
                  url: settings.url,
                  type: settings.type,
                  dataType: settings.dataType,
                  contentType: settings.contentType,
                  data: settings.data,
                  success: settings.success,
                  complete: function () {
                      if (!jQuery.isEmptyObject(settings.spinner)) {
                          jQuery(settings.spinner).hide();
                      }
                      if (!jQuery.isEmptyObject(settings.blockElement)) {
                          jQuery(settings.blockElement).unblock();
                      }

                  },

                  error: function (xhr, status, error) {
                      var errorMessage = error || xhr.statusText;
                      if (xhr.status == 403) {
                          alert("Sorry, your session has expired. Please login again to continue");
                          window.location.href = "/Login.aspx";
                      }
                      if (xhr.status == 0) {
                          commonViewModel.DisplayErrorTextMessage('You are offline!!! Please Check Your Network.');
                      } else if (xhr.status == 404) {
                          commonViewModel.DisplayErrorTextMessage('404 Requested not found.');
                      } else if (xhr.status == 500) {
                          commonViewModel.DisplayErrorTextMessage('500 Internel Server Error.');
                      } else if (status == 'parsererror') {
                          commonViewModel.DisplayErrorTextMessage('Error Parsing JSON Request failed.');
                      } else if (status == 'timeout') {
                          commonViewModel.DisplayErrorTextMessage('Request Time out.');
                      } else {
                          commonViewModel.DisplayErrorTextMessage(errorMessage);
                      }
                      if (!jQuery.isEmptyObject(settings.spinner)) {
                          jQuery(settings.spinner).hide();
                      }
                      if (!jQuery.isEmptyObject(settings.blockElement)) {
                          jQuery(settings.blockElement).unblock();
                      }
                  }
              }); // end $.ajax()
          }; // end ajaxRequest() 

          // call our ajax request function. notice above
          // that we only define the function. here we make
          // the original call.
          ajaxRequest();

      } // end getViaAjax()
    ); // end return statement
})(jQuery);


