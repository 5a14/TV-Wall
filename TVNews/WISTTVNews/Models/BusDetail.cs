using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WISTTVNews.Models
{
    public class BusDetail
    {
        //公車站牌名稱
        public string StopNameZhtw { get; set; }//中文名稱
        public string StopNameEn { get; set; }//英文名稱
        //路線名稱
        public string RouteNameZhtw { get; set; }//中文名稱
        public string RouteNameEn { get; set; }//英文名稱
        //去返程 = ['0: 去程', '1: 返程', '2: 迴圈', '255: 未知'],
        public string Direction { get; set; }
        //到站時間預估(秒) [當StopStatus値為1~4或PlateNumb値為-1時，EstimateTime値為空値; 反之，EstimateTime有値] (須改為分鐘)
        public string EstimateTime { get; set; }
        //車輛狀態備註 = ['0: 正常', '1: 尚未發車', '2: 交管不停靠', '3: 末班車已過', '4: 今日未營運']
        public string StopStatus { get; set; }
        //資料型態種類 = ['0: 未知', '1: 定期', '2: 非定期']
        public string MessageType { get; set; }
        //平台資料更新時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz)
        public string UpdateTime { get; set; }
        //public DateTime UpdateTime { get; set; }

        #region API使用資訊
        //BusN1EstimateTime {
        //    StopUID(string, optional) : 站牌唯一識別代碼，規則為 {業管機關簡碼} + {StopID}，其中 {業管機關簡碼} 可於Authority API中的AuthorityCode欄位查詢,
        //    StopID (string, optional): 地區既用中之站牌代碼(為原資料內碼) ,
        //    StopName(NameType, optional) : 站牌名 ,
        //    RouteUID(string, optional) : 路線唯一識別代碼，規則為 {業管機關代碼} + {RouteID}，其中 {業管機關代碼} 可於Authority API中的AuthorityCode欄位查詢,
        //    RouteID (string, optional): 地區既用中之路線代碼(為原資料內碼) ,
        //    RouteName(NameType, optional) : 路線名稱 ,
        //    Direction(string) : 去返程(該方向指的是此車牌車輛目前所在路線的去返程方向，非指站站牌所在路線的去返程方向，使用時請加值業者多加注意) = ['0: 去程', '1: 返程', '2: 迴圈', '255: 未知'],
        //    EstimateTime(integer, optional) : 到站時間預估(秒) [當StopStatus値為1~4或PlateNumb値為-1時，EstimateTime値為空値; 反之，EstimateTime有値] ,
        //    StopStatus(string, optional) : 車輛狀態備註 = ['0: 正常', '1: 尚未發車', '2: 交管不停靠', '3: 末班車已過', '4: 今日未營運'],
        //    MessageType(string, optional) : 資料型態種類 = ['0: 未知', '1: 定期', '2: 非定期'],
        //    SrcUpdateTime(DateTime, optional) : 來源端平台資料更新時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz)[公總使用TCP動態即時推播故沒有提供此欄位, 而非公總系統因提供整包資料更新, 故有提供此欄] ,
        //    UpdateTime(DateTime) : 本平台資料更新時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz)
        //    }
        //    NameType {
        //    Zh_tw(string, optional) : 中文繁體名稱 ,
        //    En(string, optional) : 英文名稱 
        //} 
        #endregion

        #region 實際顯示的JSon格式(範例)
        //{
        //"StopUID": "NWT163093",
        //"StopID": "163093",
        //"StopName": {
        //  "Zh_tw": "東方科學園區",
        //  "En": "Oriental Science Park"
        //},
        //"RouteUID": "NWT17013",
        //"RouteID": "17013",
        //"RouteName": {
        //  "Zh_tw": "F915",
        //  "En": "F915"
        //},
        //"Direction": 1,
        //"EstimateTime": 1016,
        //"StopStatus": 0,
        //"MessageType": 0,
        //"SrcUpdateTime": "2019-04-15T09:04:00+08:00",
        //"UpdateTime": "2019-04-15T09:04:02+08:00"
        //}
        #endregion
    }
}