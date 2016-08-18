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
    public class AboutUsController : BaseController
    {
        //
        // GET: /AboutUs/

        public ActionResult Index()
        {
            if (CultureHelper.GetLang() == Enums.Lang.Ar)
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("من نحن", "من نحن").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("من نحن", "من نحن").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("من نحن", "من نحن").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("من نحن", "من نحن").Title;

            }
            else
            {
                ViewBag.Keywords = CultureHelper.GetSeoUserLanguageValue("AboutUS", "AboutUS").Keywords;
                ViewBag.Description = CultureHelper.GetSeoUserLanguageValue("AboutUS", "AboutUS").Description;
                ViewBag.Author = CultureHelper.GetSeoUserLanguageValue("AboutUS", "AboutUS").Author;
                ViewBag.Title = CultureHelper.GetSeoUserLanguageValue("AboutUS", "AboutUS").Title;

            }
            return View();
        }

        public static HomeDetail AboutUsDetails()
        {
            var _unitOfWork = new UnitOfWork();
          return _unitOfWork.HomeDetails.FindById(1);
        }
    }
}
