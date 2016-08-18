var BrandApp = angular.module("BrandApp", []);


(function (angular) {


    function brandCtrl($scope, $http) {
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.NameEn = '';
        $scope.CanDelete = false;
        $scope.Brands = [];


        $scope.GetAllBrands = function () {
            $scope.Brands = [];
            var category = { Id: 0, Name: 'أختر الماركة', ImageUrl: '', CanDelete: false };
            $scope.Brands.push(category);
            $http.get('/Product/GetAllBrands').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Brands.push(data.ReturnedData[i]);
                }
                $scope.selectedBrand = $scope.Brands[0];

            }).error(function (data) {
                $scope.Brands = data || [];
            });

        };

        $scope.Save = function () {
            validateScript.validate($("#BrandFrm"));
            if ($("#divWarning").hasClass("invisible")) {



                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Title").val();
                $scope.NameEn = $("#txt_TitleEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");

                var brand = {};
                brand.Id = $scope.Id;
                brand.Name = $scope.Name;
                brand.NameEn = $scope.NameEn;
                brand.ImageUrl = $scope.ImageUrl;



                $http.post('/Product/SaveBrand', JSON.stringify(brand)).success(function (data) {
                    if (data.ClientStatusCode == 0) {

                        $scope.ImageUrl = '';
                        $scope.ImageUrl = $("#imgUserImage").attr("src", '');
                        $scope.Name = '';
                        if ($scope.Id > 0) {
                            $scope.Id = 0;

                            $scope.CanDelete = false;
                            $scope.GetAllBrands();
                        }
                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                    }
                    else {
                        commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                    }
                });


            };

        };

        $scope.Delete = function () {
            if ($scope.selectedBrand.Id > 0) {
                $http.post('/Product/DeleteBrand/' + $scope.selectedBrand.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllBrands();

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

        $scope.GetBrand = function () {
            if ($scope.selectedBrand.Id > 0) {
                $http.get('/Product/GetBrand/' + $scope.selectedBrand.Id).success(function (data) {
                    $scope.ImageUrl = data.ReturnedData.ImageUrl;
                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.CanDelete = $scope.selectedBrand.CanDelete;
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


    };

    BrandApp.controller("brandCtrl", brandCtrl);



})(angular);

