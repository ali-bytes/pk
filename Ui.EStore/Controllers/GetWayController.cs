using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.EStore;
using Domain.EStore.Repositories;
using Ui.EStore.Helpers;

namespace Ui.EStore.Controllers
{
    public class GetWayController : BaseController
    {
        readonly IUnitOfWork _unitOfWork = new UnitOfWork();


        #region ActionResult
        public ActionResult Index()
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("طرق الدفع", "طرق الدفع").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("طرق الدفع", "طرق الدفع").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("طرق الدفع", "طرق الدفع").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("طرق الدفع", "طرق الدفع").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("Getways", "Getways").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("Getways", "Getways").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("Getways", "Getways").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("Getways", "Getways").Title;

            }
            return View();
        }

        public ActionResult AddGetway()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "GetWay", "AddGetWay"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditGetWay()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "GetWay", "EditGetWay"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }
        #endregion

        #region GetWay

        [HttpPost]
        public JsonResult SaveGetWay(GetWay getWay)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (getWay.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "GetWay", "AddGetWay"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "GetWay", "EditGetWay"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }


            }
            if (_unitOfWork.GetWaies.FindAll().Any(c => c.Name == getWay.Name && c.Id != getWay.Id))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            var isSaved = _unitOfWork.GetWaies.Add(getWay);
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
        public JsonResult GetAllGetWaies()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.GetWaies.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.NameEn,
                r.BerifEn,
                r.Name,
                r.Berif,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        private bool CanDelete(GetWay getWay)
        {
            var Carts = _unitOfWork.Carts.FindAll().Where(c => c.DeliveryId == getWay.Id);
            if (Carts.Any())
            {
                return false;
            }

            return true;
        }
        public static List<GetWay> GetAllGetWays()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.GetWaies.FindAll().ToList();
        }
        [HttpGet]
        public JsonResult GetGetWay(int id)
        {
            var clientMessage = new ClientMessage<GetWay>();
            var categories = _unitOfWork.GetWaies.FindFirstOrDefault(id);
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
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "GetWay", "DeleteGetWay"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.GetWaies.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.GetWaies.Remove(categories);
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
