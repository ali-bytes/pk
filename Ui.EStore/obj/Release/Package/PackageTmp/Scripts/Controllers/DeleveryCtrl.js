var DeleveyApp = angular.module("DeleveyApp", ['ngSanitize']);
//'angular-underscore'

(function (angular) {

 

    function deleveyCtrl($scope, $http) {
        $scope.totalItems = 0;

        $scope.Complite = false;
        $scope.Deleveries = [];
        $scope.MyCart = [];
        $scope.enableSave = true;
        $scope.Cost = 0,
        $scope.Id = 0;
        $scope.Name = '';
        $scope.NameEn = '';
        $scope.CanDelete = false;
        $scope.ShowDelevery = function () {
            if ($scope.MyCart === undefined || $scope.MyCart.MyCartDetailse === undefined || $scope.MyCart.MyCartDetailse.length === 0) {
                alert("أختر  المنتجات");
                return;
            }
            if ($scope.enableSave === false) {
                alert("تاكد من البيانات ");
                return;
            }
            $scope.Complite = true;
        };

        $scope.Save = function () {
            validateScript.validate($("#categoryFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Title").val();


                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;
                category.NameEn = $scope.NameEn;
                category.Cost = $scope.Cost;


                $http.post('/Product/SaveDelevery', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) {
                        $scope.Name = '';
                        $scope.NameEn = '';
                        $scope.Cost = '';
                        if ($scope.Id > 0) {

                            $scope.Deleveries = [];
                            $scope.Id = 0;

                            $scope.CanDelete = false;
                            $scope.GetAllDeleveries();
                        }
                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                    }
                    else {
                        commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                    }
                });


            };

        };

        $scope.GetAllDeleveries = function () {
            $scope.Deleveries = [];
            var category = { Id: 0, Name: 'أختر طريقة الشحن', Cost: 0, CanDelete: false };
            $scope.Deleveries.push(category);
            $http.get('/Product/GetAllDeleveries').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Deleveries.push(data.ReturnedData[i]);
                }
                $scope.selectedDelevey = $scope.Deleveries[0];

            }).error(function (data) {
                $scope.Deleveries = data || [];
            });

        };

        $scope.Getdelivery = function () {
            if ($scope.selectedDelevey.Id > 0) {
                $http.get('/Product/Getdelivery/' + $scope.selectedDelevey.Id).success(function (data) {

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.Cost = data.ReturnedData.Cost;
                    $scope.CanDelete = $scope.selectedDelevey.CanDelete;
                }).error(function (data) {

                });

            } else {

                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
                $scope.Cost = '';
                $scope.CanDelete = false;
            }
        };

        $scope.getGetdeliveryById = function (id) {
            var match = _.filter($scope.Deleveries, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedDelevey = $scope.Deleveries[0];

                return;
            }
            $scope.selectedDelevey = match[0];
            $scope.GetdeliveryCost();
        };

        $scope.getGetGetWayById = function (id) {
            var match = _.filter($scope.GetWays, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectGetway = $scope.GetWays[0];

                return;
            }
            $scope.selectGetway = match[0];
            $scope.GetGetWay($scope.selectGetway.Id);
        };

        $scope.CheckQuntaty = function (id, quntaty, numId) {
            if (quntaty <= 0) {
                alert("ادخل قيمة اكبر من الصفر");
                $scope.enableSave = false;
                return;
            }

            if (quntaty > id) {
                alert("الكمية المتاحة (" + id + ")  لاتغطى  الكمية المطلوبة( " + quntaty + ") ");
                $scope.enableSave = false;
                return;
            }
            var match = _.filter($scope.MyCart.MyCartDetailse, function (item) {

                return item.Id === numId;
            });
            if (match.length <= 0) {


                return;
            }

            var cartDetails = $scope.MyCart.MyCartDetailse;
            for (var i = 0; i < cartDetails.length; i++) {
                if (match[0].Id == cartDetails[i].Id) {
                    cartDetails[i].Quntaty = quntaty;
                    cartDetails[i].Total = quntaty * match[0].Price;
                }
            }

          
            $http.post('/Home/SaveMyCartDetails', JSON.stringify(cartDetails)).success(function (data) {
               
                window.location.href = '/Home/MyCart';
                 
               
            }
        );

           
            $scope.CalcItemsTotal();



            $scope.enableSave = true;
        };
        $scope.GetMyCart = function () {

            $http.get('/Home/GetMyCart').success(function (data) {

                $scope.MyCart = data.ReturnedData;
                $scope.getGetGetWayById($scope.MyCart.GetwayId);
                $scope.getGetdeliveryById($scope.MyCart.DeliveryId);

                $scope.CalcItemsTotal();

            }).error(function (data) {

            });
        };

        $scope.Delete = function () {
            if ($scope.selectedDelevey.Id > 0) {
                $http.post('/Product/Deletedelivery/' + $scope.selectedDelevey.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllDeleveries();


                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.NameEn = '';
                    $scope.Cost = '';
                    $scope.CanDelete = false;

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };

        $scope.GetAllGetWaies = function () {

            $scope.GetWays = [];

            $http.get("/GetWay/GetAllGetWaies").success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    data.ReturnedData[i].Berif = "";
                    $scope.GetWays.push(data.ReturnedData[i]);
                }
                $scope.selectGetway = $scope.GetWays[0];

            }).error(function (data) {
                $scope.GetWays = data || [];
            });
        };

        $scope.getGetWayBerif = function (id) {
            var match = _.filter($scope.GetWays, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectGetway = $scope.GetWays[0];

                return;
            }
            $scope.selectGetway = match[0];

        };


        $scope.GetGetWay = function (id) {


            $http.get('/GetWay/GetGetWay/' + id).success(function (data) {
                $scope.selectGetway.Berif = "";
                $scope.getGetWayBerif(id);
                $scope.selectGetway.Berif = data.ReturnedData.Berif;
                $scope.selectGetway.BerifEn = data.ReturnedData.BerifEn;
                


            }).error(function () {

            });


        };

        $scope.GetdeliveryCost = function () {
            if ($scope.selectedDelevey.Id > 0) {
                $http.get('/Product/Getdelivery/' + $scope.selectedDelevey.Id).success(function (data) {

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.Cost = data.ReturnedData.Cost;
                    $scope.GetFullTotal();
                }).error(function (data) {

                });

            } else {

                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
                $scope.Cost = '';
                $scope.CanDelete = false;
            }
        };

        $scope.DeleteItem = function (id) {
            //$scope.MyCart.MyCartDetailse = _.reject($scope.MyCart.MyCartDetailse, function (item) {
            //    return item.Id === id;
            //});
            $http.get('/Home/DeleteFromCart/' + id).success(function (data) {

                location.reload();
                $scope.MyCart = data.ReturnedData;
                $scope.getGetGetWayById($scope.MyCart.GetwayId);
                // $scope.getGetdeliveryById($scope.MyCart.DeliveryId);

                $scope.CalcItemsTotal();

            });
        };


        $scope.CalcItemsTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.MyCart.MyCartDetailse.length; i++) {
                total += $scope.MyCart.MyCartDetailse[i].Total;
            }
            $scope.totalItems = total;
            $scope.GetFullTotal();
        };

        $scope.GetFullTotal = function () {



            $scope.CartTotal = $scope.Cost + $scope.totalItems;
        };

        $scope.SaveMyCart = function () {

            if ($scope.enableSave === true) {


                if ($scope.selectedDelevey.Id < 1) {
                    alert("أختر طريقة الشحن");
                    return;
                }

                if ($scope.selectGetway === undefined || $scope.selectGetway.Id < 1) {
                    alert("أختر طريقة الدفع");

                    return;
                }

                if ($scope.MyCart === undefined || $scope.MyCart.MyCartDetailse === undefined || $scope.MyCart.MyCartDetailse.length === 0) {
                    alert("أختر  المنتجات");
                    return;
                }

                var myCart = {};
                myCart.DeliveryId = $scope.selectedDelevey.Id;
                myCart.Total = $scope.CartTotal;
                myCart.GetwayId = $scope.selectGetway.Id;

                $scope.MyCartDetailse = [];
                for (var i = 0; i < $scope.MyCart.MyCartDetailse.length ; i++) {
                    var myCartDetailse = {};
                    myCartDetailse.Price = $scope.MyCart.MyCartDetailse[i].Price;
                    myCartDetailse.Total = $scope.MyCart.MyCartDetailse[i].Total;
                    myCartDetailse.Quntaty = $scope.MyCart.MyCartDetailse[i].Quntaty;

                    myCartDetailse.ProductId = $scope.MyCart.MyCartDetailse[i].ProductId;
                    $scope.MyCartDetailse.push(myCartDetailse);
                }

                myCart.MyCartDetailse = $scope.MyCartDetailse;
                $http.post('/Home/SaveMyCart', JSON.stringify(myCart)).success(function (data) {
                    if (data.ClientStatusCode === 0) {

                        alert(data.ClientMessageContent[0]);
                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                        window.location.href = '/Home';
                        $scope.reset();
                    } else {
                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                        window.location.href = '/account/login';
                    }

                });

                $scope.reset = function () {
                    $scope.MyCart = null;


                };
            } else {
                alert("عفوا يجب مراجعة البيانات ");
            }
        };
    }
    DeleveyApp.controller("deleveyCtrl", deleveyCtrl);



})(angular);

