using _69zg.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _69zg.Models;
using static _69zg.Common.ResquestUtil;
using _69zg.DBManager;

namespace _69zg.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string url)
        {
            ViewBag.url = url; //url;
            return View();
        }

        [HttpPost]
        public JsonResult Login()
        {
            string pwd = GetValue(Request, "upd");
            string name = GetValue(Request, "uname");
            string url= GetValue(Request, "sourceurl");
            string flag = "SUCCESS";
            string filedMes = "用户名或密码错误！";
            if (string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(name))
            {
                flag = "FAILED";
            }
            else
            {
                pwd = pwd.GetMd5Str();
                C_userinfo users = MSSQLManager.GetDmodel().C_userinfo.FirstOrDefault(c => c.name == name && c.pwd == pwd);
                if (users != null)
                {
                    if (users.endtime != null && users.endtime < DateTime.Now)
                    {
                        flag = "FAILED";
                        filedMes = "账号已过期，请联系管理员";
                    }
                    else
                    { 
                    users.name = name;
                    users.lastlogdate = DateTime.Now;
                        MSSQLManager.GetDmodel().Entry(users).State = System.Data.Entity.EntityState.Modified;
                        MSSQLManager.GetDmodel().SaveChanges();
                    Session["currentuser"] = users;
                    }
                }
                else
                {
                    flag = "FAILED";
                }
            }
            string []jsonresul = { flag, url, filedMes };
            return Json(jsonresul,JsonRequestBehavior.DenyGet);
        }

        public ActionResult Logout()
        {
            Session.Remove("currentuser");
            return RedirectToAction("Index");
        }
    }
}