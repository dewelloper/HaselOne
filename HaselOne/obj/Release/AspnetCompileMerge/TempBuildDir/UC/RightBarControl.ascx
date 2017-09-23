<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RightBarControl.ascx.cs" Inherits="HaselOne.UC.RightBarControl" %>

<div id="divContainer">
    <div class="title">
        Haselchat'e hoş geldiniz
        <%--<br />
        [<span id='spanUser'></span>]--%>
    </div>
    <div id="divusers" class="users">
    </div>
    <div id="divChat" class="chatRoom">

        <div class="content">
            <div id="divChatWindow" class="chatWindow">
            </div>
        </div>
        <div class="messageBar">
            <input class="textbox" type="text" id="txtMessage" onkeypress="EnterKeyPress();" />
            <input id="btnSendMsg" type="button" value="Gönder" class="submitButton" />
        </div>
    </div>
    <input id="hdId" type="hidden" />
    <input id="hdUserName" type="hidden" />
</div>
