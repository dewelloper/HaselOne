<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SegmentStats.aspx.cs" Inherits="HaselOne.Moduls.Reports.SegmentStats" %>

<%@ Import Namespace="DAL.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="startup_scripts" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="demo-container" ng-controller="SegmentStatsReportController">

        <div class="list-toggle">
            <button type="button" class="btn btn-default" ng-click="IsFilterExpanded = !IsFilterExpanded">
                <i class="fa fa-filter fa-fw"></i>Filtreler
                <i class="fa fa-fw" ng-class="{'fa-chevron-right': !IsFilterExpanded, 'fa-chevron-down': IsFilterExpanded}"></i>
            </button>
            <button type="button" class="btn btn-success" ng-click="GridService.ExportToExcel('grid')"><i class="fa fa-file-excel-o fa-fw"></i>Excel'e Aktar</button>
        </div>

        <div class="task-list panel-collapse collapse" ng-class="{'in' :IsFilterExpanded}">
            <div class="row">
                <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                    <label><b>Operasyon</b></label>
                    <div search-select ng-model="ReportFilter.Category" options="CategorySelectOptions"></div>
                </div>
                <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                    <label><b>Satış Mühendisi</b></label>
                    <div search-select multiple ng-model="ReportFilter.Salesmans" options="SalesmanSelectOptions"></div>
                </div>
                <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                    <label><b>Bölge</b></label>
                    <div search-select multiple ng-model="ReportFilter.Areas" options="AreaSelectOptions"></div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                    <label><b>MP Kategori</b></label>
                    <div search-select multiple ng-model="ReportFilter.MachineparkCategories" options="MpCategorySelectOptions"></div>
                </div>
                <div class="col-md-4 col-sm-6 col-xs-12 form-group">
                    <label><b>Marka</b></label>
                    <div search-select multiple ng-model="ReportFilter.Marks" options="MarkSelectOptions"></div>
                </div>
            </div>
        </div>

        <div id="grid" dx-data-grid="GridService.SetOptions(dataGridOptions)"></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptbase" runat="server">

    <script src="/Scripts/_Controllers/StatsReportController.js?v=<%=Helper.StaticGuid %>"></script>
</asp:Content>