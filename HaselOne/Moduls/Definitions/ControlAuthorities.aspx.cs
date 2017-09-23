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
    public partial class ControlAuthorities : System.Web.UI.Page
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
            Session["PageId"] = 7;

           

            if (!IsPostBack)
            {
                if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                    LoadOldPage();
                else LoadLast20();
            }
        }

        private void LoadOldPage()
        {
            List<Gn_ControlAuthorities> users = new List<Gn_ControlAuthorities>();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            LoadPaginator(users);
        }

        private void LoadLast20()
        {
            List<Gn_ControlAuthorities> users = _us.GetUsersLast20CA().ToList();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            PIndex = 1;
            LoadPaginator(users);
        }

        private string CreateGridHtml(string criteria, ref List<Gn_ControlAuthorities> users)
        {
            if (PIndex == 0)
                PIndex = 1;

            if (criteria != "")
            {
                users = _us.GetUsersWhereCA(criteria, (PIndex - 1) * _pageSize, _pageSize).ToList();
                if (users.Count <= 0)
                    return "";
            }
            else if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                users = _us.GetUsersWhereCA(Session["criteria"].ToString(), (PIndex - 1) * _pageSize, _pageSize).ToList();
            else users = _us.GetUsersWhereCA("", (PIndex - 1) * _pageSize, _pageSize).ToList();

            Session["usersSearchResult"] = users;
            string custListHtml = " <div class=\"portlet-body flip-scroll\">"
                              + "          <table class=\"table table-bordered table-striped table-condensed flip-content\">"
                              + "              <thead class=\"flip -content\">"
                              + "                  <tr>"
                              + "                      <th>Kullanıcı Adı</th>"
                              + "                      <th>Kontrol Adı Adı</th>"
                              + "                      <th>Aktiflik</th>"
                              + "                      <th>Görünürlük</th>"
                              + "                      <th>Detay</th>"
                              + "                  </tr>"
                              + "              </thead>"
                              + "              <tbody>";

            foreach (Gn_ControlAuthorities usr in users)
            {
                string backColorClass2 = "bckColorGreen";
                if (usr.IsEnable == false)
                    backColorClass2 = "bckColorRed";
                string backColorClass = "bckColorGreen";
                if (usr.IsVisible == false)
                    backColorClass = "bckColorRed";

                Gn_User u = _us.GetUserById(Convert.ToInt32(usr.UserId));
                Gn_Control control = _us.GetControlById(Convert.ToInt32(usr.ControlId));
                custListHtml += "<tr>"
                             + "     <td>" + u.Name + "</td>"
                             + "     <td>" + control.ControlId + "</td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass2 + "\"><a alt=\"" + usr.IsEnable + "\" onclick=\"ChangeActivePassiveCAEnable(" + usr.Id + ");\">Aktiflik</a></span></td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass + "\"><a alt=\"" + usr.IsVisible + "\" onclick=\"ChangeActivePassiveCAVisible(" + usr.Id + ");\">Görünürlük</a></span></td>"
                             + "     <td><span class=\"label label-sm label - info \"><a href=\"ControlAuthoritiesDetail.aspx?UId=" + usr.Id + "\">Detay</a></span></td>"
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
            List<Gn_ControlAuthorities> users = new List<Gn_ControlAuthorities>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
            LoadPaginator(users);
        }

        private void LoadPaginator(List<Gn_ControlAuthorities> users)
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
            List<Gn_ControlAuthorities> users = new List<Gn_ControlAuthorities>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
        }
    }
}