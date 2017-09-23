<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserProfileChange.aspx.cs" Inherits="HaselOne.UserProfileChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdnUserId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label id="knlow" runat="server"></label>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Ad</b></label>
            <input class="form-control controls hero" type="text" id="txtUserName" name="userName" runat="server" placeholder="Ad Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Soyad</b></label>
            <input class="form-control hero" type="text" id="txtUserSurname" runat="server" placeholder="Soyad Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Kullanıcı Adı</b></label>
            <input class="form-control" type="text" id="txtUserUserName" runat="server" placeholder="Kullanıcı Adı Yazınız" maxlength="50" />
        </div>

        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Email</b></label>
            <input class="form-control" type="text" id="txtUserEmail" runat="server" placeholder="Email Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Kullanıcı Şifresi</b></label>
            <input class="form-control" type="text" id="txtUserPassword" runat="server" placeholder="Kullanıcı Şifresi" maxlength="50" />
        </div>

        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Gsm</b></label>
            <input class="form-control" type="text" id="txtUserGsm" runat="server" placeholder="Gsm Yazınız" maxlength="50" />
        </div>

        <div class="col-md-9 col-sm-9 col-xs-12">
            <br />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUpload" />
                </Triggers>
                <ContentTemplate>
                    <label class="col-md-3 col-sm-3 col-xs-3"><b>Profil Resmi için</b></label>
                    <asp:FileUpload ID="FileUpload1" runat="server" class="col-md-3 col-sm-3 col-xs-3" />
                    <asp:Button ID="btnUpload" runat="server" Text="Resmi Yükle" OnClick="btnUpload_Click" class="col-md-3 col-sm-3 col-xs-3" />
                    <label id="lblmsg" runat="server" class="col-md-3 col-sm-3 col-xs-3"></label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
    </div>
    <br />
    <br />
    <div class="col-xs-12">
        <a class="btn btn-info recordAndClose" onserverclick="btnUserUpdate_ServerClick" id="btnUserUpdate" runat="server">Güncelle
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server"></asp:Content>