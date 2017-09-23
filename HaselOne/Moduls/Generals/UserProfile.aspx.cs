using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class UserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          

            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            int uid = Convert.ToInt32(Session["UserId"]);
            Dictionary<int, UserKnowledge> dUk = Session["UK"] as Dictionary<int, UserKnowledge>;
            UserKnowledge uk = dUk[uid] as UserKnowledge;
            if (uk.User != null && uk.User.ImagePath != null)
            {
                string litleImage = uk.User.ImagePath;
                string bgImage = litleImage.Replace("_45_", "_750_");
                dashimgBigAvatar1.Src = bgImage;
            }
            if (uk.User.Email != null)
                dashemail1.InnerText = uk.User.Email;
            dashemail1.HRef = uk.User.Email + "?subject=Hasel CRM Üzerinden Gönderilen Mail";
            //if (uk.User.MainArea != null)
            //    dashlocation1.InnerText = uk.User.MainArea;
            if (uk.User.Gsm != null)
                dashmobile1.InnerText = uk.User.Gsm;
            if (uk.User.Phone != null)
                dashphone1.InnerText = uk.User.Phone;
            if (uk.User.Title != null)
                dashposition1.InnerText = uk.User.Title;

        }

        protected void btnUpdateUserProfile_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("UserProfileChange.Aspx");
        }
    }
}