using DAL;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class ControlDetail : System.Web.UI.Page
    {
        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                LoadIfExistUser();
            }
        }

        private void LoadIfExistUser()
        {
            if (Request.QueryString["UId"] != null)
            {
                int uid = Convert.ToInt32(Request.QueryString["UId"]);
                hdnControlId.Value = uid.ToString();
                Gn_Control user = _us.GetControlById(uid);
                txtControlId.Value = user.ControlId;
                txtControlModulId.Value = user.ModulId.ToString();
                txtControlPageId.Value = user.PageId.ToString();
                txtControlTextEng.Value = user.ControlTextEng;
                txtControlType.Value = user.ControlTypeName;
                rbControlEnable.SelectedIndex = user.IsEnable == true ? 0 : 1;
                rbControlVisibile.SelectedIndex = user.IsVisible == true ? 0 : 1;
                btnUserInsert.Visible = false;
            }
            else btnUserUpdate.Visible = false;
        }
    }
}