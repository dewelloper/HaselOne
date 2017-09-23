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
    public partial class ControlAuthoritiesDetail : System.Web.UI.Page
    {
        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!IsPostBack)
            {
                Util.Utility.LoadCombo<Gn_User>(ddUserCA, _us.GetUsers().OrderBy(k => k.UserName).ToList(), "UserName", "Id");
                ddUserCA.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_Control>(ddControlCA, _us.GetControls().OrderBy(k => k.ControlId).ToList(), "ControlId", "Id");
                ddControlCA.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                LoadIfExistUser();
            }
        }

        private void LoadIfExistUser()
        {
            if (Request.QueryString["UId"] != null)
            {
                int uid = Convert.ToInt32(Request.QueryString["UId"]);
                hdnCAId.Value = uid.ToString();
                Gn_ControlAuthorities ca = _us.GetCAById(uid);
                if (ca.UserId != 0)
                {
                    ddUserCA.SelectedItem.Text = ddUserCA.Items.FindByValue(ca.UserId.ToString()).Text;
                    ddUserCA.SelectedItem.Value = ddUserCA.Items.FindByValue(ca.UserId.ToString()).Value;
                }
                if (ca.ControlId != 0)
                {
                    ddControlCA.SelectedItem.Text = ddControlCA.Items.FindByValue(ca.ControlId.ToString()).Text;
                    ddControlCA.SelectedItem.Value = ddControlCA.Items.FindByValue(ca.ControlId.ToString()).Value;
                }
                rbCAActivePassive.SelectedIndex = ca.IsEnable == true ? 0 : 1;
                rbCAVisibility.SelectedIndex = ca.IsVisible == true ? 0 : 1;
                btnUserInsert.Visible = false;
            }
            else btnUserUpdate.Visible = false;
        }
    }
}