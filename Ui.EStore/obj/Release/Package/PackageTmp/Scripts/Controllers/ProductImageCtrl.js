var ProductImageApp = angular.module("ProductImageApp", []);


(function (angular) {

    function productimageCtrl($scope, $http) {
        $scope.ProductImages = [];
        $scope.Products = [];
        $scope.ImageUrl = '';
        $scope.Berif = '';
        $scope.totalImages = 0;
        $scope.PageNum = 1;
        $scope.GetAllProducts = function () {

            $scope.Products = [];
            var category = { Id: 0, Name: 'أختر المنتج' };
            $scope.Products.push(category);
            $http.get('/Product/GetAllproducts').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Products.push(data.ReturnedData[i]);
                }
                $scope.selectedProduct = $scope.Products[0];

            }).error(function (data) {
                $scope.Products = data || [];
            });
        };

        $scope.Save = function () {
            validateScript.validate($("#ProductFrm"));
            if ($("#divWarning").hasClass("invisible")) {


                //$scope.Id = $("#txt_Berif").val();

                $scope.ImageUrl = $("#imgUserImage").attr("src");

                $scope.Berif = $("#txt_Berif").val();;



                var productimage = {};
                productimage.Id = $scope.Id;

                productimage.ImageUrl = $scope.ImageUrl;
                productimage.ProductId = $scope.selectedProduct.Id;
                productimage.Berif = $scope.Berif;

                $http.post('/Product/SaveImages', JSON.stringify(productimage)).success(function (data) {
                    $scope.ImageUrl = '';
                    $scope.Id = 0;

                    $scope.Berif = '';

                    if ($scope.Id > 0) {


                        $scope.GetAllProductImagesByProductId($scope.PageNum);
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                });


            };

        };

        $scope.GetAllProductImages = function () {

            $scope.ProductImages = [];
            var category = { Id: 0, Name: 'أختر الصورة' };
            $scope.ProductImages.push(category);
            $http.get('/Product/GetAllproductImages').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.ProductImages.push(data.ReturnedData[i]);
                }
                $scope.selectedProductImage = $scope.ProductImages[0];

            }).error(function (data) {
                $scope.ProductImages = data || [];
            });
        };

        $scope.GetAllProductImagesByProductId = function (id) {
            $scope.PageNum = id;
            $scope.ProductImages = [];
            var pager = {};
            pager.Id = $scope.selectedProduct.Id;
            pager.PageNum = id;
            $http.post('/Product/GetAllProductImagesByProductIdPage', JSON.stringify(pager)).success(function (data) {

                $scope.totalImages = data.ReturnedData.length;
                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.ProductImages.push(data.ReturnedData[i]);

                }
                $scope.selectedProductImage = $scope.ProductImages[0];

            }).error(function (data) {
                $scope.ProductImages = data || [];
            });
        };

        $scope.GetProductImage = function (id) {
            if (id>0) {
                
           
            $http.get('/Product/GetProductImage/' + id ).success(function (data) {

                $scope.ImageUrl = data.ReturnedData.ImageUrl;

                $scope.Id = data.ReturnedData.Id;

                $scope.Berif = data.ReturnedData.Berif;


            }).error(function (data) {

            });

        } else {
            $scope.ImageUrl = '';
            $scope.Id = 0;

            $scope.Berif = '';


            $scope.selectedProduct.Id = 0;
        }
    };

    $scope.Delete = function () {
        if ($scope.selectedProductImage.Id > 0) {
            $http.post('/Product/DeleteProductImage/' + $scope.selectedProductImage.Id).success(function (data) {
                commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                $scope.ImageUrl = '';
                $scope.Id = 0;

                $scope.Berif = '';

                $scope.GetAllProductImagesByProductId();

            }).error(function (data) {
                commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

            });
        }

    };

}

    ProductImageApp.controller("productimageCtrl", productimageCtrl);



})(angular);

