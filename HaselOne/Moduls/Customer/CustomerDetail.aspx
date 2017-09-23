<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerDetail.aspx.cs"
    Inherits="HaselOne.CustomerDetail" EnableEventValidation="false" %>

<%@ Import Namespace="DAL.Helper" %>
<%@ Import Namespace="HaselOne.Controler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <asp:Button runat="server" ID="btnDeleteTest" Text="Silme" />--%>
    <div ng-controller="CustomerController">
        <div class="col-md-12">
            <asp:HiddenField ID="hdnSalesmanTotal" runat="server" />
            <asp:HiddenField ID="hdnFormStatus" runat="server" />
            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <asp:HiddenField ID="hdnLongLatChain" runat="server" />

            <asp:HiddenField ID="hdnGeneralInsert" runat="server" />

            <asp:HiddenField ID="hdnGeneralEdit" runat="server" />
            <asp:HiddenField ID="hdnGeneralDelete" runat="server" />
            <asp:HiddenField ID="hdnLocationInsert" runat="server" />
            <asp:HiddenField ID="hdnLocationEdit" runat="server" />
            <asp:HiddenField ID="hdnLocationDelete" runat="server" />

            <asp:HiddenField ID="hdnAuthenticatorInsert" runat="server" />
            <asp:HiddenField ID="hdnAuthenticatorEdit" runat="server" />
            <asp:HiddenField ID="hdnAuthenticatorDelete" runat="server" />
            <asp:HiddenField ID="hdnEngineerInsert" runat="server" />
            <asp:HiddenField ID="hdnEngineerEdit" runat="server" />
            <asp:HiddenField ID="hdnEngineerDelete" runat="server" />
            <asp:HiddenField ID="hdnMachineparkInsert" runat="server" />
            <asp:HiddenField ID="hdnMachineparkEdit" runat="server" />
            <asp:HiddenField ID="hdnMachineparkDelete" runat="server" />

            <asp:HiddenField ID="hdnInterviewInsert" runat="server" />
            <asp:HiddenField ID="hdnInterviewEdit" runat="server" />
            <asp:HiddenField ID="hdnInterviewDelete" runat="server" />

            <asp:HiddenField ID="hdnFlagEditAuth" runat="server" Value="0" />

            <asp:HiddenField ID="hdnRequestInsert" runat="server" />
            <asp:HiddenField ID="hdnRequestEdit" runat="server" />
            <asp:HiddenField ID="hdnRequestDelete" runat="server" />

            <div class="portlet">
                <asp:Label ID="lblSaveStatus" runat="server" Text="fdsfds" Font-Bold="True"></asp:Label>

                <!-- BEGIN PAGE TITLE-->

                <h1 class="page-title">
                    <span id="cariWhois" runat="server">Cari Arayın ya da Oluşturun</span>

                    <% if (Request.QueryString["cid"] != null)
                        { %>

                    <!-- Onaylı Cari -->

                    <small>
                        <span id="spnOnayli" visible="False" runat="server" class="fa-stack tooltips" data-style="default" data-container="body" data-original-title="Onaylı" aria-hidden="true"><i class="fa fa-certificate fa-stack-2x font-green-jungle"></i><i class="fa fa-check fa-stack-1x fa-inverse"></i></span>

                        <!-- Onaysız Cari -->
                        <span id="spnOnaysiz" visible="False" runat="server" class="fa-stack tooltips" data-style="default" data-container="body" data-original-title="Onaysız" aria-hidden="true"><i class="fa fa-certificate fa-stack-2x font-red-thunderbird"></i><i class="fa fa-close fa-stack-1x fa-inverse"></i></span>
                    </small>
                    <% } %>
                    <small id="smlCustomerName" runat="server"></small>
                    <a id="btnNewCustomerTop" runat="server" type="button" class="pull-right btn btn-sm blue" style="margin-right: 5px; display: none;" href="/Moduls/Customer/CustomerDetail.Aspx"><i class="fa fa-edit" aria-hidden="true"></i><span class="hidden-xs">Yeni Cari Oluştur</span></a>
                </h1>
                <!-- END PAGE TITLE-->
                <div class="portlet-title">
                    <div class="caption">
                        <a id="message" runat="server" style="color: red;"></a>
                    </div>
                    <div class="caption col-md-12 col-sm-12 col-xs-12">
                        <asp:TextBox ID="txtUnvan" runat="server" class="form-control headautocomplete" placeholder="Cari İsmi..." type="text"></asp:TextBox>
                    </div>
                </div>
                <div class="portlet-body">
                    <div id="tabContainer" class="tabbable-custom tabbable-tabdrop">
                        <ul class="nav nav-tabs">
                            <li class="active">
                                <a href="#tab_general" data-toggle="tab">Genel Bilgiler </a>
                            </li>
                            <li id="tlocation" runat="server">
                                <a href="#tab_locations" data-toggle="tab" class="locationstab">Lokasyonlar
                                <span id="spLocCount" runat="server" class="label-secondary badge"></span>
                                </a>
                            </li>
                            <li id="tauth" runat="server">
                                <a href="#tab_Auth" data-toggle="tab">Yetkililer
                                <span id="spAuthCount" runat="server" class="label-secondary badge"></span>
                                </a>
                            </li>
                            <li id="tengineers" runat="server">
                                <a href="#tab_saleengineers" id="tabEngineer" data-toggle="tab">Satış Mühendisleri
                                <span id="spSaleEngineerCount" runat="server" class="label-secondary badge"></span>
                                </a>
                            </li>
                            <%-- <li id="tmpark" runat="server">
                                <a href="#tab_machinepark" data-toggle="tab" id="tbMachinepark">Makine Parkı
                                <span id="spMpCount" runat="server" class="label-secondary badge"></span>
                                </a>
                            </li>--%>
                            <li id="Li1" runat="server">
                                <a href="#tab_machineparkNew" data-toggle="tab" id="tbMachineparkNew" ng-click="initTab('initMachineparkTab')">Makine Parkı
                               <span id="spRequestCount" class="label-secondary badge">{{CustomerStats.ActiveMachinepark}}</span>
                                </a>
                            </li>
                            <li id="tInterviews" runat="server">
                                <a href="#tab_Interview" data-toggle="tab" id="tvInterview" ng-click="initTab('initInterviewTab')">Görüşmeler
                                <span id="spInterviewCount" runat="server" class="label-secondary badge">0</span>
                                </a>
                            </li>

                            <li id="tRequest" runat="server" visible="False">
                                <a href="#tab_Request" data-toggle="tab" id="tvRequest" ng-click="initTab('initRequestTab')">Talepler
                                <span id="spRequestCount" class="label-secondary badge">{{CustomerStats.Request}}</span>
                                </a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane active" id="tab_general">

                                <%--                            <a class="btn btn-success recordAndNew" onclick="NewCustomer();">Yeni Cari Aç
                            </a>--%>
                                <div class="row form">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                        <div class="alert alert-danger" runat="server" id="divAlertBg" visible="False">

                                            <label id="labellHaselDurum" runat="server" class=""></label>
                                            <%--  <strong>CARİ ONAYI İÇİN</strong><br>Lokasyon tanımı yapılmalı<br>Satış Mühendisi tanımı yapılmalı <br>Makine Parkı tanımı yapılmalı--%>
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Unvan (fatura)</b></label>
                                        <input class="form-control controls" type="text" id="Cm_CustomerISIM" name="Cm_CustomerISIM" runat="server" placeholder="Unvanı Giriniz" maxlength="200" required title="* 5 karakterden fazla olmalı" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Cari Kodu</b></label>
                                        <input class="form-control" type="text" id="Cm_CustomerKOD" runat="server" placeholder="Cari Kodu Yazınız" maxlength="50" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Kısa Tanım</b></label>
                                        <input class="form-control" type="text" id="HSL_KISALTMA" runat="server" placeholder="Kısa Tanım Yazınız" maxlength="50" required title="* Gerekli" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Vergi Dairesi</b></label>
                                        <input class="form-control" type="text" id="HSL_VD" runat="server" placeholder="Vergi Dairesini Yazınız" maxlength="50" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Vergi No</b></label>
                                        <input class="form-control" type="text" id="HSL_VN" runat="server" placeholder="Vergi Numarasını Yazınız" maxlength="50" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Sektör</b></label>
                                        <asp:DropDownList ID="ddHSL_SEKTORID" runat="server" class="form-control" data-placeholder="Sektör Seçiniz">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Müşteri Firma</b></label>
                                        <select class="form-control todo-taskbody-tags select2" id="ddHSL_FIRMA" runat="server" data-placeholder="Seçiniz" required title="* Gerekli">
                                            <option value="">Seçiniz</option>
                                            <option value="1">Hasel</option>
                                            <option value="2">Rentlift</option>
                                        </select>
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Rentlift Netsis Cari Kodu</b></label>
                                        <input class="form-control" type="text" id="Cm_CustomerKODO" runat="server" placeholder="Rentlift Cari Kodu Giriniz" maxlength="50" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Hasel Netsis Cari Kodu</b></label>
                                        <input class="form-control" type="text" id="Cm_CustomerKODH" runat="server" placeholder="Hasel Cari Kodu Giriniz" maxlength="50" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                        <label><b>Web Sitesi</b></label>
                                        <input class="form-control" type="text" id="HSL_WEB" runat="server" placeholder="Websitesi Adresini Yazınız" maxlength="50" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group" id="divDurum">
                                        <label id="Label1" runat="server" class=""><b>Durum</b></label>
                                        <div>
                                            <asp:RadioButtonList CssClass="mt-radio mt-radio-outline" ID="rblHaselDurum" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0">Onaysız</asp:ListItem>
                                                <asp:ListItem Value="1">Onaylı</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-12 form-group" style="visibility: hidden;">
                                        <label><b>Ana Cari mi?</b></label>
                                        <div class="input-group">
                                            <asp:RadioButtonList CssClass="mt-radio-inline" ID="rblMainCustomer" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1">Evet</asp:ListItem>
                                                <asp:ListItem Value="2">Hayır</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-12 col-xs-12 form-group">
                                        <a class="btn btn-success recordAndClose" onclick="SaveGeneral();" id="btnSave" runat="server">Güncelle</a>
                                        <%--  </div>--%>
                                        <% if (Request.QueryString["CId"] != null)
                                            { %>
                                        <%--<div class="col-md-12 col-sm-12 col-xs-12 form-group">--%>
                                        <asp:Button CssClass="btn red recordAndClose" ID="btnDeleteGeneral" runat="server" Text="Sil" OnClientClick="return DeleteConfirm()" Visible="False" OnClick="btnDeleteGeneral_Click" />

                                        <% } %>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane" id="tab_locations">

                                <div id="dynamicallycreatedinputcontainer"></div>
                                <a class="list-toggle-container">
                                    <div class="list-toggle done uppercase">
                                        <button style="margin-bottom: 10px !important;" type="button" class="btn btn-danger btnLocation" onclick="ToggleLocation();"><i class="fa fa-plus"></i>Yeni Kayıt Ekle</button>
                                    </div>
                                </a>
                                <div class="task-list panel-collapse collapse location">
                                    <input id="pacInput" class="controls" type="text" placeholder="Lokasyon Ara">
                                    <div class="form">
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>İl</b></label>
                                            <asp:DropDownList ID="ddLocCity" runat="server" class="form-control" data-placeholder="İl Seçiniz" required title="İl Tanımı zorunludur">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>İlçe</b></label>
                                            <asp:DropDownList ID="ddLocRegion" runat="server" class="form-control" data-placeholder="İlçe Seçiniz" required title="İlçe Tanımı zorunludur">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Lokasyon Adı</b></label>
                                            <input class="form-control" type="text" id="txtLocDefinition" runat="server" placeholder="ör:MERKEZ" maxlength="50" required title="Lokasyon Tanımı zorunludur" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Adres</b></label>
                                            <input class="form-control" type="text" id="txtLocAddress" runat="server" placeholder="Adres Yazınız" maxlength="200" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Telefon</b></label>
                                            <input class="form-control" type="text" id="txtLocTel" runat="server" placeholder="Telefon Yazınız" maxlength="50" title="Max 10 karakter ör:555XXXXXXX" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Faks</b></label>
                                            <input class="form-control" type="text" id="txtLocFax" runat="server" placeholder="Fax Yazınız" maxlength="50" title="Max 10 karakter ör:555XXXXXXX" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Enlem</b></label>
                                            <input class="form-control" type="text" id="txtLocLat" runat="server" placeholder="Latitude Yazınız" maxlength="50" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Boylam</b></label>
                                            <input class="form-control" type="text" id="txtLocLong" runat="server" placeholder="Longitude Yazınız" maxlength="50" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Enlem ve Boylam'ı Haritadan Seçin</b></label>
                                            <a id="showMyMap" class="form-control btn btn-sm red">Harita Aç/Kapat <i class="fa fa-map-marker"></i></a>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12 form-group" id="MyMap">
                                            <div id="map" class="gmaps"></div>
                                        </div>

                                        <div class="clearfix"></div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <br />
                                            <label style="display: none;"><b>Fatura Adresi mi?</b></label>
                                            <asp:RadioButtonList ID="rbLocFat" runat="server" RepeatDirection="Horizontal" Style="display: none;">
                                                <asp:ListItem Value="1">Evet</asp:ListItem>
                                                <asp:ListItem Value="2">Hayır</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group" style="display: none;">
                                            <br />
                                            <label><b>Aktif mi</b></label>
                                            <div class="input-group">
                                                <asp:RadioButtonList ID="rbIsLocActive" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">Evet</asp:ListItem>
                                                    <asp:ListItem Value="2">Hayır</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group" style="display: none;">
                                            <br />
                                            <label><b>Silinmişlik</b></label>
                                            <div class="input-group">
                                                <asp:RadioButtonList ID="rbIsLocDeleted" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">Evet</asp:ListItem>
                                                    <asp:ListItem Value="2">Hayır</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 form-group">
                                        <a class="col-xs-3 btn btn-info recordLocationAndClose RACLE1" onclick="LocationSave(1);" style="margin-right: 10px !important;">Kaydet ve Kapat
                                        </a>
                                        <a class="col-xs-3 btn btn-success recordLocationAndNew RACLE2" onclick="LocationSave(2);" style="margin-right: 10px !important;">Kaydet ve Yeni Kayıt Aç
                                        </a>
                                        <a class="col-xs-2 btn btn-danger RACLE3" onclick="LocationDelete();" style="display: none; margin-right: 10px !important;">Sil
                                        </a>
                                        <a class="col-xs-3">
                                            <input type="button" id="btnaddmarker" class="btn btn-warning" value="Yeni Koordinat Belirle" style="margin-right: 10px !important;" />
                                        </a>
                                    </div>
                                </div>

                                <div id="locationList" runat="server"></div>
                            </div>
                            <div class="tab-pane" id="tab_Auth">
                                <a class="list-toggle-container">
                                    <div class="list-toggle done uppercase">
                                        <button type="button" class="btn btn-danger btnNewAuthenticator" onclick="ToggAuthenticators();"><i class="fa fa-plus"></i>Yeni Kayıt Ekle</button>
                                    </div>
                                </a>
                                <div class="task-list panel-collapse collapse auth">
                                    <div class="row form">
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Yetkili Adı</b></label>
                                            <input class="form-control" type="text" id="txtAuthName" runat="server" placeholder="Yetkili Adını Yazınız" maxlength="50" required title="Yetkili Adı zorunludur" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Gsm ((0 hariç))</b></label>
                                            <input class="form-control" type="text" id="txtAuthGsm" runat="server" placeholder="Gsm Yazınız. Ör:532XXXXXXX" maxlength="50" title="Max 10 Karakter ör:555XXXXXXX" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Telefon</b></label>
                                            <input class="form-control" type="text" id="txtAuthPhone" runat="server" placeholder="Yetkili Telefonu Yazınız" maxlength="50" title="Max 10 Karakter ör:555XXXXXXX" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Fax</b></label>
                                            <input class="form-control" type="text" id="txtAuthFax" runat="server" placeholder="Yetkili Fax Yazınız" maxlength="50" title="Max 10 Karakter ör:555XXXXXXX" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Email</b></label>
                                            <input class="form-control" type="text" id="txtAuthEmail" runat="server" placeholder="Yetkili Email Yazınız" maxlength="50" title="Email formatı hatalı" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Ünvan</b></label>
                                            <input class="form-control" type="text" id="txtAuthTitle" runat="server" placeholder="Yetkili Ünvanı Yazınız" maxlength="50" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Lokasyon</b></label>
                                            <asp:DropDownList ID="ddAutLocation" runat="server" class="form-control" data-placeholder="Lokasyon Seçiniz" required title="Lokasyon seçimi zorunludur">
                                            </asp:DropDownList>
                                        </div>
                                        <div class=" col-xs-12 form-group">
                                            <a class="col-xs-3 btn btn-info recordAndClose RACA1" onclick="AuthenticatorSave(1);" style="margin-right: 10px !important;">Kaydet ve Kapat
                                            </a>
                                            <a class="col-xs-3 btn btn-success recordAndNew RACA2" onclick="AuthenticatorSave(2);" style="margin-right: 10px !important;">Kaydet ve Yeni Kayıt Aç
                                            </a>
                                            <a class="col-xs-3 btn btn-danger RACA3" onclick="AuthDelete();" style="display: none; margin-right: 10px !important;">Sil
                                            </a>
                                        </div>
                                    </div>
                                </div>
                                <div id="authenticatorList" runat="server"></div>
                            </div>

                            <div class="tab-pane" id="tab_saleengineers">
                                <a class="list-toggle-container">
                                    <div class="list-toggle done uppercase">
                                        <button type="button" class="btn btn-danger btnNewAuthenticator" onclick="ToggSEs();"><i class="fa fa-plus"></i>Yeni Kayıt Ekle</button>
                                    </div>
                                </a>
                                <div class="task-list panel-collapse collapse salesman">
                                    <div class="row form">
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Operasyon Tipi</b></label>
                                            <asp:DropDownList ID="ddSalesmanTypes" runat="server" class="form-control" data-placeholder="Satıcı Türü Seçiniz" required title="Operasyon Seçmelisiniz...">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label id="lblSaleEngineer"><b>Satış Mühendisi</b></label>
                                            <asp:DropDownList ID="ddSalesEngineeers" runat="server" class="form-control" data-placeholder="Satış Mühendisi Seçiniz" required title="Satıcı Seçmelisiniz...">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group" style="display: none;">
                                            <br />
                                            <label><b>Bayrak</b></label>
                                            <div class="input-group">
                                                <asp:RadioButtonList ID="rbSEFlag" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">İşaretli</asp:ListItem>
                                                    <asp:ListItem Value="2">İşaretsiz</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group" style="display: none;">
                                            <label><b>Aktif mi</b></label>
                                            <div class="input-group">
                                                <asp:RadioButtonList ID="rbSEAktivity" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">Evet</asp:ListItem>
                                                    <asp:ListItem Value="2">Hayır</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group" style="display: none;">
                                            <label><b>Silinmişlik</b></label>
                                            <div class="input-group">
                                                <asp:RadioButtonList ID="rbSEDeleted" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">Evet</asp:ListItem>
                                                    <asp:ListItem Value="2">Hayır</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 form-group">
                                            <a class="col-xs-3 btn btn-info recordAndClose RACSE1" onclick="SalesmanSave(1);" style="margin-right: 10px !important;">Kaydet ve Kapat
                                            </a>
                                            <a class="col-xs-3 btn btn-success recordAndNew RACSE2" onclick="SalesmanSave(2);" style="margin-right: 10px !important;">Kaydet ve Yeni Kayıt Aç
                                            </a>
                                            <a class="col-xs-3 btn btn-danger RACSE3" onclick="SalesmanDelete();" style="display: none; margin-right: 10px !important;">Sil
                                            </a>
                                        </div>
                                    </div>
                                </div>
                                <div id="saleEngineersContent" runat="server"></div>
                            </div>
                            <!-- *******************************************MACHINEPARK TAB********************************************************* -->

                            <div class="tab-pane" id="tab_machineparkNew" ng-controller="CustomerMachineparkController">
                                <div class="portlet light bordered">
                                    <div class="portlet-title tabbable-custom tabbable-tabdrop no-padding">
                                        <div class="caption pull-right">
                                            <div class="btn-toolbar remove-margin" ng-if="IsActiveTab">
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-default" ng-if="!IsEditPanelActive" ng-click="SetEditPanel(!IsEditPanelActive); btnMachineparkNew_Click();" tooltip="Yeni Makine Ekle">
                                                        <i class="fa fa-plus"></i>
                                                    </button>
                                                    <button type="button" class="btn btn-default" ng-if="IsEditPanelActive" ng-click="CloseEditPanel()" tooltip="İptal">
                                                        <i class="fa fa-times"></i>
                                                    </button>
                                                </div>
                                                <div class="btn-group">
                                                    <%-- <button type="button" class="btn btn-default" tooltip="Düzenle" ng-disabled="IsLoading || dataGridOptions.SelectedRow == null ||dataGridOptions.SelectedRows.length > 1 " ng-click="GetMachinepark(dataGridOptions.SelectedRow.Id);">
                                                        <i class="fa fa-pencil" ng-if="!IsLoading || IsSaving"></i>
                                                        <i class="fa fa-spinner fa-spin" ng-if="IsLoading && !IsSaving"></i>
                                                    </button>--%>
                                                    <button type="button" class="btn btn-default" ng-disabled="!IsEditPanelActive || mp_Form.$invalid || IsSaving || (dataGridOptions.SelectedRow != null && dataGridOptions.SelectedRow.HasRequest)" tooltip="Kaydet" ng-click="SaveMachinepark()">
                                                        <i class="fa fa-save" ng-if="!IsSaving"></i>
                                                        <i class="fa fa-spinner fa-spin" ng-if="IsSaving"></i>
                                                    </button>
                                                    <%--  <button type="button" class="btn btn-default" tooltip="Sil" ng-disabled="dataGridOptions.SelectedRows == null || dataGridOptions.SelectedRows.length == 0   || dataGridOptions.SelectedRow.HasRequest" ng-click="DeleteMachinepark()">
                                                        <i class="fa fa-trash-o"></i>
                                                    </button>--%>
                                                </div>
                                                <%--<div class="btn-group">
                                                    <button type="button" class="btn btn-default" ng-disabled="dataGridOptions.SelectedRows == null ||  dataGridOptions.SelectedRows.length != 1 || dataGridOptions.SelectedRow.HasRequest" ng-click="CopyMachinepark()" tooltip="Kopyala">
                                                        <i class="fa fa-copy"></i>
                                                    </button>
                                                </div>
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-default" ng-disabled="dataGridOptions.SelectedRow == null || dataGridOptions.SelectedRows.length != 1 ||  !dataGridOptions.SelectedRow.HasRequest" tooltip="Talebe Git" ng-click="GoToRequest()">
                                                        <i class="fa fa-tumblr"></i>
                                                    </button>
                                                    <button type="button" class="btn btn-default" ng-disabled="dataGridOptions.SelectedRows == null || dataGridOptions.SelectedRows.length == 0" tooltip="Elden Çıkar" ng-click="ReleaseMachinePark()">
                                                        <i class="fa fa-share-square-o"></i>
                                                    </button>
                                                </div>
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-default" ng-disabled="1== 1 || dataGridOptions.SelectedRows == null || dataGridOptions.SelectedRows.length == 0" tooltip="Kirala">
                                                        <i class="fa fa-sign-out"></i>
                                                    </button>
                                                    <button type="button" class="btn btn-default" ng-disabled="1== 1 || dataGridOptions.SelectedRows == null || dataGridOptions.SelectedRows.length == 0" tooltip="Kirayı Bitir">
                                                        <i class="fa fa-sign-in"></i>
                                                    </button>
                                                </div>--%>
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-default" tooltip="Yenile" ng-click="GridService.Refresh('machineparkGrid')">
                                                        <i class="fa fa-refresh"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="btn-toolbar remove-margin" ng-if="!IsActiveTab">
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-default" ng-if="IsEditPanelActive" ng-click="CloseEditPanel()" tooltip="Kapat">
                                                        <i class="fa fa-times"></i>
                                                        <i class="fa fa-spinner fa-spin" ng-if="IsLoading && !IsSaving"></i>
                                                    </button>
                                                    <%--<button type="button" class="btn btn-default" ng-if="!IsEditPanelActive" tooltip="Düzenle" ng-disabled="IsLoading || dataGridOptionsForPassives.SelectedRow == null ||dataGridOptionsForPassives.SelectedRows.length > 1 " ng-click="GetMachinepark(dataGridOptionsForPassives.SelectedRow.Id);">
                                                        <i class="fa fa-pencil" ng-if="!IsLoading || IsSaving"></i>
                                                        <i class="fa fa-spinner fa-spin" ng-if="IsLoading && !IsSaving"></i>
                                                    </button>--%>
                                                    <button type="button" class="btn btn-default" ng-disabled="!IsEditPanelActive || mp_Form.$invalid || IsSaving || (dataGridOptionsForPassives.SelectedRow != null && dataGridOptionsForPassives.SelectedRow.HasRequest)" tooltip="Kaydet" ng-click="SaveMachinepark()">
                                                        <i class="fa fa-save" ng-if="!IsSaving"></i>
                                                        <i class="fa fa-spinner fa-spin" ng-if="IsSaving"></i>
                                                    </button>
                                                    <%--  <button type="button" class="btn btn-default" tooltip="Sil" ng-disabled="dataGridOptionsForPassives.SelectedRows == null || dataGridOptionsForPassives.SelectedRows.length == 0   || dataGridOptionsForPassives.SelectedRow.HasRequest" ng-click="DeleteMachinepark()">
                                                        <i class="fa fa-trash-o"></i>
                                                    </button>--%>
                                                </div>
                                                <%--  <div class="btn-group">
                                                    <button type="button" class="btn btn-default" ng-disabled="dataGridOptionsForPassives.SelectedRows == null || dataGridOptionsForPassives.SelectedRows.length == 0" tooltip="Geri Al" ng-click="UnreleaseMachinePark()">
                                                        <i class="fa fa-reply"></i>
                                                    </button>
                                                    <button type="button" class="btn btn-default" ng-disabled="dataGridOptionsForPassives.SelectedRow == null || dataGridOptionsForPassives.SelectedRows.length > 1 ||  !dataGridOptionsForPassives.SelectedRow.HasRequest" tooltip="Talebe Git" ng-click="GoToRequest()">
                                                        <i class="fa fa-tumblr"></i>
                                                    </button>
                                                </div>--%>
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-default" tooltip="Yenile" ng-click="GridService.Refresh('machineparkGridPassive')">
                                                        <i class="fa fa-refresh"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                        <ul class="nav nav-tabs pull-left">
                                            <li class="active">
                                                <a href="#machineparkTab_activeMp" data-toggle="tab" ng-click="IsActiveTab = true">Aktif Makine Parkları
                                                    <span class="label-secondary badge">{{CustomerStats.ActiveMachinepark}}</span>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="#machineparkTab_passiveMp" data-toggle="tab" ng-click="IsActiveTab = false">Pasif Makine Parkları
                                                    <span class="label-secondary badge">{{CustomerStats.PassiveMachinepark}}</span>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="portlet-body no-padding">
                                        <div class="tab-content">
                                            <message-panel id="MessagePanelMachinePark" ng-model="MachineparkpanelResult"></message-panel>
                                            <div class="task-list panel-collapse collapse" id="MachineParkPanel">
                                                <div name="mp_Form" ng-form class="form">
                                                    <div class="row">

                                                        <div class="col-md-4 col-sm-6 col-xs-12">

                                                            <label class="control-label bold">Makine Kategorisi</label>

                                                            <div name="mp_Category" tree-select ng-model="CustomerMachinepark.CategoryId" options="MPCategory_SelectOptions" ng-enabled="!CustomerMachinepark.HasRequest" ng-required="true"></div>
                                                            <div validation-message="mp_Form.mp_Category">
                                                                <required></required>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                                            <label class="control-label bold">Marka</label>
                                                            <div search-select ng-model="MpMark" options="MpMark_SelectOptions" on-adding="SaveMpMark()" ng-enabled="!CustomerMachinepark.HasRequest" ng-sync-value="CustomerMachinepark.MarkId"></div>
                                                        </div>
                                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                                            <label class="control-label bold">Model</label>
                                                            <div search-select ng-model="MpModel" options="MpModel_SelectOptions" on-adding="SaveMpModel()" ng-enabled="!CustomerMachinepark.HasRequest" ng-sync-value="CustomerMachinepark.ModelId"></div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                                            <label class="control-label bold">Seri No</label>
                                                            <input type="text" class="form-control" placeholder="Seri No Giriniz" ng-disabled="CustomerMachinepark.HasRequest || CustomerMachinepark.Quantity > 1" ng-blur="CustomerMachinepark.Quantity = (CustomerMachinepark.SerialNo !=null && CustomerMachinepark.SerialNo !='') ? 1 : CustomerMachinepark.Quantity" ng-model="CustomerMachinepark.SerialNo">
                                                        </div>
                                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                                            <label class="control-label bold">Üretim Yıl</label>
                                                            <div search-select ng-model="MpYear" options="MpYear_SelectOptions" ng-disabled="CustomerMachinepark.HasRequest" ng-enabled="!CustomerMachinepark.HasRequest" ng-sync-value="CustomerMachinepark.ManufactureYear"></div>
                                                        </div>
                                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                                            <label class="control-label bold">Satın Alma Tarihi</label>
                                                            <input type="text" date-picker options="{}" class="form-control" name="saleDate" placeholder="Tarih giriniz" size="16" ng-disabled="CustomerMachinepark.HasRequest" ng-model="CustomerMachinepark.SaleDate" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                                            <label class="control-label bold">Adet</label>
                                                            <input name="mp_Quantity" type="number" class="form-control" placeholder="Adet giriniz" ng-change="CustomerMachinepark.SerialNo = CustomerMachinepark.Quantity > 1 ? null : CustomerMachinepark.SerialNo" ng-model="CustomerMachinepark.Quantity" hold-min-number="1" ng-required="true" ng-disabled="CustomerMachinepark.HasRequest || CustomerMachinepark.SerialNo != null && CustomerMachinepark.SerialNo != ''">
                                                            <div validation-message="mp_Form.mp_Quantity">
                                                                <required></required>
                                                                <min-number params="1"></min-number>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                                            <label class="control-label bold">Konum</label>
                                                            <div search-select ng-model="MpLocation" options="MpLocation_SelectOptions" ng-enabled="!CustomerMachinepark.HasRequest" ng-sync-value="CustomerMachinepark.LocationId"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="tab-pane active" id="machineparkTab_activeMp">
                                                <div class="table-scrollable myView">
                                                    <div id="machineparkGrid" dx-data-grid="GridService.SetOptions(dataGridOptions)" dx-item-alias="entity">
                                                        <div data-options="dxTemplate:{ name:'cellTemplateHasRequest' }">
                                                            <span ng-if="entity.data.HasRequest" class="badge badge-danger" tooltip="#{{entity.data.RequestId}}">T
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="machineparkTab_passiveMp">
                                                <div class="table-scrollable myView">
                                                    <div id="machineparkGridPassive" dx-data-grid="GridService.SetOptions(dataGridOptionsForPassives)" dx-item-alias="entity">
                                                        <div data-options="dxTemplate:{ name:'cellTemplateHasRequest' }">
                                                            <span ng-if="entity.data.HasRequest" class="badge badge-danger" tooltip="#{{entity.data.RequestId}}">T
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--  <br>
                                <a class="ReturnedMachines btn blue">Seçilenleri Kirala</a>
                                <a class="ReturnedMachines btn blue">Seçilenleri İade Al</a>
                                <a class="ReturnAndRent btn blue">Seçilenleri İade Al ve Kirala</a>
                                <br>
                                <br>--%>
                            </div>

                            <div class="tab-pane" id="tab_machinepark">
                                <a class="list-toggle-container">
                                    <div class="list-toggle done uppercase">
                                        <button type="button" class="btn btn-danger btnMachinepark" onclick="ToggleNewMachinepark();"><i class="fa fa-plus"></i>Yeni Kayıt Ekle</button>
                                    </div>
                                </a>
                                <div class="task-list panel-collapse collapse machinepark">
                                    <div class="row form" style="margin-bottom: 20px !important;">
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Makine Kategorisi</b></label>
                                            <asp:DropDownList ID="ddMpMachineparkType" runat="server" class="bs-select form-control select2" data-placeholder="Makine Türü Seçiniz" required title="Makine türü seçilmelidir">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Marka</b></label>
                                            <asp:DropDownList ID="ddMpMarks" runat="server" class="form-control todo-taskbody-tags select2" data-placeholder="Marka Seçiniz">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 select-editable form-group">

                                            <label><b>Model</b></label>
                                            <input class="form-control" id="txtMpModel" runat="server" type="text" name="Adet" />
                                        </div>

                                        <div class="col-md-4 col-sm-6 col-xs-12 select-editable form-group">
                                            <label><b>Seri No</b></label>
                                            <input class="form-control" id="txtMpSerialNo" runat="server" type="text" name="Seri No" onblur="QuantityOne();" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Yıl</b></label>
                                            <asp:DropDownList ID="ddMpYears" runat="server" class="form-control todo-taskbody-tags select2" data-placeholder="Seçiniz">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Edinme Şekli</b></label>
                                            <asp:DropDownList ID="ddMpRetireOrOwnered" runat="server" class="form-control todo-taskbody-tags select2" data-placeholder="Seçiniz">
                                                <asp:ListItem Value="1">Kiralık</asp:ListItem>
                                                <asp:ListItem Value="2">Özmal</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Satın Alma Tarihi</b></label>
                                            <input class="form-control" id="dateSaleDate" runat="server" type="date" name="saleDate" data-placeholder="Satın alma Tarihi" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Elden Çıkarma Tarihi (Planlanan)</b></label>
                                            <input class="form-control" id="datePlanedRelease" runat="server" type="date" name="datePlanedRelease" data-placeholder="Elden Çıkartma (Planlanan)" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Elden Çıkarma Tarihi</b></label>
                                            <input class="form-control" id="dateRelease" runat="server" type="date" name="dateRelease" data-placeholder="Elden Çıkartma" />
                                        </div>

                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b></b></label>
                                            <input class="form-control" min="1" id="txtMpMachineparkCount" runat="server" type="number" name="Adet" required title="Adet girilmesi zorunludur" />
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Konum</b></label>
                                            <asp:DropDownList ID="ddMpMachineparkLocation" runat="server" class="form-control todo-taskbody-tags select2" data-placeholder="Konum Seçiniz">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 form-group">
                                        <a class="col-xs-3 btn btn-info recordAndClose RACMP1" onclick="MachineParkSaveAndClose(1);" style="margin-right: 10px !important;">Kaydet ve Kapat
                                        </a>
                                        <a class="col-xs-3 btn btn-success recordAndNew RACMP2" onclick="MachineParkSaveAndClose(2);" style="margin-right: 10px !important;">Kaydet ve Yeni Kayıt Aç
                                        </a>
                                    </div>
                                </div>
                                <div id="mpContent" runat="server"></div>
                            </div>

                            <!-- *******************************************END MACHINEPARK TAB********************************************************* -->

                            <!-- *******************************************start INTERVIEW interview  TAB  interviewtab INTERVIEWTAB********************************************************* -->
                            <div class="tab-pane" id="tab_Interview" ng-controller="CustomerInterviewController">
                            
                                <pre>{{CustomerInterview}}</pre>
                           
                                <message-panel id="MessgePanelInterview" ng-model="CustomerInterview"></message-panel>
                                <div class="task-list panel-collapse collapse" id ="InterviewForm">
                                    <div class="row form">
                                            <div class="btn-toolbar margin-bottom-10; margin-top-10;" style="margin-left: 9px !important;">
                                                <button type="button" class="btn btn-default" tooltip="Kaydet" ng-disable="IsSaveBtnActive" ng-click="saveForm()">
                                                    <i class="fa fa-save"></i>
                                                </button>
                                                <button type="button" class="btn btn-default" ng-click="ToggInterviewers()" tooltip="Kapat">
                                                    <i class="fa fa-times"></i>
                                                </button>
                                            </div>
                                        <input type="hidden" id="hdnCustomerInterviewId" value="0" />
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Görüşen</b></label>
                                            <div search-select ng-model="InterviewUserModel" options="InterviewUser_SelectOptions" ng-sync-value="CustomerInterview.UserId"></div>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Yetkili</b></label>
                                            <div search-select ng-model="InterviewAuthenticatorModel" options="InterviewAuthenticator_SelectOptions" ng-sync-value="CustomerInterview.AuthenticatorId"></div>
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Görüşme Tipi</b></label>
                                            <div search-select ng-model="InterviewModel" options="Interview_SelectOptions" ng-sync-value="CustomerInterview.InterviewTypeId"></div>

                                        </div>

                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Görüşme Tarihi</b></label>
                                            <input class="input-group form-control form-control-inline date date-picker" size="16" type="date" ng-model="CustomerInterview.InterviewDate">
                                        </div>
                                        <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Önem</b></label>
                                            <div search-select ng-model="InterviewImportantModel" options="InterviewImportant_SelectOptions" ng-sync-value="CustomerInterview.ImportantId"></div>

                                        </div>
                                           <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                            <label><b>Tamamlandı</b></label>
                                            <input type="checkbox" ng-model="CustomerInterview.Interviewed" />

                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 form-group">
                                            <label><b>Not</b></label>
                                            <%--   $.trim(tinymce.get('textNote').getContent());--%>
                                            <input type="text" id="textNote" name="textNote"  class="tinymce" value="" >{{CustomerInterview.Note}}</input>
                                            
                                            <%--<asp:TextBox ID="textNote" ClientIDMode="Static" Rows="30" CssClass="tinymce" runat="server" name="textNote" TextMode="MultiLine"></asp:TextBox>--%>
                                        </div>

                                    </div>
                                </div>
                                
                                     <div id="customerInterviewGrid" dx-data-grid="GridService.SetOptions(customerInterviewGrid)" dx-item-alias="entity">                                         
                                     </div>

                            </div>
                            <!-- *******************************************end INTERVIEW interview  TAB********************************************************* -->

                            <!-- *******************************************REQUESTTAB********************************************************* -->
                            <div class="tab-pane" id="tab_Request" ng-controller="CustomerRequestController">
                                <div id="RequestForm">
                                    <message-panel id="MessgePanelRequest" ng-model="RequestResult"></message-panel>

                                    <%--<div class="btn-toolbar margin-bottom-10 margin-top-10">
                                        <div class="btn-group">

                                            <button type="button" class="btn btn-default" ng-if="!IsEditPanelActive" ng-click="SetEditPanel(!IsEditPanelActive); btnRequestNew_Click();" tooltip="Ekle">
                                                <i class="fa fa-plus"></i>
                                            </button>
                                            <button type="button" class="btn btn-default" ng-if="IsEditPanelActive" ng-click="SetEditPanel(!IsEditPanelActive); btnRequestNew_Click();" tooltip="Kapat">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                            <button type="button" class="btn btn-default" tooltip="Kaydet" ng-disable="IsEditPanelActive" ng-click="RequestSave()">
                                                <i class="fa fa-save"></i>
                                            </button>
                                            <button type="button" class="btn btn-default" tooltip="Sil" ng-disable="IsEditPanelActive" ng-click="RequestDelete()">
                                                <i class="fa fa-trash"></i>
                                            </button>
                                        </div>

                                        <div class="btn-group">
                                            <button type="button" class="btn btn-default" tooltip="Yenile" ng-click="scope.btnRefresh_Click">
                                                <i class="fa fa-refresh"></i>
                                            </button>
                                        </div>
                                    </div>--%>
                                    <div class="task-list panel-collapse collapse request">
                                        <div class="row form">
                                            <div class="btn-toolbar margin-bottom-10; margin-top-10;" style="margin-left: 9px !important;">
                                                <button type="button" class="btn btn-default" tooltip="Kaydet" ng-disable="IsSaveBtnActive" ng-click="RequestSave()">
                                                    <i class="fa fa-save"></i>
                                                </button>
                                                <button type="button" class="btn btn-default" ng-click="ToggRequesters_Everyclose();" tooltip="Kapat">
                                                    <i class="fa fa-times"></i>
                                                </button>
                                            </div>
                                            <input type="hidden" id="hdnCustomerRequestId" value="0" />
                                            <div class="task-list panel-collapse collapse" id="RequestPanel" style="display: block;">

                                                <div class="form">
                                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Satış Temsilcisi</b></label>
                                                                <div>
                                                                    <div search-select ng-model="SalesmanModel" options="SalesmanList_SelectOptions" ng-sync-value="CustomerRequest.SalesmanId"></div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Kategori</b></label>

                                                                <div id="test" search-select ng-model="MpCategory" options="MPCategory_SelectOptions" ng-sync-value="CustomerRequest.CategoryId"></div>
                                                            </div>

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Marka</b></label>
                                                                <div>
                                                                    <div search-select ng-model="MpMark" options="MpMark_SelectOptions" ng-sync-value="CustomerRequest.MarkId"></div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Model</b></label>
                                                                <div>
                                                                    <div search-select ng-model="MpModel" options="MpModel_SelectOptions" ng-sync-value="CustomerRequest.ModelId"></div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Adet</b></label>
                                                                <div>
                                                                    <input ng-readonly="QuantityIsReadonly" class="input-group form-control form-control-inline" ng-model="CustomerRequest.Quantity" type="number" />
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Satış Tipi</b></label>
                                                                <div>
                                                                    <div search-select ng-model="SalesType" options="SalesType_SelectOptions" ng-sync-value="CustomerRequest.SalesType"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                                        <div class="row">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Talep Tarihi</b></label>
                                                                <div>
                                                                    <input class="input-group form-control form-control-inline date date-picker" size="16" type="date" ng-model="CustomerRequest.RequestDate">
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Satın alma Tarihi</b></label>
                                                                <div>
                                                                    <input class="input-group form-control form-control-inline date date-picker" size="16" type="date" ng-model="CustomerRequest.EstimatedBuyDate">
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Kira Süresi</b></label>
                                                                <div>
                                                                    <input ng-readonly="!RentModelDisableReadonly" class="input-group form-control form-control-inline" ng-model="CustomerRequest.UseDuration" type="number" />
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Kira Süresi (Birim)</b></label>
                                                                <div>
                                                                    <div search-select ng-enabled="RentModelDisableReadonly" ng-model="UseDurationUnit" options="UseDurationUnit_SelectOptions" ng-sync-value="CustomerRequest.UseDurationUnit"></div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Talebi Bulan</b></label>
                                                                <div>
                                                                    <div search-select ng-model="OwnerModel" options="Owner_SelectOptions" ng-sync-value="CustomerRequest.OwnerId"></div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Talep Kaynağı</b></label>
                                                                <div>
                                                                    <div search-select ng-model="ChannelModel" options="Channel_SelectOptions" ng-sync-value="CustomerRequest.ChannelId"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                                        <div class="row">

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Kondisyon</b></label>
                                                                <div>
                                                                    <div search-select ng-model="ConditionType" options="ConditionTypeList_SelectOptions" ng-sync-value="CustomerRequest.ConditionType"></div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Aylık Çalışma Saati</b></label>
                                                                <div>
                                                                    <input class="input-group form-control form-control-inline" ng-model="CustomerRequest.MonthlyWorkingHours" type="number" />
                                                                    <%-- <span class="help-inline font-red">Kaç adet yazınız.</span>--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Not</b></label>
                                                                <div>
                                                                    <textarea class="form-control" id="areaNote"
                                                                        ng-model="CustomerRequest.Note" rows="9"></textarea>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <br>
                                                                <label><b>Sonuç</b></label>
                                                                <div>
                                                                    <label>{{CustomerRequest.ResultText}}</label>
                                                                    <%--<div search-select ng-model="ResultType" options="ResultTypeList_SelectOptions" ng-sync-value="CustomerRequest.ResultType"></div>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="margin-bottom: 30px;"></div>
                                </div>
                                <% var dicSalesType = new DictonaryStaticList().dicSalesType;
                                    var dicConType = new DictonaryStaticList().dicConditionType; %>
                                <div id="RequestTabPanel">
                                    <div class="tabbable tabbable-tabdrop">
                                        <ul class="nav nav-tabs">
                                            <li class="active">
                                                <a href="#tabOpenRequest" data-toggle="tab" aria-expanded="true" ng-click="RequestOpencloseTab_Click('open')">Açık Talepler</a>
                                            </li>
                                            <li class="">
                                                <a href="#tabCloseRequest" data-toggle="tab" aria-expanded="false" ng-click="RequestOpencloseTab_Click('close')">Kapalı Talepler</a>
                                            </li>
                                        </ul>
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="tabOpenRequest">
                                                <%-- <p>Açık Talepler </p>--%>
                                                <div id="customerRequestGrid_Open" dx-data-grid="GridService.SetOptions(dataGridOptions_Open)" dx-item-alias="entity">
                                                    <div data-options="dxTemplate:{ name:'cellTemplateSalesType' }">
                                                        <span title="{{entity.data.SalesType}}" class="label-danger badge tooltips" ng-if="entity.data.SalesType == '<%= dicSalesType[(int)eSalesType.Hepsi] %>'">S/K</span>
                                                        <span title="{{entity.data.SalesType}}" class="label-danger badge tooltips" ng-if="entity.data.SalesType == '<%= dicSalesType[(int)eSalesType.Kiralik] %>'">K</span>
                                                        <span title="{{entity.data.SalesType}}" class="label-danger badge tooltips" ng-if="entity.data.SalesType == '<%= dicSalesType[(int)eSalesType.Satilik] %>'">S</span>
                                                    </div>
                                                    <div data-options="dxTemplate:{ name:'cellTemplateConditionType' }">
                                                        <span title="{{entity.data.ConditionType}}" class="label-danger badge tooltips" ng-if="entity.data.ConditionType == '<%= dicConType[(int)eConditionType.Sifir] %>'">S</span>
                                                        <span title="{{entity.data.ConditionType}}" class="label-danger badge tooltips" ng-if="entity.data.ConditionType == '<%= dicConType[(int)eConditionType.Hepsi] %>'">S/2</span>
                                                        <span title="{{entity.data.ConditionType}}" class="label-danger badge tooltips" ng-if="entity.data.ConditionType == '<%= dicConType[(int)eConditionType.IkinciEl] %>'">2</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="tabCloseRequest">
                                                <%-- <p>Kapalı Talepler </p>--%>
                                                <div id="customerRequestGrid_Close" dx-data-grid="GridService.SetOptions(dataGridOptions_Close)" dx-item-alias="entity">
                                                    <div data-options="dxTemplate:{ name:'cellTemplateSalesType' }">
                                                        <span title="{{entity.data.SalesType}}" class="label-danger badge tooltips" ng-if="entity.data.SalesType == '<%= dicSalesType[(int)eSalesType.Hepsi] %>'">S/K</span>
                                                        <span title="{{entity.data.SalesType}}" class="label-danger badge tooltips" ng-if="entity.data.SalesType == '<%= dicSalesType[(int)eSalesType.Kiralik] %>'">K</span>
                                                        <span title="{{entity.data.SalesType}}" class="label-danger badge tooltips" ng-if="entity.data.SalesType == '<%= dicSalesType[(int)eSalesType.Satilik] %>'">S</span>
                                                    </div>
                                                    <div data-options="dxTemplate:{ name:'cellTemplateConditionType' }">
                                                        <span title="{{entity.data.ConditionType}}" class="label-danger badge tooltips" ng-if="entity.data.ConditionType == '<%= dicConType[(int)eConditionType.Sifir] %>'">S</span>
                                                        <span title="{{entity.data.ConditionType}}" class="label-danger badge tooltips" ng-if="entity.data.ConditionType == '<%= dicConType[(int)eConditionType.Hepsi] %>'">S/2</span>
                                                        <span title="{{entity.data.ConditionType}}" class="label-danger badge tooltips" ng-if="entity.data.ConditionType == '<%= dicConType[(int)eConditionType.IkinciEl] %>'">2</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="PreRequestMachineParkPanel"></div>
                                <div id="RequestMachineParkPanel" ng-show="MachineParkGridPanelShow">

                                    <b>Talebin Makinesi</b>
                                    <div class="table-scrollable">
                                        <message-panel id="MesPanReqMacPark" ng-model="RequestMpWrapper"></message-panel>
                                        <div id="customerRequestMachineParkGrid" dx-data-grid="GridService.SetOptions(dataGridOptionMachinePark)" dx-item-alias="entity">
                                            <div data-options="dxTemplate:{ name:'cellTemplateMachineParkSelect' }">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Button Style="display: none;" ID="btnSaveGeneral" runat="server" Text="SaveGeneral" OnClick="btnSaveGeneral_Click" />
        <asp:Button Style="display: none;" ID="btnNewCustomer" runat="server" Text="SaveGeneral" OnClick="btnNewCustomer_Click" />
        <asp:Button Style="display: none;" ID="btnMachineparkSaveAndClose" runat="server" Text="MachineparkSaveAndClose" OnClick="btnMachineparkSaveAndClose_Click" />

        <%--popup start--%>
        <div id="modalSalesmanAdd" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Satışcı ekleme</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row form">
                            <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                <label><b>Operasyon Tipi</b></label>
                                <asp:DropDownList ID="pddSalesmanTypesPopup" popupelement="true" runat="server" class="form-control" data-placeholder="Satıcı Türü Seçiniz" required title="Operasyon Seçmelisiniz...">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                                <label><b>Satış Mühendisi</b></label>
                                <asp:DropDownList ID="pddSalesEngineeersPopup" runat="server" class="form-control" data-placeholder="Satış Mühendisi Seçiniz" required title="Satıcı Seçmelisiniz...">
                                </asp:DropDownList>
                            </div>
                            <div class="clearfix"></div>

                            <div class="col-xs-12 form-group">
                                <a class="col-xs-3 btn btn-info recordAndClose RACSE1" onclick="salesmanPopupInsert();" style="margin-right: 10px !important;">Kaydet ve Kapat
                                </a>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <%-- <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <style type="text/css">
        .fixedHeight {
            color: red;
            font-size: 12px;
            max-height: 200px;
            max-width: 550px;
            margin-bottom: 10px;
            overflow-x: auto;
            overflow-y: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script src="../../Scripts/tinymce/tinymce.min.js"></script>
    <script src="../../Scripts/Misc/bootstrap-tabdrop.js"></script>
    <script type="text/javascript">

        function AddDefaultLocations(map) {

            var iconBase = 'https://maps.google.com/mapfiles/kml/shapes/';

            function addMarker(feature, map) {
                var marker = new google.maps.Marker({
                    position: feature.position,
                    map: map,
                    dragable: true
                });
            }
            var customerId = parseInt($("input[id*='hdnCustomerId']").val());

            if (!customerId > 0)
                return;

            var features = [];

            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/GetLocationByCustomerId',
                data: "{customerId:" + customerId + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    for (var i = 0; i < data.d.length; i++) {
                        features.push({
                            position: new google.maps.LatLng(data.d[i].Longitude, data.d[i].Latitude),
                            type: 'info'
                        });
                    }

                    for (var i = 0, feature; feature = features[i]; i++) {
                        addMarker(feature, map);
                    }
                },
                error: function (data) {
                    alert(data.d);
                }
            });
        }

        var auMap = null;
        function initAutocomplete() {

            var myLatlng = { lat: 40.890786, lng: 29.3512593 };
            var coordinates = '';
            var isUnd = $("input[id*='hdnLongLatChain']").val();// $("#ContentPlaceHolder1_hdnLongLatChain").val();
            if (isUnd != undefined && isUnd != '') {
                coordinates = $("input[id*='hdnLongLatChain']").val().split("|");
                var cor12 = coordinates[0].split(",");
                myLatlng = { lat: parseFloat(cor12[0]), lng: parseFloat(cor12[1]) };
            }
            var map = {};
            if (document.getElementById('map')) {
                var map = new google.maps.Map(document.getElementById('map'), {
                    center: myLatlng,
                    zoom: 5,
                    mapTypeId: 'roadmap'
                });
                auMap = map;

                // Create the search box and link it to the UI element.

                var input = document.getElementById('pacInput');
                if (input != null) {
                    var i = document.createElement("input"); //input element, text
                    i.setAttribute('type', "text");
                    i.setAttribute('id', "pacinputInvisible");
                    i.setAttribute('class', "controls");
                    i.setAttribute('placeholder', "Lokasyon Ara");
                    i.setAttribute('style', 'z-index: 0;position: absolute;left: 113px;top: 0px;width: 150px;margin-top: 10px;');
                    input = i;
                    $("#pacInput").hide();
                }

                var searchBox = new google.maps.places.SearchBox(input);
                map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

                // Bias the SearchBox results towards current map's viewport.
                map.addListener('bounds_changed', function () {
                    searchBox.setBounds(map.getBounds());
                });

                var markers = [];
                // Listen for the event fired when the user selects a prediction and retrieve
                // more details for that place.
                searchBox.addListener('places_changed', function () {
                    var places = searchBox.getPlaces();

                    document.getElementById('ContentPlaceHolder1_txtLocLong').value = places[0].geometry.location.lng();
                    document.getElementById('ContentPlaceHolder1_txtLocLat').value = places[0].geometry.location.lat();
                    ll.lat = places[0].geometry.location.lat();
                    ll.lng = places[0].geometry.location.lng();
                    document.getElementById('btnaddmarker').click();

                    if (places.length == 0) {
                        return;
                    }

                    // Clear out the old markers.
                    markers.forEach(function (marker) {
                        marker.setMap(null);
                    });
                    markers = [];

                    // For each place, get the icon, name and location.
                    var bounds = new google.maps.LatLngBounds();
                    places.forEach(function (place) {
                        if (!place.geometry) {
                            console.log("Returned place contains no geometry");
                            return;
                        }
                        var icon = {
                            url: place.icon,
                            size: new google.maps.Size(71, 71),
                            origin: new google.maps.Point(0, 0),
                            anchor: new google.maps.Point(17, 34),
                            scaledSize: new google.maps.Size(25, 25)
                        };

                        // Create a marker for each place.
                        markers.push(new google.maps.Marker({
                            map: map,
                            icon: icon,
                            title: place.name,
                            position: place.geometry.location
                        }));

                        if (place.geometry.viewport) {
                            // Only geocodes have viewport.
                            bounds.union(place.geometry.viewport);
                        } else {
                            bounds.extend(place.geometry.location);
                        }
                    });
                    map.fitBounds(bounds);
                });

                AddDefaultLocations(map);
            }

        }

        var ll = { lat: 40.890786, lng: 29.3512593 };

        $(document).ready(function () {
            $("#pacInput").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });

            $('#btnaddmarker').on('click', function () {
                var image = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';
                marker = new google.maps.Marker({
                    position: ll,
                    title: 'Yeni Lokasyon',
                    draggable: true,
                    map: auMap,
                    icon: image
                });

                google.maps.event.addListener(marker, 'dragend', function (event) {
                    $("#<%=txtLocLong.ClientID%>").val(this.getPosition().lng());
                    $("#<%=txtLocLat.ClientID%>").val(this.getPosition().lat());
                });
            });
        })

        $(document).ready(function () {
            $(".headautocomplete").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/HaselSOAService.asmx/GetPossibleCustomers") %>',
                        data: "{ 'key': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                if (item.split('|').length == 10)
                                    item = 'NetSis _____  ' + item;
                                return {
                                    label: item.split('|')[0] + item.split('|')[1],
                                    val: item//.split('|')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            LogE("arama terim " + htmlEscape($("[id$='txtUnvan']").val()) + " " + response.responseText, "759 - customerdetail.aspx", "");
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    var url = window.location.href;
                    var lineItems = ui.item.val.split('|');
                    var urlCallerIdentifier = lineItems[1];
                    if (url.indexOf('?') > -1) {
                        var index = url.indexOf('?');
                        url = url.substring(0, index);
                    }
                    if (url.indexOf('#') > -1) {
                        var index = url.indexOf('?');
                        url = url.substring(0, index);
                    }
                    if (urlCallerIdentifier.indexOf('-') <= 0) {
                        url += '?CId=' + urlCallerIdentifier;
                        window.location.href = url;
                    }
                    else {
                        sessionStorage.setItem('firm', lineItems[0].toString().replace("NetSis _____", ""));
                        sessionStorage.setItem('identifier', lineItems[1].toString());
                        sessionStorage.setItem('code', lineItems[1].toString());
                        sessionStorage.setItem('address', lineItems[2].toString());
                        sessionStorage.setItem('city', lineItems[3].toString());
                        sessionStorage.setItem('region', lineItems[4].toString());
                        sessionStorage.setItem('tel', lineItems[5].toString());
                        sessionStorage.setItem('fax', lineItems[7].toString());
                        sessionStorage.setItem('VD', lineItems[8].toString());
                        sessionStorage.setItem('VN', lineItems[9].toString());
                        window.location.href = url + "?key=firstLoad";
                    }
                }

            });

            $("#ContentPlaceHolder1_txtUnvan").autocomplete("widget").addClass("fixedHeight");

            $("#showMyMap").click(function () {
                $("#MyMap").slideToggle("slow");
                setTimeout(function () { initAutocomplete(); }, 200);
                $('body, html').animate({ scrollTop: 280 }, 800);
            });
            <% if (Request.QueryString["cid"] == null /*&& Request.Url.ToString().IndexOf("localhost") == -1*/)
        { %>
            $("a[data-toggle='tab'").prop('disabled', true);
            $("a[data-toggle='tab'").each(function () {
                $(this).prop('data-href', $(this).attr('href')); // hold you original href
                $(this).attr('href', '#'); // clear href
            });
            $("a[data-toggle='tab'").addClass('disabled-link');
             <% } %>
        });

        var getUrlParameter = function getUrlParameter(sParam) {
            var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : sParameterName[1];
                }
            }
        };

        function SetGoogleMaptoScreen() {
            event.preventDefault();
        }

        function SetGoogleMaptoScreenLitle() {
            if (document.getElementById('map').style.width == '320px') {
                document.getElementById('map').style.width = ($(window).width() - 120).toString() + 'px';
                document.getElementById('map').style.height = '400px';
                setTimeout(function () { initAutocomplete(); }, 200);
                event.preventDefault();
            }
            else {
                document.getElementById('map').style.width = '320px';
                document.getElementById('map').style.height = '40px';
                setTimeout(function () { initAutocomplete(); }, 200);
                event.preventDefault();
            }
        }

        /****runtime generator code begin**/

        function GetColor(item) {
            var colorList = <% = this.GetColorJson() %>;
            var itemResul = colorList[item];
            if (itemResul !== undefined) {
                return itemResul;
            }
            return colorList["Diğer"];
        }

        var cw = <% = this.GetWrapper() %>;

        <%=this.FlagVisibilityForJs()%>

        /****runtime generator code end**/
    </script>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?libraries=places&key=AIzaSyD9I-2EPgPI_DikhQrvqWYPzzR2mUoDVyU&callback=initAutocomplete">
    </script>
    <script>
        $(document).ready(function () {

            var cid = getQueryStringByName("cid");
            if (!isEmptyLocal(cid)) {

                if ($("[id$='spSaleEngineerCount']").length > 0) {
                    var item = $("[id$='spSaleEngineerCount']")[0].innerHTML;
                    if (parseInt(item) === 0) {
                        $("#modalSalesmanAdd").modal({
                            backdrop: 'static',
                            keyboard: true
                        });
                    }
                }
            }

        });
    </script>
    <link href="../../Content/Css/CustomerFix.css?v=<%= Helper.StaticGuid %>" rel="stylesheet" />
    <script src="../../Scripts/Customer.js?v=<%= Helper.StaticGuid %>"></script>
    <script src="../../Scripts/_Services/CustomerService.js?v=<%= Helper.StaticGuid %>"></script>
    <script src="../../Scripts/_Services/MachineparkService.js?v=<%= Helper.StaticGuid %>"></script>
    <script src="../../Scripts/_Services/InterviewService.js?v=<%= Helper.StaticGuid %>"></script>
    <script src="../../Scripts/_Controllers/CustomerInterviewController.js?v=<%= Helper.StaticGuid %>"></script>
    <script src="../../Scripts/_Controllers/CustomerControllerTmp.js?v=<%= Helper.StaticGuid %>"></script>
    <script src="../../Scripts/_Controllers/CustomerController.js?v=<%= Helper.StaticGuid %>"></script>
</asp:Content>
