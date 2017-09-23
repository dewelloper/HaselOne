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
   


    public partial class CategoryRightDetails : System.Web.UI.Page
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
            Session["PageId"] = 13;

          

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
                Util.Utility.LoadCategories(ddCustomerMachineparkCategoriy, _cs.GetCategories().OrderBy(k => k.CategoryName).ToList());
                ddCustomerMachineparkCategoriy.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });

                Util.Utility.LoadCombo(ddCategoriRightGroups, _cs.GetOperationsAll().ToList(), "Title", "Id");
                ddCategoriRightGroups.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });


            }

            if (pageMode == Mode.Edit)
            {

                var entity = _cs.GetCategoryRightDetails().FirstOrDefault(m => m.Id == this.Id);
                if (entity == null)
                    throw new Exception("Boyle bir kayit yok");

                ddCategoriRightGroups.SelectedValue = entity.CRGId.ToString();
                ddCustomerMachineparkCategoriy.SelectedValue = entity.CategoryId.ToString();
                
            }

            containerList.InnerHtml = CreateGridHtml();


        }

        private string CreateGridHtml()
        {
            
            var list =( from catRigDet in _cs.GetCategoryRightDetails()
               join categorie in _cs.GetOperationsAll() on catRigDet.CRGId equals categorie.Id
                join mpcategori in _cs.GetCategories() on catRigDet.CategoryId equals  mpcategori.Id
                       
                select new
                {
                    catRigDet.Id,
                    catRigDet.IsDeleted,
                    categoriTitle =categorie.Title,
                    mpCategoryName=  mpcategori.CategoryName,
                    
                }).ToList();
           

            
            string custListHtml = " <div class=\"portlet-body flip-scroll\">"
                              + "          <table class=\"table table-bordered table-striped table-condensed flip-content\">"
                              + "              <thead class=\"flip -content\">"
                              + "                  <tr>"
                              + "                      <th>Kategori</th>"
                              + "                      <th>Grup</th>"
                              + "                      <th>Sil</th>"
                              + "                      <th>Düzenle</th>"
                              + "                  </tr>"
                              + "              </thead>"
                              + "              <tbody>";
            string path = HttpContext.Current.Request.Url.AbsolutePath;
            foreach (var item in list.OrderByDescending(m=>m.Id))
            {
                string backColorClass = "bckColorGreen";
                string backColorClass2 = "bckColorGreen";
                if (item.IsDeleted == true)
                    backColorClass = "bckColorRed";

                custListHtml += "<tr>"
                             + "     <td>" + item.categoriTitle + "</td>"
                             + "     <td>" + item.mpCategoryName+ "</td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass + "\"><a alt=\"" + item.IsDeleted + "\" onclick=\"Delete(" + item.Id + ");\">Sil</a></span></td>"
                             + "     <td><a href='"+String.Format(path+"?id="+item.Id) +"'/>Düzenle</td>"
                            + "   </tr>";
            }
            custListHtml += "</tbody></table></div>";
            return custListHtml;
        }


    }
}