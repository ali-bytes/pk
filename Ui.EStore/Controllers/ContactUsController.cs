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
    public class ContactUsController : BaseController
    {
        //
        // GET: /ContactUs/

        public ActionResult Index()
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("أتصل بنا", "أتصل بنا").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("أتصل بنا", "أتصل بنا").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("أتصل بنا", "أتصل بنا").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("أتصل بنا", "أتصل بنا").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("ContactUs", "ContactUs").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("ContactUs", "ContactUs").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("ContactUs", "ContactUs").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("ContactUs", "ContactUs").Title;

            }

            return View();
        }

        public static HomeDetail AboutUsDetails()
        {
            var _unitOfWork = new UnitOfWork();
            return _unitOfWork.HomeDetails.FindById(3);
        }
    }
}
