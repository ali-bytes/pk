using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Domain.EStore;
using Domain.EStore.Repositories;
using MRS.Models;
using Ui.EStore.Helpers;
using Ui.EStore.Models;

namespace Ui.EStore.Controllers
{
    public class AdminController : BaseController
    {

        readonly IUnitOfWork _unitOfWork = new UnitOfWork();



        // GET: /Admin/Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
                Response.Redirect("/Admin");

            return View();
        }

        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            Response.Redirect("/Admin/Login");
            return View();
        }

        public ActionResult LanguageManager()
        {

            return View();
        }

        //
        // GET: /Admin/ValidateUser
        public JsonResult ValidateUser(User usr)
        {
            var clientMessage = new ClientMessage<string>();

            var regUser =
                _unitOfWork.Users.FindAll().FirstOrDefault(
                    u =>
                    u.Email == usr.Email &&
                    u.Password == Security.EncodePassword(usr.Password) && u.IsActive == true);
            if (regUser != null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                Session["UserID"] = regUser.Id;
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;

                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "بيانات غير صحيحه") };// "خطأ بيانات دخولك غير صحيحه"

            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        // GET: /Admin/
        public ActionResult SeoManager()
        {


            return View();
        }
        public ActionResult Index()
        {
            Security.CheckLogin();

            return View();
        }

        public ActionResult AboutUs()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }
        public static HomeDetail TitleDetails()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.HomeDetails.FindById(6);
        }

        public ActionResult SetTitle()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }

        public ActionResult ContactUs()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }
        public ActionResult RequestUnderApproved()
        {

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "RequestUnderApproved"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }

        public ActionResult RequestApproved()
        {

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "RequestApproved"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }


        public JsonResult GetpageTools(PageLanguage page)
        {
            var clientMessage = new ClientMessage<IQueryable>();

            var unitOfWork = new UnitOfWork();


            List<TwoProps> usersList = new List<TwoProps>();

            var languagepage = unitOfWork.LanguagePages.Find(c => c.PageId == page.PageId && c.LanguageId == page.LanguageId).FirstOrDefault();
            if (languagepage != null)
            {


                usersList =
                    unitOfWork.LanguageTools.Find(c => c.LanguagePageId == languagepage.LanguagePageId).ToList()
                        .Select(r => new TwoProps
                        {
                            Id = r.LanguageToolId,
                            Name = unitOfWork.PageTools.FindById(r.ToolId).ToolName
                        }).ToList();
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = usersList.AsQueryable();
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSeopageTools(PageLanguage page)
        {
            var clientMessage = new ClientMessage<IQueryable>();

            var unitOfWork = new UnitOfWork();


            List<TwoProps> usersList = new List<TwoProps>();

            var languagepage = unitOfWork.LanguagePages.Find(c => c.PageId == page.PageId && c.LanguageId == page.LanguageId).FirstOrDefault();
            if (languagepage != null)
            {


                usersList =
                    unitOfWork.SeoLanguageTools.Find(c => c.LanguagePageId == languagepage.LanguagePageId).ToList()
                        .Select(r => new TwoProps
                        {
                            Id = r.LanguageToolId,
                            Name = unitOfWork.SeoPageTools.FindById(r.ToolId).ToolName
                        }).ToList();
            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = usersList.AsQueryable();
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SeoUrlMagerDataById(int id, int pageId)
        {
            var clientMessage = new ClientMessage<SeoUrlModel>();


            var unitOfWork = new UnitOfWork();
            var seourl = new SeoUrlModel();
            var speciality = unitOfWork.SeoManagers.Find(c => c.Id == id).FirstOrDefault();
            if (speciality != null)
            {
                seourl.LanguagePageId = speciality.LanguagePageId;
                seourl.SeoUrl = speciality.SeoUrl;
                seourl.ToolId = speciality.ToolId;
                seourl.UrlParameter = speciality.UrlParameter;
                seourl.ActionName = speciality.ActionName;
                seourl.ControllerName = speciality.ControllerName;
            }


            seourl.PageUrl = unitOfWork.SeoPageTools.FindById(seourl.ToolId).PageUrl;

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = seourl;

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetToolLanguageDataById(int id)
        {
            var clientMessage = new ClientMessage<SeoUrlModel>();


            var unitOfWork = new UnitOfWork();
            var seourl = new SeoUrlModel();
            var speciality =
                unitOfWork.LanguageTools.Find(c => c.LanguageToolId == id).FirstOrDefault();
            if (speciality != null)
            {
                seourl.LanguagePageId = speciality.LanguagePageId;
                seourl.SeoUrl = speciality.LanguageToolValue;
                seourl.ToolId = speciality.ToolId;
            }

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = seourl;

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSeoToolLanguageDataById(int id)
        {
            var clientMessage = new ClientMessage<SeoUrlModel>();


            var unitOfWork = new UnitOfWork();
            var seourl = new SeoUrlModel();
            var speciality =
                unitOfWork.SeoLanguageTools.Find(c => c.LanguageToolId == id).FirstOrDefault();
            if (speciality != null)
            {
                seourl.LanguagePageId = speciality.LanguagePageId;
                seourl.SeoUrl = speciality.SeoUrl;
                seourl.ToolId = speciality.ToolId;
                seourl.UrlParameter = speciality.UrlParameter;
                seourl.ActionName = speciality.ActionName;
                seourl.ControllerName = speciality.ControllerName;
                seourl.Keywords = speciality.Keywords;
                seourl.Title = speciality.Title;
                seourl.Author = speciality.Author;
                seourl.Description = speciality.Description;
            }
            seourl.PageUrl = unitOfWork.SeoPageTools.FindById(seourl.ToolId).PageUrl;

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = seourl;

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveSeo(SeoLanguageTool seoLanguageTool)
        {
            var clientMessage = new ClientMessage<string>();

            var unitOfWork = new UnitOfWork();


            var seo = unitOfWork.SeoLanguageTools.FindAll().FirstOrDefault(s => s.LanguageToolId == seoLanguageTool.LanguageToolId);




            if (seo == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "رابط غير موجود" };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            //var sseourl =
            //    unitOfWork.SeoLanguageTools.FindAll()
            //        .FirstOrDefault(s => s.SeoUrl.Trim().ToLower() == seoLanguageTool.SeoUrl.Trim().ToLower() && s.LanguageToolId != seo.Id);
            //if (sseourl != null)
            //{
            //    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
            //    clientMessage.ReturnedData = null;
            //    clientMessage.ClientMessageContent = new List<string>() { "رابط السيو مكرر يجب تغييره فى  " + sseourl.Title };
            //    return Json(clientMessage, JsonRequestBehavior.AllowGet);
            //}



            seo.Keywords = seoLanguageTool.Keywords;
            seo.Author = seoLanguageTool.Author;
            seo.Title = seoLanguageTool.Title;
            seo.Description = seoLanguageTool.Description;


            unitOfWork.SeoLanguageTools.Add(seo);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string>() { "تم الحفظ" };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveLanguageValue(SeoLanguageTool seoLanguageTool)
        {
            var clientMessage = new ClientMessage<string>();

            var unitOfWork = new UnitOfWork();


            var seo = unitOfWork.LanguageTools.FindAll().FirstOrDefault(s => s.LanguageToolId == seoLanguageTool.LanguageToolId);




            if (seo == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "رابط غير موجود" };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }



            seo.LanguageToolValue = seoLanguageTool.SeoUrl;


            unitOfWork.LanguageTools.Add(seo);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string>() { "تم الحفظ" };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveSeoUrl(SeoManager seoManager)
        {
            var clientMessage = new ClientMessage<string>();

            var unitOfWork = new UnitOfWork();


            var seo = unitOfWork.SeoManagers.FindAll().FirstOrDefault(s => s.Id == seoManager.Id) ?? new SeoManager();



            seo.ControllerName = seoManager.ControllerName;
            seo.ActionName = seoManager.ActionName;
            seo.SeoUrl = seoManager.SeoUrl;
            seo.UrlParameter = seoManager.UrlParameter;

            unitOfWork.SeoManagers.Add(seo);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string>() { "تم الحفظ" };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }



        #region AboutUs
        [HttpGet]
        public JsonResult GetAboutUs()
        {
            var clientMessage = new ClientMessage<HomeDetail>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };//"عفوا ليس لديك صلاحية "
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            var aboutUs = _unitOfWork.HomeDetails.FindById(1);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = aboutUs;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult GetTitle()
        {
            var clientMessage = new ClientMessage<HomeDetail>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };//"عفوا ليس لديك صلاحية "
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            var aboutUs = _unitOfWork.HomeDetails.FindById(6);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = aboutUs;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult SaveAboutUs(HomeDetail homeDetail)
        {
            
            var clientMessage = new ClientMessage<string>();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            var isSaved = _unitOfWork.HomeDetails.Add(homeDetail);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") }; //"تم حفظ البيانات"
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };//تعذر الحفظ ,اعد المحاولة او اتصل بالدعم الفنى "
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }



        #endregion

        #region SaveContactUs
        [HttpGet]
        public JsonResult GetContactUs()
        {
            var clientMessage = new ClientMessage<HomeDetail>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            var aboutUs = _unitOfWork.HomeDetails.FindById(3);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = aboutUs;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult SaveContactUs(HomeDetail homeDetail)
        {
            var clientMessage = new ClientMessage<string>();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditAboutUs"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            var isSaved = _unitOfWork.HomeDetails.Add(homeDetail);
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



        #endregion

        public JsonResult SendContactUs(string Name, string Email, string Phone, string website, string Address, string Message)
        {
            var clientMessage = new ClientMessage<string>();




            string FullMessage = "";
            FullMessage += @" <style>
            .row {
                width: 50%;
                height: 45px;
                margin: 10px auto;
                border: 1px solid #dedede;
                font-family: 'Droid Arabic Kufi';
                text-align: right;
                font-size: 14px;
                color: #808080;
            }

                .row p {
                    margin: 0px 10px 0px 0px;
                    padding: 10px 0px 0px 0px;
                }

                .row .title {
                    height: 45px;
                    width: 30%;
                    border-left: 1px solid #dedede;
                    float: right;
                }

                .row .content {
                    height: 45px;
                    float: right;
                }
        </style>";

            FullMessage +=
                " <div class=\"row\"><div class=\"title\"><p>الاسم</p></div><div class=\"content\"><p>" + Name + "</p></div></div><div class=\"row\"><div class=\"title\"><p>البريد الالكترونى</p></div><div class=\"content\"><p>" + Email + "</p></div></div><div class=\"row\">";
            FullMessage +=
                "   <div class=\"title\"><p>التليفون</p></div><div class=\"content\"><p>+" + Phone + "+</p></div></div><div class=\"row\"><div class=\"title\"><p>نشاط الشركة</p></div><div class=\"content\"><p>" + website + "</p></div></div>";
            FullMessage +=
                  "  <div class=\"row\"><div class=\"title\"><p>العنوان</p></div><div class=\"content\"><p>" + Address + "</p></div></div>";
            FullMessage +=
                "  <div class=\"row\" style=\"height:200px;\"><div class=\"title\" style=\"height:200px;\"><p>الرسالة</p></div><div class=\"content\" style=\"height:200px;width:65%\"><p>" + Message + " </p></div></div>";

            var sendTo = new MailAddress(ConfigurationManager.AppSettings["Address"]);
            var from = new MailAddress(Email);
            using (var MyMessage = new MailMessage(from, sendTo)
            {
                Subject = Name,
                Body = FullMessage,
                IsBodyHtml = true
            })
            {

                var emailClient = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    /*EnableSsl = true,*/
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                        ConfigurationManager.AppSettings["Account"],
                        ConfigurationManager.AppSettings["pswrd"]),
                    Timeout = 999999999
                };
                emailClient.Send(MyMessage);
                emailClient.Dispose();
            }

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }


        #region  Speciality



        [HttpPost]
        public JsonResult SaveSpeciality(Speciality speciality)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (speciality.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "AddSpeciality"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditSpeciality"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

                if (_unitOfWork.Specialities.FindAll().Any(c => c.Name == speciality.Name && c.Id != speciality.Id))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            var isSaved = _unitOfWork.Specialities.Add(speciality);
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
        public JsonResult GetAllSpecialitys()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Specialities.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.NameEn,
                r.Name,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }




        private bool CanDelete(Speciality speciality)
        {

            return true;
        }

        [HttpGet]
        public JsonResult GetSpeciality(int id)
        {
            var clientMessage = new ClientMessage<Speciality>();
            var categories = _unitOfWork.Specialities.FindFirstOrDefault(id);
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
        public JsonResult DeleteSpeciality(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "DeleteSpeciality"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Specialities.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.Specialities.Remove(categories);
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

        #region User

        public ActionResult AddUser()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "AddUser"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }
        public ActionResult EditUser()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditUser"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }

        [HttpPost]
        public JsonResult SaveUser(User user)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (user.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "AddUser"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
                user.Password = Security.EncodePassword(user.Password);
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditUser"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }



                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = Security.EncodePassword(user.Password);

                }
                else
                {
                    user.Password = _unitOfWork.Users.FindById(user.Id).Password;
                }
            }
            if (_unitOfWork.Users.FindAll().Any(c => c.UserName == user.UserName && c.Id != user.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.Users.Add(user);
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
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى")
 };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllUsers()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Users.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.Email,
                r.IsActive,
                r.RuleId,

                r.UserName,
                CanDelete = CanDelete(r)

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        private bool CanDelete(User user)
        {
            if (_unitOfWork.UserOperations.FindAll().Any(c => c.UserId == user.Id))
            {
                return false;
            }

            return true;
        }

        [HttpGet]
        public JsonResult GetUser(int id)
        {
            var clientMessage = new ClientMessage<User>();
            var categories = _unitOfWork.Users.FindFirstOrDefault(id);
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
        public JsonResult DeleteUser(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "DeleteUser"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Users.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.Users.Remove(categories);
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

        #region Rule

        public ActionResult AddRole()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "AddRole"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }
        public ActionResult EditRole()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditRole"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }

        [HttpPost]
        public JsonResult SaveRule(Rule rule)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (rule.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "Editrule"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { "عفوا ليس لديك صلاحية " };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "Editrule"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { "عفوا ليس لديك صلاحية " };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            if (_unitOfWork.Rules.FindAll().Any(c => c.Name == rule.Name && c.Id != rule.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { "الصلاحية مضافة  من قبل " };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.Rules.Add(rule);
            if (isSaved)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { "تم حفظ الصلاحية  " };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { "تعذر الحفظ ,اعد المحاولة او اتصل بالدعم الفنى " };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllRules()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Rules.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.IsActive,

                r.Name,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        private bool CanDelete(Rule rule)
        {

            if (_unitOfWork.Users.FindAll().Any(c => c.RuleId == rule.Id))
            {
                return false;
            }
            if (_unitOfWork.RulePrivileges.FindAll().Any(c => c.RuleId == rule.Id))
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public JsonResult GetRule(int id)
        {
            var clientMessage = new ClientMessage<Rule>();
            var categories = _unitOfWork.Rules.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { "تحقق من الصلاحية  " };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteRule(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "admin", "DeleteRule"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { "عفوا ليس لديك صلاحية " };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Rules.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { "تحقق من الصلاحية  " };

                }

                var isDeleted = _unitOfWork.Rules.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { "تم حذف الصلاحية " };
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { "تعذر الحفظ ,اعد المحاولة او اتصل بالدعم الفنى " };
                }
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { "تاكد من البيانات  " };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region MyRules

        public static List<Rule> GetSysRule()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            var rules = unitOfWork.Rules.FindAll().ToList();
            return rules;
        }

        public ActionResult Editrule()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditRole"))
            {
                return RedirectToAction("Error403", "Home");
            }


            return View();
        }

        #endregion

        #region FAQ

        public static List<FAQ> GetFAQsView()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.FAQs.FindAll().OrderBy(s => s.FAQId).ToList();
        }

        // GET: /SupportCenter/AddFAQToDb
        public JsonResult AddFAQToDb(FAQ faq)
        {

            var clientMessage = new ClientMessage<string>();

            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "NewFAQ"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "عفوا ليس لديك صلاحية " };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            faq.FAQCreateDate = DateTime.Now.ToUniversalTime(); ;




            var tmpService =
                _unitOfWork.FAQs.FindAll().FirstOrDefault(
                    cat =>
                    cat.FAQTitle == faq.FAQTitle);
            if (tmpService != null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "السؤال مضاف من قبل " };
            }

            _unitOfWork.FAQs.Add(faq);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        // GET: /SupportCenter/GetAllFAQs
        public JsonResult GetAllFAQs()
        {
            var clientMessage = new ClientMessage<IQueryable>();




            List<TwoProps> usersList;

            usersList = _unitOfWork.FAQs.FindAll().Select(
                            u =>
                            new TwoProps()
                            {

                                Id = u.FAQId,
                                Name = u.FAQTitle
                            }).ToList();


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = usersList.AsQueryable();
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        // GET: /TrainingServices/GetFAQ
        public JsonResult GetFAQ(int faqId)
        {
            var clientMessage = new ClientMessage<IQueryable>();
            //  Convert.ToDecimal(Session) == SYS_UserID for profile page


            var DownloadFile =
                _unitOfWork.FAQs.FindAll().Where(u => u.FAQId == faqId)
                           .Select(
                               us =>
                               new
                               {
                                   us.FAQCreateDate,
                                   us.FAQDetails,
                                   us.FAQDetailsEn,

                                   us.FAQId,
                                   us.FAQTitle,
                                   us.FAQTitleEn,



                               });
            if (!DownloadFile.Any())
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "السؤال غير موجود " };
            }
            else
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = DownloadFile;
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        // GET: /TrainingServices/EditFAQToDb
        public JsonResult EditFAQToDb(FAQ faq)
        {
            var clientMessage = new ClientMessage<string>();

            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditFAQ"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "عفوا ليس لديك صلاحية " };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }



            var tmpusr = _unitOfWork.FAQs.FindAll().FirstOrDefault(u => u.FAQId == faq.FAQId);
            if (tmpusr == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "السؤال غير موجود " };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }




            tmpusr.FAQDetails = faq.FAQDetails;

            tmpusr.FAQId = faq.FAQId;
            tmpusr.FAQTitle = faq.FAQTitle;


            _unitOfWork.FAQs.Add(tmpusr);
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string>() { "تم الحفظ" };
            return Json(clientMessage, JsonRequestBehavior.AllowGet);


        }

        // GET: /TrainingServices/DeleteFAQToDb
        public JsonResult DeleteFAQToDb(FAQ faq)
        {
            var clientMessage = new ClientMessage<string>();

            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditFAQ"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { "عفوا ليس لديك صلاحية " };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            var exsistCategory =
               _unitOfWork.FAQs.FindAll().FirstOrDefault(c => (c.FAQId == faq.FAQId));

            if (exsistCategory != null)
            {

                _unitOfWork.FAQs.Remove(exsistCategory);


            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = null;
            clientMessage.ClientMessageContent = new List<string>() { "" };

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //    GET: /Admin/GetRoleDetailsByRoleId
        public JsonResult GetRoleDetailsByRoleId(int roleId)
        {
            var clientMessage = new ClientMessage<IQueryable>();

            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "Editrule"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            var allPrivilage = new List<PrivilegeDetails>();
            var privileges = _unitOfWork.PrivilegeActions.FindAll();


            foreach (var sysPrivilegeAction in privileges)
            {
                var privilage = new PrivilegeDetails
                {
                    SysActionId = sysPrivilegeAction.Id.ToString(),
                    SysControllerId = sysPrivilegeAction.ControllerName,
                    SysPrivilegeId = Convert.ToDecimal(sysPrivilegeAction.PrivilegeId),
                    SysPrivilegeCategoryId = Convert.ToDecimal(_unitOfWork.Privileges.FindAll().First(p => p.Id == sysPrivilegeAction.PrivilegeId)
                                .PrivilegeCategoryId)
                };
                privilage.SysPrivilegeCategoryName = _unitOfWork.PrivilegeCategories.FindAll().First(p => p.Id == privilage.SysPrivilegeCategoryId)
                        .Name;
                privilage.SysPrivilegeDisplayName =
                    _unitOfWork.Privileges.FindAll().First(p => p.Id == sysPrivilegeAction.PrivilegeId)
                        .Name;

                privilage.SysPrivilageState =
                    _unitOfWork.RulePrivileges.FindAll().FirstOrDefault(
                        r => r.RuleId == roleId && r.PrivilegeId == sysPrivilegeAction.PrivilegeId) !=
                    null
                        ? true
                        : false;
                allPrivilage.Add(privilage);

            }
            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = allPrivilage.AsQueryable();


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = allPrivilage.AsQueryable();

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        // GET: /Admin/SaveRoleDetails
        public JsonResult SaveRoleDetails(List<PrivilegeRule> roles)
        {
            var clientMessage = new ClientMessage<IQueryable>();

            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "Editrule"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }

            var privoles = _unitOfWork.RulePrivileges.Find(c => c.RuleId == roles.First().SysRoleId);
            _unitOfWork.RulePrivileges.RemoveAll(privoles);

            //dataContext.ExecuteCommand("Delete from SYS_RulePrivileges WHERE SYS_RuleID=" + roles.First().SysRoleId);
            //dataContext.SubmitChanges();

            foreach (var role in roles)
            {
                if (role.SysPrivilageState == true)
                {
                    var sysrole = new RulePrivilege { PrivilegeId = role.SysPrivilegeId, RuleId = role.SysRoleId };
                    _unitOfWork.RulePrivileges.Add(sysrole);
                }
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string>() { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
            }

            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }


    }
}
