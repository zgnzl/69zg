using Subgurim.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace _69zg.Common
{
    public static class ResquestUtil
    {
            /// <summary>
            /// 获取请求参数
            /// </summary>
            /// <param name="request">当前请求</param>
            /// <param name="key">键值</param>
            /// <param name="defaultvalue">默认值</param>
            /// <returns></returns>
            public static string GetValue( string key, string defaultvalue = "")
            {
            HttpRequest request = HttpContext.Current.Request;
                string result = string.Empty;
                if (request.RequestType == "POST")
                {
                    result = request.Form[key];
                }
                else
                {
                    result = request.QueryString[key];
                }
                if (!string.IsNullOrEmpty(result))
                {
                    result= HttpUtility.UrlDecode(result, Encoding.GetEncoding("utf-8"));
                }
                else
                {
                    result = defaultvalue;
                }
                return MatchUrlParam(result);
            }
            public static string GetMd5Str(this string input)
            {
                 StringBuilder md5 = new StringBuilder();
                 MD5 md = MD5.Create();
                 byte[] md5hash=  md.ComputeHash(Encoding.UTF8.GetBytes(input));
            for (int i = 0; i < md5hash.Length; i++)
            {
                md5.Append(md5hash[i].ToString("X"));
            }
            return md5.ToString(); ;
            }

        private static string MatchUrlParam(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Trim();
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = Regex.Replace(text, "[？]", "?");    //全角？
            text = Regex.Replace(text, "[×]", "*");     //全角*
            text = text.Replace("'", "''");

            if (!string.IsNullOrEmpty(text))
            {
                string match = "[\\]\\[\"'\\{\\\\]";
                Regex reg = new Regex(match);
                text = Regex.Replace(text, match, "");
            }
            if (!string.IsNullOrEmpty(text))
                text = text.Trim();
            return text;
        }
        /// <summary>
        /// C#将IP地址转为长整形
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToNumber(string ip)
        {
            string[] arr = ip.Split('.');
            return 256 * 256 * 256 * long.Parse(arr[0]) + 256 * 256 * long.Parse(arr[1]) + 256 * long.Parse(arr[2]) + long.Parse(arr[3]);
        }
        /// <summary>
        /// C#判断IP地址是否为私有/内网ip地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsPrivateIp(string ip)
        {
            long ABegin = IpToNumber("10.0.0.0"), AEnd = IpToNumber("10.255.255.255"),//A类私有IP地址
             BBegin = IpToNumber("172.16.0.0"), BEnd = IpToNumber("172.31.255.255"),//'B类私有IP地址
             CBegin = IpToNumber("192.168.0.0"), CEnd = IpToNumber("192.168.255.255"),//'C类私有IP地址
             IpNum = IpToNumber(ip);
            return (ABegin <= IpNum && IpNum <= AEnd) || (BBegin <= IpNum && IpNum <= BEnd) || (CBegin <= IpNum && IpNum <= CEnd);
        }
        /// <summary>
        /// C#获取真实IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = HttpContext.Current.Request.ServerVariables["http_x_forwarded_for"];
            if (string.IsNullOrEmpty(ip)) ip = HttpContext.Current.Request.ServerVariables["remote_addr"];
            else//代理ip地址有内容，判断是否符合ipv4地址或者是否为内网地址
            {
                ip = ip.Trim().Replace(" ", "");
                if (!Regex.IsMatch(ip, @"^\d+(\.\d+){3}$") || IsPrivateIp(ip))
                    ip = HttpContext.Current.Request.ServerVariables["remote_addr"];//不符合规则或者内网/私有地址使用remote_addr代替
            }
            return ip == "::1" ? "127.0.0.1" : ip;
        }
        public static string GetPlatName()
        {
            string name = System.Web.HttpContext.Current.Request.Browser.Platform;
            switch (name.ToUpper())
            {
                case "WINNT": return "PC端";
                case "UNKNOWN": return "移动端";
                default: return "PC端";
            }
        }

        public static string GetIpCity(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return "";
            try
            {
                string databasePath = HttpContext.Current.Server.MapPath("~/app_data/GeoLiteCity.dat");
                LookupService service = new LookupService(databasePath);
                Location loc = service.getLocation(ipAddress);
                return loc != null ? loc.city : "";
            }
            catch
            {
                return "";
            }
        }
    }
}
