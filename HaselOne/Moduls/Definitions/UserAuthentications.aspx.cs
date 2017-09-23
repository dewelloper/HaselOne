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
    public partial class UserAuthentications : System.Web.UI.Page
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
            Session["PageId"] = 6;

           

            if (!IsPostBack)
            {
                if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                    LoadOldPage();
                else LoadLast20();
            }
        }

        private void LoadOldPage()
        {
            List<Gn_UserRights> users = new List<Gn_UserRights>();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            LoadPaginator(users);
        }

        private void LoadLast20()
        {
            List<Gn_UserRights> users = _us.GetUsersLast20UserRights().ToList();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            PIndex = 1;
            LoadPaginator(users);
        }

        private string CreateGridHtml(string criteria, ref List<Gn_UserRights> users)
        {
            if (PIndex == 0)
                PIndex = 1;

            if (criteria != "")
            {
                users = _us.GetUsersWhereUserRights(criteria, (PIndex - 1) * _pageSize, _pageSize).ToList();
                if (users.Count <= 0)
                    return "";
            }
            else if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                users = _us.GetUsersWhereUserRights(Session["criteria"].ToString(), (PIndex - 1) * _pageSize, _pageSize).ToList();
            else users = _us.GetUsersWhereUserRights("", (PIndex - 1) * _pageSize, _pageSize).ToList();

            Session["usersSearchResult"] = users;
            users = users.OrderBy(k => k.ModulId).ToList();
            string custListHtml = " <div class=\"portlet-body flip-scroll\">"
                              + "          <table class=\"table table-bordered table-striped table-condensed flip-content\">"
                              + "              <thead class=\"flip -content\">"
                              + "                  <tr>"
                              + "                      <th>Kullanıcı Adı</th>"
                              + "                      <th>Modül Adı</th>"
                              + "                      <th>Ekleme</th>"
                              + "                      <th>Güncelleme</th>"
                              + "                      <th>Görünürlük</th>"
                              + "                      <th>Silme</th>"
                              + "                      <th>Silinmişlik</th>"
                              + "                      <th>Aktiflik</th>"
                              + "                      <th>Detay</th>"
                              + "                  </tr>"
                              + "              </thead>"
                              + "              <tbody>";

            foreach (Gn_UserRights usr in users)
            {
                string backColorClass2 = "bckColorGreen";
                if (usr.IsActive == false)
                    backColorClass2 = "bckColorRed";
                string backColorClass = "bckColorGreen";
                if (usr.IsDeleted == true)
                    backColorClass = "bckColorRed";

                Gn_User u = _us.GetUserById(usr.UserId);
                Gn_UserModuls um = _us.GetUserModulById(usr.ModulId);
                custListHtml += "<tr>"
                             + "     <td>" + u.UserName + "</td>"
                             + "     <td>" + um.ModulName + "</td>"
                             + "     <td>" + usr.RecordInsert + "</td>"
                             + "     <td>" + usr.RecordEdit + "</td>"
                             + "     <td>" + usr.RecordShow + "</td>"
                             + "     <td>" + usr.RecordDelete + "</td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass + "\"><a alt=\"" + usr.IsDeleted + "\" onclick=\"ChangeActivePassiveUserUADelete(" + usr.Id + ");\">Silinmişlik</a></span></td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass2 + "\"><a alt=\"" + usr.IsActive + "\" onclick=\"ChangeActivePassiveUserUAActivity(" + usr.Id + ");\">Aktiflik</a></span></td>"
                             + "     <td><span class=\"label label-sm label - info \"><a href=\"UserAuthenticationDetail.aspx?UId=" + usr.Id + "\">Detay</a></span></td>"
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
            List<Gn_UserRights> users = new List<Gn_UserRights>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
            LoadPaginator(users);
        }

        private void LoadPaginator(List<Gn_UserRights> users)
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
            List<Gn_UserRights> users = new List<Gn_UserRights>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
        }
    }
}