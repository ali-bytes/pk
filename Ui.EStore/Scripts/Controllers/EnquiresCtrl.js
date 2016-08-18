var EnquiryApp = angular.module("EnquiryApp", ['ngSanitize']);

(function (angular) {
    function enquiryCtrl($scope, $http) {
        $scope.Enquiries = [];

        $scope.Products = [];

        $scope.Question = '';
        $scope.Replay = '';
        $scope.UserName = "";
        $scope.Email = "";
        $scope.Mobile = "";
        $scope.Time = "";
        $scope.IsActive = false;

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
            validateScript.validate($("#enquiryFrm"));
            if ($("#divWarning").hasClass("invisible")) {

               

             
                $scope.Replay = $("#txt_Details").val();
                $scope.Question = $("#txt_Question").val();
                $scope.IsActive = ($("#Chk_IsActive").is(':checked'));

               



                var category = {};
                category.Replay = $scope.Replay;
                category.Question = $scope.Question;
                category.IsActive = $scope.IsActive;
                category.Id = $scope.Id;


                $http.post('/Product/SaveEnquiry', JSON.stringify(category)).success(function (data) {

                    $scope.Replay = '';

                    $scope.Question = '';
                    $scope.IsActive = '';
                  
                    $scope.Enquiries = [];
                  
                    if ($scope.Id > 0) {
                        $scope.Id = 0;
                        $scope.GetAllProducts();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                });


            };

        };


        $scope.GetAllEnquiries = function () {
            $scope.Enquiries = [];
            var category = { Id: 0, Question: 'أختر الاستفسار' };
            $scope.Enquiries.push(category);
            $http.get('/Product/GetEnquiriesByProductId/' + $scope.selectedProduct.Id).success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Enquiries.push(data.ReturnedData[i]);
                }
                $scope.selectedEnquiry = $scope.Enquiries[0];

            }).error(function (data) {
                $scope.Enquiries = data || [];
            });

        };


        $scope.GetEnquiry = function () {
            if ($scope.selectedEnquiry.Id > 0) {
                $http.get('/Product/GetEnquiry/' + $scope.selectedEnquiry.Id).success(function (data) {

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Question = data.ReturnedData.Question;
                    $scope.Replay = data.ReturnedData.Replay;
                    $scope.UserName = data.ReturnedData.UserName;
                    $scope.Email = data.ReturnedData.Email;
                    $scope.Mobile = data.ReturnedData.Mobile;
                    $scope.IsActive = data.ReturnedData.IsActive;


                  

                }).error(function (data) {

                });

            } else {

                $scope.Id = 0;
                $scope.Question = "";
                $scope.Replay = "";
                $scope.UserName = "";
                $scope.Email = "";
                $scope.Mobile = "";
                $scope.IsActive = false;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedEnquiry.Id > 0) {
                $http.post('/Product/DeleteEnquiry/' + $scope.selectedEnquiry.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllEnquiries();


                    $scope.Id = 0;
                    $scope.Question = "";
                    $scope.Replay = "";
                    $scope.UserName = "";
                    $scope.Email = "";
                    $scope.Mobile = "";
                    $scope.IsActive = false;

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };
    };

    EnquiryApp.controller("enquiryCtrl", enquiryCtrl);



})(window.angular);