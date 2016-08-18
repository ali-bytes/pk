using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Ui.EStore.Helpers;

namespace Ui.EStore.Controllers
{
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
                // Validate culture name
                cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe
            }

            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
                //Update by sakr

                // Validate culture name
                cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe
                cultureCookie = new HttpCookie("_culture");
                cultureCookie.Value = cultureName;
                cultureCookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cultureCookie);
            }


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
    }
}