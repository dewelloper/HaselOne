<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserGroupAuthenticationDetail.aspx.cs" Inherits="HaselOne.UserGroupAuthenticationDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdnUserGroupRightId" runat="server" />
    <asp:HiddenField ID="hdnUserGroupDetailId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kullanıcı Grubu</b></label>
            <asp:DropDownList ID="ddUserGroupRightsGroups" runat="server" class="form-control" data-placeholder="Kullanıcı Grubu Seçiniz">
            </asp:DropDownList>
        </div>

        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Departman</b></label>
            <asp:DropDownList ID="ddDepartments" runat="server" class="form-control" data-placeholder="Departman Seçiniz">
            </asp:DropDownList>
        </div>

        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Rol</b></label>
            <asp:DropDownList ID="ddroles" runat="server" class="form-control" data-placeholder="Rol Seçiniz">
            </asp:DropDownList>
        </div>

        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Modül</b></label>
            <asp:DropDownList ID="ddUserGroupRightsModuls" runat="server" class="form-control" data-placeholder="Modül Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Ekleme</b></label>
            <asp:CheckBox ID="chkUserGroupRightsInsert" runat="server" class="form-control" data-placeholder="Ekleme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Güncelleme</b></label>
            <asp:CheckBox ID="chkUserGroupRightsEdit" runat="server" class="form-control" data-placeholder="Güncelleme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Görme</b></label>
            <asp:CheckBox ID="chkUserGroupRightsShow" runat="server" class="form-control" data-placeholder="Göreme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Silme</b></label>
            <asp:CheckBox ID="chkUserGroupRightsDelete" runat="server" class="form-control" data-placeholder="Silme"></asp:CheckBox>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Aktif-Pasif</b></label>
            <asp:RadioButtonList ID="rbUserGroupRightsActivePassive" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Aktif</asp:ListItem>
                <asp:ListItem Value="2">Pasif</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Silinmiş</b></label>
            <asp:RadioButtonList ID="rbUserGroupRightsDelete" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Evet</asp:ListItem>
                <asp:ListItem Value="2">Hayır</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-xs-12">
            <br />
            <a class="btn btn-info recordAndClose" onclick="UpdateUserGroupRights(0);" id="btnUserUpdate" runat="server">Güncelle
            </a>
            <a class="btn btn-danger recordAndClose" onclick="UpdateUserGroupRights(1);" id="btnUserInsert" runat="server">Kaydet
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function UpdateUserGroupRights(workingmode) {
            var userGroupRightId = 0;
            if ($("#<%=hdnUserGroupRightId.ClientID%>").val() != "")
                userGroupRightId = parseInt($("#<%=hdnUserGroupRightId.ClientID%>").val());
            var userGroupId = parseInt($("#<%=ddUserGroupRightsGroups.ClientID%>" + " option:selected").val());
            var modulId = parseInt($("#<%=ddUserGroupRightsModuls.ClientID%>" + " option:selected").val());

            var userGroupDetailId = 0;
            if ($("#<%=hdnUserGroupDetailId.ClientID%>").val() != "")
                userGroupDetailId = parseInt($("#<%=hdnUserGroupDetailId.ClientID%>").val());

            var departmentId = parseInt($("#<%=ddDepartments.ClientID%>" + " option:selected").val());
            var roleId = parseInt($("#<%=ddroles.ClientID%>" + " option:selected").val());

            var uinsert = $("#<%=chkUserGroupRightsInsert.ClientID%>").is(':checked');
            var uedit = $("#<%=chkUserGroupRightsEdit.ClientID%>").is(':checked');
            var ushow = $("#<%=chkUserGroupRightsShow.ClientID%>").is(':checked');
            var udelete = $("#<%=chkUserGroupRightsDelete.ClientID%>").is(':checked');
            var activity = $("#<%=rbUserGroupRightsActivePassive.ClientID%>");
            var isActive = activity.find('input[type=radio]:checked').val() == 1 ? true : false;
            var delety = $("#<%=rbUserGroupRightsDelete.ClientID%>");
            var isdeleted = delety.find('input[type=radio]:checked').val() == 1 ? true : false;
            var vm = parseInt(workingmode);
            var uid = parseInt($("#hdnUserId").val());
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/SaveUserGroupRight',
                data: "{userGroupRightId:" + userGroupRightId + ",userGroupId:" + userGroupId + ",modulId:" + modulId + ",uinsert:" + uinsert + ",uedit:" + uedit + ",ushow:" + ushow + ",udelete:" + udelete + ",isActive:" + isActive + ",isdeleted:" + isdeleted + ",workingmode:" + vm + ",uid:" + uid + ",departmentId:" + departmentId + ",roleId:" + roleId + ",userGroupDetailId=" + userGroupDetailId + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d == true) {
                        <%Session["UkChanged"] = "1";%>;
                        alert("Kullanıcı Grubu başarı ile güncellenmiştir...!");
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