using System.IO;
using System.Text;
using System.Web.Caching;
using System.Xml;

namespace _69zg.Common
{
   public static class XmlHelper
    {
        public static string filepath = "ConfigFile/ApplicationSetting.xml";
        public static XmlDocument xml;

        static XmlHelper()
        {
            xml = GetXmlDocument(filepath);
        }

        public static XmlDocument GetXmlDocument(string filepath)
        {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(filepath);
                return xmldoc;
        }
        public static string GetTextFromXml(XmlDocument xmldoc, string nodename)
        {
            StringBuilder returnsb = new StringBuilder();
            foreach (XmlNode xmlnode in xmldoc.SelectNodes(xmldoc.DocumentElement.Name + "/" + nodename))
            {
                returnsb.Append(xmlnode.InnerText + ";");
            }
            return returnsb.ToString().Trim(';');
        }
    }
}