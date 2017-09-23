<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationRoleChart.aspx.cs" Inherits="HaselOne.OperationRoleChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-12">
        <table>
            <tr>
                <td>
                    <asp:DropDownList OnSelectedIndexChanged="ddOperations_SelectedIndexChanged" class="boundedDiv" ID="ddOperations" runat="server" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
        </table>

        <div id="opChartContent" class="categoryCustomer" runat="server">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server"></asp:Content>