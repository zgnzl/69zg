using System;

namespace _69zg.Common
{
    public static class ErrorMessageCode
    {
        public static string GetMessage(this string code)
        {
            try
            {
                switch (code)
                {
                    case "1000": return "您没有权限，进行此操作！";
                    case "2000": return "操作{0}失败！";
                    case "2001": return "<span style='color:red'>{0}</span>添加成功!";
                    default: return "未知原因操作失败，请联系管理员!";
                }
            }
            catch (Exception e) { return e.Message; }
        }
    }
}
