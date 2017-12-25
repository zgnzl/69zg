using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _69zg.Common
{
    public class ErrorMsg
    {
        private string errorReason;     //段值
        private string errorCode;       //错误代码编号
        private string errorUrl;            //错误代码处理页面链接地址
        private string errorMsgFile = "../Error.ini";       //错误代码信息文件

        private IniFile ini;                    //处理ini文件	

        public ErrorMsg()
        {
            ini = new IniFile();
            errorReason = "ErrorReason";
            string ErrorMsgFile = "~/Error.ini";
            if (HttpContext.Current == null)
            {
                errorMsgFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Error.ini");
            }
            else
            {
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(ErrorMsgFile)) == true)
                {
                    errorMsgFile = HttpContext.Current.Server.MapPath(ErrorMsgFile);
                }
                else
                {
                    errorMsgFile = HttpContext.Current.Server.MapPath(errorMsgFile);
                }
            }
            ini.IniSetPath(errorMsgFile);
        }

        public ErrorMsg(string ErrorIniFile)
        {
            ini = new IniFile();
            errorReason = "ErrorReason";
            errorMsgFile = HttpContext.Current.Server.MapPath(ErrorIniFile);
            if (System.IO.File.Exists(ErrorIniFile) != true)
            {
                errorMsgFile = HttpContext.Current.Server.MapPath(errorMsgFile);
            }
            ini.IniSetPath(errorMsgFile);
        }

        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
            set
            {
                if (value == "0")
                {
                    errorCode = "-1111";
                }
                else
                {
                    errorCode = value;
                }
            }
        }

        public string ErrorUrl
        {
            get
            {
                return errorUrl;
            }
            set
            {
                errorUrl = value;
            }
        }

        public string ErrorMsgFile
        {
            get
            {
                return errorMsgFile;
            }
            set
            {
                errorMsgFile = value;
                if (ini != null)
                {
                    ini.IniSetPath(errorMsgFile);
                }
            }
        }

        /// <summary>
        /// 取错误信息
        /// </summary>
        /// <returns></returns>
        public string GetCodeMsg()
        {
            if (ini == null)
            {
                return null;
            }

            string msg = null;
            msg = ini.IniReadValue(errorReason, errorCode);

            if (msg == null || msg == string.Empty)
            {
                msg = errorCode;
            }
            return msg;
        }

        /// <summary>
        /// 通过代码值直接获取错误信息
        /// </summary>
        /// <returns></returns>
        public string GetCodeMsg(string strErrCode)
        {
            if (strErrCode == "0")
            {
                errorCode = "-1";
            }
            errorCode = strErrCode;

            return GetCodeMsg();
        }

        /// <summary>
        /// 通过代码值直接获取错误信息,并格式化错误信息
        /// </summary>
        /// <returns></returns>
        public string GetCodeMsg(string strErrCode, string formatStr)
        {
            if (strErrCode == "0")
            {
                errorCode = "-1";
            }
            errorCode = strErrCode;

            return string.Format(GetCodeMsg(), formatStr);
        }

        /// <summary>
        /// 获取URL
        /// </summary>
        /// <returns></returns>
        public string GetUrlContent()
        {
            if (ini == null)
            {
                return null;
            }

            string msg = ini.IniReadValue(errorReason, errorUrl);
            return msg;
        }

        public string GetUrlContent(string strErrorUrl)
        {
            if (strErrorUrl == null || strErrorUrl == string.Empty)
                return null;

            errorUrl = strErrorUrl;

            return GetUrlContent();
        }

        public void ShowMsg()
        {
            string msg;
            msg = GetCodeMsg();
            ShowMsg(msg);
        }

        public void ShowMsg(string msg)
        {
            HttpContext.Current.Response.Write("<script language=javascript >\r\n" +
                "alert('" + msg.Replace("'", "\\'") + "');\r\n" +
                "</script>");
        }

        public void ConfirmMsg()
        {
            string msg;
            msg = GetCodeMsg();
            ConfirmMsg(msg);
        }

        /// <summary>
        /// 弹出用户确认对话框
        /// </summary>
        /// <param name="msg"></param>
        public void ConfirmMsg(string msg)
        {
            HttpContext.Current.Response.Write("<script language=javascript >\r\n"
                + "var truthBeTold = window.confirm('" + msg + "');\r\n"
                + "if (truthBeTold == false)\r\n"
                + "{window.close();}\r\n"
                + "</script>");
        }

        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="succUrl"></param>
        public void ConfirmMsg(string msg, string succUrl)
        {
            HttpContext.Current.Response.Write("<script language=javascript >\r\n"
                + "var truthBeTold = window.confirm('" + msg + "');\r\n"
                + "if (truthBeTold == false)\r\n"
                + "{window.close();}\r\n"
                + "else\r\n"
                + "{window.location = '" + succUrl + "';}\r\n"
                + "</script>");
        }

        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="succUrl"></param>
        public void ConfirmMsg(string msg, string succUrl, string strWindow)
        {
            HttpContext.Current.Response.Write("<script language=javascript >\r\n"
                + "var truthBeTold = window.confirm('" + msg + "');\r\n"
                + "if (truthBeTold == false)\r\n"
                + "{" + strWindow + ".close();}\r\n"
                + "else\r\n"
                + "{" + strWindow + ".location = '" + succUrl + "';}\r\n"
                + "</script>");
        }

        public void TurnPage()
        {
            string msg;
            msg = GetUrlContent();
            HttpContext.Current.Response.Redirect(msg, false);
        }

        public void TurnPage(string url)
        {
            HttpContext.Current.Response.Redirect(url, false);
        }

        /// <summary>
        /// 转至错误处理页
        /// </summary>
        /// <param name="ErrFile"></param>
        /// <param name="code"></param>
        public void TurnPageMsgByCode(string ErrFile, string code)
        {
            if (code == null || code == string.Empty)
            {
                ErrorCode = "-1";
            }
            else
            {
                ErrorCode = code;
                if (code == "-1203" || code == "-3209")
                {
                    ErrFile = "Re" + ErrFile;
                }
            }

            if (ErrFile.IndexOf("?") < 0)
            {
                ErrFile += "?ErrorString=" + ErrorCode;
            }
            else
            {
                ErrFile += "&ErrorString=" + ErrorCode;
            }

            HttpContext.Current.Response.Redirect(ErrFile, false);
        }

        /// <summary>
        /// 转至错误处理页
        /// </summary>
        /// <param name="ErrFile">错误处理文件</param>
        /// <param name="code">错误代码</param>
        /// <param name="errUrl">错误后转向的URL</param>
        public void TurnPageMsgByCode(string ErrFile, string code, string errUrl)
        {
            if (code == null || code == string.Empty)
            {
                ErrorCode = "-1";
            }
            else
            {
                ErrorCode = code;
            }

            if (ErrFile.IndexOf("?") < 0)
            {
                ErrFile += "?ErrorString=" + ErrorCode;
            }
            else
            {
                ErrFile += "&ErrorString=" + ErrorCode;
            }

            if (errUrl != null && errUrl != string.Empty)
            {
                ErrFile += "&ErrorStringURL=" + errUrl;
            }
            HttpContext.Current.Response.Redirect(ErrFile, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="iToDo">0:不执行其他操作；1:关闭；2：后退</param>
        public void ShowAlertMsg(string strMsg, int iToDo)
        {
            ShowMsg(strMsg);
            switch (iToDo)
            {
                case 0:
                    break;
                case 1:
                    HttpContext.Current.Response.Write("<script>self.close();</script>");
                    break;
                case 2:
                    HttpContext.Current.Response.Write("<script>history.back()</script>");
                    break;
                case 3:
                    HttpContext.Current.Response.Write("<script>top.close();</script>");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="iToDo">0:不执行其他操作；1:关闭；2：后退;3 关闭top空体 4 刷新父窗口</param>
        public void ShowAlertMsgByCode(string code, int iToDo)
        {
            ErrorCode = code;
            ShowMsg();
            switch (iToDo)
            {
                case 0:
                    break;
                case 1:
                    HttpContext.Current.Response.Write("<script>self.close();</script>");
                    break;
                case 2:
                    HttpContext.Current.Response.Write("<script>history.back()</script>");
                    break;
                case 3:
                    HttpContext.Current.Response.Write("<script>top.close();</script>");
                    break;
                case 4:
                    HttpContext.Current.Response.Write("<script>\r\n"
                        + " window.parent.location.reload();\r\n"
                        + " setTimeout('self.close();',1000);\r\n"
                        + "</script>\r\n");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="iToDo">显示信息后要转向的页面</param>
        public void ShowAlertMsgByCodeTurnPage(string code, string newurl)
        {
            ErrorCode = code;
            ShowMsg();
            if (newurl == null || newurl == string.Empty)
                return;

            HttpContext.Current.Response.Write("<script language=javascript >\r\n" +
                "window.location.href='" + newurl + "';\r\n" +
                "</script>");
        }

        /// <summary>
        /// 清除Cookie后转向指定网页
        /// </summary>
        /// <param name="newurl"></param>
        public void RemoveCookieTurnPage(string newurl)
        {
            if (newurl == null || newurl == string.Empty)
                return;

            HttpContext.Current.Response.Write("\r\n<script language=javascript >\r\n"
                + "setCookie(\"RecordID\",\"\")\r\n"
                + "window.setTimeout(window.location.href='" + newurl + "',100000);\r\n" +
                "</script>\r\n");
        }

        /// <summary>
        /// 显示用户确认的表单
        /// </summary>
        /// <param name="code">提示信息码</param>		
        public void ShowConfirmMsgByCode(string code)
        {
            if (code == null || code == string.Empty)
            {
                code = "0";
            }
            ErrorCode = code;
            ConfirmMsg();
        }

        public void ShowConfirmMsgByCode(string code, string succUrl)
        {
            if (code == null || code == string.Empty)
            {
                code = "0";
            }
            ErrorCode = code;
            ConfirmMsg(GetCodeMsg(), succUrl);
        }
        public void ShowConfirmMsgByCode(string code, string succUrl, string strWindow)
        {
            if (code == null || code == string.Empty)
            {
                code = "0";
            }
            ErrorCode = code;
            ConfirmMsg(GetCodeMsg(), succUrl, strWindow);
        }
    }

    /// <summary>
	/// ini 的摘要说明。
	/// </summary>
	public class IniFile
    {
        public string path;    //INI文件名
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //声明读写INI文件的API函数
        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        public IniFile()
        {
        }

        public void IniSetPath(string INIPath)
        {
            path = INIPath;
        }
        //类的构造函数，传递INI文件名

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        //写INI文件

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
        //读取INI文件指定
    }
}
