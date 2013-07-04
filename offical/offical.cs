using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Collections;

namespace loginauthmothed

{
    public class offical:auth
    {
        public string getname()
        {
            return "正版验证"; 
        }

        string ProfileId;
        string ProfileName;
        string SessionToken;


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="passwd">密码</param>
        /// <returns>0：正常登陆；1：服务器无响应；2：服务器响应无法解析；3：密码错误</returns>
        public bool login(string username, string passwd) 
        {
            HttpWebRequest auth = (HttpWebRequest)WebRequest.Create("https://login.minecraft.net");
            auth.Method = "POST";
            StringBuilder PostData = new StringBuilder();
            PostData.Append("user=");
            PostData.Append(username);
            PostData.Append("&password=");
            PostData.Append(passwd);
            PostData.Append("&version=14");
            auth.ContentType = "text/html";
            Hashtable logindata = new Hashtable();
            logindata.Add("user", username);
            logindata.Add("password", passwd);
            logindata.Add("version", 14);
            StreamReader AuthAnsStream;
            //byte[] postdata = Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(PostData.ToString()));
            byte[] postdata = Encoding.UTF8.GetBytes(ParsToString(logindata));
            auth.ContentLength = postdata.LongLength;
            Stream poststream = auth.GetRequestStream();
            poststream.Write(postdata, 0, postdata.Length);
            poststream.Close();
            try
            {
                HttpWebResponse authans = (HttpWebResponse)auth.GetResponse();
                AuthAnsStream = new StreamReader(authans.GetResponseStream());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            string AuthAnsString = AuthAnsStream.ReadToEnd();
            string[] split = AuthAnsString.Split(':');
            if (split.Length == 5)
            {
                ProfileId = split[4];
                ProfileName = split[2];
                SessionToken = split[3];
                if (String.IsNullOrWhiteSpace(ProfileId) || String.IsNullOrWhiteSpace(ProfileName) || String.IsNullOrWhiteSpace(SessionToken))
                {
                    throw new Exception("服务器响应无法解析");
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public string getsession()
        {
            return SessionToken;
        }

        public string getPid()
        {
            return ProfileId;
        }

        public string getPname()
        {
            return ProfileName;
        }


        private static String ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
            }
            return sb.ToString();
        }
    }
}
