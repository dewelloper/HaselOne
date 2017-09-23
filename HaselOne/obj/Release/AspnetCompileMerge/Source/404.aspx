<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="HaselOne._404" %>

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
    <%: Styles.Render("~/bundles/css/devexpress")  %>
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
                    <h1 class="page-title">404 Sayfa Bulunamadı
                            <small></small>
                    </h1>
                    <!-- END PAGE TITLE-->
                    <!-- END PAGE HEADER-->
                    <div class="note note-danger">
                        <p>
                            Aradığınız kaynak kaldırılmış, adı değiştirilmiş ya da geçici olarak kullanım dışı.
                        </p>
                        <div id="err" runat="server"></div>
                    </div>
                    <div class="note note-info">
                        <p>
                            <b>En olası nedenler:</b><br />
                            Belirtilen dizin veya dosya Web sunucusunda yok.<br />
                            URL bir yazım hatası içeriyor.<br />
                        </p>
                    </div>
                    <div class="note note-success">
                        <p>
                            <b>Yapabilecekleriniz</b><br />
                            <a href="/">Ana sayfa</a>ya gidin ve site menüsünü kullanarak ilerleyin.<br />
                            Bu sayfayı daha önce sık kullanılanlarınıza eklemiş olabilirsiniz ve sayfa kaldırılmış olabilir. Dolayısı ile bu sayfayı sık kulllanılanlardan kaldırmanız faydalı olacaktır.<br />
                        </p>
                    </div>
                </div>
                <!-- END CONTENT BODY -->
            </div>
            <!-- END CONTENT -->
        </div>
    </form>
</body>
<%: Scripts.Render("~/bundles/js/global")  %>
<script type="text/javascript">

    // please note,
    // that IE11 now returns undefined again for window.chrome
    // and new Opera 30 outputs true for window.chrome
    // and new IE Edge outputs to true now for window.chrome
    // and if not iOS Chrome check
    // so use the below updated condition
    var isChromium = window.chrome,
        winNav = window.navigator,
        vendorName = winNav.vendor,
        isOpera = winNav.userAgent.indexOf("OPR") > -1,
        isIEedge = winNav.userAgent.indexOf("Edge") > -1,
        isIOSChrome = winNav.userAgent.match("CriOS");

    if (isIOSChrome) {
        // is Google Chrome on IOS
    } else if (isChromium !== null && isChromium !== undefined && vendorName === "Google Inc." && isOpera == false && isIEedge == false) {
        // is Google Chrome
    } else {
        alert('HaselOne Uygulaması google chrome versiyonları ile çalışacak şekilde yazılmıştır. Lütfen Uygulamayı Google Chrome dışında bir internet Browser ile kullanmayınız...!!');
        window.location = '/Moduls/Generals/Login.Aspx';
    }

    function Logout() {
        $.ajax({
            type: "POST",
            url: '/HaselSOAService.asmx/Logout',
            data: "{valu:1}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                window.location = '<%=ResolveUrl("~/Moduls/Generals/Login.aspx")%>';
                },
                error: function (data) {
                    alert(data.d)
                }
            });
            }

            var clicked = false;
            var xmlHttp
            var browser = navigator.appName;
            function CheckBrowser() {
                if (clicked == false) {
                    xmlHttp = GetXmlHttpObject();
                    xmlHttp.open("GET", "<%= Page.ResolveUrl("~/SessionEnd.aspx") %>", true);
                        xmlHttp.onreadystatechange = function () {
                            if (xmlHttp.readyState == 4) {
                                // alert(xmlhttp.responseText)
                            }
                        }
                        xmlHttp.send(null)
                        if (browser == "Netscape") {
                            window.location = "<%= Page.ResolveUrl("~/SessionEnd.aspx") %>";
                            //alert("Browser Terminated");
                            //openInNewWindow();
                        }
                    }
                    else {
                        //alert("Redirected");
                        clicked = false;
                    }
                }

                function GetXmlHttpObject() {
                    var xmlHttp = null;
                    try {
                        // Firefox, Opera 8.0+, Safari
                        xmlHttp = new XMLHttpRequest();
                    }
                    catch (e) {
                        //Internet Explorer
                        try {
                            xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
                        }
                        catch (e) {
                            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                        }
                    }
                    return xmlHttp;
                }

                function openInNewWindow() {
                    // Change "_blank" to something like "newWindow" to load all links in the same new window
                    var newWindow = window.open("<%= Page.ResolveUrl("~/SessionEnd.aspx") %>");
                newWindow.focus();
                return false;
            }
</script>
</html>