var CityApp = angular.module("CityApp", []);


(function (angular) {

    
    
    function cityCtrl($scope, $http) {

        $scope.Cities = [];
        $scope.Governates = [];
      
        $scope.Id = 0;
        $scope.Name = '';
        $scope.NameEn = '';
        $scope.CanDelete = false;

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

        $scope.Save = function () {
            validateScript.validate($("#CityFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                if ($scope.selectedGovernate.Id <= 0 || $scope.selectedGovernate.Id === undefined) {

                    alert("أختر المحافظة");
                    return;
                }

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Title").val();
                $scope.NameEn = $("#txt_TitleEn").val();
              

                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;
                category.NameEn = $scope.NameEn;
           
                category.GovernateId = $scope.selectedGovernate.Id;


                $http.post('/Branche/SaveCity', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) {
                        $scope.Name = '';
                        $scope.NameEn = '';
                    $scope.Cities = [];
                    $scope.CanDelete = false;
                    if ($scope.Id > 0) {
                        $scope.Id = 0;
                        $scope.GetAllCities();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                }
                else {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                }
                });


            };

        };


        $scope.GetAllCities = function () {
            $scope.Cities = [];
            var category = { Id: 0, Name: 'أختر المدينة',  categoryId: 0, CanDelete: false };
            $scope.Cities.push(category);
            $http.get('/Branche/GetAllCities').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Cities.push(data.ReturnedData[i]);
                }
                $scope.selectedCity = $scope.Cities[0];

            }).error(function (data) {
                $scope.Cities = data || [];
            });

        };


        $scope.getGovernate = function (id) {
            var match = _.filter($scope.Governates, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedGovernate = $scope.Governates[0];

                return;
            }
            $scope.selectedGovernate = match[0];
        };

        $scope.GetCity = function () {
            if ($scope.selectedCity.Id > 0) {
                $http.get('/Branche/GetCity/' + $scope.selectedCity.Id).success(function (data) {
                   
                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                  
                    $scope.getGovernate(data.ReturnedData.GovernateId);

                    $scope.CanDelete = $scope.selectedCity.CanDelete;

                }).error(function (data) {

                });

            } else {
              
                $scope.Id = 0;
                $scope.Name = '';
                $scope.NameEn = '';
                $scope.CanDelete = false;
                $scope.selectedGovernate.Id = 0;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedCity.Id > 0) {
                $http.post('/Branche/DeleteCity/' + $scope.selectedCity.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllCities();

                   
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

    CityApp.controller("cityCtrl", cityCtrl);



})(angular);

