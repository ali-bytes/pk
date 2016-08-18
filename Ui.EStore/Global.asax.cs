using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ui.EStore.Models;
using WebMatrix.WebData;

namespace Ui.EStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
       
            AuthConfig.RegisterAuth();
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
          
        }

        private void Session_Start(object sender, EventArgs e)
        {
             
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie == null)
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = "ar";
            }
         
            Response.Cookies.Add(cookie);

     
        }

        public class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                using (var context = new UsersContext())
                    context.UserProfiles.Find(1);

                if (!WebSecurity.Initialized)
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
        }
    }
}
