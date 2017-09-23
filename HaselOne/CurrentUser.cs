using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Providers.Entities;
using BusinessObjects;
using HaselOne.Util;

namespace HaselOne   
{
    public class CurrentUser
    { 
        public static int CurrentUserId
        {
            get
            {
                //  var a = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var a = HttpContext.Current.User.Identity as ClaimsIdentity;
                    int userId = Convert.ToInt32(a.FindFirst("Id").Value);
                    if (
                        Convert.ToInt32(HttpContext.Current.Session["UserId"]) != userId)
                    {
                        new Login().SessionSet(userId);
                    }

                    return userId;
                }
                else
                {
                    new Login().SessionClear();
                    HttpContext.Current.Response.Redirect("~/Moduls/Generals/Login.aspx?p=exit");
                    return 0;
                }
            }
        }

        public static string Name
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    new Login().SessionClear();
                    HttpContext.Current.Response.Redirect("~/Moduls/Generals/Login.aspx?p=exit");
                    return string.Empty;
                }
            }
        }
    }
}