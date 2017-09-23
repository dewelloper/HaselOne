<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ControlAuthorities.aspx.cs" Inherits="HaselOne.ControlAuthorities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="search-page search-content-2 col-md-12">
        <div class="row">
            <div class="col-md-12">
                <a class="btn btn-danger recordAndClose" id="btnUserInsertNew" runat="server" href="ControlAuthoritiesDetail.aspx">Yeni Kontrol Yetkisi Oluştur
                </a>
            </div>
        </div>
        <br />
        <div class="search-bar ">
            <div class="row">
                <div class="col-md-12">
                    <div class="input-group">
                        <input id="searchInput" type="text" onkeypress="SearchClicker(event);" class="form-control headautocomplete" placeholder="Gelişmiş Arama: KullanıcıGrubu Adı..." runat="server">
                        <span class="input-group-btn">
                            <button class="btn blue uppercase bold" type="button" onclick="SearchClick();">Ara</button>
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="search-container ">
                    <br />
                    <div id="containerUser" runat="server"></div>

                    <div class="search-pagination" id="paginator" runat="server">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnPageIndex" runat="server" />
    <asp:Button ID="btnSearch" Style="display: none;" runat="server" OnClick="btnSearch_Click" Text="Ara" />
    <asp:Button ID="btnChangePageIndex" runat="server" Style="display: none;" Text="NextPage" OnClick="btnChangePageIndex_Click" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function SearchClicker(e) {
            if (e.keyCode == 13) {
                document.getElementById('ContentPlaceHolder1_btnSearch').click();
                return false;
            }
        }

        function SearchClick() {
            document.getElementById('ContentPlaceHolder1_btnSearch').click();
        }

        function BeforePage(pageIndex) {
            document.getElementById('ContentPlaceHolder1_hdnPageIndex').value = -1;
            document.getElementById('ContentPlaceHolder1_btnChangePageIndex').click();
        }

        function NextPage(pageIndex) {
            document.getElementById('ContentPlaceHolder1_hdnPageIndex').value = 1;
            document.getElementById('ContentPlaceHolder1_btnChangePageIndex').click();
        }

        function ChangeActivePassiveCAEnable(caId) {
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/ChangeActivePassiveCAEnable',
                data: "{caId:" + caId + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    <%Session["UkChanged"] = "1";%>;
                    alert("Kontrol aktifliği başarı ile değiştirilmiştir...!");
                },
                error: function (data) {
                    alert(data.d)
                }
            });
        }

        function ChangeActivePassiveCAVisible(caId) {
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/ChangeActivePassiveCAVisible',
                data: "{caId:" + caId + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    <%Session["UkChanged"] = "1";%>;
                    alert("Kontrol silinirliği başarı ile değiştirilmiştir...!");
                },
                error: function (data) {
                    alert(data.d)
                }
            });
        }
    </script>
</asp:Content>