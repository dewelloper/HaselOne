<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoryManagement.aspx.cs" Inherits="HaselOne.CategoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="startup_scripts" runat="server">
    <link href="../../Content/Css/Misc/TreeViewFix.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" ng-controller="CategoryManagementController">
        <script type="text/ng-template" id="nodes_renderer.html">
                 <div ui-tree-handle class="tree-node tree-node-content">
                    <a class="btn btn-success btn-xs" ng-if="item.Categories && item.Categories.length > 0" data-nodrag ng-click="toggle(this)"><span
                         class="fa"
                         ng-class="{
                                 'fa-chevron-right': collapsed,
                                 'fa-chevron-down': !collapsed
                                   }"></span></a>
                        {{item.CategoryName}}
                  </div>
                <ol ui-tree-nodes="" ng-model="item.Categories" ng-class="{hidden: collapsed}">
                    <li ng-repeat="item in item.Categories" ui-tree-node ng-include="'nodes_renderer.html'">
                    </li>
                 </ol>
        </script>
        <div class="row">
            <div class="input-group" style="left: 25px;">
                <div class="input-group-btn">
                    <a class="btn btn-success" ng-click="expandAll()">Aç</a>
                    <a class="btn btn-warning" ng-click="collapseAll()">Kapat</a>
                    <a class="btn btn-danger" ng-click="saveChanges()">Değişiklikleri Kaydet</a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-sm-4 col-xs-12">
                <div ui-tree="treeOptions">
                    <ol ui-tree-nodes ng-model="list">
                        <li ng-repeat="item in list" ui-tree-node ng-include="'nodes_renderer.html'"></li>
                    </ol>
                </div>
            </div>

            <div class="input-group col-md-3 col-sm-4 col-xs-12">
                <br />
                <div class="row">
                    <label><b>Ana Kategori</b></label>
                    <input class="form-control hero" type="text" id="txtSelectedCategyName" ng-value="selectedCategoryName" ng-attr-title="{{selectedCategoryId}}" runat="server" placeholder="Seçilen Kategori" />
                </div>
                <br />
                <div class="row">
                    <label><b>Yeni Eklenecek Kategori Adı</b></label>
                    <input class="form-control hero" type="text" id="txtCategoryName" ng-model="NewCategoryName" runat="server" placeholder="Kategori Yazınız" />
                </div>
                <br />
                <div class="row">
                    <div class="input-group-btn">
                        <a class="btn btn-info" ng-click="Add()">Yeni Kategori Ekle</a>
                        <a class="btn btn-danger" ng-click="Delete()">Seçilen Kategoriyi Sil</a>
                        <%--<a class="btn btn-warning" ng-click="Reload()">Yenile</a>--%>
                    </div>
                </div>
            </div>
        </div>
        <%--<div js-tree="treeConfig" ng-model="originalData"  tree="treeInstance" ></div> should-apply="ignoreModelChanges()" tree-events="ready:readyCB;create_node:createNodeCB"--%>

    </div>

   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptbase" runat="server">
    <script src="/Scripts/_Controllers/CategoryManagementController.js"></script>
</asp:Content>
