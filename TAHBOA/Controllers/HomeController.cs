﻿using _69zg.DataContent;
using System.Collections.Generic;
using System.Web.Mvc;
using TAHB.Model;
using _69zg.Filter;

namespace TAHBOA.Controllers
{
    [LoginFilter()]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            _userinfo users = Session["currentuser"] as _userinfo;
            ViewBag.name = users.realname ?? users.name;
            List<_functioninfo> listfunction =DB.Context.From<_functioninfo>().ToList();
            _roleinfo role = DB.Context.From<_roleinfo>().Where(c => c.id == users.id).ToFirst();
            Session["roleinfo"] = role;
            Session["listfunction"] = listfunction;
            ViewBag.listfunction = listfunction;
            ViewBag.roleinfo = role.functionids;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}