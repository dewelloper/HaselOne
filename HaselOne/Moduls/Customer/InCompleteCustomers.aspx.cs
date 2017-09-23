using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class InCompleteCustomers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // for help pages (Content management)
            Session["ModulId"] = 1;
            Session["PageId"] = 10;

          
        }
    }
}