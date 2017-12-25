using MySql.Data.MySqlClient.Properties;
using System;

namespace _69zg.Common
{
    public class GlobalResources
    {
        private static WebLanguage m_weblanguage = WebLanguage.GB;
        private static System.Globalization.CultureInfo m_cultureinfo = null;
        public static System.Resources.ResourceManager ResourceManager = Resources.ResourceManager;

        /// <summary>
        /// 本地化语言
        /// </summary>
        public static System.Globalization.CultureInfo CultureInfo
        {
            get
            {
                if (m_cultureinfo == null)
                {
                    Innitcultureinfo();
                }
                if (Resources.Culture == null)
                    Resources.Culture = m_cultureinfo;
                return m_cultureinfo;
            }

        }
        /// <summary>
        /// 语言名称(目录)
        /// </summary>
        private static void Innitcultureinfo()
        {

            WebLanguage lType = WebLanguage.None;
            string sType =ConfigParam.GetAppSetting("encoding", "GB");
            if (string.IsNullOrEmpty(sType))
            {
                lType = WebLanguage.GB;
            }
            sType = sType.Trim().ToUpper();
            try
            {
                lType = (WebLanguage)Enum.Parse(typeof(WebLanguage), sType);
            }
            catch { }

            switch (lType)
            {
                //case WebLanguage.EN://英
                //    m_weblanguage = WebLanguage.EN;
                //    m_cultureinfo = new System.Globalization.CultureInfo("en-US");
                //    ResourceManager = new System.Resources.ResourceManager("KNet.Web.Helper.Properties.Resources-US", typeof(Resources_US).Assembly);
                //    break;
                //case WebLanguage.GER://德
                //    m_weblanguage = WebLanguage.GER;
                //    m_cultureinfo = new System.Globalization.CultureInfo("de-DE");
                //    ResourceManager = new System.Resources.ResourceManager("KNet.Web.Helper.Properties.Resources-US", typeof(Resources_US).Assembly);
                //    break;
                //case WebLanguage.FRA://法
                //    m_weblanguage = WebLanguage.FRA;
                //    m_cultureinfo = new System.Globalization.CultureInfo("fr-FR");
                //    ResourceManager = new System.Resources.ResourceManager("KNet.Web.Helper.Properties.Resources-US", typeof(Resources_US).Assembly);
                //    break;
                //case WebLanguage.JAN://日
                //    m_weblanguage = WebLanguage.JAN;
                //    m_cultureinfo = new System.Globalization.CultureInfo("ja-JP");
                //    ResourceManager = new System.Resources.ResourceManager("KNet.Web.Helper.Properties.Resources-US", typeof(Resources_US).Assembly);
                //    break;
                //case WebLanguage.KOR://朝鲜
                //    m_weblanguage = WebLanguage.KOR;
                //    m_cultureinfo = new System.Globalization.CultureInfo("ko-KR");
                //    ResourceManager = new System.Resources.ResourceManager("KNet.Web.Helper.Properties.Resources-US", typeof(Resources_US).Assembly);
                //    break;
                //case WebLanguage.BG://繁体
                //    m_weblanguage = WebLanguage.BG;
                //    m_cultureinfo = new System.Globalization.CultureInfo("zh-TW");
                //    ResourceManager = new System.Resources.ResourceManager("KNet.Web.Helper.Properties.Resources-zh-TW", typeof(Resources_zh_TW).Assembly);
                //    break;
                //default:
                //    m_weblanguage = WebLanguage.GB;
                //    m_cultureinfo = new System.Globalization.CultureInfo("zh-Cn");
                //    ResourceManager = new System.Resources.ResourceManager("KNet.Web.Helper.Properties.Resources", typeof(Resources).Assembly);
                //    break;
            }
            ResourceManager.IgnoreCase = true;

        }

        /// <summary>
        /// 语言版本类型
        /// </summary>
        public static WebLanguage LanguageType
        {
            get
            {
                if (m_weblanguage == WebLanguage.None)
                {
                    Innitcultureinfo();

                }
                return m_weblanguage;
            }
        }
    }

    public enum WebLanguage
    {
        GB = 0,
        EN = 1,
        BG = 2,
        GER = 3,
        FRA = 4,
        JAN = 5,
        KOR = 6,
        None = 7,
    }
}
