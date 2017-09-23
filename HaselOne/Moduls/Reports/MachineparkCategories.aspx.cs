using DAL;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using HaselOne.Services.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne
{
    public partial class MachineparkCategories : System.Web.UI.Page
    {
        [Dependency]
        public ICustomerService _cs { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadcategoryCustomerContent();
            }
        }

        private void LoadcategoryCustomerContent()
        {
            string html = "<table class=\"table table-bordered table-striped table-condensed flip-content\"><thead><tr><th></th><th>G-1</th><th>F-3</th><th>E-6</th><th>D-11</th><th>C-21</th><th>B-51</th><th>A-101</th><th>Toplam</th></tr></thead>";
            //List<One_CustomerMachineparkCategories> mps = _cs.GetCustomerMP()Categories.Where(k => k.ParentId == 0).ToList();
            List<One_CustomerMachineparkCategories> mps = _cs.GetCategories().Where(k => k.ParentId == 0).ToList();
            //var custsss = _cs.GetCustomers().Where(k => k.ParentId == 0).ToList();
            int i = 0;
            List<int> subChildItemIds = new List<int>();
            foreach (One_CustomerMachineparkCategories cat in mps)
            {
                html += "<tr class=\"success\"><td class=\"topTD\">" + cat.CategoryName + "</td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "</tr>";
                IQueryable<One_CustomerMachineparkCategories> catss = _cs.GetCategories();
                List<One_CustomerMachineparkCategories> mps2 = catss.Where(k => k.ParentId == cat.Id).ToList();
                foreach (One_CustomerMachineparkCategories cat2 in mps2)
                {
                    if (subChildItemIds.Contains(cat2.Id))
                        continue;

                    int thisChildIsAParent = cat2.Id;
                    List<One_CustomerMachineparkCategories> subChilds = catss.Where(k => k.ParentId == thisChildIsAParent).ToList();
                    subChildItemIds.AddRange(subChilds.Select(k => k.Id).ToList());
                    if (subChilds.Count > 0)
                    {
                        html += "<tr ><td class=\"middleTD\">" + cat2.CategoryName + "</td>";
                        html += "<td></td>";
                        html += "<td></td>";
                        html += "<td></td>";
                        html += "<td></td>";
                        html += "<td></td>";
                        html += "<td></td>";
                        html += "<td></td>";
                        html += "<td></td>";
                        html += "</tr>";
                        foreach (One_CustomerMachineparkCategories cat3 in subChilds)
                        {
                            i++;
                            html += "<tr ><td class=\"rightTD\">" + cat3.CategoryName + "</td>";
                            int oneCount3 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.Count >= 0 && k.Count < 3 && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td >" + oneCount3 + "</td>";
                            int treeCount3 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.Count >= 3 && k.Count < 6 && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td>" + treeCount3 + "</td>";
                            int elevenCount3 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.Count >= 6 && k.Count < 11 && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td>" + elevenCount3 + "</td>";
                            int twentyOneCount3 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.Count >= 11 && k.Count < 21 && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td>" + twentyOneCount3 + "</td>";
                            int fiwtyOneCount3 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.Count >= 21 && k.Count < 51 && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td>" + fiwtyOneCount3 + "</td>";
                            int hundaradOneCount3 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.Count >= 51 && k.Count < 101 && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td>" + hundaradOneCount3 + "</td>";
                            int hundaradOneCount3More = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.Count >= 101 && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td>" + hundaradOneCount3More + "</td>";
                            int totalCount3 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat3.Id && k.IsActive != false && k.IsDeleted != true).Count();
                            html += "<td>" + totalCount3 + "</td>";
                            html += "</tr>";
                        }
                    }
                    else
                    {
                        i++;
                        html += "<tr ><td class=\"middleTD\">" + cat2.CategoryName + "</td>";
                        int oneCount2 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.Count >= 0 && k.Count < 3 && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td >" + oneCount2 + "</td>";
                        int treeCount2 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.Count >= 3 && k.Count < 6 && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td>" + treeCount2 + "</td>";
                        int elevenCount2 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.Count >= 6 && k.Count < 11 && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td>" + elevenCount2 + "</td>";
                        int twentyOneCount2 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.Count >= 11 && k.Count < 21 && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td>" + twentyOneCount2 + "</td>";
                        int fiwtyOneCount2 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.Count >= 21 && k.Count < 51 && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td>" + fiwtyOneCount2 + "</td>";
                        int hundaradOneCount2 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.Count >= 51 && k.Count < 101 && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td>" + hundaradOneCount2 + "</td>";
                        int hundaradOneCount2More = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.Count >= 101 && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td>" + hundaradOneCount2More + "</td>";
                        int totalCount2 = _cs.GetCustomerMP().Where(k => k.CategoryId == cat2.Id && k.IsActive != false && k.IsDeleted != true).Count();
                        html += "<td>" + totalCount2 + "</td>";
                        html += "</tr>";
                    }
                }
            }

            html += "</table>";

            categoryCustomerContent.InnerHtml = html;
        }
    }
}