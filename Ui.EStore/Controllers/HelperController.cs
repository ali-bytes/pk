using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ui.EStore.Helpers;

namespace Ui.EStore.Controllers
{
    public class HelperController : BaseController
    {
        //
        // GET: /Helper/

        //GET: /Helper/UploadFile
        public JsonResult UploadFile()
        {
            //bool authorized = false;

            //authorized = Security.IsAuthorized(Enums.PrivilegeType.Action, "Blog", "AddArticle");

            //if (!authorized)
            //    authorized = Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "AddUser");

            //if (!authorized)
            //    authorized = Security.IsAuthorized(Enums.PrivilegeType.Action, "Admin", "EditUser");

            //if (!authorized)
            //    authorized = Security.IsAuthorized(Enums.PrivilegeType.Action, "MarketingOffers", "AddOffer");

            //if (!authorized)
            //    authorized = Security.IsAuthorized(Enums.PrivilegeType.Action, "MarketingOffers", "EditOffer");

            //if (!authorized)
            //    authorized = Security.IsAuthorized(Enums.PrivilegeType.Action, "News", "AddNews");

            //if (!authorized)
            //    authorized = Security.IsAuthorized(Enums.PrivilegeType.Action, "News", "EditNews");

            //if (!authorized)
            //{
            //    return Json("Error403: " + Resources.Resources.UnAuthorized, JsonRequestBehavior.AllowGet);
            //}

            var clientMessage = new ClientMessage<string>();

            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                string fileName = httpPostedFileBase.FileName;
                string[] parts = fileName.Split('.');
                if (parts.Length < 2)
                {
                    throw new Exception();
                }
                var random = new Random(3);

                fileName = DateTime.Now.ToFileTime() + random.Next() + "." + parts[parts.Length - 1];
                httpPostedFileBase.SaveAs(Server.MapPath("~/App_Files/" + fileName));
                clientMessage.ClientStatusCode = Enums.OperationStatus.Ok;
                clientMessage.ClientMessageContent = new List<string> { fileName };
            }
            return Json(clientMessage, JsonRequestBehavior.AllowGet);


        }

    }
}
