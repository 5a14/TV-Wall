<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WISTNews.aspx.cs" Inherits="WISTTVNews.WISTNews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <link href="Content/UsingCss.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="Content/datatables.css" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.cycle.all.js"></script>
    <script type="text/javascript" src="Scripts/datatables.js"></script>
    <script async="async" src="https://www.youtube.com/iframe_api"></script>
    <script async defer crossorigin="anonymous" src="https://connect.facebook.net/zh_TW/sdk.js#xfbml=1&version=v3.2"></script>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>News</title>

</head>

<body onload="OnloadTrigger()" id="bodyMainPage" class="bodyMainPageBackground">

    <form id="form1" runat="server">

        <div id="divPage" class="mainPageCss">
            <div id="divUp" class="row ">


                <div id="divMainInfo" class="col-sm-9 mainInfoCss">

                    <div id="divShowYouTube">
                        <%--&autoplay=1(自動播放指令) &mute=1(靜音指令，如果不加，Chrome會無法自動播放)--%><%--試試看長寬高改0--%>
                        <iframe id="YouTubePlayer" src="https://www.youtube.com/embed?listType=playlist&list=PLPv96SVEnDc0Ja1b64FXjNpJ4ujJkL1pi&autoplay=1&mute=1" frameborder="0" allowfullscreen></iframe>
                    </div>

                    <div id="divTrainDatailShow" class="maimDatailShowCss">
                        <asp:Label ID="lbTrainDatail" runat="server" Text="" CssClass="FontColor UpdataTxtCss"></asp:Label>
                        <table id="tbTrainDatail" class="FontColor" rules="ROWS">
                            <thead>
                                <tr>
                                    <th>到站時間</th>
                                    <th>狀態</th>
                                    <th>車次</th>
                                    <th>車型</th>
                                    <th>站名</th>
                                    <th>開往</th>
                                    <th>順逆向</th>
                                </tr>
                            </thead>
                        </table>
                    </div>

                    <div id="divBusDatailShow" class="maimDatailShowCss">
                        <asp:Label ID="lbBusDatail" runat="server" Text="" CssClass="FontColor UpdataTxtCss"></asp:Label>
                        <table id="tbBusDatail" class="FontColor" rules="ROWS">
                            <thead>
                                <tr>
                                    <th>路線</th>
                                    <th>預估到站</th>
                                    <th>站名</th>
                                    <th>去返程</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>


                <div id="divTopRight" class="col-sm-3">

                    <div id="divBirthday" class="row BirthdayCss">
                        <div id="divBirthdayImg" class="col-sm-4 FontColor">
                            <img id="imgBirthday" src="Image/ImgBirthday.png" />
                        </div>
                        <div id="divBirthdayTxt" class="col-sm-8 FontColor largeFontCss"></div>
                    </div>

                    <div id="divFBShow">

                        <asp:Label ID="lbFB" runat="server" Text="" CssClass="FontColor"></asp:Label>
                    </div>
                </div>
            </div>

            <div id="divBottom" class="row">

                <div id="divBottomLeft" class="col-sm-9">
                    <div id="divBottomLeftOfTop" class="row">
                        <div id="divWeather" class="col-sm-3 WeatherCss">
                            <div id="divShowWeatherTxt" class="FontColor"></div>
                        </div>

                        <div id="divWelcome" class="col-sm-9 WelcomeCss FontColor largeFontCss">
                            使用bootstrap
                        </div>
                    </div>

                    <div id="divWISTNews" class="row WISTNewsCss">
                        <div class="col-sm-3">
                            <asp:Label ID="lbDateTimeNow" runat="server" Text="" CssClass="FontColor"></asp:Label>
                        </div>
                        <div id="divShowWISTNewsTxt" class="col-sm-9 FontColor">
                        </div>
                    </div>

                </div>

                <div id="divNews" class="col-sm-3 NewsCss">
                    預期放RSS
                </div>

            </div>
        </div>
    </form>
</body>
</html>


<script type="text/javascript">

        //////Web.config使用
        ////DeBug
        //<add key="WISTNewsRunTimeUrl" value="\\News\\News.txt" />
        //<add key="WeatherRunTimeUrl" value="\\News\\Weather.txt" />
        //<add key="BirthdayRunTimeUrl" value = "\\News\\Birthday.txt"/>

        ////IIS
        //<add key="WISTNewsRunTimeUrl" value = "\\WISTTVNews\\News\\News.txt"/>
        //<add key="WeatherRunTimeUrl" value = "\\WISTTVNews\\News\\Weather.txt"/>
        //<add key="BirthdayRunTimeUrl" value = "\\WISTTVNews\\News\\Birthday.txt"/>

    function OnloadTrigger() {

        UpdateTime(); //更新時間
        ShowWISTNewsTxt();//顯示新聞輪播
        ShowWeatherTxt();//顯示天氣輪播
        ShowBirthdayTxt();//顯示生日輪播

        GetTableTrainDetails();//顯示即時火車動態
        GetTableBusDetails();//顯示即時公車動態

        ////執行的URL : http://127.0.0.1/WISTTVNews/Wistnews.aspx

        var DateTimeNow = new Date();               //現在時間
        var DateTimeNowHH = DateTimeNow.getHours(); //取時
        var DateTimeNowmm = DateTimeNow.getMinutes();//取分

        //修改時間須配合 UpdateTime() 與 ShowObject() 一起修改
        ShowObject(DateTimeNowHH, DateTimeNowmm, '0', 'OnloadTrigger');//根據時間顯示畫面
    }


    function ShowObjectOpenOrClose(OpenOrClose) {

        if (OpenOrClose != 'PublicTransportation') {
            $('divShowYouTube').show();
            $('YouTubePlayer').show();
            document.getElementById('tbTrainDatail').style.display = 'none';
            document.getElementById('tbBusDatail').style.display = 'none';
            document.getElementById('lbTrainDatail').style.display = 'none';
            document.getElementById('lbBusDatail').style.display = 'none';
        }
        else {
            document.getElementById('divShowYouTube').style.display = 'none';
            document.getElementById('YouTubePlayer').style.display = 'none';
            $('tbTrainDatail').show();
            $('tbBusDatail').show();
            $('lbTrainDatail').show();
            $('lbBusDatail').show();
        }
    }

    //修改時間須配合 OnloadTrigger() 與 UpdateTime() 一起修改
    function ShowObject(DateTimeNowHH, DateTimeNowmm, DateTimeNowss, Item) {
        //根據時間顯示不同畫面

        if (Item == 'UpdateTime') {

            //只有早上8點與下午三點跑這段，並整新整理頁面
            if ((DateTimeNowHH.toString() >= 8 && DateTimeNowmm == 0 && DateTimeNowss == 0) || (DateTimeNowHH.toString() <= 14 && DateTimeNowmm == 0 && DateTimeNowss == 0)) {

                //整新整理頁面，不重新整理會無法顯示Youtube
                window.location.reload();

                //顯示物件(Youtube)
                ShowObjectOpenOrClose('Video');

            }
            else {
                //顯示物件(大眾運輸資訊)
                ShowObjectOpenOrClose('PublicTransportation');
            }
        }
        else {
            //item == OnloadTrigger
            if (DateTimeNowHH.toString() < 8 || DateTimeNowHH.toString() > 14) {
                //顯示物件(大眾運輸資訊)
                ShowObjectOpenOrClose('PublicTransportation');
            }
            else {
                //顯示物件(Youtube)
                ShowObjectOpenOrClose('Video');
            }
        }
    }

    function UpdateTime() {
        //自動更新時間
        var DateTimeNow = new Date();               //現在時間

        var DateTimeNowYear = DateTimeNow.getFullYear();  //取年
        var DateTimeNowMonth = DateTimeNow.getMonth() + 1;//取月(getMonth是回傳0-11故須+1)
        var DateTimeNowDay = DateTimeNow.getDate();       //取日

        var DateTimeNowHH = DateTimeNow.getHours();     //取時
        var DateTimeNowmm = DateTimeNow.getMinutes();   //取分
        var DateTimeNowss = DateTimeNow.getSeconds();   //取秒
        var StrdateTimeNows = null;

        //修改時間須配合 OnloadTrigger() 與 ShowObject()  一起修改
        if ((DateTimeNowHH.toString() >= 8 && DateTimeNowmm == 0 && DateTimeNowss == 0) || (DateTimeNowHH.toString() <= 14 && DateTimeNowmm == 0 && DateTimeNowss == 0)) {
            ShowObject(DateTimeNowHH, DateTimeNowmm, DateTimeNowss, 'UpdateTime');//根據時間顯示畫面
        }


        //個位數補0
        if (DateTimeNowMonth.toString().length < 2) {
            DateTimeNowMonth = '0' + DateTimeNowMonth;
        }
        if (DateTimeNowDay.toString().length < 2) {
            DateTimeNowDay = '0' + DateTimeNowDay;
        }
        if (DateTimeNowHH.toString().length < 2) {
            DateTimeNowHH = '0' + DateTimeNowHH;
        }
        if (DateTimeNowmm.toString().length < 2) {
            DateTimeNowmm = '0' + DateTimeNowmm;
        }
        if (DateTimeNowss.toString().length < 2) {
            DateTimeNowss = '0' + DateTimeNowss;
        }

        //時間格式：2019/04/30 11:00:00
        StrdateTimeNows = DateTimeNowYear + '/' + DateTimeNowMonth + '/' + DateTimeNowDay + ' ' + DateTimeNowHH + ':' + DateTimeNowmm + ':' + DateTimeNowss;

        document.getElementById('lbDateTimeNow').innerHTML = StrdateTimeNows.toString();
        setTimeout('UpdateTime()', 1000);//每秒更新
    }


    function ShowWISTNewsTxt() {
        $.ajax({
            //url: '\\News\\News.txt',        //路徑(讀取文字檔如果加入新資料必須把所有路徑下的資料夾或文字檔關閉，否則不會更換，會變成使用暫存)
            //url: '\\WISTTVNews\\News\\News.txt',        //路徑(讀取文字檔如果加入新資料必須把所有路徑下的資料夾或文字檔關閉，否則不會更換，會變成使用暫存)
            url: '<%=ConfigurationManager.AppSettings["WISTNewsRunTimeUrl"]%>',

            dataType: 'text',               //型態
            error: function (Err) { alert(Err.responseText); }, //錯誤訊息

            success: function (Data) {

                //暫存文字陣列
                var TmpArrWISTNewsTxt = new Array();
                //資料分割並放入陣列(「,」做區隔)
                TmpArrWISTNewsTxt = Data.split(",");
                //顯示用的資料陣列
                var ArrWISTNewsTxt = null;

                //把暫存陣列的資料依序加入物件並顯示
                for (var i = 0; i < TmpArrWISTNewsTxt.length; i++) {
                    if (ArrWISTNewsTxt == null)
                        ArrWISTNewsTxt = "<div>" + TmpArrWISTNewsTxt[i] + "</div>";
                    else
                        ArrWISTNewsTxt += "<div>" + TmpArrWISTNewsTxt[i] + "</div>";
                }

                //陣列放入物件中顯示
                document.getElementById("divShowWISTNewsTxt").innerHTML = ArrWISTNewsTxt;

                //設定輪播與替換時間，根據Web.Config的設定決定輪播速度
                $('#divShowWISTNewsTxt').cycle({ timeout: '<%=ConfigurationManager.AppSettings["WISTNewsRunTimeSec"]%>' * 1000, speed: 400 });
            }
        });
    }


    function ShowWeatherTxt() {

        $.ajax({
            //url: '\\News\\Weather.txt',     //路徑(讀取文字檔如果加入新資料必須把所有路徑下的資料夾或文字檔關閉，否則不會更換，會變成使用暫存)
            //url:'\\WISTTVNews\\News\\Weather.txt',
            url: '<%=ConfigurationManager.AppSettings["WeatherRunTimeUrl"]%>',

            dataType: 'text',               //型態
            error: function (Err) { alert(Err.responseText); }, //錯誤訊息
            success: function (Data) {
                //暫存文字陣列
                var TmpArrWeatherTxt = new Array();
                //資料分割並放入陣列(「,」做區隔)
                TmpArrWeatherTxt = Data.split(",");
                //顯示用的資料陣列
                var ArrWeatherTxt = null;

                //把暫存陣列的資料依序加入物件並顯示
                for (var i = 0; i < TmpArrWeatherTxt.length; i++) {
                    if (ArrWeatherTxt == null)
                        ArrWeatherTxt = "<div>" + TmpArrWeatherTxt[i] + "</div>";
                    else
                        ArrWeatherTxt += "<div>" + TmpArrWeatherTxt[i] + "</div>";
                }

                //陣列放入物件中顯示
                document.getElementById("divShowWeatherTxt").innerHTML = ArrWeatherTxt;

                //設定天氣輪播與替換時間，根據Web.Config的設定決定輪播速度
                $('#divShowWeatherTxt').cycle({ timeout: '<%=ConfigurationManager.AppSettings["WeatherRunTimeSec"]%>' * 1000, speed: 400 });
            }
        });
    }


    function ShowBirthdayTxt() {
        $.ajax({
            //url: '\\News\\BirthdayRunTimeUrl.txt',        //路徑(讀取文字檔如果加入新資料必須把所有路徑下的資料夾或文字檔關閉，否則不會更換，會變成使用暫存)
            //url: '\\WISTTVNews\\News\\BirthdayRunTimeUrl.txt',        //路徑(讀取文字檔如果加入新資料必須把所有路徑下的資料夾或文字檔關閉，否則不會更換，會變成使用暫存)
            url: '<%=ConfigurationManager.AppSettings["BirthdayRunTimeUrl"]%>',

            dataType: 'text',               //型態
            error: function (Err) { alert(Err.responseText); }, //錯誤訊息

            success: function (Data) {

                //暫存文字陣列
                var TmpArrWISTNewsTxt = new Array();
                //資料分割並放入陣列(「,」做區隔)
                TmpArrWISTNewsTxt = Data.split(",");
                //顯示用的資料陣列
                var ArrWISTNewsTxt = null;

                //把暫存陣列的資料依序加入物件並顯示
                for (var i = 0; i < TmpArrWISTNewsTxt.length; i++) {
                    if (ArrWISTNewsTxt == null)
                        ArrWISTNewsTxt = "<div>" + TmpArrWISTNewsTxt[i] + "</div>";
                    else
                        ArrWISTNewsTxt += "<div>" + TmpArrWISTNewsTxt[i] + "</div>";
                }

                //陣列放入物件中顯示
                document.getElementById("divBirthdayTxt").innerHTML = ArrWISTNewsTxt;

                //設定輪播與替換時間，根據Web.Config的設定決定輪播速度
                $('#divBirthdayTxt').cycle({ timeout: '<%=ConfigurationManager.AppSettings["BirthdayRunTimeSec"]%>' * 1000, speed: 400 });
            }
        });
    }


    //設定Table自動換頁
    setInterval(
        function () {
            var TableTrain = $('#tbTrainDatail').DataTable();
            pageLength: 5,//顯示筆數
                TableTrain.page('next').draw('page');

            var TableBus = $('#tbBusDatail').DataTable();
            pageLength: 5,//顯示筆數
                TableBus.page('next').draw('page');
        }
        , 10000);

    //設定火車撈取Web Api時間，根據Web.Config的設定決定輪播速度
    var TimeTrainDetails = window.setInterval(GetTableTrainDetails, '<%=ConfigurationManager.AppSettings["TrainDatailsUpdateSec"]%>' * 1000);

    function GetTableTrainDetails() {
        $.ajax({
            url: 'WISTNews.aspx',  //路徑
            method: 'GET',
            async: true,
            data: {
                url: 'https://ptx.transportdata.tw/MOTC/v2/Rail/TRA/LiveBoard?$filter=Replace&$top=30&$format=JSON', item: 'TrainDatails'
            },
            success: function (Data) {

                if (Data != null & Data != "") {
                    //資料為空就不顯示
                    if (Data[0].UpdateTime != null & Data[0].UpdateTime != "") {
                        document.getElementById("lbTrainDatail").innerText = "火車資訊更新時間：" + Data[0].UpdateTime.toString();

                        $('#tbTrainDatail').dataTable({
                            pageLength: 5,//顯示筆數
                            searching: false, //關閉filter功能
                            destroy: true,
                            data: Data,

                            LengthChange: false, //關閉顯示筆數
                            bInfo: false,//關閉筆數資訊
                            //paging: false,//關閉分頁

                            paging: true,//開啟分頁

                            order: [6, 'Desc'],//排序
                            rowHeight: 'auto',

                            columns: [
                                { 'data': 'ScheduledArrivalTime' },
                                { 'data': 'DelayTime' },
                                { 'data': 'TrainNo' },
                                { 'data': 'TrainTypeName' },
                                { 'data': 'StationName' },
                                { 'data': 'EndingStationName' },
                                { 'data': 'Direction' }

                            ]
                        });
                    }
                    document.getElementById("tbTrainDatail_length").style.display = 'none';
                    document.getElementById("tbTrainDatail_paginate").style.display = 'none';
                }
            },
            error: function (Err) {
                alert(Err.responseText);

                $('#tbTrainDatail').dataTable({
                    searching: false, //關閉filter功能
                    LengthChange: false, //關閉顯示筆數
                    bInfo: false,//關閉筆數資訊
                });

            } //錯誤訊息
        });
    }

    //設定公車撈取Web Api時間，根據Web.Config的設定決定輪播速度
    var TimeBusDetails = window.setInterval(GetTableBusDetails, '<%=ConfigurationManager.AppSettings["BusDetailsUpdateSec"]%>' * 1000);

    function GetTableBusDetails() {
        $.ajax({
            url: 'WISTNews.aspx',  //路徑
            method: 'GET',
            async: true,
            data: {
                url: 'https://ptx.transportdata.tw/MOTC/v2/Bus/EstimatedTimeOfArrival/City/NewTaipei?$filter=Replace&$top=30&$format=JSON', item: 'BusDetails'
            },
            success: function (Data) {

                //資料為空就不顯示
                if (Data != null & Data != "") {
                    //資料為空就不顯示
                    if (Data[0].UpdateTime != null & Data[0].UpdateTime != "") {
                        document.getElementById("lbBusDatail").innerText = "公車資訊更新時間：" + Data[0].UpdateTime.toString();

                        $('#tbBusDatail').dataTable({
                            pageLength: 6,//顯示筆數
                            searching: false, //關閉filter功能
                            destroy: true,
                            data: Data,
                            LengthChange: false, //關閉顯示筆數
                            bInfo: false,//關閉筆數資訊
                            //paging: false,//關閉分頁
                            order: [3, 'asc'],//排序
                            rowHeight: '10px',

                            columns: [
                                { 'data': 'RouteNameZhtw' },
                                { 'data': 'EstimateTime' },
                                { 'data': 'StopNameZhtw' },
                                { 'data': 'Direction' }
                            ]
                        });
                    }
                    document.getElementById("tbBusDatail_length").style.display = 'none';
                    document.getElementById("tbBusDatail_paginate").style.display = 'none';
                }
            },
            error: function (Err) {
                alert(Err.responseText);

                $('#tbBusDatail').dataTable({
                    searching: false, //關閉filter功能
                    LengthChange: false, //關閉顯示筆數
                    bInfo: false,//關閉筆數資訊
                });

            } //錯誤訊息
        });
    }
</script>
