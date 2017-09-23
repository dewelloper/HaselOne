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
    public partial class OperationRoleChart : System.Web.UI.Page
    {

        [Dependency]
        public ICustomerService _cs { get; set; }

        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ModulId"] = 8;
            Session["PageId"] = 8;

            if (!IsPostBack)
            {
                Util.Utility.LoadCombo<Gn_Category>(ddOperations, _cs.GetOperationsAll().OrderBy(k => k.Title).ToList(), "Title", "Id");
                ddOperations.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });

                //LoadOperations();
            }
        }

        //private void LoadOperations()
        //{
        //    List<Gn_Category> crgs = _cs.GetOperationsAll().ToList();
        //    ddOperations.DataSource = crgs;
        //    ddOperations.DataTextField = "Title";
        //    ddOperations.DataValueField = "Id";
        //    ddOperations.DataBind();
        //}

        protected void ddOperations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddOperations.SelectedIndex > 0)
            {
                int selectedId = Convert.ToInt32(ddOperations.SelectedItem.Value);
                List<Gn_UserRoles> ours = _cs.GetRolesByOperationTypeId(selectedId).ToList();
                List<Gn_Area> oareas = _cs.GetAreasAll().ToList();
                string table = "<table>";
                table += "<tr>";
                foreach (Gn_Area ar in oareas)
                {
                    List<Gn_UserRoles>  our = ours.Where(k => k.AreaId == ar.Id).ToList();
                    if (our.Count == 0)
                        continue;
                    table += "<td style=\"display:table-cell; vertical-align:top; \">" + ar.AreaName;
                    table += "<ul style=\"vertical-align: top;\">";
                    try
                    {
                        foreach (Gn_UserRoles ou in our)
                        {
                            Gn_Department dep = _us.GetUserDepartmentById((Int32)ou.DepartmentRuleId);
                            Gn_Role role = _us.GetUserRuleById((Int32)ou.DepartmentRuleId);
                            Gn_User user = _us.GetUserById((Int32)ou.UserId);
                            string uName = user.Name + " " + user.Surname;
                            string dpartment = dep == null ? "Genel" : dep.DepartmentName;
                            string rolename = role == null ? "Genel" : role.RuleName;
                            table += "<li><a>" + uName + "</a></br><a style=\"color:orange; font-size:9px\"> (" + dpartment + " " + rolename + ")</a></li>";
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    table += "</ul>";
                    table += "</td>";
                }
                table += "</tr>";
                table += "</table>";

                opChartContent.InnerHtml = table;
            }
        }
    }
}