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
    public partial class Customers : System.Web.UI.Page
    {

        static int _pageSize = 20;

        [Dependency]
        public ICustomerService _cs { get; set; }

        int _pIndex = 0;
        public int PIndex
        {
            get
            {
                if (Session["PIndex"] != null)
                    return Convert.ToInt32(Session["PIndex"]);
                return _pIndex;
            }
            set
            {
                _pIndex = value;
                Session["PIndex"] = _pIndex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                //if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                //    LoadOldPage();
                //else
                LoadLast20();
            }
        }

        //private void LoadOldPage()
        //{
        //    List<Cm_Customer> cariler = new List<Cm_Customer>();
        //    containerCustomer.InnerHtml = CreateGridHtml("", ref cariler);
        //    LoadPaginator(cariler);
        //}

        private void LoadLast20()
        {
            List<Cm_Customer> cariler = _cs.GetCustomersLast20().ToList();
            containerCustomer.InnerHtml = CreateGridHtml("", ref cariler);
            PIndex = 1;
            LoadPaginator(cariler);
        }

        private string CreateGridHtml(string criteria, ref List<Cm_Customer> cariler)
        {
            if (PIndex == 0)
                PIndex = 1;

            if (criteria != "")
            {
                cariler = _cs.GetCustomersWhere(criteria, (PIndex - 1) * _pageSize, _pageSize).ToList();
                if (cariler.Count <= 0)
                    return "";
            }
            else if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                cariler = _cs.GetCustomersWhere(Session["criteria"].ToString(), (PIndex - 1) * _pageSize, _pageSize).ToList();
            else cariler = _cs.GetCustomersWhere("", (PIndex - 1) * _pageSize, _pageSize).ToList();

            Session["CarilerSearchResult"] = cariler;
            string custListHtml = " <div class=\"portlet-body flip-scroll\">"
                              + "          <table class=\"table table-bordered table-striped table-condensed flip-content\">"
                              + "              <thead class=\"flip -content\">"
                              + "                  <tr>"
                              + "                      <th>Kodu</th>"
                              + "                      <th>Adı</th>"
                              + "                      <th>Hsl Kodu</th>"
                              + "                      <th>Rlt Kodu</th>"
                             // + "                      <th>Lokasyon</th>"
                              + "                      <th>Sektör</th>"
                              + "                      <th>Web</th>"
                              + "                      <th>Durum</th>"
                              + "                  </tr>"
                              + "              </thead>"
                              + "              <tbody>";

            foreach (Cm_Customer cari in cariler)
            {
                custListHtml += "<tr>"
                             + "     <td>" + cari.ShortName + "</td>"
                             + "     <td>" + cari.Name + "</td>"
                             + "     <td>" + cari.NetsisHaselCode + " </td>"
                             + "     <td>" + cari.NetsisRentliftCode + "</td>"
                            // + "     <td>" + cari.+ "</td>"
                             + "     <td>" + cari.SectorId + "</td>"
                             + "     <td>" + cari.Web + "</td>"
                             + "     <td><span class=\"label label-sm label - info\"><a href=\"CustomerDetail.aspx?CId=" + cari.Id + "\">Detay</a></span></td>"
                             + "   </tr>";
            }
            custListHtml += "</tbody></table></div>";
            return custListHtml;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PIndex = 1;
            string criteria = searchInput.Value;
            Session["criteria"] = criteria;
            List<Cm_Customer> cariler = new List<Cm_Customer>();
            containerCustomer.InnerHtml = CreateGridHtml(criteria, ref cariler);
            LoadPaginator(cariler);
        }

        private void LoadPaginator(List<Cm_Customer> cariler)
        {
            string paginatorHtml = "<ul class=\"pagination\">";
            if (PIndex == 0)
            {
                PIndex = 1;
                Session["PIndex"] = PIndex;
            }
            paginatorHtml += "<li class=\"page-active\">"
                           + "  <a href =\"javascript:BeforePage(" + PIndex + ");\"><</a>"
                           + "</li>"
                           + " <li class=\"page-active\">"
                           + "  <a href =\"javascript:NextPage(" + PIndex + ");\">></a>"
                           + "</li>";
            paginatorHtml += "</ul>";

            paginator.InnerHtml = paginatorHtml;
        }

        protected void btnChangePageIndex_Click(object sender, EventArgs e)
        {
            int clickedIndex = Convert.ToInt32(hdnPageIndex.Value);

            if (clickedIndex <= 0)
            {
                PIndex = PIndex - 1;
            }
            else
            {
                PIndex = PIndex + 1;
            }

            string criteria = searchInput.Value;
            List<Cm_Customer> cariler = new List<Cm_Customer>();
            containerCustomer.InnerHtml = CreateGridHtml(criteria, ref cariler);
        }


    }
}