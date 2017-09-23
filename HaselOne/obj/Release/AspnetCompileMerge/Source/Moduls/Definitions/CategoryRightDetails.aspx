﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoryRightDetails.aspx.cs" Inherits="HaselOne.CategoryRightDetails" %>

<%@ Import Namespace="HaselOne" %>
<%@ Import Namespace="DAL.Helper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%
        Mode pageMode;
        int Id = 0;
        if (Request.QueryString["Id"] != null)
        {

            pageMode = Mode.Edit;
            Id = Convert.ToInt32(Request.QueryString["Id"]);
        }
        else
        {
            pageMode = Mode.Insert;
        }

    %>

    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Grup</b></label>
            <asp:DropDownList ID="ddCategoriRightGroups" required title="Zorunlu" runat="server" class="form-control" data-placeholder="Kategori Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kategori</b></label>
            <asp:DropDownList ID="ddCustomerMachineparkCategoriy" required title="Zorunlu" runat="server" class="form-control" data-placeholder="Kategori Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Aktif-Pasif</b></label>
            <asp:RadioButtonList ID="rbActivePassive" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Aktif</asp:ListItem>
                <asp:ListItem Value="0">Pasif</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="col-xs-12">
            <br />
            <% if (Mode.Edit == pageMode)
                { %>
            <a class="btn btn-info recordAndClose" onclick="InsertOrUpdate(0);" id="btnUserUpdate" runat="server">Güncelle
            </a>
            <a class="btn btn-info recordAndClose" href="CategoryRightDetails.aspx" id="A1" runat="server">Yeni Kayıt</a>
            <%}
                else
                {%>
            <a class="btn btn-danger recordAndClose" onclick="InsertOrUpdate(1);" id="btnUserInsert" runat="server">Kaydet</a>
            <%}%>
        </div>
        <br />
        <div class="search-container col-md-12 " style="margin-top: 25px;">
            <div id="containerList" runat="server"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <%
        int Id = 0;
        if (Request.QueryString["Id"] != null)
            Id = Convert.ToInt32(Request.QueryString["Id"]);

    %>
    <script type="text/javascript">
        function LocalValidation(id)
        {
            if (!$("#ctl01").valid()) {
                $("[id*=ddCategoriRightGroups]").val();
                $("[id*=ddCustomerMachineparkCategoriy]").val();
                alert("Tüm alanları doldurunuz");
                return false;

            }
            else
            {
                if(id===0)
                    if (confirm("Yeni kategori olusturacak kabul ediyormusunuz?")  ) {
                        return true;
                    } else {
                        return false;
                    }

            }

        }
        function InsertOrUpdate(workingmode) {

            var id = <%=Id%>;
            if (LocalValidation(id)) {
                var CategoryId = $("[id*=ddCustomerMachineparkCategoriy]").val();
                var CRGId = $("[id*=ddCategoriRightGroups]").val();
                var aktivepassivemi = ($("#<%=rbActivePassive.ClientID%>").find('input[type=radio]:checked').val()) === "1" ? true : false;

                var vm = parseInt(workingmode);
                $.ajax({
                    type: "POST",
                    url: '/HaselSOAService.asmx/InsertorUpdateCategoryRightDetails',
                    data: "{ id:'" + parseInt(id) +"',  categoryId:'" +parseInt(CategoryId) +"',  crgId: '" +parseInt(CRGId) +"', isActive:'"+aktivepassivemi+"'}",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function(data) {
                        if (data.d == true) {
                            alert("Islem basarili...!");
                            location.reload();
                        } else alert(data.d);
                    },
                    error: function(data) {

                        alert(data);
                    }
                });
            }
        }

        function Delete(id) {
            if (confirm("Silmek istediginize eminmisiniz?")) {
                $.ajax({
                    type: "POST",
                    url: '/HaselSOAService.asmx/DeleteEntity',
                    data: "{ id:'" + parseInt(id) + "', tableName: 'Gn_CategoryDetails'}",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function(data) {

                        alert("Silme basarili...!");
                        location.reload();

                    },
                    error: function(data) {
                        alert(data);
                    }
                });
            }
        }
    </script>
</asp:Content>