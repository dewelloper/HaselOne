using DAL;
using HaselOne.Services.Interfaces;
using HaselOne.Util;
using Microsoft.Practices.Unity;
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
    public partial class MachineparkSaleEngineers : System.Web.UI.Page
    {
        HASELONEEntities _context = new HASELONEEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            _context.Database.CommandTimeout = 300;
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

        string CreateAccordingSaleEngineer(int catAll)
        {
            int catId = Convert.ToInt32(ddCategories.SelectedItem.Value);
            List<int?> allChildIds = Utility.GetSubChilds(catId);

            string html = "<h2>Satıcı Müşteri Dağılımı</h2></br><table class=\"table table-bordered table-striped table-condensed flip-content\">"
                         + "<thead><tr><th>Satış Müh.</th><th>Zero</th><th>G-1</th><th>F-3</th><th>E-6</th><th>D-11</th><th>D-21</th>"
                         + "<th>B-51</th><th>A-101</th><th>Total</th></tr></thead>"
                         + "<tbody>";

            string categoryi = "";
            foreach (int para in allChildIds)
                categoryi += para + "|";

            var categoryId = new SqlParameter("@categoryId", SqlDbType.NVarChar)
            {
                Value = categoryi
            };

            var res = _context.Database.SqlQuery<one_sp_GetSalesEngineerMpReportByCategoryId_Result>
                ("one_sp_GetSalesEngineerMpReportByCategoryId @categoryId", categoryId).ToList();


            one_sp_GetSalesEngineerMpReportByCategoryId_Result orphan = res.Where(k => k.SaleEngineer == null).FirstOrDefault();

            int tot = Convert.ToInt32(orphan.G1) + Convert.ToInt32(orphan.F3) + Convert.ToInt32(orphan.E6) + Convert.ToInt32(orphan.D11) + Convert.ToInt32(orphan.C21) + Convert.ToInt32(orphan.B51) + Convert.ToInt32(orphan.A101);
            html += "<tr>"
                 + " <td><a href=\"MachineparkSaleEngineersDetail.Aspx?SEID=0\">Orphaned</a></td>"
                 + "<td></td>"
                 + "<td>" + orphan.G1 + "</td>"
                 + "<td>" + orphan.F3 + "</td>"
                 + "<td>" + orphan.E6 + "</td>"
                 + "<td>" + orphan.D11 + "</td>"
                 + "<td>" + orphan.C21 + "</td>"
                 + "<td>" + orphan.B51 + "</td>"
                 + "<td>" + orphan.A101 + "</td>"
                 + "<td>" + tot + "</td>"
                 + "</tr>";

            foreach (one_sp_GetSalesEngineerMpReportByCategoryId_Result x in res)
            {
                if (x.SaleEngineer == null)
                    continue;
                string oneTree = x.G1;
                string treeSix = x.F3;
                string sixEleven = x.E6;
                string elevenTwentyone = x.D11;
                string twentyoneFivtyone = x.C21;
                string fivtyoneHundaradone = x.B51;
                string hunderadoneMore = x.A101;
                int total = Convert.ToInt32(oneTree) + Convert.ToInt32(treeSix) + Convert.ToInt32(sixEleven) + Convert.ToInt32(elevenTwentyone) + Convert.ToInt32(twentyoneFivtyone) + Convert.ToInt32(fivtyoneHundaradone) + Convert.ToInt32(hunderadoneMore);
                int zero = 0;
                string userName = x.SaleEngineer;
                DFSUserSet user = _context.DFSUserSet.Where(k => k.Name.Equals(userName)).FirstOrDefault();

                List<int> saleEngineerCustomerIds = _context.One_CustomerSaleEngineer.Where(k => k.DocumentUserId == user.Id && allChildIds.Contains(k.CategoryId) && k.IsActive == true && k.IsDeleted == false).DistinctBy(c => c.CustomerId).Select(m => m.CustomerId).ToList();
                List<int> customerIds = _context.One_CustomerSaleEngineer.Where(k => k.DocumentUserId == user.Id && k.IsActive == true && k.IsDeleted == false).DistinctBy(c => c.CustomerId).Select(m => m.CustomerId).ToList();
                List<int> saleEngineerCustomerList = _context.One_CustomerSaleEngineer.Where(k => k.DocumentUserId == user.Id).DistinctBy(m => m.CustomerId).Select(n => n.CustomerId).ToList();
                int saleEngineerOne_CustomerMachineparkCount = _context.One_CustomerMachinepark.Where(k => allChildIds.Contains(k.CategoryId) && saleEngineerCustomerList.Contains(k.CustomerId)).DistinctBy(c => c.CustomerId).ToList().Count();
                zero = saleEngineerCustomerList.Count() - saleEngineerOne_CustomerMachineparkCount;

                if (total == 0)
                    continue;

                html += "<tr>"
                     + " <td><a href=\"MachineparkSaleEngineersDetail.Aspx?SEID=" + user.Id + "\">" + user.Name + "</a></td>"
                     + "<td>" + zero + "</td>"
                     + "<td>" + oneTree + "</td>"
                     + "<td>" + treeSix + "</td>"
                     + "<td>" + sixEleven + "</td>"
                     + "<td>" + elevenTwentyone + "</td>"
                     + "<td>" + twentyoneFivtyone + "</td>"
                     + "<td>" + fivtyoneHundaradone + "</td>"
                     + "<td>" + hunderadoneMore + "</td>"
                     + "<td>" + total + "</td>"
                     + "</tr>";
            };

            return html += "</tbody></table>";
        }

        static List<int> _saleEngineerCustomerIds = new List<int>();
        int GetNonSaleEnginerCount(List<int> custIds)
        {
            if (_saleEngineerCustomerIds.Count() == 0)
            {
                _saleEngineerCustomerIds = _context.One_CustomerSaleEngineer.DistinctBy(m => m.CustomerId).Select(k => k.CustomerId).ToList();
            }

            return custIds.Except(_saleEngineerCustomerIds).Count();
        }

        static List<int> _saleEngineerCustomerIds2 = new List<int>();
        int GetNonSaleEnginerParkCount(List<int> custIds)
        {
            if (_saleEngineerCustomerIds2.Count() == 0)
            {
                _saleEngineerCustomerIds2 = _context.One_CustomerSaleEngineer.DistinctBy(m => m.CustomerId).Select(k => k.CustomerId).ToList();
            }

            List<int> res = custIds.Except(_saleEngineerCustomerIds2).ToList();
            int? ress = _context.One_CustomerMachinepark.Where(k => res.Contains(k.CustomerId)).Sum(m => m.Count);
            if (ress != null)
                return Convert.ToInt32(ress);
            return 0;
        }

        string CreateAccordingMachine(int catAll)
        {
            int catId = Convert.ToInt32(ddCategories.SelectedItem.Value);
            List<int?> allChildIds = Utility.GetSubChilds(catId);

            string html = "<h2>Satıcı Makine Dağılımı</h2></br><table class=\"table table-bordered table-striped table-condensed flip-content\">"
                         + "<thead><tr><th>Satış Müh.</th><th>Zero</th><th>G-1</th><th>F-3</th><th>E-6</th><th>D-11</th><th>D-21</th>"
                         + "<th>B-51</th><th>A-101</th><th>Total</th></tr></thead>"
                         + "<tbody>";

          

            string categoryi = "";
            foreach (int para in allChildIds)
                categoryi += para + "|";

            var categoryId = new SqlParameter("@categoryId", SqlDbType.NVarChar)
            {
                Value = categoryi
            };

            var res = _context.Database.SqlQuery<one_sp_GetSalesEngineerReportByCategoryId_Result>
                ("one_sp_GetSalesEngineerReportByCategoryId @categoryId", categoryId).ToList();

            res = res.OrderBy(k => k.SaleEngineer).ToList();
            one_sp_GetSalesEngineerReportByCategoryId_Result orphan = res.Where(k => k.SaleEngineer == null).FirstOrDefault();

            int tot = Convert.ToInt32(orphan.G1) + Convert.ToInt32(orphan.F3) + Convert.ToInt32(orphan.E6) + Convert.ToInt32(orphan.D11) + Convert.ToInt32(orphan.C21) + Convert.ToInt32(orphan.B51) + Convert.ToInt32(orphan.A101);
            html += "<tr>"
                 + " <td><a href=\"MachineparkSaleEngineersDetail.Aspx?SEID=0\">Orphaned</a></td>"
                 + "<td></td>"
                 + "<td>" + orphan.G1 + "</td>"
                 + "<td>" + orphan.F3 + "</td>"
                 + "<td>" + orphan.E6 + "</td>"
                 + "<td>" + orphan.D11 + "</td>"
                 + "<td>" + orphan.C21 + "</td>"
                 + "<td>" + orphan.B51 + "</td>"
                 + "<td>" + orphan.A101 + "</td>"
                 + "<td>" + tot + "</td>"
                 + "</tr>";

            foreach (one_sp_GetSalesEngineerReportByCategoryId_Result x in res)
            {
                if (x.SaleEngineer == null)
                    continue;
                string oneTree = x.G1;
                string treeSix = x.F3;
                string sixEleven = x.E6;
                string elevenTwentyone = x.D11;
                string twentyoneFivtyone = x.C21;
                string fivtyoneHundaradone = x.B51;
                string hunderadoneMore = x.A101;
                int total = Convert.ToInt32(oneTree) + Convert.ToInt32(treeSix) + Convert.ToInt32(sixEleven) + Convert.ToInt32(elevenTwentyone) + Convert.ToInt32(twentyoneFivtyone) + Convert.ToInt32(fivtyoneHundaradone) + Convert.ToInt32(hunderadoneMore);
                int zero = 0;
                string userName = x.SaleEngineer;
                DFSUserSet user = _context.DFSUserSet.Where(k => k.Name.Equals(userName)).FirstOrDefault();
                if (user == null)
                {
                    html += "<tr>"
                         + " <td><a href=\"MachineparkSaleEngineersDetail.Aspx?SEID=0\">Non User</a></td>"
                         + "<td>" + zero + "</td>"
                         + "<td>" + oneTree + "</td>"
                         + "<td>" + treeSix + "</td>"
                         + "<td>" + sixEleven + "</td>"
                         + "<td>" + elevenTwentyone + "</td>"
                         + "<td>" + twentyoneFivtyone + "</td>"
                         + "<td>" + fivtyoneHundaradone + "</td>"
                         + "<td>" + hunderadoneMore + "</td>"
                         + "<td>" + total + "</td>"
                         + "</tr>";
                    continue;
                }

                List<int> saleEngineerCustomerIds = _context.One_CustomerSaleEngineer.Where(k => k.DocumentUserId == user.Id && allChildIds.Contains(k.CategoryId) && k.IsActive == true && k.IsDeleted == false).DistinctBy(c => c.CustomerId).Select(m => m.CustomerId).ToList();
                List<int> customerIds = _context.One_CustomerSaleEngineer.Where(k => k.DocumentUserId == user.Id && k.IsActive == true && k.IsDeleted == false).DistinctBy(c => c.CustomerId).Select(m => m.CustomerId).ToList();
                List<int> saleEngineerCustomerList = _context.One_CustomerSaleEngineer.Where(k => k.DocumentUserId == user.Id).DistinctBy(m => m.CustomerId).Select(n => n.CustomerId).ToList();
                int saleEngineerOne_CustomerMachineparkCount = _context.One_CustomerMachinepark.Where(k => allChildIds.Contains(k.CategoryId) && saleEngineerCustomerList.Contains(k.CustomerId)).DistinctBy(c => c.CustomerId).ToList().Count();
                zero = saleEngineerCustomerList.Count() - saleEngineerOne_CustomerMachineparkCount;

                if (total == 0)
                    continue;

                html += "<tr>"
                     + " <td><a href=\"MachineparkSaleEngineersDetail.Aspx?SEID=" + user.Id + "\">" + user.Name + "</a></td>"
                     + "<td>" + zero + "</td>"
                     + "<td>" + oneTree + "</td>"
                     + "<td>" + treeSix + "</td>"
                     + "<td>" + sixEleven + "</td>"
                     + "<td>" + elevenTwentyone + "</td>"
                     + "<td>" + twentyoneFivtyone + "</td>"
                     + "<td>" + fivtyoneHundaradone + "</td>"
                     + "<td>" + hunderadoneMore + "</td>"
                     + "<td>" + total + "</td>"
                     + "</tr>";
            };

            return html += "</tbody></table>";
        }


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            int catAll = chkAll.Checked == true ? 1 : 0;
            if (ddCategories.SelectedItem.Value == "0")
                catAll = 1;
            categorySaleEngineerContent.InnerHtml = CreateAccordingSaleEngineer(catAll);
        }

        protected void btnMachineLoad_Click(object sender, EventArgs e)
        {
            int catAll = chkAll.Checked == true ? 1 : 0;
            if (ddCategories.SelectedItem.Value == "0")
                catAll = 1;
            categorySaleEngineerContent.InnerHtml = CreateAccordingMachine(catAll);
        }
    }
}