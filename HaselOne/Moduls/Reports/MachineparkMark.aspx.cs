using DAL;
using HaselOne.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class MachineparkMark : System.Web.UI.Page
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
            ddCategories.DataSource = Utility.GetCategoryDropdownSource(); 
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

            List<Cm_MachineparkMark> markss = _context.Cm_MachineparkMark.ToList();
            if (catAll == 0)
            {
                foreach (Cm_MachineparkMark mark in markss)
                {
                    html += "<tr><td>" + mark.MarkName + "</td>";
                    List<One_CustomerMachinepark> mparks = _context.One_CustomerMachinepark.Where(k => allChildIds.Contains(k.CategoryId) && k.MarkId == mark.Id && k.IsActive == true && k.IsDeleted == false).ToList();
                    int? g1 = ConsolidateWithCount(mparks).Where(k => k.Count >= 1 && k.Count < 3).Sum(s => s.Count);
                    html += "<td>" + g1 + "</td>";
                    int? f3 = ConsolidateWithCount(mparks).Where(k => k.Count >= 3 && k.Count < 6).Sum(s => s.Count);
                    html += "<td>" + f3 + "</td>";
                    int? e6 = ConsolidateWithCount(mparks).Where(k => k.Count >= 6 && k.Count < 11).Sum(s => s.Count);
                    html += "<td>" + e6 + "</td>";
                    int? d11 = ConsolidateWithCount(mparks).Where(k => k.Count >= 11 && k.Count < 21).Sum(s => s.Count);
                    html += "<td>" + d11 + "</td>";
                    int? c21 = ConsolidateWithCount(mparks).Where(k => k.Count >= 21 && k.Count < 51).Sum(s => s.Count);
                    html += "<td>" + c21 + "</td>";
                    int? b51 = ConsolidateWithCount(mparks).Where(k => k.Count >= 51 && k.Count < 101).Sum(s => s.Count);
                    html += "<td>" + b51 + "</td>";
                    int? a101 = ConsolidateWithCount(mparks).Where(k => k.Count >= 101).Sum(s => s.Count);
                    html += "<td>" + a101 + "</td>";
                    int? total = ConsolidateWithCount(mparks).Sum(s => s.Count);
                    html += "<td>" + total + "</td></tr>";
                }
            }
            else
            {
                foreach (Cm_MachineparkMark mark in marks)
                {
                    html += "<tr><td>" + mark.MarkName + "</td>";
                    List<One_CustomerMachinepark> mparks = _context.One_CustomerMachinepark.Where(k => k.MarkId == mark.Id && k.IsActive == true && k.IsDeleted == false).ToList();
                    int? g1 = ConsolidateWithCount(mparks).Where(k => k.Count >= 1 && k.Count < 3).Sum(s => s.Count);
                    html += "<td>" + g1 + "</td>";
                    int? f3 = ConsolidateWithCount(mparks).Where(k => k.Count >= 3 && k.Count < 6).Sum(s => s.Count);
                    html += "<td>" + f3 + "</td>";
                    int? e6 = ConsolidateWithCount(mparks).Where(k => k.Count >= 6 && k.Count < 11).Sum(s => s.Count);
                    html += "<td>" + e6 + "</td>";
                    int? d11 = ConsolidateWithCount(mparks).Where(k => k.Count >= 11 && k.Count < 21).Sum(s => s.Count);
                    html += "<td>" + d11 + "</td>";
                    int? c21 = ConsolidateWithCount(mparks).Where(k => k.Count >= 21 && k.Count < 51).Sum(s => s.Count);
                    html += "<td>" + c21 + "</td>";
                    int? b51 = ConsolidateWithCount(mparks).Where(k => k.Count >= 51 && k.Count < 101).Sum(s => s.Count);
                    html += "<td>" + b51 + "</td>";
                    int? a101 = ConsolidateWithCount(mparks).Where(k => k.Count >= 101).Sum(s => s.Count);
                    html += "<td>" + a101 + "</td>";
                    int? total = ConsolidateWithCount(mparks).Sum(s => s.Count);
                    html += "<td>" + total + "</td></tr>";
                }
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

            List<Cm_MachineparkMark> marks = _context.Cm_MachineparkMark.ToList();
            if (catAll == 0)
            {
                foreach (Cm_MachineparkMark mark in marks)
                {
                    html += "<tr><td>" + mark.MarkName + "</td>";
                    List<One_CustomerMachinepark> mparks = _context.One_CustomerMachinepark.Where(k => allChildIds.Contains(k.CategoryId) && k.MarkId == mark.Id && k.IsActive == true && k.IsDeleted == false).ToList();
                    int g1 = ConsolidateWithCount(mparks).Where(k => k.Count >= 1 && k.Count < 3).Count();
                    html += "<td>" + g1 + "</td>";
                    int f3 = ConsolidateWithCount(mparks).Where(k => k.Count >= 3 && k.Count < 6).Count();
                    html += "<td>" + f3 + "</td>";
                    int e6 = ConsolidateWithCount(mparks).Where(k => k.Count >= 6 && k.Count < 11).Count();
                    html += "<td>" + e6 + "</td>";
                    int d11 = ConsolidateWithCount(mparks).Where(k => k.Count >= 11 && k.Count < 21).Count();
                    html += "<td>" + d11 + "</td>";
                    int c21 = ConsolidateWithCount(mparks).Where(k => k.Count >= 21 && k.Count < 51).Count();
                    html += "<td>" + c21 + "</td>";
                    int b51 = ConsolidateWithCount(mparks).Where(k => k.Count >= 51 && k.Count < 101).Count();
                    html += "<td>" + b51 + "</td>";
                    int a101 = ConsolidateWithCount(mparks).Where(k => k.Count >= 101).Count();
                    html += "<td>" + a101 + "</td>";
                    int total = ConsolidateWithCount(mparks).Count();
                    html += "<td>" + total + "</td></tr>";
                }
            }
            else
            {
                foreach (Cm_MachineparkMark mark in marks)
                {
                    html += "<tr><td>" + mark.MarkName + "</td>";
                    List<One_CustomerMachinepark> mparks = _context.One_CustomerMachinepark.Where(k => k.MarkId == mark.Id && k.IsActive == true && k.IsDeleted == false).ToList();
                    int g1 = ConsolidateWithCount(mparks).Where(k => k.Count >= 1 && k.Count < 3).Count();
                    html += "<td>" + g1 + "</td>";
                    int f3 = ConsolidateWithCount(mparks).Where(k => k.Count >= 3 && k.Count < 6).Count();
                    html += "<td>" + f3 + "</td>";
                    int e6 = ConsolidateWithCount(mparks).Where(k => k.Count >= 6 && k.Count < 11).Count();
                    html += "<td>" + e6 + "</td>";
                    int d11 = ConsolidateWithCount(mparks).Where(k => k.Count >= 11 && k.Count < 21).Count();
                    html += "<td>" + d11 + "</td>";
                    int c21 = ConsolidateWithCount(mparks).Where(k => k.Count >= 21 && k.Count < 51).Count();
                    html += "<td>" + c21 + "</td>";
                    int b51 = ConsolidateWithCount(mparks).Where(k => k.Count >= 51 && k.Count < 101).Count();
                    html += "<td>" + b51 + "</td>";
                    int a101 = ConsolidateWithCount(mparks).Where(k => k.Count >= 101).Count();
                    html += "<td>" + a101 + "</td>";
                    int total = ConsolidateWithCount(mparks).Count();
                    html += "<td>" + total + "</td></tr>";
                }
            }
            return html += "</tbody></table>";
        }

        List<One_CustomerMachinepark> ConsolidateWithCount(List<One_CustomerMachinepark> engineerOne_CustomerMachineparks)
        {
            List<One_CustomerMachinepark> consolidatedList = new List<One_CustomerMachinepark>();
            foreach (One_CustomerMachinepark cmp in engineerOne_CustomerMachineparks)
            {
                One_CustomerMachinepark currCustMachPark = consolidatedList.Where(k => k.CustomerId == cmp.CustomerId).FirstOrDefault();
                if (currCustMachPark == null)
                {
                    int? xOne_CustomerMachineparkCount = engineerOne_CustomerMachineparks.Where(k => k.CustomerId == cmp.CustomerId).Sum(m => m.Count);
                    consolidatedList.Add(new One_CustomerMachinepark()
                    {
                        Count = xOne_CustomerMachineparkCount,
                        CategoryId = cmp.CategoryId,
                        CustomerId = cmp.CustomerId,
                        MarkId = cmp.MarkId
                    });
                }
            }

            return consolidatedList;
        }


    }
}