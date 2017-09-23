<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="HaselOne.CustomerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="startup_scripts" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div ng-controller="CustomerListController">
        <div class="form editForm">
            <div class="col-md-4 col-sm-6 col-xs-12">
                <label><b>Data tipine göre:</b></label>
                <div search-select ng-model="DraftCustomers" options="DraftCustomerOptions" ng-sync-value="CustomerFilter.Id"></div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <div class="table-scrollable myView">
            <div id="customerGrid" dx-data-grid="GridService.SetOptions(dataGridOptions)" dx-item-alias="entity">
                <div data-options="dxTemplate:{ name:'cellTemplateCommand' }">
                    <div>
                        <a href="CustomerDetail.aspx?CId={{entity.data.Id}}">Detay</a>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptbase" runat="server">
    <script src="../../Scripts/_Services/CustomerService.js"></script>
    <script src="../../Scripts/_Controllers/CustomerListController.js"></script>
</asp:Content>
