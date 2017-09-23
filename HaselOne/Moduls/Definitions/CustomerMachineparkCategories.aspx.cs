using DAL;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Helper;

namespace HaselOne
{



    public partial class CustomerMachineparkCategories : System.Web.UI.Page
    {
        private Mode pageMode;
        private int Id { get; set; }
        [Dependency]
        public ICustomerService _cs { get; set; }

        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ModulId"] = 8;
            Session["PageId"] = 12;

           

            if (Request.QueryString["Id"] != null)
            {
                pageMode = Mode.Edit;
                Id = Convert.ToInt32(Request.QueryString["Id"]);
            }
            else
            {
                pageMode = Mode.Insert;
            }

            if (!IsPostBack)
            {
                Util.Utility.LoadCombo(ddCustomerMachineparkCategoriParent, _cs.GetCategories().Where(m => m.ParentId == 0).OrderBy(k => k.CategoryName).ToList(), "CategoryName", "Id");
                ddCustomerMachineparkCategoriParent.Items.Insert(0, new ListItem() { Value = "0", Text = "Ana Kategori" });

                txtCustomerMachineparkCategoriSub.Text = "";

            }

            if (pageMode == Mode.Edit)
            {

                var entity = _cs.GetCategories().FirstOrDefault(m => m.Id == this.Id);
                if (entity == null)
                    throw new Exception("Boyle bir kayit yok");

                ddCustomerMachineparkCategoriParent.SelectedValue = entity.ParentId.ToString();
                txtCustomerMachineparkCategoriSub.Text = entity.CategoryName;
            }

            containerList.InnerHtml = CreateGridHtml();


        }

        private string CreateGridHtml()
        {
            var tempList = _cs.GetCategories();
            var list = (from category in tempList
                        join parentCategoriy in tempList on category.ParentId equals parentCategoriy.Id
                        select new TempDto()
                        {
                            Id = category.Id,
                            Parent = parentCategoriy.CategoryName,
                            Title = category.CategoryName
                        }).ToList();


            list.AddRange(tempList.Where(m => m.ParentId == 0).Select(m => new TempDto()
            {
                Id = m.Id,
                Title = m.CategoryName,
                Parent = "Ana Kategori"
            })
            .ToList().OrderBy(m => m.Parent));

            string custListHtml = " <div class=\"portlet-body flip-scroll\">"
                              + "          <table class=\"table table-bordered table-striped table-condensed flip-content\">"
                              + "              <thead class=\"flip -content\">"
                              + "                  <tr>"
                              + "                      <th>Ana Kategori</th>"
                              + "                      <th>Kategori</th>"
                              + "                      <th>Sil</th>"
                              + "                      <th>Düzenle</th>"
                              + "                  </tr>"
                              + "              </thead>"
                              + "              <tbody>";
            string path = HttpContext.Current.Request.Url.AbsolutePath;
            foreach (var item in list)
            {
                string backColorClass = "bckColorGreen";
                if (item.IsDelete) backColorClass = "bckColorRed";
                custListHtml += "<tr>"
                             + "   <td>" + item.Parent +"</td>"
                             + "   <td>" + item.Title  +"</td>"
                             + "   <td><span class=\"label label-sm label - info " + backColorClass + "\"><a alt=\"" + item.IsDelete + "\" onclick=\"Delete(" + item.Id + ");\">Sil</a></span></td>"
                             + "   <td><a href='" +String. Format(path + "?id=" + item.Id) + "'/>Düzenle</td>"
                             + "</tr>";
            }
            custListHtml += "</tbody></table></div>";
            return custListHtml;
        }


    }

    public class TempDto
    {
        public int Id { get; set; }
        public string Parent { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}