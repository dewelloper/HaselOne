﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserGroupDetail.aspx.cs" Inherits="HaselOne.UserGroupDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdnUserGroupId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kullanıcı Grup Adı</b></label>
            <input class="form-control controls" type="text" id="txtUserGroupName" name="userName" runat="server" placeholder="Ad Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Aktif-Pasif</b></label>
            <asp:RadioButtonList ID="rbUserGroupActivePassive" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Aktif</asp:ListItem>
                <asp:ListItem Value="0">Pasif</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="col-xs-12">
            <br />
            <a class="btn btn-info recordAndClose" onclick="UpdateUser(0);" id="btnUserUpdate" runat="server">Güncelle
            </a>
            <a class="btn btn-danger recordAndClose" onclick="UpdateUser(1);" id="btnUserInsert" runat="server">Kaydet
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function UpdateUser(workingmode) {
            var userGroupId = 0;
            if ($("#<%=hdnUserGroupId.ClientID%>").val() != "")
                userGroupId = parseInt($("#<%=hdnUserGroupId.ClientID%>").val());
            var name = $("#<%=txtUserGroupName.ClientID%>").val();
            var aktivepassive = $("#<%=rbUserGroupActivePassive.ClientID%>");
            var aktivepassivemi = aktivepassive.find('input[type=radio]:checked').val() == 1 ? true : false;
            var vm = parseInt(workingmode);
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/SaveUserGroup',
                data: "{userGroupId:" + userGroupId + ",userGroupName:\"" + name + "\",aktivepassivemi:" + aktivepassivemi + ",workingmode:" + vm + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d == true)
                        alert("Kullanıcı Grubu başarı ile güncellenmiştir...!");
                    else alert(data.d);
                },
                error: function (data) {
                    alert(data.d)
                }
            });
        }
    </script>
</asp:Content>