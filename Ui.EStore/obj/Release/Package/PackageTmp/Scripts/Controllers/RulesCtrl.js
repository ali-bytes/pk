var RuleApp = angular.module("RuleApp", []);


(function (angular) {

    function ruleCtrl($scope, $http) {

        $scope.Rules = [];

        $scope.Id = 0;

        $scope.CanDelete = false;

        $scope.Save = function () {
            validateScript.validate($("#RuleFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                $scope.Name = $("#txt_Title").val();
                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;


                $http.post('/Admin/SaveRule', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) {
                        $scope.Name = '';
                    if ($scope.Id > 0) {

                        $scope.Rules = [];

                        $scope.Id = 0;
                      
                        $scope.CanDelete = false;
                        $scope.GetAllRules();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                }
                else {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
            }
        });


            };

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

        $scope.GetRule = function () {
            if ($scope.selectedRule.Id > 0) {
                $http.get('/Admin/GetRule/' + $scope.selectedRule.Id).success(function (data) {

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.CanDelete = $scope.selectedRule.CanDelete;
                }).error(function (data) {

                });

            } else {

                $scope.Id = 0;
                $scope.Name = '';
                $scope.CanDelete = false;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedRule.Id > 0) {
                $http.post('/Admin/DeleteRule/' + $scope.selectedRule.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.GetAllRules();


                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.CanDelete = false;

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };
    }
    RuleApp.controller("ruleCtrl", ruleCtrl);



})(angular);

