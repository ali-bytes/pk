var CategoryApp = angular.module("CategoryApp", []);


(function (angular) {

    function categoryCtrl($scope, $http) {

        $scope.MainCategories = [];
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.NameEn = '';
        $scope.CanDelete = false;

        $scope.Save = function () {
            validateScript.validate($("#categoryFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Title").val();
                $scope.NameEn = $("#txt_TitleEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");

                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;
                category.NameEn = $scope.NameEn;
                category.ImageUrl = $scope.ImageUrl;

                $http.post('/Product/SaveMainCategory', JSON.stringify(category)).success(function(data) {
                    if (data.ClientStatusCode == 0) {


                        $scope.Name = '';
                        $scope.NameEn = '';
                        $scope.ImageUrl = '';
                        $scope.ImageUrl = $("#imgUserImage").attr("src", '');
                        if ($scope.Id > 0) {

                            $scope.MainCategories = [];


                            $scope.Id = 0;

                            $scope.CanDelete = false;
                            $scope.GetAllMainCategories();
                        }

                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                    }
                    else {
                        commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                    }
                });

              
            };

        };



        $scope.GetAllMainCategories = function () {
            $scope.MainCategories = [];
            var category = { Id: 0, Name: 'أختر الفئة', ImageUrl: '', CanDelete: false };
            $scope.MainCategories.push(category);
            $http.get('/Product/GetAllMainCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.MainCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedMainCategory = $scope.MainCategories[0];

            }).error(function (data) {
                $scope.MainCategories = data || [];
            });

        };

        $scope.GetMainCategory = function () {
            if ($scope.selectedMainCategory.Id > 0) {
                $http.get('/Product/GetMainCategory/' + $scope.selectedMainCategory.Id).success(function (data) {
                    $scope.ImageUrl = data.ReturnedData.ImageUrl;
                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.CanDelete = $scope.selectedMainCategory.CanDelete;
                }).error(function (data) {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
                $scope.CanDelete = false;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedMainCategory.Id > 0) {
                $http.post('/Product/DeleteMainCategory/' + $scope.selectedMainCategory.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllMainCategories();

                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.NameEn = '';
                    $scope.CanDelete = false;

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };
    }
    CategoryApp.controller("CategoryCtrl", categoryCtrl);



})(angular);

