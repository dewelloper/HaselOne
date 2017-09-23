using BusinessObjects;
using DAL;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using DAL.Helper;
using Newtonsoft.Json;
using HaselOne.Util;

namespace HaselOne
{
    public partial class CustomerDetail : System.Web.UI.Page
    {
        [Dependency]
        public ICustomerService _cs { get; set; }

        [Dependency]
        public IUserService _us { get; set; }

        private DictonaryStaticList dicStaticList = new DictonaryStaticList();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(_cs.GetCategories().Select(m => new MachineparkCategoryWrapper() { Id = m.Id, CategoryName = m.CategoryName, ParentId = m.ParentId }).ToList()), "MachineParkCategoryList", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(_cs.GetListMachineparkMark().Where(m => m.IsActive.Value && !m.IsDeleted.Value).Select(m => new { m.Id, m.MarkName, m.IsOwnerMachine }).ToList()), "MachineparkMarkList", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(_cs.GetListMachineModel().Where(m => m.IsActive).Select(m => new { m.Id, m.CategoryId, m.Name }).ToList()), "MpModelList", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new MachineparkWrapper()), "CustomerMachineparkModel", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new CustomerRequestWrapper()), "CustomerRequestModel", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new MachineparkMarkWrapper()), "MachineparkMarkModel", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new MachineModelWrapper()), "MachineModelModel", "startup_scripts");

                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new CustomerInterviewsWrapper()), "CustomerInterviews", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(_us.ListConnectionChannel().Select(m => new { Text = m.Name, Value = m.Id, Group = m.GroupName })), "ChannelList", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(_cs.GetYears().ToList()), "MpYearList", "startup_scripts");

                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(dicStaticList.dicSalesType.Select(m => new TextValue() { Value = m.Key, Text = m.Value })), "SalesTypeList", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(dicStaticList.dicResultType.Select(m => new TextValue() { Value = m.Key, Text = m.Value })), "ResultTypeList", "startup_scripts");

                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(dicStaticList.dicConditionType.Select(m => new TextValue() { Value = m.Key, Text = m.Value })), "ConditionTypeList", "startup_scripts");

                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(dicStaticList.dicUseDurationUnitList.Select(m => new TextValue() { Value = m.Key, Text = m.Value })), "UseDurationUnitList", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(_us.GetUsers().Where(m => m.IsDeleted != true).Select(m => new TextValue { Value = m.Id, Text = m.Name + " " + m.Surname }).ToList()), "OwnerList", "startup_scripts");

                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new MachineparkFilter()), "MachineparkFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new MachineparkCategoryFilter()), "MachineparkCategoryFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new MachineparkMarkFilter()), "MachineparkMarkFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new MachineModelFilter()), "MachineModelFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new CustomerInterviewsFilter()), "CustomerInterviewsFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new CustomerRequestFilter()), "CustomerRequestFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new LocationFilter()), "LocationFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new SalesmanFilter()), "SalesmanFilter", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new Result()), "Result", "startup_scripts");
                PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(new DictonaryStaticList()), "DictonaryStaticList", "startup_scripts");
               // PageHelper.RegisterJs(this.Master, PageHelper.JsonConvertAsJsObj(new RequestOpenCloseState), "RequestOpenclose", "startup_scripts");
               // PageHelper.JsonConvertAsJsObj(new RequestOpenCloseState)
            }
            Session["ModulId"] = 1;
            Session["PageId"] = 2;

            int customerId = 0;
            if (Request.QueryString["CId"] != null)
            {
                if (!int.TryParse(Request.QueryString["CId"], out customerId))
                {
                    Response.Redirect(String.Format("../../500.aspx?aspxerrorpath={0}", "Hatali cari id"));
                    return;
                }
                var ad = Request.Url.AbsoluteUri.ToString();
                lblSaveStatus.Text = $@"Kayıtlı Cari | Kaydın linki: {ad}";
            }
            else
            {
                lblSaveStatus.Text = "Yeni kayıt(Lütfen cariyi kaydedip onaya gönderiniz)";
            }

            if (!IsPostBack)
            {
                LoadSektors();
                Util.Utility.LoadCategories(ddMpMachineparkType, _cs.GetCategories().ToList());
                foreach (ListItem ddlListItem in ddMpMachineparkType.Items)
                {
                    if (ddlListItem.Text.ToArray().ToList().Where(k => k == '.').Count() > 0)
                    {
                        ddlListItem.Text = "." + HttpUtility.HtmlDecode(ddlListItem.Text.Replace(".", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                        var operationName = _cs.GetCatNameById(Convert.ToInt32(ddlListItem.Value));
                        if (operationName != "")
                            ddlListItem.Attributes.Add("cid", operationName);
                    }
                    else
                    {
                        ddlListItem.Text = "*" + HttpUtility.HtmlDecode(ddlListItem.Text.Replace(".", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                        var operationName = _cs.GetCatNameById(Convert.ToInt32(ddlListItem.Value));
                        if (operationName != "")
                            ddlListItem.Attributes.Add("cid", operationName);
                    }
                }

                var item = new ListItem() { Value = "0", Text = "Seçiniz..." };
                // item.Attributes.Add("cid","");
                ddMpMachineparkType.Items.Insert(0, item);
                DisableUnselectableCategoryItems();
                Util.Utility.LoadCombo<Cm_MachineparkMark>(ddMpMarks, _cs.GetMarks().OrderBy(k => k.MarkName).ToList(), "MarkName", "Id");
                ddMpMarks.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Cm_CustomerLocations>(ddAutLocation, _cs.GetLocationByCustomerId(Convert.ToInt32(Request.QueryString["CId"])).OrderBy(k => k.Address).ToList(), "Name", "Id");
                ddAutLocation.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Cm_CustomerLocations>(ddMpMachineparkLocation, _cs.GetLocationByCustomerId(Convert.ToInt32(Request.QueryString["CId"])).OrderBy(k => k.Address).ToList(), "Name", "Id");
                ddMpMachineparkLocation.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });
                int uid = Convert.ToInt32(Session["UserId"]);

                Util.Utility.LoadCombo<Cm_MachineparkYear>(ddMpYears, _cs.GetYears().OrderBy(k => k.Year).ToList(), "Year", "Id");
                ddMpYears.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Cm_MachineparkOwnership>(ddMpRetireOrOwnered, _cs.GetOwnerships().OrderBy(k => k.Name).ToList(), "Name", "Id");
                ddMpRetireOrOwnered.Items.Insert(0, new ListItem() { Value = "0", Text = "Seçiniz..." });
                Util.Utility.LoadCombo<Gn_AreaCity>(ddLocCity, _cs.GetAreasForCitiesAll().OrderBy(k => k.CityName).ToList(), "CityName", "Id");
                ddLocCity.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });

                //Util.Utility.LoadCombo<Gn_Category>(pddSalesmanTypesPopup, getUserSalesmanTypeList, "Title", "Id");
                //pddSalesmanTypesPopup.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });

                Util.Utility.LoadCombo<Gn_Category>(ddSalesmanTypes, _cs.GetUserSalesmanTypesRoles(uid).OrderBy(k => k.Title).ToList(), "Title", "Id");
                ddSalesmanTypes.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });

                Util.Utility.LoadCombo<Gn_Category>(pddSalesmanTypesPopup, _cs.GetUserSalesmanTypesRoles(uid).OrderBy(k => k.Title).ToList(), "Title", "Id");
                pddSalesmanTypesPopup.Items.Insert(0, new ListItem() { Value = "", Text = "Seçiniz..." });

               
                /*CRequest Get*/

                LoadLocations();
                LoadAuthenticators();
                LoadSalesmans();
                LoadMachineparks();

              

                SetInnerModulAuthentications();
            }

            if (Request.QueryString["CId"] != null)
                btnSave.InnerText = "Güncelle";
            else btnSave.InnerText = "Kaydet";
        }

        private void IsCustomerManager(int usrId)
        {
            //_us.Where(k => k.UserId == user.Id).ToList();
            var list = _us.GetUserRuleById(usrId);
        }

        private void SetInnerModulAuthentications()
        {
            tlocation.Visible = false;
            tauth.Visible = false;
            tengineers.Visible = false;
            //tmpark.Visible = false;
            int uid = Convert.ToInt32(Session["UserId"]);
            Dictionary<int, UserKnowledge> dUk = Session["UK"] as Dictionary<int, UserKnowledge>;
            UserKnowledge uk = dUk[uid] as UserKnowledge;
            List<Gn_UserGroupRights> userGroupRights = uk.GroupRights as List<Gn_UserGroupRights>;
            List<Gn_UserRights> userRights = uk.UserRights as List<Gn_UserRights>;
            List<int?> userRoleList = uk.UserRoles;

            //int? ugid = uk.User.UserGroupId;

            tlocation.Visible = false;
            tauth.Visible = false;
            tengineers.Visible = false;
            //tmpark.Visible = false;
            tInterviews.Visible = false;
            tRequest.Visible = false;
            if (uk.User.IsAdmin == true)
            {
                tlocation.Visible = true;
                tauth.Visible = true;
                tengineers.Visible = true;
                // tmpark.Visible = true;
            }

            Gn_UserGroupRights hasGeneralShow = userGroupRights.Where(k => k.ModulId == 1 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            Gn_UserRights urhasGeneralShow = userRights.Where(k => k.ModulId == 1 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            if ((urhasGeneralShow != null && urhasGeneralShow.RecordInsert == true || hasGeneralShow != null && hasGeneralShow.RecordInsert == true))
                hdnGeneralInsert.Value = "1";
            else hdnGeneralInsert.Value = "0";
            if ((urhasGeneralShow != null && urhasGeneralShow.RecordEdit == true || hasGeneralShow != null && hasGeneralShow.RecordEdit == true))
                hdnGeneralEdit.Value = "1";
            else hdnGeneralEdit.Value = "0";
            if ((urhasGeneralShow != null && urhasGeneralShow.RecordDelete == true || hasGeneralShow != null && hasGeneralShow.RecordDelete == true))
                hdnGeneralDelete.Value = "1";
            else hdnGeneralDelete.Value = "0";

            Gn_UserGroupRights hasLocationShow = userGroupRights.Where(k => k.ModulId == 6 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            Gn_UserRights urhasLocationShow = userRights.Where(k => k.ModulId == 6 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            if (urhasLocationShow != null || hasLocationShow != null)
                tlocation.Visible = true;
            if ((urhasLocationShow != null && urhasLocationShow.RecordInsert == true || hasLocationShow != null && hasLocationShow.RecordInsert == true))
                hdnLocationInsert.Value = "1";
            else hdnLocationInsert.Value = "0";
            if ((urhasLocationShow != null && urhasLocationShow.RecordEdit == true || hasLocationShow != null && hasLocationShow.RecordEdit == true))
                hdnLocationEdit.Value = "1";
            else hdnLocationEdit.Value = "0";
            if ((urhasLocationShow != null && urhasLocationShow.RecordDelete == true || hasLocationShow != null && hasLocationShow.RecordDelete == true))
                hdnLocationDelete.Value = "1";
            else hdnLocationDelete.Value = "0";

            Gn_UserGroupRights hasAuthenticatorShow = userGroupRights.Where(k => k.ModulId == 7 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            Gn_UserRights urhasAuthenticatorShow = userRights.Where(k => k.ModulId == 7 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            if (urhasAuthenticatorShow != null || hasAuthenticatorShow != null)
                tauth.Visible = true;
            if ((urhasAuthenticatorShow != null && urhasAuthenticatorShow.RecordInsert == true || hasAuthenticatorShow != null && hasAuthenticatorShow.RecordInsert == true))
                hdnAuthenticatorInsert.Value = "1";
            else hdnAuthenticatorInsert.Value = "0";
            if ((urhasAuthenticatorShow != null && urhasAuthenticatorShow.RecordEdit == true || hasAuthenticatorShow != null && hasAuthenticatorShow.RecordEdit == true))
                hdnAuthenticatorEdit.Value = "1";
            else hdnAuthenticatorEdit.Value = "0";
            if ((urhasAuthenticatorShow != null && urhasAuthenticatorShow.RecordDelete == true || hasAuthenticatorShow != null && hasAuthenticatorShow.RecordDelete == true))
                hdnAuthenticatorDelete.Value = "1";
            else hdnAuthenticatorDelete.Value = "0";

            Gn_UserGroupRights hasSaleEngineersShow = userGroupRights.Where(k => k.ModulId == 8 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            Gn_UserRights urhasSaleEngineersShow = userRights.Where(k => k.ModulId == 8 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            if (urhasSaleEngineersShow != null || hasSaleEngineersShow != null)
                tengineers.Visible = true;
            if ((urhasSaleEngineersShow != null && urhasSaleEngineersShow.RecordInsert == true || hasSaleEngineersShow != null && hasSaleEngineersShow.RecordInsert == true))
                hdnEngineerInsert.Value = "1";
            else hdnEngineerInsert.Value = "0";
            if ((urhasSaleEngineersShow != null && urhasSaleEngineersShow.RecordEdit == true || hasSaleEngineersShow != null && hasSaleEngineersShow.RecordEdit == true))
                hdnEngineerEdit.Value = "1";
            else hdnEngineerEdit.Value = "0";
            if ((urhasSaleEngineersShow != null && urhasSaleEngineersShow.RecordDelete == true || hasSaleEngineersShow != null && hasSaleEngineersShow.RecordDelete == true))
                hdnEngineerDelete.Value = "1";
            else hdnEngineerDelete.Value = "0";

            Gn_UserGroupRights hasMachineparkShow = userGroupRights.Where(k => k.ModulId == 9 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            Gn_UserRights urhasMachineparkShow = userRights.Where(k => k.ModulId == 9 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            // if (urhasMachineparkShow != null || hasMachineparkShow != null)
            //tmpark.Visible = true;
            if ((urhasMachineparkShow != null && urhasMachineparkShow.RecordInsert == true || hasMachineparkShow != null && hasMachineparkShow.RecordInsert == true))
                hdnMachineparkInsert.Value = "1";
            else hdnMachineparkInsert.Value = "0";
            if ((urhasMachineparkShow != null && urhasMachineparkShow.RecordEdit == true || hasMachineparkShow != null && hasMachineparkShow.RecordEdit == true))
                hdnMachineparkEdit.Value = "1";
            else hdnMachineparkEdit.Value = "0";
            if ((urhasMachineparkShow != null && urhasMachineparkShow.RecordDelete == true || hasMachineparkShow != null && hasMachineparkShow.RecordDelete == true))
                hdnMachineparkDelete.Value = "1";
            else hdnMachineparkDelete.Value = "0";

            Gn_UserGroupRights hasInterviewShow = userGroupRights.Where(k => k.ModulId == 10 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            Gn_UserRights urhasInterviewShow = userRights.Where(k => k.ModulId == 10 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            if (urhasInterviewShow != null || hasInterviewShow != null)
                tInterviews.Visible = true;
            if ((urhasInterviewShow != null && urhasInterviewShow.RecordInsert == true || hasInterviewShow != null && hasInterviewShow.RecordInsert == true))
                hdnInterviewInsert.Value = "1";
            else hdnInterviewInsert.Value = "0";
            if ((urhasInterviewShow != null && urhasInterviewShow.RecordEdit == true || hasInterviewShow != null && hasInterviewShow.RecordEdit == true))
                hdnInterviewEdit.Value = "1";
            else hdnInterviewEdit.Value = "0";
            if ((urhasInterviewShow != null && urhasInterviewShow.RecordDelete == true || hasInterviewShow != null && hasInterviewShow.RecordDelete == true))
                hdnInterviewDelete.Value = "1";
            else hdnInterviewDelete.Value = "0";
            if (userRoleList != null && userRoleList.Count(m => m.Value == 5 || m.Value == 7) > 0)
            {//1-- Cari yoneticisi 2-- Operasyon yoneticisi
                hdnFlagEditAuth.Value = "1";
            }
            else
            {
                hdnFlagEditAuth.Value = 0.ToString();
            }

            Gn_UserGroupRights hasRequestShow = userGroupRights.Where(k => k.ModulId == 9 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            Gn_UserRights urhasRequestShow = userRights.Where(k => k.ModulId == 9 && k.IsActive == true && k.IsDeleted != true && k.RecordShow == true).FirstOrDefault();
            if (urhasRequestShow != null || hasRequestShow != null)
                tRequest.Visible = true;

            if ((urhasRequestShow != null && urhasRequestShow.RecordInsert == true || hasRequestShow != null && hasRequestShow.RecordInsert == true))
                hdnRequestInsert.Value = "1";
            else hdnRequestInsert.Value = "0";

            if ((urhasRequestShow != null && urhasRequestShow.RecordEdit == true || hasRequestShow != null && hasRequestShow.RecordEdit == true))
                hdnRequestEdit.Value = "1";
            else hdnRequestEdit.Value = "0";
            if ((urhasRequestShow != null && urhasRequestShow.RecordDelete == true || hasRequestShow != null && hasRequestShow.RecordDelete == true))
                hdnRequestDelete.Value = "1";
            else hdnRequestDelete.Value = "0";

            Int32 cid = Convert.ToInt32(Request.QueryString["CId"]);
            bool? isAdmin = uk.User.IsAdmin;

            if (userRoleList != null && userRoleList.Count(m => m.Value == 5 || m.Value == 8) > 0)
            {
                btnDeleteGeneral.Visible = true;
            }
            else
            {
                btnDeleteGeneral.Visible = false;
            }

            bool isEditableForYou = false;
            if (isAdmin == true)
            {
                btnDeleteGeneral.Visible = true;
                hdnAuthenticatorDelete.Value = "1";
                hdnAuthenticatorEdit.Value = "1";
                hdnAuthenticatorInsert.Value = "1";
                hdnEngineerDelete.Value = "1";
                hdnEngineerEdit.Value = "1";
                hdnEngineerInsert.Value = "1";
                hdnGeneralDelete.Value = "1";
                hdnGeneralEdit.Value = "1";
                hdnGeneralInsert.Value = "1";
                hdnLocationDelete.Value = "1";
                hdnLocationEdit.Value = "1";
                hdnLocationInsert.Value = "1";
                hdnMachineparkDelete.Value = "1";
                hdnMachineparkEdit.Value = "1";
                hdnMachineparkInsert.Value = "1";

                hdnInterviewInsert.Value = "1";
                hdnInterviewDelete.Value = "1";
                hdnInterviewEdit.Value = "1";

                hdnRequestDelete.Value = "1";
                hdnRequestInsert.Value = "1";
                hdnRequestEdit.Value = "1";

                hdnFlagEditAuth.Value = "1";
            }
            else if (cid != 0 && !(isEditableForYou = _cs.IsEditableAndInsertableCustomerForYou(cid, uk.User.Id, uk.RoleRelatedUserIds)))
            {
                hdnAuthenticatorDelete.Value = "0";
                hdnAuthenticatorEdit.Value = "0";
                hdnAuthenticatorInsert.Value = "0";
                hdnEngineerDelete.Value = "0";
                hdnEngineerEdit.Value = "0";
                hdnEngineerInsert.Value = "0";
                hdnGeneralDelete.Value = "0";
                hdnGeneralEdit.Value = "0";
                hdnGeneralInsert.Value = "0";
                hdnLocationDelete.Value = "0";
                hdnLocationEdit.Value = "0";
                hdnLocationInsert.Value = "0";
                hdnMachineparkDelete.Value = "0";
                hdnMachineparkEdit.Value = "0";
                hdnMachineparkInsert.Value = "0";

                hdnInterviewInsert.Value = "0";
                hdnInterviewDelete.Value = "0";
                hdnInterviewEdit.Value = "0";

                hdnRequestDelete.Value = "0";
                hdnRequestInsert.Value = "0";
                hdnRequestEdit.Value = "0";

                //  hdnFlagEditAuth.Value = "0";
            }

            if (isAdmin == true || isEditableForYou == true)
                LoadIfExistCustomer(true);
            else LoadIfExistCustomer(false);
        }

        private void DisableUnselectableCategoryItems()
        {
            foreach (ListItem item in ddMpMachineparkType.Items)
            {
                if (!item.Text.Contains("."))
                {
                    item.Attributes.Add("disabled", "disabled");
                }
            }
        }

        #region General

        private void LoadIfExistCustomer(bool isAuthenticated)
        {
            rblHaselDurum.SelectedIndex = 0;
            rblMainCustomer.SelectedIndex = 1;
            if (Request.QueryString["CId"] != null)
            {
                hdnCustomerId.Value = Request.QueryString["CId"].ToString();
                int customerId = Convert.ToInt32(Request.QueryString["CId"]);
                Session["CId"] = customerId;
                Cm_Customer cari = _cs.GetCustomerById(customerId);
                if (cari == null)
                {
                    Response.Redirect(String.Format("../../500.aspx?aspxerrorpath={0}", "Boyle bir cari yok Cari Id " + customerId));
                    return;
                }

                Cm_CustomerISIM.Value = cari.Name;
                cariWhois.InnerText = cari.ShortName;
                smlCustomerName.InnerText = cari.Name;
                btnNewCustomerTop.Attributes.Add("style", "display:block");
                Cm_CustomerKOD.Value = cari.NetsisHaselCode;
                HSL_KISALTMA.Value = cari.ShortName;
                HSL_VD.Value = cari.TaxOffice;
                HSL_VN.Value = cari.TaxNumber;
                CariOnayDurumMetin(cari.StatusId);

                if (cari.SectorId != null && cari.SectorId > 0)
                {
                    ddHSL_SEKTORID.SelectedValue = ddHSL_SEKTORID.Items.FindByValue(cari.SectorId.ToString()).Value;
                }
                else
                {
                    ddHSL_SEKTORID.SelectedIndex = 0;
                }

                // 0 hasel 1 index
                if (cari.IsHasel == null)
                {
                    ddHSL_FIRMA.SelectedIndex = 0;
                }
                else if (cari.IsHasel == true)
                {
                    ddHSL_FIRMA.SelectedIndex = 1;
                }
                else if (cari.IsHasel == false)
                {
                    ddHSL_FIRMA.SelectedIndex = 2;
                }

                Cm_CustomerKODO.Value = cari.NetsisRentliftCode;
                Cm_CustomerKODH.Value = cari.NetsisHaselCode;
                HSL_WEB.Value = cari.Web;
                if (cari.StatusId == 1)
                    rblHaselDurum.SelectedIndex = 1;
                else rblHaselDurum.SelectedIndex = 0;

                int uid = Convert.ToInt32(Session["UserId"]);
                string result = _cs.IsApplicableCustomer(customerId, uid, isAuthenticated).Trim();
                if (result != "")
                {
                    labellHaselDurum.InnerHtml = "<strong>CARİ'NİN ONAYLANABİLMESİ İÇİN</strong> <br/> " + result;
                    divAlertBg.Visible = true;
                    //null kontrolu ebru.caglar da null geldigi icin konulmustur.
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "starthideRbHaselDurum",
                        @"if( document.getElementById('ContentPlaceHolder1_rblHaselDurum') != null)
                         document.getElementById('ContentPlaceHolder1_rblHaselDurum').style.visibility = 'hidden';",
                        true);
                }
                else
                {
                    //adminde panel gorunmez. todo: admin yetkisine sahip olanlara otomatik kontrol yetkisi disinda gelmesi gerekiyor.
                    string str =
                        @"  if( document.getElementById('ContentPlaceHolder1_rblHaselDurum') != null)
                          {document.getElementById('ContentPlaceHolder1_rblHaselDurum').style.visibility = 'show';}";
                    divAlertBg.Visible = false;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "starthideRbHaselDurum", str, true);
                }
            }
            else rblHaselDurum.Attributes.Add("style", "display:none");
        }

        private void CariOnayDurumMetin(int? cariStatusId)
        {
            if (cariStatusId != null && cariStatusId == 1)
            {
                spnOnayli.Visible = true;
            }
            else
            {
                spnOnaysiz.Visible = true;
            }
        }

        private void LoadSektors()
        {
            List<Gn_Sector> sectors = _cs.GetSectors().OrderBy(k => k.SectorName).ToList();
            ddHSL_SEKTORID.DataTextField = "SectorName";
            ddHSL_SEKTORID.DataValueField = "Id";
            sectors.Insert(0, new Gn_Sector() { SectorName = "Seçiniz..." });
            ddHSL_SEKTORID.DataSource = sectors;
            ddHSL_SEKTORID.DataBind();
            ddHSL_SEKTORID.Items[0].Value = "";
        }

        protected void btnSaveGeneral_Click(object sender, EventArgs e)
        {
            if (ddHSL_FIRMA.SelectedIndex <= 0 || ddHSL_SEKTORID.SelectedIndex <= 0 || rblMainCustomer.SelectedItem == null || rblHaselDurum.SelectedItem == null)
            {
                message.InnerText = "Tüm seçimleri yaparak tekrar deneyin";
                return;
            }
            message.InnerText = "";

            bool IsMainCustomer = rblMainCustomer.SelectedItem != null && rblMainCustomer.SelectedItem.Value == "1";

            bool haselDurum = rblHaselDurum.SelectedItem != null && rblHaselDurum.SelectedItem.Value == "1";

            Cm_Customer cari = null;
            if (Session["CId"] == null || Session["CId"].ToString() == "0")
            {
                cari = new Cm_Customer()
                {
                    Name = Cm_CustomerISIM.Value,
                    ShortName = HSL_KISALTMA.Value,
                    TaxOffice = HSL_VD.Value,
                    TaxNumber = HSL_VN.Value,
                    SectorId = Convert.ToInt32(ddHSL_SEKTORID.SelectedItem.Value),
                    IsHasel = Convert.ToInt32(ddHSL_FIRMA.SelectedIndex) == 1 ? true : false,
                    NetsisRentliftCode = Cm_CustomerKODO.Value,
                    NetsisHaselCode = Cm_CustomerKODH.Value,
                    Web = HSL_WEB.Value,
                    StatusId = haselDurum == true ? 1 : 0
                };
                int uid = Convert.ToInt32(Session["UserId"]);
                Session["CId"] = _cs.InsertCustomer(cari, uid);
            }
            else
            {
                cari = _cs.GetCustomerById(Convert.ToInt32(Session["CId"]));
                cari.Name = Cm_CustomerISIM.Value;
                cari.ShortName = HSL_KISALTMA.Value;
                cari.TaxOffice = HSL_VD.Value;
                cari.TaxNumber = HSL_VN.Value;
                cari.SectorId = Convert.ToInt32(ddHSL_SEKTORID.SelectedItem.Value);
                cari.IsHasel = Convert.ToInt32(ddHSL_FIRMA.SelectedIndex) == 1 ? true : false;
                cari.NetsisRentliftCode = Cm_CustomerKODO.Value;
                cari.NetsisHaselCode = Cm_CustomerKODH.Value;
                cari.Web = HSL_WEB.Value;
                cari.StatusId = haselDurum == true ? 1 : 0;
                _cs.SaveCustomer(cari, Convert.ToInt32(Session["USerId"]));
            }

            message.InnerText = "Başarı ile kaydedilmiştir...";
        }

        protected void btnNewCustomer_Click(object sender, EventArgs e)
        {
            Session["CId"] = 0;

            Cm_CustomerISIM.Value = "";
            Cm_CustomerKOD.Value = "";
            HSL_KISALTMA.Value = "";
            HSL_VD.Value = "";
            HSL_VN.Value = "";
            ddHSL_SEKTORID.SelectedIndex = 0;
            ddHSL_FIRMA.SelectedIndex = 1;
            Cm_CustomerKODO.Value = "";
            Cm_CustomerKODH.Value = "";
            HSL_WEB.Value = "";
            rblHaselDurum.SelectedIndex = 1;
            rblMainCustomer.SelectedIndex = 0;

            btnSave.InnerText = "Kaydet";
        }

        #endregion General

        #region Machinepark

        protected void btnMachineparkSaveAndClose_Click(object sender, EventArgs e)
        {
            int mainCategoryId = Convert.ToInt32(ddMpMachineparkType.SelectedItem.Value);
            int selectedMarkId = Convert.ToInt32(ddMpMarks.SelectedItem.Value);
            string model = txtMpModel.Value;
            int mpCount = Convert.ToInt32(txtMpMachineparkCount.ToString());
            int retireOrOwneeredId = Convert.ToInt32(ddMpRetireOrOwnered.SelectedItem.Value);
            //int saleEnginerId = Convert.ToInt32(ddMpSaleEngineer.SelectedItem.Value);
            int machineparkLocationId = Convert.ToInt32(ddMpMachineparkLocation.SelectedItem.Value);
            int customerId = Convert.ToInt32(Session["CId"]);
        }

        #endregion Machinepark

        #region Locations

        [WebMethod]
        public string LoadRegions(string bolVal)
        {
            var res = _cs.GetRegions(bolVal).OrderBy(k => k.CityName).ToList();
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(res);
        }

        protected void ddLocArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadCities();
        }

        private void LoadLocations()
        {
            if (Request.QueryString["CId"] == null)
            {
                return;
            }
            hdnCustomerId.Value = Request.QueryString["CId"].ToString();
            int customerId = Convert.ToInt32(Request.QueryString["CId"]);
            List<Cm_CustomerLocations> locations = _cs.GetLocationByCustomerId(customerId);

            if (locations == null)
                return;
            spLocCount.InnerText = locations.Count().ToString();

            string lHtml = "<div class=\"table-scrollable\">"
                        + "<table id=\"locContentTable\" class=\"table table-striped table - bordered table - hover\"><thead>"
                        + "					<tr>"
                        + "						<th scope=\"col\"> Lokasyon</th>"
                        + "						<th scope=\"col\"> İl</th>"
                        + "						<th scope=\"col\"> İlçe</th>"
                        + "						<th scope=\"col\"> Telefon</th>"
                        + "						<th scope=\"col\"> Fax</th>"
                        + "						<th scope=\"col\"> Fat. Adr.</th>"
                        //+ "						<th scope=\"col\"> Kopyala</th>"
                        //+ "						<th scope=\"col\"> Koord</th>"
                        + "						<th scope=\"col\"> Güncelle</th>"
                        + "					</tr>"
                        + "              </thead>"
                        + "            <tbody>";

            if (locations.Count == 0)
            {
                txtLocDefinition.Value = "MERKEZ";
                txtLocDefinition.Style.Add("background-color", "#FF5722");
                txtLocDefinition.Style.Add("color", "#000 !important");
                txtLocDefinition.Style.Add("font-weight", "bold");

                txtLocDefinition.Style.Add("title", "İlk lokasyon girişinizde Merkez lokasyonu giriniz");
            }

            foreach (Cm_CustomerLocations loc in locations)
            {
                lHtml += "<tr id=locTr" + loc.Id + ">"
                    + " <td>" + loc.Name + "</td>"
                    + " <td>" + loc.CityName + "</td>"
                    + " <td>" + loc.RegionName + "</td>"
                    + " <td>" + loc.Phone + "</td>"
                    + " <td>" + loc.Fax + "</td>"
                    + " <td><input id=\"inpLocationStatus" + loc.Id + "\" class=\"btn btn-warning\" type=\"button\" value=\"" + ((loc.IsFat == true) ? "Evet" : "Hayır") + "\" onclick=\"ChangeLocation('" + loc.Id + "','inpLocationStatus" + loc.Id + "');\" /></td>"
                    + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateLocationToTop(" + loc.Id + ",'" + loc.CityName + "','" + loc.RegionName + "','" + loc.Address + "','" + loc.Name.ToString().Trim() + "','" + loc.Phone + "','" + loc.Fax + "','" + loc.Longitude + "','" + loc.Latitude + "','" + loc.IsFat + "','" + 1 + "','" + loc.IsDeleted + "'); ToggleLocationShow();\" /></td>"
                    + "</tr>";
                if (loc.Longitude != null && loc.Latitude != null && loc.Longitude.Trim() != "" && loc.Latitude.Trim() != "")
                    hdnLongLatChain.Value += loc.Longitude + "," + loc.Latitude + "|";
            }
            lHtml += "</tbody></table></div>";
            locationList.InnerHtml = lHtml;
        }

        #endregion Locations

        private void LoadAuthenticators()
        {
            if (Request.QueryString["CId"] == null)
            {
                return;
            }
            hdnCustomerId.Value = Request.QueryString["CId"].ToString();
            int customerId = Convert.ToInt32(Request.QueryString["CId"]);
            List<Cm_CustomerAuthenticators> auths = _cs.GetAuthenticatorByCustomerId(customerId);
            if (auths == null)
                return;
            spAuthCount.InnerText = auths.Count().ToString();

            string lHtml = "<div class=\"table-scrollable\">"
                        + "<table id=\"authContentTable\" class=\"table table-striped table - bordered table - hover\"><thead>"
                        + "					<tr>"
                        + "						<th scope=\"col\"> Lokasyon</th>"
                        + "						<th scope=\"col\"> Yetkili Adı</th>"
                        + "						<th scope=\"col\"> Gsm</th>"
                        + "						<th scope=\"col\"> Telefon</th>"
                        + "						<th scope=\"col\"> Fax</th>"
                        + "						<th scope=\"col\"> Email</th>"
                        + "						<th scope=\"col\"> Ünvan</th>"
                        + "						<th scope=\"col\"> İşlem</th>"
                        + "					</tr>"
                        + "              </thead>"
                        + "            <tbody>";
            foreach (Cm_CustomerAuthenticators a in auths)
            {
                string locationName = "";
                Cm_CustomerLocations lok = _cs.GetLocationById(Convert.ToInt32(a.CustomerLocationId));
                if (lok != null)
                    locationName = lok.Name;
                lHtml += "<tr id=\"authTr" + a.Id + "\">"
                    + " <td>" + locationName + "</td>"
                    + " <td>" + a.Name + "</td>"
                    + " <td>" + a.Gsm + "</td>"
                    + " <td>" + a.Phone1 + "</td>"
                    + " <td>" + a.Fax + "</td>"
                    + " <td>" + a.Email + "</td>"
                    + " <td>" + a.Title + "</td>"
                    + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateAuthenticator('" + a.Id + "', " + a.CustomerLocationId + ",'" + a.Name + "','" + a.Gsm + "','" + a.Phone1 + "','" + a.Fax + "','" + a.Email + "','" + a.Title + "'); ToggAuthenticatorsShow();\" /></td>"
                    + "</tr>";
            }
            lHtml += "</tbody></table></div>";
            authenticatorList.InnerHtml = lHtml;
        }

        private void LoadSalesmans()
        {
            if (Request.QueryString["CId"] == null)
            {
                return;
            }
            hdnCustomerId.Value = Request.QueryString["CId"].ToString();
            int customerId = Convert.ToInt32(Request.QueryString["CId"]);
            var auths = _cs.GetSalesmanById(customerId).OrderBy(m => m.Type).ThenByDescending(m => m.Flag);

            if (auths == null)
                return;
            spSaleEngineerCount.InnerText = auths.Count().ToString();

            string lHtml = "<div class=\"table-scrollable\">"
                        + "<table id=\"salesmanContentTable\" class=\"table table-striped table - bordered table - hover\"><thead>"
                        + "					<tr>"
                        + "						<th scope=\"col\"> Satıcı Adı</th>"
                        + "						<th scope=\"col\"> Tipi</th>"
                        + "						<th scope=\"col\"> Bayrak</th>"
                        + "						<th scope=\"col\"> İşlem</th>"
                        + "						<th style='" + FlagEditAuth(hdnFlagEditAuth.Value) + "' scope=\"col\"> Bayrak</th>"
                        + "					</tr>"
                        + "              </thead>"
                        + "            <tbody>";
            var colorClassDic = GetColorDictionary();
            foreach (SalesmanWraper a in auths)
            {
                SalesmanRowColorDto classDto;
                if (colorClassDic.ContainsKey(a.Type))
                {
                    classDto = colorClassDic[a.Type];
                }
                else
                {
                    classDto = colorClassDic["Diğer"];
                }
                var flagClass = (a.Flag == true) ? "fa fa-flag" : "fa fa-flag-o";
                // Gn_User ouser = _us.GetUserById(Convert.ToInt32(a.SalesmanId));
                Gn_Category st = _cs.GetSalesmanTypeById(Convert.ToInt32(a.SalesmanTypeId));
                lHtml += "<tr class='" + classDto.textClass + "' id=\"salesTr" + a.Id + "\">"
                    + " <td>" + a.Name + "</td>"
                    + " <td>" + st.Title + "</td>"
                    + " <td class='" + classDto.textClass + "' Flaged='" + a.Flag + "'><button  class='" + classDto.buttonClass + "' type=\"button\" onclick=\"ChangeSalesmanFlag('" + a.Id + "','" + a.SalesmanTypeId + "');\"><i class='" + flagClass + "' aria-hidden='true'></i></button></td>"
                    + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateSalesman('" + a.Id + "', " + a.SalesmanId + ",'" + a.SalesmanTypeId + "','" + a.Flag + "','" + a.IsActive + "','" + a.IsDeleted + "','" + a.Type + "'); ToggSalesmanShow();\" /></td>"
                    + " <td style='" + FlagEditAuth(hdnFlagEditAuth.Value) + "' ><input id='btnFlag_" + a.Id + "'  style='" + FlagVisibility(a.Flag) + "' class=\"btn btn-success\" type=\"button\" value=\"Bayrak\" onclick=\"FlagButton_Click(this);\" /></td>"
                    + "</tr>";
            }
            lHtml += "</tbody></table></div>";
            saleEngineersContent.InnerHtml = lHtml;

            //if (_cs.HasAreaDirector(customerId) != null)
            //{
            //    ddAreaDirectos.SelectedValue = "0";
            //    ddAreaDirectos.Enabled = false;
            //    ddAreaDirectos.CssClass = "form-control valid";
            //}
        }

        public string FlagVisibilityForJs()
        {
            return $@"
                    var styleHide ='{styleHide}';
                    var styleShow ='{styleShow}';
            ";
        }

        private string FlagEditAuth(string str)
        {
            if (str == 0.ToString())
            {
                return styleHide;
            }
            else return styleShow;
        }

        private readonly string styleShow = "visibility:visible;";
        private readonly string styleHide = "visibility:hidden;";

        private string FlagVisibility(bool? aFlag)
        {
            if (hdnFlagEditAuth.Value == "1")
            {
                if (aFlag == null)
                {
                    return styleShow;
                }

                if (aFlag.Value)
                    return styleHide;

                return styleShow;
            }
            return styleHide;
        }

        private void LoadMachineparks()
        {
            if (Request.QueryString["CId"] == null)
            {
                return;
            }
            hdnCustomerId.Value = Request.QueryString["CId"].ToString();
            int customerId = Convert.ToInt32(Request.QueryString["CId"]);
            var mpws = _cs.GetMachineparksById(customerId);

            if (mpws == null)
                return;
            //   spMpCount.InnerText = mpws.Count().ToString();

            string lHtml = "<div class=\"table-scrollable\">"
                        + "<table id=\"machineparkContentTable\" class=\"table table-striped table - bordered table - hover\"><thead>"
                        + "					<tr>"
                        + "						<th scope=\"col\">Makine Türü</th>"
                        + "						<th scope=\"col\">Marka</th>"
                        + "						<th scope=\"col\">Model</th>"
                        + "						<th scope=\"col\">Seri No</th>"
                        + "						<th scope=\"col\">Yıl</th>"
                        + "						<th scope=\"col\">Adet</th>"
                        + "						<th scope=\"col\">Edinme Şekli</th>"
                        + "						<th scope=\"col\">Konum</th>"
                        + "						<th scope=\"col\">Güncelle</th>"
                        + "						<th scope=\"col\">Sil</th>"
                        + "					</tr>"
                        + "              </thead>"
                        + "            <tbody>";
            foreach (MachineparkWrapper mpw in mpws)
            {
                string strSaleDate = DateFormatBootstrap(mpw.SaleDate);
                string strReleaseDate = DateFormatBootstrap(mpw.ReleaseDate);
                //  string strPlanedReleaseDate = DateFormatBootstrap(mpw.PlanedReleaseDate);

                lHtml += "<tr id=\"mpTr" + mpw.Id + "\">"
                    + " <td>" + mpw.CategoryName + "</td>"
                    + " <td>" + mpw.MarkName + "</td>"
                    /* + " <td>" + mpw.Model + "</td>"*/
                    + " <td>" + mpw.SerialNo + "</td>"
                    + " <td>" + (mpw.ManufactureYear == 0 ? "" : mpw.ManufactureYear.ToString()) + "</td>"
                    + " <td>" + mpw.Quantity + "</td>"
                    + " <td>" + mpw.OwnerName + "</td>"
                    + " <td>" + mpw.LocationName + "</td>"
                    // + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateMachinePark(" + mpw.Id + ", " + mpw.CategoryId + ",'" + mpw.MarkId + "','" + /*mpw.Model*/ + "','" + mpw.SerialNo + "'," + (mpw.ManufactureYear == null ? 2016 : mpw.ManufactureYear) + "," + mpw.Quantity + "," + mpw.OwnerId + "," + mpw.LocationId + ",'" + strSaleDate + "','" + strReleaseDate + "','" + strPlanedReleaseDate + "'); ToggMachineparkShow();\" /></td>"
                    + " <td><input class=\"btn red\" type=\"button\" value=\"Sil\" onclick=\"DeleteMachinePark(" + mpw.Id + ");\" /></td>"
                    + "</tr>";
            }
            lHtml += "</tbody></table></div>";
            mpContent.InnerHtml = lHtml;
        }

        private string DateFormatBootstrap(DateTime? date)
        {
            if (date != null)
                return date?.ToString("dd-MM-yyyy");
            return "";
        }

    

        //[WebMethod]
        //public static string GetOrders(int CustomerId)
        //{
        //    var item = g;
        //    return GeneratorHtmlGrid();

        //}

        //public List<Cm_CustomerInterviews> GetRow(int i)
        //{
        //    var listCustomerInterview = _cs.GetListCustomerInterviews(new CustomerInterviewsFilter() { CustomerId = i }).List;
        //    return listCustomerInterview;
        //}

    

        private bool isOldInterview(DateTime? plenedDateTime, DateTime? interviewDate)
        {
            if (plenedDateTime != null)
            {
                if (plenedDateTime.Value < DateTime.Now && interviewDate == null)
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<string, SalesmanRowColorDto> GetColorDictionary()
        {
            var dicColor = new Dictionary<string, SalesmanRowColorDto>();
            dicColor.Add("Linde", new SalesmanRowColorDto("Linde", "font-red-thunderbird", "bg-red-thunderbird bg-font-red-thunderbird margin-bottom-10", "padding: 10px;", "btn "));
            dicColor.Add("Combilift", new SalesmanRowColorDto("Combilift", "font-green-haze", "bg-green-haze bg-font-green-haze margin-bottom-10", "padding: 10px;", "btn "));
            dicColor.Add("Traktör", new SalesmanRowColorDto("Traktör", "font-blue-chambray", "bg-blue-chambray bg-font-blue-chambray margin-bottom-10", "padding: 10px;", "btn "));
            dicColor.Add("Kobelco", new SalesmanRowColorDto("Kobelco", "font-green-turquoise", "bg-green-turquoise bg-font-green-turquoise margin-bottom-10", "padding: 10px;", "btn "));
            dicColor.Add("İş Makinesi", new SalesmanRowColorDto("İş Makinesi", "font-green-turquoise", "bg-green-turquoise bg-font-green-turquoise margin-bottom-10", "padding: 10px;", "btn "));
            dicColor.Add("Platform", new SalesmanRowColorDto("Platform", "font-yellow-gold", "bg-yellow-gold bg-font-yellow-gold margin-bottom-10", "padding: 10px;", "btn "));
            dicColor.Add("Terex", new SalesmanRowColorDto("Terex", "font-red", "bg-red bg-font-red margin-bottom-10", "padding: 10px;", "btn "));
            dicColor.Add("Diğer", new SalesmanRowColorDto("Diğer", "font-grey-gallery", "bg-grey-gallery bg-font-grey-gallery margin-bottom-10", "padding: 10px;", "btn "));
            return dicColor;
        }

        public string GetColorJson()
        {
            return JsonConvert.SerializeObject(this.GetColorDictionary());
        }

        public string GetWrapper()
        {
            return JsonConvert.SerializeObject(new CustomerInterviewsWrapper());
        }

        protected void btnDeleteGeneral_Click(object sender, EventArgs e)
        {
            string str = _cs.CustomerDeleteorUndo(Convert.ToInt32(Request.QueryString["cid"]), (int)Session["userid"]);
            if (str.Length == 0)
            {
                Response.Redirect("CustomerDetail.aspx");
            }
        }

        public string Disabletab2()
        {
            if (Request.QueryString["CId"] != null)
            {
                int customerId = 0;
                if (!int.TryParse(Request.QueryString["CId"], out customerId))
                {
                    return "disable";
                }
            }
            return "";
        }
    }

    public class SalesmanRowColorDto
    {
        public string typeText { get; set; }
        public string textClass { get; set; }
        public string backgroundClass { get; set; }
        public string backgroundStyle { get; set; }
        public string buttonClass { get; set; }

        public SalesmanRowColorDto(string typeText, string textClass, string backgroundClass, string backgroundStyle, string buttonClass)
        {
            this.typeText = typeText;
            this.textClass = textClass;
            this.backgroundClass = backgroundClass;
            this.backgroundStyle = backgroundStyle;
            this.buttonClass = buttonClass;
        }
    }
}