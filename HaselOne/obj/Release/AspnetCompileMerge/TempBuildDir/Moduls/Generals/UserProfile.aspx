<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="HaselOne.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="portlet light profile-sidebar-portlet ">
        <!-- SIDEBAR USERPIC -->
        <div class="profile-userpic" style="max-width: 150px; max-height: 200px;">
            <img id="dashimgBigAvatar1" runat="server" src="../../Content/Images/media/profile/profile_user.jpg" class="img-responsive" alt="">
        </div>
        <!-- END SIDEBAR USERPIC -->
        <!-- SIDEBAR USER TITLE -->
        <div class="profile-usertitle">
            <div class="profile-usertitle-job" id="dashposition1" runat="server">Satış Müdürü </div>
            <div class="profile-usertitle-job" id="dashlocation1" runat="server">Orhanlı </div>
            <div class="profile-usertitle-job"><a href="" id="dashemail1" runat="server" class="lowercase">tugrul.caglar@hasel.com</a> </div>
            <div class="profile-usertitle-job" id="dashphone1" runat="server"></div>
            <div class="profile-usertitle-job" id="dashmobile1" runat="server"> </div>
        </div>
        <!-- END SIDEBAR USER TITLE -->
        <br>
        <br>
        <br>
        <div class="col-xs-12">
            <a class="btn btn-info recordAndClose" onserverclick="btnUpdateUserProfile_ServerClick" id="btnUpdateUserProfile1" runat="server">Bilgilerimi Güncelle
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server"></asp:Content>