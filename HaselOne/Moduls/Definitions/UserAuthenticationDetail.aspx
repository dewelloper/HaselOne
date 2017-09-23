<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAuthenticationDetail.aspx.cs" Inherits="HaselOne.UserAuthenticationDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdnUserRightId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kullanıcılar</b></label>
            <asp:DropDownList ID="ddUsers" runat="server" class="form-control" data-placeholder="Kullanıcı Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Modül</b></label>
            <asp:DropDownList ID="ddUserRightsModuls" runat="server" class="form-control" data-placeholder="Modül Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Ekleme</b></label>
            <asp:CheckBox ID="chkUserRightsInsert" runat="server" class="form-control" data-placeholder="Ekleme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Güncelleme</b></label>
            <asp:CheckBox ID="chkUserRightsEdit" runat="server" class="form-control" data-placeholder="Güncelleme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Görme</b></label>
            <asp:CheckBox ID="chkUserRightsShow" runat="server" class="form-control" data-placeholder="Göreme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Silme</b></label>
            <asp:CheckBox ID="chkUserRightsDelete" runat="server" class="form-control" data-placeholder="Silme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Aktif-Pasif</b></label>
            <asp:RadioButtonList ID="rbUserRightsActivePassive" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Aktif</asp:ListItem>
                <asp:ListItem Value="2">Pasif</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Silinmiş</b></label>
            <asp:RadioButtonList ID="rbUserRightsDelete" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Evet</asp:ListItem>
                <asp:ListItem Value="2">Hayır</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-xs-12">
            <br />
            <a class="btn btn-info recordAndClose" onclick="UpdateUserRights(0);" id="btnUserUpdate" runat="server">Güncelle
            </a>
            <a class="btn btn-danger recordAndClose" onclick="UpdateUserRights(1);" id="btnUserInsert" runat="server">Kaydet
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function UpdateUserRights(workingmode) {
            var userRightId = 0;
            if ($("#<%=hdnUserRightId.ClientID%>").val() != "")
                userRightId = parseInt($("#<%=hdnUserRightId.ClientID%>").val());
            var userId = parseInt($("#<%=ddUsers.ClientID%>" + " option:selected").val());
            var modulId = parseInt($("#<%=ddUserRightsModuls.ClientID%>" + " option:selected").val());
            var uinsert = $("#<%=chkUserRightsInsert.ClientID%>").is(':checked');
            var uedit = $("#<%=chkUserRightsEdit.ClientID%>").is(':checked');
            var ushow = $("#<%=chkUserRightsShow.ClientID%>").is(':checked');
            var udelete = $("#<%=chkUserRightsDelete.ClientID%>").is(':checked');
            var activity = $("#<%=rbUserRightsActivePassive.ClientID%>");
            var isActive = $('' + activity.selector + '').find('input[type=radio]:checked').val() == 1 ? true : false;
            var delety = $("#<%=rbUserRightsActivePassive.ClientID%>");
            var isdeleted = $('' + delety.selector + '').find('input[type=radio]:checked').val() == 1 ? false : true;
            var vm = parseInt(workingmode);
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/SaveUserRight',
                data: "{userRightId:" + userRightId + ",userId:" + userId + ",modulId:" + modulId + ",uinsert:" + uinsert + ",uedit:" + uedit + ",ushow:" + ushow + ",udelete:" + udelete + ",isActive:" + isActive + ",isdeleted:" + isdeleted + ",workingmode:" + vm + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d == true) {
                        alert("Kullanıcı Yetkisi başarı ile güncellenmiştir...!");
                        <%Session["UkChanged"] = "1";%>;
                    }
                    else alert(data.d);
                },
                error: function (data) {
                    alert(data.d)
                }
            });
        }
    </script>
</asp:Content>