using System.Collections;

namespace _69zg.Common
{
    public static class SessionInfo
    {
        public static bool AddSession(string key, object value)
        {
            if (SessionNotNull())
            {
                System.Web.HttpContext.Current.Session.Remove(key);
                System.Web.HttpContext.Current.Session[key] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetSessionId()
        {
            return System.Web.HttpContext.Current.Session.SessionID;
        }

        public static bool AddSession(Hashtable keyvalues)
        {
            if (SessionNotNull())
            {
                foreach (DictionaryEntry keyvalue in keyvalues)
                {
                    System.Web.HttpContext.Current.Session[keyvalue.Key.ToString()] = keyvalue.Value;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool RemoveSession(params string[] keys)
        {
            if (SessionNotNull())
            {
                foreach (string key in keys)
                {
                    System.Web.HttpContext.Current.Session.Remove(key);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public static object GetSession(string key)
        {
            if (SessionNotNull())
            {
                return System.Web.HttpContext.Current.Session[key];
            }
            else
            {
                return null;
            }
        }


        public static bool SessionNotNull()
        {
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Session == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
