using BusinessObjects;
using DAL;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Services;
using Microsoft.Owin.Security;

namespace HaselOne
{
    public partial class Login : System.Web.UI.Page
    {
        [Dependency]
        public IUserService _us { get; set; }

        private readonly string strDefaultPassword = "123";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (Request.QueryString["p"] != null && Request.QueryString["p"] == "exit")
                {
                    Request.GetOwinContext().Authentication.SignOut();
                    SessionClear();
                }
                else
                {
                    if (CurrentUser.CurrentUserId != 0)
                    {
                        Response.Redirect("~/Moduls\\Customer\\CustomerDetail.Aspx");
                    }
                }

            //userName.Value = "hamit.yildirim";
            //password.Value = "123";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string uname = userName.Value;
            string pwd = password.Value;
            UserKnowledge uk = _us.LoginUser(uname, pwd);
            if (uk != null)
            {
                var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, uk.User.UserName),
                    new Claim("Id", uk.UserId.ToString()),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", uk.UserId.ToString()),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", uk.UserId.ToString()) }, "ApplicationCookie");

                Request.GetOwinContext().Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true //LoginDto.RememberMe,
                }, identity);

                SessionSet(uk.UserId);

                if (pwd == strDefaultPassword)
                {
                    hdnPasswordchangeStatus.Value = "change";
                }
                else
                {
                    hdnPasswordchangeStatus.Value = "normal";
                    if (Request.QueryString["ReturnUrl"] != null)
                    {
                        var item = Request.QueryString["ReturnUrl"];
                        Response.Redirect(item);
                    }
                    Response.Redirect("~/Moduls\\Customer\\CustomerDetail.Aspx");
                }
            }
            else
            {
                lblLoginStatus.Visible = true;
            }
        }

        public void SessionSet(int userId)
        {
            if (_us == null)
                _us = new UserService(new UnitOfWork(context: new HASELONEEntities()));

            var gnuser = _us.GetUserById(userId);
            UserKnowledge uk = _us.LoginUser(gnuser.UserName, gnuser.Password);
            Session["uname"] = gnuser.UserName;
            Session["pwd"] = gnuser.Password;
            Session["UserId"] = uk.UserId;
            Dictionary<int, UserKnowledge> userKnowledge = new Dictionary<int, UserKnowledge>();
            userKnowledge.Add(uk.UserId, uk);
            Global.Sessions[this.Session.SessionID] = uk;
            Session["UK"] = userKnowledge;
            Session["CurrUserImagePath"] = uk.ImagePath;
        }

        public void SessionClear()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
        }

        protected void btnChanging_Click(object sender, EventArgs e)
        {
            _us.UserPasswordChange(Convert.ToInt32(Session["UserId"]), txtPassword1.Value, txtPassword2.Value);
            Response.Redirect("~/Moduls\\Customer\\CustomerDetail.Aspx");
        }
    }
}