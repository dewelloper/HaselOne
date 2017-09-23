using DAL;
using HaselOne.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class MachineparkCustomerMarks : System.Web.UI.Page
    {
        HASELONEEntities _context = new HASELONEEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            ddCategories.DataTextField = "CategoryName";
            ddCategories.DataValueField = "Id";

            ddCategories.DataSource = Util.Utility.GetCategoryDropdownSource();
            ddCategories.DataBind();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            int catAll = chkAll.Checked == true ? 1 : 0;
            if (ddCategories.SelectedItem.Value == "0")
                catAll = 1;
            machineparkContent.InnerHtml = CreateAccordingMark(catAll);
        }

        protected void btnMachineLoad_Click(object sender, EventArgs e)
        {
            int catAll = chkAll.Checked == true ? 1 : 0;
            if (ddCategories.SelectedItem.Value == "0")
                catAll = 1;
            machineparkContent.InnerHtml = CreateAccordingMarkAndMachine(catAll);
        }

        string CreateAccordingMarkAndMachine(int catAll)
        {
            int catId = Convert.ToInt32(ddCategories.SelectedItem.Value);
            List<int?> allChildIds = Utility.GetSubChilds(catId);

            List<Cm_MachineparkMark> marks = _context.Cm_MachineparkMark.ToList();
            string html = "<h2>Müşteri Marka Dağılımı</h2></br><table class=\"table table-bordered table-striped table-condensed flip-content\">"
                         + "<thead><tr><th>Marka</th><th>G-1</th><th>F-3</th><th>E-6</th><th>D-11</th><th>C-21</th><th>B-51</th><th>A-101</th><th>Toplam</th>"
                         + "</tr></thead>"
                         + "<tbody>";

            string categoryi = "";
            foreach (int para in allChildIds)
                categoryi += para + "|";

            var categoryId = new SqlParameter("@categoryId", SqlDbType.NVarChar)
            {
                Value = categoryi
            };

            var res = _context.Database.SqlQuery<one_sp_GetMarkMpReportByCategoryId_Result>
                ("one_sp_GetMarkMpReportByCategoryId @categoryId", categoryId).ToList();
            res = res.OrderBy(k => k.Mark).ToList();


            foreach (one_sp_GetMarkMpReportByCategoryId_Result mr in res)
            {
                int tot = Convert.ToInt32(mr.G1) + Convert.ToInt32(mr.F3) + Convert.ToInt32(mr.E6) + Convert.ToInt32(mr.D11) + Convert.ToInt32(mr.C21) + Convert.ToInt32(mr.B51) + Convert.ToInt32(mr.A101);
                html += "<tr><td>" + mr.Mark + "</td>";
                html += "<td>" + mr.G1 + "</td>";
                html += "<td>" + mr.F3 + "</td>";
                html += "<td>" + mr.E6 + "</td>";
                html += "<td>" + mr.D11 + "</td>";
                html += "<td>" + mr.C21 + "</td>";
                html += "<td>" + mr.B51 + "</td>";
                html += "<td>" + mr.A101 + "</td>";
                html += "<td>" + tot + "</td></tr>";
            }

            return html += "</tbody></table>";
        }

        string CreateAccordingMark(int catAll)
        {
            int catId = Convert.ToInt32(ddCategories.SelectedItem.Value);
            List<int?> allChildIds = Utility.GetSubChilds(catId);
            string html = "<h2>Müşteri Marka Dağılımı</h2></br><table class=\"table table-bordered table-striped table-condensed flip-content\">"
                         + "<thead><tr><th>Marka</th><th>G-1</th><th>F-3</th><th>E-6</th><th>D-11</th><th>C-21</th><th>B-51</th><th>A-101</th><th>Toplam</th>"
                         + "</tr></thead>"
                         + "<tbody>";

            string categoryi = "";
            foreach (int para in allChildIds)
                categoryi += para + "|";

            var categoryId = new SqlParameter("@categoryId", SqlDbType.NVarChar)
            {
                Value = categoryi
            };

            var res = _context.Database.SqlQuery<one_sp_GetMarkReportByCategoryId_Result>
                ("one_sp_GetMarkReportByCategoryId @categoryId", categoryId).ToList();
            res = res.OrderBy(k => k.Mark).ToList();


            foreach (one_sp_GetMarkReportByCategoryId_Result mr in res)
            {
                int tot = Convert.ToInt32(mr.G1) + Convert.ToInt32(mr.F3) + Convert.ToInt32(mr.E6) + Convert.ToInt32(mr.D11) + Convert.ToInt32(mr.C21) + Convert.ToInt32(mr.B51) + Convert.ToInt32(mr.A101);
                html += "<tr><td>" + mr.Mark + "</td>";
                html += "<td>" + mr.G1 + "</td>";
                html += "<td>" + mr.F3 + "</td>";
                html += "<td>" + mr.E6 + "</td>";
                html += "<td>" + mr.D11 + "</td>";
                html += "<td>" + mr.C21 + "</td>";
                html += "<td>" + mr.B51 + "</td>";
                html += "<td>" + mr.A101 + "</td>";
                html += "<td>" + tot + "</td></tr>";
            }

            return html += "</tbody></table>";
        }

    }
}