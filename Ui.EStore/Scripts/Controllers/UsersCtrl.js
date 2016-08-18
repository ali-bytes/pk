var UserApp = angular.module("UserApp", []);


(function (angular) {

    function userCtrl($scope, $http) {

        $scope.Users = [];
        $scope.Specialitys = [];
        $scope.Rules = [];

        $scope.Id = 0;
        $scope.UserName = '';
        $scope.Password = '';
        $scope.Email = '';
        $scope.IsActive = true;

        $scope.CanDelete = false;

        $scope.Save = function () {
            validateScript.validate($("#UserFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                if ($scope.selectedRule.Id <= 0 || $scope.selectedRule.Id === undefined) {

                    alert("أختر الصلاحية");
                    return;
                }



                $scope.UserName = $("#txt_Title").val();
                $scope.Password = $("#txt_Password").val();
                $scope.Email = $("#txt_Email").val();
                $scope.IsActive = ($("#Chk_IsActive").is(':checked'));

                var category = {};
                category.Id = $scope.Id;
                category.UserName = $scope.UserName;
                category.Password = $scope.Password;
                category.Email = $scope.Email;
                category.IsActive = $scope.IsActive;
                category.RuleId = $scope.selectedRule.Id;

                $http.post('/Admin/SaveUser', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) {
                        $scope.Id = 0;
                        $scope.UserName = '';
                        $scope.Password = '';
                        $scope.Email = '';
                        $scope.IsActive = false;
                        if ($scope.Id > 0) {

                            $scope.Users = [];

                            $scope.CanDelete = false;
                            $scope.GetAllUsers();
                        }
                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                    }
                    else {
                        commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                    }
                });


            };

        };



        $scope.GetAllUsers = function () {
            $scope.Users = [];
            var category = { Id: 0, UserName: 'أختر المستخدم', CanDelete: false };
            $scope.Users.push(category);
            $http.get('/Admin/GetAllUsers').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Users.push(data.ReturnedData[i]);
                }
                $scope.selectedUser = $scope.Users[0];

            }).error(function (data) {
                $scope.Users = data || [];
            });

        };

        $scope.GetUser = function () {
            if ($scope.selectedUser.Id > 0) {
                $http.get('/Admin/GetUser/' + $scope.selectedUser.Id).success(function (data) {

                    $scope.Id = data.ReturnedData.Id;
                    $scope.UserName = data.ReturnedData.UserName;
                    //$scope.Password = data.ReturnedData.Password;
                    $scope.IsActive = data.ReturnedData.IsActive;
                    $scope.Email = data.ReturnedData.Email;
                    $scope.getRule(data.ReturnedData.RuleId);
                    $scope.geSpeciality(data.ReturnedData.SpecialityID);

                    $scope.CanDelete = $scope.selectedUser.CanDelete;
                }).error(function (data) {

                });

            } else {

                $scope.Id = 0;
                $scope.UserName = '';
                $scope.CanDelete = false;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedUser.Id > 0) {
                $http.post('/Admin/DeleteUser/' + $scope.selectedUser.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllUsers();


                    $scope.Id = 0;
                    $scope.UserName = '';
                    $scope.CanDelete = false;

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };



        $scope.GetAllRules = function () {
            $scope.Rules = [];
            var category = { Id: 0, Name: 'أختر الصلاحية', CanDelete: false };
            $scope.Rules.push(category);
            $http.get('/Admin/GetAllRules').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Rules.push(data.ReturnedData[i]);
                }
                $scope.selectedRule = $scope.Rules[0];

            }).error(function (data) {
                $scope.Rules = data || [];
            });

        };
        $scope.getRule = function (id) {
            var match = _.filter($scope.Rules, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedRule = $scope.Rules[0];

                return;
            }
            $scope.selectedRule = match[0];
        };
        $scope.geSpeciality = function (id) {
            var match = _.filter($scope.Specialitys, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedSpeciality = $scope.Specialitys[0];

                return;
            }
            $scope.selectedSpeciality = match[0];
        };

    }
    UserApp.controller("userCtrl", userCtrl);



})(angular);

