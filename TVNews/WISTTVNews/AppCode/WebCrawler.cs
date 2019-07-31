using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngleSharp;
using AngleSharp.Dom;

namespace WISTTVNews.AppCode
{
    public class WebCrawler
    {
        public string GetFaceBookText()
        {
            //獲得FB貼文文字
            string Result = "";

            //暫存List
            var ListFBTime = new List<string>();
            var ListFBText = new List<string>();
            //FB網址
            string Url = "https://www.facebook.com/%E9%A3%86%E6%8D%8D-1471772763091863/";//館長FB粉絲團
            var Config = Configuration.Default.WithDefaultLoader();
            var Dom = BrowsingContext.New(Config).OpenAsync(Url).Result;
            //搜尋出目前FB貼文時間，根據物件的class查詢
            var DomFBTime = Dom.QuerySelectorAll("span.timestampContent").Select(x => x.TextContent);
            //加入List
            ListFBTime.AddRange(DomFBTime);

            //搜尋出目前FB貼文文字，根據物件的class查詢
            var DomFBText = Dom.QuerySelectorAll("span._a5_").Select(x => x.TextContent);// div.text_exposed_root
            //加入List
            ListFBText.AddRange(DomFBText);

            for (int i = 0; i < ListFBText.Count; i++)
            {
                //FB時間與貼文
                Result += ListFBTime[i] + "<br />" + ListFBText[i] + "<br /><br />";
            }

            //修改文字顯示長度，文字太長會超出版面
            int TextCount = Result.Length;
            if (TextCount > 530)
            {
                Result = Result.Substring(0, 500);
                Result = Result + " ......更多";
            }

            return Result;
        }
    }
}