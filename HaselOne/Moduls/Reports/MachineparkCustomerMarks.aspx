<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineparkCustomerMarks.aspx.cs" Inherits="HaselOne.MachineparkCustomerMarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="col-md-12">
        <table>
            <tr>
                <td>
                    <asp:DropDownList class="boundedDiv" ID="ddCategories" runat="server" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox class="boundedDiv" ID="chkAll" style="display:none;" runat="server" Text="Bütün kategorilere göre" />
                </td>
                <td>
                    <asp:Button ID="btnLoad" CssClass="btn-success boundedDiv" runat="server" Text="Makine Parkı Cari Dağılımı" OnClick="btnLoad_Click" />
                </td>
                <td>
                    <asp:Button ID="btnMachineLoad" CssClass="btn-success boundedDiv" runat="server" Text="Makine Parkı Makine Dağılımı" OnClick="btnMachineLoad_Click" />
                </td>
                <td>
                    <input id="btnPrint" type="button" class="btn-info boundedDiv" value="Yazdır" onclick="CallPrint('machineparkContent');" />
                </td>
            </tr>
        </table>
        
        <div id="machineparkContent" class="categoryCustomer" runat="server">
        </div>
    </div>
    <script type="text/javascript">
        function CallPrint(strid) {
            var prtContent = document.getElementById("ContentPlaceHolder1_" + strid);
            var WinPrint = window.open('', '', 'letf=0,top=0,width=800,height=600,toolbar=0,scrollbars=0,status=0,dir=ltr');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            prtContent.innerHTML = strOldOne;
        }
    </script>
</asp:Content>
