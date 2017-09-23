<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InCompleteCustomers.aspx.cs" Inherits="HaselOne.InCompleteCustomers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../../Content/Css/ui.themes/base/jquery-ui.css" rel="stylesheet" />
    <link href="../../Content/Css/ui.themes/base/jquery.ui.all.css" rel="stylesheet" />
    <link href="../../Content/Css/jquery.jqGrid/ui.jqgrid.css" rel="stylesheet" />
    <div class="col-md-12">
        <div class="portlet">
            <div class="portlet-title">
                <div class="caption">
                    <i class="fa fa-thumbs-up"></i><a id="cariWhois" runat="server">Cariler</a>
                    <a id="message" runat="server" style="color: red;"></a>
                </div>
                <div class="caption col-md-12 col-sm-12 col-xs-12">
                    <select class="form-control todo-taskbody-tags select" id="ddFilter" runat="server" data-placeholder="Seçiniz" required title="* Gerekli">
                        <option value="">Seçiniz</option>
                        <option value="1">Tüm Cariler</option>
                        <option value="2">Onaysız Cariler</option>
                        <option value="3">Tamamlanmayan Cariler </option>
                        <option value="4">Lokasyonu Olmayan Cariler</option>

                        <option value="5">Makinesi  Olmayan Cariler</option>
                        <option value="6">Satıcısı Olmayan Cariler</option>
                        <option value="7">Yetkilisi Olmayan Cariler</option>
                    </select>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div id='jqxWidget' style="font-size: 13px; font-family: Verdana; float: left; padding-left: 10px; width: 96%;">
        <div id="jqxgrid">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server">
    <script src="../../Scripts/_Resources/grid.locale-tr.js"></script>
    <script src="../../Scripts/Misc/jquery.jqGrid.min.js"></script>
    <%--    <script src="../../Scripts/jqwidgets/jqx-all.js"></script>--%>
    <link href="../../Scripts/Guriddo_JQGrid/plugins/searchFilter.css" rel="stylesheet" />
    <script src="../../Scripts/Guriddo_JQGrid/plugins/jquery.searchFilter.js"></script>

    <link href="../../Scripts/jqwidgets/styles/jqx.base.css" rel="stylesheet" />
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxcore.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxdata.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxbuttons.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxscrollbar.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxlistbox.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxdropdownlist.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxmenu.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxgrid.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxgrid.filter.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxgrid.sort.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxgrid.selection.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxpanel.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/globalization/globalize.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxcalendar.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxdatetimeinput.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxcheckbox.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxwindow.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxgrid.pager.js"></script>
    <script src="../../Scripts/Moment/moment.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            /*
            tamamlanmayan makınesızın ısmı tamamlanmayan carıler olarak degısıtırılecek
lokasyonsuz olan secenek Lokasyonu olmayanlar dıye degısedccek ısım sadece
eklencekler ----
Makinesi olmayan cariler
Saticisi olmayan cariler
Yetkilisi olmayan cariler

            */

            /*
             1. Tüm Cariler
             2. Onaysız Cariler
             3. Tamamlanmayan Cariler
             4. Lokasyonsuz
            */
            $("[id*=ddFilter]").change(function () {
                var value = this.value;
                GetList(value);

            });
        });
        //GetCustomersHasNotLocation
        function GetList(customerStatus) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/HaselGridService.asmx/GetCustomersLast202",
                data: "{customerStatus:'" + customerStatus + "'}",
                dataType: "json",
                success: function (data, textStatus) {
                    var data = data.d;
                    LoadJQGrid(data);
                },
                error: function (data, textStatus) {
                    alert('An error has occured retrieving data!');
                }
            });
        }

        function LoadJQGrid(data) {
            var source =
                        {

                            localdata: data,
                            datafields:
                            [
                                { name: 'Id', type: 'number' },
                                { name: 'Name', type: 'string' },
                                { name: 'NetsisHaselCode', type: 'string' },
                                { name: 'NetsisRentliftCode', type: 'string' },
                                { name: 'TaxNumber', type: 'string' },
                                { name: 'StatusId', type: 'number' },
                                { name: 'CreatorName', type: 'string' },
                                { name: 'StrCreatorDate', type: 'string' },
                                { name: 'ModifiedName', type: 'string' },
                                { name: 'StrModifiedDate', type: 'string' }
                         
                               
                            ],
                            datatype: "array",
                          
                            pager: function (pagenum, pagesize, oldpagenum) {
                                // callback called when a page or page size is changed.
                            }
                        };
            var cellsrenderer = function (row, cell, value) {
                return '<a href="CustomerDetail.aspx?CId=' + value + '" />&nbsp;Cari Detay</a>'
            }
            var addfilter = function () {
                var filtergroup = new $.jqx.filter();
                var filter_or_operator = 1;
                var filtervalue = 'Beate';
                var filtercondition = 'contains';
                var filter1 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
                filtervalue = 'Andrew';
                filtercondition = 'starts_with';
                var filter2 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);

                filtergroup.addfilter(filter_or_operator, filter1);
                filtergroup.addfilter(filter_or_operator, filter2);
                // add the filters.
                $("#jqxgrid").jqxGrid('addfilter', 'firstname', filtergroup);
                // apply the filters.
                $("#jqxgrid").jqxGrid('applyfilters');
            }
            var adapter = new $.jqx.dataAdapter(source);
            $("#jqxgrid").jqxGrid(
            {
                width: '100%',
                height: '100%',
                source: adapter,
                filterable: true,
                sortable: true,
                autoshowfiltericon: true,
                showfiltercolumnbackground: true,
                pageable: true,
                ready: function () {
                    addfilter();
                },
                autoshowfiltericon: true,
                columns: [
  
                  //{ text: 'Id', datafield: 'Id', width: '10%' },
                  { text: 'Cari İsim', datafield: 'Name', width: '30%' },
                  { text: 'H.C.Kod', datafield: 'NetsisHaselCode', width: '8%' },
                  { text: 'R.C.Kod', datafield: 'NetsisRentliftCode', width: '8%' },
                  { text: 'VN', datafield: 'TaxNumber', width: '10%' },
                  { text: 'Durum', datafield: 'StatusId', width: '4%' },
         
                { text: 'Olusturan', datafield: 'CreatorName', width: '11%' },
                { text: 'Olusturma Tarih', datafield: 'StrCreatorDate', width: '11%' },
                { text: 'Degistiren', datafield: 'ModifiedName', width: '11%' },
                { text: 'Degistirme Tarih', datafield: 'StrModifiedDate', width: '11%' },
                { text: 'Id', datafield: 'Id', width: '10%', cellsalign: 'right', cellsrenderer: cellsrenderer },
                ]
            });

            var localizationobj = {};
            localizationobj.pagergotopagestring = "Sayfaya git:";
            localizationobj.pagershowrowsstring = "Sayfayı göster:";
            localizationobj.pagerrangestring = " Düzen ";
            localizationobj.pagernextbuttonstring = "İleri";
            localizationobj.pagerpreviousbuttonstring = "Geri";
            localizationobj.sortascendingstring = "Artan sırala";
            localizationobj.sortdescendingstring = "Azalan Sırala";
            localizationobj.sortremovestring = "Sıralamayı kaldır";
            localizationobj.firstDay = 1;
            localizationobj.percentsymbol = "%";
            localizationobj.currencysymbol = "TL";
            localizationobj.currencysymbolposition = "before";
            localizationobj.decimalseparator = ".";
            localizationobj.thousandsseparator = ",";
            localizationobj.filtershowrowstring = "Koşula göre göster:";

            localizationobj.groupsheaderstring = "Gruplamak için sürükle ve buraya bırak",
            localizationobj.groupbystring = "Bu kolona göre grupla",
            localizationobj.groupremovestring = "Gruptan kaldır",
            localizationobj.filterclearstring = "Temizle",
            localizationobj.filterstring = "Filitrele",
            localizationobj.filtershowrowdatestring = "Tarihe göre göster=",
            localizationobj.filterorconditionstring = "Or",
            localizationobj.filterandconditionstring = "And",
            localizationobj.filterselectallstring = "(Tümünü Seç)",
            localizationobj.filterchoosestring = "Seçiniz=",
            localizationobj.filterstringcomparisonoperators = ['empty', 'not empty', 'contains', 'contains(match case)',
                'does not contain', 'does not contain(match case)', 'starts with', 'starts with(match case)',
                'ends with', 'ends with(match case)', 'equal', 'equal(match case)', 'null', 'not null'],
            localizationobj.filternumericcomparisonoperators = ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
            localizationobj.filterdatecomparisonoperators = ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
            localizationobj.filterbooleancomparisonoperators = ['equal', 'not equal'],
            localizationobj.validationstring = "Girilen değer doğru değil",
            localizationobj.emptydatastring = "Gösterilecek data yok",
            localizationobj.filterselectstring = "Filitre seçiniz",
            localizationobj.loadtext = "Yükleniyor...",
            localizationobj.clearstring = "Temizle",
            localizationobj.todaystring = "Bugün"

            var days = {
                // full day names
                names: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
                // abbreviated day names
                namesAbbr: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
                // shortest day names
                namesShort: ["Pz", "Pt", "Sl", "Çr", "Pr", "Cm", "Ct"]
            };
            localizationobj.days = days;
            var months = {
                // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
                names: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık", ""],
                // abbreviated month names
                namesAbbr: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Agu", "Eyl", "Eki", "Kas", "Ara", ""]
            };
            localizationobj.months = months;
            $("#jqxgrid").jqxGrid('localizestrings', localizationobj);
            $("#jqxgrid").on("filter", function (event) {
                $("#events").jqxPanel('clearcontent');
                var filterinfo = $("#jqxgrid").jqxGrid('getfilterinformation');
                var eventData = "Triggered 'filter' event";
                for (i = 0; i < filterinfo.length; i++) {
                    var eventData = "Filter Column: " + filterinfo[i].filtercolumntext;
                    $('#events').jqxPanel('prepend', '<div style="margin-top: 5px;">' + eventData + '</div>');
                }
            });
        }
    </script>
</asp:Content>