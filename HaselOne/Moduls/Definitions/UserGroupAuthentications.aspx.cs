﻿using DAL;
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
    public partial class UserGroupAuthentications : System.Web.UI.Page
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
            Session["PageId"] = 5;

           

            if (!IsPostBack)
            {
                if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                    LoadOldPage();
                else LoadLast20();
            }
        }

        private void LoadOldPage()
        {
            List<Gn_UserGroupRights> users = new List<Gn_UserGroupRights>();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            LoadPaginator(users);
        }

        private void LoadLast20()
        {
            List<Gn_UserGroupRights> users = _us.GetUsersLast20GroupRights().ToList();
            containerUser.InnerHtml = CreateGridHtml("", ref users);
            PIndex = 1;
            LoadPaginator(users);
        }

        private string CreateGridHtml(string criteria, ref List<Gn_UserGroupRights> users)
        {
            if (PIndex == 0)
                PIndex = 1;

            if (criteria != "")
            {
                users = _us.GetUsersWhereGroupRights(criteria, (PIndex - 1) * _pageSize, _pageSize).ToList();
                if (users.Count <= 0)
                    return "";
            }
            else if (Session["criteria"] != null && Session["criteria"].ToString() != "")
                users = _us.GetUsersWhereGroupRights(Session["criteria"].ToString(), (PIndex - 1) * _pageSize, _pageSize).ToList();
            else users = _us.GetUsersWhereGroupRights("", (PIndex - 1) * _pageSize, _pageSize).ToList();

            Session["usersSearchResult"] = users;
            users = users.OrderBy(k => k.UserGroupId).ToList();
            string custListHtml = " <div class=\"portlet-body flip-scroll\">"
                              + "          <table class=\"table table-bordered table-striped table-condensed flip-content\">"
                              + "              <thead class=\"flip -content\">"
                              + "                  <tr>"
                              + "                      <th>Grup</th>"
                              + "                      <th>Departman</th>"
                              + "                      <th>Rol</th>"
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

            foreach (Gn_UserGroupRights usr in users)
            {
                string backColorClass2 = "bckColorGreen";
                if (usr.IsActive == false)
                    backColorClass2 = "bckColorRed";
                string backColorClass = "bckColorGreen";
                if (usr.IsDeleted == true)
                    backColorClass = "bckColorRed";

                Gn_Category ug = _us.GetUserGroupById(usr.UserGroupId);
                Gn_Department ud = _us.GetUserDepartmentById(usr.UserGroupId);
                Gn_Role ur = _us.GetUserRuleById(usr.UserGroupId);
                Gn_UserModuls um = _us.GetUserModulById(usr.ModulId);
                custListHtml += "<tr>"
                             + "     <td>" + (ug==null ? "Genel" : ug.Title) + "</td>"
                             + "     <td>" + (ud==null ? "Genel" : ud.DepartmentName) + "</td>"
                             + "     <td>" + (ur==null ? "Genel": ur.RuleName) + "</td>"
                             + "     <td>" + um.ModulName + "</td>"
                             + "     <td>" + usr.RecordInsert + "</td>"
                             + "     <td>" + usr.RecordEdit + "</td>"
                             + "     <td>" + usr.RecordShow + "</td>"
                             + "     <td>" + usr.RecordDelete + "</td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass + "\"><a alt=\"" + usr.IsDeleted + "\" onclick=\"ChangeActivePassiveUserUGADelete(" + usr.Id + ");\">Silinmişlik</a></span></td>"
                             + "     <td><span class=\"label label-sm label - info " + backColorClass2 + "\"><a alt=\"" + usr.IsActive + "\" onclick=\"ChangeActivePassiveUserUGAActivity(" + usr.Id + ");\">Aktiflik</a></span></td>"
                             + "     <td><span class=\"label label-sm label - info \"><a href=\"UserGroupAuthenticationDetail.aspx?UId=" + usr.UserGroupId + "&DId="+ usr.Id +"\">Detay</a></span></td>"
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
            List<Gn_UserGroupRights> users = new List<Gn_UserGroupRights>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
            LoadPaginator(users);
        }

        private void LoadPaginator(List<Gn_UserGroupRights> users)
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
            List<Gn_UserGroupRights> users = new List<Gn_UserGroupRights>();
            containerUser.InnerHtml = CreateGridHtml(criteria, ref users);
        }
    }
}