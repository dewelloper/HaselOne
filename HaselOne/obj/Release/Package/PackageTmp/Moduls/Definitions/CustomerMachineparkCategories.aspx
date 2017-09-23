<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerMachineparkCategories.aspx.cs" Inherits="HaselOne.CustomerMachineparkCategories" %>

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
            <label><b>Ana Kategori</b></label>
            <asp:DropDownList ID="ddCustomerMachineparkCategoriParent" runat="server" class="form-control" required title="Zorunlu" data-placeholder="Kategori Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Alt Kategori</b></label>
            <asp:TextBox runat="server" ID="txtCustomerMachineparkCategoriSub" required title="Zorunlu" ClientIDMode="Static" class="form-control"></asp:TextBox>
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
            <a class="btn btn-info recordAndClose" href="CustomerMachineparkCategories.aspx" id="A1" runat="server">Yeni Kayıt</a>
            <% }
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
                $("[id*=ddCustomerMachineparkCategoriParent]").val();
                $("[id*=txtCustomerMachineparkCategoriSub]").val();
                alert("Tüm alanları doldurunuz");
                return false;
            }
            else {
                if (id === 0) {
                    if (confirm("Yeni kategori olusturacak kabul ediyormusunuz?")) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
            }
        }
        function Delete(id) {
            if (confirm("Silmek istediginize eminmisiniz?")) {
                $.ajax({
                    type: "POST",
                    url: '/HaselSOAService.asmx/DeleteEntity',
                    data: "{ id:'"+parseInt(id)+"', tableName: 'Cm_MachineparkCategory'}",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {

                        alert("Silme basarili...!");
                        location.reload();

                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            }

        }
        function InsertOrUpdate(workingmode) {
            var aktivepassivemi = ($("#<%=rbActivePassive.ClientID%>").find('input[type=radio]:checked').val()) === "1" ? true : false;

            var id =  <%=Id%>;
            var parentId =   $("[id*=ddCustomerMachineparkCategoriParent]").val();
            var categoryName = $("[id*=txtCustomerMachineparkCategoriSub]").val();
            if (LocalValidation(id)) {
                var vm = parseInt(workingmode);
                $.ajax({
                    type: "POST",
                    url: '/HaselSOAService.asmx/InsertorUpdateCustomerMachineparkCategories',
                    data: "{ id:'"+parseInt(id)+"',  parentId:'"+parseInt(parentId)+"',  categoryName: '"+ (categoryName)+"',  isActive: '"+ aktivepassivemi+"'}",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {
                        if (data.d == true) {
                            alert("Islem basarili...!");
                            location.reload();
                        }

                        else alert(data.d);
                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            }

        }
    </script>
</asp:Content>