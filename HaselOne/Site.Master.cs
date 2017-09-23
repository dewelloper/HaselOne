using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using BusinessObjects;
using DAL;
using Microsoft.Practices.Unity;
using HaselOne.Services.Interfaces;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Web.SessionState;
using System.Collections;
using System.Net;
using System.Reflection;
using HaselOne.UC;
using IWshRuntimeLibrary;
using Microsoft.AspNet.SignalR;
using HaselOne.Util;

namespace HaselOne
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        [Dependency]
        public ICoreService _core { get; set; }

        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentUser.CurrentUserId == 0)
            {
                Response.Redirect("~/Moduls/Generals/Login.aspx");
            }
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
            ReLoadIfUserKnowledgeChanged();

            int uid = Convert.ToInt32(Session["UserId"]);
            hdnUserId.Value = uid.ToString();
            string pageName = this.Page.Request.FilePath;
            LoadLinker();
            LoadDater();
            LoadAuthenticatedControls(uid, pageName);
            LoadUserKnowkedge();

            if (!IsPostBack)
            {
                LoadSidebarByGeneric();
            }
            //messagebox.Attributes.Add("onkeypress","SendMessage('"+ CurrUserId +"');");
        }

        private void ReLoadIfUserKnowledgeChanged()
        {
            if (Session["UkChanged"] != null)
            {
                string uname = Session["uname"].ToString();
                string pwd = Session["pwd"].ToString();
                UserKnowledge uk = _us.LoginUser(uname, pwd);
                Dictionary<int, UserKnowledge> userKnowledge = new Dictionary<int, UserKnowledge>();
                userKnowledge = Session["UK"] as Dictionary<int, UserKnowledge>;
                userKnowledge[uk.UserId] = uk;
                Session["UK"] = userKnowledge;

                Session["UkChanged"] = null;
            }
        }

        private void LoadAuthenticatedControls(int userId, string pageName)
        {
            var controls = _core.GetControls(userId, pageName);
            foreach (var ctrl in controls)
            {
                var element = ContentPlaceHolder1.FindControl(ctrl.ControlId);
                if (element == null)
                    continue;

                if (ctrl.IsVisible != null && ctrl.IsVisible == true)
                    element.Visible = true;
                else element.Visible = false;

                if (ctrl.ControlTypeName == "RadioButtonList")
                {
                    if (ctrl.IsEnable != null && ctrl.IsEnable == true)
                        (element as RadioButtonList).Enabled = true;
                    else (element as RadioButtonList).Enabled = false;
                }
                if (ctrl.ControlTypeName == "HtmlGenericControl")
                {
                    if (ctrl.IsEnable != null && ctrl.IsEnable == true)
                        (element as HtmlGenericControl).Disabled = false;
                    else (element as HtmlGenericControl).Disabled = true;
                }
                if (ctrl.ControlTypeName == "TextBox")
                {
                    if (ctrl.IsEnable != null && ctrl.IsEnable == true)
                        (element as TextBox).Enabled = true;
                    else (element as TextBox).Enabled = false;
                }
                if (ctrl.ControlTypeName == "DropDownList")
                {
                    if (ctrl.IsEnable != null && ctrl.IsEnable == true)
                        (element as DropDownList).Enabled = true;
                    else (element as DropDownList).Enabled = false;
                }
            }
        }

        private void LoadUserKnowkedge()
        {
            if (Session["UK"] != null)
            {
                int uid = Convert.ToInt32(CurrentUser.CurrentUserId);
                Dictionary<int, UserKnowledge> dUk = Session["UK"] as Dictionary<int, UserKnowledge>;
                UserKnowledge uk = dUk[uid] as UserKnowledge;
                userSpan.InnerText = uk.User.Name;
                imgAvatar.Src = uk.User.ImagePath;

                Dictionary<int, UserKnowledge> userKnowledge = new Dictionary<int, UserKnowledge>();
                if (Session["UK"] != null)
                    userKnowledge = Session["UK"] as Dictionary<int, UserKnowledge>;

                if (!userKnowledge.ContainsKey(uk.UserId))
                    userKnowledge.Add(uk.UserId, uk);
                Session["UK"] = userKnowledge;

                LoadUsersToScreen(Global.Sessions);

                int userid = uk.UserId;
                string username = uk.User.UserName;
                txtNickName.Value = username;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "chatlogin", "$('#btnStartChat').click();", true);
            }
        }

        private void LoadUsersToScreen(Dictionary<string, UserKnowledge> userKnowledge)
        {
            linkedUserCount.InnerText = userKnowledge.Count().ToString();
            //string userLeftSidebarHtml = "";
            //List<string> tmpUsrName = new List<string>();
            //foreach (KeyValuePair<string, UserKnowledge> entry in userKnowledge)
            //{
            //    Gn_User user = (entry.Value as UserKnowledge).User;
            //    if (tmpUsrName.Contains(user.UserName))
            //        continue;
            //    tmpUsrName.Add(user.UserName);
            //    string imgPath = (entry.Value as UserKnowledge).User.ImagePath;
            //    if (imgPath != null)
            //    {
            //        imgPath = imgPath.Replace("~/", "\\");
            //        userLeftSidebarHtml += " <li class=\"media\" ng-click=\"targetedUser('" + user.Id + "');\">"
            //                              + "          <div class=\"media-status\">"
            //                              + "              <span class=\"badge badge-success\"></span>"
            //                              + "          </div>"
            //                              + "          <img class=\"media-object\" src=\"" + imgPath + "\" alt =\"...\">"
            //                              + "          <div class=\"media-body\">"
            //                              + "              <h4 class=\"media-heading\">" + user.UserName + "</h4>"
            //                              + "              <div class=\"media-heading-sub\">Kod:" + user.Id + "</div>"
            //                              + "          </div>"
            //                              + "      </li>";
            //    }
            //    else
            //    {
            //        userLeftSidebarHtml += " <li class=\"media\" ng-click=\"targetedUser('" + user.Id + "');\">"
            //                              + "          <div class=\"media-status\">"
            //                              + "              <span class=\"badge badge-success\"></span>"
            //                              + "          </div>"
            //                              + "          <img class=\"media-object\" src=\"\" alt =\"...\">"
            //                              + "          <div class=\"media-body\" >"
            //                              + "              <h4 class=\"media-heading\">" + user.UserName + "</h4>"
            //                              + "              <div class=\"media-heading-sub\">Kod:" + user.Id + "</div>"
            //                              + "          </div>"
            //                              + "      </li>";
            //    }
            //}
            // masterUlUsers.InnerHtml = userLeftSidebarHtml;
        }

        public static List<SessionStateItemCollection> GetAllUserSessions()
        {
            List<SessionStateItemCollection> sessionlist = new List<SessionStateItemCollection>();
            List<Hashtable> hTables = new List<Hashtable>();
            PropertyInfo propInfo = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static);
            object CacheInternal = propInfo.GetValue(null, null);
            dynamic fieldInfo = CacheInternal.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                object[] _caches = (object[])fieldInfo.GetValue(CacheInternal);
                for (int i = 0; i <= _caches.Length - 1; i++)
                {
                    Hashtable hTable = (Hashtable)_caches[i].GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_caches[i]);
                    hTables.Add(hTable);
                }
            }
            else
            {
                object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
                Hashtable c2 = (Hashtable)obj.GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
                if (c2 == null)
                    return null;
                hTables.Add(c2);
            }

            if (hTables.Count <= 0)
                return null;

            foreach (var hTable_loopVariable in hTables)
            {
                var hTable = hTable_loopVariable;
                foreach (DictionaryEntry entry in hTable)
                {
                    object value = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
                    if (value.GetType().ToString() == "System.Web.SessionState.InProcSessionState")
                    {
                        SessionStateItemCollection sCollection = (SessionStateItemCollection)value.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(value);
                        if (sCollection != null)
                            sessionlist.Add(sCollection);
                    }
                }
            }

            return sessionlist;
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
                    //navMapContent += "<li>"
                    //    + "        <a href=\"" + mapPart + ".aspx\" > " + mapPart + " </a >"
                    //    + "         <i class=\"fa fa-circle\"></i>"
                    //    + "    </li>";
                    navMapContent += $@"
                            <li>
                                <a href='{mapPart}'>{mapPart}</a>
                                <i class='fa fa-circle'></i>
                            </li>
";
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
                                 + "       <i class=\"fa fa-home\"></i>"
                                 + "       <span class=\"title\">Genel Görünüm</span>"
                                 + "       <span class=\"selected\"></span>"
                                 + "       <span class=\"arrow open\"></span>"
                                 + "   </a>"
                                 + "   <ul class=\"sub-menu\" id=\"liDashboard\">"
                                 + "       <li class=\"nav-item start active open\">"
                                 + "           <a href =\"" + Page.ResolveUrl("~/Moduls/Generals/Dashboard.aspx") + "\" class=\"nav-link \">"
                                 + "               <i class=\"fa fa-bar-chart\"></i>"
                                 + "               <span class=\"title\">Dashboard</span>"
                                 + "               <span class=\"selected\"></span>"
                                 + "           </a>"
                                 + "       </li>"
                                 + "   </ul>"
                                 + " </li>";

            int uid = Convert.ToInt32(Session["UserId"]);
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
                List<Gn_ModulsAndMenus> childMenus = uk.ModulAndMenus.FindAll(k => k.ParentId == menu.Id).OrderBy(k => k.PageId).ToList();
                if (childMenus.Count > 0)
                    sidebarHtml += "<ul class=\"sub-menu\">";
                foreach (Gn_ModulsAndMenus cmenu in childMenus)
                {
                    List<Gn_ModulsAndMenus> innerChildMenus = uk.ModulAndMenus.FindAll(k => k.ParentId == cmenu.Id).OrderBy(k => k.PageId).ToList();
                    if (innerChildMenus.Count > 0)
                    {
                        sidebarHtml += "<li class=\"nav-item\">"
                                    + "    <a href = \"javascript:;\" class=\"nav-link nav-toggle\">"
                                    + "        <span class=\"title\">" + cmenu.MenuName + "</span>"
                                    + "        <span class=\"arrow\"></span>"
                                    + "    </a>"
                                    + "<ul class=\"sub-menu\" style=\"display: none; \">";
                    }
                    else
                    {
                        sidebarHtml += " <li class=\"nav-item\" id=\"liCariler\">"
                                       + "      <a href =  \"javascript:NavigateWithAjax('" + cmenu.PageName + "');\" class=\"nav-link \">"
                                       + "          <span class=\"title\">" + cmenu.MenuName + "</span>"
                                       + "      </a>";
                    }
                    foreach (Gn_ModulsAndMenus imenu in innerChildMenus)
                    {
                        sidebarHtml += " <li class=\"nav-item\" id=\"liCariler\">"
                                       + "      <a href =  \"javascript:NavigateWithAjax('" + imenu.PageName + "');\" class=\"nav-link \">"
                                       + "          <span class=\"title\">" + imenu.MenuName + "</span>"
                                       + "      </a>";
                        sidebarHtml += " </li>";
                    }
                    if (innerChildMenus.Count > 0)
                        sidebarHtml += "</ul>";

                    sidebarHtml += " </li>";
                }
                if (childMenus.Count > 0)
                    sidebarHtml += "</ul>";

                sidebarHtml += "</li>";
            }

            sideBarUL.InnerHtml = sidebarHtml;
        }

        public void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Hasel CRM-ERP kısayolu";   // The description of the shortcut
            shortcut.IconLocation = Page.ResolveUrl("~/favicon.ico"); //@"C:\Hasel\HaselOne\ONEHASEL\HaselOne\favicon.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShortMes", "alert('Kısayolunuz başarıyla oluşturulmuştur.');", true);
        }

        protected void btnShortcutClicker_Click(object sender, EventArgs e)
        {
            CreateShortcut("HASEL CRM_ERP Sistemi", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.hasel.com");//Assembly.GetExecutingAssembly().Location
        }

        #region props

        //public string CurrUserName()
        //{
        //    return Session["uname"].ToString();
        //}

        //public string CurrUserId
        //{
        //    get
        //    {
        //        return Session["UserId"].ToString();
        //    }
        //}

        //public string CurrDateTime()
        //{
        //    return DateTime.Now.TimeOfDay.ToString();
        //}

        //public string CurrUserProgileImagePath()
        //{
        //    return Session["CurrUserImagePath"].ToString();
        //}

        #endregion props
    }
}