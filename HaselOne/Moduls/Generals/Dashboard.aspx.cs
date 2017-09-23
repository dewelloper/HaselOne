using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          

            LoadUserDashBoard();
        }

        private void LoadUserDashBoard()
        {
            int uid = Convert.ToInt32(Session["UserId"]);
            Dictionary<int, UserKnowledge> dUk = Session["UK"] as Dictionary<int, UserKnowledge>;
            UserKnowledge uk = dUk[uid] as UserKnowledge;
            if (uk.User != null && uk.User.ImagePath != null)
            {
                string litleImage = uk.User.ImagePath;
                string bgImage = litleImage.Replace("_45_", "_750_");
                dashimgBigAvatar.Src = bgImage;
            }
            if(uk.User.Email != null)
                dashemail.InnerText = uk.User.Email;
            dashemail.HRef = uk.User.Email + "?subject=Hasel CRM Üzerinden Gönderilen Mail";
            //if (uk.User.MainArea != null)
            //    dashlocation.InnerText = uk.User.MainArea;
            if (uk.User.Gsm != null)
                dashmobile.InnerText = uk.User.Gsm;
            if (uk.User.Phone != null)
                dashphone.InnerText = uk.User.Phone;
            if (uk.User.Title != null)
                dashposition.InnerText = uk.User.Title;

        }

        protected void btnUpdateUserProfile_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("UserProfileChange.Aspx");
        }
    }
}