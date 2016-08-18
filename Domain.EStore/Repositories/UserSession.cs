using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.EStore.Repositories
{
    public class UserSession
    {

        public MyCartPrduct MyCartPrduct
        {
            get
            {
                if (ContainsInSession("MyCartPrduct"))
                {

                    return GetFromSession("MyCartPrduct") as MyCartPrduct;
                }
                SetInSession("MyCartPrductCount", 0);
                return new MyCartPrduct();
            }
            set
            {
                if (value.MyCartDetailse == null)
                {
                    value.MyCartDetailse = new List<MyCartDetails>();
                }
                SetInSession("MyCartPrduct", value);
                SetInSession("MyCartPrductCount", value.MyCartDetailse.Count());
                SetInSession("MyCartPrductCache", value.MyCartDetailse.Sum(c=>c.Total));
            }
        }

        public int MyCartPrductCount
        {
            get
            {
                if (ContainsInSession("MyCartPrductCount"))
                {
                    if (MyCartPrduct.MyCartDetailse != null)
                    {


                        SetInSession("MyCartPrductCount", MyCartPrduct.MyCartDetailse.Count);
                    }
                    else
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetFromSession("MyCartPrductCount"));
                }

                return 0;
            }
            set
            {

                SetInSession("MyCartPrductCount", value);

            }
        }

        public decimal MyCartPrductCache
        {
            get
            {
                if (ContainsInSession("MyCartPrductCache"))
                {
                    if (MyCartPrduct.MyCartDetailse != null)
                    {


                        SetInSession("MyCartPrductCache", MyCartPrduct.MyCartDetailse.Sum(c=>c.Total));
                    }
                    else
                    {
                        return 0;
                    }
                    return Convert.ToDecimal(GetFromSession("MyCartPrductCache"));
                }

                return 0;
            }
            set
            {

                SetInSession("MyCartPrductCache", value);

            }
        }

        public bool GetCurrentCart
        {
            get
            {
                if (ContainsInSession("GetCurrentCart"))
                {

                    return Convert.ToBoolean(GetFromSession("GetCurrentCart"));
                }

                return false;
            }
            set
            {

                SetInSession("GetCurrentCart", value);
            }
        }

        public Language UserLanguage
        {
            get
            {
                if (ContainsInSession("UserLanguageId"))
                {
                    return (GetFromSession("UserLanguageId") as Language);
                }

                return null;
            }

            set
            {
                SetInSession("UserLanguageId", value);
            }
        }
        #region Session
        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public bool ContainsInSession(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

        public void RemoveFromSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        private void SetInSession(string key, object value)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return;
            }
            HttpContext.Current.Session[key] = value;
        }

        private object GetFromSession(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return null;
            }
            return HttpContext.Current.Session[key];
        }

        private void UpdateInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        private string GetQueryStringValue(string key)
        {
            return HttpContext.Current.Request.QueryString.Get(key);
        }

        #endregion

    }
}