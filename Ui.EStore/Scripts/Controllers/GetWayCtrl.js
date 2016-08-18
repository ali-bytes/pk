(function (angular) {
    var getWayApp = angular.module("getWayApp", []);
    

    function getWayCtrl($scope, $http) {
      
        $scope.GetWays = [];
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.Berif = '';
        $scope.NameEn = '';
        $scope.BerifEn = '';


        $scope.Save = function () {
            validateScript.validate($("#getwayFrm"));
            if ($("#divWarning").hasClass("invisible")) {

               

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Name").val();
                $scope.NameEn = $("#txt_NameEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");
           
                $scope.Berif = $("#txt_Berif").val();
                $scope.BerifEn = $("#txt_BerifEn").val();
              


                var getway = {};
                getway.Id = $scope.Id;
                getway.Name = $scope.Name;
                getway.NameEn = $scope.NameEn;
                getway.ImageUrl = $scope.ImageUrl;
              
                getway.Berif = $scope.Berif;
                getway.BerifEn = $scope.BerifEn;
              
               
                $http.post('/GetWay/SaveGetWay', JSON.stringify(getway)).success(function (data) {
                    if (data.ClientStatusCode == 0) {  $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.NameEn = '';
                    $scope.BerifEn = '';

                    $scope.Berif = '';
                
                    if ($scope.Id > 0) {


                        $scope.GetAllGetWaies();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                }
                else {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                }
                });


            };

        };

        $scope.GetAllGetWaies = function () {

            $scope.GetWays = [];
            var getway = { Id: 0, Name: 'أختر طريقة الدفع', ImageUrl: '',  CanDelete:false };
            $scope.GetWays.push(getway);
            $http.get('/GetWay/GetAllGetWaies').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.GetWays.push(data.ReturnedData[i]);
                }
                $scope.selectGetway = $scope.GetWays[0];

            }).error(function (data) {
                $scope.GetWays = data || [];
            });
        };

        $scope.GetGetWay = function () {
            if ($scope.selectGetway.Id > 0) {
                $http.get('/GetWay/GetGetWay/' + $scope.selectGetway.Id).success(function (data) {

                    $scope.ImageUrl = data.ReturnedData.ImageUrl;

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                   
                    $scope.Berif = data.ReturnedData.Berif;
                    $scope.BerifEn = data.ReturnedData.BerifEn;
                    $scope.CanDelete = $scope.selectGetway.CanDelete;

                   
                }).error(function () {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
                $scope.BerifEn = '';

                $scope.Berif = '';
           
               
            }
        };

        $scope.Delete = function () {
            if ($scope.selectGetway.Id > 0) {
                $http.post('/GetWay/DeleteBrand/' + $scope.selectGetway.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.NameEn = '';
                    $scope.BerifEn = '';

                    $scope.Berif = '';
                  
                    $scope.GetAllGetWaies();

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };


    }

    getWayApp.controller("getWayCtrl", getWayCtrl);



})(window.angular);




