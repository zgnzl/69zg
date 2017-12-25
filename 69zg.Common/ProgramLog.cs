using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _69zg.Common
{
   public class ProgramLog
    {
        private static object _lock = new object();
        private static string recordlog =ConfigParam.GetAppSetting("recordlog", "false");
        private static string logPath = ConfigParam.GetAppSetting("logPath");
        /// <summary>
        /// 记录错误信息日志
        /// </summary>
        public static void WriteErrorLog(string exMsg)
        {
            if (recordlog == "false")
                return;

            if (string.IsNullOrEmpty(logPath))
            {
                logPath = string.Format("{0}log\\", AppDomain.CurrentDomain.BaseDirectory);
            }
            logPath += string.Format("CNKI_{0}_Error.txt", string.Format("{0:d}", DateTime.Now));


            StreamWriter sw = null;

            try
            {
                lock (_lock)
                {
                    string dir = Path.GetDirectoryName(logPath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    sw = new StreamWriter(logPath, true, System.Text.Encoding.UTF8);
                    sw.WriteLine(DateTime.Now.TimeOfDay.ToString() + ":" + exMsg);
                    sw.Close();
                }
            }
            catch
            {
                if (sw != null)
                    sw.Close();
            }
        }
    }
}
