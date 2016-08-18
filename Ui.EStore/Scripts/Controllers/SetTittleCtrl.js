var aboutUs = angular.module("aboutUs", ['ngSanitize']);


(function (angular) {

    function aboutUsCtrl($scope, $http) {
      
        $scope.ImageUrl = '';
        $scope.title = '';
        $scope.style = '';

        $scope.GetAboutUs = function () {

            $http.get('/Admin/GetTitle').success(function (data) {

                if (data.ClientStatusCode == 0) {
                    $scope.id = data.ReturnedData.Id;
                    $scope.title = data.ReturnedData.Title;
                  
                    $scope.ImageUrl = data.ReturnedData.ImageUrl;
                    $scope.style = "background: #d7d6d8 url(" + $scope.ImageUrl + ") right no-repeat;";
                }
            }).error(function (data, status) {


            });

        };
        $scope.Save = function () {

            validateScript.validate($("#aboutUsFrm"));
            if ($("#divWarning").hasClass("invisible")) {

               
                $scope.ImageUrl = $("#imgUserImage").attr("src");

                var aboutus = {};
                aboutus.Id = $scope.id;
                aboutus.Title = $scope.title;
              
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

