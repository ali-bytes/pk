var productRating = angular.module("productRating", ['ngSanitize']);

(function (angular) {




    function productRatingCtrl($scope, $http) {
        $scope.ratevalue = 5;
        $scope.ProductId = 0;

        $scope.SaveRate = function () {
            var rate = {};
            rate.Rate = $scope.ratevalue;
            rate.ProductId = $scope.ProductId;

            $http.post('/Product/SaveRating', JSON.stringify(rate)).success(function (data) {
                alert("شكرا لتقيمكم منتجنا ");

            });
        };
    }

    productRating.controller("productRatingCtrl", productRatingCtrl);



})(window.angular);

