(function (angular) {
    var NewApp = angular.module("NewApp", []);

    NewApp.directive('wholeNumber', function () {
        return {
            link: function (scope, elem) {
                var num = (parseFloat(elem.val(), 10));
                decimalOnly(elem, num);
                elem.on("blur", function () {
                    num = num > (scope.maxValue) || (isNaN(num)) ? 0 : num;
                    return num;
                });
                elem.on("change", function () {
                    num = num > (scope.maxValue) || (isNaN(num)) ? 0 : num;
                    return num;

                });
            }
        };
    });



    function productCtrl($scope, $http) {
        $scope.News = [];
     
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Title = '';
      
        $scope.Berif = '';
        $scope.Details = '';
     
        $scope.TitleEn = '';

        $scope.BerifEn = '';
        $scope.DetailsEn = '';
        
        $scope.Save = function () {
            validateScript.validate($("#productCtrl"));
            if ($("#divWarning").hasClass("invisible")) {

              

                //$scope.Id = $("#txt_Berif").val();
                $scope.Title = $("#txt_Name").val();
                $scope.TitleEn = $("#txt_NameEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");
                $scope.Berif = $("#txt_Berif").val();
                $scope.Details = $("#txt_Details").val();
                $scope.BerifEn = $("#txt_BerifEn").val();
                $scope.DetailsEn = $("#txt_DetailsEn").val();


                var product = {};
                product.Id = $scope.Id;
                product.Title = $scope.Title;
                product.TitleEn = $scope.TitleEn;
                product.ImageUrl = $scope.ImageUrl;
            
                product.Berif = $scope.Berif;
                product.Details = $scope.Details;
                product.BerifEn = $scope.BerifEn;
                product.DetailsEn = $scope.DetailsEn;
                
                $http.post('/News/SaveSysNew', JSON.stringify(product)).success(function (data) {
                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Title = '';
                    $scope.TitleEn = '';
                
                    $scope.Berif = '';
                    $scope.Details = '';
                    
                    $scope.BerifEn = '';
                    $scope.DetailsEn = '';
                    
                    if ($scope.Id > 0) {


                        $scope.GetAllNews();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                });


            };

        };

        $scope.GetAllNews = function () {

            $scope.News = [];
            var category = { Id: 0, Title: 'أختر الخبر', ImageUrl: '' };
            $scope.News.push(category);
            $http.get('/News/GetAllSysNews').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.News.push(data.ReturnedData[i]);
                }
                $scope.selectedNew = $scope.News[0];

            }).error(function (data) {
                $scope.News = data || [];
            });
        };

        $scope.GetNew = function () {
            if ($scope.selectedNew.Id > 0) {
                $http.get('/News/GetSysNew/' + $scope.selectedNew.Id).success(function (data) {

                    $scope.ImageUrl = data.ReturnedData.ImageUrl;

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Title = data.ReturnedData.Title;
                    $scope.TitleEn = data.ReturnedData.TitleEn;
                
                    $scope.Berif = data.ReturnedData.Berif;
                    $scope.Details = data.ReturnedData.Details;
                    $scope.BerifEn = data.ReturnedData.BerifEn;
                    $scope.DetailsEn = data.ReturnedData.DetailsEn;
                  

                }).error(function (data) {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Title = '';
              
                $scope.Berif = '';
                $scope.Details = '';
                $scope.TitleEn = '';

                $scope.BerifEn = '';
                $scope.DetailsEn = '';
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedNew.Id > 0) {
                $http.post('/News/DeleteSysNew/' + $scope.selectedNew.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Title = '';
                   
                    $scope.Berif = '';
                    $scope.Details = '';
                 
                    $scope.TitleEn = '';

                    $scope.BerifEn = '';
                    $scope.DetailsEn = '';
                    
                    $scope.GetAllNews();

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };


    }

    NewApp.controller("productCtrl", productCtrl);



})(window.angular);

