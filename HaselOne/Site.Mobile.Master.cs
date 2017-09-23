using BusinessObjects;
using DAL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
          

            if (!IsPostBack)
            {
                LoadSidebarByGeneric();
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadLinker();
            LoadDater();
        }

        private void LoadDater()
        {
            spanDater.InnerText = DateTime.Now.Date.ToLongDateString();
        }

        private void LoadLinker()
        {
            string navMapContent = "";
            string[] navigateMap = Request.RawUrl.Split('\\');

            foreach (string mapPart in navigateMap)
            {
                if (mapPart.Contains("aspx") || navigateMap.Length == 1)
                {
                    navMapContent += "<li>"
                        + "        <a href=\"" + mapPart + ".aspx\" > " + mapPart + " </a >"
                        + "         <i class=\"fa fa-circle\"></i>"
                        + "    </li>";
                }
                else
                {
                    navMapContent += "    < li>"
                                    + "        <span>" + mapPart + "</span>"
                                    + "    </li>";
                }
            }
            linker.InnerHtml = navMapContent;
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected void btnClicker_Click1(object sender, EventArgs e)
        {
            string targetUrl = hdnSelectedMenu.Value;
            Response.Redirect(targetUrl);

        }

        private void LoadSidebarByGeneric()
        {
            string sidebarHtml = " <li class=\"sidebar-toggler-wrapper hide\" id=\"liSideBarToggler\">"
                                 + "  <div class=\"sidebar-toggler\">"
                                 + "       <span></span>"
                                 + "   </div>"
                                 + " </li>"
                                 + " <li class=\"nav-item start open\" id=\"liGeneralAppearance\">"
                                 + "   <a href = \"javascript:;\" class=\"nav-link nav-toggle\">"
                                 + "       <i class=\"icon-home\"></i>"
                                 + "       <span class=\"title\">Genel Görünüm</span>"
                                 + "       <span class=\"selected\"></span>"
                                 + "       <span class=\"arrow open\"></span>"
                                 + "   </a>"
                                 + "   <ul class=\"sub-menu\" id=\"liDashboard\">"
                                 + "       <li class=\"nav-item start active open\">"
                                 + "           <a href = \"Dashboard.aspx\" class=\"nav-link \">"
                                 + "               <i class=\"icon-bar-chart\"></i>"
                                 + "               <span class=\"title\">Dashboard</span>"
                                 + "               <span class=\"selected\"></span>"
                                 + "           </a>"
                                 + "       </li>"
                                 + "   </ul>"
                                 + " </li>";

            int uid = CurrentUser.CurrentUserId;
            Dictionary<int, UserKnowledge> dUk = Session["UK"] as Dictionary<int, UserKnowledge>;
            UserKnowledge uk = dUk[uid] as UserKnowledge;
            List<Gn_ModulsAndMenus> rootMenus = uk.ModulAndMenus.FindAll(k => k.ParentId == 0);
            foreach (Gn_ModulsAndMenus menu in rootMenus)
            {
                sidebarHtml += " <li class=\"nav-item\" id=\"li" + menu.MenuName + "\">"
                               + "     <a href = \"javascript:;\" class=\"nav-link nav-toggle\">"
                               + "          <i class=\"" + menu.IconName + "\"></i>"
                               + "         <span class=\"title\">" + menu.MenuName + "</span>"
                               + "         <span class=\"arrow\"></span>"
                               + "     </a>";
                List<Gn_ModulsAndMenus> childMenus = uk.ModulAndMenus.FindAll(k => k.ParentId == menu.Id);

                if (childMenus.Count > 0)
                    sidebarHtml += "<ul class=\"sub-menu\">";
                foreach (Gn_ModulsAndMenus cmenu in childMenus)
                {
                    sidebarHtml += " <li class=\"nav-item\" id=\"liCariler\">"
                                   + "      <a href = \"" + cmenu.PageName + "\" class=\"nav-link \">"
                                   + "          <pan class=\"title\">" + cmenu.MenuName + "</span>"
                                   + "      </a>"
                                   + " </li>";
                }
                if (childMenus.Count > 0)
                    sidebarHtml += "</ul>";

                sidebarHtml += "</li>";
            }

            sideBarUL.InnerHtml = sidebarHtml;



        }
    }
}