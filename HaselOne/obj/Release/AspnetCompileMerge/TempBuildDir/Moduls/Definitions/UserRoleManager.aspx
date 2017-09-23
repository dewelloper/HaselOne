<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserRoleManager.aspx.cs" Inherits="HaselOne.UserRoleManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdnUserId" runat="server" />
    <div class="col-md-12 col-sm-12 col-xs-12">
        <label><b id="userLabel" runat="server"></b></label>
    </div>
    <div class="col-md-8 col-sm-8 col-xs-8">
        <br />
        <label><b>Tanımlı Roller</b></label>
        <asp:DropDownList ID="ddRelatedRoles" runat="server" class="form-control" data-placeholder="Rol Seçiniz">
        </asp:DropDownList>
    </div>
    <div class="col-md-4 col-sm-4 col-xs-4">
        <br />
        <label><b>Bölgeler</b></label>
        <asp:DropDownList ID="ddAreas" runat="server" class="form-control" data-placeholder="Bölge Seçiniz">
        </asp:DropDownList>
    </div>
    <div class="col-md-2 col-sm-2 col-xs-2">
        <br />
        <input type="button" id="btnRoleInsert" onclick="AddRole();" class="form-control" value="Ekle" />
    </div>
    <div class="col-md-2 col-sm-2 col-xs-2">
        <br />
        <input type="button" id="btnRoleRemove" onclick="RemoveRole();" class="form-control" value="Çıkar" />
    </div>
    <div class="col-md-12 col-sm-12 col-xs-12">
        <br />
        <label><b>Atanmış Roller</b></label>
        <asp:ListBox ID="libRoles" runat="server" class="form-control" data-placeholder="Roller"></asp:ListBox>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function AddRole() {
            var userId = 0;
            if ($("#<%=hdnUserId.ClientID%>").val() != "")
                 userId = parseInt($("#<%=hdnUserId.ClientID%>").val());
             var role = $("#<%=ddRelatedRoles.ClientID%>").val();
             var area = $("#<%=ddAreas.ClientID%>").val();
             $.ajax({
                 type: "POST",
                 url: '/HaselSOAService.asmx/AddRole',
                 data: "{role:" + role + ",area:" + area + ",uid:" + userId + "}",
                 contentType: 'application/json; charset=utf-8',
                 dataType: 'json',
                 success: function (data) {
                     var x = document.getElementById('ContentPlaceHolder1_libRoles');
                     var option = document.createElement("option");
                     option.text = data.d.DgoId + ':' + data.d.FullName;
                     x.add(option);
                 }, error: function (data) {
                     alert(data.d);
                 }
             });
         }
         function RemoveRole() {
             var userId = 0;
             if ($("#<%=hdnUserId.ClientID%>").val() != "")
                userId = parseInt($("#<%=hdnUserId.ClientID%>").val());
            var role = parseInt(document.getElementById('ContentPlaceHolder1_libRoles').value);

            var selectedItem = document.getElementById('ContentPlaceHolder1_libRoles');
            var selectedVal = selectedItem.options[selectedItem.selectedIndex].innerHTML;
            var i1 = selectedVal.indexOf('(');
            var i2 = selectedVal.indexOf(')');
            i2 = i2 - (i1 + 1);
            var val = selectedVal.substr(i1 + 1, i2);

            var area = parseInt(val);
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/RemoveRole',
                data: "{drid:" + role + ",area:" + area + ",uid:" + userId + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    var x = document.getElementById('ContentPlaceHolder1_libRoles');
                    x.remove(x.selectedIndex);
                },
                error: function (data) {
                    alert(data.d)
                }
            });
        }
    </script>
</asp:Content>