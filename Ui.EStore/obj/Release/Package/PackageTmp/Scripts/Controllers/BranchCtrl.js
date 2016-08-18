var BranchApp = angular.module("BranchApp", []);


(function (angular) {

    BranchApp.config([
        '$httpProvider', function ($httpProvider) {
            // Initialize get if not there
            if (!$httpProvider.defaults.headers.get) {
                $httpProvider.defaults.headers.get = {};
            }

            // Enables Request.IsAjaxRequest() in ASP.NET MVC
            $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';

            // Disable IE ajax request caching
            $httpProvider.defaults.headers.get['If-Modified-Since'] = '0';
        }
    ]);
    function branchCtrl($scope, $http) {
        $scope.Branchs = [];
        $scope.Cities = [];
        $scope.Governates = [];

        $scope.Id = 0;
        $scope.ImageUrl = '';
        $scope.Name = '';
        $scope.NameEn = '';
        $scope.Address = '';
        $scope.AddressEn = '';
        $scope.Phone = '';
        $scope.Mobile = '';
        $scope.Details = '';
        $scope.DetailsEn = '';
        $scope.search = {};
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

        $scope.GetAllCities = function () {
            $scope.Cities = [];
            var category = { Id: 0, Name: 'أختر المدينة', categoryId: 0, CanDelete: false };
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

        $scope.GetGovernateCities = function () {
            $scope.Cities = [];
            var category = { Id: 0, Name: 'أختر المدينة', categoryId: 0, CanDelete: false };
            $scope.Cities.push(category);
            $scope.search.GovernateId = $scope.selectedGovernate.Id;
            $http.get('/Branche/GetAllCitiesByGovernateId/' + $scope.selectedGovernate.Id).success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Cities.push(data.ReturnedData[i]);
                }
                $scope.selectedCity = $scope.Cities[0];

            }).error(function (data) {
                $scope.Cities = data || [];
            });

        };
        $scope.SelectCities = function () {
            
            $scope.search.CityId = $scope.selectedCity.Id;
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

        $scope.getCity = function (id) {
            var match = _.filter($scope.Cities, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedCity = $scope.Cities[0];

                return;
            }
            $scope.selectedCity = match[0];
        };

        $scope.Save = function () {
            validateScript.validate($("#branchfrm"));
            if ($("#divWarning").hasClass("invisible")) {

                if ($scope.selectedGovernate.Id <= 0 || $scope.selectedGovernate.Id === undefined) {

                    alert("أختر المحافظة");
                    return;
                }
                if ($scope.selectedCity.Id <= 0 || $scope.selectedCity.Id === undefined) {

                    alert("أختر المدينة");
                    return;
                }

                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Name").val();
                $scope.NameEn = $("#txt_NameEn").val();
                $scope.Details = $("#txt_Details").val();
                $scope.Address = $("#txt_address").val();
                $scope.DetailsEn = $("#txt_DetailsEn").val();
                $scope.AddressEn = $("#txt_addressEn").val();
                $scope.Phone = $("#txt_Phone").val();
                $scope.Mobile = $("#txt_Mobile").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");




                var category = {};
                category.Id = $scope.Id;
                category.Name = $scope.Name;
                category.Address = $scope.Address;
                category.NameEn = $scope.NameEn;
                category.AddressEn = $scope.AddressEn;
                category.Phone = $scope.Phone;
                category.Mobile = $scope.Mobile;
                category.Details = $scope.Details;
                category.DetailsEn = $scope.DetailsEn;
                category.ImageUrl = $scope.ImageUrl;

                category.GovernateId = $scope.selectedGovernate.Id;
                category.CityId = $scope.selectedCity.Id;


                $http.post('/Branche/SaveBranch', JSON.stringify(category)).success(function (data) {
                    if (data.ClientStatusCode == 0) {
                        $scope.Name = '';

                        $scope.Details = '';
                        $scope.Address = '';
                        $scope.AddressEn = '';
                        $scope.Phone = '';
                        $scope.Mobile = '';
                        $scope.ImageUrl = $("#imgUserImage").attr("src", '');
                        $scope.Branchs = [];
                        $scope.CanDelete = false;
                        if ($scope.Id > 0) {
                            $scope.Id = 0;
                            $scope.GetAllBranches();
                        }
                        commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                    }
                    else {
                        commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);
                    }
                });


            };

        };


        $scope.GetAllBranches = function () {
            $scope.Branchs = [];
            var category = { Id: 0, Name: 'أختر الفرع', GovernateId: 0, CityId: 0, CanDelete: false };
            $scope.Branchs.push(category);
            $http.get('/Branche/GetAllBranchs').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Branchs.push(data.ReturnedData[i]);
                }
                $scope.selectedBranch = $scope.Branchs[0];

            }).error(function (data) {
                $scope.Branchs = data || [];
            });

        };

        $scope.GetFullBranchs = function () {
            $scope.Branchs = [];
           
            $http.get('/Branche/GetFullBranchs').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Branchs.push(data.ReturnedData[i]);
                }
                $scope.selectedBranch = $scope.Branchs[0];

            }).error(function (data) {
                $scope.Branchs = data || [];
            });

        };


        $scope.GetBranch = function () {
            if ($scope.selectedBranch.Id > 0) {
                $http.get('/Branche/GetBranch/' + $scope.selectedBranch.Id).success(function (data) {

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.Address = data.ReturnedData.Address;
                    $scope.AddressEn = data.ReturnedData.AddressEn;
                    $scope.Phone = data.ReturnedData.Phone;
                    $scope.Mobile = data.ReturnedData.Mobile;
                    $scope.Details = data.ReturnedData.Details;
                    $scope.DetailsEn = data.ReturnedData.DetailsEn;
                    $scope.ImageUrl = data.ReturnedData.ImageUrl;


                    $scope.getGovernate(data.ReturnedData.GovernateId);
                    $scope.getCity(data.ReturnedData.CityId);

                    $scope.CanDelete = $scope.selectedBranch.CanDelete;

                }).error(function (data) {

                });

            } else {

                $scope.Id = 0;
                $scope.Name = '';

                $scope.Details = '';
                $scope.Address = '';
                $scope.NameEn = '';
                $scope.DetailsEn = '';
                $scope.AddressEn = '';
                $scope.Phone = '';
                $scope.Mobile = '';
                $scope.CanDelete = false;
                $scope.selectedGovernate.Id = 0;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedBranch.Id > 0) {
                $http.post('/Branche/DeleteBrand/' + $scope.selectedBranch.Id).success(function (data) {
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

    BranchApp.controller("branchCtrl", branchCtrl);



})(angular);

