function removeElement(element) {
    element && element.parentNode && element.parentNode.removeChild(element);
}

function ClearDropDownItems(id) {
    var select = document.getElementById(id);
    var length = select.options.length;
    for (i = 0; i < length; i++) {
        select.options[i] = null;
    }
}

$(document).ready(function () {


    //$(".AddRequest").click(function () {
    //    $("#AddRequest").slideToggle("slow");
    //    $("#DoIt1").animate({ width: "toggle", opacity: "toggle" }, "slow");
    //    $("#DoIt1Bg").toggleClass("bg-yellow-crusta bg-font-yellow-crusta", 1000, "easeOutSine");
    //});
    //$(".ShowRequestResult").click(function () {
    //    $("#ShowRequestResult").slideToggle("slow");
    //    $("#DoIt1").animate({ width: "toggle", opacity: "toggle" }, "slow");
    //    $("#DoIt1Bg").toggleClass("bg-yellow-crusta bg-font-yellow-crusta", 1000, "easeOutSine");
    //});

    //$(".DoIt1").click(function () {
    //    $("#DoIt1").animate({ width: "toggle", opacity: "toggle" }, "slow");
    //});

   
});


function ToggleNewMachinepark() {
    $(".collapse.machinepark").collapse('toggle');
    $(".RACMP1").text('Kaydet ve Kapat');
    $(".RACMP2").text('Kaydet ve Yeni Kayıt Aç');
    MachineParkformClean("insert");

    SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpMachineparkType");
    SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpMarks");
    SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpYears");
    SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpRetireOrOwnered");
    SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpMachineparkLocation");

}

function MachineParkformClean(mode) {
    $("input[id*='txtMpModel']").val("");
    $("input[id*='txtMpSerialNo']").val("");
    $("input[id*='txtMpMachineparkCount']").val("");
    $("[id*='dateSaleDate']").val("");
    $("[id*='datePlanedRelease']").val("");
    $("[id*='dateRelease']").val("");
    $("[id*='ddMpRetireOrOwnered']").val("0");
    $("[id*='ddMpMachineparkLocation']").val("");

    if (mode == "update") {
        $("[id*='ddMpMachineparkType']").attr("disabled", true);
    }
    if (mode === "insert") {
        $("[id*='ddMpMachineparkType']").attr("disabled", false);
    }


}

function ToggMachineparkShow() {
    if ($(".machinepark.collapse.in").length <= 0)
        $(".collapse.machinepark").collapse('toggle');
    $('body, html').animate({ scrollTop: 180 }, 800);

}

function ToggleLocation() {
    $(".collapse.location").collapse('toggle');
    $(".RACLE1").text('Kaydet ve Kapat');
    $(".RACLE2").text('Kaydet ve Yeni Kayıt Aç');
    SetDropWithCity("Seçiniz...", "ContentPlaceHolder1_ddLocCity");

    $("[id*='ddLocCity']").val('');
    $("[id*='ddLocRegion']").val('');
    $("input[id*='txtLocAddress']").val('');
    $("input[id*='txtLocDefinition']").val('');
    $("input[id*='txtLocTel']").val('');

    $("input[id*='txtLocFax']").val('');
    $("input[id*='txtLocLong']").val('');
    $("input[id*='txtLocLat']").val('');
    $(".RACLE3").css({ 'display': "none" });
    $("input[id*='txtLocDefinition']").prop("readonly", false);
    setTimeout(function () { initAutocomplete(); }, 200);
}

function QuantityOne() {
    if (document.getElementById("ContentPlaceHolder1_txtMpSerialNo").value.trim() != "") {
        document.getElementById("ContentPlaceHolder1_txtMpMachineparkCount").value = "1";
        document.getElementById("ContentPlaceHolder1_txtMpMachineparkCount").setAttribute("disabled", "disabled");
    }
    else {
        document.getElementById("ContentPlaceHolder1_txtMpMachineparkCount").removeAttribute("disabled");
    }
}

function ToggleLocationShow() {
    if ($(".location.collapse.in").length <= 0)
        $(".collapse.location").collapse('toggle');
    $('body, html').animate({ scrollTop: 180 }, 800);
}

function ToggSEs() {
    $(".collapse.salesman").collapse('toggle');
    $(".RACSE1").text('Kaydet ve Kapat');
    $(".RACSE2").text('Kaydet ve Yeni Kayıt Aç');
    SetDropText("Seçiniz...", "ContentPlaceHolder1_ddSalesEngineeers");
    //ClearDropDownItems("ContentPlaceHolder1_ddSalesmanTypes");
    var rbSEAktivity1 = $("input[id*='rbSEAktivity']");
    rbSEAktivity1.val([1]);
}

function ToggSalesmanShow() {
    if ($(".salesman.collapse.in").length <= 0)
        $(".collapse.salesman").collapse('toggle');
    $('body, html').animate({ scrollTop: 180 }, 800);
}

function ToggAuthenticators() {
    $(".collapse.auth").collapse('toggle');
    $(".RACA1").text('Kaydet ve Kapat');
    $(".RACA2").text('Kaydet ve Yeni Kayıt Aç');
    $(".RACA3").css({ 'display': "none" });
    SetDropText("Seçiniz...", "ContentPlaceHolder1_ddAutLocation");
    $("input[id*='txtAuthGsm']").val('');
    $("input[id*='AuthPhone']").val('');
    $("input[id*='txtAuthName']").val('');
    $("input[id*='txtAuthFax']").val('');
    $("input[id*='txtAuthEmail']").val('');
    $("input[id*='txtAuthTitle']").val('');
}



function ToggAuthenticatorsShow() {
    if ($(".auth.collapse.in").length <= 0)
        $(".collapse.auth").collapse('toggle');
    $('body, html').animate({ scrollTop: 180 }, 800);
}

function SaveGeneral() {
    if (!$("#ctl01").valid()) {
        alert('Lütfen gerekli alanları doldurunuz');
        return;
    }
    TaxNumberValid();
    var customerId = 0;
    if ($("a[id*='btnSave']").text().trim() == "Güncelle") {
        customerId = parseInt($("input[id*='hdnCustomerId']").val());
        if (($("input[id*='hdnGeneralEdit']").val().trim() == "0")) {
            alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
            return;
        }
    }
    else if (($("input[id*='hdnGeneralInsert']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }
    btnSaveHide(true);
    var hsldurumclientid = $("input[id*='rblHaselDurum']");
    var rblmaincutid = $("input[id*='rblMainCustomer']");
    var isMainCustomer = $('#ContentPlaceHolder1_rblMainCustomer_0').prop('checked');
    var haselStatus = "";
    if ($('#ContentPlaceHolder1_rblHaselDurum_1').length != 0) {
        haselStatus = $('#ContentPlaceHolder1_rblHaselDurum_1').prop('checked') == true ? "Onaylı" : "Onaysız";
    } else {
        haselStatus = $("#ContentPlaceHolder1_spnOnaysiz").length == 1 ? "Onaysız" : "Onaylı";
    }

    if (haselStatus == 'Onaylı') {
        var merkezCount = 0;
        $('#locContentTable tr').each(function () {
            if (TextHasMerkez(this.cells[0].innerText))
                merkezCount++;
        });
        if (merkezCount < 1) {
            alert("Lokasyonlar sekmesinde MERKEZ isimli lokasyon bulunamadı. Taslağın onaylanabilmesi için MERKEZ lokasyonu olmalıdır...");
            btnSaveHide(false); return;
        }
    }
    var customerName = $("input[id*='Cm_CustomerISIM']").val();
    var customerCode = $("input[id*='Cm_CustomerKOD']").val();
    var hslShort = $("input[id*='HSL_KISALTMA']").val();
    var taxOffice = $("input[id*='HSL_VD']").val();
    var taxNumber = $("input[id*='HSL_VN']").val();
    var sectorId = $("select[id*='ddHSL_SEKTORID']").val();
    var firmType = $("select[id*='ddHSL_FIRMA']").val();
    var hslCustomerCode = $("input[id*='Cm_CustomerKODO']").val();
    var hslCustomerCodeH = $("input[id*='Cm_CustomerKODH']").val();
    var webSite = $("input[id*='HSL_WEB']").val();
    var uid = parseInt($("#hdnUserId").val());



    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/SaveCustomer',
        data: "{customerId:" + customerId + ",isMainCustomer:" + isMainCustomer + ",haselStatus:'" + haselStatus + "',customerName:'" + customerName
        + "',customerCode:'" + customerCode + "',hslShort:'" + hslShort + "',taxOffice:'" + taxOffice + "',taxNumber:'" + taxNumber + "',sectorId:'" + sectorId + "',firmType:" + firmType + ",hslCustomerCode:'" + hslCustomerCode + "',hslCustomerCodeH:'" + hslCustomerCodeH + "',webSite:'" + webSite + "',uid:" + uid + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            debugger;
            var url = window.location.href;
            if (data.d.toString() !== "updated") {
                if (url.indexOf('?') <= -1) {
                    url = url + "?CId=" + data.d;
                } else {
                    var ind = url.indexOf('=');
                    var ext = url.substr(ind + 1, url.length - (ind + 1));
                    url = url.replace(ext, data.d);
                    url = url.replace("key", "CId");
                    url = url + "&key=firstLoad";
                    $("a[id*='cariWhois']").val(customerName);
                    $("input[id*='hdnCustomerId']").val(data.d);
                }
            }

            window.location.href = url;
            btnSaveHide(false);
            $("#ContentPlaceHolder1_btnSave").text('Güncelle');
            alert('İşlem başarı ile gerçekleştirilmiştir....!');
        },
        error: function (data) {
            alert(data.d);
        }
    });
}

function btnSaveHide(hide) {
    if ($("a[id*='btnSave']").html() === "Kaydet") {
        if (hide) {
            $("a[id*='btnSave']").hide();
        } else {
            $("a[id*='btnSave']").show();
        }

    }
}

function LocationSave(btnno) {
    if (!$("#ctl01").valid()) {
        alert('Lütfen gerekli alanları doldurunuz');
        return;
    }



    var customerId = parseInt($("input[id*='hdnCustomerId']").val());
    if (customerId.toString() == "NaN") {
        alert("Cari tanımı yapılmamış!..");
        return;
    }

    var workingmode = true;
    var str = $(".RACLE1").text().trim();
    if (str.indexOf('Güncelle') !== -1) {
        workingmode = false;
        customerId = locationIdTo;
        if (($("input[id*='hdnLocationEdit']").val().trim() == "0")) {
            alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
            return;
        }
    }
    else if (($("input[id*='hdnLocationInsert']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }

    var location = $("select[id*='ddLocCity']" + " option:selected").text();
    var region = $("select[id*='ddLocRegion']" + " option:selected").text();
    var address = $("input[id*='txtLocAddress']").val();
    var shordtdef = $("input[id*='txtLocDefinition']").val();

    var locCntTable1 = $("table[id*='locContentTable']");
    if (locCntTable1.find("tr").length <= 1 && !TextHasMerkez(shordtdef)) {
        alert("İlk girilen lokasyon, merkez lokasyon olarak belirlenmelidir. Eğer bu girdiğiniz kayıt merkez lokasyona ait değilse lütfen merkez öncelikle carinin merkez lokasyonunu \"Lokasyon Adı\" Merkez olacak şekilde giriniz.");
        return;
    }

    if (!isEmptyLocal($("[id*='txtLocLat']").val()) || !isEmptyLocal($("[id*='txtLocLong']").val())) {
        if (CoordinateValidator($("[id*='txtLocLat']").val()) && CoordinateValidator($("[id*='txtLocLong']").val())) { //enlem boylam bos olmucak

        } else {
            alert("Gecerli koordinat bilgisi giriniz. Girmis oldugunuz bilgi hatalidir.");
            return;
        }
    }


    //update gelen deger merkez degilse. bu validasyonu kontrol eder
    if (!$("input[id*='txtLocDefinition']").prop("readonly")) {
        if (TableHasValue("locContentTable", 0, "Merkez") && TextHasMerkez(shordtdef)  /*&& workingmode === trueUPDATE DE MERKEZ KONTROLU YAPMASI ICIN KALDIRILMISTIR*/) {
            alert("Merkez lokasyonu daha önce kaydedilmiş, lokasyona başka bir Kısa Tanım yapınız..");
            return;
        }
    }



    var tel = $("input[id*='txtLocTel']").val();
    var fax = $("input[id*='txtLocFax']").val();
    var longitude = $("input[id*='txtLocLong']").val();
    var latitude = $("input[id*='txtLocLat']").val();
    var faturakesebilir = $("input[id*='rbLocFat']");
    var faturakesebilirmi = faturakesebilir.find('input[type=radio]:checked').val() == 1 ? true : false;
    var uid = parseInt($("#hdnUserId").val());
    var isLocActive = true; // $('#ContentPlaceHolder1_rbIsLocActive_0').prop('checked');
    var isLocDeleted = $('#ContentPlaceHolder1_rbIsLocDeleted_0').prop('checked');

    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/SaveNewLocation',
        data: "{customerId:" + customerId + ",location:'" + (location) + "',region:'" + (region) + "',address:'" + address + "',shordtdef:'" + shordtdef + "',tel:'" + (tel) + "',fax:'" + (fax) + "',longitude:'" + latitude + "',latitude:'" + longitude + "',faturakesebilirmi:" + faturakesebilirmi + ",uid:" + uid + ",isLocActive:" + isLocActive + ",isLocDeleted:" + isLocDeleted + ",workingmode:" + workingmode + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            var locDiv = $("input[id*='locationList']");
            var locCntTable = $("table[id*='locContentTable']");
            if (locCntTable != null && locCntTable != 'undefined') {
                var lHtml = "<tr id=\"locTr" + data.d.Id + "\">"
                    + " <td>" + nullToEmpty(data.d.Name) + "</td>"
                    + " <td>" + nullToEmpty(data.d.CityName) + "</td>"
                    + " <td>" + nullToEmpty(data.d.RegionName) + "</td>"
                    + " <td>" + nullToEmpty(data.d.Phone) + "</td>"
                    + " <td>" + nullToEmpty(data.d.Fax) + "</td>"
                    + " <td><input class=\"btn btn-warning\" type=\"button\" value=\"" + ((data.d.IsFat == true) ? "Evet" : "Hayır") + "\" id=\"inpLocationStatus" + data.d.Id + "\" onclick=\"ChangeLocation('" + data.d.Id + "','inpLocationStatus" + data.d.Id + "');\" /></td>"
                    + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateLocationToTop('" + data.d.Id + "', '" + data.d.CityName + "','" + data.d.RegionName + "','" + data.d.Address + "','" + data.d.Name + "','" + nullToEmpty(data.d.Phone) + "','" + nullToEmpty(data.d.Fax) + "','" + data.d.Longitude + "','" + data.d.Latitude + "'," + data.d.IsFat + ", true," + data.d.IsDeleted + "); ToggleLocationShow();\" /></td>"
                    + "</tr>";
                locCntTable.find("tr").last().after(lHtml);
                if (workingmode == 0 && locationIdTo > 0)
                    removeElement(document.getElementById('locTr' + locationIdTo));

                var option = document.createElement('option');
                option.text = data.d.Name;
                option.value = data.d.Id;
                //yetkili tanimlamada lokasyon
                var locSelect = document.getElementById("ContentPlaceHolder1_ddAutLocation");
                var statusListChanged = false;

                for (var i = 0; i < locSelect.length; i++) {
                    if (locSelect[i].value === option.value) {
                        locSelect[i].innerHTML = option.text;
                        statusListChanged = true;
                    }
                }
                if (statusListChanged !== true) {
                    //$("#ContentPlaceHolder1_ddAutLocation").append(option);
                    locSelect.add(option, 0);
                }

                //makina parkinda konum
                var option2 = document.createElement('option');
                option2.text = data.d.Name;
                option2.value = data.d.Id;
                statusListChanged = false;
                var locMpSelect = document.getElementById("ContentPlaceHolder1_ddMpMachineparkLocation")
                for (var y = 0; y < locMpSelect.length; y++) {
                    if (locMpSelect[y].value === option2.value) {
                        locMpSelect[y].innerHTML = option2.text;
                        statusListChanged = true;
                    }
                }
                if (statusListChanged !== true) {
                    //$("#ContentPlaceHolder1_ddMpMachineparkLocation").append(option2);
                    locMpSelect.add(option2, 0);
                }


                var lCount = parseInt($("span[id*='spLocCount']").text().trim());
                if (workingmode == true)
                    $("span[id*='spLocCount']").text((lCount + 1).toString());

                $(".RACLE1").text('Kaydet ve Kapat');
                $(".RACLE2").text('Kaydet ve Yeni Kayıt Aç');
                SetDropWithCity("Seçiniz...", "ContentPlaceHolder1_ddLocCity");
                $("input[id*='ddLocRegion']").val('');
                $("input[id*='txtLocAddress']").val('');
                $("input[id*='txtLocDefinition']").val('');
                $("input[id*='txtLocTel']").val('');
                $("input[id*='txtLocFax']").val('');
                $("input[id*='txtLocLong']").val('');
                $("input[id*='txtLocLat']").val('');
                $(".RACLE3").css({ 'display': "none" });
                $('body, html').animate({ scrollTop: 180 }, 800);

                alert("Lokasyon bilgisi başarı ile kaydedilmiştir");
            }
            if (btnno == 1)
                $(".collapse.location").collapse('hide');
            else $(".collapse.location").collapse('show');

            sessionStorage.removeItem('city');
            sessionStorage.removeItem('region');
            sessionStorage.removeItem('tel');
            sessionStorage.removeItem('fax');
            sessionStorage.removeItem('VD');
            sessionStorage.removeItem('VN');
            sessionStorage.removeItem('firm');
            sessionStorage.removeItem('address');
            sessionStorage.removeItem('code');
        },
        error: function (data) {
            alert(data.d.Name)
        }
    });
}
//Tablo adi aranicak kolon index ve metni girerseniz size true false doner
function TableHasValue(tableSelector, coloumnIndex, textValue) {
    try {
        var status = false;
        var row = $('#' + tableSelector + ' tr');

        if (row.length > 1) {
            for (var i = 1; i < row.length; i++) {
                var tempCell = row[i].cells[coloumnIndex].innerText;
                if (!isEmptyLocal(tempCell)) {
                    if (tempCell.trim().toUpperCase() === textValue.trim().toUpperCase()) {
                        status = true;
                        break;
                    }
                }

            }
        }
        return status;
    } catch (e) {
        alert("Hata: lokasyon merkez validasyonunda hata olustu");
        return true;

    }

}
//Tablo adi aranicak kolon index ve metni girerseniz size o kolonda kactane o metinden var onu soyler
function TableValueCount(tableSelector, coloumnIndex, textValue) {
    try {
        var row = $('#' + tableSelector + ' tr');
        var top = 0;
        if (row.length > 1) {
            for (var i = 1; i < row.length; i++) {
                var tempCell = row[i].cells[coloumnIndex].innerText;
                if (!isEmptyLocal(tempCell)) {
                    if (tempCell.trim().toUpperCase() === textValue.trim().toUpperCase()) {
                        top++;
                    }
                }

            }
        }
        return top;
    } catch (e) {
        alert("Hata: lokasyon merkez validasyonunda hata olustu");
        return true;
    }

}

function SetDropWithCity(textToFind, name) {
    var dd = document.getElementById(name);
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].text === textToFind) {
            dd.selectedIndex = i;
            selectedCityId = dd.options[i].value;
            if (selectedCityId == 0) {
                SetDropText(".", "ContentPlaceHolder1_ddLocRegion");
            }
            break;
        }
    }
}

function SetDropText(textToFind, name) {
    var dd = document.getElementById(name);
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].text === textToFind) {
            dd.selectedIndex = i;
            if (dd.options[i].value == 0)
                dd.options[i].disabled = true;
            break;
        }
    }
}

function SetDropVal(valToFind, name) {
    var dd = document.getElementById(name);
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].value === valToFind) {
            dd.selectedIndex = i;
            break;
        }
    }
}

var selectedCityId = 0;
var locationIdTo = 0;
function UpdateLocationToTop(id, ddLocCityS, ddLocRegionS, txtLocAddressS, txtLocDefinitionS, txtLocTelS, txtLocFaxS, txtLocLongS, txtLocLatS, rbLocFatB, rbIsLocActiveB, rbIsLocDeletedB) {

    SetDropWithCity(ddLocCityS, 'ContentPlaceHolder1_ddLocCity');
    var cityName = $("select[id*='ddLocCity']  option:selected").text();
    $.ajax({
        type: "POST",
        //url: '/HaselSOAService.asmx/LoadRegions',
        url: '/HaselSOAService.asmx/LoadDistrict',
        //data: "{bolVal:" + selectedCityId + "}",
        data: "{cityName:'" + cityName + "'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            var dat = JSON.parse(data.d);
            $("select[id*='ddLocRegion']").find('option').remove();
            if (dat.length > 0) $("select[id*='ddLocRegion']").append($('<option></option>').val("").html("Seçiniz"));
            $(dat).each(function (index, item) {
                $("select[id*='ddLocRegion']").append($('<option></option>').val(item.Id).html(item.RegionName));
            });
            SetDropWithCity(ddLocRegionS, 'ContentPlaceHolder1_ddLocRegion');
        },
        error: function (data) {
            alert(data.d)
        }
    });

    var rbSEAktivity1 = $("input[id*='rbIsLocActive']");
    rbSEAktivity1.val([rbIsLocActiveB == "True" ? 1 : 2]);
    var rbLocFatB1 = $("input[id*='rbLocFat']");
    rbLocFatB1.val([rbLocFatB == "True" ? 1 : 2]);
    var rbIsLocDeletedB1 = $("input[id*='rbIsLocDeleted']");
    rbIsLocDeletedB1.val([rbIsLocDeletedB == "True" ? 1 : 2]);

    document.getElementById('ContentPlaceHolder1_txtLocAddress').value = txtLocAddressS.toString();
    document.getElementById('ContentPlaceHolder1_txtLocDefinition').value = txtLocDefinitionS.toString();
    document.getElementById('ContentPlaceHolder1_txtLocTel').value = txtLocTelS.toString();
    document.getElementById('ContentPlaceHolder1_txtLocFax').value = txtLocFaxS.toString();
    document.getElementById('ContentPlaceHolder1_txtLocLong').value = txtLocLatS.toString();
    document.getElementById('ContentPlaceHolder1_txtLocLat').value = txtLocLongS.toString();

    $(".RACLE1").text('Güncelle ve Kapat');
    $(".RACLE2").text('Güncelle ve Yeni Kayıt Aç');

    if (document.getElementById('ContentPlaceHolder1_hdnLocationDelete').value.toString().trim() == "1")
        $(".RACLE3").css({ 'display': "block" });

    locationIdTo = id;
    setTimeout(function () { initAutocomplete(); }, 200);

    //tek bir merkez lokasyonu varsa readonly olucak. birden fazla varsa editable olucaka

    var merkezCount = 0;
    $('#locContentTable tr').each(function () {

        if (TextHasMerkez(this.cells[0].innerText))
            merkezCount++;
    });


    if (TableValueCount("locContentTable", 0, "Merkez") === 1 && TextHasMerkez(txtLocDefinitionS)) {//LISTE 1 ADET MERKEZ VAR VE EDIT E GELEN MERKEZ ISE READONLY
        $("input[id*='txtLocDefinition']").prop("readonly", true);
    } else {
        $("input[id*='txtLocDefinition']").prop("readonly", false);
    }
}

function TextHasMerkez(mrk) {
    if (mrk.length > 0) {
        if (mrk.trim().toUpperCase() === "MERKEZ")
            return true;
    }
    return false;
    // if (mrk.trim().toUpperCase().indexOf("MERKEZ") > -1)

}

function LocationDelete() {
    if (confirm("Lokasyonu silmek istediğinizden emin misiniz!") === false) {
        return;
    }
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/LocationDelete',
        data: "{locationIdTo:" + locationIdTo + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.d.toString().indexOf('Merkez') == 0) {
                alert(data.d);
                return;
            }
            if (data.d.toString().indexOf('Bu') == 0) {
                alert(data.d);
                return;
            }
            removeElement(document.getElementById('locTr' + locationIdTo));
            alert("Lokasyon başarı ile silinmiştir");

            $(".RACLE1").text('Kaydet ve Kapat');
            $(".RACLE2").text('Kaydet ve Yeni Kayıt Aç');
            var lCount = parseInt($("span[id*='spLocCount']").text().trim());
            $("span[id*='spLocCount']").text((lCount - 1).toString());

            $(".RACLE3").css({ 'display': "none" });




            $("#ContentPlaceHolder1_ddAutLocation option[value='" + locationIdTo + "']").remove();
            $("#ContentPlaceHolder1_ddMpMachineparkLocation option[value='" + locationIdTo + "']").remove();
        },
        error: function (data) {
            alert(data.d);
        }
    });
}

function TaxNumberValid() {
   
    var cid = getQueryStringByName("cid");
    if (isEmptyLocal(cid)) {
        cid = 0;
    }
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/TaxNumberValid',
        data: "{strTaxNumber:'" + $("#ContentPlaceHolder1_HSL_VN").val() + "', customerId:"+parseInt(cid)+"}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.d.length > 0) {
                alert(data.d);
            }
        }
    });
}

function ChangeLocation(locId, inputName) {
    if (($("input[id*='hdnLocationEdit']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/ChangeLocation',
        data: "{locId:" + locId + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            $('#locContentTable tr').each(function () {
                if (this.cells[5].children.length > 0)
                    this.cells[5].children[0].value = 'Hayır';
            });
            var faturayesno = data.d.toString().toUpperCase();
            var result = "";
            if (faturayesno == "TRUE")
                result = "Evet";
            else result = "Hayır";

            document.getElementById(inputName).value = result;
            alert("Ana lokasyon değiştirilmiştir...");
        },
        error: function (data) {
            alert(data.d)
        }
    });
}

function GetLocations(locId) {
    var longControlName = 'inpLong' + locId;
    var latControlName = 'inpLat' + locId;
    document.getElementById(longControlName).value = $("input[id*='txtLocLong']").val();
    document.getElementById(latControlName).value = $("input[id*='txtLocLat']").val();
}

function UpdateLocation(locId) {
    if (($("input[id*='hdnLocationEdit']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }
    var longControlName = 'inpLong' + locId;
    var latControlName = 'inpLat' + locId;
    var long = document.getElementById(longControlName).value;
    var lat = document.getElementById(latControlName).value;
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/UpdateCustomerCoLocation',
        data: "{locId:" + parseInt(locId) + ",Longitude:" + lat + ",Latitude:" + long + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.d == true)
                alert("Lokasyon bilgisi başarı ile eklenmiştir");
            else alert(data.d);
        },
        error: function (data) {
            alert(data.d)
        }
    });
}

var global_TaxNumber = "";
$(document).ready(function () {
    $(".locationstab").click(function () {
        setTimeout(function () { initAutocomplete(); }, 200);
        if ($(".location.collapse.in").length <= 0)
            $("#MyMap").slideToggle("slow");
    });

    //$("input[id*='txtLocLong']").editableSelect();

    $("#tabEngineer").click(function () {
    });

    //onayli onaysiz cari option gizlendiginde o konteyniri komple kaldiriyoruz
    if ($("#ContentPlaceHolder1_rblHaselDurum").attr("style") == "display:none" || $("#ContentPlaceHolder1_rblHaselDurum").attr("style") == "visibility: hidden;") {
        $("#divDurum").remove();
    }

    if ($("#ContentPlaceHolder1_rblHaselDurum")[0] == undefined) {
        $("#divDurum").remove();
    }


    glbTaxNumber = $("#ContentPlaceHolder1_HSL_VN").val();


});

$(document).ready(function () {
    $('#ctl01').validate({
        rules: {
            ctl00$ContentPlaceHolder1$Cm_CustomerISIM: {
                minlength: 5,
                required: true
            },
            ctl00$ContentPlaceHolder1$txtMpMachineparkCount: {
                required: true
            },
            ContentPlaceHolder1_txtLocDefinition: {
                required: true
            },
            ContentPlaceHolder1_ddLocCity: {
                required: true
            },
            ContentPlaceHolder1_ddLocRegion: {
                required: true
            },
            ContentPlaceHolder1_txtAuthName: {
                required: true
            },
            ContentPlaceHolder1_ddAutLocation: {
                required: true
            },
            //ContentPlaceHolder1_ddMpMachineparkLocation: {
            //    required: true
            //},
            ContentPlaceHolder1_ddSalesEngineeers: {
                required: true
            },
            //ctl00$ContentPlaceHolder1$ddMpMarks: {
            //    required: true
            //},
            ContentPlaceHolder1_ddSalesmanTypes: {
                required: true
            },
            ContentPlaceHolder1_ddHSL_FIRMA: {
                required: true
            },
            ContentPlaceHolder1_HSL_KISALTMA: {
                required: true
            },
            ContentPlaceHolder1_ddHSL_SEKTORID: {
                required: true
            },
            ctl00$ContentPlaceHolder1$txtAuthGsm: {
                required: false,
                digits: true,
                maxlength: 10
            },
            ctl00$ContentPlaceHolder1$txtAuthPhone: {
                required: false,
                digits: true,
                maxlength: 10
            },
            ctl00$ContentPlaceHolder1$txtAuthEmail: {
                required: false,
                email: true
            },
            ctl00$ContentPlaceHolder1$txtLocTel: {
                required: false,
                digits: true,
                maxlength: 10
            },
            ctl00$ContentPlaceHolder1$txtLocFax: {
                required: false,
                digits: true,
                maxlength: 10
            },
        },
        highlight: function (element) {
            $(element).removeClass('success').addClass('error');
        },
        success: function (element) {
            $(element).removeClass('error').addClass('success');
        },
        //errorElement: "div",
        //wrapper: "div class=\"message\"",
        //errorPlacement: function (error, element) {
        //    offset = element.offset();
        //    error.insertBefore(element);
        //    //error.addClass('message');  // add a class to the wrapper
        //    error.css('position', 'absolute');
        //    error.css('left', offset.left + element.outerWidth() + 5);
        //    error.css('top', offset.top - 3);
        //}
    });
});

$(document).ready(function () {
    $("select[id*='ddLocCity']").change(function () {
        var cityName = $("select[id*='ddLocCity']  option:selected").text(); // $("select[id*='ddLocCity']").val();
        $.ajax({
            type: "POST",
            // url: '/HaselSOAService.asmx/LoadRegions', 
            url: '/HaselSOAService.asmx/LoadDistrict',
            data: "{cityName:'" + cityName + "'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                var dat = JSON.parse(data.d);
                $("select[id*='ddLocRegion']").find('option').remove();
                if (dat.length > 0) $("select[id*='ddLocRegion']").append($('<option></option>').val("").html("Seçiniz"));
                $(dat).each(function (index, item) {
                    $("select[id*='ddLocRegion']").append($('<option></option>').val(item.Id).html(item.RegionName));
                });
            },
            error: function (data) {
                alert(data.d)
            }
        });
    });

    $("select[id$='ddSalesmanTypes']").change(function () {
        SalesmanTypesBindForPopup("ddSalesmanTypes", "ddSalesEngineeers");
    });


    $("select[id$='pddSalesmanTypesPopup']").change(function () {
        SalesmanTypesBindForPopup("pddSalesmanTypesPopup", "pddSalesEngineeersPopup");
    });

    $("select[id*='ddLocRegion']").change(function () {
        var region = $("#ContentPlaceHolder1_ddLocRegion option:selected").text();
        if (document.getElementById('pacinputInvisible') != null) {
            document.getElementById('pacinputInvisible').value = region;
            document.getElementById('pacinputInvisible').focus();
            setTimeout(function () { ClickAutoCompleteItem(); }, 1000);
        }

    });

});

$(document).ready(function() {
    $("#ContentPlaceHolder1_ddInterviewUser > option").each(function (e) {
        if (this.value == "0") {
            this.value = ""
        }

    })
});

function SalesmanTypesBind(ctrlTypName, ctrlEnginerName) {
   
    var otid = parseFloat($("select[id$='" + ctrlTypName + "']").val());
    if (!isEmptyLocal(otid)) {

         $.ajax({
             type: "POST",
             url: '/HaselSOAService.asmx/GetSalesmansByOperationTypeId',
             data: "{operationId:" + otid + " }",
             contentType: 'application/json; charset=utf-8',
             dataType: 'json',
             success: function (data) {
                 $("select[id$='" + ctrlEnginerName + "']").find('option').remove();
                 $("select[id$='" + ctrlEnginerName + "']").append($('<option></option>').val("-1").html("Seçiniz"));
                 $(data.d).each(function (index, item) {
                     if (item.Id == 0)
                         $("select[id$='" + ctrlEnginerName + "']").append($('<option disabled></option>').val(item.Id).html(item.UserName));
                     else $("select[id$='" + ctrlEnginerName + "']").append($('<option></option>').val(item.Id).html(item.UserName));
                 });
             },
             error: function (data) {
                 alert("satis muhendisi yuklenirken hata olustu " + ctrlTypName + " " + ctrlEnginerName + data.d);
             }
         });
    }
   
}
function FlagButton_Click(btnId) {
    bootbox.dialog({
        message: "Bayrağı bu satıcıya devretmek istediğinizden emin misiniz?",
        title: "Onay",
        buttons: {
            success: {
                label: "Evet!",
                className: "green",
                callback: function () {
                    $(btnId).parent().prev().prev().find("button").trigger("click");
                    FlagDisableProcesdure();
                }
            },
            danger: {
                label: "Hayır",
                className: "red",
                callback: function () {

                }
            }
        }
    });
}


function SalesmanTypesBindForPopup(ctrlTypName, ctrlEnginerName) {
    var lUserid = parseInt($("[id$='hdnUserId']").val());
    var otid = parseFloat($("select[id$='" + ctrlTypName + "']").val());
    $("select[id$='" + ctrlEnginerName + "']").find('option').remove();
    if (String(otid) != "NaN") {
          $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/GetSalesmansByOperationTypeIdForpopup',
        data: "{operationId:" + otid + ", userId:" + lUserid + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
        
            $("select[id$='" + ctrlEnginerName + "']").append($('<option></option>').val("-1").html("Seçiniz"));
            $(data.d).each(function (index, item) {
                if (item.Id === 0) {
                    $("select[id$='" + ctrlEnginerName + "']")
                        .append($('<option disabled></option>').val(item.Id).html(item.UserName));
                } else {
                    $("select[id$='" + ctrlEnginerName + "']").append($('<option></option>').val(item.Id).html(item.UserName));
                }

            });
        },
        error: function (data) {
            alert("satis muhendisi yuklenirken hata olustu " + ctrlTypName + " " + ctrlEnginerName + data.d);
        }
    });
    }
  
}

function salesmanPopupInsert() {
   
    var customerId = getQueryStringByName("cid");
    var salesMan = parseInt($("select[id$='pddSalesEngineeersPopup']" + " option:selected").val());
    var salesType = parseInt($("select[id*='pddSalesmanTypesPopup']" + " option:selected").val());
    if ($("select[id$='pddSalesEngineeersPopup']").val() == "-1" || $("select[id$='pddSalesmanTypesPopup']").val() == "") {
        alert("Lütfen Operasyon Tipi ve Satış Mühendisi seçiniz");
        return;
    }
    //if (isEmptyLocal(salesMan) && isEmptyLocal(salesType)) { //sorun verdi
    //    alert("Lütfen satış elemanı seçiniz");
    //    return;
    //}
    var uid = parseInt($("#hdnUserId").val());


    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/SalesmanSave',
        data: "{customerId:" + customerId + ",selesDirector:0,salesMan:'" + salesMan + "',salesType:" + salesType + ",salesFlag:true ,salesAktivity:" + true + ",saleDeleted:false,workingmode:1,uid:" + uid + ",rowId:0}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (data) {
            var mes = data.d.toString();
            location.reload();

        },
        error: function (data) {
            alert("hata" + data);
        }
    });
}

function ClickAutoCompleteItem() {
    //var searchBox = new google.maps.places.SearchBox(document.getElementById('pacinputInvisible'));
    //$("#autoComplete").trigger(downKeyEvent);
    if (document.getElementsByClassName('pac-item-query')[0] !== undefined) {//js error veriyordu.
        document.getElementsByClassName('pac-item-query')[0].click();
    }

}

$(document).ready(function () {
    $('#salesmanContentTable tr').each(function () {
        $(this).children().find("i").parent().attr("disabled", true);

    });
});
function FlagDisableProcesdure() {
    $('#salesmanContentTable tr').each(function () {
        $(this).children().find("i").parent().attr("disabled", true);
    });
    var indextr = 0;
    var indextr2 = 0;
    $('#salesmanContentTable tr').each(function () {
        if ($("input[id$=hdnFlagEditAuth").val() == "1") {

            if (indextr !== 0) { //runtime 
                var elementI = $(this).children().find("i");
                if (elementI.attr('class').indexOf('fa-flag-o') > -1) {
                    $(this.cells[4]).find("input").css({ "visibility": "visible" });
                } else {
                    $(this.cells[4]).find("input").css({ "visibility": "hidden" });
                }
            } else {
                $(this.cells[4]).css({ "visibility": "visible" });
            }
        }

        if ($("input[id$=hdnFlagEditAuth").val() == "0") {
            if (indextr2 !== 0) {
                $(this.cells[4]).find("input").css({ "visibility": "hidden" });
            } else {
                $(this.cells[4]).css({ "visibility": "visible" });
            }

        }
        indextr++;
        indextr2++;
    });



    ///*mailden dolayi kapanmistir*/
    //$("#salesmanContentTable").find("i").parent().attr("disabled", true);

    //var types = TypeHtmlCount();
    //for (var q = 0; q < types.length; q++) {
    //    if (types[q] === " Tipi" || types[q] === "") {
    //        continue;
    //    }

    //    //markalarin sayisini tespit eder
    //    var arrCount = $.grep(types, function (elem) {
    //        return elem === types[q];
    //    }).length;

    //    if (arrCount === 1) {
    //        $('#salesmanContentTable tr').each(function () {
    //            if (this.cells[1].innerHTML === types[q]) {
    //                $(this).children().find("i").parent().attr("disabled", true);
    //            }
    //        });
    //    }

    // }
    //ayni grubdan 
    //if (count === 1) {
    //    $("#salesmanContentTable").find("i").parent().attr("disabled", true);
    //} else {
    //    $("#salesmanContentTable").find("i").parent().attr("disabled", false);

    //}
}

function AuthenticatorSave(btnno) {
    if (!$("#ctl01").valid()) {
        alert('Lütfen gerekli alanları doldurunuz');
        return;
    }

    var customerId = parseInt($("input[id*='hdnCustomerId']").val());
    if (customerId.toString() == "NaN") {
        alert("Cari tanımı yapılmamış!..");
        return;
    }

    var workingmode = 1;
    var str = $(".RACA1").text().trim();
    if (str.indexOf('Güncelle') !== -1) {
        workingmode = 0;
        customerId = authenticatorId;
        if (($("input[id*='hdnAuthenticatorEdit']").val().trim() == "0")) {
            alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
            return;
        }
    }
    else if (($("input[id*='hdnAuthenticatorInsert']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }

    var authLocation = parseInt($("select[id*='ddAutLocation']" + " option:selected").val());
    var locationName = $("select[id*='ddAutLocation']" + " option:selected").text();
    var authName = $("input[id*='txtAuthName']").val();
    var authGsm = $("input[id*='txtAuthGsm']").val();
    var authPhone = $("input[id*='txtAuthPhone']").val();
    var authFax = $("input[id*='txtAuthFax']").val();
    var authEmail = $("input[id*='txtAuthEmail']").val();
    var authTitle = $("input[id*='txtAuthTitle']").val();
    var vm = parseInt(workingmode);
    var uid = parseInt($("#hdnUserId").val());
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/AuthenticatorSave',
        data: "{customerId:" + customerId + ",authLocation:" + authLocation + ",authName:'" + authName + "',authGsm:'" + authGsm + "',authPhone:'" + authPhone + "',authFax:'" + authFax + "',authEmail:'" + authEmail + "',authTitle:'" + authTitle + "',workingmode:" + vm + ",uid:" + uid + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            var authCntTable = $("table[id*='authContentTable']");
            if (authCntTable != null && authCntTable != 'undefined') {
                var lHtml = "<tr id=\"authTr" + data.d.Id + "\">"
                   + " <td>" + locationName + "</td>"
                   + " <td>" + data.d.Name + "</td>"
                   + " <td>" + data.d.Gsm + "</td>"
                   + " <td>" + data.d.Phone1 + "</td>"
                   + " <td>" + data.d.Fax + "</td>"
                   + " <td>" + data.d.Email + "</td>"
                   + " <td>" + data.d.Title + "</td>"
                   + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateAuthenticator('" + data.d.Id + "', " + data.d.CustomerLocationId + ",'" + data.d.Name + "','" + data.d.Gsm + "','" + data.d.Phone1 + "','" + data.d.Fax + "','" + data.d.Email + "','" + data.d.Title + "'); ToggAuthenticatorsShow();\" /></td>"
                   + "</tr>";
                authCntTable.find("tr").last().after(lHtml);
                if (workingmode == 0 && authenticatorId > 0)
                    removeElement(document.getElementById('authTr' + authenticatorId));
                var lCount = parseInt($("span[id*='spAuthCount']").text().trim());
                $("span[id*='spAuthCount']").text((lCount + 1).toString());

                $(".RACA1").text('Kaydet ve Kapat');
                $(".RACA2").text('Kaydet ve Yeni Kayıt Aç');
                $(".RACA3").css({ 'display': "none" });
                SetDropText("Seçiniz...", "ContentPlaceHolder1_ddAutLocation");
                $("input[id*='txtAuthGsm']").val('');
                $("input[id*='AuthPhone']").val('');
                $("input[id*='txtAuthName']").val('');
                $("input[id*='txtAuthFax']").val('');
                $("input[id*='txtAuthEmail']").val('');
                $("input[id*='txtAuthTitle']").val('');
                $('body, html').animate({ scrollTop: 180 }, 800);

                var option2 = document.createElement('option');
                option2.text = data.d.Name;
                option2.value = data.d.Id;
                var status = false;
                var interviewAuth = document.getElementById("ContentPlaceHolder1_ddInterviewAuthenticator");
                for (var y = 0; y < interviewAuth.length; y++) {
                    if (interviewAuth[y].value === option2.value) {
                        interviewAuth[y].innerHTML = option2.text;
                        status = true;
                    }
                }
                if (status !== true) {
                    interviewAuth.add(option2, 0);
                }


                alert("Yetkili bilgisi başarı ile kaydedilmiştir...");
            }
            if (btnno == 1)
                $(".collapse.auth").collapse('hide');
            else $(".collapse.auth").collapse('show');
        },
        error: function (data) {
            alert(data.d.Name);
        }
    });
}
var authenticatorId = 0;
function UpdateAuthenticator(id, locId, name, gsm, phone, fax, email, title) {
    //$("select[id*='ddAutLocation']" + " option:selected").val(locId);

    if (locId == undefined) {
        alert("Bu yetkilinin lokasyonu bulunamamistir.");
    } else {
        SetDropVal(locId.toString(), "ContentPlaceHolder1_ddAutLocation");
    }

    $("input[id*='txtAuthName']").val(name);
    $("input[id*='txtAuthGsm']").val(gsm);
    $("input[id*='txtAuthPhone']").val(phone);
    $("input[id*='txtAuthFax']").val(fax);
    $("input[id*='txtAuthEmail']").val(email);
    $("input[id*='txtAuthTitle']").val(title);
    $(".RACA1").text('Güncelle ve Kapat');
    $(".RACA2").text('Güncelle ve Yeni Kayıt Aç');
    authenticatorId = id;

    if (document.getElementById('ContentPlaceHolder1_hdnAuthenticatorDelete').value.toString().trim() == "1")
        $(".RACA3").css({ 'display': "block" });
}

var SalesmanDto = (function () {
    function SalesmanDto(id, salesmanId, typeId) {
        this.id = id;
        this.salesmanId = salesmanId;
        this.typeId = typeId;
    }
    return SalesmanDto;
}());


function SalesmanInsertUpdateValidator() {
    var salesmanTypes = $("[id$=ddSalesmanTypes]");
    var ddSalesEngineers = $("[id$=ddSalesEngineeers]");
    if (salesmanTypes.val() !== "" && ddSalesEngineers.val() !== "") {
        var recordList = [];
        $("#salesmanContentTable").find("input").each(function (item) {

            var inputElm = this;
            if (inputElm.id.indexOf("btnFlag") === -1) {//yeni gelen flag butonuna dahil etmemesi icin
                var arrAttr = inputElm.getAttribute("onclick").split(",");//"UpdateSalesman('58256', 676,'1','True','True','False','Linde'); ToggSalesmanShow();"
                //console.log("sales: " + arrAttr[1] + " tip: " + arrAttr[2]);

                var salesman = new SalesmanDto(
                    arrAttr[0].replace("UpdateSalesman('", "").replace("'", ""),
                    arrAttr[1].replace(" ", ""),
                    arrAttr[2].replace("'", "").replace("'", ""));

                recordList.push(salesman);
            }

        });
        var status = true;
        $.each(recordList, function (key, itemRecord) {
            if (itemRecord.salesmanId === ddSalesEngineers.val() && itemRecord.typeId === salesmanTypes.val() && salesmanId !== itemRecord.id) {
                status = false;
                return false;//loop break
            }
        });

        return status;

    }
    return true;


}

function SalesmanSave(btnno) {

    FlagDisableProcesdure();
    if (!$("#ctl01").valid()) {
        alert('Lütfen gerekli alanları doldurunuz');
        return;
    }

    if (SalesmanInsertUpdateValidator() === false) {
        alert("Bu satici bu tipde mevcuttur");
        return;
    }

    var customerId = parseInt($("input[id*='hdnCustomerId']").val());
    if (customerId.toString() == "NaN") {
        alert("Cari tanımı yapılmamış!..");
        return;
    }

    var workingmode = 1;
    var str = $(".RACSE1").text().trim();
    if (str.indexOf('Güncelle') !== -1) {
        workingmode = 0;
        customerId = salesmanId;
        if (($("input[id*='hdnEngineerEdit']").val().trim() == "0")) {
            alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
            return;
        }
    }
    else if (($("input[id*='hdnEngineerInsert']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }

    var selesDirector = 0;
    var salesMan = parseInt($("select[id*='ddSalesEngineeers']" + " option:selected").val());
    var salesType = parseInt($("select[id*='ddSalesmanTypes']" + " option:selected").val());

    var salesFlag = $('#ContentPlaceHolder1_rbSEFlag_0').prop('checked');
    var salesAktivity = $('#ContentPlaceHolder1_rbSEAktivity_0').prop('checked');
    var saleDeleted = $('#ContentPlaceHolder1_rbSEDeleted_0').prop('checked');
  
    var saleTypeName = $("select[id*='ddSalesmanTypes']" + " option:selected").text();
    var typedSalesmanCount = 0;
    $('#salesmanContentTable tr').each(function () {
        //if (this.cells[1].innerText.trim() == saleTypeName)
        typedSalesmanCount++;
    });

    if (isFlagedSalesman == "True" && typedSalesmanCount == 1) {
        alert("Bu satıcı geçerli kategoride tek BAYRAKLI satıcı olduğu için bayrağı kaldıramazsınız, öncelikle bu kategoride başka bir satıcı tanımlayarak BAYRAĞI başka bir satıcıya TAŞIMALISINIZ...");
        return;
    }

    var vm = parseInt(workingmode);
    var uid = parseInt($("#hdnUserId").val());

    var typeColor = GetColor($("[id*=ddSalesmanTypes]>option:selected").html());



    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/SalesmanSave',
        data: "{customerId:" + customerId + ",selesDirector:" + selesDirector + ",salesMan:'" + salesMan + "',salesType:" + salesType + ",salesFlag:" + salesFlag + ",salesAktivity:" + salesAktivity + ",saleDeleted:" + saleDeleted + ",workingmode:" + vm + ",uid:" + uid + ", rowId:"+salesmanId+"}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (data) {
            var mes = data.d.toString();
            if (mes.indexOf('Dikkat!') >= 0) {
                alert(mes);
                return;
            }
            document.getElementById('ContentPlaceHolder1_ddSalesEngineeers').selectedIndex = 0;
            document.getElementById('ContentPlaceHolder1_ddSalesmanTypes').selectedIndex = 0;
            var rbFlager1 = $("input[id*='rbSEFlag']");
            rbFlager1.find('input[type=radio]:checked').text('');
            var rbSEAktivity1 = $("input[id*='rbSEAktivity']");
            rbSEAktivity1.find('input[type=radio]:checked').text('');
            var rbSEDeleted1 = $("input[id*='rbSEDeleted']");
            rbSEDeleted1.find('input[type=radio]:checked').text('');
            var salesmanCntTable = $("table[id*='salesmanContentTable']");
            if (salesmanCntTable != null && salesmanCntTable != 'undefined') {
                if (data.d.Flag == true) {
                    $('#salesmanContentTable tr').each(function () {
                        if (this.cells[2].children != undefined && this.cells[1].innerText.trim() == data.d.Type)
                            this.cells[2].children[0].children[0].src = "http://localhost:3612/images/flag_red_flu.png";
                    });
                }

                //   var src = (data.d.Flag == true) ? "../../images/flag_red.png" : "../../images/flag_red_flu.png";
                var flagClass = (data.d.Flag == true) ? "fa fa-flag" : "fa fa-flag-o";

                var lHtml = "<tr class=" + typeColor.textClass + " id=\"salesTr" + data.d.Id + "\">"
                    + " <td>" + data.d.Name + "</td>"
                    + " <td>" + data.d.Type + "</td>"
                    + " <td Flaged='" + data.d.Flag + "'><button class=" + typeColor.buttonClass + " type=\"button\" onclick=\"ChangeSalesmanFlag('" + data.d.Id + "','" + data.d.SalesmanTypeId + "');\"><i class='" + flagClass + "' aria-hidden='true'></i></button></td>"
                    + " <td class=" + typeColor.textClass + "><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateSalesman('" + data.d.Id + "', " + data.d.SalesmanId + ",'" + data.d.SalesmanTypeId + "','" + data.d.Flag + "','" + data.d.IsActive + "','" + data.d.IsDeleted + "','" + data.d.Type + "'); ToggSalesmanShow();\" /></td>"
                    + " <td style='" + FlagEditAuth() + "'><input  id='btnFlag_" + data.d.Id + "'  style='" + FlagVisibility(data.d.Flag) + "' class=\"btn btn-success\" type=\"button\" value=\"Bayrak\" onclick=\"FlagButton_Click(this);\" /></td>"

                   + "</tr>";
                if (data.d.IsDeleted != true)
                    $("#salesmanContentTable tbody").append(lHtml);
                if (workingmode == 0 && salesmanId > 0)
                    removeElement(document.getElementById('salesTr' + salesmanId));
                var lCount = parseInt($("span[id*='spSaleEngineerCount']").text().trim());
                if (workingmode == 1)
                    $("span[id*='spSaleEngineerCount']").text((lCount + 1).toString());
                $("select[id*='ddAreaDirectos']").text('');

                $(".RACSE1").text('Kaydet ve Kapat');
                $(".RACSE2").text('Kaydet ve Yeni Kayıt Aç');
                SetDropText("Seçiniz...", "ContentPlaceHolder1_ddSalesEngineeers");
                //ClearDropDownItems("ContentPlaceHolder1_ddSalesmanTypes");
                $('body, html').animate({ scrollTop: 180 }, 800);

                alert("İşlem Başarı ile gerçekleştirilmiştir...");
            }
            if (btnno == 1)
                $(".collapse.salesman").collapse('hide');
            else $(".collapse.salesman").collapse('show');
        },
        error: function (data) {
            alert(data.d.Name);
        }
    });
    FlagDisableProcesdure();
}



function FlagEditAuth() {
    var tAuth = $("#hdnFlagEditAuth").val();
    if (tAuth === "1") {
        return styleShow;
    }

    if (tAuth === "0")
        return styleHide;

    return styleShow;
}


function FlagVisibility(localFlag) {
    if (isEmptyLocal(localFlag)) {
        return styleShow;
    }

    if (localFlag.Value)
        return styleHide;

    return styleShow;
}

function getPosition(string, subString, index) {
    return string.split(subString, index).join(subString).length;
}

var salesmanId = 0;
var isFlagedSalesman = false;
var isFlagedSalesmanGroup = false;
isFlagedSalesmanGroup2 = false;
var typeName = '';

function UpdateSalesman(id, SalesmanId, SalesmanTypeId, IsFlagged, IsActive, IsDeleted, typee) {
    var lUserid = parseInt($("[id$='hdnUserId']").val());
    debugger;
    isFlagedSalesmanGroup2 = false;
    SetDropVal(SalesmanTypeId.toString(), "ContentPlaceHolder1_ddSalesmanTypes");
    typeName = typee;
    var otid = parseFloat($("select[id*='ddSalesmanTypes']").val());
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/GetSalesmansByOperationTypeIdForpopup',
        data: "{operationId:" + SalesmanTypeId + ", userId:" + lUserid + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (data) {
            $("select[id$='ddSalesEngineeers']").find('option').remove();
            $(data.d).each(function (index, item) {
                $("select[id$='ddSalesEngineeers']").append($('<option></option>').val(item.Id).html(item.UserName));
            });
            SetDropVal(SalesmanId.toString(), "ContentPlaceHolder1_ddSalesEngineeers");

            var counter = 0;
            $('#salesmanContentTable tr').each(function () {
                if (this.cells[2].getAttribute('flaged') != null && this.cells[2].getAttribute('flaged').toString().toLowerCase() == "true")
                    counter++;
            });

            var counter2 = 0;
            $('#salesmanContentTable tr').each(function () {
                if (this.cells[1].innerText == typeName) {
                    counter2++;
                }
            });

            if (counter > 1)
                isFlagedSalesmanGroup = true;

            if (counter2 > 1)
                isFlagedSalesmanGroup2 = true;
        },
        error: function (data) {
            alert(data.d);
        }
    });

    isFlagedSalesman = IsFlagged;
    $(".RACSE1").text('Güncelle ve Kapat');
    $(".RACSE2").text('Güncelle ve Yeni Kayıt Aç');
    salesmanId = id;

    if (document.getElementById('ContentPlaceHolder1_hdnEngineerDelete').value.toString().trim() == "1")
        $(".RACSE3").css({ 'display': "block" });

    FlagDisableProcesdure();
}

function IsContains2Categories() {
    var mpCategories = "";
    var strTemp = "";
    $('#machineparkContentTable tr').each(function () {
        strTemp = this.cells[0].innerHTML;
        if (mpCategories.indexOf(strTemp) === -1) {
            mpCategories += strTemp + "|";
        }
    });

    var mpCats = mpCategories.split("|");
    var findCatsInCombo = "";
    for (var k = 1; k < mpCats.length; k++) {
        if (mpCats[k].trim() == 0)
            continue;
        var targetElementAttrName = mpCats[k].replace('.', '');

        var dd = document.getElementById("ContentPlaceHolder1_ddMpMachineparkType");
        for (var i = 0; i < dd.options.length; i++) {
            if (dd.options[i].getAttribute('cid') == null)
                continue;
            var isFound = false;
            if (dd.options[i].text.indexOf(targetElementAttrName) > -1) {
                if (dd.options[i].getAttribute('cid').split(',').length > 2) {
                    findCatsInCombo += dd.options[i].getAttribute('cid') + '|';
                    isFound = true;
                }
            }
            if (isFound)
                findCatsInCombo += '*';
        }
    }

    var foundCategories = findCatsInCombo.split('*');
    var targetDeletionCategory = typeName;

    var addedSalesmanCategories = "";
    var strTmp = "";
    $('#salesmanContentTable tr').each(function () {
        strTmp = this.cells[1].innerHTML;
        if (addedSalesmanCategories.indexOf(strTmp) == -1) {
            addedSalesmanCategories += strTmp + "|";
        }
    });

    var addedSalesmanCategories = addedSalesmanCategories.split('|');

    var fcount = 0;
    for (var k = 0; k < foundCategories.length; k++) {
        var fcat = foundCategories[k];
        if (fcat == null || fcat.trim() == "")
            continue;
        for (var i = 0; i < addedSalesmanCategories.length; i++) {
            var acat = addedSalesmanCategories[i];
            if (acat == null || acat.trim() == "")
                continue;
            if (fcat.indexOf(acat) > -1)
                fcount++;
        }
    }

    if (fcount > 1)
        return true;
    return false;
}

function SalesmanDelete() {
    FlagDisableProcesdure();
    if (confirm("Satıcıyı silmek istediğinizden emin misiniz!") == false) {
        return;
    }

    //  var IsContains2 = IsContains2Categories();
    //if (IsContains2 == true)
    //    typeName = "other";

    if (/*!IsContains2 &&*/ isFlagedSalesmanGroup2 == true && isFlagedSalesman.toString().toLocaleLowerCase() == "true") {
        alert('Bayraklı bir satıcıyı silemezsiniz!...');
        return;
    }
    var customerId = parseInt($("input[id*='hdnCustomerId']").val());
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/SalesmanDelete',
        data: "{salesmanId:" + salesmanId + ",customerId:" + customerId + ",operationType:'" + typeName + "'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.d.length > 10) {
                alert(data.d);
                return;
            }
            removeElement(document.getElementById('salesTr' + salesmanId));
            alert("Satıcı başarı ile silinmiştir");

            $(".RACSE1").text('Kaydet ve Kapat');
            $(".RACSE2").text('Kaydet ve Yeni Kayıt Aç');
            var lCount = parseInt($("span[id*='spSaleEngineerCount']").text().trim());
            $("span[id*='spSaleEngineerCount']").text((lCount - 1).toString());
            $(".RACSE3").css({ 'display': "none" });

        },
        error: function (data) {
            alert(data.d)
        }
    });
    FlagDisableProcesdure();
}

function AuthDelete() {
    if (confirm("Yetkiliyi silmek istediğinizden emin misiniz!") == false) {
        return;
    }

    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/AuthDelete',
        data: "{authenticatorId:" + authenticatorId + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            removeElement(document.getElementById('authTr' + authenticatorId));
            alert("Yetkili başarı ile silinmiştir");

            $(".RACA1").text('Kaydet ve Kapat');
            $(".RACA2").text('Kaydet ve Yeni Kayıt Aç');
            var lCount = parseInt($("span[id*='spAuthCount']").text().trim());
            $("span[id*='spAuthCount']").text((lCount - 1).toString());
            $(".RACA3").css({ 'display': "none" });

            $("#ContentPlaceHolder1_ddInterviewAuthenticator option[value='" + authenticatorId + "']").remove();
        },
        error: function (data) {
            alert(data.d)
        }
    });
}

$.arrayIntersect = function (a, b) {
    return $.grep(a, function (i) {
        return $.inArray(i, b) > -1;
    });
};

var mcahineParkId = 0;
function MachineParkSaveAndClose(btnno) {
    if ($("#ctl01").valid()) {
        var insertedOpTypes = MachineParkValidator();
        var cattr = $("select[id*='ddMpMachineparkType']" + " option:selected").attr("cid").split(',');
        var ccc = $.arrayIntersect(insertedOpTypes, cattr);
        if (ccc.length <= 1) {
            alert("Bu kategoride satıcı olmadığı için makine parkı girişi yapamazsınız!..");
            return;
        }
    }



    $("#ctl01").valid();
    var customerId = parseInt($("input[id*='hdnCustomerId']").val());
    if (customerId.toString() == "NaN") {
        alert("Cari tanımı yapılmamış!..");
        return;
    }

    var workingmode = 1;
    var str = $(".RACMP1").text().trim();
    if (str.indexOf('Güncelle') != -1) {
        workingmode = 0;
        customerId = mcahineParkId;
        if (($("input[id*='hdnMachineparkEdit']").val().trim() == "0")) {
            alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
            return;
        }
    }
    else if (($("input[id*='hdnMachineparkInsert']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }

    var salesmanCntTableRef = $("table[id*='salesmanContentTable']");
    var lenTr = parseInt(salesmanCntTableRef.find("tr").length);
    if (lenTr <= 1) {
        alert("Bu işlem için öncelikle satıcı bölümünden Satıcı tanımlamanız gerekmektedir...");
        return;
    }

    var mpCount = parseInt($("input[id*='txtMpMachineparkCount']").val());
    var categoryId = parseInt($("select[id*='ddMpMachineparkType']" + " option:selected").val());
    if (mpCount.toString().trim() == "NaN" || mpCount == 0) {
        return alert('Makine adeti girilmesi zorunludur!');
    }
    if (customerId.toString().trim() == "NaN" || customerId == 0) {
        return alert('Müşteri bilgileri öncelikle girilmesi zorunludur!');
    }
    if (categoryId.toString().trim() == "NaN" || categoryId == 0) {
        return alert('Kategori(Makine Türü) seçimi zorunludur!');
    }

    var saleDate = $("input[id*='dateSaleDate']").val();
    var releaseDate = $("input[id*='dateRelease']").val();
    var planedReleaseDate = $("input[id*='datePlanedRelease']").val();
    var isDateValid = MachineParkDateValidation(saleDate, releaseDate, planedReleaseDate);

    if (isDateValid.length > 0) {
        return alert(isDateValid);
    }

    var markId = 30;
    if ($("select[id*='ddMpMarks']" + " option:selected").val() != "") {
        markId = parseInt($("select[id*='ddMpMarks']" + " option:selected").val());
    }

    var modelName = $("input[id*='txtMpModel']").val();
    var serialNo = $("input[id*='txtMpSerialNo']").val();
    var year1 = parseInt($("select[id*='ddMpYears']" + " option:selected").text());
    if (isNaN(parseInt($("select[id*='ddMpYears']" + " option:selected").text())))
        year1 = 0;

    var ownerShip = parseInt($("select[id*='ddMpRetireOrOwnered']" + " option:selected").val());
    var mplocation = 0;
    if ($("select[id*='ddMpMachineparkLocation']" + " option:selected").text().indexOf('Seç') < 0)
        mplocation = parseInt($("select[id*='ddMpMachineparkLocation']" + " option:selected").val());
    var vm = parseInt(workingmode);
    var uid = parseInt($("#hdnUserId").val());
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/MachineparkSave',
        data: "{customerId:" + customerId + ",categoryId:" + categoryId + ",markId:" + markId + ",modelName:'" + modelName
            + "',serialNo:'" + serialNo + "',year:" + year1 + ",saleDate:'" + saleDate + "',mpCount:" + mpCount + ",ownerShip:"
            + ownerShip + ",mplocation:" + mplocation + ",workingmode:" + vm + ",uid:" + uid
            + ",releaseDate:'" + releaseDate + "',planedReleaseDate:'" + planedReleaseDate + "'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {

            if (data.d.Id == -1) {
                alert(data.d.ErrMsg);
                return;
            }
            $("input[id*='txtMpModel']").val('');
            $("input[id*='txtMpSerialNo']").val('');
            $("input[id*='txtMpMachineparkCount']").val('');

            var saleDateFormat = JsonToHtmlFormat(data.d.SaleDate);
            var releaseDateFormat = JsonToHtmlFormat(data.d.ReleaseDate);
            var planedReleaseDateFormat = JsonToHtmlFormat(data.d.PlanedReleaseDate);

            if (data.d != null)
                $('table#machineparkContentTable tr#' + data.d.Id + '').remove();
            var mpCntTable = $("table[id*='machineparkContentTable']");
            if (mpCntTable != null && mpCntTable != 'undefined') {
                var selectedYear;
                if (data.d.ManufactureYear != null) {
                    selectedYear = data.d.ManufactureYear == 0 ? "" : data.d.ManufactureYear.toString();
                }


                var lHtml = "<tr id=\"mpTr" + data.d.Id + "\">"
                    + " <td>" + data.d.CategoryName + "</td>"
                    + " <td>" + data.d.MarkName + "</td>"
                    + " <td>" + data.d.Model + "</td>"
                    + " <td>" + data.d.SerialNo + "</td>"
                    + " <td>" + selectedYear + "</td>"
                    + " <td>" + data.d.Quantity + "</td>"
                    + " <td>" + nullToEmpty(data.d.OwnerName) + "</td>"
                    + " <td>" + nullToEmpty(data.d.LocationName) + "</td>"
                    + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Güncelle\" onclick=\"UpdateMachinePark(" + data.d.Id + ", " + data.d.CategoryId + ",'" + data.d.MarkId + "','" + data.d.Model + "','" + data.d.SerialNo + "'," + data.d.ManufactureYear + "," + data.d.Quantity + "," + data.d.OwnerId + "," + data.d.LocationId + ",'" + saleDateFormat + "','" + releaseDateFormat + "','" + planedReleaseDateFormat + "'); ToggMachineparkShow();\" /></td>"
                    + " <td><input class=\"btn btn-success\" type=\"button\" value=\"Sil\" onclick=\"DeleteMachinePark(" + data.d.Id + ");\" /></td>"
                    + "</tr>";
                mpCntTable.find("tr").last().after(lHtml);
                if (workingmode == 0 && mcahineParkId > 0)
                    removeElement(document.getElementById('mpTr' + mcahineParkId));
                if (workingmode != 0) {
                    var lCount = parseInt($("span[id*='spMpCount']").text().trim());
                    $("span[id*='spMpCount']").text((lCount + 1).toString());
                }
            }

            $(".RACMP1").text('Kaydet ve Kapat');
            $(".RACMP2").text('Kaydet ve Yeni Kayıt Aç');
            $("input[id*='txtMpModel']").val('');
            $("input[id*='txtMpSerialNo']").val('');
            $("input[id*='txtMpMachineparkCount']").val('');
            SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpMachineparkType");
            SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpMarks");
            SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpYears");
            SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpRetireOrOwnered");
            SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpMachineparkLocation");

            alert("İşlem başarı ile gerçekleştirilmiştir");
            $('body, html').animate({ scrollTop: 180 }, 800);

            if (btnno == 1)
                $(".collapse.machinepark").collapse('hide');
            else $(".collapse.machinepark").collapse('show');
        },
        error: function (data) {

            alert(data.d.CategoryName);
        }
    });
}

function setSelectedValue(selectObj, valueToSet) {
    for (var i = 0; i < selectObj.options.length; i++) {
        if (selectObj.options[i].text == valueToSet) {
            selectObj.options[i].selected = true;
            return;
        }
    }
}

Date.prototype.formatDDMMYYYY = function () {
    return this.getFullYear()
    "/" + this.getMonth()
    "/" + (this.getDate() + 1);
}

function UpdateMachinePark(id, CategoryId, MarkId, Model, SerialNo, ManufactureYear, Quantity, OwnerId, LocationId, SaleDate, ReleaseDate, PlanedReleaseDate) {
    MachineParkformClean("update");

    mcahineParkId = id;
    document.getElementById('ContentPlaceHolder1_ddMpMachineparkType').value = CategoryId;
    if (MarkId != '')
        document.getElementById('ContentPlaceHolder1_ddMpMarks').value = MarkId;
    else document.getElementById('ContentPlaceHolder1_ddMpMarks').value = '';

    document.getElementById('ContentPlaceHolder1_txtMpSerialNo').value = SerialNo;
    setSelectedValue(document.getElementById('ContentPlaceHolder1_ddMpYears'), ManufactureYear.toString());

    var htmlDate = DateJsontoHtmlFormat(SaleDate);
    var htmlReleaseDate = DateJsontoHtmlFormat(ReleaseDate);
    var htmlPlanedReleaseDate = DateJsontoHtmlFormat(PlanedReleaseDate);

    if (!isEmptyLocal(htmlDate))
        document.getElementById('ContentPlaceHolder1_dateSaleDate').value = htmlDate;
    if (!isEmptyLocal(htmlReleaseDate))
        document.getElementById('ContentPlaceHolder1_dateRelease').value = htmlReleaseDate;
    if (!isEmptyLocal(htmlPlanedReleaseDate))
        document.getElementById('ContentPlaceHolder1_datePlanedRelease').value = htmlPlanedReleaseDate;

    document.getElementById('ContentPlaceHolder1_txtMpMachineparkCount').value = Quantity;
    document.getElementById('ContentPlaceHolder1_ddMpRetireOrOwnered').value = OwnerId;
    if (LocationId == 0) {
        SetDropText("Seçiniz...", "ContentPlaceHolder1_ddMpMachineparkLocation");
    }
    else document.getElementById('ContentPlaceHolder1_ddMpMachineparkLocation').value = LocationId;
    document.getElementById('ContentPlaceHolder1_txtMpModel').value = Model;

    $(".RACMP1").text('Güncelle ve Kapat');
    $(".RACMP2").text('Güncelle ve Yeni Kayıt Aç');
}


function DeleteConfirm() {
    return confirm("Silmek istediğinizden emin misiniz?")
}

function DeleteMachinePark(rowId) {
    if (confirm("Makine parkını silmek istediğinizden emin misiniz!") == false) {
        return;
    }
    if (($("input[id*='hdnMachineparkDelete']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }
    var uid = parseInt($("#hdnUserId").val());
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/DeleteMachinePark',
        data: "{rowId:" + rowId + ",uid:" + uid + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.d.length > 10) {
                alert(data.d);
                return;
            }

            $('table#machineparkContentTable tr#mpTr' + data.d + '').remove();
            var lCount = parseInt($("span[id*='spMpCount']").text().trim());
            $("span[id*='spMpCount']").text((lCount - 1).toString());
            alert("Makine Parkı Başarıyla silinmiştir");
        },
        error: function (data) {
            alert(data.d)
        }
    });
}

$(document).ready(function () {
    FlagDisableProcesdure();
    var my_param = getUrlParameter('key');
    if (my_param == "firstLoad") {
        if ($(".location.collapse.in").length <= 0)
            $(".collapse.location").collapse('toggle');
        $(".RACLE1").text('Kaydet ve Kapat');
        $(".RACLE2").text('Kaydet ve Yeni Kayıt Aç');
        $(".RACLE3").css({ 'display': "none" });

        if (document.getElementById('ContentPlaceHolder1_Cm_CustomerISIM').value.trim() == "")
            document.getElementById('ContentPlaceHolder1_Cm_CustomerISIM').value = sessionStorage.getItem('firm').toString().trimStart().trimEnd();
       
        if (sessionStorage.getItem('firm')!=null)
            document.getElementById('ContentPlaceHolder1_cariWhois').value = sessionStorage.getItem('firm').toString().trimStart().trimEnd();

        document.getElementById('ContentPlaceHolder1_txtLocAddress').value = sessionStorage.getItem('address');
        SetDropWithCity(sessionStorage.getItem('city'), 'ContentPlaceHolder1_ddLocCity');
        SetDropWithCity(sessionStorage.getItem('region'), 'ContentPlaceHolder1_ddLocRegion');
        var telWithExistable0 = "";
        if (sessionStorage.getItem('tel') != null) {
             telWithExistable0 = sessionStorage.getItem('tel').replace(/ /g, '');
        }
        
        if (telWithExistable0.length > 0 && telWithExistable0[0] == 0)
            telWithExistable0 = telWithExistable0.substr(1, telWithExistable0.length - 1);
        document.getElementById('ContentPlaceHolder1_txtLocTel').value = telWithExistable0.toString();
        document.getElementById('ContentPlaceHolder1_txtLocFax').value = sessionStorage.getItem('fax');
        document.getElementById('ContentPlaceHolder1_HSL_VD').value = sessionStorage.getItem('VD');
        document.getElementById('ContentPlaceHolder1_HSL_VN').value = sessionStorage.getItem('VN');

        if (isEmptyLocal(getQueryStringByName("CId"))) {
            $("#ContentPlaceHolder1_btnSave").text('Kaydet');
        }

        
        if (sessionStorage.getItem('identifier').indexOf('-H') > 0) {
            debugger;
            document.getElementById('ContentPlaceHolder1_Cm_CustomerKODH').value = Remove_Char_H_O(sessionStorage.getItem('code'));
            document.getElementById('ContentPlaceHolder1_Cm_CustomerKODH').style.color = "yellow";
            document.getElementById('ContentPlaceHolder1_Cm_CustomerKODH').style.backgroundColor = "#2b3643";
            $("[id$='ddHSL_FIRMA']").val("1");
        }
        else {
            debugger;
            document.getElementById('ContentPlaceHolder1_Cm_CustomerKODO').value = Remove_Char_H_O(sessionStorage.getItem('code'));
            document.getElementById('ContentPlaceHolder1_Cm_CustomerKODO').style.color = "yellow";
            document.getElementById('ContentPlaceHolder1_Cm_CustomerKODO').style.backgroundColor = "#2b3643";
                    $("[id$='ddHSL_FIRMA']").val("2");
        }

        var adr = sessionStorage.getItem('address');
        if (adr != null) {
            document.getElementById('ContentPlaceHolder1_txtLocAddress').value = adr;
            SetDropWithCity(sessionStorage.getItem('city'), 'ContentPlaceHolder1_ddLocCity');
            $.ajax({
                type: "POST",
                url: '/HaselSOAService.asmx/LoadDistrict',
                // url: '/HaselSOAService.asmx/GetRegionsByCityName',
                data: "{cityName:'" + sessionStorage.getItem('city').toString() + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    var dat = JSON.parse(data.d);
                    $("select[id*='ddLocRegion']").find('option').remove();
                    $(dat).each(function (index, item) {
                        $("select[id*='ddLocRegion']").append($('<option></option>').val(item.Id).html(item.RegionName));
                    });
                    SetDropWithCity(sessionStorage.getItem('region').toString().toUpperCase(), 'ContentPlaceHolder1_ddLocRegion');
                },
                error: function (data) {
                    alert(data.d);
                }
            });
            document.getElementById('ContentPlaceHolder1_txtLocFax').value = sessionStorage.getItem('fax');
            if ($(".location.collapse.in").length <= 0)
                $(".collapse.location").collapse('toggle');
        }
    }
});



function ChangeSalesmanFlag(id, flagTypeId) {

    var nlocalId = id;
    var groupTypeName = $("#salesTr" + id.toString()).children()[1].innerText;
    var targetButtonx;
    var newRowId = 0;
    if (($("input[id*='hdnEngineerEdit']").val().trim() == "0")) {
        alert("Bu işlem için yetkiniz yoktur, lütfen sistem yöneticinizle görüşün...");
        return;
    }
    $.ajax({
        type: "POST",
        url: '/HaselSOAService.asmx/ChangeSalesmanFlag',
        data: "{flagId:" + parseInt(id) + ",flagTypeId:" + parseInt(flagTypeId) + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (data) {


            $('#salesmanContentTable tr').each(function () {
                if (this.cells[2].children != undefined && this.cells[1].innerText.trim() == groupTypeName) {

                    var localId = this.getAttribute("id");

                    $("#" + localId).find("i").attr("class", "fa fa-flag-o");
                    var commandStr = this.cells[3].children[0].getAttribute('onclick').toString();
                    var commandParts = commandStr.split(',');
                    commandParts[3] = "'False'";
                    var newCommand = commandParts.toString();
                    this.cells[3].children[0].setAttribute('onclick', newCommand);

                    if (localId.replace("salesTr", "") === nlocalId) {

                        newRowId = data.d.CmCustomerSalesmanId;


                        var lastId = localId.replace("salesTr", "");
                        var a = $("tr[id=" + localId + "]").html().split(lastId).join(newRowId);
                        $("#" + localId).html(a);
                        $("#" + localId).attr("id", "salesTr" + newRowId);

                        // $("#salesTr" + newRowId.toString()).children()[4].setAttribute("style", "visibility:hidden");
                    }
                }
            });

            var faturayesno = data.d.FlagStatus.toString().toUpperCase();
            var result = "";
            if (faturayesno == "TRUE") {
                result = "fa fa-flag";
                var commandStr = $("#salesTr" + newRowId.toString()).children()[3].children[0].getAttribute('onclick')
                    .toString();
                var commandParts = commandStr.split(',');
                commandParts[3] = "'True'";
                var newCommand = commandParts.toString();
                $("#salesTr" + newRowId.toString()).children()[3].children[0].setAttribute('onclick', newCommand);

            } else {
                result = "fa fa-flag-o";
            }
            targetButtonx = $("#salesTr" + newRowId.toString()).children()[2].children[0].children[0];
            targetButtonx.className = result;
            //alert("Ana lokasyon değiştirilmiştir...");
        },
        error: function (data) {
            alert(data.d);
        }
    });


}


function MachineParkDateValidation(saleDate, releaseDate, planedReleaseDate) {

    //İleride ihtiyacımız olursa "*Elden Çıkarıldığı Tarih*"'ten "*Satın Alma Tarihi*" çıkarıldığında makinenin kullanım süresine ulaşabiliriz.

    if (!isEmptyLocal(planedReleaseDate) && !isEmptyLocal(saleDate)) {
        if (getDateDiff(planedReleaseDate, saleDate) < 0) {
            return 'Planlanan Elden Çıkarma Tarihi, Satın Alma Tarihi\'nden küçük olamaz.';
        }
    }

    //Satın Alma Tarihi olmayabilir, bunda bence hata vermesi gerekmez.)
    if (!isEmptyLocal(saleDate) && !isEmptyLocal(releaseDate)) {
        if (getDateDiff(releaseDate, saleDate) < 0) {
            return "Elden Çıkarıldığı Tarih, Satın Alma Tarihinden küçük olamaz.";
        }
    }

    if (!isEmptyLocal(releaseDate)) {
        if (getDateDiff(todayTurkishFormat(), releaseDate) < 0) {
            return "Elden Çıkarıldığı Tarih gelecekte olamaz.";
        }
    }

    return "";


}

$(document).ready(function () {

    $("[id*='txtMpMachineparkCount']").keypress(function (e) {

        if (e.which === 45 || e.which === 43) {//- karekterini girmemeleri icin
            event.preventDefault();
            return false;
        }
    });

    $("[id*=datePlanedRelease]")
        .change(function () {
            if (confirm("Bu kayıt makine parkından silinecektir, onaylıyor musunuz?") === true) {
                console.log(true);
            }
        });

    //Elden Çıkarıldığı Tarih girildiğinde uyarı verir,
    //“_Bu kayıt makine parkından silinecektir, onaylıyor musunuz?_” şeklinde.

  

});

/* global function handler*/
/*
window.onerror = function (errorMessage, errorUrl, errorLine) {
    try {

        if (errorUrl.indexOf("localhost") == -1) {

                $.ajax({
                    type: 'POST',
                    url: '/HaselSOAService.asmx/Log',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: "{em:'" + errorMessage.split("'").join("|") + "',eu:'" + errorUrl + " -wlocalhref:" + window.location.href + "' , el:'" + errorLine + "', us:'" + $("[id*=hdnUser]").val() + "'}",
                    success: function () {
                        if (console && console.log) {
                            console.error(errorMessage, errorUrl , errorLine);
                        }
                    },
                    error: function () {
                        if (console && console.error) {
                            console.log('hata JS error report gonderme islemi basarilamadi! Hata');
                            console.error(errorMessage, errorUrl, errorLine);
                        }
                    }
                });

                return true;
            

        }
    } catch (ex) {
        alert("js log send error. ");
    }
};
*/
//window.alert = function (msg) {
//    bootbox.dialog({
//        message: msg,
//        title: "Mesaj",
//        onEscape: true,
//        buttons: {
//            success: {
//                label: "Tamam",
//                className: "green bootboxbutton",
//                callback: function() {
//                    console.log("enter press");
//                }
//            }
//        }
//    }).init(function () {
//        glb_StatusBootbox = true;
//    });
//}

//window.confirm = function(message, caption) {
//    caption = caption || 'Confirmation';
//    bootbox.dialog({
//        message: message,
//        title: "Mesaj",
//        buttons: {
//            success: {
//                label: "Tamam",
//                className: "green bootboxbutton",
//                callback: function() {
//                    console.log("tamam press");
//                    return true;
//                }
//            },
//        cancel:{
//                label: "Iptal",
//                className: "green bootboxbutton",
//                callback: function () {
//                    console.log("iptal press");
//                    return false;
//                }
//            }
//        }
//    }).init(function () {
//        glb_StatusBootbox = true;
//    });
//};

var glb_StatusBootbox = false;

function MachineParkValidator() {
    var currentSalesman = "";
    var strTemp = "";
    $('#salesmanContentTable tr').each(function () {
        strTemp = this.cells[1].innerHTML;
        if (currentSalesman.indexOf(strTemp) === -1) {
            currentSalesman += strTemp + "|";
        }

    });
    return currentSalesman.split("|");
}

function TypeHtmlCount() {
    var CurrentSalesman = "";
    var strTemp = "";
    $('#salesmanContentTable tr').each(function () {
        strTemp = this.cells[1].innerHTML;

        CurrentSalesman += strTemp + "|";

    });

    var arrCategory = CurrentSalesman.split("|");
    return arrCategory;
}

//TOOLS FUNCTION
function todayTurkishFormat() {
    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth() + 1;
    var curr_year = d.getFullYear();
    return curr_year + "-" + curr_month + "-" + curr_date;
}

function Remove_Char_H_O(stringCode) {
    if (!isEmptyLocal(stringCode)) {
        if (stringCode.indexOf("-H") > 0) {
            return stringCode.substring(0, stringCode.length - 2);
        }
        if (stringCode.indexOf("-O") > 0) {
            return stringCode.substring(0, stringCode.length - 2);
        }
    }
    return stringCode;
} 

function isEmptyLocal(val) {
    return (val === undefined || val == null || val.length <= 0 || val === "null" ) ? true : false;
}

function DateJsontoHtmlFormat(strJsonDate) {
    var temp = strJsonDate;//.trim();
    if (!isEmptyLocal(temp)) {
        if (temp.indexOf('.') >= 0) {
            var dpts = temp.split('.');
            var yearprt = dpts[2].split(' ');
            return yearprt[0] + '-' + dpts[1] + '-' + dpts[0];
        }
        else {
            var dpts = temp.split('-');
            var yearprt = dpts[2].split(' ');
            return yearprt[0] + '-' + dpts[1] + '-' + dpts[0];
        }
    }
    return temp;
}

function nullToEmpty(str) {
    if (str === "null") {
        return "";
    }
    if (str === undefined) {
        return "";
    }
    if (str === null) return "";
    return str;
}

var JsonToHtmlFormat = function (_date) {
    if (!isEmptyLocal(_date)) {
        return (new Date(parseInt(_date.toString().replace('/Date(', '')))).format("dd-MM-yyyy");
    }
    return "";

};

function getDateDiff(time1, time2) {
    var str1 = time1.split('-');
    var str2 = time2.split('-');
    //                yyyy   , mm       , dd
    var t1 = new Date(str1[0], str1[1] - 1, str1[2]);
    var t2 = new Date(str2[0], str2[1] - 1, str2[2]);
    var diffMS = t1 - t2;
    var diffS = diffMS / 1000;
    var diffM = diffS / 60;
    var diffH = diffM / 60;
    var diffD = diffH / 24;
    return diffD;
}

function getQueryStringByName(name, url) {
    if (!url) {
        url = window.location.href.toLowerCase();
    }
    name = name.toLowerCase();
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

$(document).ready(function () {

    $(document).keypress(function (e) {

        if (e.which === 13) {
            //if (glb_StatusBootbox) {
            //    glb_StatusBootbox = false;
            //    $("[data-bb-handler='success']").click();
            //    return false;
            //}
            //  var a = $("a[aria-expanded='true']")[0].innerText; stabil degil
            // var a = $(".nav-tabs > li[class='active'] > a > span[id*='ContentPlaceHolder']").attr('id');

            if (e.target.id === "pacinputInvisible") {
                event.preventDefault();
                return false;
            }//google haritada yer secerken enter a basarsa sayfa post oluyor
            if (e.target.id === "ContentPlaceHolder1_txtUnvan") {
                event.preventDefault();
                return false;
            }//arama enter
            
            if (e.target.id === "areaNote") {
                event.preventDefault();
                var s = $("#areaNote").val();
                $("#areaNote").val(s + "\n");
                return false;
            }

            var a = $(" #tabContainer > .nav-tabs > li[class='active'] > a")[0];
            var strHref = a.getAttribute("href");
            TabTrigger(strHref);

            //post olmamasi icin
            event.preventDefault();
            return false;
        }
    });

});



function TabTrigger(tabName) {
    if (tabName === "#tab_general") {
        SaveGeneral();
        return;
    }
    if (tabName === "#tab_locations") {
        LocationSave(1);
        return;
    }
    if (tabName === "#tab_Auth") {
        AuthenticatorSave(1);
        return;
    }
    if (tabName === "#tab_saleengineers") {
        SalesmanSave(1);
        return;
    }
    if (tabName === "#tab_machinepark") {
        MachineParkSaveAndClose(1);
        return;
    }

    if (tabName === "#tab_Interview") {
        InterviewInsertorUpdate(1);
        return;
    }
    if (tabName === "#tab_Request") {
        return;
    }
    alert("Lutfen kaydet butonunu kullaniniz");
    return;
}

function CoordinateValidator(latitudeorLongitude) {
    var regex_coords = new RegExp("[-]?[0-9]*[.]{0,1}[0-9]");
    var reg = new RegExp("^-?([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}");
    return regex_coords.exec(latitudeorLongitude);
}

function GetHdn() {
    $("[id*='hdn']").each(function () {
        console.log(this);
    });
}
function htmlEscape(str) {
    return str
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

// I needed the opposite function today, so adding here too:
function htmlUnescape(str) {
    return str
        .replace(/&quot;/g, '"')
        .replace(/&#39;/g, "'")
        .replace(/&lt;/g, '<')
        .replace(/&gt;/g, '>')
        .replace(/&amp;/g, '&');
}

function InterviewIsValid() {
    /*
    hdnInterviewInsert
			hdnInterviewDelete
			hdnInterviewEdit
    */


    if (!$("#ctl01").valid()) {
        alert('Lütfen gerekli alanları doldurunuz');
        return false;
    }

    if (isEmptyLocal(getQueryStringByName("cid"))) {
        alert("Gerçerli bir cari açınız");
        return false;
    }

    if ($("#hdnCustomerInterviewId").val() == 0 && $("[id$='hdnInterviewInsert']").val() != "1") {//insert yetkisi
        alert("Kayit yetkiniz yoktur");
        return false;
    }

    if ($("#hdnCustomerInterviewId").val() > 0 && $("[id$='hdnInterviewEdit']").val() != "1") {//edit yetkisi
        alert("Duzenleme yetkiniz yoktur");
        return false;
    }

    if ($("#hdnCustomerInterviewId").val() > 0 && $("[id$='hdnInterviewEdit']").val() == "1") {
        var a = InterviewIsValidDelAndrUpdate();
         
        if (!a) {//true islemi yapabilir. false islemi yapamaz
            alert("Bu kayıt sizin operasyon ve bölgenize ait değildir, bu işlemi gerçekleştiremessiniz.");
            return false;
        }
    }
    return true;
}
function InterviewIsValidDelAndrUpdate() {
    var tempd = false;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/HaselSOAService.asmx/InterviewUpdateDeleteValidation",
        data: JSON.stringify({ userId: parseInt($("[id='hdnUserId']").val()), interviewId: parseInt($("#hdnCustomerInterviewId").val()) }),
        dataType: "json",
        async: false,
        success: function (response) {
             
            tempd = response.d;

            // return response.d;//true or false
        },
        error: function (response) {
            alert("validasyon hata");
            tempd = response.d;
        }

    });

    return tempd;


}
function InterviewSave(a) {
    InterviewInsertorUpdate();
}

function InterviewMapInsert() {

    cw.Id = $("#hdnCustomerInterviewId").val();
    cw.AuthenticatorId = $("[id$='ddInterviewAuthenticator']").val();
    cw.CustomerId = getQueryStringByName("cid");
    cw.strInterviewDate = $("[id$='dateInterview']").val();
    cw.InterviewTypeId = $("[id$='ddInterviewType']").val();
    cw.Note = $.trim(tinymce.get('textNote').getContent());
    cw.strPlannedInterviewDate = $("[id$='datePlannedInterview']").val();
    cw.UserId = $("[id$=ddInterviewUser]").val();
    cw.CurrentUser = $("[id$='hdnUserId']").val();
    cw.InterviewImportantId = $("[id$='ddInterviewImportant']").val();
    cw.IsDelete = false;
}
function InterviewFormClear() {
    $("#hdnCustomerInterviewId").val("0");
    $("[id$='ddInterviewAuthenticator']").val("");
    //cid dont reset
    $("[id$='dateInterview']").val("");
    $("[id$='ddInterviewType']").val("");
    $("[id$='textNote']").val("");
    $("[id$='datePlannedInterview']").val("");
    $("[id$='ddInterviewUser']").val("");
    $("[id$='ddInterviewImportant']").val("");
    $.trim(tinymce.get('textNote').setContent(""));

    $(".RACI1").text("Kaydet ve Kapat");
    $(".RACI2").text("Kaydet ve Yeni Kayıt Aç");
    $(".RACI1").show();
    $(".RACI2").show();
    $(".RACI3").hide();


}

function ToggInterviewers() {
    $(".collapse.interview").collapse('toggle');
    InterviewFormClear();

}
function ToggRequesters() {
      $(".collapse.request").collapse('toggle');
}
function ToggRequesters_Everyopen() {
    if (!$( ".collapse.request" ).is( ":visible" )) {
        $(".collapse.request").collapse('toggle');
    }
}

function ToggRequesters_Everyclose() {
    if ($( ".collapse.request" ).is( ":visible" )) {
        $(".collapse.request").collapse('toggle');
    }
}

function InterviewInsertorUpdate() {
    if (!InterviewIsValid()) {
        return;
    }

    InterviewMapInsert();

    if (IsDeleted_Global) {
        cw.IsDeleted = true;
    } else {
        cw.IsDeleted = false;
    }

    var ivTable = $("table[id*='interviewContentTable']");
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/HaselSOAService.asmx/CustomerInterviewsInsertorUpdate",
        data: JSON.stringify({ item: cw }),
        dataType: "json",
        async: false,
        success: function (response) {
            if (response.d.HasError === false && response.d.List.length > 0) {
                alert("İşlem başarılı bir şekilde gerçekleştirilmiştir");
              
                var important = "";
                if(!isEmptyLocal($("[id$='ddInterviewImportant']").val())) {
                    important= $("[id$='ddInterviewImportant'] option:selected").text();
                }

                var ent = response.d.List[0];
                var lHtml = "<tr id=\"ivTr" + ent.Id + "\">"
                 + " <td>" + DateJsontoHtmlFormat($("[id*='dateInterview']").val()) + "</td>"
                 + " <td>" + DateJsontoHtmlFormat($("[id*='datePlannedInterview']").val()) + "</td>"
                 + " <td>" + $("[id$='InterviewUser'] option:selected").text() + "</td>"
                 + " <td>" + $("[id$='ddInterviewAuthenticator'] option:selected").text() + "</td>"
                 + " <td>" + $("[id$='ddInterviewType'] option:selected").text() + "</td>"
                 + " <td>" + important+ "</td>"
                 + " <td><input class='btn btn-success' type='button' value='Güncelle' onclick =\"UpdateCustomerInterview(" + ent.Id + "," + ent.AuthenticatorId + ", '" + JsonToHtmlFormat(ent.InterviewDate) + "'," + ent.InterviewTypeId + ",'','" + JsonToHtmlFormat(ent.PlannedInterviewDate) + "'," + ent.UserId + "); ToggleInterviewShow();\" /></td>"
                 + "</tr>";
                var rowid = "ivTr" + ent.Id;
                $("tr[id=" + rowid + "]").remove();
                if (IsDeleted_Global !== true) {
                    ivTable.find("tr").last().after(lHtml);
                }
                InterviewFormClear();
                $("#ContentPlaceHolder1_spInterviewCount")
                    .html($("table[id*='interviewContentTable']").find("tr").length - 1);


            } else {
                alert("Hata: Kayıt işlemi gerçekleştirilemedi" + response.d.ErrorDetail);
            }
            console.log(response);

        },
        error: function (response) {
            alert(response);
            console.log(response);
        }
    });

}
function UpdateCustomerInterview(id, authenticatorId, interviewDate, interviewTypeId, note, plannedInterviewDate, userId) {
    $("#hdnCustomerInterviewId").val(id);
    $("[id$='ddInterviewAuthenticator']").val(authenticatorId);
    $("[id$='dateInterview']").val(DateJsontoHtmlFormat(interviewDate));
    $("[id$='ddInterviewType']").val(interviewTypeId);
   
    $("[id$='datePlannedInterview']").val(DateJsontoHtmlFormat(plannedInterviewDate));
    $("[id$='ddInterviewUser']").val(userId);
    GetInterViewNote(id);
}
function GetInterViewNote(rowId) {

   
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/HaselSOAService.asmx/InterviewGetNote",
        data: "{Id:"+rowId+"}",
        dataType: "json",
        async: false,
        success: function (response) {
            $("[id$='ddInterviewImportant']").val(response.d.ImportantId);
            $.trim(tinymce.get('textNote').setContent(response.d.Note));
        }
    });
}

var IsDeleted_Global = false;
function InterviewDelete() {
    if ($("[id$='hdnInterviewDelete']").val() !== "1") {//delete yetkisi
        alert("Duzenleme yetkiniz yoktur");
    }
    IsDeleted_Global = true;
    InterviewInsertorUpdate();
    IsDeleted_Global = false;

}
function ToggleInterview() {
    InterviewFormClear();
    setTimeout(function () { initAutocomplete(); }, 200);
}

function ToggleInterviewShow() {
    if ($(".interview.collapse.in").length <= 0)
        $(".collapse.interview").collapse('toggle');
    $('body, html').animate({ scrollTop: 180 }, 800);

    $(".RACI1").text('Güncelle ve Kapat');
    $(".RACI2").text('Güncelle ve Yeni Kayıt Aç');
    $(".RACI3").css({ 'display': "block" });

}

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
    ]
    // content_css: ['//www.tinymce.com/css/codepen.min.css']
});