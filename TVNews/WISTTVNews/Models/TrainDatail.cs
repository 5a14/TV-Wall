using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WISTTVNews.Models
{
    public class TrainDatail
    {

        //trainDatail.TrainNo 

        //trainDatail.StationName 
        //trainDatail.TrainTypeName
        //trainDatail.TripLine 
        //trainDatail.EndingStationName 
        //trainDatail.ScheduledArrivalTime 
        //trainDatail.ScheduledDepartureTime 
        //trainDatail.DelayTime 
        //trainDatail.Directio

        //public string StationID { get; set; }//車站代碼(台鐵定義的)
        public string StationName { get; set; }//車站名稱(站名)
        public string TrainNo { get; set; }//車次代碼(號)
        public string TrainTypeName { get; set; } //列車車種(自強、莒光..)
        public string TripLine { get; set; }//山海線類型 = ['0: 不經山海線', '1: 山線', '2: 海線']
        public string EndingStationName { get; set; }//終點車站名稱
        public string ScheduledArrivalTime { get; set; }//表訂到站時間(格式: HH:mm:ss)
        public string ScheduledDepartureTime { get; set; }//表訂離站時間(格式: HH:mm:ss) 

        //public int DelayTime { get; set; }//誤點時間(0:準點;>=1誤點) 
        public string DelayTime { get; set; }//誤點時間(0:準點;>=1誤點) 

        public string UpdateTime { get; set; }//台鐵資料更新時間
        public string Direction { get; set; } //順逆行 = ['0: 順行(北回)', '1: 逆行(南回)']


        #region API使用資訊
        ////API使用資訊
        //Inline Model[
        //RailLiveBoard
        //]
        //RailLiveBoard {
        //StationID(string) : 車站代碼 ,
        //StationName(NameType) : 車站名稱 ,
        //TrainNo(string) : 車次代碼 ,
        //Direction(string) : 順逆行 = ['0: 順行', '1: 逆行'],
        //TrainTypeID(string) : 列車車種代碼 ,
        //TrainTypeCode(string) : 列車車種簡碼 ,
        //TrainTypeName(NameType) : 列車車種名稱 ,
        //TripLine(string, optional) : 山海線類型 = ['0: 不經山海線', '1: 山線', '2: 海線'],
        //EndingStationID(string) : 車次終點車站代號 ,
        //EndingStationName(NameType) : 車次終點車站名稱 ,
        //ScheduledArrivalTime(string) : 表訂到站時間(格式: HH:mm:ss) ,
        //ScheduledDepartureTime(string) : 表訂離站時間(格式: HH:mm:ss) ,
        //DelayTime(integer) : 誤點時間(0:準點;>=1誤點) ,
        //SrcUpdateTime(DateTime) : 來源端平台資料更新時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz) ,
        //UpdateTime(DateTime) : 本平台資料更新時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz)
        //}
        //    NameType {
        //Zh_tw(string, optional) : 中文繁體名稱 ,
        //En(string, optional) : 英文名稱
        //} 
        #endregion

    }
}