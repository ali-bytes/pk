var CategoryApp = angular.module("CategoryApp", []);


(function (angular) {

    function categoryCtrl($scope, $http) {

        $scope.Categories = [];
        $scope.MainCategories = [];
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.NameEn = '';
        $scope.CanDelete = false;

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

        $scope.Save = function () {
            validateScript.validate($("#categoryFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                if ($scope.selectedMainCategory.Id <= 0 || $scope.selectedMainCategory.Id === undefined) {

                    alert("أختر الفئة الرئيسية");
                    return;
                }

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Title").val();
                $scope.NameEn = $("#txt_TitleEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");

                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;
                category.NameEn = $scope.NameEn;
                category.ImageUrl = $scope.ImageUrl;
                category.MainCategoryId = $scope.selectedMainCategory.Id;


                $http.post('/Product/SaveCategory', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) {
                        $scope.Name = '';
                        $scope.NameEn = '';
                        $scope.ImageUrl = '';
                        if ($scope.Id > 0) {

                            $scope.Categories = [];



                            $scope.Id = 0;

                            $scope.CanDelete = false;
                            $scope.GetAllCategories();
                        }
                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                    }
                    else {
                        commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                    }
                });


            };

        };


        $scope.GetAllCategories = function () {
            $scope.Categories = [];
            var category = { Id: 0, Name: 'أختر الفئة', ImageUrl: '', categoryId: 0, CanDelete: false };
            $scope.Categories.push(category);
            $http.get('/Product/GetAllCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Categories.push(data.ReturnedData[i]);
                }
                $scope.selectedCategory = $scope.Categories[0];

            }).error(function (data) {
                $scope.Categories = data || [];
            });

        };


        $scope.getMainCategory = function (id) {
            var match = _.filter($scope.MainCategories, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedMainCategory = $scope.MainCategories[0];

                return;
            }
            $scope.selectedMainCategory = match[0];
        };

        $scope.GetCategory = function () {
            if ($scope.selectedCategory.Id > 0) {
                $http.get('/Product/GetCategory/' + $scope.selectedCategory.Id).success(function (data) {
                    $scope.ImageUrl = data.ReturnedData.ImageUrl;
                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    //$scope.selectedMainCategory.Id = data.ReturnedData.MainCategoryId;
                    $scope.getMainCategory(data.ReturnedData.MainCategoryId);

                    $scope.CanDelete = $scope.selectedCategory.CanDelete;

                }).error(function (data) {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
                $scope.CanDelete = false;
                $scope.selectedMainCategory.Id = 0;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedCategory.Id > 0) {
                $http.post('/Product/DeleteCategory/' + $scope.selectedCategory.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllCategories();

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

    CategoryApp.controller("categoryCtrl", categoryCtrl);



})(angular);

