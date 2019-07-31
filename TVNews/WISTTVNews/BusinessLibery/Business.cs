using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WISTTVNews.AppCode;
using WISTTVNews.Models;
using WISTTVNews.DAO;

namespace WISTTVNews.BusinessLibery
{
    public class Business
    {
        //log檔宣告
        static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WebServiceJson Json = new WebServiceJson();
        public WISTNewsDAO DAO = new WISTNewsDAO();
        public WebCrawler Crawler = new WebCrawler(); 

        public string GetJsonDatail(string Url, string Item)
        {
            //獲得公車或火車的JSon資訊
            string Result = "";

            Result = Json.GetBusOrTrainJson(Url, Item);

            return Result;
        }

        public void GetWeatherDetails(string Url)
        {
            //獲得天氣資訊，輸入文字檔讓前端輪播
            Json.GetWeatherJsonToTxt(Url);
        }

        public void GetEmpBirthday()
        {
            //獲得員工生日，輸入文字檔讓前端輪播
            DAO.GetEmpBirthdayDAO();
        }

        public string GetFaceBookText()
        {
            //獲得FB貼文文字
            string Result = "";

            Result = Crawler.GetFaceBookText();

            return Result;
        }
    }
}