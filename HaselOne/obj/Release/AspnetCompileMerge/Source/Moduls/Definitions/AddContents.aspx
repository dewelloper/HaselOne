<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddContents.aspx.cs" Inherits="HaselOne.AddContents" %>


<asp:Content ID="Content1" ContentPlaceHolderID="startup_scripts" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:HiddenField ID="hdnCreatorId" runat="server" />
    <div class="form" style="margin-bottom: 20px !important;">
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>İçerik Tipi</b></label>
            <asp:DropDownList ID="ddContentTypes" runat="server" class="form-control" data-placeholder="İçerik Tipi Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Modül Seçimi</b></label>
            <asp:DropDownList ID="ddModuls" runat="server" class="form-control" data-placeholder="Modül Seçiniz">
            </asp:DropDownList>
        </div>
        <div class="col-md-3 col-sm-4 col-xs-12">
            <br />
            <label><b>Menü Seçimi</b></label>
            <asp:DropDownList ID="ddMenus" runat="server" class="form-control" data-placeholder="Menü Seçiniz">
            </asp:DropDownList>
        </div>

        <div class="col-md-12 col-sm-12 col-xs-12">
            <br />
            <label><b>İçerik</b></label>
            Seçilen modül ve sayfaya ilişkin html kodlarınızı buradan editleyiniz...
                <asp:TextBox ID="txtContent" runat="server" ClientIDMode="Static" TextMode="MultiLine" Rows="30" Style="width: 95%" CssClass="tinymce" />
        </div>

        <div class="col-xs-12">
            <br />
            <a class="btn btn-info recordAndClose" onclick="SaveCMContent();" id="btnUserUpdate" runat="server">Kaydet
            </a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptbase" runat="server">
    <script src="../../Scripts/tinymce/tinymce.js"></script>

    <script type="text/javascript">
        tinymce.init({
            selector: ".tinymce",
            theme: 'modern',
            plugins: ['advlist autolink lists link image charmap print preview hr anchor pagebreak',
            'searchreplace wordcount visualblocks visualchars code fullscreen',
            'insertdatetime media nonbreaking save table contextmenu directionality',
            'emoticons template paste textcolor colorpicker textpattern imagetools'
            ],
            toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
            toolbar2: 'print preview media | forecolor backcolor emoticons',
            image_advtab: true,
            language: 'tr_TR',
            templates: [
            { title: 'Test template 1', content: 'Test 1' },
            { title: 'Test template 2', content: 'Test 2' }
            ],
            content_css: ['//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css',
             '//www.tinymce.com/css/codepen.min.css'
            ]
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("select[id*='ddModuls']").change(function () {
                var modulIdP = parseInt($("select[id*='ddModuls']  option:selected").val());
                $.ajax({
                    type: "POST",
                    url: '/HaselSOAService.asmx/GetMenusByModulId',
                    data: "{modulId:" + modulIdP + "}",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {
                        var dat = data.d;
                        $("select[id*='ddMenus']").find('option').remove();
                        if (dat.length > 0) $("select[id*='ddMenus']").append($('<option></option>').val("").html("Seçiniz"));
                        $(dat).each(function (index, item) {
                            $("select[id*='ddMenus']").append($('<option></option>').val(item.Id).html(item.MenuName));
                        });
                    },
                    error: function (data) {
                        alert(data.d)
                    }
                });
            });

            $("select[id*='ddMenus']").change(function () {
                var contentTypeId = $("select[id*='ddContentTypes']  option:selected").val();
                var modulId = $("select[id*='ddModuls']  option:selected").val();
                var menuIdP = parseInt($("select[id*='ddMenus']  option:selected").val());
                $.ajax({
                    type: "POST",
                    url: '/HaselSOAService.asmx/GetContentIfExist',
                    data: "{contentTypeId:" + contentTypeId + ",modulId:" + modulId + ",menuId:" + menuIdP + "}",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {
                        var dat = data.d;
                        tinyMCE.activeEditor.setContent(dat);
                    },
                    error: function (data) {
                        alert(data.d)
                    }
                });
            });
        });

        function SaveCMContent(workingmode) {
            var contentTypeId = $("select[id*='ddContentTypes']  option:selected").val();
            var modulId = $("select[id*='ddModuls']  option:selected").val();
            var menuId = $("select[id*='ddMenu']  option:selected").val();
            var content = $.trim(tinymce.get('txtContent').getContent());
            content = content.replace("'", "");
            content = content.replace("\"", "");
            var creatorId = parseInt($("input[id*='hdnCreatorId']").val());
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/SaveCMContent',
                data: "{modulId:" + modulId + ",pageId:" + menuId + ",content:'" + content + "',contentTypeId:" + contentTypeId + ",creatorId:" + creatorId + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d == true)
                        alert("Content başarı ile güncellenmiştir...!");
                    else alert(data.d);
                },
                error: function (data) {
                    alert(data.d);
                }
            });
        }
    </script>      
</asp:Content>
