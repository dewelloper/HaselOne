using BusinessObjects;
using HaselOne.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class CustomerList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustomerFilter cust = new CustomerFilter();
                cust.UserId = Convert.ToInt32(Session["UserId"]);
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(cust), "CustomerFilter", "startup_scripts");
            }
        }
    }
}