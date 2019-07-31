using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WISTTVNews.Models;
using System.Configuration;
using System.Data;
using log4net;
using WISTTVNews.AppCode;

namespace WISTTVNews.DAO
{
    public class WISTNewsDAO
    {
        //log檔宣告
        static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebServiceJson webServiceJson = new WebServiceJson();

        //取得當天生日人員
        public void GetEmpBirthdayDAO()
        {
            EmpBirthdayDetail EmpBirthday = new EmpBirthdayDetail();
            List<EmpBirthdayDetail> ListEmpBirthdayDetail = new List<EmpBirthdayDetail>();
            DataTable dt = new DataTable();

            SqlConnection MyCn = new SqlConnection(ConfigurationManager.ConnectionStrings["nchrwitsConnectionString"].ConnectionString);//開啟SQL連線

            string strSQL = String.Empty;

            strSQL = @"SELECT * FROM T1;";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MyCn.ConnectionString))
                {
                    sqlConn.Open();

                    using (var cmd = new SqlCommand(strSQL, sqlConn))//宣告可帶參數的物件
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            EmpBirthday = new EmpBirthdayDetail();

                            EmpBirthday.EMP_ID = reader["EMP_ID"].ToString();
                            EmpBirthday.EMP_NAME = reader["EMP_NAME"].ToString();
                            EmpBirthday.EMP_ENAME = reader["EMP_ENAME"].ToString();

                            ListEmpBirthdayDetail.Add(EmpBirthday);
                        }

                        //資料轉JSon放入文字檔
                        webServiceJson.EmpBirthToTxt(ListEmpBirthdayDetail);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                //撈取公司資訊異常
                logger.Debug("撈取公司資訊異常 " + " SQL字串：" + strSQL + " 錯誤訊息：" + ex);
            }

        }
    }
}