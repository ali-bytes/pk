var aboutUs = angular.module("aboutUs", ['ngSanitize']);


(function (angular) {

    function aboutUsCtrl($scope, $http) {
        $scope.berif = '';
        $scope.details = '';
        $scope.berifEn = '';
        $scope.detailsEn = '';
        $scope.titleEn = '';
        $scope.ImageUrl = '';
        $scope.title = '';
        $scope.style = '';

        $scope.GetAboutUs = function () {
            
            $http.get('/Admin/GetAboutUs').success(function (data) {
                
                if (data.ClientStatusCode == 0) {
                    $scope.id = data.ReturnedData.Id;
                    $scope.title = data.ReturnedData.Title;
                    $scope.berif = data.ReturnedData.Berif;
                    $scope.details = data.ReturnedData.Details;
                    $scope.titleEn = data.ReturnedData.TitleEn;
                    $scope.berifEn = data.ReturnedData.BerifEn;
                    $scope.detailsEn = data.ReturnedData.DetailsEn;
                    $scope.ImageUrl = data.ReturnedData.ImageUrl;
                    $scope.style = "background: #d7d6d8 url(" + $scope.ImageUrl + ") right no-repeat;";
                }
            }).error(function (data, status) {


            });

        };
        $scope.Save = function () {

            validateScript.validate($("#aboutUsFrm"));
            if ($("#divWarning").hasClass("invisible")) {
                
                $scope.berif = $("#txt_Berif").val();
                $scope.details = $("#txt_Details").val();
                $scope.berifEn = $("#txt_BerifEn").val();
                $scope.detailsEn = $("#txt_DetailsEn").val();
                $scope.ImageUrl =$("#imgUserImage").attr("src");

                var aboutus = {};
                aboutus.Id = $scope.id;
                aboutus.Title = $scope.title;
                aboutus.Berif = $scope.berif;
                aboutus.Details = $scope.details;
                aboutus.TitleEn = $scope.titleEn;
                aboutus.BerifEn = $scope.berifEn;
                aboutus.DetailsEn = $scope.detailsEn;
                aboutus.ImageUrl = $scope.ImageUrl;

                $http.post('/Admin/SaveAboutUs', JSON.stringify(aboutus)).success(function (data) {
                   
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                   
                });
            }

        };
        
        $scope.GetAboutUs();



    };

    aboutUs.controller("aboutUsCtrl", aboutUsCtrl);
    
  })(angular);

