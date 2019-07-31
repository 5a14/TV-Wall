using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using WISTTVNews.Models;
using log4net;

namespace WISTTVNews.AppCode
{
    public class WebServiceJson : System.Web.UI.Page
    {
        //log檔宣告
        static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<BusDetail> P_BusDetailsList = new List<BusDetail>();       //公車資訊
        public List<TrainDatail> P_TrainDatailsList = new List<TrainDatail>(); //火車資訊

        public string GetBusOrTrainJson(string Url, string Item)
        {
            //利用模擬爬蟲方式跳過認證，撈公車或火車的JSon資料
            if (Url != null & Url != "")
            {
                using (WebClient WebClient = new WebClient())
                {
                    // 指定 WebClient 的編碼
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    WebClient.Encoding = Encoding.UTF8;
                    // 指定 WebClient 的 Content-Type header
                    WebClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");

                    if (Item == "TrainDatails")
                    {
                        string StrInsertTrainStationName = "";//暫存要插入陣列的文字，用於Web Api呼叫用的字串
                        try
                        {
                            //替代「Replace」
                            //"https://ptx.transportdata.tw/MOTC/v2/Rail/TRA/LiveBoard?$filter=Replace&$orderby=Direction&top=30&$format=JSON";

                            string StrTrainStationName = System.Web.Configuration.WebConfigurationManager.AppSettings["TrainStationName"];//利用Web.Config的appSettings設定
                            Array ArrTrainStationName = StrTrainStationName.Split(',');//利用字串分割依序把資料加入陣列
                            int IntIndexArrItem = 0;//計算陣列Index

                            //利用迴圈依序加入Web Api使用的字串
                            foreach (var ArrItem in ArrTrainStationName)
                            {
                                IntIndexArrItem += 1;
                                if (IntIndexArrItem == 1)
                                    StrInsertTrainStationName += "(contains(StationName/Zh_tw,'" + ArrItem.ToString().Trim() + "'))";
                                else //多條件時需要用「or」串資料
                                    StrInsertTrainStationName += "or(contains(StationName/Zh_tw,'" + ArrItem.ToString().Trim() + "'))";
                            }

                            //取代在原本Url的部分字串
                            string StrModifyUrl = Url.Replace("Replace", StrInsertTrainStationName.Trim());

                            //從網路url上取得資料
                            var Body = WebClient.DownloadString(StrModifyUrl);
                            //資料轉換為可格式化的資料
                            dynamic TmpData = JsonConvert.DeserializeObject(Body);

                            foreach (var Datails in TmpData)
                            {
                                TrainDatail Train = new TrainDatail();

                                Train.TrainNo = Datails.TrainNo.ToString(); //車次代碼(號)
                                Train.StationName = Datails.StationName.Zh_tw.ToString(); //車站名稱(站名)
                                Train.TrainTypeName = StrModifyTrainTypeName(Datails.TrainTypeName.Zh_tw.ToString().Trim());//列車車種(自強、莒光..)
                                Train.TripLine = StrModifyTripLine(Datails.TripLine.ToString().Trim());//山海線類型 = ['0: 不經山海線', '1: 山線', '2: 海線']
                                Train.EndingStationName = Datails.EndingStationName.Zh_tw.ToString();//終點車站名稱
                                Train.ScheduledArrivalTime = Datails.ScheduledArrivalTime.ToString();//表訂到站時間(格式: HH:mm:ss)
                                Train.ScheduledDepartureTime = Datails.ScheduledDepartureTime.ToString();//表訂離站時間(格式: HH:mm:ss)
                                Train.DelayTime = StrModifyTrainDelayTime(Datails.DelayTime.ToString().Trim());//誤點時間(0:準點;>=1誤點)
                                Train.Direction = StrModifyTrainDirection(Datails.Direction.ToString().Trim());//順逆行 = ['0: 順行(北回)', '1: 逆行(南回)']
                                Train.UpdateTime = Datails.UpdateTime.ToString("yyyy/MM/dd HH:mm:ss").Trim();//台鐵資料更新時間

                                P_TrainDatailsList.Add(Train);
                            }

                            var Json = JsonConvert.SerializeObject(P_TrainDatailsList);//資料轉換JSon格式

                            return Json;//回傳Json格式資料
                        }
                        catch (Exception ex)
                        {
                            //火車資訊異常
                            Logger.Debug("火車資訊異常 插入URL的字串：" + StrInsertTrainStationName + " Item：" + Item + " URL：" + Url.ToString() + "\n" + " 錯誤訊息：" + ex);
                        }
                    }

                    if (Item == "BusDetails")
                    {
                        string StrInsertBusStopName = "";//暫存要插入陣列的文字，用於Web Api呼叫用的字串
                        try
                        {
                            //替代「Replace」
                            //https://ptx.transportdata.tw/MOTC/v2/Bus/EstimatedTimeOfArrival/City/NewTaipei?$filter=Replace&$top=30&$format=JSON

                            string StrBusStopName = System.Web.Configuration.WebConfigurationManager.AppSettings["BusStopName"];//利用Web.Config的appSettings設定
                            Array ArrBusStopName = StrBusStopName.Split(',');//利用字串分割依序把資料加入陣列
                            int IntIndexArrItem = 0;//計算陣列Index

                            //利用迴圈依序加入Web Api使用的字串
                            foreach (var ArrItem in ArrBusStopName)
                            {
                                IntIndexArrItem += 1;
                                if (IntIndexArrItem == 1)
                                    StrInsertBusStopName += "(contains(StopName/Zh_tw,'" + ArrItem.ToString().Trim() + "'))";
                                else //多條件時需要用「or」串資料
                                    StrInsertBusStopName += "or(contains(StopName/Zh_tw,'" + ArrItem.ToString().Trim() + "'))";
                            }

                            //取代在原本Url的部分字串
                            string StrModifyUrl = Url.Replace("Replace", StrInsertBusStopName.Trim());

                            //從網路url上取得資料
                            var Body = WebClient.DownloadString(StrModifyUrl);
                            //資料轉換為可格式化的資料
                            dynamic TmpData = JsonConvert.DeserializeObject(Body);

                            foreach (var Datails in TmpData)
                            {
                                BusDetail Bus = new BusDetail();

                                Bus.StopNameZhtw = Datails.StopName.Zh_tw.ToString();  //公車站牌中文名稱
                                Bus.RouteNameZhtw = Datails.RouteName.Zh_tw.ToString();//路線中文名稱
                                Bus.RouteNameEn = Datails.RouteName.En.ToString();     //路線英文名稱
                                Bus.Direction = StrModifyBusDirection(Datails.Direction.ToString().Trim());//去返程['0: 去程', '1: 返程', '2: 迴圈', '255: 未知']
                                Bus.StopStatus = Datails.StopStatus.ToString().Trim();  //車輛狀態備註['0: 正常', '1: 尚未發車', '2: 交管不停靠', '3: 末班車已過', '4: 今日未營運']

                                if (Bus.StopStatus == "0")//顯示到站時間
                                    Bus.EstimateTime = StrModifyBusEstimateTime(Convert.ToDouble(Datails.EstimateTime));//到站時間預估[當StopStatus値為1~4或PlateNumb値為-1時，EstimateTime値為空値; 反之，EstimateTime有値]
                                else //顯示公車目前狀態
                                    Bus.EstimateTime = StrModifyBusStopStatus(Bus.StopStatus.Trim());


                                Bus.UpdateTime = Convert.ToDateTime(Datails.UpdateTime).ToString("yyyy/MM/dd HH:mm:ss");//平台資料更新時間

                                P_BusDetailsList.Add(Bus);
                            }

                            var json = JsonConvert.SerializeObject(P_BusDetailsList);//資料轉換JSon格式
                            return json;//回傳Json格式資料
                        }
                        catch (Exception ex)
                        {
                            //公車資訊異常
                            Logger.Debug("公車資訊異常 插入URL的字串：" + StrInsertBusStopName + " Item：" + Item + " URL：" + Url.ToString() + "\n" + " 錯誤訊息：" + ex);
                        }
                    }
                    System.Web.HttpContext.Current.Response.End();//關閉
                }
            }
            return "";
        }


        public void GetWeatherJsonToTxt(string Url)
        {
            //撈取JSon資料
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
                Request.Method = WebRequestMethods.Http.Get;
                Request.ContentType = "application/json";

                using (var Response = (HttpWebResponse)Request.GetResponse())
                {
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var Stream = Response.GetResponseStream())
                        using (var Reader = new StreamReader(Stream))
                        {
                            var Temp = Reader.ReadToEnd();

                            dynamic TmpData = JsonConvert.DeserializeObject(Temp);

                            //寫入資料流(絕對位置)
                            FileStream Fs = new FileStream(Server.MapPath("~") + "\\News\\Weather.txt", FileMode.Create);

                            int TotalCount = 0;//計數縣市量
                            foreach (var ItemRecordsLocation in TmpData.records.location)
                            {
                                byte[] Data = new byte[] { };
                                TotalCount += 1;

                                //縣市名稱
                                Data = System.Text.Encoding.UTF8.GetBytes(ItemRecordsLocation.locationName.ToString() + "<br/>");
                                //開始寫入(縣市名稱)
                                Fs.Write(Data, 0, Data.Length);

                                //天氣狀況Wx
                                Data = System.Text.Encoding.UTF8.GetBytes(ItemRecordsLocation.weatherElement[0].time[0].parameter.parameterName.ToString() + "<br/>");
                                //開始寫入(天氣狀況Wx)
                                Fs.Write(Data, 0, Data.Length);

                                //降雨機率PoP + "%"
                                Data = System.Text.Encoding.UTF8.GetBytes("降雨機率：" + ItemRecordsLocation.weatherElement[1].time[0].parameter.parameterName.ToString() + "%" + "<br/>");
                                //開始寫入(降雨機率PoP)
                                Fs.Write(Data, 0, Data.Length);

                                //溫度(最低溫)
                                Data = System.Text.Encoding.UTF8.GetBytes(ItemRecordsLocation.weatherElement[2].time[0].parameter.parameterName.ToString() + "-");
                                //開始寫入溫度(最低溫)
                                Fs.Write(Data, 0, Data.Length);

                                //溫度(最高溫)
                                if (TotalCount != 22) //修改目前縣市數量，會影響輪播資料的顯示
                                    Data = System.Text.Encoding.UTF8.GetBytes(ItemRecordsLocation.weatherElement[3].time[0].parameter.parameterName.ToString() + ",");
                                else
                                    Data = System.Text.Encoding.UTF8.GetBytes(ItemRecordsLocation.weatherElement[3].time[0].parameter.parameterName.ToString());

                                //開始寫入(最高溫)
                                Fs.Write(Data, 0, Data.Length);
                            }
                            TotalCount = 0;

                            //清空緩衝區、關閉流
                            Fs.Flush();
                            Fs.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //天氣資訊異常
                Logger.Debug("天氣資訊異常使用的 URL：" + Url.ToString() + "\n" + " 天氣錯誤訊息：" + ex);
            }
        }


        #region 火車資料細節回傳
        public string StrModifyTripLine(string TripLine)
        {
            //修改山海線狀態
            string Result = "";

            if (TripLine == "1")
                Result = "山線";
            else if (TripLine == "2")
                Result = "海線";
            else
                Result = "";

            return Result;
        }

        public string StrModifyTrainTypeName(string TrainTypeName)
        {
            //修改車次名稱
            string Result = "";

            if (TrainTypeName.Contains("普快") || TrainTypeName.Contains("區間") || TrainTypeName.Contains("復興"))
                return TrainTypeName;

            if (TrainTypeName.Contains("莒光"))
                return Result = "莒光";

            if (TrainTypeName.Contains("普悠瑪"))
                Result = "普悠瑪";
            else if (TrainTypeName.Contains("太魯閣"))
                Result = "太魯閣";
            else
                Result = "自強";

            return Result;
        }

        public string StrModifyTrainDelayTime(string DelayTime)
        {
            //修改誤點時間顯示
            string Result = "";

            if (DelayTime == "0")
                Result = "準點";
            else
                Result = "晚" + DelayTime + "分";

            return Result;
        }

        public string StrModifyTrainDirection(string Direction)
        {
            //修改順逆行 = ['0: 順行(北回)', '1: 逆行(南回)']
            string Result = "";

            if (Direction == "0")
                Result = "順行";
            else
                Result = "逆行";

            return Result;
        }
        #endregion

        #region 公車資料細節回傳
        public string StrModifyBusDirection(string Direction)
        {
            //修改公車去返程狀態
            string Result = "";

            if (Direction == "0")
                Result = "去程";
            else if (Direction == "1")
                Result = "返程";
            else if (Direction == "2")
                Result = "迴圈";
            else
                Result = "未知";

            return Result;
        }

        public string StrModifyBusEstimateTime(double EstimateTime)
        {
            //修改公車到站完整時間
            string Result = "";
            double StrHour = new double();
            double StrMinute = new double();
            double StrSecond = new double();

            StrHour = Math.Floor(EstimateTime / 3600);
            StrMinute = Math.Floor((EstimateTime % 3600) / 60);
            StrSecond = ((EstimateTime % 3600) % 60);

            if (StrHour > 0)
                Result = StrHour.ToString() + "時";

            if (StrMinute > 0)
                Result += StrMinute.ToString() + "分";

            if (StrSecond > 0)
                Result += StrSecond.ToString() + "秒";

            if (StrHour == 0 & StrMinute == 0 & StrSecond == 0)
                Result = "到站";

            return Result;
        }

        public string StrModifyBusStopStatus(string StopStatus)
        {
            //修改公車目前狀況
            string Result = "";

            if (StopStatus == "1")
                Result = "尚未發車";
            else if (StopStatus == "2")
                Result = "交管不停靠";
            else if (StopStatus == "3")
                Result = "末班車已過";
            else if (StopStatus == "4")
                Result = "今日未營運";
            else
                Result = "無到站時間";

            return Result;
        }
        #endregion


        public void EmpBirthToTxt(List<EmpBirthdayDetail> EmpBirthday)
        {
            //寫入資料流(絕對位置)
            FileStream Fs = new FileStream(Server.MapPath("~") + "\\News\\Birthday.txt", FileMode.Create);

            int TotalCount = 0;//索引值
            foreach (var Item in EmpBirthday)
            {
                byte[] Data = new byte[] { };
                TotalCount += 1;

                //員工姓名
                Data = System.Text.Encoding.UTF8.GetBytes(Item.EMP_NAME + "<br/>");
                //開始寫入(員工姓名)
                Fs.Write(Data, 0, Data.Length);

                //員工英文名字
                Data = System.Text.Encoding.UTF8.GetBytes(Item.EMP_ENAME.Trim() + "<br/>");
                //開始寫入員工英文名字
                Fs.Write(Data, 0, Data.Length);

                //員工ID
                if (EmpBirthday.Count() == TotalCount)
                    Data = System.Text.Encoding.UTF8.GetBytes(Item.EMP_ID);
                else
                    Data = System.Text.Encoding.UTF8.GetBytes(Item.EMP_ID + ",");

                //開始寫入員工ID
                Fs.Write(Data, 0, Data.Length);
            }

            //清空緩衝區、關閉流
            Fs.Flush();
            Fs.Close();
        }

    }
}