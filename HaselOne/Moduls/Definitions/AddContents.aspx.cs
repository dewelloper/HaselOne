using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace HaselOne
{
    public partial class AddContents : System.Web.UI.Page
    {


        [Dependency]
        public ICustomerService _cs { get; set; }

        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ModulId"] = 8;
            Session["PageId"] = 14;

            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                    hdnCreatorId.Value = Session["UserId"].ToString();
                else Response.Redirect("/Moduls/Generals/Login.aspx", false);

                Util.Utility.LoadCombo<Gn_ContentTypes>(ddContentTypes, _cs.GetContentTypesAll().OrderBy(k => k.ContentName).ToList(), "ContentName", "Id");
                ddContentTypes.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });

                Util.Utility.LoadCombo<Gn_UserModuls>(ddModuls, _cs.GetModuls().OrderBy(k => k.ModulName).ToList(), "ModulName", "Id");
                ddModuls.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
        }


    }
}