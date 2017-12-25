using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _69zg.Common
{
   public class ConfigParam
    {
        /// <summary>
        /// 获取当前应用程序配置的AppSetingsSection节点值
        /// </summary>
        /// <param name="key">AppSetingsSection键值</param>
        /// <param name="defaultValue">键值不存在默认返回值</param>
        /// <returns>返回字符串值</returns>
        public static string GetAppSetting(string key, string defaultValue = null)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }

        /// <summary>
        /// 获取当前应用程序配置的ConnectionStrings节点值
        /// </summary>
        /// <param name="key">ConnectionStrings键值</param>
        /// <param name="defaultValue">键值不存在默认返回值</param>
        /// <returns>返回ConnectionString</returns>
        public static string GetConnectionString(string key, string defaultValue = null)
        {
            if (ConfigurationManager.ConnectionStrings[key] == null)
                return defaultValue;
            else
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        /// <summary>
        /// 获取当前应用程序配置的ConnectionStrings节点值
        /// </summary>
        /// <param name="key">ConnectionStrings键值</param>
        /// <param name="defaultValue">键值不存在默认返回值</param>
        /// <returns>返回ProviderName</returns>
        public static string GetConnectionStringProviderName(string key, string defaultValue = null)
        {
            if (ConfigurationManager.ConnectionStrings[key] == null)
                return defaultValue;
            else
                return ConfigurationManager.ConnectionStrings[key].ProviderName;
        }

        /// <summary>
        /// 动态获取当前应用程序的appsetting的节点值
        /// </summary>
        /// <param name="key">ConnectionStrings键值</param>
        /// <returns>返回ProviderName</returns>
        public static string GetOpenExeConfiguration(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config.AppSettings.Settings[key].Value;
        }

        public static string GetRefreshAppSetting(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。
            return ConfigurationManager.AppSettings[key];
        }
    }
}
