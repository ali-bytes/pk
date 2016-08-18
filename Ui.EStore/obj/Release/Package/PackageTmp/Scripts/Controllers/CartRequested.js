var CartRequestApp = angular.module("CartRequestApp", ["ngSanitize"]);
//'angular-underscore'

(function (angular) {

    function cartRequestCtrl($scope, $http) {
        $scope.EnableShow = false;
        $scope.Deleveries = [];
        $scope.MyCart = [];
        $scope.FullCart = null;
        $scope.GetNewCarts = function () {
            $scope.MyCart = [];
            $scope.FullCart = null;
            $http.get("/Home/GetequestCartUnder").success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.MyCart.push(data.ReturnedData[i]);
                }
                $scope.selectCart = $scope.MyCart[0];

            }).error(function (data) {
                $scope.MyCart = data || [];
            });

        };

        $scope.GetNewDeleveredCarts = function () {
            $scope.MyCart = [];
            $scope.FullCart = null;
            $http.get("/Home/GetNewDeleveredCarts").success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.MyCart.push(data.ReturnedData[i]);
                }
                $scope.selectCart = $scope.MyCart[0];

            }).error(function (data) {
                $scope.MyCart = data || [];
            });

        };

        $scope.ShowCart = function (id) {
            $http.get("/Home/GetCartDetailsById/" + id).success(function (data) {
                $scope.EnableShow = true;
                $scope.FullCart = data.ReturnedData;

            }).error(function (data) {
                $scope.FullCart = data || [];
                $scope.EnableShow = false;
            });
        };

        $scope.ApprovedCart = function (id) {
            $http.post("/Home/ApprovedCart/" + id).success(function (data) {
                $scope.GetNewCarts();
                $scope.EnableShow = false;
                commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
            });
        };

        $scope.CancelCart = function (id) {
            $http.post("/Home/CancelCart/" + id).success(function (data) {
                $scope.GetNewCarts();
                $scope.EnableShow = false;
                commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
            });
        };

        $scope.DeleveriedCart = function (id) {
            $http.post("/Home/DeleveriedCart/" + id).success(function (data) {
                $scope.GetNewCarts();
                $scope.EnableShow = false;
                commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
            });
        };

        $scope.CancelDeleveriedCart = function (id) {
            $http.post("/Home/CancelDeleveriedCart/" + id).success(function (data) {
                $scope.GetNewCarts();
                $scope.EnableShow = false;
                commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
            });
        };


    }

    CartRequestApp.controller("cartRequestCtrl", cartRequestCtrl);



})(angular);

