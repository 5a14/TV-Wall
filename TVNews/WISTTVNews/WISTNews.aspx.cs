using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using System.Text;
using System.Data.SqlClient;
using WISTTVNews.Models;
using WISTTVNews.BusinessLibery;
using log4net;


namespace WISTTVNews
{
    public partial class WISTNews : System.Web.UI.Page
    {
        //log檔宣告
        static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //天氣Url
        private string UrlWeatherDetails = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-A81FC80D-B149-4BA4-A6D5-384C156201DC&format=JSON&elementName=Wx,PoP,MinT,MaxT";

        public Business Business = new Business();

        protected void Page_Load(object sender, EventArgs e)
        {
            ////取得生日並放入文字檔顯示
            Business.GetEmpBirthday();

            Business.GetWeatherDetails(UrlWeatherDetails); //天氣資訊
            string StrJsonDatail = Business.GetJsonDatail(Request.QueryString["url"], Request.QueryString["item"]);//公車或火車資訊

            //顯示前端的JSON資料
            if (StrJsonDatail != "")
                JSonShow(StrJsonDatail);

            //顯示撈取的FB文字
            lbFB.Text = Business.GetFaceBookText();
        }


        public void JSonShow(string StrJson)
        {
            //前端顯示使用
            Response.ContentType = "text/json";
            Response.Write(StrJson);
            Response.End();//關閉
        }

    }
}