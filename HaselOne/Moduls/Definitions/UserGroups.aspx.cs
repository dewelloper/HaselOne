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
    public partial class UserGroups : System.Web.UI.Page
    {
        static int _pageSize = 20;

        [Dependency]
        public IUserService _us { get; set; }

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
            Session["ModulId"] = 8;
            Session["PageId"] = 2;

            

            if (!IsPostBack)
            {
                if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                    LoadOldPage();
                else LoadLast20();
            }
        }

        private void LoadOldPage()
        {
            List<Gn_Category> users = new List<Gn_Category>();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            LoadPaginator(users);
        }

        private void LoadLast20()
        {
            List<Gn_Category> users = _us.GetUsersLast20UG().ToList();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            PIndex = 1;
            LoadPaginator(users);
        }

        private string CreateGridHtml(string criteria, ref List<Gn_Category> users)
        {
            if (PIndex == 0)
                PIndex = 1;

            if (criteria != "")
            {
                users = _us.GetUsersWhereUG(criteria, (PIndex - 1) * _pageSize, _pageSize).ToList();
                if (users.Count <= 0)
                    return "";
            }
            else if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                users = _us.GetUsersWhereUG(Session["criteria"].ToString(), (PIndex - 1) * _pageSize, _pageSize).ToList();
            else users = _us.GetUsersWhereUG("", (PIndex - 1) * _pageSize, _pageSize).ToList();

            Session["usersSearchResult"] = users;
            string custListHtml = " <div class=\"portlet-body flip-scroll\">"
                              + "          <table class=\"table table-bordered table-striped table-condensed flip-content\">"
                              + "              <thead class=\"flip -content\">"
                              + "                  <tr>"
                              + "                      <th>Grup Adı</th>"
                              + "                      <th>Aktiflik</th>"
                              + "                      <th>Detay</th>"
                              + "                  </tr>"
                              + "              </thead>"
                              + "              <tbody>";

            foreach (Gn_Category usr in users)
            {
                string backColorClass2 = "bckColorGreen";
                if (usr.IsActive == false)
                    backColorClass2 = "bckColorRed";

                custListHtml += "<tr>"
                             + "     <td>" + usr.Title + "</td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass2 + "\"><a alt=\"" + usr.IsActive + "\" onclick=\"ChangeActivePassiveUserUG(" + usr.Id + ");\">Aktiflik</a></span></td>"
                             + "     <td><span class=\"label label-sm label - info \"><a href=\"UserGroupDetail.aspx?UId=" + usr.Id + "\">Detay</a></span></td>"
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
            List<Gn_Category> users = new List<Gn_Category>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
            LoadPaginator(users);
        }

        private void LoadPaginator(List<Gn_Category> users)
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
            List<Gn_Category> users = new List<Gn_Category>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
        }
    }
}