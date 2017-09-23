<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HaselOne.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="tr">
<head runat="server">
    <meta charset="utf-8" />
    <title>HaselOne - Login</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <meta content="Preview page of Metronic Admin Theme #1 for " name="description" />
    <meta content="" name="author" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&amp;subset=all" rel="stylesheet" type="text/css" />
    <%: Styles.Render("~/bundles/css/global") %>
    <%: Styles.Render("~/bundles/css/loginPageLevel")  %>
    <%: Styles.Render("~/bundles/css/theme")  %>
    <%: Styles.Render("~/bundles/css/loginLayout")  %>
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body class=" login">
    <!-- BEGIN LOGO -->
    <div class="logo">
        <a href="index.html">
            <img src="/images/logo.png" />
        </a>
    </div>
    <!-- END LOGO -->
    <!-- BEGIN LOGIN -->
    <div class="content">
        <!-- BEGIN LOGIN FORM -->
        <form id="form1" runat="server">
            <h3 class="form-title font-green">Giriş</h3>
            <div class="alert alert-danger display-hide">
                <button class="close" data-close="alert"></button>
                <span>Kullanıcı adı ve şifresini girmeniz gerekmektedir. </span>
            </div>
            <div class="form-group">
                <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                <label class="control-label visible-ie8 visible-ie9">Username</label>
                <input id="userName" runat="server" onkeypress="EnterLogin(event);" class="form-control form-control-solid placeholder-no-fix" type="text" autocomplete="off" placeholder="Kullanıcı Adı" name="username" />
            </div>
            <div class="form-group">
                <label class="control-label visible-ie8 visible-ie9">Password</label>
                <input id="password" runat="server" onkeypress="EnterLogin(event);" class="form-control form-control-solid placeholder-no-fix" type="password" autocomplete="off" placeholder="Şifre" name="password" />
            </div>
            <div class="form-actions">
                <button type="button" class="btn green uppercase" onclick="Login();">GİRİŞ</button>
                <%--                <label class="rememberme check mt-checkbox mt-checkbox-outline">
                    <input type="checkbox" name="remember" value="1" />Hatırla <span></span>
                </label>
                <a href="javascript:;" id="forget-password" class="forget-password">Şifremi Unuttum?</a>--%>
                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" Style="display: none;" />
                <label runat="server" id="lblLoginStatus" Visible="False" class="label-danger">Girmiş olduğunuz kullanıcı adı veya şifre yanlıştır.</label>
            </div>
            <br />
            <input type="hidden" runat="server" id="hdnPasswordchangeStatus" value="normal" />

            <%--popup--%>
            <div id="modalPasswordChange" class="modal fade" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--  <button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <h4 class="modal-title">Lütfen şifrenizi değiştiriniz.</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-xs-12">
                                    <label for="txtPassword1"><b>Yeni şifre</b></label>
                                    <br/>
                                    <input type="password" class="form-control" name="txtPassword1" id="txtPassword1" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-xs-12 ">
                                    <label for="txtPassword2"><b>Yeni şifre tekrar</b></label>
                                    <br/>
                                    <input type="password" class="form-control" name="txtPassword2" id="txtPassword2" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-xs-12 ">
                                    <br/>
                                    <asp:Button ID="btnChanging" CssClass="btn green uppercase" OnClientClick="return PasswordValidation();" runat="server" Text="Değiştir" OnClick="btnChanging_Click" />
                                </div>
                            </div>
                        </div>


                        <div class="modal-footer">
                            <%-- <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                        </div>
                    </div>

                </div>
            </div>

        </form>
    </div>
    <div class="copyright">2016 © HASELONE. </div>
    
    <!--[if lt IE 9]>
    <script src="/Scripts/Misc/respond.min.js"></script>
    <script src="/Scripts/Misc/excanvas.min.js"></script> 
    <script src="/Scripts/Misc/ie8.fix.min.js"></script> 
    <![endif]-->
     <%: Scripts.Render("~/bundles/js/global")  %>
     <%: Scripts.Render("~/bundles/js/coreplugins") %>
     <%: Scripts.Render("~/bundles/js/loginLayout") %>
   
    <script type="text/javascript">
        $(document)
            .ready(function() {
                $("input:password")
                    .keypress(function() {

                    });

                if ($("#hdnPasswordchangeStatus").val() === "change") {
                    $("#modalPasswordChange").modal();
                }

                $("#txtPassword").keypress(function (e) {
                   if (e.which === 13) {
                       document.getElementById('btnChanging').click();
                   }

                });

                $("#txtPassword2").keypress(function (e) {
                    if (e.which === 13) {
                        document.getElementById('btnChanging').click();
                    }

                });

            });

        function Login() {
            document.getElementById('btnLogin').click();
        }

        function EnterLogin(e) {
            if (e.keyCode == 13) {
                document.getElementById('btnLogin').click();
            }
        }

        function PasswordValidation() {
         
            var validation = false;
            if ($("#txtPassword1").val() === $("#txtPassword2").val()) {
                validation = true;
            } else {
                alert("Şifreler aynı değil");
                validation = false;
            }

            if ($("#txtPassword1").val().length > 5) {
                validation = true;
            } else {
                alert("Şifre en az 6 karekter olmalıdır.");
                validation = false;
            }

            return validation;
        }
    </script>

</body>
</html>
