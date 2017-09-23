<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpForm.aspx.cs" Inherits="HaselOne.Moduls.HelpContents.HelpForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Yardım Sayfası</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="modal-header" style="font-size:18px;">
        <b>Hasel CRM Yardım</b>
    </div>
    <div id="helpContainer" runat="server" class="modal-body" style="max-height:500px;overflow:hidden;overflow-y:scroll;">
    </div>
    </form> 
</body>
</html>
