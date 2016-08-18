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
    public class BrancheController : BaseController
    {
        readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        //
        // GET: /Branche/

        #region ActionResult
        public ActionResult AddBranche()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "AddBranche"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }
        public ActionResult PayBranch()
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("الفروع", "الفروع").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("الفروع", "الفروع").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("الفروع", "الفروع").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("الفروع", "الفروع").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("Branches", "Branches").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("Branches", "Branches").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("Branches", "Branches").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("Branches", "Branches").Title;

            }

            return View();
        }

        public ActionResult EditBranche()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "EditBranche"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult AddCity()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "AddCity"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditCity()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "EditCity"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult AddGovernate()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "AddGovernate"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult EditGovernate()
        {
            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "EditGovernate"))
            {
                return RedirectToAction("Error403", "Home");
            }
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        #endregion


        #region Branche

        [HttpPost]
        public JsonResult SaveBranch(Branch branch)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (branch.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "AddBranche"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "EditBranche"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

                
            } 
            if (_unitOfWork.Branchs.FindAll().Any(c => c.Name == branch.Name && c.Id != branch.Id))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            var isSaved = _unitOfWork.Branchs.Add(branch);
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
        public JsonResult GetAllBranchs()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Branchs.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.Name,
                r.NameEn,
                r.CityId,
                r.Address,
                r.AddressEn,
                r.DetailsEn,
                r.Details,
                r.GovernateId,
                r.Mobile,
                r.Phone,
                CanDelete=CanDelete(r)

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFullBranchs()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Branchs.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.ImageUrl,
                r.Name,

                r.AddressEn,
                r.DetailsEn,
                r.NameEn,
                r.CityId,
                CityName = _unitOfWork.Cities.FindById(r.CityId).Name,
                CityNameEn = _unitOfWork.Cities.FindById(r.CityId).NameEn,
                r.Address,
                r.Details,
                r.GovernateId,
                GovernateName = _unitOfWork.Governates.FindById(r.GovernateId).Name,
                GovernateNameEn = _unitOfWork.Governates.FindById(r.GovernateId).NameEn,
                r.Mobile,
                r.Phone,
              

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        private bool CanDelete(Branch brach)
        {
            return true;
        }

        [HttpGet]
        public JsonResult GetBranch(int id)
        {
            var clientMessage = new ClientMessage<Branch>();
            var categories = _unitOfWork.Branchs.FindFirstOrDefault(id);
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
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "DeleteBranche"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }
            if (id > 0)
            {
                var categories = _unitOfWork.Branchs.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

                var isDeleted = _unitOfWork.Branchs.Remove(categories);
                if (isDeleted)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "حفظ البيانات") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "خطا فنى") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
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

        #region Governate

        [HttpPost]
        public JsonResult SaveGovernate(Governate governate)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (governate.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "AddGovernate"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "EditGovernate"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

              
            }  if (_unitOfWork.Governates.FindAll().Any(c => c.Name == governate.Name && c.Id != governate.Id))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            var isSaved = _unitOfWork.Governates.Add(governate);
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
        public JsonResult GetAllGovernates()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Governates.FindAll().ToList().Select(r => new
            {
                r.Id,

                r.Name,
                CanDelete = CanDelete(r)
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllCitiesByGovernateId(int id)
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Cities.FindAll().Where(c => c.GovernateId == id).ToList().Select(r => new
            {
                r.Id,

                r.Name,
               
            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        private bool CanDelete(Governate governate)
        {

            if (_unitOfWork.Branchs.FindAll().Any(c => c.GovernateId == governate.Id))
            {
                return false;
            }
            if (_unitOfWork.Cities.FindAll().Any(c => c.GovernateId == governate.Id))
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public JsonResult GetGovernate(int id)
        {
            var clientMessage = new ClientMessage<Governate>();
            var categories = _unitOfWork.Governates.FindFirstOrDefault(id);
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
        public JsonResult DeleteGovernate(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "DeleteGovernate"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Governates.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود" )  };

                }

                var isDeleted = _unitOfWork.Governates.Remove(categories);
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

        #region City

        [HttpPost]
        public JsonResult SaveCity(City city)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();

            if (city.Id <= 0)
            {

                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "AddCity"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "EditCity"))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }

              
            } 
            if (_unitOfWork.Cities.FindAll().Any(c => c.Name == city.Name && c.GovernateId == city.GovernateId && c.Id != city.Id))
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                    return Json(clientMessage, JsonRequestBehavior.AllowGet);
                }
            var isSaved = _unitOfWork.Cities.Add(city);
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
        public JsonResult GetAllCities()
        {
            var clientMessage = new ClientMessage<IQueryable>();
            var categories = _unitOfWork.Cities.FindAll().ToList().Select(r => new
            {
                r.Id,
                r.GovernateId,
                r.Name,
                r.NameEn,
               CanDelete=  CanDelete(r)

            }).AsQueryable();

            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }

        private bool CanDelete(City city)
        {
            if (_unitOfWork.Branchs.FindAll().Any(c => c.CityId == city.Id))
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public JsonResult GetCity(int id)
        {
            var clientMessage = new ClientMessage<City>();
            var categories = _unitOfWork.Cities.FindFirstOrDefault(id);
            if (categories == null)
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل موجود مسبقا") };
                return Json(clientMessage, JsonRequestBehavior.AllowGet);
            }


            clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
            clientMessage.ReturnedData = categories;
            return Json(clientMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteCity(int id)
        {
            var clientMessage = new ClientMessage<string>();

            Security.CheckLogin();
            if (!Security.IsAuthorized(Enums.PrivilegeType.Action, "Branch", "DeleteCity"))
            {
                clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                clientMessage.ReturnedData = null;
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "لا يوجد صلاحية") };

            }
            if (id > 0)
            {
                var categories = _unitOfWork.Cities.FindFirstOrDefault(id);
                if (categories == null)
                {
                    clientMessage.ClientStatusCode = Enums.OperationStatus.Error;
                    clientMessage.ReturnedData = null;
                    clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود") };

                }

                var isDeleted = _unitOfWork.Cities.Remove(categories);
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
                clientMessage.ClientMessageContent = new List<string> { CultureHelper.GetUserLanguageValue("عام", "السجل  غير موجود" ) };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);

        }


        #endregion


    }
}
