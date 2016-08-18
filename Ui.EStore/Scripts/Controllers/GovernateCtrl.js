var governateApp = angular.module("governateApp", []);


(function (angular) {

    function governateCtrl($scope, $http) {

        $scope.Governates = [];
     
        $scope.Id = 0;
   
        $scope.CanDelete = false;

        $scope.Save = function () {
            validateScript.validate($("#GovernateFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Title").val();
            

                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;
                category.NameEn = $scope.NameEn;
             

                $http.post('/Branche/SaveGovernate', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) { 
                    $scope.Name = '';
                    if ($scope.Id > 0) {

                        $scope.Governates = [];


                        $scope.CanDelete = false;
                      
                        $scope.Id = 0;
                      
                        $scope.GetAllGovernates();
                    }
                  
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                }
                else {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                }
                });


            };

        };



        $scope.GetAllGovernates = function () {
            $scope.Governates = [];
            var category = { Id: 0, Name: 'أختر المحافظة', CanDelete: false };
            $scope.Governates.push(category);
            $http.get('/Branche/GetAllGovernates').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Governates.push(data.ReturnedData[i]);
                }
                $scope.selectedGovernate = $scope.Governates[0];

            }).error(function (data) {
                $scope.Governates = data || [];
            });

        };

        $scope.GetGovernate = function () {
            if ($scope.selectedGovernate.Id > 0) {
                $http.get('/Branche/GetGovernate/' + $scope.selectedGovernate.Id).success(function (data) {
                   
                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.CanDelete = $scope.selectedGovernate.CanDelete;
                }).error(function (data) {

                });

            } else {
                
                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
                $scope.CanDelete = false;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedGovernate.Id > 0) {
                $http.post('/Branche/DeleteGovernate/' + $scope.selectedGovernate.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllGovernates();

                 
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
    governateApp.controller("governateCtrl", governateCtrl);



})(angular);

