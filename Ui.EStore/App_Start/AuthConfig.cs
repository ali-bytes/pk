using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Ui.EStore.Models;

namespace Ui.EStore
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "718711021581924",
                appSecret: "3edfb9a55e3a990e5a1a5529ec8d629f");
    //appId: "1051750851520783",
    //            appSecret: "7983e617fa374a2f860e5d9faf9ef3de");
           // OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
