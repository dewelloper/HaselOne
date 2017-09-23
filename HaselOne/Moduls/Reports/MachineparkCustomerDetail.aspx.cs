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
    public partial class MachineparkCustomerDetail : System.Web.UI.Page
    {
        HASELONEEntities _context = new HASELONEEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["CustomerId"] = Request["cariId"];
                if (Session["CustomerId"] != null)
                {
                    int customerId = Convert.ToInt32(Session["CustomerId"]);
                    customerName.InnerText = _context.HSL_CARI.Where(k => k.Id == customerId).FirstOrDefault().HSL_CARIISIM;
                    LoadcategoryCustomerContent(customerId);
                }
            }
        }

        private void LoadcategoryCustomerContent(int customerId)
        {
            string html = "<table class=\"table\"><thead><tr><th>Kategori</th><th>Adet</th></tr></thead>";
            List<One_CustomerMachineparkCategories> mps = _context.One_CustomerMachineparkCategories.Where(k => k.ParentId == 0).ToList();
            int i = 0;
            List<int> subChildItemIds = new List<int>();
            foreach (One_CustomerMachineparkCategories cat in mps)
            {
                List<int?> schilds = Utility.GetSubChilds(cat.Id);
                int? totalmpCount = _context.One_CustomerMachinepark.Where(k => k.CustomerId == customerId && schilds.Contains(k.CategoryId) && k.IsActive != false && k.IsDeleted != true).Sum(m => m.Count);
                if (totalmpCount <= 0 || totalmpCount == null)
                    continue;

                html += "<tr class=\"bg-primary\"><td class=\"topTD\">" + cat.CategoryName + "</td>";
                html += "<td><a class=\"toLeftTub\">" + totalmpCount + "</a></td>";
                html += "</tr>";
                List<One_CustomerMachineparkCategories> mps2 = _context.One_CustomerMachineparkCategories.Where(k => k.ParentId == cat.Id).ToList();
                foreach (One_CustomerMachineparkCategories cat2 in mps2)
                {
                    if (subChildItemIds.Contains(cat2.Id))
                        continue;

                    int thisChildIsAParent = cat2.Id;
                    List<One_CustomerMachineparkCategories> subChilds = _context.One_CustomerMachineparkCategories.Where(k => k.ParentId == thisChildIsAParent).ToList();
                    subChildItemIds.AddRange(subChilds.Select(k => k.Id).ToList());
                    if (subChilds.Count > 0)
                    {
                        List<int?> scids = Utility.GetSubChilds(cat2.Id);
                        int? oneCount2 = _context.One_CustomerMachinepark.Where(k => k.CustomerId == customerId && scids.Contains(k.CategoryId) && k.IsActive != false && k.IsDeleted != true).Sum(m => m.Count);
                        if (oneCount2 <= 0 || oneCount2 == null)
                            continue;

                        html += "<tr class=\"success\"><td class=\"middleTD\">" + cat2.CategoryName + "</td>";
                        html += "<td><a class=\"toLeftTub \">" + oneCount2 + "</a></td>";
                        html += "</tr>";
                        foreach (One_CustomerMachineparkCategories cat3 in subChilds)
                        {
                            i++;
                            int? oneCount3 = _context.One_CustomerMachinepark.Where(k => k.CustomerId == customerId && k.CategoryId == cat3.Id && k.IsActive != false && k.IsDeleted != true).Sum(m => m.Count);
                            if (oneCount3 <= 0 || oneCount3 == null)
                                continue;
                            html += "<tr ><td class=\"rightTD\">" + cat3.CategoryName + "</td>";
                            html += "<td >" + oneCount3 + "</td>";
                            html += "</tr>";
                        }
                    }
                    else
                    {
                        i++;
                        int? oneCount2 = _context.One_CustomerMachinepark.Where(k => k.CustomerId == customerId && k.CategoryId == cat2.Id && k.IsActive != false && k.IsDeleted != true).Sum(m => m.Count);
                        if (oneCount2 <= 0 || oneCount2 == null)
                            continue;

                        html += "<tr ><td class=\"middleTD\">" + cat2.CategoryName + "</td>";
                        html += "<td >" + oneCount2 + "</td>";
                        html += "</tr>";
                    }
                }
            }

            html += "</table>";

            categoryCustomerDetailContent.InnerHtml = html;
        }
    }
}