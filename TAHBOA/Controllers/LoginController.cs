using _69zg.Common;
using _69zg.DataContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAHB.Model;

namespace TAHBOA.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string username, string password)
        {
            if (Session["currentuser"] != null)
            {
                Response.Redirect("./home");
            }
            if (string.IsNullOrEmpty(username))
                return View();
            bool result = false;
            string message = "";
            if (!string.IsNullOrEmpty(password))
            {
                password = password.GetMd5Str();
                _userinfo users = DB.Context.From<_userinfo>().Where(c => c.name == username && c.pwd == password).ToFirst();
                if (users != null)
                {
                    if (users.endtime != null && users.endtime < DateTime.Now)
                    {
                        message = "账号已过期，请联系管理员";
                    }
                    else if (users.userlock == -1)
                    {
                        message = "账号被锁，请联系管理员";
                    } else
                    {
                        Session["currentuser"] = users;
                        result = true;
                    }
                }
                message = "用户名或密码错误";
            }
            return Json(new
            {
                message = message,
                result = result
            }
                );
        }

        public ActionResult LogOut()
        {
            Session.Clear();
         return  RedirectToAction("index");
        }
    }
}