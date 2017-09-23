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
    public partial class UserGroupAuthenticationDetail : System.Web.UI.Page
    {
        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Util.Utility.LoadCombo<Gn_Category>(ddUserGroupRightsGroups, _us.GetUserGroups().OrderBy(k => k.Title).ToList(), "Title", "Id");
                ddUserGroupRightsGroups.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_UserModuls>(ddUserGroupRightsModuls, _us.GetModuls().OrderBy(k => k.ModulName).ToList(), "ModulName", "Id");
                ddUserGroupRightsModuls.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_Department>(ddDepartments, _us.GetDepartments().OrderBy(k => k.Id).ToList(), "DepartmentName", "Id");
                ddDepartments.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_Role>(ddroles, _us.GetRoles().OrderBy(k => k.RuleName).ToList(), "RuleName", "Id");
                ddroles.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                LoadIfExistUser();
            }
        }

        private void LoadIfExistUser()
        {
            if (Request.QueryString["UId"] != null)
            {
                int uid = Convert.ToInt32(Request.QueryString["UId"]);
                hdnUserGroupRightId.Value = uid.ToString();
                int did = Convert.ToInt32(Request.QueryString["DId"]);
                hdnUserGroupDetailId.Value = did.ToString();

                Gn_UserGroupRights user = _us.GetGroupRightsById(uid);

                if (user.ModulId != 0)
                {
                    ddUserGroupRightsModuls.SelectedItem.Text = ddUserGroupRightsModuls.Items.FindByValue(user.ModulId.ToString()).Text;
                    ddUserGroupRightsModuls.SelectedItem.Value = user.ModulId.ToString();
                }
                chkUserGroupRightsInsert.Checked = user.RecordInsert == true?true:false;
                chkUserGroupRightsEdit.Checked = user.RecordEdit == true ? true : false;
                chkUserGroupRightsShow.Checked = user.RecordShow == true ? true : false;
                chkUserGroupRightsDelete.Checked = user.RecordDelete == true ? true : false;
                rbUserGroupRightsActivePassive.SelectedIndex = user.IsActive == true ? 0 : 1;
                rbUserGroupRightsDelete.SelectedIndex = user.IsDeleted == true ? 0 : 1;
                btnUserInsert.Visible = false;

                Gn_Department dep = _us.GetDepartmentByUserId(uid);
                if (dep != null)
                {
                    ddDepartments.SelectedItem.Text = dep.DepartmentName;
                    ddDepartments.SelectedItem.Value = dep.Id.ToString();
                }
                List<Gn_Role> roles = _us.GetRoleByUserId(uid);
                if(roles != null && roles.Count > 0)
                {
                    ddroles.SelectedItem.Text = roles[0].RuleName;
                    ddroles.SelectedItem.Value = roles[0].Id.ToString();
                }

                Gn_Category groups = _us.GetGroupByUserId(uid);
                if (groups != null)
                {
                    ddUserGroupRightsGroups.SelectedItem.Text = groups.Title;
                    ddUserGroupRightsGroups.SelectedItem.Value = groups.Id.ToString();
                }
            }
            else btnUserUpdate.Visible = false;
        }
    }
}