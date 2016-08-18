using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.EStore;
using Domain.EStore.Repositories;
using Microsoft.Web.WebPages.OAuth;
using Ui.EStore.Helpers;
using Ui.EStore.Models;
using System.Net;
using System.Net.Mail;
using WebMatrix.WebData;

namespace Ui.EStore.Controllers
{
    public class HomeController : BaseController
    {
        readonly IUnitOfWork _unitOfWork = new UnitOfWork();
        readonly UserSession _userSession = new UserSession();

        // GET: /Home/SetCulture
        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
            {
                cookie.Value = culture; // update cookie value
                //UserSession _userSession = new UserSession();
                //var languageserver = new LanguageService(new UnitOfWork());
                //var language = languageserver.GetLanguageByLangType(culture);
                //if (language==null)
                //{
                //    languageserver.SaveLanguage(new Language
                //    {
                //        LanguageName = CultureHelper.
                //    });
                //}
                //_userSession.UserLanguage = languageserver.GetLanguageByLangType(culture);

            }

            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        #region MyRegion

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult NewFAQ()
        {
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "NewFAQ"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

      

        public ActionResult EditFAQ()
        {
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditFAQ"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }


        #endregion
        #region ActionResult



        public ActionResult MyCart()
        {
            var myCart = _userSession.MyCartPrduct;
            ViewBag.MyCart = myCart;
            return View();
        }

         [HttpPost]
        public JsonResult SaveMyCartDetails(List<MyCartDetails> myCartDetails)
        {
            var clientMessage = new ClientMessage<string>();
                var myCart = _userSession.MyCartPrduct;
                if (myCart.MyCartDetailse == null)
                {
                    myCart.MyCartDetailse = new List<MyCartDetails>();
                }

            myCart.MyCartDetailse = myCartDetails;

            if (Request.IsAuthenticated)
            {
                var carts =
                    _unitOfWork.Carts.Find(
                        c => c.IsShow == false && c.CustomerId == WebSecurity.CurrentUserId && c.Status == null)
                        .FirstOrDefault();



                if (carts == null)
                {
                    carts = new Cart {CustomerId = WebSecurity.CurrentUserId, Time = DateTime.Now, Total = 0};
                    _unitOfWork.Carts.Add(carts);
                }
                else
                {
                    var OldcartDetails = _unitOfWork.CartDetails.FindAll().Where(c => c.CartId == carts.Id);
                    foreach (var oldcartDetail in OldcartDetails)
                    {
                        _unitOfWork.CartDetails.Remove(oldcartDetail);
                    }
                }

                foreach (var myCartDetailse in myCart.MyCartDetailse)
                {
                    var mycartdetails = new CartDetail
                    {
                        CartId = carts.Id,
                        Price = myCartDetailse.Price,
                        ProductId = myCartDetailse.ProductId,

                        Quntity = myCartDetailse.Quntaty,
                        Total = myCartDetailse.Total
                    };



                    if (_unitOfWork.CartDetails.IsValid(mycartdetails).IsValid)
                    {
                        _unitOfWork.CartDetails.Add(mycartdetails);
                        //  productdetails.Id = mycartdetails.Id;
                        var cartproduct =
                            _unitOfWork.Products.Find(p => p.Id == mycartdetails.ProductId).FirstOrDefault();
                        if (cartproduct != null)
                        {
                            cartproduct.AvailableQuntaty -= 1;
                            _unitOfWork.Products.Add(cartproduct);
                        }
                    }



                  

                }
                _userSession.MyCartPrduct = myCart;
            }
            else
            {
                
                _userSession.MyCartPrduct = myCart;
            }

            ViewBag.MyCart = myCart;



            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddToMyCart(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                int productId;
                if (!int.TryParse(id, out productId))
                    return RedirectToAction("Error404", "Home");
                var product = _unitOfWork.Products.FindById(productId);
                if (product == null)
                {
                    return RedirectToAction("Error404", "Home");
                }



                ViewBag.product = product;

                var priceafter = product.SalePrice;
                var priceaftrerPercent = product.DiscountPrice;
                if (product.DiscountPrice > 0)
                {
                    if (product.DiscountPercent)
                    {

                        priceafter = product.SalePrice - (priceaftrerPercent * (product.SalePrice) / 100);

                    }
                    else
                    {

                        priceaftrerPercent = 100 - (((product.SalePrice - priceaftrerPercent) / product.SalePrice) * 100);

                        priceafter = product.SalePrice - (priceaftrerPercent * (product.SalePrice) / 100);
                    }
                }
                var myCart = _userSession.MyCartPrduct;
                if (myCart.MyCartDetailse == null)
                {
                    myCart.MyCartDetailse = new List<MyCartDetails>();
                }

                var productdetails = new MyCartDetails
                {

                    ProductId = product.Id,
                    Price = priceafter,
                    Quntaty = 1,
                    AvilabelQuntaty = product.AvailableQuntaty,
                    ProductName = product.Name,
                    Total = priceafter
                };

                if (Request.IsAuthenticated)
                {
                    var carts =
                        _unitOfWork.Carts.Find(
                            c => c.IsShow == false && c.CustomerId == WebSecurity.CurrentUserId && c.Status == null).FirstOrDefault();
                    if (carts == null)
                    {
                        carts = new Cart { CustomerId = WebSecurity.CurrentUserId, Time = DateTime.Now, Total = 0 };
                        _unitOfWork.Carts.Add(carts);
                    }

                    var mycartdetails = new CartDetail
                    {
                        CartId = carts.Id,
                        Price = productdetails.Price,
                        ProductId = productdetails.ProductId,

                        Quntity = productdetails.Quntaty,
                        Total = productdetails.Total
                    };

                    if (_unitOfWork.CartDetails.IsValid(mycartdetails).IsValid)
                    {
                        _unitOfWork.CartDetails.Add(mycartdetails);
                        productdetails.Id = mycartdetails.Id;
                        var cartproduct = _unitOfWork.Products.Find(p => p.Id == mycartdetails.ProductId).FirstOrDefault();
                        if (cartproduct != null)
                        {
                            cartproduct.AvailableQuntaty -= 1;
                            _unitOfWork.Products.Add(cartproduct);
                        }
                    }


                    myCart.MyCartDetailse.Add(productdetails);
                    _userSession.MyCartPrduct = myCart;

                }
                else
                {
                    if (myCart.MyCartDetailse.Count > 0)
                    {
                        productdetails.Id = myCart.MyCartDetailse.Count + 1;
                    }
                    else
                    {
                        productdetails.Id = 1;

                    }

                    myCart.MyCartDetailse.Add(productdetails);
                    _userSession.MyCartPrduct = myCart;
                }
                ViewBag.MyCart = myCart;
            }


            return RedirectToAction("MyCart", "Home");
        }

        [HttpPost]
        public JsonResult SaveMyCart(MyCartPrduct myCart)
        {
            var clientMessage = new ClientMessage<string>();
            _userSession.MyCartPrduct.CustomerId = WebSecurity.CurrentUserId;
            _userSession.MyCartPrduct.DeliveryId = myCart.DeliveryId;
            _userSession.MyCartPrduct.Total = myCart.Total;
            _userSession.MyCartPrduct.Time = DateTime.Now.ToString();
            _userSession.MyCartPrduct.GetwayId = myCart.GetwayId;
            if (!Request.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { "يجب التسجيل اولا  " };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            var cart = _unitOfWork.Carts.Find(
                            c => c.IsShow == false && c.CustomerId == WebSecurity.CurrentUserId && c.Status == null).FirstOrDefault();
            if (cart != null)
            {
                cart.Time = DateTime.Now;
                cart.Total = myCart.Total;
                cart.CustomerId = WebSecurity.CurrentUserId;
                cart.DeliveryId = myCart.DeliveryId;
                cart.GetwayId = myCart.GetwayId; ;
                cart.IsShow = true;
            }

            if (_unitOfWork.Carts.IsValid(cart).IsValid)
            {
                _unitOfWork.Carts.Add(cart);
                _userSession.MyCartPrduct = new MyCartPrduct();



            }

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string> { "تم حفظ الطلب  " };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
            // return View();
        }

        [HttpGet]
        public JsonResult DeleteFromCart(string id)
        {
            var clientMessage = new ClientMessage<MyCartPrduct>();

            if (!string.IsNullOrEmpty(id))
            {


                int productId;
                if (int.TryParse(id, out productId))
                {
                    var mycartdetails = _userSession.MyCartPrduct.MyCartDetailse.FirstOrDefault(c => c.Id == productId);

                    if (mycartdetails != null)
                    {
                        _unitOfWork.CartDetails.Remove(mycartdetails.Id);
                        _userSession.MyCartPrduct.MyCartDetailse.Remove(mycartdetails);

                    }
                }
            }
            var myCart = _userSession.MyCartPrduct;
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = myCart;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMyCart()
        {
            var clientMessage = new ClientMessage<MyCartPrduct>();
            var myCart = _userSession.MyCartPrduct;
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = myCart;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddSlider()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Home", "AddSlider"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditSlider()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Home", "AddSlider"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        // GET: /Home/Error403
        public ActionResult Error403()
        {
            return View();
        }

        //
        // GET: /Home/Error404
        public ActionResult Error404()
        {
            return View();
        }

        //
        // GET: /Home/

        public ActionResult LoginTest()
        {
            return View();
        }

        public ActionResult Index()
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("الصفحة الرئيسية", "الصفحة الرئيسية").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("الصفحة الرئيسية", "الصفحة الرئيسية").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("الصفحة الرئيسية", "الصفحة الرئيسية").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("الصفحة الرئيسية", "الصفحة الرئيسية").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("Home", "Home").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("Home", "Home").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("Home", "Home").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("Home", "Home").Title;

            }

            return View();
        }

        public ActionResult OfferDetails(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] tmpStr = id.Split('-');
                if (tmpStr.Count() < 2)
                    return RedirectToAction("Error404", "Home");

                int productId;
                if (!int.TryParse(tmpStr[0], out productId))
                    return RedirectToAction("Error404", "Home");
                var slider = _unitOfWork.SliderImages.FindById(productId);
                if (slider == null)
                {
                    return RedirectToAction("Error404", "Home");
                }



                ViewBag.title = slider.Name;
                ViewBag.slider = slider;
                if (CultureHelper.GetLang() == Enums.Lang.Ar)
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(slider.Name, "العروض").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(slider.Name, "العروض").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(slider.Name, "العروض").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(slider.Name, "العروض").Title;

                }
                else
                {
                    ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue(slider.NameEn, "Offers").Keywords;
                    ViewBag.Description = CultureHelper.GetSeoUserLanguageValue(slider.NameEn, "Offers").Description;
                    ViewBag.Author = CultureHelper.GetSeoUserLanguageValue(slider.NameEn, "Offers").Author;
                    ViewBag.Title = CultureHelper.GetSeoUserLanguageValue(slider.NameEn, "Offers").Title;

                }

                ViewBag.RootUrl = ConfigurationManager.AppSettings["RootUrl"];
            }
            return View();
        }

        #endregion


        #region Customer

        public static Customer GetLoginCustomer(int userId)
        {
            //if ()
            //{

            //}
            return null;
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(CustomerLogin customer)
        {
            if (ModelState.IsValid)
            {


                var regUser =
                  _unitOfWork.Customers.FindAll().ToList().FirstOrDefault(
                       u =>
                       u.Email == customer.Email &&
                       u.Password == Security.EncodePassword(customer.Password) && u.IsActive);
                if (regUser != null)
                {

                    Session["CustomerID"] = regUser.Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = CultureHelper.GetUserLanguageValue("عام", "بيانات الدخول خطا"); //"خطأ بيانات دخولك غير صحيحه";
                    return View("Login");

                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterCustomer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.ConfirmPassword != customer.Password)
                {
                    ModelState.AddModelError("Password", CultureHelper.GetUserLanguageValue("عام", "عدم تشابه كلمة المرور") );
                    return View();
                }
                var user = new Customer
                {
                    Email = customer.Email,
                    Password = Security.EncodePassword(customer.Password),
                    UserName = customer.UserName
                };
                var isValid = _unitOfWork.Customers.IsValid(user);
                if (isValid.IsValid)
                {



                    var isSaved = _unitOfWork.Customers.Add(user);
                    if (isSaved)
                    {
                        Session["CustomerID"] = user.Id;
                        return RedirectToAction("Index");
                    }

                }
                else
                {

                    foreach (var error in isValid.ValidationErrors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
            }
            return View();
        }

        #endregion

        #region SliderImage

        [
        HttpPost]
        public JsonResult SaveSliderImage(SliderImage sliderImage)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (sliderImage.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Home", "AddSlider"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Home", "EditSlider"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

                if (_unitOfWork.SliderImages.FindAll().Any(c => c.Name == sliderImage.Name && c.Id != sliderImage.Id))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            var isSaved = _unitOfWork.SliderImages.Add(sliderImage);
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
        public JsonResult GetAllSliderImages()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.SliderImages.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.Name,
                r.NameEn,

                r.OrderNum,
                r.Berif,
                r.BerifEn,
                r.Details,
                r.DetailsEn,
                r.SalePrice,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public static List<SliderImage> GetsSliderImages()
        {
            var unitOfWork = new UnitOfWork();

            return unitOfWork.SliderImages.FindAll().ToList();
        }
        private bool CanDelete(SliderImage sliderImage)
        {


            return true;
        }

        [HttpGet]
        public JsonResult GetSliderImage(int id)
        {
            var clientMessage = new ClientMessage<SliderImage>();
            var categories = _unitOfWork.SliderImages.FindFirstOrDefault(id);
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
        public JsonResult DeleteSliderImage(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Home", "DeleteSlider"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.SliderImages.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.SliderImages.Remove(categories);
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

        #region Emails

        [HttpPost]
        public JsonResult SaveEmail(SysEmail sysEmail)
        {
            var clientMessage = new ClientMessage<string>();




            if (_unitOfWork.SysEmails.FindAll().Any(c => c.Email == sysEmail.Email))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            sysEmail.IsActive = true;
            sysEmail.Time = DateTime.Now;
            var isSaved = _unitOfWork.SysEmails.Add(sysEmail);
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
        public JsonResult GetAllEmails()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.SysEmails.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.Email,
                r.IsActive,
                r.Time,
                r.Notes,
            
                r.UserName,


            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetEmail(int id)
        {
            var clientMessage = new ClientMessage<SliderImage>();
            var categories = _unitOfWork.SliderImages.FindFirstOrDefault(id);
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
        public JsonResult DeleteEmail(string email)
        {
            var clientMessage = new ClientMessage<string>();



            if (string.IsNullOrEmpty(email))
            {
                var categories = _unitOfWork.SysEmails.Find(e => e.Email.ToLower().Trim() == email.ToLower().Trim()).FirstOrDefault();
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.SysEmails.Remove(categories);
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

        public JsonResult GetequestCartUnder()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var carts = _unitOfWork.Carts.FindAll().Where(c => c.Status == null).ToList().Select(r => new
            {
                r.Id,
                r.CustomerId,
                CustomerName = _unitOfWork.UserProfiles.FindById(r.CustomerId).UserName,
                Email = _unitOfWork.UserProfiles.FindById(r.CustomerId).Email,
                Phone = _unitOfWork.UserProfiles.FindById(r.CustomerId).Phone,
                Address = _unitOfWork.UserProfiles.FindById(r.CustomerId).Address

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = carts;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewDeleveredCarts()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var carts = _unitOfWork.Carts.FindAll().Where(c => c.Status == true && c.DeliveryState == null).ToList().Select(r => new
            {
                r.Id,
                r.CustomerId,
                CustomerName = _unitOfWork.UserProfiles.FindById(r.CustomerId).UserName,
                Email = _unitOfWork.UserProfiles.FindById(r.CustomerId).Email,
                Phone = _unitOfWork.UserProfiles.FindById(r.CustomerId).Phone,
                Address = _unitOfWork.UserProfiles.FindById(r.CustomerId).Address


            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = carts;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCartDetailsById(string id)
        {
            var cartId = Convert.ToInt32(id);
            var clientMessage = new ClientMessage<MyCartPrduct>();
            var myCart = new MyCartPrduct();
            var cart = _unitOfWork.Carts.Find(c => c.Id == cartId).FirstOrDefault();
            if (cart != null)
            {
                myCart.CartId = cart.Id;
                myCart.CustomerId = cart.CustomerId;
                var customer = _unitOfWork.UserProfiles.Find(c => c.Id == cart.CustomerId).FirstOrDefault();
                if (customer != null) {
                    myCart.CustomerName = customer.UserName;
                myCart.CustomerAddress = customer.Address;
            }
                myCart.DeliveryId = Convert.ToInt32(cart.DeliveryId);
                var delivery = _unitOfWork.Deliveries.Find(c => c.Id == cart.DeliveryId).FirstOrDefault();
                if (delivery != null)
                    myCart.DeliveryName = delivery.Name;
                myCart.GetwayId = Convert.ToInt32(cart.GetwayId);
                myCart.Total = cart.Total;
                var getWay = _unitOfWork.GetWaies.Find(g => g.Id == cart.GetwayId).FirstOrDefault();
                if (getWay != null)
                    myCart.GetWayName = getWay.Name;

                myCart.Time = cart.Time.ToShortDateString();

                var details = _unitOfWork.CartDetails.Find(c => c.CartId == cart.Id);
                myCart.MyCartDetailse = new List<MyCartDetails>();
                foreach (var cartDetail in details)
                {
                    var det = new MyCartDetails
                    {
                        Id = cartDetail.Id,
                        CartId = cartDetail.CartId,
                        Price = cartDetail.Price,
                        ProductId = cartDetail.ProductId,
                        ProductName = _unitOfWork.Products.FindById(cartDetail.ProductId).Name,
                        Quntaty = cartDetail.Quntity,
                        Total = cartDetail.Total
                    };
                    myCart.MyCartDetailse.Add(det);
                }
            }

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = myCart;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovedCart(string id)
        {
            var cartId = Convert.ToInt32(id);
            var clientMessage = new ClientMessage<string>();

            var cart = _unitOfWork.Carts.Find(c => c.Id == cartId).FirstOrDefault();
            if (cart != null)
            {
                cart.Status = true;
                _unitOfWork.Carts.Add(cart);
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string> { "تم  الموافقة  " };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CancelCart(string id)
        {
            var cartId = Convert.ToInt32(id);
            var clientMessage = new ClientMessage<string>();

            var cart = _unitOfWork.Carts.Find(c => c.Id == cartId).FirstOrDefault();
            if (cart != null)
            {
                cart.Status = false;
                _unitOfWork.Carts.Add(cart);
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string> { "تم  الرفض  " };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleveriedCart(string id)
        {
            var cartId = Convert.ToInt32(id);
            var clientMessage = new ClientMessage<string>();
            var myCart = new MyCartPrduct();
            var cart = _unitOfWork.Carts.Find(c => c.Id == cartId).FirstOrDefault();
            if (cart != null)
            {
                cart.DeliveryState = true;
                _unitOfWork.Carts.Add(cart);
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string> { "تم  الموافقة  " };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CancelDeleveriedCart(string id)
        {
            var cartId = Convert.ToInt32(id);
            var clientMessage = new ClientMessage<string>();
            var myCart = new MyCartPrduct();
            var cart = _unitOfWork.Carts.Find(c => c.Id == cartId).FirstOrDefault();
            if (cart != null)
            {
                cart.DeliveryState = false;
                _unitOfWork.Carts.Add(cart);
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string> { "تم  الرفض  " };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
    }
}
