using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.EStore;
using Ui.EStore.Helpers;
using Domain.EStore.Repositories;

namespace Ui.EStore.Controllers
{
    public class NewsController : BaseController
    {
        readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        #region ActionResult
        public ActionResult AddNew()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "New", "AddNew"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditNew()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "New", "EditNew"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }
        //
        // GET: /News/

        public ActionResult Index()
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("الاخبار", "الاخبار").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("الاخبار", "الاخبار").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("الاخبار", "الاخبار").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("الاخبار", "الاخبار").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("News", "News").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("News", "News").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("News", "News").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("News", "News").Title;

            }

            return View();
        }

        #endregion

        #region SysNew

        [HttpPost]
        public JsonResult SaveSysNew(SysNew sysNew)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (sysNew.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "New", "AddNew"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "New", "EditNew"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            var isSaved = _unitOfWork.SysNews.Add(sysNew);
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
        public JsonResult GetAllSysNews()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.SysNews.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.Berif,
                r.Details,
                r.ImageUrl,
                r.Title,
                r.TitleEn,
                r.BerifEn,
                r.DetailsEn,
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetSysNew(int id)
        {
            var clientMessage = new ClientMessage<SysNew>();
            var categories = _unitOfWork.SysNews.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود" ) };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteSysNew(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "New", "DeleteNew"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.SysNews.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.SysNews.Remove(categories);
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


    }
}
