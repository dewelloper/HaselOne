<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="HaselOne.UserDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdnUserId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Ad</b></label>
            <input class="form-control controls hero" type="text" id="txtUserName" name="userName" runat="server" placeholder="Ad Yazınız" maxlength="200" required title="Ad enaz 2 karakter olmalıdır" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Soyad</b></label>
            <input class="form-control hero" type="text" id="txtUserSurname" runat="server" placeholder="Soyad Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Kullanıcı Adı</b></label>
            <input class="form-control" type="text" id="txtUserUserName" runat="server" placeholder="Kullanıcı Adı Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Pozisyon</b></label>
            <input type="text" id="txtUserPosition" class="form-control" runat="server" placeholder="Pozisyonu Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Email</b></label>
            <input class="form-control" type="text" id="txtUserEmail" runat="server" placeholder="Email Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Kullanıcı Şifresi</b></label>
            <input class="form-control" type="text" id="txtUserPassword" runat="server" placeholder="Kullanıcı Şifresi" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Telefon</b></label>
            <input class="form-control" type="text" id="txtUserPhone" runat="server" placeholder="Telefon Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Gsm</b></label>
            <input class="form-control" type="text" id="txtUserGsm" runat="server" placeholder="Gsm Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Fax</b></label>
            <input class="form-control" type="text" id="txtFaxNumber" runat="server" placeholder="Fax Yazınız" maxlength="50" />
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Bölge</b></label>
            <asp:DropDownList ID="ddMainArea" runat="server" class="form-control" data-placeholder="Bölge Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Admin mi?</b></label>
            <asp:RadioButtonList ID="rbIsAdmin" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Evet</asp:ListItem>
                <asp:ListItem Value="2">Hayır</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="row"></div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Yetki Grubu</b></label>
            <asp:DropDownList ID="ddUserAuthenticationGroup" runat="server" class="form-control forDochuman" data-placeholder="Yetki Grubu Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Şube Kodu(NetSis)</b></label>
            <asp:DropDownList ID="ddBranchCode" runat="server" class="form-control forDochuman" data-placeholder="Şube Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Departman Adı</b></label>
            <asp:DropDownList ID="ddDepartments" runat="server" class="form-control forDochuman" data-placeholder="Bölüm Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Bölge Müdürü mü?</b></label>
            <asp:RadioButtonList ID="rbAreaDirector" runat="server" RepeatDirection="Horizontal" CssClass="forDochuman">
                <asp:ListItem Value="1">Evet</asp:ListItem>
                <asp:ListItem Value="2">Hayır</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Satıcı mı?</b></label>
            <asp:RadioButtonList ID="rbSalesman" runat="server" RepeatDirection="Horizontal" CssClass="forDochuman">
                <asp:ListItem Value="1">Evet</asp:ListItem>
                <asp:ListItem Value="2">Hayır</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="col-md-9 col-sm-9 col-xs-12">
            <br />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUpload" />
                </Triggers>
                <ContentTemplate>
                    <label class="col-md-3 col-sm-3 col-xs-3"><b>Profil Resmi için</b></label>
                    <asp:FileUpload ID="FileUpload1" runat="server" class="col-md-3 col-sm-3 col-xs-3" />
                    <asp:Button ID="btnUpload" runat="server" Text="Resmi Yükle" OnClick="btnUpload_Click" class="col-md-3 col-sm-3 col-xs-3" />
                    <label id="lblmsg" runat="server" class="col-md-3 col-sm-3 col-xs-3"></label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <div class="col-xs-12">
            <a class="btn btn-info recordAndClose" onclick="UpdateUser(0);" id="btnUserUpdate" runat="server">Güncelle
            </a>
            <a class="btn btn-danger recordAndClose" onclick="UpdateUser(1);" id="btnUserInsert" runat="server">Kaydet
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script type="text/javascript">
        function UpdateUser(workingmode) {
            var userId = 0;
            if ($("#<%=hdnUserId.ClientID%>").val() != "")
                userId = parseInt($("#<%=hdnUserId.ClientID%>").val());
            var name = $("#<%=txtUserName.ClientID%>").val();
            var surname = $("#<%=txtUserSurname.ClientID%>").val();
            var username = $("#<%=txtUserUserName.ClientID%>").val();
            var level = 1;
            var email = $("#<%=txtUserEmail.ClientID%>").val();
            var position = $("#<%=txtUserPosition.ClientID%>").val();;
            var userGroup = $("#<%=ddUserAuthenticationGroup.ClientID%>" + " option:selected").text();
            var phone = $("#<%=txtUserPhone.ClientID%>").val();
            var gsm = $("#<%=txtUserGsm.ClientID%>").val();
            var userpassword = $("#<%=txtUserPassword.ClientID%>").val();
            var aktivepassive = 1;
            var aktivepassivemi = true;
            var admin = $("#<%=rbIsAdmin.ClientID%>");
            var isadmin = admin.find('input[type=radio]:checked').val() == 1 ? true : false;

            var fax = $("#<%=txtFaxNumber.ClientID%>").val();
            var branchCode = parseInt($("#<%=ddBranchCode.ClientID%>" + " option:selected").val());
            var mainArea = $("#<%=ddMainArea.ClientID%>" + " option:selected").text();
            var subArea = 1;
            var businessGroup = '1';
            var department = $("#<%=ddDepartments.ClientID%>" + " option:selected").text();
            var areaDirect = $("#<%=rbAreaDirector.ClientID%>");
            var areaDirector = areaDirect.find('input[type=radio]:checked').val() == 1 ? true : false;
            var sales = $("#<%=rbSalesman.ClientID%>");
            var salesman = sales.find('input[type=radio]:checked').val() == 1 ? true : false;
            var vm = parseInt(workingmode);
            var levl = parseInt(level);
            var uid = parseInt($("#hdnUserId").val());
            var fileName = '';
            if (document.getElementById("ContentPlaceHolder1_FileUpload1") != null)
                fileName = document.getElementById("ContentPlaceHolder1_FileUpload1").value;
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/SaveUser',
                data: "{userId:" + userId + ",userGroup:'" + userGroup + "',name:\"" + name + "\",surname:\"" + surname + "\",username:\"" + username + "\",level:" + levl + ",email:\"" + email + "\",position:\"" + position + "\",phone:\"" + phone + "\",gsm:\"" + gsm + "\",aktivepassivemi:" + aktivepassivemi + ",workingmode:" + vm + ",fax:\"" + fax + "\",branchCode:\"" + branchCode + "\",mainArea:\"" + mainArea + "\",subArea:\"" + subArea + "\",businessGroup:\"" + businessGroup + "\",department:\"" + department + "\",areaDirector:" + areaDirector + ",salesman:" + salesman + ",uid:" + uid + ",fileName:'" + fileName + "',isadmin:" + isadmin + ", password:'" + userpassword + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    alert("İşlem başarı ile gerçekleştirilmiştir..!");
                },
                error: function (data) {
                    alert(data.d)
                }
            });
        }
    </script>
</asp:Content>