using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.EStore;
using Domain.EStore.Repositories;
using Ui.EStore.Helpers;
using Ui.EStore.Models;
using System.Configuration;
using WebMatrix.WebData;

namespace Ui.EStore.Controllers
{
    public class ProductController : BaseController
    {
        static readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        //
        // GET: /Product/

        #region ActionResult
        public ActionResult Index()
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("المنتجات", "المنتجات").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("المنتجات", "المنتجات").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("المنتجات", "المنتجات").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("المنتجات", "المنتجات").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("Products", "Products").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("Products", "Products").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("Products", "Products").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("Products", "Products").Title;

            }
            return View();
        }
        public ActionResult ShowDelevery()
        {

            return View();
        }
        public ActionResult AllProducts()
        {
            return View();
        }

        public ActionResult EditProductCount()
        {
            return View();
        }
        public ActionResult EditEnquiries()
        {
            return View();
        }

        public ActionResult SearchProduct(string name)
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("بحث", "المنتجات").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("بحث", "المنتجات").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("بحث", "المنتجات").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("بحث", "المنتجات").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("Search", "Products").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("Search", "Products").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("Search", "Products").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("Search", "Products").Title;

            }

            if (!string.IsNullOrEmpty(name))
            {
                @ViewBag.SearchName = name;
                //var product = _unitOfWork.Products.FindAll() .Where(c => c.Name.Contains(name)).ToList();

                //@ViewBag.product = product.ToList();
            }
            return View();
        }
        public ActionResult ProductView(string id)
        {
            


            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");

                int productId;
                if (!int.TryParse(tmpStr[0], out productId))
                    return RedirectToAction("Error404", "Home");
                var product = _unitOfWork.Products.FindById(productId);
                if (product == null)
                {
                    return RedirectToAction("Error404", "Home");
                }



                string maincategory;
                string category;
                string productname;
                if (CultureHelper.GetLang() == Enums.Lang.Ar)
                {
                    maincategory = _unitOfWork.MainCategories.FindById(product.MainCategoryId).Name;
                    category = _unitOfWork.Categories.FindById(product.CategoryId).Name;
                    productname = product.Name;
                }
                else
                {
                    maincategory = _unitOfWork.MainCategories.FindById(product.MainCategoryId).NameEn;
                    category = _unitOfWork.Categories.FindById(product.CategoryId).NameEn;
                    productname = product.NameEn;

                }

                string productName = maincategory + "-" + category + '-' + productname;
                ViewBag.MainCategory = maincategory;
                ViewBag.Category = category;
                ViewBag.Rate = GetRate(product.Id);
                ViewBag.title = productName;
                ViewBag.product = product;
                ViewBag.productFace = product.Berif.StripTagsCharArray();
                ViewBag.RootUrl = ConfigurationManager.AppSettings["RootUrl"];

                if (CultureHelper.GetLang() == Enums.Lang.Ar)
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(productname, "المنتجات").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(productname, "المنتجات").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(productname, "المنتجات").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(productname, "المنتجات").Title;

                }
                else
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(productname, "Products").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(productname, "Products").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(productname, "Products").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(productname, "Products").Title;

                }

            }
            return View();
        }

        private double GetRate(int productId)
        {
            // var product = _unitOfWork.Products.FindById(productId);

            var rates = _unitOfWork.Ratings.FindAll().Where(c => c.ProductId == productId);
            if (rates.Any())
            {


                return rates.Average(c => c.Rate);
            }
            return 0;
        }

        public static List<TwoProps> FullRates(int productId)
        {
            var rat = new List<TwoProps>();
            var rates = _unitOfWork.Ratings.FindAll().Where(c => c.ProductId == productId).GroupBy(c => c.Rate);
            foreach (var rate in rates)
            {
                rat.Add(new TwoProps
                {
                    Id = rate.Key,
                    Name = rate.Count().ToString()

                });
            }

            return rat.OrderByDescending(d => d.Id).ToList();

        }
        public static List<Product> Search(string name)
        {
            var product = new List<Product>();
            if (!string.IsNullOrEmpty(name))
            {

                product = _unitOfWork.Products.FindAll().Where(c => c.Name.Contains(name)).ToList();


            }
            return product;
        }
        public ActionResult AddMainCategory()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddMainCategory"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditMainCategory()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditMainCategory"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult AddDelevery()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddDelevery"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditDelevery()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditDelevery"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult AddCategory()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddCategory"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditCategory()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditCategory"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult AddBrand()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddBrand"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditBrand()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditBrand"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult AddProduct()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddProduct"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditProduct()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditProduct"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }
        public ActionResult AddProductImages()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddProductImage"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditProductImages()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditProductImage"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult MainCategoryView(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");

                int maincategoryId;
                if (!int.TryParse(tmpStr[0], out maincategoryId))
                    return RedirectToAction("Error404", "Home");
                var mainCategories = _unitOfWork.MainCategories.Find(c => c.Id == maincategoryId).FirstOrDefault();
                if (mainCategories == null)
                {
                    return RedirectToAction("Error404", "Home");
                }


                ViewBag.MainCategories = mainCategories;

            }
            return View();
        }

        public ActionResult LestCategoryView(string id, string Brands = "0")
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");
                int brand;
                if (!int.TryParse(Brands, out brand))
                    return RedirectToAction("Error404", "Home");
                int lestcategoryId;
                if (!int.TryParse(tmpStr[0], out lestcategoryId))
                    return RedirectToAction("Error404", "Home");
                var lestCategories = _unitOfWork.LestCategories.Find(c => c.Id == lestcategoryId).FirstOrDefault();
                if (lestCategories == null)
                {
                    return RedirectToAction("Error404", "Home");
                }

                var mainCategories = _unitOfWork.MainCategories.Find(c => c.Id == lestCategories.MainCategoryId).FirstOrDefault();
                ViewBag.MainCategories = mainCategories;
                ViewBag.Categories = _unitOfWork.Categories.Find(c => c.Id == lestCategories.CategoryId).FirstOrDefault(); ;
                ViewBag.BrandId = brand;

                ViewBag.LestCategories = lestCategories;
            }
            var brands = _unitOfWork.Brands.FindAll();
            var li = new List<SelectListItem>();
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                li.Add(new SelectListItem { Text = "الكل ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.Name, Value = brand.Id.ToString() }));

            }
            else
            {
                li.Add(new SelectListItem { Text = "All ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.NameEn, Value = brand.Id.ToString() }));

            }


            ViewData["brands"] = li;


            return View();
        }
        public ActionResult ManageLestCategories()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "ManageLestCategories"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }
        public ActionResult AddLestCategories()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "ManageLestCategories"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }


        public ActionResult CategoryProduct(string id, string Brands = "0")
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");

                int categoryId;
                int brand;
                if (!int.TryParse(Brands, out brand))
                    return RedirectToAction("Error404", "Home");
                if (!int.TryParse(tmpStr[0], out categoryId))
                    return RedirectToAction("Error404", "Home");
                var categories = _unitOfWork.Categories.Find(c => c.Id == categoryId).FirstOrDefault();
                if (categories == null)
                {
                    return RedirectToAction("Error404", "Home");
                }

                var mainCategories = _unitOfWork.MainCategories.Find(c => c.Id == categories.MainCategoryId).FirstOrDefault();
                ViewBag.MainCategories = mainCategories;
                ViewBag.Categories = categories;
                ViewBag.BrandId = brand;

                if (CultureHelper.GetLang() == Enums.Lang.Ar)
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(categories.Name, "المنتجات").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(categories.Name, "المنتجات").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(categories.Name, "المنتجات").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(categories.Name, "المنتجات").Title;

                }
                else
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(categories.NameEn, "Products").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(categories.NameEn, "Products").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(categories.NameEn, "Products").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(categories.NameEn, "Products").Title;

                }

            }
            var brands = _unitOfWork.Brands.FindAll();

            var li = new List<SelectListItem>();
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                li.Add(new SelectListItem { Text = "الكل ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.Name, Value = brand.Id.ToString() }));

            }
            else
            {
                li.Add(new SelectListItem { Text = "All ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.NameEn, Value = brand.Id.ToString() }));

            }
            ViewData["brands"] = li;
            return View();
        }


        public ActionResult GetProductsByLestCategoryId(string id, string Brands = "0")
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");

                int LestcategoryId;
                int brand;
                if (!int.TryParse(Brands, out brand))
                    return RedirectToAction("Error404", "Home");
                if (!int.TryParse(tmpStr[0], out LestcategoryId))
                    return RedirectToAction("Error404", "Home");
                var lestcategories = _unitOfWork.LestCategories.Find(c => c.Id == LestcategoryId).FirstOrDefault();
                if (lestcategories == null)
                {
                    return RedirectToAction("Error404", "Home");
                }

                var categories = _unitOfWork.Categories.Find(c => c.Id == lestcategories.CategoryId).FirstOrDefault();
                ViewBag.Categories = categories;

                var mainCategories = _unitOfWork.MainCategories.Find(c => c.Id == lestcategories.MainCategoryId).FirstOrDefault();
                ViewBag.MainCategories = mainCategories;
                ViewBag.LestCategories = lestcategories;
                ViewBag.BrandId = brand;
                if (CultureHelper.GetLang() == Enums.Lang.Ar)
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(lestcategories.Name, "المنتجات").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(lestcategories.Name, "المنتجات").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(lestcategories.Name, "المنتجات").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(lestcategories.Name, "المنتجات").Title;

                }
                else
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(lestcategories.NameEn, "Products").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(lestcategories.NameEn, "Products").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(lestcategories.NameEn, "Products").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(lestcategories.NameEn, "Products").Title;

                }
            }
            var brands = _unitOfWork.Brands.FindAll();
            var li = new List<SelectListItem>();
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                li.Add(new SelectListItem { Text = "الكل ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.Name, Value = brand.Id.ToString() }));

            }
            else
            {
                li.Add(new SelectListItem { Text = "All ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.NameEn, Value = brand.Id.ToString() }));

            }
            ViewData["brands"] = li;
            return View();
        }

        public ActionResult Brands(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");

                int categoryId;


                if (!int.TryParse(tmpStr[0], out categoryId))
                    return RedirectToAction("Error404", "Home");
                var categories = _unitOfWork.Brands.Find(c => c.Id == categoryId).FirstOrDefault();
                if (categories == null)
                {
                    return RedirectToAction("Error404", "Home");
                }


                ViewBag.Brands = categories;

                if (CultureHelper.GetLang() == Enums.Lang.Ar)
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Title;

                }
                else
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Title;

                }

            }

            return View();
        }
        #endregion

        #region MainCategory

        [HttpPost]
        public JsonResult SaveMainCategory(MainCategory mainCategory)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (mainCategory.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddMainCategory"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }



            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditMainCategory"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            if (_unitOfWork.MainCategories.FindAll().Any(c => c.Name == mainCategory.Name & c.Id != mainCategory.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.MainCategories.Add(mainCategory);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllMainCategories()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.MainCategories.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.Name,

                r.NameEn,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public static List<MainCategory> GetMainCategoryList()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.MainCategories.FindAll().ToList();
        }
        private bool CanDelete(MainCategory mainCategory)
        {
            var categories = _unitOfWork.Categories.FindAll().Where(c => c.MainCategoryId == mainCategory.Id);
            if (categories.Any())
            {
                return false;
            }
            var products = _unitOfWork.Products.FindAll().Where(c => c.MainCategoryId == mainCategory.Id);

            if (products.Any())
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public JsonResult GetMainCategory(int id)
        {
            var clientMessage = new ClientMessage<MainCategory>();
            var categories = _unitOfWork.MainCategories.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteMainCategory(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "DeleteMainCategory"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.MainCategories.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.MainCategories.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetCategoryByMainId(int id)
        {
            var clientMessage = new ClientMessage<IQueryable>();

            var categories = _unitOfWork.Categories.Find(c => c.MainCategoryId == id);

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLestCategoriesByCategoryId(int id)
        {
            var clientMessage = new ClientMessage<IQueryable>();

            var categories = _unitOfWork.LestCategories.Find(c => c.CategoryId == id);

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Category
        [HttpPost]
        public JsonResult SaveCategory(Category category)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (category.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddCategory"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditCategory"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            if (_unitOfWork.Categories.FindAll().Any(c => c.Name == category.Name & c.MainCategoryId == category.MainCategoryId & c.Id != category.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.Categories.Add(category);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllCategories()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Categories.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.MainCategoryId,
                r.Name,
                r.NameEn,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        public static List<MainCategory> GetAllCategories2()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            List<MainCategory> categories = _unitOfWork.MainCategories.FindAll().ToList();

            //clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            //clientMessage.ReturnedData = categories;
            return categories;
            //return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public static List<Category> GetCategoriesByMainCategoryId(int maincategoryId)
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.Categories.Find(c => c.MainCategoryId == maincategoryId).ToList();

        }
        public static List<LestCategory> GetLestCategoriesByCategoryId(int categoryId)
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.LestCategories.Find(c => c.CategoryId == categoryId).ToList();

        }

        private static bool CanDelete(Category category)
        {
            var products = _unitOfWork.Products.FindAll().Where(c => c.CategoryId == category.Id);

            if (products.Any())
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public JsonResult GetCategory(int id)
        {
            var clientMessage = new ClientMessage<Category>();
            var categories = _unitOfWork.Categories.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteCategory(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "DeleteCategory"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Categories.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.Categories.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Brands

        [HttpPost]
        public JsonResult SaveBrand(Brand brand)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (brand.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddBrand"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditBrand"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            if (_unitOfWork.Brands.FindAll().Any(c => c.Name == brand.Name && c.Id != brand.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.Brands.Add(brand);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllBrands()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Brands.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.Name,
                r.NameEn,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        private bool CanDelete(Brand brand)
        {
            var products = _unitOfWork.Products.FindAll().Where(c => c.BrandId == brand.Id);

            if (products.Any())
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public JsonResult GetBrand(int id)
        {
            var clientMessage = new ClientMessage<Brand>();
            var categories = _unitOfWork.Brands.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteBrand(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "DeleteBrand"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Brands.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.Brands.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Products

        public static List<Brand> GetAlBrands()
        {
            return _unitOfWork.Brands.FindAll().ToList();
        }


        public ActionResult GetBrandProducts(string id, string categoryId)
        {

            var brandId = Convert.ToInt32(id);
            @ViewBag.BrandId = brandId;

            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");

                int category;
                int brand;
                if (!int.TryParse(id, out brand))
                    return RedirectToAction("Error404", "Home");
                if (!int.TryParse(tmpStr[0], out category))
                    return RedirectToAction("Error404", "Home");
                var categories = _unitOfWork.Categories.Find(c => c.Id == category).FirstOrDefault();
                if (categories == null)
                {
                    return RedirectToAction("Error404", "Home");
                }

                var mainCategories = _unitOfWork.MainCategories.Find(c => c.Id == categories.MainCategoryId).FirstOrDefault();
                ViewBag.MainCategories = mainCategories;
                ViewBag.Categories = categories;

                List<Product> products = new List<Product>();
                products = _unitOfWork.Products.Find(c => c.CategoryId == category && c.BrandId == brand).ToList();
                ViewBag.products = products;
            }

            var brands = _unitOfWork.Brands.FindAll();
            var li = new List<SelectListItem>();
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                li.Add(new SelectListItem { Text = "الكل ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.Name, Value = brand.Id.ToString() }));

            }
            else
            {
                li.Add(new SelectListItem { Text = "All ", Value = "0" });
                li.AddRange(brands.Select(brand => new SelectListItem { Text = brand.NameEn, Value = brand.Id.ToString() }));

            }

            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("الماركات", "المنتجات").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("Brands", "Products").Title;

            }

            ViewData["brands"] = li;
            return RedirectToAction("CategoryProduct", "Product");


        }

        [HttpPost]
        public JsonResult SaveProduct(Product product)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            bool isNew = product.Id <= 0;

            Product oldproduct = null;
            if (isNew)
            {
                product.Time = DateTime.Now;
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddProduct"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                oldproduct = _unitOfWork.Products.FindById(product.Id);
                product.Time = oldproduct.Time;

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditProduct"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

                if (_unitOfWork.Products.FindAll().Any(c => c.Name == product.Name && c.Id != product.Id))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }


            var isSaved = _unitOfWork.Products.Add(product);
            if (isSaved)
            {
                var productImage = new ProductImage
                {
                    ImageUrl = product.ImageUrl,
                    ProductId = product.Id,
                    Berif = product.Berif

                };

                if (isNew)
                {

                    _unitOfWork.ProductImages.Add(productImage);




                }
                else
                {
                    productImage = _unitOfWork.ProductImages.Find(i => i.ImageUrl == oldproduct.ImageUrl).FirstOrDefault();
                    if (productImage != null)
                    {
                        productImage.ImageUrl = product.ImageUrl;
                        productImage.Berif = product.Berif;
                        _unitOfWork.ProductImages.Add(productImage);
                    }

                }
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProductCount(Product product)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();




            var oldproduct = _unitOfWork.Products.FindById(product.Id);

            oldproduct.AvailableQuntaty = product.AvailableQuntaty;



            var isSaved = _unitOfWork.Products.Add(oldproduct);
            if (isSaved)
            {

                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllproducts()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var products = _unitOfWork.Products.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.Name,
                r.NameEn,
                r.MainCategoryId,
                r.Notes,
                r.NotesEn,
                r.SalePrice,
                r.AvailableQuntaty,
                r.Berif,
                r.BerifEn,
                r.BrandId,
                r.CategoryId,
                r.Details,
                r.DetailsEn,
                r.BestSeller,
                r.DiscountPercent,
                r.DiscountPrice,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = products;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllproductsByLestCategoryId(int id)
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var products = _unitOfWork.Products.FindAll().Where(p => p.LestCategoryId == id).ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.Name,
                r.NameEn,
                r.MainCategoryId,
                r.Notes,
                r.NotesEn,
                r.SalePrice,
                r.AvailableQuntaty,
                r.Berif,
                r.BerifEn,
                r.BrandId,
                r.CategoryId,
                r.Details,
                r.DetailsEn,
                r.BestSeller,
                r.DiscountPercent,
                r.DiscountPrice,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = products;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        public static List<Product> HomeProducts()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.Products.FindAll().Where(c => c.IsHome == true).ToList();
        }

        public static List<Product> BestSellerProducts()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.Products.FindAll().Where(c => c.BestSeller == true).ToList();
        }

        [HttpGet]
        public JsonResult GetAllproductsByMainCategory()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.MainCategories.FindAll();
            var categoryProducts = new List<CategoryProducts>();
            foreach (var mainCategory in categories)
            {
                var products = new List<Product>();
                products = _unitOfWork.Products.FindAll().ToList().Where(p => p.MainCategoryId == mainCategory.Id).ToList();

                categoryProducts.Add(new CategoryProducts
                {
                    MainCategory = mainCategory,
                    Products = products
                });

            }

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categoryProducts.AsQueryable();
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public static List<Product> GetRelatedProduct(int productid)
        {
            var unitOfWork = new UnitOfWork();
            List<Product> products = new List<Product>();
            var product = unitOfWork.Products.FindById(productid);
            //&& p.Name.Contains(product.Name)
            products =
                unitOfWork.Products.Find(p => p.CategoryId == product.CategoryId)
                    .Take(10).ToList();
            return products;
        }


        private bool CanDelete(Product product)
        {

            var enquiries = _unitOfWork.Enquiries.FindAll().Where(c => c.ProductId == product.Id);

            if (enquiries.Any())
            {
                return false;
            }

            return true;
        }

        [HttpGet]
        public JsonResult GetProduct(int id)
        {
            var clientMessage = new ClientMessage<Product>();
            var categories = _unitOfWork.Products.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "DeleteProduct"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Products.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var productShow = _unitOfWork.ProductShows.Find(c => c.ProductId == id).FirstOrDefault();
                if (productShow != null)
                {
                    _unitOfWork.ProductShows.Remove(productShow);
                }

                var productImage = _unitOfWork.ProductImages.FindAll().Where(c => c.ProductId == id);
                foreach (var image in productImage)
                {
                    _unitOfWork.ProductImages.Remove(image);
                }
                var isDeleted = _unitOfWork.Products.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }

        public static List<Product> GetProductsByMainCategoryId(int mainCategoryId)
        {
            var unitOfWork = new UnitOfWork();
            List<Product> products = new List<Product>();
            products = unitOfWork.Products.Find(c => c.MainCategoryId == mainCategoryId).OrderByDescending(c => c.Id).Take(10).ToList();

            return products;
        }

        public static List<Product> GetProductsByCategoryId(int CategoryId, int BrandId)
        {
            var unitOfWork = new UnitOfWork();
            List<Product> products = new List<Product>();
            if (BrandId > 0)
            {


                products = unitOfWork.Products.Find(c => c.CategoryId == CategoryId && c.BrandId == BrandId).ToList();
            }
            else
            {
                products = unitOfWork.Products.Find(c => c.CategoryId == CategoryId).ToList();

            }


            return products;
        }

        public static List<Product> GetProductsByLestCategoryId(int LestCategoryId, int BrandId)
        {
            var unitOfWork = new UnitOfWork();
            List<Product> products = new List<Product>();
            if (BrandId > 0)
            {
                products = unitOfWork.Products.Find(c => c.LestCategoryId == LestCategoryId && c.BrandId == BrandId).ToList();
            }
            else
            {
                products = unitOfWork.Products.Find(c => c.LestCategoryId == LestCategoryId).ToList();
            }


            return products;
        }


        public static List<Product> GetProductsByBrandId(int BrandId)
        {
            var unitOfWork = new UnitOfWork();
            List<Product> products = new List<Product>();
            if (BrandId > 0)
            {
                products = unitOfWork.Products.Find(c => c.BrandId == BrandId).ToList();
            }



            return products;
        }

        #endregion

        #region Enquiries

        [HttpPost]
        public JsonResult SaveEnquiry(Enquiry enquiry)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            bool isSaved = false;
            if (enquiry.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddEnquiry"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
                enquiry.Time = DateTime.Now;
                isSaved = _unitOfWork.Enquiries.Add(enquiry);

            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditEnquiry"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

                var enquie = _unitOfWork.Enquiries.FindFirstOrDefault(enquiry.Id);
                if (enquie != null)
                {
                    enquie.Question = enquiry.Question;
                    enquie.Replay = enquiry.Replay;
                    enquie.IsActive = enquiry.IsActive;
                    isSaved = _unitOfWork.Enquiries.Add(enquie);

                }

            }

            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllEnquiries()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var products = _unitOfWork.Enquiries.FindAll().ToList().Select(r => new
            {
                r.Id,

                r.Email,
                r.IsActive,
                r.Mobile,
                r.ProductId,
                r.Replay,
                r.Time,
                r.UserName,
                r.UserNameEn

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = products;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEnquiriesByProductId(int id)
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var products = _unitOfWork.Enquiries.FindAll().Where(c => c.ProductId == id).ToList().Select(r => new
            {
                r.Id,
                r.Question,
                r.Email,
                r.IsActive,
                r.Mobile,
                r.ProductId,
                r.Replay,
                r.Time,
                r.UserNameEn,
                r.UserName
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = products;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public static List<Enquiry> GetProductEnquiries(int productId)
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.Enquiries.Find(e => e.ProductId == productId && e.IsActive == true).ToList();
        }
        [HttpGet]
        public JsonResult GetEnquiry(int id)
        {
            var clientMessage = new ClientMessage<Enquiry>();
            var categories = _unitOfWork.Enquiries.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود" )   };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteEnquiry(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "DeleteEnqury"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Enquiries.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.Enquiries.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region ProductImages

        [HttpPost]
        public JsonResult SaveImageList(List<ProductImage> product)
        {
            var clientMessage = new ClientMessage<string>();
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddProductImage"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            foreach (var isSaved in product.Select(productImage => _unitOfWork.ProductImages.Add(productImage)))
            {
                if (isSaved)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);

                }
            }

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveImages(ProductImage product)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (product.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddProductImage"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditProductImage"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

                //if (_unitOfWork.ProductImages.FindAll().Any(c => c.ImageUrl == product.ImageUrl && c.Id != product.Id))
                //{
                //    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                //    clientMessage.ReturnedData = null;
                //    clientMessage.ClientMessageContent = new List<string> { "الصورة  مضافة من قبل " };
                //    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                //}
            }
            var isSaved = _unitOfWork.ProductImages.Add(product);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllproductImages()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var products = _unitOfWork.ProductImages.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.ProductId,

                r.Berif,
                r.BerifEn,
                ProductName = _unitOfWork.Products.FindById(r.ProductId)

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = products;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllProductImagesByProductId(int id)
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var products = _unitOfWork.ProductImages.FindAll().Where(p => p.ProductId == id).ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.ProductId,

                r.BerifEn,
                r.Berif,
                ProductName = _unitOfWork.ProductImages.FindById(r.ProductId)

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = products;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }


        public static List<ProductImage> ProductProductImages(int id)
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.ProductImages.FindAll().Where(p => p.ProductId == id).ToList();

        }

        [HttpPost]
        public JsonResult GetAllProductImagesByProductIdPage(Pager pager)
        {
            int pagingSize = 20;
            var clientMessage = new ClientMessage<IQueryable>();
            List<int> totalIds =
                _unitOfWork.ProductImages.FindAll().Where(ar => ar.ProductId == pager.Id).OrderByDescending(ar => ar.Id)
                           .Take(pager.PageNum * pagingSize).Select(ar => ar.Id).ToList();

            if (totalIds.Count < pager.PageNum * pagingSize)
                pagingSize = totalIds.Count % pagingSize;

            List<int> requiredIds = totalIds.OrderBy(i => i).Take(pagingSize).ToList();

            var products = _unitOfWork.ProductImages.FindAll().Where(ar => requiredIds.Contains(ar.Id)).ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.ProductId,

                r.Berif,
                r.BerifEn,
                ProductName = _unitOfWork.Products.FindById(r.ProductId)

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = products;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductImage(int id)
        {
            var clientMessage = new ClientMessage<ProductImage>();
            var categories = _unitOfWork.ProductImages.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProductImage(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "DeleteProductImage"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.ProductImages.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.ProductImages.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود" )  };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Delevery

        [HttpPost]
        public JsonResult SaveDelevery(delivery delivery)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (delivery.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "AddDelevery"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditDelevery"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            if (_unitOfWork.Deliveries.FindAll().Any(c => c.Name == delivery.Name & c.Id != delivery.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.Deliveries.Add(delivery);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllDeleveries()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Deliveries.FindAll().ToList().Select(r => new
            {
                r.Id,

                r.Name,
                r.NameEn,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public static List<delivery> GetDeliveriesList()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.Deliveries.FindAll().ToList();
        }
        private bool CanDelete(delivery delivery)
        {
            var Carts = _unitOfWork.Carts.FindAll().Where(c => c.DeliveryId == delivery.Id);
            if (Carts.Any())
            {
                return false;
            }

            return true;
        }

        [HttpGet]
        public JsonResult Getdelivery(int id)
        {
            var clientMessage = new ClientMessage<delivery>();
            var categories = _unitOfWork.Deliveries.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Deletedelivery(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "EditDelevery"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Deliveries.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.Deliveries.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى")
 };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }

        #endregion


        #region LestLestCategory
        [HttpPost]
        public JsonResult SaveLestCategory(LestCategory lestCategory)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (lestCategory.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "ManageLestCategories"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "ManageLestCategories"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            if (_unitOfWork.LestCategories.FindAll().Any(c => c.Name == lestCategory.Name & c.CategoryId == lestCategory.CategoryId & c.Id != lestCategory.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.LestCategories.Add(lestCategory);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLestCategories()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var LestCategories = _unitOfWork.LestCategories.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.MainCategoryId,
                r.CategoryId,
                r.Name,
                r.NameEn,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = LestCategories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }




        private bool CanDelete(LestCategory LestCategory)
        {
            var products = _unitOfWork.Products.FindAll().Where(c => c.LestCategoryId == LestCategory.Id);

            if (products.Any())
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public JsonResult GetLestCategory(int id)
        {
            var clientMessage = new ClientMessage<LestCategory>();
            var lestCategories = _unitOfWork.LestCategories.FindFirstOrDefault(id);
            if (lestCategories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = lestCategories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteLestCategory(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Product", "ManageLestCategories"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> {CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية")  };

            }
            if (id > 0)
            {
                var lestCategories = _unitOfWork.LestCategories.FindFirstOrDefault(id);
                if (lestCategories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.LestCategories.Remove(lestCategories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult SaveRating(Rating rating)
        {
            rating.Time = DateTime.Now;
            var clientMessage = new ClientMessage<string>();

            var isSaved = _unitOfWork.Ratings.Add(rating);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveRates(int productId, int rate)
        {
            Rating rating = new Rating();
            rating.Rate = rate;
            rating.ProductId = productId;
            rating.UserId = WebSecurity.CurrentUserId;
            rating.Time = DateTime.Now;

            if (_unitOfWork.Ratings.FindAll().FirstOrDefault(r => r.ProductId == productId && r.UserId == WebSecurity.CurrentUserId) != null)
            {
                return Content("تم تسجيل تقييمك من قبل", "text/html");
            }

            if (_unitOfWork.Ratings.Add(rating))
            {
                return Content("تم حفظ ", "text/html");
            }
            else
            {
                return Content("خطا يجب مراجعة  ", "text/html");
            }


        }

        #endregion

    }
}
