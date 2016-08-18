var EmailApp = angular.module("EmailApp", []);


(function (angular) {

    function emailCtrl($scope, $http) {

        $scope.emails = [];
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.CanDelete = false;

        $scope.Save = function () {
            validateScript.validate($("#EmailFrm"));
            if ($("#divWarning").hasClass("invisible")) {


                $scope.Name = $("#txt_Title").val();


                var category = {};
                category.Id = $scope.Id;
                category.Email = $scope.Name;


                $http.post('/Home/SaveEmail', JSON.stringify(category)).success(function (data) {


                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.Result = data.ClientMessageContent[0];

                });


            };

        };



        $scope.GetAllEmails = function () {
            $scope.MainCategories = [];

            $http.get('/Home/GetAllEmails').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.MainCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedMainCategory = $scope.MainCategories[0];

            }).error(function (data) {
                $scope.MainCategories = data || [];
            });

        };

        $scope.GetEmail = function () {
            if ($scope.selectedMainCategory.Id > 0) {
                $http.get('/Home/GetEmail/' + $scope.selectedMainCategory.Id).success(function (data) {
                    $scope.ImageUrl = data.ReturnedData.ImageUrl;
                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.CanDelete = $scope.selectedMainCategory.CanDelete;
                }).error(function (data) {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Name = '';
                $scope.CanDelete = false;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedMainCategory.Id > 0) {
                $http.post('/Home/DeleteEmail/' + $scope.selectedMainCategory.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllEmails();

                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.CanDelete = false;

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };
    }
    EmailApp.controller("emailCtrl", emailCtrl);



})(angular);

