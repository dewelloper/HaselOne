using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class apperror : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["aspxerrorpath"] != null)
            {
                Session["Err"] = Request.QueryString["aspxerrorpath"];
                string url = Request.Url.AbsolutePath;
                Response.Redirect(url);
            }

            if (Session["Err"] != null)
                err.InnerHtml = Session["Err"].ToString();
        }
    }
}