using BusinessObjects;
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
    public partial class UserRoleManager : System.Web.UI.Page
    {

        [Dependency]
        public IUserService _us { get; set; }

        [Dependency]
        public ICustomerService _cs { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                Util.Utility.LoadCombo<RoleWrapper>(ddRelatedRoles, _us.GetRolesAll().OrderBy(k => k.FullName).ToList(), "FullName", "DgoId");
                ddRelatedRoles.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_Area>(ddAreas, _cs.GetAreasAll().OrderBy(k => k.AreaName).ToList(), "AreaName", "Id");
                ddAreas.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                LoadExistRoles();

            }
        }

        private void LoadExistRoles()
        {
            if (Request.QueryString["UId"] != null)
            {
                int uid = Convert.ToInt32(Request.QueryString["UId"]);
                Gn_User u = _us.GetUserById(uid);
                userLabel.InnerText = u.Name + " " + u.Surname;
                hdnUserId.Value = uid.ToString();

                List<RoleWrapper> roles = _us.GetRolesByUserId(uid);
                if (roles != null && roles.Count > 0)
                {
                    foreach (RoleWrapper r in roles)
                    {
                        r.FullName = r.DgoId + ":" + r.FullName;
                        libRoles.Items.Add(new ListItem(r.FullName, r.DgoId.ToString()));
                    }
                }
            }
        }
    }
}