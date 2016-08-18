var SliderApp = angular.module("SliderApp", ['ngSanitize']);
(function (angular) {
  

    SliderApp.directive('wholeNumber', function () {
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


    function sliderCtrl($scope, $http) {
        $scope.Sliderimages = [];
        $scope.Sliders = [];
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.NameEn = '';
     
        $scope.Berif = '';
        $scope.BerifEn = '';
        $scope.Details = '';
        $scope.DetailsEn = '';
        $scope.SalePrice = 0;
     

        $scope.Save = function () {
            validateScript.validate($("#ProductFrm"));
            if ($("#divWarning").hasClass("invisible")) {

               

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Name").val();
                $scope.NameEn = $("#txt_NameEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");
              
                $scope.Berif = $("#txt_Berif").val();
                $scope.BerifEn = $("#txt_BerifEn").val();
                $scope.Details = $("#txt_Details").val();
                $scope.DetailsEn = $("#txt_DetailsEn").val();
                $scope.SalePrice = $("#txt_Sale").val();
               


                var product = {};
                product.Id = $scope.Id;
                product.Name = $scope.Name;
                product.NameEn = $scope.NameEn;
                product.ImageUrl = $scope.ImageUrl;
          
                product.Berif = $scope.Berif;
                product.BerifEn = $scope.BerifEn;
                product.Details = $scope.Details;
                product.DetailsEn = $scope.DetailsEn;
                product.SalePrice = $scope.SalePrice;
            
                $http.post('/Home/SaveSliderImage', JSON.stringify(product)).success(function (data) {
                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.NameEn = '';
                    $scope.ImageUrl = $("#imgUserImage").attr("src", '');
                    $scope.Berif = '';
                    $scope.BerifEn = '';
                    $scope.Details = '';
                    $scope.DetailsEn = '';
                    $scope.SalePrice = 0;
                 
                    if ($scope.Id > 0) {


                        $scope.GetAllSliders();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                });


            };

        };

        $scope.GetAllSliders = function () {

            $scope.Sliders = [];
            var category = { Id: 0, Name: 'أختر الشريحة',CanDelete:false, ImageUrl: '' };
            $scope.Sliders.push(category);
            $http.get('/Home/GetAllSliderImages').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Sliders.push(data.ReturnedData[i]);
                }
                $scope.selectedSlider = $scope.Sliders[0];

            }).error(function (data) {
                $scope.Sliders = data || [];
            });
        };

      //  $scope.GetAllSliders();
        $scope.GetSlider = function () {
            if ($scope.selectedSlider.Id > 0) {
                $http.get('/Home/GetSliderImage/' + $scope.selectedSlider.Id).success(function (data) {

                    $scope.ImageUrl = data.ReturnedData.ImageUrl;

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                   
                    $scope.Berif = data.ReturnedData.Berif;
                    $scope.BerifEn = data.ReturnedData.BerifEn;
                    $scope.Details = data.ReturnedData.Details;
                    $scope.DetailsEn = data.ReturnedData.DetailsEn;
                    $scope.SalePrice = data.ReturnedData.SalePrice;
                 

                    $scope.CanDelete = $scope.selectedSlider.CanDelete;

                }).error(function (data) {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
              
                $scope.Berif = '';
                $scope.BerifEn = '';
                $scope.Details = '';
                $scope.DetailsEn = '';
                $scope.SalePrice = 0;
          
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedSlider.Id > 0) {
                $http.post('/Home/DeleteSliderImage/' + $scope.selectedSlider.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.NameEn = '';
                 
                    $scope.Berif = '';
                    $scope.BerifEn = '';
                    $scope.Details = '';
                    $scope.DetailsEn = '';
                    $scope.SalePrice = 0;
                           

                    $scope.GetAllSliders();

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };

        $scope.GetSliderImage = function () {

            $scope.Sliderimages = [];
          
            $http.get('/Home/GetAllSliderImages').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Sliderimages.push(data.ReturnedData[i]);
                }
                $scope.selectedSliderimage = $scope.Sliderimages[0];

            }).error(function (data) {
                $scope.Sliderimages = data || [];
            });
        };
        

    }

    SliderApp.controller("sliderCtrl", sliderCtrl);



})(window.angular);

