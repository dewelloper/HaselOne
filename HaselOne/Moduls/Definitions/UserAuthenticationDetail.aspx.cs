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
    public partial class UserAuthenticationDetail : System.Web.UI.Page
    {
        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                Util.Utility.LoadCombo<Gn_User>(ddUsers, _us.GetUsers().OrderBy(k => k.UserName).ToList(), "UserName", "Id");
                ddUsers.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_UserModuls>(ddUserRightsModuls, _us.GetModuls().OrderBy(k => k.ModulName).ToList(), "ModulName", "Id");
                ddUserRightsModuls.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                LoadIfExistUser();
            }
        }

        private void LoadIfExistUser()
        {
            if (Request.QueryString["UId"] != null)
            {
                int uid = Convert.ToInt32(Request.QueryString["UId"]);
                hdnUserRightId.Value = uid.ToString();
                Gn_UserRights user = _us.GetUserRightsById(uid);
                if (user.UserId != 0)
                {
                    ddUsers.SelectedItem.Text = ddUsers.Items.FindByValue(user.UserId.ToString()).Text;
                    ddUsers.SelectedItem.Value = ddUsers.Items.FindByValue(user.UserId.ToString()).Value;
                }
                if (user.ModulId != 0)
                {
                    ddUserRightsModuls.SelectedItem.Text = ddUserRightsModuls.Items.FindByValue(user.ModulId.ToString()).Text;
                    ddUserRightsModuls.SelectedItem.Value = user.ModulId.ToString();
                }
                chkUserRightsInsert.Checked = user.RecordInsert == true ? true : false;
                chkUserRightsEdit.Checked = user.RecordEdit == true ? true : false;
                chkUserRightsShow.Checked = user.RecordShow == true ? true : false;
                chkUserRightsDelete.Checked = user.RecordDelete == true ? true : false;
                rbUserRightsActivePassive.SelectedIndex = user.IsActive == true ? 0 : 1;
                rbUserRightsDelete.SelectedIndex = user.IsDeleted == true ? 0 : 1;
                btnUserInsert.Visible = false;
            }
            else btnUserUpdate.Visible = false;
        }
    }
}