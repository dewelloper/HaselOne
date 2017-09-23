<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ControlDetail.aspx.cs" Inherits="HaselOne.ControlDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdnControlId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Modül No</b></label>
            <input class="form-control controls" type="text" id="txtControlModulId" name="modulId" runat="server" placeholder="Modül No Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Sayfa No</b></label>
            <input class="form-control controls" type="text" id="txtControlPageId" name="txtControlPageId" runat="server" placeholder="Sayfa No Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kontrol Adı</b></label>
            <input class="form-control controls" type="text" id="txtControlId" name="txtControlId" runat="server" placeholder="Kontrol Adını(css id) Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kontrol Tipi</b></label>
            <input class="form-control controls" type="text" id="txtControlType" name="txtControlType" runat="server" placeholder="Kontrol Tipini Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <%--        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kontrol Text (İleri Uçlar için SEO Engine gibi)</b></label>
            <input class="form-control controls" type="text" id="txtControlAdvencedText" name="txtControlAdvencedText" runat="server" placeholder="Kontrol Adını(css id) Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>--%>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Kontrol Text Eng</b></label>
            <input class="form-control controls" type="text" id="txtControlTextEng" name="txtControlTextEng" runat="server" placeholder="Kontrol Adını(css id) Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Default Aktif-Pasif</b></label>
            <asp:RadioButtonList ID="rbControlEnable" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Aktif</asp:ListItem>
                <asp:ListItem Value="2">Pasif</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <br />
            <label><b>Defaul görünürlük</b></label>
            <asp:RadioButtonList ID="rbControlVisibile" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Görünsün</asp:ListItem>
                <asp:ListItem Value="2">Görünmesin</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="col-xs-12">
            <br />
            <a class="btn btn-info recordAndClose" onclick="UpdateControl(0);" id="btnUserUpdate" runat="server">Güncelle
            </a>
            <a class="btn btn-danger recordAndClose" onclick="UpdateControl(1);" id="btnUserInsert" runat="server">Kaydet
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function UpdateControl(workingmode) {
            var controlId = 0;
            if ($("#<%=hdnControlId.ClientID%>").val() != "")
                controlId = parseInt($("#<%=hdnControlId.ClientID%>").val());
            var modulId = parseInt($("#<%=txtControlModulId.ClientID%>").val());
            var pageId = parseInt($("#<%=txtControlPageId.ClientID%>").val());
            var controlIdName = $("#<%=txtControlId.ClientID%>").val();
            var controlType = $("#<%=txtControlType.ClientID%>").val();
            var textEng = $("#<%=txtControlTextEng.ClientID%>").val();
            var isEnableity = $("#<%=rbControlEnable.ClientID%>");
            var isEnable = $('' + isEnableity.selector + '').find('input[type=radio]:checked').val() == 1 ? true : false;
            var isVisibleity = $("#<%=rbControlVisibile.ClientID%>");
            var isVisible = $('' + isVisibleity.selector + '').find('input[type=radio]:checked').val() == 1 ? true : false;
            var vm = parseInt(workingmode);
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/SaveControl',
                data: "{modulId:" + modulId + ",pageId:" + pageId + ",controlId:" + controlId + ",controlText:\"" + controlIdName + "\",controlTextEng:\"" + textEng + "\",controlTypeName:\"" + controlType + "\",isEnable:" + isEnable + ",isVisible:" + isVisible + ",workingmode:" + vm + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d == true)
                        alert("Kontrol Noktası başarı ile güncellenmiştir...!");
                    else alert(data.d);
                },
                error: function (data) {
                    alert(data.d)
                }
            });
        }
    </script>
</asp:Content>