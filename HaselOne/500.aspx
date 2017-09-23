<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="HaselOne._500" %>

<!DOCTYPE html>
<html lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <title>Hasel Manager</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <meta content="Hasel CRM" name="description" />
    <meta content="Hasel İstif Makinaları San. ve Tic. A.Ş." name="author" />

    <%: Styles.Render("~/bundles/css/global")  %>
    <%: Styles.Render("~/bundles/css/pageLevel")  %>
    <%: Styles.Render("~/bundles/css/theme")  %>
    <%: Styles.Render("~/bundles/css/layout")  %>

</head>
<body class="page-header-fixed page-sidebar-closed-hide-logo page-content-white page-sidebar-closed">
    <form id="form1" runat="server">
        <!-- BEGIN CONTAINER -->
        <div class="page-container">
            <!-- BEGIN CONTENT -->
            <!-- BEGIN SIDEBAR -->
            <div class="page-sidebar-wrapper">
                <!-- BEGIN SIDEBAR -->
                <div class="page-sidebar navbar-collapse collapse">
                </div>
                <!-- END SIDEBAR -->
            </div>
            <!-- END SIDEBAR -->
            <div class="page-content-wrapper">
                <!-- BEGIN CONTENT BODY -->
                <div class="page-content">
                    <!-- BEGIN PAGE TITLE-->
                    <h1 class="page-title">500 Sunucu Hatası
                            <small></small>
                    </h1>
                    <!-- END PAGE TITLE-->
                    <!-- END PAGE HEADER-->
                    <div class="note note-danger">
                        <p><b>Sunucuya şu an erişilemiyor.</b><br />
                            Lütfen bir süre sonra tekrar deneyiniz.<br />
                            Eğer sorun devam ediyorsa lütfen sistem yöneticisine danışınız.</p>
                        <div id="err" runat="server"></div>
                    </div>
                </div>
                <!-- END CONTENT BODY -->
            </div>
            <!-- END CONTENT -->
        </div>
    </form>
</body>
</html>
