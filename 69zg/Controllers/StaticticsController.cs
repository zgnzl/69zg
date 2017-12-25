using _69zg.Common;
using _69zg.DBManager;
using _69zg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _69zg.Controllers
{
    public class StaticticsController : Controller
    {
        // GET: Statictics
        private const string  logurl="http://www.zg69.cn/statictics/log/";

        public JavaScriptResult Index(string id)
        {
            if (MSSQLManager.GetDmodel().statisticsUser.FirstOrDefault(c => c.useruid == id) != null)
            {
                HttpRequestBase request = Request;

                statisticsLog statistics = new statisticsLog();
                statistics.useruid = id;
                string ip = "";
                if (request.UserHostAddress != "::1")
                {

                    foreach (string cip in request.UserHostAddress.Split('.'))
                    {
                        ip += cip.PadLeft(3, '0');
                    }

                }
                else
                {
                    ip = "127000000001";
                }
                statistics.ip = long.Parse(ip);
                statistics.url = ResquestUtil.GetValue(Request, "sourceurl");
                statistics.title = ResquestUtil.GetValue(Request, "title"); 
                statistics.datetime = DateTime.Now;
                statistics.descprition = request.UserAgent;
                MSSQLManager.GetDmodel().Entry(statistics).State = EntityState.Added;
                MSSQLManager.GetDmodel().SaveChanges();
            }

            StringBuilder docwrite = new StringBuilder();
            docwrite.Append("document.write(\"");
            docwrite.Append("<span >");
            docwrite.Append(" <a target='zg69statictics' href='");
            docwrite.Append(logurl);
            docwrite.Append(id);
            docwrite.Append("' title='网站统计'>");
            docwrite.Append("<img alt='统计' src='http://www.zg69.cn/Images/statictics/statictics.gif' style='object-fit:fill;height:20px;' />");
            docwrite.Append("</ a ></ span >\")");
            return JavaScript(docwrite.ToString());
        }

        [LoginFilter]
        public ActionResult Log(string id) {
            
            if(Session!=null && Session["currentuser"]!=null)
            { 
            C_userinfo user = Session["currentuser"] as C_userinfo;
                
            var model = MSSQLManager.GetDmodel().statisticsLog.Where(c=>c.useruid==id);
            return View(model);
            }
            return View();
        }
    }
}