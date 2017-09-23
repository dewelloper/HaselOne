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
    public partial class MachineparkSaleEngineersDetail : System.Web.UI.Page
    {
        HASELONEEntities _context = new HASELONEEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _context.Database.CommandTimeout = 300;
                Session["SEID"] = Request["SEID"]; // Sale engineer Id
                Session["SSID"] = Request["SSID"]; // selected segment Id
                int SEID = 0;
                if (Session["SEID"] != null)
                {
                    SEID = Convert.ToInt32(Session["SEID"]);
                    DFSUserSet user = _context.DFSUserSet.Where(k => k.Id == SEID).FirstOrDefault();
                    if (user != null)
                        saleEngineerName.InnerText = user.Name;
                }
                int SSID = Convert.ToInt32(Session["SSID"]);
                LoadContent(SEID, SSID);
            }
        }

        static List<int> _saleEngineerCustomerIds = new List<int>();
        List<int> GetNonSaleEnginerCount(List<int> custIds)
        {
            if (_saleEngineerCustomerIds.Count() == 0)
            {
                _saleEngineerCustomerIds = _context.One_CustomerSaleEngineer.DistinctBy(m => m.CustomerId).Select(k => k.CustomerId).ToList();
            }

            return custIds.Except(_saleEngineerCustomerIds).ToList();
        }

        private void LoadContent(int saleEngineerId, int selectedSegmentId)
        {
            string html = "<h2>Satıcı Müşteri Detayları</h2></br><table class=\"table table-bordered table-striped table-condensed flip-content\">"
                         + "<thead><tr><th>Müşteri adı</th><th>Toplam Makine Park Adeti</th><th>Satıcı Makine Parkı Adeti</th><th>Detay</th>"
                         + "</tr></thead>"
                         + "<tbody>";
            List<int> cIds = _context.One_CustomerSaleEngineer.Where(k => k.DocumentUserId == saleEngineerId).GroupBy(m => m.CustomerId).Select(m => m.FirstOrDefault()).Select(c => c.CustomerId).ToList();

            if (saleEngineerId == 0)
            {
                List<int> xoneTree = _context.One_CustomerMachinepark.Where(k => k.IsActive == true && k.IsDeleted == false).DistinctBy(z => z.CustomerId).Select(m => m.CustomerId).ToList();
                cIds = GetNonSaleEnginerCount(xoneTree);
            }

            List<int?> saleEngineerCategoryIds = new List<int?>();
            foreach (int customerId in cIds)
            {
                if (saleEngineerId > 0)
                {
                    saleEngineerCategoryIds = _context.One_CustomerSaleEngineer.Where(k => k.CustomerId == customerId && k.DocumentUserId == saleEngineerId).Select(c => c.CategoryId).ToList();
                }

                HSL_CARI cari = _context.HSL_CARI.Where(k => k.Id == customerId).FirstOrDefault();
                if (cari != null)
                {
                    int? mpcCount = _context.One_CustomerMachinepark.Where(k => k.CustomerId == customerId && k.IsActive == true && k.IsDeleted == false).Sum(p => p.Count);
                    int? saleEngineerMPCount = _context.One_CustomerMachinepark.Where(k => saleEngineerCategoryIds.Contains(k.CategoryId) && k.CustomerId == customerId && k.IsActive == true && k.IsDeleted == false).Sum(p => p.Count);
                    html += "<tr>"
                         + " <td><a class=\"linkHover\" href=\"/Customers.Aspx?custId=" + cari.Id + "\">" + cari.HSL_CARIISIM + "</a></td>"
                         + "<td class=\"bg-info\">" + mpcCount + "</td>"
                         + "<td class=\"bg-warning\">" + saleEngineerMPCount + "</td>"
                         + "<td><a role=\"button\" Style=\"cursor:pointer;\" class=\"btn btn-warning\" href=\"/Moduls\\Reports\\MachineparkCustomerDetail.aspx?cariId=" + cari.Id + "\">Makine Parkı</a></td>"
                         + "</tr>";
                }
            }

            saleEngineerDetailContent.InnerHtml = html;
        }
    }
}