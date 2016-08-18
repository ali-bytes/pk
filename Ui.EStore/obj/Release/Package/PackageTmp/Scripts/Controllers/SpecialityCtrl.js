var SpecialityApp = angular.module("SpecialityApp", []);


(function (angular) {

    function specialityCtrl($scope, $http) {

        $scope.Specialitys = [];

        $scope.Id = 0;

        $scope.CanDelete = false;

        $scope.Save = function () {
            validateScript.validate($("#SpecialityFrm"));
            if ($("#divWarning").hasClass("invisible")) {


                $scope.Name = $("#txt_Title").val();
                $scope.NameEn = $("#txt_TitleEn").val();


                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;
                category.NameEn = $scope.NameEn;


                $http.post('/Admin/SaveSpeciality', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) {    $scope.Name = '';
                    if ($scope.Id > 0) {

                        $scope.Specialitys = [];



                        $scope.Id = 0;
                      
                        $scope.CanDelete = false;
                        $scope.GetAllSpecialitys();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                }
                else {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                }
                });


            };

        };



        $scope.GetAllSpecialitys = function () {
            $scope.Specialitys = [];
            var category = { Id: 0, Name: 'أختر التخصص', CanDelete: false };
            $scope.Specialitys.push(category);
            $http.get('/Admin/GetAllSpecialitys').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Specialitys.push(data.ReturnedData[i]);
                }
                $scope.selectedSpeciality = $scope.Specialitys[0];

            }).error(function (data) {
                $scope.Specialitys = data || [];
            });

        };

        $scope.GetSpeciality = function () {
            if ($scope.selectedSpeciality.Id > 0) {
                $http.get('/Admin/GetSpeciality/' + $scope.selectedSpeciality.Id).success(function (data) {

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.CanDelete = $scope.selectedSpeciality.CanDelete;
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
            if ($scope.selectedSpeciality.Id > 0) {
                $http.post('/Admin/DeleteSpeciality/' + $scope.selectedSpeciality.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllSpecialitys();


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
    SpecialityApp.controller("specialityCtrl", specialityCtrl);



})(angular);

