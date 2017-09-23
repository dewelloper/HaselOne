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
    public partial class UserGroupDetail : System.Web.UI.Page
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
                hdnUserGroupId.Value = uid.ToString();
                Gn_Category user = _us.GetGnCategoriById(uid);
                if (user.IsActive == null)
                {
                    rbUserGroupActivePassive.SelectedValue = "0";
                }
                if (user.IsActive == true)
                {
                    rbUserGroupActivePassive.SelectedValue = "1";
                }
                else
                {
                    rbUserGroupActivePassive.SelectedValue = "0";
                }
               
                txtUserGroupName.Value = user.Title;
                btnUserInsert.Visible = false;
            }
            else btnUserUpdate.Visible = false;
        }
    }
}