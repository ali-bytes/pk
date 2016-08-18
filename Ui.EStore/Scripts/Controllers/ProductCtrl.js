
var productApp = angular.module("productApp", ['ngSanitize']);

(function (angular) {

    productApp.directive('myMaxlength', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                var maxlength = Number(attrs.myMaxlength);
                function fromUser(text) {
                    if (text.length > maxlength) {
                        var transformedInput = text.substring(0, maxlength);
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                        return transformedInput;
                    }
                    return text;
                }
                ngModelCtrl.$parsers.push(fromUser);
            }
        };
    });
    
    productApp.directive('wholeNumber', function () {
        return {
            link: function (scope, elem) {
                var num = (parseFloat(elem.val(), 10));
                decimalOnly(elem, num);
                elem.on("blur", function () {
                    num = num > (scope.maxValue) || (isNaN(num)) ? 0 : num;
                    return num;
                });
                elem.on("change", function () {
                    num = num > (scope.maxValue) || (isNaN(num)) ? 0 : num;
                    return num;

                });
            }
        };
    });


    function productCtrl($scope, $http) {
        $scope.RateValue = 5;
        $scope.MainCategories = [];
        $scope.Categories = [];
        $scope.LestCategories = [];
        $scope.Brands = [];
        $scope.Products = [];
        $scope.ImageUrl = '';
        $scope.Id = 0;
        $scope.Name = '';
        $scope.Notes = '';
        $scope.Berif = '';
        $scope.Details = '';
        $scope.NameEn = '';
        $scope.NotesEn = '';
        $scope.BerifEn = '';
        $scope.DetailsEn = '';
        
        $scope.SalePrice = 0;
        $scope.DiscountPrice = 0;
        $scope.DiscountPercent = false;
        $scope.IsHome = false;
        $scope.BestSeller = false;
        $scope.AvailableQuntaty = 0;
        $scope.CanDelete = false;
        $scope.NewProducts = [];
        $scope.ProductCategories = [];
        $scope.GetFullCategories = function () {
            $scope.Categories = [];
            var category = { Id: 0, Name: 'أختر الفئة', ImageUrl: '', categoryId: 0, CanDelete: false };
            $scope.Categories.push(category);
            $http.get('/Product/GetAllCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Categories.push(data.ReturnedData[i]);
                }
                $scope.selectedCategory = $scope.Categories[0];

            }).error(function (data) {
                $scope.Categories = data || [];
            });

        };
        $scope.CategoryProducts = [];
        $scope.UserName = '';
        $scope.UserNameEn = '';
        $scope.Email = '';
        $scope.Question = '';
        $scope.Mobile = '';
        $scope.ProductId = 0;


        //  $scope.fullstring =JSON.parse($scope.isString);
        $scope.GetAllMainCategories = function () {
            $scope.MainCategories = [];
            $scope.Categories = [];
           
         
            $http.get('/Product/GetAllMainCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.MainCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedMainCategory = $scope.MainCategories[0];

            }).error(function (data) {
                $scope.MainCategories = data || [];
            });

        };
        $scope.GetAllLestCategories = function () {

            $scope.LestCategories = [];
            var category = { Id: 0, Name: 'أختر الفئة الصغرى', ImageUrl: '', MainCategoryId: 0, CanDelete: false };
            $scope.LestCategories.push(category);
            $http.get('/Product/GetAllLestCategoriesByCategoryId/' + $scope.selectedCategory.Id).success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.LestCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedLestCategory = $scope.LestCategories[0];

            }).error(function (data) {
                $scope.LestCategories = data || [];
            });
        };
        $scope.getMainCategory = function (id) {
            var match = _.filter($scope.MainCategories, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedMainCategory = $scope.MainCategories[0];

                return;
            }
            $scope.selectedMainCategory = match[0];
        };
        
        $scope.GetFullLestCategory = function () {
            $scope.Products = [];
            $scope.LestCategories = [];
            var category = { Id: 0, Name: 'أختر الفئة الصغرى', ImageUrl: '', MainCategoryId: 0, CanDelete: false };
            $scope.LestCategories.push(category);
            $http.get('/Product/GetAllLestCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.LestCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedLestCategory = $scope.LestCategories[0];

            }).error(function (data) {
                $scope.LestCategories = data || [];
            });
        };
        $scope.getCategory = function (id) {
            var match = _.filter($scope.Categories, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedCategory = $scope.Categories[0];

                return;
            }
            $scope.selectedCategory = match[0];
        };
        $scope.getLestCategory = function (id) {
            var match = _.filter($scope.LestCategories, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedLestCategory = $scope.LestCategories[0];

                return;
            }
            $scope.selectedLestCategory = match[0];
        };
        
        $scope.getBrand = function (id) {
            var match = _.filter($scope.Brands, function (item) {
                return item.Id === id;
            });
            if (match.length <= 0) {
                $scope.selectedBrand = $scope.Brands[0];

                return;
            }
            $scope.selectedBrand = match[0];
        };

        $scope.GetAllCategories = function () {
            $scope.Products = [];
            $scope.Categories = [];
            $scope.LestCategories = [];
            var category = { Id: 0, Name: 'أختر الفئة', ImageUrl: '', MainCategoryId: 0, CanDelete: false };
            $scope.Categories.push(category);
            $http.get('/Product/GetCategoryByMainId/' + $scope.selectedMainCategory.Id).success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Categories.push(data.ReturnedData[i]);
                }
                $scope.selectedCategory = $scope.Categories[0];

            }).error(function (data) {
                $scope.Categories = data || [];
            });

        };

        $scope.GetAllBrands = function () {
            $scope.Brands = [];
            var category = { Id: 0, Name: 'أختر الماركة', ImageUrl: '', CanDelete: false };
            $scope.Brands.push(category);
            $http.get('/Product/GetAllBrands').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Brands.push(data.ReturnedData[i]);
                }
                $scope.selectedBrand = $scope.Brands[0];

            }).error(function (data) {
                $scope.Brands = data || [];
            });

        };

        $scope.Save = function () {
            validateScript.validate($("#ProductFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                if ($scope.selectedMainCategory.Id <= 0 || $scope.selectedMainCategory.Id === undefined) {

                    alert("أختر الفئة الرئيسية");
                    return;
                }

                if ($scope.selectedCategory.Id <= 0 || $scope.selectedCategory.Id === undefined) {

                    alert("أختر الفئة الفئة الفرعية");
                    return;
                }
                if ($scope.selectedBrand.Id <= 0 || $scope.selectedBrand.Id === undefined) {

                    alert("أختر الماركة ");
                    return;
                }


                //$scope.Id = $("#txt_Berif").val();
                $scope.Name = $("#txt_Name").val();
                $scope.NameEn = $("#txt_NameEn").val();
                $scope.ImageUrl = $("#imgUserImage").attr("src");
                $scope.Notes = $("#txt_Notes").val();
                $scope.Berif = $("#txt_Berif").val();
                $scope.Details = $("#txt_Details").val();
                $scope.NotesEn = $("#txt_NotesEn").val();
                $scope.BerifEn = $("#txt_BerifEn").val();
                $scope.DetailsEn = $("#txt_DetailsEn").val();
                $scope.SalePrice = $("#txt_Sale").val();
                $scope.DiscountPrice = $("#txt_Discount").val();
                $scope.DiscountPercent = ($("#Chk_IsPresent").is(':checked'));
                $scope.IsHome = ($("#Chk_IsPresent").is(':checked'));
                $scope.AvailableQuntaty = $("#txt_Quntaty").val();
                $scope.BestSeller = ($("#Chk_BestSeller").is(':checked'));

                var product = {};
                product.Id = $scope.Id;
                product.Name = $scope.Name;
                product.NameEn = $scope.NameEn;
                product.ImageUrl = $scope.ImageUrl;
                product.BrandId = $scope.selectedBrand.Id;
                product.MainCategoryId = $scope.selectedMainCategory.Id;
                product.CategoryId = $scope.selectedCategory.Id;
                product.LestCategoryId = $scope.selectedLestCategory.Id;
                product.Berif = $scope.Berif;
                product.Details = $scope.Details;
                product.BerifEn = $scope.BerifEn;
                product.DetailsEn = $scope.DetailsEn;
                product.SalePrice = $scope.SalePrice;
                product.DiscountPrice = $scope.DiscountPrice;

                product.DiscountPercent = $scope.DiscountPercent;
                product.IsHome = $scope.IsHome;
                product.BestSeller = $scope.BestSeller;
                product.AvailableQuntaty = $scope.AvailableQuntaty;
                product.Notes = $scope.Notes;
                product.NotesEn = $scope.NotesEn;
                
                $http.post('/Product/SaveProduct', JSON.stringify(product)).success(function (data) {
                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.Notes = '';
                    $scope.Berif = '';
                    $scope.Details = '';
                    $scope.NameEn = '';
                    $scope.NotesEn = '';
                    $scope.BerifEn = '';
                    $scope.DetailsEn = '';
                    $scope.SalePrice = 0;
                    $scope.DiscountPrice = 0;
                    $scope.DiscountPercent = false;
                    product.IsHome = false;
                    product.BestSeller = false;
                    $scope.AvailableQuntaty = 0;
                    $scope.CanDelete = false;
                    if ($scope.Id > 0) {


                        $scope.GetAllProducts();
                    }
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
                });


            };

        };

        $scope.SaveCount = function myfunction() {
            $scope.AvailableQuntaty = $("#txt_Quntaty").val();
            var product = {};
            product.Id = $scope.Id;
            product.AvailableQuntaty = $scope.AvailableQuntaty;

            $http.post('/Product/SaveProductCount', JSON.stringify(product)).success(function(data) {
                $scope.AvailableQuntaty = 0;
                if ($scope.Id > 0) {
                    $scope.Id = 0;

                    $scope.GetAllProducts();
                }
                commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);
            });
        };

        $scope.GetAllProducts = function () {

            $scope.Products = [];
            var category = { Id: 0, Name: 'أختر المنتج', ImageUrl: '', MainCategoryId: 0, CanDelete: false };
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

        $scope.GetProduct = function () {
            if ($scope.selectedProduct.Id > 0) {
                $http.get('/Product/GetProduct/' + $scope.selectedProduct.Id).success(function (data) {

                    $scope.ImageUrl = data.ReturnedData.ImageUrl;

                    $scope.Id = data.ReturnedData.Id;
                    $scope.Name = data.ReturnedData.Name;
                    $scope.NameEn = data.ReturnedData.NameEn;
                    $scope.Notes = data.ReturnedData.Notes;
                    $scope.Berif = data.ReturnedData.Berif;
                    $scope.Details = data.ReturnedData.Details;
                    $scope.NotesEn = data.ReturnedData.NotesEn;
                    $scope.BerifEn = data.ReturnedData.BerifEn;
                    $scope.DetailsEn = data.ReturnedData.DetailsEn;
                    $scope.SalePrice = data.ReturnedData.SalePrice;
                    $scope.DiscountPrice = data.ReturnedData.DiscountPrice;
                    $scope.DiscountPercent = data.ReturnedData.DiscountPercent;
                    $scope.IsHome = data.ReturnedData.IsHome;
                    $scope.BestSeller = data.ReturnedData.BestSeller;
                    $scope.AvailableQuntaty = data.ReturnedData.AvailableQuntaty;

                    $scope.getMainCategory(data.ReturnedData.MainCategoryId);

                    $scope.getCategory(data.ReturnedData.CategoryId);
                    $scope.getLestCategory(data.ReturnedData.LestCategoryId);
                    $scope.getBrand(data.ReturnedData.BrandId);

                    $scope.CanDelete = $scope.selectedProduct.CanDelete;

                }).error(function (data) {

                });

            } else {
                $scope.ImageUrl = '';
                $scope.Id = 0;
                $scope.Name = '';
                $scope.Notes = '';
                $scope.Berif = '';
                $scope.Details = '';
                $scope.NameEn = '';
                $scope.NotesEn = '';
                $scope.BerifEn = '';
                $scope.DetailsEn = '';
                $scope.SalePrice = 0;
                $scope.DiscountPrice = 0;
                $scope.DiscountPercent = false;
                $scope.AvailableQuntaty = 0;
                $scope.IsHome = false; 
                $scope.BestSeller = false;
                $scope.CanDelete = false;
                $scope.selectedMainCategory.Id = 0;
            }
        };

        $scope.Delete = function () {
            if ($scope.selectedProduct.Id > 0) {
                $http.post('/Product/DeleteProduct/' + $scope.selectedProduct.Id).success(function (data) {
                    commonViewModel.displayTextSuccessMessage(data.ClientMessageContent[0]);

                    $scope.ImageUrl = '';
                    $scope.Id = 0;
                    $scope.Name = '';
                    $scope.Notes = '';
                    $scope.Berif = '';
                    $scope.Details = '';
                    $scope.NameEn = '';
                    $scope.NotesEn = '';
                    $scope.BerifEn = '';
                    $scope.DetailsEn = '';
                    $scope.SalePrice = 0;
                    $scope.DiscountPrice = 0;
                    $scope.DiscountPercent = false;
                    $scope.BestSeller = false;
                    $scope.AvailableQuntaty = 0;
                    $scope.CanDelete = false;
                    $scope.GetAllProducts();

                }).error(function (data) {
                    commonViewModel.DisplayErrorTextMessage(data.ClientMessageContent[0]);

                });
            }

        };
        
        $scope.GetNewProducts = function () {

            $scope.NewProducts = [];
            $scope.ProductCategories = [];
            $http.get('/Product/GetAllMainCategories').success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.ProductCategories.push(data.ReturnedData[i]);
                }
                $scope.selectedMainCategory = $scope.ProductCategories[0];



                $http.get('/Product/GetAllproducts').success(function (productdata) {

                    for (var i = 0; i < productdata.ReturnedData.length; i++) {
                        $scope.NewProducts.push(productdata.ReturnedData[i]);
                    }
                    $scope.selectedNewProduct = $scope.NewProducts[0];
                    
                }).error(function (productdata) {
                    $scope.NewProducts = productdata || [];
                });
            }).error(function (data) {
                $scope.MainCategories = data || [];
            });
        };

        $scope.HomeProducts = function () {

            $scope.HomeProducts = [];
          
            $http.get('/Product/HomeProducts').success(function (productdata) {

                    for (var i = 0; i < productdata.ReturnedData.length; i++) {
                        $scope.HomeProducts.push(productdata.ReturnedData[i]);
                    }
                    $scope.selectedNewProduct = $scope.HomeProducts[0];
                    jQuery("div#slider1").codaSlider();

                }).error(function (productdata) {
                    $scope.HomeProducts = productdata || [];
                });
            
        };
        
        $scope.GetMainCategoryProducts = function () {

            $scope.CategoryProducts = [];

            $http.get('/Product/GetAllproductsByMainCategory').success(function (data) {

  

                    $scope.CategoryProducts = data.ReturnedData;





            }).error(function (data) {
                $scope.CategoryProducts = data || [];
            });
        };

  
        $scope.SaveEnquiry = function () {
            validateScript.validate($("#ProductFrm"));
            if ($("#divWarning").hasClass("invisible")) {

                $scope.ProductId = $("#product_Id").val();
                if ($scope.ProductId <= 0) {
                    return;
                }
                var enquirie = {};
                enquirie.UserName = $scope.UserName;
                enquirie.UserNameEn = $scope.UserNameEn;
                enquirie.Email = $scope.Email;
                enquirie.Question = $scope.Question;
                enquirie.Mobile = $scope.Mobile;
                enquirie.ProductId = $scope.ProductId;
                
                $http.post('/Product/SaveEnquiry', JSON.stringify(enquirie)).success(function (data) {
                    $scope.UserName = '';
                    $scope.UserNameEn = '';
                    $scope.Email = '';
                    $scope.Question = '';
                    $scope.Mobile = '';
                    $scope.ProductId = '';
                    alert("سيتم الرد فى خلال 24 ساعة");
                }).error(function (data) {

                });
            }
        };
        
        
        $scope.GetAllLestCategoriesProduct = function () {

            $scope.Products = [];
            var category = { Id: 0, Name: 'أختر المنتج', ImageUrl: '', MainCategoryId: 0, CanDelete: false };
            $scope.Products.push(category);
            $http.get('/Product/GetAllproductsByLestCategoryId/' + $scope.selectedLestCategory.Id).success(function (data) {

                for (var i = 0; i < data.ReturnedData.length; i++) {
                    $scope.Products.push(data.ReturnedData[i]);
                }
                $scope.selectedProduct = $scope.Products[0];

            }).error(function (data) {
                $scope.Products = data || [];
            });
        };

        $scope.SaveRate = function () {
            var rate = {};
            $scope.ProductId =  $("#Rateproduct_Id").val();
           
            rate.Rate = $scope.RateValue;
            rate.ProductId = $scope.ProductId;

            $http.post('/Product/SaveRating', JSON.stringify(rate)).success(function (data) {
                alert("شكرا لتقيمكم منتجنا ");

            });
        };
  
    }

    productApp.controller("productCtrl", productCtrl);



})(window.angular);

