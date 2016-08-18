
var LestCategoriesApp = angular.module("LestCategoriesApp", ['ngSanitize']);

(function (angular) {


    LestCategoriesApp.directive('wholeNumber', function () {
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


    function lestCategoryCtrl($scope, $http) {
        $scope.MainCategories = [];
        $scope.Categories = [];
     
        $scope.LestCategories = [];
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.NameEn = '';
       
       
        $scope.GetFullCategories = function () {
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
        $scope.CategoryLestCategories = [];
       


        //  $scope.fullstring =JSON.parse($scope.isString);
        $scope.GetAllMainCategories = function () {
            $scope.MainCategories = [];
            $scope.Categories = [];


            $http.get('/Product/GetAllMainCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.MainCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedMainCategory = $scope.MainCategories[0];

            }).error(function (data) {
                $scope.MainCategories = data || [];
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

        $scope.getCategory = function (id) {
            var match = _.filter($scope.Categories, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedCategory = $scope.Categories[0];

                return;
            }
            $scope.selectedCategory = match[0];
        };

     

        $scope.GetAllCategories = function () {
            $scope.Categories = [];
            var category = { Id: 0, Name: 'أختر الفئة', ImageUrl: '', MainCategoryId: 0, CanDelete: false };
            $scope.Categories.push(category);
            $http.get('/Product/GetCategoryByMainId/' + $scope.selectedMainCategory.Id).success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Categories.push(data.ReturnedData[i]);
                }
                $scope.selectedCategory = $scope.Categories[0];

            }).error(function (data) {
                $scope.Categories = data || [];
            });

        };

  

        $scope.Save = function () {
            validateScript.validate($("#LestCategoryFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                if ($scope.selectedMainCategory.Id <= 0 || $scope.selectedMainCategory.Id === undefined) {

                    alert("أختر الفئة الرئيسية");
                    return;
                }

                if ($scope.selectedCategory.Id <= 0 || $scope.selectedCategory.Id === undefined) {

                    alert("أختر الفئة الفئة الفرعية");
                    return;
                }
              


                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Name").val();
                $scope.NameEn = $("#txt_NameEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");
              
                var product = {};
                product.Id = $scope.Id;
                product.Name = $scope.Name;
                product.NameEn = $scope.NameEn;
                product.ImageUrl = $scope.ImageUrl;
              
                product.MainCategoryId = $scope.selectedMainCategory.Id;
                product.CategoryId = $scope.selectedCategory.Id;
               

                $http.post('/Product/SaveLestCategory', JSON.stringify(product)).success(function (data) {
                    $scope.ImageUrl = '';
                 
                   
                  
                  
                    if ($scope.Id > 0) {
  $scope.CanDelete = false;   $scope.Id = 0;
                        $scope.LestCategories = [];
                        $scope.GetAllLestCategories();
                    }
                    $scope.Name = '';
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                });


            };

        };

    

        $scope.GetAllLestCategories = function () {

            $scope.LestCategories = [];
            var category = { Id: 0, Name: 'أختر الفئة الصغرى', ImageUrl: '', MainCategoryId: 0, CanDelete: false };
            $scope.LestCategories.push(category);
            $http.get('/Product/GetAllLestCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.LestCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedLestCategory = $scope.LestCategories[0];

            }).error(function (data) {
                $scope.LestCategories = data || [];
            });
        };

        $scope.GetLestCategory = function () {
            if ($scope.selectedLestCategory.Id > 0) {
                $http.get('/Product/GetLestCategory/' + $scope.selectedLestCategory.Id).success(function (data) {

                    $scope.ImageUrl = data.ReturnedData.ImageUrl;

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                   
                  

                    $scope.getMainCategory(data.ReturnedData.MainCategoryId);

                    $scope.getCategory(data.ReturnedData.CategoryId);
                   

                    $scope.CanDelete = $scope.selectedLestCategory.CanDelete;

                }).error(function (data) {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
              
              
                $scope.selectedMainCategory.Id = 0;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedLestCategory.Id > 0) {
                $http.post('/Product/DeleteLestCategory/' + $scope.selectedLestCategory.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.NameEn = '';
                   
                    $scope.GetAllLestCategories();

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };



    }

    LestCategoriesApp.controller("lestCategoryCtrl", lestCategoryCtrl);



})(window.angular);

