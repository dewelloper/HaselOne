<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ControlAuthoritiesDetail.aspx.cs" Inherits="HaselOne.ControlAuthoritiesDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdnCAId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kullanıcılar</b></label>
            <asp:DropDownList ID="ddUserCA" runat="server" class="form-control" data-placeholder="Kullanıcı Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kontroller</b></label>
            <asp:DropDownList ID="ddControlCA" runat="server" class="form-control" data-placeholder="Kontrol Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Aktif-Pasif</b></label>
            <asp:RadioButtonList ID="rbCAActivePassive" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Aktif</asp:ListItem>
                <asp:ListItem Value="2">Pasif</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Görünürlük</b></label>
            <asp:RadioButtonList ID="rbCAVisibility" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Evet</asp:ListItem>
                <asp:ListItem Value="2">Hayır</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="col-xs-12">
            <br />
            <a class="btn btn-info recordAndClose" onclick="UpdateCA(0);" id="btnUserUpdate" runat="server">Güncelle
            </a>
            <a class="btn btn-danger recordAndClose" onclick="UpdateCA(1);" id="btnUserInsert" runat="server">Kaydet
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function UpdateCA(workingmode) {
            var caId = 0;
            if ($("#<%=hdnCAId.ClientID%>").val() != "" && workingmode != 1)
                caId = parseInt($("#<%=hdnCAId.ClientID%>").val());
            else caId = parseInt($("#<%=ddControlCA.ClientID%>" + " option:selected").val());
            var userId = parseInt($("#<%=ddUserCA.ClientID%>" + " option:selected").val());
            var enablity = $("#<%=rbCAActivePassive.ClientID%>");
            var isEnable = enablity.find('input[type=radio]:checked').val() == 1 ? true : false;
            var visibility = $("#<%=rbCAVisibility.ClientID%>");
            var isVisible = visibility.find('input[type=radio]:checked').val() == 1 ? true : false;
            var vm = parseInt(workingmode);
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/SaveCA',
                data: "{caId:" + caId + ",userId:" + userId + ",isVisible:" + isVisible + ",isEnable:" + isEnable + ",workingmode:" + vm + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d == true) {
                        <%Session["UkChanged"] = "1";%>;
                        alert("Kontrol Yetkisi başarı ile güncellenmiştir...!");
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